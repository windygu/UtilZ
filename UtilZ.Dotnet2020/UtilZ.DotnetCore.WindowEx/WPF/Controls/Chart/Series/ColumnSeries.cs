using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    /// <summary>
    /// 条形图
    /// </summary>
    public class ColumnSeries : SeriesAbs, IColumnSeries
    {
        private SeriesOrientation _orientation = SeriesOrientation.Vertical;
        /// <summary>
        /// 获取或设置ColumnSeries方向
        /// </summary>
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
        /// <summary>
        /// 获取或设置ColumnSeries水平方向高度,垂直方向宽度,为double.NaN则自动计算,默认为double.NaN
        /// </summary>
        public double Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// 获取ColumnSeries样式
        /// </summary>
        /// <returns>ColumnSeries样式</returns>
        public Style GetStyle()
        {
            return base.Style;
        }


        private readonly List<Rectangle> _columnElementList = new List<Rectangle>();



        /// <summary>
        /// 构造函数
        /// </summary>
        public ColumnSeries()
            : base()
        {

        }

        /// <summary>
        /// 重写StyleChanged
        /// </summary>
        /// <param name="style">新样式</param>
        protected override void StyleChanged(Style style)
        {
            base.RemoveLegendItem();
            Brush legendBrush = AxisHelper.CreateColumn(this).Fill.Clone();
            base.AddLegendItem(new SeriesLegendItem(legendBrush, base.Title, this));

            foreach (var columnElement in this._columnElementList)
            {
                columnElement.Style = style;
            }
        }



        /// <summary>
        /// 重写PrimitiveAdd
        /// </summary>
        /// <param name="canvas"></param>
        protected override void PrimitiveAdd(Canvas canvas)
        {
            this._columnElementList.Clear();
            Brush legendBrush = AxisHelper.CreateColumn(this).Fill.Clone();
            base.AddLegendItem(new SeriesLegendItem(legendBrush, base.Title, this));

            if (base._values == null || base._values.Count == 0)
            {
                return;
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
            double x, y;
            Rectangle columnElement;

            foreach (var value in base._values)
            {
                x = this.AxisX.GetX(value);
                if (!AxisHelper.DoubleHasValue(x))
                {
                    continue;
                }

                y = this.AxisY.GetY(value);
                if (!AxisHelper.DoubleHasValue(y))
                {
                    continue;
                }

                columnElement = AxisHelper.CreateColumn(this);
                if (value != null)
                {
                    AxisHelper.SetColumnTooltipText(this, value.TooltipText, columnElement);
                }

                if (AxisHelper.DoubleHasValue(this._size))
                {
                    columnElement.Height = this._size;
                }

                canvas.Children.Add(columnElement);
                this._columnElementList.Add(columnElement);

                if (this.AxisY.IsAxisYLeft())
                {
                    columnElement.Width = x;
                    Canvas.SetLeft(columnElement, ChartConstant.ZERO_D);
                }
                else
                {
                    columnElement.Width = canvas.Width - x;
                    Canvas.SetRight(columnElement, ChartConstant.ZERO_D);
                }

                Canvas.SetTop(columnElement, y);
            }
        }





        private void PrimitiveAddVertical(Canvas canvas)
        {
            double x, y;
            Rectangle columnElement;

            foreach (var value in base._values)
            {
                x = this.AxisX.GetX(value);
                if (!AxisHelper.DoubleHasValue(x))
                {
                    continue;
                }

                y = this.AxisY.GetY(value);
                if (!AxisHelper.DoubleHasValue(y))
                {
                    continue;
                }

                columnElement = AxisHelper.CreateColumn(this);
                if (value != null)
                {
                    AxisHelper.SetColumnTooltipText(this, value.TooltipText, columnElement);
                }

                if (AxisHelper.DoubleHasValue(this._size))
                {
                    columnElement.Width = this._size;
                }

                canvas.Children.Add(columnElement);
                this._columnElementList.Add(columnElement);

                if (this.AxisX.IsAxisXBottom())
                {
                    columnElement.Height = canvas.Height - y;
                    Canvas.SetBottom(columnElement, ChartConstant.ZERO_D);
                }
                else
                {
                    columnElement.Height = y;
                    Canvas.SetTop(columnElement, ChartConstant.ZERO_D);
                }

                Canvas.SetLeft(columnElement, x);
            }
        }




        /// <summary>
        /// 重写PrimitiveRemove
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
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





        /// <summary>
        /// 重写VisibilityChanged
        /// </summary>
        /// <param name="oldVisibility"></param>
        /// <param name="newVisibility"></param>
        protected override void VisibilityChanged(Visibility oldVisibility, Visibility newVisibility)
        {
            foreach (var columnElement in this._columnElementList)
            {
                columnElement.Visibility = newVisibility;
            }
        }
    }
}
