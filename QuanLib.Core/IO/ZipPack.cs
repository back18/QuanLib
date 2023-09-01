using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    public class ZipPack : IDisposable
    {
        public ZipPack(string path)
        {
            _archive = ZipFile.OpenRead(path);
            _entrys = new();
            foreach (var entrie in _archive.Entries)
                _entrys.Add(entrie.FullName, entrie);
        }

        private const char SEPARATOR = '/';

        private readonly ZipArchive _archive;

        private readonly Dictionary<string, ZipArchiveEntry> _entrys;

        public ReadOnlyCollection<ZipArchiveEntry> Entries => _archive.Entries;

        public ZipArchiveEntry? GetEntry(string path) => _archive.GetEntry(path);

        public bool ExistsFile(string? path)
        {
            if (path is null)
                return false;

            return _entrys.ContainsKey(path);
        }

        public bool ExistsDirectory(string? path)
        {
            if (path is null)
                return false;

            if (!path.EndsWith(SEPARATOR))
                path += SEPARATOR;

            foreach (var entry in _entrys)
            {
                if (entry.Key.StartsWith(path))
                    return true;
            }

            return false;
        }

        public ZipArchiveEntry[] GetFiles()
        {
            List<ZipArchiveEntry> result = new();
            foreach (var entry in _entrys)
            {
                if (!entry.Key.Contains(SEPARATOR))
                    result.Add(entry.Value);
            }

            return result.ToArray();
        }

        public ZipArchiveEntry[] GetFiles(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException($"“{nameof(path)}”不能为 null 或空。", nameof(path));

            if (!path.EndsWith(SEPARATOR))
                path += SEPARATOR;

            List<ZipArchiveEntry> result = new();
            foreach (var entry in _entrys)
            {
                if (entry.Key.StartsWith(path))
                {
                    string right = entry.Key[path.Length..];
                    if (!string.IsNullOrEmpty(right) && !right.Contains(SEPARATOR))
                        result.Add(entry.Value);
                }
            }

            return result.ToArray();
        }

        public string[] GetDirectorys()
        {
            List<string> result = new();
            foreach (var entry in _entrys)
            {
                string[] items = entry.Key.Split(SEPARATOR);
                if (items.Length > 1 && !result.Contains(items[0]))
                    result.Add(items[0]);
            }

            return result.ToArray();
        }

        public string[] GetDirectorys(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException($"“{nameof(path)}”不能为 null 或空。", nameof(path));

            if (!path.EndsWith(SEPARATOR))
                path += SEPARATOR;

            HashSet<string> result = new();
            foreach (var entry in _entrys)
            {
                if (entry.Key.StartsWith(path))
                {
                    string right = entry.Key[path.Length..];
                    string[] items = right.Split(SEPARATOR);
                    if (items.Length > 1 && !result.Contains(items[0]))
                        result.Add(items[0]);
                }
            }

            return result.ToArray();
        }

        public void Dispose()
        {
            _archive.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
