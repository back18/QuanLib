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

        public static int DivisionFloor(int left, int right)
        {
            //return (int)Math.Floor((double)left / right);
            return left / right;
        }

        public static int DivisionCeiling(int left, int right)
        {
            //return (int)Math.Ceiling((double)left / right);
            int remainder = left % right;
            if (remainder > 0)
                return left / right + 1;
            else if (remainder < 0)
                return left / right - 1;
            else
                return left / right;
        }
    }
}
