using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.FileListeners
{
    public class TextFileListener : FileListener
    {
        public TextFileListener(string path, Encoding encoding) : base(path)
        {
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            OnWriteByte += TextFileListener_OnWriteByte;
        }

        private readonly Encoding _encoding;

        public event Action<string> OnWriteLine = (obj) => { };

        private void TextFileListener_OnWriteByte(byte[] bytes)
        {
            string str = _encoding.GetString(bytes);
            string[] lines = str.Split(Environment.NewLine);
            foreach (string line in lines)
                OnWriteLine.Invoke(line.TrimEnd('\r'));
        }
    }
}
