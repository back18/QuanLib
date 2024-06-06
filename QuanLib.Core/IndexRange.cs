using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public readonly struct IndexRange(int start, int end)
    {
        public readonly int Start = start;

        public readonly int End = end;

        public int Count => End - Start + 1;

        public bool IsWithinRange(int index)
        {
            return index >= Start && index <= End;
        }

        public override string ToString()
        {
            return $"{Start}-{End}";
        }
    }
}
