namespace ChatServer
{
    partial class Main
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.UserMenus = new System.Windows.Forms.ToolStripMenuItem();
            this.ManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AutoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FirstBuyerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RoomMenus = new System.Windows.Forms.ToolStripMenuItem();
            this.RoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChatHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GameMenus = new System.Windows.Forms.ToolStripMenuItem();
            this.GameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GameHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChargeMenus = new System.Windows.Forms.ToolStripMenuItem();
            this.ApproveChargeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DischargeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mailboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lettersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noticesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EnvMenus = new System.Windows.Forms.ToolStripMenuItem();
            this.FinancialMenus = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9.2F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UserMenus,
            this.RoomMenus,
            this.GameMenus,
            this.ChargeMenus,
            this.mailboxToolStripMenuItem,
            this.EnvMenus,
            this.FinancialMenus});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(959, 25);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "회원관리";
            // 
            // UserMenus
            // 
            this.UserMenus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ManagerToolStripMenuItem,
            this.AutoMenuItem,
            this.FirstBuyerMenuItem});
            this.UserMenus.Name = "UserMenus";
            this.UserMenus.Size = new System.Drawing.Size(80, 21);
            this.UserMenus.Text = "会员管理";
            // 
            // ManagerToolStripMenuItem
            // 
            this.ManagerToolStripMenuItem.Name = "ManagerToolStripMenuItem";
            this.ManagerToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.ManagerToolStripMenuItem.Text = "管理者目录";
            this.ManagerToolStripMenuItem.Click += new System.EventHandler(this.ManagerToolStripMenuItem_Click);
            // 
            // AutoMenuItem
            // 
            this.AutoMenuItem.Name = "AutoMenuItem";
            this.AutoMenuItem.Size = new System.Drawing.Size(151, 22);
            this.AutoMenuItem.Text = "电脑 目录";
            this.AutoMenuItem.Click += new System.EventHandler(this.AutoMenuItem_Click);
            // 
            // FirstBuyerMenuItem
            // 
            this.FirstBuyerMenuItem.Name = "FirstBuyerMenuItem";
            this.FirstBuyerMenuItem.Size = new System.Drawing.Size(151, 22);
            this.FirstBuyerMenuItem.Text = "消费者目录";
            this.FirstBuyerMenuItem.Click += new System.EventHandler(this.FirstBuyerMenuItem_Click);
            // 
            // RoomMenus
            // 
            this.RoomMenus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RoomMenuItem,
            this.ChatHistoryToolStripMenuItem});
            this.RoomMenus.Name = "RoomMenus";
            this.RoomMenus.Size = new System.Drawing.Size(110, 21);
            this.RoomMenus.Text = "聊天频道管理";
            // 
            // RoomMenuItem
            // 
            this.RoomMenuItem.Name = "RoomMenuItem";
            this.RoomMenuItem.Size = new System.Drawing.Size(166, 22);
            this.RoomMenuItem.Text = "聊天频道目录";
            this.RoomMenuItem.Click += new System.EventHandler(this.RoomMenuItem_Click);
            // 
            // ChatHistoryToolStripMenuItem
            // 
            this.ChatHistoryToolStripMenuItem.Name = "ChatHistoryToolStripMenuItem";
            this.ChatHistoryToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.ChatHistoryToolStripMenuItem.Text = "元宝聊天信息";
            this.ChatHistoryToolStripMenuItem.Click += new System.EventHandler(this.ChatHistoryToolStripMenuItem_Click);
            // 
            // GameMenus
            // 
            this.GameMenus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GameMenuItem,
            this.GameHistoryToolStripMenuItem});
            this.GameMenus.Name = "GameMenus";
            this.GameMenus.Size = new System.Drawing.Size(80, 21);
            this.GameMenus.Text = "游戏管理";
            // 
            // GameMenuItem
            // 
            this.GameMenuItem.Name = "GameMenuItem";
            this.GameMenuItem.Size = new System.Drawing.Size(166, 22);
            this.GameMenuItem.Text = "游戏目录";
            this.GameMenuItem.Click += new System.EventHandler(this.GameMenuItem_Click);
            // 
            // GameHistoryToolStripMenuItem
            // 
            this.GameHistoryToolStripMenuItem.Name = "GameHistoryToolStripMenuItem";
            this.GameHistoryToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.GameHistoryToolStripMenuItem.Text = "元宝信息目录";
            this.GameHistoryToolStripMenuItem.Click += new System.EventHandler(this.GameHistoryToolStripMenuItem_Click);
            // 
            // ChargeMenus
            // 
            this.ChargeMenus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ApproveChargeMenuItem,
            this.DischargeToolStripMenuItem});
            this.ChargeMenus.Name = "ChargeMenus";
            this.ChargeMenus.Size = new System.Drawing.Size(80, 21);
            this.ChargeMenus.Text = "充值管理";
            // 
            // ApproveChargeMenuItem
            // 
            this.ApproveChargeMenuItem.Name = "ApproveChargeMenuItem";
            this.ApproveChargeMenuItem.Size = new System.Drawing.Size(166, 22);
            this.ApproveChargeMenuItem.Text = "充值/兑换";
            this.ApproveChargeMenuItem.Click += new System.EventHandler(this.ApproveChargeMenuItem_Click);
            // 
            // DischargeToolStripMenuItem
            // 
            this.DischargeToolStripMenuItem.Name = "DischargeToolStripMenuItem";
            this.DischargeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.DischargeToolStripMenuItem.Text = "充值兑换目录";
            this.DischargeToolStripMenuItem.Click += new System.EventHandler(this.DischargeToolStripMenuItem_Click);
            // 
            // mailboxToolStripMenuItem
            // 
            this.mailboxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lettersToolStripMenuItem,
            this.noticesToolStripMenuItem});
            this.mailboxToolStripMenuItem.Name = "mailboxToolStripMenuItem";
            this.mailboxToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.mailboxToolStripMenuItem.Text = "邮箱管理";
            this.mailboxToolStripMenuItem.Click += new System.EventHandler(this.mailboxToolStripMenuItem_Click);
            // 
            // lettersToolStripMenuItem
            // 
            this.lettersToolStripMenuItem.Name = "lettersToolStripMenuItem";
            this.lettersToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.lettersToolStripMenuItem.Text = "信息";
            this.lettersToolStripMenuItem.Click += new System.EventHandler(this.lettersToolStripMenuItem_Click);
            // 
            // noticesToolStripMenuItem
            // 
            this.noticesToolStripMenuItem.Name = "noticesToolStripMenuItem";
            this.noticesToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.noticesToolStripMenuItem.Text = "公告事项";
            this.noticesToolStripMenuItem.Click += new System.EventHandler(this.noticesToolStripMenuItem_Click);
            // 
            // EnvMenus
            // 
            this.EnvMenus.Name = "EnvMenus";
            this.EnvMenus.Size = new System.Drawing.Size(50, 21);
            this.EnvMenus.Text = "设置";
            this.EnvMenus.Click += new System.EventHandler(this.EnvMenus_Click);
            // 
            // FinancialMenus
            // 
            this.FinancialMenus.Name = "FinancialMenus";
            this.FinancialMenus.Size = new System.Drawing.Size(80, 21);
            this.FinancialMenus.Text = "财务结算";
            this.FinancialMenus.Click += new System.EventHandler(this.FinancialMenus_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 567);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.Name = "Main";
            this.Text = "Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem UserMenus;
        private System.Windows.Forms.ToolStripMenuItem ManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AutoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FirstBuyerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RoomMenus;
        private System.Windows.Forms.ToolStripMenuItem RoomMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChatHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GameMenus;
        private System.Windows.Forms.ToolStripMenuItem GameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GameHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChargeMenus;
        private System.Windows.Forms.ToolStripMenuItem DischargeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EnvMenus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem mailboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lettersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noticesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ApproveChargeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FinancialMenus;
    }
}