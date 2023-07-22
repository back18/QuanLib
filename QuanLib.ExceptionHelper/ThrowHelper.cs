using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ExceptionHelpe
{
    public static class ThrowHelper
    {
        public static void TryThrowArgumentOutOfRangeException<T>(T min, T max, T value, string name) where T : IComparable
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(name, value, $"“{name}”应该在 {min} 到 {max} 之间");
        }

        public static void TryThrowArgumentOutOfMinException<T>(T min, T value, string name) where T : IComparable
        {
            if (value.CompareTo(min) < 0)
                throw new ArgumentOutOfRangeException(name, value, $"“{name}”应该大于等于 {min}");
        }

        public static void TryThrowArgumentOutOfMaxException<T>(T max, T value, string name) where T : IComparable
        {
            if (value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(name, value, $"“{name}”的值应该小于等于 {max}");
        }
    }
}
