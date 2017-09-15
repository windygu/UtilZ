﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Winform.DropdownBox;
using UtilZ.Components.ConfigModel;
using UtilZ.Lib.Base.Log;
using UtilZ.Components.ConfigBLL;

namespace UtilZ.Components.ConfigManager.UCViews
{
    public partial class UCParaView : UCViewBase
    {
        private readonly ConfigParaGroup _allGroup = new ConfigParaGroup();
        public UCParaView() : base()
        {
            InitializeComponent();
        }

        private void UCParaView_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            this._allGroup.Name = "全部";
            this._allGroup.Des = "全部";
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public override void RefreshData()
        {
            try
            {
                List<ConfigParaGroup> groups = this._configLogic.GetAllConfigParaGroup();
                groups.Insert(0, this._allGroup);
                ConfigParaGroup selectedItem = null;
                if (comGroup.Items.Count > 0)
                {
                    var oldSelectedItem = DropdownBoxHelper.GetGenericFromComboBox<ConfigParaGroup>(comGroup);
                    if (oldSelectedItem != null)
                    {
                        var ret = (from tmpItem in groups where oldSelectedItem.ID == tmpItem.ID select tmpItem).ToList();
                        if (ret.Count > 0)
                        {
                            selectedItem = ret[0];
                        }
                    }
                }

                if (selectedItem == null && groups.Count > 0)
                {
                    selectedItem = groups[0];
                }

                this.comGroup.SelectedIndexChanged -= this.comGroup_SelectedIndexChanged;
                try
                {
                    DropdownBoxHelper.BindingIEnumerableGenericToComboBox<ConfigParaGroup>(comGroup, groups, nameof(ConfigParaGroup.Name), null);
                }
                finally
                {
                    this.comGroup.SelectedIndex = -1;
                    this.comGroup.SelectedIndexChanged += this.comGroup_SelectedIndexChanged;
                    DropdownBoxHelper.SetGenericToComboBox(comGroup, selectedItem);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void comGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = DropdownBoxHelper.GetGenericFromComboBox<ConfigParaGroup>(comGroup);
                if (selectedItem == null)
                {
                    return;
                }

                if (this._allGroup == selectedItem)
                {
                    selectedItem = null;
                }

                List<ConfigParaKeyValue> paras = this._configLogic.GetGroupConfigParaKeyValue(selectedItem);
                pgConfigParaKeyValue.ShowData("ConfigParaKeyValue", paras);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void pgConfigParaKeyValue_SelectionChanged(object sender, Lib.WinformEx.PageGrid.SelectionChangedArgs e)
        {
            try
            {
                var configParaKeyValue = (ConfigParaKeyValue)e.Row;
                List<ConfigParaServiceMap> validDomainServices = this._configLogic.GetValidDomainConfigParaServiceMap(configParaKeyValue);
                var mapList = (from tmpItem in validDomainServices select new ConfigParaServiceMap2(tmpItem)).ToList();
                pgValidDomain.ShowData("ConfigParaServiceMap2", mapList);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void pgConfigParaKeyValue_DataRowDoubleClick(object sender, Lib.WinformEx.PageGrid.DataRowDoubleClickArgs e)
        {
            try
            {
                var configParaKeyValue = (ConfigParaKeyValue)e.Row;
                var frm = new FConfigParaKeyValueEdit(this._configLogic, configParaKeyValue);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.pgConfigParaKeyValue_SelectionChanged(sender, new Lib.WinformEx.PageGrid.SelectionChangedArgs(-1, -1, configParaKeyValue, null, null));
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}