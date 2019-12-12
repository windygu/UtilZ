using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public interface IChartLegend
    {
        AxisDockOrientation ChartLegendOrientation { get; set; }

        double HorizontalWidth { get; }

        double VerticalHeight { get; }
        FrameworkElement GetChartLegendControl();

        void UpdateLegend(List<SeriesLegendItem> legendBrushList);
    }
}
