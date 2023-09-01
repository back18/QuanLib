using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class NumberUtil
    {
        public static bool[] ToBoolArray(byte value)
        {
            bool[] result = new bool[8];
            int b = 7;
            for (int i = 0; i < 8; i++)
                result[i] = (value & 1 << b--) != 0;
            return result;
        }

        public static bool[] ToBoolArray(string value)
        {
            return ToBoolArray(Convert.ToByte(value, 16));
        }
    }
}
