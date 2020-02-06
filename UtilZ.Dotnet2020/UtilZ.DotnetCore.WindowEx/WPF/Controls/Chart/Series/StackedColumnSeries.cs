using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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




        private readonly List<Rectangle> _columnElementList = new List<Rectangle>();

        public StackedColumnSeries()
           : base()
        {

        }


        protected override void PrimitiveGetAxisValueArea(AxisAbs axis, out double min, out double max)
        {
            //AxisHelper.GetAxisMinAndMax(axis, this.Values, out min, out max);
            throw new NotImplementedException();
        }


        protected override void PrimitiveAdd(Canvas canvas)
        {
            throw new NotImplementedException();
        }

        protected override bool PrimitiveRemove(Canvas canvas)
        {
            return true;
        }

        protected override void VisibilityChanged(Visibility oldVisibility, Visibility newVisibility)
        {
            throw new NotImplementedException();
        }
    }
}
