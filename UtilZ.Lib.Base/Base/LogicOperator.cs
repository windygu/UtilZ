using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base
{
    /// <summary>
    /// 逻辑运算符枚举
    /// </summary>
    public enum LogicOperator
    {
        /// <summary>
        /// 逻辑运算符且
        /// </summary>
        [NDisplayNameAttribute("且")]
        AND,

        /// <summary>
        /// 逻辑运算符或
        /// </summary>
        [NDisplayNameAttribute("或")]
        OR
    }
}
