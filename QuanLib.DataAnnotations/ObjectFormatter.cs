using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.DataAnnotations
{
    public static class ObjectFormatter
    {
        public static string Format(object? value)
        {
            if (value is null)
                return "null";

            if (value is string str)
                return $"\"{str}\"";

            if (value is IEnumerable enumerable)
            {
                List<string> list = [];
                foreach (var item in enumerable)
                    list.Add(Format(item));
                return $"[{string.Join(", ", list)}]";
            }

            return value.ToString() ?? "null";
        }
    }
}
