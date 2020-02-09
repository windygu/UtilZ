using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class ChartChildValue : ChartChildValueAbs
    {
        public object Value { get; private set; }

        public ChartChildValue(object value)
            : base()
        {
            this.Value = value;
        }

        public ChartChildValue(object value, string tooltipText, object tag)
            : base(tooltipText, tag)
        {
            this.Value = value;
        }

        public override object GetValue()
        {
            return this.Value;
        }
    }
}
