using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 写入工具
    /// </summary>
    public static class WriteUtil
    {
        public static void WriteLine<T>(IEnumerable<T> values, char separator = ' ')
        {
            System.Console.WriteLine(string.Join(separator, values));
        }
    }
}
