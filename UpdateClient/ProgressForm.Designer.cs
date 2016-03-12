namespace UpdateClient
{
    partial class ProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.progressUpdate = new System.Windows.Forms.ProgressBar();
            this.labelState = new System.Windows.Forms.Label();
            this.tmr_receivefile = new System.Windows.Forms.Timer(this.components);
            this.lbl_progress = new System.Windows.Forms.Label();
            this.tmr_sendfile = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // progressUpdate
            // 
            this.progressUpdate.Location = new System.Drawing.Point(16, 51);
            this.progressUpdate.Name = "progressUpdate";
            this.progressUpdate.Size = new System.Drawing.Size(462, 17);
            this.progressUpdate.TabIndex = 0;
            // 
            // labelState
            // 
            this.labelState.BackColor = System.Drawing.Color.Transparent;
            this.labelState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelState.Location = new System.Drawing.Point(16, 22);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(217, 26);
            this.labelState.TabIndex = 1;
            this.labelState.Text = "labelState";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tmr_receivefile
            // 
            this.tmr_receivefile.Enabled = true;
            this.tmr_receivefile.Interval = 1000;
            // 
            // lbl_progress
            // 
            this.lbl_progress.AutoSize = true;
            this.lbl_progress.BackColor = System.Drawing.Color.Transparent;
            this.lbl_progress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lbl_progress.Location = new System.Drawing.Point(354, 27);
            this.lbl_progress.Name = "lbl_progress";
            this.lbl_progress.Size = new System.Drawing.Size(52, 16);
            this.lbl_progress.TabIndex = 3;
            this.lbl_progress.Text = "进程：";
            // 
            // tmr_sendfile
            // 
            this.tmr_sendfile.Interval = 1000;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::UpdateClient.Properties.Resources.bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CanResize = false;
            this.ClientSize = new System.Drawing.Size(487, 82);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_progress);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.progressUpdate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgressForm_FormClosing);
            this.Load += new System.EventHandler(this.ProgressForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressUpdate;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Timer tmr_receivefile;
        private System.Windows.Forms.Label lbl_progress;
        private System.Windows.Forms.Timer tmr_sendfile;
    }
}

