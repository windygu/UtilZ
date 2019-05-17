namespace UtilZ.Dotnet.SHPDGPULoadPlugin
{
    partial class UCGPUStatusShowControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lineChartControl = new UtilZ.Dotnet.WindowEx.Winform.Controls.LineChartControl();
            this.SuspendLayout();
            // 
            // lineChartControl
            // 
            this.lineChartControl.BackColor = System.Drawing.Color.Black;
            this.lineChartControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineChartControl.DrawDirection = false;
            this.lineChartControl.GridLineColor = System.Drawing.Color.SeaGreen;
            this.lineChartControl.GridLineWidth = 1F;
            this.lineChartControl.IsMoveGrid = true;
            this.lineChartControl.Location = new System.Drawing.Point(0, 0);
            this.lineChartControl.MinimumSize = new System.Drawing.Size(50, 30);
            this.lineChartControl.Name = "lineChartControl";
            this.lineChartControl.Size = new System.Drawing.Size(294, 110);
            this.lineChartControl.TabIndex = 0;
            this.lineChartControl.Text = "lineChartControl1";
            // 
            // UCGPULoadDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lineChartControl);
            this.Name = "UCGPULoadDisplayControl";
            this.Size = new System.Drawing.Size(294, 110);
            this.Load += new System.EventHandler(this.UCGPUStatusShowControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private WindowEx.Winform.Controls.LineChartControl lineChartControl;
    }
}
