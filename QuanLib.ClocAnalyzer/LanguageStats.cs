using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer
{
    public record LanguageStats(string Language, int FileCount, int BlankLines, int CommentLines, int CodeLines) : IStats;
}
