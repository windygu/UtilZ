using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class ChartColumnItem : ChartItemAbs, IChartColumnItem
    {
        public LabelAxisItem LabelAxisID { get; set; }

        public override double Value
        {
            get { throw new ApplicationException(); }
            set { throw new ApplicationException(); }
        }

        public ColumnSeriesItem[] Values { get; set; }

        public ChartColumnItem()
        {

        }
    }
}
