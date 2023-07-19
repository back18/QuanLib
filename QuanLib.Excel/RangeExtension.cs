using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using ME = Microsoft.Office.Interop.Excel;

namespace QuanLib.Excel
{
    public static class RangeExtension
    {
        public static int GetRowsCount(this ME.Range range) => range.Rows.Count;

        public static int GetColumnsCount(this ME.Range range) => range.Columns.Count;

        public static int GetCellCount(this ME.Range range) => range.GetRowsCount() * range.GetColumnsCount();

        public static int GetHyperlinkCount(this ME.Range range) => range.Hyperlinks.Count;

        public static int GetValueCount(this ME.Range range)
        {
            if (range.Value is null)
                return 0;

            int count = 0;
            foreach (var item in range.Value)
                if (item is not null)
                    count++;
            return count;
        }

        public static bool IsOneRow(this ME.Range range) => range.GetRowsCount() == 1;

        public static bool IsOneColumn(this ME.Range range) => range.GetColumnsCount() == 1;

        public static bool IsOneCell(this ME.Range range) => range.GetRowsCount() == 1 && range.GetColumnsCount() == 1;

        public static string? GetValue(this ME.Range range) => range.Value.ToString();

        public static bool TryGetValue(this ME.Range range, [MaybeNullWhen(false)] out string value)
        {
            if (ContainsValue(range))
            {
                value = range.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public static bool ContainsValue(this ME.Range range) => range.Value is not null;

        public static void SetValue(this ME.Range range, string value) => range.Value = value;

        public static Hyperlink GetHyperlink(this ME.Range range)
        {
            foreach (var item in range.Hyperlinks)
                return (Hyperlink)item;

            throw new InvalidOperationException();
        }

        public static bool TryGetHyperlink(this ME.Range range, [MaybeNullWhen(false)] out Hyperlink hyperlink)
        {
            if (range.ContainsHyperlink())
            {
                hyperlink = range.GetHyperlink();
                return true;
            }
            else
            {
                hyperlink = default;
                return false;
            }
        }

        public static bool ContainsHyperlink(this ME.Range range) => range.Hyperlinks.Count == range.GetCellCount();

        public static void SetHyperlink(this ME.Range range, string address, string? text = null, string? screenTip = null)
        {
            string s = string.Empty;
            if (text is null)
                s += "0";
            else s += "1";
            if (screenTip is null)
                s += "0";
            else s += "1";

            switch (s)
            {
                case "00":
                    range.Hyperlinks.Add(range, address);
                    break;
                case "10":
                    range.Hyperlinks.Add(range, address, TextToDisplay: text);
                    break;
                case "01":
                    range.Hyperlinks.Add(range, address, ScreenTip: screenTip);
                    break;
                case "11":
                    range.Hyperlinks.Add(range, address, TextToDisplay: text, ScreenTip: screenTip);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static void FillValue(this ME.Range range, string value)
        {
            foreach (ME.Range cell in range.Cells)
                cell.Value = value;
        }
    }
}
