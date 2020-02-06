using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    internal interface IColumnSeries : ISeries
    {
        SeriesOrientation Orientation { get; set; }

        double Size { get; set; }

        Style GetStyle();
    }
}
