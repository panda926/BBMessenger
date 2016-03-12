namespace ChatServer
{
    partial class MngEnviroment
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
            this.label2 = new System.Windows.Forms.Label();
            this.textChargePoint = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBonusPoint = new System.Windows.Forms.TextBox();
            this.labelSummary = new System.Windows.Forms.Label();
            this.textNewAccount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textImageUploadPath = new System.Windows.Forms.TextBox();
            this.textEveryDayPoint = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtChargeSiteUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textChargeGivePercent = new System.Windows.Forms.TextBox();
            this.textCashRate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(231, 293);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 6;
            this.buttonOk.Text = "设置";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(384, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "充值积分";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // textChargePoint
            // 
            this.textChargePoint.Location = new System.Drawing.Point(455, 84);
            this.textChargePoint.Name = "textChargePoint";
            this.textChargePoint.Size = new System.Drawing.Size(63, 20);
            this.textChargePoint.TabIndex = 21;
            this.textChargePoint.TextChanged += new System.EventHandler(this.textChargePoint_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(228, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 18;
            this.label5.Text = "登录积分";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // textBonusPoint
            // 
            this.textBonusPoint.Location = new System.Drawing.Point(296, 84);
            this.textBonusPoint.Name = "textBonusPoint";
            this.textBonusPoint.Size = new System.Drawing.Size(63, 20);
            this.textBonusPoint.TabIndex = 19;
            this.textBonusPoint.TextChanged += new System.EventHandler(this.textBonusPoint_TextChanged);
            // 
            // labelSummary
            // 
            this.labelSummary.AutoSize = true;
            this.labelSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSummary.Location = new System.Drawing.Point(45, 45);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(79, 15);
            this.labelSummary.TabIndex = 16;
            this.labelSummary.Text = "每天注册次数";
            // 
            // textNewAccount
            // 
            this.textNewAccount.Location = new System.Drawing.Point(137, 45);
            this.textNewAccount.Name = "textNewAccount";
            this.textNewAccount.Size = new System.Drawing.Size(63, 20);
            this.textNewAccount.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(42, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "存款网站 路径";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(42, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 15);
            this.label7.TabIndex = 22;
            this.label7.Text = "更新图片路径";
            // 
            // textImageUploadPath
            // 
            this.textImageUploadPath.Location = new System.Drawing.Point(137, 129);
            this.textImageUploadPath.Name = "textImageUploadPath";
            this.textImageUploadPath.Size = new System.Drawing.Size(381, 20);
            this.textImageUploadPath.TabIndex = 23;
            // 
            // textEveryDayPoint
            // 
            this.textEveryDayPoint.Location = new System.Drawing.Point(137, 84);
            this.textEveryDayPoint.Name = "textEveryDayPoint";
            this.textEveryDayPoint.Size = new System.Drawing.Size(63, 20);
            this.textEveryDayPoint.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(66, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "每天积分";
            // 
            // txtChargeSiteUrl
            // 
            this.txtChargeSiteUrl.Location = new System.Drawing.Point(137, 173);
            this.txtChargeSiteUrl.Name = "txtChargeSiteUrl";
            this.txtChargeSiteUrl.Size = new System.Drawing.Size(381, 20);
            this.txtChargeSiteUrl.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(66, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 15);
            this.label4.TabIndex = 25;
            this.label4.Text = "存款利率";
            // 
            // textChargeGivePercent
            // 
            this.textChargeGivePercent.Location = new System.Drawing.Point(137, 214);
            this.textChargeGivePercent.Name = "textChargeGivePercent";
            this.textChargeGivePercent.Size = new System.Drawing.Size(63, 20);
            this.textChargeGivePercent.TabIndex = 26;
            // 
            // textCashRate
            // 
            this.textCashRate.Location = new System.Drawing.Point(342, 214);
            this.textCashRate.Name = "textCashRate";
            this.textCashRate.Size = new System.Drawing.Size(63, 20);
            this.textCashRate.TabIndex = 26;
            this.textCashRate.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(228, 215);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 15);
            this.label6.TabIndex = 25;
            this.label6.Text = "游戏货币转换率";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // MngEnviroment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 336);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textCashRate);
            this.Controls.Add(this.textChargeGivePercent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtChargeSiteUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textImageUploadPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textChargePoint);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBonusPoint);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelSummary);
            this.Controls.Add(this.textEveryDayPoint);
            this.Controls.Add(this.textNewAccount);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MngEnviroment";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.MngUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textChargePoint;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBonusPoint;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.TextBox textNewAccount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textImageUploadPath;
        private System.Windows.Forms.TextBox textEveryDayPoint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtChargeSiteUrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textChargeGivePercent;
        private System.Windows.Forms.TextBox textCashRate;
        private System.Windows.Forms.Label label6;
    }
}