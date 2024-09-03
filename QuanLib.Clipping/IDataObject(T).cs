using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public interface IDataObject<T> : IDataObject where T : notnull
    {
        public T GetData();
    }
}
