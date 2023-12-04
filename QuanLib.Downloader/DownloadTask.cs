using Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Downloader
{
    public class DownloadTask : IDisposable
    {
        public DownloadTask(string url, string? path = null, DownloadConfiguration? configuration = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(url, nameof(url));

            var builder = DownloadBuilder.New().WithUrl(url);
            if (!string.IsNullOrEmpty(path))
                builder.WithFileLocation(path);
            if (configuration is not null)
                builder.WithConfiguration(configuration);

            Download = builder.Build();
            Download.DownloadProgressChanged += Download_DownloadProgressChanged;
            MemoryStream = new((int)Download.TotalFileSize);
        }

        public IDownload Download { get; }

        public MemoryStream MemoryStream { get; }

        public Task<Stream> Task => _Task ?? throw new InvalidOperationException("下载任务未开始");
        private Task<Stream>? _Task;

        public DownloadProgressChangedEventArgs DownloadProgressChangedEventArgs => _DownloadProgressChangedEventArgs ?? throw new InvalidOperationException("下载任务未开始");
        private DownloadProgressChangedEventArgs? _DownloadProgressChangedEventArgs;

        public bool DownloadProgressAvailable => _DownloadProgressChangedEventArgs is not null;

        private void Download_DownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
        {
            _DownloadProgressChangedEventArgs = e;
            MemoryStream.Write(e.ReceivedBytes);
        }

        public async Task<Stream> StartAsync()
        {
            _Task = GetTask();
            return await _Task;
        }

        private async Task<Stream> GetTask()
        {
            Stream stream = await Download.StartAsync();
            if (stream is not null)
                return stream;
            else
                return MemoryStream;
        }

        public void Dispose()
        {
            Download.Dispose();
            MemoryStream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
