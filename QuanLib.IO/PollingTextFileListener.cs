using QuanLib.Core;
using QuanLib.Core.Events;
using QuanLib.IO.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class PollingTextFileListener : PollingFileListener, ITextListener
    {
        public PollingTextFileListener(string path, Encoding encoding, int delayMilliseconds = 500, ILoggerGetter? loggerGetter = null) : base(path, delayMilliseconds, loggerGetter)
        {
            ArgumentNullException.ThrowIfNull(path, nameof(path));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            Encoding = encoding;
            _textCache = new();

            WriteText += OnWriteText;
            WriteLineText += OnWriteLineText;
        }

        private readonly StringBuilder _textCache;

        public Encoding Encoding { get; }

        public event ValueEventHandler<ITextListener, ValueEventArgs<string>> WriteText;

        public event ValueEventHandler<ITextListener, ValueEventArgs<string>> WriteLineText;

        protected override void OnWriteBytes(PollingFileListener sender, BytesEventArgs e)
        {
            base.OnWriteBytes(sender, e);

            string text;
            try
            {
                text = Encoding.GetString(e.Buffer, e.Index, e.Length);
            }
            catch
            {
                return;
            }

            WriteText.Invoke(this, new(text));
        }

        protected virtual void OnWriteText(ITextListener sender, ValueEventArgs<string> e)
        {
            string[] lines = e.Argument.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
            _textCache.Append(lines[0]);

            if (lines.Length > 1)
            {
                if (_textCache.Length > 0)
                {
                    lines[0] = _textCache.ToString();
                    _textCache.Clear();
                    _textCache.Append(lines[^1]);
                }

                for (int i = 0; i < lines.Length - 1; i++)
                {
                    WriteLineText.Invoke(this, new(lines[i]));
                }
            }
        }

        protected virtual void OnWriteLineText(ITextListener sender, ValueEventArgs<string> e) { }
    }
}
