using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public abstract class LineSeriesBase : SeriesAbs
    {
        private readonly Path _pathLine = new Path();
        private readonly List<PointInfo> _pointInfoList = new List<PointInfo>();
        private readonly List<FrameworkElement> _pointGeometryList = new List<FrameworkElement>();

        public LineSeriesBase()
        {
            var style = base.Style;
            if (style == null)
            {
                style = ChartStyleHelper.CreateLineDefaultStyle();
            }
            this._pathLine.Style = style;
            this.EnableTooltipChanged(base.EnableTooltip);
            this.VisibilityChanged(Visibility.Visible, base.Visibility);
        }

        protected override void StyleChanged(Style style)
        {
            this._pathLine.Style = style;
        }

        #region Tooltip
        protected override void EnableTooltipChanged(bool enableTooltip)
        {
            if (enableTooltip)
            {
                this._pathLine.MouseEnter += PathLine_MouseEnter;
                this._pathLine.MouseMove += PathLine_MouseMove;
                this._pathLine.MouseLeave += PathLine_MouseLeave;
            }
            else
            {
                this._pathLine.MouseEnter -= PathLine_MouseEnter;
                this._pathLine.MouseMove -= PathLine_MouseMove;
                this._pathLine.MouseLeave -= PathLine_MouseLeave;
            }
        }

        private void PathLine_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var path = (Path)sender;
            path.ToolTip = null;
        }

        private void PathLine_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.SetTooltip(sender, e);
        }

        private void PathLine_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.SetTooltip(sender, e);
        }

        private void SetTooltip(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var path = (Path)sender;
            if (this._pointInfoList == null || this._pointInfoList.Count == 0)
            {
                path.ToolTip = null;
                return;
            }

            var point = e.GetPosition((FrameworkElement)path.Parent);
            const double PRE = 5d;

            List<Tuple<double, PointInfo>> list = null;
            double x, y, distance;
            foreach (var pointInfo in this._pointInfoList)
            {
                x = point.X - pointInfo.Point.X;
                y = point.Y - pointInfo.Point.Y;
                if (Math.Abs(x) < PRE && Math.Abs(y) < PRE)
                {
                    distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                    if (list == null)
                    {
                        list = new List<Tuple<double, PointInfo>>();
                    }

                    list.Add(new Tuple<double, PointInfo>(distance, pointInfo));
                }
            }

            if (list == null)
            {
                path.ToolTip = null;
                return;
            }

            var result = list.OrderBy(t => { return t.Item1; }).First();
            if (string.IsNullOrWhiteSpace(result.Item2.Item.TooltipText))
            {
                path.ToolTip = null;
                return;
            }

            path.ToolTip = result.Item2.Item.TooltipText;
        }
        #endregion



        protected override void PrimitiveGetAxisValueArea(AxisAbs axis, out double min, out double max)
        {
            AxisHelper.GetAxisMinAndMax(axis, this.Values, out min, out max);
        }


        protected override void PrimitiveAdd(Canvas canvas, Rect chartArea)
        {
            this._pointInfoList.Clear();
            this._pointGeometryList.Clear();

            List<List<PointInfo>> pointInfoListCollection = this.GeneratePointList();
            if (pointInfoListCollection == null || pointInfoListCollection.Count == 0)
            {
                return;
            }

            this._pathLine.Data = this.CreatePathGeometry(pointInfoListCollection);
            if (!canvas.Children.Contains(this._pathLine))
            {
                canvas.Children.Add(this._pathLine);
                Canvas.SetLeft(this._pathLine, chartArea.X);
                Canvas.SetTop(this._pathLine, chartArea.Y);
            }

            this.DrawPointGeometry(canvas, chartArea.X, chartArea.Y, this._pointInfoList);

            foreach (var pointInfoList in pointInfoListCollection)
            {
                this._pointInfoList.AddRange(pointInfoList);
            }
        }

        private void DrawPointGeometry(Canvas canvas, double left, double top, List<PointInfo> pointInfoList)
        {
            var createPointFunc = base.CreatePointFunc;
            if (createPointFunc == null)
            {
                return;
            }

            foreach (var pointInfo in pointInfoList)
            {
                var pointGeometry = createPointFunc(pointInfo);
                if (pointGeometry == null)
                {
                    continue;
                }

                if (pointGeometry.ToolTip == null)
                {
                    pointGeometry.ToolTip = pointInfo.Item.TooltipText;
                }

                var leftOffset = pointGeometry.Width / 2;
                var topOffset = pointGeometry.Height / 2;
                canvas.Children.Add(pointGeometry);
                Canvas.SetLeft(pointGeometry, pointInfo.Point.X - leftOffset + left);
                Canvas.SetTop(pointGeometry, pointInfo.Point.Y - topOffset + top);

                this._pointGeometryList.Add(pointGeometry);
            }
        }

        protected abstract Geometry CreatePathGeometry(List<List<PointInfo>> pointInfoListCollection);


        private List<List<PointInfo>> GeneratePointList()
        {
            if (base._values == null || base._values.Count == 0)
            {
                return null;
            }

            List<List<PointInfo>> pointInfoListCollection = null;
            List<PointInfo> pointList = null;
            double x, y;

            foreach (var item in base._values)
            {
                x = this.AxisX.GetX(item);
                if (!AxisHelper.DoubleHasValue(x))
                {
                    pointList = null;
                    continue;
                }

                y = this.AxisY.GetY(item);
                if (!AxisHelper.DoubleHasValue(y))
                {
                    pointList = null;
                    continue;
                }

                if (pointList == null)
                {
                    pointList = new List<PointInfo>();
                    if (pointInfoListCollection == null)
                    {
                        pointInfoListCollection = new List<List<PointInfo>>();
                    }

                    pointInfoListCollection.Add(pointList);
                }

                pointList.Add(new PointInfo(new Point(x, y), item));
            }

            return pointInfoListCollection;
        }




        protected override void PrimitiveRemove(Canvas canvas)
        {
            this._pointInfoList.Clear();
            canvas.Children.Remove(this._pathLine);
            this._pathLine.Data = null;
            this._pathLine.Style = null;
            this._pathLine.MouseEnter -= PathLine_MouseEnter;
            this._pathLine.MouseMove -= PathLine_MouseMove;
            this._pathLine.MouseLeave -= PathLine_MouseLeave;

            foreach (var pointGeometry in this._pointGeometryList)
            {
                canvas.Children.Remove(pointGeometry);
            }
            this._pointGeometryList.Clear();
        }



        protected override void PrimitiveFillLegendItemToList(List<SeriesLegendItem> legendBrushList)
        {
            legendBrushList.Add(new SeriesLegendItem(this._pathLine.Stroke, base.Title, this));
        }


        protected override void VisibilityChanged(Visibility oldVisibility, Visibility newVisibility)
        {
            this._pathLine.Visibility = newVisibility;
            foreach (var pointGeometry in this._pointGeometryList)
            {
                pointGeometry.Visibility = newVisibility;
            }
        }
    }
}
