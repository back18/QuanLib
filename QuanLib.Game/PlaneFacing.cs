using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Game
{
    public readonly struct PlaneFacing(Facing xFacing, Facing yFacing) : IEquatable<PlaneFacing>
    {
        public static readonly PlaneFacing XpXp = new(Facing.Xp, Facing.Xp);
        public static readonly PlaneFacing XpXm = new(Facing.Xp, Facing.Xm);
        public static readonly PlaneFacing XpYp = new(Facing.Xp, Facing.Yp);
        public static readonly PlaneFacing XpYm = new(Facing.Xp, Facing.Ym);
        public static readonly PlaneFacing XpZp = new(Facing.Xp, Facing.Zp);
        public static readonly PlaneFacing XpZm = new(Facing.Xp, Facing.Zm);

        public static readonly PlaneFacing XmXp = new(Facing.Xm, Facing.Xp);
        public static readonly PlaneFacing XmXm = new(Facing.Xm, Facing.Xm);
        public static readonly PlaneFacing XmYp = new(Facing.Xm, Facing.Yp);
        public static readonly PlaneFacing XmYm = new(Facing.Xm, Facing.Ym);
        public static readonly PlaneFacing XmZp = new(Facing.Xm, Facing.Zp);
        public static readonly PlaneFacing XmZm = new(Facing.Xm, Facing.Zm);

        public static readonly PlaneFacing YpXp = new(Facing.Yp, Facing.Xp);
        public static readonly PlaneFacing YpXm = new(Facing.Yp, Facing.Xm);
        public static readonly PlaneFacing YpYp = new(Facing.Yp, Facing.Yp);
        public static readonly PlaneFacing YpYm = new(Facing.Yp, Facing.Ym);
        public static readonly PlaneFacing YpZp = new(Facing.Yp, Facing.Zp);
        public static readonly PlaneFacing YpZm = new(Facing.Yp, Facing.Zm);

        public static readonly PlaneFacing YmXp = new(Facing.Ym, Facing.Xp);
        public static readonly PlaneFacing YmXm = new(Facing.Ym, Facing.Xm);
        public static readonly PlaneFacing YmYp = new(Facing.Ym, Facing.Yp);
        public static readonly PlaneFacing YmYm = new(Facing.Ym, Facing.Ym);
        public static readonly PlaneFacing YmZp = new(Facing.Ym, Facing.Zp);
        public static readonly PlaneFacing YmZm = new(Facing.Ym, Facing.Zm);

        public static readonly PlaneFacing ZpXp = new(Facing.Zp, Facing.Xp);
        public static readonly PlaneFacing ZpXm = new(Facing.Zp, Facing.Xm);
        public static readonly PlaneFacing ZpYp = new(Facing.Zp, Facing.Yp);
        public static readonly PlaneFacing ZpYm = new(Facing.Zp, Facing.Ym);
        public static readonly PlaneFacing ZpZp = new(Facing.Zp, Facing.Zp);
        public static readonly PlaneFacing ZpZm = new(Facing.Zp, Facing.Zm);

        public static readonly PlaneFacing ZmXp = new(Facing.Zm, Facing.Xp);
        public static readonly PlaneFacing ZmXm = new(Facing.Zm, Facing.Xm);
        public static readonly PlaneFacing ZmYp = new(Facing.Zm, Facing.Yp);
        public static readonly PlaneFacing ZmYm = new(Facing.Zm, Facing.Ym);
        public static readonly PlaneFacing ZmZp = new(Facing.Zm, Facing.Zp);
        public static readonly PlaneFacing ZmZm = new(Facing.Zm, Facing.Zm);

        private static readonly Dictionary<PlaneFacing, Facing> _normalMap;

        static PlaneFacing()
        {
            _normalMap = new Dictionary<PlaneFacing, Facing>
            {
                { XpYm, Facing.Zp },
                { YmXm, Facing.Zp },
                { XmYp, Facing.Zp },
                { YpXp, Facing.Zp },
                { XmYm, Facing.Zm },
                { YmXp, Facing.Zm },
                { XpYp, Facing.Zm },
                { YpXm, Facing.Zm },
                { ZmYm, Facing.Xp },
                { YmZp, Facing.Xp },
                { ZpYp, Facing.Xp },
                { YpZm, Facing.Xp },
                { ZpYm, Facing.Xm },
                { YmZm, Facing.Xm },
                { ZmYp, Facing.Xm },
                { YpZp, Facing.Xm },
                { XpZp, Facing.Yp },
                { ZpXm, Facing.Yp },
                { XmZm, Facing.Yp },
                { ZmXp, Facing.Yp },
                { XpZm, Facing.Ym },
                { ZmXm, Facing.Ym },
                { XmZp, Facing.Ym },
                { ZpXp, Facing.Ym }
            };
        }

        public Facing XFacing { get; } = xFacing;

        public Facing YFacing { get; } = yFacing;

        public Facing NormalFacing
        {
            get
            {
                if (_normalMap.TryGetValue(this, out var normal))
                    return normal;
                throw new InvalidOperationException();
            }
        }

        public PlaneFacing UpRotate()
        {
            Facing yFacing = YFacing.LeftRotate(XFacing);
            return new(XFacing, yFacing);
        }

        public PlaneFacing DownRotate()
        {
            Facing yFacing = YFacing.RightRotate(XFacing);
            return new(XFacing, yFacing);
        }

        public PlaneFacing LeftRotate()
        {
            Facing xFacing = XFacing.RightRotate(YFacing);
            return new(xFacing, YFacing);
        }

        public PlaneFacing RightRotate()
        {
            Facing xFacing = XFacing.LeftRotate(YFacing);
            return new(xFacing, YFacing);
        }

        public PlaneFacing ClockwiseRotate()
        {
            Facing xFacing = XFacing.LeftRotate(NormalFacing);
            Facing yFacing = YFacing.LeftRotate(NormalFacing);
            return new(xFacing, yFacing);
        }

        public PlaneFacing CounterclockwiseRotate()
        {
            Facing xFacing = XFacing.RightRotate(NormalFacing);
            Facing yFacing = YFacing.RightRotate(NormalFacing);
            return new(xFacing, yFacing);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(XFacing, YFacing);
        }

        public override bool Equals(object? obj)
        {
            return obj is PlaneFacing other && Equals(other);
        }

        public bool Equals(PlaneFacing other)
        {
            return this == other;
        }

        public static bool operator ==(PlaneFacing left, PlaneFacing right)
        {
            return left.XFacing == right.XFacing && left.YFacing == right.YFacing;
        }

        public static bool operator !=(PlaneFacing left, PlaneFacing right)
        {
            return !(left == right);
        }
    }
}
