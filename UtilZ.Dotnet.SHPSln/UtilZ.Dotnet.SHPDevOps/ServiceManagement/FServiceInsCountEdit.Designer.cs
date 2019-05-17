namespace UtilZ.Dotnet.SHPDevOps.ServiceManagement
{
    partial class FServiceInsCountEdit
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
            this.numServiceInsCount = new System.Windows.Forms.NumericUpDown();
            this.btnCancell = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numServiceInsCount)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务实例个数";
            // 
            // numServiceInsCount
            // 
            this.numServiceInsCount.Location = new System.Drawing.Point(96, 22);
            this.numServiceInsCount.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numServiceInsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numServiceInsCount.Name = "numServiceInsCount";
            this.numServiceInsCount.Size = new System.Drawing.Size(220, 21);
            this.numServiceInsCount.TabIndex = 1;
            this.numServiceInsCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnCancell
            // 
            this.btnCancell.Location = new System.Drawing.Point(241, 49);
            this.btnCancell.Name = "btnCancell";
            this.btnCancell.Size = new System.Drawing.Size(75, 23);
            this.btnCancell.TabIndex = 3;
            this.btnCancell.Text = "取消";
            this.btnCancell.UseVisualStyleBackColor = true;
            this.btnCancell.Click += new System.EventHandler(this.btnCancell_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(160, 49);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FServiceInsCountEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 80);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancell);
            this.Controls.Add(this.numServiceInsCount);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FServiceInsCountEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "部署服务实例信息";
            ((System.ComponentModel.ISupportInitialize)(this.numServiceInsCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numServiceInsCount;
        private System.Windows.Forms.Button btnCancell;
        private System.Windows.Forms.Button btnOk;
    }
}