namespace WinFormA
{
    partial class FNetTansfer2
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
            this.txtSendIp = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnResetPacketInfo = new System.Windows.Forms.Button();
            this.btnPacketInfo = new System.Windows.Forms.Button();
            this.numPackageRate = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.btnStream = new System.Windows.Forms.Button();
            this.comboBoxLogLevel = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxLockLog = new System.Windows.Forms.CheckBox();
            this.btnChoiceDataDir = new System.Windows.Forms.Button();
            this.txtDataDir = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnInit = new System.Windows.Forms.Button();
            this.txtDstIp = new System.Windows.Forms.TextBox();
            this.numDstPort = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numSrcPort = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numParallelThreadCount = new System.Windows.Forms.NumericUpDown();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnSendData = new System.Windows.Forms.Button();
            this.btnSendText = new System.Windows.Forms.Button();
            this.rtxtMsg = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.logControl = new UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF();
            ((System.ComponentModel.ISupportInitialize)(this.numPackageRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDstPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreadCount)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSendIp
            // 
            this.txtSendIp.Location = new System.Drawing.Point(95, 8);
            this.txtSendIp.Name = "txtSendIp";
            this.txtSendIp.Size = new System.Drawing.Size(140, 21);
            this.txtSendIp.TabIndex = 57;
            this.txtSendIp.Text = "127.0.0.1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(38, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 56;
            this.label9.Text = "发送端IP";
            // 
            // btnResetPacketInfo
            // 
            this.btnResetPacketInfo.Location = new System.Drawing.Point(584, 125);
            this.btnResetPacketInfo.Name = "btnResetPacketInfo";
            this.btnResetPacketInfo.Size = new System.Drawing.Size(75, 23);
            this.btnResetPacketInfo.TabIndex = 55;
            this.btnResetPacketInfo.Text = "重置包信息";
            this.btnResetPacketInfo.UseVisualStyleBackColor = true;
            this.btnResetPacketInfo.Click += new System.EventHandler(this.btnResetPacketInfo_Click);
            // 
            // btnPacketInfo
            // 
            this.btnPacketInfo.Location = new System.Drawing.Point(502, 126);
            this.btnPacketInfo.Name = "btnPacketInfo";
            this.btnPacketInfo.Size = new System.Drawing.Size(75, 23);
            this.btnPacketInfo.TabIndex = 54;
            this.btnPacketInfo.Text = "包信息";
            this.btnPacketInfo.UseVisualStyleBackColor = true;
            this.btnPacketInfo.Click += new System.EventHandler(this.btnPacketInfo_Click);
            // 
            // numPackageRate
            // 
            this.numPackageRate.DecimalPlaces = 2;
            this.numPackageRate.Location = new System.Drawing.Point(457, 157);
            this.numPackageRate.Name = "numPackageRate";
            this.numPackageRate.Size = new System.Drawing.Size(120, 21);
            this.numPackageRate.TabIndex = 53;
            this.numPackageRate.ValueChanged += new System.EventHandler(this.numPackageRate_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(371, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 52;
            this.label8.Text = "丢包率(百分比)";
            // 
            // btnStream
            // 
            this.btnStream.Location = new System.Drawing.Point(420, 127);
            this.btnStream.Name = "btnStream";
            this.btnStream.Size = new System.Drawing.Size(75, 23);
            this.btnStream.TabIndex = 51;
            this.btnStream.Text = "流";
            this.btnStream.UseVisualStyleBackColor = true;
            this.btnStream.Click += new System.EventHandler(this.btnStream_Click);
            // 
            // comboBoxLogLevel
            // 
            this.comboBoxLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLogLevel.FormattingEnabled = true;
            this.comboBoxLogLevel.Location = new System.Drawing.Point(238, 157);
            this.comboBoxLogLevel.Name = "comboBoxLogLevel";
            this.comboBoxLogLevel.Size = new System.Drawing.Size(49, 20);
            this.comboBoxLogLevel.TabIndex = 50;
            this.comboBoxLogLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxLogLevel_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(160, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 49;
            this.label7.Text = "日志显示级别";
            // 
            // checkBoxLockLog
            // 
            this.checkBoxLockLog.AutoSize = true;
            this.checkBoxLockLog.Location = new System.Drawing.Point(293, 158);
            this.checkBoxLockLog.Name = "checkBoxLockLog";
            this.checkBoxLockLog.Size = new System.Drawing.Size(72, 16);
            this.checkBoxLockLog.TabIndex = 48;
            this.checkBoxLockLog.Text = "锁定日志";
            this.checkBoxLockLog.UseVisualStyleBackColor = true;
            this.checkBoxLockLog.CheckedChanged += new System.EventHandler(this.checkBoxLockLog_CheckedChanged);
            // 
            // btnChoiceDataDir
            // 
            this.btnChoiceDataDir.Location = new System.Drawing.Point(279, 31);
            this.btnChoiceDataDir.Name = "btnChoiceDataDir";
            this.btnChoiceDataDir.Size = new System.Drawing.Size(33, 23);
            this.btnChoiceDataDir.TabIndex = 47;
            this.btnChoiceDataDir.Text = "...";
            this.btnChoiceDataDir.UseVisualStyleBackColor = true;
            this.btnChoiceDataDir.Click += new System.EventHandler(this.btnChoiceDataDir_Click);
            // 
            // txtDataDir
            // 
            this.txtDataDir.Location = new System.Drawing.Point(95, 33);
            this.txtDataDir.Name = "txtDataDir";
            this.txtDataDir.Size = new System.Drawing.Size(180, 21);
            this.txtDataDir.TabIndex = 46;
            this.txtDataDir.Text = "E:\\Tmp";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 45;
            this.label6.Text = "数据存放目录";
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(13, 127);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(75, 23);
            this.btnInit.TabIndex = 44;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // txtDstIp
            // 
            this.txtDstIp.Location = new System.Drawing.Point(446, 8);
            this.txtDstIp.Name = "txtDstIp";
            this.txtDstIp.Size = new System.Drawing.Size(140, 21);
            this.txtDstIp.TabIndex = 43;
            this.txtDstIp.Text = "127.0.0.1";
            // 
            // numDstPort
            // 
            this.numDstPort.Location = new System.Drawing.Point(644, 9);
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
            this.numDstPort.TabIndex = 42;
            this.numDstPort.Value = new decimal(new int[] {
            6102,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(588, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 41;
            this.label5.Text = "目标端口";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(389, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 40;
            this.label4.Text = "目的地IP";
            // 
            // numSrcPort
            // 
            this.numSrcPort.Location = new System.Drawing.Point(323, 9);
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
            this.numSrcPort.TabIndex = 39;
            this.numSrcPort.Value = new decimal(new int[] {
            6101,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(241, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 38;
            this.label3.Text = "接收数据端口";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 37;
            this.label2.Text = "收数据线程数";
            // 
            // numParallelThreadCount
            // 
            this.numParallelThreadCount.Location = new System.Drawing.Point(95, 159);
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
            this.numParallelThreadCount.TabIndex = 36;
            this.numParallelThreadCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(95, 127);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 35;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(338, 127);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 34;
            this.btnSendFile.Text = "文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(257, 127);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(75, 23);
            this.btnSendData.TabIndex = 33;
            this.btnSendData.Text = "数据";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnSendText
            // 
            this.btnSendText.Location = new System.Drawing.Point(176, 127);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(75, 23);
            this.btnSendText.TabIndex = 32;
            this.btnSendText.Text = "文本";
            this.btnSendText.UseVisualStyleBackColor = true;
            this.btnSendText.Click += new System.EventHandler(this.btnSendText_Click);
            // 
            // rtxtMsg
            // 
            this.rtxtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtMsg.Location = new System.Drawing.Point(95, 60);
            this.rtxtMsg.Name = "rtxtMsg";
            this.rtxtMsg.Size = new System.Drawing.Size(678, 54);
            this.rtxtMsg.TabIndex = 31;
            this.rtxtMsg.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 30;
            this.label1.Text = "消息";
            // 
            // logControl
            // 
            this.logControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logControl.IsLock = false;
            this.logControl.Location = new System.Drawing.Point(13, 186);
            this.logControl.MaxItemCount = 100;
            this.logControl.Name = "logControl";
            this.logControl.Size = new System.Drawing.Size(760, 368);
            this.logControl.TabIndex = 29;
            // 
            // FNetTansfer2
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
            this.Name = "FNetTansfer2";
            this.Text = "FNetTansfer2";
            this.Load += new System.EventHandler(this.FNetTansfer2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPackageRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDstPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreadCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSendIp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnResetPacketInfo;
        private System.Windows.Forms.Button btnPacketInfo;
        private System.Windows.Forms.NumericUpDown numPackageRate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnStream;
        private System.Windows.Forms.ComboBox comboBoxLogLevel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxLockLog;
        private System.Windows.Forms.Button btnChoiceDataDir;
        private System.Windows.Forms.TextBox txtDataDir;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.TextBox txtDstIp;
        private System.Windows.Forms.NumericUpDown numDstPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numSrcPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numParallelThreadCount;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.Button btnSendText;
        private System.Windows.Forms.RichTextBox rtxtMsg;
        private System.Windows.Forms.Label label1;
        private UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF logControl;
    }
}