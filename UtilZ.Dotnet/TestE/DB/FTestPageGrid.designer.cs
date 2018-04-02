namespace TestE.DB
{
    partial class FTestPageGrid
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
            this.label1 = new System.Windows.Forms.Label();
            this.comDB = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSwitchPageSize = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.ucPageGridControl1 = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.btnColSettingVisible = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 41;
            this.label1.Text = "数据库";
            // 
            // comDB
            // 
            this.comDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comDB.FormattingEnabled = true;
            this.comDB.Location = new System.Drawing.Point(65, 12);
            this.comDB.Name = "comDB";
            this.comDB.Size = new System.Drawing.Size(114, 20);
            this.comDB.TabIndex = 40;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(195, 12);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 43;
            this.btnTest.Text = "Query";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnSwitchPageSize
            // 
            this.btnSwitchPageSize.Location = new System.Drawing.Point(287, 12);
            this.btnSwitchPageSize.Name = "btnSwitchPageSize";
            this.btnSwitchPageSize.Size = new System.Drawing.Size(75, 23);
            this.btnSwitchPageSize.TabIndex = 44;
            this.btnSwitchPageSize.Text = "SwitchPageSize";
            this.btnSwitchPageSize.UseVisualStyleBackColor = true;
            this.btnSwitchPageSize.Click += new System.EventHandler(this.btnSwitchPageSize_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.ucPageGridControl1);
            this.groupBox1.Location = new System.Drawing.Point(20, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(557, 377);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "分页表格控件";
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(7, 357);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(46, 16);
            this.checkBox1.TabIndex = 43;
            this.checkBox1.Text = "选择";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // ucPageGridControl1
            // 
            this.ucPageGridControl1.AdvanceSettingVisible = true;
            this.ucPageGridControl1.AlignDirection = false;
            this.ucPageGridControl1.ColumnSettingStatus = UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Hiden;
            this.ucPageGridControl1.ColumnSettingVisible = false;
            this.ucPageGridControl1.ColumnSettingWidth = 20;
            this.ucPageGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPageGridControl1.FocusedRowIndex = -1;
            this.ucPageGridControl1.IsLastColumnAutoSizeModeFill = true;
            this.ucPageGridControl1.Location = new System.Drawing.Point(3, 17);
            this.ucPageGridControl1.Name = "ucPageGridControl1";
            this.ucPageGridControl1.PageSizeMaximum = 100;
            this.ucPageGridControl1.PageSizeVisible = true;
            this.ucPageGridControl1.PagingVisible = true;
            this.ucPageGridControl1.RowNumVisible = true;
            this.ucPageGridControl1.Size = new System.Drawing.Size(551, 357);
            this.ucPageGridControl1.TabIndex = 42;
            this.ucPageGridControl1.QueryData += new System.EventHandler<UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.Interface.QueryDataArgs>(this.ucPageGridControl1_QueryData);
            this.ucPageGridControl1.PageSizeChanged += new System.EventHandler<UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.Interface.PageSizeChangedArgs>(this.ucPageGridControl1_PageSizeChanged);
            // 
            // btnColSettingVisible
            // 
            this.btnColSettingVisible.Location = new System.Drawing.Point(368, 12);
            this.btnColSettingVisible.Name = "btnColSettingVisible";
            this.btnColSettingVisible.Size = new System.Drawing.Size(75, 23);
            this.btnColSettingVisible.TabIndex = 46;
            this.btnColSettingVisible.Text = "ColSettingVisible";
            this.btnColSettingVisible.UseVisualStyleBackColor = true;
            this.btnColSettingVisible.Click += new System.EventHandler(this.btnColSettingVisible_Click);
            // 
            // FTestPageGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 447);
            this.Controls.Add(this.btnColSettingVisible);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSwitchPageSize);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comDB);
            this.Name = "FTestPageGrid";
            this.Text = "FTestPageGrid";
            this.Load += new System.EventHandler(this.FTestPageGrid_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comDB;
        private UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl ucPageGridControl1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnSwitchPageSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnColSettingVisible;
    }
}