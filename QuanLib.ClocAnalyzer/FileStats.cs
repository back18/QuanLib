using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer
{
    public record FileStats(string Language, string FilePath, int BlankLines, int CommentLines, int CodeLines) : IStats;
}
