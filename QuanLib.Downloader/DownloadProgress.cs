using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Downloader
{
    public readonly struct DownloadProgress(long totalFileSize, long downloadedFileSize, double bytesPerSecondSpeed, double averageBytesPerSecondSpeed)
    {
        public long TotalFileSize { get; } = totalFileSize;

        public long DownloadedFileSize { get; } = downloadedFileSize;

        public double BytesPerSecondSpeed { get;} = bytesPerSecondSpeed;

        public double AverageBytesPerSecondSpeed { get;} = averageBytesPerSecondSpeed;

        public double ProgressPercentage => TotalFileSize == 0 ? 0 : ((double)DownloadedFileSize * 100) / TotalFileSize;

        public TimeSpan RemainingTime => BytesPerSecondSpeed == 0 ? TimeSpan.Zero : TimeSpan.FromSeconds((TotalFileSize - DownloadedFileSize) / BytesPerSecondSpeed);

        public TimeSpan AverageRemainingTime => AverageBytesPerSecondSpeed == 0 ? TimeSpan.Zero : TimeSpan.FromSeconds((TotalFileSize - DownloadedFileSize) / AverageBytesPerSecondSpeed);
    }
}
