using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    public static class HashManager
    {
        public static readonly SHA1 SHA1 = SHA1.Create();

        public static readonly SHA256 SHA256 = SHA256.Create();

        public static readonly SHA384 SHA384 = SHA384.Create();

        public static readonly SHA512 SHA512 = SHA512.Create();

        public static readonly MD5 MD5 = MD5.Create();

        public static HashAlgorithm Get(HashType hashType)
        {
            return hashType switch
            {
                HashType.SHA1 => SHA1,
                HashType.SHA256 => SHA256,
                HashType.SHA384 => SHA384,
                HashType.SHA512 => SHA512,
                HashType.MD5 => MD5,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
