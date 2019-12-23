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
using UtilZ.Dotnet.WindowEx.WPF.Controls;

namespace WpfApp1
{
    /// <summary>
    /// TestCabinetControl.xaml 的交互逻辑
    /// </summary>
    public partial class TestCabinetControl : Window
    {
        public TestCabinetControl()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new List<CabinetInfoGroup>();

            var group1 = new CabinetInfoGroup();
            group1.Name = "第一排机柜";

            int locationIndex;
            Random rnd = new Random();
            const int MAX_HEIGHT = 5;
            const int MIN_HEIGHT = 1;
            int height;

            for (int i = 0; i < 3; i++)
            {
                var cabinetInfo1 = new CabinetInfo();
                cabinetInfo1.Name = "外部设备";
                cabinetInfo1.Height = 42;

                locationIndex = 1;
                while (locationIndex < cabinetInfo1.Height)
                {
                    height = rnd.Next(MIN_HEIGHT, MAX_HEIGHT);
                    if (locationIndex + height > cabinetInfo1.Height)
                    {
                        break;
                    }

                    cabinetInfo1.CabinetDeviceUnitList.Add(new CabinetDeviceUnit() { BeginLocation = locationIndex, Height = height, DeviceList = new List<CabinetDevice>() { new CabinetDevice() { DeviceName = $"PDU{locationIndex}", DeviceBackground = Brushes.Aquamarine } } });
                    locationIndex = locationIndex + height + rnd.Next(MIN_HEIGHT, MAX_HEIGHT - 2);
                }

                group1.Group.Add(cabinetInfo1);
            }

            list.Add(group1);


            ((UCCabinetControl)zoomTranslateContainerControl.Child).CabinetInfoGroupList = list;
        }
    }
}
