using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class NumberUtil
    {
        public static bool[] ToBitArray(byte value)
        {
            bool[] result = new bool[8];
            for (int b = 0; b < 8; b++)
                result[b] = (value & (1 << (7 - b))) != 0;
            return result;
        }

        public static bool[] ToBitArray(string value)
        {
            return ToBitArray(Convert.ToByte(value, 16));
        }
    }
}
