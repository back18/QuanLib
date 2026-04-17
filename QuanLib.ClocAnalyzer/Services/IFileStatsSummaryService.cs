using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services
{
    public interface IFileStatsSummaryService
    {
        public FolderStats[] Summarize(string rootFolder, IEnumerable<FileStats> fileStats);
    }
}
