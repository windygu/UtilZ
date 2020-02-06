using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class BackgroundLabelLineSegment
    {
        public Point Point1 { get; private set; }

        public Point Point2 { get; private set; }



        public BackgroundLabelLineSegment(Point point1, Point point2)
        {
            this.Point1 = point1;
            this.Point2 = point2;
        }
    }
}
