﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls
{
    /// <summary>
    /// UCCabinetU.xaml 的交互逻辑
    /// </summary>
    public partial class UCCabinetDeviceUControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCCabinetDeviceUControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 更新机柜设备
        /// </summary>
        /// <param name="deviceUnit"></param>
        /// <param name="deviceNameStyle"></param>
        public void UpdateCabinetDevice(CabinetDeviceUnit deviceUnit, Style deviceNameStyle)
        {
            this.Height = deviceUnit.Height * CabinetConstant.SINGLE_U_HEIGHT;
            this.SetCabinetUnit(deviceUnit);
            this.SetCabinetDevice(deviceUnit, deviceNameStyle);
        }

        private void SetCabinetDevice(CabinetDeviceUnit deviceUnit, Style deviceNameStyle)
        {
            stackPanel.Children.Clear();
            double cabinetDeviceUnitHeight = this.Height / deviceUnit.DeviceList.Count;

            foreach (var device in deviceUnit.DeviceList)
            {
                var cabinetDeviceUnitControl = new UCCabinetDeviceControl();
                cabinetDeviceUnitControl.DeviceNameStyle = deviceNameStyle;
                cabinetDeviceUnitControl.DataContext = device;
                cabinetDeviceUnitControl.Height = cabinetDeviceUnitHeight;
                stackPanel.Children.Insert(0, cabinetDeviceUnitControl);
            }
        }

        private void SetCabinetUnit(CabinetDeviceUnit deviceUnit)
        {
            grid.RowDefinitions.Clear();
            double rowHeight = this.Height / deviceUnit.Height;
            int index = deviceUnit.BeginLocation + deviceUnit.Height - 1;

            for (int i = 0; i < deviceUnit.Height; i++)
            {
                var rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(rowHeight, GridUnitType.Pixel);
                grid.RowDefinitions.Add(rowDefinition);

                var cabinetUnitControl = new UCCabinetCalibrationControl();
                cabinetUnitControl.DataContext = new CabinetUnit(index);
                grid.Children.Add(cabinetUnitControl);
                Grid.SetRow(cabinetUnitControl, i);
                index--;
            }
        }


        public void UpdateDeviceNameStyle(Style style)
        {
            if (stackPanel.Children.Count == 0)
            {
                return;
            }

            foreach (var ele in stackPanel.Children)
            {
                if (ele is UCCabinetDeviceControl)
                {
                    ((UCCabinetDeviceControl)ele).DeviceNameStyle = style;
                }
            }
        }
    }
}
