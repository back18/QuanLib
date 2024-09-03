using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public interface IDataObject
    {
        public string Format { get; }

        public object GetDate();

        public abstract static IDataObject Create(object data);
    }
}
