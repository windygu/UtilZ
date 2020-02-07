﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class PointInfo
    {
        public Point Point { get; private set; }

        public IChartValue Item { get; private set; }

        public PointInfo(Point point, IChartValue item)
        {
            this.Point = point;
            this.Item = item;
        }

        public override string ToString()
        {
            return $"{this.Point.X},{this.Point.Y}";
        }
    }
}
