﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace UtilZ.Dotnet.WindowEx.Winform.Controls
{
    /// <summary>
    /// FlowLayoutPanel子类
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultProperty("FlowDirection")]
    [Designer("System.Windows.Forms.Design.FlowLayoutPanelDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [Docking(DockingBehavior.Ask)]
    [ProvideProperty("FlowBreak", typeof(Control))]
    [IODescriptionAttribute("DescriptionFlowLayoutPanel")]
    public class FlowLayoutPanelZ : FlowLayoutPanel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowLayoutPanelZ()
            : base()
        {

        }

        /// <summary>
        /// 触发OnSizeChanged事件
        /// </summary>
        public void OnSizeChanged()
        {
            base.OnSizeChanged(null);
        }
    }
}
