using Downloader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Download.DownloadFileCompleted += Download_DownloadFileCompleted;
            _buffer = new();
        }

        private MemoryStream _buffer;

        public string Url { get; }

        public string? Path { get; }

        public DownloadConfiguration? Configuration { get; }

        public IDownload Download { get; private set; }

        public Task<Stream> Task => _Task ?? throw new InvalidOperationException("下载任务未开始");
        private Task<Stream>? _Task;

        public DownloadProgressChangedEventArgs DownloadProgressChangedEventArgs => _DownloadProgressChangedEventArgs ?? throw new InvalidOperationException("下载任务未开始");
        private DownloadProgressChangedEventArgs? _DownloadProgressChangedEventArgs;

        public bool DownloadProgressAvailable => _DownloadProgressChangedEventArgs is not null;

        private void Download_DownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
        {
            _DownloadProgressChangedEventArgs = e;
            _buffer.Write(e.ReceivedBytes);
        }

        private void Download_DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            if (_buffer.Length != Download.TotalFileSize && File.Exists(Path))
            {
                try
                {
                    using FileStream fileStream = File.OpenRead(Path);
                    if (fileStream.Length == Download.TotalFileSize)
                    {
                        _buffer.Position = 0;
                        _buffer.SetLength(0);
                        fileStream.CopyTo(_buffer);
                    }
                }
                catch
                {

                }
            }
        }

        public async Task<Stream> StartAsync()
        {
            if (Download.Status == DownloadStatus.None)
            {
                _Task = DownloadAsync();
                return await _Task;
            }

            throw new InvalidOperationException("尝试重复启动下载任务");
        }

        public void RetryIfFailed()
        {
            if (Download.Status == DownloadStatus.Failed)
            {
                Download.Dispose();
                _buffer.Dispose();

                var builder = GetBuilder();
                Download = builder.Build();
                Download.DownloadProgressChanged += Download_DownloadProgressChanged;
                _buffer = new();
                _Task = DownloadAsync();
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

        private async Task<Stream> DownloadAsync()
        {
            if (Path is not null && File.Exists(Path))
            {
                try
                {
                    File.Delete(Path);
                }
                catch
                {
                    return Stream.Null;
                }
            }

            Stream stream = await Download.StartAsync();
            if (stream == Stream.Null && _buffer.Length == Download.TotalFileSize)
                return _buffer;
            else
                return stream;
        }

        public void Dispose()
        {
            Download.Dispose();
            _buffer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
