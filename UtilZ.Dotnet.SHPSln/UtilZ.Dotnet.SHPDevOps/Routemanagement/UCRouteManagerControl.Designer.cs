namespace UtilZ.Dotnet.SHPDevOps.Routemanagement
{
    partial class UCRouteManagerControl
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDataRoute = new System.Windows.Forms.TabPage();
            this.dgvDataRoute = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.tabPageServiceRoute = new System.Windows.Forms.TabPage();
            this.dgvServiceIns = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.cmsDataRoute = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDataRouteAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDataRouteDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDataRouteModify = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDataRouteClear = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsServiceIns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiServiceInsDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPageDataRoute.SuspendLayout();
            this.tabPageServiceRoute.SuspendLayout();
            this.cmsDataRoute.SuspendLayout();
            this.cmsServiceIns.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageDataRoute);
            this.tabControl1.Controls.Add(this.tabPageServiceRoute);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(700, 500);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageDataRoute
            // 
            this.tabPageDataRoute.Controls.Add(this.dgvDataRoute);
            this.tabPageDataRoute.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataRoute.Name = "tabPageDataRoute";
            this.tabPageDataRoute.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataRoute.Size = new System.Drawing.Size(692, 474);
            this.tabPageDataRoute.TabIndex = 0;
            this.tabPageDataRoute.Text = "数据路由";
            this.tabPageDataRoute.UseVisualStyleBackColor = true;
            // 
            // dgvDataRoute
            // 
            this.dgvDataRoute.AlignDirection = true;
            this.dgvDataRoute.ColumnSettingStatus = UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Disable;
            this.dgvDataRoute.ColumnSettingWidth = 20;
            this.dgvDataRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDataRoute.EnableColumnHeaderContextMenuStripHiden = true;
            this.dgvDataRoute.EnablePagingBar = false;
            this.dgvDataRoute.EnableRowNum = true;
            this.dgvDataRoute.EnableUserAdjustPageSize = true;
            this.dgvDataRoute.FocusedRowIndex = -1;
            this.dgvDataRoute.IsLastColumnAutoSizeModeFill = true;
            this.dgvDataRoute.Location = new System.Drawing.Point(3, 3);
            this.dgvDataRoute.Name = "dgvDataRoute";
            this.dgvDataRoute.PageSizeMaximum = 100;
            this.dgvDataRoute.PageSizeMinimum = 1;
            this.dgvDataRoute.Size = new System.Drawing.Size(686, 468);
            this.dgvDataRoute.TabIndex = 0;
            // 
            // tabPageServiceRoute
            // 
            this.tabPageServiceRoute.Controls.Add(this.dgvServiceIns);
            this.tabPageServiceRoute.Location = new System.Drawing.Point(4, 22);
            this.tabPageServiceRoute.Name = "tabPageServiceRoute";
            this.tabPageServiceRoute.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServiceRoute.Size = new System.Drawing.Size(692, 474);
            this.tabPageServiceRoute.TabIndex = 1;
            this.tabPageServiceRoute.Text = "服务实例";
            this.tabPageServiceRoute.UseVisualStyleBackColor = true;
            // 
            // dgvServiceIns
            // 
            this.dgvServiceIns.AlignDirection = true;
            this.dgvServiceIns.ColumnSettingStatus = UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Hiden;
            this.dgvServiceIns.ColumnSettingWidth = 20;
            this.dgvServiceIns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServiceIns.EnableColumnHeaderContextMenuStripHiden = true;
            this.dgvServiceIns.EnablePagingBar = false;
            this.dgvServiceIns.EnableRowNum = true;
            this.dgvServiceIns.EnableUserAdjustPageSize = true;
            this.dgvServiceIns.FocusedRowIndex = -1;
            this.dgvServiceIns.IsLastColumnAutoSizeModeFill = true;
            this.dgvServiceIns.Location = new System.Drawing.Point(3, 3);
            this.dgvServiceIns.Name = "dgvServiceIns";
            this.dgvServiceIns.PageSizeMaximum = 100;
            this.dgvServiceIns.PageSizeMinimum = 1;
            this.dgvServiceIns.Size = new System.Drawing.Size(686, 468);
            this.dgvServiceIns.TabIndex = 1;
            // 
            // cmsDataRoute
            // 
            this.cmsDataRoute.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDataRouteAdd,
            this.tsmiDataRouteDelete,
            this.tsmiDataRouteModify,
            this.tsmiDataRouteClear});
            this.cmsDataRoute.Name = "cmsDataRoute";
            this.cmsDataRoute.Size = new System.Drawing.Size(125, 92);
            // 
            // tsmiDataRouteAdd
            // 
            this.tsmiDataRouteAdd.Name = "tsmiDataRouteAdd";
            this.tsmiDataRouteAdd.Size = new System.Drawing.Size(124, 22);
            this.tsmiDataRouteAdd.Text = "添加路由";
            this.tsmiDataRouteAdd.Click += new System.EventHandler(this.tsmiDataRouteAdd_Click);
            // 
            // tsmiDataRouteDelete
            // 
            this.tsmiDataRouteDelete.Name = "tsmiDataRouteDelete";
            this.tsmiDataRouteDelete.Size = new System.Drawing.Size(124, 22);
            this.tsmiDataRouteDelete.Text = "删除路由";
            this.tsmiDataRouteDelete.Click += new System.EventHandler(this.tsmiDataRouteDelete_Click);
            // 
            // tsmiDataRouteModify
            // 
            this.tsmiDataRouteModify.Name = "tsmiDataRouteModify";
            this.tsmiDataRouteModify.Size = new System.Drawing.Size(124, 22);
            this.tsmiDataRouteModify.Text = "修改路由";
            this.tsmiDataRouteModify.Click += new System.EventHandler(this.tsmiDataRouteModify_Click);
            // 
            // tsmiDataRouteClear
            // 
            this.tsmiDataRouteClear.Name = "tsmiDataRouteClear";
            this.tsmiDataRouteClear.Size = new System.Drawing.Size(124, 22);
            this.tsmiDataRouteClear.Text = "清空路由";
            this.tsmiDataRouteClear.Click += new System.EventHandler(this.tsmiDataRouteClear_Click);
            // 
            // cmsServiceIns
            // 
            this.cmsServiceIns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiServiceInsDelete});
            this.cmsServiceIns.Name = "cmsServiceIns";
            this.cmsServiceIns.Size = new System.Drawing.Size(153, 48);
            // 
            // tsmiServiceInsDelete
            // 
            this.tsmiServiceInsDelete.Name = "tsmiServiceInsDelete";
            this.tsmiServiceInsDelete.Size = new System.Drawing.Size(152, 22);
            this.tsmiServiceInsDelete.Text = "删除";
            this.tsmiServiceInsDelete.Click += new System.EventHandler(this.tsmiServiceInsDelete_Click);
            // 
            // UCRouteManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "UCRouteManagerControl";
            this.Size = new System.Drawing.Size(700, 500);
            this.Load += new System.EventHandler(this.UCServiceManagerControl_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageDataRoute.ResumeLayout(false);
            this.tabPageServiceRoute.ResumeLayout(false);
            this.cmsDataRoute.ResumeLayout(false);
            this.cmsServiceIns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDataRoute;
        private WindowEx.Winform.Controls.PageGrid.UCPageGridControl dgvDataRoute;
        private System.Windows.Forms.TabPage tabPageServiceRoute;
        private WindowEx.Winform.Controls.PageGrid.UCPageGridControl dgvServiceIns;
        private System.Windows.Forms.ContextMenuStrip cmsDataRoute;
        private System.Windows.Forms.ToolStripMenuItem tsmiDataRouteAdd;
        private System.Windows.Forms.ToolStripMenuItem tsmiDataRouteDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiDataRouteModify;
        private System.Windows.Forms.ToolStripMenuItem tsmiDataRouteClear;
        private System.Windows.Forms.ContextMenuStrip cmsServiceIns;
        private System.Windows.Forms.ToolStripMenuItem tsmiServiceInsDelete;
    }
}
