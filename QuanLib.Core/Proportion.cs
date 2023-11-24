using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public readonly struct Proportion(int child, int parent)
    {
        public int Child { get; } = child;

        public int Parent { get; } = parent;

        public DivisionResult GetDivisionResult()
        {
            return DivisionResult.Compute(Child, Parent);
        }

        public double GetRatio()
        {
            return (double)Child / Parent;
        }

        public double GetRatio(int digits)
        {
            return Math.Round((double)Child / Parent, digits);
        }

        public double GetRatio(int digits, MidpointRounding mode)
        {
            return Math.Round((double)Child / Parent, digits, mode);
        }

        public double GetPercentage()
        {
            return (double)Child / Parent * 100;
        }

        public double GetPercentage(int digits)
        {
            return Math.Round((double)Child / Parent * 100, digits);
        }

        public double GetPercentage(int digits, MidpointRounding mode)
        {
            return Math.Round((double)Child / Parent * 100, digits, mode);
        }
    }
}
