using QuanLib.ExceptionHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib
{
    /// <summary>
    /// 32位十六进制数
    /// </summary>
    public class Hex32
    {
        static Hex32()
        {
            ToHexMap = new();
            char num = '0';
            for (byte i = 0; i < 10; i++)
                ToHexMap.Add(i, num++);
            ToHexMap.Add(10, 'a');
            ToHexMap.Add(11, 'b');
            ToHexMap.Add(12, 'c');
            ToHexMap.Add(13, 'd');
            ToHexMap.Add(14, 'e');
            ToHexMap.Add(15, 'f');

            ToDecMap= new();
            foreach (var item in ToHexMap)
                ToDecMap.Add(item.Value, item.Key);
        }

        public Hex32()
        {
            _hex = new byte[8];
        }

        public Hex32(int value)
        {
            _hex = new byte[8];

            string hexString = Convert.ToString(value, 16);
            if (hexString.Length < 8)
            {
                hexString = StringUtil.Copy("0", 8 - hexString.Length) + hexString;
            }
            for (int i = 0; i < 8; i++)
            {
                _hex[i] = ToDecMap[hexString[7 - i]];
            }
        }

        private static readonly Dictionary<byte, char> ToHexMap;

        private static readonly Dictionary<char, byte> ToDecMap;

        private byte[] _hex;

        public int Bit
        {
            get
            {
                for (int i = 7; i >= 0; i--)
                    if (_hex[i] != 0)
                        return i + 1;
                return 1;
            }
        }

        public string ToString(int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, MessageHelper.GetMessage(MessageKey.字符串长度小于0));

            if (Bit < length)
                return StringUtil.Copy("0", length - Bit) + ToString();
            return ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            for (int i = Bit - 1; i >= 0; i--)
                sb.Append(ToHexMap[_hex[i]]);

            return sb.ToString();
        }

        public static Hex32 operator ++(Hex32 value)
        {
            for (int i = 0; i < 8; i++)
            {
                if (value._hex[i] < 15)
                {
                    value._hex[i]++;
                    break;
                }
                else value._hex[i] = 0;
            }

            return value;
        }

        public static Hex32 operator --(Hex32 value)
        {
            for (int i = 0; i < 8; i++)
            {
                if (value._hex[i] > 0)
                {
                    value._hex[i]--;
                    break;
                }
                else value._hex[i] = 15;
            }

            return value;
        }
    }
}
