using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class CollectionValidator
    {
        public static bool TryValidateNull<T>(IEnumerable<T> enumerable, out int[] indexs)
        {
            ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));

            List<int> indexList = [];
            int index = 0;
            foreach (T? value in enumerable)
            {
                if (value is null)
                    indexList.Add(index);
                index++;
            }

            indexs = indexList.ToArray();
            return indexs.Length == 0;
        }

        public static void ValidateNull<T>(IEnumerable<T> enumerable, string name)
        {
            ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            if (TryValidateNull(enumerable, out var indexs))
                return;

            throw new ArgumentException("集合的一个或多个子项为null：" + string.Join(", ", indexs.Select(s => $"[{s}]")), name);
        }

        public static bool TryValidateNullOrEmpty(IEnumerable<string> enumerable, out int[] indexs)
        {
            ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));

            List<int> indexList = [];
            int index = 0;
            foreach (string value in enumerable)
            {
                if (string.IsNullOrEmpty(value))
                    indexList.Add(index);
                index++;
            }

            indexs = indexList.ToArray();
            return indexs.Length == 0;
        }

        public static void ValidateNullOrEmpty(IEnumerable<string> enumerable, string name)
        {
            ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            if (TryValidateNullOrEmpty(enumerable, out var indexs))
                return;

            throw new ArgumentException("字符串集合的一个或多个子项为null或空：" + string.Join(", ", indexs.Select(s => $"[{s}]")), name);
        }

        public static bool TryValidateNullOrWhiteSpace(IEnumerable<string> enumerable, out int[] indexs)
        {
            ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));

            List<int> indexList = [];
            int index = 0;
            foreach (string value in enumerable)
            {
                if (string.IsNullOrWhiteSpace(value))
                    indexList.Add(index);
                index++;
            }

            indexs = indexList.ToArray();
            return indexs.Length == 0;
        }

        public static void ValidateNullOrWhiteSpace(IEnumerable<string> enumerable, string name)
        {
            ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            if (TryValidateNullOrWhiteSpace(enumerable, out var indexs))
                return;

            throw new ArgumentException("字符串集合的一个或多个子项为null或空白：" + string.Join(", ", indexs.Select(s => $"[{s}]")), name);
        }
    }
}
