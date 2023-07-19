using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 控制台工具
    /// </summary>
    public static class ConsoleUtil
    {
        static ConsoleUtil()
        {
            ConsoleCharWidth = new();
        }

        /// <summary>
        /// 控制台字符宽度
        /// </summary>
        private readonly static ConsoleCharWidth ConsoleCharWidth;

        /// <summary>
        /// 获取指定TAB宽度的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="tab"></param>
        /// <returns></returns>
        public static string GetToTab(string value, int tab)
        {
            if (tab < 0)
                throw new ArgumentException("TAB宽度不可小于0");
            else if (tab == 0)
                return string.Empty;

            if (GetWidth(value) > tab * 8)
            {
                return GetToWidth(value, tab * 8);
            }
            else return value + GetTab(value, tab);
        }

        /// <summary>
        /// 获取指定宽度的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string GetToWidth(string? value, int width)
        {
            if (width < 0)
                throw new ArgumentException("宽度不可小于0");

            value ??= string.Empty;

            if (GetWidth(value) > width)
            {
                int criticalLen = width / 2;

                for (int i = criticalLen; i <= value.Length; i++)
                    if (GetWidth(value[..i]) > width)
                    {
                        criticalLen = i - 1;
                        break;
                    }

                if (GetWidth(value[..criticalLen]) == width)
                {
                    if (GetWidth(value[criticalLen - 1]) == 1)
                    {
                        if (GetWidth(value[criticalLen - 2]) == 1)
                            return value[..(criticalLen - 2)] + '…';
                        else if (GetWidth(value[criticalLen - 2]) == 2)
                            return value[..(criticalLen - 2)] + "… ";
                    }
                    else if (GetWidth(value[criticalLen - 1]) == 2)
                        return value[..(criticalLen - 1)] + '…';
                }
                else if (GetWidth(value[..criticalLen]) == width - 1)
                {
                    if (GetWidth(value[criticalLen - 1]) == 1)
                        return value[..(criticalLen - 1)] + '…';
                    else if (GetWidth(value[criticalLen - 1]) == 2)
                        return value[..(criticalLen - 1)] + "… ";
                }
                else if (GetWidth(value[..criticalLen]) == width - 2)
                    return value[..criticalLen] + '…';

                throw new NotImplementedException();
            }
            else return PadRightToWidth(value, width, ' ');
        }

        /// <summary>
        /// 向字符串左边填充指定字符至指定宽度
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="width">目标宽度</param>
        /// <param name="symbol">字符</param>
        /// <returns></returns>
        public static string PadLeftToWidth(string value, int width, char symbol)
        {
            int oldWidth = GetWidth(value);

            if (oldWidth < width)
                return value;

            StringBuilder Result = new();
            for (int i = 0; i < width - oldWidth; i++)
                Result.Append(symbol);
            Result.Append(value);
            return Result.ToString();
        }

        /// <summary>
        /// 向字符串右边填充指定字符至指定宽度
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="width">目标宽度</param>
        /// <param name="symbol">字符</param>
        /// <returns></returns>
        public static string PadRightToWidth(string value, int width, char symbol)
        {
            int oldWidth = GetWidth(value);

            if (oldWidth > width)
                return value;

            StringBuilder Result = new(value);
            for (int i = 0; i < width - oldWidth; i++)
                Result.Append(symbol);
            return Result.ToString();
        }

        /// <summary>
        /// 获取指定字符串达到指定TAB宽度所需要的TAB
        /// </summary>
        /// <param name = "value" >目标字符串</param >
        /// <param name="tab">TAB宽度</param>
        public static string GetTab(string value, int tab)
        {
            if (tab < 0)
                throw new ArgumentException("TAB宽度不可小于0");
            else if (tab == 0)
                return string.Empty;

            int ResultLen = (tab - GetWidth(value) / 8);
            StringBuilder Resul = new("");
            for (int i = 0; i < ResultLen; i++) Resul.Append('\t');
            return Resul.ToString();
        }

        /// <summary>
        /// 字符串List中宽度最长的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? MaxWidthOf(IEnumerable<string?> value)
        {
            return MaxWidthOf(value.ToArray());
        }

        /// <summary>
        /// 字符串数组中宽度最长的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string? MaxWidthOf(string?[] value)
        {
            if (value is null)
                return null;

            string max = string.Empty;
            for (int i = 0; i < value.Length; i++)
                if (GetWidth(value[i]) > GetWidth(max))
                    max = value[i];
            return max;
        }

        /// <summary>
        /// 获取字符串宽度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetWidth(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            int width = 0;
            for (int i = 0; i < value.Length; i++)
                width += GetWidth(value[i]);
            return width;
        }

        /// <summary>
        /// 获取字符宽度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetWidth(char value) => ConsoleCharWidth[value];

        public static void Write(this object obj) => Console.Write(obj);

        public static void WriteLine(this object obj) => Console.WriteLine(obj);
    }
}
