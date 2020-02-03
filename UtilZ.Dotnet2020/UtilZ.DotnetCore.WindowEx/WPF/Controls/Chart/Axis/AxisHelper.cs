using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UtilZ.DotnetCore.WindowEx.Base;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class AxisHelper
    {
        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double DoubleToCeilingInteger(double value, long? muilt = null)
        {
            long m;
            if (muilt == null)
            {
                m = CalDoubleToIntegerMuilt(value);
            }
            else
            {
                m = muilt.Value;
            }

            return Math.Ceiling(value / m) * m;
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double DoubleToFloorInteger(double value, long? muilt = null)
        {
            long m;
            if (muilt == null)
            {
                m = CalDoubleToIntegerMuilt(value);
            }
            else
            {
                m = muilt.Value;
            }

            return Math.Floor(value / m) * m;
        }

        public static long CalDoubleToIntegerMuilt(double value)
        {
            int length = ((long)(Math.Abs(value) / 10)).ToString().Length;
            return (long)Math.Pow(10, length);
        }



        public static bool DoubleHasValue(double value)
        {
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                return false;
            }

            return true;
        }





        public static void DrawXAxisLabelLine(AxisAbs axis, Canvas canvas, List<double> xList)
        {
            if (!axis.AxisLine)
            {
                return;
            }

            GeometryGroup geometryGroup = new GeometryGroup();
            Point point1, point2;

            foreach (var x in xList)
            {
                if (axis.IsAxisXBottom())
                {
                    point1 = new Point(x, AxisConstant.ZERO_D);
                    point2 = new Point(x, axis.LabelSize);
                }
                else
                {
                    point1 = new Point(x, canvas.Height - axis.LabelSize);
                    point2 = new Point(x, canvas.Height);
                }

                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = point1;
                LineSegment lineSegment = new LineSegment(point2, true);
                pathFigure.Segments.Add(lineSegment);

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures = new PathFigureCollection(new PathFigure[] { pathFigure });
                geometryGroup.Children.Add(pathGeometry);
            }

            var labelLinePath = new Path();
            labelLinePath.Data = geometryGroup;
            labelLinePath.Style = axis.AxisLineStyle;
            if (labelLinePath.Style == null)
            {
                labelLinePath.Style = ChartStyleHelper.CreateAxisLabelLineStyle();
            }
            canvas.Children.Add(labelLinePath);
        }

        public static void DrawYAxisLabelLine(AxisAbs axis, Canvas canvas, List<double> yList)
        {
            if (!axis.AxisLine)
            {
                return;
            }

            GeometryGroup geometryGroup = new GeometryGroup();
            Point point1, point2;

            foreach (var y in yList)
            {
                if (axis.IsAxisYLeft())
                {
                    point1 = new Point(canvas.Width - axis.LabelSize, y);
                    point2 = new Point(canvas.Width, y);
                }
                else
                {
                    point1 = new Point(AxisConstant.ZERO_D, y);
                    point2 = new Point(axis.LabelSize, y);
                }

                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = point1;
                LineSegment lineSegment = new LineSegment(point2, true);
                pathFigure.Segments.Add(lineSegment);

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures = new PathFigureCollection(new PathFigure[] { pathFigure });
                geometryGroup.Children.Add(pathGeometry);
            }

            var labelLinePath = new Path();
            labelLinePath.Data = geometryGroup;
            labelLinePath.Style = axis.AxisLineStyle;
            if (labelLinePath.Style == null)
            {
                labelLinePath.Style = ChartStyleHelper.CreateAxisLabelLineStyle();
            }
            canvas.Children.Add(labelLinePath);
        }




        public static int CalculateLabelCount(double area, double axisSize, double labelStep)
        {
            int separatorCount;
            separatorCount = (int)(area / labelStep);
            if (area % labelStep > 0)
            {
                separatorCount += 1;
            }

            return separatorCount;
        }

        public static double CalculateLabelStepSize(double area, double axisSize, double labelStep)
        {
            return labelStep * axisSize / area;
        }



        public static TextBlock CreateLabelControl(AxisAbs axis, string label)
        {
            var textBlock = new TextBlock();
            textBlock.Text = label;
            if (axis.LabelStyle == null)
            {
                textBlock.Style = ChartStyleHelper.GetAxisLabelStyle(axis.DockOrientation);
            }
            else
            {
                textBlock.Style = axis.LabelStyle;
            }

            return textBlock;
        }





        private static TextBlock _measureTextLabel = null;
        public static Rect MeasureScaleTextSize(AxisAbs axis, string labelText)
        {
            if (_measureTextLabel == null)
            {
                _measureTextLabel = new TextBlock();
            }

            TextBlock measureTextLabel = _measureTextLabel;
            measureTextLabel.Text = labelText;
            if (axis.LabelStyle == null)
            {
                measureTextLabel.Style = ChartStyleHelper.GetAxisLabelStyle(axis.DockOrientation);
            }
            else
            {
                measureTextLabel.Style = axis.LabelStyle;
            }

            return UITextHelper.MeasureTextSize(measureTextLabel);
        }









        public static void GetAxisMinAndMax(AxisAbs axis, ChartCollection<IChartItem> values, out double min, out double max)
        {
            min = double.NaN;
            max = double.NaN;

            if (values == null || values.Count == 0)
            {
                return;
            }

            double pre = double.IsNaN(axis.PRE) ? 0d : axis.PRE;
            foreach (var value in values)
            {
                if (double.IsNaN(value.Value))
                {
                    continue;
                }

                if (double.IsNaN(min) || value.Value - min < pre)
                {
                    min = value.Value;
                }

                if (double.IsNaN(max) || value.Value - max > pre)
                {
                    max = value.Value;
                }
            }
        }
    }
}
