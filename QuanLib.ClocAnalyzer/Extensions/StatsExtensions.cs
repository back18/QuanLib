using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Extensions
{
    public static class StatsExtensions
    {
        public static int GetTotalLines(this IStats stats)
        {
            return stats.CodeLines + stats.CommentLines + stats.BlankLines;
        }
    }
}
