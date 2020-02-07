using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public abstract class ChartChildValueAbs : ChartItemAbs, IChartChildValue
    {
        public ChartChildValueAbs()
            : base()
        {

        }

        public ChartChildValueAbs(string tooltipText, object tag)
            : base(tooltipText, tag)
        {

        }

        public abstract object GetValue();
    }
}
