using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.GenericVector
{
    public struct Vector2<T>(T x, T y) : IVector2<T>, IEquatable<Vector2<T>> where T : INumber<T>, IConvertible
    {
        public T this[int index]
        {
            readonly get
            {
                return index switch
                {
                    0 => X,
                    1 => Y,
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
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public T X { get; set; } = x;

        public T Y { get; set; } = y;

        public static T Dot(Vector2<T> vector1, Vector2<T> vector2)
        {
            return (vector1.X * vector2.X)
                 + (vector1.Y * vector2.Y);
        }

        public static T DistanceSquared(Vector2<T> value1, Vector2<T> value2)
        {
            Vector2<T> difference = value1 - value2;
            return Dot(difference, difference);
        }

        public static double Distance(Vector2<T> value1, Vector2<T> value2)
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

        public readonly bool Equals(Vector2<T> other)
        {
            return this == other;
        }

        public override readonly bool Equals(object? obj)
        {
            return (obj is Vector2 other) && Equals(other);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Vector2<T> operator -(Vector2<T> value)
        {
            return new(
                -value.X,
                -value.Y);
        }

        public static Vector2<T> operator +(Vector2<T> left, Vector2<T> right)
        {
            return new(
                left.X + right.X,
                left.Y + right.Y);
        }

        public static Vector2<T> operator -(Vector2<T> left, Vector2<T> right)
        {
            return new(
                left.X - right.X,
                left.Y - right.Y);
        }

        public static Vector2<T> operator *(Vector2<T> left, Vector2<T> right)
        {
            return new(
                left.X * right.X,
                left.Y * right.Y);
        }

        public static Vector2<T> operator /(Vector2<T> left, Vector2<T> right)
        {
            return new(
                left.X / right.X,
                left.Y / right.Y);
        }

        public static Vector2<T> operator +(Vector2<T> left, T right)
        {
            return new(
                left.X + right,
                left.Y + right);
        }

        public static Vector2<T> operator -(Vector2<T> left, T right)
        {
            return new(
                left.X - right,
                left.Y - right);
        }

        public static Vector2<T> operator *(Vector2<T> left, T right)
        {
            return new(
                left.X * right,
                left.Y * right);
        }

        public static Vector2<T> operator /(Vector2<T> left, T right)
        {
            return new(
                left.X / right,
                left.Y / right);
        }

        public static bool operator ==(Vector2<T> left, Vector2<T> right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y);
        }

        public static bool operator !=(Vector2<T> left, Vector2<T> right)
        {
            return !(left == right);
        }
    }
}
