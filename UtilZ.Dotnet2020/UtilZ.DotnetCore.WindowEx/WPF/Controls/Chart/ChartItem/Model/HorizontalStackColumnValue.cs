using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class HorizontalStackColumnValue : ChartAxisValueAbs
    {
        public object Label { get; private set; }
        public List<IChartChildValue> Values { get; private set; }

        public HorizontalStackColumnValue(object label, List<IChartChildValue> values)
            : base()
        {
            Values = values;
            Label = label; 
        }

        public override object GetXValue()
        {
            return this.Values;
        }

        public override object GetYValue()
        {
            return this.Label;
        }
    }
}
