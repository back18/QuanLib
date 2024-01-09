using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TextFormat
{
    public class BytesFormatter
    {
        public BytesFormatter(BytesFormatText formatText)
        {
            FormatText = formatText;
            UnitCount = 1;
            MinUnit = BytesUnit.B;
        }

        public BytesFormatText FormatText { get; }

        public int UnitCount { get; set; }

        public BytesUnit MinUnit { get; set; }

        public string Format(long bytes) => Format(new BytesSpan(bytes));

        public string Format(BytesSpan value)
        {
            BytesUnit bytesUnit;
            if (value.TotalBytes < BytesSpan.BytesPerKiloBytes)
                bytesUnit = BytesUnit.B;
            else if (value.TotalBytes < BytesSpan.BytesPerMegaBytes)
                bytesUnit = BytesUnit.KB;
            else if (value.TotalBytes < BytesSpan.BytesPerGigabytes)
                bytesUnit = BytesUnit.MB;
            else if (value.TotalBytes < BytesSpan.BytesPerTeraBytes)
                bytesUnit = BytesUnit.GB;
            else if (value.TotalBytes < BytesSpan.BytesPerPetaBytes)
                bytesUnit = BytesUnit.TB;
            else if (value.TotalBytes < BytesSpan.BytesPerExaBytes)
                bytesUnit = BytesUnit.PB;
            else
                bytesUnit = BytesUnit.EB;

            if (bytesUnit < MinUnit)
                bytesUnit = MinUnit;

            StringBuilder sb = new();
            for (int i = 0; i < UnitCount; i++)
            {
                sb.Append(Format(value, bytesUnit));

                if (bytesUnit == MinUnit)
                    break;
                else
                    bytesUnit--;
            }

            return sb.ToString();
        }

        private string Format(BytesSpan value, BytesUnit bytesUnit)
        {
            return bytesUnit switch
            {
                BytesUnit.B => value.Bytes + FormatText.B,
                BytesUnit.KB => value.KiloBytes + FormatText.KB,
                BytesUnit.MB => value.MegaBytes + FormatText.MB,
                BytesUnit.GB => value.Gigabytes + FormatText.GB,
                BytesUnit.TB => value.TeraBytes + FormatText.TB,
                BytesUnit.PB => value.PetaBytes + FormatText.PB,
                BytesUnit.EB => value.ExaBytes + FormatText.EB,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
