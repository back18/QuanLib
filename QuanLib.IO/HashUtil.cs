using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static HashAlgorithm Create(this HashType hashType)
        {
            return hashType switch
            {
                HashType.SHA1 => SHA1.Create(),
                HashType.SHA256 => SHA256.Create(),
                HashType.SHA384 => SHA384.Create(),
                HashType.SHA512 => SHA512.Create(),
                HashType.MD5 => MD5.Create(),
                _ => throw new InvalidEnumArgumentException(nameof(hashType), (int)hashType, typeof(HashType))
            };
        }

        public static int GetHashSizeInBits(this HashType hashType)
        {
            return hashType switch
            {
                HashType.SHA1 => SHA1.HashSizeInBits,
                HashType.SHA256 => SHA256.HashSizeInBits,
                HashType.SHA384 => SHA384.HashSizeInBits,
                HashType.SHA512 => SHA512.HashSizeInBits,
                HashType.MD5 => MD5.HashSizeInBits,
                _ => throw new InvalidEnumArgumentException(nameof(hashType), (int)hashType, typeof(HashType))
            };
        }

        public static int GetHashSizeInBytes(this HashType hashType)
        {
            return hashType switch
            {
                HashType.SHA1 => SHA1.HashSizeInBytes,
                HashType.SHA256 => SHA256.HashSizeInBytes,
                HashType.SHA384 => SHA384.HashSizeInBytes,
                HashType.SHA512 => SHA512.HashSizeInBytes,
                HashType.MD5 => MD5.HashSizeInBytes,
                _ => throw new InvalidEnumArgumentException(nameof(hashType), (int)hashType, typeof(HashType))
            };
        }

        public static int GetHashSizeInChars(this HashType hashType)
        {
            return hashType switch
            {
                HashType.SHA1 => SHA1.HashSizeInBytes * 2,
                HashType.SHA256 => SHA256.HashSizeInBytes * 2,
                HashType.SHA384 => SHA384.HashSizeInBytes * 2,
                HashType.SHA512 => SHA512.HashSizeInBytes * 2,
                HashType.MD5 => MD5.HashSizeInBytes * 2,
                _ => throw new InvalidEnumArgumentException(nameof(hashType), (int)hashType, typeof(HashType))
            };
        }

        private static string ToHexString(byte[] bytes, bool toLower = false)
        {
            string result = Convert.ToHexString(bytes);
            return toLower ? result.ToLower() : result;
        }

        #region 同步方法

        public static byte[] GetHashValue(byte[] bytes, HashType hashType)
        {
            ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

            using HashAlgorithm hashAlgorithm = Create(hashType);
            return hashAlgorithm.ComputeHash(bytes);
        }

        public static byte[] GetHashValue(Stream stream, HashType hashType)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            using HashAlgorithm hashAlgorithm = Create(hashType);
            return hashAlgorithm.ComputeHash(stream);
        }

        public static byte[] GetHashValue(string path, HashType hashType)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            using FileStream fileStream = File.OpenRead(path);
            byte[] result = GetHashValue(fileStream, hashType);
            return result;
        }

        public static string GetHashString(byte[] bytes, HashType hashType)
        {
            byte[] hash = GetHashValue(bytes, hashType);
            return ToHexString(hash, true);
        }

        public static string GetHashString(Stream stream, HashType hashType)
        {
            byte[] hash = GetHashValue(stream, hashType);
            return ToHexString(hash, true);
        }

        public static string GetHashString(string path, HashType hashType)
        {
            byte[] hash = GetHashValue(path, hashType);
            return ToHexString(hash, true);
        }

        public static bool HashEquals(byte[] bytes1, byte[] bytes2, HashType hashType)
        {
            ArgumentNullException.ThrowIfNull(bytes1, nameof(bytes1));
            ArgumentNullException.ThrowIfNull(bytes2, nameof(bytes2));

            if (ReferenceEquals(bytes1, bytes2))
                return true;
            if (bytes1.Length != bytes2.Length)
                return false;

            return GetHashValue(bytes1, hashType).SequenceEqual(GetHashValue(bytes2, hashType));
        }

        public static bool HashEquals(Stream stream1, Stream stream2, HashType hashType)
        {
            ArgumentNullException.ThrowIfNull(stream1, nameof(stream1));
            ArgumentNullException.ThrowIfNull(stream2, nameof(stream2));

            if (ReferenceEquals(stream1, stream2))
                return true;
            if (stream1.CanSeek && stream2.CanSeek &&
                (stream1.Length - stream1.Position) != (stream2.Length - stream2.Position))
                return false;

            return GetHashValue(stream1, hashType).SequenceEqual(GetHashValue(stream2, hashType));
        }

        public static bool HashEquals(string path1, string path2, HashType hashType)
        {
            ArgumentException.ThrowIfNullOrEmpty(path1, nameof(path1));
            ArgumentException.ThrowIfNullOrEmpty(path2, nameof(path2));

            FileInfo fileInfo1 = new(path1);
            FileInfo fileInfo2 = new(path2);

            if (fileInfo1.FullName == fileInfo2.FullName)
                return true;
            if (!fileInfo1.Exists && !fileInfo2.Exists)
                return true;
            if (!fileInfo1.Exists || !fileInfo2.Exists)
                return false;
            if (fileInfo1.Length != fileInfo2.Length)
                return false;

            return GetHashValue(path1, hashType).SequenceEqual(GetHashValue(path2, hashType));
        }

        #endregion

        #region 异步方法

        public static async Task<byte[]> GetHashValueAsync(byte[] bytes, HashType hashType, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

            using HashAlgorithm hashAlgorithm = Create(hashType);
            using MemoryStream memoryStream = new(bytes);
            return await hashAlgorithm.ComputeHashAsync(memoryStream, cancellationToken);
        }

        public static async Task<byte[]> GetHashValueAsync(Stream stream, HashType hashType, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            using HashAlgorithm hashAlgorithm = Create(hashType);
            return await hashAlgorithm.ComputeHashAsync(stream, cancellationToken);
        }

        public static async Task<byte[]> GetHashValueAsync(string path, HashType hashType, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            using FileStream fileStream = File.OpenRead(path);
            return await GetHashValueAsync(fileStream, hashType, cancellationToken);
        }

        public static async Task<string> GetHashStringAsync(byte[] bytes, HashType hashType, CancellationToken cancellationToken = default)
        {
            byte[] hash = await GetHashValueAsync(bytes, hashType, cancellationToken).ConfigureAwait(false);
            return ToHexString(hash, true);
        }

        public static async Task<string> GetHashStringAsync(Stream stream, HashType hashType, CancellationToken cancellationToken = default)
        {
            byte[] hash = await GetHashValueAsync(stream, hashType, cancellationToken).ConfigureAwait(false);
            return ToHexString(hash, true);
        }

        public static async Task<string> GetHashStringAsync(string path, HashType hashType, CancellationToken cancellationToken = default)
        {
            byte[] hash = await GetHashValueAsync(path, hashType, cancellationToken).ConfigureAwait(false);
            return ToHexString(hash, true);
        }

        public static async Task<bool> HashEqualsAsync(byte[] bytes1, byte[] bytes2, HashType hashType, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(bytes1, nameof(bytes1));
            ArgumentNullException.ThrowIfNull(bytes2, nameof(bytes2));

            if (ReferenceEquals(bytes1, bytes2))
                return true;
            if (bytes1.Length != bytes2.Length)
                return false;

            Task<byte[]> task1 = GetHashValueAsync(bytes1, hashType, cancellationToken);
            Task<byte[]> task2 = GetHashValueAsync(bytes2, hashType, cancellationToken);

            byte[] result1 = await task1.ConfigureAwait(false);
            byte[] result2 = await task2.ConfigureAwait(false);
            return result1.SequenceEqual(result2);
        }

        public static async Task<bool> HashEqualsAsync(Stream stream1, Stream stream2, HashType hashType, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream1, nameof(stream1));
            ArgumentNullException.ThrowIfNull(stream2, nameof(stream2));

            if (ReferenceEquals(stream1, stream2))
                return true;
            if (stream1.CanSeek && stream2.CanSeek &&
                (stream1.Length - stream1.Position) != (stream2.Length - stream2.Position))
                return false;

            Task<byte[]> task1 = GetHashValueAsync(stream1, hashType, cancellationToken);
            Task<byte[]> task2 = GetHashValueAsync(stream2, hashType, cancellationToken);

            byte[] result1 = await task1.ConfigureAwait(false);
            byte[] result2 = await task2.ConfigureAwait(false);
            return result1.SequenceEqual(result2);
        }

        public static async Task<bool> HashEqualsAsync(string path1, string path2, HashType hashType, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(path1, nameof(path1));
            ArgumentException.ThrowIfNullOrEmpty(path2, nameof(path2));

            FileInfo fileInfo1 = new(path1);
            FileInfo fileInfo2 = new(path2);

            if (fileInfo1.FullName == fileInfo2.FullName)
                return true;
            if (!fileInfo1.Exists && !fileInfo2.Exists)
                return true;
            if (!fileInfo1.Exists || !fileInfo2.Exists)
                return false;
            if (fileInfo1.Length != fileInfo2.Length)
                return false;

            Task<byte[]> task1 = GetHashValueAsync(path1, hashType, cancellationToken);
            Task<byte[]> task2 = GetHashValueAsync(path2, hashType, cancellationToken);

            byte[] result1 = await task1.ConfigureAwait(false);
            byte[] result2 = await task2.ConfigureAwait(false);
            return result1.SequenceEqual(result2);
        }

        #endregion
    }
}
