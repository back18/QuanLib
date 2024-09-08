using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.GenericStructs
{
    public readonly struct GenericStruct<T1, T2, T3>(T1 itemA, T2 itemB, T3 itemC) : IEquatable<GenericStruct<T1, T2, T3>>
    {
        public readonly T1 ItemA = itemA;
        public readonly T2 ItemB = itemB;
        public readonly T3 ItemC = itemC;

        public bool Equals(GenericStruct<T1, T2, T3> other)
        {
            return Equals(ItemA, other.ItemA) && Equals(ItemB, other.ItemB) && Equals(ItemC, other.ItemC);
        }

        public override bool Equals(object? obj)
        {
            return obj is GenericStruct<T1, T2, T3> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemA, ItemB, ItemC);
        }

        public static bool operator ==(GenericStruct<T1, T2, T3> left, GenericStruct<T1, T2, T3> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GenericStruct<T1, T2, T3> left, GenericStruct<T1, T2, T3> right)
        {
            return !left.Equals(right);
        }
    }
}
