using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Extensions
{
    public static class StreamExtension
    {
        public static string ToUtf8Text(this Stream stream)
        {
            using StreamReader reader = new(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static string[] ToUtf8TextLines(this Stream stream)
        {
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
