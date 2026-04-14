using Downloader;
using Microsoft.Extensions.Logging;
using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Downloader.Services.Implementations
{
    public class DownloadService : IDownloadService
    {
        public DownloadService(IDownloadConfigurationProvider configurationProvider, ILoggerFactory? loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(configurationProvider, nameof(configurationProvider));

            _configurationProvider = configurationProvider;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory?.CreateLogger<DownloadService>();

            MaxTryAgainOnFailure = int.MaxValue;
            AgainDelayMilliseconds = 3000;
            MinProgressInterval = 1000;
        }

        private readonly IDownloadConfigurationProvider _configurationProvider;
        private readonly ILoggerFactory? _loggerFactory;
        private readonly ILogger<DownloadService>? _logger;

        public int MaxTryAgainOnFailure { get; set; }

        public int AgainDelayMilliseconds { get; set; }

        public int MinProgressInterval { get; set; }

        public async Task<Stream> DownloadAsync(string url, CancellationToken cancellationToken = default)
        {
            using EnhancedDownload download = BuildDownload(url);
            return await DownloadAsync(download, cancellationToken);
        }

        public async Task<Stream> DownloadAsync(string url, string path, bool openFileStream, CancellationToken cancellationToken = default)
        {
            using EnhancedDownload download = BuildDownload(url, path, openFileStream);
            return await DownloadAsync(download, cancellationToken);
        }

        public async Task<Stream> DownloadAsync(string url, IProgress<DownloadProgress>? progress, CancellationToken cancellationToken = default)
        {
            using EnhancedDownload download = BuildDownload(url, progress);
            return await DownloadAsync(download, cancellationToken);
        }

        public async Task<Stream> DownloadAsync(string url, string path, bool openFileStream, IProgress<DownloadProgress>? progress, CancellationToken cancellationToken = default)
        {
            using EnhancedDownload download = BuildDownload(url, path, openFileStream, progress);
            return await DownloadAsync(download, cancellationToken);
        }

        private async Task<Stream> DownloadAsync(EnhancedDownload download, CancellationToken cancellationToken = default)
        {
            int failureCount = 0;
            while (true)
            {
                if (_logger?.IsEnabled(LogLevel.Information) == true)
                    _logger.LogInformation("开始下载: {Url}", download.Url);
                Stream stream = await download.StartAsync(cancellationToken).ConfigureAwait(false);

                if (download.Status is DownloadStatus.None or DownloadStatus.Failed)
                {
                    Exception? error = download.Error;
                    _logger?.LogWarning("下载失败: {Error}", ObjectFormatter.Format(error));

                    if (failureCount++ < MaxTryAgainOnFailure)
                    {
                        int delayMilliseconds = Math.Max(0, AgainDelayMilliseconds);
                        if (_logger?.IsEnabled(LogLevel.Information) == true)
                            _logger.LogInformation("{DelayMilliseconds}ms后重试...", delayMilliseconds);

                        await Task.Delay(delayMilliseconds, cancellationToken).ConfigureAwait(false);
                        continue;
                    }
                    else
                    {
                        if (error is not null)
                            throw error;
                        else
                            throw new InvalidDataException("文件下载失败，未知错误");
                    }
                }

                if (_logger?.IsEnabled(LogLevel.Information) == true)
                {
                    _logger.LogInformation("下载完成: {Url}", download.Url);
                    string fileLocation = download.FileLocation;
                    if (!string.IsNullOrEmpty(fileLocation))
                        _logger.LogInformation("文件已保存到: {FileLocation}", fileLocation);
                }

                return stream;
            }
        }

        private EnhancedDownload BuildDownload(string url, IProgress<DownloadProgress>? progress = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(url, nameof(url));

            EnhancedDownload download = EnhancedDownloadBuilder.New()
                .WithConfiguration(_configurationProvider.GlobalConfiguration)
                .WithUrl(url)
                .WithProgress(progress)
                .WithMinProgressInterval(MinProgressInterval)
                .WithLogging(_loggerFactory)
                .Build();

            return download;
        }

        private EnhancedDownload BuildDownload(string url, string path, bool openFileStream, IProgress<DownloadProgress>? progress = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(url, nameof(url));
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            EnhancedDownloadBuilder builder = EnhancedDownloadBuilder.New()
                .WithConfiguration(_configurationProvider.GlobalConfiguration)
                .WithUrl(url)
                .WithFileLocation(path)
                .WithProgress(progress)
                .WithMinProgressInterval(MinProgressInterval)
                .WithLogging(_loggerFactory);

            if (openFileStream)
                builder.OpenFileStreamAfterCompletion();

            return builder.Build();
        }
    }
}
