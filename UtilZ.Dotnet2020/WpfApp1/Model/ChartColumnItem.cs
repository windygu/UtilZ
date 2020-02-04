using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilZ.DotnetCore.WindowEx.WPF.Controls;

namespace WpfApp1.Model
{
    public class ChartColumnItem : ChartItemAbs
    {
        public LabelAxisItem LabelAxisID { get; set; }
        public double Y { get; set; }


        //public ColumnSeriesItem[] Values { get; set; }

        public ChartColumnItem()
        {

        }

        public override object GetXValue()
        {
            return this.LabelAxisID;
        }

        public override object GetYValue()
        {
            return this.Y;
        }
    }
}
