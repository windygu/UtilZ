using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.SHPBase.Common
{
    public class ColorHelper
    {
        private static readonly ReadOnlyCollection<Color> _colors;
        private static readonly Random _rnd = new Random();


        public static ReadOnlyCollection<Color> Colors
        {
            get { return _colors; }
        }

        static ColorHelper()
        {
            var colors = new List<Color>();
            var fields = typeof(Color).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty);
            foreach (var field in fields)
            {
                colors.Add((Color)field.GetValue(null, null));
            }

            _colors = new ReadOnlyCollection<Color>(colors);
        }

        public static Color GetRandColor()
        {
            return _colors[_rnd.Next(0, _colors.Count)];
        }
    }
}
