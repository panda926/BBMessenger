namespace ChatServer
{
    partial class AddNotice
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
            this.label8 = new System.Windows.Forms.Label();
            this.textContent = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textTitle = new System.Windows.Forms.TextBox();
            this.comboKind = new System.Windows.Forms.ComboBox();
            this.labelKind = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(225, 312);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "确定";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "内容:";
            // 
            // textContent
            // 
            this.textContent.Location = new System.Drawing.Point(81, 99);
            this.textContent.Multiline = true;
            this.textContent.Name = "textContent";
            this.textContent.Size = new System.Drawing.Size(386, 190);
            this.textContent.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "题目:";
            // 
            // textTitle
            // 
            this.textTitle.Location = new System.Drawing.Point(81, 60);
            this.textTitle.Name = "textTitle";
            this.textTitle.Size = new System.Drawing.Size(386, 20);
            this.textTitle.TabIndex = 21;
            // 
            // comboKind
            // 
            this.comboKind.FormattingEnabled = true;
            this.comboKind.Location = new System.Drawing.Point(81, 28);
            this.comboKind.Name = "comboKind";
            this.comboKind.Size = new System.Drawing.Size(140, 21);
            this.comboKind.TabIndex = 23;
            // 
            // labelKind
            // 
            this.labelKind.AutoSize = true;
            this.labelKind.Location = new System.Drawing.Point(28, 31);
            this.labelKind.Name = "labelKind";
            this.labelKind.Size = new System.Drawing.Size(34, 13);
            this.labelKind.TabIndex = 24;
            this.labelKind.Text = "种类:";
            // 
            // AddNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 351);
            this.Controls.Add(this.labelKind);
            this.Controls.Add(this.comboKind);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textTitle);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textContent);
            this.Controls.Add(this.buttonOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddNotice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "公告添加";
            this.Load += new System.EventHandler(this.AddUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textContent;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textTitle;
        private System.Windows.Forms.ComboBox comboKind;
        private System.Windows.Forms.Label labelKind;
    }
}