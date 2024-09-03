using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T[] GetFlags<T>(this T flags) where T : struct, Enum
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
