using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Extensions
{
    public static class StreamExtensions
    {
        public static string ReadAllText(this Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            return stream.ReadAllText(Encoding.UTF8);
        }

        public static string ReadAllText(this Stream stream, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            if (stream.CanSeek && stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);

            using StreamReader reader = new(stream, encoding);
            return reader.ReadToEnd();
        }

        public static string[] ReadAllLines(this Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            return stream.ReadAllLines(Encoding.UTF8);
        }

        public static string[] ReadAllLines(this Stream stream, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            if (stream.CanSeek && stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);

            using StreamReader reader = new(stream, Encoding.UTF8);
            List<string> lines = [];
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line is not null)
                    lines.Add(line);
            }
            return lines.ToArray();
        }
    }
}
