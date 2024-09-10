using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TextFormat
{
    public class EnglishBytesUnitText : BytesUnitText
    {
        public static readonly EnglishBytesUnitText Default = new();

        public override string B => "Bytes";

        public override string KB => "KiloBytes";

        public override string MB => "MegaBytes";

        public override string GB => "Gigabytes";

        public override string TB => "TeraBytes";

        public override string PB => "PetaBytes";

        public override string EB => "ExaBytes";
    }
}
