using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public interface IChartAxisValue : IChartValue
    {
        object GetXValue();
        object GetYValue();
    }
}
