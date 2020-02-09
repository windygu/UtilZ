using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public abstract class ChartItemAbs : IChartItem
    {
        public string TooltipText { get; set; }

        public object Tag { get; set; }

        public ChartItemAbs()
        {

        }

        public ChartItemAbs(string tooltipText, object tag)
        {
            this.TooltipText = tooltipText;
            this.Tag = tag;
        }
    }
}
