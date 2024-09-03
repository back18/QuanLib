using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DataObjectAttribute : Attribute
    {
        public DataObjectAttribute(string format)
        {
            ArgumentNullException.ThrowIfNull(format, nameof(format));

            Format = format;
        }

        public string Format { get; }
    }
}
