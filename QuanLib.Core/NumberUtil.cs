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
            int size = sizeof(byte) * 8;
            bool[] result = new bool[size];
            for (int b = 0; b < size; b++)
                result[b] = (value & (1 << (size - 1 - b))) != 0;
            return result;
        }

        public static bool[] ToBitArray(sbyte value)
        {
            int size = sizeof(sbyte) * 8;
            bool[] result = new bool[size];
            for (int b = 0; b < size; b++)
                result[b] = (value & (1 << (size - 1 - b))) != 0;
            return result;
        }

        public static bool[] ToBitArray(short value)
        {
            int size = sizeof(short) * 8;
            bool[] result = new bool[size];
            for (int b = 0; b < size; b++)
                result[b] = (value & (1 << (size - 1 - b))) != 0;
            return result;
        }

        public static bool[] ToBitArray(ushort value)
        {
            int size = sizeof(ushort) * 8;
            bool[] result = new bool[size];
            for (int b = 0; b < size; b++)
                result[b] = (value & (1 << (size - 1 - b))) != 0;
            return result;
        }

        public static bool[] ToBitArray(int value)
        {
            int size = sizeof(int) * 8;
            bool[] result = new bool[size];
            for (int b = 0; b < size; b++)
                result[b] = (value & (1 << (size - 1 - b))) != 0;
            return result;
        }

        public static bool[] ToBitArray(uint value)
        {
            int size = sizeof(uint) * 8;
            bool[] result = new bool[size];
            for (int b = 0; b < size; b++)
                result[b] = (value & (1 << (size - 1 - b))) != 0;
            return result;
        }

        public static byte ToByte(bool[] bits)
        {
            int size = sizeof(byte) * 8;
            ArgumentNullException.ThrowIfNull(bits, nameof(bits));
            ThrowHelper.ArrayLengthOutOfRange(size, bits, nameof(bits));

            byte value = 1;
            byte result = 0;
            for (int i = size - 1; i >= 0; i--)
            {
                if (bits[i])
                    result += value;
                value *= 2;
            }

            return result;
        }

        public static sbyte ToSbyte(bool[] bits)
        {
            int size = sizeof(sbyte) * 8;
            ArgumentNullException.ThrowIfNull(bits, nameof(bits));
            ThrowHelper.ArrayLengthOutOfRange(size, bits, nameof(bits));

            sbyte value = 1;
            sbyte result = 0;
            for (int i = size - 1; i >= 0; i--)
            {
                if (bits[i])
                    result += value;
                value *= 2;
            }

            return result;
        }

        public static short ToShort(bool[] bits)
        {
            int size = sizeof(short) * 8;
            ArgumentNullException.ThrowIfNull(bits, nameof(bits));
            ThrowHelper.ArrayLengthOutOfRange(size, bits, nameof(bits));

            short value = 1;
            short result = 0;
            for (int i = size - 1; i >= 0; i--)
            {
                if (bits[i])
                    result += value;
                value *= 2;
            }

            return result;
        }

        public static ushort ToUshort(bool[] bits)
        {
            int size = sizeof(ushort) * 8;
            ArgumentNullException.ThrowIfNull(bits, nameof(bits));
            ThrowHelper.ArrayLengthOutOfRange(size, bits, nameof(bits));

            ushort value = 1;
            ushort result = 0;
            for (int i = size - 1; i >= 0; i--)
            {
                if (bits[i])
                    result += value;
                value *= 2;
            }

            return result;
        }

        public static int ToInt(bool[] bits)
        {
            int size = sizeof(int) * 8;
            ArgumentNullException.ThrowIfNull(bits, nameof(bits));
            ThrowHelper.ArrayLengthOutOfRange(size, bits, nameof(bits));

            int value = 1;
            int result = 0;
            for (int i = size - 1; i >= 0; i--)
            {
                if (bits[i])
                    result += value;
                value *= 2;
            }

            return result;
        }

        public static uint ToUint(bool[] bits)
        {
            int size = sizeof(uint) * 8;
            ArgumentNullException.ThrowIfNull(bits, nameof(bits));
            ThrowHelper.ArrayLengthOutOfRange(size, bits, nameof(bits));

            uint value = 1;
            uint result = 0;
            for (int i = size - 1; i >= 0; i--)
            {
                if (bits[i])
                    result += value;
                value *= 2;
            }

            return result;
        }

        public static bool[] Negate(bool[] bits)
        {
            ArgumentNullException.ThrowIfNull(bits, nameof(bits));

            bool[] result = new bool[bits.Length];
            for (int i = 0; i < bits.Length; i++)
                result[i] = !bits[i];

            return result;
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
