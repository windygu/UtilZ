namespace TestUtilZDB
{
    partial class FTest
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
            this.btnQueryData = new System.Windows.Forms.Button();
            this.btnQueryPage = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnDBInfo = new System.Windows.Forms.Button();
            this.btnStoredProc = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnAtom = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comDB = new System.Windows.Forms.ComboBox();
            this.pageGridControl1 = new UtilZ.Lib.WinformEx.PageGrid.PageGridControl();
            this.SuspendLayout();
            // 
            // btnQueryData
            // 
            this.btnQueryData.Location = new System.Drawing.Point(136, 144);
            this.btnQueryData.Name = "btnQueryData";
            this.btnQueryData.Size = new System.Drawing.Size(75, 23);
            this.btnQueryData.TabIndex = 48;
            this.btnQueryData.Text = "QueryData";
            this.btnQueryData.UseVisualStyleBackColor = true;
            this.btnQueryData.Click += new System.EventHandler(this.btnQueryData_Click);
            // 
            // btnQueryPage
            // 
            this.btnQueryPage.Location = new System.Drawing.Point(54, 144);
            this.btnQueryPage.Name = "btnQueryPage";
            this.btnQueryPage.Size = new System.Drawing.Size(75, 23);
            this.btnQueryPage.TabIndex = 47;
            this.btnQueryPage.Text = "QueryPage";
            this.btnQueryPage.UseVisualStyleBackColor = true;
            this.btnQueryPage.Click += new System.EventHandler(this.btnQueryPage_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(136, 113);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 46;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(54, 114);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 45;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(54, 202);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(156, 172);
            this.listBox1.TabIndex = 44;
            // 
            // btnDBInfo
            // 
            this.btnDBInfo.Location = new System.Drawing.Point(136, 84);
            this.btnDBInfo.Name = "btnDBInfo";
            this.btnDBInfo.Size = new System.Drawing.Size(75, 23);
            this.btnDBInfo.TabIndex = 43;
            this.btnDBInfo.Text = "DBInfo";
            this.btnDBInfo.UseVisualStyleBackColor = true;
            this.btnDBInfo.Click += new System.EventHandler(this.btnDBInfo_Click);
            // 
            // btnStoredProc
            // 
            this.btnStoredProc.Location = new System.Drawing.Point(54, 84);
            this.btnStoredProc.Name = "btnStoredProc";
            this.btnStoredProc.Size = new System.Drawing.Size(75, 23);
            this.btnStoredProc.TabIndex = 42;
            this.btnStoredProc.Text = "StoredProc";
            this.btnStoredProc.UseVisualStyleBackColor = true;
            this.btnStoredProc.Click += new System.EventHandler(this.btnStoredProc_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(135, 54);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 41;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnAtom
            // 
            this.btnAtom.Location = new System.Drawing.Point(54, 54);
            this.btnAtom.Name = "btnAtom";
            this.btnAtom.Size = new System.Drawing.Size(75, 23);
            this.btnAtom.TabIndex = 40;
            this.btnAtom.Text = "Atom";
            this.btnAtom.UseVisualStyleBackColor = true;
            this.btnAtom.Click += new System.EventHandler(this.btnAtom_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 39;
            this.label1.Text = "数据库";
            // 
            // comDB
            // 
            this.comDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comDB.FormattingEnabled = true;
            this.comDB.Location = new System.Drawing.Point(54, 17);
            this.comDB.Name = "comDB";
            this.comDB.Size = new System.Drawing.Size(114, 20);
            this.comDB.TabIndex = 38;
            // 
            // pageGridControl1
            // 
            this.pageGridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageGridControl1.BackColor = System.Drawing.SystemColors.Control;
            this.pageGridControl1.ColumnSettingVisible = true;
            this.pageGridControl1.DataReadOnly = true;
            this.pageGridControl1.GridContextMenuStrip = null;
            this.pageGridControl1.Location = new System.Drawing.Point(304, 22);
            this.pageGridControl1.MinimumSize = new System.Drawing.Size(30, 30);
            this.pageGridControl1.MuiltSelect = true;
            this.pageGridControl1.Name = "pageGridControl1";
            this.pageGridControl1.PagingVisible = true;
            this.pageGridControl1.Size = new System.Drawing.Size(481, 352);
            this.pageGridControl1.TabIndex = 49;
            // 
            // FTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 450);
            this.Controls.Add(this.pageGridControl1);
            this.Controls.Add(this.btnQueryData);
            this.Controls.Add(this.btnQueryPage);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnDBInfo);
            this.Controls.Add(this.btnStoredProc);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.btnAtom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comDB);
            this.Name = "FTest";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FTest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQueryData;
        private System.Windows.Forms.Button btnQueryPage;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnDBInfo;
        private System.Windows.Forms.Button btnStoredProc;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnAtom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comDB;
        private UtilZ.Lib.WinformEx.PageGrid.PageGridControl pageGridControl1;
    }
}

