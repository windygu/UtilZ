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
            this.dataGridView = new UtilZ.Lib.Winform.ZDataGridView();
            this.panelPage = new System.Windows.Forms.Panel();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrePage = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numPageSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numPageIndex = new System.Windows.Forms.NumericUpDown();
            this.panelColVisibleSetting = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.cmsColVisibleSetting = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiHidenCol = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowModel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowHidenColList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHidenHidenColList = new System.Windows.Forms.ToolStripMenuItem();
            this.panelContent = new System.Windows.Forms.Panel();
            this.labelPageCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panelPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPageIndex)).BeginInit();
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
            this.dataGridView.MouseRightButtonChangeSelectedRow = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(225, 85);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.VirtualMode = true;
            // 
            // panelPage
            // 
            this.panelPage.Controls.Add(this.labelPageCount);
            this.panelPage.Controls.Add(this.btnLastPage);
            this.panelPage.Controls.Add(this.btnNextPage);
            this.panelPage.Controls.Add(this.btnPrePage);
            this.panelPage.Controls.Add(this.label4);
            this.panelPage.Controls.Add(this.numPageSize);
            this.panelPage.Controls.Add(this.label2);
            this.panelPage.Controls.Add(this.btnFirstPage);
            this.panelPage.Controls.Add(this.label3);
            this.panelPage.Controls.Add(this.numPageIndex);
            this.panelPage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPage.Location = new System.Drawing.Point(0, 85);
            this.panelPage.Name = "panelPage";
            this.panelPage.Size = new System.Drawing.Size(365, 25);
            this.panelPage.TabIndex = 3;
            // 
            // btnLastPage
            // 
            this.btnLastPage.BackgroundImage = global::UtilZ.Lib.Winform.Properties.Resources.lastPage;
            this.btnLastPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLastPage.FlatAppearance.BorderSize = 0;
            this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLastPage.Location = new System.Drawing.Point(240, 4);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(15, 15);
            this.btnLastPage.TabIndex = 20;
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.BackgroundImage = global::UtilZ.Lib.Winform.Properties.Resources.nextPage;
            this.btnNextPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextPage.FlatAppearance.BorderSize = 0;
            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextPage.Location = new System.Drawing.Point(223, 4);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(15, 15);
            this.btnNextPage.TabIndex = 19;
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrePage
            // 
            this.btnPrePage.BackgroundImage = global::UtilZ.Lib.Winform.Properties.Resources.prePage;
            this.btnPrePage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrePage.FlatAppearance.BorderSize = 0;
            this.btnPrePage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrePage.Location = new System.Drawing.Point(126, 4);
            this.btnPrePage.Name = "btnPrePage";
            this.btnPrePage.Size = new System.Drawing.Size(15, 15);
            this.btnPrePage.TabIndex = 18;
            this.btnPrePage.UseVisualStyleBackColor = true;
            this.btnPrePage.Click += new System.EventHandler(this.btnPrePage_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "条";
            // 
            // numPageSize
            // 
            this.numPageSize.AutoSize = true;
            this.numPageSize.Location = new System.Drawing.Point(31, 1);
            this.numPageSize.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numPageSize.Name = "numPageSize";
            this.numPageSize.Size = new System.Drawing.Size(57, 21);
            this.numPageSize.TabIndex = 17;
            this.numPageSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPageSize.ValueChanged += new System.EventHandler(this.numPageSize_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "每页";
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.BackgroundImage = global::UtilZ.Lib.Winform.Properties.Resources.firstPage;
            this.btnFirstPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFirstPage.FlatAppearance.BorderSize = 0;
            this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirstPage.Location = new System.Drawing.Point(109, 4);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(15, 15);
            this.btnFirstPage.TabIndex = 9;
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(99, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "|";
            // 
            // numPageIndex
            // 
            this.numPageIndex.AutoSize = true;
            this.numPageIndex.Location = new System.Drawing.Point(144, 1);
            this.numPageIndex.Name = "numPageIndex";
            this.numPageIndex.Size = new System.Drawing.Size(78, 21);
            this.numPageIndex.TabIndex = 11;
            this.numPageIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPageIndex.ValueChanged += new System.EventHandler(this.numPageIndex_ValueChanged);
            // 
            // panelColVisibleSetting
            // 
            this.panelColVisibleSetting.BackColor = System.Drawing.SystemColors.Control;
            this.panelColVisibleSetting.Controls.Add(this.labelTitle);
            this.panelColVisibleSetting.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelColVisibleSetting.Location = new System.Drawing.Point(225, 0);
            this.panelColVisibleSetting.MinimumSize = new System.Drawing.Size(20, 0);
            this.panelColVisibleSetting.Name = "panelColVisibleSetting";
            this.panelColVisibleSetting.Size = new System.Drawing.Size(140, 85);
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
            this.panelContent.Size = new System.Drawing.Size(365, 85);
            this.panelContent.TabIndex = 5;
            // 
            // labelPageCount
            // 
            this.labelPageCount.AutoSize = true;
            this.labelPageCount.Location = new System.Drawing.Point(261, 5);
            this.labelPageCount.Name = "labelPageCount";
            this.labelPageCount.Size = new System.Drawing.Size(23, 12);
            this.labelPageCount.TabIndex = 21;
            this.labelPageCount.Text = "0页";
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
            ((System.ComponentModel.ISupportInitialize)(this.numPageSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPageIndex)).EndInit();
            this.panelColVisibleSetting.ResumeLayout(false);
            this.cmsColVisibleSetting.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ZDataGridView dataGridView;
        private System.Windows.Forms.Panel panelPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numPageIndex;
        private System.Windows.Forms.Panel panelColVisibleSetting;
        private System.Windows.Forms.ContextMenuStrip cmsColVisibleSetting;
        private System.Windows.Forms.ToolStripMenuItem tsmiHidenCol;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowHidenColList;
        private System.Windows.Forms.ToolStripMenuItem tsmiHidenHidenColList;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowModel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numPageSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrePage;
        private System.Windows.Forms.Label labelPageCount;
    }
}
