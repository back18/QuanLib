using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public interface IClipboard
    {
        public ClipboardMode GetClipboardMode();

        public IDataObject? GetDataObject();

        public object? GetData();

        public object? GetData(string format);

        public object? GetData(string format, bool autoConvert);

        public void SetDataObject(IDataObject dataObject, ClipboardMode clipboardMode);

        public void SetData(string format, object data, ClipboardMode clipboardMode);

        public void Clear();
    }
}
