using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    public class ConsoleComboBox
    {
        public ConsoleComboBox(IEnumerable<string> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            Text = string.Empty;
            Prompt = string.Empty;
            _DisplayCount = 10;

            _alls = new(items);
            _items = new(items);
            _range = new(_items.Count, 10);
        }

        private readonly List<string> _alls;

        private List<string> _items;

        private DisplayRange _range;

        public int DisplayCount
        {
            get => _DisplayCount;
            set
            {
                _DisplayCount = value;
                _range = new(_items.Count, value);
            }
        }
        private int _DisplayCount;

        public int ActualShowCount => _range.ActualShowCount;

        public int DisplaySelectIndex => _range.ShowSelectIndex;

        public string? SelectItem
        {
            get
            {
                if (_range.SelectIndex >= 0)
                    return _items[_range.SelectIndex];
                return null;
            }
        }

        public string Text { get; private set; }

        public string Prompt { get; private set; }

        public void Prev() => _range.Prev();

        public void Next() => _range.Next();

        public void Update(string text)
        {
            if (text == Text && text != string.Empty)
                return;

            Text = text ?? throw new ArgumentNullException(nameof(text));
            _items = new(
                from item in _alls
                where item.StartsWith(text)
                select item);

            if (_range.ItemsCount != _items.Count)
                _range = new(_items.Count, _range.DisplayCount);

            if (SelectItem is not null)
                Prompt = SelectItem[Text.Length..];
            else
                Prompt = string.Empty;
        }

        public string[] GetShowItems()
        {
            if (_items.Count == 0)
                return Array.Empty<string>();

            return _items.GetRange(_range.StartIndex, _range.ActualShowCount).ToArray();
        }

        private class DisplayRange
        {
            public DisplayRange(int itemsCount, int displayCount)
            {
                ItemsCount = itemsCount;
                DisplayCount = displayCount;
                ActualShowCount = itemsCount < displayCount ? itemsCount : displayCount;
                EndIndex = ActualShowCount - 1;
                if (ActualShowCount > 0)
                {
                    StartIndex = 0;
                    SelectIndex = 0;
                }
                else
                {
                    StartIndex = -1;
                    SelectIndex = -1;
                }
            }

            public int ItemsCount { get; }

            public int DisplayCount { get; }

            public int ActualShowCount { get; }

            public int StartIndex { get; private set; }

            public int EndIndex { get; private set; }

            public int SelectIndex { get; private set; }

            public int ShowSelectIndex => ActualShowCount < 0 ? -1 : SelectIndex - StartIndex;

            public void Prev()
            {
                if (SelectIndex > StartIndex)
                {
                    SelectIndex--;
                    if (StartIndex > 0)
                    {
                        StartIndex--;
                        EndIndex--;
                    }
                }
            }

            public void Next()
            {
                if (SelectIndex < EndIndex)
                {
                    SelectIndex++;
                    if (EndIndex < ActualShowCount - 1)
                    {
                        StartIndex++;
                        EndIndex++;
                    }
                }
            }
        }
    }
}
