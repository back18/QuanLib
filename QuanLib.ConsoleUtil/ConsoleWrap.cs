using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 表示换行上下文信息
    /// </summary>
    public readonly struct ConsoleWrap
    {
        public ConsoleWrap(CursorPosition up, CursorPosition down)
        {
            Up = up;
            Down = down;
        }

        public CursorPosition Up { get; }

        public CursorPosition Down { get; }
    }
}
