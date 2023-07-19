using System;
using System.Runtime.InteropServices;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 控制台绘制基类
    /// </summary>
    public abstract class ConsoleDraw
    {
        protected ConsoleDraw(int width)
        {
            Width = width;
            InitialBackgroundColor = DrawBackgroundColor = System.Console.BackgroundColor;
            InitialForegroundColor = DrawForegroundColor = System.Console.ForegroundColor;
            IsCursor = false;
            DrawStartPosition = 0;
            AutoSetBufferWidth = false;
            AutoSetWindowWidth = false;
        }

        /// <summary>
        /// 初始背景颜色
        /// </summary>
        public ConsoleColor InitialBackgroundColor { get; private set; }

        /// <summary>
        /// 初始字体颜色
        /// </summary>
        public ConsoleColor InitialForegroundColor { get; private set; }

        /// <summary>
        /// 绘制时背景颜色
        /// </summary>
        public ConsoleColor DrawBackgroundColor
        {
            get => _DrawBackgroundColor;
            set
            {
                _DrawBackgroundColor = value;
            }
        }
        private ConsoleColor _DrawBackgroundColor;

        /// <summary>
        /// 绘制时字体颜色
        /// </summary>
        public ConsoleColor DrawForegroundColor
        {
            get => _DrawForegroundColor;
            set
            {
                _DrawForegroundColor = value;
            }
        }
        private ConsoleColor _DrawForegroundColor;

        /// <summary>
        /// 渲染区域宽度
        /// </summary>
        public int Width
        {
            get => _Width;
            protected set
            {
                if (value < 1)
                    throw new ArgumentException("渲染区域宽度小于1");

                if (value > System.Console.BufferWidth)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && AutoSetBufferWidth)
                    {
                        System.Console.BufferWidth = value + 1;
                        _Width = value;
                    }
                    else throw new ArgumentException($"渲染区域宽度({value})必须小于或等于缓冲区宽度-1({System.Console.BufferWidth - 1})");
                }
                else _Width = value;

                if (value > System.Console.WindowWidth && RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && AutoSetWindowWidth)
                    System.Console.WindowWidth = value;

                if (IsCursor)
                    DrawStartPosition = GetCursorCenter(Width);
            }
        }
        private int _Width;

        /// <summary>
        /// 是否居中渲染
        /// </summary>
        public bool IsCursor
        {
            get => _IsCursor;
            set
            {
                _IsCursor = value;
                if (value)
                    DrawStartPosition = GetCursorCenter(Width);
                else DrawStartPosition = 0;
            }
        }
        private bool _IsCursor;

        /// <summary>
        /// 渲染起始位置
        /// </summary>
        public int DrawStartPosition
        {
            get => _DrawStartPosition;
            set
            {
                if (value < 0)
                    throw new ArgumentException("渲染起始位置不可小于0");
                else _DrawStartPosition = value;

                if (value == GetCursorCenter(Width))
                    _IsCursor = true;
                else _IsCursor = false;
            }
        }
        private int _DrawStartPosition;

        /// <summary>
        /// 是否根据渲染区域宽度自动调整缓冲去宽度
        /// </summary>
        public bool AutoSetBufferWidth { get; set; }

        /// <summary>
        /// 是否根据渲染区域宽度自动调整窗口宽度
        /// </summary>
        public bool AutoSetWindowWidth { get; set; }

        /// <summary>
        /// 渲染结束位置
        /// </summary>
        public int DrawEndPosition => DrawStartPosition + Width;

        /// <summary>
        /// 应用绘制时颜色
        /// </summary>
        protected void ApplyDrawColors()
        {
            System.Console.BackgroundColor = DrawBackgroundColor;
            System.Console.ForegroundColor = DrawForegroundColor;
        }

        /// <summary>
        /// 应用初始颜色
        /// </summary>
        protected void ApplyInitialColors()
        {
            System.Console.BackgroundColor = InitialBackgroundColor;
            System.Console.ForegroundColor = InitialForegroundColor;
        }

        /// <summary>
        /// 若光标位置在渲染起始位置的左边，那么移动光标到渲染起始位置
        /// </summary>
        protected void SetDrawStartPosition()
        {
            if (System.Console.CursorLeft < DrawStartPosition)
                System.Console.CursorLeft = DrawStartPosition;
        }

        /// <summary>
        /// 居中打印一行
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontColor"></param>
        protected virtual void CenterWriteLine(string text, ConsoleColor fontColor)
        {
            DividingLine dividingLine = new(Width, text);
            dividingLine.DrawForegroundColor = fontColor;
            dividingLine.DividingLineSymbol = ' ';
            dividingLine.DrawStartPosition = DrawStartPosition;
            dividingLine.Print();
            FillLine();
        }

        /// <summary>
        /// 填充空白符至渲染结束位置
        /// </summary>
        protected virtual void Fill()
        {
            if (System.Console.BackgroundColor == InitialBackgroundColor)
            {
                System.Console.CursorLeft = DrawEndPosition;
                return;
            }

            SetDrawStartPosition();

            if (System.Console.CursorLeft < DrawEndPosition)
                ConsoleDrawUtil.Fill(' ', DrawEndPosition - System.Console.CursorLeft);
        }

        /// <summary>
        /// 换行
        /// </summary>
        protected virtual void Line()
        {
            System.Console.WriteLine();
            System.Console.CursorLeft = DrawStartPosition;
        }

        /// <summary>
        /// 输出指定数量的换行
        /// </summary>
        /// <param name="count"></param>
        protected void Line(int count)
        {
            for (int i = 0; i < count; i++)
                FillLine();
        }

        /// <summary>
        /// 填充空白符至渲染结束位置并换行
        /// </summary>
        protected virtual void FillLine()
        {
            Fill();
            Line();
        }

        /// <summary>
        /// 输出
        /// </summary>
        public abstract void Print();

        /// <summary>
        /// 应用其他渲染器的参数
        /// </summary>
        /// <param name="consoleDraw"></param>
        /// <returns>返回自身</returns>
        public ConsoleDraw Apply(ConsoleDraw consoleDraw)
        {
            InitialBackgroundColor = consoleDraw.InitialBackgroundColor;
            InitialForegroundColor = consoleDraw.InitialForegroundColor;
            DrawBackgroundColor = consoleDraw.DrawBackgroundColor;
            DrawForegroundColor = consoleDraw.DrawForegroundColor;
            Width = consoleDraw.Width;
            IsCursor = consoleDraw.IsCursor;
            AutoSetBufferWidth = consoleDraw.AutoSetBufferWidth;
            AutoSetWindowWidth = consoleDraw.AutoSetWindowWidth;
            return this;
        }

        /// <summary>
        /// 获取光标位于居中文本的第一个字的位置
        /// </summary>
        /// <param name="textWidth"></param>
        /// <returns></returns>
        public static int GetCursorCenter(int textWidth)
        {
            if (System.Console.WindowWidth - textWidth < 0)
                return 0;
            else return (int)Math.Round((System.Console.WindowWidth - textWidth) / 2.0, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 获取光标位于居中文本的第一个字的位置
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetCursorCenter(string text) => GetCursorCenter(ConsoleUtil.GetWidth(text));

        /// <summary>
        /// 获取光标位于居中文本的第一个字的位置
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetCursorCenter(object text) => GetCursorCenter(ConsoleUtil.GetWidth(text.ToString()));

        /// <summary>
        /// 使光标位于居中文本的第一个字的位置
        /// </summary>
        /// <param name="textWidth">文本宽度</param>
        public static void CursorCenter(int textWidth) => System.Console.CursorLeft = GetCursorCenter(textWidth);

        /// <summary>
        /// 使光标位于居中文本的第一个字的位置
        /// </summary>
        /// <param name="text"></param>
        public static void CursorCenter(string text) => System.Console.CursorLeft = GetCursorCenter(ConsoleUtil.GetWidth(text));

        /// <summary>
        /// 使光标位于居中文本的第一个字的位置
        /// </summary>
        /// <param name="text"></param>
        public static void CursorCenter(object text) => System.Console.CursorLeft = GetCursorCenter(ConsoleUtil.GetWidth(text.ToString()));
    }
}
