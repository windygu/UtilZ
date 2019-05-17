namespace UtilZ.Dotnet.SHPAgent
{
    partial class FAppMonitorItemEdit
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
            this.btnChioceAppExeFile = new System.Windows.Forms.Button();
            this.lbAppExeFilePath = new System.Windows.Forms.Label();
            this.txtAppExeFilePath = new System.Windows.Forms.TextBox();
            this.txtAppName = new System.Windows.Forms.TextBox();
            this.lbAppName = new System.Windows.Forms.Label();
            this.txtAppArguments = new System.Windows.Forms.TextBox();
            this.lbAppArguments = new System.Windows.Forms.Label();
            this.btnCancell = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProcessFilePath = new System.Windows.Forms.TextBox();
            this.lbProcessFilePath = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnChioceProcessFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnChioceAppExeFile
            // 
            this.btnChioceAppExeFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChioceAppExeFile.Location = new System.Drawing.Point(499, 11);
            this.btnChioceAppExeFile.Name = "btnChioceAppExeFile";
            this.btnChioceAppExeFile.Size = new System.Drawing.Size(43, 23);
            this.btnChioceAppExeFile.TabIndex = 0;
            this.btnChioceAppExeFile.Text = "...";
            this.btnChioceAppExeFile.UseVisualStyleBackColor = true;
            this.btnChioceAppExeFile.Click += new System.EventHandler(this.btnChioceAppExeFile_Click);
            // 
            // lbAppExeFilePath
            // 
            this.lbAppExeFilePath.AutoSize = true;
            this.lbAppExeFilePath.Location = new System.Drawing.Point(13, 17);
            this.lbAppExeFilePath.Name = "lbAppExeFilePath";
            this.lbAppExeFilePath.Size = new System.Drawing.Size(77, 12);
            this.lbAppExeFilePath.TabIndex = 1;
            this.lbAppExeFilePath.Text = "启动文件路径";
            // 
            // txtAppExeFilePath
            // 
            this.txtAppExeFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAppExeFilePath.Location = new System.Drawing.Point(97, 13);
            this.txtAppExeFilePath.Name = "txtAppExeFilePath";
            this.txtAppExeFilePath.Size = new System.Drawing.Size(396, 21);
            this.txtAppExeFilePath.TabIndex = 2;
            // 
            // txtAppName
            // 
            this.txtAppName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAppName.Location = new System.Drawing.Point(97, 69);
            this.txtAppName.Name = "txtAppName";
            this.txtAppName.Size = new System.Drawing.Size(396, 21);
            this.txtAppName.TabIndex = 4;
            // 
            // lbAppName
            // 
            this.lbAppName.AutoSize = true;
            this.lbAppName.Location = new System.Drawing.Point(13, 73);
            this.lbAppName.Name = "lbAppName";
            this.lbAppName.Size = new System.Drawing.Size(77, 12);
            this.lbAppName.TabIndex = 3;
            this.lbAppName.Text = "应用程序名称";
            // 
            // txtAppArguments
            // 
            this.txtAppArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAppArguments.Location = new System.Drawing.Point(97, 98);
            this.txtAppArguments.Name = "txtAppArguments";
            this.txtAppArguments.Size = new System.Drawing.Size(396, 21);
            this.txtAppArguments.TabIndex = 6;
            // 
            // lbAppArguments
            // 
            this.lbAppArguments.AutoSize = true;
            this.lbAppArguments.Location = new System.Drawing.Point(37, 102);
            this.lbAppArguments.Name = "lbAppArguments";
            this.lbAppArguments.Size = new System.Drawing.Size(53, 12);
            this.lbAppArguments.TabIndex = 5;
            this.lbAppArguments.Text = "启动参数";
            // 
            // btnCancell
            // 
            this.btnCancell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancell.Location = new System.Drawing.Point(418, 125);
            this.btnCancell.Name = "btnCancell";
            this.btnCancell.Size = new System.Drawing.Size(75, 23);
            this.btnCancell.TabIndex = 7;
            this.btnCancell.Text = "取消";
            this.btnCancell.UseVisualStyleBackColor = true;
            this.btnCancell.Click += new System.EventHandler(this.btnCancell_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(338, 125);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(2, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(2, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "*";
            // 
            // txtProcessFilePath
            // 
            this.txtProcessFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcessFilePath.Location = new System.Drawing.Point(97, 42);
            this.txtProcessFilePath.Name = "txtProcessFilePath";
            this.txtProcessFilePath.Size = new System.Drawing.Size(396, 21);
            this.txtProcessFilePath.TabIndex = 12;
            // 
            // lbProcessFilePath
            // 
            this.lbProcessFilePath.AutoSize = true;
            this.lbProcessFilePath.Location = new System.Drawing.Point(13, 46);
            this.lbProcessFilePath.Name = "lbProcessFilePath";
            this.lbProcessFilePath.Size = new System.Drawing.Size(77, 12);
            this.lbProcessFilePath.TabIndex = 11;
            this.lbProcessFilePath.Text = "进程文件路径";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(2, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "*";
            // 
            // btnChioceProcessFile
            // 
            this.btnChioceProcessFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChioceProcessFile.Location = new System.Drawing.Point(499, 40);
            this.btnChioceProcessFile.Name = "btnChioceProcessFile";
            this.btnChioceProcessFile.Size = new System.Drawing.Size(43, 23);
            this.btnChioceProcessFile.TabIndex = 14;
            this.btnChioceProcessFile.Text = "...";
            this.btnChioceProcessFile.UseVisualStyleBackColor = true;
            this.btnChioceProcessFile.Click += new System.EventHandler(this.btnChioceProcessFile_Click);
            // 
            // FAppMonitorItemEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 154);
            this.ControlBox = false;
            this.Controls.Add(this.btnChioceProcessFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtProcessFilePath);
            this.Controls.Add(this.lbProcessFilePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancell);
            this.Controls.Add(this.txtAppArguments);
            this.Controls.Add(this.lbAppArguments);
            this.Controls.Add(this.txtAppName);
            this.Controls.Add(this.lbAppName);
            this.Controls.Add(this.txtAppExeFilePath);
            this.Controls.Add(this.lbAppExeFilePath);
            this.Controls.Add(this.btnChioceAppExeFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FAppMonitorItemEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑监视项";
            this.Load += new System.EventHandler(this.FAppMonitorItemEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChioceAppExeFile;
        private System.Windows.Forms.Label lbAppExeFilePath;
        private System.Windows.Forms.TextBox txtAppExeFilePath;
        private System.Windows.Forms.TextBox txtAppName;
        private System.Windows.Forms.Label lbAppName;
        private System.Windows.Forms.TextBox txtAppArguments;
        private System.Windows.Forms.Label lbAppArguments;
        private System.Windows.Forms.Button btnCancell;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProcessFilePath;
        private System.Windows.Forms.Label lbProcessFilePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnChioceProcessFile;
    }
}