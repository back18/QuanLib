using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuanLib.Parsing
{
    public class StringParser : IParser<string>
    {
        public string Parse(string s)
        {
            ArgumentNullException.ThrowIfNull(s);
            return s;
        }

        public bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out string result)
        {
            result = s;
            return s is not null;
        }

        public string Parse(string s, IFormatProvider? provider)
        {
            ArgumentNullException.ThrowIfNull(s);
            return s;
        }

        public bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out string result)
        {
            result = s;
            return s is not null;
        }

        object IParser.Parse(string s)
        {
            ArgumentNullException.ThrowIfNull(s);
            return s;
        }

        bool IParser.TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out object result)
        {
            result = s;
            return s is not null;
        }

        object IParser.Parse(string s, IFormatProvider? provider)
        {
            ArgumentNullException.ThrowIfNull(s);
            return s;
        }

        bool IParser.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out object result)
        {
            result = s;
            return s is not null;
        }
    }
}
