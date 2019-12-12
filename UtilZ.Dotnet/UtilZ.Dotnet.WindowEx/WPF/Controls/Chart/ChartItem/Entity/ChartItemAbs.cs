using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public abstract class ChartItemAbs : IChartItem
    {
        public virtual double Value { get; set; }

        public string TooltipText { get; set; }

        public object Tag { get; set; }

        public ChartItemAbs()
            : base()
        {

        }

        public ChartItemAbs(double value, string tooltipText)
            : this(tooltipText, null)
        {
            this.Value = value;
        }

        public ChartItemAbs(string tooltipText, object tag)
            : base()
        {
            this.TooltipText = tooltipText;
            this.Tag = tag;
        }
    }
}
