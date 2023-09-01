using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class EnumUtil
    {
        public static T[] ToArray<T>() where T : Enum
        {
            // 获取枚举的所有值
            var enumValues = Enum.GetValues(typeof(T));

            // 创建一个T[]数组，并将枚举值转换为T类型
            var result = new T[enumValues.Length];
            for (int i = 0; i < enumValues.Length; i++)
            {
                result[i] = (T)(enumValues.GetValue(i) ?? throw new InvalidOperationException());
            }

            // 返回结果
            return result;
        }
    }
}
