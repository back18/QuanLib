using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.FileSystem
{
    public class DeviceNode : ContainerNode
    {
        public DeviceNode(string name, char separatorChar)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));

            Name = name;
            SeparatorChar = separatorChar;
        }

        public override string Name { get; }

        public override ContainerNode? ParentNode { get => null; internal set => throw new NotSupportedException("根节点不支持设置父级节点"); }

        public char SeparatorChar { get; }

        public DirectoryNode? GetDirectoryNode(string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys, nameof(keys));

            if (keys.Length == 0)
                return null;

            if (keys.Length == 1)
                return GetAsDirectoryNode(keys[0]);

            ContainerNode containerNode = this;
            foreach (string key in keys.SkipLast(1))
            {
                DirectoryNode? directoryNode = containerNode.GetAsDirectoryNode(key);
                if (directoryNode is null)
                    return null;

                containerNode = directoryNode;
            }

            return containerNode.GetAsDirectoryNode(keys[^1]);
        }

        public DirectoryNode? GetDirectoryNode(string? path)
        {
            if (path is null)
                return null;

            string[] keys = ToKeys(path);
            return GetDirectoryNode(keys);
        }

        public FileNode? GetFileNode(string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys, nameof(keys));

            if (keys.Length == 0)
                return null;

            if (keys.Length == 1)
                return GetAsFileNode(keys[0]);

            DirectoryNode? directoryNode = GetDirectoryNode(keys.SkipLast(1).ToArray());
            return directoryNode?.GetAsFileNode(keys[^1]);
        }

        public FileNode? GetFileNode(string? path)
        {
            if (path is null)
                return null;

            string[] keys = ToKeys(path);
            return GetFileNode(keys);
        }

        public DirectoryNode[] GetDirectoryNodes(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            string[] keys = ToKeys(path);
            if (keys.Length == 0)
                return GetDirectoryNodes();

            DirectoryNode? directoryNode = GetDirectoryNode(keys);
            return directoryNode is not null ? directoryNode.GetDirectoryNodes() : throw new ArgumentException($"路径“{path}”的文件夹不存在");
        }

        public FileNode[] GetFileNodes(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            string[] keys = ToKeys(path);
            if (keys.Length == 0)
                return GetFileNodes();

            DirectoryNode? directoryNode = GetDirectoryNode(keys);
            return directoryNode is not null ? directoryNode.GetFileNodes() : throw new ArgumentException($"路径“{path}”的文件夹不存在");
        }

        public bool ExistsDirectoryNode(string? path)
        {
            return GetDirectoryNode(path) is not null;
        }

        public bool ExistsFileNode(string? path)
        {
            return GetFileNode(path) is not null;
        }

        public DirectoryNode CreateDirectoryNode(string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys, nameof(keys));

            if (keys.Length == 0)
                throw new ArgumentException("需要创建的节点列表为空");

            ContainerNode containerNode = this;
            foreach (string key in keys)
            {
                if (containerNode.TryGetValue(key, out var node))
                {
                    if (node is not DirectoryNode directoryNode)
                        throw new InvalidOperationException($"路径“{node}”包含非目录节点");

                    containerNode = directoryNode;
                }
                else
                {
                    DirectoryNode directoryNode = new(key);
                    containerNode.Add(directoryNode);
                    containerNode = directoryNode;
                }
            }

            return (DirectoryNode)containerNode;
        }

        public DirectoryNode CreateDirectoryNode(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            string[] keys = ToKeys(path);
            return CreateDirectoryNode(keys);
        }

        public FileNode CreateFileNode(string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys, nameof(keys));

            if (keys.Length == 0)
                throw new ArgumentException("需要创建的节点列表为空");

            ContainerNode containerNode = this;
            string[] directorKeys = keys.SkipLast(1).ToArray();
            if (keys.Length > 1)
            {
                containerNode = CreateDirectoryNode(directorKeys);
            }

            if (containerNode.TryGetValue(keys[^1], out var node))
            {
                if (node is not FileNode fileNode)
                    throw new InvalidOperationException($"路径“{node}”包含非文件节点");

                return fileNode;
            }
            else
            {
                FileNode fileNode = new(keys[^1]);
                containerNode.Add(fileNode);
                return fileNode;
            }
        }

        public FileNode CreateFileNode(string path)
        {
            string[] keys = ToKeys(path);
            return CreateFileNode(keys);
        }

        public void DeleteDirectoryNode(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            DirectoryNode? directoryNode = GetDirectoryNode(path) ?? throw new ArgumentException($"路径“{path}”的文件夹不存在");
            directoryNode.Delete();
        }

        public void DeleteFileNode(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            DirectoryNode? directoryNode = GetDirectoryNode(path) ?? throw new ArgumentException($"路径“{path}”的文件夹不存在");
            directoryNode.Delete();
        }

        private string[] ToKeys(string path)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                if (!path.StartsWith(Name))
                    throw new ArgumentException($"路径的根目录应该为“{Name}”");

                path = path[Name.Length..];
            }

            path = path.TrimStart(SeparatorChar);
            if (string.IsNullOrEmpty(path))
                return [];

            string[] keys = path.Split(SeparatorChar);
            return keys;
        }
    }
}
