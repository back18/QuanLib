using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    public readonly struct CursorPosition
    {
        public CursorPosition()
        {
            CursorLeft = 0;
            CursorTop = 0;
        }

        public CursorPosition(int cursorLeft, int cursorTop)
        {
            CursorLeft = cursorLeft;
            CursorTop = cursorTop;
        }

        public  int CursorLeft { get; }

        public int CursorTop { get; }

        public static CursorPosition Now => new(Console.CursorLeft, Console.CursorTop);

        public void SetToConsole()
        {
            Console.SetCursorPosition(CursorLeft, CursorTop);
        }

        public void SetToConsole(int leftOffset, int topOffset)
        {
            Console.SetCursorPosition(CursorLeft + leftOffset, CursorTop + topOffset);
        }
    }
}
