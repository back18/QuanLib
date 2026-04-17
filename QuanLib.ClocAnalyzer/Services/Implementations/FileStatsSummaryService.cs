using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.ClocAnalyzer.Services.Implementations
{
    public class FileStatsSummaryService : IFileStatsSummaryService
    {
        public FolderStats[] Summarize(string rootFolder, IEnumerable<FileStats> fileStats)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(rootFolder, nameof(rootFolder));
            ArgumentNullException.ThrowIfNull(fileStats, nameof(fileStats));

            Dictionary<string, FolderStatsBuilder> builders = [];
            builders.Add(rootFolder, new FolderStatsBuilder(rootFolder, null));

            fileStats = fileStats
                .Select(s => new { FileStats = s, Depth = GetDepth(s.FilePath) })
                .OrderBy(x => x.Depth)
                .Select(s => s.FileStats);

            foreach (FileStats stats in fileStats)
            {
                string? folderPath = Path.GetDirectoryName(stats.FilePath);
                if (string.IsNullOrEmpty(folderPath))
                    continue;

                if (!builders.TryGetValue(folderPath, out var builder))
                {
                    FolderStatsBuilder? parentBuilder = GetParentBuilder(builders, folderPath);
                    builder = new FolderStatsBuilder(folderPath, parentBuilder);
                    builders.Add(folderPath, builder);
                }
            }

            foreach (FileStats stats in fileStats)
            {
                string? folderPath = Path.GetDirectoryName(stats.FilePath);
                if (string.IsNullOrEmpty(folderPath))
                    continue;

                if (builders.TryGetValue(folderPath, out var builder))
                    builder.AddFileStats(stats);
            }

            return builders.Values.Select(b => b.Build()).ToArray();
        }

        private static int GetDepth(string path)
        {
            return path.Count(c => c == Path.DirectorySeparatorChar);
        }

        private static FolderStatsBuilder? GetParentBuilder(Dictionary<string, FolderStatsBuilder> builders, string folderPath)
        {
            string? parentFolderPath = Path.GetDirectoryName(folderPath);
            if (string.IsNullOrEmpty(parentFolderPath))
                return null;

            if (builders.TryGetValue(parentFolderPath, out var parentBuilder))
                return parentBuilder;

            FolderStatsBuilder? grandParentBuilder = GetParentBuilder(builders, parentFolderPath);
            if (grandParentBuilder is null)
                return null;

            parentBuilder = new FolderStatsBuilder(parentFolderPath, grandParentBuilder);
            builders.Add(parentFolderPath, parentBuilder);
            return parentBuilder;
        }
    }
}
