namespace UtilZ.Components.ConfigManager.UCViews
{
    partial class UCServiceView
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pgServiceList = new UtilZ.Lib.WinformEx.PageGrid.PageGridControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pgServiceParaList = new UtilZ.Lib.WinformEx.PageGrid.PageGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer.Size = new System.Drawing.Size(800, 600);
            this.splitContainer.SplitterDistance = 367;
            this.splitContainer.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pgServiceList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 600);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务列表";
            // 
            // pgServiceList
            // 
            this.pgServiceList.BackColor = System.Drawing.SystemColors.Control;
            this.pgServiceList.ColumnSettingVisible = false;
            this.pgServiceList.DataReadOnly = true;
            this.pgServiceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgServiceList.GridContextMenuStrip = null;
            this.pgServiceList.Location = new System.Drawing.Point(3, 17);
            this.pgServiceList.MinimumSize = new System.Drawing.Size(30, 30);
            this.pgServiceList.MuiltSelect = false;
            this.pgServiceList.Name = "pgServiceList";
            this.pgServiceList.PagingVisible = false;
            this.pgServiceList.Size = new System.Drawing.Size(361, 580);
            this.pgServiceList.TabIndex = 0;
            this.pgServiceList.DataRowDoubleClick += new System.EventHandler<UtilZ.Lib.WinformEx.PageGrid.DataRowDoubleClickArgs>(this.pgServiceList_DataRowDoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pgServiceParaList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(429, 600);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "参数列表";
            // 
            // pgServiceParaList
            // 
            this.pgServiceParaList.BackColor = System.Drawing.SystemColors.Control;
            this.pgServiceParaList.ColumnSettingVisible = false;
            this.pgServiceParaList.DataReadOnly = true;
            this.pgServiceParaList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgServiceParaList.GridContextMenuStrip = null;
            this.pgServiceParaList.Location = new System.Drawing.Point(3, 17);
            this.pgServiceParaList.MinimumSize = new System.Drawing.Size(30, 30);
            this.pgServiceParaList.MuiltSelect = false;
            this.pgServiceParaList.Name = "pgServiceParaList";
            this.pgServiceParaList.PagingVisible = false;
            this.pgServiceParaList.Size = new System.Drawing.Size(423, 580);
            this.pgServiceParaList.TabIndex = 1;
            this.pgServiceParaList.DataRowDoubleClick += new System.EventHandler<UtilZ.Lib.WinformEx.PageGrid.DataRowDoubleClickArgs>(this.pgServiceParaList_DataRowDoubleClick);
            // 
            // UCServiceView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "UCServiceView";
            this.Load += new System.EventHandler(this.UCServiceView_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox groupBox1;
        private Lib.WinformEx.PageGrid.PageGridControl pgServiceList;
        private System.Windows.Forms.GroupBox groupBox2;
        private Lib.WinformEx.PageGrid.PageGridControl pgServiceParaList;
    }
}
