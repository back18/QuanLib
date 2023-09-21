using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    public class DirectoryManager
    {
        public DirectoryManager(string directory)
        {
            FullPath = directory ?? throw new ArgumentNullException(nameof(directory));
        }

        public string FullPath { get; }

        public string Combine(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            return Path.Combine(FullPath, path);
        }

        public bool Exists()
        {
            return Directory.Exists(FullPath);
        }

        public void CreateIfNotExists()
        {
            if (Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);
        }

        public void Delete()
        {
            Directory.Delete(FullPath);
        }

        public void Delete(bool recursive)
        {
            Directory.Delete(FullPath, recursive);
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

            return Directory.Exists(Combine(name));
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(FullPath);
        }

        public string[] GetFiles(string searchPattern)
        {
            return Directory.GetFiles(FullPath, searchPattern);
        }

        public string[] GetDirectorys()
        {
            return Directory.GetDirectories(FullPath);
        }

        public string[] GetDirectorys(string searchPattern)
        {
            return Directory.GetDirectories(FullPath, searchPattern);
        }

        public override string ToString()
        {
            return FullPath;
        }
    }
}
