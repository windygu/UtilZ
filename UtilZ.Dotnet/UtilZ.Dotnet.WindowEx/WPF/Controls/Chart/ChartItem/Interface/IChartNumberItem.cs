using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public interface IChartNumberItem : IChartItem
    {
        double AxisValue { get; }
    }
}
