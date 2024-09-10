using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TextFormat
{
    public class TimeFormatter
    {
        private const long TicksPerYear = TimeSpan.TicksPerDay * 365;

        public TimeFormatter(TimeUnitText formatText)
        {
            UnitText = formatText;
            TextFormat = TextFormat.MultipleUnits;
            MinUnit = TimeUnit.Second;
            DigitCount = 2;
        }

        public TimeUnitText UnitText { get; }

        public TextFormat TextFormat { get; set; }

        public TimeUnit MinUnit { get; set; }

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

        public string Format(long ticks) => Format(new TimeSpan(ticks));

        public string Format(TimeSpan timeSpan)
        {
            TimeUnit timeUnit = GetMaxUnit(timeSpan);
            if (timeUnit < MinUnit)
                timeUnit = MinUnit;

            return TextFormat switch
            {
                TextFormat.StaticDecimals => StaticDecimals(timeSpan, timeUnit),
                TextFormat.DynamicDecimals => DynamicDecimals(timeSpan, timeUnit),
                TextFormat.MultipleUnits => MultipleUnits(timeSpan, timeUnit),
                _ => throw new InvalidOperationException(),
            };
        }

        private string StaticDecimals(TimeSpan timeSpan, TimeUnit timeUnit)
        {
            double value = GetTotalOfUnit(timeSpan, timeUnit);
            value = Math.Round(value, DigitCount, MidpointRounding.ToNegativeInfinity);
            return value + UnitText.Get(timeUnit);
        }

        private string DynamicDecimals(TimeSpan timeSpan, TimeUnit timeUnit)
        {
            double value = GetTotalOfUnit(timeSpan, timeUnit);
            int digits = Math.Max(0, DigitCount - 1 - (int)Math.Floor(Math.Log10(Math.Abs(value))));
            value = Math.Round(value, digits, MidpointRounding.ToNegativeInfinity);
            return value + UnitText.Get(timeUnit);
        }

        private string MultipleUnits(TimeSpan timeSpan, TimeUnit timeUnit)
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i < DigitCount && timeUnit >= MinUnit; i++, timeUnit--)
            {
                stringBuilder.Append(GetValueOfUnit(timeSpan, timeUnit));
                stringBuilder.Append(UnitText.Get(timeUnit));
            }
            return stringBuilder.ToString();
        }

        private static int GetValueOfUnit(TimeSpan timeSpan, TimeUnit timeUnit)
        {
            return timeUnit switch
            {
                TimeUnit.Tikc => GetCurrentTicks(timeSpan),
                TimeUnit.Microsecond => timeSpan.Microseconds,
                TimeUnit.Millisecond => timeSpan.Milliseconds,
                TimeUnit.Minute => timeSpan.Minutes,
                TimeUnit.Second => timeSpan.Seconds,
                TimeUnit.Hour => timeSpan.Hours,
                TimeUnit.Day => timeSpan.Days,
                TimeUnit.Year => GetTotalYears(timeSpan),
                _ => throw new InvalidEnumArgumentException(nameof(timeUnit), (int)timeUnit, typeof(TimeUnit)),
            };
        }

        private static double GetTotalOfUnit(TimeSpan timeSpan, TimeUnit timeUnit)
        {
            return timeUnit switch
            {
                TimeUnit.Tikc => timeSpan.Ticks,
                TimeUnit.Microsecond => timeSpan.TotalMicroseconds,
                TimeUnit.Millisecond => timeSpan.TotalMilliseconds,
                TimeUnit.Minute => timeSpan.TotalMinutes,
                TimeUnit.Second => timeSpan.TotalSeconds,
                TimeUnit.Hour => timeSpan.TotalHours,
                TimeUnit.Day => timeSpan.TotalDays,
                TimeUnit.Year => GetTotalYears(timeSpan),
                _ => throw new InvalidEnumArgumentException(nameof(timeUnit), (int)timeUnit, typeof(TimeUnit)),
            };
        }

        private static TimeUnit GetMaxUnit(TimeSpan value)
        {
            TimeUnit timeUnit;
            if (value.Ticks < TimeSpan.TicksPerMicrosecond)
                timeUnit = TimeUnit.Tikc;
            else if (value.Ticks < TimeSpan.TicksPerMillisecond)
                timeUnit = TimeUnit.Microsecond;
            else if (value.Ticks < TimeSpan.TicksPerSecond)
                timeUnit = TimeUnit.Millisecond;
            else if (value.Ticks < TimeSpan.TicksPerMinute)
                timeUnit = TimeUnit.Second;
            else if (value.Ticks < TimeSpan.TicksPerHour)
                timeUnit = TimeUnit.Minute;
            else if (value.Ticks < TimeSpan.TicksPerDay)
                timeUnit = TimeUnit.Hour;
            else if (value.Ticks < TicksPerYear)
                timeUnit = TimeUnit.Day;
            else
                timeUnit = TimeUnit.Year;
            return timeUnit;
        }

        private static int GetCurrentTicks(TimeSpan timeSpan) => (int)(timeSpan.Ticks % TimeSpan.TicksPerMicrosecond);

        private static int GetTotalYears(TimeSpan timeSpan) => (int)(timeSpan.Ticks / TicksPerYear);
    }
}
