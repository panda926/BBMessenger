namespace ChatServer
{
    partial class MngGame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MngGame));
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
            this.GameView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Owner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GameView)).BeginInit();
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
            this.labelSummary.Size = new System.Drawing.Size(67, 15);
            this.labelSummary.TabIndex = 5;
            this.labelSummary.Text = "游戏总数量";
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
            // GameView
            // 
            this.GameView.AllowUserToAddRows = false;
            this.GameView.AllowUserToDeleteRows = false;
            this.GameView.AllowUserToResizeColumns = false;
            this.GameView.AllowUserToResizeRows = false;
            this.GameView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GameView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.IdColumn,
            this.dataGridViewTextBoxColumn2,
            this.Nomer,
            this.Owner,
            this.Column1});
            this.GameView.Location = new System.Drawing.Point(12, 52);
            this.GameView.Name = "GameView";
            this.GameView.ReadOnly = true;
            this.GameView.RowHeadersVisible = false;
            this.GameView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GameView.Size = new System.Drawing.Size(747, 480);
            this.GameView.TabIndex = 12;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle1.NullValue")));
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn3.HeaderText = "图象";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // IdColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.IdColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.IdColumn.HeaderText = "游戏帐号";
            this.IdColumn.Name = "IdColumn";
            this.IdColumn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn2.HeaderText = "游戏名字";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // Nomer
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Nomer.DefaultCellStyle = dataGridViewCellStyle4;
            this.Nomer.HeaderText = "高度";
            this.Nomer.Name = "Nomer";
            this.Nomer.ReadOnly = true;
            // 
            // Owner
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Owner.DefaultCellStyle = dataGridViewCellStyle5;
            this.Owner.HeaderText = "宽度";
            this.Owner.Name = "Owner";
            this.Owner.ReadOnly = true;
            // 
            // Column1
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column1.HeaderText = "上传地址";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 200;
            // 
            // MngGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 576);
            this.Controls.Add(this.GameView);
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
            this.Name = "MngGame";
            this.Text = "游戏目录";
            this.Load += new System.EventHandler(this.MngUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GameView)).EndInit();
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
        private System.Windows.Forms.DataGridView GameView;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Owner;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}