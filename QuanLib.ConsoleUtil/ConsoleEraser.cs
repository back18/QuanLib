using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    public class ConsoleEraser
    {
        public ConsoleEraser(int startLine)
        {
            StartLine = startLine;
            LineCount = 0;
            LineMaxWidth = Console.BufferWidth > Console.WindowWidth ? Console.BufferWidth : Console.WindowWidth;
            _EmptyLine = new(' ', LineCount);
        }

        /// <summary>
        /// 开始行
        /// </summary>
        public int StartLine { get; private set; }

        /// <summary>
        /// 结束行
        /// </summary>
        public int EndLine => StartLine + LineCount;

        /// <summary>
        /// 行数
        /// </summary>
        public int LineCount { get; private set; }

        /// <summary>
        /// 每行最大宽度
        /// </summary>
        public int LineMaxWidth { get; private set; }

        /// <summary>
        /// 一个可以覆盖整行的空白字符串
        /// </summary>
        public string EmptyLine
        {
            get
            {
                if (_EmptyLine.Length != LineMaxWidth)
                    _EmptyLine = new(' ', LineMaxWidth);
                return _EmptyLine;
            }
        }
        private string _EmptyLine;

        public void AddLineNumber(int lineNumber)
        {
            LineCount += lineNumber;
        }

        public void Reset(int startLine)
        {
            StartLine = startLine;
            Reset();
        }

        public void Reset()
        {
            LineCount = 0;
            LineMaxWidth = Console.BufferWidth > Console.WindowWidth ? Console.BufferWidth : Console.WindowWidth;
        }

        public void Erase(bool developerMode = false)
        {
            Console.SetCursorPosition(0, StartLine);

            if (developerMode )
            {
                string emptyLine = new('-', LineMaxWidth);
                for (int i = 0; i < LineCount; i++)
                    Console.Write(emptyLine);
            }
            else
            {
                for (int i = 0; i < LineCount; i++)
                    Console.Write(EmptyLine);
            }
        }
    }
}
