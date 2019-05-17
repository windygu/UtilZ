namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.btnLoadPage = new System.Windows.Forms.Button();
            this.btnExcuteJS = new System.Windows.Forms.Button();
            this.btnGetJSValue = new System.Windows.Forms.Button();
            this.btnSendProcessMessage = new System.Windows.Forms.Button();
            this.winCefBrowser1 = new UtilZ.Dotnet.CEF.Win.WinCefBrowser();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadPage
            // 
            this.btnLoadPage.Location = new System.Drawing.Point(9, 15);
            this.btnLoadPage.Name = "btnLoadPage";
            this.btnLoadPage.Size = new System.Drawing.Size(75, 23);
            this.btnLoadPage.TabIndex = 1;
            this.btnLoadPage.Text = "LoadPage";
            this.btnLoadPage.UseVisualStyleBackColor = true;
            this.btnLoadPage.Click += new System.EventHandler(this.btnLoadPage_Click);
            // 
            // btnExcuteJS
            // 
            this.btnExcuteJS.Location = new System.Drawing.Point(91, 14);
            this.btnExcuteJS.Name = "btnExcuteJS";
            this.btnExcuteJS.Size = new System.Drawing.Size(75, 23);
            this.btnExcuteJS.TabIndex = 2;
            this.btnExcuteJS.Text = "ExcuteJS";
            this.btnExcuteJS.UseVisualStyleBackColor = true;
            this.btnExcuteJS.Click += new System.EventHandler(this.btnExcuteJS_Click);
            // 
            // btnGetJSValue
            // 
            this.btnGetJSValue.Location = new System.Drawing.Point(173, 14);
            this.btnGetJSValue.Name = "btnGetJSValue";
            this.btnGetJSValue.Size = new System.Drawing.Size(75, 23);
            this.btnGetJSValue.TabIndex = 3;
            this.btnGetJSValue.Text = "GetJSValue";
            this.btnGetJSValue.UseVisualStyleBackColor = true;
            this.btnGetJSValue.Click += new System.EventHandler(this.btnGetJSValue_Click);
            // 
            // btnSendProcessMessage
            // 
            this.btnSendProcessMessage.Location = new System.Drawing.Point(255, 13);
            this.btnSendProcessMessage.Name = "btnSendProcessMessage";
            this.btnSendProcessMessage.Size = new System.Drawing.Size(151, 23);
            this.btnSendProcessMessage.TabIndex = 4;
            this.btnSendProcessMessage.Text = "SendProcessMessage";
            this.btnSendProcessMessage.UseVisualStyleBackColor = true;
            this.btnSendProcessMessage.Click += new System.EventHandler(this.btnSendProcessMessage_Click);
            // 
            // winCefBrowser1
            // 
            this.winCefBrowser1.BrowserSettings = null;
            this.winCefBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winCefBrowser1.Location = new System.Drawing.Point(0, 0);
            this.winCefBrowser1.Name = "winCefBrowser1";
            this.winCefBrowser1.Size = new System.Drawing.Size(650, 374);
            this.winCefBrowser1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnExcuteJS);
            this.splitContainer1.Panel1.Controls.Add(this.btnSendProcessMessage);
            this.splitContainer1.Panel1.Controls.Add(this.btnLoadPage);
            this.splitContainer1.Panel1.Controls.Add(this.btnGetJSValue);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.winCefBrowser1);
            this.splitContainer1.Size = new System.Drawing.Size(650, 426);
            this.splitContainer1.SplitterDistance = 48;
            this.splitContainer1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 426);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UtilZ.Dotnet.CEF.Win.WinCefBrowser winCefBrowser1;
        private System.Windows.Forms.Button btnLoadPage;
        private System.Windows.Forms.Button btnExcuteJS;
        private System.Windows.Forms.Button btnGetJSValue;
        private System.Windows.Forms.Button btnSendProcessMessage;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

