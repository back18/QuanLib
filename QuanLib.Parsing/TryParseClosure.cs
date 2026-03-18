using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuanLib.Parsing
{
    public class TryParseClosure<T>
    {
        public TryParseClosure(ParseHandler<T> parseHandler)
        {
            ArgumentNullException.ThrowIfNull(parseHandler, nameof(parseHandler));

            _parseHandler = parseHandler;
        }

        private readonly ParseHandler<T> _parseHandler;

        public bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out T result)
        {
            if (s is null)
            {
                result = default;
                return false;
            }

            try
            {
                result = _parseHandler.Invoke(s, provider);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
