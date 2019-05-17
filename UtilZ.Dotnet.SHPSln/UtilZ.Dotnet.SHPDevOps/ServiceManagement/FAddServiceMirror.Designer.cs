namespace UtilZ.Dotnet.SHPDevOps.ServiceManagement
{
    partial class FAddServiceMirror
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
            this.txtStartArgs = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProcessFilePath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtMirrorFilePath = new System.Windows.Forms.TextBox();
            this.btnCancell = new System.Windows.Forms.Button();
            this.btnChioceMirrorFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMainFilePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numVer = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lb2 = new System.Windows.Forms.Label();
            this.rtxtDes = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxServiceMirrorType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numDeploySecondsTimeout = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numVer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeploySecondsTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // txtStartArgs
            // 
            this.txtStartArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStartArgs.Location = new System.Drawing.Point(93, 95);
            this.txtStartArgs.Name = "txtStartArgs";
            this.txtStartArgs.Size = new System.Drawing.Size(533, 21);
            this.txtStartArgs.TabIndex = 54;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 53;
            this.label7.Text = "启动参数";
            // 
            // txtProcessFilePath
            // 
            this.txtProcessFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcessFilePath.Location = new System.Drawing.Point(93, 68);
            this.txtProcessFilePath.Name = "txtProcessFilePath";
            this.txtProcessFilePath.Size = new System.Drawing.Size(533, 21);
            this.txtProcessFilePath.TabIndex = 52;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 51;
            this.label6.Text = "进程文件路径";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(470, 383);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 43;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtMirrorFilePath
            // 
            this.txtMirrorFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMirrorFilePath.Location = new System.Drawing.Point(94, 12);
            this.txtMirrorFilePath.Name = "txtMirrorFilePath";
            this.txtMirrorFilePath.Size = new System.Drawing.Size(533, 21);
            this.txtMirrorFilePath.TabIndex = 45;
            this.txtMirrorFilePath.TextChanged += new System.EventHandler(this.txtMirrorFilePath_TextChanged);
            // 
            // btnCancell
            // 
            this.btnCancell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancell.Location = new System.Drawing.Point(551, 383);
            this.btnCancell.Name = "btnCancell";
            this.btnCancell.Size = new System.Drawing.Size(75, 23);
            this.btnCancell.TabIndex = 42;
            this.btnCancell.Text = "取消";
            this.btnCancell.UseVisualStyleBackColor = true;
            this.btnCancell.Click += new System.EventHandler(this.btnCancell_Click);
            // 
            // btnChioceMirrorFile
            // 
            this.btnChioceMirrorFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChioceMirrorFile.Location = new System.Drawing.Point(633, 10);
            this.btnChioceMirrorFile.Name = "btnChioceMirrorFile";
            this.btnChioceMirrorFile.Size = new System.Drawing.Size(41, 23);
            this.btnChioceMirrorFile.TabIndex = 48;
            this.btnChioceMirrorFile.Text = "...";
            this.btnChioceMirrorFile.UseVisualStyleBackColor = true;
            this.btnChioceMirrorFile.Click += new System.EventHandler(this.btnChioceMirrorFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 44;
            this.label2.Text = "镜像文件路径";
            // 
            // txtMainFilePath
            // 
            this.txtMainFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMainFilePath.Location = new System.Drawing.Point(94, 41);
            this.txtMainFilePath.Name = "txtMainFilePath";
            this.txtMainFilePath.Size = new System.Drawing.Size(533, 21);
            this.txtMainFilePath.TabIndex = 47;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 49;
            this.label4.Text = "版本";
            // 
            // numVer
            // 
            this.numVer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numVer.Location = new System.Drawing.Point(93, 122);
            this.numVer.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numVer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVer.Name = "numVer";
            this.numVer.Size = new System.Drawing.Size(533, 21);
            this.numVer.TabIndex = 50;
            this.numVer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 46;
            this.label3.Text = "启动文件路径";
            // 
            // lb2
            // 
            this.lb2.AutoSize = true;
            this.lb2.Location = new System.Drawing.Point(59, 205);
            this.lb2.Name = "lb2";
            this.lb2.Size = new System.Drawing.Size(29, 12);
            this.lb2.TabIndex = 55;
            this.lb2.Text = "备注";
            // 
            // rtxtDes
            // 
            this.rtxtDes.Location = new System.Drawing.Point(94, 202);
            this.rtxtDes.Name = "rtxtDes";
            this.rtxtDes.Size = new System.Drawing.Size(533, 175);
            this.rtxtDes.TabIndex = 57;
            this.rtxtDes.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 58;
            this.label1.Text = "服务镜像类型";
            // 
            // comboBoxServiceMirrorType
            // 
            this.comboBoxServiceMirrorType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxServiceMirrorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxServiceMirrorType.FormattingEnabled = true;
            this.comboBoxServiceMirrorType.Location = new System.Drawing.Point(93, 149);
            this.comboBoxServiceMirrorType.Name = "comboBoxServiceMirrorType";
            this.comboBoxServiceMirrorType.Size = new System.Drawing.Size(533, 20);
            this.comboBoxServiceMirrorType.TabIndex = 59;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 60;
            this.label5.Text = "服务部署时长";
            // 
            // numDeployMillisecondsTimeout
            // 
            this.numDeploySecondsTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numDeploySecondsTimeout.Location = new System.Drawing.Point(93, 175);
            this.numDeploySecondsTimeout.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.numDeploySecondsTimeout.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numDeploySecondsTimeout.Name = "numDeployMillisecondsTimeout";
            this.numDeploySecondsTimeout.Size = new System.Drawing.Size(533, 21);
            this.numDeploySecondsTimeout.TabIndex = 61;
            this.numDeploySecondsTimeout.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(630, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 62;
            this.label8.Text = "秒";
            // 
            // FAddServiceMirror
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 412);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numDeploySecondsTimeout);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxServiceMirrorType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtxtDes);
            this.Controls.Add(this.lb2);
            this.Controls.Add(this.txtStartArgs);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtProcessFilePath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtMirrorFilePath);
            this.Controls.Add(this.btnCancell);
            this.Controls.Add(this.btnChioceMirrorFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMainFilePath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numVer);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FAddServiceMirror";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "上传服务镜像";
            this.Load += new System.EventHandler(this.FServiceUpgrade_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numVer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeploySecondsTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStartArgs;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtProcessFilePath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtMirrorFilePath;
        private System.Windows.Forms.Button btnCancell;
        private System.Windows.Forms.Button btnChioceMirrorFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMainFilePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numVer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb2;
        private System.Windows.Forms.RichTextBox rtxtDes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxServiceMirrorType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numDeploySecondsTimeout;
        private System.Windows.Forms.Label label8;
    }
}