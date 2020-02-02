using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class DateTimeAxis : AxisAbs
    {
        public DateTimeAxis()
            : base()
        {

        }

        protected override void PrimitiveDrawX(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection, double axisWidth)
        {
            throw new NotImplementedException();
        }

        protected override double PrimitiveDrawY(Canvas axisCanvas, ChartCollection<ISeries> seriesCollection, double axisHeight)
        {
            throw new NotImplementedException();
        }

        protected override double PrimitiveGetX(IChartItem chartItem)
        {
            throw new NotImplementedException();
        }

        protected override double PrimitiveGetXAxisHeight()
        {
            throw new NotImplementedException();
        }

        protected override double PrimitiveGetY(IChartItem chartItem)
        {
            throw new NotImplementedException();
        }
    }
}
