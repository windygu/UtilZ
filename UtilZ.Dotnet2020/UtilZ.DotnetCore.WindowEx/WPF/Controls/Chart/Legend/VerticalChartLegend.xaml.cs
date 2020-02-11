using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    /// <summary>
    /// VerticalChartLegend.xaml 的交互逻辑
    /// </summary>
    public partial class VerticalChartLegend : UserControl, IChartLegend
    {
        public VerticalChartLegend()
        {
            InitializeComponent();

            this.UpdateItemsControlStytle();
        }

        public ChartDockOrientation DockOrientation { get; set; } = ChartDockOrientation.Right;
        public double Size
        {
            get
            {
                return this.Width;
            }
            set
            {
                this.Width = value;
            }
        }


        public bool IsChecked
        {
            get
            {
                return itemsControl.IsEnabled;
            }
            set
            {
                if (itemsControl.IsEnabled == value)
                {
                    return;
                }

                itemsControl.IsEnabled = value;
                this.UpdateItemsControlStytle();
            }
        }

        private void UpdateItemsControlStytle()
        {
            if (itemsControl.IsEnabled)
            {
                itemsControl.Style = this.Resources[ChartLegendResourceDictionaryKeyConstant.ALOOW_EDIT_LEGEND_ITEMCONTROL_STYLE_KEY] as Style;
            }
            else
            {
                itemsControl.Style = this.Resources[ChartLegendResourceDictionaryKeyConstant.NO_EDIT_LEGEND_ITEMCONTROL_STYLE_KEY] as Style;
            }
        }

        public FrameworkElement LegendControl
        {
            get { return this; }
        }

        public void UpdateLegend(List<SeriesLegendItem> legendBrushList)
        {
            itemsControl.ItemsSource = legendBrushList;
        }
    }
}
