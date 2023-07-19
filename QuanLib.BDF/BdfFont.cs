using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.BDF
{
    public class BdfFont : IReadOnlyDictionary<char, FontData>
    {
        private BdfFont(ConcurrentDictionary<char, FontData> fonts)
        {
            _fonts = fonts ?? throw new ArgumentNullException(nameof(fonts));

            Dictionary<int, int> widths = new();
            Dictionary<int, int> heights = new();
            foreach (var font in _fonts.Values)
            {
                widths.TryAdd(font.Width, 0);
                heights.TryAdd(font.Height, 0);
                widths[font.Width]++;
                heights[font.Height]++;
            }

            List<KeyValuePair<int, int>> widthlist = new();
            List<KeyValuePair<int, int>> heightlist = new();
            foreach (var width in widths)
                widthlist.Add(new(width.Key, width.Value));
            foreach (var height in heights)
                heightlist.Add(new(height.Key, height.Value));
            widthlist = widthlist.OrderByDescending(w => w.Value).ToList();
            heightlist = heightlist.OrderByDescending(h => h.Value).ToList();
            if (widths.Count == 0)
            {
                HalfWidth = 0;
                FullWidth = 0;
            }
            else if (widthlist.Count == 1)
            {
                HalfWidth = widthlist[0].Key;
                FullWidth = widthlist[0].Key;
            }
            else
            {
                if (widthlist[0].Key < widthlist[1].Key)
                {
                    HalfWidth = widthlist[0].Key;
                    FullWidth = widthlist[1].Key;
                }
                else
                {
                    FullWidth = widthlist[0].Key;
                    HalfWidth = widthlist[1].Key;
                }
            }

            Height = heightlist.Count > 0 ? heightlist[0].Key : 0;
        }

        private readonly ConcurrentDictionary<char, FontData> _fonts;

        public int HalfWidth { get; }

        public int FullWidth { get; }

        public int Height { get; }

        public FontData this[char key] => _fonts[key];

        public IEnumerable<char> Keys => _fonts.Keys;

        public IEnumerable<FontData> Values => _fonts.Values;

        public int Count => _fonts.Count;

        public bool ContainsKey(char key)
        {
            return _fonts.ContainsKey(key);
        }

        public bool TryGetValue(char key, [MaybeNullWhen(false)] out FontData value)
        {
            return _fonts.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<char, FontData>> GetEnumerator()
        {
            return _fonts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_fonts).GetEnumerator();
        }

        public Size GetTotalSize(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            int width = 0;
            int height = 0;
            foreach (var c in value)
            {
                width += _fonts[c].Width;
                if (_fonts[c].Height > height)
                    height = _fonts[c].Height;
            }
            return new(width, height);
        }

        public int GetLeftLayoutMaxCount(int maxWidth, string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            int width = 0;
            for (int i = 0; i < value.Length; i++)
            {
                width += _fonts[value[i]].Width;
                if (width == maxWidth)
                    return i + 1;
                else if (width > maxWidth)
                    return i;
            }
            return width;
        }

        public int GetRightLayoutMaxCount(int maxWidth, string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            int width = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                width += _fonts[value[i]].Width;
                if (width == maxWidth)
                    return value.Length - i + 1;
                else if (width > maxWidth)
                    return value.Length - i;
            }
            return width;
        }

        public int GetTopLayoutMaxCount(int maxHeight, string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            int height = 0;
            for (int i = 0; i < value.Length; i++)
            {
                height += _fonts[value[i]].Height;
                if (height == maxHeight)
                    return i + 1;
                else if (height > maxHeight)
                    return i;
            }
            return height;
        }

        public int GetBottomLayoutMaxCount(int maxHeight, string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            int height = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                height += _fonts[value[i]].Height;
                if (height == maxHeight)
                    return value.Length - i + 1;
                else if (height > maxHeight)
                    return value.Length - i;
            }
            return height;
        }

        public static BdfFont Load(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            string[] lines = File.ReadAllLines(path);

            ConcurrentDictionary<char, FontData> fonts = new();
            List<(int start, int end)> ranges = new();
            int start = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith('S') && lines[i].StartsWith("STARTCHAR"))
                    start = i + 1;
                else if (lines[i].StartsWith('E') && lines[i].StartsWith("ENDCHAR"))
                    ranges.Add((start, i - 1));
            }

            int count = 0;
            Parallel.ForEach(ranges, range =>
            {
                FontData font = Parse(range.start, range.end);
                while (!fonts.TryAdd(font.Char, font)) ;
                Interlocked.Increment(ref count);
            });

            while (count < ranges.Count)
                Thread.Sleep(10);

            return new(fonts);

            FontData Parse(int start, int end)
            {
                bool readmap = false;
                int mapindex = 0;

                char @char = default;
                int width = default;
                int height = default;
                int xOffset = default;
                int yOffset = default;
                bool[][]? data = default;
                for (int i = start; i <= end; i++)
                {
                    if (readmap)
                    {
                        int count = lines[i].Length / 2;
                        data![mapindex] = new bool[count * 8];
                        for (int j = 0; j < count; j++)
                            BoolArrayMap.FromUpper(lines[i].Substring(j * 2, 2)).CopyTo(data[mapindex], j * 8);
                        mapindex++;
                    }
                    else if (lines[i].StartsWith("ENCODING"))
                    {
                        @char = Convert.ToChar(uint.Parse(lines[i][9..]));
                    }
                    else if (lines[i].StartsWith("BBX"))
                    {
                        string[] items = lines[i].Split(' ');
                        width = Convert.ToInt32(items[1]);
                        height = Convert.ToInt32(items[2]);
                        xOffset = Convert.ToInt32(items[3]);
                        yOffset = Convert.ToInt32(items[4]);
                    }
                    else if (lines[i].StartsWith("BITMAP"))
                    {
                        readmap = true;
                        data = new bool[end - i][];
                    }
                }

                return new(@char, width, height, xOffset, yOffset, data ?? Array.Empty<bool[]>());
            }
        }
    }
}
