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

            if (type == typeof(string))
                return new(StringParseHandler, StringTryParseHandler);

            if (!IsImplIParsable(type))
                throw new InvalidOperationException($"类型“{type}”未实现 IParsable<TSelf> 接口");

            Type iParsableType = GetIParsableType(type);
            InterfaceMapping interfaceMapping = type.GetInterfaceMap(iParsableType);

            MethodInfo? parseMethod =
                interfaceMapping.TargetMethods.Where(s => s.Name == "Parse").FirstOrDefault() ??
                interfaceMapping.TargetMethods.Where(s => s.Name == $"System.IParsable<{type.Namespace}.{type.Name}>.Parse").FirstOrDefault() ??
                throw new InvalidOperationException($"在类型“{type}”中找不到 Parse 方法");

            MethodInfo? tryParseMethod =
                interfaceMapping.TargetMethods.Where(s => s.Name == "TryParse").FirstOrDefault() ??
                interfaceMapping.TargetMethods.Where(s => s.Name == $"System.IParsable<{type.Namespace}.{type.Name}>.TryParse").FirstOrDefault() ??
                throw new InvalidOperationException($"在类型“{type}”中找不到 TryParse 方法");

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

        public static Parser<T> FromType<T>() where T : IParsable<T>
        {
            return new(T.Parse, T.TryParse);
        }

        public static bool IsImplIParsable(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            return type.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IParsable<>));
        }

        public static Type GetIParsableType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            return typeof(IParsable<>).MakeGenericType(type);
        }

        private static string StringParseHandler(string s, IFormatProvider? provider)
        {
            ArgumentNullException.ThrowIfNull(s, nameof(s));

            return s;
        }

        private static bool StringTryParseHandler([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out object result)
        {
            if (s is null)
            {
                result = null;
                return false;
            }

            result = s;
            return true;
        }
    }
}
