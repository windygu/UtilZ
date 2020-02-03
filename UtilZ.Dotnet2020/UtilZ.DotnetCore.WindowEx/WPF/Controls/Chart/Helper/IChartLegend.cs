using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public interface IChartLegend
    {
        ChartDockOrientation DockOrientation { get; set; }

        /// <summary>
        /// 水平方向高度;垂直方向宽度
        /// </summary>
        double Size { get; set; }

        HorizontalAlignment HorizontalAlignment { get; set; }
        VerticalAlignment VerticalAlignment { get; set; }
        Thickness Margin { get; set; }
        FrameworkElement GetChartLegendControl();

        void UpdateLegend(List<SeriesLegendItem> legendBrushList);

        bool IsChecked { get; set; }
    }
}
