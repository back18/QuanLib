using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QuanLib.IO.Extensions
{
    public static class FileInfoExtensions
    {
        extension(FileInfo source)
        {
            public bool OpenReadIfExists([MaybeNullWhen(false)] out FileStream result)
            {
                if (source.Exists)
                {
                    result = source.OpenRead();
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool OpenTextIfExists([MaybeNullWhen(false)] out StreamReader result)
            {
                if (source.Exists)
                {
                    result = source.OpenText();
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllBytesIfExists([MaybeNullWhen(false)] out byte[] result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllBytes(source.FullName);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllTextIfExists([MaybeNullWhen(false)] out string result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllText(source.FullName);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllTextIfExists(Encoding encoding, [MaybeNullWhen(false)] out string result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllText(source.FullName, encoding);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllLinesIfExists([MaybeNullWhen(false)] out string[] result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllLines(source.FullName);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllLinesIfExists(Encoding encoding, [MaybeNullWhen(false)] out string[] result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllLines(source.FullName, encoding);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllBytesAsyncIfExists([MaybeNullWhen(false)] out Task<byte[]> result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllBytesAsync(source.FullName);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllTextAsyncIfExists([MaybeNullWhen(false)] out Task<string> result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllTextAsync(source.FullName);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllTextAsyncIfExists(Encoding encoding, [MaybeNullWhen(false)] out Task<string> result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllTextAsync(source.FullName, encoding);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllLinesAsyncIfExists([MaybeNullWhen(false)] out Task<string[]> result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllLinesAsync(source.FullName);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            public bool ReadAllLinesAsyncIfExists(Encoding encoding, [MaybeNullWhen(false)] out Task<string[]> result)
            {
                if (source.Exists)
                {
                    result = File.ReadAllLinesAsync(source.FullName, encoding);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
        }
    }
}
