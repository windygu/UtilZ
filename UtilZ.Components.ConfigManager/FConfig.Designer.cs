namespace UtilZ.Components.ConfigManager
{
    partial class FConfig
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pgConfigParaKeyValue = new UtilZ.Lib.WinformEx.PageGrid.PageGridControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pgValidDomain = new UtilZ.Lib.WinformEx.PageGrid.PageGridControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tscbGroup = new System.Windows.Forms.ToolStripComboBox();
            this.tsbServiceMapManager = new System.Windows.Forms.ToolStripButton();
            this.tsbParaGroupManager = new System.Windows.Forms.ToolStripButton();
            this.tsbSearch = new System.Windows.Forms.ToolStripButton();
            this.tsbConfigParaManager = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer.Size = new System.Drawing.Size(784, 536);
            this.splitContainer.SplitterDistance = 546;
            this.splitContainer.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pgConfigParaKeyValue);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 536);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数列表";
            // 
            // pgConfigParaKeyValue
            // 
            this.pgConfigParaKeyValue.BackColor = System.Drawing.SystemColors.Control;
            this.pgConfigParaKeyValue.ColumnSettingVisible = true;
            this.pgConfigParaKeyValue.DataReadOnly = true;
            this.pgConfigParaKeyValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgConfigParaKeyValue.GridContextMenuStrip = null;
            this.pgConfigParaKeyValue.Location = new System.Drawing.Point(3, 17);
            this.pgConfigParaKeyValue.MinimumSize = new System.Drawing.Size(30, 30);
            this.pgConfigParaKeyValue.MuiltSelect = false;
            this.pgConfigParaKeyValue.Name = "pgConfigParaKeyValue";
            this.pgConfigParaKeyValue.PagingVisible = true;
            this.pgConfigParaKeyValue.Size = new System.Drawing.Size(540, 516);
            this.pgConfigParaKeyValue.TabIndex = 0;
            this.pgConfigParaKeyValue.DataRowDoubleClick += new System.EventHandler<UtilZ.Lib.WinformEx.PageGrid.DataRowDoubleClickArgs>(this.pgConfigParaKeyValue_DataRowDoubleClick);
            this.pgConfigParaKeyValue.SelectionChanged += new System.EventHandler<UtilZ.Lib.WinformEx.PageGrid.SelectionChangedArgs>(this.pgConfigParaKeyValue_SelectionChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pgValidDomain);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(234, 536);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "作用域";
            // 
            // pgValidDomain
            // 
            this.pgValidDomain.BackColor = System.Drawing.SystemColors.Control;
            this.pgValidDomain.ColumnSettingVisible = false;
            this.pgValidDomain.DataReadOnly = true;
            this.pgValidDomain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgValidDomain.GridContextMenuStrip = null;
            this.pgValidDomain.Location = new System.Drawing.Point(3, 17);
            this.pgValidDomain.MinimumSize = new System.Drawing.Size(30, 30);
            this.pgValidDomain.MuiltSelect = false;
            this.pgValidDomain.Name = "pgValidDomain";
            this.pgValidDomain.PagingVisible = false;
            this.pgValidDomain.Size = new System.Drawing.Size(228, 516);
            this.pgValidDomain.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbServiceMapManager,
            this.tsbParaGroupManager,
            this.tsbSearch,
            this.tscbGroup,
            this.tsbConfigParaManager});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tscbGroup
            // 
            this.tscbGroup.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tscbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbGroup.Name = "tscbGroup";
            this.tscbGroup.Size = new System.Drawing.Size(121, 25);
            // 
            // tsbServiceMapManager
            // 
            this.tsbServiceMapManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbServiceMapManager.Image = global::UtilZ.Components.ConfigManager.Properties.Resources.Map;
            this.tsbServiceMapManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbServiceMapManager.Name = "tsbServiceMapManager";
            this.tsbServiceMapManager.Size = new System.Drawing.Size(23, 22);
            this.tsbServiceMapManager.Text = "服务映射管理";
            this.tsbServiceMapManager.Click += new System.EventHandler(this.tsbServiceMapManager_Click);
            // 
            // tsbParaGroupManager
            // 
            this.tsbParaGroupManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbParaGroupManager.Image = global::UtilZ.Components.ConfigManager.Properties.Resources.group;
            this.tsbParaGroupManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbParaGroupManager.Name = "tsbParaGroupManager";
            this.tsbParaGroupManager.Size = new System.Drawing.Size(23, 22);
            this.tsbParaGroupManager.Text = "配置参数组管理";
            this.tsbParaGroupManager.Click += new System.EventHandler(this.tsbParaGroupManager_Click);
            // 
            // tsbSearch
            // 
            this.tsbSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSearch.Image = global::UtilZ.Components.ConfigManager.Properties.Resources.search;
            this.tsbSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSearch.Name = "tsbSearch";
            this.tsbSearch.Size = new System.Drawing.Size(23, 22);
            this.tsbSearch.Text = "查询";
            this.tsbSearch.Click += new System.EventHandler(this.tsbSearch_Click);
            // 
            // tsbConfigParaManager
            // 
            this.tsbConfigParaManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbConfigParaManager.Image = global::UtilZ.Components.ConfigManager.Properties.Resources.ParaManager;
            this.tsbConfigParaManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConfigParaManager.Name = "tsbConfigParaManager";
            this.tsbConfigParaManager.Size = new System.Drawing.Size(23, 22);
            this.tsbConfigParaManager.Text = "配置参数管理";
            this.tsbConfigParaManager.Click += new System.EventHandler(this.tsbConfigParaManager_Click);
            // 
            // FConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置参数管理";
            this.Load += new System.EventHandler(this.FConfig_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbServiceMapManager;
        private System.Windows.Forms.ToolStripButton tsbParaGroupManager;
        private System.Windows.Forms.ToolStripButton tsbSearch;
        private System.Windows.Forms.ToolStripComboBox tscbGroup;
        private Lib.WinformEx.PageGrid.PageGridControl pgConfigParaKeyValue;
        private Lib.WinformEx.PageGrid.PageGridControl pgValidDomain;
        private System.Windows.Forms.ToolStripButton tsbConfigParaManager;
    }
}

