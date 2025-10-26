using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Game
{
    public struct CubeRange(Vector3<int> startPosition, Vector3<int> endPosition)
    {
        public CubeRange(int x1, int y1, int z1, int x2, int y2, int z2) :
            this(new Vector3<int>(x1, y1, z1), new Vector3<int>(x2, y2, z2)) { }

        public Vector3<int> StartPosition { get; set; } = startPosition;

        public Vector3<int> EndPosition { get; set; } = endPosition;

        public readonly Vector3<int> Range => new(
            Math.Abs(EndPosition.X - StartPosition.X) + 1,
            Math.Abs(EndPosition.Y - StartPosition.Y) + 1,
            Math.Abs(EndPosition.Z - StartPosition.Z) + 1);

        public readonly int Volume
        {
            get
            {
                Vector3<int> range = Range;
                return range.X * range.Y * range.Z;
            }
        }

        public readonly CubeRange Normalize()
        {
            Vector3<int> startPosition = new(Math.Min(StartPosition.X, EndPosition.X), Math.Min(StartPosition.Y, EndPosition.Y), Math.Min(StartPosition.Z, EndPosition.Z));
            Vector3<int> endPosition = new(Math.Max(StartPosition.X, EndPosition.X), Math.Max(StartPosition.Y, EndPosition.Y), Math.Max(StartPosition.Z, EndPosition.Z));
            return new CubeRange(startPosition, endPosition);
        }

        public readonly bool IsOverlaps(CubeRange other, bool normalize = true)
        {
            CubeRange source;
            if (normalize)
            {
                source = Normalize();
                other = other.Normalize();
            }
            else
            {
                source = this;
            }

            return !(other.StartPosition.X > source.EndPosition.X ||
                    other.EndPosition.X < source.StartPosition.X ||
                    other.StartPosition.Y > source.EndPosition.Y ||
                    other.EndPosition.Y < source.StartPosition.Y ||
                    other.StartPosition.Z > source.EndPosition.Z ||
                    other.EndPosition.Z < source.StartPosition.Z);
        }

        public readonly bool IsContains(CubeRange other, bool normalize = true)
        {
            CubeRange source;
            if (normalize)
            {
                source = Normalize();
                other = other.Normalize();
            }
            else
            {
                source = this;
            }

            return other.StartPosition.X >= source.StartPosition.X &&
                   other.EndPosition.X <= source.EndPosition.X &&
                   other.StartPosition.Y >= source.StartPosition.Y &&
                   other.EndPosition.Y <= source.EndPosition.Y &&
                   other.StartPosition.Z >= source.StartPosition.Z &&
                   other.EndPosition.Z <= source.EndPosition.Z;
        }

        public readonly bool IsEquals(CubeRange other, bool normalize = true)
        {
            CubeRange source;
            if (normalize)
            {
                source = Normalize();
                other = other.Normalize();
            }
            else
            {
                source = this;
            }

            return other.StartPosition.X == source.StartPosition.X &&
                   other.EndPosition.X == source.EndPosition.X &&
                   other.StartPosition.Y == source.StartPosition.Y &&
                   other.EndPosition.Y == source.EndPosition.Y &&
                   other.StartPosition.Z == source.StartPosition.Z &&
                   other.EndPosition.Z == source.EndPosition.Z;
        }
    }
}
