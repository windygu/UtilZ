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

namespace UtilZ.Components.ConfigManager
{
    public partial class FServiceMapManager : Form
    {
        public FServiceMapManager()
        {
            InitializeComponent();
        }

        private readonly ConfigLogic _configLogic;
        private readonly UIBindingList<ConfigParaServiceMap> _bindItems = new UIBindingList<ConfigParaServiceMap>();
        private List<ConfigParaServiceMap> _srcItems;
        public FServiceMapManager(ConfigLogic configLogic) : this()
        {
            this._configLogic = configLogic;
        }

        private void FServiceMapManager_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            try
            {
                this._srcItems = this._configLogic.GetAllConfigParaServiceMap();
                this._bindItems.AddRange(this._srcItems);
                this.pgServiceMap.ShowData("FServiceMapManager.ConfigParaServiceMap", this._bindItems, null, new ConfigParaServiceMap().GetAllowEditColumns());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }

        private void tsmiAdd_Click(object sender, EventArgs e)
        {
            this._bindItems.Add(new ConfigParaServiceMap());
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.pgServiceMap.SelectedRows.Length == 0)
                {
                    return;
                }

                var delItems = (from selectedRow in this.pgServiceMap.SelectedRows select (ConfigParaServiceMap)((DataGridViewRow)selectedRow).DataBoundItem).ToList();
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
                List<ConfigParaServiceMap> addItems = (from tmpItem in this._bindItems where !this._srcItems.Contains(tmpItem) select tmpItem).ToList();
                List<ConfigParaServiceMap> delItems = (from tmpItem in this._srcItems where !this._bindItems.Contains(tmpItem) select tmpItem).ToList();
                List<ConfigParaServiceMap> updateItems = (from tmpItem in this._bindItems where !addItems.Contains(tmpItem) && !delItems.Contains(tmpItem) select tmpItem).ToList();
                this._configLogic.SaveConfigParaServiceMap(addItems, delItems, updateItems);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Loger.Error(ex);
            }
        }
    }
}
