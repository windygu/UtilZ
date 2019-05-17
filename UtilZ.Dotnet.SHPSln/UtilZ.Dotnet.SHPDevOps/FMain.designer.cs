using UtilZ.Dotnet.SHPDevOps.HostManamement;

namespace UtilZ.Dotnet.SHPDevOps
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageHost = new System.Windows.Forms.TabPage();
            this.ucHostManager = new UtilZ.Dotnet.SHPDevOps.HostManamement.UCHostManagerControl();
            this.tabPageService = new System.Windows.Forms.TabPage();
            this.ucServiceManagerControl = new UtilZ.Dotnet.SHPDevOps.ServiceManagement.UCServiceManagerControl();
            this.tabPageRoute = new System.Windows.Forms.TabPage();
            this.ucRouteManagerControl = new UtilZ.Dotnet.SHPDevOps.Routemanagement.UCRouteManagerControl();
            this.tabPageExtend = new System.Windows.Forms.TabPage();
            this.tabControlExtend = new System.Windows.Forms.TabControl();
            this.tabPageSetting = new System.Windows.Forms.TabPage();
            this.ucSettingControl = new UtilZ.Dotnet.SHPDevOps.Setting.UCSettingControl();
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
            this.checkBoxLockLog = new System.Windows.Forms.CheckBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiNotifyIconOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNotifyIconExit = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageHost.SuspendLayout();
            this.tabPageService.SuspendLayout();
            this.tabPageRoute.SuspendLayout();
            this.tabPageExtend.SuspendLayout();
            this.tabPageSetting.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.cmsNotifyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.logControl);
            this.splitContainer1.Size = new System.Drawing.Size(984, 740);
            this.splitContainer1.SplitterDistance = 567;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageHost);
            this.tabControl.Controls.Add(this.tabPageService);
            this.tabControl.Controls.Add(this.tabPageRoute);
            this.tabControl.Controls.Add(this.tabPageExtend);
            this.tabControl.Controls.Add(this.tabPageSetting);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(984, 567);
            this.tabControl.TabIndex = 1;
            // 
            // tabPageHost
            // 
            this.tabPageHost.Controls.Add(this.ucHostManager);
            this.tabPageHost.Location = new System.Drawing.Point(4, 22);
            this.tabPageHost.Name = "tabPageHost";
            this.tabPageHost.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHost.Size = new System.Drawing.Size(976, 541);
            this.tabPageHost.TabIndex = 0;
            this.tabPageHost.Text = "主机";
            this.tabPageHost.UseVisualStyleBackColor = true;
            // 
            // ucHostManager
            // 
            this.ucHostManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHostManager.Location = new System.Drawing.Point(3, 3);
            this.ucHostManager.Name = "ucHostManager";
            this.ucHostManager.Size = new System.Drawing.Size(970, 535);
            this.ucHostManager.TabIndex = 0;
            // 
            // tabPageService
            // 
            this.tabPageService.Controls.Add(this.ucServiceManagerControl);
            this.tabPageService.Location = new System.Drawing.Point(4, 22);
            this.tabPageService.Name = "tabPageService";
            this.tabPageService.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageService.Size = new System.Drawing.Size(976, 541);
            this.tabPageService.TabIndex = 1;
            this.tabPageService.Text = "服务";
            this.tabPageService.UseVisualStyleBackColor = true;
            // 
            // ucServiceManagerControl
            // 
            this.ucServiceManagerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucServiceManagerControl.Location = new System.Drawing.Point(3, 3);
            this.ucServiceManagerControl.Name = "ucServiceManagerControl";
            this.ucServiceManagerControl.Size = new System.Drawing.Size(970, 535);
            this.ucServiceManagerControl.TabIndex = 0;
            // 
            // tabPageRoute
            // 
            this.tabPageRoute.Controls.Add(this.ucRouteManagerControl);
            this.tabPageRoute.Location = new System.Drawing.Point(4, 22);
            this.tabPageRoute.Name = "tabPageRoute";
            this.tabPageRoute.Size = new System.Drawing.Size(976, 541);
            this.tabPageRoute.TabIndex = 2;
            this.tabPageRoute.Text = "路由";
            this.tabPageRoute.UseVisualStyleBackColor = true;
            // 
            // ucRouteManagerControl
            // 
            this.ucRouteManagerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRouteManagerControl.Location = new System.Drawing.Point(0, 0);
            this.ucRouteManagerControl.Name = "ucRouteManagerControl";
            this.ucRouteManagerControl.Size = new System.Drawing.Size(976, 541);
            this.ucRouteManagerControl.TabIndex = 0;
            // 
            // tabPageExtend
            // 
            this.tabPageExtend.Controls.Add(this.tabControlExtend);
            this.tabPageExtend.Location = new System.Drawing.Point(4, 22);
            this.tabPageExtend.Name = "tabPageExtend";
            this.tabPageExtend.Size = new System.Drawing.Size(976, 541);
            this.tabPageExtend.TabIndex = 3;
            this.tabPageExtend.Text = "扩展";
            this.tabPageExtend.UseVisualStyleBackColor = true;
            // 
            // tabControlExtend
            // 
            this.tabControlExtend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlExtend.Location = new System.Drawing.Point(0, 0);
            this.tabControlExtend.Name = "tabControlExtend";
            this.tabControlExtend.SelectedIndex = 0;
            this.tabControlExtend.Size = new System.Drawing.Size(976, 541);
            this.tabControlExtend.TabIndex = 1;
            // 
            // tabPageSetting
            // 
            this.tabPageSetting.Controls.Add(this.ucSettingControl);
            this.tabPageSetting.Location = new System.Drawing.Point(4, 22);
            this.tabPageSetting.Name = "tabPageSetting";
            this.tabPageSetting.Size = new System.Drawing.Size(976, 541);
            this.tabPageSetting.TabIndex = 4;
            this.tabPageSetting.Text = "设置";
            this.tabPageSetting.UseVisualStyleBackColor = true;
            // 
            // ucSettingControl
            // 
            this.ucSettingControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSettingControl.Location = new System.Drawing.Point(0, 0);
            this.ucSettingControl.Name = "ucSettingControl";
            this.ucSettingControl.Size = new System.Drawing.Size(976, 541);
            this.ucSettingControl.TabIndex = 0;
            // 
            // logControl
            // 
            this.logControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logControl.IsLock = false;
            this.logControl.Location = new System.Drawing.Point(0, 0);
            this.logControl.MaxItemCount = 100;
            this.logControl.Name = "logControl";
            this.logControl.Size = new System.Drawing.Size(984, 169);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 740);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(984, 22);
            this.statusStrip.TabIndex = 5;
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
            // checkBoxLockLog
            // 
            this.checkBoxLockLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxLockLog.AutoSize = true;
            this.checkBoxLockLog.Location = new System.Drawing.Point(883, 744);
            this.checkBoxLockLog.Name = "checkBoxLockLog";
            this.checkBoxLockLog.Size = new System.Drawing.Size(48, 16);
            this.checkBoxLockLog.TabIndex = 6;
            this.checkBoxLockLog.Text = "锁定";
            this.checkBoxLockLog.UseVisualStyleBackColor = true;
            this.checkBoxLockLog.CheckedChanged += new System.EventHandler(this.checkBoxLockLog_CheckedChanged);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Location = new System.Drawing.Point(936, 740);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(47, 22);
            this.btnClearLog.TabIndex = 7;
            this.btnClearLog.Text = "清空";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.cmsNotifyIcon;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon";
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
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 762);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.checkBoxLockLog);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SHP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FMain_FormClosed);
            this.Load += new System.EventHandler(this.FMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageHost.ResumeLayout(false);
            this.tabPageService.ResumeLayout(false);
            this.tabPageRoute.ResumeLayout(false);
            this.tabPageExtend.ResumeLayout(false);
            this.tabPageSetting.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.cmsNotifyIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UCHostManagerControl ucHostManager;
        private System.Windows.Forms.SplitContainer splitContainer1;
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
        private System.Windows.Forms.CheckBox checkBoxLockLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageHost;
        private System.Windows.Forms.TabPage tabPageService;
        private System.Windows.Forms.TabPage tabPageRoute;
        private System.Windows.Forms.TabPage tabPageExtend;
        private System.Windows.Forms.TabControl tabControlExtend;
        private ServiceManagement.UCServiceManagerControl ucServiceManagerControl;
        private Routemanagement.UCRouteManagerControl ucRouteManagerControl;
        private System.Windows.Forms.TabPage tabPageSetting;
        private Setting.UCSettingControl ucSettingControl;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip cmsNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem tsmiNotifyIconOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmiNotifyIconExit;
    }
}

