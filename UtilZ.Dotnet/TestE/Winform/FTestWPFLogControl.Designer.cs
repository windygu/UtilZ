namespace TestE.Winform
{
    partial class FTestWPFLogControl
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
            this.btnTest = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnThreadTest = new System.Windows.Forms.Button();
            this.cbIsLock = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.logControl1 = new UtilZ.Dotnet.WindowEx.WPF.Controls.LogControl();
            this.btnPer = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(109, 13);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnThreadTest
            // 
            this.btnThreadTest.Location = new System.Drawing.Point(210, 12);
            this.btnThreadTest.Name = "btnThreadTest";
            this.btnThreadTest.Size = new System.Drawing.Size(75, 23);
            this.btnThreadTest.TabIndex = 3;
            this.btnThreadTest.Text = "ThreadTest";
            this.btnThreadTest.UseVisualStyleBackColor = true;
            this.btnThreadTest.Click += new System.EventHandler(this.btnThreadTest_Click);
            // 
            // cbIsLock
            // 
            this.cbIsLock.AutoSize = true;
            this.cbIsLock.Location = new System.Drawing.Point(321, 20);
            this.cbIsLock.Name = "cbIsLock";
            this.cbIsLock.Size = new System.Drawing.Size(48, 16);
            this.cbIsLock.TabIndex = 4;
            this.cbIsLock.Text = "锁定";
            this.cbIsLock.UseVisualStyleBackColor = true;
            this.cbIsLock.CheckedChanged += new System.EventHandler(this.cbIsLock_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(538, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 5;
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost1.Location = new System.Drawing.Point(12, 50);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(808, 445);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.logControl1;
            // 
            // btnPer
            // 
            this.btnPer.Location = new System.Drawing.Point(431, 16);
            this.btnPer.Name = "btnPer";
            this.btnPer.Size = new System.Drawing.Size(75, 23);
            this.btnPer.TabIndex = 6;
            this.btnPer.Text = "性能";
            this.btnPer.UseVisualStyleBackColor = true;
            this.btnPer.Click += new System.EventHandler(this.btnPer_Click);
            // 
            // FTestWPFLogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 507);
            this.Controls.Add(this.btnPer);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cbIsLock);
            this.Controls.Add(this.btnThreadTest);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.elementHost1);
            this.Name = "FTestWPFLogControl";
            this.Text = "FTestWPFLogControl";
            this.Load += new System.EventHandler(this.FTestWPFLogControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private UtilZ.Dotnet.WindowEx.WPF.Controls.LogControl logControl1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnThreadTest;
        private System.Windows.Forms.CheckBox cbIsLock;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnPer;
    }
}