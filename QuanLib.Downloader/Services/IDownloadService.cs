using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Downloader.Services
{
    public interface IDownloadService
    {
        public Task<Stream> DownloadAsync(string url, CancellationToken cancellationToken = default);

        public Task<Stream> DownloadAsync(string url, string path, bool openFileStream, CancellationToken cancellationToken = default);

        public Task<Stream> DownloadAsync(string url, IProgress<DownloadProgress> progress, CancellationToken cancellationToken = default);

        public Task<Stream> DownloadAsync(string url, string path, bool openFileStream, IProgress<DownloadProgress> progress, CancellationToken cancellationToken = default);
    }
}
