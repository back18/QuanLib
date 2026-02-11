using Downloader;
using Microsoft.Extensions.Logging;
using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuanLib.Downloader
{
    public class EnhancedDownload : IDownload, INotifyPropertyChanged
    {
        internal EnhancedDownload(
            string url,
            string? folder,
            string? filename,
            bool openFileStream,
            DownloadConfiguration? configuration,
            IProgress<DownloadProgress>? progress,
            ILoggerFactory? loggerFactory)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

            Url = url;
            Folder = folder ?? string.Empty;
            Filename = filename ?? string.Empty;
            _downloadService = new DownloadService(configuration, loggerFactory);
            _progress = progress;
            _openFileStream = openFileStream;
            _status = _downloadService.Status;

            _downloadService.DownloadStarted += Download_DownloadStarted;
            _downloadService.DownloadProgressChanged += Download_DownloadProgressChanged;
            _downloadService.DownloadFileCompleted += Download_DownloadFileCompleted;
        }

        private readonly Lock _lock = new();
        private readonly IDownloadService _downloadService;
        private readonly IProgress<DownloadProgress>? _progress;
        private readonly bool _openFileStream;

        private DownloadStatus _status;
        private DownloadProgress _latestProgress;
        private bool _hasProgress;
        private long _reportTimestamp;

        public string Url { get; }

        public string Folder { get; }

        public string Filename { get; }

        public long TotalFileSize => _downloadService.Package?.TotalFileSize ?? DownloadedFileSize;

        public long DownloadedFileSize => (_downloadService.Package?.ReceivedBytesSize).GetValueOrDefault();

        public DownloadStatus Status => _downloadService.Status;

        public DownloadPackage Package => _downloadService.Package;

        public int MinProgressInterval
        {
            get => field;
            set
            {
                ThrowHelper.ArgumentOutOfMin(1, value, nameof(MinProgressInterval));
                if (value != field)
                    OnPropertyChanged(ref field, value);
            }
        }

        public double BytesPerSecondSpeed
        {
            get => field;
            private set
            {
                if (value != field)
                    OnPropertyChanged(ref field, value);
            }
        }

        public double AverageBytesPerSecondSpeed
        {
            get => field;
            private set
            {
                if (value != field)
                    OnPropertyChanged(ref field, value);
            }
        }

        public double ProgressPercentage
        {
            get => field;
            private set
            {
                if (value != field)
                    OnPropertyChanged(ref field, value);
            }
        }

        public TimeSpan RemainingTime
        {
            get => field;
            private set
            {
                if (value != field)
                    OnPropertyChanged(ref field, value);
            }
        }

        public TimeSpan AverageRemainingTime
        {
            get => field;
            private set
            {
                if (value != field)
                    OnPropertyChanged(ref field, value);
            }
        }

        public Stream? ResultStream
        {
            get => field;
            private set
            {
                OnPropertyChanged(ref field, value);
            }
        }

        public Exception? Error
        {
            get => field;
            private set
            {
                OnPropertyChanged(ref field, value);
            }
        }

        public event EventHandler<DownloadStartedEventArgs> DownloadStarted
        {
            add => _downloadService.DownloadStarted += value;
            remove => _downloadService.DownloadStarted -= value;
        }

        public event EventHandler<AsyncCompletedEventArgs> DownloadFileCompleted
        {
            add => _downloadService.DownloadFileCompleted += value;
            remove => _downloadService.DownloadFileCompleted -= value;
        }

        public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged
        {
            add => _downloadService.DownloadProgressChanged += value;
            remove => _downloadService.DownloadProgressChanged -= value;
        }

        public event EventHandler<DownloadProgressChangedEventArgs> ChunkDownloadProgressChanged
        {
            add => _downloadService.ChunkDownloadProgressChanged += value;
            remove => _downloadService.ChunkDownloadProgressChanged -= value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Reset()
        {
            _latestProgress = default;
            _hasProgress = false;
            _reportTimestamp = 0;

            BytesPerSecondSpeed = _latestProgress.BytesPerSecondSpeed;
            AverageBytesPerSecondSpeed = _latestProgress.AverageBytesPerSecondSpeed;
            ProgressPercentage = _latestProgress.ProgressPercentage;
            RemainingTime = _latestProgress.RemainingTime;
            AverageRemainingTime = _latestProgress.AverageRemainingTime;
            ResultStream = null;
            Error = null;
        }

        private void SyncStatus()
        {
            if (_status != _downloadService.Status)
            {
                _status = _downloadService.Status;
                OnPropertyChanged(nameof(Status));
            }
        }

        private void SyncProgress()
        {
            if (TotalFileSize != _latestProgress.TotalFileSize)
                OnPropertyChanged(nameof(TotalFileSize));
            if (DownloadedFileSize != _latestProgress.DownloadedFileSize)
                OnPropertyChanged(nameof(DownloadedFileSize));
        }

        public async Task<Stream> StartAsync(CancellationToken cancellationToken = default)
        {
            if (_downloadService.Status is not DownloadStatus.None or DownloadStatus.Created)
                Reset();

            SyncStatus();
            Stream stream = await StartDownloadAsync(cancellationToken).ConfigureAwait(false);

            if (_openFileStream &&
                _downloadService.Status == DownloadStatus.Completed &&
                !string.IsNullOrWhiteSpace(Folder) &&
                !string.IsNullOrWhiteSpace(Filename) &&
                stream == Stream.Null)
            {
                string filePath = Path.Combine(Folder, Filename);
                for (int i = 0; i < 10; i++)
                {
                    if (File.Exists(filePath))
                        break;
                    else
                        await Task.Delay(100, cancellationToken).ConfigureAwait(false);
                }

                stream = File.OpenRead(filePath);
                ResultStream = stream;
            }

            return stream;
        }

        private async Task<Stream> StartDownloadAsync(CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(Folder) && !string.IsNullOrWhiteSpace(Filename))
            {
                string fullPath = Path.Combine(Folder, Filename);
                await _downloadService.DownloadFileTaskAsync(Url, fullPath, cancellationToken).ConfigureAwait(false);
                return Stream.Null;
            }
            else if (!string.IsNullOrWhiteSpace(Filename))
            {
                string fullPath = Path.GetFullPath(Filename);
                await _downloadService.DownloadFileTaskAsync(Url, fullPath, cancellationToken).ConfigureAwait(false);
                return Stream.Null;
            }
            else if (!string.IsNullOrWhiteSpace(Folder))
            {
                DirectoryInfo directoryInfo = new(Folder);
                await _downloadService.DownloadFileTaskAsync(Url, directoryInfo, cancellationToken).ConfigureAwait(false);
                return Stream.Null;
            }
            else
            {
                Stream? stream = await _downloadService.DownloadFileTaskAsync(Url, cancellationToken).ConfigureAwait(false);
                if (stream is not null)
                    return stream;
                else
                    return Stream.Null;
            }
        }

        public async Task StopAsync()
        {
            await _downloadService.CancelTaskAsync().ConfigureAwait(false);
            SyncStatus();
        }

        public void Stop()
        {
            _downloadService.CancelTaskAsync().GetAwaiter().GetResult();
            SyncStatus();
        }

        public void Pause()
        {
            _downloadService.Pause();
            SyncStatus();
        }

        public void Resume()
        {
            _downloadService.Resume();
            SyncStatus();
        }

        public bool TryGetLatestProgress(out DownloadProgress result)
        {
            lock (_lock)
            {
                if (_hasProgress)
                {
                    _hasProgress = false;
                    result = _latestProgress;
                    return true;
                }
                else
                {
                    result = default;
                    return false;
                }
            }
        }

        private void Download_DownloadStarted(object? sender, DownloadStartedEventArgs e)
        {
            lock (_lock)
            {
                SyncStatus();
                SyncProgress();
                _latestProgress = new DownloadProgress(TotalFileSize, DownloadedFileSize, 0, 0);
                _hasProgress = true;

                if (_progress is not null)
                {
                    _progress.Report(_latestProgress);
                    _reportTimestamp = Stopwatch.GetTimestamp();
                }
            }
        }

        private void Download_DownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
        {
            lock (_lock)
            {
                SyncStatus();
                SyncProgress();
                _latestProgress = new DownloadProgress(
                    e.TotalBytesToReceive,
                    e.ReceivedBytesSize,
                    e.BytesPerSecondSpeed,
                    e.AverageBytesPerSecondSpeed);
                _hasProgress = true;

                BytesPerSecondSpeed = _latestProgress.BytesPerSecondSpeed;
                AverageBytesPerSecondSpeed = _latestProgress.AverageBytesPerSecondSpeed;
                ProgressPercentage = _latestProgress.ProgressPercentage;
                RemainingTime = _latestProgress.RemainingTime;
                AverageRemainingTime = _latestProgress.AverageRemainingTime;

                if (_progress is not null)
                {
                    long timestamp = Stopwatch.GetTimestamp();
                    int interval = (int)Stopwatch.GetElapsedTime(_reportTimestamp, timestamp).TotalMilliseconds;
                    if (interval >= MinProgressInterval || _latestProgress.DownloadedFileSize == _latestProgress.TotalFileSize)
                    {
                        _progress.Report(_latestProgress);
                        _reportTimestamp = timestamp;
                    }
                }
            }
        }

        private void Download_DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            if (_downloadService.Status == DownloadStatus.Completed)
            {
                ResultStream = _downloadService.Package.Storage?.OpenRead();
            }
            else if (_downloadService.Status == DownloadStatus.Failed)
            {
                Error = e.Error;
            }

            SyncStatus();
        }

        protected virtual void OnPropertyChanged<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, newValue))
                return;

            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _downloadService.DownloadStarted -= Download_DownloadStarted;
            _downloadService.DownloadProgressChanged -= Download_DownloadProgressChanged;
            _downloadService.DownloadFileCompleted -= Download_DownloadFileCompleted;
            _downloadService.Clear().GetAwaiter().GetResult();

            SyncStatus();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            _downloadService.DownloadStarted -= Download_DownloadStarted;
            _downloadService.DownloadProgressChanged -= Download_DownloadProgressChanged;
            _downloadService.DownloadFileCompleted -= Download_DownloadFileCompleted;
            await _downloadService.Clear().ConfigureAwait(false);

            SyncStatus();
            GC.SuppressFinalize(this);
        }
    }
}
