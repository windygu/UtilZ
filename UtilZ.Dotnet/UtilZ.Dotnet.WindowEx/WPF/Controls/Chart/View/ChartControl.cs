using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public partial class ChartControl : UserControl
    {
        #region 依赖属性
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(nameof(Series), typeof(ChartSeries), typeof(ChartControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty ChartHeightProperty =
           DependencyProperty.Register(nameof(ChartHeight), typeof(double), typeof(ChartControl),
               new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty ChartWidthProperty =
           DependencyProperty.Register(nameof(ChartWidth), typeof(double), typeof(ChartControl),
               new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(OnPropertyChangedCallback)));

        public static readonly DependencyProperty ManaulComitProperty =
           DependencyProperty.Register(nameof(ManaulComit), typeof(bool), typeof(ChartControl),
               new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnPropertyChangedCallback)));


        public ChartSeries Series
        {
            get { return (ChartSeries)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public double ChartHeight
        {
            get { return (double)GetValue(ChartHeightProperty); }
            set
            {
                if (!double.IsNaN(value) && value < 0)
                {
                    throw new ArgumentOutOfRangeException("表格高度不能为负数");
                }

                SetValue(ChartHeightProperty, value);
            }
        }

        public double ChartWidth
        {
            get { return (double)GetValue(ChartWidthProperty); }
            set
            {
                if (!double.IsNaN(value) && value < 0)
                {
                    throw new ArgumentOutOfRangeException("表格宽度不能为负数");
                }

                SetValue(ChartWidthProperty, value);
            }
        }

        public bool ManaulComit
        {
            get { return (bool)GetValue(ManaulComitProperty); }
            set
            {
                SetValue(ManaulComitProperty, value);
            }
        }

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selfControl = (ChartControl)d;
            if (e.Property == SeriesProperty)
            {
                //selfControl.SeriesChanged((ChartSeries)e.OldValue, (ChartSeries)e.NewValue);
            }
            else if (e.Property == ManaulComitProperty)
            {
                if ((bool)e.NewValue)
                {
                    return;
                }
            }

            selfControl.UpdateChart();
        }
        #endregion









        private readonly Canvas _chartCanvas;

        public ChartControl()
        {
            this._chartCanvas = new Canvas();
            this.Content = this._chartCanvas;
        }







        private void UpdateChart()
        {
            if (this.IgnoreUpdateChart())
            {
                return;
            }

            Canvas chartCanvas = this._chartCanvas;
            chartCanvas.Children.Clear();

            ChartSeries chartSeries = this.Series;
            if (chartSeries == null ||
                chartCanvas.ActualHeight <= 0 ||
                chartCanvas.ActualWidth <= 0)
            {
                return;
            }

            chartSeries.Validate();

            AxisFreezeInfo axisFreezeInfo = this.GetAxisFreezeInfo();

            Rect chartArea;
            Rect canvasChartArea = this.CalCanvasChartArea(axisFreezeInfo, chartSeries, chartCanvas, out chartArea);
            this.AddChartControl(canvasChartArea, axisFreezeInfo, chartSeries, chartCanvas);
            this.DrawAxisLine(chartCanvas, canvasChartArea, chartSeries);
            //this.DrawSeparatorLine(chartCanvas, seriesCollection.Axes, chartArea);

            //this.UpdateSeries(chartCanvas, seriesCollection, chartArea);

            //this._chartArea = chartArea;
            //this.UpdateLegend(seriesCollection);
            //this._fullUpdateCharted = true;
            //this._canvas.Children.Add(lineX);
            //this._canvas.Children.Add(lineY);
        }

        private void DrawAxisLine(Canvas axisCanvas, Rect axisArea, ChartSeries chartSeries)
        {
            if (!chartSeries.EnableCoordinateLine)
            {
                return;
            }

            Style coordinateAxisStyle = chartSeries.CoordinateAxisStyle;
            if (coordinateAxisStyle == null)
            {
                coordinateAxisStyle = new Style();
                coordinateAxisStyle.TargetType = typeof(Line);
                coordinateAxisStyle.Setters.Add(new Setter(Line.StrokeProperty, Brushes.Gray));
                coordinateAxisStyle.Setters.Add(new Setter(Line.StrokeThicknessProperty, 2d));
                coordinateAxisStyle.Setters.Add(new Setter(Panel.ZIndexProperty, -1));
            }

            var line = new Line();
            line.Style = coordinateAxisStyle;
            var axisDockOrientation = AxisDockOrientation.Left;
            switch (axisDockOrientation)
            {
                case AxisDockOrientation.Left:
                    line.X1 = axisCanvas.Width - line.StrokeThickness;
                    line.Y1 = axisArea.Y;

                    line.X2 = line.X1;
                    line.Y2 = axisArea.Y + axisArea.Height;
                    break;
                case AxisDockOrientation.Top:
                    line.X1 = axisArea.X;
                    line.Y1 = axisCanvas.Height - line.StrokeThickness;

                    line.X2 = axisArea.X + axisArea.Width;
                    line.Y2 = line.Y1;
                    break;
                case AxisDockOrientation.Right:
                    line.X1 = 0d;
                    line.Y1 = axisArea.Y;

                    line.X2 = line.X1;
                    line.Y2 = axisArea.Y + axisArea.Height;
                    break;
                case AxisDockOrientation.Bottom:
                    line.X1 = axisArea.X;
                    line.Y1 = 0d;

                    line.X2 = axisArea.X + axisArea.Width;
                    line.Y2 = line.Y1;
                    break;
                default:
                    throw new NotImplementedException();
            }

            axisCanvas.Children.Add(line);

            //var polyline = new Polyline();
            //polyline.Style = coordinateAxisStyle;
            //polyline.Points.Add(new Point(chartArea.X + chartArea.Width, chartArea.Y + chartArea.Height));
            //polyline.Points.Add(new Point(chartArea.X, chartArea.Y + chartArea.Height));
            //polyline.Points.Add(new Point(chartArea.X, chartArea.Y));
            //axisCanvas.Children.Add(polyline);
        }

        private void AddChartControl(Rect chartArea, AxisFreezeInfo axisFreezeInfo, ChartSeries chartSeries, Canvas chartCanvas)
        {
            double left, top, tmp;
            FrameworkElement axisControl;

            switch (axisFreezeInfo.AxisFreezeType)
            {
                case AxisFreezeType.None:
                    var axisXList = chartSeries.Axes.AxisXTopList;
                    if (axisXList != null && axisXList.Count > 0)
                    {
                        left = chartArea.X;
                        top = chartArea.Y;
                        foreach (var axisX in axisXList)
                        {
                            //axisX.UpdateAxis();
                            axisControl = axisX.GetAxisControl();
                            chartCanvas.Children.Add(axisControl);
                            Canvas.SetLeft(axisControl, left);
                            tmp = axisX.GetAxisXHeight();
                            Canvas.SetTop(axisControl, top - tmp);
                            top -= tmp;
                        }
                    }

                    axisXList = chartSeries.Axes.AxisXBottomList;
                    if (axisXList != null && axisXList.Count > 0)
                    {
                        left = chartArea.X;
                        top = chartArea.Y + chartArea.Height;
                        foreach (var axisX in axisXList)
                        {
                            axisControl = axisX.GetAxisControl();
                            chartCanvas.Children.Add(axisControl);
                            Canvas.SetLeft(axisControl, left);
                            tmp = axisX.GetAxisXHeight();
                            Canvas.SetTop(axisControl, top);
                            top += tmp;
                        }
                    }



                    break;
                case AxisFreezeType.X:

                    break;
                case AxisFreezeType.Y:

                    break;
                case AxisFreezeType.All:

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private Rect CalCanvasChartArea(AxisFreezeInfo axisFreezeInfo, ChartSeries chartSeries, Canvas chartCanvas, out Rect chartArea)
        {
            double axisXTopHeight, axisXBottomHeight;
            double chartAreaY, chartAreaHeight;
            this.CalCanvasChartAxisXAreaInfo(axisFreezeInfo, chartSeries, chartCanvas, out axisXTopHeight, out axisXBottomHeight, out chartAreaY, out chartAreaHeight);

            double chartAreaX, chartAreaWidth;
            this.CalChartAxisYAreaInfo(axisFreezeInfo, chartSeries, chartCanvas, chartAreaHeight, out chartAreaX, out chartAreaWidth);

            this.AddLegendControl(chartSeries, chartCanvas, ref chartAreaY, ref chartAreaHeight, ref chartAreaX, ref chartAreaWidth);


            chartArea = new Rect();
            this.SetAxisXWidth(axisFreezeInfo, chartSeries, chartAreaWidth);

            return new Rect(chartAreaX, chartAreaY, chartAreaWidth, chartAreaHeight);
        }

        private void AddLegendControl(ChartSeries chartSeries, Canvas chartCanvas,
            ref double chartAreaY, ref double chartAreaHeight, ref double chartAreaX, ref double chartAreaWidth)
        {
            IChartLegend chartLegend = chartSeries.ChartLegend;
            if (chartLegend == null)
            {
                return;
            }

            var chartLegendControl = chartLegend.GetChartLegendControl();
            if (chartLegendControl != null)
            {
                return;
            }

            chartLegendControl.Style = chartSeries.ChartLegendStyle;
            Grid gridLegend = new Grid();
            gridLegend.Name = nameof(gridLegend);
            gridLegend.Children.Add(chartLegendControl);
            chartCanvas.Children.Add(gridLegend);

            switch (chartLegend.ChartLegendOrientation)
            {
                case AxisDockOrientation.Left:
                    chartAreaX += chartLegend.HorizontalWidth;
                    chartAreaWidth -= chartLegend.HorizontalWidth;

                    gridLegend.Height = chartCanvas.ActualWidth;
                    gridLegend.Width = chartLegend.HorizontalWidth;
                    Canvas.SetLeft(gridLegend, 0d);
                    Canvas.SetTop(gridLegend, 0d);
                    break;
                case AxisDockOrientation.Right:
                    chartAreaWidth -= chartLegend.HorizontalWidth;

                    gridLegend.Height = chartCanvas.ActualWidth;
                    gridLegend.Width = chartLegend.HorizontalWidth;
                    Canvas.SetLeft(gridLegend, chartAreaX + chartAreaWidth);
                    Canvas.SetTop(gridLegend, 0d);
                    break;
                case AxisDockOrientation.Top:
                    chartAreaY += chartLegend.VerticalHeight;
                    chartAreaHeight -= chartLegend.VerticalHeight;

                    gridLegend.Width = chartCanvas.ActualWidth;
                    gridLegend.Height = chartLegend.VerticalHeight;
                    Canvas.SetLeft(gridLegend, 0d);
                    Canvas.SetTop(gridLegend, 0d);
                    break;
                case AxisDockOrientation.Bottom:
                    chartAreaHeight -= chartLegend.VerticalHeight;

                    gridLegend.Width = chartCanvas.ActualWidth;
                    gridLegend.Height = chartLegend.VerticalHeight;
                    Canvas.SetLeft(gridLegend, chartAreaX);
                    Canvas.SetTop(gridLegend, chartAreaY + chartAreaHeight);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void SetAxisXWidth(AxisFreezeInfo axisFreezeInfo, ChartSeries chartSeries, double chartAxisXWidth)
        {
            //switch(axisFreezeInfo)

            //var axisYList = chartSeries.Axes.AxisYLeftList;
            //if (axisYList != null && axisYList.Count > 0)
            //{
            //    foreach (var axisY in axisYList)
            //    {
            //        axisYLeftWidth += axisY.GetAxisYWidth(chartAreaHeight);
            //    }
            //}

            //axisYList = chartSeries.Axes.AxisYRightList;
            //if (axisYList != null && axisYList.Count > 0)
            //{
            //    foreach (var axisY in axisYList)
            //    {
            //        axisYRightWidth += axisY.GetAxisYWidth(chartAreaHeight);
            //    }
            //}
        }

        private void CalChartAxisYAreaInfo(AxisFreezeInfo axisFreezeInfo, ChartSeries chartSeries, Canvas chartCanvas,
            double chartAreaHeight, out double chartAreaX, out double chartAreaWidth)
        {
            double axisYLeftWidth = 0d, axisYRightWidth = 0d;
            var axisYList = chartSeries.Axes.AxisYLeftList;
            if (axisYList != null && axisYList.Count > 0)
            {
                foreach (var axisY in axisYList)
                {
                    axisYLeftWidth += axisY.GetAxisYWidth(chartAreaHeight);
                }
            }

            axisYList = chartSeries.Axes.AxisYRightList;
            if (axisYList != null && axisYList.Count > 0)
            {
                foreach (var axisY in axisYList)
                {
                    axisYRightWidth += axisY.GetAxisYWidth(chartAreaHeight);
                }
            }

            chartAreaX = axisYLeftWidth;
            chartAreaWidth = chartCanvas.ActualWidth - axisYLeftWidth - axisYRightWidth;
            if (axisFreezeInfo.AxisFreezeType == AxisFreezeType.X || axisFreezeInfo.AxisFreezeType == AxisFreezeType.All)
            {
                chartAreaWidth -= this.GetScrollBarSize();
            }
        }

        private void CalCanvasChartAxisXAreaInfo(AxisFreezeInfo axisFreezeInfo, ChartSeries chartSeries, Canvas chartCanvas,
            out double axisXTopHeight, out double axisXBottomHeight, out double chartAreaY, out double chartAreaHeight)
        {
            axisXTopHeight = 0d;
            axisXBottomHeight = 0d;

            var axisXList = chartSeries.Axes.AxisXTopList;
            if (axisXList != null && axisXList.Count > 0)
            {
                foreach (var axisX in axisXList)
                {
                    axisXTopHeight += axisX.GetAxisXHeight();
                }
            }

            axisXList = chartSeries.Axes.AxisXBottomList;
            if (axisXList != null && axisXList.Count > 0)
            {
                foreach (var axisX in axisXList)
                {
                    axisXBottomHeight += axisX.GetAxisXHeight();
                }
            }

            chartAreaY = axisXTopHeight;
            chartAreaHeight = chartCanvas.ActualHeight - axisXTopHeight - axisXBottomHeight;
            if (axisFreezeInfo.AxisFreezeType == AxisFreezeType.Y || axisFreezeInfo.AxisFreezeType == AxisFreezeType.All)
            {
                chartAreaHeight -= this.GetScrollBarSize();
            }
        }





        private AxisFreezeInfo GetAxisFreezeInfo()
        {
            AxisFreezeType axisFreezeType;
            if (double.IsNaN(this.ChartHeight))
            {
                if (double.IsNaN(this.ChartWidth))
                {
                    axisFreezeType = AxisFreezeType.None;
                }
                else
                {
                    axisFreezeType = AxisFreezeType.Y;
                }
            }
            else
            {
                if (double.IsNaN(this.ChartWidth))
                {
                    axisFreezeType = AxisFreezeType.X;
                }
                else
                {
                    axisFreezeType = AxisFreezeType.All;
                }
            }

            return new AxisFreezeInfo(this.ChartWidth, this.ChartHeight, axisFreezeType);
        }



        private bool IgnoreUpdateChart()
        {
            if (this.ManaulComit || !this.IsLoaded)
            {
                return true;
            }

            return false;
        }



        #region ScrollViewerStyle
        private static StytleContainerControl _stytleContainerControl = null;
        private StytleContainerControl GetStytleContainerControl()
        {
            if (_stytleContainerControl == null)
            {
                _stytleContainerControl = new StytleContainerControl();
            }

            return _stytleContainerControl;
        }

        private Style GetScrollViewStyle()
        {
            var stytleContainerControl = GetStytleContainerControl();
            if (stytleContainerControl != null)
            {
                return stytleContainerControl.ScrollViewerStyle;
            }

            return null;
        }

        private double GetScrollBarSize()
        {
            const double SCROLL_BAR_DEFAULT_SIZE = 18d;
            var stytleContainerControl = GetStytleContainerControl();
            if (stytleContainerControl != null)
            {
                return stytleContainerControl.ScrollBarSize;
            }

            return SCROLL_BAR_DEFAULT_SIZE;
        }
        #endregion
    }
}
