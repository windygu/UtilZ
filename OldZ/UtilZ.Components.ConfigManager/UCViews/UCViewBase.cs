using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Components.ConfigBLL;

namespace UtilZ.Components.ConfigManager.UCViews
{
    public partial class UCViewBase : UserControl
    {
        public UCViewBase()
        {
            InitializeComponent();
        }

        protected ConfigLogic _configLogic;

        public ConfigLogic ConfigLogic
        {
            get { return _configLogic; }
            set { _configLogic = value; }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public virtual void RefreshData()
        {
            throw new NotImplementedException();
        }
    }
}
