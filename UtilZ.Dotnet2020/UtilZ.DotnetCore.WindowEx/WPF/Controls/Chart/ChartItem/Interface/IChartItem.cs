using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    /// <summary>
    /// Chart项接口
    /// </summary>
    public interface IChartItem
    {
        /// <summary>
        /// Series上点显示的TooltipText
        /// </summary>
        string TooltipText { get; }

        /// <summary>
        /// Tag
        /// </summary>
        object Tag { get; }
    }
}
