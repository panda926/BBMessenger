using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ChatEngine;

using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for PictureView.xaml
    /// </summary>
    
    public partial class PictureView : Window
    {
        public PictureView()
        {
            InitializeComponent();
        }

        public void InitPictureView(ClassPictureDetailInfo pDetail)
        {
            stackPanel1.Children.Clear();
            WebDownloader.GetInstance().CancelDownload(this);

            for (int i = 0; i < pDetail.ClassType.Count; i++)
            {
                string filePath = "ImageFile/" + pDetail.ClassType[i].Class_File_Name;

               WebDownloader.GetInstance().DownloadFile(filePath, PictureDownloadComplete, this);
            }
        }

        public void PictureDownloadComplete(string filePath)
        {
            string strImagePath = filePath;
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri(strImagePath, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            System.Windows.Controls.Image _image = new Image();
            _image.Source = bi;

            stackPanel1.Children.Add(_image);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.pictureView = null;
        }

    }
}
