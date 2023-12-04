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

            Url = url;
            Path = path;
            Configuration = configuration;

            var builder = GetBuilder();
            Download = builder.Build();
            Download.DownloadProgressChanged += Download_DownloadProgressChanged;
            MemoryStream = new((int)Download.TotalFileSize);
        }

        public string Url { get; }

        public string? Path { get; }

        public DownloadConfiguration? Configuration { get; }

        public IDownload Download { get; private set; }

        public MemoryStream MemoryStream { get; private set; }

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
            if (Download.Status == DownloadStatus.None)
            {
                _Task = GetTask();
                return await _Task;
            }

            throw new InvalidOperationException("尝试重复启动下载任务");
        }

        public void RetryIfFailed()
        {
            if (Download.Status == DownloadStatus.Failed)
            {
                Download.Dispose();
                MemoryStream.Dispose();

                var builder = GetBuilder();
                Download = builder.Build();
                Download.DownloadProgressChanged += Download_DownloadProgressChanged;
                MemoryStream = new((int)Download.TotalFileSize);
                _Task = GetTask();
            }
        }

        private DownloadBuilder GetBuilder()
        {
            DownloadBuilder builder = DownloadBuilder.New().WithUrl(Url);
            if (!string.IsNullOrEmpty(Path))
                builder.WithFileLocation(Path);
            if (Configuration is not null)
                builder.WithConfiguration(Configuration);

            return builder;
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
