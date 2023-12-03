using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Format
{
    public class TimeSpanFormatter
    {
        private const long TicksPerYear = TimeSpan.TicksPerDay * 365;

        public TimeSpanFormatter(TimeFormatText formatText)
        {
            FormatText = formatText;
            UnitCount = 1;
            MinUnit = TimeUnit.Tikc;
        }

        public TimeFormatText FormatText { get; }

        public int UnitCount { get; set; }

        public TimeUnit MinUnit { get; set; }

        public string Format(TimeSpan value)
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

            StringBuilder sb = new();
            for (int i = 0; i < UnitCount; i++)
            {
                if (timeUnit == TimeUnit.Month)
                    timeUnit--;

                sb.Append(Format(value, timeUnit));

                if (timeUnit == MinUnit)
                    break;
                else
                    timeUnit--;
            }

            return sb.ToString();
        }

        private string Format(TimeSpan value, TimeUnit timeUnit)
        {
            return timeUnit switch
            {
                TimeUnit.Year => GetYears(value) + FormatText.Year,
                TimeUnit.Month => throw new InvalidOperationException("不支持月份单位"),
                TimeUnit.Day => value.Days + FormatText.Day,
                TimeUnit.Hour => value.Hours + FormatText.Hour,
                TimeUnit.Minute => value.Minutes + FormatText.Minute,
                TimeUnit.Second => value.Seconds + FormatText.Second,
                TimeUnit.Millisecond => value.Milliseconds + FormatText.Millisecond,
                TimeUnit.Microsecond => value.Microseconds + FormatText.Microsecond,
                TimeUnit.Tikc => GetTicks(value) + FormatText.Tikc,
                _ => throw new InvalidOperationException(),
            };
        }

        private static long GetYears(TimeSpan value) => value.Ticks / TicksPerYear;

        private static long GetTicks(TimeSpan value) => value.Ticks % TimeSpan.TicksPerMicrosecond;
    }
}
