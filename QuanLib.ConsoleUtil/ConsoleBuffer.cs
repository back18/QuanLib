using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    public class ConsoleBuffer
    {
        public ConsoleBuffer()
        {
            _buffer = new();
            _cursors = new();
            UserIndex = 0;
            RefreshCursors();
        }

        private readonly StringBuilder _buffer;

        private readonly List<CursorPosition> _cursors;

        /// <summary>
        /// 缓冲区长度
        /// </summary>
        public int Length => _buffer.Length;

        /// <summary>
        /// 光标起始位置
        /// </summary>
        public CursorPosition StartCursor { get; set; }

        /// <summary>
        /// 光标截止位置
        /// </summary>
        public CursorPosition EndCursor => _cursors[^1];

        /// <summary>
        /// 用户光标位置
        /// </summary>
        public CursorPosition UserCursor => _cursors[UserIndex];

        /// <summary>
        /// 用户光标处于输入缓冲区的索引位置
        /// </summary>
        public int UserIndex { get; private set; }

        /// <summary>
        /// 每行最大宽度
        /// </summary>
        public int LineMaxWidth { get; private set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int LineCount => _cursors[^1].CursorTop - _cursors[0].CursorTop + 1;

        /// <summary>
        /// 刷新光标映射表
        /// </summary>
        public void RefreshCursors()
        {
            _cursors.Clear();
            LineMaxWidth = Console.BufferWidth > Console.WindowWidth ? Console.BufferWidth : Console.WindowWidth;
            int line = StartCursor.CursorTop;
            int width = StartCursor.CursorLeft;
            for (int i = 0; i < _buffer.Length; i++)
            {
                _cursors.Add(new(width, line));
                width += ConsoleUtil.GetWidth(_buffer[i]);
                if (width == LineMaxWidth)
                {
                    width = 0;
                    line++;
                }
                else if (width > LineMaxWidth)
                {
                    width = ConsoleUtil.GetWidth(_buffer[i]);
                    line++;
                }
            }
            _cursors.Add(new(width, line));
        }

        /// <summary>
        /// 移动光标
        /// </summary>
        /// <param name="index"></param>
        public void MoveCursor(int index)
        {
            UserIndex = index;
            if (UserIndex < 0)
                UserIndex = 0;
            else if (UserIndex > _buffer.Length)
                UserIndex = _buffer.Length;

            _cursors[UserIndex].SetToConsole();
        }

        /// <summary>
        /// 偏移光标
        /// </summary>
        public void OffsetCursor(int offset)
        {
            MoveCursor(UserIndex + offset);
        }

        public void MoveCursorToStart()
        {
            MoveCursor(0);
        }

        public void MoveCursorToEnd()
        {
            MoveCursor(_buffer.Length);
        }

        //public void MoveCursorToPrev()
        //{

        //}

        //public void MoveCursorToNext()
        //{

        //}

        public CursorPosition GetCursor(int index)
            => _cursors[index];

        public void Append(char value)
        {
            if (UserIndex == _buffer.Length)
                _buffer.Append(value);
            else
                _buffer.Insert(UserIndex, value);
            RefreshCursors();
            OffsetCursor(1);
        }

        public void Append(string value)
        {
            if (UserIndex == _buffer.Length)
                _buffer.Append(value);
            else
                _buffer.Insert(UserIndex, value);
            RefreshCursors();
            OffsetCursor(value.Length);
        }

        public void Remove()
        {
            if (_buffer.Length == 0)
                return;

            _buffer.Remove(UserIndex - 1, 1);
            RefreshCursors();
            OffsetCursor(-1);
        }

        public void Clear()
        {
            _buffer.Clear();
            _cursors.Clear();
            RefreshCursors();
            UserIndex = 0;
        }

        public override string ToString()
        {
            return _buffer.ToString();
        }
    }
}
