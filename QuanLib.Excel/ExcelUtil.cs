using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Excel
{
    public static class ExcelUtil
    {
        private static readonly char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static string ToLetter(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "参数不能为等于或小于0");

            List<int> items = new();
            StringBuilder sb = new();
            while (true)
            {
                (int result, int remainder) result = Get(value);
                items.Add(result.remainder);
                if (result.result == 0)
                    break;
                else value = result.result;
            }

            items.Reverse();
            foreach (var item in items)
                sb.Append(letters[item - 1]);

            return sb.ToString();

            static (int result, int remainder) Get(int value)
            {
                (int result, int remainder) result;
                result.result = value / 26;
                result.remainder = value % 26;
                if (result.remainder == 0 && result.result > 0)
                {
                    result.remainder = 26;
                    result.result--;
                }
                return result;
            }
        }

        public static string ToString(int row, int column)
        {
            return ToLetter(column) + row;
        }

        public static string ToString(int startRow, int startColumn, int endRow, int endColumn)
        {
            return ToString(startRow, startColumn) + ":" + ToString(endRow, endColumn);
        }
    }
}
