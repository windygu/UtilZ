using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public class ChartDateTimeItem : ChartItemAbs, IChartDateTimeItem
    {
        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public ChartDateTimeItem(DateTime time, double value, string tooltipText)
            : base(value, tooltipText)
        {
            this._time = time;

        }

        public override string ToString()
        {
            return $"{_time} {Value}";
        }
    }
}
