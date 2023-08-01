using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
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

        private readonly ZipArchive _archive;

        private readonly Dictionary<string, ZipArchiveEntry> _entrys;

        public ReadOnlyCollection<ZipArchiveEntry> Entries => _archive.Entries;

        public ZipArchiveEntry? GetEntry(string path) => _archive.GetEntry(path);

        public bool ExistsEntry(string? path)
        {
            if (path is null)
                return false;

            return _entrys.ContainsKey(path);
        }

        public bool ExistsDirectory(string? path)
        {
            if (path is null)
                return false;

            if (path[^1] != '/')
                path += '/';

            return _entrys.ContainsKey(path);
        }

        public ZipArchiveEntry[] GetEntrys(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException($"“{nameof(path)}”不能为 null 或空。", nameof(path));

            if (path[^1] != '/')
                path += '/';

            List<ZipArchiveEntry> result = new();
            foreach (var entry in _entrys)
            {
                if (entry.Key.StartsWith(path))
                {
                    string sub = entry.Key[path.Length..];
                    if (!string.IsNullOrEmpty(sub) && !sub.Contains('/'))
                        result.Add(entry.Value);
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
