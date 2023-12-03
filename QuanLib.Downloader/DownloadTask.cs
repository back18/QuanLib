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
        }

        public IDownload Download { get; }

        public Task<Stream> Task => _Task ?? throw new InvalidOperationException("下载任务未开始");
        private Task<Stream>? _Task;

        public async Task<Stream> StartAsync()
        {
            _Task = Download.StartAsync();
            return await _Task;
        }

        public void Dispose()
        {
            Download.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
