using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.GenericVector
{
    public struct Vector3<T>(T x, T y, T z) : IVector3<T>, IEquatable<Vector3<T>> where T : INumber<T>, IConvertible
    {
        public T this[int index]
        {
            readonly get
            {
                return index switch
                {
                    0 => X,
                    1 => Y,
                    2 => Z,
                    _ => throw new IndexOutOfRangeException(),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public T X { get; set; } = x;

        public T Y { get; set; } = y;

        public T Z { get; set; } = z;

        public static T Dot(Vector3<T> vector1, Vector3<T> vector2)
        {
            return (vector1.X * vector2.X)
                 + (vector1.Y * vector2.Y)
                 + (vector1.Z * vector2.Z);
        }

        public static Vector3<T> Cross(Vector3<T> vector1, Vector3<T> vector2)
        {
            return new(
                (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y),
                (vector1.Z * vector2.X) - (vector1.X * vector2.Z),
                (vector1.X * vector2.Y) - (vector1.Y * vector2.X));
        }

        public static T DistanceSquared(Vector3<T> value1, Vector3<T> value2)
        {
            Vector3<T> difference = value1 - value2;
            return Dot(difference, difference);
        }

        public static double Distance(Vector3<T> value1, Vector3<T> value2)
        {
            T distanceSquared = DistanceSquared(value1, value2);
            return Math.Sqrt(distanceSquared.ToDouble(null));
        }

        public readonly T LengthSquared()
        {
            return Dot(this, this);
        }

        public readonly double Length()
        {
            T lengthSquared = LengthSquared();
            return Math.Sqrt(lengthSquared.ToDouble(null));
        }

        public readonly bool Equals(Vector3<T> other)
        {
            return this == other;
        }

        public override readonly bool Equals(object? obj)
        {
            return (obj is Vector3 other) && Equals(other);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static Vector3<T> operator -(Vector3<T> value)
        {
            return new(
                -value.X,
                -value.Y,
                -value.Z);
        }

        public static Vector3<T> operator +(Vector3<T> left, Vector3<T> right)
        {
            return new(
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z);
        }

        public static Vector3<T> operator -(Vector3<T> left, Vector3<T> right)
        {
            return new(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z);
        }

        public static Vector3<T> operator *(Vector3<T> left, Vector3<T> right)
        {
            return new(
                left.X * right.X,
                left.Y * right.Y,
                left.Z * right.Z);
        }

        public static Vector3<T> operator /(Vector3<T> left, Vector3<T> right)
        {
            return new(
                left.X / right.X,
                left.Y / right.Y,
                left.Z / right.Z);
        }

        public static Vector3<T> operator +(Vector3<T> left, T right)
        {
            return new(
                left.X + right,
                left.Y + right,
                left.Z + right);
        }

        public static Vector3<T> operator -(Vector3<T> left, T right)
        {
            return new(
                left.X - right,
                left.Y - right,
                left.Z - right);
        }

        public static Vector3<T> operator *(Vector3<T> left, T right)
        {
            return new(
                left.X * right,
                left.Y * right,
                left.Z * right);
        }

        public static Vector3<T> operator /(Vector3<T> left, T right)
        {
            return new(
                left.X / right,
                left.Y / right,
                left.Z / right);
        }

        public static bool operator ==(Vector3<T> left, Vector3<T> right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z);
        }

        public static bool operator !=(Vector3<T> left, Vector3<T> right)
        {
            return !(left == right);
        }
    }
}
