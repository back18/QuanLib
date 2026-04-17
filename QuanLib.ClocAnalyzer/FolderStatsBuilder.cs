using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer
{
    public class FolderStatsBuilder
    {
        public FolderStatsBuilder(string folderPath, FolderStatsBuilder? parentBuilder = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(folderPath, nameof(folderPath));

            FolderPath = folderPath;
            _parentBuilder = parentBuilder;
        }

        private readonly FolderStatsBuilder? _parentBuilder;
        private readonly Stats _stats = new();
        private readonly Dictionary<string, Stats> _languageStats = [];

        public string FolderPath { get; }

        public void AddFileStats(FileStats fileStats)
        {
            ArgumentNullException.ThrowIfNull(fileStats, nameof(fileStats));

            _stats.CodeLines += fileStats.CodeLines;
            _stats.CommentLines += fileStats.CommentLines;
            _stats.BlankLines += fileStats.BlankLines;
            _stats.FileCount++;

            if (!_languageStats.TryGetValue(fileStats.Language, out var languageStat))
            {
                languageStat = new Stats();
                _languageStats[fileStats.Language] = languageStat;
            }

            languageStat.CodeLines += fileStats.CodeLines;
            languageStat.CommentLines += fileStats.CommentLines;
            languageStat.BlankLines += fileStats.BlankLines;
            languageStat.FileCount++;

            _parentBuilder?.AddFileStats(fileStats);
        }

        public FolderStats Build()
        {
            return new FolderStats(FolderPath, _languageStats.Select(s =>
            {
                string language = s.Key;
                Stats stats = s.Value;
                return new LanguageStats(language, stats.FileCount, stats.BlankLines, stats.CommentLines, stats.CodeLines);
            }))
            {
                FileCount = _stats.FileCount,
                CodeLines = _stats.CodeLines,
                CommentLines = _stats.CommentLines,
                BlankLines = _stats.BlankLines
            };
        }

        private class Stats
        {
            public int FileCount;

            public int CodeLines;

            public int CommentLines;

            public int BlankLines;
        }
    }
}
