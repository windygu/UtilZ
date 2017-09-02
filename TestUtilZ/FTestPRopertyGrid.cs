using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Winform.PropertyGrid.Demo;

namespace TestUtilZ
{
    public partial class FTestPRopertyGrid : Form
    {
        public FTestPRopertyGrid()
        {
            InitializeComponent();
        }

        private readonly DemoModel _demoModel = new DemoModel();

        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            propertyGrid1.SelectedObject = _demoModel;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }
    }
}
