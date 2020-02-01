using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public interface ISeries : INotifyPropertyChanged
    {
        AxisAbs AxisX { get; set; }

        AxisAbs AxisY { get; set; }

        Func<PointInfo, FrameworkElement> CreatePointFunc { get; set; }

        event NotifyCollectionChangedEventHandler ValuesCollectionChanged;
        ChartCollection<IChartItem> Values { get; set; }

        void GetAxisValueArea(AxisAbs axis, out double min, out double max);

        Style Style { get; set; }

        string Title { get; set; }

        void Draw(Canvas canvas, Rect chartArea);

        void Clear();

        void Update();

        void FillLegendItemToList(List<SeriesLegendItem> legendBrushList);

        Visibility Visibility { get; set; }

        object Tag { get; set; }
    }
}
