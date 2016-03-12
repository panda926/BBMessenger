namespace ChatServer
{
    partial class LogView
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
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.ResultView = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearLog.Location = new System.Drawing.Point(349, 178);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLog.TabIndex = 20;
            this.buttonClearLog.Text = "初始化";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            // 
            // ResultView
            // 
            this.ResultView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultView.FormattingEnabled = true;
            this.ResultView.Location = new System.Drawing.Point(12, 12);
            this.ResultView.Name = "ResultView";
            this.ResultView.Size = new System.Drawing.Size(412, 160);
            this.ResultView.TabIndex = 19;
            // 
            // LogView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 209);
            this.Controls.Add(this.buttonClearLog);
            this.Controls.Add(this.ResultView);
            this.Name = "LogView";
            this.Text = "登陆窗口";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.ListBox ResultView;
    }
}