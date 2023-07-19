using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib
{
    /// <summary>
    /// char工具类
    /// </summary>
    public static class CharUtil
    {
        /// <summary>
        /// 复制指定数量的字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Copy(char value, int count)
        {
            if (count < 0)
                throw new ArgumentException("子符数量不可小于0");
            else if (count == 0)
                return string.Empty;

            char[] chars = new char[count];
            Array.Fill(chars, value);

            return new string(chars);
        }
    }
}
