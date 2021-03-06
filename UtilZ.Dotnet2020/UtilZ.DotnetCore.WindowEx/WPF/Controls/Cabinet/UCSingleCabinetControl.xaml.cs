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

namespace UtilZ.DotnetCore.WindowEx.WPF.Controls
{
    /// <summary>
    /// UCCabinetControl.xaml 的交互逻辑
    /// </summary>
    public partial class UCSingleCabinetControl : UserControl
    {
        #region 依赖属性
        /// <summary>
        /// 烟雾值依赖属性
        /// </summary>
        public static readonly DependencyProperty CabinetInfoProperty =
            DependencyProperty.Register(nameof(CabinetInfo), typeof(CabinetInfo), typeof(UCSingleCabinetControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChangedCallback)));

        /// <summary>
        /// 设备名称样式依赖属性
        /// </summary>
        public static readonly DependencyProperty DeviceNameStyleProperty =
            DependencyProperty.Register(nameof(DeviceNameStyle), typeof(Style), typeof(UCSingleCabinetControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChangedCallback)));

        /// <summary>
        /// 机柜名称样式依赖属性
        /// </summary>
        public static readonly DependencyProperty CabinetNameStyleProperty =
            DependencyProperty.Register(nameof(CabinetNameStyle), typeof(Style), typeof(UCSingleCabinetControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChangedCallback)));


        /// <summary>
        /// 获取或设置机柜名称样式
        /// </summary>
        public Style CabinetNameStyle
        {
            get
            {
                return (Style)base.GetValue(CabinetNameStyleProperty);
            }
            set
            {
                base.SetValue(CabinetNameStyleProperty, value);
            }
        }

        /// <summary>
        /// 获取或设置设备名称样式
        /// </summary>
        public Style DeviceNameStyle
        {
            get
            {
                return (Style)base.GetValue(DeviceNameStyleProperty);
            }
            set
            {
                base.SetValue(DeviceNameStyleProperty, value);
            }
        }

        /// <summary>
        /// 获取或设置机柜设备列表
        /// </summary>
        public CabinetInfo CabinetInfo
        {
            get
            {
                return (CabinetInfo)base.GetValue(CabinetInfoProperty);
            }
            set
            {
                base.SetValue(CabinetInfoProperty, value);
            }
        }

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selfControl = (UCSingleCabinetControl)d;
            if (e.Property == CabinetInfoProperty)
            {
                selfControl.SetCabinetDeviceUnit((CabinetInfo)e.NewValue);
            }
            else if (e.Property == DeviceNameStyleProperty)
            {
                selfControl.SetDeviceNameStyle((Style)e.NewValue);
            }
            else if (e.Property == CabinetNameStyleProperty)
            {
                selfControl.SetCabinetNameStyle((Style)e.NewValue);
            }
        }
        #endregion

        private static readonly double _titleHeight;
        private static readonly double _bottomHeight;

        static UCSingleCabinetControl()
        {
            var control = new UCSingleCabinetControl();
            _titleHeight = ((GridLength)control.Resources["titleHeight"]).Value;
            _bottomHeight = ((GridLength)control.Resources["bottomHeight"]).Value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCSingleCabinetControl()
        {
            InitializeComponent();

            this.CabinetNameStyle = GetCabinetNameDefaultStyle();
        }

        private static Style _cabinetNameDefaultStyle = null;
        private static Style GetCabinetNameDefaultStyle()
        {
            if (_cabinetNameDefaultStyle == null)
            {
                var cabinetNameDefaultStyle = new Style();
                cabinetNameDefaultStyle.TargetType = typeof(TextBlock);
                cabinetNameDefaultStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.White));
                cabinetNameDefaultStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 14d));
                cabinetNameDefaultStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
                cabinetNameDefaultStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));
                cabinetNameDefaultStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
                _cabinetNameDefaultStyle = cabinetNameDefaultStyle;
            }

            return _cabinetNameDefaultStyle;
        }

        /// <summary>
        /// 计算机柜控件高度
        /// </summary>
        /// <param name="height">机柜身体高度</param>
        /// <returns>机柜控件高度</returns>
        public static double CalCabinetControlHeight(int height)
        {
            return height * CabinetConstant.SINGLE_U_HEIGHT + _titleHeight + _bottomHeight;
        }

        private void SetCabinetDeviceUnit(CabinetInfo cabinetInfo)
        {
            stackPanel.Children.Clear();
            if (cabinetInfo == null)
            {
                return;
            }

            tbName.Text = cabinetInfo.Name;
            this.Height = CalCabinetControlHeight(cabinetInfo.Height);

            int targetIndex = 1;
            var cabinetDeviceUnitList = cabinetInfo.CabinetDeviceUnitList;
            if (cabinetDeviceUnitList == null)
            {
                //空U
                this.FillEmptyU(targetIndex, cabinetInfo.Height);
                return;
            }

            cabinetDeviceUnitList = cabinetDeviceUnitList.OrderBy(t => { return t.BeginLocation; }).ToList();
            targetIndex = 0;
            int tmp;

            foreach (var cabinetDeviceUnit in cabinetDeviceUnitList)
            {
                if (cabinetDeviceUnit.BeginLocation > targetIndex)
                {
                    if (targetIndex > 0)
                    {
                        tmp = cabinetDeviceUnit.BeginLocation - targetIndex;
                    }
                    else
                    {
                        targetIndex = 1;
                        tmp = cabinetDeviceUnit.BeginLocation - 1;
                    }

                    //插入空U
                    this.FillEmptyU(targetIndex, tmp);
                }

                //插入设备U
                var cabinetDeviceUControl = new UCCabinetDeviceUControl();
                cabinetDeviceUControl.UpdateCabinetDevice(cabinetDeviceUnit, this.DeviceNameStyle);
                stackPanel.Children.Insert(0, cabinetDeviceUControl);
                targetIndex = cabinetDeviceUnit.BeginLocation + cabinetDeviceUnit.Height;
            }

            if (targetIndex <= cabinetInfo.Height)
            {
                this.FillEmptyU(targetIndex, cabinetInfo.Height - targetIndex + 1);
            }
        }

        private void FillEmptyU(int begindex, int count)
        {
            var cabinetEmptyUControl = new UCCabinetEmptyUControl();
            var emptyCabinetDeviceUnit = new CabinetDeviceUnit();
            emptyCabinetDeviceUnit.BeginLocation = begindex;
            emptyCabinetDeviceUnit.Height = count;
            cabinetEmptyUControl.UpdateCabinetDevice(emptyCabinetDeviceUnit);
            stackPanel.Children.Insert(0, cabinetEmptyUControl);
        }



        private void SetDeviceNameStyle(Style style)
        {
            if (stackPanel.Children.Count == 0)
            {
                return;
            }

            foreach (var ele in stackPanel.Children)
            {
                if (ele is UCCabinetDeviceUControl)
                {
                    ((UCCabinetDeviceUControl)ele).UpdateDeviceNameStyle(style);
                }
            }
        }

        private void SetCabinetNameStyle(Style style)
        {
            tbName.Style = style;
        }
    }
}
