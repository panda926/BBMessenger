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
    
    public partial class PictureView : BaseWindow
    {
        string m_strImageFolderName;
        public PictureView()
        {
            InitializeComponent();
        }

        public void InitPictureView(ClassPictureDetailInfo pDetail)
        {
            m_strImageFolderName = pDetail.ClassType[0].Class_Img_Folder;
            stackPanel1.Children.Clear();

            for (int i = 0; i < pDetail.ClassType.Count; i++)
            {
                string filePath = string.Empty;
                if (pDetail.ClassType[i].Class_File_Type == 0)
                {
                    //filePath = "ImageFile/" + pDetail.ClassType[i].Class_File_Name;
                    filePath = pDetail.ClassType[i].Class_File_Name;
                    WebDownloader.GetInstance().DownloadFile(filePath, PictureDownloadComplete, this);
                }
                else if (pDetail.ClassType[i].Class_File_Type == 2)
                {
                    //filePath = "ImageFile/" + pDetail.ClassType[i].Class_Video_Title;
                    string[] strVideoTitles = new string[] { "" };
                    string[] stringSeparators = new string[] { "||" };
                    strVideoTitles = pDetail.ClassType[i].Class_Video_Title.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < strVideoTitles.Length; j++)
                    {
                        //filePath = "VideoFile/" + strVideoTitles[j];
                        filePath = strVideoTitles[j];
                        WebDownloader.GetInstance().DownloadFile(filePath, PictureDownloadComplete, this);
                    }
                }

               //WebDownloader.GetInstance().DownloadFile(filePath, PictureDownloadComplete, this);
            }
        }

        public void PictureDownloadComplete(string filePath)
        {
            try
            {
                //복호화한다. 2014-2-25 by pakcj 
                //string decPath = WebDownloader.GetInstance().DecryptFile(filePath);
                string decPath = filePath;

                string strImagePath = decPath;
                BitmapImage bi = new BitmapImage();

                bi.BeginInit();
                bi.UriSource = new Uri(strImagePath, UriKind.RelativeOrAbsolute);
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();

                System.Windows.Controls.Image _image = new Image();
                _image.Source = bi;

                stackPanel1.Children.Add(_image);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.pictureView = null;

            if (Main.m_listPreviewingImgFolder.Contains(m_strImageFolderName))
            {
                Main.m_listPreviewingImgFolder.Remove(m_strImageFolderName);
            }
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

    }
}
