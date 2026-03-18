using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuanLib.Parsing
{
    public class EnumParser<TEnum> : IParser<TEnum> where TEnum : Enum
    {
        private readonly EnumParser _enumParser = new(typeof(TEnum));

        public TEnum Parse(string s)
        {
            return Parse(s, null);
        }

        public TEnum Parse(string s, IFormatProvider? provider)
        {
            return (TEnum)_enumParser.Parse(s, provider);
        }

        public bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out TEnum result)
        {
            return TryParse(s, null, out result);
        }

        public bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TEnum result)
        {
            if (_enumParser.TryParse(s, provider, out var objResult) && objResult is TEnum enumResult)
            {
                result = enumResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        object IParser.Parse(string s)
        {
            return _enumParser.Parse(s);
        }

        object IParser.Parse(string s, IFormatProvider? provider)
        {
            return _enumParser.Parse(s, provider);
        }

        bool IParser.TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out object result)
        {
            return _enumParser.TryParse(s, out result);
        }

        bool IParser.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out object result)
        {
            return _enumParser.TryParse(s, provider, out result);
        }
    }
}
