namespace TestE.DB
{
    partial class FTestDB
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
            this.btnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comDB = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnEFQuery = new System.Windows.Forms.Button();
            this.btnTableStruct = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(194, 12);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 46;
            this.btnTest.Text = "Query";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 45;
            this.label1.Text = "数据库";
            // 
            // comDB
            // 
            this.comDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comDB.FormattingEnabled = true;
            this.comDB.Location = new System.Drawing.Point(64, 12);
            this.comDB.Name = "comDB";
            this.comDB.Size = new System.Drawing.Size(114, 20);
            this.comDB.TabIndex = 44;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(19, 47);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(769, 391);
            this.dataGridView1.TabIndex = 47;
            // 
            // btnEFQuery
            // 
            this.btnEFQuery.Location = new System.Drawing.Point(296, 12);
            this.btnEFQuery.Name = "btnEFQuery";
            this.btnEFQuery.Size = new System.Drawing.Size(75, 23);
            this.btnEFQuery.TabIndex = 48;
            this.btnEFQuery.Text = "EFQuery";
            this.btnEFQuery.UseVisualStyleBackColor = true;
            this.btnEFQuery.Click += new System.EventHandler(this.btnEFQuery_Click);
            // 
            // btnTableStruct
            // 
            this.btnTableStruct.Location = new System.Drawing.Point(389, 11);
            this.btnTableStruct.Name = "btnTableStruct";
            this.btnTableStruct.Size = new System.Drawing.Size(75, 23);
            this.btnTableStruct.TabIndex = 49;
            this.btnTableStruct.Text = "TableStruct";
            this.btnTableStruct.UseVisualStyleBackColor = true;
            this.btnTableStruct.Click += new System.EventHandler(this.btnTableStruct_Click);
            // 
            // FTestDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTableStruct);
            this.Controls.Add(this.btnEFQuery);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comDB);
            this.Name = "FTestDB";
            this.Text = "FTestDB";
            this.Load += new System.EventHandler(this.FTestDB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comDB;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnEFQuery;
        private System.Windows.Forms.Button btnTableStruct;
    }
}