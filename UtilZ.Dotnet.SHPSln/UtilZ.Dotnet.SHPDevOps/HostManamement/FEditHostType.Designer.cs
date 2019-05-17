namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    partial class FEditHostType
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
            this.txtHostTypeDes = new System.Windows.Forms.TextBox();
            this.lbHostDes = new System.Windows.Forms.Label();
            this.txtHostTypeName = new System.Windows.Forms.TextBox();
            this.lbHostTypeName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(217, 62);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancell
            // 
            this.btnCancell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancell.Location = new System.Drawing.Point(297, 62);
            this.btnCancell.Name = "btnCancell";
            this.btnCancell.Size = new System.Drawing.Size(75, 23);
            this.btnCancell.TabIndex = 19;
            this.btnCancell.Text = "取消";
            this.btnCancell.UseVisualStyleBackColor = true;
            this.btnCancell.Click += new System.EventHandler(this.btnCancell_Click);
            // 
            // txtHostTypeDes
            // 
            this.txtHostTypeDes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHostTypeDes.Location = new System.Drawing.Point(72, 35);
            this.txtHostTypeDes.Name = "txtHostTypeDes";
            this.txtHostTypeDes.Size = new System.Drawing.Size(300, 21);
            this.txtHostTypeDes.TabIndex = 18;
            // 
            // lbHostDes
            // 
            this.lbHostDes.AutoSize = true;
            this.lbHostDes.Location = new System.Drawing.Point(13, 40);
            this.lbHostDes.Name = "lbHostDes";
            this.lbHostDes.Size = new System.Drawing.Size(53, 12);
            this.lbHostDes.TabIndex = 17;
            this.lbHostDes.Text = "类型描述";
            // 
            // txtHostTypeName
            // 
            this.txtHostTypeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHostTypeName.Location = new System.Drawing.Point(72, 8);
            this.txtHostTypeName.Name = "txtHostTypeName";
            this.txtHostTypeName.Size = new System.Drawing.Size(300, 21);
            this.txtHostTypeName.TabIndex = 16;
            // 
            // lbHostTypeName
            // 
            this.lbHostTypeName.AutoSize = true;
            this.lbHostTypeName.Location = new System.Drawing.Point(13, 12);
            this.lbHostTypeName.Name = "lbHostTypeName";
            this.lbHostTypeName.Size = new System.Drawing.Size(53, 12);
            this.lbHostTypeName.TabIndex = 15;
            this.lbHostTypeName.Text = "类型名称";
            // 
            // FEditHostType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 92);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancell);
            this.Controls.Add(this.txtHostTypeDes);
            this.Controls.Add(this.lbHostDes);
            this.Controls.Add(this.txtHostTypeName);
            this.Controls.Add(this.lbHostTypeName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FEditHostType";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑主机类型";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancell;
        private System.Windows.Forms.TextBox txtHostTypeDes;
        private System.Windows.Forms.Label lbHostDes;
        private System.Windows.Forms.TextBox txtHostTypeName;
        private System.Windows.Forms.Label lbHostTypeName;
    }
}