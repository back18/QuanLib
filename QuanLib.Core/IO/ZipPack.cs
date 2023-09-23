using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    public class ZipPack : IReadOnlyDictionary<string, ZipArchiveEntry>, IDisposable
    {
        public ZipPack(string path, Encoding? encoding = null)
        {
            _archive = ZipFile.Open(path, ZipArchiveMode.Read, encoding);
            _items = new();
            foreach (var entrie in _archive.Entries)
                _items.Add(entrie.FullName, entrie);
        }

        public ZipPack(Stream stream)
        {
            _archive = new ZipArchive(stream);
            _items = new();
            foreach (var entrie in _archive.Entries)
                _items.Add(entrie.FullName, entrie);
        }

        private const char SEPARATOR = '/';

        private readonly ZipArchive _archive;

        private readonly Dictionary<string, ZipArchiveEntry> _items;

        public ReadOnlyCollection<ZipArchiveEntry> Entries => _archive.Entries;

        public IEnumerable<string> Keys => _items.Keys;

        public IEnumerable<ZipArchiveEntry> Values => _items.Values;

        public int Count => _items.Count;

        public ZipArchiveEntry this[string key] => _items[key];

        public bool ExistsFile(string? path)
        {
            if (path is null)
                return false;

            return _items.ContainsKey(path);
        }

        public bool ExistsDirectory(string? path)
        {
            if (path is null)
                return false;

            if (!path.EndsWith(SEPARATOR))
                path += SEPARATOR;

            foreach (var entry in _items)
            {
                if (entry.Key.StartsWith(path))
                    return true;
            }

            return false;
        }

        public ZipArchiveEntry[] GetFiles()
        {
            List<ZipArchiveEntry> result = new();
            foreach (var entry in _items)
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
            foreach (var entry in _items)
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
            foreach (var entry in _items)
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
            foreach (var entry in _items)
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

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out ZipArchiveEntry value)
        {
            return _items.TryGetValue(key, out value);
        }

        public bool ContainsKey(string key)
        {
            return _items.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, ZipArchiveEntry>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }

        public void Dispose()
        {
            _archive.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
