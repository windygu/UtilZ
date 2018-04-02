namespace UtilZ.Dotnet.IWindowEx.Winform.PageGrid
{
    partial class FColumnDisplaySetting
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
            this.listBoxSettingCols = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxSettingCols
            // 
            this.listBoxSettingCols.AllowDrop = true;
            this.listBoxSettingCols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSettingCols.FormattingEnabled = true;
            this.listBoxSettingCols.ItemHeight = 12;
            this.listBoxSettingCols.Location = new System.Drawing.Point(0, 0);
            this.listBoxSettingCols.Name = "listBoxSettingCols";
            this.listBoxSettingCols.Size = new System.Drawing.Size(284, 262);
            this.listBoxSettingCols.TabIndex = 0;
            // 
            // FColumnDisplaySetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.listBoxSettingCols);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FColumnDisplaySetting";
            this.Text = "列显示设置";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxSettingCols;
    }
}