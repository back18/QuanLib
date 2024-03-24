using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Game
{
    public static class Vector3Util
    {
        public static Vector3<int> ToIntVector3(this Vector3<float> source)
        {
            int x = (int)Math.Round(source.X, MidpointRounding.ToNegativeInfinity);
            int y = (int)Math.Round(source.Y, MidpointRounding.ToNegativeInfinity);
            int z = (int)Math.Round(source.Z, MidpointRounding.ToNegativeInfinity);
            return new(x, y, z);
        }

        public static Vector3<int> ToIntVector3(this Vector3<double> source)
        {
            int x = (int)Math.Round(source.X, MidpointRounding.ToNegativeInfinity);
            int y = (int)Math.Round(source.Y, MidpointRounding.ToNegativeInfinity);
            int z = (int)Math.Round(source.Z, MidpointRounding.ToNegativeInfinity);
            return new(x, y, z);
        }

        public static Vector3<int> ToIntVector3(this Vector3<decimal> source)
        {
            int x = (int)Math.Round(source.X, MidpointRounding.ToNegativeInfinity);
            int y = (int)Math.Round(source.Y, MidpointRounding.ToNegativeInfinity);
            int z = (int)Math.Round(source.Z, MidpointRounding.ToNegativeInfinity);
            return new(x, y, z);
        }

        public static Vector3<T> Offset<T>(this Vector3<T> position, int facing, T offset) where T : INumber<T>, IConvertible
        {
            switch (facing)
            {
                //case Facing.Xp:
                case 1:
                    position.X += offset;
                    break;
                //case Facing.Xm:
                case -1:
                    position.X -= offset;
                    break;
                //case Facing.Yp:
                case 2:
                    position.Y += offset;
                    break;
                //case Facing.Ym:
                case -2:
                    position.Y -= offset;
                    break;
                //case Facing.Zp:
                case 3:
                    position.Z += offset;
                    break;
                //case Facing.Zm:
                case -3:
                    position.Z -= offset;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return position;
        }
    }
}
