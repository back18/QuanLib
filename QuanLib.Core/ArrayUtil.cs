using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class ArrayUtil
    {
        public static void Initialize<T>(T[] array) where T : class, new()
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            for (int i = 0; i < array.Length; i++)
                array[i] = new();
        }

        public static void Fill<T>(T[,] array, T value)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    array[i, j] = value;
        }

        public static void Fill<T>(T[,,] array, T value)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            int dimension2 = array.GetLength(2);
            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    for (int k = 0; k < dimension2; k++)
                        array[i, j, k] = value;
        }

        public static T[] Create<T>(int dimension0, T value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            ThrowHelper.ArgumentOutOfMin(0, dimension0, nameof(dimension0));

            T[] newarray = new T[dimension0];
            Array.Fill(newarray, value);
            return newarray;
        }

        public static T[,] Create<T>(int dimension0, int dimension1, T value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            ThrowHelper.ArgumentOutOfMin(0, dimension0, nameof(dimension0));
            ThrowHelper.ArgumentOutOfMin(0, dimension1, nameof(dimension1));

            T[,] newarray = new T[dimension0, dimension1];
            Fill(newarray, value);
            return newarray;
        }

        public static T[,,] Create<T>(int dimension0, int dimension1, int dimension2, T value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            ThrowHelper.ArgumentOutOfMin(0, dimension0, nameof(dimension0));
            ThrowHelper.ArgumentOutOfMin(0, dimension1, nameof(dimension1));
            ThrowHelper.ArgumentOutOfMin(0, dimension2, nameof(dimension2));

            T[,,] newarray = new T[dimension0, dimension1, dimension2];
            Fill(newarray, value);
            return newarray;
        }

        public static T[,] Clone<T>(this T[,] array)
        {
            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            T[,] newarray = new T[dimension0, dimension1];

            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    newarray[i, j] = array[i, j];

            return newarray;
        }

        public static T[,,] Clone<T>(this T[,,] array)
        {
            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            int dimension2 = array.GetLength(2);
            T[,,] newarray = new T[dimension0, dimension1, dimension2];

            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    for (int k = 0; k < dimension2; k++)
                        newarray[i, j, k] = array[i, j, k];

            return newarray;
        }

        public static T[] PadLeft<T>(T[] array, T padding)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            T[] newArray = new T[array.Length + 1];
            array.CopyTo(newArray, 1);
            newArray[0] = padding;

            return newArray;
        }

        public static T[] PadRight<T>(T[] array, T padding)
        {
            ArgumentNullException.ThrowIfNull(array, nameof(array));

            T[] newArray = new T[array.Length + 1];
            array.CopyTo(newArray, 0);
            newArray[^1] = padding;

            return newArray;
        }
    }
}
