using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Microsoft.DirectX.AudioVideoPlayback;


namespace ChatServer
{
    public partial class PreviewVideo : Form
    {
        public string _VideoSource = null;

        public PreviewVideo()
        {
            InitializeComponent();
        }

        private void PreviewVideo_Load(object sender, EventArgs e)
        {
            if (_VideoSource == null)
                return;

            string url = string.Format("\\\\127.0.0.1\\{0}", _VideoSource);

            //Video v = Video.FromFile(url);
            //v.Owner = this;
            //v.Play();
        }
    }
}
