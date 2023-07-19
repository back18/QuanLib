using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using ME = Microsoft.Office.Interop.Excel;

namespace QuanLib.Excel
{
    public static class WorksheetExtension
    {
        public static int GetRowsCount(this Worksheet worksheet) => worksheet.Rows.Count;

        public static int GetColumnsCount(this Worksheet worksheet) => worksheet.Columns.Count;

        public static int GetUsedColumnsCount(this Worksheet worksheet) => worksheet.UsedRange.Columns.Count;

        public static int GetUsedRowsCount(this Worksheet worksheet) => worksheet.UsedRange.Rows.Count;

        public static ME.Range GetCell(this Worksheet worksheet, int row, int column) => (ME.Range)worksheet.Cells[row, column];

        public static string? GetValue(this Worksheet worksheet, int row, int column) => worksheet.GetCell(row, column).GetValue();

        public static bool TryGetValue(this Worksheet worksheet, int row, int column, [MaybeNullWhen(false)] out string value)
            => worksheet.GetCell(row, column).TryGetValue(out value);

        public static bool ContainsValue(this Worksheet worksheet, int row, int column) => worksheet.GetCell(row, column).ContainsValue();

        public static void SetValue(this Worksheet worksheet, int row, int column, string value) => worksheet.GetCell(row, column).SetValue(value);

        public static Hyperlink GetHyperlink(this Worksheet worksheet, int row, int column)
            => worksheet.GetCell(row, column).GetHyperlink();

        public static bool TryGetHyperlink(this Worksheet worksheet, int row, int column, [MaybeNullWhen(false)] out Hyperlink hyperlink)
            => worksheet.GetCell(row, column).TryGetHyperlink(out hyperlink);

        public static bool ContainsHyperlink(this Worksheet worksheet, int row, int column) => worksheet.GetCell(row, column).ContainsHyperlink();

        public static void SetHyperlink(this Worksheet worksheet, int row, int column, string address, string? text = null, string? screenTip = null)
            => worksheet.GetCell(row, column).SetHyperlink(address, text, screenTip);

        public static ME.Range GetRange(this Worksheet worksheet, int startRow, int startColumn, int endRow, int endColumn)
            => worksheet.Range[ExcelUtil.ToString(startRow, startColumn, endRow, endColumn)];
    }
}
