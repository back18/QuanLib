using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class EnumUtil
    {
        public static T[] GetFlags<T>(T flags) where T : struct, Enum
        {
            List<T> result = [];
            T[] values = Enum.GetValues<T>();

            foreach (T value in values)
            {
                if (flags.HasFlag(value))
                    result.Add(value);
            }

            return result.ToArray();
        }
    }
}
