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





        public static void DrawXAxisLabelLine(AxisAbs axis, Canvas canvas, List<Point> axisPointList)
        {
            if (!axis.AxisLine)
            {
                return;
            }

            GeometryGroup geometryGroup = new GeometryGroup();
            Point point1, point2;

            foreach (var axisPoint in axisPointList)
            {
                switch (axis.DockOrientation)
                {
                    case ChartDockOrientation.Top:
                        point1 = new Point(axisPoint.X, axisPoint.Y - axis.LabelSize);
                        point2 = new Point(axisPoint.X, axisPoint.Y);
                        break;
                    case ChartDockOrientation.Bottom:
                        point1 = new Point(axisPoint.X, axisPoint.Y);
                        point2 = new Point(axisPoint.X, axisPoint.Y + axis.LabelSize);
                        break;
                    case ChartDockOrientation.Left:
                    case ChartDockOrientation.Right:
                        break;
                    default:
                        throw new NotImplementedException();
                }

                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = point1;
                LineSegment lineSegment = new LineSegment(point2, true);
                pathFigure.Segments.Add(lineSegment);

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures = new PathFigureCollection(new PathFigure[] { pathFigure });
                geometryGroup.Children.Add(pathGeometry);
            }

            var separatorPath = new Path();
            separatorPath.Data = geometryGroup;
            separatorPath.Style = axis.AxisLineStyle;
            if (separatorPath.Style == null)
            {
                ChartStyleHelper.CreateAxisSeparatorStyle();
            }
            canvas.Children.Add(separatorPath);
        }

        public static void DrawYAxisLabelLine(AxisAbs axis, Canvas canvas, List<Point> axisPointList)
        {
            if (!axis.AxisLine)
            {
                return;
            }

            GeometryGroup geometryGroup = new GeometryGroup();
            Point point1, point2;

            foreach (var axisPoint in axisPointList)
            {
                switch (axis.DockOrientation)
                {
                    case ChartDockOrientation.Left:
                        point1 = new Point(axisPoint.X - axis.LabelSize, axisPoint.Y);
                        point2 = new Point(axisPoint.X, axisPoint.Y);
                        break;
                    case ChartDockOrientation.Right:
                        point1 = new Point(axisPoint.X, axisPoint.Y);
                        point2 = new Point(axisPoint.X + axis.LabelSize, axisPoint.Y);
                        break;
                    case ChartDockOrientation.Top:
                    case ChartDockOrientation.Bottom:
                    default:
                        throw new NotImplementedException();
                }

                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = point1;
                LineSegment lineSegment = new LineSegment(point2, true);
                pathFigure.Segments.Add(lineSegment);

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures = new PathFigureCollection(new PathFigure[] { pathFigure });
                geometryGroup.Children.Add(pathGeometry);
            }

            var separatorPath = new Path();
            separatorPath.Data = geometryGroup;
            separatorPath.Style = axis.AxisLineStyle;
            if (separatorPath.Style == null)
            {
                ChartStyleHelper.CreateAxisSeparatorStyle();
            }
            canvas.Children.Add(separatorPath);
        }




        public static int CalSeparatorCount(double area, double axisSize, double labelStep)
        {
            int separatorCount;
            separatorCount = (int)(area / labelStep);
            if (area % labelStep > 0)
            {
                separatorCount += 1;
            }

            return separatorCount;
        }

        public static double CalSeparatorSize(double area, double axisSize, double labelStep)
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
    }
}
