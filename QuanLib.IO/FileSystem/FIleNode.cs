using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.FileSystem
{
    public class FileNode : Node
    {
        public FileNode(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            Name = name;
        }

        public override string Name { get; }
    }
}
