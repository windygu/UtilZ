using UtilZ.Lib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Winform.OriginalDataShow
{
    /// <summary>
    /// 原始数据显示类型
    /// </summary>
    public enum OriginalDataShowType
    {
        /// <summary>
        /// ASC编码
        /// </summary>
        [NDisplayNameAttribute("ASC编码")]
        ASC,

        /// <summary>
        /// Unicode编码
        /// </summary>
        [NDisplayNameAttribute("Unicode编码")]
        Unicode,

        /// <summary>
        /// 二进制
        /// </summary>
        [NDisplayNameAttribute("二进制")]
        Bin,

        /// <summary>
        /// 十六进制
        /// </summary>
        [NDisplayNameAttribute("十六进制")]
        Hex
    }
}
