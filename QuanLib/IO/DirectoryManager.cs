using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public class DirectoryManager
    {
        public DirectoryManager(string directory)
        {
            Directory = directory ?? throw new ArgumentNullException(nameof(directory));
        }

        public string Directory { get; }

        public string Combine(string path)
        {
            return Path.Combine(Directory, path);
        }

        public bool ExistsFile(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));

            return File.Exists(Combine(name));
        }

        public bool ExistsDirectory(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));

            return System.IO.Directory.Exists(Combine(name));
        }

        public string[] GetFiles()
        {
            return System.IO.Directory.GetFiles(Directory);
        }

        public string[] GetFiles(string searchPattern)
        {
            return System.IO.Directory.GetFiles(Directory, searchPattern);
        }

        public string[] GetDirectorys()
        {
            return System.IO.Directory.GetDirectories(Directory);
        }

        public string[] GetDirectorys(string searchPattern)
        {
            return System.IO.Directory.GetDirectories(Directory, searchPattern);
        }

        public override string ToString()
        {
            return Directory;
        }
    }
}
