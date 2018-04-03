namespace UtilZ.Dotnet.ILWindowEx.Winform.PageGrid
{
    partial class FGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FGrid));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.panelPage = new System.Windows.Forms.Panel();
            this.labelRecordInfo = new System.Windows.Forms.Label();
            this.btnProPage = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList();
            this.labelPageInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.LinkLabel();
            this.numPage = new System.Windows.Forms.NumericUpDown();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiVisibleCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopyRow = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panelPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
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
            this.dataGridView.Size = new System.Drawing.Size(434, 117);
            this.dataGridView.TabIndex = 0;
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
            this.panelPage.Location = new System.Drawing.Point(0, 117);
            this.panelPage.Name = "panelPage";
            this.panelPage.Size = new System.Drawing.Size(434, 29);
            this.panelPage.TabIndex = 1;
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
            this.btnProPage.ImageList = this.imageList;
            this.btnProPage.Location = new System.Drawing.Point(1, 3);
            this.btnProPage.Name = "btnProPage";
            this.btnProPage.Size = new System.Drawing.Size(23, 23);
            this.btnProPage.TabIndex = 9;
            this.btnProPage.Text = "<";
            this.btnProPage.UseVisualStyleBackColor = true;
            this.btnProPage.Click += new System.EventHandler(this.btnProPage_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "next.png");
            this.imageList.Images.SetKeyName(1, "pre.png");
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
            this.btnGo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnGo_LinkClicked);
            // 
            // numPage
            // 
            this.numPage.Location = new System.Drawing.Point(55, 3);
            this.numPage.Name = "numPage";
            this.numPage.Size = new System.Drawing.Size(78, 21);
            this.numPage.TabIndex = 11;
            this.numPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numPage_KeyDown);
            // 
            // btnNextPage
            // 
            this.btnNextPage.ImageList = this.imageList;
            this.btnNextPage.Location = new System.Drawing.Point(28, 3);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(23, 23);
            this.btnNextPage.TabIndex = 10;
            this.btnNextPage.Text = ">";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopy,
            this.tsmiVisibleCopy,
            this.tsmiCopyRow});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(182, 92);
            this.contextMenuStrip.Opening += this.contextMenuStrip_Opening;
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Name = "tsmiCopy";
            this.tsmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tsmiCopy.Size = new System.Drawing.Size(181, 22);
            this.tsmiCopy.Text = "复制单元格";
            this.tsmiCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
            // 
            // tsmiVisibleCopy
            // 
            this.tsmiVisibleCopy.Name = "tsmiVisibleCopy";
            this.tsmiVisibleCopy.Size = new System.Drawing.Size(181, 22);
            this.tsmiVisibleCopy.Text = "复制行";
            this.tsmiVisibleCopy.Click += new System.EventHandler(this.tsmiVisibleCopy_Click);
            // 
            // tsmiCopyRow
            // 
            this.tsmiCopyRow.Name = "tsmiCopyRow";
            this.tsmiCopyRow.Size = new System.Drawing.Size(181, 22);
            this.tsmiCopyRow.Text = "复制整行";
            this.tsmiCopyRow.Click += new System.EventHandler(this.tsmiCopyRow_Click);
            // 
            // FGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 146);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panelPage);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MinimumSize = new System.Drawing.Size(425, 38);
            this.Name = "FGrid";
            this.Text = "数据显示表格";
            this.Load += new System.EventHandler(this.FGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panelPage.ResumeLayout(false);
            this.panelPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Panel panelPage;
        private System.Windows.Forms.Label labelRecordInfo;
        private System.Windows.Forms.Button btnProPage;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Label labelPageInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel btnGo;
        private System.Windows.Forms.NumericUpDown numPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiVisibleCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyRow;
    }
}