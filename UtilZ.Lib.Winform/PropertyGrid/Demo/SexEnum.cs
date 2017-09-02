using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base;
using UtilZ.Lib.Winform.PropertyGrid.TypeConverters;

namespace UtilZ.Lib.Winform.PropertyGrid.Demo
{
    /// <summary>
    /// PropertyGrid枚举
    /// </summary>
    public enum SexEnum
    {
        /// <summary>
        /// M
        /// </summary>
        [NDisplayNameAttribute(DisplayName = "男")]
        M,

        /// <summary>
        /// F
        /// </summary>
        //[NDisplayNameAttribute(DisplayName = "女")]
        F,

        /// <summary>
        /// O
        /// </summary>
        [NDisplayNameAttribute(DisplayName = "中性")]
        O
    }
}
