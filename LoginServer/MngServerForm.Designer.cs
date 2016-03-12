namespace LoginServer
{
    partial class MngServerForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ServerView = new System.Windows.Forms.DataGridView();
            this.Owner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxSourcePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browseSourcePath = new System.Windows.Forms.Button();
            this.browseTargetPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTargetPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textBoxDownloadPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ServerView)).BeginInit();
            this.SuspendLayout();
            // 
            // ServerView
            // 
            this.ServerView.AllowUserToAddRows = false;
            this.ServerView.AllowUserToDeleteRows = false;
            this.ServerView.AllowUserToResizeColumns = false;
            this.ServerView.AllowUserToResizeRows = false;
            this.ServerView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ServerView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Owner,
            this.dataGridViewTextBoxColumn3,
            this.Column1,
            this.IdColumn});
            this.ServerView.Location = new System.Drawing.Point(12, 109);
            this.ServerView.Name = "ServerView";
            this.ServerView.ReadOnly = true;
            this.ServerView.RowHeadersVisible = false;
            this.ServerView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ServerView.Size = new System.Drawing.Size(523, 197);
            this.ServerView.TabIndex = 13;
            // 
            // Owner
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Owner.DefaultCellStyle = dataGridViewCellStyle4;
            this.Owner.HeaderText = "开始时间";
            this.Owner.Name = "Owner";
            this.Owner.ReadOnly = true;
            this.Owner.Width = 90;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn3.HeaderText = "服务器路径";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.Width = 170;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "下载路径";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 170;
            // 
            // IdColumn
            // 
            this.IdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.IdColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.IdColumn.HeaderText = "登录计数";
            this.IdColumn.Name = "IdColumn";
            this.IdColumn.ReadOnly = true;
            // 
            // textBoxSourcePath
            // 
            this.textBoxSourcePath.Location = new System.Drawing.Point(86, 12);
            this.textBoxSourcePath.Name = "textBoxSourcePath";
            this.textBoxSourcePath.Size = new System.Drawing.Size(411, 20);
            this.textBoxSourcePath.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "源路径:";
            // 
            // browseSourcePath
            // 
            this.browseSourcePath.Location = new System.Drawing.Point(503, 10);
            this.browseSourcePath.Name = "browseSourcePath";
            this.browseSourcePath.Size = new System.Drawing.Size(32, 23);
            this.browseSourcePath.TabIndex = 17;
            this.browseSourcePath.Text = "...";
            this.browseSourcePath.UseVisualStyleBackColor = true;
            this.browseSourcePath.Click += new System.EventHandler(this.browseSourcePath_Click);
            // 
            // browseTargetPath
            // 
            this.browseTargetPath.Location = new System.Drawing.Point(503, 36);
            this.browseTargetPath.Name = "browseTargetPath";
            this.browseTargetPath.Size = new System.Drawing.Size(32, 23);
            this.browseTargetPath.TabIndex = 17;
            this.browseTargetPath.Text = "...";
            this.browseTargetPath.UseVisualStyleBackColor = true;
            this.browseTargetPath.Click += new System.EventHandler(this.browseTargetPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "运行路径:";
            // 
            // textBoxTargetPath
            // 
            this.textBoxTargetPath.Location = new System.Drawing.Point(86, 38);
            this.textBoxTargetPath.Name = "textBoxTargetPath";
            this.textBoxTargetPath.Size = new System.Drawing.Size(411, 20);
            this.textBoxTargetPath.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "运行服务器列表:";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(460, 315);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 20;
            this.buttonUpdate.Text = "搜索";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // textBoxDownloadPath
            // 
            this.textBoxDownloadPath.Location = new System.Drawing.Point(86, 315);
            this.textBoxDownloadPath.Name = "textBoxDownloadPath";
            this.textBoxDownloadPath.Size = new System.Drawing.Size(225, 20);
            this.textBoxDownloadPath.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 318);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "下载路径:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MngServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 347);
            this.Controls.Add(this.textBoxDownloadPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.textBoxTargetPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSourcePath);
            this.Controls.Add(this.browseTargetPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browseSourcePath);
            this.Controls.Add(this.ServerView);
            this.MaximizeBox = false;
            this.Name = "MngServerForm";
            this.Text = "服务器登录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MngServerForm_FormClosing);
            this.Load += new System.EventHandler(this.MngServerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ServerView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ServerView;
        private System.Windows.Forms.TextBox textBoxSourcePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseSourcePath;
        private System.Windows.Forms.Button browseTargetPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTargetPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.TextBox textBoxDownloadPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Owner;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.Timer timer1;
    }
}

