using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.Extensions
{
    public static class PathExtensions
    {
        public static string PathCombine(this string source, string path)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Path.Combine(source, path);
        }

        public static string PathCombine(this string source, string path1, string path2)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Path.Combine(source, path1, path2);
        }

        public static string PathCombine(this string source, string path1, string path2, string path3)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Path.Combine(source, path1, path2, path3);
        }

        public static FileInfo CreateFileInfo(this string source)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new(source);
        }

        public static DirectoryInfo CreateDirectoryInfo(this string source)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new(source);
        }
    }
}
