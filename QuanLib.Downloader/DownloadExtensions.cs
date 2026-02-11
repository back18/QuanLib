using Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Downloader
{
    public static class DownloadExtensions
    {
        extension(IDownload download)
        {
            public string FileLocation
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(download.Folder) || string.IsNullOrWhiteSpace(download.Filename))
                        return string.Empty;

                    return Path.Combine(download.Folder, download.Filename);
                }
            }
        }
    }
}
