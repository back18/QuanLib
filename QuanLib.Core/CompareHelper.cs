using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class CompareHelper
    {
        public static bool CompareEqual(object? value, object? other)
        {
            return value?.GetType() == other?.GetType() && Equals(value, other);
        }

        public static bool CompareNotEqual(object? value, object? other)
        {
            return value?.GetType() == other?.GetType() && !Equals(value, other);
        }

        public static bool CompareLessThan(object? value, object? other)
        {
            if (value is IComparable ivalue && other is IComparable iother && ivalue.GetType() == iother.GetType())
            {
                return ivalue.CompareTo(iother) < 0;
            }
            else
            {
                return false;
            }
        }

        public static bool CompareLessThanOrEquals(object? value, object? other)
        {
            if (value is IComparable ivalue && other is IComparable iother && ivalue.GetType() == iother.GetType())
            {
                return ivalue.CompareTo(iother) <= 0;
            }
            else
            {
                return false;
            }
        }

        public static bool CompareGreaterThan(object? value, object? other)
        {
            if (value is IComparable ivalue && other is IComparable iother && ivalue.GetType() == iother.GetType())
            {
                return ivalue.CompareTo(iother) > 0;
            }
            else
            {
                return false;
            }
        }

        public static bool CompareGreaterThanOrEquals(object? value, object? other)
        {
            if (value is IComparable ivalue && other is IComparable iother && ivalue.GetType() == iother.GetType())
            {
                return ivalue.CompareTo(iother) >= 0;
            }
            else
            {
                return false;
            }
        }
        public static bool Compare(object? value, object? other, CompareOperator compareOperator)
        {
            return compareOperator switch
            {
                CompareOperator.Equal => CompareEqual(value, other),
                CompareOperator.NotEqual => CompareNotEqual(value, other),
                CompareOperator.LessThan => CompareLessThan(value, other),
                CompareOperator.LessThanOrEquals => CompareLessThanOrEquals(value, other),
                CompareOperator.GreaterThan => CompareGreaterThan(value, other),
                CompareOperator.GreaterThanOrEquals => CompareGreaterThanOrEquals(value, other),
                _ => throw new InvalidOperationException(),
            };
        }


        public static string ToSymbol(this CompareOperator compareOperator)
        {
            return compareOperator switch
            {
                CompareOperator.Equal => "==",
                CompareOperator.NotEqual => "!=",
                CompareOperator.LessThan => "<",
                CompareOperator.LessThanOrEquals => "<=",
                CompareOperator.GreaterThan => ">",
                CompareOperator.GreaterThanOrEquals => ">=",
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
