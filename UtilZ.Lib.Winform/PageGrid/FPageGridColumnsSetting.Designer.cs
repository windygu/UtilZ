namespace UtilZ.Lib.Winform.PageGrid
{
    partial class FPageGridColumnsSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listBoxCol = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxMenu = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiHiden = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFloat = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDock = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpHidenCol = new System.Windows.Forms.TabPage();
            this.tpColSetting = new System.Windows.Forms.TabPage();
            this.dgvColumnSetting = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMenu)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpHidenCol.SuspendLayout();
            this.tpColSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxCol
            // 
            this.listBoxCol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCol.FormattingEnabled = true;
            this.listBoxCol.ItemHeight = 12;
            this.listBoxCol.Location = new System.Drawing.Point(3, 3);
            this.listBoxCol.Name = "listBoxCol";
            this.listBoxCol.Size = new System.Drawing.Size(182, 140);
            this.listBoxCol.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "显示设置";
            // 
            // pictureBoxMenu
            // 
            this.pictureBoxMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxMenu.Image = global::UtilZ.Lib.Winform.Properties.Resources.Dropdown;
            this.pictureBoxMenu.Location = new System.Drawing.Point(182, 6);
            this.pictureBoxMenu.Name = "pictureBoxMenu";
            this.pictureBoxMenu.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxMenu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMenu.TabIndex = 2;
            this.pictureBoxMenu.TabStop = false;
            this.pictureBoxMenu.Click += new System.EventHandler(this.pictureBoxMenu_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHiden,
            this.tsmiFloat,
            this.tsmiDock,
            this.tsmiSave});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(99, 92);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // tsmiHiden
            // 
            this.tsmiHiden.Name = "tsmiHiden";
            this.tsmiHiden.Size = new System.Drawing.Size(98, 22);
            this.tsmiHiden.Text = "隐藏";
            this.tsmiHiden.Click += new System.EventHandler(this.tsmiHiden_Click);
            // 
            // tsmiFloat
            // 
            this.tsmiFloat.Name = "tsmiFloat";
            this.tsmiFloat.Size = new System.Drawing.Size(98, 22);
            this.tsmiFloat.Text = "浮动";
            this.tsmiFloat.Click += new System.EventHandler(this.tsmiFloat_Click);
            // 
            // tsmiDock
            // 
            this.tsmiDock.Name = "tsmiDock";
            this.tsmiDock.Size = new System.Drawing.Size(98, 22);
            this.tsmiDock.Text = "停靠";
            this.tsmiDock.Click += new System.EventHandler(this.tsmiDock_Click);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.Size = new System.Drawing.Size(98, 22);
            this.tsmiSave.Text = "保存";
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpHidenCol);
            this.tabControl1.Controls.Add(this.tpColSetting);
            this.tabControl1.Location = new System.Drawing.Point(2, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(196, 172);
            this.tabControl1.TabIndex = 3;
            // 
            // tpHidenCol
            // 
            this.tpHidenCol.Controls.Add(this.listBoxCol);
            this.tpHidenCol.Location = new System.Drawing.Point(4, 22);
            this.tpHidenCol.Name = "tpHidenCol";
            this.tpHidenCol.Padding = new System.Windows.Forms.Padding(3);
            this.tpHidenCol.Size = new System.Drawing.Size(188, 146);
            this.tpHidenCol.TabIndex = 0;
            this.tpHidenCol.Text = "隐藏列";
            this.tpHidenCol.UseVisualStyleBackColor = true;
            // 
            // tpColSetting
            // 
            this.tpColSetting.Controls.Add(this.dgvColumnSetting);
            this.tpColSetting.Location = new System.Drawing.Point(4, 22);
            this.tpColSetting.Name = "tpColSetting";
            this.tpColSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tpColSetting.Size = new System.Drawing.Size(188, 146);
            this.tpColSetting.TabIndex = 1;
            this.tpColSetting.Text = "高级";
            this.tpColSetting.UseVisualStyleBackColor = true;
            // 
            // dgvColumnSetting
            // 
            this.dgvColumnSetting.AllowUserToAddRows = false;
            this.dgvColumnSetting.AllowUserToDeleteRows = false;
            this.dgvColumnSetting.AllowUserToResizeRows = false;
            this.dgvColumnSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumnSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvColumnSetting.Location = new System.Drawing.Point(3, 3);
            this.dgvColumnSetting.MultiSelect = false;
            this.dgvColumnSetting.Name = "dgvColumnSetting";
            this.dgvColumnSetting.RowHeadersVisible = false;
            this.dgvColumnSetting.RowTemplate.Height = 23;
            this.dgvColumnSetting.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvColumnSetting.Size = new System.Drawing.Size(182, 140);
            this.dgvColumnSetting.TabIndex = 0;
            // 
            // FPageGridColumnsSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(200, 200);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBoxMenu);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPageGridColumnsSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "显示列设置";
            this.Load += new System.EventHandler(this.FPageGridColumnsSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMenu)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpHidenCol.ResumeLayout(false);
            this.tpColSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnSetting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxCol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxMenu;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiHiden;
        private System.Windows.Forms.ToolStripMenuItem tsmiFloat;
        private System.Windows.Forms.ToolStripMenuItem tsmiDock;
        private System.Windows.Forms.ToolStripMenuItem tsmiSave;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpHidenCol;
        private System.Windows.Forms.TabPage tpColSetting;
        private System.Windows.Forms.DataGridView dgvColumnSetting;
    }
}