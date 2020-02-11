using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class ChartNoAxisValue : ChartNoAxisValueAbs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="title"></param>
        /// <param name="style"></param>
        /// <param name="tooltip"></param>
        public ChartNoAxisValue(object value, string label, string title, Style style, string tooltip)
       : base(value, label, title, style, tooltip)
        {

        }
    }
}
