using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Components.ConfigBLL;
using UtilZ.Components.ConfigModel;
using UtilZ.Lib.Base.Log;
using UtilZ.Lib.Winform.DropdownBox;

namespace UtilZ.Components.ConfigManager
{
    public partial class FConfig : Form
    {
        private readonly ConfigLogic _configLogic = new ConfigLogic();
        public FConfig()
        {
            InitializeComponent();
        }

        private void RefreshParaGroup()
        {
            try
            {
                List<ConfigParaGroup> groups = this._configLogic.GetAllConfigParaGroup();
                ConfigParaGroup selectedItem = null;
                if (tscbGroup.Items.Count > 0)
                {
                    var oldSelectedItem = DropdownBoxHelper.GetGenericFromToolStripComboBox<ConfigParaGroup>(tscbGroup);
                    if (oldSelectedItem != null)
                    {
                        var ret = (from tmpItm in groups where oldSelectedItem.ID == tmpItm.ID select tmpItm).ToList();
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

                this.tscbGroup.SelectedIndexChanged -= this.tscbGroup_SelectedIndexChanged;
                try
                {
                    DropdownBoxHelper.BindingIEnumerableGenericToToolStripComboBox<ConfigParaGroup>(tscbGroup, groups, nameof(ConfigParaGroup.Name), null);
                }
                finally
                {
                    this.tscbGroup.SelectedIndex = -1;
                    this.tscbGroup.SelectedIndexChanged += this.tscbGroup_SelectedIndexChanged;
                    DropdownBoxHelper.SetGenericToToolStripComboBox<ConfigParaGroup>(tscbGroup, selectedItem);
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void FConfig_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            try
            {
                this._configLogic.Init();
                this.RefreshParaGroup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }

        private void tsbServiceMapManager_Click(object sender, EventArgs e)
        {
            var frm = new FServiceMapManager(this._configLogic);
            frm.ShowDialog();
        }

        private void tsbParaGroupManager_Click(object sender, EventArgs e)
        {
            var frm = new FParaGroupManager(this._configLogic);
            frm.ShowDialog();
            this.RefreshParaGroup();
        }

        private void tsbConfigParaManager_Click(object sender, EventArgs e)
        {
            var frm = new FParaManager(this._configLogic);
            frm.ShowDialog();
            this.RefreshParaGroup();
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {

        }

        private void tscbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = DropdownBoxHelper.GetGenericFromToolStripComboBox<ConfigParaGroup>(tscbGroup);
                if (selectedItem == null)
                {
                    return;
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
