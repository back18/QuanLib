using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class ThrowHelper
    {
        public static void ArgumentOutOfRange<T>(T min, T max, T value, string name) where T : IComparable
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(name, value, $"“{name}”的值应该在 {min} 到 {max} 之间");
        }

        public static void ArgumentOutOfRange<T>(T expectant, T value, string name) where T : IComparable
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value.CompareTo(expectant) < 0 || value.CompareTo(expectant) > 0)
                throw new ArgumentOutOfRangeException(name, value, $"“{name}”的值只能为 {expectant}");
        }

        public static void ArgumentOutOfMin<T>(T min, T value, string name) where T : IComparable
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value.CompareTo(min) < 0)
                throw new ArgumentOutOfRangeException(name, value, $"“{name}”的值应该大于或等于 {min}");
        }

        public static void ArgumentOutOfMax<T>(T max, T value, string name) where T : IComparable
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(name, value, $"“{name}”的值应该小于或等于 {max}");
        }

        public static void ArrayLengthOutOfRange<T>(int minLength, int maxLength, T[] value, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value is null || value.Length < minLength || value.Length > maxLength)
                throw new ArgumentOutOfRangeException(name, value, $"数组“{name}”的长度应该在 {minLength} 到 {maxLength} 之间");
        }

        public static void ArrayLengthOutOfRange<T>(int length, T[] value, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value is null || value.Length != length)
                throw new ArgumentOutOfRangeException(name, value, $"数组“{name}”的长度只能为 {length}");
        }

        public static void ArrayLengthOutOfMin<T>(int minLength, T[] value, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value is null || value.Length < minLength)
                throw new ArgumentOutOfRangeException(name, value, $"数组“{name}”的长度应该大于或等于 {minLength}");
        }

        public static void ArrayLengthOutOfMax<T>(int maxLength, T[] value, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value is null || value.Length > maxLength)
                throw new ArgumentOutOfRangeException(name, value, $"数组“{name}”的长度应该小于或等于 {maxLength}");
        }

        public static void StringLengthOutOfRange(int minLength, int maxLength, string value, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value is null || value.Length < minLength || value.Length > maxLength)
                throw new ArgumentOutOfRangeException(name, value, $"字符串“{name}”的长度应该在 {minLength} 到 {maxLength} 之间");
        }

        public static void StringLengthOutOfRange(int length, string value, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value is null || value.Length != length)
                throw new ArgumentOutOfRangeException(name, value, $"字符串“{name}”的长度只能为 {length}");
        }

        public static void StringLengthOutOfMin(int minLength, string value, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value is null || value.Length < minLength)
                throw new ArgumentOutOfRangeException(name, value, $"字符串“{name}”的长度应该大于或等于 {minLength}");
        }

        public static void StringLengthOutOfMax(int maxLength, string value, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
            if (value is null || value.Length > maxLength)
                throw new ArgumentOutOfRangeException(name, value, $"字符串“{name}”的长度应该小于或等于 {maxLength}");
        }
    }
}
