using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuanLib.IO.Zip
{
    public static class MethodAliasExtensions
    {
        public static bool ExistsFile(this ZipPack zipPack, [NotNullWhen(true)] string? path)
        {
            ArgumentNullException.ThrowIfNull(zipPack, nameof(zipPack));
            return zipPack.FileExists(path);
        }

        public static bool ExistsDirectory(this ZipPack zipPack, [NotNullWhen(true)] string? path)
        {
            ArgumentNullException.ThrowIfNull(zipPack, nameof(zipPack));
            return zipPack.DirectoryExists(path);
        }

        public static ZipItem GetFile(this ZipPack zipPack, string path)
        {
            ArgumentNullException.ThrowIfNull(zipPack, nameof(zipPack));
            return zipPack.ReadZipItem(path);
        }

        public static bool TryGetFile(this ZipPack zipPack, [NotNullWhen(true)] string? path, [MaybeNullWhen(false)] out ZipItem result)
        {
            ArgumentNullException.ThrowIfNull(zipPack, nameof(zipPack));
            return zipPack.TryReadZipItem(path, out result);
        }

        public static void AddFile(this ZipPack zipPack, string path, byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(zipPack, nameof(zipPack));
            zipPack.WriteFile(path, bytes);
        }


        public static void AddFile(this ZipPack zipPack, string path, Stream inputStream)
        {
            ArgumentNullException.ThrowIfNull(zipPack, nameof(zipPack));
            zipPack.WriteFile(path, inputStream);
        }

        public static void RemoveFile(this ZipPack zipPack, string path)
        {
            ArgumentNullException.ThrowIfNull(zipPack, nameof(zipPack));
            zipPack.DeleteFile(path);
        }
    }
}
