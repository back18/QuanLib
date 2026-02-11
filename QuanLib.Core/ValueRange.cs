using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public readonly struct ValueRange<T> where T : struct, IComparable<T>
    {
        public ValueRange(T min, T max)
        {
            ThrowHelper.ArgumentOutOfMin(min, max, nameof(max));

            Min = min;
            Max = max;
        }

        public T Min { get; }

        public T Max { get; }

        public readonly bool IsWithinRange(T value)
        {
            return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
        }
    }
}
