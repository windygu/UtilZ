namespace UtilZ.Dotnet.SHPDevOps.ServiceManagement
{
    partial class UCServiceManagerControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvServiceInfo = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.cmsServiceInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiServiceInfoAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServiceInfoDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServiceInfoModify = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServiceInfoClear = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvServiceMirror = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtxtVersionDes = new System.Windows.Forms.RichTextBox();
            this.cmsServiceMirror = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiServiceMirrorUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServiceMirrorUsage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServiceMirrorDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServiceInfoDeploy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsServiceInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.cmsServiceMirror.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvServiceInfo
            // 
            this.dgvServiceInfo.AlignDirection = true;
            this.dgvServiceInfo.ColumnSettingStatus = UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Disable;
            this.dgvServiceInfo.ColumnSettingWidth = 20;
            this.dgvServiceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServiceInfo.EnableColumnHeaderContextMenuStripHiden = true;
            this.dgvServiceInfo.EnablePagingBar = false;
            this.dgvServiceInfo.EnableRowNum = true;
            this.dgvServiceInfo.EnableUserAdjustPageSize = true;
            this.dgvServiceInfo.FocusedRowIndex = -1;
            this.dgvServiceInfo.IsLastColumnAutoSizeModeFill = true;
            this.dgvServiceInfo.Location = new System.Drawing.Point(0, 0);
            this.dgvServiceInfo.Name = "dgvServiceInfo";
            this.dgvServiceInfo.PageSizeMaximum = 100;
            this.dgvServiceInfo.PageSizeMinimum = 1;
            this.dgvServiceInfo.Size = new System.Drawing.Size(427, 500);
            this.dgvServiceInfo.TabIndex = 2;
            this.dgvServiceInfo.DataRowSelectionChanged += new System.EventHandler<UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.DataRowSelectionChangedArgs>(this.dgvServiceInfo_DataRowSelectionChanged);
            // 
            // cmsServiceInfo
            // 
            this.cmsServiceInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiServiceInfoAdd,
            this.tsmiServiceInfoDelete,
            this.tsmiServiceInfoModify,
            this.tsmiServiceInfoClear,
            this.tsmiServiceInfoDeploy});
            this.cmsServiceInfo.Name = "cmsServiceRoute";
            this.cmsServiceInfo.Size = new System.Drawing.Size(153, 136);
            // 
            // tsmiServiceInfoAdd
            // 
            this.tsmiServiceInfoAdd.Name = "tsmiServiceInfoAdd";
            this.tsmiServiceInfoAdd.Size = new System.Drawing.Size(152, 22);
            this.tsmiServiceInfoAdd.Text = "添加服务";
            this.tsmiServiceInfoAdd.Click += new System.EventHandler(this.tsmiServiceTypeAdd_Click);
            // 
            // tsmiServiceInfoDelete
            // 
            this.tsmiServiceInfoDelete.Name = "tsmiServiceInfoDelete";
            this.tsmiServiceInfoDelete.Size = new System.Drawing.Size(152, 22);
            this.tsmiServiceInfoDelete.Text = "删除服务";
            this.tsmiServiceInfoDelete.Click += new System.EventHandler(this.tsmiServiceTypeDelete_Click);
            // 
            // tsmiServiceInfoModify
            // 
            this.tsmiServiceInfoModify.Name = "tsmiServiceInfoModify";
            this.tsmiServiceInfoModify.Size = new System.Drawing.Size(152, 22);
            this.tsmiServiceInfoModify.Text = "修改服务";
            this.tsmiServiceInfoModify.Click += new System.EventHandler(this.tsmiServiceTypeModify_Click);
            // 
            // tsmiServiceInfoClear
            // 
            this.tsmiServiceInfoClear.Name = "tsmiServiceInfoClear";
            this.tsmiServiceInfoClear.Size = new System.Drawing.Size(152, 22);
            this.tsmiServiceInfoClear.Text = "清空服务";
            this.tsmiServiceInfoClear.Click += new System.EventHandler(this.tsmiServiceTypeClear_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvServiceInfo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(700, 500);
            this.splitContainer1.SplitterDistance = 427;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(269, 500);
            this.splitContainer2.SplitterDistance = 184;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvServiceMirror);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 184);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务镜像列表";
            // 
            // dgvServiceMirror
            // 
            this.dgvServiceMirror.AlignDirection = true;
            this.dgvServiceMirror.ColumnSettingStatus = UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Hiden;
            this.dgvServiceMirror.ColumnSettingWidth = 20;
            this.dgvServiceMirror.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServiceMirror.EnableColumnHeaderContextMenuStripHiden = true;
            this.dgvServiceMirror.EnablePagingBar = false;
            this.dgvServiceMirror.EnableRowNum = true;
            this.dgvServiceMirror.EnableUserAdjustPageSize = true;
            this.dgvServiceMirror.FocusedRowIndex = -1;
            this.dgvServiceMirror.IsLastColumnAutoSizeModeFill = true;
            this.dgvServiceMirror.Location = new System.Drawing.Point(3, 17);
            this.dgvServiceMirror.Name = "dgvServiceMirror";
            this.dgvServiceMirror.PageSizeMaximum = 100;
            this.dgvServiceMirror.PageSizeMinimum = 1;
            this.dgvServiceMirror.Size = new System.Drawing.Size(263, 164);
            this.dgvServiceMirror.TabIndex = 3;
            this.dgvServiceMirror.DataRowSelectionChanged += new System.EventHandler<UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.DataRowSelectionChangedArgs>(this.dgvServiceVersion_DataRowSelectionChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtxtVersionDes);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(269, 312);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "镜像描述";
            // 
            // rtxtVersionDes
            // 
            this.rtxtVersionDes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtVersionDes.Location = new System.Drawing.Point(3, 17);
            this.rtxtVersionDes.Name = "rtxtVersionDes";
            this.rtxtVersionDes.ReadOnly = true;
            this.rtxtVersionDes.Size = new System.Drawing.Size(263, 292);
            this.rtxtVersionDes.TabIndex = 0;
            this.rtxtVersionDes.Text = "";
            // 
            // cmsServiceMirror
            // 
            this.cmsServiceMirror.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiServiceMirrorUpload,
            this.tsmiServiceMirrorUsage,
            this.tsmiServiceMirrorDelete});
            this.cmsServiceMirror.Name = "cmsServiceMirror";
            this.cmsServiceMirror.Size = new System.Drawing.Size(125, 70);
            // 
            // tsmiServiceMirrorUpload
            // 
            this.tsmiServiceMirrorUpload.Name = "tsmiServiceMirrorUpload";
            this.tsmiServiceMirrorUpload.Size = new System.Drawing.Size(124, 22);
            this.tsmiServiceMirrorUpload.Text = "上传镜像";
            this.tsmiServiceMirrorUpload.Click += new System.EventHandler(this.tsmiServiceMirrorUpload_Click);
            // 
            // tsmiServiceMirrorUsage
            // 
            this.tsmiServiceMirrorUsage.Name = "tsmiServiceMirrorUsage";
            this.tsmiServiceMirrorUsage.Size = new System.Drawing.Size(124, 22);
            this.tsmiServiceMirrorUsage.Text = "使用镜像";
            this.tsmiServiceMirrorUsage.Click += new System.EventHandler(this.tsmiServiceMirrorUsage_Click);
            // 
            // tsmiServiceMirrorDelete
            // 
            this.tsmiServiceMirrorDelete.Name = "tsmiServiceMirrorDelete";
            this.tsmiServiceMirrorDelete.Size = new System.Drawing.Size(124, 22);
            this.tsmiServiceMirrorDelete.Text = "删除";
            this.tsmiServiceMirrorDelete.Click += new System.EventHandler(this.tsmiServiceMirrorDelete_Click);
            // 
            // tsmiServiceInfoDeploy
            // 
            this.tsmiServiceInfoDeploy.Name = "tsmiServiceInfoDeploy";
            this.tsmiServiceInfoDeploy.Size = new System.Drawing.Size(152, 22);
            this.tsmiServiceInfoDeploy.Text = "部署服务";
            this.tsmiServiceInfoDeploy.Click += new System.EventHandler(this.tsmiServiceInfoDeploy_Click);
            // 
            // UCServiceManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UCServiceManagerControl";
            this.Size = new System.Drawing.Size(700, 500);
            this.Load += new System.EventHandler(this.UCServiceManagerControl_Load);
            this.cmsServiceInfo.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.cmsServiceMirror.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WindowEx.Winform.Controls.PageGrid.UCPageGridControl dgvServiceInfo;
        private System.Windows.Forms.ContextMenuStrip cmsServiceInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceInfoAdd;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceInfoDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceInfoModify;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceInfoClear;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox rtxtVersionDes;
        private WindowEx.Winform.Controls.PageGrid.UCPageGridControl dgvServiceMirror;
        private System.Windows.Forms.ContextMenuStrip cmsServiceMirror;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceMirrorDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceMirrorUsage;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceMirrorUpload;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceInfoDeploy;
    }
}
