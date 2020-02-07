using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public abstract class ChartValueAbs : ChartItemAbs, IChartValue
    {
        public ChartValueAbs()
            : base()
        {

        }

        public ChartValueAbs(string tooltipText, object tag)
            : base(tooltipText, tag)
        {

        }

        public abstract object GetXValue();

        public abstract object GetYValue();
    }
}
