using QuanLib.Core;
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
    public class ZipPack : IFileSystem, IDisposable
    {
        private const char SeparatorChar = '/';

        public ZipPack(string path, ZipArchiveMode mode, Encoding encoding)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));
            if (mode != ZipArchiveMode.Read && mode != ZipArchiveMode.Update)
                throw new ArgumentException("ZipArchiveMode必须为Read或Update", nameof(mode));

            _mode = mode;
            _archive = ZipFile.Open(path, mode, encoding);
            _rootNode = BuildRootNode(_archive);
        }

        public ZipPack(string path, ZipArchiveMode mode) : this(path, mode, Encoding.UTF8) { }

        public ZipPack(string path) : this(path, ZipArchiveMode.Read) { }

        public ZipPack(Stream stream, ZipArchiveMode mode, Encoding encoding, bool leaveOpen)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));
            if (mode != ZipArchiveMode.Read && mode != ZipArchiveMode.Update)
                throw new ArgumentException("ZipArchiveMode必须为Read或Update", nameof(mode));

            ThrowHelper.StreamNotSupportSeek(stream);
            ThrowHelper.StreamNotSupportRead(stream);
            if (mode == ZipArchiveMode.Update)
                ThrowHelper.StreamNotSupportWrite(stream);

            _mode = mode;
            _archive = new(stream, mode, leaveOpen, encoding);
            _rootNode = BuildRootNode(_archive);
        }

        public ZipPack(Stream stream, ZipArchiveMode mode, Encoding encoding) : this(stream, mode, encoding, true) { }

        public ZipPack(Stream stream, ZipArchiveMode mode) : this(stream, mode, Encoding.UTF8) { }

        public ZipPack(Stream stream) : this(stream, ZipArchiveMode.Read) { }

        private bool _disposed;
        private readonly ZipArchiveMode _mode;
        private readonly ZipArchive _archive;
        private readonly DeviceNode _rootNode;

        public char PathSeparator => SeparatorChar;

        public bool DirectoryExists([NotNullWhen(true)] string? path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            return _rootNode.DirectoryNodeExists(path);
        }

        public bool FileExists([NotNullWhen(true)] string? path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            return _rootNode.FileNodeExists(path);
        }

        public string[] GetDirectoryPaths(string path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            return _rootNode.GetDirectoryNodes(path).Select(s => s.GetFullName(SeparatorChar)).ToArray();
        }

        public string[] GetFilePaths(string path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            return _rootNode.GetFileNodes(path).Select(s => s.GetFullName(SeparatorChar)).ToArray();
        }

        public Stream ReadFile(string path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            ZipItem zipItem = ReadZipItem(path);
            return zipItem.OpenStream();
        }

        public bool TryReadFile([NotNullWhen(true)] string? path, [MaybeNullWhen(false)] out Stream outputStream)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (TryReadZipItem(path, out var zipItem))
            {
                try
                {
                    outputStream = zipItem.OpenStream();
                    return true;
                }
                catch
                {
                    outputStream = null;
                    return false;
                }
            }
            else
            {
                outputStream = null;
                return false;
            }
        }

        public void CreateDirectory(string path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_mode != ZipArchiveMode.Update)
                throw new NotSupportedException("当前ZipPack实例不支持写入操作");

            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            if (_rootNode.FileNodeExists(path))
                throw new InvalidOperationException($"路径“{path}”已存在文件，无法创建目录");
            if (_rootNode.DirectoryNodeExists(path))
                return;

            DirectoryNode directoryNode = _rootNode.CreateDirectoryNode(path);
            path = directoryNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar);
            if (!path.EndsWith(SeparatorChar))
                path += SeparatorChar;

            _archive.CreateEntry(path);
        }

        public void DeleteDirectory(string path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_mode != ZipArchiveMode.Update)
                throw new NotSupportedException("当前ZipPack实例不支持写入操作");

            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            DirectoryNode? directoryNode = _rootNode.GetDirectoryNode(path);
            if (directoryNode is null)
                return;
            if (directoryNode.Count > 0)
                throw new InvalidOperationException("目录不为空，无法删除");

            path = directoryNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar);
            ZipArchiveEntry? entry = _archive.GetEntry(path);

            entry?.Delete();
            directoryNode.Delete();
        }

        public void WriteFile(string path, byte[] bytes)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_mode != ZipArchiveMode.Update)
                throw new NotSupportedException("当前ZipPack实例不支持写入操作");

            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
            if (_rootNode.DirectoryNodeExists(path))
                throw new InvalidOperationException($"路径“{path}”已存在目录，无法创建文件");

            FileNode fileNode = _rootNode.CreateFileNode(path);
            string fullPath = fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar);
            try
            {
                ZipArchiveEntry? entry = _archive.GetEntry(fullPath);
                entry?.Delete();
                entry = _archive.CreateEntry(fullPath);

                using Stream stream = entry.Open();
                stream.Write(bytes);
            }
            catch
            {
                try
                {
                    fileNode.Delete();
                    ZipArchiveEntry? entry = _archive.GetEntry(fullPath);
                    entry?.Delete();
                }
                catch
                {
                    //仅尝试回滚，不处理异常
                }
                throw;
            }
        }

        public void WriteFile(string path, Stream inputStream)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_mode != ZipArchiveMode.Update)
                throw new NotSupportedException("当前ZipPack实例不支持写入操作");

            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            ArgumentNullException.ThrowIfNull(inputStream, nameof(inputStream));
            ThrowHelper.StreamNotSupportRead(inputStream);
            if (_rootNode.DirectoryNodeExists(path))
                throw new InvalidOperationException($"路径“{path}”已存在目录，无法创建文件");

            FileNode fileNode = _rootNode.CreateFileNode(path);
            string fullPath = fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar);
            try
            {
                ZipArchiveEntry? entry = _archive.GetEntry(fullPath);
                entry?.Delete();
                entry = _archive.CreateEntry(fullPath);

                using Stream entryStream = entry.Open();
                inputStream.CopyTo(entryStream);
            }
            catch
            {
                try
                {
                    fileNode.Delete();
                    ZipArchiveEntry? entry = _archive.GetEntry(fullPath);
                    entry?.Delete();
                }
                catch
                {
                    //仅尝试回滚，不处理异常
                }
                throw;
            }
        }

        public void DeleteFile(string path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_mode != ZipArchiveMode.Update)
                throw new NotSupportedException("当前ZipPack实例不支持写入操作");

            FileNode? fileNode = _rootNode.GetFileNode(path);
            if (fileNode is null)
                return;

            string fullPath = fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar);
            ZipArchiveEntry? entry = _archive.GetEntry(fullPath);
            if (entry is null)
                return;

            fileNode.Delete();
            entry.Delete();
        }

        public ZipItem ReadZipItem(string path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            FileNode? fileNode = _rootNode.GetFileNode(path)
                ?? throw new FileNotFoundException($"未找到路径“{path}”的文件");

            string fullPath = fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar);
            ZipArchiveEntry? entry = _archive.GetEntry(fullPath)
                ?? throw new FileNotFoundException($"未找到路径“{fullPath}”的文件");

            return new ZipItem(entry);
        }

        public bool TryReadZipItem([NotNullWhen(true)] string? path, [MaybeNullWhen(false)] out ZipItem result)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (string.IsNullOrEmpty(path))
                goto failed;

            FileNode? fileNode = _rootNode.GetFileNode(path);
            if (fileNode is null)
                goto failed;

            string fullPath = fileNode.GetFullName(SeparatorChar).TrimStart(SeparatorChar);
            ZipArchiveEntry? entry;
            try
            {
                entry = _archive.GetEntry(fullPath);
            }
            catch
            {
                goto failed;
            }

            if (entry is null)
                goto failed;

            result = new ZipItem(entry);
            return true;

            failed:
            result = null;
            return false;
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
            if (_disposed)
                return;

            _disposed = true;
            _archive.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
