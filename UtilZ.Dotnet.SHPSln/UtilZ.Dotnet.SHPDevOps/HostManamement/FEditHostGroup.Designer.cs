namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    partial class FEditHostGroup
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancell = new System.Windows.Forms.Button();
            this.txtHostGroupDes = new System.Windows.Forms.TextBox();
            this.lbHostIp = new System.Windows.Forms.Label();
            this.txtHostGroupName = new System.Windows.Forms.TextBox();
            this.lbHostName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(217, 66);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 14;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancell
            // 
            this.btnCancell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancell.Location = new System.Drawing.Point(297, 66);
            this.btnCancell.Name = "btnCancell";
            this.btnCancell.Size = new System.Drawing.Size(75, 23);
            this.btnCancell.TabIndex = 13;
            this.btnCancell.Text = "取消";
            this.btnCancell.UseVisualStyleBackColor = true;
            this.btnCancell.Click += new System.EventHandler(this.btnCancell_Click);
            // 
            // txtHostGroupDes
            // 
            this.txtHostGroupDes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHostGroupDes.Location = new System.Drawing.Point(72, 39);
            this.txtHostGroupDes.Name = "txtHostGroupDes";
            this.txtHostGroupDes.Size = new System.Drawing.Size(300, 21);
            this.txtHostGroupDes.TabIndex = 12;
            // 
            // lbHostIp
            // 
            this.lbHostIp.AutoSize = true;
            this.lbHostIp.Location = new System.Drawing.Point(13, 44);
            this.lbHostIp.Name = "lbHostIp";
            this.lbHostIp.Size = new System.Drawing.Size(53, 12);
            this.lbHostIp.TabIndex = 11;
            this.lbHostIp.Text = "分组描述";
            // 
            // txtHostGroupName
            // 
            this.txtHostGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHostGroupName.Location = new System.Drawing.Point(72, 12);
            this.txtHostGroupName.Name = "txtHostGroupName";
            this.txtHostGroupName.Size = new System.Drawing.Size(300, 21);
            this.txtHostGroupName.TabIndex = 10;
            // 
            // lbHostName
            // 
            this.lbHostName.AutoSize = true;
            this.lbHostName.Location = new System.Drawing.Point(13, 16);
            this.lbHostName.Name = "lbHostName";
            this.lbHostName.Size = new System.Drawing.Size(53, 12);
            this.lbHostName.TabIndex = 9;
            this.lbHostName.Text = "分组名称";
            // 
            // FEditHostGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 92);
            this.ControlBox = false;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancell);
            this.Controls.Add(this.txtHostGroupDes);
            this.Controls.Add(this.lbHostIp);
            this.Controls.Add(this.txtHostGroupName);
            this.Controls.Add(this.lbHostName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "FEditHostGroup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑分组";
            this.Load += new System.EventHandler(this.FEditHostGroup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancell;
        private System.Windows.Forms.TextBox txtHostGroupDes;
        private System.Windows.Forms.Label lbHostIp;
        private System.Windows.Forms.TextBox txtHostGroupName;
        private System.Windows.Forms.Label lbHostName;
    }
}