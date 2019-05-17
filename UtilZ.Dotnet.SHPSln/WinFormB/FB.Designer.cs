namespace WinFormB
{
    partial class FB
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnSendData = new System.Windows.Forms.Button();
            this.btnSendText = new System.Windows.Forms.Button();
            this.rtxtMsg = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.logControlF1 = new UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.numParallelThreadCount = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreadCount)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(498, 70);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 11;
            this.btnSendFile.Text = "文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(498, 41);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(75, 23);
            this.btnSendData.TabIndex = 10;
            this.btnSendData.Text = "数据";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnSendText
            // 
            this.btnSendText.Location = new System.Drawing.Point(497, 12);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(75, 23);
            this.btnSendText.TabIndex = 9;
            this.btnSendText.Text = "文本";
            this.btnSendText.UseVisualStyleBackColor = true;
            this.btnSendText.Click += new System.EventHandler(this.btnSendText_Click);
            // 
            // rtxtMsg
            // 
            this.rtxtMsg.Location = new System.Drawing.Point(80, 12);
            this.rtxtMsg.Name = "rtxtMsg";
            this.rtxtMsg.Size = new System.Drawing.Size(411, 75);
            this.rtxtMsg.TabIndex = 8;
            this.rtxtMsg.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "消息";
            // 
            // logControlF1
            // 
            this.logControlF1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logControlF1.IsLock = false;
            this.logControlF1.Location = new System.Drawing.Point(12, 93);
            this.logControlF1.MaxItemCount = 100;
            this.logControlF1.Name = "logControlF1";
            this.logControlF1.Size = new System.Drawing.Size(560, 357);
            this.logControlF1.TabIndex = 6;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(12, 64);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(62, 23);
            this.btnClearLog.TabIndex = 12;
            this.btnClearLog.Text = "清除";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // numParallelThreadCount
            // 
            this.numParallelThreadCount.Location = new System.Drawing.Point(12, 37);
            this.numParallelThreadCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numParallelThreadCount.Name = "numParallelThreadCount";
            this.numParallelThreadCount.Size = new System.Drawing.Size(61, 21);
            this.numParallelThreadCount.TabIndex = 13;
            this.numParallelThreadCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numParallelThreadCount.ValueChanged += new System.EventHandler(this.numParallelThreadCount_ValueChanged);
            // 
            // FB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 462);
            this.Controls.Add(this.numParallelThreadCount);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnSendData);
            this.Controls.Add(this.btnSendText);
            this.Controls.Add(this.rtxtMsg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logControlF1);
            this.Name = "FB";
            this.Text = "B";
            this.Load += new System.EventHandler(this.FB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreadCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.Button btnSendText;
        private System.Windows.Forms.RichTextBox rtxtMsg;
        private System.Windows.Forms.Label label1;
        private UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF logControlF1;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.NumericUpDown numParallelThreadCount;
    }
}

