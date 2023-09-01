using QuanLib.Core;
using QuanLib.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.FileListeners
{
    public class TextFileListener : FileListener, ITextListener
    {
        public TextFileListener(string path, Encoding encoding) : base(path)
        {
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));

            WriteLineText += OnWriteLineText;
        }

        private readonly Encoding _encoding;

        public event EventHandler<ITextListener, TextEventArgs> WriteLineText;

        protected virtual void OnWriteLineText(ITextListener sender, TextEventArgs e) { }

        protected override void OnWriteBytes(FileListener sender, BytesEventArgs e)
        {
            base.OnWriteBytes(sender, e);

            string str = _encoding.GetString(e.Bytes);
            string[] lines = str.Split(Environment.NewLine);
            foreach (string line in lines)
                WriteLineText.Invoke(this, new(line.TrimEnd('\r')));
        }
    }
}
