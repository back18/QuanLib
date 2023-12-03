using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Format
{
    public class EnglishBytesFormatText : BytesFormatText
    {
        public static readonly EnglishBytesFormatText Default = new();

        public override string B => "Byte";

        public override string KB => "KiloByte";

        public override string MB => "MegaByte";

        public override string GB => "Gigabyte";

        public override string TB => "TeraByte";

        public override string PB => "PetaByte";

        public override string EB => "ExaByte";
    }
}
