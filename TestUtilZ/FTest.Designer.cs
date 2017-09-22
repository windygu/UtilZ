namespace TestUtilZ
{
    partial class FTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ucPageGridControl1 = new UtilZ.Lib.Winform.PageGrid.UCPageGridControl();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(37, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(131, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ucPageGridControl1
            // 
            this.ucPageGridControl1.AdvanceSettingVisible = true;
            this.ucPageGridControl1.ColumnSettingStatus = UtilZ.Lib.Winform.PageGrid.PageGridColumnSettingStatus.Hiden;
            this.ucPageGridControl1.ColumnSettingVisible = true;
            this.ucPageGridControl1.ColumnSettingWidth = 20;
            this.ucPageGridControl1.FocusedRowIndex = -1;
            this.ucPageGridControl1.IsLastColumnAutoSizeModeFill = false;
            this.ucPageGridControl1.Location = new System.Drawing.Point(12, 63);
            this.ucPageGridControl1.Name = "ucPageGridControl1";
            this.ucPageGridControl1.PagingVisible = true;
            this.ucPageGridControl1.RowNumVisible = true;
            this.ucPageGridControl1.Size = new System.Drawing.Size(596, 340);
            this.ucPageGridControl1.TabIndex = 1;
            this.ucPageGridControl1.QueryData += new System.EventHandler<UtilZ.Lib.Winform.PageGrid.Interface.QueryDataArgs>(this.ucPageGridControl1_QueryData);
            this.ucPageGridControl1.PageSizeChanged += new System.EventHandler<UtilZ.Lib.Winform.PageGrid.Interface.PageSizeChangedArgs>(this.ucPageGridControl1_PageSizeChanged);
            // 
            // FTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(620, 415);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ucPageGridControl1);
            this.Controls.Add(this.button1);
            this.Name = "FTest";
            this.Text = "FTest";
            this.Load += new System.EventHandler(this.FTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private UtilZ.Lib.Winform.PageGrid.UCPageGridControl ucPageGridControl1;
        private System.Windows.Forms.Button button2;
    }
}