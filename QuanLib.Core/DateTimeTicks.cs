using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    /// <summary>
    /// 存储日期时间的刻度数
    /// </summary>
    public struct DateTimeTicks
    {
        /// <summary>
        /// 刻
        /// </summary>
        public const long Ticks = 1;

        /// <summary>
        /// 毫秒
        /// </summary>
        public const long Millisecond = 10000;

        /// <summary>
        /// 秒
        /// </summary>
        public const long Second = 10000000;

        /// <summary>
        /// 分
        /// </summary>
        public const long Minute = 600000000;

        /// <summary>
        /// 时
        /// </summary>
        public const long Hour = 36000000000;

        /// <summary>
        /// 日
        /// </summary>
        public const long Day = 864000000000;

        /// <summary>
        /// 年
        /// </summary>
        public const long Year = 315360000000000;
    }
}
