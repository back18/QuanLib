using QuanLib.ExceptionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuanLib.Verification
{
    /// <summary>
    /// 数据验证
    /// </summary>
    public static class ValidateHelper
    {
        /// <summary>
        /// 验证文本长度
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minLen"></param>
        /// <param name="maxLen"></param>
        /// <returns></returns>
        public static bool TextLen(string value, int minLen, int maxLen, out string message)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            if (minLen < 0)
                throw new ArgumentOutOfRangeException(nameof(minLen), minLen, MessageHelper.GetMessage(MessageKey.文本长度下限小于0));
            if (maxLen < 0)
                throw new ArgumentOutOfRangeException(nameof(maxLen), maxLen, MessageHelper.GetMessage(MessageKey.文本长度上限小于0));

            if (value.Length < minLen)
            {
                message = $"文本长度下限为 {minLen}";
                return false;
            }

            if (value.Length > maxLen)
            {
                message = $"文本长度上限为 {maxLen}";
                return false;
            }

            message = string.Empty;
            return true;
        }

        /// <summary>
        /// 验证对象是否在指定范围之内
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valus"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Range<T>(T valus, T min, T max, out string message) where T : IComparable<T>
        {
            if (valus.CompareTo(min) < 0 || valus.CompareTo(max) > 0)
            {
                message = $"值需要在 {min} 至 {max} 之间";
                return false;
            }

            message = string.Empty;
            return true;
        }

        /// <summary>
        /// 验证xml节点名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool XmlNodeName(string value, out string message)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value == string.Empty)
            {
                message = "XML节点名不能为空";
                return false;
            }

            if (char.IsNumber(value[0]))
            {
                message = "XML节点名不能以数字开头";
                return false;
            }

            return XmlText(value, out message);
        }

        /// <summary>
        /// 验证xml文本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool XmlText(string value, out string message)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            Regex regex = new($"[#<>&'\"\\x00-\\x08\\x0b-\\x0c\\x0e-\\x1f]");
            if (regex.IsMatch(value))
            {
                message = "XML文本不能包含以下字符：\" [ # < > '";
                return false;
            }

            message = string.Empty;
            return true;
        }
    }
}
