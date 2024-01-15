using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.Zip
{
    public class ZipItem
    {
        public ZipItem(ZipArchiveEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry, nameof(entry));

            _entry = entry;
        }

        private readonly ZipArchiveEntry _entry;

        public long Length => _entry.Length;

        public string Name => _entry.Name;

        public string FullName => _entry.FullName;

        public Stream OpenStream()
        {
            return _entry.Open();
        }

        public byte[] ReadAllBytes()
        {
            using Stream stream = _entry.Open();
            byte[] result = new byte[stream.Length];
            stream.Read(result, 0, result.Length);
            return result;
        }

        public string ReadAllText(Encoding encoding)
        {
            using Stream stream = _entry.Open();
            using StreamReader reader = new(stream, encoding);
            return reader.ReadToEnd();
        }

        public string ReadAllText()
        {
            return ReadAllText(Encoding.UTF8);
        }
    }
}
