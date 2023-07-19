using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib
{
    /// <summary>
    /// 数组工具类
    /// </summary>
    public static class ArrayUtil
    {
        /// <summary>
        /// 使用无参构造函数构造数组内所有元素
        /// </summary>
        /// <typeparam name="T">必须是引用类型，必须拥有无参构造函数</typeparam>
        /// <param name="array"></param>
        public static void Initialize<T>(T[] array) where T : class, new()
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = new();
        }

        /// <summary>
        /// 填充数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        public static void Fill<T>(T[,] array, T value)
        {
            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    array[i, j] = value;
        }

        /// <summary>
        /// 填充数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        public static void Fill<T>(T[,,] array, T value)
        {
            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            int dimension2 = array.GetLength(2);
            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    for (int k = 0; k < dimension2; k++)
                        array[i, j, k] = value;
        }

        public static T[] FillToNewArray<T>(int dimension0, T value)
        {
            if (dimension0 < 0)
                throw new ArithmeticException("新数组长度不能小于0");

            T[] newarray = new T[dimension0];
            Array.Fill(newarray, value);
            return newarray;
        }

        public static T[,] FillToNewArray<T>(int dimension0, int dimension1, T value)
        {
            if (dimension0 < 0 || dimension1 < 0)
                throw new ArithmeticException("新数组长度不能小于0");

            T[,] newarray = new T[dimension0, dimension1];
            Fill(newarray, value);
            return newarray;
        }

        public static T[,,] FillToNewArray<T>(int dimension0, int dimension1, int dimension2, T value)
        {
            if (dimension0 < 0 || dimension1 < 0 || dimension2 < 0)
                throw new ArithmeticException("新数组长度不能小于0");

            T[,,] newarray = new T[dimension0, dimension1, dimension2];
            Fill(newarray, value);
            return newarray;
        }

        public static T[,] Copy<T>(T[,] array)
        {
            int dimension0 = array.GetLength(0);
            int dimension1 = array.GetLength(1);
            T[,] newarray = new T[dimension0, dimension1];
            for (int i = 0; i < dimension0; i++)
                for (int j = 0; j < dimension1; j++)
                    newarray[i, j] = array[i, j];

            return newarray;
        }

        public static T[,,] Copy<T>(T[,,] array)
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

        /// <summary>
        /// 乱序比较两个数组元素的值是否一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public static bool DisorderEquals<T>(T[] arg1, T[] arg2)
        {
            if (arg1 is null)
                if (arg2 is null)
                    return true;
                else return false;

            if (arg1.Length != arg2.Length)
                return false;

            foreach (var item in arg1)
                if (Array.IndexOf(arg2, item) == -1)
                    return false;
            return true;
        }

        /// <summary>
        /// 泛型数组转string数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string?[] ToStrings<T>(T[] array)
        {
            string?[] result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = array[i]?.ToString();

            return result;
        }

        /// <summary>
        /// 向泛型数组的开头插入一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static T[] PadLeft<T>(T[] array, T padding)
        {
            T[] newArray = new T[array.Length + 1];

            Array.Copy(array, 0, newArray, 1, array.Length);
            newArray[0] = padding;

            return newArray;
        }

        /// <summary>
        /// 向泛型数组的末尾插入一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static T[] PadRight<T>(T[] array, T padding)
        {
            T[] newArray = new T[array.Length + 1];

            array.CopyTo(newArray, 0);
            newArray[^1] = padding;

            return newArray;
        }
    }
}
