using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TextFormat
{
    public class AbbreviationTimeUnitText : TimeUnitText
    {
        public static readonly AbbreviationTimeUnitText Default = new();

        public override string Tikc => "t";

        public override string Microsecond => "us";

        public override string Millisecond => "ms";

        public override string Second => "s";

        public override string Minute => "min";

        public override string Hour => "h";

        public override string Day => "d";

        public override string Month => "mo";

        public override string Year => "y";
    }
}
