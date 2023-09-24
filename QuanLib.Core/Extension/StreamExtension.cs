using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Extension
{
    public static class StreamExtension
    {
        public static string ToUtf8Text(this Stream stream)
        {
            return ToText(stream, Encoding.UTF8);
        }

        public static string[] ToUtf8TextLines(this Stream stream)
        {
            return ToTextLines(stream, Encoding.UTF8);
        }

        public static string ToText(this Stream stream, Encoding encoding)
        {
            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));

            if (stream.CanSeek && stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);

            using StreamReader reader = new(stream, encoding);
            return reader.ReadToEnd();
        }

        public static string[] ToTextLines(this Stream stream, Encoding encoding)
        {
            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));

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
