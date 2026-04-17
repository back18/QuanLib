using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer
{
    public class ClocOptions
    {
        public const char DefaultCsvDelimiter = ',';

        public string? OutputTarget { get; set; } = null;

        public OutputContentFormat OutputContentFormat { get; set; } = OutputContentFormat.ByLang;

        public OutputFileFormat OutputFileFormat { get; set; } = OutputFileFormat.None;

        public char CsvDelimiter { get; set; } = DefaultCsvDelimiter;

        public bool IsQuiet { get; set; } = false;

        public List<string>? ExcludeDir { get; set; }

        public List<string>? ExcludeLang { get; set; }

        public List<string>? ExcludeExt { get; set; }

        public List<string>? IncludeLang { get; set; }

        public List<string>? IncludeExt { get; set; }

        public List<string>? OtherOptions { get; set; }
    }
}
