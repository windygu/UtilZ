using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public abstract class LineSeriesBase : SeriesAbs
    {
        private readonly Path _pathLine = new Path();
        private readonly List<PointInfo> _pointInfoList = new List<PointInfo>();
        private readonly List<FrameworkElement> _pointGeometryList = new List<FrameworkElement>();

        public LineSeriesBase()
            : base()
        {
            var style = base.Style;
            if (style == null)
            {
                style = ChartStyleHelper.GetLineSeriesDefaultStyle();
            }
            this.StyleChanged(style);
            this.EnableTooltipChanged(base.EnableTooltip);
            this.VisibilityChanged(Visibility.Visible, base.Visibility);
        }

        protected override void StyleChanged(Style style)
        {
            this._pathLine.Style = style;
            base.AddOrReplaceLegendItem(new SeriesLegendItem(this._pathLine.Stroke, base.Title, this));
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
            //DotnetStd.Ex.Log.Loger.Info($"X:{point.X}        Y:{point.Y}");


            List<Tuple<double, PointInfo>> list = null;
            double x, y, distance;
            foreach (var pointInfo in this._pointInfoList)
            {
                x = point.X - pointInfo.Point.X;
                y = point.Y - pointInfo.Point.Y;
                if (Math.Abs(x) < base.TooltipArea && Math.Abs(y) < base.TooltipArea)
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





        protected override void PrimitiveAdd(Canvas canvas)
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
            }

            this.DrawPointGeometry(canvas, this._pointInfoList);
        }

        private void DrawPointGeometry(Canvas canvas, List<PointInfo> pointInfoList)
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
                Canvas.SetLeft(pointGeometry, pointInfo.Point.X - leftOffset);
                Canvas.SetTop(pointGeometry, pointInfo.Point.Y - topOffset);

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
                this._pointInfoList.Add(pointList.Last());
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
