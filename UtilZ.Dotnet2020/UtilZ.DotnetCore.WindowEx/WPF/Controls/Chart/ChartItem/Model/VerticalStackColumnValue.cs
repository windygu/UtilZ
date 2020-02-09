using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class VerticalStackColumnValue : ChartAxisValueAbs
    {
        public List<IChartChildValue> Values { get; private set; }
        public object Label { get; private set; }


        public VerticalStackColumnValue(object label, List<IChartChildValue> values)
             : base()
        {
            Label = label;
            Values = values;
        }

        public override object GetXValue()
        {
            return this.Label;
        }

        public override object GetYValue()
        {
            return this.Values;
        }
    }
}
