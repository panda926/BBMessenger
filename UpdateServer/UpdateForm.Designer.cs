namespace UpdateServer
{
    partial class UpdateForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LogTimer = new System.Windows.Forms.Timer(this.components);
            this.tmr_receivefile = new System.Windows.Forms.Timer(this.components);
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textBoxTargetPath = new System.Windows.Forms.TextBox();
            this.LabelTotalNum = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSourcePath = new System.Windows.Forms.TextBox();
            this.browseTargetPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.browseSourcePath = new System.Windows.Forms.Button();
            this.FilesView = new System.Windows.Forms.DataGridView();
            this.textBoxDownloadPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboDownloadKind = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Owner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.FilesView)).BeginInit();
            this.SuspendLayout();
            // 
            // LogTimer
            // 
            this.LogTimer.Interval = 500;
            // 
            // tmr_receivefile
            // 
            this.tmr_receivefile.Interval = 1000;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(461, 79);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 31;
            this.buttonUpdate.Text = "注册";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click_1);
            // 
            // textBoxTargetPath
            // 
            this.textBoxTargetPath.Location = new System.Drawing.Point(87, 38);
            this.textBoxTargetPath.Name = "textBoxTargetPath";
            this.textBoxTargetPath.Size = new System.Drawing.Size(411, 20);
            this.textBoxTargetPath.TabIndex = 29;
            // 
            // LabelTotalNum
            // 
            this.LabelTotalNum.AutoSize = true;
            this.LabelTotalNum.Location = new System.Drawing.Point(12, 158);
            this.LabelTotalNum.Name = "LabelTotalNum";
            this.LabelTotalNum.Size = new System.Drawing.Size(106, 13);
            this.LabelTotalNum.TabIndex = 28;
            this.LabelTotalNum.Text = "可下载的文件列表:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "运行路径:";
            // 
            // textBoxSourcePath
            // 
            this.textBoxSourcePath.Location = new System.Drawing.Point(87, 12);
            this.textBoxSourcePath.Name = "textBoxSourcePath";
            this.textBoxSourcePath.Size = new System.Drawing.Size(411, 20);
            this.textBoxSourcePath.TabIndex = 30;
            // 
            // browseTargetPath
            // 
            this.browseTargetPath.Location = new System.Drawing.Point(504, 36);
            this.browseTargetPath.Name = "browseTargetPath";
            this.browseTargetPath.Size = new System.Drawing.Size(32, 23);
            this.browseTargetPath.TabIndex = 24;
            this.browseTargetPath.Text = "...";
            this.browseTargetPath.UseVisualStyleBackColor = true;
            this.browseTargetPath.Click += new System.EventHandler(this.browseTargetPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "源路径:";
            // 
            // browseSourcePath
            // 
            this.browseSourcePath.Location = new System.Drawing.Point(504, 10);
            this.browseSourcePath.Name = "browseSourcePath";
            this.browseSourcePath.Size = new System.Drawing.Size(32, 23);
            this.browseSourcePath.TabIndex = 25;
            this.browseSourcePath.Text = "...";
            this.browseSourcePath.UseVisualStyleBackColor = true;
            this.browseSourcePath.Click += new System.EventHandler(this.browseSourcePath_Click);
            // 
            // FilesView
            // 
            this.FilesView.AllowUserToAddRows = false;
            this.FilesView.AllowUserToDeleteRows = false;
            this.FilesView.AllowUserToResizeColumns = false;
            this.FilesView.AllowUserToResizeRows = false;
            this.FilesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FilesView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Owner,
            this.dataGridViewTextBoxColumn3});
            this.FilesView.Location = new System.Drawing.Point(13, 184);
            this.FilesView.Name = "FilesView";
            this.FilesView.ReadOnly = true;
            this.FilesView.RowHeadersVisible = false;
            this.FilesView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FilesView.Size = new System.Drawing.Size(523, 228);
            this.FilesView.TabIndex = 23;
            // 
            // textBoxDownloadPath
            // 
            this.textBoxDownloadPath.Location = new System.Drawing.Point(87, 119);
            this.textBoxDownloadPath.Name = "textBoxDownloadPath";
            this.textBoxDownloadPath.ReadOnly = true;
            this.textBoxDownloadPath.Size = new System.Drawing.Size(449, 20);
            this.textBoxDownloadPath.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "下载路径:";
            // 
            // comboDownloadKind
            // 
            this.comboDownloadKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDownloadKind.FormattingEnabled = true;
            this.comboDownloadKind.Location = new System.Drawing.Point(415, 155);
            this.comboDownloadKind.Name = "comboDownloadKind";
            this.comboDownloadKind.Size = new System.Drawing.Size(121, 21);
            this.comboDownloadKind.TabIndex = 36;
            this.comboDownloadKind.SelectedIndexChanged += new System.EventHandler(this.comboDownloadKind_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(351, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "下载样:";
            // 
            // Owner
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Owner.DefaultCellStyle = dataGridViewCellStyle1;
            this.Owner.HeaderText = "文件名";
            this.Owner.Name = "Owner";
            this.Owner.ReadOnly = true;
            this.Owner.Width = 420;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn3.HeaderText = "版本";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 424);
            this.Controls.Add(this.comboDownloadKind);
            this.Controls.Add(this.textBoxDownloadPath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.textBoxTargetPath);
            this.Controls.Add(this.LabelTotalNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSourcePath);
            this.Controls.Add(this.browseTargetPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browseSourcePath);
            this.Controls.Add(this.FilesView);
            this.Name = "UpdateForm";
            this.Text = "下载管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdateForm_FormClosing);
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FilesView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer LogTimer;
        private System.Windows.Forms.Timer tmr_receivefile;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.TextBox textBoxTargetPath;
        private System.Windows.Forms.Label LabelTotalNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSourcePath;
        private System.Windows.Forms.Button browseTargetPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseSourcePath;
        private System.Windows.Forms.DataGridView FilesView;
        private System.Windows.Forms.TextBox textBoxDownloadPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboDownloadKind;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Owner;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}

