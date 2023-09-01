using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class BoolArrayMap
    {
        static BoolArrayMap()
        {
            _bytes = new();
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                _bytes.Add((byte)i, NumberUtil.ToBoolArray((byte)i));
            }

            _lowers = new();
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
                _lowers.Add(i.ToString("x2"), _bytes[(byte)i]);

            _uppers = new();
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
                _uppers.Add(i.ToString("X2"), _bytes[(byte)i]);
        }

        private static readonly Dictionary<byte, bool[]> _bytes;

        private static readonly Dictionary<string, bool[]> _lowers;

        private static readonly Dictionary<string, bool[]> _uppers;

        public static bool[] FromByte(byte b) => _bytes[b];

        public static bool[] FromLiwer(string s) => _lowers[s];

        public static bool[] FromUpper(string s) => _uppers[s];
    }
}
