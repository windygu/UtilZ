namespace WinFormA
{
    partial class FA
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
            this.logControl = new UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF();
            this.label1 = new System.Windows.Forms.Label();
            this.rtxtMsg = new System.Windows.Forms.RichTextBox();
            this.btnSendText = new System.Windows.Forms.Button();
            this.btnSendData = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.numParallelThreadCount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numSrcPort = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numDstPort = new System.Windows.Forms.NumericUpDown();
            this.txtDstIp = new System.Windows.Forms.TextBox();
            this.btnInit = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDataDir = new System.Windows.Forms.TextBox();
            this.btnChoiceDataDir = new System.Windows.Forms.Button();
            this.checkBoxLockLog = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxLogLevel = new System.Windows.Forms.ComboBox();
            this.btnStream = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.numPackageRate = new System.Windows.Forms.NumericUpDown();
            this.btnPacketInfo = new System.Windows.Forms.Button();
            this.btnResetPacketInfo = new System.Windows.Forms.Button();
            this.txtSendIp = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDstPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPackageRate)).BeginInit();
            this.SuspendLayout();
            // 
            // logControl
            // 
            this.logControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logControl.IsLock = false;
            this.logControl.Location = new System.Drawing.Point(12, 182);
            this.logControl.MaxItemCount = 100;
            this.logControl.Name = "logControl";
            this.logControl.Size = new System.Drawing.Size(760, 368);
            this.logControl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "消息";
            // 
            // rtxtMsg
            // 
            this.rtxtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtMsg.Location = new System.Drawing.Point(94, 56);
            this.rtxtMsg.Name = "rtxtMsg";
            this.rtxtMsg.Size = new System.Drawing.Size(678, 54);
            this.rtxtMsg.TabIndex = 2;
            this.rtxtMsg.Text = "";
            // 
            // btnSendText
            // 
            this.btnSendText.Location = new System.Drawing.Point(175, 123);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(75, 23);
            this.btnSendText.TabIndex = 3;
            this.btnSendText.Text = "文本";
            this.btnSendText.UseVisualStyleBackColor = true;
            this.btnSendText.Click += new System.EventHandler(this.btnSendText_Click);
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(256, 123);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(75, 23);
            this.btnSendData.TabIndex = 4;
            this.btnSendData.Text = "数据";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(337, 123);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 5;
            this.btnSendFile.Text = "文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(94, 123);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 6;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // numParallelThreadCount
            // 
            this.numParallelThreadCount.Location = new System.Drawing.Point(94, 155);
            this.numParallelThreadCount.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numParallelThreadCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numParallelThreadCount.Name = "numParallelThreadCount";
            this.numParallelThreadCount.Size = new System.Drawing.Size(61, 21);
            this.numParallelThreadCount.TabIndex = 7;
            this.numParallelThreadCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numParallelThreadCount.ValueChanged += new System.EventHandler(this.numParallelThreadCount_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "收数据线程数";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(240, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "接收数据端口";
            // 
            // numSrcPort
            // 
            this.numSrcPort.Location = new System.Drawing.Point(322, 5);
            this.numSrcPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numSrcPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSrcPort.Name = "numSrcPort";
            this.numSrcPort.Size = new System.Drawing.Size(61, 21);
            this.numSrcPort.TabIndex = 10;
            this.numSrcPort.Value = new decimal(new int[] {
            6101,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(388, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "目的地IP";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(587, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "目标端口";
            // 
            // numDstPort
            // 
            this.numDstPort.Location = new System.Drawing.Point(643, 5);
            this.numDstPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numDstPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDstPort.Name = "numDstPort";
            this.numDstPort.Size = new System.Drawing.Size(61, 21);
            this.numDstPort.TabIndex = 13;
            this.numDstPort.Value = new decimal(new int[] {
            6102,
            0,
            0,
            0});
            // 
            // txtDstIp
            // 
            this.txtDstIp.Location = new System.Drawing.Point(445, 4);
            this.txtDstIp.Name = "txtDstIp";
            this.txtDstIp.Size = new System.Drawing.Size(140, 21);
            this.txtDstIp.TabIndex = 14;
            this.txtDstIp.Text = "127.0.0.1";
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(12, 123);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(75, 23);
            this.btnInit.TabIndex = 15;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "数据存放目录";
            // 
            // txtDataDir
            // 
            this.txtDataDir.Location = new System.Drawing.Point(94, 29);
            this.txtDataDir.Name = "txtDataDir";
            this.txtDataDir.Size = new System.Drawing.Size(180, 21);
            this.txtDataDir.TabIndex = 17;
            this.txtDataDir.Text = "E:\\Tmp";
            // 
            // btnChoiceDataDir
            // 
            this.btnChoiceDataDir.Location = new System.Drawing.Point(278, 27);
            this.btnChoiceDataDir.Name = "btnChoiceDataDir";
            this.btnChoiceDataDir.Size = new System.Drawing.Size(33, 23);
            this.btnChoiceDataDir.TabIndex = 18;
            this.btnChoiceDataDir.Text = "...";
            this.btnChoiceDataDir.UseVisualStyleBackColor = true;
            this.btnChoiceDataDir.Click += new System.EventHandler(this.btnChoiceDataDir_Click);
            // 
            // checkBoxLockLog
            // 
            this.checkBoxLockLog.AutoSize = true;
            this.checkBoxLockLog.Location = new System.Drawing.Point(292, 154);
            this.checkBoxLockLog.Name = "checkBoxLockLog";
            this.checkBoxLockLog.Size = new System.Drawing.Size(72, 16);
            this.checkBoxLockLog.TabIndex = 19;
            this.checkBoxLockLog.Text = "锁定日志";
            this.checkBoxLockLog.UseVisualStyleBackColor = true;
            this.checkBoxLockLog.CheckedChanged += new System.EventHandler(this.checkBoxLockLog_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(159, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "日志显示级别";
            // 
            // comboBoxLogLevel
            // 
            this.comboBoxLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLogLevel.FormattingEnabled = true;
            this.comboBoxLogLevel.Location = new System.Drawing.Point(237, 153);
            this.comboBoxLogLevel.Name = "comboBoxLogLevel";
            this.comboBoxLogLevel.Size = new System.Drawing.Size(49, 20);
            this.comboBoxLogLevel.TabIndex = 21;
            this.comboBoxLogLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxLogLevel_SelectedIndexChanged);
            // 
            // btnStream
            // 
            this.btnStream.Location = new System.Drawing.Point(419, 123);
            this.btnStream.Name = "btnStream";
            this.btnStream.Size = new System.Drawing.Size(75, 23);
            this.btnStream.TabIndex = 22;
            this.btnStream.Text = "流";
            this.btnStream.UseVisualStyleBackColor = true;
            this.btnStream.Click += new System.EventHandler(this.btnStream_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(370, 158);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "丢包率(百分比)";
            // 
            // numPackageRate
            // 
            this.numPackageRate.DecimalPlaces = 2;
            this.numPackageRate.Location = new System.Drawing.Point(456, 153);
            this.numPackageRate.Name = "numPackageRate";
            this.numPackageRate.Size = new System.Drawing.Size(120, 21);
            this.numPackageRate.TabIndex = 24;
            this.numPackageRate.ValueChanged += new System.EventHandler(this.numPackageRate_ValueChanged);
            // 
            // btnPacketInfo
            // 
            this.btnPacketInfo.Location = new System.Drawing.Point(501, 122);
            this.btnPacketInfo.Name = "btnPacketInfo";
            this.btnPacketInfo.Size = new System.Drawing.Size(75, 23);
            this.btnPacketInfo.TabIndex = 25;
            this.btnPacketInfo.Text = "包信息";
            this.btnPacketInfo.UseVisualStyleBackColor = true;
            this.btnPacketInfo.Click += new System.EventHandler(this.btnPacketInfo_Click);
            // 
            // btnResetPacketInfo
            // 
            this.btnResetPacketInfo.Location = new System.Drawing.Point(583, 121);
            this.btnResetPacketInfo.Name = "btnResetPacketInfo";
            this.btnResetPacketInfo.Size = new System.Drawing.Size(75, 23);
            this.btnResetPacketInfo.TabIndex = 26;
            this.btnResetPacketInfo.Text = "重置包信息";
            this.btnResetPacketInfo.UseVisualStyleBackColor = true;
            this.btnResetPacketInfo.Click += new System.EventHandler(this.btnResetPacketInfo_Click);
            // 
            // txtSendIp
            // 
            this.txtSendIp.Location = new System.Drawing.Point(94, 4);
            this.txtSendIp.Name = "txtSendIp";
            this.txtSendIp.Size = new System.Drawing.Size(140, 21);
            this.txtSendIp.TabIndex = 28;
            this.txtSendIp.Text = "127.0.0.1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(37, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 27;
            this.label9.Text = "发送端IP";
            // 
            // FA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.txtSendIp);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnResetPacketInfo);
            this.Controls.Add(this.btnPacketInfo);
            this.Controls.Add(this.numPackageRate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnStream);
            this.Controls.Add(this.comboBoxLogLevel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBoxLockLog);
            this.Controls.Add(this.btnChoiceDataDir);
            this.Controls.Add(this.txtDataDir);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.txtDstIp);
            this.Controls.Add(this.numDstPort);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numSrcPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numParallelThreadCount);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnSendData);
            this.Controls.Add(this.btnSendText);
            this.Controls.Add(this.rtxtMsg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logControl);
            this.Name = "FA";
            this.Text = "传输测试";
            this.Load += new System.EventHandler(this.FA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDstPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPackageRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF logControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtxtMsg;
        private System.Windows.Forms.Button btnSendText;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.NumericUpDown numParallelThreadCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numSrcPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numDstPort;
        private System.Windows.Forms.TextBox txtDstIp;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDataDir;
        private System.Windows.Forms.Button btnChoiceDataDir;
        private System.Windows.Forms.CheckBox checkBoxLockLog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxLogLevel;
        private System.Windows.Forms.Button btnStream;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numPackageRate;
        private System.Windows.Forms.Button btnPacketInfo;
        private System.Windows.Forms.Button btnResetPacketInfo;
        private System.Windows.Forms.TextBox txtSendIp;
        private System.Windows.Forms.Label label9;
    }
}

