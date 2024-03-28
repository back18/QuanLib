using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Extensions
{
    public static class StreamExtensions
    {
        public static string ToUtf8Text(this Stream stream)
        {
            return stream.ToText(Encoding.UTF8);
        }

        public static string[] ToUtf8TextLines(this Stream stream)
        {
            return stream.ToTextLines(Encoding.UTF8);
        }

        public static string ToText(this Stream stream, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            if (stream.CanSeek && stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);

            using StreamReader reader = new(stream, encoding);
            return reader.ReadToEnd();
        }

        public static string[] ToTextLines(this Stream stream, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

            if (stream.CanSeek && stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);

            using StreamReader reader = new(stream, Encoding.UTF8);
            List<string> lines = new();
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
