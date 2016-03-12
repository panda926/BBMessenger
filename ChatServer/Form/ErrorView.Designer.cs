namespace ChatServer
{
    partial class ErrorView
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
            this.buttonClearError = new System.Windows.Forms.Button();
            this.ResultView = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonClearError
            // 
            this.buttonClearError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearError.Location = new System.Drawing.Point(231, 177);
            this.buttonClearError.Name = "buttonClearError";
            this.buttonClearError.Size = new System.Drawing.Size(75, 23);
            this.buttonClearError.TabIndex = 21;
            this.buttonClearError.Text = "初始化";
            this.buttonClearError.UseVisualStyleBackColor = true;
            // 
            // ResultView
            // 
            this.ResultView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultView.FormattingEnabled = true;
            this.ResultView.Location = new System.Drawing.Point(12, 12);
            this.ResultView.Name = "ResultView";
            this.ResultView.Size = new System.Drawing.Size(294, 160);
            this.ResultView.TabIndex = 20;
            this.ResultView.DoubleClick += new System.EventHandler(this.ResultView_DoubleClick);
            // 
            // ErrorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 208);
            this.Controls.Add(this.buttonClearError);
            this.Controls.Add(this.ResultView);
            this.Name = "ErrorView";
            this.Text = "错误窗口";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClearError;
        private System.Windows.Forms.ListBox ResultView;
    }
}