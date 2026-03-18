using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace QuanLib.Parsing
{
    public static class ParserPriority
    {
        static ParserPriority()
        {
            _cache = [];
            RegisterAll();
        }

        private static readonly ConcurrentDictionary<Type, int> _cache;

        public static int GetPriority<T>()
        {
            return GetPriority(typeof(T));
        }

        public static int GetPriority(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            if (_cache.TryGetValue(type, out var priority))
                return priority;
            else if (type.IsEnum)
                return GetPriority<bool>() - 1;
            else
                return 0;
        }

        public static bool Contains<T>()
        {
            return Contains(typeof(T));
        }

        public static bool Contains(Type type)
        {
            return _cache.ContainsKey(type);
        }

        public static void Register<T>(int priority)
        {
            Register(typeof(T), priority);
        }

        public static void Register(Type type, int priority)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            _cache[type] = priority;
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
            Register<bool>(1200_000_000);
            Register<Guid>(1100_000_000);
            Register<IPNetwork>(1000_000_000);

            Register<sbyte>(2000_000);
            Register<byte>(1900_000);
            Register<short>(1800_000);
            Register<ushort>(1700_000);
            Register<int>(1600_000);
            Register<uint>(1500_000);
            Register<long>(1400_000);
            Register<ulong>(1300_000);
            Register<Int128>(1200_000);
            Register<UInt128>(1100_000);
            Register<BigInteger>(1000_000);

            Register<Half>(1500);
            Register<float>(1400);
            Register<double>(1300);
            Register<decimal>(1200);
            Register<NFloat>(1100);
            Register<Complex>(1000);

            Register<TimeSpan>(-1000);
            Register<TimeOnly>(-1100);
            Register<DateOnly>(-1200);
            Register<DateTime>(-1300);
            Register<DateTimeOffset>(-1400);

            Register<IPAddress>(-1000_000);
            Register<IntPtr>(-1100_000);
            Register<UIntPtr>(-1200_000);

            Register<char>(int.MinValue + 1);
            Register<string>(int.MinValue);
        }
    }
}
