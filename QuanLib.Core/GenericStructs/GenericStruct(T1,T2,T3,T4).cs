using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.GenericStructs
{
    public readonly struct GenericStruct<T1, T2, T3, T4>(T1 itemA, T2 itemB, T3 itemC, T4 itemD) : IEquatable<GenericStruct<T1, T2, T3, T4>>
    {
        public readonly T1 ItemA = itemA;
        public readonly T2 ItemB = itemB;
        public readonly T3 ItemC = itemC;
        public readonly T4 ItemD = itemD;

        public bool Equals(GenericStruct<T1, T2, T3, T4> other)
        {
            return Equals(ItemA, other.ItemA) && Equals(ItemB, other.ItemB) && Equals(ItemC, other.ItemC) && Equals(ItemD, other.ItemD);
        }

        public override bool Equals(object? obj)
        {
            return obj is GenericStruct<T1, T2, T3, T4> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemA, ItemB, ItemC, ItemD);
        }

        public static bool operator ==(GenericStruct<T1, T2, T3, T4> left, GenericStruct<T1, T2, T3, T4> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GenericStruct<T1, T2, T3, T4> left, GenericStruct<T1, T2, T3, T4> right)
        {
            return !left.Equals(right);
        }
    }
}
