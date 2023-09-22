using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    /// <summary>
    /// 哈希工具类
    /// </summary>
    public static class HashUtil
    {
        public static byte[] GetHashValue(byte[] bytes, HashType hashType)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            return HashManager.Get(hashType).ComputeHash(bytes);
        }

        public static byte[] GetHashValue(Stream stream, HashType hashType)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return HashManager.Get(hashType).ComputeHash(stream);
        }

        public static byte[] GetHashValue(string path, HashType hashType)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException($"“{nameof(path)}”不能为 null 或空。", nameof(path));

            using FileStream fileStream = new(path, FileMode.Open);
            byte[] result = GetHashValue(fileStream, hashType);
            fileStream.Close();
            return result;
        }

        public static string GetHashString(byte[] bytes, HashType hashType)
        {
            byte[] hash = GetHashValue(bytes, hashType);
            return BytesToHexString(hash);
        }

        public static string GetHashString(Stream stream, HashType hashType)
        {
            byte[] hash = GetHashValue(stream, hashType);
            return BytesToHexString(hash);
        }

        public static string GetHashString(string path, HashType hashType)
        {
            byte[] hash = GetHashValue(path, hashType);
            return BytesToHexString(hash);
        }

        public static bool Equals(Stream stream1, Stream stream2, HashType hashType)
        {
            return GetHashValue(stream1, hashType).SequenceEqual(GetHashValue(stream2, hashType));
        }

        public static bool Equals(string path1, string path2, HashType hashType)
        {
            return GetHashValue(path1, hashType).SequenceEqual(GetHashValue(path2, hashType));
        }

        private static string BytesToHexString(byte[] bytes)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            StringBuilder result = new();
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString("x2"));
            return result.ToString();
        }
    }
}
