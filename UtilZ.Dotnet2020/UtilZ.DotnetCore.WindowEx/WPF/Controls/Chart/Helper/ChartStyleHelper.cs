using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class ChartStyleHelper
    {
        public static Style CreateAxisSeparatorStyle()
        {
            var style = new Style();
            style.TargetType = typeof(System.Windows.Shapes.Path);
            style.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4F4F4"))));
            //style.Setters.Add(new Setter(Path.StrokeProperty, Brushes.Gray));
            style.Setters.Add(new Setter(Path.StrokeThicknessProperty, 1.0d));
            return style;
        }

        private static Dictionary<ChartDockOrientation, Style> _axisLabelDefaultStyleDic = new System.Collections.Generic.Dictionary<ChartDockOrientation, Style>();
        public static Style GetAxisLabelStyle(ChartDockOrientation axisDockOrientation)
        {
            Style defaultLabelStyle;
            if (_axisLabelDefaultStyleDic.TryGetValue(axisDockOrientation, out defaultLabelStyle))
            {
                return defaultLabelStyle;
            }

            defaultLabelStyle = new Style();
            defaultLabelStyle.TargetType = typeof(TextBlock);
            //defaultLabelStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Gray));
            defaultLabelStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White));
            defaultLabelStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 12d));

            const double MARGIN = 5d;
            Thickness margin;
            switch (axisDockOrientation)
            {
                case ChartDockOrientation.Left:
                    margin = new Thickness(0d, 0d, MARGIN, 0d);
                    break;
                case ChartDockOrientation.Right:
                    margin = new Thickness(MARGIN, 0d, 0d, 0d);
                    break;
                case ChartDockOrientation.Top:
                    margin = new Thickness(0d, 0d, 0d, MARGIN);
                    break;
                case ChartDockOrientation.Bottom:
                    margin = new Thickness(0d, MARGIN, 0d, 0d);
                    break;
                default:
                    throw new NotImplementedException();
            }
            defaultLabelStyle.Setters.Add(new Setter(TextBlock.MarginProperty, margin));
            _axisLabelDefaultStyleDic.Add(axisDockOrientation, defaultLabelStyle);

            return defaultLabelStyle;
        }

        public static Style CreateAxisTitleStyle(ChartDockOrientation axisDockOrientation)
        {
            var defaultTitleStyle = new Style();
            defaultTitleStyle.TargetType = typeof(TextBlock);
            defaultTitleStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Gray));
            defaultTitleStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 14d));

            const double MARGIN = 10d;
            Thickness margin;
            switch (axisDockOrientation)
            {
                case ChartDockOrientation.Left:
                    margin = new Thickness(0d - MARGIN, 0d, 0d, 0d);
                    break;
                case ChartDockOrientation.Right:
                    margin = new Thickness(MARGIN, 0d, 0d, 0d);
                    break;
                case ChartDockOrientation.Top:
                    margin = new Thickness(0d, 0d - MARGIN, 0d, 0d);
                    break;
                case ChartDockOrientation.Bottom:
                    margin = new Thickness(0d, MARGIN, 0d, 0d);
                    break;
                default:
                    throw new NotImplementedException();
            }
            defaultTitleStyle.Setters.Add(new Setter(TextBlock.MarginProperty, margin));

            return defaultTitleStyle;
        }

        public static Style CreateLineStyle()
        {
            var style = new Style();
            style.TargetType = typeof(Path);
            style.Setters.Add(new Setter(Path.StrokeProperty, Brushes.Gray));
            style.Setters.Add(new Setter(Path.StrokeThicknessProperty, 2d));
            //style.Setters.Add(new Setter(Path.FillProperty, Brushes.White));

            var trigger = new Trigger();
            trigger.Property = Path.IsMouseOverProperty;
            trigger.Value = true;
            trigger.Setters.Add(new Setter(Path.StrokeThicknessProperty, 3d));
            style.Triggers.Add(trigger);
            return style;
        }


        //public static Style CreateStepLineStyle()
        //{
        //    var style = new Style();
        //    style.TargetType = typeof(Polyline);
        //    style.Setters.Add(new Setter(Polyline.StrokeProperty, Brushes.Gray));
        //    style.Setters.Add(new Setter(Polyline.StrokeThicknessProperty, 2d));
        //    //style.Setters.Add(new Setter(Path.FillProperty, Brushes.White));

        //    var trigger = new Trigger();
        //    trigger.Property = Polyline.IsMouseOverProperty;
        //    trigger.Value = true;
        //    trigger.Setters.Add(new Setter(Polyline.StrokeThicknessProperty, 3d));
        //    style.Triggers.Add(trigger);
        //    return style;
        //}
    }
}
