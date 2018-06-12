namespace TestE.Common
{
    partial class FTestAsynQueue
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnPro = new System.Windows.Forms.Button();
            this.btnCons = new System.Windows.Forms.Button();
            this.checkBoxStopCons = new System.Windows.Forms.CheckBox();
            this.logControl1 = new UtilZ.Dotnet.WindowEx.Winform.Controls.LogControl_bk();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(44, 59);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 0;
            // 
            // btnPro
            // 
            this.btnPro.Location = new System.Drawing.Point(44, 30);
            this.btnPro.Name = "btnPro";
            this.btnPro.Size = new System.Drawing.Size(75, 23);
            this.btnPro.TabIndex = 1;
            this.btnPro.Text = " 生产";
            this.btnPro.UseVisualStyleBackColor = true;
            this.btnPro.Click += new System.EventHandler(this.btnPro_Click);
            // 
            // btnCons
            // 
            this.btnCons.Location = new System.Drawing.Point(146, 29);
            this.btnCons.Name = "btnCons";
            this.btnCons.Size = new System.Drawing.Size(75, 23);
            this.btnCons.TabIndex = 2;
            this.btnCons.Text = "消费";
            this.btnCons.UseVisualStyleBackColor = true;
            this.btnCons.Click += new System.EventHandler(this.btnCons_Click);
            // 
            // checkBoxStopCons
            // 
            this.checkBoxStopCons.AutoSize = true;
            this.checkBoxStopCons.Location = new System.Drawing.Point(247, 33);
            this.checkBoxStopCons.Name = "checkBoxStopCons";
            this.checkBoxStopCons.Size = new System.Drawing.Size(132, 16);
            this.checkBoxStopCons.TabIndex = 3;
            this.checkBoxStopCons.Text = "消费者终止线程停止";
            this.checkBoxStopCons.UseVisualStyleBackColor = true;
            // 
            // logControl1
            // 
            this.logControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logControl1.IsLock = false;
            this.logControl1.Location = new System.Drawing.Point(44, 87);
            this.logControl1.MaxItemCount = 100;
            this.logControl1.Name = "logControl1";
            this.logControl1.Size = new System.Drawing.Size(467, 351);
            this.logControl1.TabIndex = 4;
            // 
            // FTestAsynQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 450);
            this.Controls.Add(this.logControl1);
            this.Controls.Add(this.checkBoxStopCons);
            this.Controls.Add(this.btnCons);
            this.Controls.Add(this.btnPro);
            this.Controls.Add(this.textBox1);
            this.Name = "FTestAsynQueue";
            this.Text = "FTestAsynQueue";
            this.Load += new System.EventHandler(this.FTestAsynQueue_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnPro;
        private System.Windows.Forms.Button btnCons;
        private System.Windows.Forms.CheckBox checkBoxStopCons;
        private UtilZ.Dotnet.WindowEx.Winform.Controls.LogControl_bk logControl1;
    }
}