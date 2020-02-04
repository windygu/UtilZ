using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.DotnetCore.WindowEx.WPF.Controls;

namespace WpfApp1.Model
{
    public class ChartDateTimeItem : ChartItemAbs
    {
        private DateTime _x;
        public DateTime X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y { get; set; }

        public ChartDateTimeItem(DateTime x, double value, string tooltipText)
            : base(tooltipText, null)
        {
            this._x = x;
            this.Y = value;
        }

        public override string ToString()
        {
            return $"{_x} {Y}";
        }

        public override object GetXValue()
        {
            return this.X;
        }

        public override object GetYValue()
        {
            return this.Y;
        }
    }


    public class ChartDateTimeItem2 : ChartItemAbs
    {
        public DateTime Y { get; set; }

        public double X { get; set; }

        public ChartDateTimeItem2(DateTime y, double x, string tooltipText)
            : base(tooltipText, null)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }

        public override object GetXValue()
        {
            return this.X;
        }

        public override object GetYValue()
        {
            return this.Y;
        }
    }
}
