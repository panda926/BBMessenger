using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
//using AForge.Video;
//using AForge.Video.DirectShow;
//using AForge.Video.FFMPEG;
using ChatEngine;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel;

namespace ChatClient
{
	/// <summary>
	/// AlbermControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class PictureControl : UserControl
	{
        public MyHome myHome = null;

        public PictureControl()
		{
			this.InitializeComponent();
		}

        public void InitVideoList(List<IconInfo> videoLsit)
        {
            if (videoLsit != null)
            {
                for (int i = 0; i < videoLsit.Count; i++)
                {
                    if (videoLsit[i].Icon.Contains(".mp4") == true)
                    {
                        try
                        {
                            //char[] delimiterChars = { '\\', '.' };
                            //string[] imgwords = videoLsit[i].Icon.Split(delimiterChars);
                            //int imgcount = imgwords.Length;
                            //string imgName = imgwords[imgcount - 2].ToString();

                            //Image videoImg = new Image();
                            //videoImg.Width = 85;
                            //videoImg.Height = 63;
                            //videoImg.Cursor = System.Windows.Input.Cursors.Hand;
                            //videoImg.Margin = new Thickness(2, 2, 2, 2);
                            //string fileName = "\\Videos\\" + imgName + ".gif";
                            //videoImg.Source = ImageDownloader.GetInstance().GetImage(fileName);
                            //videoImg.DataContext = videoLsit[i].Icon;
                            //videoImg.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(videoImg_MouseDown);
                            //videoImg.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(videoImg_MouseRightButtonDown);
                            //wrapPanel1.Children.Add(videoImg);

                            // 2013-12-30: GreenRose
                            videoLsit[i].Icon = videoLsit[i].Icon.Replace("\\", "/");
                            videoLsit[i].Icon = videoLsit[i].Icon.Replace(".mp4", ".jpg");
                            WebDownloader.GetInstance().DownloadFile(videoLsit[i].Icon, TitleDownloadComplete, this);
                            videoLsit[i].Icon = videoLsit[i].Icon.Replace(".jpg", ".mp4");
                        }
                        catch (Exception ex)
                        {
                            string strError = ex.ToString();
                            ErrorCollection.GetInstance().SetErrorInfo(strError);
                        }

                    }
                }
            }
        }

        // 2013-12-30: GreenRose
        public void TitleDownloadComplete(string filePath)
        {
            try
            {
                string strImagePath = filePath;
                BitmapImage bi = new BitmapImage();

                bi.BeginInit();
                bi.UriSource = new Uri(strImagePath, UriKind.RelativeOrAbsolute);
                bi.EndInit();

                var faceGrid = new Grid();
                faceGrid.Width = 82;
                faceGrid.Height = 81;
                faceGrid.Margin = new Thickness(0);

                Image finalImage = new Image();
                finalImage.Cursor = System.Windows.Input.Cursors.Hand;
                finalImage.Margin = new Thickness(2, 1, 2, 1);
                finalImage.DataContext = filePath;
                finalImage.Source = bi;
                finalImage.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(videoImg_MouseDown);
                faceGrid.Children.Add(finalImage);
                wrapPanel1.Children.Add(faceGrid);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        void videoImg_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image selectImg = sender as Image;
            ContextMenu contextMenu = new ContextMenu();
            MenuItem item1 = new MenuItem();
            item1.Click += new RoutedEventHandler(item1_Click);
            item1.Header = "删除图像";
            item1.DataContext = selectImg.DataContext.ToString();
            contextMenu.Items.Add(item1);
            contextMenu.IsOpen = true;
        }

        void item1_Click(object sender, RoutedEventArgs e)
        {
            var selectMenu = sender as MenuItem;
            string imgIcon = selectMenu.DataContext.ToString();
            
        }

        public void VideoDownloadComplete(string filePath)
        {
            Utils.FileExecute(filePath);

        }

        void videoImg_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var video = sender as Image;
            string videoName = video.DataContext.ToString();

            //WebDownloader.GetInstance().DownloadFile(videoName, VideoDownloadComplete, this);
            int nIndex = videoName.LastIndexOf("\\");
            videoName = videoName.Substring(nIndex + 1);
            videoName = videoName.Replace(".jpg", ".mp4");
            //SelectVideoView selectVideoView = new SelectVideoView();
            //selectVideoView._strMediaFile = Login._ServerPath + "Images/Face/" + videoName;
            //selectVideoView.Show();

            WebDownloader.GetInstance().DownloadFile("Images/Face/" + videoName, VideoDownloadComplete, this);


        } 
        

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (Main.videoCreat == null)
            {
                Main.videoCreat = new VideoCreat();
                Main.videoCreat.myHome = myHome;
                Main.videoCreat.Show();
            }
        }
       
	}
}