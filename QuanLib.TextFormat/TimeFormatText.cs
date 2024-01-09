using QuanLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TextFormat
{
    public abstract class TimeFormatText
    {
        public abstract string Tikc { get; }

        public abstract string Microsecond { get; }

        public abstract string Millisecond { get; }

        public abstract string Second { get; }

        public abstract string Minute { get; }

        public abstract string Hour { get; }

        public abstract string Day { get; }

        public abstract string Month { get; }

        public abstract string Year { get; }

        public string Get(TimeUnit timeUnit)
        {
            return timeUnit switch
            {
                TimeUnit.Tikc => Tikc,
                TimeUnit.Microsecond => Microsecond,
                TimeUnit.Millisecond => Millisecond,
                TimeUnit.Second => Second,
                TimeUnit.Minute => Minute,
                TimeUnit.Hour => Hour,
                TimeUnit.Day => Day,
                TimeUnit.Month => Month,
                TimeUnit.Year => Year,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
