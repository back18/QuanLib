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

        public static void CopyTo(this Stream stream, Stream destination, long position, long length)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            ArgumentNullException.ThrowIfNull(destination, nameof(destination));

            if (!stream.CanSeek)
                throw new ArgumentException("流不支持查找", nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("流不支持读取", nameof(stream));
            if (!destination.CanWrite)
                throw new ArgumentException("目标流不支持写入", nameof(destination));

            ThrowHelper.ArgumentOutOfRange(0, stream.Length - 1, position, nameof(position));
            ThrowHelper.ArgumentOutOfRange(0, stream.Length - position, length, nameof(length));

            if (stream.Position != position)
                stream.Seek(position, SeekOrigin.Begin);

            long total = 0;
            byte[] buffer = new byte[1];
            do
            {
                int read = stream.Read(buffer, 0, (int)Math.Min(buffer.Length, length - total));
                if (read <= 0)
                    break;
                destination.Write(buffer, 0, read);
                total += read;
            } while (total < length);
        }
    }
}
