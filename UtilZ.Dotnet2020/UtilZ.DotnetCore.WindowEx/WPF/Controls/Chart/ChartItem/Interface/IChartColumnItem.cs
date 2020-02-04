﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public interface IChartColumnItem : IChartItem
    {
        LabelAxisItem LabelAxisID { get; }

        ColumnSeriesItem[] Values { get; }
    }


    public class ColumnSeriesItem : ChartItemAbs
    {
        public AxisAbs Axis { get; set; }

        public Style Style { get; set; }

        public string Title { get; set; }

        public ColumnSeriesItem()
            : base()
        {

        }

        public ColumnSeriesItem(double value, string tooltipText)
            : base(tooltipText, tooltipText)
        {

        }

        public ColumnSeriesItem(string tooltipText, object tag)
            : base(tooltipText, tag)
        {

        }

        public override object GetXValue()
        {
            throw new NotImplementedException();
        }

        public override object GetYValue()
        {
            throw new NotImplementedException();
        }
    }
}
