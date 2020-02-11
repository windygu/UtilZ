using System;
using System.Collections.Generic;
using System.IO;
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
using UtilZ.DotnetStd.Ex.Model;

namespace WpfApp1
{
    /// <summary>
    /// VADResultChart.xaml 的交互逻辑
    /// </summary>
    public partial class VADResultChart : Window
    {
        private VADResultChartVM _vm;
        public VADResultChart()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._vm = new VADResultChartVM();
            this.DataContext = this._vm;
        }

        private void btnLoadAll_Click(object sender, RoutedEventArgs e)
        {
            this._vm.LoadAll();
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "*.txt|*.txt";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            this._vm.LoadFile(ofd.FileNames);
        }
    }

    public class VADResultChartVM : BaseModelAbs
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

        private double _chartMinHeight = double.NaN;
        public double ChartMinHeight
        {
            get { return _chartMinHeight; }
            set
            {
                _chartMinHeight = value;
                base.OnRaisePropertyChanged();
            }
        }

        private double _chartMinWidth = double.NaN;
        public double ChartMinWidth
        {
            get { return _chartMinWidth; }
            set
            {
                _chartMinWidth = value;
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

        public VADResultChartVM()
        {
            var axes = new ChartCollection<AxisAbs>();
            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.X,
                DockOrientation = ChartDockOrientation.Bottom,
                Orientation = AxisOrientation.LeftToRight,
                LabelStep = double.NaN
            });

            axes.Add(new NumberAxis()
            {
                AxisType = AxisType.Y,
                DockOrientation = ChartDockOrientation.Left,
                Orientation = AxisOrientation.BottomToTop,
                MinValue = 0,
                MaxValue = 1,
                LabelStep = 1d
            });
            this._axes = axes;


            this._legend = new VerticalChartLegend()
            {
                DockOrientation = ChartDockOrientation.Left,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.Transparent,
                Width = 250d,
                IsChecked = true
            };
        }


        public void LoadAll()
        {
            const string DIR = @"F:\Projects\Demo\2020-1-13\WpfApp1\bin\Debug\Log";
            string[] filePathArr = Directory.GetFiles(DIR, "*.txt");
            this.LoadFile(filePathArr);
        }

        public void LoadFile(string[] filePathArr)
        {
            var series = new ChartCollection<ISeries>();
            double x, y;

            foreach (var filePath in filePathArr)
            {
                IEnumerable<string> lines = System.IO.File.ReadLines(filePath);
                var values = new ChartCollection<IChartValue>();
                string[] strArr;
                foreach (string line in lines)
                {
                    strArr = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (strArr.Length != 2)
                    {
                        continue;
                    }

                    x = double.Parse(strArr[0]) / 1000;
                    y = strArr[1].Contains("有信号") ? 1d : 0d;
                    values.Add(new ChartAxisValue(x, y, $"{x}s_{y}"));
                }

                series.Add(new StepLineSeries()
                {
                    AxisX = this._axes[0],
                    AxisY = this._axes[1],
                    EnableTooltip = false,
                    Values = values,
                    Title = System.IO.Path.GetFileNameWithoutExtension(filePath),
                    Style = ChartStyleHelper.CreateLineSeriesStyle(ColorBrushHelper.GetNextColor()),
                    Orientation = SeriesOrientation.Horizontal
                });
            }

            this.Series = series;
        }
    }
}
