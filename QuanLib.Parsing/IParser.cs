using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuanLib.Parsing
{
    public interface IParser
    {
        public object Parse(string s);

        public bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out object result);

        public object Parse(string s, IFormatProvider? provider);

        public bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out object result);
    }
}
