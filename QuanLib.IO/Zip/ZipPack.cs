using QuanLib.IO.FileSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.Zip
{
    public class ZipPack : IDisposable
    {
        private const char SeparatorChar = '/';

        public ZipPack(string path, ZipArchiveMode mode, Encoding encoding)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            _archive = ZipFile.Open(path, mode, encoding);
            _rootNode = BuildRootNode(_archive);
        }

        public ZipPack(string path, ZipArchiveMode mode) : this(path, mode, Encoding.UTF8) { }

        public ZipPack(string path) : this(path, ZipArchiveMode.Read) { }

        public ZipPack(Stream stream, ZipArchiveMode mode, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            _archive = new(stream, mode, true, encoding);
            _rootNode = BuildRootNode(_archive);
        }

        public ZipPack(Stream stream, ZipArchiveMode mode) : this(stream, mode, Encoding.UTF8) { }

        public ZipPack(Stream stream) : this(stream, ZipArchiveMode.Read) { }

        private readonly ZipArchive _archive;

        private readonly DeviceNode _rootNode;

        public bool ExistsDirectory(string? path)
        {
            return _rootNode.ExistsDirectoryNode(path);
        }

        public bool ExistsFile(string? path)
        {
            return _rootNode.ExistsDirectoryNode(path);
        }

        public string[] GetDirectoryPaths(string path)
        {
            return _rootNode.GetDirectoryNodes(path).Select(s => s.GetFullName(SeparatorChar)).ToArray();
        }

        public string[] GetFilePaths(string path)
        {
            return _rootNode.GetFileNodes(path).Select(s => s.GetFullName(SeparatorChar)).ToArray();
        }

        public ZipItem GetFile(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            FileNode? fileNode = _rootNode.GetFileNode(path);
            ZipArchiveEntry? entry = fileNode is null ? null : _archive.GetEntry(fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar));
            if (fileNode is null || entry is null)
                throw new ArgumentException($"路径“{path}”的文件不存在");

            return new(entry);
        }

        public bool TryGetFile(string? path, [MaybeNullWhen(false)] out ZipItem result)
        {
            if (string.IsNullOrEmpty(path))
            {
                result = null;
                return false;
            }

            FileNode? fileNode = _rootNode.GetFileNode(path);
            ZipArchiveEntry? entry = fileNode is null ? null : _archive.GetEntry(fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar));
            if (fileNode is null || entry is null)
            {
                result = null;
                return false;
            }

            result = new(entry);
            return true;
        }

        public void AddFile(string path, byte[] data)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            FileNode fileNode = _rootNode.CreateFileNode(path);
            ZipArchiveEntry entry = _archive.CreateEntry(fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar));
            using Stream stream = entry.Open();
            stream.Write(data, 0, data.Length);
        }

        public void AddFile(string path, Stream stream)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            if (stream.CanSeek && stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);

            FileNode fileNode = _rootNode.CreateFileNode(path);
            ZipArchiveEntry entry = _archive.CreateEntry(fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar));
            using Stream entryStream = entry.Open();
            stream.CopyTo(entryStream);
        }

        public void RemoveFile(string path)
        {
            FileNode? fileNode = _rootNode.GetFileNode(path);
            ZipArchiveEntry? entry = fileNode is null ? null : _archive.GetEntry(fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar));
            if (fileNode is null || entry is null)
                throw new ArgumentException($"路径“{path}”的文件不存在");

            entry.Delete();
            fileNode.Delete();
        }

        private static DeviceNode BuildRootNode(ZipArchive archive)
        {
            ArgumentNullException.ThrowIfNull(archive, nameof(archive));

            DeviceNode rootNode = new(string.Empty, SeparatorChar);
            foreach (var entry in archive.Entries)
            {
                string path = entry.FullName;
                if (path.EndsWith(SeparatorChar))
                {
                    path = path.TrimEnd(SeparatorChar);
                    rootNode.CreateDirectoryNode(path);
                }
                else
                {
                    rootNode.CreateFileNode(path);
                }
            }

            return rootNode;
        }

        public void Dispose()
        {
            _archive.Dispose();
        }
    }
}
