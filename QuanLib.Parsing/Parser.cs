using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Parsing
{
    public class Parser<T> : IParser<T> where T : notnull
    {
        public Parser(ParseHandler<T> parseHandler)
        {
            ArgumentNullException.ThrowIfNull(parseHandler, nameof(parseHandler));

            TryParseClosure<T> tryParseClosure = new(parseHandler);
            _parseHandler = parseHandler;
            _tryParseHandler = tryParseClosure.TryParse;
        }

        public Parser(TryParseHandler<T> tryParseHandler)
        {
            ArgumentNullException.ThrowIfNull(tryParseHandler, nameof(tryParseHandler));

            ParseClosure<T> parseClosure = new(tryParseHandler);
            _parseHandler = parseClosure.Parse;
            _tryParseHandler = tryParseHandler;
        }

        public Parser(ParseHandler<T> parseHandler, TryParseHandler<T> tryParseHandler)
        {
            ArgumentNullException.ThrowIfNull(parseHandler, nameof(parseHandler));
            ArgumentNullException.ThrowIfNull(tryParseHandler, nameof(tryParseHandler));

            _parseHandler = parseHandler;
            _tryParseHandler = tryParseHandler;
        }

        private readonly ParseHandler<T> _parseHandler;

        private readonly TryParseHandler<T> _tryParseHandler;

        public T Parse(string s)
        {
            return Parse(s, null);
        }

        public bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out T result)
        {
            return TryParse(s, null, out result);
        }

        public T Parse(string s, IFormatProvider? provider)
        {
            return _parseHandler.Invoke(s, provider);
        }

        public bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out T result)
        {
            return _tryParseHandler.Invoke(s, provider, out result);
        }

        object IParser.Parse(string s)
        {
            return Parse(s);
        }

        bool IParser.TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out object result)
        {
            if (TryParse(s, out var result2))
            {
                result = result2;
                return true;
            }
            else
            {
                result = result2;
                return false;
            }
        }

        object IParser.Parse(string s, IFormatProvider? provider)
        {
            return Parse(s, provider);
        }

        bool IParser.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out object result)
        {
            if (TryParse(s, provider, out var result2))
            {
                result = result2;
                return true;
            }
            else
            {
                result = result2;
                return false;
            }
        }
    }
}
