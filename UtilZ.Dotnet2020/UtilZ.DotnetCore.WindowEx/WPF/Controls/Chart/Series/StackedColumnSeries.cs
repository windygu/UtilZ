using System;
using System.Collections;
using System.Collections.Generic;
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
            set => throw new NotSupportedException($"请通过{nameof(StyleList)}属性设置堆叠样式");
        }

        private List<Style> _styleList = null;
        public List<Style> StyleList
        {
            get { return _styleList; }
            set
            {
                _styleList = value;
                base.OnRaisePropertyChanged(nameof(StyleList));
            }
        }

        public Style GetStyle()
        {
            return this.GetStyle(0);
        }

        private Style GetStyle(int index)
        {
            Style style;
            if (this._styleList == null ||
                index < 0 ||
                index >= this._styleList.Count)
            {
                style = ChartStyleHelper.CreateColumnSeriesStyle(ColorBrushHelper.GetColorByIndex(index));
            }
            else
            {
                style = this._styleList[index];
            }
            return style;
        }


        private readonly List<Rectangle> _columnElementList = new List<Rectangle>();

        public StackedColumnSeries()
           : base()
        {

        }




        protected override void PrimitiveAdd(Canvas canvas)
        {
            this._columnElementList.Clear();
            //Brush legendBrush = AxisHelper.CreateColumn(this).Fill.Clone();
            //base.AddOrReplaceLegendItem(new SeriesLegendItem(legendBrush, base.Title, this));

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
            throw new NotImplementedException();
        }

        private void PrimitiveAddVertical(Canvas canvas)
        {
            throw new NotImplementedException();
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
