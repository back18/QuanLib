using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TextFormat
{
    public class EnglishTimeUnitText : TimeUnitText
    {
        public static readonly EnglishTimeUnitText Default = new();

        public override string Tikc => "Tikcs";

        public override string Microsecond => "Microseconds";

        public override string Millisecond => "Milliseconds";

        public override string Second => "Seconds";

        public override string Minute => "Minutes";

        public override string Hour => "Hours";

        public override string Day => "Days";

        public override string Month => "Months";

        public override string Year => "Years";
    }
}
