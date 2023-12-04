using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Downloader
{
    public static class DownloaderUtil
    {
        public static string FormatProgressBar(long totalBytes, long downloadedBytes, int length, char background = '□', char foreground = '■')
        {
            ThrowHelper.ArgumentOutOfMin(0, totalBytes, nameof(totalBytes));
            ThrowHelper.ArgumentOutOfRange(0, totalBytes, downloadedBytes, nameof(downloadedBytes));
            ThrowHelper.ArgumentOutOfMin(0, length, nameof(length));

            if (length == 0)
                return string.Empty;

            double progress = (double)downloadedBytes / totalBytes;
            int foregroundLength = (int)Math.Round(progress * length);
            int backgroundLength = length - foregroundLength;

            StringBuilder sb = new();
            sb.Append(foreground, foregroundLength);
            sb.Append(background, backgroundLength);

            return sb.ToString();
        }
    }
}
