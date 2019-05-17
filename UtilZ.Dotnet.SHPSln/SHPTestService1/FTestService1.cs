using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;

namespace SHPTestService1
{
    public partial class FTestService1 : Form
    {
        public FTestService1()
        {
            InitializeComponent();
        }

        private void FTestService1_Load(object sender, EventArgs e)
        {
            try
            {
                RedirectOuputCenter.Add(new RedirectOutputChannel(this.LogOutputToUI, System.Configuration.ConfigurationManager.AppSettings["redirectToUIAppendName"]));
                this.button1_Click(sender, e);
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "启动Agent异常");
            }
        }

        /// <summary>
        /// 日志输出到UI
        /// </summary>
        /// <param name="e"></param>
        private void LogOutputToUI(RedirectOuputItem e)
        {
            try
            {
                if (e == null || e.Item == null)
                {
                    return;
                }

                string logInfo = string.Format("{0} {1} {2}", e.Item.Time.ToString("yyyy-MM-dd HH:mm:ss"), LogConstant.GetLogLevelName(e.Item.Level), e.Item.Content);
                logControl.AddLog(logInfo, e.Item.Level);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private TestService1BLL _testBLL = null;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._testBLL == null)
                {
                    this._testBLL = new TestService1BLL();
                    this.Text = this._testBLL.ServiceInsName;
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void FTestService1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._testBLL != null)
            {
                this._testBLL.Stop();
            }
        }
    }
}
