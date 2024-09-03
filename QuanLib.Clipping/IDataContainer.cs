using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public interface IDataContainer
    {
        public ClipboardMode ClipboardMode { get; }

        public IDataObject GetDataObject();

        public object GetData();

        public object? GetData(string format);

        public object? GetData(string format, bool autoConvert);

        public object? GetData(Type dataType);

        public object? GetData(Type dataType, bool autoConvert);
    }
}
