namespace ChatServer
{
    partial class MngRoom
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.comboKind = new System.Windows.Forms.ComboBox();
            this.buttonFind = new System.Windows.Forms.Button();
            this.labelSummary = new System.Windows.Forms.Label();
            this.buttonDelRoom = new System.Windows.Forms.Button();
            this.buttonEditRoom = new System.Windows.Forms.Button();
            this.buttonAddRoom = new System.Windows.Forms.Button();
            this.textContent = new System.Windows.Forms.TextBox();
            this.RoomView = new System.Windows.Forms.DataGridView();
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Owner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.RoomView)).BeginInit();
            this.SuspendLayout();
            // 
            // comboKind
            // 
            this.comboKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboKind.FormattingEnabled = true;
            this.comboKind.Location = new System.Drawing.Point(426, 16);
            this.comboKind.Name = "comboKind";
            this.comboKind.Size = new System.Drawing.Size(94, 21);
            this.comboKind.TabIndex = 2;
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(683, 14);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 4;
            this.buttonFind.Text = "搜索";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // labelSummary
            // 
            this.labelSummary.AutoSize = true;
            this.labelSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSummary.Location = new System.Drawing.Point(13, 18);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(55, 15);
            this.labelSummary.TabIndex = 5;
            this.labelSummary.Text = "总会员数";
            // 
            // buttonDelRoom
            // 
            this.buttonDelRoom.Location = new System.Drawing.Point(204, 545);
            this.buttonDelRoom.Name = "buttonDelRoom";
            this.buttonDelRoom.Size = new System.Drawing.Size(75, 23);
            this.buttonDelRoom.TabIndex = 8;
            this.buttonDelRoom.Text = "删除";
            this.buttonDelRoom.UseVisualStyleBackColor = true;
            this.buttonDelRoom.Click += new System.EventHandler(this.buttonDelRoom_Click);
            // 
            // buttonEditRoom
            // 
            this.buttonEditRoom.Location = new System.Drawing.Point(108, 545);
            this.buttonEditRoom.Name = "buttonEditRoom";
            this.buttonEditRoom.Size = new System.Drawing.Size(75, 23);
            this.buttonEditRoom.TabIndex = 7;
            this.buttonEditRoom.Text = "修改";
            this.buttonEditRoom.UseVisualStyleBackColor = true;
            this.buttonEditRoom.Click += new System.EventHandler(this.buttonEditRoom_Click);
            // 
            // buttonAddRoom
            // 
            this.buttonAddRoom.Location = new System.Drawing.Point(13, 545);
            this.buttonAddRoom.Name = "buttonAddRoom";
            this.buttonAddRoom.Size = new System.Drawing.Size(75, 23);
            this.buttonAddRoom.TabIndex = 6;
            this.buttonAddRoom.Text = "添加";
            this.buttonAddRoom.UseVisualStyleBackColor = true;
            this.buttonAddRoom.Click += new System.EventHandler(this.buttonAddRoom_Click);
            // 
            // textContent
            // 
            this.textContent.Location = new System.Drawing.Point(537, 16);
            this.textContent.Name = "textContent";
            this.textContent.Size = new System.Drawing.Size(120, 20);
            this.textContent.TabIndex = 10;
            // 
            // RoomView
            // 
            this.RoomView.AllowUserToAddRows = false;
            this.RoomView.AllowUserToDeleteRows = false;
            this.RoomView.AllowUserToResizeColumns = false;
            this.RoomView.AllowUserToResizeRows = false;
            this.RoomView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RoomView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdColumn,
            this.dataGridViewTextBoxColumn2,
            this.Nomer,
            this.Owner,
            this.dataGridViewTextBoxColumn3,
            this.Column1});
            this.RoomView.Location = new System.Drawing.Point(12, 52);
            this.RoomView.Name = "RoomView";
            this.RoomView.ReadOnly = true;
            this.RoomView.RowHeadersVisible = false;
            this.RoomView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.RoomView.Size = new System.Drawing.Size(747, 480);
            this.RoomView.TabIndex = 12;
            // 
            // IdColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.IdColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.IdColumn.HeaderText = "频道帐号";
            this.IdColumn.Name = "IdColumn";
            this.IdColumn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn2.HeaderText = "频道名字";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // Nomer
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Nomer.DefaultCellStyle = dataGridViewCellStyle3;
            this.Nomer.HeaderText = "人数";
            this.Nomer.Name = "Nomer";
            this.Nomer.ReadOnly = true;
            // 
            // Owner
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Owner.DefaultCellStyle = dataGridViewCellStyle4;
            this.Owner.HeaderText = "创建人";
            this.Owner.Name = "Owner";
            this.Owner.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn3.HeaderText = "价格";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // Column1
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column1.HeaderText = "积分";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // MngRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 576);
            this.Controls.Add(this.RoomView);
            this.Controls.Add(this.textContent);
            this.Controls.Add(this.buttonDelRoom);
            this.Controls.Add(this.buttonEditRoom);
            this.Controls.Add(this.buttonAddRoom);
            this.Controls.Add(this.labelSummary);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.comboKind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MngRoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "频道管理";
            this.Load += new System.EventHandler(this.MngUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RoomView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboKind;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.Button buttonDelRoom;
        private System.Windows.Forms.Button buttonEditRoom;
        private System.Windows.Forms.Button buttonAddRoom;
        private System.Windows.Forms.TextBox textContent;
        private System.Windows.Forms.DataGridView RoomView;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Owner;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}