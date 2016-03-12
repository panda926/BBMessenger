namespace ChatClient
{
    partial class WomanVideoFormcs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WomanVideoFormcs));
            this.panelLocal = new System.Windows.Forms.Panel();
            this.panelRemote = new System.Windows.Forms.Panel();
            this.panelLocal.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLocal
            // 
            this.panelLocal.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelLocal.BackgroundImage = global::ChatClient.Properties.Resources.videowindow;
            this.panelLocal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelLocal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLocal.Controls.Add(this.panelRemote);
            this.panelLocal.Location = new System.Drawing.Point(13, 35);
            this.panelLocal.Name = "panelLocal";
            this.panelLocal.Size = new System.Drawing.Size(395, 303);
            this.panelLocal.TabIndex = 0;
            // 
            // panelRemote
            // 
            this.panelRemote.Location = new System.Drawing.Point(194, 198);
            this.panelRemote.Name = "panelRemote";
            this.panelRemote.Size = new System.Drawing.Size(200, 100);
            this.panelRemote.TabIndex = 0;
            this.panelRemote.Visible = false;
            // 
            // WomanVideoFormcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(420, 350);
            this.Controls.Add(this.panelLocal);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WomanVideoFormcs";
            this.Text = "WomanVideoFormcs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WomanVideoFormcs_FormClosing);
            this.Load += new System.EventHandler(this.WomanVideoFormcs_Load);
            this.panelLocal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLocal;
        private System.Windows.Forms.Panel panelRemote;
    }
}