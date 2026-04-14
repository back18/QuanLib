using Downloader;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Downloader.Services
{
    public interface IDownloadConfigurationProvider
    {
        public DownloadConfiguration GlobalConfiguration { get; }

        public DownloadConfiguration CreateDefaultConfiguration();
    }
}
