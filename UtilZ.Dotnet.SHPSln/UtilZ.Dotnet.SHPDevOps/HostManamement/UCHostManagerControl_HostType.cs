using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Model;

namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    public partial class UCHostManagerControl
    {
        #region 主机类型
        private void tsmiHostTypeAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var frm = new FEditHostType(this._hostManager, null);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                //this._hostManager.AddHostType(frm.GetHostTypeItem());
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiHostTypeDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvHostType.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var hostTypeItem = (HostTypeItem)selectedRows[0].DataBoundItem;
                this._hostManager.RemoveHostType(hostTypeItem);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiHostTypeModify_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = this.dgvHostType.SelectedRows;
                if (selectedRows == null || selectedRows.Length == 0)
                {
                    return;
                }

                var hostTypeItem = (HostTypeItem)selectedRows[0].DataBoundItem;
                var frm = new FEditHostType(this._hostManager, hostTypeItem);
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                //this._hostManager.ModifyHostType(hostTypeItem, frm.GetHostTypeItem());
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void tsmiHostTypeClear_Click(object sender, EventArgs e)
        {
            try
            {
                this._hostManager.ClearHostType();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
        #endregion
    }
}
