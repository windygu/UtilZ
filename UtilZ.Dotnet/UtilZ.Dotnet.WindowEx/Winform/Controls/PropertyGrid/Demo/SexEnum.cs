using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base;
using UtilZ.Lib.Base.Foundation;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PropertyGrid.TypeConverters;
using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Dotnet.WindowEx.Winform.Controls.PropertyGrid.Demo
{
    /// <summary>
    /// PropertyGrid枚举
    /// </summary>
    public enum SexEnum
    {
        /// <summary>
        /// M
        /// </summary>
        [DisplayNameExAttribute(DisplayName = "男")]
        M,

        /// <summary>
        /// F
        /// </summary>
        //[NDisplayNameAttribute(DisplayName = "女")]
        F,

        /// <summary>
        /// O
        /// </summary>
        [DisplayNameExAttribute(DisplayName = "中性")]
        O
    }
}
