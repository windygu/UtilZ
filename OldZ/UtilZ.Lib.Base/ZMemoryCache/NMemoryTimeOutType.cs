using UtilZ.Lib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Foundation;

namespace UtilZ.Lib.Base.ZMemoryCache
{
    /// <summary>
    /// 超时时间类型
    /// </summary>
    public enum NMemoryTimeOutType
    {
        /// <summary>
        /// 有效值到达指定时刻
        /// </summary>
        [DisplayNameExAttribute("有效值到达指定时刻")]
        ExpiresAt = 1,

        /// <summary>
        /// 有效时长
        /// </summary>
        [DisplayNameExAttribute("有效时长")]
        ValidFor = 2,

        /// <summary>
        /// 从不
        /// </summary>
        [DisplayNameExAttribute("从不")]
        Never = 3
    }
}
