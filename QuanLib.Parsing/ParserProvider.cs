using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace QuanLib.Parsing
{
    public static class ParserProvider
    {
        private static readonly StringParser _stringParser = new();
        private static readonly Type[] _parseParameterTypes1 = [typeof(string), typeof(IFormatProvider)];
        private static readonly Type[] _parseParameterTypes2 = [typeof(string)];

        public static IParser<TEnum> CreateEnumParser<TEnum>() where TEnum : Enum
        {
            return new EnumParser<TEnum>();
        }

        public static IParser<T> CreateParser<T>() where T : IParsable<T>
        {
            return new Parser<T>(T.Parse, T.TryParse);
        }

        public static IParser CreateParser(Type type)
        {
            if (type.Equals(typeof(string)))
                return _stringParser;

            if (type.IsEnum)
                return new EnumParser(type);

            if (type.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IParsable<>)))
            {
                Type parsableType = typeof(IParsable<>).MakeGenericType(type);
                InterfaceMapping interfaceMapping = type.GetInterfaceMap(parsableType);

                MethodInfo? parseMethod =
                    interfaceMapping.TargetMethods.FirstOrDefault(s => s.Name == "Parse") ??
                    interfaceMapping.TargetMethods.FirstOrDefault(s => s.Name == $"System.IParsable<{ObjectFormatter.Format(type)}>.Parse") ??
                    throw new InvalidOperationException($"在类型“{type}”中找不到 Parse 方法");

                MethodInfo? tryParseMethod =
                    interfaceMapping.TargetMethods.FirstOrDefault(s => s.Name == "TryParse") ??
                    interfaceMapping.TargetMethods.FirstOrDefault(s => s.Name == $"System.IParsable<{ObjectFormatter.Format(type)}>.TryParse") ??
                    throw new InvalidOperationException($"在类型“{type}”中找不到 TryParse 方法");

                ParseInvoker parseInvoker = new(parseMethod);
                TryParseInvoker tryParseInvoker = new(tryParseMethod);
                return new Parser<object>(parseInvoker.Parse, tryParseInvoker.TryParse);
            }
            else
            {
                MethodInfo? parseMethod = GetParseMethod(type);
                MethodInfo? tryParseMethod = GetTryParseMethod(type);

                if (parseMethod is not null && tryParseMethod is not null)
                {
                    ParseInvoker parseInvoker = new(parseMethod);
                    TryParseInvoker tryParseInvoker = new(tryParseMethod);
                    return new Parser<object>(parseInvoker.Parse, tryParseInvoker.TryParse);
                }
                else if (parseMethod is not null)
                {
                    ParseInvoker parseInvoker = new(parseMethod);
                    TryParseClosure<object> tryParseClosure = new(parseInvoker.Parse);
                    return new Parser<object>(parseInvoker.Parse, tryParseClosure.TryParse);
                }
                else if (tryParseMethod is not null)
                {
                    TryParseInvoker tryParseInvoker = new(tryParseMethod);
                    ParseClosure<object> parseClosure = new(tryParseInvoker.TryParse);
                    return new Parser<object>(parseClosure.Parse, tryParseInvoker.TryParse);
                }
                else
                {
                    throw new NotSupportedException($"类型“{type}”未实现 IParsable<TSelf> 接口，且找不到 Parse 方法或 TryParse 方法");
                }
            }
        }

        private static MethodInfo? GetParseMethod(Type type)
        {
            return type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, _parseParameterTypes1) ??
                   type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, _parseParameterTypes2);
        }

        private static MethodInfo? GetTryParseMethod(Type type)
        {
            return type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, [typeof(string), typeof(IFormatProvider), type.MakeByRefType()]) ??
                   type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, [typeof(string), type.MakeByRefType()]);
        }
    }
}
