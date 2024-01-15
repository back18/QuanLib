using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.FileSystem
{
    public abstract class Node
    {
        public abstract string Name { get; }

        public virtual ContainerNode? ParentNode { get; internal set; }

        public virtual string GetFullName(char separatorChar)
        {
            if (ParentNode is not null)
                return ParentNode.GetFullName(separatorChar) + separatorChar + Name;
            else
                return Name;
        }

        public void Delete()
        {
            if (ParentNode is null)
                return;

            ParentNode.Remvoe(Name);
            ParentNode = null;
        }

        public DeviceNode? AsDeviceNode()
        {
            return this as DeviceNode;
        }

        public DirectoryNode? AsDirectoryNode()
        {
            return this as DirectoryNode;
        }

        public FileNode? AsFIleNode()
        {
            return this as FileNode;
        }

        public override string ToString()
        {
            return GetFullName('/');
        }
    }
}
