using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public readonly struct BytesSpan : IEquatable<BytesSpan>, IComparable, IComparable<BytesSpan>
    {
        private const int BASE = 1024;
        public const long BytesPerKiloBytes = BASE;
        public const long BytesPerMegaBytes = BytesPerKiloBytes * BASE;
        public const long BytesPerGigabytes = BytesPerMegaBytes * BASE;
        public const long BytesPerTeraBytes = BytesPerGigabytes * BASE;
        public const long BytesPerPetaBytes = BytesPerTeraBytes * BASE;
        public const long BytesPerExaBytes = BytesPerPetaBytes * BASE;

        public BytesSpan(long bytes)
        {
            _bytes = bytes;
        }

        public BytesSpan(int kiloBytes, int bytes)
        {
            _bytes =
                kiloBytes * BytesPerKiloBytes +
                bytes;
        }

        public BytesSpan(int megaBytes, int kiloBytes, int bytes)
        {
            _bytes =
                megaBytes * BytesPerMegaBytes +
                kiloBytes * BytesPerKiloBytes +
                bytes;
        }

        public BytesSpan(int gigaBytes, int megaBytes, int kiloBytes, int bytes)
        {
            _bytes =
                gigaBytes * BytesPerGigabytes +
                megaBytes * BytesPerMegaBytes +
                kiloBytes * BytesPerKiloBytes +
                bytes;
        }

        public BytesSpan(int teraBytes, int gigaBytes, int megaBytes, int kiloBytes, int bytes)
        {
            _bytes =
                teraBytes * BytesPerTeraBytes +
                gigaBytes * BytesPerGigabytes +
                megaBytes * BytesPerMegaBytes +
                kiloBytes * BytesPerKiloBytes +
                bytes;
        }

        public BytesSpan(int petaBytes, int teraBytes, int gigaBytes, int megaBytes, int kiloBytes, int bytes)
        {
            _bytes =
                petaBytes * BytesPerPetaBytes +
                teraBytes * BytesPerTeraBytes +
                gigaBytes * BytesPerGigabytes +
                megaBytes * BytesPerMegaBytes +
                kiloBytes * BytesPerKiloBytes +
                bytes;
        }

        public BytesSpan(int exaBytes, int petaBytes, int teraBytes, int gigaBytes, int megaBytes, int kiloBytes, int bytes)
        {
            _bytes =
                exaBytes * BytesPerExaBytes +
                petaBytes * BytesPerPetaBytes +
                teraBytes * BytesPerTeraBytes +
                gigaBytes * BytesPerGigabytes +
                megaBytes * BytesPerMegaBytes +
                kiloBytes * BytesPerKiloBytes +
                bytes;
        }

        private readonly long _bytes;

        public int Bytes => (int)(_bytes % BASE);

        public int KiloBytes => (int)(_bytes / BytesPerKiloBytes % BASE);

        public int MegaBytes => (int)(_bytes / BytesPerMegaBytes % BASE);

        public int Gigabytes => (int)(_bytes / BytesPerGigabytes % BASE);

        public int TeraBytes => (int)(_bytes / BytesPerTeraBytes % BASE);

        public int PetaBytes => (int)(_bytes / BytesPerPetaBytes % BASE);

        public int ExaBytes => (int)(_bytes / BytesPerExaBytes % BASE);

        public double TotalBytes => _bytes;

        public double TotalKiloBytes => (double)_bytes / BytesPerKiloBytes;

        public double TotalMegaBytes => (double)_bytes / BytesPerMegaBytes;

        public double TotalGigaBytes => (double)_bytes / BytesPerGigabytes;

        public double TotalTeraBytes => (double)_bytes / BytesPerTeraBytes;

        public double TotalPetaBytes => (double)_bytes / BytesPerPetaBytes;

        public double TotalExaBytes => (double)_bytes / BytesPerExaBytes;

        public override int GetHashCode()
        {
            return _bytes.GetHashCode();
        }

        public bool Equals(BytesSpan other)
        {
            return _bytes == other._bytes;
        }

        public override bool Equals(object? obj)
        {
            return obj is BytesSpan bytesSpan && Equals(bytesSpan);
        }

        public int CompareTo(object? obj)
        {
            return _bytes.CompareTo(obj);
        }

        public int CompareTo(BytesSpan other)
        {
            return _bytes.CompareTo(other._bytes);
        }

        public static bool operator ==(BytesSpan left, BytesSpan right) => left._bytes == right._bytes;

        public static bool operator !=(BytesSpan left, BytesSpan right) => left._bytes != right._bytes;

        public static bool operator <(BytesSpan left, BytesSpan right) => left._bytes < right._bytes;

        public static bool operator <=(BytesSpan left, BytesSpan right) => left._bytes <= right._bytes;

        public static bool operator >(BytesSpan left, BytesSpan right) => left._bytes > right._bytes;

        public static bool operator >=(BytesSpan left, BytesSpan right) => left._bytes >= right._bytes;

        public static BytesSpan operator +(BytesSpan left, BytesSpan right) => new(left._bytes + right._bytes);

        public static BytesSpan operator -(BytesSpan left, BytesSpan right) => new(left._bytes - right._bytes);
    }
}
