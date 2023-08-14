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

        public override string ToString()
        {
            return Directory;
        }
    }
}
