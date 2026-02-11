using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.IO.Extensions
{
    public static class FileSystemInfoExtensions
    {
        extension(FileSystemInfo source)
        {
            public void DeleteIfExists()
            {
                if (source.Exists)
                    source.Delete();
            }
        }
    }
}
