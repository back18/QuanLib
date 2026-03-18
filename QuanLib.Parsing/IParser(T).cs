using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuanLib.Parsing
{
    public interface IParser<T> : IParser where T : notnull
    {
        public new T Parse(string s);

        public bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out T result);

        public new T Parse(string s, IFormatProvider? provider);

        public bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out T result);
    }
}
