namespace WinFormA
{
    partial class FUdpNet
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
            this.label11 = new System.Windows.Forms.Label();
            this.comPri = new System.Windows.Forms.ComboBox();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numSendCount = new System.Windows.Forms.NumericUpDown();
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
            this.cbDataWriteFile = new System.Windows.Forms.CheckBox();
            this.numPosition = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numLen = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.txtDataFilePath = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnDataFilePath = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSendCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDstPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLen)).BeginInit();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(29, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 120;
            this.label11.Text = "发送策略";
            // 
            // comPri
            // 
            this.comPri.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comPri.FormattingEnabled = true;
            this.comPri.Location = new System.Drawing.Point(88, 110);
            this.comPri.Name = "comPri";
            this.comPri.Size = new System.Drawing.Size(75, 20);
            this.comPri.TabIndex = 119;
            this.comPri.SelectedIndexChanged += new System.EventHandler(this.comPri_SelectedIndexChanged);
            // 
            // numTimeout
            // 
            this.numTimeout.DecimalPlaces = 2;
            this.numTimeout.Location = new System.Drawing.Point(452, 111);
            this.numTimeout.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numTimeout.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(82, 21);
            this.numTimeout.TabIndex = 118;
            this.numTimeout.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(338, 118);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 12);
            this.label9.TabIndex = 117;
            this.label9.Text = "发送超时时长(毫秒)";
            // 
            // numSendCount
            // 
            this.numSendCount.Location = new System.Drawing.Point(239, 109);
            this.numSendCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numSendCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSendCount.Name = "numSendCount";
            this.numSendCount.Size = new System.Drawing.Size(82, 21);
            this.numSendCount.TabIndex = 114;
            this.numSendCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(180, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 113;
            this.label8.Text = "发送次数";
            // 
            // btnStream
            // 
            this.btnStream.Location = new System.Drawing.Point(327, 161);
            this.btnStream.Name = "btnStream";
            this.btnStream.Size = new System.Drawing.Size(75, 23);
            this.btnStream.TabIndex = 112;
            this.btnStream.Text = "流";
            this.btnStream.UseVisualStyleBackColor = true;
            this.btnStream.Click += new System.EventHandler(this.btnStream_Click);
            // 
            // comboBoxLogLevel
            // 
            this.comboBoxLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLogLevel.FormattingEnabled = true;
            this.comboBoxLogLevel.Location = new System.Drawing.Point(485, 81);
            this.comboBoxLogLevel.Name = "comboBoxLogLevel";
            this.comboBoxLogLevel.Size = new System.Drawing.Size(49, 20);
            this.comboBoxLogLevel.TabIndex = 111;
            this.comboBoxLogLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxLogLevel_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(407, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 110;
            this.label7.Text = "日志显示级别";
            // 
            // checkBoxLockLog
            // 
            this.checkBoxLockLog.AutoSize = true;
            this.checkBoxLockLog.Location = new System.Drawing.Point(540, 82);
            this.checkBoxLockLog.Name = "checkBoxLockLog";
            this.checkBoxLockLog.Size = new System.Drawing.Size(72, 16);
            this.checkBoxLockLog.TabIndex = 109;
            this.checkBoxLockLog.Text = "锁定日志";
            this.checkBoxLockLog.UseVisualStyleBackColor = true;
            this.checkBoxLockLog.CheckedChanged += new System.EventHandler(this.checkBoxLockLog_CheckedChanged);
            // 
            // btnChoiceDataDir
            // 
            this.btnChoiceDataDir.Location = new System.Drawing.Point(745, 6);
            this.btnChoiceDataDir.Name = "btnChoiceDataDir";
            this.btnChoiceDataDir.Size = new System.Drawing.Size(33, 23);
            this.btnChoiceDataDir.TabIndex = 108;
            this.btnChoiceDataDir.Text = "...";
            this.btnChoiceDataDir.UseVisualStyleBackColor = true;
            this.btnChoiceDataDir.Click += new System.EventHandler(this.btnChoiceDataDir_Click);
            // 
            // txtDataDir
            // 
            this.txtDataDir.Location = new System.Drawing.Point(561, 8);
            this.txtDataDir.Name = "txtDataDir";
            this.txtDataDir.Size = new System.Drawing.Size(180, 21);
            this.txtDataDir.TabIndex = 107;
            this.txtDataDir.Text = "D:\\Tmp";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(478, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 106;
            this.label6.Text = "数据存放目录";
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(88, 81);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(75, 23);
            this.btnInit.TabIndex = 105;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // txtDstIp
            // 
            this.txtDstIp.Location = new System.Drawing.Point(211, 8);
            this.txtDstIp.Name = "txtDstIp";
            this.txtDstIp.Size = new System.Drawing.Size(140, 21);
            this.txtDstIp.TabIndex = 104;
            this.txtDstIp.Text = "127.0.0.1";
            // 
            // numDstPort
            // 
            this.numDstPort.Location = new System.Drawing.Point(409, 9);
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
            this.numDstPort.TabIndex = 103;
            this.numDstPort.Value = new decimal(new int[] {
            6101,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(353, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 102;
            this.label5.Text = "目标端口";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(154, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 101;
            this.label4.Text = "目的地IP";
            // 
            // numSrcPort
            // 
            this.numSrcPort.Location = new System.Drawing.Point(88, 9);
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
            this.numSrcPort.TabIndex = 100;
            this.numSrcPort.Value = new decimal(new int[] {
            6102,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 99;
            this.label3.Text = "接收数据端口";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 98;
            this.label2.Text = "收数据线程数";
            // 
            // numParallelThreadCount
            // 
            this.numParallelThreadCount.Location = new System.Drawing.Point(342, 83);
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
            this.numParallelThreadCount.TabIndex = 97;
            this.numParallelThreadCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numParallelThreadCount.ValueChanged += new System.EventHandler(this.numParallelThreadCount_ValueChanged);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(169, 81);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 96;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(246, 162);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 95;
            this.btnSendFile.Text = "文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(169, 161);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(75, 23);
            this.btnSendData.TabIndex = 94;
            this.btnSendData.Text = "数据";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.btnSendData_Click);
            // 
            // btnSendText
            // 
            this.btnSendText.Location = new System.Drawing.Point(88, 161);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(75, 23);
            this.btnSendText.TabIndex = 93;
            this.btnSendText.Text = "文本";
            this.btnSendText.UseVisualStyleBackColor = true;
            this.btnSendText.Click += new System.EventHandler(this.btnSendText_Click);
            // 
            // rtxtMsg
            // 
            this.rtxtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtMsg.Location = new System.Drawing.Point(88, 36);
            this.rtxtMsg.Name = "rtxtMsg";
            this.rtxtMsg.Size = new System.Drawing.Size(678, 38);
            this.rtxtMsg.TabIndex = 92;
            this.rtxtMsg.Text = "2,3,1,100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 91;
            this.label1.Text = "消息";
            // 
            // logControl
            // 
            this.logControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logControl.IsLock = false;
            this.logControl.Location = new System.Drawing.Point(6, 189);
            this.logControl.MaxItemCount = 100;
            this.logControl.Name = "logControl";
            this.logControl.Size = new System.Drawing.Size(760, 368);
            this.logControl.TabIndex = 90;
            // 
            // cbDataWriteFile
            // 
            this.cbDataWriteFile.AutoSize = true;
            this.cbDataWriteFile.Location = new System.Drawing.Point(540, 117);
            this.cbDataWriteFile.Name = "cbDataWriteFile";
            this.cbDataWriteFile.Size = new System.Drawing.Size(96, 16);
            this.cbDataWriteFile.TabIndex = 121;
            this.cbDataWriteFile.Text = "数据写入文件";
            this.cbDataWriteFile.UseVisualStyleBackColor = true;
            // 
            // numPosition
            // 
            this.numPosition.Location = new System.Drawing.Point(473, 164);
            this.numPosition.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numPosition.Name = "numPosition";
            this.numPosition.Size = new System.Drawing.Size(82, 21);
            this.numPosition.TabIndex = 123;
            this.numPosition.ValueChanged += new System.EventHandler(this.numPosition_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(414, 168);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 122;
            this.label10.Text = "起始位置";
            // 
            // numLen
            // 
            this.numLen.Location = new System.Drawing.Point(603, 163);
            this.numLen.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numLen.Name = "numLen";
            this.numLen.Size = new System.Drawing.Size(82, 21);
            this.numLen.TabIndex = 125;
            this.numLen.ValueChanged += new System.EventHandler(this.numLen_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(568, 165);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 124;
            this.label12.Text = "长度";
            // 
            // txtDataFilePath
            // 
            this.txtDataFilePath.Location = new System.Drawing.Point(88, 136);
            this.txtDataFilePath.Name = "txtDataFilePath";
            this.txtDataFilePath.ReadOnly = true;
            this.txtDataFilePath.Size = new System.Drawing.Size(524, 21);
            this.txtDataFilePath.TabIndex = 127;
            this.txtDataFilePath.Text = "D:\\Projects\\powerdesigner16.5.rar";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(31, 141);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 126;
            this.label13.Text = "数据文件";
            // 
            // btnDataFilePath
            // 
            this.btnDataFilePath.Location = new System.Drawing.Point(618, 134);
            this.btnDataFilePath.Name = "btnDataFilePath";
            this.btnDataFilePath.Size = new System.Drawing.Size(33, 23);
            this.btnDataFilePath.TabIndex = 128;
            this.btnDataFilePath.Text = "...";
            this.btnDataFilePath.UseVisualStyleBackColor = true;
            this.btnDataFilePath.Click += new System.EventHandler(this.btnDataFilePath_Click);
            // 
            // FUdpNet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.btnDataFilePath);
            this.Controls.Add(this.txtDataFilePath);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.numLen);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.numPosition);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbDataWriteFile);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.comPri);
            this.Controls.Add(this.numTimeout);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numSendCount);
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
            this.Name = "FUdpNet";
            this.Text = "FUdpNet";
            this.Load += new System.EventHandler(this.FUdpNet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSendCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDstPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numParallelThreadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comPri;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numSendCount;
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
        private System.Windows.Forms.CheckBox cbDataWriteFile;
        private System.Windows.Forms.NumericUpDown numPosition;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numLen;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtDataFilePath;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnDataFilePath;
    }
}