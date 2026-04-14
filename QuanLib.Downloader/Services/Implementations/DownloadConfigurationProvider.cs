using Downloader;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Downloader.Services.Implementations
{
    public class DownloadConfigurationProvider : IDownloadConfigurationProvider
    {
        public DownloadConfigurationProvider()
        {
            GlobalConfiguration = CreateDefaultConfiguration();
        }

        public DownloadConfiguration GlobalConfiguration { get; }

        public DownloadConfiguration CreateDefaultConfiguration()
        {
            return new DownloadConfiguration()
            {
                ClearPackageOnCompletionWithFailure = true
            };
        }
    }
}
