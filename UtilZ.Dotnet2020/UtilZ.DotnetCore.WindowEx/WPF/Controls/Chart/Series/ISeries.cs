using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public interface ISeries : INotifyPropertyChanged
    {
        AxisAbs AxisX { get; set; }

        AxisAbs AxisY { get; set; }

        Func<PointInfo, FrameworkElement> CreatePointFunc { get; set; }

        event NotifyCollectionChangedEventHandler ValuesCollectionChanged;
        ChartCollection<IChartItem> Values { get; set; }

        Style Style { get; set; }

        bool EnableTooltip { get; set; }

        double TooltipArea { get; set; }

        string Title { get; set; }

        void Add(Canvas canvas);

        /// <summary>
        /// 返回值:true:需要全部重绘;false:不需要重绘
        /// </summary>
        /// <returns></returns>
        bool Remove();

        void Update();

        void FillLegendItemToList(List<SeriesLegendItem> legendBrushList);

        Visibility Visibility { get; set; }

        object Tag { get; set; }
    }
}
