using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilZ.DotnetCore.WindowEx.WPF.Controls;

namespace WpfApp1.Model
{
    public class ChartColumnItemHorizontal : ChartItemAbs
    {
        public DateTime Y { get; set; }
        public double X { get; set; }


        public ChartColumnItemHorizontal(DateTime y, double x, string tooltip)
            : base(tooltip, null)
        {
            X = x;
            Y = y;
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

    public class ChartColumnItemVertical : ChartItemAbs
    {
        public double Y { get; set; }
        public DateTime X { get; set; }



        public ChartColumnItemVertical(DateTime x, double y, string tooltip)
             : base(tooltip, null)
        {
            X = x;
            Y = y;
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
