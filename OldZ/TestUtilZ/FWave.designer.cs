namespace TestUtilZ
{
    partial class FWave
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnOpenFile1 = new System.Windows.Forms.Button();
            this.btnStop1 = new System.Windows.Forms.Button();
            this.btnPause1 = new System.Windows.Forms.Button();
            this.btnPlay1 = new System.Windows.Forms.Button();
            this.wavePlayer1 = new UtilZ.Lib.Wav.WavePlayer();
            this.wavePlayer2 = new UtilZ.Lib.Wav.WavePlayer();
            this.btnOpenFile2 = new System.Windows.Forms.Button();
            this.btnStop2 = new System.Windows.Forms.Button();
            this.btnPause2 = new System.Windows.Forms.Button();
            this.btnPlay2 = new System.Windows.Forms.Button();
            this.cbRingHear = new System.Windows.Forms.CheckBox();
            this.cbIsMergeChanel = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 55);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.wavePlayer1);
            this.splitContainer1.Panel1.Controls.Add(this.btnOpenFile1);
            this.splitContainer1.Panel1.Controls.Add(this.btnStop1);
            this.splitContainer1.Panel1.Controls.Add(this.btnPause1);
            this.splitContainer1.Panel1.Controls.Add(this.btnPlay1);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnOpenFile2);
            this.splitContainer1.Panel2.Controls.Add(this.btnStop2);
            this.splitContainer1.Panel2.Controls.Add(this.btnPause2);
            this.splitContainer1.Panel2.Controls.Add(this.btnPlay2);
            this.splitContainer1.Panel2.Controls.Add(this.wavePlayer2);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(977, 533);
            this.splitContainer1.SplitterDistance = 238;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnOpenFile1
            // 
            this.btnOpenFile1.Location = new System.Drawing.Point(3, 3);
            this.btnOpenFile1.Name = "btnOpenFile1";
            this.btnOpenFile1.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile1.TabIndex = 30;
            this.btnOpenFile1.Text = "OpenFile";
            this.btnOpenFile1.UseVisualStyleBackColor = true;
            this.btnOpenFile1.Click += new System.EventHandler(this.btnOpenFile1_Click);
            // 
            // btnStop1
            // 
            this.btnStop1.Location = new System.Drawing.Point(246, 3);
            this.btnStop1.Name = "btnStop1";
            this.btnStop1.Size = new System.Drawing.Size(75, 23);
            this.btnStop1.TabIndex = 29;
            this.btnStop1.Text = "Stop";
            this.btnStop1.UseVisualStyleBackColor = true;
            this.btnStop1.Click += new System.EventHandler(this.btnStop1_Click);
            // 
            // btnPause1
            // 
            this.btnPause1.Location = new System.Drawing.Point(165, 3);
            this.btnPause1.Name = "btnPause1";
            this.btnPause1.Size = new System.Drawing.Size(75, 23);
            this.btnPause1.TabIndex = 28;
            this.btnPause1.Text = "Pause";
            this.btnPause1.UseVisualStyleBackColor = true;
            this.btnPause1.Click += new System.EventHandler(this.btnPause1_Click);
            // 
            // btnPlay1
            // 
            this.btnPlay1.Location = new System.Drawing.Point(84, 3);
            this.btnPlay1.Name = "btnPlay1";
            this.btnPlay1.Size = new System.Drawing.Size(75, 23);
            this.btnPlay1.TabIndex = 27;
            this.btnPlay1.Text = "Play";
            this.btnPlay1.UseVisualStyleBackColor = true;
            this.btnPlay1.Click += new System.EventHandler(this.btnPlay1_Click);
            // 
            // wavePlayer1
            // 
            this.wavePlayer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wavePlayer1.BackgroudColor = System.Drawing.Color.Black;
            this.wavePlayer1.DbWidth = 50;
            this.wavePlayer1.DrawInterval = 100;
            this.wavePlayer1.EnableDbAreaBackground = true;
            this.wavePlayer1.EnableLogoAreaBackground = false;
            this.wavePlayer1.EnableTimeBackground = true;
            this.wavePlayer1.EnableWavAreaBackground = false;
            this.wavePlayer1.EnableZoomWavBackground = false;
            this.wavePlayer1.Freq = 44100;
            this.wavePlayer1.IsDrawChannelDivideLine = true;
            this.wavePlayer1.IsDrawPlayLine = true;
            this.wavePlayer1.IsDrawWavMidLine = true;
            this.wavePlayer1.IsMergeChanel = true;
            this.wavePlayer1.IsVersionValidate = true;
            this.wavePlayer1.Location = new System.Drawing.Point(3, 32);
            this.wavePlayer1.MinimumSize = new System.Drawing.Size(200, 130);
            this.wavePlayer1.Name = "wavePlayer1";
            this.wavePlayer1.PlayLocationLineRefreshInterval = 100;
            this.wavePlayer1.QualityCoefficient = 1;
            this.wavePlayer1.Size = new System.Drawing.Size(971, 203);
            this.wavePlayer1.TabIndex = 31;
            this.wavePlayer1.Text = "wavePlayer1";
            this.wavePlayer1.WavSelecteMouseStyle = System.Windows.Forms.Cursors.IBeam;
            this.wavePlayer1.ZoomHeight = 60;
            this.wavePlayer1.ZoomMuilt = 2;
            this.wavePlayer1.ZoomWavDisplayAreaMmoveMouseStyle = System.Windows.Forms.Cursors.SizeAll;
            // 
            // wavePlayer2
            // 
            this.wavePlayer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wavePlayer2.BackgroudColor = System.Drawing.Color.Black;
            this.wavePlayer2.DbWidth = 50;
            this.wavePlayer2.DrawInterval = 100;
            this.wavePlayer2.EnableDbAreaBackground = true;
            this.wavePlayer2.EnableLogoAreaBackground = false;
            this.wavePlayer2.EnableTimeBackground = true;
            this.wavePlayer2.EnableWavAreaBackground = false;
            this.wavePlayer2.EnableZoomWavBackground = false;
            this.wavePlayer2.Freq = 44100;
            this.wavePlayer2.IsDrawChannelDivideLine = true;
            this.wavePlayer2.IsDrawPlayLine = true;
            this.wavePlayer2.IsDrawWavMidLine = true;
            this.wavePlayer2.IsMergeChanel = true;
            this.wavePlayer2.IsVersionValidate = true;
            this.wavePlayer2.Location = new System.Drawing.Point(4, 42);
            this.wavePlayer2.MinimumSize = new System.Drawing.Size(200, 130);
            this.wavePlayer2.Name = "wavePlayer2";
            this.wavePlayer2.PlayLocationLineRefreshInterval = 100;
            this.wavePlayer2.QualityCoefficient = 1;
            this.wavePlayer2.Size = new System.Drawing.Size(970, 231);
            this.wavePlayer2.TabIndex = 31;
            this.wavePlayer2.Text = "wavePlayer2";
            this.wavePlayer2.WavSelecteMouseStyle = System.Windows.Forms.Cursors.IBeam;
            this.wavePlayer2.ZoomHeight = 60;
            this.wavePlayer2.ZoomMuilt = 2;
            this.wavePlayer2.ZoomWavDisplayAreaMmoveMouseStyle = System.Windows.Forms.Cursors.SizeAll;
            // 
            // btnOpenFile2
            // 
            this.btnOpenFile2.Location = new System.Drawing.Point(3, 13);
            this.btnOpenFile2.Name = "btnOpenFile2";
            this.btnOpenFile2.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile2.TabIndex = 35;
            this.btnOpenFile2.Text = "OpenFile";
            this.btnOpenFile2.UseVisualStyleBackColor = true;
            this.btnOpenFile2.Click += new System.EventHandler(this.btnOpenFile2_Click);
            // 
            // btnStop2
            // 
            this.btnStop2.Location = new System.Drawing.Point(246, 13);
            this.btnStop2.Name = "btnStop2";
            this.btnStop2.Size = new System.Drawing.Size(75, 23);
            this.btnStop2.TabIndex = 34;
            this.btnStop2.Text = "Stop";
            this.btnStop2.UseVisualStyleBackColor = true;
            this.btnStop2.Click += new System.EventHandler(this.btnStop2_Click);
            // 
            // btnPause2
            // 
            this.btnPause2.Location = new System.Drawing.Point(165, 13);
            this.btnPause2.Name = "btnPause2";
            this.btnPause2.Size = new System.Drawing.Size(75, 23);
            this.btnPause2.TabIndex = 33;
            this.btnPause2.Text = "Pause";
            this.btnPause2.UseVisualStyleBackColor = true;
            this.btnPause2.Click += new System.EventHandler(this.btnPause2_Click);
            // 
            // btnPlay2
            // 
            this.btnPlay2.Location = new System.Drawing.Point(84, 13);
            this.btnPlay2.Name = "btnPlay2";
            this.btnPlay2.Size = new System.Drawing.Size(75, 23);
            this.btnPlay2.TabIndex = 32;
            this.btnPlay2.Text = "Play";
            this.btnPlay2.UseVisualStyleBackColor = true;
            this.btnPlay2.Click += new System.EventHandler(this.btnPlay2_Click);
            // 
            // cbRingHear
            // 
            this.cbRingHear.AutoSize = true;
            this.cbRingHear.Location = new System.Drawing.Point(16, 12);
            this.cbRingHear.Name = "cbRingHear";
            this.cbRingHear.Size = new System.Drawing.Size(46, 16);
            this.cbRingHear.TabIndex = 41;
            this.cbRingHear.Text = "环听";
            this.cbRingHear.UseVisualStyleBackColor = true;
            this.cbRingHear.CheckedChanged += new System.EventHandler(this.cbRingHear_CheckedChanged);
            // 
            // cbIsMergeChanel
            // 
            this.cbIsMergeChanel.AutoSize = true;
            this.cbIsMergeChanel.Checked = true;
            this.cbIsMergeChanel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsMergeChanel.Location = new System.Drawing.Point(68, 12);
            this.cbIsMergeChanel.Name = "cbIsMergeChanel";
            this.cbIsMergeChanel.Size = new System.Drawing.Size(154, 16);
            this.cbIsMergeChanel.TabIndex = 40;
            this.cbIsMergeChanel.Text = "是否合并左右声道波形图";
            this.cbIsMergeChanel.UseVisualStyleBackColor = true;
            this.cbIsMergeChanel.CheckedChanged += new System.EventHandler(this.cbIsMergeChanel_CheckedChanged);
            // 
            // FWave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 600);
            this.Controls.Add(this.cbRingHear);
            this.Controls.Add(this.cbIsMergeChanel);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FWave";
            this.Text = "FWave";
            this.Load += new System.EventHandler(this.FWave_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private UtilZ.Lib.Wav.WavePlayer wavePlayer1;
        private System.Windows.Forms.Button btnOpenFile1;
        private System.Windows.Forms.Button btnStop1;
        private System.Windows.Forms.Button btnPause1;
        private System.Windows.Forms.Button btnPlay1;
        private UtilZ.Lib.Wav.WavePlayer wavePlayer2;
        private System.Windows.Forms.Button btnOpenFile2;
        private System.Windows.Forms.Button btnStop2;
        private System.Windows.Forms.Button btnPause2;
        private System.Windows.Forms.Button btnPlay2;
        private System.Windows.Forms.CheckBox cbRingHear;
        private System.Windows.Forms.CheckBox cbIsMergeChanel;
    }
}