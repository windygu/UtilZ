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
using System.Windows.Shapes;

namespace WpfApp1.Properties
{
    /// <summary>
    /// TestChartWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestChartWindow : Window
    {
        public TestChartWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private DateTimeOffset AbsoluteExpiration;
        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //var ticks1 = DateTimeOffset.Now.Ticks;
            //var ticks2 = DateTimeOffset.Now.Ticks;
            //var ticks3 = DateTimeOffset.Now.Ticks;
            //var ticks4 = DateTimeOffset.Now.Ticks;
            //AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow, TimeSpan.FromSeconds(1000));

            DateTimeOffset dd = new DateTimeOffset(1970, 1, 1, 0, 0, 0,TimeSpan.FromTicks(0L));
            //TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            var ticks1 = (DateTimeOffset.Now - dd).Ticks;
            var ticks2 = (DateTimeOffset.Now - dd).Ticks;
            var ticks3 = (DateTimeOffset.Now - dd).Ticks;
        }
    }
}
