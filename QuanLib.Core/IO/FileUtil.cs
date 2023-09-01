using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    public static class FileUtil
    {
        public static PathType GetPathType(string path)
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (path == drive.Name)
                    return PathType.Drive;
            }

            if (Directory.Exists(path))
                return PathType.Directory;
            else if (File.Exists(path))
                return PathType.File;

            return PathType.Unknown;
        }
    }
}
