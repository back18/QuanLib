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
    }
}
