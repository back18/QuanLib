using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class DirectoryUtil
    {
        public static void CreateDirectoryIfNotExists(string directoryPath)
        {
            ArgumentNullException.ThrowIfNull(directoryPath, nameof(directoryPath));

            string fullPath = Path.GetFullPath(directoryPath);

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }

        public static string[] GetAllFiles(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            List<string> result = [];
            string[] directorys = Directory.GetDirectories(path);
            result.AddRange(Directory.GetFiles(path));

            foreach (string directory in directorys)
                result.AddRange(GetAllFiles(directory));

            return result.ToArray();
        }

        public static string[] GetAllDirectories(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            List<string> result = [];
            string[] directories = Directory.GetDirectories(path);
            result.AddRange(directories);

            foreach (string directory in directories)
                result.AddRange(GetAllDirectories(directory));

            return result.ToArray();
        }
    }
}
