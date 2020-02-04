using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UtilZ.DotnetCore.WindowEx.WPF.Controls;
using UtilZ.DotnetStd.Ex.Log;
using UtilZ.DotnetStd.Ex.Log.Appender;
using UtilZ.DotnetStd.Ex.Model;
using WpfApp1.Model;

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

            var redirectAppenderToUI = (RedirectAppender)Loger.GetAppenderByName(null, "RedirectToUI");
            if (redirectAppenderToUI != null)
            {
                redirectAppenderToUI.RedirectOuput += RedirectLogOutput;
            }
        }

        private void RedirectLogOutput(object sender, RedirectOuputArgs e)
        {
            string str;
            try
            {
                str = string.Format("{0} {1}", DateTime.Now, e.Item.Content);
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }

            logControl.AddLog(str, e.Item.Level);
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            this._vm.Test();
        }
    }

    internal class TestChartVM : BaseModelAbs
    {
        #region define
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
        #endregion

        private readonly Random _rnd = new Random();
        public TestChartVM()
        {

        }


        public void Test()
        {
            //TestNumAxis1();
            //TestNumAxis2();
            //TestDateTimeAxis();


            TestLineSeries();
            //TestVerStepLineSeries();
        }


        private const string fileName = "StepLineSeriesValues.txt";
        private void TestVerStepLineSeries()
        {
            int minY = -100, maxY = 100;
            double minX = -1000, maxX = 1000;
            DateTime minTime = DateTime.Parse("2010-01-01 00:00:00");
            DateTime maxTime = DateTime.Parse("2012-01-01 00:00:00");

            this.ManaulComit = true;
            var axes = new ChartCollection<AxisAbs>();
            axes.Add(new DateTimeAxis()
            {
                AxisType = AxisType.Y,
                DockOrientation = ChartDockOrientation.Left,
                Orientation = AxisOrientation.TopToBottom,
                MinValue = minTime,
                MaxValue = maxTime,
                LabelStep = null
            });

            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Bottom,
                Orientation = AxisOrientation.LeftToRight,
                MinValue = minX,
                MaxValue = maxX,
                LabelStep = double.NaN
            });

            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Bottom,
                Orientation = AxisOrientation.LeftToRight,
                MinValue = minY,
                MaxValue = maxY,
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
                DockOrientation = ChartDockOrientation.Top,
                Orientation = AxisOrientation.RightToLeft,
                MinValue = -100000,
                MaxValue = 100000,
                LabelStep = double.NaN
            });
            this.Axes = axes;


            var series = new ChartCollection<ISeries>();
            series.Add(new LineSeries()
            {
                AxisX = axes[1],
                AxisY = axes[0],
                LineSeriesType = LineSeriesType.Bezier,
                EnableTooltip = true,
                Title = "LineSeries",
                Style = ChartStyleHelper.CreateLineStyle(Brushes.Gray)
            });
            series.Add(new LineSeries()
            {
                AxisX = axes[2],
                AxisY = axes[0],
                LineSeriesType = LineSeriesType.Bezier,
                EnableTooltip = true,
                Title = "DateTimeLineSeries",
                Style = ChartStyleHelper.CreateLineStyle(Brushes.Green),
                //CreatePointFunc = this.CreatePointFunc
            });
            series.Add(new StepLineSeries()
            {
                AxisX = axes[2],
                AxisY = axes[0],
                EnableTooltip = true,
                Title = "DateTimeStepLineSeries",
                Style = ChartStyleHelper.CreateLineStyle(Brushes.Red),
                Orientation = StepLineOrientation.Vertical
            });


            double value;
            DateTime time;
            ChartCollection<IChartItem> values3 = new ChartCollection<IChartItem>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                string[] strArr;
                while (line != null)
                {
                    strArr = line.Split('_', 2, StringSplitOptions.RemoveEmptyEntries);
                    time = DateTime.Parse(strArr[0]);
                    value = double.Parse(strArr[1]);
                    values3.Add(new ChartDateTimeItem2(time, value, $"{time.ToString()}_{value}"));
                    line = sr.ReadLine();
                }
            }
            series[2].Values = values3;



            this.Series = series;
            this.Legend = new HorizontalChartLegend()
            {
                DockOrientation = ChartDockOrientation.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.Transparent
            };
            this.ManaulComit = false;
        }

        private void TestLineSeries()
        {
            int minY = -100, maxY = 100;
            double minX = -1000, maxX = 1000;
            DateTime minTime = DateTime.Parse("2010-01-01 00:00:00");
            DateTime maxTime = DateTime.Parse("2012-01-01 00:00:00");

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
                MinValue = minX,
                MaxValue = maxX,
                LabelStep = double.NaN
            });

            axes.Add(new DateTimeAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Bottom,
                Orientation = AxisOrientation.LeftToRight,
                MinValue = minTime,
                MaxValue = maxTime,
                LabelStep = null
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
                DockOrientation = ChartDockOrientation.Top,
                Orientation = AxisOrientation.RightToLeft,
                MinValue = -100000,
                MaxValue = 100000,
                LabelStep = double.NaN
            });
            this.Axes = axes;


            var series = new ChartCollection<ISeries>();
            series.Add(new LineSeries()
            {
                AxisX = axes[1],
                AxisY = axes[0],
                LineSeriesType = LineSeriesType.Bezier,
                EnableTooltip = true,
                Title = "LineSeries",
                Style = ChartStyleHelper.CreateLineStyle(Brushes.Gray)
            });
            series.Add(new LineSeries()
            {
                AxisX = axes[2],
                AxisY = axes[0],
                LineSeriesType = LineSeriesType.Bezier,
                EnableTooltip = true,
                Title = "DateTimeLineSeries",
                Style = ChartStyleHelper.CreateLineStyle(Brushes.Green),
                //CreatePointFunc = this.CreatePointFunc
            });
            series.Add(new StepLineSeries()
            {
                AxisX = axes[2],
                AxisY = axes[0],
                EnableTooltip = true,
                Title = "DateTimeStepLineSeries",
                Style = ChartStyleHelper.CreateLineStyle(Brushes.Red),
                Orientation = StepLineOrientation.Horizontal
            });


            double value;
            double axisXValueStep = 10;
            double axisXValue = minX;
            ChartCollection<IChartItem> values = new ChartCollection<IChartItem>();
            while (axisXValue < maxX)
            {
                value = _rnd.Next(minY, maxY);
                values.Add(new ChartNumberItem(axisXValue, value, $"{axisXValue}_{value}"));
                axisXValue += axisXValueStep;
            }
            series[0].Values = values;



            DateTime time = minTime;
            TimeSpan ts = maxTime - time;
            ChartCollection<IChartItem> values2 = new ChartCollection<IChartItem>();
            double stepTotalMilliseconds = ts.TotalMilliseconds / ((maxY - minY) / axisXValueStep);
            while (time < maxTime)
            {
                value = _rnd.Next(minY, maxY);
                values2.Add(new ChartDateTimeItem(time, value, $"{time.ToString()}_{value}"));
                time = time.AddMilliseconds(stepTotalMilliseconds);
            }
            series[1].Values = values2;


            //StringBuilder sb = new StringBuilder();
            ChartCollection<IChartItem> values3 = new ChartCollection<IChartItem>();
            minY = minY / 10;
            maxY = maxY / 10;
            time = minTime;
            while (time < maxTime)
            {
                value = _rnd.Next(minY, maxY) * 10;
                values3.Add(new ChartDateTimeItem(time, value, $"{time.ToString()}_{value}"));
                //sb.AppendLine($"{time.ToString("yyyy-MM-dd HH:mm:ss")}_{value}");
                ts = maxTime - time;
                time = time.AddDays(_rnd.Next(1, (int)ts.TotalDays));
            }
            //File.WriteAllText(fileName, sb.ToString());
            series[2].Values = values3;



            this.Series = series;
            this.Legend = new VerticalChartLegend()
            {
                DockOrientation = ChartDockOrientation.Right,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.Transparent
            };
            //this.Legend = new HorizontalChartLegend()
            //{
            //    DockOrientation = ChartDockOrientation.Bottom,
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    VerticalAlignment = VerticalAlignment.Center,
            //    Background = Brushes.Transparent
            //};
            this.ManaulComit = false;
        }


        private FrameworkElement CreatePointFunc(PointInfo pointInfo)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = Brushes.Red;
            ellipse.StrokeThickness = 0d;
            ellipse.Width = 5d;
            ellipse.Height = 5d;
            ellipse.Margin = new Thickness(-2.5d, -2.5d, 0d, 0d);
            ellipse.ToolTip = $"X:{pointInfo.Point.X}     Y:{pointInfo.Point.Y}       {pointInfo.Item.ToString()}";
            return ellipse;
        }


        private void TestDateTimeAxis()
        {
            DateTime min = DateTime.Parse("2010-01-01 00:00:00");
            DateTime max = DateTime.Parse("2010-12-31 23:23:59");

            var axes = new ChartCollection<AxisAbs>();
            axes.Add(new DateTimeAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Bottom,
                Orientation = AxisOrientation.LeftToRight,
                MinValue = min,
                MaxValue = max,
                LabelStep = null
            });
            axes.Add(new DateTimeAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Top,
                Orientation = AxisOrientation.RightToLeft,
                MinValue = min,
                MaxValue = max,
                LabelStep = null
            });
            axes.Add(new DateTimeAxis()
            {
                AxisType = AxisType.Y,
                DockOrientation = ChartDockOrientation.Left,
                Orientation = AxisOrientation.BottomToTop,
                MinValue = min,
                MaxValue = max,
                LabelStep = null
            });
            axes.Add(new DateTimeAxis()
            {
                AxisType = AxisType.Y,
                DockOrientation = ChartDockOrientation.Right,
                Orientation = AxisOrientation.TopToBottom,
                MinValue = min,
                MaxValue = max,
                LabelStep = null
            });
            this.Axes = axes;
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
