using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public static class FileUtil
    {
        public static PathType GetPathType(string path)
        {
            ArgumentNullException.ThrowIfNull(path, nameof(path));

            string fullPath = Path.GetFullPath(path);

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (fullPath == drive.Name)
                    return PathType.Drive;
            }

            if (Directory.Exists(fullPath))
                return PathType.Directory;
            else if (File.Exists(fullPath))
                return PathType.File;

            return PathType.Unknown;
        }

        public static void CreateFileDirectoryIfNotExists(string filePath)
        {
            ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));

            string fullPath = Path.GetFullPath(filePath);
            string? directoryPath = Path.GetDirectoryName(fullPath);
            if (directoryPath is null)
                return;

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }
    }
}
