namespace ChatClient
{
    partial class ManVideoFormcs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManVideoFormcs));
            this.panel1 = new System.Windows.Forms.Panel();
            this.qqGlassButton1 = new ControlExs.QQGlassButton();
            this.qqGlassButton2 = new ControlExs.QQGlassButton();
            ((System.ComponentModel.ISupportInitialize)(this.qqGlassButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qqGlassButton2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::ChatClient.Properties.Resources.videowindow;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(8, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 180);
            this.panel1.TabIndex = 1;            
            // 
            // qqGlassButton1
            // 
            this.qqGlassButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.qqGlassButton1.BackColor = System.Drawing.Color.Transparent;
            this.qqGlassButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.qqGlassButton1.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.qqGlassButton1.Image = ((System.Drawing.Image)(resources.GetObject("qqGlassButton1.Image")));
            this.qqGlassButton1.Location = new System.Drawing.Point(8, 218);
            this.qqGlassButton1.Name = "qqGlassButton1";
            this.qqGlassButton1.Size = new System.Drawing.Size(23, 22);
            this.qqGlassButton1.TabIndex = 2;
            this.qqGlassButton1.TabStop = false;
            this.qqGlassButton1.ToolTipText = null;
            this.qqGlassButton1.Click += new System.EventHandler(this.qqGlassButton1_Click);
            // 
            // qqGlassButton2
            // 
            this.qqGlassButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.qqGlassButton2.BackColor = System.Drawing.Color.Transparent;
            this.qqGlassButton2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.qqGlassButton2.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.qqGlassButton2.Image = ((System.Drawing.Image)(resources.GetObject("qqGlassButton2.Image")));
            this.qqGlassButton2.Location = new System.Drawing.Point(33, 219);
            this.qqGlassButton2.Name = "qqGlassButton2";
            this.qqGlassButton2.Size = new System.Drawing.Size(23, 22);
            this.qqGlassButton2.TabIndex = 3;
            this.qqGlassButton2.TabStop = false;
            this.qqGlassButton2.ToolTipText = null;
            this.qqGlassButton2.Click += new System.EventHandler(this.qqGlassButton2_Click);
            // 
            // ManVideoFormcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CanResize = false;
            this.ClientSize = new System.Drawing.Size(256, 245);
            this.Controls.Add(this.qqGlassButton2);
            this.Controls.Add(this.qqGlassButton1);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximumSize = new System.Drawing.Size(336, 305);
            this.Name = "ManVideoFormcs";
            this.Tag = "TempSoloVideoForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManVideoFormcs_FormClosing);
            this.Load += new System.EventHandler(this.ManVideoFormcs_Load);
            this.SizeChanged += new System.EventHandler(this.ManVideoFormcs_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.qqGlassButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qqGlassButton2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ControlExs.QQGlassButton qqGlassButton1;
        private ControlExs.QQGlassButton qqGlassButton2;
    }
}