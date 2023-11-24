using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public readonly struct DivisionResult(int result, int remainder)
    {
        public readonly int Result = result;

        public readonly int Remainder = remainder;

        public static DivisionResult Compute(int left, int right)
        {
            int result = left / right;
            int remainder = right % left;
            return new(result, remainder);
        }
    }
}
