using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public readonly struct JoinHelper(char separatorChar)
    {
        public char SeparatorChar { get; } = separatorChar;

        public string Combine(string value1, string value2)
        {
            ArgumentException.ThrowIfNullOrEmpty(value1, nameof(value1));
            ArgumentException.ThrowIfNullOrEmpty(value2, nameof(value2));

            value1 = value1.TrimEnd(SeparatorChar);
            value2 = value2.TrimStart(SeparatorChar);
            return $"{value1}{SeparatorChar}{value2}";
        }

        public string Combine(string value1, string value2, string value3)
        {
            ArgumentException.ThrowIfNullOrEmpty(value1, nameof(value1));
            ArgumentException.ThrowIfNullOrEmpty(value2, nameof(value2));
            ArgumentException.ThrowIfNullOrEmpty(value3, nameof(value3));

            value1 = value1.TrimEnd(SeparatorChar);
            value2 = value2.Trim(SeparatorChar);
            value3 = value3.TrimStart(SeparatorChar);
            return $"{value1}{SeparatorChar}{value2}{SeparatorChar}{value3}";
        }

        public string Combine(string value1, string value2, string value3, string value4)
        {
            ArgumentException.ThrowIfNullOrEmpty(value1, nameof(value1));
            ArgumentException.ThrowIfNullOrEmpty(value2, nameof(value2));
            ArgumentException.ThrowIfNullOrEmpty(value3, nameof(value3));
            ArgumentException.ThrowIfNullOrEmpty(value4, nameof(value4));

            value1 = value1.TrimEnd(SeparatorChar);
            value2 = value2.Trim(SeparatorChar);
            value3 = value3.Trim(SeparatorChar);
            value4 = value4.TrimStart(SeparatorChar);
            return $"{value1}{SeparatorChar}{value2}{SeparatorChar}{value3}{SeparatorChar}{value4}";
        }

        public string Combine(params string[] values)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));
            if (values.Length == 0)
                return string.Empty;
            if (values.Length == 1)
                return values[0];

            foreach (var value in values)
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException($"“{nameof(values)}”的一个或多个子项为 null 或空", nameof(values));
            }

            StringBuilder sb = new();
            sb.Append(values[0].TrimEnd(SeparatorChar));
            for (int i = 1; i < values.Length - 1; i++)
            {
                sb.Append(SeparatorChar);
                sb.Append(values[i].Trim(SeparatorChar));
            }
            sb.Append(SeparatorChar);
            sb.Append(values[^1].TrimStart(SeparatorChar));

            return sb.ToString();
        }
    }
}
