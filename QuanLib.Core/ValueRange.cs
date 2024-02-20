using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public readonly struct ValueRange<T> where T : IComparable
    {
        public ValueRange(T min, T max)
        {
            ArgumentNullException.ThrowIfNull(min, nameof(min));
            ArgumentNullException.ThrowIfNull(max, nameof(max));
            ThrowHelper.ArgumentOutOfMin(min, max, nameof(max));

            Min = min;
            Max = max;
        }

        public T Min { get; }

        public T Max { get; }

        public readonly bool IsWithinRange(T value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
        }
    }
}
