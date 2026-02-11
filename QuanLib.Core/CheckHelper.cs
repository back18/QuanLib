using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class CheckHelper
    {
        public static bool Range<T>(T min, T max, T value) where T : struct, IComparable<T>
        {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
        }

        public static bool Min<T>(T value, T min) where T : struct, IComparable<T>
        {
            return value.CompareTo(min) >= 0;
        }

        public static bool Max<T>(T value, T max) where T : struct, IComparable<T>
        {
            return value.CompareTo(max) <= 0;
        }
    }
}
