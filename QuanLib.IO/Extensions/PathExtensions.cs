using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.Extensions
{
    public static class PathExtensions
    {
        extension(string source)
        {
            public string PathCombine(string path)
            {
                return Path.Combine(source, path);
            }

            public string PathCombine(string path1, string path2)
            {
                return Path.Combine(source, path1, path2);
            }

            public string PathCombine(string path1, string path2, string path3)
            {
                return Path.Combine(source, path1, path2, path3);
            }

            public string PathCombine(params string[] paths)
            {
                ArgumentNullException.ThrowIfNull(paths, nameof(paths));

                string[] allPaths = new string[paths.Length + 1];
                allPaths[0] = source;
                paths.CopyTo(allPaths, 1);
                return Path.Combine(allPaths);
            }

            public FileInfo CreateFileInfo()
            {
                return new(source);
            }

            public DirectoryInfo CreateDirectoryInfo()
            {
                return new(source);
            }
        }
    }
}
