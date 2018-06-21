using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;

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

            var subLog = new UtilZ.Dotnet.Ex.Log.LogOutput.LogOutputSubscribeItem(null, null);
            subLog.LogOutput += SubLog_LogOutput;
            Loger.LogOutput.AddLogOutput(subLog);
            Loger.LogOutput.Enable = true;
        }

        private void LogSysInnerLog_Log(object sender, UtilZ.Dotnet.Ex.Log.Model.InnerLogOutputArgs e)
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

        private void SubLog_LogOutput(object sender, UtilZ.Dotnet.Ex.Log.Model.LogOutputArgs e)
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
            Loger.Info("Info Test");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Loger.Debug("Debug");
            }
            catch (Exception ex)
            {
                Loger.Error("XX", ex);
            }
        }
    }
}
