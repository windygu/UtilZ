using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class StepLineSeries : LineSeriesBase
    {
        public StepLineSeries()
            : base()
        {

        }

        protected override Geometry CreatePathGeometry(List<List<PointInfo>> pointInfoListCollection)
        {
            PathFigureCollection pfc = new PathFigureCollection();

            foreach (var pointInfoList in pointInfoListCollection)
            {
                if (pointInfoList == null || pointInfoList.Count < 2)
                {
                    continue;
                }

                var polyLineSegment = new PolyLineSegment();
                polyLineSegment.Points = new PointCollection();

                PointInfo lastPointInfo = pointInfoList[0], current;
                polyLineSegment.Points.Add(lastPointInfo.Point);

                int lastIndex = pointInfoList.Count - 1;
                for (int i = 1; i < pointInfoList.Count; i++)
                {
                    current = pointInfoList[i];
                    if (Math.Abs(current.Point.Y - lastPointInfo.Point.Y) > base.AxisY.PRE)
                    {
                        //高度发生变化,线拐弯
                        polyLineSegment.Points.Add(new Point(current.Point.X, lastPointInfo.Point.Y));
                        if (i == lastIndex)
                        {
                            break;
                        }
                    }

                    polyLineSegment.Points.Add(current.Point);
                    lastPointInfo = current;
                }

                var pathFigure = new PathFigure();
                pathFigure.StartPoint = polyLineSegment.Points[0];
                pathFigure.Segments.Add(polyLineSegment);
                pfc.Add(pathFigure);
            }

            return new PathGeometry(pfc);
        }
    }
}
