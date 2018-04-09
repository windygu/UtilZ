using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Interface;
using UtilZ.Dotnet.DBModel.Config;
using UtilZ.Dotnet.DBModel.DBObject;
using UtilZ.Dotnet.WindowEx.Winform.Base;

namespace TestE.DB
{
    public partial class FTestDB : Form
    {
        public FTestDB()
        {
            InitializeComponent();
        }

        private void FTestDB_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

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

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                DBConfigElement dbConfig = DropdownBoxHelper.GetGenericFromComboBox<DBConfigElement>(comDB);
                IDBAccess dbAccess = DBAccessManager.GetDBAccessInstance(dbConfig.DBID);
                //dataGridView1.DataSource = dbAccess.QueryData(@"select * from Stu");
                //dataGridView1.DataSource = dbAccess.QueryT<Stu>(@"select * from Stu");
                dataGridView1.DataSource = dbAccess.QueryPagingData(@"select * from Stu", "ID", 10, 1, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    [DBTableAttribute("Stu")]
    public class Stu
    {
        [DBColumnAttribute("ID", true, DBFieldDataAccessType.R)]
        public int ID { get; set; }

        [DisplayName("姓名")]
        [DBColumnAttribute("Name")]
        public string Name { get; set; }

        [DisplayName("年龄")]
        [DBColumnAttribute("Age")]
        public int Age { get; set; }

        [DisplayName("地址")]
        [DBColumnAttribute("Addr")]
        public string Addr { get; set; }
    }
}
