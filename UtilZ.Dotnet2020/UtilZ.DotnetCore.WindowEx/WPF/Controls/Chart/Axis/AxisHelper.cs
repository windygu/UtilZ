﻿using System;
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
            if (!axis.AxisLine || xList == null || xList.Count == 0)
            {
                return;
            }

            var labelLinePath = new Path();
            labelLinePath.Style = axis.AxisLineStyle;
            if (labelLinePath.Style == null)
            {
                labelLinePath.Style = ChartStyleHelper.CreateAxisLabelLineStyle();
            }

            GeometryGroup geometryGroup = new GeometryGroup();
            Point point1, point2;
            double x;
            int lastIndex = xList.Count - 1;

            for (int i = 0; i < xList.Count; i++)
            {
                x = xList[i];
                //if (axis.Orientation == AxisOrientation.RightToLeft && i == 0)
                //{
                //    //刻度从右向左,所以最右侧的要向左偏移labelLinePath.StrokeThickness
                //    x = x - labelLinePath.StrokeThickness;
                //}
                //else if (axis.Orientation == AxisOrientation.LeftToRight && i == lastIndex)
                //{
                //    //刻度从左向右,所以最右侧的要向左偏移labelLinePath.StrokeThickness
                //    x = x - labelLinePath.StrokeThickness;
                //}

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

                PathFigure labelPathFigure = new PathFigure();
                labelPathFigure.StartPoint = point1;
                labelPathFigure.Segments.Add(new LineSegment(point2, true));
                geometryGroup.Children.Add(new PathGeometry()
                {
                    Figures = new PathFigureCollection(new PathFigure[] { labelPathFigure })
                });
            }


            //坐标轴
            if (axis.IsAxisXBottom())
            {
                point1 = new Point(xList.First(), AxisConstant.ZERO_D);
                point2 = new Point(xList.Last(), AxisConstant.ZERO_D);
            }
            else
            {
                point1 = new Point(xList.First(), canvas.Height);
                point2 = new Point(xList.Last(), canvas.Height);
            }
            PathFigure axisPathFigure = new PathFigure();
            axisPathFigure.StartPoint = point1;
            axisPathFigure.Segments.Add(new LineSegment(point2, true));
            geometryGroup.Children.Add(new PathGeometry()
            {
                Figures = new PathFigureCollection(new PathFigure[] { axisPathFigure })
            });


            labelLinePath.Data = geometryGroup;
            canvas.Children.Add(labelLinePath);
        }

        public static void DrawYAxisLabelLine(AxisAbs axis, Canvas canvas, List<double> yList)
        {
            if (!axis.AxisLine || yList == null || yList.Count == 0)
            {
                return;
            }

            var labelLinePath = new Path();
            labelLinePath.Style = axis.AxisLineStyle;
            if (labelLinePath.Style == null)
            {
                labelLinePath.Style = ChartStyleHelper.CreateAxisLabelLineStyle();
            }

            GeometryGroup geometryGroup = new GeometryGroup();
            Point point1, point2;
            double y;
            int lastIndex = yList.Count - 1;

            for (int i = 0; i < yList.Count; i++)
            {
                y = yList[i];
                //if (axis.Orientation == AxisOrientation.BottomToTop && i == 0)
                //{
                //    //刻度从下向上,所以最右侧的要向左偏移labelLinePath.StrokeThickness
                //    y = y - labelLinePath.StrokeThickness;
                //}
                //else if (axis.Orientation == AxisOrientation.TopToBottom && i == lastIndex)
                //{
                //    //刻度从上向下,所以最右侧的要向左偏移labelLinePath.StrokeThickness
                //    y = y - labelLinePath.StrokeThickness;
                //}

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

                PathFigure labelPathFigure = new PathFigure();
                labelPathFigure.StartPoint = point1;
                labelPathFigure.Segments.Add(new LineSegment(point2, true));
                geometryGroup.Children.Add(new PathGeometry()
                {
                    Figures = new PathFigureCollection(new PathFigure[] { labelPathFigure })
                });
            }


            //坐标轴
            if (axis.IsAxisYLeft())
            {
                point1 = new Point(canvas.Width, yList.First());
                point2 = new Point(canvas.Width, yList.Last());
            }
            else
            {
                point1 = new Point(AxisConstant.ZERO_D, yList.First());
                point2 = new Point(AxisConstant.ZERO_D, yList.Last());
            }
            PathFigure axisPathFigure = new PathFigure();
            axisPathFigure.StartPoint = point1;
            axisPathFigure.Segments.Add(new LineSegment(point2, true));
            geometryGroup.Children.Add(new PathGeometry()
            {
                Figures = new PathFigureCollection(new PathFigure[] { axisPathFigure })
            });


            labelLinePath.Data = geometryGroup;
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
        public static Rect MeasureLabelTextSize(AxisAbs axis, string labelText)
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

            double pre = double.IsNaN(axis.PRE) ? AxisConstant.ZERO_D : axis.PRE;
            object obj;
            double tmp;
            foreach (var value in values)
            {
                if (value == null)
                {
                    continue;
                }

                switch (axis.AxisType)
                {
                    case AxisType.X:
                        obj = value.GetXValue();
                        break;
                    case AxisType.Y:
                        obj = value.GetYValue();
                        break;
                    default:
                        throw new NotImplementedException(axis.AxisType.ToString());
                }

                tmp = ConvertToDouble(obj);
                if (!DoubleHasValue(tmp))
                {
                    continue;
                }

                if (double.IsNaN(min) || tmp - min < pre)
                {
                    min = tmp;
                }

                if (double.IsNaN(max) || tmp - max > pre)
                {
                    max = tmp;
                }
            }
        }



        public static double ConvertToDouble(object obj)
        {
            if (obj == null)
            {
                return double.NaN;
            }

            double value;
            if (obj is double)
            {
                value = (double)obj;
            }
            else
            {
                try
                {
                    value = Convert.ToDouble(obj);
                }
                catch
                {
                    value = double.NaN;
                }
            }

            return value;
        }


        public static DateTime? ConvertToDateTime(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            DateTime? value;
            if (obj is DateTime)
            {
                value = (DateTime)obj;
            }
            else
            {
                try
                {
                    value = Convert.ToDateTime(obj);
                }
                catch
                {
                    value = null;
                }
            }

            return value;
        }
    }
}
