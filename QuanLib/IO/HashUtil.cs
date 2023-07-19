using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    /// <summary>
    /// 哈希工具类
    /// </summary>
    public static class HashUtil
    {
        /// <summary>
        /// 获取字节指定数组的哈希值
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static byte[] GetHash(byte[] bytes, HashType hashType)
        {
            return HashAlgorithm.Create(hashType.ToString()).ComputeHash(bytes);
        }

        /// <summary>
        /// 获取指定流的哈希值
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static byte[] GetHash(Stream stream, HashType hashType)
        {
            return HashAlgorithm.Create(hashType.ToString()).ComputeHash(stream);
        }

        /// <summary>
        /// 获取指定路径目标文件的哈希值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static byte[] GetHash(string path, HashType hashType)
        {
            FileStream fileStream = new(path, FileMode.Open);
            byte[] result = GetHash(fileStream, hashType);
            fileStream.Close();
            return result;
        }

        /// <summary>
        /// 获取字节指定数组的哈希值字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static string GetHashString(byte[] bytes, HashType hashType)
        {
            byte[] hash = GetHash(bytes, hashType);
            return BytesToHexString(hash);
        }

        /// <summary>
        /// 获取指定路径目标文件的哈希值字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static string GetHashString(Stream stream, HashType hashType)
        {
            byte[] hash = GetHash(stream, hashType);
            return BytesToHexString(hash);
        }

        /// <summary>
        /// 获取指定路径目标文件的哈希值字符串
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static string GetHashString(string path, HashType hashType)
        {
            byte[] hash = GetHash(path, hashType);
            return BytesToHexString(hash);
        }

        /// <summary>
        /// 确定两个流的哈希值是否相对
        /// </summary>
        /// <param name="stream1"></param>
        /// <param name="stream2"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static bool Equals(Stream stream1, Stream stream2, HashType hashType)
        {
            return Enumerable.SequenceEqual(GetHash(stream1, hashType), GetHash(stream2, hashType));
        }

        /// <summary>
        /// 确定两个路径目标文件的哈希值是否相等
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="hashType"></param>
        /// <returns></returns>
        public static bool Equals(string path1, string path2, HashType hashType)
        {
            FileStream fileStream1 = new (path1, FileMode.Open);
            FileStream fileStream2 = new (path2, FileMode.Open);
            bool result = Equals(fileStream1, fileStream2, hashType);
            fileStream1.Close();
            fileStream2.Close();
            return result;
        }

        private static string BytesToHexString(byte[] bytes)
        {
            string[] hashString = new string[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                hashString[i] = Convert.ToString(bytes[i], 16);

            return string.Join(' ', hashString);
        }
    }
}
