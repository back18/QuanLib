using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public readonly struct IndexRange
    {
        public IndexRange(int start, int end)
        {
            ThrowHelper.ArgumentOutOfMin(0, start, nameof(start));
            ThrowHelper.ArgumentOutOfMin(start, end, nameof(end));

            Start = start;
            End = end;
        }

        public int Start { get; }

        public int End { get; }

        public bool IsWithinRange(int index)
        {
            return index >= Start && index <= End;
        }
    }
}
