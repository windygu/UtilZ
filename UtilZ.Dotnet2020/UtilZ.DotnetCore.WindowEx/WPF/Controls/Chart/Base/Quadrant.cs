using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    /// <summary>
    /// XY坐标系象限类型枚举
    /// </summary>
    public enum Quadrant
    {
        /// <summary>
        /// 第一象限[x>0,y>0]
        /// </summary>
        One,

        /// <summary>
        /// 第二象限[x<0,y>0]
        /// </summary>
        Two,

        /// <summary>
        /// 第三象限[x<0,y<0]
        /// </summary>
        Three,

        /// <summary>
        /// 第四象限[x>0,y<0]
        /// </summary>
        Four
    }
}
