namespace SicboClient
{
    partial class SicboView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SicboView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SicboClient.Properties.Resources.VIEW_BACK;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DoubleBuffered = true;
            this.Name = "SicboView";
            this.Size = new System.Drawing.Size(759, 612);
            this.Load += new System.EventHandler(this.SicboView_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SicboView_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SicboView_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SicboView_MouseDown);
            this.Resize += new System.EventHandler(this.SicboView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
