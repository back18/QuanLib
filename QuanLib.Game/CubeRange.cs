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
            Math.Abs(StartPosition.Z - EndPosition.Z) + 1);

        public readonly int Volume
        {
            get
            {
                Vector3<int> range = Range;
                return range.X * range.Y * range.Z;
            }
        }
    }
}
