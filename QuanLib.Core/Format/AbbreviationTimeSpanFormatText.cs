using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Format
{
    public class AbbreviationTimeSpanFormatText : TimeFormatText
    {
        public static readonly AbbreviationTimeSpanFormatText Default = new();

        public override string Year => "y";

        public override string Month => "mo";

        public override string Day => "d";

        public override string Hour => "h";

        public override string Minute => "min";

        public override string Second => "s";

        public override string Millisecond => "ms";

        public override string Microsecond => "us";

        public override string Tikc => "t";
    }
}
