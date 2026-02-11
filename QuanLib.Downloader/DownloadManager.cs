using Downloader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuanLib.Downloader
{
    public class DownloadManager : INotifyPropertyChanged
    {
        public DownloadManager(DownloadConfiguration? configuration = null, int maximumParallel = -1)
        {
            _downloads = [];
            Downloads = new(_downloads);
            Configuration = configuration ?? new DownloadConfiguration();
            MaximumParallel = maximumParallel <= 0 ? -1 : maximumParallel;
        }

        private readonly Lock _lock = new();
        private readonly ObservableCollection<IDownload> _downloads;

        public ReadOnlyObservableCollection<IDownload> Downloads { get; }

        public DownloadConfiguration Configuration { get; }

        public int MaximumParallel { get; }

        public int TotalCount
        {
            get => field;
            set
            {
                if (value != field)
                    OnPropertyChanged(ref field, value);
            }
        }

        public int RunningCount
        {
            get => field;
            set
            {
                if (value != field)
                    OnPropertyChanged(ref field, value);
            }
        }

        public int CompletedCount
        {
            get => field;
            set
            {
                if (value != field)
                    OnPropertyChanged(ref field, value);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Submit(DownloadItem downloadItem) => Submit(downloadItem.Url, downloadItem.Path);

        public void Submit(string url, string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            IDownload download = EnhancedDownloadBuilder.New()
                .WithConfiguration(Configuration)
                .WithUrl(url)
                .WithFileLocation(path)
                .Build();

            Submit(download);
        }

        public void Submit(IDownload download)
        {
            ArgumentNullException.ThrowIfNull(download, nameof(download));

            lock (_lock)
            {
                if (_downloads.Contains(download))
                    return;

                download.DownloadFileCompleted += Download_DownloadFileCompleted;
                _downloads.Add(download);
                Update();
            }
        }

        public void ClearCompleted()
        {
            lock (_lock)
            {
                foreach (var download in _downloads.Where(s => s.Status is DownloadStatus.Completed or DownloadStatus.Stopped or DownloadStatus.Failed).ToArray())
                {
                    download.DownloadFileCompleted -= Download_DownloadFileCompleted;
                    download.Dispose();
                    _downloads.Remove(download);
                }
            }
        }

        public void StopForAll()
        {
            lock (_lock)
            {
                foreach (IDownload download in _downloads)
                {
                    if (download.Status is DownloadStatus.Running or DownloadStatus.Paused)
                        download.Stop();
                }
            }
        }

        public void PauseForAll()
        {
            lock ( _lock)
            {
                foreach (var download in _downloads)
                {
                    if (download.Status is DownloadStatus.Running)
                        download.Pause();
                }
            }
        }

        public void ResumeForAll()
        {
            lock (_lock)
            {
                foreach (var download in _downloads)
                {
                    if (download.Status is DownloadStatus.Paused)
                        download.Resume();
                }
            }
        }

        private void Download_DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            if (sender is IDownload download && _downloads.Contains(download))
            {
                lock (_lock)
                    Update();
            }
        }

        private void Update()
        {
            TotalCount = _downloads.Count;
            CompletedCount = _downloads.Count(s => s.Status is DownloadStatus.Completed or DownloadStatus.Stopped or DownloadStatus.Failed);
            RunningCount = TotalCount - CompletedCount;

            var created = _downloads.Where(s => s.Status is DownloadStatus.None or DownloadStatus.Created);
            if (created.Any())
            {
                //受限并发
                if (MaximumParallel > 0)
                {
                    int downloadingCount = _downloads.Count(s => s.Status is DownloadStatus.Running or DownloadStatus.Paused);
                    int idleCount = MaximumParallel - downloadingCount;
                    if (idleCount > 0)
                    {
                        IDownload[] downloads = created.ToArray();
                        int maxCount = Math.Min(idleCount, downloads.Length);
                        for (int i = 0; i < maxCount; i++)
                            downloads[i].StartAsync();
                    }
                }
                //无限并发
                else
                {
                    foreach (IDownload download in created)
                        download.StartAsync();
                }
            }
        }

        protected virtual void OnPropertyChanged<T>(ref T field, T newValue, [CallerMemberName] string? name = null)
        {
            if (Equals(field, newValue))
                return;

            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
