using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Format
{
    public class EnglishTimeSpanFormatText : TimeFormatText
    {
        public static readonly EnglishTimeSpanFormatText Default = new();

        public override string Tikc => "tick";

        public override string Microsecond => "microsecond";

        public override string Millisecond => "millisecond";

        public override string Second => "second";

        public override string Minute => "minute";

        public override string Hour => "hour";

        public override string Day => "day";

        public override string Month => "month";

        public override string Year => "year";
    }
}
