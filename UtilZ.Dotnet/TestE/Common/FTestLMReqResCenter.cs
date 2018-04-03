﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.LRPC;

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
            LRPCCenter.CreateChannel("1", ResAction);
        }

        private object ResAction(object p)
        {
            return string.Format("{0}_{1}", 123, p);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            object ret = LRPCCenter.LRPCCall("1", "abc");
            string str = ret.ToString();
            MessageBox.Show(str);
        }
    }
}
