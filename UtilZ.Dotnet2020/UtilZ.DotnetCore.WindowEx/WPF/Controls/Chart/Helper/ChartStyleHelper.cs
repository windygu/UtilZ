using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class ChartStyleHelper
    {
        private static Style _defaultAxisLabelLineStyle = null;
        public static Style GetDefaultAxisLabelLineStyle()
        {
            if (_defaultAxisLabelLineStyle == null)
            {
                var style = new Style();
                style.TargetType = typeof(System.Windows.Shapes.Path);
                //style.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4F4F4"))));
                style.Setters.Add(new Setter(Path.StrokeProperty, Brushes.White));
                style.Setters.Add(new Setter(Path.StrokeThicknessProperty, 1.0d));
                _defaultAxisLabelLineStyle = style;
            }

            return _defaultAxisLabelLineStyle;
        }


        private static Style _defaultBackgroundLabelLineStyle = null;
        public static Style GetDefaultBackgroundLabelLineStyle()
        {
            if (_defaultBackgroundLabelLineStyle == null)
            {
                var style = new Style();
                style.TargetType = typeof(System.Windows.Shapes.Path);
                style.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F4F4F4"))));
                //style.Setters.Add(new Setter(Path.StrokeProperty, Brushes.Gray));
                style.Setters.Add(new Setter(Path.StrokeThicknessProperty, 1.0d));
                _defaultBackgroundLabelLineStyle = style;
            }

            return _defaultBackgroundLabelLineStyle;
        }


        private static Dictionary<ChartDockOrientation, Style> _axisLabelDefaultStyleDic = null;
        public static Style GetAxisLabelStyle(ChartDockOrientation axisDockOrientation)
        {
            if (_axisLabelDefaultStyleDic == null)
            {
                _axisLabelDefaultStyleDic = new Dictionary<ChartDockOrientation, Style>();
            }

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


        private static Dictionary<ChartDockOrientation, Style> _axisTitleStyleDic = null;
        public static Style CreateAxisTitleStyle(ChartDockOrientation axisDockOrientation)
        {
            if (_axisTitleStyleDic == null)
            {
                _axisTitleStyleDic = new Dictionary<ChartDockOrientation, Style>();
            }

            Style defaultTitleStyle;
            if (_axisLabelDefaultStyleDic.TryGetValue(axisDockOrientation, out defaultTitleStyle))
            {
                return defaultTitleStyle;
            }

            defaultTitleStyle = new Style();
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
            _axisTitleStyleDic.Add(axisDockOrientation, defaultTitleStyle);

            return defaultTitleStyle;
        }


        private static Style _lineSeriesDefaultStyle = null;
        public static Style GetLineSeriesDefaultStyle()
        {
            if (_lineSeriesDefaultStyle == null)
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
                _lineSeriesDefaultStyle = style;
            }

            return _lineSeriesDefaultStyle;
        }


        public static Style CreateLineSeriesStyle(Brush stroke, double strokeThickness = 2d, double mouseOverStrokeThickness = 3d)
        {
            var style = new Style();
            style.TargetType = typeof(Path);
            style.Setters.Add(new Setter(Path.StrokeProperty, stroke));
            style.Setters.Add(new Setter(Path.StrokeThicknessProperty, strokeThickness));
            //style.Setters.Add(new Setter(Path.FillProperty, Brushes.White));

            if (AxisHelper.DoubleHasValue(mouseOverStrokeThickness) && mouseOverStrokeThickness > AxisConstant.ZERO_D)
            {
                var trigger = new Trigger();
                trigger.Property = Path.IsMouseOverProperty;
                trigger.Value = true;
                trigger.Setters.Add(new Setter(Path.StrokeThicknessProperty, mouseOverStrokeThickness));
                style.Triggers.Add(trigger);
            }

            return style;
        }


        public static Style CreateColumnSeriesStyle(Brush fill, Brush stroke, double strokeThickness, double mouseOverStrokeThickness = 1d)
        {
            var style = new Style();
            style.TargetType = typeof(Rectangle);
            style.Setters.Add(new Setter(Rectangle.FillProperty, fill));
            style.Setters.Add(new Setter(Rectangle.StrokeProperty, stroke));
            style.Setters.Add(new Setter(Rectangle.StrokeThicknessProperty, strokeThickness));

            if (AxisHelper.DoubleHasValue(mouseOverStrokeThickness) && mouseOverStrokeThickness > AxisConstant.ZERO_D)
            {
                var trigger = new Trigger();
                trigger.Property = Rectangle.IsMouseOverProperty;
                trigger.Value = true;
                trigger.Setters.Add(new Setter(Rectangle.StrokeThicknessProperty, mouseOverStrokeThickness));
                style.Triggers.Add(trigger);
            }

            return style;
        }


        public static Style CreateColumnSeriesStyle(Brush fill, double mouseOverOpacity = 0.8d)
        {
            var style = new Style();
            style.TargetType = typeof(Rectangle);
            style.Setters.Add(new Setter(Rectangle.FillProperty, fill));
            style.Setters.Add(new Setter(Rectangle.StrokeThicknessProperty, AxisConstant.ZERO_D));

            if (AxisHelper.DoubleHasValue(mouseOverOpacity) && mouseOverOpacity > AxisConstant.ZERO_D)
            {
                var trigger = new Trigger();
                trigger.Property = Rectangle.IsMouseOverProperty;
                trigger.Value = true;
                trigger.Setters.Add(new Setter(Rectangle.OpacityProperty, mouseOverOpacity));
                style.Triggers.Add(trigger);
            }

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
