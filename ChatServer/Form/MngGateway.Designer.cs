namespace ChatServer
{
    partial class MngGateway
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
            this.comboKind = new System.Windows.Forms.ComboBox();
            this.buttonFind = new System.Windows.Forms.Button();
            this.labelSummary = new System.Windows.Forms.Label();
            this.buttonDelGateway = new System.Windows.Forms.Button();
            this.buttonEditGateway = new System.Windows.Forms.Button();
            this.buttonAddGateway = new System.Windows.Forms.Button();
            this.textContent = new System.Windows.Forms.TextBox();
            this.GatewayView = new System.Windows.Forms.DataGridView();
            this.Owner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GatewayView)).BeginInit();
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
            // buttonDelGateway
            // 
            this.buttonDelGateway.Location = new System.Drawing.Point(204, 545);
            this.buttonDelGateway.Name = "buttonDelGateway";
            this.buttonDelGateway.Size = new System.Drawing.Size(75, 23);
            this.buttonDelGateway.TabIndex = 8;
            this.buttonDelGateway.Text = "删除";
            this.buttonDelGateway.UseVisualStyleBackColor = true;
            this.buttonDelGateway.Click += new System.EventHandler(this.buttonDelRoom_Click);
            // 
            // buttonEditGateway
            // 
            this.buttonEditGateway.Location = new System.Drawing.Point(108, 545);
            this.buttonEditGateway.Name = "buttonEditGateway";
            this.buttonEditGateway.Size = new System.Drawing.Size(75, 23);
            this.buttonEditGateway.TabIndex = 7;
            this.buttonEditGateway.Text = "修改";
            this.buttonEditGateway.UseVisualStyleBackColor = true;
            this.buttonEditGateway.Click += new System.EventHandler(this.buttonEditRoom_Click);
            // 
            // buttonAddGateway
            // 
            this.buttonAddGateway.Location = new System.Drawing.Point(13, 545);
            this.buttonAddGateway.Name = "buttonAddGateway";
            this.buttonAddGateway.Size = new System.Drawing.Size(75, 23);
            this.buttonAddGateway.TabIndex = 6;
            this.buttonAddGateway.Text = "添加";
            this.buttonAddGateway.UseVisualStyleBackColor = true;
            this.buttonAddGateway.Click += new System.EventHandler(this.buttonAddRoom_Click);
            // 
            // textContent
            // 
            this.textContent.Location = new System.Drawing.Point(537, 16);
            this.textContent.Name = "textContent";
            this.textContent.Size = new System.Drawing.Size(120, 20);
            this.textContent.TabIndex = 10;
            // 
            // GatewayView
            // 
            this.GatewayView.AllowUserToAddRows = false;
            this.GatewayView.AllowUserToDeleteRows = false;
            this.GatewayView.AllowUserToResizeColumns = false;
            this.GatewayView.AllowUserToResizeRows = false;
            this.GatewayView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GatewayView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Owner,
            this.IdColumn,
            this.dataGridViewTextBoxColumn2,
            this.Nomer});
            this.GatewayView.Location = new System.Drawing.Point(12, 52);
            this.GatewayView.Name = "GatewayView";
            this.GatewayView.ReadOnly = true;
            this.GatewayView.RowHeadersVisible = false;
            this.GatewayView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GatewayView.Size = new System.Drawing.Size(747, 480);
            this.GatewayView.TabIndex = 12;
            // 
            // Owner
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Owner.DefaultCellStyle = dataGridViewCellStyle1;
            this.Owner.HeaderText = "银行";
            this.Owner.Name = "Owner";
            this.Owner.ReadOnly = true;
            this.Owner.Width = 120;
            // 
            // IdColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.IdColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.IdColumn.HeaderText = "账号";
            this.IdColumn.Name = "IdColumn";
            this.IdColumn.ReadOnly = true;
            this.IdColumn.Width = 130;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn2.HeaderText = "自我";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // Nomer
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Nomer.DefaultCellStyle = dataGridViewCellStyle4;
            this.Nomer.HeaderText = "出入口";
            this.Nomer.Name = "Nomer";
            this.Nomer.ReadOnly = true;
            this.Nomer.Width = 270;
            // 
            // MngGateway
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 576);
            this.Controls.Add(this.GatewayView);
            this.Controls.Add(this.textContent);
            this.Controls.Add(this.buttonDelGateway);
            this.Controls.Add(this.buttonEditGateway);
            this.Controls.Add(this.buttonAddGateway);
            this.Controls.Add(this.labelSummary);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.comboKind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MngGateway";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gateway";
            this.Load += new System.EventHandler(this.MngUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GatewayView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboKind;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.Button buttonDelGateway;
        private System.Windows.Forms.Button buttonEditGateway;
        private System.Windows.Forms.Button buttonAddGateway;
        private System.Windows.Forms.TextBox textContent;
        private System.Windows.Forms.DataGridView GatewayView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Owner;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nomer;
    }
}