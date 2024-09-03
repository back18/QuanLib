using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static void Initialize<T>(this T[] array) where T : new()
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            for (int i = 0; i < array.Length; i++)
                array[i] = new();
        }

        public static void Initialize<T>(this T[,] array) where T : new()
        {
            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    array[i, j] = new();
        }

        public static void Initialize<T>(this T[,,] array) where T : new()
        {
            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            int dimension2 = array.GetLength(2);
            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    for (int k = 0; k < dimension2; k++)
                        array[i, j, k] = new();
        }

        public static void Fill<T>(this T[] array, T value)
        {
            Array.Fill(array, value);
        }

        public static void Fill<T>(this T[] array, T value, int startIndex, int count)
        {
            Array.Fill(array, value, startIndex, count);
        }

        public static void Fill<T>(this T[,] array, T value)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    array[i, j] = value;
        }

        public static void Fill<T>(this T[,,] array, T value)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            int dimension2 = array.GetLength(2);
            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    for (int k = 0; k < dimension2; k++)
                        array[i, j, k] = value;
        }

        public static T[] Clone<T>(this T[] array)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            T[] result = new T[array.Length];
            array.CopyTo(result, 0);
            return result;
        }

        public static T[,] Clone<T>(this T[,] array)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            T[,] result = new T[dimension0, dimension1];

            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    result[i, j] = array[i, j];

            return result;
        }

        public static T[,,] Clone<T>(this T[,,] array)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            int dimension2 = array.GetLength(2);
            T[,,] result = new T[dimension0, dimension1, dimension2];

            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    for (int k = 0; k < dimension2; k++)
                        result[i, j, k] = array[i, j, k];

            return result;
        }

        public static T[] LeftExpansion<T>(this T[] array, int count)
        {
            ThrowHelper.ArgumentOutOfMin(0, count, nameof(count));

            if (count == 0)
                return array;

            T[] result = new T[array.Length + count];
            array.CopyTo(result, count);
            return result;
        }

        public static T[] RightExpansion<T>(this T[] array, int count)
        {
            ThrowHelper.ArgumentOutOfMin(0, count, nameof(count));

            if (count == 0)
                return array;

            T[] result = new T[array.Length + count];
            array.CopyTo(result, 0);
            return result;
        }

        public static T[] PadLeft<T>(this T[] array, int totalLength)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            int count = totalLength - array.Length;
            if (count <= 0)
                return array;

            T[] result = LeftExpansion(array, count);
            return result;
        }

        public static T[] PadLeft<T>(this T[] array, int totalLength, T paddingValue)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            int count = totalLength - array.Length;
            if (count <= 0)
                return array;

            T[] result = LeftExpansion(array, count);
            Array.Fill(result, paddingValue, 0, count);
            return result;
        }

        public static T[] PadRight<T>(this T[] array, int totalLength)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            int count = totalLength - array.Length;
            if (count <= 0)
                return array;

            T[] result = RightExpansion(array, count);
            return result;
        }

        public static T[] PadRight<T>(this T[] array, int totalLength, T paddingValue)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            int count = totalLength - array.Length;
            if (count <= 0)
                return array;

            T[] result = RightExpansion(array, count);
            Array.Fill(result, paddingValue, array.Length, count);
            return result;
        }

        public static T[] LeftAddend<T>(this T[] array, T value)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            T[] result = LeftExpansion(array, 1);
            result[0] = value;
            return result;
        }

        public static T[] LeftAddend<T>(this T[] array, T[] values)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            T[] result = LeftExpansion(array, values.Length);
            values.CopyTo(result, 0);
            return result;
        }

        public static T[] RightAddend<T>(this T[] array, T value)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            T[] result = RightExpansion(array, 1);
            result[^1] = value;
            return result;
        }

        public static T[] RightAddend<T>(this T[] array, T[] values)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            T[] result = RightExpansion(array, values.Length);
            values.CopyTo(result, array.Length);
            return result;
        }

        public static T[] Insert<T>(this T[] array, int index, T value)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ThrowHelper.ArgumentOutOfRange(0, array.Length, index, nameof(index));

            if (index == 0)
                return LeftAddend(array, value);
            if (index == array.Length)
                return RightAddend(array, value);

            T[] result = new T[array.Length + 1];
            Array.Copy(array, 0, result, 0, index);
            Array.Copy(array, index, result, index + 1, array.Length - index);
            result[index] = value;
            return result;
        }

        public static T[] RemoveAt<T>(this T[] array, int index)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ThrowHelper.ArgumentOutOfRange(0, array.Length - 1, index, nameof(index));

            T[] result = new T[array.Length - 1];
            Array.Copy(array, 0, result, 0, index);
            Array.Copy(array, index + 1, result, index, array.Length - index - 1);
            return result;
        }

        public static bool OrderEquals<T>(this T[] array, T[] other)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentNullException.ThrowIfNull(other, nameof(other));

            if (array.Length != other.Length)
                return false;

            for (int i = 0; i < array.Length; i++)
            {
                if (!Equals(array[i], other[i]))
                    return false;
            }

            return true;
        }

        public static bool OrderReferenceEquals<T>(this T[] array, T[] other)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentNullException.ThrowIfNull(other, nameof(other));

            if (array.Length != other.Length)
                return false;

            for (int i = 0; i < array.Length; i++)
            {
                if (!ReferenceEquals(array[i], other[i]))
                    return false;
            }

            return true;
        }
    }
}
