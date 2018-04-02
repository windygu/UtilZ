using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.ILEx.Compress;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.Interface;

namespace TestE.Common
{
    public partial class FTest : Form
    {
        public FTest()
        {
            InitializeComponent();
        }

        private void FTest_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            var dgv = this.ucPageGridControl1.GridControl;
            dgv.MouseRightButtonChangeSelectedRow = true;
            dgv.MultiSelect = false;
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            var items = new BindingListEx<TestDGVModel>();
            items.Add(new TestDGVModel());
            items.Add(new TestDGVModel());
            items.Add(new TestDGVModel());
            try
            {
                ucPageGridControl1.ShowData(items);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ucPageGridControl1.ColumnSettingVisible = !ucPageGridControl1.ColumnSettingVisible;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ucPageGridControl1.PagingVisible = !ucPageGridControl1.PagingVisible;
        }

        private void ucPageGridControl1_QueryData(object sender, QueryDataArgs e)
        {

        }

        private void ucPageGridControl1_PageSizeChanged(object sender, PageSizeChangedArgs e)
        {

        }

        private void btnTestRar_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog();
            if (openFile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filePath = openFile.FileName;
            label1.Text = "DecompressRar...";
            Task.Factory.StartNew(() =>
            {
                try
                {
                    //CompressHelper.DecompressRar(filePath, @"G:\Tmp\test", true);
                    var files = CompressHelper.GetRarFileList(filePath);
                    CompressHelper.DecompressRar(filePath, files.Take(3), @"G:\Tmp\test", true);

                    this.Invoke(new Action(() =>
                    {
                        label1.Text = "OK";
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        label1.Text = ex.Message;
                    }));
                }
            });
        }
    }

    public class TestDGVModel
    {
        public bool IsSelected { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Addr { get; set; }
    }
}
