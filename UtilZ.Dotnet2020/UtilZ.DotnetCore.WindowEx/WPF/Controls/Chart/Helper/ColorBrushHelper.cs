﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls.Chart
{
    public class ColorBrushHelper
    {
        /// <summary>
        /// Gets or sets the application level default series color list
        /// </summary>
        private static List<Brush> _colorBrushList;
        //private static Random _colorBrushRandom;


        static ColorBrushHelper()
        {
            //_colorBrushRandom = new Random();
            _colorBrushList = new List<Brush>
            {
              new SolidColorBrush(Color.FromRgb(33, 149, 242)),
               new SolidColorBrush(Color.FromRgb(243, 67, 54)),
               new SolidColorBrush(Color.FromRgb(254, 192, 7)),
               new SolidColorBrush(Color.FromRgb(96, 125, 138)),
               new SolidColorBrush(Color.FromRgb(0, 187, 211)),
               new SolidColorBrush(Color.FromRgb(254, 87, 34)),
               new SolidColorBrush(Color.FromRgb(63, 81, 180)),
               new SolidColorBrush(Color.FromRgb(204, 219, 57)),
               new SolidColorBrush(Color.FromRgb(0, 149, 135)),
               new SolidColorBrush(Color.FromRgb(232, 30, 99)),
               new SolidColorBrush(Color.FromRgb(76, 174, 80))
            };
        }

        private static int _colorBrushIndex = 0;
        public static Brush GetNextColor()
        {
            //var r = _colorBrushRandom.Next(0, _colorBrushList.Count);
            //return _colorBrushList[(r) % _colorBrushList.Count];

            if (_colorBrushIndex >= _colorBrushList.Count)
            {
                _colorBrushIndex = 0;
            }

            return _colorBrushList[_colorBrushIndex++];
        }

        public static Brush GetColorByIndex(ref int index)
        {
            if (index >= _colorBrushList.Count)
            {
                index = 0;
            }

            return _colorBrushList[index];
        }
    }
}
