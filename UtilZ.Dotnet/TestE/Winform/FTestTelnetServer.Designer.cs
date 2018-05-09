namespace TestE.Winform
{
    partial class FTestTelnetServer
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
            this.logControl1 = new UtilZ.Dotnet.WindowEx.Winform.Controls.LogControl();
            this.btnTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // logControl1
            // 
            this.logControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logControl1.IsLock = false;
            this.logControl1.Location = new System.Drawing.Point(12, 49);
            this.logControl1.MaxItemCount = 100;
            this.logControl1.Name = "logControl1";
            this.logControl1.Size = new System.Drawing.Size(776, 389);
            this.logControl1.TabIndex = 0;
            this.logControl1.Load += new System.EventHandler(this.logControl1_Load);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(13, 13);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // FTestTelnetServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.logControl1);
            this.Name = "FTestTelnetServer";
            this.Text = "FTestTelnetServer";
            this.Load += new System.EventHandler(this.FTestTelnetServer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UtilZ.Dotnet.WindowEx.Winform.Controls.LogControl logControl1;
        private System.Windows.Forms.Button btnTest;
    }
}