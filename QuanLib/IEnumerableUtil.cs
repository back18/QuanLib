using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib
{
    /// <summary>
    /// IEnumerable工具类
    /// </summary>
    public static class IEnumerableUtil
    {
        /// <summary>
        /// 检查集合内是否包含空对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool ContainsNull<T>(IEnumerable<T> list)
        {
            if (list is null)
                return true;
            foreach (var item in list)
                if (item is null)
                    return true;
            return false;
        }
    }
}
