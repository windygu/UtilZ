using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class PointInfo
    {
        public Point Point { get; private set; }

        public IChartItem Item { get; private set; }

        public PointInfo(Point point, IChartItem item)
        {
            this.Point = point;
            this.Item = item;
        }
    }
}
