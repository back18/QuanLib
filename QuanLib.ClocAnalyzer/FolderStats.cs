using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer
{
    public class FolderStats : IStats
    {
        public FolderStats(string folderPath, IEnumerable<LanguageStats> languageStats)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(folderPath, nameof(folderPath));
            ArgumentNullException.ThrowIfNull(languageStats, nameof(languageStats));

            FolderPath = folderPath;
            LanguageStats = languageStats.ToDictionary(s => s.Language);
        }

        public IReadOnlyDictionary<string, LanguageStats> LanguageStats { get; }

        public string FolderPath { get; init; }

        public required int FileCount { get; init; }

        public required int CodeLines { get; init; }

        public required int CommentLines { get; init; }

        public required int BlankLines { get; init; }
    }
}
