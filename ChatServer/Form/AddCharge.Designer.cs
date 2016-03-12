namespace ChatServer
{
    partial class AddCharge
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textMoney = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textUserId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textServiceMoney = new System.Windows.Forms.TextBox();
            this.textRealMoney = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCash = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(169, 189);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "确定";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(279, 189);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(237, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "金额:";
            // 
            // textMoney
            // 
            this.textMoney.Location = new System.Drawing.Point(279, 25);
            this.textMoney.Name = "textMoney";
            this.textMoney.Size = new System.Drawing.Size(84, 20);
            this.textMoney.TabIndex = 19;
            this.textMoney.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textMoney.TextChanged += new System.EventHandler(this.textMoney_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "会员账号:";
            // 
            // textUserId
            // 
            this.textUserId.Location = new System.Drawing.Point(85, 26);
            this.textUserId.Name = "textUserId";
            this.textUserId.Size = new System.Drawing.Size(127, 20);
            this.textUserId.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "佣金:";
            // 
            // textServiceMoney
            // 
            this.textServiceMoney.Location = new System.Drawing.Point(279, 69);
            this.textServiceMoney.Name = "textServiceMoney";
            this.textServiceMoney.ReadOnly = true;
            this.textServiceMoney.Size = new System.Drawing.Size(84, 20);
            this.textServiceMoney.TabIndex = 23;
            this.textServiceMoney.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textRealMoney
            // 
            this.textRealMoney.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textRealMoney.Location = new System.Drawing.Point(169, 118);
            this.textRealMoney.Name = "textRealMoney";
            this.textRealMoney.ReadOnly = true;
            this.textRealMoney.Size = new System.Drawing.Size(194, 29);
            this.textRealMoney.TabIndex = 23;
            this.textRealMoney.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(113, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "存款:";
            // 
            // labelCash
            // 
            this.labelCash.ForeColor = System.Drawing.Color.Red;
            this.labelCash.Location = new System.Drawing.Point(378, 28);
            this.labelCash.Name = "labelCash";
            this.labelCash.Size = new System.Drawing.Size(138, 18);
            this.labelCash.TabIndex = 27;
            this.labelCash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddCharge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 240);
            this.Controls.Add(this.labelCash);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textRealMoney);
            this.Controls.Add(this.textServiceMoney);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textUserId);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textMoney);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCharge";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "兑换";
            this.Load += new System.EventHandler(this.AddUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textMoney;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textUserId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textServiceMoney;
        private System.Windows.Forms.TextBox textRealMoney;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCash;
    }
}