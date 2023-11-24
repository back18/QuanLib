using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class ByteBitMapping
    {
        static ByteBitMapping()
        {
            _bytes = [];
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
                _bytes.Add((byte)i, NumberUtil.ToBitArray((byte)i));

            _lowers = [];
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
                _lowers.Add(i.ToString("x2"), _bytes[(byte)i]);

            _uppers = [];
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
                _uppers.Add(i.ToString("X2"), _bytes[(byte)i]);
        }

        private static readonly Dictionary<byte, bool[]> _bytes;

        private static readonly Dictionary<string, bool[]> _lowers;

        private static readonly Dictionary<string, bool[]> _uppers;

        public static bool[] FromByte(byte b) => Clone(_bytes[b]);

        public static bool[] FromLiwer(string s) => Clone(_lowers[s]);

        public static bool[] FromUpper(string s) => Clone(_uppers[s]);

        private static bool[] Clone(bool[] source)
        {
            bool[] result = new bool[source.Length];
            source.CopyTo(result, 0);
            return result;
        }
    }
}
