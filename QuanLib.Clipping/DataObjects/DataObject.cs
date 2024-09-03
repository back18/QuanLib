using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping.DataObjects
{
    public class DataObject : IDataObject<object>
    {
        public DataObject(string format, object data)
        {
            ArgumentException.ThrowIfNullOrEmpty(format, nameof(format));
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            Format = format;
            _data = data;
        }

        private readonly object _data;

        public string Format { get; }

        public object GetData()
        {
            return _data;
        }

        object IDataObject.GetDate()
        {
            return GetData();
        }

        public static IDataObject Create(object data)
        {
            throw new NotImplementedException();
        }
    }
}
