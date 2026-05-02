using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QuanLib.IO.Extensions
{
    public static class StreamExtensions
    {
        extension(Stream source)
        {
            public byte[] ReadAllBytes()
            {
                ResetStreamPosition(source);
                byte[] result = new byte[source.Length];
                source.ReadExactly(result, 0, result.Length);
                return result;
            }

            public string ReadAllText()
            {
                ResetStreamPosition(source);
                using StreamReader reader = new(source);
                return reader.ReadToEnd();
            }

            public string ReadAllText(Encoding encoding)
            {
                ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

                ResetStreamPosition(source);
                using StreamReader reader = new(source, encoding);
                return reader.ReadToEnd();
            }

            public string[] ReadAllLines()
            {
                ResetStreamPosition(source);
                using StreamReader reader = new(source);
                List<string> lines = [];
                while (reader.ReadLine() is string line)
                    lines.Add(line);

                return lines.ToArray();
            }

            public string[] ReadAllLines(Encoding encoding)
            {
                ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

                ResetStreamPosition(source);
                using StreamReader reader = new(source, encoding);
                List<string> lines = [];
                while (reader.ReadLine() is string line)
                    lines.Add(line);

                return lines.ToArray();
            }

            public async Task<byte[]> ReadAllBytesAsync()
            {
                ResetStreamPosition(source);
                byte[] result = new byte[source.Length];
                await source.ReadExactlyAsync(result, 0, result.Length);
                return result;
            }

            public async Task<string> ReadAllTextAsync()
            {
                ResetStreamPosition(source);
                using StreamReader reader = new(source);
                return await reader.ReadToEndAsync();
            }

            public async Task<string> ReadAllTextAsync(Encoding encoding)
            {
                ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

                ResetStreamPosition(source);
                using StreamReader reader = new(source, encoding);
                return await reader.ReadToEndAsync();
            }

            public async Task<string[]> ReadAllLinesAsync()
            {
                ResetStreamPosition(source);
                using StreamReader reader = new(source);
                List<string> lines = [];
                while ((await reader.ReadLineAsync().ConfigureAwait(false)) is string line)
                    lines.Add(line);

                return lines.ToArray();
            }

            public async Task<string[]> ReadAllLinesAsync(Encoding encoding)
            {
                ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

                ResetStreamPosition(source);
                using StreamReader reader = new(source, encoding);
                List<string> lines = [];
                while ((await reader.ReadLineAsync().ConfigureAwait(false)) is string line)
                    lines.Add(line);

                return lines.ToArray();
            }
        }

        private static void ResetStreamPosition(Stream stream)
        {
            if (stream.CanSeek && stream.Position != 0)
                stream.Seek(0, SeekOrigin.Begin);
        }
    }
}
