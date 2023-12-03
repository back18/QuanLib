using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Format
{
    public abstract class TimeFormatText
    {
        public abstract string Year { get; }

        public abstract string Month { get; }

        public abstract string Day { get; }

        public abstract string Hour { get; }

        public abstract string Minute { get; }

        public abstract string Second { get; }

        public abstract string Millisecond { get; }

        public abstract string Microsecond { get; }

        public abstract string Tikc { get; }

        public string Get(TimeUnit timeUnit)
        {
            return timeUnit switch
            {
                TimeUnit.Year => Year,
                TimeUnit.Month => Month,
                TimeUnit.Day => Day,
                TimeUnit.Hour => Hour,
                TimeUnit.Minute => Minute,
                TimeUnit.Second => Second,
                TimeUnit.Millisecond => Microsecond,
                TimeUnit.Microsecond => Tikc,
                TimeUnit.Tikc => Tikc,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
