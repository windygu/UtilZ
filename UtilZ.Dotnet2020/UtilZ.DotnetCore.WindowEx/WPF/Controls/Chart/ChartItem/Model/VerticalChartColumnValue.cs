using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class VerticalChartColumnValue : ChartAxisValueAbs
    {
        public object Value { get; private set; }
        public object Label { get; private set; }



        public VerticalChartColumnValue(object label, object value, string tooltip)
             : base(tooltip, null)
        {
            Label = label;
            Value = value;
        }

        public override object GetXValue()
        {
            return this.Label;
        }

        public override object GetYValue()
        {
            return this.Value;
        }
    }
}
