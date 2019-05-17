namespace UtilZ.Dotnet.SHPDevOps.Base
{
    partial class UCHostTreeControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCHostTreeControl));
            this.tvHost = new System.Windows.Forms.TreeView();
            this.imageListTreeViewHost = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tvHost
            // 
            this.tvHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvHost.FullRowSelect = true;
            this.tvHost.ImageIndex = 0;
            this.tvHost.ImageList = this.imageListTreeViewHost;
            this.tvHost.Location = new System.Drawing.Point(0, 0);
            this.tvHost.Name = "tvHost";
            this.tvHost.SelectedImageIndex = 0;
            this.tvHost.Size = new System.Drawing.Size(399, 340);
            this.tvHost.TabIndex = 1;
            // 
            // imageListTreeViewHost
            // 
            this.imageListTreeViewHost.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeViewHost.ImageStream")));
            this.imageListTreeViewHost.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeViewHost.Images.SetKeyName(0, "Group.png");
            this.imageListTreeViewHost.Images.SetKeyName(1, "Host.png");
            this.imageListTreeViewHost.Images.SetKeyName(2, "OffLine.png");
            // 
            // UCHostTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvHost);
            this.Name = "UCHostTreeControl";
            this.Size = new System.Drawing.Size(399, 340);
            this.Load += new System.EventHandler(this.UCHostTreeControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TreeView tvHost;
        private System.Windows.Forms.ImageList imageListTreeViewHost;
    }
}
