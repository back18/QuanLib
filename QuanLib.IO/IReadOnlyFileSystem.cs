using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuanLib.IO
{
    public interface IReadOnlyFileSystem
    {
        public char PathSeparator { get; }

        public bool DirectoryExists([NotNullWhen(true)] string? path);

        public bool FileExists([NotNullWhen(true)] string? path);

        public string[] GetDirectoryPaths(string path);

        public string[] GetFilePaths(string path);

        public Stream ReadFile(string path);

        public bool TryReadFile([NotNullWhen(true)] string? path, [MaybeNullWhen(false)] out Stream outputStream);
    }
}
