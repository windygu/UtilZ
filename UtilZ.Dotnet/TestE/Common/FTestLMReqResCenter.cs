using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.LRR;

namespace TestE.Common
{
    public partial class FTestLMReqResCenter : Form
    {
        public FTestLMReqResCenter()
        {
            InitializeComponent();
        }

        private void FTestLocalMeseageReqResCenter_Load(object sender, EventArgs e)
        {
            LMReqResCenter.RegisteRes(new LMRes(1, ResAction));
        }

        private object ResAction(object p)
        {
            return 123;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var req = new LMReq(1, "abc");
            LMReqResCenter.Req(req);
            string str = req.ResResult.ToString();
        }
    }
}
