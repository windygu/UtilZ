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
using UtilZ.Lib.Base.DataStruct.UIBinding;
using UtilZ.Lib.Base.Log;
using UtilZ.Lib.Winform.DropdownBox;

namespace UtilZ.Components.ConfigManager
{
    public partial class FParaManager : Form
    {
        public FParaManager()
        {
            InitializeComponent();
        }

        private readonly ConfigLogic _configLogic;
        private readonly UIBindingList<ConfigParaKeyValue> _bindItems = new UIBindingList<ConfigParaKeyValue>();
        private List<ConfigParaKeyValue> _srcItems;
        public FParaManager(ConfigLogic configLogic) : this()
        {
            this._configLogic = configLogic;
        }

        private void FParaManager_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            try
            {
                List<ConfigParaGroup> groups = this._configLogic.GetAllConfigParaGroup();
                DropdownBoxHelper.BindingIEnumerableGenericToComboBox<ConfigParaGroup>(comboBoxParaGroup, groups, nameof(ConfigParaGroup.Name), null);
                this._srcItems = this._configLogic.GetAllConfigParaKeyValue();
                this._bindItems.AddRange(this._srcItems);
                this.pgPara.ShowData(this._bindItems, "FParaManager.ConfigParaKeyValue", null, null, new ConfigParaKeyValue().GetAllowEditColumns());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }

        private void tsmiAdd_Click(object sender, EventArgs e)
        {
            this._bindItems.Add(new ConfigParaKeyValue());
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.pgPara.SelectedRows.Length == 0)
                {
                    return;
                }

                var delItems = (from selectedRow in this.pgPara.SelectedRows select (ConfigParaKeyValue)((DataGridViewRow)selectedRow).DataBoundItem).ToList();
                this._bindItems.RemoveArrange(delItems);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<ConfigParaKeyValue> addItems = (from tmpItem in this._bindItems where !this._srcItems.Contains(tmpItem) select tmpItem).ToList();
                List<ConfigParaKeyValue> delItems = (from tmpItem in this._srcItems where !this._bindItems.Contains(tmpItem) select tmpItem).ToList();
                List<ConfigParaKeyValue> updateItems = (from tmpItem in this._bindItems where !addItems.Contains(tmpItem) && !delItems.Contains(tmpItem) select tmpItem).ToList();
                this._configLogic.SaveConfigParaKeyValueEdit(addItems, delItems, updateItems);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }

        private void comboBoxParaGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pgPara.SelectedRows.Length == 0)
                {
                    return;
                }

                var group = DropdownBoxHelper.GetGenericFromComboBox<ConfigParaGroup>(comboBoxParaGroup);
                foreach (var selectedRow in this.pgPara.SelectedRows)
                {
                    ((ConfigParaKeyValue)((DataGridViewRow)selectedRow).DataBoundItem).Group = group;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }

        private void pgPara_SelectionChanged(object sender, Lib.Winform.PageGrid.Interface.DataRowSelectionChangedArgs e)
        {
            try
            {
                if (e.Row == null)
                {
                    return;
                }

                var group = ((ConfigParaKeyValue)e.Row).Group;
                if (group == null)
                {
                    ((ConfigParaKeyValue)e.Row).Group = DropdownBoxHelper.GetGenericFromComboBox<ConfigParaGroup>(comboBoxParaGroup);
                }
                else
                {
                    DropdownBoxHelper.SetGenericToComboBox(comboBoxParaGroup, group);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }
    }
}
