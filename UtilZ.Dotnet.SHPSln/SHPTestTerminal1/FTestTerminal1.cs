using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.Appender;

namespace SHPTestTerminal1
{
    public partial class FTestTerminal1 : Form
    {
        public FTestTerminal1()
        {
            InitializeComponent();
        }

        private void FTestTerminal1_Load(object sender, EventArgs e)
        {
            try
            {
                //ObservableCollection<int> aa = null;
                //BindingList<int> bb = null;



                //Collection<int> ca = aa;
                //ca.Add
                //ca.Remove
                //ca.Clear
                //ca.Contains
                //ca.CopyTo

                //var xx = new BindingCollection();


                var redirectAppenderToUI = (RedirectAppender)Loger.GetAppenderByName(null, System.Configuration.ConfigurationManager.AppSettings["redirectToUIAppendName"]);
                if (redirectAppenderToUI != null)
                {
                    redirectAppenderToUI.RedirectOuput += RedirectOuput;
                }
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
        private void RedirectOuput(object sender, RedirectOuputArgs e)
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

        private TestTerminl1BLL _testBLL = null;
        private void btnInit_Click(object sender, EventArgs e)
        {
            try
            {
                Loger.Info("初始化开始...");
                if (this._testBLL == null)
                {
                    this._testBLL = new TestTerminl1BLL();
                }
                Loger.Info("初始化完成...");
            }
            catch (Exception ex)
            {
                Loger.Error(ex, "初始化异常");
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (this._testBLL == null)
                    {
                        Loger.Warn("未初始化...");
                        return;
                    }

                    this._testBLL.Request();
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            });
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (this._testBLL == null)
                    {
                        Loger.Warn("未初始化...");
                        return;
                    }

                    this._testBLL.Post();
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                }
            });
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            logControl.Clear();
        }
    }
}
