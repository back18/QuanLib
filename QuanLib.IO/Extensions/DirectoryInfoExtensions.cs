using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.Extensions
{
    public static class DirectoryInfoExtensions
    {
        extension(DirectoryInfo source)
        {
            public FileInfo CombineFile(string path)
            {
                return new(Path.Combine(source.FullName, path));
            }

            public FileInfo CombineFile(string path1, string path2)
            {
                return new(Path.Combine(source.FullName, path1, path2));
            }

            public FileInfo CombineFile(string path1, string path2, string path3)
            {
                return new(Path.Combine(source.FullName, path1, path2, path3));
            }

            public FileInfo CombineFile(params string[] paths)
            {
                ArgumentNullException.ThrowIfNull(paths, nameof(paths));

                string[] allPaths = new string[paths.Length + 1];
                allPaths[0] = source.FullName;
                paths.CopyTo(allPaths, 1);
                return new(Path.Combine(allPaths));
            }

            public DirectoryInfo CombineDirectory(string path)
            {
                return new(Path.Combine(source.FullName, path));
            }

            public DirectoryInfo CombineDirectory(string path1, string path2)
            {
                return new(Path.Combine(source.FullName, path1, path2));
            }

            public DirectoryInfo CombineDirectory(string path1, string path2, string path3)
            {
                return new(Path.Combine(source.FullName, path1, path2, path3));
            }

            public DirectoryInfo CombineDirectory(params string[] paths)
            {
                ArgumentNullException.ThrowIfNull(paths, nameof(paths));

                string[] allPaths = new string[paths.Length + 1];
                allPaths[0] = source.FullName;
                paths.CopyTo(allPaths, 1);
                return new(Path.Combine(allPaths));
            }

            public string[] GetFilePaths()
            {
                return Directory.GetFiles(source.FullName);
            }

            public string[] GetFilePaths(string searchPattern)
            {
                return Directory.GetFiles(source.FullName, searchPattern);
            }

            public string[] GetFilePaths(string searchPattern, SearchOption searchOption)
            {
                return Directory.GetFiles(source.FullName, searchPattern, searchOption);
            }

            public string[] GetFilePaths(string searchPattern, EnumerationOptions enumerationOptions)
            {
                return Directory.GetFiles(source.FullName, searchPattern, enumerationOptions);
            }

            public string[] GetDirectoriePaths()
            {
                return Directory.GetDirectories(source.FullName);
            }

            public string[] GetDirectoriePaths(string searchPattern)
            {
                return Directory.GetDirectories(source.FullName, searchPattern);
            }

            public string[] GetDirectoriePaths(string searchPattern, SearchOption searchOption)
            {
                return Directory.GetDirectories(source.FullName, searchPattern, searchOption);
            }

            public string[] GetDirectoriePaths(string searchPattern, EnumerationOptions enumerationOptions)
            {
                return Directory.GetDirectories(source.FullName, searchPattern, enumerationOptions);
            }

            public void CreateIfNotExists()
            {
                if (!source.Exists)
                    source.Create();
            }
        }
    }
}
