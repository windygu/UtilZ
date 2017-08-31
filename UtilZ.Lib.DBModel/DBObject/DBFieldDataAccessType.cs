using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base;

namespace UtilZ.Lib.DBModel.DBObject
{
    /// <summary>
    /// 字段数据访问类型
    /// </summary>
    public enum DBFieldDataAccessType
    {
        //R:读;I:插入;M:修改

        /// <summary>
        /// 读写改
        /// </summary>
        [NDisplayNameAttribute("读写改")]
        RIM,

        /// <summary>
        /// 读改
        /// </summary>
        [NDisplayNameAttribute("读改")]
        RM,

        /// <summary>
        /// 只读
        /// </summary>
        [NDisplayNameAttribute("只读")]
        R,

        /// <summary>
        /// 插入读
        /// </summary>
        [NDisplayNameAttribute("插入读")]
        IR,

        /// <summary>
        /// 只写
        /// </summary>
        [NDisplayNameAttribute("只写")]
        I,

        /// <summary>
        /// 改写
        /// </summary>
        [NDisplayNameAttribute("改写")]
        IM
    }
}
