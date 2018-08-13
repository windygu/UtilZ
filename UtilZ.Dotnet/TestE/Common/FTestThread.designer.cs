namespace TestE.Common
{
    partial class FTestThread
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.checkBoxSync = new System.Windows.Forms.CheckBox();
            this.logControlF1 = new UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 23);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(93, 23);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(283, 23);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 23);
            this.btnAbort.TabIndex = 8;
            this.btnAbort.Text = "Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // checkBoxSync
            // 
            this.checkBoxSync.AutoSize = true;
            this.checkBoxSync.Location = new System.Drawing.Point(175, 29);
            this.checkBoxSync.Name = "checkBoxSync";
            this.checkBoxSync.Size = new System.Drawing.Size(72, 16);
            this.checkBoxSync.TabIndex = 10;
            this.checkBoxSync.Text = "同步停止";
            this.checkBoxSync.UseVisualStyleBackColor = true;
            // 
            // logControlF1
            // 
            this.logControlF1.IsLock = false;
            this.logControlF1.Location = new System.Drawing.Point(13, 66);
            this.logControlF1.MaxItemCount = 100;
            this.logControlF1.Name = "logControlF1";
            this.logControlF1.Size = new System.Drawing.Size(405, 324);
            this.logControlF1.TabIndex = 11;
            // 
            // FTestThread
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 402);
            this.Controls.Add(this.logControlF1);
            this.Controls.Add(this.checkBoxSync);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Name = "FTestThread";
            this.Text = "FTestThread";
            this.Load += new System.EventHandler(this.FTestThread_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.CheckBox checkBoxSync;
        private UtilZ.Dotnet.WindowEx.Winform.Controls.LogControlF logControlF1;
    }
}