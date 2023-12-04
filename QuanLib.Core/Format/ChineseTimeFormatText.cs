using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Format
{
    public class ChineseTimeFormatText : TimeFormatText
    {
        public static readonly ChineseTimeFormatText Default = new();

        public override string Year => "年";

        public override string Month => "个月";

        public override string Day => "天";

        public override string Hour => "小时";

        public override string Minute => "分钟";

        public override string Second => "秒";

        public override string Millisecond => "毫秒";

        public override string Microsecond => "微妙";

        public override string Tikc => "刻";
    }
}
