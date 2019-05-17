namespace UtilZ.Dotnet.SHPAgent
{
    partial class FMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiNotifyIconOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNotifyIconExit = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvAppMonitor = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.logControl = new UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslStartTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslRuntime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslMemory = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslCPUUse = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslThreadCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslHandleCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmsAppMonitor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAppMonitorAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAppMonitorDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAppMonitorModify = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAppMonitorStart = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAppMonitorStop = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAppMonitorRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.checkBoxLockLog = new System.Windows.Forms.CheckBox();
            this.tsmiAppMonitorFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsNotifyIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.cmsAppMonitor.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.cmsNotifyIcon;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Agent";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // cmsNotifyIcon
            // 
            this.cmsNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNotifyIconOpen,
            this.tsmiNotifyIconExit});
            this.cmsNotifyIcon.Name = "cmsNotifyIcon";
            this.cmsNotifyIcon.Size = new System.Drawing.Size(101, 48);
            // 
            // tsmiNotifyIconOpen
            // 
            this.tsmiNotifyIconOpen.Name = "tsmiNotifyIconOpen";
            this.tsmiNotifyIconOpen.Size = new System.Drawing.Size(100, 22);
            this.tsmiNotifyIconOpen.Text = "打开";
            this.tsmiNotifyIconOpen.Click += new System.EventHandler(this.tsmiNotifyIconOpen_Click);
            // 
            // tsmiNotifyIconExit
            // 
            this.tsmiNotifyIconExit.Name = "tsmiNotifyIconExit";
            this.tsmiNotifyIconExit.Size = new System.Drawing.Size(100, 22);
            this.tsmiNotifyIconExit.Text = "退出";
            this.tsmiNotifyIconExit.Click += new System.EventHandler(this.tsmiNotifyIconExit_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgvAppMonitor);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.logControl);
            this.splitContainer.Size = new System.Drawing.Size(784, 540);
            this.splitContainer.SplitterDistance = 270;
            this.splitContainer.TabIndex = 2;
            // 
            // dgvAppMonitor
            // 
            this.dgvAppMonitor.AlignDirection = true;
            this.dgvAppMonitor.ColumnSettingStatus = WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Disable;
            this.dgvAppMonitor.ColumnSettingWidth = 20;
            this.dgvAppMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAppMonitor.FocusedRowIndex = -1;
            this.dgvAppMonitor.IsLastColumnAutoSizeModeFill = true;
            this.dgvAppMonitor.Location = new System.Drawing.Point(0, 0);
            this.dgvAppMonitor.Name = "dgvAppMonitor";
            this.dgvAppMonitor.PageSizeMaximum = 100;
            this.dgvAppMonitor.EnablePagingBar = false;
            this.dgvAppMonitor.EnableRowNum = true;
            this.dgvAppMonitor.Size = new System.Drawing.Size(784, 270);
            this.dgvAppMonitor.TabIndex = 0;
            // 
            // logControl
            // 
            this.logControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logControl.IsLock = false;
            this.logControl.Location = new System.Drawing.Point(0, 0);
            this.logControl.MaxItemCount = 100;
            this.logControl.Name = "logControl";
            this.logControl.Size = new System.Drawing.Size(784, 266);
            this.logControl.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsslStartTime,
            this.toolStripStatusLabel2,
            this.tsslRuntime,
            this.toolStripStatusLabel3,
            this.tsslMemory,
            this.toolStripStatusLabel5,
            this.tsslCPUUse,
            this.toolStripStatusLabel4,
            this.tsslThreadCount,
            this.toolStripStatusLabel7,
            this.tsslHandleCount});
            this.statusStrip.Location = new System.Drawing.Point(0, 540);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel1.Text = "启动时间:";
            // 
            // tsslStartTime
            // 
            this.tsslStartTime.Name = "tsslStartTime";
            this.tsslStartTime.Size = new System.Drawing.Size(119, 17);
            this.tsslStartTime.Text = "2018-3-21 09:11:12";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel2.Text = "运行时长:";
            // 
            // tsslRuntime
            // 
            this.tsslRuntime.Name = "tsslRuntime";
            this.tsslRuntime.Size = new System.Drawing.Size(96, 17);
            this.tsslRuntime.Text = "0天0小时0分0秒";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(35, 17);
            this.toolStripStatusLabel3.Text = "内存:";
            // 
            // tsslMemory
            // 
            this.tsslMemory.Name = "tsslMemory";
            this.tsslMemory.Size = new System.Drawing.Size(35, 17);
            this.tsslMemory.Text = "0MB";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(35, 17);
            this.toolStripStatusLabel5.Text = "CPU:";
            // 
            // tsslCPUUse
            // 
            this.tsslCPUUse.Name = "tsslCPUUse";
            this.tsslCPUUse.Size = new System.Drawing.Size(26, 17);
            this.tsslCPUUse.Text = "0%";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel4.Text = "线程数";
            // 
            // tsslThreadCount
            // 
            this.tsslThreadCount.Name = "tsslThreadCount";
            this.tsslThreadCount.Size = new System.Drawing.Size(22, 17);
            this.tsslThreadCount.Text = "15";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel7.Text = "句柄数";
            // 
            // tsslHandleCount
            // 
            this.tsslHandleCount.Name = "tsslHandleCount";
            this.tsslHandleCount.Size = new System.Drawing.Size(29, 17);
            this.tsslHandleCount.Text = "110";
            // 
            // cmsAppMonitor
            // 
            this.cmsAppMonitor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAppMonitorAdd,
            this.tsmiAppMonitorDelete,
            this.tsmiAppMonitorModify,
            this.tsmiAppMonitorStart,
            this.tsmiAppMonitorStop,
            this.tsmiAppMonitorRestart,
            this.tsmiAppMonitorFileLocation});
            this.cmsAppMonitor.Name = "cmsAppMonitor";
            this.cmsAppMonitor.Size = new System.Drawing.Size(153, 180);
            // 
            // tsmiAppMonitorAdd
            // 
            this.tsmiAppMonitorAdd.Name = "tsmiAppMonitorAdd";
            this.tsmiAppMonitorAdd.Size = new System.Drawing.Size(152, 22);
            this.tsmiAppMonitorAdd.Text = "添加";
            this.tsmiAppMonitorAdd.Click += new System.EventHandler(this.tsmiAppMonitorAdd_Click);
            // 
            // tsmiAppMonitorDelete
            // 
            this.tsmiAppMonitorDelete.Name = "tsmiAppMonitorDelete";
            this.tsmiAppMonitorDelete.Size = new System.Drawing.Size(152, 22);
            this.tsmiAppMonitorDelete.Text = "删除";
            this.tsmiAppMonitorDelete.Click += new System.EventHandler(this.tsmiAppMonitorDelete_Click);
            // 
            // tsmiAppMonitorModify
            // 
            this.tsmiAppMonitorModify.Name = "tsmiAppMonitorModify";
            this.tsmiAppMonitorModify.Size = new System.Drawing.Size(152, 22);
            this.tsmiAppMonitorModify.Text = "修改";
            this.tsmiAppMonitorModify.Click += new System.EventHandler(this.tsmiAppMonitorModify_Click);
            // 
            // tsmiAppMonitorStart
            // 
            this.tsmiAppMonitorStart.Name = "tsmiAppMonitorStart";
            this.tsmiAppMonitorStart.Size = new System.Drawing.Size(152, 22);
            this.tsmiAppMonitorStart.Text = "启动";
            this.tsmiAppMonitorStart.Click += new System.EventHandler(this.tsmiAppMonitorStart_Click);
            // 
            // tsmiAppMonitorStop
            // 
            this.tsmiAppMonitorStop.Name = "tsmiAppMonitorStop";
            this.tsmiAppMonitorStop.Size = new System.Drawing.Size(152, 22);
            this.tsmiAppMonitorStop.Text = "停止";
            this.tsmiAppMonitorStop.Click += new System.EventHandler(this.tsmiAppMonitorStop_Click);
            // 
            // tsmiAppMonitorRestart
            // 
            this.tsmiAppMonitorRestart.Name = "tsmiAppMonitorRestart";
            this.tsmiAppMonitorRestart.Size = new System.Drawing.Size(152, 22);
            this.tsmiAppMonitorRestart.Text = "重启";
            this.tsmiAppMonitorRestart.Click += new System.EventHandler(this.tsmiAppMonitorRestart_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Location = new System.Drawing.Point(737, 540);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(47, 22);
            this.btnClearLog.TabIndex = 9;
            this.btnClearLog.Text = "清空";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // checkBoxLockLog
            // 
            this.checkBoxLockLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxLockLog.AutoSize = true;
            this.checkBoxLockLog.Location = new System.Drawing.Point(684, 544);
            this.checkBoxLockLog.Name = "checkBoxLockLog";
            this.checkBoxLockLog.Size = new System.Drawing.Size(48, 16);
            this.checkBoxLockLog.TabIndex = 8;
            this.checkBoxLockLog.Text = "锁定";
            this.checkBoxLockLog.UseVisualStyleBackColor = true;
            this.checkBoxLockLog.CheckedChanged += new System.EventHandler(this.checkBoxLockLog_CheckedChanged);
            // 
            // tsmiAppMonitorFileLocation
            // 
            this.tsmiAppMonitorFileLocation.Name = "tsmiAppMonitorFileLocation";
            this.tsmiAppMonitorFileLocation.Size = new System.Drawing.Size(152, 22);
            this.tsmiAppMonitorFileLocation.Text = "打开文件位置";
            this.tsmiAppMonitorFileLocation.Click += new System.EventHandler(this.tsmiAppMonitorFileLocation_Click);
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.checkBoxLockLog);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agent";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FMain_FormClosed);
            this.Load += new System.EventHandler(this.FMain_Load);
            this.cmsNotifyIcon.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.cmsAppMonitor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip cmsNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem tsmiNotifyIconOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmiNotifyIconExit;
        private System.Windows.Forms.SplitContainer splitContainer;
        private WindowEx.Winform.Controls.LogControlF logControl;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsslStartTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsslRuntime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tsslMemory;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel tsslCPUUse;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tsslThreadCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel tsslHandleCount;
        private System.Windows.Forms.ContextMenuStrip cmsAppMonitor;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppMonitorAdd;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppMonitorDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppMonitorModify;
        private WindowEx.Winform.Controls.PageGrid.UCPageGridControl dgvAppMonitor;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppMonitorStart;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppMonitorStop;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppMonitorRestart;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.CheckBox checkBoxLockLog;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppMonitorFileLocation;
    }
}

