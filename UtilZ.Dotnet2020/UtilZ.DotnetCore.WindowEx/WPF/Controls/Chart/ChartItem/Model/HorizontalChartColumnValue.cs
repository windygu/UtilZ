using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class HorizontalChartColumnValue : ChartAxisValueAbs
    {
        public object Label { get; private set; }
        public object Value { get; private set; }


        public HorizontalChartColumnValue(object label, object value, string tooltip)
            : base(tooltip, null)
        {
            Value = value;
            Label = label;
        }

        public override object GetXValue()
        {
            return this.Value;
        }

        public override object GetYValue()
        {
            return this.Label;
        }
    }
}
