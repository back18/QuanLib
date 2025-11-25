using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Extensions
{
    public static class GuidExtensions
    {
        private static readonly char[] HEX_CHARS_LOWER = [
            '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];

        private static readonly char[] HEX_CHARS_UPPER = [
            '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];

        public static string GetLastChars(this Guid guid, int length = 12, bool uppercase = false)
        {
            ThrowHelper.ArgumentOutOfRange(1, 12, length, nameof(length));

            char[] hexChars = uppercase ? HEX_CHARS_UPPER : HEX_CHARS_LOWER;
            Span<char> buffer = stackalloc char[length];
            Span<byte> bytes = stackalloc byte[16];
            guid.TryWriteBytes(bytes);

            for (int i = 15, bufferIndex = length - 1; i >= 0; i--)
            {
                byte b = bytes[i];

                if (bufferIndex < 0)
                    break;
                buffer[bufferIndex--] = hexChars[b & 0x0f];

                if (bufferIndex < 0)
                    break;
                buffer[bufferIndex--] = hexChars[(b >> 4) & 0x0f];
            }

            return new string(buffer);
        }

        public static string GetFirstChars(this Guid guid, int length = 8, bool uppercase = false)
        {
            ThrowHelper.ArgumentOutOfRange(1, 8, length, nameof(length));

            char[] hexChars = uppercase ? HEX_CHARS_UPPER : HEX_CHARS_LOWER;
            Span<char> buffer = stackalloc char[length];
            Span<byte> bytes = stackalloc byte[16];
            guid.TryWriteBytes(bytes);

            for (int i = 3, bufferIndex = 0; i >= 0; i--)
            {
                byte b = bytes[i];

                if (bufferIndex >= length)
                    break;
                buffer[bufferIndex++] = hexChars[(b >> 4) & 0x0f];

                if (bufferIndex >= length)
                    break;
                buffer[bufferIndex++] = hexChars[b & 0x0f];
            }

            return new string(buffer);
        }

        public static bool LastEquals(this Guid source, Guid other, int length = 12)
        {
            ThrowHelper.ArgumentOutOfRange(1, 12, length, nameof(length));

            Span<byte> sourceBytes = stackalloc byte[16];
            Span<byte> otherBytes = stackalloc byte[16];
            source.TryWriteBytes(sourceBytes);
            other.TryWriteBytes(otherBytes);

            for (int byteCount = length / 2, i = 15; byteCount > 0 && i >= 0; byteCount--, i--)
            {
                if (sourceBytes[i] != otherBytes[i])
                    return false;
            }

            if (length % 2 != 0)
            {
                int i = 15 - (length / 2);
                return (sourceBytes[i] & 0x0f) == (otherBytes[i] & 0x0f);
            }

            return true;
        }

        public static bool FirstEquals(this Guid source, Guid other, int length = 8)
        {
            ThrowHelper.ArgumentOutOfRange(1, 12, length, nameof(length));

            Span<byte> sourceBytes = stackalloc byte[16];
            Span<byte> otherBytes = stackalloc byte[16];
            source.TryWriteBytes(sourceBytes);
            other.TryWriteBytes(otherBytes);

            for (int byteCount = length / 2, i = 3; byteCount > 0 && i >= 0; byteCount--, i--)
            {
                if (sourceBytes[i] != otherBytes[i])
                    return false;
            }

            if (length % 2 != 0)
            {
                int i = 3 - (length / 2);
                return ((sourceBytes[i] >> 4) & 0x0f) == ((otherBytes[i] >> 4) & 0x0f);
            }

            return true;
        }
    }
}
