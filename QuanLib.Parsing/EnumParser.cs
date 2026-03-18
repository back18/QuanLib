using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuanLib.Parsing
{
    public class EnumParser : IParser
    {
        public EnumParser(Type targetType)
        {
            ArgumentNullException.ThrowIfNull(targetType, nameof(targetType));
            if (!targetType.IsEnum)
                throw new ArgumentException("Target type must be an enum.", nameof(targetType));

            TargetType = targetType;
        }

        private static readonly char[] _digits = "0123456789+-".ToCharArray();

        public Type TargetType { get; }

        public object Parse(string s)
        {
            return Parse(s, null);
        }

        public object Parse(string s, IFormatProvider? provider)
        {
            ArgumentException.ThrowIfNullOrEmpty(s, nameof(s));
            if (s.All(ch => Array.IndexOf(_digits, ch) != -1))
                throw new FormatException("Unable to parse digits into enum");

            return Enum.Parse(TargetType, s, true);
        }

        public bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out object result)
        {
            return TryParse(s, null, out result);
        }

        public bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out object result)
        {
            if (string.IsNullOrEmpty(s) || s.All(ch => Array.IndexOf(_digits, ch) != -1))
            {
                result = null;
                return false;
            }

            return Enum.TryParse(TargetType, s, true, out result);
        }
    }
}
