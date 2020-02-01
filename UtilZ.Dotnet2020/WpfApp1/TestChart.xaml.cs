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
using System.Windows.Shapes;
using UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart;
using UtilZ.DotnetStd.Ex.Model;

namespace WpfApp1
{
    /// <summary>
    /// TestChart.xaml 的交互逻辑
    /// </summary>
    public partial class TestChart : Window
    {
        public TestChart()
        {
            InitializeComponent();
        }

        private TestChartVM _vm;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._vm = (TestChartVM)this.DataContext;
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            this._vm.Test();
        }
    }

    internal class TestChartVM : BaseModelAbs
    {
        private ChartCollection<ISeries> _series = null;
        public ChartCollection<ISeries> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                base.OnRaisePropertyChanged();
            }
        }

        private double _chartHeight = double.NaN;
        public double ChartHeight
        {
            get { return _chartHeight; }
            set
            {
                _chartHeight = value;
                base.OnRaisePropertyChanged();
            }
        }

        private double _chartWidth = double.NaN;
        public double ChartWidth
        {
            get { return _chartWidth; }
            set
            {
                _chartWidth = value;
                base.OnRaisePropertyChanged();
            }
        }

        private ChartCollection<AxisAbs> _axes = null;
        public ChartCollection<AxisAbs> Axes
        {
            get { return _axes; }
            set
            {
                _axes = value;
                base.OnRaisePropertyChanged();
            }
        }


        private IChartLegend _legend = null;
        public IChartLegend Legend
        {
            get { return _legend; }
            set
            {
                _legend = value;
                base.OnRaisePropertyChanged();
            }
        }

        private bool _manaulComit = false;
        public bool ManaulComit
        {
            get { return _manaulComit; }
            set
            {
                _manaulComit = value;
                base.OnRaisePropertyChanged();
            }
        }



        public TestChartVM()
        {

        }


        public void Test()
        {
            var axes = new ChartCollection<AxisAbs>();
            axes.Add(new NumberAxis() { AxisType = AxisType.Y, DockOrientation = ChartDockOrientation.Left, MinValue = 0, MaxValue = 100, LabelStep = 20 });
            axes.Add(new NumberAxis() { AxisType = AxisType.Y, DockOrientation = ChartDockOrientation.Right, MinValue = -100, MaxValue = 100, LabelStep = double.NaN });
            axes.Add(new NumberAxis() { AxisType = AxisType.X, DockOrientation = ChartDockOrientation.Bottom, MinValue = 0, MaxValue = 1000, LabelStep = 200 });
            axes.Add(new NumberAxis() { AxisType = AxisType.X, DockOrientation = ChartDockOrientation.Top, MinValue = -1000, MaxValue = 1000, LabelStep = double.NaN });
            this.Axes = axes;
        }
    }
}
