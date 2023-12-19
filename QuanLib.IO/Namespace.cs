using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public static class Namespace
    {
        private const char CHAR = '.';

        public static string Combine(string path1, string path2)
        {
            ArgumentException.ThrowIfNullOrEmpty(path1, nameof(path1));
            ArgumentException.ThrowIfNullOrEmpty(path2, nameof(path2));

            path1 = path1.TrimEnd(CHAR);
            path2 = path2.TrimStart(CHAR);
            return $"{path1}.{path2}";
        }

        public static string Combine(string path1, string path2, string path3)
        {
            ArgumentException.ThrowIfNullOrEmpty(path1, nameof(path1));
            ArgumentException.ThrowIfNullOrEmpty(path2, nameof(path2));
            ArgumentException.ThrowIfNullOrEmpty(path3, nameof(path3));

            path1 = path1.TrimEnd(CHAR);
            path2 = path2.Trim(CHAR);
            path3 = path3.TrimStart(CHAR);
            return $"{path1}.{path2}.{path3}";
        }

        public static string Combine(string path1, string path2, string path3, string path4)
        {
            ArgumentException.ThrowIfNullOrEmpty(path1, nameof(path1));
            ArgumentException.ThrowIfNullOrEmpty(path2, nameof(path2));
            ArgumentException.ThrowIfNullOrEmpty(path3, nameof(path3));
            ArgumentException.ThrowIfNullOrEmpty(path4, nameof(path4));

            path1 = path1.TrimEnd(CHAR);
            path2 = path2.Trim(CHAR);
            path3 = path3.Trim(CHAR);
            path4 = path4.TrimStart(CHAR);
            return $"{path1}.{path2}.{path3}.{path4}";
        }

        public static string Combine(params string[] paths)
        {
            ArgumentNullException.ThrowIfNull(paths, nameof(paths));
            if (paths.Length == 0)
                return string.Empty;
            if (paths.Length == 1)
                return paths[0];

            foreach (var path in paths)
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentException($"“{nameof(paths)}”的一个或多个子项为 null 或空", nameof(paths));
            }

            StringBuilder sb = new();
            sb.Append(paths[0].TrimEnd(CHAR));
            for (int i = 1; i < paths.Length - 1; i++)
            {
                sb.Append(CHAR);
                sb.Append(paths[i].Trim(CHAR));
            }
            sb.Append(CHAR);
            sb.Append(paths[^1].TrimStart(CHAR));

            return sb.ToString();
        }
    }
}
