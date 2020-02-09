using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class ChartAxisValue : ChartAxisValueAbs
    {
        public object XValue { get; private set; }
        public object YValue { get; private set; }


        public ChartAxisValue(object xValue, object yValue, string tooltipText)
          : base(tooltipText, null)
        {
            this.XValue = xValue;
            this.YValue = yValue;
        }

        public override object GetXValue()
        {
            return this.XValue;
        }

        public override object GetYValue()
        {
            return this.YValue;
        }

        public override string ToString()
        {
            return $"XValue:{XValue},YValue:{YValue}";
        }
    }
}
