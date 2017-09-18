namespace UtilZ.Lib.Winform.PageGrid
{
    partial class UCPageGridControl
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.panelPage = new System.Windows.Forms.Panel();
            this.labelRecordInfo = new System.Windows.Forms.Label();
            this.btnProPage = new System.Windows.Forms.Button();
            this.labelPageInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.LinkLabel();
            this.numPage = new System.Windows.Forms.NumericUpDown();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.panelColVisibleSetting = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.cmsColVisibleSetting = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiHidenCol = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowModel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowHidenColList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHidenHidenColList = new System.Windows.Forms.ToolStripMenuItem();
            this.panelContent = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panelPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).BeginInit();
            this.panelColVisibleSetting.SuspendLayout();
            this.cmsColVisibleSetting.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowDrop = true;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(225, 81);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.VirtualMode = true;
            // 
            // panelPage
            // 
            this.panelPage.Controls.Add(this.labelRecordInfo);
            this.panelPage.Controls.Add(this.btnProPage);
            this.panelPage.Controls.Add(this.labelPageInfo);
            this.panelPage.Controls.Add(this.label3);
            this.panelPage.Controls.Add(this.label1);
            this.panelPage.Controls.Add(this.btnGo);
            this.panelPage.Controls.Add(this.numPage);
            this.panelPage.Controls.Add(this.btnNextPage);
            this.panelPage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPage.Location = new System.Drawing.Point(0, 81);
            this.panelPage.Name = "panelPage";
            this.panelPage.Size = new System.Drawing.Size(365, 29);
            this.panelPage.TabIndex = 3;
            // 
            // labelRecordInfo
            // 
            this.labelRecordInfo.AutoSize = true;
            this.labelRecordInfo.Location = new System.Drawing.Point(174, 8);
            this.labelRecordInfo.Name = "labelRecordInfo";
            this.labelRecordInfo.Size = new System.Drawing.Size(71, 12);
            this.labelRecordInfo.TabIndex = 14;
            this.labelRecordInfo.Text = "第0条/共0条";
            // 
            // btnProPage
            // 
            this.btnProPage.Location = new System.Drawing.Point(1, 3);
            this.btnProPage.Name = "btnProPage";
            this.btnProPage.Size = new System.Drawing.Size(23, 23);
            this.btnProPage.TabIndex = 9;
            this.btnProPage.Text = "<";
            this.btnProPage.UseVisualStyleBackColor = true;
            // 
            // labelPageInfo
            // 
            this.labelPageInfo.AutoSize = true;
            this.labelPageInfo.Location = new System.Drawing.Point(292, 8);
            this.labelPageInfo.Name = "labelPageInfo";
            this.labelPageInfo.Size = new System.Drawing.Size(71, 12);
            this.labelPageInfo.TabIndex = 16;
            this.labelPageInfo.Text = "第0页/共0页";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(282, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "|";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(164, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "|";
            // 
            // btnGo
            // 
            this.btnGo.AutoSize = true;
            this.btnGo.Location = new System.Drawing.Point(138, 8);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(29, 12);
            this.btnGo.TabIndex = 12;
            this.btnGo.TabStop = true;
            this.btnGo.Text = "跳转";
            this.btnGo.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // numPage
            // 
            this.numPage.AutoSize = true;
            this.numPage.Location = new System.Drawing.Point(55, 3);
            this.numPage.Name = "numPage";
            this.numPage.Size = new System.Drawing.Size(78, 21);
            this.numPage.TabIndex = 11;
            this.numPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(28, 3);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(23, 23);
            this.btnNextPage.TabIndex = 10;
            this.btnNextPage.Text = ">";
            this.btnNextPage.UseVisualStyleBackColor = true;
            // 
            // panelColVisibleSetting
            // 
            this.panelColVisibleSetting.BackColor = System.Drawing.SystemColors.Control;
            this.panelColVisibleSetting.Controls.Add(this.labelTitle);
            this.panelColVisibleSetting.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelColVisibleSetting.Location = new System.Drawing.Point(225, 0);
            this.panelColVisibleSetting.MinimumSize = new System.Drawing.Size(20, 0);
            this.panelColVisibleSetting.Name = "panelColVisibleSetting";
            this.panelColVisibleSetting.Size = new System.Drawing.Size(140, 81);
            this.panelColVisibleSetting.TabIndex = 4;
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(2, 2);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(15, 50);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "显示设置";
            this.labelTitle.Click += new System.EventHandler(this.labelTitle_Click);
            // 
            // cmsColVisibleSetting
            // 
            this.cmsColVisibleSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHidenCol,
            this.tsmiShowModel,
            this.tsmiShowHidenColList,
            this.tsmiHidenHidenColList});
            this.cmsColVisibleSetting.Name = "cmsColVisibleSetting";
            this.cmsColVisibleSetting.Size = new System.Drawing.Size(159, 92);
            // 
            // tsmiHidenCol
            // 
            this.tsmiHidenCol.Name = "tsmiHidenCol";
            this.tsmiHidenCol.Size = new System.Drawing.Size(158, 22);
            this.tsmiHidenCol.Text = "隐藏列";
            this.tsmiHidenCol.Click += new System.EventHandler(this.tsmiHidenCol_Click);
            // 
            // tsmiShowModel
            // 
            this.tsmiShowModel.Name = "tsmiShowModel";
            this.tsmiShowModel.Size = new System.Drawing.Size(158, 22);
            this.tsmiShowModel.Text = "列显示模式";
            this.tsmiShowModel.Visible = false;
            // 
            // tsmiShowHidenColList
            // 
            this.tsmiShowHidenColList.Name = "tsmiShowHidenColList";
            this.tsmiShowHidenColList.Size = new System.Drawing.Size(158, 22);
            this.tsmiShowHidenColList.Text = "显示隐藏列列表";
            this.tsmiShowHidenColList.Click += new System.EventHandler(this.tsmiShowHidenColList_Click);
            // 
            // tsmiHidenHidenColList
            // 
            this.tsmiHidenHidenColList.Name = "tsmiHidenHidenColList";
            this.tsmiHidenHidenColList.Size = new System.Drawing.Size(158, 22);
            this.tsmiHidenHidenColList.Text = "隐藏隐藏列列表";
            this.tsmiHidenHidenColList.Click += new System.EventHandler(this.tsmiHidenHidenColList_Click);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.SystemColors.Highlight;
            this.panelContent.Controls.Add(this.dataGridView);
            this.panelContent.Controls.Add(this.panelColVisibleSetting);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(365, 81);
            this.panelContent.TabIndex = 5;
            // 
            // UCPageGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelPage);
            this.Name = "UCPageGridControl";
            this.Size = new System.Drawing.Size(365, 110);
            this.Load += new System.EventHandler(this.UCPageGridControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panelPage.ResumeLayout(false);
            this.panelPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).EndInit();
            this.panelColVisibleSetting.ResumeLayout(false);
            this.cmsColVisibleSetting.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Panel panelPage;
        private System.Windows.Forms.Label labelRecordInfo;
        private System.Windows.Forms.Button btnProPage;
        private System.Windows.Forms.Label labelPageInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel btnGo;
        private System.Windows.Forms.NumericUpDown numPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Panel panelColVisibleSetting;
        private System.Windows.Forms.ContextMenuStrip cmsColVisibleSetting;
        private System.Windows.Forms.ToolStripMenuItem tsmiHidenCol;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowHidenColList;
        private System.Windows.Forms.ToolStripMenuItem tsmiHidenHidenColList;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowModel;
    }
}
