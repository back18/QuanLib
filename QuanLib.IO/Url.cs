using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO
{
    public static class Url
    {
        private const char SEPARATOR_CHAR = '/';

        private static readonly JoinHelper JoinHelper = new(SEPARATOR_CHAR);

        public static string Combine(string url1, string url2)
        {
            ArgumentException.ThrowIfNullOrEmpty(url1, nameof(url1));
            ArgumentException.ThrowIfNullOrEmpty(url2, nameof(url2));

            return JoinHelper.Combine(url1, url2);
        }

        public static string Combine(string url1, string url2, string url3)
        {
            ArgumentException.ThrowIfNullOrEmpty(url1, nameof(url1));
            ArgumentException.ThrowIfNullOrEmpty(url2, nameof(url2));
            ArgumentException.ThrowIfNullOrEmpty(url3, nameof(url3));

            return JoinHelper.Combine(url1, url2, url3);
        }

        public static string Combine(string url1, string url2, string url3, string url4)
        {
            ArgumentException.ThrowIfNullOrEmpty(url1, nameof(url1));
            ArgumentException.ThrowIfNullOrEmpty(url2, nameof(url2));
            ArgumentException.ThrowIfNullOrEmpty(url3, nameof(url3));
            ArgumentException.ThrowIfNullOrEmpty(url4, nameof(url4));

            return JoinHelper.Combine(url1, url2, url3, url4);
        }

        public static string Combine(params string[] urls)
        {
            ArgumentNullException.ThrowIfNull(urls, nameof(urls));
            if (urls.Length == 0)
                return string.Empty;
            if (urls.Length == 1)
                return urls[0];

            foreach (var url in urls)
            {
                if (string.IsNullOrEmpty(url))
                    throw new ArgumentException($"“{nameof(urls)}”的一个或多个子项为 null 或空", nameof(urls));
            }

            return JoinHelper.Combine(urls);
        }
    }
}
