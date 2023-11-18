using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.GenericVector
{
    public interface IVector3<T> where T : INumber<T>, IConvertible
    {
        public T this[int index] { get; set; }

        public T X { get; set; }

        public T Y { get; set; }

        public T Z { get; set; }
    }
}
