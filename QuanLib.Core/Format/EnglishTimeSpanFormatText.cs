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

        public override string Year => "year";

        public override string Month => "month";

        public override string Day => "day";

        public override string Hour => "hour";

        public override string Minute => "minute";

        public override string Second => "second";

        public override string Millisecond => "millisecond";

        public override string Microsecond => "microsecond";

        public override string Tikc => "tick";
    }
}
