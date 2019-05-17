namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    partial class FEditHost
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
            this.components = new System.ComponentModel.Container();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancell = new System.Windows.Forms.Button();
            this.txtHostIp = new System.Windows.Forms.TextBox();
            this.lbHostIp = new System.Windows.Forms.Label();
            this.txtHostName = new System.Windows.Forms.TextBox();
            this.lbHostName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxHostType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvDisablePort = new UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.UCPageGridControl();
            this.cmsDisablePort = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDisablePortAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisablePortModify = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisablePortDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDisablePortClear = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.cmsDisablePort.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(217, 282);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancell
            // 
            this.btnCancell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancell.Location = new System.Drawing.Point(297, 282);
            this.btnCancell.Name = "btnCancell";
            this.btnCancell.Size = new System.Drawing.Size(75, 23);
            this.btnCancell.TabIndex = 19;
            this.btnCancell.Text = "取消";
            this.btnCancell.UseVisualStyleBackColor = true;
            this.btnCancell.Click += new System.EventHandler(this.btnCancell_Click);
            // 
            // txtHostIp
            // 
            this.txtHostIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHostIp.Location = new System.Drawing.Point(72, 35);
            this.txtHostIp.Name = "txtHostIp";
            this.txtHostIp.Size = new System.Drawing.Size(300, 21);
            this.txtHostIp.TabIndex = 18;
            // 
            // lbHostIp
            // 
            this.lbHostIp.AutoSize = true;
            this.lbHostIp.Location = new System.Drawing.Point(25, 40);
            this.lbHostIp.Name = "lbHostIp";
            this.lbHostIp.Size = new System.Drawing.Size(41, 12);
            this.lbHostIp.TabIndex = 17;
            this.lbHostIp.Text = "主机IP";
            // 
            // txtHostName
            // 
            this.txtHostName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHostName.Location = new System.Drawing.Point(72, 8);
            this.txtHostName.Name = "txtHostName";
            this.txtHostName.Size = new System.Drawing.Size(300, 21);
            this.txtHostName.TabIndex = 16;
            // 
            // lbHostName
            // 
            this.lbHostName.AutoSize = true;
            this.lbHostName.Location = new System.Drawing.Point(13, 12);
            this.lbHostName.Name = "lbHostName";
            this.lbHostName.Size = new System.Drawing.Size(53, 12);
            this.lbHostName.TabIndex = 15;
            this.lbHostName.Text = "主机名称";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "主机类型";
            // 
            // comboBoxHostType
            // 
            this.comboBoxHostType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxHostType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHostType.FormattingEnabled = true;
            this.comboBoxHostType.Location = new System.Drawing.Point(73, 63);
            this.comboBoxHostType.Name = "comboBoxHostType";
            this.comboBoxHostType.Size = new System.Drawing.Size(299, 20);
            this.comboBoxHostType.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvDisablePort);
            this.groupBox1.Location = new System.Drawing.Point(72, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 187);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "不可分配端口";
            // 
            // dgvDisablePort
            // 
            this.dgvDisablePort.AlignDirection = true;
            this.dgvDisablePort.ColumnSettingStatus = UtilZ.Dotnet.WindowEx.Winform.Controls.PageGrid.PageGridColumnSettingStatus.Disable;
            this.dgvDisablePort.ColumnSettingWidth = 20;
            this.dgvDisablePort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDisablePort.FocusedRowIndex = -1;
            this.dgvDisablePort.IsLastColumnAutoSizeModeFill = true;
            this.dgvDisablePort.Location = new System.Drawing.Point(3, 17);
            this.dgvDisablePort.Name = "dgvDisablePort";
            this.dgvDisablePort.PageSizeMaximum = 100;
            this.dgvDisablePort.EnablePagingBar = false;
            this.dgvDisablePort.EnableRowNum = true;
            this.dgvDisablePort.Size = new System.Drawing.Size(294, 167);
            this.dgvDisablePort.TabIndex = 0;
            // 
            // cmsDisablePort
            // 
            this.cmsDisablePort.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDisablePortAdd,
            this.tsmiDisablePortModify,
            this.tsmiDisablePortDelete,
            this.tsmiDisablePortClear});
            this.cmsDisablePort.Name = "cmsDisablePort";
            this.cmsDisablePort.Size = new System.Drawing.Size(153, 114);
            this.cmsDisablePort.Opening += new System.ComponentModel.CancelEventHandler(this.cmsDisablePort_Opening);
            // 
            // tsmiDisablePortAdd
            // 
            this.tsmiDisablePortAdd.Name = "tsmiDisablePortAdd";
            this.tsmiDisablePortAdd.Size = new System.Drawing.Size(152, 22);
            this.tsmiDisablePortAdd.Text = "添加";
            this.tsmiDisablePortAdd.Click += new System.EventHandler(this.tsmiDisablePortAdd_Click);
            // 
            // tsmiDisablePortModify
            // 
            this.tsmiDisablePortModify.Name = "tsmiDisablePortModify";
            this.tsmiDisablePortModify.Size = new System.Drawing.Size(152, 22);
            this.tsmiDisablePortModify.Text = "修改";
            this.tsmiDisablePortModify.Click += new System.EventHandler(this.tsmiDisablePortModify_Click);
            // 
            // tsmiDisablePortDelete
            // 
            this.tsmiDisablePortDelete.Name = "tsmiDisablePortDelete";
            this.tsmiDisablePortDelete.Size = new System.Drawing.Size(152, 22);
            this.tsmiDisablePortDelete.Text = "删除";
            this.tsmiDisablePortDelete.Click += new System.EventHandler(this.tsmiDisablePortDelete_Click);
            // 
            // tsmiDisablePortClear
            // 
            this.tsmiDisablePortClear.Name = "tsmiDisablePortClear";
            this.tsmiDisablePortClear.Size = new System.Drawing.Size(152, 22);
            this.tsmiDisablePortClear.Text = "清空";
            this.tsmiDisablePortClear.Click += new System.EventHandler(this.tsmiDisablePortClear_Click);
            // 
            // FEditHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 312);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBoxHostType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancell);
            this.Controls.Add(this.txtHostIp);
            this.Controls.Add(this.lbHostIp);
            this.Controls.Add(this.txtHostName);
            this.Controls.Add(this.lbHostName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FEditHost";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑主机信息";
            this.Load += new System.EventHandler(this.FEditHost_Load);
            this.groupBox1.ResumeLayout(false);
            this.cmsDisablePort.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancell;
        private System.Windows.Forms.TextBox txtHostIp;
        private System.Windows.Forms.Label lbHostIp;
        private System.Windows.Forms.TextBox txtHostName;
        private System.Windows.Forms.Label lbHostName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxHostType;
        private System.Windows.Forms.GroupBox groupBox1;
        private WindowEx.Winform.Controls.PageGrid.UCPageGridControl dgvDisablePort;
        private System.Windows.Forms.ContextMenuStrip cmsDisablePort;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisablePortAdd;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisablePortModify;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisablePortDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisablePortClear;
    }
}