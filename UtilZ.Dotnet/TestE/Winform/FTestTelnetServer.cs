using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.Log;

namespace TestE.Winform
{
    public partial class FTestTelnetServer : Form
    {
        private readonly TelnetServer ts;
        public FTestTelnetServer()
        {
            InitializeComponent();

            ts = new TelnetServer(ProCallback, "测试服务", new TelnetAuthInfo("yf", "qwe123"), IPAddress.Parse("0.0.0.0"), 14002, 3);
        }


        private static string ProCallback(string cmd)
        {
            Loger.Info(cmd);
            return string.Format("{0} res {1}", DateTime.Now.ToString(), cmd);
        }

        private void FTestTelnetServer_Load(object sender, EventArgs e)
        {
            var subLog = new UtilZ.Dotnet.Ex.Log.LogOutput.LogOutputSubscribeItem(null, null);
            subLog.LogOutput += SubLog_LogOutput;
            Loger.LogOutput.AddLogOutput(subLog);
            Loger.LogOutput.Enable = true;
            ts.Start();
        }

        private void SubLog_LogOutput(object sender, UtilZ.Dotnet.Ex.Log.Model.LogOutputArgs e)
        {
            logControl1.AddLog(string.Format("{0} {1}", e.Item.Time, e.Item.Content));
        }

        private void logControl1_Load(object sender, EventArgs e)
        {

        }

        private readonly Random _Rnd = new Random();
        private void btnTest_Click(object sender, EventArgs e)
        {
            Loger.Error(_Rnd.Next().ToString());
        }
    }
}
