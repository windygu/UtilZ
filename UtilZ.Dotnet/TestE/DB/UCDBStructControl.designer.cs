namespace TestE.DB
{
    partial class UCDBStructControl
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
            this.dgvTables = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvTableFields = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.txtTableFilter = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.comboBoxDB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTableFieldFilter = new System.Windows.Forms.TextBox();
            this.checkMultTable = new System.Windows.Forms.CheckBox();
            this.cmsCheck = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAllCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUnCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClearCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGenerateModel = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cmsCheck.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTables
            // 
            this.dgvTables.AlignDirection = true;
            this.dgvTables.ColumnSettingStatus = UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Hiden;
            this.dgvTables.ColumnSettingWidth = 20;
            this.dgvTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTables.EnableColumnHeaderContextMenuStripHiden = true;
            this.dgvTables.EnablePagingBar = false;
            this.dgvTables.EnableRowNum = true;
            this.dgvTables.EnableUserAdjustPageSize = true;
            this.dgvTables.FocusedRowIndex = -1;
            this.dgvTables.IsLastColumnAutoSizeModeFill = true;
            this.dgvTables.Location = new System.Drawing.Point(0, 0);
            this.dgvTables.Name = "dgvTables";
            this.dgvTables.PageSizeMaximum = 100;
            this.dgvTables.PageSizeMinimum = 1;
            this.dgvTables.Size = new System.Drawing.Size(605, 692);
            this.dgvTables.TabIndex = 0;
            this.dgvTables.DataRowDoubleClick += new System.EventHandler<UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.DataRowClickArgs>(this.dgvTables_DataRowDoubleClick);
            this.dgvTables.DataRowSelectionChanged += new System.EventHandler<UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.DataRowSelectionChangedArgs>(this.dgvTables_DataRowSelectionChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(20, 72);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvTables);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvTableFields);
            this.splitContainer1.Size = new System.Drawing.Size(1360, 692);
            this.splitContainer1.SplitterDistance = 605;
            this.splitContainer1.TabIndex = 4;
            // 
            // dgvTableFields
            // 
            this.dgvTableFields.AlignDirection = true;
            this.dgvTableFields.ColumnSettingStatus = UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Hiden;
            this.dgvTableFields.ColumnSettingWidth = 20;
            this.dgvTableFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTableFields.EnableColumnHeaderContextMenuStripHiden = true;
            this.dgvTableFields.EnablePagingBar = false;
            this.dgvTableFields.EnableRowNum = true;
            this.dgvTableFields.EnableUserAdjustPageSize = true;
            this.dgvTableFields.FocusedRowIndex = -1;
            this.dgvTableFields.IsLastColumnAutoSizeModeFill = true;
            this.dgvTableFields.Location = new System.Drawing.Point(0, 0);
            this.dgvTableFields.Name = "dgvTableFields";
            this.dgvTableFields.PageSizeMaximum = 100;
            this.dgvTableFields.PageSizeMinimum = 1;
            this.dgvTableFields.Size = new System.Drawing.Size(751, 692);
            this.dgvTableFields.TabIndex = 1;
            // 
            // txtTableFilter
            // 
            this.txtTableFilter.Location = new System.Drawing.Point(67, 45);
            this.txtTableFilter.Name = "txtTableFilter";
            this.txtTableFilter.Size = new System.Drawing.Size(256, 21);
            this.txtTableFilter.TabIndex = 6;
            this.txtTableFilter.TextChanged += new System.EventHandler(this.txtTableFilter_TextChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(338, 11);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // comboBoxDB
            // 
            this.comboBoxDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDB.FormattingEnabled = true;
            this.comboBoxDB.Location = new System.Drawing.Point(67, 13);
            this.comboBoxDB.Name = "comboBoxDB";
            this.comboBoxDB.Size = new System.Drawing.Size(256, 20);
            this.comboBoxDB.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "数据库";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "过滤";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1068, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "过滤";
            // 
            // txtTableFieldFilter
            // 
            this.txtTableFieldFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTableFieldFilter.Location = new System.Drawing.Point(1103, 40);
            this.txtTableFieldFilter.Name = "txtTableFieldFilter";
            this.txtTableFieldFilter.Size = new System.Drawing.Size(256, 21);
            this.txtTableFieldFilter.TabIndex = 10;
            this.txtTableFieldFilter.TextChanged += new System.EventHandler(this.txtTableFieldFilter_TextChanged);
            // 
            // checkMultTable
            // 
            this.checkMultTable.AutoSize = true;
            this.checkMultTable.Location = new System.Drawing.Point(338, 47);
            this.checkMultTable.Name = "checkMultTable";
            this.checkMultTable.Size = new System.Drawing.Size(48, 16);
            this.checkMultTable.TabIndex = 12;
            this.checkMultTable.Text = "多表";
            this.checkMultTable.UseVisualStyleBackColor = true;
            this.checkMultTable.CheckedChanged += new System.EventHandler(this.checkBoxAllTableField_CheckedChanged);
            // 
            // cmsCheck
            // 
            this.cmsCheck.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAllCheck,
            this.tsmiUnCheck,
            this.tsmiClearCheck,
            this.tsmiGenerateModel});
            this.cmsCheck.Name = "cmsCheck";
            this.cmsCheck.Size = new System.Drawing.Size(153, 114);
            // 
            // tsmiAllCheck
            // 
            this.tsmiAllCheck.Name = "tsmiAllCheck";
            this.tsmiAllCheck.Size = new System.Drawing.Size(152, 22);
            this.tsmiAllCheck.Text = "全选";
            this.tsmiAllCheck.Click += new System.EventHandler(this.tsmiAllCheck_Click);
            // 
            // tsmiUnCheck
            // 
            this.tsmiUnCheck.Name = "tsmiUnCheck";
            this.tsmiUnCheck.Size = new System.Drawing.Size(152, 22);
            this.tsmiUnCheck.Text = "反选";
            this.tsmiUnCheck.Click += new System.EventHandler(this.tsmiUnCheck_Click);
            // 
            // tsmiClearCheck
            // 
            this.tsmiClearCheck.Name = "tsmiClearCheck";
            this.tsmiClearCheck.Size = new System.Drawing.Size(152, 22);
            this.tsmiClearCheck.Text = "清除";
            this.tsmiClearCheck.Click += new System.EventHandler(this.tsmiClearCheck_Click);
            // 
            // tsmiGenerateModel
            // 
            this.tsmiGenerateModel.Name = "tsmiGenerateModel";
            this.tsmiGenerateModel.Size = new System.Drawing.Size(152, 22);
            this.tsmiGenerateModel.Text = "生成模型";
            this.tsmiGenerateModel.Click += new System.EventHandler(this.tsmiGenerateModel_Click);
            // 
            // UCDBStructControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkMultTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTableFieldFilter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxDB);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.txtTableFilter);
            this.Controls.Add(this.btnLoad);
            this.Name = "UCDBStructControl";
            this.Size = new System.Drawing.Size(1400, 800);
            this.Load += new System.EventHandler(this.UCDBStructControl_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.cmsCheck.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl dgvTables;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl dgvTableFields;
        private System.Windows.Forms.TextBox txtTableFilter;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ComboBox comboBoxDB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTableFieldFilter;
        private System.Windows.Forms.CheckBox checkMultTable;
        private System.Windows.Forms.ContextMenuStrip cmsCheck;
        private System.Windows.Forms.ToolStripMenuItem tsmiAllCheck;
        private System.Windows.Forms.ToolStripMenuItem tsmiUnCheck;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearCheck;
        private System.Windows.Forms.ToolStripMenuItem tsmiGenerateModel;
    }
}
