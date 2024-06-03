using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class FileList : IReadOnlyList<string>
    {
        private FileList(string directory, List<string> files, int index)
        {
            Directory = directory;
            _files = files;
            CurrentIndex = index;
        }

        private readonly List<string> _files;

        public string this[int index] => _files[index];

        public int Count => _files.Count;

        public int CurrentIndex { get; private set; }

        public string? CurrentFile
        {
            get
            {
                if (_files.Count == 0)
                    return null;
                return _files[CurrentIndex];
            }
        }

        public string Directory { get; }

        public bool Contains(string item)
        {
            return _files.Contains(item);
        }

        public string? GetPrevious()
        {
            if (_files.Count == 0)
                return null;

            CurrentIndex--;
            if (CurrentIndex < 0)
                CurrentIndex = _files.Count - 1;

            return _files[CurrentIndex];
        }

        public string? GetNext()
        {
            if (_files.Count == 0)
                return null;

            CurrentIndex++;
            if (CurrentIndex >= _files.Count)
                CurrentIndex = 0;

            return _files[CurrentIndex];
        }

        public static FileList LoadDirectory(string directory, IEnumerable<string> extensions)
        {
            ArgumentException.ThrowIfNullOrEmpty(directory, nameof(directory));
            ThrowHelper.DirectoryNotFound(directory);

            string[] items = System.IO.Directory.GetFiles(directory);
            List<string> extensionList = new(extensions);
            List<string> files = [];

            foreach (string item in items)
            {
                if (extensionList.Contains(Path.GetExtension(item).TrimStart('.')))
                    files.Add(item);
            }

            int index;
            if (files.Count > 0)
                index = 0;
            else
                index = -1;

            return new(directory, files, index);
        }

        public static FileList LoadFile(string file, IEnumerable<string> extensions)
        {
            ArgumentException.ThrowIfNullOrEmpty(file, nameof(file));
            ThrowHelper.FileNotFound(file);

            string directory = Path.GetDirectoryName(file) ?? throw new InvalidOperationException();
            string[] items = System.IO.Directory.GetFiles(directory);
            List<string> extensionList = new(extensions);
            List<string> files = new();

            foreach (string item in items)
            {
                if (extensionList.Contains(Path.GetExtension(item).TrimStart('.')))
                    files.Add(item);
            }

            int index = files.IndexOf(file);

            return new(directory, files, index);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _files.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_files).GetEnumerator();
        }
    }
}
