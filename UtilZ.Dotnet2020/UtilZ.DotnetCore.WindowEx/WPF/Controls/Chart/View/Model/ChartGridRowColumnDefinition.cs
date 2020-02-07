using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    internal class ChartGridRowColumnDefinition
    {
        public RowColumnDefinitionItem Legend { get; private set; }

        public RowColumnDefinitionItem LeftAxis { get; private set; }

        public RowColumnDefinitionItem RightAxis { get; private set; }

        public RowColumnDefinitionItem TopAxis { get; private set; }

        public RowColumnDefinitionItem BottomAxis { get; private set; }

        public RowColumnDefinitionItem Chart { get; private set; }


        /// <summary>
        /// NoFreeze
        /// </summary>
        /// <param name="hasLegend"></param>
        /// <param name="legend"></param>
        /// <param name="chartGrid"></param>
        /// <param name="axisYWidthInfo"></param>
        /// <param name="axisXHeightInfo"></param>
        public ChartGridRowColumnDefinition(bool hasLegend, IChartLegend legend, Grid chartGrid,
           AxisYWidthInfo axisYWidthInfo, AxisXHeightInfo axisXHeightInfo)
        {
            if (hasLegend)
            {
                this.Legend = new RowColumnDefinitionItem();
            }

            if (axisYWidthInfo.LeftAxisTotalWidth > AxisConstant.ZERO_D)
            {
                this.LeftAxis = new RowColumnDefinitionItem();
            }

            if (axisYWidthInfo.RightAxisTotalWidth > AxisConstant.ZERO_D)
            {
                this.RightAxis = new RowColumnDefinitionItem();
            }

            if (axisXHeightInfo.TopAxisTotalHeight > AxisConstant.ZERO_D)
            {
                this.TopAxis = new RowColumnDefinitionItem();
            }

            if (axisXHeightInfo.BottomAxisTotalHeight > AxisConstant.ZERO_D)
            {
                this.BottomAxis = new RowColumnDefinitionItem();
            }

            this.Chart = new RowColumnDefinitionItem();



            if (axisYWidthInfo.LeftAxisTotalWidth > AxisConstant.ZERO_D)
            {
                this.LeftAxis.CreateColumn(chartGrid, axisYWidthInfo.LeftAxisTotalWidth, GridUnitType.Pixel);
            }
            this.Chart.CreateColumn(chartGrid, AxisConstant.GRID_START_SIZE, GridUnitType.Star);
            if (axisYWidthInfo.RightAxisTotalWidth > AxisConstant.ZERO_D)
            {
                this.RightAxis.CreateColumn(chartGrid, axisYWidthInfo.RightAxisTotalWidth, GridUnitType.Pixel);
            }


            if (axisXHeightInfo.TopAxisTotalHeight > AxisConstant.ZERO_D)
            {
                this.TopAxis.CreateRow(chartGrid, axisXHeightInfo.TopAxisTotalHeight, GridUnitType.Pixel);
            }
            this.Chart.CreateRow(chartGrid, AxisConstant.GRID_START_SIZE, GridUnitType.Star);
            if (axisXHeightInfo.BottomAxisTotalHeight > AxisConstant.ZERO_D)
            {
                this.BottomAxis.CreateRow(chartGrid, axisXHeightInfo.BottomAxisTotalHeight, GridUnitType.Pixel);
            }

            if (hasLegend)
            {
                this.Legend.CreateLegendRowColumn(chartGrid, legend);
            }


            if (this.LeftAxis != null)
            {
                this.LeftAxis.MergeRowColumn(chartGrid, this.Chart);
            }

            if (this.RightAxis != null)
            {
                this.RightAxis.MergeRowColumn(chartGrid, this.Chart);
            }

            if (this.TopAxis != null)
            {
                this.TopAxis.MergeRowColumn(chartGrid, this.Chart);
            }

            if (this.BottomAxis != null)
            {
                this.BottomAxis.MergeRowColumn(chartGrid, this.Chart);
            }

            this.Chart.MergeRowColumn(chartGrid, this.Chart);
        }


        /// <summary>
        /// FreezeY
        /// </summary>
        /// <param name="hasLegend"></param>
        /// <param name="legend"></param>
        /// <param name="chartGrid"></param>
        /// <param name="axisYWidthInfo"></param>
        public ChartGridRowColumnDefinition(bool hasLegend, IChartLegend legend, Grid chartGrid,
          AxisYWidthInfo axisYWidthInfo)
        {
            if (hasLegend)
            {
                this.Legend = new RowColumnDefinitionItem();
            }

            if (axisYWidthInfo.LeftAxisTotalWidth > AxisConstant.ZERO_D)
            {
                this.LeftAxis = new RowColumnDefinitionItem();
            }

            if (axisYWidthInfo.RightAxisTotalWidth > AxisConstant.ZERO_D)
            {
                this.RightAxis = new RowColumnDefinitionItem();
            }

            this.Chart = new RowColumnDefinitionItem();





            if (axisYWidthInfo.LeftAxisTotalWidth > AxisConstant.ZERO_D)
            {
                this.LeftAxis.CreateColumn(chartGrid, axisYWidthInfo.LeftAxisTotalWidth, GridUnitType.Pixel);
            }
            this.Chart.CreateColumn(chartGrid, AxisConstant.GRID_START_SIZE, GridUnitType.Star);
            if (axisYWidthInfo.RightAxisTotalWidth > AxisConstant.ZERO_D)
            {
                this.RightAxis.CreateColumn(chartGrid, axisYWidthInfo.RightAxisTotalWidth, GridUnitType.Pixel);
            }

            if (hasLegend)
            {
                this.Legend.CreateLegendRowColumn(chartGrid, legend);
            }

            if (this.LeftAxis != null)
            {
                this.LeftAxis.MergeRowColumn(chartGrid, this.Chart);
            }

            if (this.RightAxis != null)
            {
                this.RightAxis.MergeRowColumn(chartGrid, this.Chart);
            }

            this.Chart.MergeRowColumn(chartGrid, this.Chart);
        }

        public ChartGridRowColumnDefinition(Grid chartContentGrid, AxisXHeightInfo axisXHeightInfo)
        {
            if (axisXHeightInfo.TopAxisTotalHeight > AxisConstant.ZERO_D)
            {
                this.TopAxis = new RowColumnDefinitionItem();
            }

            if (axisXHeightInfo.BottomAxisTotalHeight > AxisConstant.ZERO_D)
            {
                this.BottomAxis = new RowColumnDefinitionItem();
            }

            this.Chart = new RowColumnDefinitionItem();


            if (axisXHeightInfo.TopAxisTotalHeight > AxisConstant.ZERO_D)
            {
                this.TopAxis.CreateRow(chartContentGrid, axisXHeightInfo.TopAxisTotalHeight, GridUnitType.Pixel);
            }
            this.Chart.CreateRow(chartContentGrid, AxisConstant.GRID_START_SIZE, GridUnitType.Star);
            if (axisXHeightInfo.BottomAxisTotalHeight > AxisConstant.ZERO_D)
            {
                this.BottomAxis.CreateRow(chartContentGrid, axisXHeightInfo.BottomAxisTotalHeight, GridUnitType.Pixel);
            }



            if (this.TopAxis != null)
            {
                this.TopAxis.MergeRowColumn(chartContentGrid, this.Chart);
            }

            if (this.BottomAxis != null)
            {
                this.BottomAxis.MergeRowColumn(chartContentGrid, this.Chart);
            }

            this.Chart.MergeRowColumn(chartContentGrid, this.Chart);
        }
    }


    public class RowColumnDefinitionItem
    {
        private RowDefinition _row = null;
        private ColumnDefinition _column = null;

        public int RowIndex { get; set; } = AxisConstant.ROW_COLUMN_DEFAULT_INDEX;

        public int ColumnIndex { get; set; } = AxisConstant.ROW_COLUMN_DEFAULT_INDEX;

        public int RowSpan { get; set; } = AxisConstant.ROW_COLUMN_DEFAULT_INDEX;

        public int ColumnSpan { get; set; } = AxisConstant.ROW_COLUMN_DEFAULT_INDEX;


        public RowColumnDefinitionItem()
        {

        }


        internal void CreateRow(Grid chartGrid, double height, GridUnitType gridUnitType)
        {
            this._row = new RowDefinition() { Height = new GridLength(height, gridUnitType) };
            chartGrid.RowDefinitions.Add(this._row);

        }

        internal void CreateColumn(Grid chartGrid, double width, GridUnitType gridUnitType)
        {
            this._column = new ColumnDefinition() { Width = new GridLength(width, gridUnitType) };
            chartGrid.ColumnDefinitions.Add(this._column);
        }


        internal void CreateLegendRowColumn(Grid chartGrid, IChartLegend legend)
        {
            double legendSize = legend.Size;
            switch (legend.DockOrientation)
            {
                case ChartDockOrientation.Left:
                case ChartDockOrientation.Right:
                    this._column = new ColumnDefinition() { Width = new GridLength(legend.Size, GridUnitType.Pixel) };
                    if (legend.DockOrientation == ChartDockOrientation.Left)
                    {
                        chartGrid.ColumnDefinitions.Insert(0, this._column);
                        this.ColumnIndex = 0;
                    }
                    else
                    {
                        chartGrid.ColumnDefinitions.Add(this._column);
                        this.ColumnIndex = chartGrid.ColumnDefinitions.Count - 1;
                    }
                    this.RowIndex = 0;
                    this.RowSpan = chartGrid.RowDefinitions.Count;
                    break;
                case ChartDockOrientation.Top:
                case ChartDockOrientation.Bottom:
                    this._row = new RowDefinition() { Height = new GridLength(legend.Size, GridUnitType.Pixel) };
                    if (legend.DockOrientation == ChartDockOrientation.Top)
                    {
                        chartGrid.RowDefinitions.Insert(0, this._row);
                        this.RowIndex = 0;
                    }
                    else
                    {
                        chartGrid.RowDefinitions.Add(this._row);
                        this.RowIndex = chartGrid.RowDefinitions.Count - 1;
                    }

                    this.ColumnIndex = 0;
                    this.ColumnSpan = chartGrid.ColumnDefinitions.Count;
                    break;
                default:
                    throw new NotImplementedException(legend.DockOrientation.ToString());
            }
        }


        internal void MergeRowColumn(Grid chartGrid, RowColumnDefinitionItem chartRowColumnDefinitionItem)
        {
            if (this._column == null)
            {
                this._column = chartRowColumnDefinitionItem._column;
            }

            if (this._row == null)
            {
                this._row = chartRowColumnDefinitionItem._row;
            }

            if (this._row != null)
            {
                this.RowIndex = chartGrid.RowDefinitions.IndexOf(this._row);
            }

            if (this._column != null)
            {
                this.ColumnIndex = chartGrid.ColumnDefinitions.IndexOf(this._column);
            }
        }
    }
}
