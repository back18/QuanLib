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
    public class DirectoryManager
    {
        public DirectoryManager(string directory)
        {
            ArgumentException.ThrowIfNullOrEmpty(directory, nameof(directory));

            FullPath = Path.GetFullPath(directory);
            DictionaryName = Path.GetFileName(directory);

            _items = new();
        }

        private readonly Dictionary<string, DirectoryManager> _items;

        public string FullPath { get; }

        public string DictionaryName { get; }

        public DirectoryManager this[string name] => _items[name];

        public T AddDirectory<T>(string name) where T : DirectoryManager
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

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
            ArgumentNullException.ThrowIfNull(path, nameof(path));

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
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            return File.Exists(Combine(name));
        }

        public bool ExistsDirectory(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

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
    }
}
