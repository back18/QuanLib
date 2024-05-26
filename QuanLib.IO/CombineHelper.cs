using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public static class CombineHelper
    {
        public static string Combine(char separatorChar, string value1, string value2)
        {
            ArgumentException.ThrowIfNullOrEmpty(value1, nameof(value1));
            ArgumentException.ThrowIfNullOrEmpty(value2, nameof(value2));

            value1 = value1.TrimEnd(separatorChar);
            value2 = value2.TrimStart(separatorChar);
            return $"{value1}{separatorChar}{value2}";
        }

        public static string Combine(char separatorChar, string value1, string value2, string value3)
        {
            ArgumentException.ThrowIfNullOrEmpty(value1, nameof(value1));
            ArgumentException.ThrowIfNullOrEmpty(value2, nameof(value2));
            ArgumentException.ThrowIfNullOrEmpty(value3, nameof(value3));

            value1 = value1.TrimEnd(separatorChar);
            value2 = value2.Trim(separatorChar);
            value3 = value3.TrimStart(separatorChar);
            return $"{value1}{separatorChar}{value2}{separatorChar}{value3}";
        }

        public static string Combine(char separatorChar, string value1, string value2, string value3, string value4)
        {
            ArgumentException.ThrowIfNullOrEmpty(value1, nameof(value1));
            ArgumentException.ThrowIfNullOrEmpty(value2, nameof(value2));
            ArgumentException.ThrowIfNullOrEmpty(value3, nameof(value3));
            ArgumentException.ThrowIfNullOrEmpty(value4, nameof(value4));

            value1 = value1.TrimEnd(separatorChar);
            value2 = value2.Trim(separatorChar);
            value3 = value3.Trim(separatorChar);
            value4 = value4.TrimStart(separatorChar);
            return $"{value1}{separatorChar}{value2}{separatorChar}{value3}{separatorChar}{value4}";
        }

        public static string Combine(char separatorChar, params string[] values)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));
            CollectionValidator.ValidateNullOrEmpty(values, nameof(values));

            if (values.Length == 0)
                return string.Empty;
            if (values.Length == 1)
                return values[0];

            StringBuilder sb = new();
            sb.Append(values[0].TrimEnd(separatorChar));
            for (int i = 1; i < values.Length - 1; i++)
            {
                sb.Append(separatorChar);
                sb.Append(values[i].Trim(separatorChar));
            }
            sb.Append(separatorChar);
            sb.Append(values[^1].TrimStart(separatorChar));

            return sb.ToString();
        }
    }
}
