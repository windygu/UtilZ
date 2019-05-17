using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.SHPBase.Monitor;

namespace UtilZ.Dotnet.SHPAgent
{
    public partial class FAppMonitorItemEdit : Form
    {
        /// <summary>
        /// 构造函数-添加
        /// </summary>
        public FAppMonitorItemEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数-修改
        /// </summary>
        /// <param name="monitorItem">编辑项</param>
        public FAppMonitorItemEdit(AppMonitorItem monitorItem)
        : this()
        {
            txtAppExeFilePath.Text = monitorItem.AppExeFilePath;
            txtProcessFilePath.Text = monitorItem.AppProcessFilePath;
            txtAppName.Text = monitorItem.AppName;
            txtAppArguments.Text = monitorItem.Arguments;
        }

        public AppMonitorItem GetAppMonitorItem()
        {
            return new AppMonitorItem(txtAppName.Text, txtAppExeFilePath.Text, txtProcessFilePath.Text, txtAppArguments.Text);
        }

        private void FAppMonitorItemEdit_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

        }

        private void btnChioceAppExeFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "(可执行程序*.exe)|*.exe|(全部*.*)|*.*";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtAppExeFilePath.Text = ofd.FileName;
            if (string.IsNullOrWhiteSpace(txtProcessFilePath.Text))
            {
                txtProcessFilePath.Text = ofd.FileName;
            }

            txtAppName.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
            txtAppArguments.Text = string.Empty;
        }

        private void btnChioceProcessFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "(可执行程序*.exe)|*.exe|(全部*.*)|*.*";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtProcessFilePath.Text = ofd.FileName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var appFilePath = txtAppExeFilePath.Text;
            if (string.IsNullOrWhiteSpace(appFilePath))
            {
                MessageBox.Show($"{lbAppExeFilePath.Text}不能为空");
                return;
            }

            if (!File.Exists(appFilePath))
            {
                MessageBox.Show($"{lbAppExeFilePath.Text}指示文件不存在");
                return;
            }

            var processFilePath = txtProcessFilePath.Text;
            if (string.IsNullOrWhiteSpace(processFilePath))
            {
                MessageBox.Show($"{lbProcessFilePath.Text}不能为空");
                return;
            }

            if (!File.Exists(processFilePath))
            {
                MessageBox.Show($"{lbProcessFilePath.Text}指示文件不存在");
                return;
            }

            var appName = txtAppName.Text;
            if (string.IsNullOrWhiteSpace(appFilePath))
            {
                MessageBox.Show($"{lbAppName.Text}不能为空");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancell_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
