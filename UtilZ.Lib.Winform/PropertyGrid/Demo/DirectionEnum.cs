using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base;

namespace UtilZ.Lib.Winform.PropertyGrid.Demo
{
    /// <summary>
    /// PropertyGrid枚举
    /// </summary>
    public enum DirectionEnum
    {
        /// <summary>
        /// 左
        /// </summary>
        [NDisplayNameAttribute(DisplayName = "左")]
        Left,

        /// <summary>
        /// 右
        /// </summary>
        [NDisplayNameAttribute(DisplayName = "右")]
        Right,

        /// <summary>
        /// 上
        /// </summary>
        //[NDisplayNameAttribute(DisplayName = "上")]
        Up,

        /// <summary>
        /// 下
        /// </summary>
        //[NDisplayNameAttribute(DisplayName = "下")]
        Down
    }
}
