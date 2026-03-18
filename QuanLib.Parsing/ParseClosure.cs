using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Parsing
{
    public class ParseClosure<T>
    {
        public ParseClosure(TryParseHandler<T> tryParseHandler)
        {
            ArgumentNullException.ThrowIfNull(tryParseHandler, nameof(tryParseHandler));

            _tryParseHandler = tryParseHandler;
        }

        private readonly TryParseHandler<T> _tryParseHandler;

        public T Parse(string s, IFormatProvider? provider)
        {
            ArgumentNullException.ThrowIfNull(s, nameof(s));

            if (_tryParseHandler.Invoke(s, provider, out var result))
                return result;
            else
                throw new FormatException();
        }
    }
}
