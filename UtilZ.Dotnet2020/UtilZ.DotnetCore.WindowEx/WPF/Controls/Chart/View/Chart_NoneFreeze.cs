using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    //NoneFreeze
    public partial class Chart
    {
        private void UpdateNoneFreeze(AxisFreezeInfo axisFreezeInfo, ChartCollection<AxisAbs> axisCollection, ChartCollection<ISeries> seriesCollection,
            IChartLegend legend, Canvas chartCanvas, Grid chartGrid)
        {
            chartCanvas.Width = axisFreezeInfo.Width;
            chartCanvas.Height = axisFreezeInfo.Height;
            this.Content = chartCanvas;

            double left = 0d, top = 0d, right = 0d, bottom = 0d;

            if (legend != null)
            {
                FrameworkElement legendControl = legend.GetChartLegendControl();
                if (legendControl != null)
                {
                    var legendSize = legend.Size;
                    if (legendSize > 0)
                    {
                        legendControl.HorizontalAlignment = legend.HorizontalAlignment;
                        legendControl.VerticalAlignment = legend.VerticalAlignment;
                        if (legend.Margin != null)
                        {
                            legendControl.Margin = legend.Margin.Value;
                        }
                        chartGrid.Children.Add(legendControl);
                        chartCanvas.Children.Add(chartGrid);

                        switch (legend.Orientation)
                        {
                            case ChartDockOrientation.Left:
                                left += legendSize;
                                chartGrid.Width = legendSize;
                                chartGrid.Height = axisFreezeInfo.Height;
                                Canvas.SetLeft(chartGrid, 0);
                                Canvas.SetTop(chartGrid, 0);
                                break;
                            case ChartDockOrientation.Top:
                                top += legendSize;
                                chartGrid.Width = axisFreezeInfo.Width;
                                chartGrid.Height = legendSize;
                                Canvas.SetLeft(chartGrid, 0);
                                Canvas.SetTop(chartGrid, 0);
                                break;
                            case ChartDockOrientation.Right:
                                right += legendSize;
                                chartGrid.Width = legendSize;
                                chartGrid.Height = axisFreezeInfo.Height;
                                Canvas.SetRight(chartGrid, 0);
                                Canvas.SetTop(chartGrid, 0);
                                break;
                            case ChartDockOrientation.Bottom:
                                bottom += legendSize;
                                chartGrid.Width = axisFreezeInfo.Width;
                                chartGrid.Height = legendSize;
                                Canvas.SetLeft(chartGrid, 0);
                                Canvas.SetBottom(chartGrid, 0);
                                break;
                            default:
                                throw new NotImplementedException(legend.Orientation.ToString());
                        }
                    }
                }
            }




            //计算四个方向坐标占总宽度-高度
            double leftAxisTotalWidth = 0d, topAxisTotalHeight = 0d, rightAxisTotalWidth = 0d, bottomAxisTotalHeight = 0d;
            bool hasAxis = axisCollection != null && axisCollection.Count > 0;
            if (hasAxis)
            {
                double axisSize;
                foreach (var axis in axisCollection)
                {
                    axisSize = axis.Size;
                    switch (axis.AxisType)
                    {
                        case AxisType.X:
                            switch (axis.DockOrientation)
                            {
                                case ChartDockOrientation.Top:
                                    topAxisTotalHeight += axisSize;
                                    break;
                                case ChartDockOrientation.Bottom:
                                    bottomAxisTotalHeight += axisSize;
                                    break;
                                default:
                                    throw new ArgumentException($"X坐标{this.GetType().Name}停靠位置{axis.DockOrientation.ToString()}无效");
                            }
                            break;
                        case AxisType.Y:
                            switch (axis.DockOrientation)
                            {
                                case ChartDockOrientation.Left:
                                    leftAxisTotalWidth += axisSize;
                                    break;
                                case ChartDockOrientation.Right:
                                    rightAxisTotalWidth += axisSize;
                                    break;
                                default:
                                    throw new ArgumentException($"Y坐标{this.GetType().Name}停靠位置{axis.DockOrientation.ToString()}无效");
                            }
                            break;
                        default:
                            throw new NotImplementedException(axis.AxisType.ToString());
                    }
                }
            }

            double chartAreaWidth = axisFreezeInfo.Width - left - right - leftAxisTotalWidth - rightAxisTotalWidth;
            double chartAreaHeight = axisFreezeInfo.Height - top - bottom - topAxisTotalHeight - bottomAxisTotalHeight;

            //绘制坐标
            if (hasAxis)
            {
                double xAxisWidth = chartAreaWidth;
                double yAxisHeight = chartAreaHeight;

                double axisLeft = left, axisTop = top, axisRight = right, axisBottom = bottom;
                double axisSize;
                FrameworkElement axisControl;
                foreach (var axis in axisCollection)
                {
                    axisSize = axis.Size;
                    axisControl = axis.GetAxisControl();
                    chartCanvas.Children.Add(axisControl);

                    switch (axis.AxisType)
                    {
                        case AxisType.X:
                            axisControl.Height = axisSize;
                            axisControl.Width = xAxisWidth;

                            switch (axis.DockOrientation)
                            {
                                case ChartDockOrientation.Top:
                                    Canvas.SetLeft(axisControl, leftAxisTotalWidth);
                                    Canvas.SetTop(axisControl, axisTop);
                                    axisTop += axisSize;
                                    break;
                                case ChartDockOrientation.Bottom:
                                    Canvas.SetLeft(axisControl, leftAxisTotalWidth);
                                    Canvas.SetBottom(axisControl, axisBottom);
                                    axisBottom += axisSize;
                                    break;
                                default:
                                    throw new ArgumentException($"X坐标{this.GetType().Name}停靠位置{axis.DockOrientation.ToString()}无效");
                            }
                            break;
                        case AxisType.Y:
                            axisControl.Height = yAxisHeight;
                            axisControl.Width = axisSize;
                            switch (axis.DockOrientation)
                            {
                                case ChartDockOrientation.Left:
                                    Canvas.SetLeft(axisControl, axisLeft);
                                    Canvas.SetTop(axisControl, topAxisTotalHeight);
                                    axisLeft += axisSize;
                                    break;
                                case ChartDockOrientation.Right:
                                    Canvas.SetRight(axisControl, axisRight);
                                    Canvas.SetTop(axisControl, topAxisTotalHeight);
                                    axisRight += axisSize;
                                    break;
                                default:
                                    throw new ArgumentException($"Y坐标{this.GetType().Name}停靠位置{axis.DockOrientation.ToString()}无效");
                            }
                            break;
                        default:
                            throw new NotImplementedException(axis.AxisType.ToString());
                    }

                    //最后 绘制坐标,是因为此时才确定
                    axis.Draw(seriesCollection);
                }
            }

            double chartAreaX = left + leftAxisTotalWidth;
            double chartAreaY = top + topAxisTotalHeight;
            var chartArea = new Rect(chartAreaX, chartAreaY, chartAreaWidth, chartAreaHeight);



            //todo..Test
            Rectangle reg = new Rectangle();
            reg.Width = chartArea.Width;
            reg.Height = chartArea.Height;
            reg.Fill = Brushes.Green;
            chartCanvas.Children.Add(reg);
            Canvas.SetLeft(reg, chartArea.X);
            Canvas.SetTop(reg, chartArea.Y);
        }
    }
}