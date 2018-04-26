using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.DBIBase.DBBase.Core;
using UtilZ.Dotnet.DBIBase.DBBase.Interface;
using UtilZ.Dotnet.DBIBase.DBModel.Config;
using UtilZ.Dotnet.DBIBase.DBModel.DBObject;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.Dotnet.Ex.Log;
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
                //dataGridView1.DataSource = dbAccess.QueryPagingData(@"select * from Stu", "ID", 10, 1, true);

                string sql = @"select * from Stu";
                var dbCon = dbAccess.CreateConnection(DBVisitType.R);
                var cmd = dbCon.Con.CreateCommand();
                cmd.CommandText = sql;
                var da = dbAccess.CreateDbDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                ds.EnforceConstraints = false;
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dbCon.Dispose();


                //DataTable dt = dbAccess.QueryData(sql);
                //dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEFQuery_Click(object sender, EventArgs e)
        {
            try
            {
                DBConfigElement dbConfig = DropdownBoxHelper.GetGenericFromComboBox<DBConfigElement>(comDB);
                // var xx = new UtilZ.Dotnet.DBSQLite.Core.SQLiteDBAccess(1, dbConfig.DatabaseName);

                IDBAccess dbAccess = DBAccessManager.GetDBAccessInstance(dbConfig.DBID);
                using (var context = dbAccess.CreateEFDbContext(DBVisitType.R))
                {
                    dataGridView1.DataSource = context.Query<Stu>().ToList();
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTableStruct_Click(object sender, EventArgs e)
        {
            try
            {
                DBConfigElement dbConfig = DropdownBoxHelper.GetGenericFromComboBox<DBConfigElement>(comDB);
                IDBAccess dbAccess = DBAccessManager.GetDBAccessInstance(dbConfig.DBID);
                dataGridView1.DataSource = dbAccess.GetTableFieldInfos("STU");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }
    }

    [TableAttribute("Stu")]
    public class Stu
    {
        [ColumnAttribute("ID")]
        public int ID { get; set; }

        [DisplayName("姓名")]
        [ColumnAttribute("NAME")]
        public string Name { get; set; }

        [DisplayName("年龄")]
        [ColumnAttribute("AGE")]
        public int Age { get; set; }

        [DisplayName("地址")]
        [ColumnAttribute("ADDR")]
        public string Addr { get; set; }
    }

    //[DBTableAttribute("Stu")]
    //public class Stu
    //{
    //    [DBColumnAttribute("ID", true, DBFieldDataAccessType.R)]
    //    public int ID { get; set; }

    //    [DisplayName("姓名")]
    //    [DBColumnAttribute("Name")]
    //    public string Name { get; set; }

    //    [DisplayName("年龄")]
    //    [DBColumnAttribute("Age")]
    //    public int Age { get; set; }

    //    [DisplayName("地址")]
    //    [DBColumnAttribute("Addr")]
    //    public string Addr { get; set; }
    //}
}
