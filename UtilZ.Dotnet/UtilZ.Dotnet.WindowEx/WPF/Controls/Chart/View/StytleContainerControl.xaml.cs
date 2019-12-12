using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    /// <summary>
    /// Stytle.xaml 的交互逻辑
    /// </summary>
    public partial class StytleContainerControl : UserControl
    {
        private readonly Style _scrollViewStytle;
        private readonly double _scrollBarSize;

        public StytleContainerControl()
        {
            InitializeComponent();

            this._scrollViewStytle = (Style)this.Resources["scrollViewStyle"];
            this._scrollBarSize = (double)this.Resources["scrollBarSize"];
        }


        public Style ScrollViewerStyle
        {
            get { return _scrollViewStytle; }
        }

        public double ScrollBarSize
        {
            get { return _scrollBarSize; }
        }
    }
}
