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
            public string ReadAllText()
            {
                return source.ReadAllText(Encoding.UTF8);
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
                return source.ReadAllLines(Encoding.UTF8);
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

            public Task<string> ReadAllTextAsync()
            {
                return source.ReadAllTextAsync(Encoding.UTF8); 
            }

            public Task<string> ReadAllTextAsync(Encoding encoding)
            {
                ArgumentNullException.ThrowIfNull(encoding, nameof(encoding));

                ResetStreamPosition(source);
                using StreamReader reader = new(source, encoding);
                return reader.ReadToEndAsync();
            }

            public Task<string[]> ReadAllLinesAsync()
            {
                return source.ReadAllLinesAsync(Encoding.UTF8);
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
