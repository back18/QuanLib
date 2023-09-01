using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    /// <summary>
    /// string工具类
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// 复制指定数量的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Copy(string value, int count)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"“{nameof(value)}”不能为 null 或空。", nameof(value));
            if (count < 0)
                throw new ArgumentException("字符串数量小于0");
            else if (count == 0)
                return string.Empty;

            string[] strings = new string[count];
            Array.Fill(strings, value);

            return string.Concat(strings);
        }

        /// <summary>
        /// 提取字符串的字母
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ExtractLetter(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            StringBuilder Result = new();
            foreach (char c in value)
                if (char.IsLetter(c))
                    Result.Append(c);
            return Result.ToString();
        }

        /// <summary>
        /// 提取字符串的数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ExtractNumber(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            StringBuilder Result = new();
            bool isMinus = false;
            bool isSpot = false;
            foreach (char c in value)
                if (char.IsNumber(c))
                    Result.Append(c);
                else if (c.Equals('-'))
                {
                    if (isMinus || Result.Length > 0)
                        break;
                    else
                    {
                        Result.Append(c);
                        isMinus = true;
                    }
                }
                else if (c.Equals('.'))
                {
                    if (isSpot)
                        break;
                    else
                    {
                        Result.Append(c);
                        isSpot = true;
                    }
                }
            return Result.ToString();
        }

        /// <summary>
        /// 拆分字符串后获取对应下标的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static string SplitAt(string value, char separator, int index)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"“{nameof(value)}”不能为 null 或空。", nameof(value));
            if (index < 0)
                throw new IndexOutOfRangeException("下标不能小于0");

            string[] array = value.Split(separator);
            if (index > array.Length - 1)
                throw new IndexOutOfRangeException($"字符串拆分后只有{array.Length}个元素，无法检索到下标{index}");
            else return array[index];
        }

        /// <summary>
        /// string[]转int[]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int[] ToInts(string[] value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length == 0)
                return Array.Empty<int>();

            int[] result = new int[value.Length];
            for (int i = 0; i < value.Length; i++)
                result[i] = int.Parse(value[i]);

            return result;
        }

        /// <summary>
        /// 泛型数组转字符串数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string[] ToString<T>(T[] array)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length == 0)
                return Array.Empty<string>();

            string[] stringArray = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
                stringArray[i] = array[i].ToString();

            return stringArray;
        }

        /// <summary>
        /// 字符串转decimal后比较大小
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public static bool Comparison(string arg1, string arg2, ComparisonSymbol compare)
        {
            if (!decimal.TryParse(arg1, out decimal num1))
                throw new FormatException($"\"{arg1}\"无法解析为decimal类型");
            if (!decimal.TryParse(arg2, out decimal num2))
                throw new FormatException($"\"{arg2}\"无法解析为decimal类型");

            return compare switch
            {
                ComparisonSymbol.Large => num1 > num2,
                ComparisonSymbol.LargeEqual => num1 >= num2,
                ComparisonSymbol.Small => num1 < num2,
                ComparisonSymbol.SmallEqual => num1 <= num2,
                ComparisonSymbol.Equal => num1 == num2,
                ComparisonSymbol.NoEqual => num1 != num2,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
