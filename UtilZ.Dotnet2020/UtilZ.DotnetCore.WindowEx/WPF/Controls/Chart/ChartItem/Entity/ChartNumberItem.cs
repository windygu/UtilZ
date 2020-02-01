using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class ChartNumberItem : ChartItemAbs, IChartNumberItem
    {
        public double AxisXValue { get; set; }


        public ChartNumberItem(double axisXValue, double value, string tooltipText)
          : base(value, tooltipText)
        {
            this.AxisXValue = axisXValue;
        }

        public override string ToString()
        {
            return $"{AxisXValue} {Value}";
        }
    }
}
