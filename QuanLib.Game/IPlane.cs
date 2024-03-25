using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Game
{
    public interface IPlane
    {
        public int Width { get; }

        public int Height { get; }

        public Facing NormalFacing { get; }
    }
}
