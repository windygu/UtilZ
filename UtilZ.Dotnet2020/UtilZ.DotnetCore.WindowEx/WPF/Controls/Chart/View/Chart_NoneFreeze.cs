using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    //NoneFreeze
    public partial class Chart
    {
        private void UpdateNoneFreeze_bk(AxisFreezeInfo axisFreezeInfo, ChartCollection<AxisAbs> axisCollection, ChartCollection<ISeries> seriesCollection,
            IChartLegend legend, Canvas chartCanvas, Grid chartGrid)
        {
            /************************************************************************************************************
             * 步骤:
             * 1.添加legend,并计算出发四周所占高度或宽度
             * 2.计算X轴总高度
             * 3.根据X轴总高度计算Y轴高度(图表区域高度)
             * 4.根据Y轴高度计算刻度值中最大宽度的刻度值,以求出单个Y轴宽度和所有Y轴宽度(为了解决Y轴刻度显示不完或空白过多)
             * 5.绘制X轴
             * 6.根据2和3计算图表区域宽度
             * 7.绘各种图
             * 8.填充legend
             ************************************************************************************************************/

            //chartCanvas.Width = axisFreezeInfo.Width;
            //chartCanvas.Height = axisFreezeInfo.Height;
            //this.Content = chartCanvas;



            //double left = 0d, top = 0d, right = 0d, bottom = 0d;

            ////第一步
            //bool hasLegend = false;
            //if (legend != null)
            //{
            //    FrameworkElement legendControl = legend.GetChartLegendControl();
            //    if (legendControl != null)
            //    {
            //        var legendSize = legend.Size;
            //        if (legendSize > 0)
            //        {
            //            legendControl.HorizontalAlignment = legend.HorizontalAlignment;
            //            legendControl.VerticalAlignment = legend.VerticalAlignment;
            //            chartGrid.Children.Add(legendControl);//使用Grid包装,主是为了居中对齐好布局,Canvas中水平垂直方向太麻烦
            //            chartCanvas.Children.Add(chartGrid);
            //            hasLegend = true;

            //            switch (legend.DockOrientation)
            //            {
            //                case ChartDockOrientation.Left:
            //                    left += legendSize;
            //                    chartGrid.Width = legendSize;
            //                    chartGrid.Height = axisFreezeInfo.Height;
            //                    Canvas.SetLeft(chartGrid, 0);
            //                    Canvas.SetTop(chartGrid, 0);
            //                    break;
            //                case ChartDockOrientation.Top:
            //                    top += legendSize;
            //                    chartGrid.Width = axisFreezeInfo.Width;
            //                    chartGrid.Height = legendSize;
            //                    Canvas.SetLeft(chartGrid, 0);
            //                    Canvas.SetTop(chartGrid, 0);
            //                    break;
            //                case ChartDockOrientation.Right:
            //                    right += legendSize;
            //                    chartGrid.Width = legendSize;
            //                    chartGrid.Height = axisFreezeInfo.Height;
            //                    Canvas.SetRight(chartGrid, 0);
            //                    Canvas.SetTop(chartGrid, 0);
            //                    break;
            //                case ChartDockOrientation.Bottom:
            //                    bottom += legendSize;
            //                    chartGrid.Width = axisFreezeInfo.Width;
            //                    chartGrid.Height = legendSize;
            //                    Canvas.SetLeft(chartGrid, 0);
            //                    Canvas.SetBottom(chartGrid, 0);
            //                    break;
            //                default:
            //                    throw new NotImplementedException(legend.DockOrientation.ToString());
            //            }
            //        }
            //    }
            //}


            ////第二步
            //double topAxisTotalHeight = 0d, bottomAxisTotalHeight = 0d;
            //bool hasAxis = axisCollection != null && axisCollection.Count > 0;
            //if (hasAxis)
            //{
            //    double axisHeight;
            //    foreach (var axis in axisCollection)
            //    {
            //        axis.Validate();//验证坐标轴有效性

            //        if (axis.AxisType != AxisType.X)
            //        {
            //            continue;
            //        }

            //        axisHeight = axis.GetXAxisHeight();
            //        if (axis.IsAxisXBottom())
            //        {
            //            bottomAxisTotalHeight += axisHeight;
            //        }
            //        else
            //        {
            //            topAxisTotalHeight += axisHeight;
            //        }
            //    }
            //}

            ////第三步
            //double chartAreaHeight = axisFreezeInfo.Height - top - bottom - topAxisTotalHeight - bottomAxisTotalHeight;
            //double leftAxisTotalWidth = 0d, rightAxisTotalWidth = 0d;
            //if (hasAxis)
            //{
            //    double yAxisHeight = chartAreaHeight;
            //    double axisWidth;
            //    FrameworkElement axisYControl;
            //    double axisLeft = left, axisRight = right;

            //    foreach (var axis in axisCollection)
            //    {
            //        if (axis.AxisType != AxisType.Y)
            //        {
            //            continue;
            //        }

            //        axis.DrawY(seriesCollection, yAxisHeight);
            //        axisWidth = axis.Width;
            //        axisYControl = axis.GetAxisControl();
            //        chartCanvas.Children.Add(axisYControl);
            //        if (axis.IsAxisYLeft())
            //        {
            //            Canvas.SetLeft(axisYControl, axisLeft);
            //            Canvas.SetTop(axisYControl, topAxisTotalHeight);
            //            leftAxisTotalWidth += axisWidth;
            //            axisLeft += axisWidth;
            //        }
            //        else
            //        {
            //            Canvas.SetRight(axisYControl, axisRight);
            //            Canvas.SetTop(axisYControl, topAxisTotalHeight);
            //            rightAxisTotalWidth += axisWidth;
            //            axisRight += axisWidth;
            //        }
            //    }
            //}


            ////第四步
            //double chartAreaWidth = axisFreezeInfo.Width - left - right - leftAxisTotalWidth - rightAxisTotalWidth;

            ////第五步
            //if (hasAxis)
            //{
            //    double xAxisWidth = chartAreaWidth;
            //    FrameworkElement axisXControl;
            //    double axisTop = top, axisBottom = bottom;

            //    foreach (var axis in axisCollection)
            //    {
            //        if (axis.AxisType != AxisType.X)
            //        {
            //            continue;
            //        }

            //        axis.DrawX(seriesCollection, xAxisWidth);
            //        axisXControl = axis.GetAxisControl();
            //        chartCanvas.Children.Add(axisXControl);
            //        if (axis.IsAxisXBottom())
            //        {
            //            Canvas.SetLeft(axisXControl, leftAxisTotalWidth);
            //            Canvas.SetBottom(axisXControl, axisBottom);
            //            axisBottom += axisXControl.Height;
            //        }
            //        else
            //        {
            //            Canvas.SetLeft(axisXControl, leftAxisTotalWidth);
            //            Canvas.SetTop(axisXControl, axisTop);
            //            axisTop += axisXControl.Height;
            //        }
            //    }
            //}


            ////第六步
            //var chartAreaSize = new Size(chartAreaWidth, chartAreaHeight);
            //double chartAreaX = left + leftAxisTotalWidth;
            //double chartAreaY = top + topAxisTotalHeight;
            //var chartArea = new Rect(chartAreaX, chartAreaY, chartAreaWidth, chartAreaHeight);


            ////第七步
            //if (seriesCollection != null && seriesCollection.Count > 0)
            //{
            //    foreach (ISeries series in seriesCollection)
            //    {
            //        series.Add(chartCanvas, chartAreaSize);
            //    }
            //}

            ////第八步
            //if (hasLegend)
            //{
            //    var legendBrushList = new List<SeriesLegendItem>();
            //    if (seriesCollection != null && seriesCollection.Count > 0)
            //    {
            //        foreach (ISeries series in seriesCollection)
            //        {
            //            series.FillLegendItemToList(legendBrushList);
            //        }
            //    }

            //    legend.UpdateLegend(legendBrushList);
            //}
        }


        private void UpdateNoneFreeze(AxisFreezeInfo axisFreezeInfo, ChartCollection<AxisAbs> axisCollection, ChartCollection<ISeries> seriesCollection,
            IChartLegend legend, Canvas chartCanvas, Grid chartGrid)
        {
            /************************************************************************************************************
             * 步骤:
             * 1.添加legend,并计算出发四周所占高度或宽度
             * 2.计算X轴总高度
             * 3.根据X轴总高度计算图表区域高度高度(等于Y轴高度)
             * 4.根据Y轴高度绘制Y轴,并计算Y轴宽度
             * 5.根据Y轴宽度计算X轴宽度并绘制X轴
             * 6.绘制坐标背景标记线
             * 7.绘各Series
             * 8.填充legend
             * 9.布局UI
             ************************************************************************************************************/

            chartGrid.Width = axisFreezeInfo.Width;
            chartGrid.Height = axisFreezeInfo.Height;
            //chartGrid.ShowGridLines = true;
            //chartCanvas.Background = ColorBrushHelper.GetNextColor();
            this.Content = chartGrid;





            //第一步 添加legend,并计算出发四周所占高度或宽度
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


            //第二步 计算X轴总高度
            double topAxisTotalHeight = AxisConstant.ZERO_D, bottomAxisTotalHeight = AxisConstant.ZERO_D;
            bool hasAxis = axisCollection != null && axisCollection.Count > AxisConstant.ZERO_I;
            if (hasAxis)
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

            //第三步 根据X轴总高度计算图表区域高度高度(等于Y轴高度)
            double yAxisHeight = axisFreezeInfo.Height - topAxisTotalHeight - bottomAxisTotalHeight - top - bottom;



            //第四步 根据Y轴高度绘制Y轴,并计算Y轴宽度
            double leftAxisTotalWidth = AxisConstant.ZERO_D, rightAxisTotalWidth = AxisConstant.ZERO_D;
            Dictionary<AxisAbs, List<double>> axisYLabelDic = null;
            if (hasAxis)
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
                    chartGrid.Children.Add(axisYControl);

                    if (axis.IsAxisYLeft())
                    {
                        axisYControl.HorizontalAlignment = HorizontalAlignment.Left;
                        axisYControl.Margin = new Thickness(axisLeft, AxisConstant.ZERO_D, AxisConstant.ZERO_D, AxisConstant.ZERO_D);
                        leftAxisTotalWidth += axis.Width;
                        axisLeft += axis.Width;
                    }
                    else
                    {
                        axisYControl.HorizontalAlignment = HorizontalAlignment.Right;
                        axisYControl.Margin = new Thickness(AxisConstant.ZERO_D, AxisConstant.ZERO_D, axisRight, AxisConstant.ZERO_D);
                        rightAxisTotalWidth += axis.Width;
                        axisRight += axis.Width;
                    }
                }
            }


            //第五步 根据Y轴宽度计算X轴宽度并绘制X轴
            Dictionary<AxisAbs, List<double>> axisXLabelDic = null;
            double xAxisWidth = axisFreezeInfo.Width - leftAxisTotalWidth - rightAxisTotalWidth - left - right;
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
                    chartGrid.Children.Add(axisXControl);
                    if (axis.IsAxisXBottom())
                    {
                        axisXControl.VerticalAlignment = VerticalAlignment.Bottom;
                        axisXControl.Margin = new Thickness(AxisConstant.ZERO_D, AxisConstant.ZERO_D, AxisConstant.ZERO_D, axisBottom);
                        axisBottom += axisXControl.Height;
                    }
                    else
                    {
                        axisXControl.VerticalAlignment = VerticalAlignment.Top;
                        axisXControl.Margin = new Thickness(AxisConstant.ZERO_D, axisTop, AxisConstant.ZERO_D, AxisConstant.ZERO_D);
                        axisTop += axisXControl.Height;
                    }
                }
            }

            //第六步 绘制坐标背景标记线
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
                        labelLineSegments.Add(new BackgroundLabelLineSegment(new Point(AxisConstant.ZERO_D, y), new Point(xAxisWidth, y)));
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
                        labelLineSegments.Add(new BackgroundLabelLineSegment(new Point(x, AxisConstant.ZERO_D), new Point(x, yAxisHeight)));
                    }
                    backgroundLabelLine = kv.Key.CreateBackgroundLabelLine(labelLineSegments);
                    if (backgroundLabelLine != null)
                    {
                        chartCanvas.Children.Add(backgroundLabelLine);
                    }
                }
            }




            //第七步 绘各Series
            chartCanvas.Width = xAxisWidth;
            chartCanvas.Height = yAxisHeight;
            chartGrid.Children.Add(chartCanvas);
            if (seriesCollection != null && seriesCollection.Count > AxisConstant.ZERO_I)
            {
                foreach (ISeries series in seriesCollection)
                {
                    series.Add(chartCanvas);
                }
            }



            //第八步 填充legend
            if (hasLegend)
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



            //第九步 布局UI
            var chartGridRowColumnDefinition = new ChartGridRowColumnDefinition(hasLegend, legend, chartGrid,
                leftAxisTotalWidth, rightAxisTotalWidth, topAxisTotalHeight, bottomAxisTotalHeight);
            if (hasLegend)
            {
                this.SetRowColumn(chartGrid, legend.LegendControl, chartGridRowColumnDefinition.Legend);
            }

            if (seriesCollection != null && seriesCollection.Count > AxisConstant.ZERO_I)
            {
                RowColumnDefinitionItem rowColumnDefinition;
                foreach (var axis in axisCollection)
                {
                    switch (axis.DockOrientation)
                    {
                        case ChartDockOrientation.Left:
                            rowColumnDefinition = chartGridRowColumnDefinition.LeftAxis;
                            break;
                        case ChartDockOrientation.Top:
                            rowColumnDefinition = chartGridRowColumnDefinition.TopAxis;
                            break;
                        case ChartDockOrientation.Right:
                            rowColumnDefinition = chartGridRowColumnDefinition.RightAxis;
                            break;
                        case ChartDockOrientation.Bottom:
                            rowColumnDefinition = chartGridRowColumnDefinition.BottomAxis;
                            break;
                        default:
                            throw new NotImplementedException(axis.DockOrientation.ToString());
                    }

                    this.SetRowColumn(chartGrid, axis.AxisControl, rowColumnDefinition);
                }
            }

            this.SetRowColumn(chartGrid, chartCanvas, chartGridRowColumnDefinition.Chart);
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