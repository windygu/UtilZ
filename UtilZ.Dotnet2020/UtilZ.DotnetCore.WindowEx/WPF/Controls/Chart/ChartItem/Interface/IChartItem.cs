using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public interface IChartItem
    {
        string TooltipText { get; }

        object Tag { get; }
    }
}
