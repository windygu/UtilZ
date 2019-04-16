namespace TestE.DB
{
    partial class FTableField
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
            this.dgvTableFields = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTableFieldFilter = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // dgvTableFields
            // 
            this.dgvTableFields.AlignDirection = true;
            this.dgvTableFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTableFields.ColumnSettingStatus = UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Hiden;
            this.dgvTableFields.ColumnSettingWidth = 20;
            this.dgvTableFields.EnableColumnHeaderContextMenuStripHiden = true;
            this.dgvTableFields.EnablePagingBar = false;
            this.dgvTableFields.EnableRowNum = true;
            this.dgvTableFields.EnableUserAdjustPageSize = true;
            this.dgvTableFields.FocusedRowIndex = -1;
            this.dgvTableFields.IsLastColumnAutoSizeModeFill = true;
            this.dgvTableFields.Location = new System.Drawing.Point(12, 39);
            this.dgvTableFields.Name = "dgvTableFields";
            this.dgvTableFields.PageSizeMaximum = 100;
            this.dgvTableFields.PageSizeMinimum = 1;
            this.dgvTableFields.Size = new System.Drawing.Size(660, 511);
            this.dgvTableFields.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "过滤";
            // 
            // txtTableFieldFilter
            // 
            this.txtTableFieldFilter.Location = new System.Drawing.Point(53, 12);
            this.txtTableFieldFilter.Name = "txtTableFieldFilter";
            this.txtTableFieldFilter.Size = new System.Drawing.Size(256, 21);
            this.txtTableFieldFilter.TabIndex = 13;
            this.txtTableFieldFilter.TextChanged += new System.EventHandler(this.txtTableFieldFilter_TextChanged);
            // 
            // FTableField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 562);
            this.Controls.Add(this.dgvTableFields);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTableFieldFilter);
            this.Name = "FTableField";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FTableField";
            this.Load += new System.EventHandler(this.FTableField_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl dgvTableFields;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTableFieldFilter;
    }
}