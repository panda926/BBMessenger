namespace ChatServer
{
    partial class MngNotice
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.NoticeView = new System.Windows.Forms.DataGridView();
            this.labelSummary = new System.Windows.Forms.Label();
            this.buttonDelUser = new System.Windows.Forms.Button();
            this.buttonAddUser = new System.Windows.Forms.Button();
            this.buttonReadNotice = new System.Windows.Forms.Button();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.NoticeView)).BeginInit();
            this.SuspendLayout();
            // 
            // NoticeView
            // 
            this.NoticeView.AllowUserToAddRows = false;
            this.NoticeView.AllowUserToDeleteRows = false;
            this.NoticeView.AllowUserToResizeColumns = false;
            this.NoticeView.AllowUserToResizeRows = false;
            this.NoticeView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NoticeView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.User,
            this.Column1,
            this.Column3,
            this.Column2,
            this.column11});
            this.NoticeView.Location = new System.Drawing.Point(12, 51);
            this.NoticeView.Name = "NoticeView";
            this.NoticeView.ReadOnly = true;
            this.NoticeView.RowHeadersVisible = false;
            this.NoticeView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.NoticeView.Size = new System.Drawing.Size(905, 484);
            this.NoticeView.TabIndex = 1;
            // 
            // labelSummary
            // 
            this.labelSummary.AutoSize = true;
            this.labelSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSummary.Location = new System.Drawing.Point(13, 18);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(67, 15);
            this.labelSummary.TabIndex = 5;
            this.labelSummary.Text = "总公告数量";
            // 
            // buttonDelUser
            // 
            this.buttonDelUser.Location = new System.Drawing.Point(842, 545);
            this.buttonDelUser.Name = "buttonDelUser";
            this.buttonDelUser.Size = new System.Drawing.Size(75, 23);
            this.buttonDelUser.TabIndex = 8;
            this.buttonDelUser.Text = "删除";
            this.buttonDelUser.UseVisualStyleBackColor = true;
            this.buttonDelUser.Click += new System.EventHandler(this.buttonDelUser_Click);
            // 
            // buttonAddUser
            // 
            this.buttonAddUser.Location = new System.Drawing.Point(13, 545);
            this.buttonAddUser.Name = "buttonAddUser";
            this.buttonAddUser.Size = new System.Drawing.Size(75, 23);
            this.buttonAddUser.TabIndex = 6;
            this.buttonAddUser.Text = "添加";
            this.buttonAddUser.UseVisualStyleBackColor = true;
            this.buttonAddUser.Click += new System.EventHandler(this.buttonAddUser_Click);
            // 
            // buttonReadNotice
            // 
            this.buttonReadNotice.Location = new System.Drawing.Point(750, 545);
            this.buttonReadNotice.Name = "buttonReadNotice";
            this.buttonReadNotice.Size = new System.Drawing.Size(75, 23);
            this.buttonReadNotice.TabIndex = 6;
            this.buttonReadNotice.Text = "预览";
            this.buttonReadNotice.UseVisualStyleBackColor = true;
            this.buttonReadNotice.Click += new System.EventHandler(this.buttonEditUser_Click);
            // 
            // Id
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Id.DefaultCellStyle = dataGridViewCellStyle1;
            this.Id.HeaderText = "号码";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Width = 70;
            // 
            // User
            // 
            this.User.HeaderText = "寄件人";
            this.User.Name = "User";
            this.User.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "收件人";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column3
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column3.HeaderText = "题目";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 200;
            // 
            // Column2
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "内容";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 300;
            // 
            // column11
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.column11.DefaultCellStyle = dataGridViewCellStyle4;
            this.column11.HeaderText = "日期";
            this.column11.Name = "column11";
            this.column11.ReadOnly = true;
            // 
            // MngNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 576);
            this.Controls.Add(this.buttonDelUser);
            this.Controls.Add(this.buttonReadNotice);
            this.Controls.Add(this.buttonAddUser);
            this.Controls.Add(this.labelSummary);
            this.Controls.Add(this.NoticeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MngNotice";
            this.Text = "公告";
            this.Load += new System.EventHandler(this.MngUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NoticeView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView NoticeView;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.Button buttonDelUser;
        private System.Windows.Forms.Button buttonAddUser;
        private System.Windows.Forms.Button buttonReadNotice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn column11;
    }
}