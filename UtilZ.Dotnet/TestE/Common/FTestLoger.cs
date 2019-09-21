using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.Ex.Log.Appender;

namespace TestE.Common
{
    public partial class FTestLoger : Form
    {
        public FTestLoger()
        {
            InitializeComponent();

            LogSysInnerLog.Log += LogSysInnerLog_Log;

            //System.Threading.Thread.Sleep(20 * 1000);
            //Loger.LoadInit();

            var redirectAppenderToUI = (RedirectAppender)Loger.GetAppenderByName(null, null);
            if (redirectAppenderToUI != null)
            {
                redirectAppenderToUI.RedirectOuput += RedirectLogOutput;
            }

            checkBox1.Checked = true;
        }

        private void LogSysInnerLog_Log(object sender, InnerLogOutputArgs e)
        {
            string str;
            if (e.Ex == null)
            {
                str = "LogSysInnerLog_Log";
            }
            else
            {
                str = e.Ex.Message;
            }

            logControlF1.AddLog(str, 1);
        }

        private void RedirectLogOutput(object sender, RedirectOuputArgs e)
        {
            string str;
            try
            {
                str = string.Format("{0} {1}", DateTime.Now, e.Item.Content);
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }

            logControlF1.AddLog(str, e.Item.Level);
        }

        private void FTestLoger_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Loger.Error("Error Test");
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Loger.GetLoger(null).GetAppenders()[0].Config.Layout = string.Format("{0} {1} {2}", LogConstant.TIME, LogConstant.LEVEL, LogConstant.CONTENT);
            Loger.Debug("Debug");
            Loger.Info("Info");
            Loger.Warn("Warn");

            try
            {
                Test1();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }

            try
            {
                //Loger.Debug("Debug");
                Test1();
            }
            catch (Exception ex)
            {
                Loger.Error("有自定义消息内容", ex);
            }

            try
            {
                object obj = null;
                string name = obj.ToString();
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }

            try
            {
                object obj = null;
                string name = obj.ToString();
            }
            catch (Exception ex)
            {
                Loger.Error("就一层日志", ex);
            }
        }

        private void Test1()
        {
            try
            {
                Test2();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("111", ex);
            }
        }

        private void Test2()
        {
            try
            {
                Test3();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("222", ex);
            }
        }

        private void Test3()
        {
            try
            {
                object obj = null;
                string name = obj.ToString();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("333", ex);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                var redirectAppenderToUI = (RedirectAppender)Loger.GetAppenderByName(null, null);
                if (redirectAppenderToUI != null)
                {
                    redirectAppenderToUI.RedirectOuput += RedirectLogOutput;
                }
            }
            else
            {
                var redirectAppenderToUI = (RedirectAppender)Loger.GetAppenderByName(null, null);
                if (redirectAppenderToUI != null)
                {
                    redirectAppenderToUI.RedirectOuput -= RedirectLogOutput;
                }
            }

        }
    }
}
