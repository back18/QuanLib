using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Downloader
{
    public readonly struct DownloadItem(string url, string path)
    {
        public readonly string Url = url;

        public readonly string Path = path;
    }
}
