using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilZ.Dotnet.SQLitePwdTool
{
    public partial class FSQLitePwdSetting : Form
    {
        public FSQLitePwdSetting()
        {
            InitializeComponent();
        }

        private void FSQLitePwdSetting_Load(object sender, EventArgs e)
        {

        }

        private void btnChioceDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtDBPath.Text = ofd.FileName;
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            try
            {
                string dbPath = txtDBPath.Text.Trim();
                if (string.IsNullOrWhiteSpace(dbPath))
                {
                    return;
                }

                if (!File.Exists(dbPath))
                {
                    return;
                }

                string conStr = this.GetDBConStr(dbPath, txtOldPwd.Text);
                using (SQLiteConnection con = new SQLiteConnection(conStr))
                {
                    con.Open();
                    con.ChangePassword(txtNewPwd.Text);
                }

                MessageBox.Show("设置密码成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置密码失败," + ex.Message);
            }
        }

        /// <summary>
        /// 生成数据库连接字符串
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <returns>数据库连接字符串</returns>
        private string GetDBConStr(string dbPath, string pwd)
        {
            SQLiteConnectionStringBuilder scsb = new SQLiteConnectionStringBuilder();
            scsb.Pooling = true;
            scsb.DataSource = dbPath;
            if (!string.IsNullOrEmpty(pwd))
            {
                scsb.Password = pwd;
            }

            string dbDir = Path.GetDirectoryName(scsb.DataSource);
            if (!Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }

            scsb.ReadOnly = false;
            return scsb.ConnectionString;
        }
    }
}
