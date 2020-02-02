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


        private readonly Random _rnd = new Random();
        public TestChartVM()
        {

        }


        public void Test()
        {
            //TestNumAxis1();
            //TestNumAxis2();

            TestLineSeries();
        }


        private void TestLineSeries()
        {
            int minY = -100, maxY = 100;

            this.ManaulComit = true;
            var axes = new ChartCollection<AxisAbs>();
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.Y,
                DockOrientation = ChartDockOrientation.Left,
                Orientation = AxisOrientation.BottomToTop,
                MinValue = minY,
                MaxValue = maxY,
                LabelStep = double.NaN
            });
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Bottom,
                Orientation = AxisOrientation.LeftToRight,
                MinValue = -1000,
                MaxValue = 1000,
                LabelStep = double.NaN
            });
            this.Axes = axes;


            var series = new ChartCollection<ISeries>();
            series.Add(new LineSeries() { AxisX = axes[1], AxisY = axes[0], LineSeriesType = LineSeriesType.Bezier, EnableTooltip = true, Title = "LineSeries" });
            series.Add(new StepLineSeries() { AxisX = axes[1], AxisY = axes[0], EnableTooltip = false, Title = "StepLineSeries" });

            double axisXValue = ((NumberAxis)axes[1]).MinValue, value, axisXValueStep = 10;
            ChartCollection<IChartItem> values = new ChartCollection<IChartItem>();
            while (axisXValue < ((NumberAxis)axes[1]).MaxValue)
            {
                value = _rnd.Next(minY, maxY);
                values.Add(new ChartNumberItem(axisXValue, value, null));
                axisXValue += axisXValueStep;
            }
            series[0].Values = values;

            this.Series = series;
            this.ManaulComit = false;
        }

        private void TestNumAxis2()
        {
            var axes = new ChartCollection<AxisAbs>();
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.Y,
                DockOrientation = ChartDockOrientation.Left,
                Orientation = AxisOrientation.BottomToTop,
                MinValue = -100,
                MaxValue = 100,
                LabelStep = double.NaN
            });
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.Y,
                DockOrientation = ChartDockOrientation.Right,
                Orientation = AxisOrientation.TopToBottom,
                MinValue = -100000,
                MaxValue = 100000,
                LabelStep = double.NaN
            });
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Bottom,
                Orientation = AxisOrientation.LeftToRight,
                MinValue = -1000,
                MaxValue = 1000,
                LabelStep = double.NaN
            });
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Top,
                Orientation = AxisOrientation.RightToLeft,
                MinValue = -100000,
                MaxValue = 100000,
                LabelStep = double.NaN
            });
            this.Axes = axes;
        }

        private void TestNumAxis1()
        {
            var axes = new ChartCollection<AxisAbs>();
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.Y,
                DockOrientation = ChartDockOrientation.Left,
                Orientation = AxisOrientation.BottomToTop,
                MinValue = 0,
                MaxValue = 100,
                LabelStep = 20
            });
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.Y,
                DockOrientation = ChartDockOrientation.Right,
                Orientation = AxisOrientation.BottomToTop,
                MinValue = -100,
                MaxValue = 100,
                LabelStep = double.NaN
            });
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Bottom,
                Orientation = AxisOrientation.LeftToRight,
                MinValue = 0,
                MaxValue = 1000,
                LabelStep = 200
            });
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Top,
                Orientation = AxisOrientation.LeftToRight,
                MinValue = -1000,
                MaxValue = 1000,
                LabelStep = double.NaN
            });
            this.Axes = axes;
        }
    }
}
