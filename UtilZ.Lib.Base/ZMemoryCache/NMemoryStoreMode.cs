using UtilZ.Lib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.ZMemoryCache
{
    /// <summary>
    /// 存储模式
    /// </summary>
    public enum NMemoryStoreMode
    {
        /// <summary>
        /// 添加
        /// </summary>
        [NDisplayNameAttribute("添加")]
        Add = 1,

        /// <summary>
        /// 替换
        /// </summary>
        [NDisplayNameAttribute("替换")]
        Replace = 2,

        /// <summary>
        /// 设置[不存在该项则添加,存在则替换旧的值]
        /// </summary>
        [NDisplayNameAttribute("设置")]
        Set = 3,
    }
}
