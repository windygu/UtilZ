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
            /************************************************************************************************************
             * 步骤:
             * 1.添加legend,并计算出发四周所占高度或宽度
             * 2.计算X轴总高度
             * 3.根据X轴总高度计算Y轴高度(图表区域高度)
             * 4.根据Y轴高度计算刻度值中最大宽度的刻度值,以求出单个Y轴宽度和所有Y轴宽度(为了解决Y轴刻度显示不完或空白过多)
             * 5.绘制X轴
             * 6.根据2和3计算图表区域宽度
             * 7.绘图
             * 8.填充legend
             ************************************************************************************************************/

            chartCanvas.Width = axisFreezeInfo.Width;
            chartCanvas.Height = axisFreezeInfo.Height;
            this.Content = chartCanvas;



            double left = 0d, top = 0d, right = 0d, bottom = 0d;

            //第一步
            bool hasLegend = false;
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
                        chartGrid.Children.Add(legendControl);//使用Grid包装,主是为了居中对齐好布局,Canvas中水平垂直方向太麻烦
                        chartCanvas.Children.Add(chartGrid);
                        hasLegend = true;

                        switch (legend.DockOrientation)
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
                                throw new NotImplementedException(legend.DockOrientation.ToString());
                        }
                    }
                }
            }


            //第二步
            double topAxisTotalHeight = 0d, bottomAxisTotalHeight = 0d;
            bool hasAxis = axisCollection != null && axisCollection.Count > 0;
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

            //第三步
            double chartAreaHeight = axisFreezeInfo.Height - top - bottom - topAxisTotalHeight - bottomAxisTotalHeight;
            double leftAxisTotalWidth = 0d, rightAxisTotalWidth = 0d;
            if (hasAxis)
            {
                double yAxisHeight = chartAreaHeight;
                double axisWidth;
                FrameworkElement axisYControl;
                double axisLeft = left, axisRight = right;

                foreach (var axis in axisCollection)
                {
                    if (axis.AxisType != AxisType.Y)
                    {
                        continue;
                    }

                    axisWidth = axis.DrawY(seriesCollection, yAxisHeight);
                    axisYControl = axis.GetAxisControl();
                    chartCanvas.Children.Add(axisYControl);
                    if (axis.IsAxisYLeft())
                    {
                        Canvas.SetLeft(axisYControl, axisLeft);
                        Canvas.SetTop(axisYControl, topAxisTotalHeight);
                        leftAxisTotalWidth += axisWidth;
                        axisLeft += axisWidth;
                    }
                    else
                    {
                        Canvas.SetRight(axisYControl, axisRight);
                        Canvas.SetTop(axisYControl, topAxisTotalHeight);
                        rightAxisTotalWidth += axisWidth;
                        axisRight += axisWidth;
                    }
                }
            }


            //第四步
            double chartAreaWidth = axisFreezeInfo.Width - left - right - leftAxisTotalWidth - rightAxisTotalWidth;

            //第五步
            if (hasAxis)
            {
                double xAxisWidth = chartAreaWidth;
                FrameworkElement axisXControl;
                double axisTop = top, axisBottom = bottom;

                foreach (var axis in axisCollection)
                {
                    if (axis.AxisType != AxisType.X)
                    {
                        continue;
                    }

                    axis.DrawX(seriesCollection, xAxisWidth);
                    axisXControl = axis.GetAxisControl();
                    chartCanvas.Children.Add(axisXControl);
                    if (axis.IsAxisXBottom())
                    {
                        Canvas.SetLeft(axisXControl, leftAxisTotalWidth);
                        Canvas.SetBottom(axisXControl, axisBottom);
                        axisBottom += axisXControl.Height;
                    }
                    else
                    {
                        Canvas.SetLeft(axisXControl, leftAxisTotalWidth);
                        Canvas.SetTop(axisXControl, axisTop);
                        axisTop += axisXControl.Height;
                    }
                }
            }


            //第六步
            double chartAreaX = left + leftAxisTotalWidth;
            double chartAreaY = top + topAxisTotalHeight;
            var chartArea = new Rect(chartAreaX, chartAreaY, chartAreaWidth, chartAreaHeight);


            //第七步
            if (seriesCollection != null && seriesCollection.Count > 0)
            {
                foreach (ISeries series in seriesCollection)
                {
                    series.Add(chartCanvas, chartArea);
                }
            }

            //第八步
            if (hasLegend)
            {
                var legendBrushList = new List<SeriesLegendItem>();
                if (seriesCollection != null && seriesCollection.Count > 0)
                {
                    foreach (ISeries series in seriesCollection)
                    {
                        series.FillLegendItemToList(legendBrushList);
                    }
                }

                legend.UpdateLegend(legendBrushList);
            }

            ////todo..Test
            //Rectangle reg = new Rectangle();
            //reg.Width = chartArea.Width;
            //reg.Height = chartArea.Height;
            //reg.Fill = Brushes.Green;
            //chartCanvas.Children.Add(reg);
            //Canvas.SetLeft(reg, chartArea.X);
            //Canvas.SetTop(reg, chartArea.Y);
        }




        /*
    private void UpdateNoneFreeze_bk(AxisFreezeInfo axisFreezeInfo, ChartCollection<AxisAbs> axisCollection, ChartCollection<ISeries> seriesCollection,
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
    */
    }
}