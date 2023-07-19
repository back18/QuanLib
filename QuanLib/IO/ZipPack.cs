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
            archive = ZipFile.OpenRead(path);
            entrys = new();
            foreach (var entrie in archive.Entries)
                entrys.Add(entrie.FullName, entrie);
        }

        private readonly ZipArchive archive;

        private readonly Dictionary<string, ZipArchiveEntry> entrys;

        public ReadOnlyCollection<ZipArchiveEntry> Entries => archive.Entries;

        public ZipArchiveEntry? GetEntry(string path) => archive.GetEntry(path);

        public bool ExistsEntry(string? path)
        {
            if (path is null)
                return false;

            return entrys.ContainsKey(path);
        }

        public bool ExistsDirectory(string? path)
        {
            if (path is null)
                return false;

            if (path[^1] != '/')
                path += '/';

            return entrys.ContainsKey(path);
        }

        public ZipArchiveEntry[] GetEntrys(string path)
        {
            if (path is null)
                return Array.Empty<ZipArchiveEntry>();

            if (path[^1] != '/')
                path += '/';

            List<ZipArchiveEntry> result = new();
            foreach (var entry in entrys)
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
            archive.Dispose();
            GC.SuppressFinalize(this);
            throw new NotImplementedException();
        }
    }
}
