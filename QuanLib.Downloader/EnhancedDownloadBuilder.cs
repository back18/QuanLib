using Downloader;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Downloader
{
    public class EnhancedDownloadBuilder
    {
        private string? _url;
        private string? _folder;
        private string? _filename;
        private bool _openFileStream;
        private DownloadConfiguration? _configuration;
        private IProgress<DownloadProgress>? _progress;
        private int _minProgressInterval;
        private ILoggerFactory? _loggerFactory;

        public static EnhancedDownloadBuilder New() => new();

        public EnhancedDownloadBuilder WithUrl(string url)
        {
            _url = url;
            return this;
        }

        public EnhancedDownloadBuilder WithUrl(Uri uri)
        {
            ArgumentNullException.ThrowIfNull(uri, nameof(uri));

            return WithUrl(uri.LocalPath);
        }

        public EnhancedDownloadBuilder WithFolder(string folder)
        {
            _folder = folder;
            return this; 
        }

        public EnhancedDownloadBuilder WithFilename(string filename)
        {
            _filename = filename;
            return this; 
        }

        public EnhancedDownloadBuilder WithFileLocation(string fullPath)
        {
            fullPath = Path.GetFullPath(fullPath);
            _filename = Path.GetFileName(fullPath);
            _folder = Path.GetDirectoryName(fullPath);
            return this;
        }

        public EnhancedDownloadBuilder WithFolder(DirectoryInfo directoryInfo)
        {
            ArgumentNullException.ThrowIfNull(directoryInfo, nameof(directoryInfo));

            return WithFolder(directoryInfo.FullName);
        }

        public EnhancedDownloadBuilder WithFileLocation(FileInfo fileInfo)
        {
            ArgumentNullException.ThrowIfNull(fileInfo, nameof(fileInfo));

            return WithFileLocation(fileInfo.FullName);
        }

        public EnhancedDownloadBuilder WithConfiguration(DownloadConfiguration? configuration)
        {
            _configuration = configuration;
            return this;
        }

        public EnhancedDownloadBuilder WithProgress(IProgress<DownloadProgress>? progress)
        {
            _progress = progress;
            return this;
        }

        public EnhancedDownloadBuilder WithMinProgressInterval(int minProgressInterval)
        {
            _minProgressInterval = minProgressInterval;
            return this;
        }

        public EnhancedDownloadBuilder WithLogging(ILoggerFactory? loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }

        public EnhancedDownloadBuilder OpenFileStreamAfterCompletion()
        {
            _openFileStream = true;
            return this;
        }

        public EnhancedDownload Build()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(_url);

            EnhancedDownload enhancedDownload = new(
                _url,
                _folder,
                _filename,
                _openFileStream,
                _configuration,
                _progress,
                _loggerFactory);

            if (_minProgressInterval > 0)
                enhancedDownload.MinProgressInterval = _minProgressInterval;

            return enhancedDownload;
        }
    }
}
