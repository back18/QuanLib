using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    /// <summary>
    /// 哈希算法类型
    /// </summary>
    public enum HashType
    {
        /// <summary>
        /// 160位SHA值
        /// </summary>
        SHA1,

        /// <summary>
        /// 256位SHA值
        /// </summary>
        SHA256,

        /// <summary>
        /// 384位SHA值
        /// </summary>
        SHA384,

        /// <summary>
        /// 512位SHA值
        /// </summary>
        SHA512,

        /// <summary>
        /// 信息摘要算法5
        /// </summary>
        MD5
    }
}
