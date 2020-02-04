using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public interface IChartNumberItem : IChartItem
    {
        double AxisXValue { get; }
    }
}
