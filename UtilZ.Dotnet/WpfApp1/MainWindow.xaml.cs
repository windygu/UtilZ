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
using UtilZ.Dotnet.Ex.Log;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private int _index = 1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            logControl.AddLog(string.Format("{0}_{1} Debug", DateTime.Now, _index++), LogLevel.Debug);
            logControl.AddLog(string.Format("{0}_{1} Error", DateTime.Now, _index++), LogLevel.Error);
            logControl.AddLog(string.Format("{0}_{1} Faltal", DateTime.Now, _index++), LogLevel.Fatal);
            logControl.AddLog(string.Format("{0}_{1} Info", DateTime.Now, _index++), LogLevel.Info);
            logControl.AddLog(string.Format("{0}_{1} Warn", DateTime.Now, _index++), LogLevel.Warn);
        }
    }
}
