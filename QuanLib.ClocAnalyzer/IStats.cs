using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer
{
    public interface IStats
    {
        public int CodeLines { get; }

        public int CommentLines { get; }

        public int BlankLines { get; }
    }
}
