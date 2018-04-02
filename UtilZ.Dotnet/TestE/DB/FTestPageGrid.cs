using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestE.DB.Model;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Interface;
using UtilZ.Dotnet.DBModel.Config;
using UtilZ.Dotnet.WindowEx.Winform.Base;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.Interface;

namespace TestE.DB
{
    public partial class FTestPageGrid : Form
    {
        public FTestPageGrid()
        {
            InitializeComponent();
        }

        private void FTestPageGrid_Load(object sender, EventArgs e)
        {
            try
            {
                List<DBConfigElement> configItems = ConfigManager.GetAllConfigItems();
                DropdownBoxHelper.BindingIEnumerableGenericToComboBox<DBConfigElement>(comDB, configItems, "ConName");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DBConfigElement GetConfig()
        {
            return DropdownBoxHelper.GetGenericFromComboBox<DBConfigElement>(comDB);
        }

        private IDBAccess GetDBAccess()
        {
            return this.GetDBAccess(this.GetConfig().DBID);
        }

        private IDBAccess GetDBAccess(int dbid)
        {
            return DBAccessManager.GetDBAccessInstance(dbid);
        }
        private void QueryPageInfo(int pageSize)
        {
            var dal = GetDBAccess();
            //var dbPageInfo = dal.QueryPageInfo(pageSize, "select count(0) from Stu");
            var dbPageInfo = dal.QueryPageInfoT<Stu>(pageSize);
            ucPageGridControl1.SetPageInfo(new PageInfo(dbPageInfo.PageCount, dbPageInfo.PageSize, 1, dbPageInfo.TotalCount));
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            this.QueryPageInfo(ucPageGridControl1.PageSizeMaximum);
        }

        private void ucPageGridControl1_PageSizeChanged(object sender, UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.Interface.PageSizeChangedArgs e)
        {
            this.QueryPageInfo(e.PageSize);
        }


        private void ucPageGridControl1_QueryData(object sender, UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.Interface.QueryDataArgs e)
        {
            var dal = GetDBAccess();
            var stus = dal.QueryTPaging<Stu>(e.PageSize, e.PageIndex, "ID", true);
            ucPageGridControl1.ShowData(stus, "TestE.DB.FTestPageGrid.ucPageGridControl1_QueryData");
        }

        private void btnSwitchPageSize_Click(object sender, EventArgs e)
        {
            //ucPageGridControl1.PageSizeVisible = !ucPageGridControl1.PageSizeVisible;
            ucPageGridControl1.AlignDirection = !ucPageGridControl1.AlignDirection;
        }

        private void btnColSettingVisible_Click(object sender, EventArgs e)
        {
            ucPageGridControl1.ColumnSettingVisible = !ucPageGridControl1.ColumnSettingVisible;
        }
    }
}
