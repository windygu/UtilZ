namespace UtilZ.Dotnet.SHPDevOps.Setting
{
    partial class UCSettingControl
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
            this.btnDevOpsMigrate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDevOpsMigrate
            // 
            this.btnDevOpsMigrate.Location = new System.Drawing.Point(18, 16);
            this.btnDevOpsMigrate.Name = "btnDevOpsMigrate";
            this.btnDevOpsMigrate.Size = new System.Drawing.Size(75, 23);
            this.btnDevOpsMigrate.TabIndex = 0;
            this.btnDevOpsMigrate.Text = "运控迁移";
            this.btnDevOpsMigrate.UseVisualStyleBackColor = true;
            this.btnDevOpsMigrate.Click += new System.EventHandler(this.btnDevOpsMigrate_Click);
            // 
            // UCSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDevOpsMigrate);
            this.Name = "UCSettingControl";
            this.Size = new System.Drawing.Size(369, 246);
            this.Load += new System.EventHandler(this.UCSettingControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDevOpsMigrate;
    }
}
