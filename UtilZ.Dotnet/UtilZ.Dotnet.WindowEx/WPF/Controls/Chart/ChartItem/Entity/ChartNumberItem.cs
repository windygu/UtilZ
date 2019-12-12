using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public class ChartNumberItem : ChartItemAbs, IChartNumberItem
    {
        public double AxisValue { get; set; }


        public ChartNumberItem(double axisXValue, double value, string tooltipText)
          : base(value, tooltipText)
        {
            this.AxisValue = axisXValue;
        }

        public override string ToString()
        {
            return $"{AxisValue} {Value}";
        }
    }
}
