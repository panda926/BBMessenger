namespace HorseClient
{
    partial class HorseView
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
            this.RenderWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // RenderWorker
            // 
            this.RenderWorker.WorkerSupportsCancellation = true;
            this.RenderWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RenderWorker_DoWork);
            // 
            // HorseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.DoubleBuffered = true;
            this.Name = "HorseView";
            this.Size = new System.Drawing.Size(350, 251);
            this.Load += new System.EventHandler(this.HorseView_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HorseView_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HorseView_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HorseView_MouseDown);
            this.Resize += new System.EventHandler(this.HorseView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker RenderWorker;
    }
}

