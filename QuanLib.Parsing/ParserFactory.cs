using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace QuanLib.Parsing
{
    public static class ParserFactory
    {
        static ParserFactory()
        {
            _cache = [];
            RegisterAll();
        }

        private static readonly ConcurrentDictionary<Type, IParser> _cache;

        public static IParser GetEnumParser<TEnum>() where TEnum : Enum
        {
            return _cache.GetOrAdd(typeof(TEnum), (type) => ParserProvider.CreateEnumParser<TEnum>());
        }

        public static IParser GetParser<T>() where T : IParsable<T>
        {
            return _cache.GetOrAdd(typeof(T), (type) => ParserProvider.CreateParser<T>());
        }

        public static IParser GetParser(Type type)
        {
            return _cache.GetOrAdd(type, ParserProvider.CreateParser);
        }

        public static bool Contains<T>() where T : IParsable<T>
        {
            return Contains(typeof(T));
        }

        public static bool Contains(Type type)
        {
            return _cache.ContainsKey(type); 
        }

        public static void Register<T>() where T : IParsable<T>
        {
            Register(typeof(T), new Parser<T>(T.Parse, T.TryParse));
        }

        public static void Register(Type type, IParser parser)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));
            ArgumentNullException.ThrowIfNull(parser, nameof(parser));

            _cache[type] = parser;
        }

        public static bool Unregister<T>() where T : IParsable<T>
        {
            return Unregister(typeof(T));
        }

        public static bool Unregister(Type type)
        {
            return _cache.Remove(type, out _);
        }

        public static void UnregisterAll()
        {
            _cache.Clear();
        }

        private static void RegisterAll()
        {
            Register<string>();
            Register<char>();
            Register<bool>();

            Register<sbyte>();
            Register<byte>();
            Register<short>();
            Register<ushort>();
            Register<int>();
            Register<uint>();
            Register<long>();
            Register<ulong>();
            Register<Int128>();
            Register<UInt128>();
            Register<BigInteger>();

            Register<Half>();
            Register<float>();
            Register<double>();
            Register<decimal>();
            Register<NFloat>();
            Register<Complex>();

            Register<TimeSpan>();
            Register<TimeOnly>();
            Register<DateOnly>();
            Register<DateTime>();
            Register<DateTimeOffset>();

            Register<Guid>();
            Register<IPNetwork>();
            Register<IPAddress>();

            Register<IntPtr>();
            Register<UIntPtr>();
        }
    }
}
