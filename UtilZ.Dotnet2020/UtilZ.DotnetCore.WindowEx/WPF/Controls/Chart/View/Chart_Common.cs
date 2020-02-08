using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    //Common
    public partial class Chart
    {
        private LegendAddResult AddLegend(IChartLegend legend, Grid chartGrid)
        {
            double left = 0d, top = 0d, right = 0d, bottom = 0d;
            double legendSize = double.NaN;
            bool hasLegend = false;
            if (legend != null)
            {
                FrameworkElement legendControl = legend.LegendControl;
                if (legendControl != null)
                {
                    legendSize = legend.Size;
                    if (legendSize > AxisConstant.ZERO_D)
                    {
                        chartGrid.Children.Add(legendControl);//使用Grid包装,主是为了居中对齐好布局,Canvas中水平垂直方向太麻烦
                        hasLegend = true;

                        switch (legend.DockOrientation)
                        {
                            case ChartDockOrientation.Left:
                                left = legendSize;
                                break;
                            case ChartDockOrientation.Top:
                                top = legendSize;
                                break;
                            case ChartDockOrientation.Right:
                                right = legendSize;
                                break;
                            case ChartDockOrientation.Bottom:
                                bottom = legendSize;
                                break;
                            default:
                                throw new NotImplementedException(legend.DockOrientation.ToString());
                        }
                    }
                }
            }

            return new LegendAddResult(hasLegend, left, top, right, bottom);
        }

        private AxisXHeightInfo CalculateAxisXHeight(ChartCollection<AxisAbs> axisCollection)
        {
            double topAxisTotalHeight = AxisConstant.ZERO_D, bottomAxisTotalHeight = AxisConstant.ZERO_D;
            if (axisCollection != null && axisCollection.Count > AxisConstant.ZERO_I)
            {
                double axisHeight;
                foreach (var axis in axisCollection)
                {
                    axis.Validate();//验证坐标轴有效性

                    if (axis.AxisType != AxisType.X)
                    {
                        continue;
                    }

                    axisHeight = axis.GetXAxisHeight();
                    if (axis.IsAxisXBottom())
                    {
                        bottomAxisTotalHeight += axisHeight;
                    }
                    else
                    {
                        topAxisTotalHeight += axisHeight;
                    }
                }
            }

            return new AxisXHeightInfo(topAxisTotalHeight, bottomAxisTotalHeight);
        }

        private AxisYWidthInfo DrawAxisYByAxisXHeightInfo(ChartCollection<AxisAbs> axisCollection, UIElementCollection children, ChartCollection<ISeries> seriesCollection, double yAxisHeight, double top)
        {
            if (!AxisHelper.DoubleHasValue(yAxisHeight) || yAxisHeight <= AxisConstant.ZERO_D)
            {
                return new AxisYWidthInfo(AxisConstant.ZERO_D, AxisConstant.ZERO_D, null);
            }

            double leftAxisTotalWidth = AxisConstant.ZERO_D, rightAxisTotalWidth = AxisConstant.ZERO_D;
            Dictionary<AxisAbs, List<double>> axisYLabelDic = null;
            if (axisCollection != null && axisCollection.Count > AxisConstant.ZERO_I)
            {
                FrameworkElement axisYControl;
                double axisLeft = AxisConstant.ZERO_D, axisRight = AxisConstant.ZERO_D;
                List<double> yList;
                foreach (var axis in axisCollection)
                {
                    if (axis.AxisType != AxisType.Y)
                    {
                        continue;
                    }

                    yList = axis.DrawY(seriesCollection, yAxisHeight);
                    if (axis.EnableBackgroundLabelLine && yList != null)
                    {
                        if (axisYLabelDic == null)
                        {
                            axisYLabelDic = new Dictionary<AxisAbs, List<double>>();
                        }
                        axisYLabelDic.Add(axis, yList);
                    }

                    axisYControl = axis.AxisControl;
                    axisYControl.VerticalAlignment = VerticalAlignment.Top;
                    children.Add(axisYControl);

                    if (axis.IsAxisYLeft())
                    {
                        axisYControl.HorizontalAlignment = HorizontalAlignment.Left;
                        axisYControl.Margin = new Thickness(axisLeft, top, AxisConstant.ZERO_D, AxisConstant.ZERO_D);
                        leftAxisTotalWidth += axis.Width;
                        axisLeft += axis.Width;
                    }
                    else
                    {
                        axisYControl.HorizontalAlignment = HorizontalAlignment.Right;
                        axisYControl.Margin = new Thickness(AxisConstant.ZERO_D, top, axisRight, AxisConstant.ZERO_D);
                        rightAxisTotalWidth += axis.Width;
                        axisRight += axis.Width;
                    }
                }
            }

            return new AxisYWidthInfo(leftAxisTotalWidth, rightAxisTotalWidth, axisYLabelDic);
        }

        private Dictionary<AxisAbs, List<double>> DrawAxisX(ChartCollection<AxisAbs> axisCollection, ChartCollection<ISeries> seriesCollection, Grid chartGrid, double xAxisWidth, double left)
        {
            if (!AxisHelper.DoubleHasValue(xAxisWidth) || xAxisWidth <= AxisConstant.ZERO_D)
            {
                return null;
            }

            Dictionary<AxisAbs, List<double>> axisXLabelDic = null;
            bool hasAxis = axisCollection != null && axisCollection.Count > AxisConstant.ZERO_I;
            if (hasAxis)
            {
                FrameworkElement axisXControl;
                double axisTop = AxisConstant.ZERO_D, axisBottom = AxisConstant.ZERO_D;
                List<double> xList;

                foreach (var axis in axisCollection)
                {
                    if (axis.AxisType != AxisType.X)
                    {
                        continue;
                    }

                    xList = axis.DrawX(seriesCollection, xAxisWidth);
                    if (axis.EnableBackgroundLabelLine && xList != null)
                    {
                        if (axisXLabelDic == null)
                        {
                            axisXLabelDic = new Dictionary<AxisAbs, List<double>>();
                        }
                        axisXLabelDic.Add(axis, xList);
                    }

                    axisXControl = axis.AxisControl;
                    axisXControl.HorizontalAlignment = HorizontalAlignment.Left;
                    chartGrid.Children.Add(axisXControl);
                    if (axis.IsAxisXBottom())
                    {
                        axisXControl.VerticalAlignment = VerticalAlignment.Bottom;
                        axisXControl.Margin = new Thickness(left, axisTop, AxisConstant.ZERO_D, AxisConstant.ZERO_D);
                        axisTop += axisXControl.Height;
                    }
                    else
                    {
                        axisXControl.VerticalAlignment = VerticalAlignment.Top;
                        axisXControl.Margin = new Thickness(left, AxisConstant.ZERO_D, AxisConstant.ZERO_D, axisBottom);
                        axisBottom += axisXControl.Height;
                    }
                }
            }

            return axisXLabelDic;
        }

        private void DrawAxisBackgroundLabelLine(Canvas chartCanvas, Dictionary<AxisAbs, List<double>> axisYLabelDic, Dictionary<AxisAbs, List<double>> axisXLabelDic)
        {
            Path backgroundLabelLine;
            List<BackgroundLabelLineSegment> labelLineSegments = null;
            if (axisYLabelDic != null)
            {
                labelLineSegments = new List<BackgroundLabelLineSegment>();
                foreach (var kv in axisYLabelDic)
                {
                    labelLineSegments.Clear();
                    foreach (var y in kv.Value)
                    {
                        labelLineSegments.Add(new BackgroundLabelLineSegment(new Point(AxisConstant.ZERO_D, y), new Point(chartCanvas.Width, y)));
                    }
                    backgroundLabelLine = kv.Key.CreateBackgroundLabelLine(labelLineSegments);
                    if (backgroundLabelLine != null)
                    {
                        chartCanvas.Children.Add(backgroundLabelLine);
                    }
                }
            }

            if (axisXLabelDic != null)
            {
                if (labelLineSegments == null)
                {
                    labelLineSegments = new List<BackgroundLabelLineSegment>();
                }

                foreach (var kv in axisXLabelDic)
                {
                    labelLineSegments.Clear();
                    foreach (var x in kv.Value)
                    {
                        labelLineSegments.Add(new BackgroundLabelLineSegment(new Point(x, AxisConstant.ZERO_D), new Point(x, chartCanvas.Height)));
                    }
                    backgroundLabelLine = kv.Key.CreateBackgroundLabelLine(labelLineSegments);
                    if (backgroundLabelLine != null)
                    {
                        chartCanvas.Children.Add(backgroundLabelLine);
                    }
                }
            }
        }

        private void DrawSeries(Canvas chartCanvas, ChartCollection<ISeries> seriesCollection)
        {
            if (chartCanvas.Width <= AxisConstant.ZERO_D || chartCanvas.Height <= AxisConstant.ZERO_D)
            {
                return;
            }

            if (seriesCollection != null && seriesCollection.Count > AxisConstant.ZERO_I)
            {
                foreach (ISeries series in seriesCollection)
                {
                    series.Add(chartCanvas);
                }
            }
        }


        private void UpdateLegend(IChartLegend legend, ChartCollection<ISeries> seriesCollection)
        {
            var legendBrushList = new List<SeriesLegendItem>();
            if (seriesCollection != null && seriesCollection.Count > AxisConstant.ZERO_I)
            {
                foreach (ISeries series in seriesCollection)
                {
                    series.FillLegendItemToList(legendBrushList);
                }
            }

            legend.UpdateLegend(legendBrushList);
        }

        private void SetRowColumn(Grid chartGrid, FrameworkElement element, RowColumnDefinitionItem rowColumnDefinition)
        {
            if (element == null || rowColumnDefinition == null)
            {
                return;
            }

            if (chartGrid.RowDefinitions.Count > 1 && rowColumnDefinition.RowIndex != AxisConstant.ROW_COLUMN_DEFAULT_INDEX)
            {
                Grid.SetRow(element, rowColumnDefinition.RowIndex);
                if (rowColumnDefinition.RowSpan != AxisConstant.ROW_COLUMN_DEFAULT_INDEX)
                {
                    Grid.SetRowSpan(element, rowColumnDefinition.RowSpan);
                }
            }

            if (chartGrid.ColumnDefinitions.Count > 1 && rowColumnDefinition.ColumnIndex != AxisConstant.ROW_COLUMN_DEFAULT_INDEX)
            {
                Grid.SetColumn(element, rowColumnDefinition.ColumnIndex);
                if (rowColumnDefinition.ColumnSpan != AxisConstant.ROW_COLUMN_DEFAULT_INDEX)
                {
                    Grid.SetColumnSpan(element, rowColumnDefinition.ColumnSpan);
                }
            }
        }

    }
}
