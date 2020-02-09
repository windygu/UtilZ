using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class StackedColumnSeries : SeriesAbs, IColumnSeries
    {
        private SeriesOrientation _orientation = SeriesOrientation.Vertical;
        public SeriesOrientation Orientation
        {
            get { return _orientation; }
            set
            {
                if (_orientation == value)
                {
                    return;
                }

                _orientation = value;
                base.OnRaisePropertyChanged(nameof(Orientation));
            }
        }

        private double _size = double.NaN;
        public double Size
        {
            get { return _size; }
            set { _size = value; }
        }


        public override Style Style
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException($"请通过{nameof(TitleStyleDic)}属性设置堆叠标题名称以及样式");
        }

        public override string Title
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException($"请通过{nameof(TitleStyleDic)}属性设置堆叠标题名称以及样式");
        }

        private Dictionary<string, Style> _titleStyleDic = null;
        /// <summary>
        /// 堆叠标题及样式[key:标题;value:样式(为null自动创建)]
        /// </summary>
        public Dictionary<string, Style> TitleStyleDic
        {
            get { return _titleStyleDic; }
            set
            {
                _titleStyleDic = value;
                this.StyleChanged2();
            }
        }


        public Style GetStyle()
        {
            return this.GetStyle(0);
        }

        private Style GetStyle(int index)
        {
            Style style;
            if (this._titleStyleDic == null ||
                index < 0 ||
                index >= this._titleStyleDic.Count)
            {
                style = ChartStyleHelper.CreateColumnSeriesStyle(ColorBrushHelper.GetColorByIndex(index));
            }
            else
            {
                style = this._titleStyleDic.Values.ElementAt(index);
            }
            return style;
        }


        private readonly List<Rectangle> _columnElementList = new List<Rectangle>();

        public StackedColumnSeries()
           : base()
        {

        }


        protected override void StyleChanged(Style style)
        {
        }

        private void StyleChanged2()
        {
            if (this._columnElementList.Count > 0)
            {
                throw new NotSupportedException();
            }
            else
            {
                base.RemoveLegendItem();
            }

            //var legendTemplateColumn = new Rectangle();
            //Brush legendBrush;
            //for (int i = 0; i < this._columnElementList.Count; i++)
            //{
            //    this._columnElementList[i].Style = this.GetStyle(i);
            //    legendBrush = this._columnElementList[i].Fill;
            //    base.AddLegendItem(new SeriesLegendItem(legendBrush, this._titleStyleDic.Keys.ElementAt(i), this));
            //}

            //titleStyleDic.Count -
            //if ()
        }



        protected override void PrimitiveAdd(Canvas canvas)
        {
            this._columnElementList.Clear();
            if (this._titleStyleDic == null || this._titleStyleDic.Count == 0)
            {
                return;
            }

            //添加到SeriesLegendItem集合中
            var legendTemplateColumn = new Rectangle();
            Brush legendBrush;
            for (int i = 0; i < this._titleStyleDic.Count; i++)
            {
                legendTemplateColumn.Style = this.GetStyle(i);
                legendBrush = legendTemplateColumn.Fill.Clone();
                base.AddLegendItem(new SeriesLegendItem(legendBrush, this._titleStyleDic.Keys.ElementAt(i), this));
            }

            switch (this._orientation)
            {
                case SeriesOrientation.Horizontal:
                    this.PrimitiveAddHorizontal(canvas);
                    break;
                case SeriesOrientation.Vertical:
                    this.PrimitiveAddVertical(canvas);
                    break;
                default:
                    throw new NotImplementedException(this._orientation.ToString());
            }
        }

        private void PrimitiveAddHorizontal(Canvas canvas)
        {
            double x, y, leftOrRight;
            object obj;
            IChartAxisValue chartAxisValue;
            IEnumerable enumerable;
            IChartChildValue chartChildValue;
            Rectangle columnElement;
            int stytleIndex;

            foreach (var value in base._values)
            {
                chartAxisValue = value as IChartAxisValue;
                if (chartAxisValue == null)
                {
                    continue;
                }

                y = this.AxisY.GetY(value);
                if (!AxisHelper.DoubleHasValue(y))
                {
                    continue;
                }

                obj = chartAxisValue.GetXValue();
                if (obj == null || !(obj is IEnumerable))
                {
                    continue;
                }

                enumerable = (IEnumerable)obj;
                stytleIndex = -1;
                leftOrRight = AxisConstant.ZERO_D;

                foreach (var item in enumerable)
                {
                    stytleIndex++;
                    if (item == null || !(item is IChartChildValue))
                    {
                        continue;
                    }

                    chartChildValue = (IChartChildValue)item;
                    x = this.AxisX.GetX(chartChildValue);
                    if (!AxisHelper.DoubleHasValue(x))
                    {
                        continue;
                    }

                    columnElement = AxisHelper.CreateColumn(this);
                    columnElement.Style = this.GetStyle(stytleIndex);
                    AxisHelper.SetColumnTooltipText(this, chartChildValue.TooltipText, columnElement);


                    if (AxisHelper.DoubleHasValue(this._size))
                    {
                        columnElement.Height = this._size;
                    }

                    canvas.Children.Add(columnElement);
                    this._columnElementList.Add(columnElement);

                    if (this.AxisY.IsAxisYLeft())
                    {
                        columnElement.Width = x;
                        Canvas.SetLeft(columnElement, leftOrRight);
                    }
                    else
                    {
                        columnElement.Width = canvas.Width - x;
                        Canvas.SetRight(columnElement, leftOrRight);
                    }

                    Canvas.SetTop(columnElement, y);
                    leftOrRight += columnElement.Width;
                }
            }
        }

        private void PrimitiveAddVertical(Canvas canvas)
        {
            double x, y, topOrBottom;
            IChartAxisValue chartAxisValue;
            object obj;
            IEnumerable enumerable;
            IChartChildValue chartChildValue;
            Rectangle columnElement;
            int stytleIndex;

            foreach (var value in base._values)
            {
                chartAxisValue = value as IChartAxisValue;
                if (chartAxisValue == null)
                {
                    continue;
                }

                x = this.AxisX.GetX(chartAxisValue);
                if (!AxisHelper.DoubleHasValue(x))
                {
                    continue;
                }


                obj = chartAxisValue.GetYValue();
                if (obj == null || !(obj is IEnumerable))
                {
                    continue;
                }

                enumerable = (IEnumerable)obj;
                stytleIndex = -1;
                topOrBottom = AxisConstant.ZERO_D;

                foreach (var item in enumerable)
                {
                    stytleIndex++;
                    if (item == null || !(item is IChartChildValue))
                    {
                        continue;
                    }

                    chartChildValue = (IChartChildValue)item;
                    y = this.AxisY.GetY(chartChildValue);
                    if (!AxisHelper.DoubleHasValue(y))
                    {
                        continue;
                    }

                    columnElement = AxisHelper.CreateColumn(this);
                    columnElement.Style = this.GetStyle(stytleIndex);
                    AxisHelper.SetColumnTooltipText(this, chartChildValue.TooltipText, columnElement);

                    if (AxisHelper.DoubleHasValue(this._size))
                    {
                        columnElement.Width = this._size;
                    }

                    canvas.Children.Add(columnElement);
                    this._columnElementList.Add(columnElement);

                    if (this.AxisX.IsAxisXBottom())
                    {
                        columnElement.Height = canvas.Height - y;
                        Canvas.SetBottom(columnElement, topOrBottom);
                    }
                    else
                    {
                        columnElement.Height = y;
                        Canvas.SetTop(columnElement, topOrBottom);
                    }

                    Canvas.SetLeft(columnElement, x);
                    topOrBottom += columnElement.Height;
                }
            }
        }






        protected override bool PrimitiveRemove(Canvas canvas)
        {
            //条形力移除后需要完整的重新绘制
            //foreach (var columnElement in this._columnElementList)
            //{
            //    base._canvas.Children.Remove(columnElement);
            //}

            //this._columnElementList.Clear();

            return true;
        }




        protected override void VisibilityChanged(Visibility oldVisibility, Visibility newVisibility)
        {
            foreach (var columnElement in this._columnElementList)
            {
                columnElement.Visibility = newVisibility;
            }
        }
    }
}
