using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.DotnetCore.WindowEx.WPF.Controls;

namespace WpfApp1.Model
{
    public class ChartNumberItem : ChartItemAbs
    {
        public double X { get; set; }
        public double Y { get; set; }


        public ChartNumberItem(double x, double value, string tooltipText)
          : base(tooltipText, null)
        {
            this.X = x;
            this.Y = value;
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
