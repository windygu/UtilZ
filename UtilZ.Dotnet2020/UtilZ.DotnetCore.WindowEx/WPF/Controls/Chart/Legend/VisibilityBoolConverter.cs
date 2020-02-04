using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using UtilZ.DotnetCore.WindowEx.WPF.Base;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    public class VisibilityBoolConverter : ValueConverterAbs
    {
        public VisibilityBoolConverter()
        {

        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            var visibility = (Visibility)value;
            if (visibility == Visibility.Visible)
            {
                return true;
            }

            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            if ((bool)value)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }
    }
}
