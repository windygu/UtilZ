using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestE.DB.Units;
using UtilZ.Dotnet.DBIBase.DBModel.Config;
using UtilZ.Dotnet.DBIBase.DBModel.Model;
using UtilZ.Dotnet.Ex.LocalMessageCenter.LMQ;
using UtilZ.Dotnet.WindowEx.Winform.Base;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.Interface;

namespace TestE.DB
{
    public partial class FTest : Form
    {
        public FTest()
        {
            InitializeComponent();

            pageGridControl1.QueryData += pageGridControl1_QueryAssignPageData;
        }

        private readonly UnitTestBase _unitTest = new UnitTestBase();
        private void FTest_Load(object sender, EventArgs e)
        {
            try
            {
                List<DBConfigElement> configItems = ConfigManager.GetAllConfigItems();
                DropdownBoxHelper.BindingIEnumerableGenericToComboBox<DBConfigElement>(comDB, configItems, "ConName");
                //var ret = typeof(DBAccessBase).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static| System.Reflection.BindingFlags.Instance);
                //_unitTest.RegisteDBModel();

                var subItem = new SubscibeItem("xx", this.MessageNotify);
                LMQCenter.Subscibe(subItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void MessageNotify(SubscibeItem subscibeItem, object obj)
        {
            if (obj == null)
            {
                return;
            }

            if (listBox1.InvokeRequired)
            {
                this.Invoke(new Action(() => { this.MessageNotify(subscibeItem, obj); }));
            }
            else
            {
                listBox1.Items.Add(obj.ToString());
            }
        }


        private DBConfigElement GetConfig()
        {
            return DropdownBoxHelper.GetGenericFromComboBox<DBConfigElement>(comDB);
        }
        private void btnAtom_Click(object sender, EventArgs e)
        {
            _unitTest.Atom(this.GetConfig());
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            _unitTest.Insert(this.GetConfig());
        }

        private void btnStoredProc_Click(object sender, EventArgs e)
        {
            _unitTest.StoredProc(this.GetConfig());
        }

        private void btnDBInfo_Click(object sender, EventArgs e)
        {
            _unitTest.TestDBInfo(this.GetConfig());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _unitTest.TestUpdate(this.GetConfig());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _unitTest.TestDelete(this.GetConfig());
        }

        private void btnQueryPage_Click(object sender, EventArgs e)
        {

        }

        private readonly int _pageSize = 3;
        private DBConfigElement _config;
        private void btnQueryData_Click(object sender, EventArgs e)
        {
            this._config = this.GetConfig();
            this.Query(this._config, 1);
        }

        void pageGridControl1_QueryAssignPageData(object sender, QueryDataArgs e)
        {
            this.Query(this._config, e.PageIndex);
        }

        private void Query(DBConfigElement config, int pageIndex)
        {
            try
            {
                DBPageInfo dbPageInfo = _unitTest.QueryPage(config, _pageSize);
                if (dbPageInfo.PageCount == 0)
                {
                    pageIndex = 1;
                }

                int count;
                object value = _unitTest.QueryData(config, pageIndex, _pageSize, out count);
                pageGridControl1.SetPageInfo(new PageInfo( dbPageInfo.PageCount, _pageSize, pageIndex, dbPageInfo.TotalCount));
                pageGridControl1.ShowData(value, "Stu");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
