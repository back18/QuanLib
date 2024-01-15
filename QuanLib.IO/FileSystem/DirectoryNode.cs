using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.FileSystem
{
    public class DirectoryNode : ContainerNode
    {
        public DirectoryNode(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            Name = name;
        }

        public override string Name { get; }
    }
}
