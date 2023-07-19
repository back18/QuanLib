using System;
using System.Text;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 控制台绘制工具类
    /// </summary>
    public static class ConsoleDrawUtil
    {
        /// <summary>
        /// 输出指定数量的换行符
        /// </summary>
        /// <param name="count">换行符数量</param>
        public static void Line(int count) => Fill('\n', count);

        /// <summary>
        /// 输出指定数量的字符
        /// </summary>
        /// <param name="value">字符</param>
        /// <param name="count">符号数量</param>
        public static void Fill(char value, int count) => System.Console.Write(StringUtil.Copy(value, count));

        /// <summary>
        /// 输出指定数量的字符串
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="count">符号数量</param>
        public static void Fill(string value, int count) => System.Console.Write(StringUtil.Copy(value, count));

        /// <summary>
        /// 居中打印
        /// </summary>
        /// <param name="text"></param>
        public static void CenterWrite(string text)
        {
            ConsoleDraw.CursorCenter(text);
            System.Console.Write(text);
        }

        /// <summary>
        /// 居中打印
        /// </summary>
        /// <param name="text"></param>
        public static void CenterWrite(object text) => CenterWrite(text.ToString());

        /// <summary>
        /// 居中打印一行
        /// </summary>
        /// <param name="text"></param>
        public static void CenterWriteLine(string text)
        {
            ConsoleDraw.CursorCenter(text);
            System.Console.WriteLine(text);
        }

        /// <summary>
        /// 居中打印一行
        /// </summary>
        /// <param name="text"></param>
        public static void CenterWriteLine(object text) => CenterWriteLine(text.ToString());

        /// <summary>
        /// 输出控制台可用颜色
        /// </summary>
        public static void OutputConsoleColor()
        {
            ConsoleColor DefaultBackgroundColor = System.Console.BackgroundColor;
            ConsoleColor DefaultForegroundColor = System.Console.ForegroundColor;

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.BackgroundColor = (ConsoleColor)0;
            System.Console.Write(((ConsoleColor)0).ToString());
            while (System.Console.CursorLeft < System.Console.BufferWidth - 1)
                System.Console.Write(' ');
            System.Console.WriteLine();

            System.Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 1; i <= 15; i++)
            {
                System.Console.BackgroundColor = (ConsoleColor)i;
                System.Console.Write(((ConsoleColor)i).ToString());
                while (System.Console.CursorLeft < System.Console.BufferWidth - 1)
                    System.Console.Write(' ');
                System.Console.WriteLine();
            }
            System.Console.WriteLine();

            System.Console.BackgroundColor = DefaultBackgroundColor;
            System.Console.ForegroundColor = DefaultForegroundColor;
        }

        /// <summary>
        /// 输出Unicode字符表
        /// </summary>
        /// <param name="column">列数</param>
        public static void OutputUnicode(int column)
        {
            //int U = 0;
            //while (U <= 65535)
            //{
            //    for (int i = 0; i < column; i++)
            //    {
            //        if (U > 65535) break;
            //        System.Console.Write(NumberUtil.ToHexString(U, 4, CharFormat.Upper) + "\t");
            //        for (int j = 0; j < 16; j++)
            //        {
            //            System.Console.Write((char)U + "\t");
            //            U++;
            //        }
            //    }
            //    System.Console.WriteLine();
            //}
        }
    }
}
