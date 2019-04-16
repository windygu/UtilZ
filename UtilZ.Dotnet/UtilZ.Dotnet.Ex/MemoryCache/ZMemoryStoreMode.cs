using UtilZ.Dotnet.Ex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Attributes;

namespace UtilZ.Dotnet.Ex.MemoryCache
{
    /// <summary>
    /// 存储模式
    /// </summary>
    public enum ZMemoryStoreMode
    {
        /// <summary>
        /// 添加
        /// </summary>
        [DisplayNameExAttribute("添加")]
        Add = 1,

        /// <summary>
        /// 替换
        /// </summary>
        [DisplayNameExAttribute("替换")]
        Replace = 2,

        /// <summary>
        /// 设置[不存在该项则添加,存在则替换旧的值]
        /// </summary>
        [DisplayNameExAttribute("设置")]
        Set = 3,
    }
}
