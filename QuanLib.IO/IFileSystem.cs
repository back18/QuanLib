using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.IO
{
    public interface IFileSystem : IReadOnlyFileSystem
    {
        public void CreateDirectory(string path);

        public void DeleteDirectory(string path);

        public void WriteFile(string path, byte[] bytes);

        public void WriteFile(string path, Stream inputStream);

        public void DeleteFile(string path);
    }
}
