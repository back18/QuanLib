using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TextFormat
{
    public class BytesFormatter
    {
        public BytesFormatter(BytesUnitText formatText)
        {
            UnitText = formatText;
            TextFormat = TextFormat.DynamicDecimals;
            _DigitCount = 3;
            MinUnit = BytesUnit.B;
        }

        public BytesUnitText UnitText { get; }

        public TextFormat TextFormat { get; set; }

        public BytesUnit MinUnit { get; set; }

        public int DigitCount
        {
            get => _DigitCount;
            set
            {
                ThrowHelper.ArgumentOutOfMin(0, value, nameof(value));
                _DigitCount = value;
            }
        }
        private int _DigitCount;

        public string Format(long bytes) => Format(new BytesSpan(bytes));

        public string Format(BytesSpan bytesSpan)
        {
            BytesUnit bytesUnit = GetMaxUnit(bytesSpan);
            if (bytesUnit < MinUnit)
                bytesUnit = MinUnit;

            return TextFormat switch
            {
                TextFormat.StaticDecimals => StaticDecimals(bytesSpan, bytesUnit),
                TextFormat.DynamicDecimals => DynamicDecimals(bytesSpan, bytesUnit),
                TextFormat.MultipleUnits => MultipleUnits(bytesSpan, bytesUnit),
                _ => throw new InvalidOperationException(),
            };
        }

        private string StaticDecimals(BytesSpan bytesSpan, BytesUnit bytesUnit)
        {
            double value = GetTotalOfUnit(bytesSpan, bytesUnit);
            value = Math.Round(value, DigitCount, MidpointRounding.ToNegativeInfinity);
            return value + UnitText.Get(bytesUnit);
        }

        private string DynamicDecimals(BytesSpan bytesSpan, BytesUnit bytesUnit)
        {
            double value = GetTotalOfUnit(bytesSpan, bytesUnit);
            int digits = Math.Max(0, DigitCount - 1 - (int)Math.Floor(Math.Log10(Math.Abs(value))));
            value = Math.Round(value, digits, MidpointRounding.ToNegativeInfinity);
            return value + UnitText.Get(bytesUnit);
        }

        private string MultipleUnits(BytesSpan bytesSpan, BytesUnit bytesUnit)
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i < DigitCount && bytesUnit >= MinUnit; i++, bytesUnit--)
            {
                stringBuilder.Append(GetValueOfUnit(bytesSpan, bytesUnit));
                stringBuilder.Append(UnitText.Get(bytesUnit));
            }
            return stringBuilder.ToString();
        }

        private static int GetValueOfUnit(BytesSpan bytesSpan, BytesUnit bytesUnit)
        {
            return bytesUnit switch
            {
                BytesUnit.B => bytesSpan.Bytes,
                BytesUnit.KB => bytesSpan.KiloBytes,
                BytesUnit.MB => bytesSpan.MegaBytes,
                BytesUnit.GB => bytesSpan.Gigabytes,
                BytesUnit.TB => bytesSpan.TeraBytes,
                BytesUnit.PB => bytesSpan.PetaBytes,
                BytesUnit.EB => bytesSpan.ExaBytes,
                _ => throw new InvalidEnumArgumentException(nameof(bytesUnit), (int)bytesUnit, typeof(BytesUnit)),
            };
        }

        private static double GetTotalOfUnit(BytesSpan bytesSpan, BytesUnit bytesUnit)
        {
            return bytesUnit switch
            {
                BytesUnit.B => bytesSpan.TotalBytes,
                BytesUnit.KB => bytesSpan.TotalKiloBytes,
                BytesUnit.MB => bytesSpan.TotalMegaBytes,
                BytesUnit.GB => bytesSpan.TotalGigaBytes,
                BytesUnit.TB => bytesSpan.TotalTeraBytes,
                BytesUnit.PB => bytesSpan.TotalPetaBytes,
                BytesUnit.EB => bytesSpan.TotalExaBytes,
                _ => throw new InvalidEnumArgumentException(nameof(bytesUnit), (int)bytesUnit, typeof(BytesUnit)),
            };
        }

        private static BytesUnit GetMaxUnit(BytesSpan bytesSpan)
        {
            BytesUnit bytesUnit;
            if (bytesSpan.TotalBytes < BytesSpan.BytesPerKiloBytes)
                bytesUnit = BytesUnit.B;
            else if (bytesSpan.TotalBytes < BytesSpan.BytesPerMegaBytes)
                bytesUnit = BytesUnit.KB;
            else if (bytesSpan.TotalBytes < BytesSpan.BytesPerGigaBytes)
                bytesUnit = BytesUnit.MB;
            else if (bytesSpan.TotalBytes < BytesSpan.BytesPerTeraBytes)
                bytesUnit = BytesUnit.GB;
            else if (bytesSpan.TotalBytes < BytesSpan.BytesPerPetaBytes)
                bytesUnit = BytesUnit.TB;
            else if (bytesSpan.TotalBytes < BytesSpan.BytesPerExaBytes)
                bytesUnit = BytesUnit.PB;
            else
                bytesUnit = BytesUnit.EB;
            return bytesUnit;
        }
    }
}
