using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.BDF
{
    public class FontData
    {
        public FontData(char @char, int width, int height, int xOffset, int yOffset, bool[][] data)
        {
            Char = @char;
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;
            _data = data;
        }

        private readonly bool[][] _data;

        public char Char { get; }

        public int Width { get; }

        public int Height { get; }

        public int XOffset { get; }

        public int YOffset { get; }

        public bool[,] GetBinary()
        {
            bool[,] result = new bool[Width, Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    result[x, y] = _data[y][x];
            return result;
        }
    }
}
