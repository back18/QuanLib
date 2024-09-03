using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public readonly struct TypeConvertInfo : IEquatable<TypeConvertInfo>
    {
        public readonly Type SourceType;

        public readonly Type TargetType;

        public TypeConvertInfo()
        {
            SourceType = typeof(object);
            TargetType = typeof(object);
        }

        public TypeConvertInfo(Type sourceType, Type targetType)
        {
            ArgumentNullException.ThrowIfNull(sourceType, nameof(sourceType));
            ArgumentNullException.ThrowIfNull(targetType, nameof(targetType));

            SourceType = sourceType;
            TargetType = targetType;
        }

        public bool Equals(TypeConvertInfo other)
        {
            return SourceType.Equals(other.SourceType) && TargetType.Equals(other.TargetType);
        }

        public override bool Equals(object? obj)
        {
            return obj is TypeConvertInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceType, TargetType);
        }

        public static bool operator ==(TypeConvertInfo left, TypeConvertInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypeConvertInfo left, TypeConvertInfo right)
        {
            return !left.Equals(right);
        }
    }
}
