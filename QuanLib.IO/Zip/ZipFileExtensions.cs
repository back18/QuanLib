using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace QuanLib.IO.Zip
{
    public static class ZipFileExtensions
    {
        extension (ZipArchiveEntry entry)
        {
            public Stream OpenStream()
            {
                return entry.Open();
            }

            public byte[] ReadAllBytes()
            {
                using Stream stream = entry.Open();
                byte[] result = new byte[stream.Length];
                stream.ReadExactly(result, 0, result.Length);
                return result;
            }

            public string ReadAllText()
            {
                using Stream stream = entry.Open();
                using StreamReader reader = new(stream);
                return reader.ReadToEnd();
            }

            public string ReadAllText(Encoding encoding)
            {
                ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

                using Stream stream = entry.Open();
                using StreamReader reader = new(stream, encoding);
                return reader.ReadToEnd();
            }

            public string[] ReadAllLines()
            {
                using Stream stream = entry.Open();
                using StreamReader reader = new(stream);

                List<string> lines = [];
                while (reader.ReadLine() is string line)
                    lines.Add(line);

                return lines.ToArray();
            }

            public string[] ReadAllLines(Encoding encoding)
            {
                ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

                using Stream stream = entry.Open();
                using StreamReader reader = new(stream, encoding);

                List<string> lines = [];
                while (reader.ReadLine() is string line)
                    lines.Add(line);

                return lines.ToArray();
            }

            public async Task<byte[]> ReadAllBytesAsync()
            {
                using Stream stream = entry.Open();
                byte[] result = new byte[stream.Length];
                await stream.ReadExactlyAsync(result, 0, result.Length);
                return result;
            }

            public async Task<string> ReadAllTextAsync()
            {
                using Stream stream = entry.Open();
                using StreamReader reader = new(stream);
                return await reader.ReadToEndAsync();
            }

            public async Task<string> ReadAllTextAsync(Encoding encoding)
            {
                ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

                using Stream stream = entry.Open();
                using StreamReader reader = new(stream, encoding);
                return await reader.ReadToEndAsync();
            }

            public async Task<string[]> ReadAllLinesAsync()
            {
                using Stream stream = entry.Open();
                using StreamReader reader = new(stream);

                List<string> lines = [];
                while ((await reader.ReadLineAsync().ConfigureAwait(false)) is string line)
                    lines.Add(line);

                return lines.ToArray();
            }

            public async Task<string[]> ReadAllLinesAsync(Encoding encoding)
            {
                ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

                using Stream stream = entry.Open();
                using StreamReader reader = new(stream, encoding);

                List<string> lines = [];
                while ((await reader.ReadLineAsync().ConfigureAwait(false)) is string line)
                    lines.Add(line);

                return lines.ToArray();
            }
        }
    }
}
