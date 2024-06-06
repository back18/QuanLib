using QuanLib.Core;
using SixLabors.ImageSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.BDF
{
    public class FontData
    {
        public FontData(char @char, int width, int height, int xOffset, int yOffset, BitArray bits)
        {
            ArgumentNullException.ThrowIfNull(bits, nameof(bits));
            if (bits.Count != width * height)
                throw new ArgumentException("BitArray的位数应该为" + width * height);

            Char = @char;
            Width = width;
            Height = height;
            XOffset = xOffset;
            YOffset = yOffset;
            _bits = bits;
        }

        private readonly BitArray _bits;

        public char Char { get; }

        public int Width { get; }

        public int Height { get; }

        public int XOffset { get; }

        public int YOffset { get; }

        public bool[,] GetBitArray(bool isNegative = false)
        {
            bool[,] result = new bool[Width, Height];
            Func<int, int, bool> get = isNegative ? ((x, y) => !_bits[y * Width + x]) : ((x, y) => _bits[y * Width + x]);

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    result[x, y] = get(x, y);

            return result;
        }

        public bool[,] GetBitArray(int pixelSize, bool isNegative = false)
        {
            ThrowHelper.ArgumentOutOfMin(1, pixelSize, nameof(pixelSize));

            if (pixelSize == 1)
                return GetBitArray(isNegative);

            Size size = new(Width * pixelSize, Height * pixelSize);
            bool[,] result = new bool[size.Width, size.Height];
            Func<int, int, bool> get = isNegative ? ((x, y) => !_bits[y * Width + x]) : ((x, y) => _bits[y * Width + x]);

            for (int y1 = 0, y2 = 0; y1 < Height; y1++, y2 += pixelSize)
            {
                for (int x1 = 0, x2 = 0; x1 < Width; x1++, x2 += pixelSize)
                {
                    if (get(x1, y1))
                    {
                        int yend = y2 + pixelSize;
                        int xend = x2 + pixelSize;
                        for (int y3 = y2; y3 < yend; y3++)
                            for (int x3 = x2; x3 < xend; x3++)
                                result[x3, y3] = true;
                    }
                }
            }

            return result;
        }
    }
}
