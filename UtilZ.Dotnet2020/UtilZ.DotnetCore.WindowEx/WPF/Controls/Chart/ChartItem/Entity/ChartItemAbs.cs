using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
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

        public abstract object GetXValue();

        public abstract object GetYValue();
    }
}
