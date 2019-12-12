using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    /// <summary>
    /// VerticalChartLegend.xaml 的交互逻辑
    /// </summary>
    public partial class VerticalChartLegend : UserControl, IChartLegend
    {
        public VerticalChartLegend()
        {
            InitializeComponent();
        }

        private AxisDockOrientation _chartLegendOrientation = AxisDockOrientation.Right;
        public AxisDockOrientation ChartLegendOrientation
        {
            get { return this._chartLegendOrientation; }
            set
            {
                if (this._chartLegendOrientation == value)
                {
                    return;
                }

                this._chartLegendOrientation = value;
                //todo..
            }
        }

        public double HorizontalWidth
        {
            get { return this.Width; }
        }

        public double VerticalHeight
        {
            get { throw new NotSupportedException(); }
        }


        public FrameworkElement GetChartLegendControl()
        {
            return this;
        }

        public void UpdateLegend(List<SeriesLegendItem> legendBrushList)
        {
            itemsControl.ItemsSource = legendBrushList;
        }
    }
}
