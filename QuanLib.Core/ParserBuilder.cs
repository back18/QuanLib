using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class ParserBuilder
    {
        public static Parser<object> FromType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            MethodInfo? parseMethod = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(IFormatProvider)], null);
            if (parseMethod is null || parseMethod.ReturnType != type)
                throw new InvalidOperationException($"在类型“{type}”中找不到合法的 Parse 方法");

            MethodInfo? tryParseMethod = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, [typeof(string), typeof(IFormatProvider), type.MakeByRefType()]);
            if (tryParseMethod is null || tryParseMethod.ReturnType != typeof(bool))
                throw new InvalidOperationException($"在类型“{type}”中找不到合法的 TryParse 方法");

            object parseHandler(string s, IFormatProvider? provider)
            {
                return parseMethod.Invoke(null, [s, provider]) ?? throw new FormatException();
            }

            bool tryParseHandler([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out object result)
            {
                object?[] parameters = [s, null, null];
                object? returnValue = tryParseMethod.Invoke(null, parameters);
                result = parameters[2];
                if (returnValue is bool b)
                    return b;
                else
                    return false;
            }

            return new(parseHandler, tryParseHandler);
        }

        public static Parser<T> FromType<T>()
        {
            Type type = typeof(T);

            MethodInfo? parseMethod = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(IFormatProvider)], null);
            if (parseMethod is null || parseMethod.ReturnType != type)
                throw new InvalidOperationException($"在类型“{type}”中找不到合法的 Parse 方法");

            MethodInfo? tryParseMethod = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, [typeof(string), typeof(IFormatProvider), type.MakeByRefType()]);
            if (tryParseMethod is null || tryParseMethod.ReturnType != typeof(bool))
                throw new InvalidOperationException($"在类型“{type}”中找不到合法的 TryParse 方法");

            T parseHandler(string s, IFormatProvider? provider)
            {
                return (T)(parseMethod!.Invoke(null, [s, provider]) ?? throw new FormatException());
            }

            bool tryParseHandler([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out T result)
            {
                object?[] parameters = [s, null, null];
                object? returnValue = tryParseMethod!.Invoke(null, parameters);
                if (parameters[2] is T t)
                    result = t;
                else
                    result = default;
                if (returnValue is bool b)
                    return b;
                else
                    return false;
            }

            return new(parseHandler, tryParseHandler);
        }
    }
}
