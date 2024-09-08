using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.GenericStructs
{
    public readonly struct GenericStruct<T1, T2>(T1 itemA, T2 itemB) : IEquatable<GenericStruct<T1, T2>>
    {
        public readonly T1 ItemA = itemA;
        public readonly T2 ItemB = itemB;

        public bool Equals(GenericStruct<T1, T2> other)
        {
            return Equals(ItemA, other.ItemA) && Equals(ItemB, other.ItemB);
        }

        public override bool Equals(object? obj)
        {
            return obj is GenericStruct<T1, T2> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemA, ItemB);
        }

        public static bool operator ==(GenericStruct<T1, T2> left, GenericStruct<T1, T2> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GenericStruct<T1, T2> left, GenericStruct<T1, T2> right)
        {
            return !left.Equals(right);
        }
    }
}
