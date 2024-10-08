﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.TextFormat
{
    public class ChineseBytesUnitText : BytesUnitText
    {
        public static readonly ChineseBytesUnitText Default = new();

        public override string B => "字节";

        public override string KB => "千字节";

        public override string MB => "兆字节";

        public override string GB => "吉字节";

        public override string TB => "太字节";

        public override string PB => "拍字节";

        public override string EB => "艾字节";
    }
}
