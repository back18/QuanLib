using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TextFormat
{
    public class EnglishBytesFormatText : BytesFormatText
    {
        public static readonly EnglishBytesFormatText Default = new();

        public override string B => "Bytes";

        public override string KB => "KiloBytes";

        public override string MB => "MegaBytes";

        public override string GB => "Gigabytes";

        public override string TB => "TeraBytes";

        public override string PB => "PetaBytes";

        public override string EB => "ExaBytes";
    }
}
