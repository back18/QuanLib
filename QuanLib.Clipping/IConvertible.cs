using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public interface IConvertible<T> where T : IDataObject
    {
        public T Convert();
    }
}
