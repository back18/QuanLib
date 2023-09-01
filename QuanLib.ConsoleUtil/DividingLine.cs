using QuanLib.Core;
using System;
using System.Text;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 分界线
    /// </summary>
    public class DividingLine : ConsoleDraw
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">分界线宽度</param>
        /// <param name="title">标题</param>
        public DividingLine(int width, string? title = null) : base(width)
        {
            if (width <= 0)
                throw new ArgumentException("分界线宽度不可小于0");

            Title = title;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 分界线符号
        /// </summary>
        public char DividingLineSymbol
        {
            get => _DividingLineSymbol;
            set
            {
                if (ConsoleUtil.GetWidth(value) < 1)
                    throw new ArgumentException("分界线符号宽度小于1");
                else _DividingLineSymbol = value;
            }
        }
        private char _DividingLineSymbol = '=';

        /// <summary>
        /// 分界线符号宽度
        /// </summary>
        public int DividingLineSymbolWidth => ConsoleUtil.GetWidth(DividingLineSymbol);

        /// <summary>
        /// 打印分界线
        /// </summary>
        public override void Print()
        {
            ApplyDrawColors();

            if (string.IsNullOrEmpty(Title))
                PrintDividingLine();
            else PrintTitleDividingLine();

            ApplyInitialColors();
        }

        private void PrintDividingLine()
        {
            //分界线符号数量
            int SymbolCount = (int)Math.Round(Width / (double)ConsoleUtil.GetWidth(DividingLineSymbol), MidpointRounding.ToNegativeInfinity);

            StringBuilder dividingLine = new();
            for (int i = 0; i < SymbolCount; i++)
                dividingLine.Append(DividingLineSymbol);
            while (ConsoleUtil.GetWidth(dividingLine.ToString()) < Width)
                dividingLine.Append(' ');

            System.Console.CursorLeft = DrawStartPosition;
            System.Console.Write(dividingLine);
        }

        private void PrintTitleDividingLine()
        {
            //分界线符号数量
            int SymbolCount;

            //如果标题宽度大于渲染区域宽度，那么直接输出对应宽度的标题，否则计算分界线符号数量
            if (ConsoleUtil.GetWidth(Title) > Width)
            {
                System.Console.WriteLine(ConsoleUtil.GetToWidth(Title, Width));
                return;
            }
            else SymbolCount = (int)Math.Round((Width - ConsoleUtil.GetWidth(Title)) / 2.0 / DividingLineSymbolWidth, MidpointRounding.ToNegativeInfinity);

            //子分界线
            string subDividingLine = new(DividingLineSymbol, SymbolCount);
            //完整分界线
            StringBuilder dividingLine = new(subDividingLine + Title + subDividingLine);

            //完整分界线宽度
            int dividingLineWidth = ConsoleUtil.GetWidth(dividingLine.ToString());

            //完整分界线宽度修正
            if (dividingLineWidth == Width - 1)
            {
                if (DividingLineSymbolWidth == 1)
                    dividingLine.Append(DividingLineSymbol);
                else dividingLine.Append(' ');
            }
            else if (dividingLineWidth <= Width - 2)
            {
                if (ConsoleUtil.GetWidth(DividingLineSymbol) == 1)
                {
                    dividingLine.Append(DividingLineSymbol);
                    dividingLine.Append(DividingLineSymbol);
                }
                else dividingLine.Append(DividingLineSymbol);
            }

            System.Console.CursorLeft = DrawStartPosition;
            System.Console.Write(dividingLine);
        }
    }
}
