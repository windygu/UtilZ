﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Log;

namespace TestE.Common
{
    public partial class FTestLoger : Form
    {
        public FTestLoger()
        {
            InitializeComponent();
        }

        private void FTestLoger_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                object obj = null;
                var ret = obj.ToString();
            }
            catch (Exception ex)
            {
                Loger.Error("XX", ex);
            }
        }
    }
}
