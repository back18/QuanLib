using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    public readonly struct TextColor
    {
        public TextColor()
        {
            BackgroundColor = DefaultBackgroundColor;
            ForegroundColor = DefaultForegroundColor;
        }

        public TextColor(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
        }

        public const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

        public const ConsoleColor DefaultForegroundColor = ConsoleColor.White;

        /// <summary>
        /// 背景色
        /// </summary>
        public ConsoleColor BackgroundColor { get; }

        /// <summary>
        /// 前景色
        /// </summary>
        public ConsoleColor ForegroundColor { get; }

        public static TextColor Now
            => new(Console.BackgroundColor, Console.ForegroundColor);

        public void SetToConsole()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
        }

        public TextColor SetBackgroundColor(ConsoleColor backgroundColor)
        {
            return new(backgroundColor, ForegroundColor);
        }

        public TextColor SetForegroundColor(ConsoleColor foregroundColor)
        {
            return new(BackgroundColor, foregroundColor);
        }

        public static TextColor FormatBackgroundColor(ConsoleColor backgroundColor)
        {
            return new(backgroundColor, Console.ForegroundColor);
        }

        public static TextColor FormatForegroundColor(ConsoleColor foregroundColor)
        {
            return new(Console.BackgroundColor, foregroundColor);
        }
    }
}
