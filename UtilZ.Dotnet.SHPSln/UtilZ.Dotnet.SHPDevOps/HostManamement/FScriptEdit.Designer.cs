namespace UtilZ.Dotnet.SHPDevOps.HostManamement
{
    partial class FScriptEdit
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
            this.comboBoxScriptType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtxtScriptContent = new System.Windows.Forms.RichTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancell = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numSecondsTimeout = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numSecondsTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxScriptType
            // 
            this.comboBoxScriptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxScriptType.FormattingEnabled = true;
            this.comboBoxScriptType.Location = new System.Drawing.Point(72, 10);
            this.comboBoxScriptType.Name = "comboBoxScriptType";
            this.comboBoxScriptType.Size = new System.Drawing.Size(121, 20);
            this.comboBoxScriptType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "脚本类型";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "脚本内容";
            // 
            // rtxtScriptContent
            // 
            this.rtxtScriptContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtScriptContent.Location = new System.Drawing.Point(72, 36);
            this.rtxtScriptContent.Name = "rtxtScriptContent";
            this.rtxtScriptContent.Size = new System.Drawing.Size(500, 294);
            this.rtxtScriptContent.TabIndex = 3;
            this.rtxtScriptContent.Text = "";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(416, 336);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 30;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancell
            // 
            this.btnCancell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancell.Location = new System.Drawing.Point(497, 336);
            this.btnCancell.Name = "btnCancell";
            this.btnCancell.Size = new System.Drawing.Size(75, 23);
            this.btnCancell.TabIndex = 29;
            this.btnCancell.Text = "取消";
            this.btnCancell.UseVisualStyleBackColor = true;
            this.btnCancell.Click += new System.EventHandler(this.btnCancell_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(213, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 31;
            this.label3.Text = "执行超时时长(s)";
            // 
            // numMillisecondsTimeout
            // 
            this.numSecondsTimeout.Location = new System.Drawing.Point(314, 9);
            this.numSecondsTimeout.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.numSecondsTimeout.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numSecondsTimeout.Name = "numMillisecondsTimeout";
            this.numSecondsTimeout.Size = new System.Drawing.Size(120, 21);
            this.numSecondsTimeout.TabIndex = 32;
            this.numSecondsTimeout.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // FScriptEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 366);
            this.Controls.Add(this.numSecondsTimeout);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancell);
            this.Controls.Add(this.rtxtScriptContent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxScriptType);
            this.Name = "FScriptEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "脚本编辑";
            this.Load += new System.EventHandler(this.FScriptEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numSecondsTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxScriptType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtxtScriptContent;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancell;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numSecondsTimeout;
    }
}