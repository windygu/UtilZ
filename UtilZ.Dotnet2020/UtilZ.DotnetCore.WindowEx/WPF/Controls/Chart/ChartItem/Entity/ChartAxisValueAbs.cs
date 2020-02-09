using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public abstract class ChartAxisValueAbs : ChartValueAbs, IChartAxisValue
    {
        public ChartAxisValueAbs()
              : base()
        {

        }

        public ChartAxisValueAbs(string tooltipText, object tag)
            : base(tooltipText, tag)
        {

        }

        public abstract object GetXValue();

        public abstract object GetYValue();
    }
}
