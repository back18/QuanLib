using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Format
{
    public class AbbreviationBytesFormatText : BytesFormatText
    {
        public static readonly AbbreviationBytesFormatText Default = new();

        public override string B => "B";

        public override string KB => "K";

        public override string MB => "M";

        public override string GB => "G";

        public override string TB => "T";

        public override string PB => "P";

        public override string EB => "E";
    }
}
