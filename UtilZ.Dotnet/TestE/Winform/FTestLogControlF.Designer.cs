namespace TestE.Winform
{
    partial class FTestLogControlF
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
            this.cbIsLock = new System.Windows.Forms.CheckBox();
            this.btnThreadTest = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.logControl1 = new UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF();
            this.SuspendLayout();
            // 
            // cbIsLock
            // 
            this.cbIsLock.AutoSize = true;
            this.cbIsLock.Location = new System.Drawing.Point(209, 12);
            this.cbIsLock.Name = "cbIsLock";
            this.cbIsLock.Size = new System.Drawing.Size(48, 16);
            this.cbIsLock.TabIndex = 7;
            this.cbIsLock.Text = "锁定";
            this.cbIsLock.UseVisualStyleBackColor = true;
            // 
            // btnThreadTest
            // 
            this.btnThreadTest.Location = new System.Drawing.Point(115, 12);
            this.btnThreadTest.Name = "btnThreadTest";
            this.btnThreadTest.Size = new System.Drawing.Size(75, 23);
            this.btnThreadTest.TabIndex = 6;
            this.btnThreadTest.Text = "ThreadTest";
            this.btnThreadTest.UseVisualStyleBackColor = true;
            this.btnThreadTest.Click += new System.EventHandler(this.btnThreadTest_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(19, 13);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // logControl1
            // 
            this.logControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logControl1.BackColor = System.Drawing.Color.Green;
            this.logControl1.IsLock = false;
            this.logControl1.Location = new System.Drawing.Point(12, 66);
            this.logControl1.MaxItemCount = 100;
            this.logControl1.Name = "logControl1";
            this.logControl1.Size = new System.Drawing.Size(776, 372);
            this.logControl1.TabIndex = 0;
            // 
            // FTestLogControlF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbIsLock);
            this.Controls.Add(this.btnThreadTest);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.logControl1);
            this.Name = "FTestLogControlF";
            this.Text = "FTestLogControlF";
            this.Load += new System.EventHandler(this.FTestLogControlF_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF logControl1;
        private System.Windows.Forms.CheckBox cbIsLock;
        private System.Windows.Forms.Button btnThreadTest;
        private System.Windows.Forms.Button btnTest;
    }
}