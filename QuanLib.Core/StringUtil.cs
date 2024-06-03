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
        public static string Repeat(string value, int count)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            ThrowHelper.ArgumentOutOfMin(0, count, nameof(count));

            return string.Concat(Enumerable.Repeat(value, count));
        }
    }
}
