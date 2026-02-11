using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class ThrowHelper
    {
        public static void ArgumentOutOfRange<T>(T min, T max, T value, string? paramName = null) where T : struct, IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(paramName, value, $"“{paramName}”的值应该在 {min} 到 {max} 之间，实际值为 {value}");
        }

        public static void ArgumentOutOfRange<T>(T expectant, T value, string? paramName = null) where T : struct, IEquatable<T>
        {
            if (!value.Equals(expectant))
                throw new ArgumentOutOfRangeException(paramName, value, $"“{paramName}”的值只能为 {expectant}，实际值为 {value}");
        }

        public static void ArgumentOutOfMin<T>(T min, T value, string? paramName = null) where T : struct, IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                throw new ArgumentOutOfRangeException(paramName, value, $"“{paramName}”的值应该大于或等于 {min}，实际值为 {value}");
        }

        public static void ArgumentOutOfMax<T>(T max, T value, string? paramName = null) where T : struct, IComparable<T>
        {
            if (value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(paramName, value, $"“{paramName}”的值应该小于或等于 {max}，实际值为 {value}");
        }

        public static void ArrayLengthOutOfRange<T>(int minLength, int maxLength, T[] value, string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(value, paramName);
            if (value.Length < minLength || value.Length > maxLength)
                throw new ArgumentException($"数组“{paramName}”的长度应该在 {minLength} 到 {maxLength} 之间，实际长度为 {value.Length}", paramName);
        }

        public static void ArrayLengthOutOfRange<T>(int length, T[] value, string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(value, paramName);
            if (value.Length != length)
                throw new ArgumentException($"数组“{paramName}”的长度只能为 {length}，实际长度为 {value.Length}", paramName);
        }

        public static void ArrayLengthOutOfMin<T>(int minLength, T[] value, string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(value, paramName);
            if (value.Length < minLength)
                throw new ArgumentException($"数组“{paramName}”的长度应该大于或等于 {minLength}，实际长度为 {value.Length}", paramName);
        }

        public static void ArrayLengthOutOfMax<T>(int maxLength, T[] value, string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(value, paramName);
            if (value.Length > maxLength)
                throw new ArgumentException($"数组“{paramName}”的长度应该小于或等于 {maxLength}，实际长度为 {value.Length}", paramName);
        }

        public static void StringLengthOutOfRange(int minLength, int maxLength, string value, string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(value, paramName);
            if (value.Length < minLength || value.Length > maxLength)
                throw new ArgumentException($"字符串“{paramName}”的长度应该在 {minLength} 到 {maxLength} 之间，实际长度为 {value.Length}", paramName);
        }

        public static void StringLengthOutOfRange(int length, string value, string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(value, paramName);
            if (value.Length != length)
                throw new ArgumentException($"字符串“{paramName}”的长度只能为 {length}，实际长度为 {value.Length}", paramName);
        }

        public static void StringLengthOutOfMin(int minLength, string value, string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(value, paramName);
            if (value.Length < minLength)
                throw new ArgumentException($"字符串“{paramName}”的长度应该大于或等于 {minLength}，实际长度为 {value.Length}", paramName);
        }

        public static void StringLengthOutOfMax(int maxLength, string value, string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(value, paramName);
            if (value.Length > maxLength)
                throw new ArgumentException($"字符串“{paramName}”的长度应该小于或等于 {maxLength}，实际长度为 {value.Length}", paramName);
        }

        public static void FileNotFound([NotNullWhen(true)] string? path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"路径“{path}”的文件不存在", path);
        }

        public static void DirectoryNotFound([NotNullWhen(true)] string? path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"路径“{path}”的目录不存在");
        }

        public static void StreamNotSupportRead(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream);
            if (!stream.CanRead)
                throw new NotSupportedException("流不支持读取");
        }

        public static void StreamNotSupportWrite(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream);
            if (!stream.CanWrite)
                throw new NotSupportedException("流不支持写入");
        }

        public static void StreamNotSupportSeek(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream);
            if (!stream.CanSeek)
                throw new NotSupportedException("流不支持查找");
        }

        public static void StreamNotSupportTimeout(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream);
            if (!stream.CanTimeout)
                throw new NotSupportedException("流不支持超时");
        }
    }
}
