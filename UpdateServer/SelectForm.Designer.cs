namespace UpdateServer
{
    partial class SelectForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.SourceView = new System.Windows.Forms.DataGridView();
            this.Owner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonDeSelect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SourceView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetView)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(742, 444);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 32;
            this.buttonUpdate.Text = "注册";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // SourceView
            // 
            this.SourceView.AllowDrop = true;
            this.SourceView.AllowUserToAddRows = false;
            this.SourceView.AllowUserToDeleteRows = false;
            this.SourceView.AllowUserToResizeColumns = false;
            this.SourceView.AllowUserToResizeRows = false;
            this.SourceView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SourceView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Owner,
            this.dataGridViewTextBoxColumn3});
            this.SourceView.Location = new System.Drawing.Point(12, 12);
            this.SourceView.Name = "SourceView";
            this.SourceView.ReadOnly = true;
            this.SourceView.RowHeadersVisible = false;
            this.SourceView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.SourceView.Size = new System.Drawing.Size(376, 422);
            this.SourceView.TabIndex = 33;
            this.SourceView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SourceView_CellDoubleClick);
            // 
            // Owner
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Owner.DefaultCellStyle = dataGridViewCellStyle9;
            this.Owner.HeaderText = "文件名";
            this.Owner.Name = "Owner";
            this.Owner.ReadOnly = true;
            this.Owner.Width = 250;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn3.HeaderText = "创建时间";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // TargetView
            // 
            this.TargetView.AllowDrop = true;
            this.TargetView.AllowUserToAddRows = false;
            this.TargetView.AllowUserToDeleteRows = false;
            this.TargetView.AllowUserToResizeColumns = false;
            this.TargetView.AllowUserToResizeRows = false;
            this.TargetView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TargetView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.TargetView.Location = new System.Drawing.Point(441, 12);
            this.TargetView.Name = "TargetView";
            this.TargetView.ReadOnly = true;
            this.TargetView.RowHeadersVisible = false;
            this.TargetView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TargetView.Size = new System.Drawing.Size(376, 422);
            this.TargetView.TabIndex = 33;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn1.HeaderText = "文件名";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 250;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewTextBoxColumn2.HeaderText = "创建时间";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(394, 166);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(41, 33);
            this.buttonSelect.TabIndex = 32;
            this.buttonSelect.Text = "-->";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonDeSelect
            // 
            this.buttonDeSelect.Location = new System.Drawing.Point(394, 253);
            this.buttonDeSelect.Name = "buttonDeSelect";
            this.buttonDeSelect.Size = new System.Drawing.Size(41, 33);
            this.buttonDeSelect.TabIndex = 32;
            this.buttonDeSelect.Text = "<--";
            this.buttonDeSelect.UseVisualStyleBackColor = true;
            this.buttonDeSelect.Click += new System.EventHandler(this.buttonDeSelect_Click);
            // 
            // SelectForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 475);
            this.Controls.Add(this.TargetView);
            this.Controls.Add(this.SourceView);
            this.Controls.Add(this.buttonDeSelect);
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.buttonUpdate);
            this.Name = "SelectForm";
            this.Text = "选择文件和文件夹";
            this.Load += new System.EventHandler(this.SelectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SourceView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TargetView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.DataGridView SourceView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Owner;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridView TargetView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonDeSelect;
    }
}