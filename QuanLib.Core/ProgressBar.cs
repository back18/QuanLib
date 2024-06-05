using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public struct ProgressBar
    {
        public ProgressBar(int total)
        {
            ThrowHelper.ArgumentOutOfMin(0, total, nameof(total));

            Total = total;
            Current = 0;
            Length = 10;
            ProgressCharacter = '■';
            BackgroundCharacter = '□';
        }

        public int Total { get; }

        public int Current { get; set; }

        public int Length { get; set; }

        public char ProgressCharacter { get; set; }

        public char BackgroundCharacter { get; set; }

        public override readonly string ToString()
        {
            if (Length <= 0)
                return string.Empty;

            if (Total == 0 || Current > Total)
                return new string(ProgressCharacter, Length);

            double progress = (double)Current / Total;
            int leftCount = (int)Math.Round(Length * progress);
            int rightCount = Length - leftCount;
            StringBuilder stringBuilder = new();
            stringBuilder.Append(ProgressCharacter, leftCount);
            stringBuilder.Append(BackgroundCharacter, rightCount);

            return stringBuilder.ToString();
        }
    }
}
