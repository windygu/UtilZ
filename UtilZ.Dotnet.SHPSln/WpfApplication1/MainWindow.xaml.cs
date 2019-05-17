using CefSharp.Wpf;
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

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ChromiumWebBrowser _browser;
        public MainWindow()
        {
            InitializeComponent();

            this._browser = new ChromiumWebBrowser();
            grid.Children.Add(this._browser);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = @"file:///D:\Projects\Self\ServiceHostedPlatform\trunk\Code\UtilZ.Dotnet.SHPSln\WindowsFormsApplication1\HTMLPage1.html";
            //Browser.NavigateTo(@"file:///E:/Soft/Ftp/cef/Cef4CSharp/HYHC.Cef4CSharp/Build/BindingTest.html");
            //Browser.NavigateTo(url);
            //this._browser.Load(url);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
           
        }
    }
}
