using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class Parser<T>
    {
        public Parser(ParseHandler<T> parseHandler)
        {
            ArgumentNullException.ThrowIfNull(parseHandler, nameof(parseHandler));

            _parseHandler = parseHandler;
            _tryParseHandler = ([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out T result) =>
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
            };
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

        public T Parse(string s, IFormatProvider? provider)
        {
            try
            {
                return _parseHandler.Invoke(s, provider);
            }
            catch (TargetInvocationException targetInvocationException)
            {
                if (targetInvocationException.InnerException is not null)
                    throw targetInvocationException.InnerException;
                else
                    throw;
            }
        }

        public bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out T result)
        {
            return _tryParseHandler.Invoke(s, provider, out result);
        }
    }
}
