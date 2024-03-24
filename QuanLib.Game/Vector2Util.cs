using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Game
{
    public static class Vector2Util
    {
        public static Vector2<int> ToIntVector2(this Vector2<float> source)
        {
            int x = (int)Math.Round(source.X, MidpointRounding.ToNegativeInfinity);
            int y = (int)Math.Round(source.Y, MidpointRounding.ToNegativeInfinity);
            return new(x, y);
        }

        public static Vector2<int> ToIntVector2(this Vector2<double> source)
        {
            int x = (int)Math.Round(source.X, MidpointRounding.ToNegativeInfinity);
            int y = (int)Math.Round(source.Y, MidpointRounding.ToNegativeInfinity);
            return new(x, y);
        }

        public static Vector2<int> ToIntVector2(this Vector2<decimal> source)
        {
            int x = (int)Math.Round(source.X, MidpointRounding.ToNegativeInfinity);
            int y = (int)Math.Round(source.Y, MidpointRounding.ToNegativeInfinity);
            return new(x, y);
        }
    }
}
