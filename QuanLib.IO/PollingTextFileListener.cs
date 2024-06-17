using QuanLib.Core;
using QuanLib.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class PollingTextFileListener : PollingFileListener, ITextListener
    {
        public PollingTextFileListener(string path, Encoding encoding, ILoggerGetter? loggerGetter = null) : base(path, loggerGetter)
        {
            ArgumentNullException.ThrowIfNull(path, nameof(path));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            Encoding = encoding;
            _temp = new();

            WriteText += OnWriteText;
            WriteLineText += OnWriteLineText;
        }

        private readonly StringBuilder _temp;

        public Encoding Encoding { get; }

        public event EventHandler<ITextListener, EventArgs<string>> WriteText;

        public event EventHandler<ITextListener, EventArgs<string>> WriteLineText;

        protected override void OnWriteBytes(PollingFileListener sender, EventArgs<byte[]> e)
        {
            base.OnWriteBytes(sender, e);

            WriteText.Invoke(this, new(Encoding.GetString(e.Argument)));
        }

        protected virtual void OnWriteText(ITextListener sender, EventArgs<string> e)
        {
            string[] lines = e.Argument.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            _temp.Append(lines[0]);
            if (lines.Length > 1)
            {
                if (_temp.Length > 0)
                {
                    lines[0] = _temp.ToString();
                    _temp.Clear();
                    _temp.Append(lines[^1]);
                }
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    WriteLineText.Invoke(this, new(lines[i]));
                }
            }
        }

        protected virtual void OnWriteLineText(ITextListener sender, EventArgs<string> e) { }
    }
}
