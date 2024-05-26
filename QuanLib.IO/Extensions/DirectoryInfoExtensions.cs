using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static FileInfo CombineFile(this DirectoryInfo source, string path)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new(Path.Combine(source.FullName, path));
        }

        public static FileInfo CombineFile(this DirectoryInfo source, string path1, string path2)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new(Path.Combine(source.FullName, path1, path2));
        }

        public static FileInfo CombineFile(this DirectoryInfo source, string path1, string path2, string path3)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new(Path.Combine(source.FullName, path1, path2, path3));
        }

        public static DirectoryInfo CombineDirectory(this DirectoryInfo source, string path)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new(Path.Combine(source.FullName, path));
        }

        public static DirectoryInfo CombineDirectory(this DirectoryInfo source, string path1, string path2)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new(Path.Combine(source.FullName, path1, path2));
        }

        public static DirectoryInfo CombineDirectory(this DirectoryInfo source, string path1, string path2, string path3)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return new(Path.Combine(source.FullName, path1, path2, path3));
        }

        public static string[] GetFilePaths(this DirectoryInfo source)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Directory.GetFiles(source.FullName);
        }

        public static string[] GetFilePaths(this DirectoryInfo source, string searchPattern)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Directory.GetFiles(source.FullName, searchPattern);
        }

        public static string[] GetFilePaths(this DirectoryInfo source, string searchPattern, EnumerationOptions enumerationOptions)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Directory.GetFiles(source.FullName, searchPattern, enumerationOptions);
        }

        public static string[] GetFilePaths(this DirectoryInfo source, string searchPattern, SearchOption searchOption)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Directory.GetFiles(source.FullName, searchPattern, searchOption);
        }

        public static string[] GetDirectoriePaths(this DirectoryInfo source)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Directory.GetDirectories(source.FullName);
        }

        public static string[] GetDirectoriePaths(this DirectoryInfo source, string searchPattern)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Directory.GetDirectories(source.FullName, searchPattern);
        }

        public static string[] GetDirectoriePaths(this DirectoryInfo source, string searchPattern, EnumerationOptions enumerationOptions)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Directory.GetDirectories(source.FullName, searchPattern, enumerationOptions);
        }

        public static string[] GetDirectoriePaths(this DirectoryInfo source, string searchPattern, SearchOption searchOption)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            return Directory.GetDirectories(source.FullName, searchPattern, searchOption);
        }

        public static void CreateIfNotExists(this DirectoryInfo source)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            if (!source.Exists)
                source.Create();
        }
    }
}
