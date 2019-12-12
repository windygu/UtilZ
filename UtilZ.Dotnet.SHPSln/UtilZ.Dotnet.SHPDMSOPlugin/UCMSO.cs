using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;
using UtilZ.Dotnet.SHPBase.Common;
using UtilZ.Dotnet.SHPBase.Model;
using UtilZ.Dotnet.WindowEx.Base.PartAsynWait.Model;
using UtilZ.Dotnet.WindowEx.Winform.Controls.PartAsynWait;

namespace UtilZ.Dotnet.SHPDMSOPlugin
{
    public partial class UCMSO : UserControl
    {
        private readonly MSOBLL _bll;
        public UCMSO()
        {
            InitializeComponent();
        }

        internal UCMSO(MSOBLL bll)
            : this()
        {
            this._bll = bll;
        }

        private void UCMSO_Load(object sender, EventArgs e)
        {

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            this.Test(true);
        }

        private void btnTestInter_Click(object sender, EventArgs e)
        {
            this.Test(false);
        }

        private void Test(bool flag)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                Loger.Warn("内容不能为空");
                return;
            }

            var hostInfoObj = Ex.LRPC.LRPCCore.RemoteCallF(SHPConstant.GET_HOST_IFNO_CHANNEL_NAME, null);
            var hostInfo = hostInfoObj as HostInfo;
            if (hostInfo == null)
            {
                Loger.Warn("没有选中的主机");
                return;
            }

            try
            {
                var para = new PartAsynWaitPara<Tuple<HostInfo, MSOBLL, string, bool>, object>();
                para.Caption = "测试...";
                para.IsShowCancel = false;
                para.Para = new Tuple<HostInfo, MSOBLL, string, bool>(hostInfo, this._bll, textBox1.Text, flag);
                para.Function = (p) =>
                {
                    return p.Para.Item2.Test(p.Para.Item1, p.Para.Item3, p.Para.Item4);
                };

                para.Completed = (ret) =>
                {
                    if (ret.Status == PartAsynExcuteStatus.Exception)
                    {
                        Loger.Error(ret.Exception);
                    }
                };

                WinformPartAsynWaitHelper.Wait(para, this);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }
}
