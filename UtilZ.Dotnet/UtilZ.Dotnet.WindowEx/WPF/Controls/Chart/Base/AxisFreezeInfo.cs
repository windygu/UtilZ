using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    internal class AxisFreezeInfo
    {
        public double ChartWidth { get; private set; }

        public double ChartHeight { get; private set; }

        public AxisFreezeType AxisFreezeType { get; private set; }

        public AxisFreezeInfo(double chartWidth, double chartHeight, AxisFreezeType axisFreezeType)
        {
            this.ChartWidth = chartWidth;
            this.ChartHeight = chartHeight;
            this.AxisFreezeType = axisFreezeType;
        }
    }
}
