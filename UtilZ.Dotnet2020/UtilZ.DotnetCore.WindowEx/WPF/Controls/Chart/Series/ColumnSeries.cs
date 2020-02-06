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



        private readonly List<Rectangle> _columnElementList = new List<Rectangle>();



        public ColumnSeries()
            : base()
        {

        }

        protected override void PrimitiveAdd(Canvas canvas)
        {
            this._columnElementList.Clear();
            Brush legendBrush = AxisHelper.CreateColumn(this, null).Fill.Clone();
            base.AddOrReplaceLegendItem(new SeriesLegendItem(legendBrush, base.Title, this));

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
            if (base._values == null || base._values.Count == 0)
            {
                return;
            }

            double x, y;
            Rectangle columnElement;
            foreach (var item in base._values)
            {
                x = this.AxisX.GetX(item);
                if (!AxisHelper.DoubleHasValue(x))
                {
                    continue;
                }

                y = this.AxisY.GetY(item);
                if (!AxisHelper.DoubleHasValue(y))
                {
                    continue;
                }

                columnElement = AxisHelper.CreateColumn(this, item);
                if (AxisHelper.DoubleHasValue(this._size))
                {
                    columnElement.Height = this._size;
                }

                canvas.Children.Add(columnElement);
                this._columnElementList.Add(columnElement);

                if (this.AxisY.IsAxisYLeft())
                {
                    columnElement.Width = x;
                    Canvas.SetLeft(columnElement, AxisConstant.ZERO_D);
                }
                else
                {
                    columnElement.Width = canvas.Width - x;
                    Canvas.SetRight(columnElement, AxisConstant.ZERO_D);
                }

                Canvas.SetTop(columnElement, y);
            }
        }





        private void PrimitiveAddVertical(Canvas canvas)
        {
            if (base._values == null || base._values.Count == 0)
            {
                return;
            }

            double x, y;
            Rectangle columnElement;
            foreach (var item in base._values)
            {
                x = this.AxisX.GetX(item);
                if (!AxisHelper.DoubleHasValue(x))
                {
                    continue;
                }

                y = this.AxisY.GetY(item);
                if (!AxisHelper.DoubleHasValue(y))
                {
                    continue;
                }

                columnElement = AxisHelper.CreateColumn(this, item);
                if (AxisHelper.DoubleHasValue(this._size))
                {
                    columnElement.Width = this._size;
                }

                canvas.Children.Add(columnElement);
                this._columnElementList.Add(columnElement);

                if (this.AxisX.IsAxisXBottom())
                {
                    columnElement.Height = canvas.Height - y;
                    Canvas.SetBottom(columnElement, AxisConstant.ZERO_D);
                }
                else
                {
                    columnElement.Height = y;
                    Canvas.SetTop(columnElement, AxisConstant.ZERO_D);
                }

                Canvas.SetLeft(columnElement, x);
            }
        }





        protected override bool PrimitiveRemove(Canvas canvas)
        {
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
