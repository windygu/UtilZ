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

            //chartGrid.ShowGridLines = true;
            //chartCanvas.Background = ColorBrushHelper.GetNextColor();
            this.Content = chartGrid;



            //第一步 添加legend,并计算出发四周所占高度或宽度
            LegendAddResult legendAddResult = this.AddLegend(legend, chartGrid);


            //第二步 计算X轴总高度
            AxisXHeightInfo axisXHeightInfo = CalculateAxisXHeight(axisCollection);


            //第三步 根据X轴总高度计算图表区域高度高度(等于Y轴高度)
            double yAxisHeight = axisFreezeInfo.Height - axisXHeightInfo.TopAxisTotalHeight - axisXHeightInfo.BottomAxisTotalHeight - legendAddResult.Top - legendAddResult.Bottom;
            if (yAxisHeight < AxisConstant.ZERO_D)
            {
                yAxisHeight = AxisConstant.ZERO_D;
            }

            //第四步 根据Y轴高度绘制Y轴,并计算Y轴宽度
            AxisYWidthInfo axisYWidthInfo = this.DrawAxisYByAxisXHeightInfo(axisCollection, chartGrid.Children, seriesCollection, yAxisHeight, AxisConstant.ZERO_D);


            //第五步 根据Y轴宽度计算X轴宽度并绘制X轴
            double xAxisWidth = axisFreezeInfo.Width - axisYWidthInfo.LeftAxisTotalWidth - axisYWidthInfo.RightAxisTotalWidth - legendAddResult.Left - legendAddResult.Right;
            if (xAxisWidth < AxisConstant.ZERO_D)
            {
                xAxisWidth = AxisConstant.ZERO_D;
            }

            Dictionary<AxisAbs, List<double>> axisXLabelDic = this.DrawAxisX(axisCollection, seriesCollection, chartGrid, xAxisWidth);

            chartCanvas.Width = xAxisWidth;
            chartCanvas.Height = yAxisHeight;
            chartGrid.Children.Add(chartCanvas);

            //第六步 绘制坐标背景标记线
            this.DrawAxisBackgroundLabelLine(chartCanvas, axisYWidthInfo.AxisYLabelDic, axisXLabelDic);

            //第七步 绘各Series
            this.DrawSeries(chartCanvas, seriesCollection);


            //第八步 填充legend
            if (legendAddResult.HasLegend)
            {
                this.UpdateLegend(legend, seriesCollection);
            }

            //第九步 布局UI
            var chartGridRowColumnDefinition = new ChartGridRowColumnDefinition(legendAddResult.HasLegend, legend, chartGrid, axisYWidthInfo, axisXHeightInfo);
            if (legendAddResult.HasLegend)
            {
                this.SetRowColumn(chartGrid, legend.LegendControl, chartGridRowColumnDefinition.Legend);
            }

            if (axisCollection != null && axisCollection.Count > AxisConstant.ZERO_I)
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
    }
}