using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    public class DirectoryManager : IReadOnlyDictionary<string, DirectoryManager>
    {
        public DirectoryManager(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException($"“{nameof(directory)}”不能为 null 或空。", nameof(directory));

            FullPath = Path.GetFullPath(directory);
            DictionaryName = Path.GetFileName(directory);

            _items = new();
        }

        private readonly Dictionary<string, DirectoryManager> _items;

        public string FullPath { get; }

        public string DictionaryName { get; }

        public IEnumerable<string> Keys => _items.Keys;

        public IEnumerable<DirectoryManager> Values => _items.Values;

        public int Count => _items.Count;

        public DirectoryManager this[string key] => _items[key];

        public T AddDirectory<T>(string name) where T : DirectoryManager
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));

            Type type = typeof(T);
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
                {
                    object instance = constructor.Invoke(new object[] { Combine(name) });
                    T directory = (T)instance;
                    _items.Add(directory.DictionaryName, directory);
                    return directory;
                }
            }

            throw new ArgumentException("找不到合适的构造函数");
        }

        public string Combine(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            return Path.Combine(FullPath, path);
        }

        public bool Exists()
        {
            return Directory.Exists(FullPath);
        }

        public void CreateIfNotExists()
        {
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);
        }

        public void Delete()
        {
            Directory.Delete(FullPath);
        }

        public void Delete(bool recursive)
        {
            Directory.Delete(FullPath, recursive);
        }

        public bool ExistsFile(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));

            return File.Exists(Combine(name));
        }

        public bool ExistsDirectory(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));

            return Directory.Exists(Combine(name));
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(FullPath);
        }

        public string[] GetFiles(string searchPattern)
        {
            return Directory.GetFiles(FullPath, searchPattern);
        }

        public string[] GetDirectorys()
        {
            return Directory.GetDirectories(FullPath);
        }

        public string[] GetDirectorys(string searchPattern)
        {
            return Directory.GetDirectories(FullPath, searchPattern);
        }

        public void BuildDirectoryTree()
        {
            CreateIfNotExists();
            foreach (DirectoryManager directory in _items.Values)
                directory.BuildDirectoryTree();
        }

        public override string ToString()
        {
            return FullPath;
        }

        public bool ContainsKey(string key)
        {
            return _items.ContainsKey(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out DirectoryManager value)
        {
            return _items.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, DirectoryManager>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }
    }
}
