namespace UtilZ.Dotnet.SHPDevOps
{
    partial class FTest
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
            this.numAddCount = new System.Windows.Forms.NumericUpDown();
            this.btnAddChannel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelChannelCount = new System.Windows.Forms.Label();
            this.btnClearChannel = new System.Windows.Forms.Button();
            this.btnClearData = new System.Windows.Forms.Button();
            this.btnAddData = new System.Windows.Forms.Button();
            this.btnBegin = new System.Windows.Forms.Button();
            this.btnAddData2 = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnKeyValuePair = new System.Windows.Forms.Button();
            this.btnKeyValuePair2 = new System.Windows.Forms.Button();
            this.btnSetingBK = new System.Windows.Forms.Button();
            this.btnPartValueBegin = new System.Windows.Forms.Button();
            this.btnPartValueEnd = new System.Windows.Forms.Button();
            this.checkBoxDirection = new System.Windows.Forms.CheckBox();
            this.usageControl1 = new UtilZ.Dotnet.WindowEx.Winform.Controls.LineChartControl();
            this.checkBoxDrawBkGrid = new System.Windows.Forms.CheckBox();
            this.checkBoxShowTitle = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numAddCount)).BeginInit();
            this.SuspendLayout();
            // 
            // numAddCount
            // 
            this.numAddCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numAddCount.Location = new System.Drawing.Point(72, 289);
            this.numAddCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAddCount.Name = "numAddCount";
            this.numAddCount.Size = new System.Drawing.Size(120, 21);
            this.numAddCount.TabIndex = 2;
            this.numAddCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnAddChannel
            // 
            this.btnAddChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddChannel.Location = new System.Drawing.Point(216, 288);
            this.btnAddChannel.Name = "btnAddChannel";
            this.btnAddChannel.Size = new System.Drawing.Size(75, 23);
            this.btnAddChannel.TabIndex = 4;
            this.btnAddChannel.Text = "添加通道";
            this.btnAddChannel.UseVisualStyleBackColor = true;
            this.btnAddChannel.Click += new System.EventHandler(this.btnAddChannel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 294);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "数据项数";
            // 
            // labelChannelCount
            // 
            this.labelChannelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelChannelCount.AutoSize = true;
            this.labelChannelCount.Location = new System.Drawing.Point(314, 294);
            this.labelChannelCount.Name = "labelChannelCount";
            this.labelChannelCount.Size = new System.Drawing.Size(41, 12);
            this.labelChannelCount.TabIndex = 6;
            this.labelChannelCount.Text = "通道数";
            // 
            // btnClearChannel
            // 
            this.btnClearChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearChannel.Location = new System.Drawing.Point(377, 285);
            this.btnClearChannel.Name = "btnClearChannel";
            this.btnClearChannel.Size = new System.Drawing.Size(75, 23);
            this.btnClearChannel.TabIndex = 7;
            this.btnClearChannel.Text = "清空通道";
            this.btnClearChannel.UseVisualStyleBackColor = true;
            this.btnClearChannel.Click += new System.EventHandler(this.btnClearChannel_Click);
            // 
            // btnClearData
            // 
            this.btnClearData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearData.Location = new System.Drawing.Point(458, 285);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(75, 23);
            this.btnClearData.TabIndex = 8;
            this.btnClearData.Text = "清空数据";
            this.btnClearData.UseVisualStyleBackColor = true;
            this.btnClearData.Click += new System.EventHandler(this.btnClearData_Click);
            // 
            // btnAddData
            // 
            this.btnAddData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddData.Location = new System.Drawing.Point(117, 327);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(75, 23);
            this.btnAddData.TabIndex = 9;
            this.btnAddData.Text = "添加数据";
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // btnBegin
            // 
            this.btnBegin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBegin.Location = new System.Drawing.Point(216, 326);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(75, 23);
            this.btnBegin.TabIndex = 10;
            this.btnBegin.Text = "Begin";
            this.btnBegin.UseVisualStyleBackColor = true;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            // 
            // btnAddData2
            // 
            this.btnAddData2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddData2.Location = new System.Drawing.Point(316, 326);
            this.btnAddData2.Name = "btnAddData2";
            this.btnAddData2.Size = new System.Drawing.Size(75, 23);
            this.btnAddData2.TabIndex = 11;
            this.btnAddData2.Text = "添加数据2";
            this.btnAddData2.UseVisualStyleBackColor = true;
            this.btnAddData2.Click += new System.EventHandler(this.btnAddData2_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnd.Location = new System.Drawing.Point(409, 326);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(75, 23);
            this.btnEnd.TabIndex = 12;
            this.btnEnd.Text = "End";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnKeyValuePair
            // 
            this.btnKeyValuePair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKeyValuePair.Location = new System.Drawing.Point(188, 357);
            this.btnKeyValuePair.Name = "btnKeyValuePair";
            this.btnKeyValuePair.Size = new System.Drawing.Size(103, 23);
            this.btnKeyValuePair.TabIndex = 13;
            this.btnKeyValuePair.Text = "KeyValuePair";
            this.btnKeyValuePair.UseVisualStyleBackColor = true;
            this.btnKeyValuePair.Click += new System.EventHandler(this.btnKeyValuePair_Click);
            // 
            // btnKeyValuePair2
            // 
            this.btnKeyValuePair2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKeyValuePair2.Location = new System.Drawing.Point(316, 357);
            this.btnKeyValuePair2.Name = "btnKeyValuePair2";
            this.btnKeyValuePair2.Size = new System.Drawing.Size(103, 23);
            this.btnKeyValuePair2.TabIndex = 14;
            this.btnKeyValuePair2.Text = "KeyValuePair2";
            this.btnKeyValuePair2.UseVisualStyleBackColor = true;
            this.btnKeyValuePair2.Click += new System.EventHandler(this.btnKeyValuePair2_Click);
            // 
            // btnSetingBK
            // 
            this.btnSetingBK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetingBK.Location = new System.Drawing.Point(539, 283);
            this.btnSetingBK.Name = "btnSetingBK";
            this.btnSetingBK.Size = new System.Drawing.Size(75, 23);
            this.btnSetingBK.TabIndex = 15;
            this.btnSetingBK.Text = "SetingBK";
            this.btnSetingBK.UseVisualStyleBackColor = true;
            this.btnSetingBK.Click += new System.EventHandler(this.btnSetingBK_Click);
            // 
            // btnPartValueBegin
            // 
            this.btnPartValueBegin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPartValueBegin.Location = new System.Drawing.Point(458, 356);
            this.btnPartValueBegin.Name = "btnPartValueBegin";
            this.btnPartValueBegin.Size = new System.Drawing.Size(104, 23);
            this.btnPartValueBegin.TabIndex = 16;
            this.btnPartValueBegin.Text = "PartValueBegin";
            this.btnPartValueBegin.UseVisualStyleBackColor = true;
            this.btnPartValueBegin.Click += new System.EventHandler(this.btnPartValueBegin_Click);
            // 
            // btnPartValueEnd
            // 
            this.btnPartValueEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPartValueEnd.Location = new System.Drawing.Point(587, 356);
            this.btnPartValueEnd.Name = "btnPartValueEnd";
            this.btnPartValueEnd.Size = new System.Drawing.Size(104, 23);
            this.btnPartValueEnd.TabIndex = 17;
            this.btnPartValueEnd.Text = "PartValueEnd";
            this.btnPartValueEnd.UseVisualStyleBackColor = true;
            this.btnPartValueEnd.Click += new System.EventHandler(this.btnPartValueEnd_Click);
            // 
            // checkBoxDirection
            // 
            this.checkBoxDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDirection.AutoSize = true;
            this.checkBoxDirection.Location = new System.Drawing.Point(509, 326);
            this.checkBoxDirection.Name = "checkBoxDirection";
            this.checkBoxDirection.Size = new System.Drawing.Size(228, 16);
            this.checkBoxDirection.TabIndex = 18;
            this.checkBoxDirection.Text = "绘制方向[true:左->右;false:右->左]";
            this.checkBoxDirection.UseVisualStyleBackColor = true;
            this.checkBoxDirection.CheckedChanged += new System.EventHandler(this.checkBoxDirection_CheckedChanged);
            // 
            // usageControl1
            // 
            this.usageControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usageControl1.BackColor = System.Drawing.Color.Black;
            this.usageControl1.DrawDirection = false;
            this.usageControl1.GridLineColor = System.Drawing.Color.SeaGreen;
            this.usageControl1.GridLineWidth = 1F;
            this.usageControl1.IsMoveGrid = true;
            this.usageControl1.Location = new System.Drawing.Point(38, 23);
            this.usageControl1.MinimumSize = new System.Drawing.Size(200, 130);
            this.usageControl1.Name = "usageControl1";
            this.usageControl1.ShowGrid = true;
            this.usageControl1.ShowTitle = true;
            this.usageControl1.Size = new System.Drawing.Size(653, 245);
            this.usageControl1.TabIndex = 3;
            this.usageControl1.Text = "usageControl1";
            // 
            // checkBoxDrawBkGrid
            // 
            this.checkBoxDrawBkGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDrawBkGrid.AutoSize = true;
            this.checkBoxDrawBkGrid.Location = new System.Drawing.Point(620, 288);
            this.checkBoxDrawBkGrid.Name = "checkBoxDrawBkGrid";
            this.checkBoxDrawBkGrid.Size = new System.Drawing.Size(96, 16);
            this.checkBoxDrawBkGrid.TabIndex = 19;
            this.checkBoxDrawBkGrid.Text = "显示背景表格";
            this.checkBoxDrawBkGrid.UseVisualStyleBackColor = true;
            this.checkBoxDrawBkGrid.CheckedChanged += new System.EventHandler(this.checkBoxDrawBkGrid_CheckedChanged);
            // 
            // checkBoxShowTitle
            // 
            this.checkBoxShowTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowTitle.AutoSize = true;
            this.checkBoxShowTitle.Location = new System.Drawing.Point(15, 357);
            this.checkBoxShowTitle.Name = "checkBoxShowTitle";
            this.checkBoxShowTitle.Size = new System.Drawing.Size(72, 16);
            this.checkBoxShowTitle.TabIndex = 20;
            this.checkBoxShowTitle.Text = "显示标题";
            this.checkBoxShowTitle.UseVisualStyleBackColor = true;
            this.checkBoxShowTitle.CheckedChanged += new System.EventHandler(this.checkBoxShowTitle_CheckedChanged);
            // 
            // FTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 395);
            this.Controls.Add(this.checkBoxShowTitle);
            this.Controls.Add(this.checkBoxDrawBkGrid);
            this.Controls.Add(this.checkBoxDirection);
            this.Controls.Add(this.btnPartValueEnd);
            this.Controls.Add(this.btnPartValueBegin);
            this.Controls.Add(this.btnSetingBK);
            this.Controls.Add(this.btnKeyValuePair2);
            this.Controls.Add(this.btnKeyValuePair);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnAddData2);
            this.Controls.Add(this.btnBegin);
            this.Controls.Add(this.btnAddData);
            this.Controls.Add(this.btnClearData);
            this.Controls.Add(this.btnClearChannel);
            this.Controls.Add(this.labelChannelCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddChannel);
            this.Controls.Add(this.usageControl1);
            this.Controls.Add(this.numAddCount);
            this.Name = "FTest";
            this.Text = "FTest";
            this.Load += new System.EventHandler(this.FTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numAddCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown numAddCount;
        private UtilZ.Dotnet.WindowEx.Winform.Controls.LineChartControl usageControl1;
        private System.Windows.Forms.Button btnAddChannel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelChannelCount;
        private System.Windows.Forms.Button btnClearChannel;
        private System.Windows.Forms.Button btnClearData;
        private System.Windows.Forms.Button btnAddData;
        private System.Windows.Forms.Button btnBegin;
        private System.Windows.Forms.Button btnAddData2;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnKeyValuePair;
        private System.Windows.Forms.Button btnKeyValuePair2;
        private System.Windows.Forms.Button btnSetingBK;
        private System.Windows.Forms.Button btnPartValueBegin;
        private System.Windows.Forms.Button btnPartValueEnd;
        private System.Windows.Forms.CheckBox checkBoxDirection;
        private System.Windows.Forms.CheckBox checkBoxDrawBkGrid;
        private System.Windows.Forms.CheckBox checkBoxShowTitle;
    }
}