using QuanLib.Core;
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

        public bool[,] GetBitArray(bool isNegative = false)
        {
            Func<int, int, bool> func = isNegative ? ((x, y) => !_data[y][x]) : ((x, y) => _data[y][x]);
            bool[,] result = new bool[Width, Height];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    result[x, y] = func(x, y);
            return result;
        }

        public bool[,] GetBitArray(int pixelSize, bool isNegative = false)
        {
            ThrowHelper.ArgumentOutOfMin(1, pixelSize, nameof(pixelSize));

            if (pixelSize == 1)
                return GetBitArray(isNegative);

            Func<int, int, bool> func = isNegative ? ((x, y) => !_data[y][x]) : ((x, y) => _data[y][x]);
            Size size = new(Width * pixelSize, Height * pixelSize);
            bool[,] result = new bool[size.Width, size.Height];
            for (int y1 = 0, y2 = 0; y1 < Height; y1++, y2 += pixelSize)
                for (int x1 = 0, x2 = 0; x1 < Width; x1++, x2 += pixelSize)
                {
                    if (func(x1, y1))
                    {
                        int yend = y2 + pixelSize;
                        int xend = x2 + pixelSize;
                        for (int y3 = y2; y3 < yend; y3++)
                            for (int x3 = x2; x3 < xend; x3++)
                                result[x3, y3] = true;
                    }
                }

            return result;
        }
    }
}
