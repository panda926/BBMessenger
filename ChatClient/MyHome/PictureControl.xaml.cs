using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
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
                    if (videoLsit[i].Icon.Contains("Videos") == true)
                    {
                        try
                        {
                            char[] delimiterChars = { '\\', '.' };
                            string[] imgwords = videoLsit[i].Icon.Split(delimiterChars);
                            int imgcount = imgwords.Length;
                            string imgName = imgwords[imgcount - 2].ToString();

                            Image videoImg = new Image();
                            videoImg.Width = 85;
                            videoImg.Height = 63;
                            videoImg.Cursor = System.Windows.Input.Cursors.Hand;
                            videoImg.Margin = new Thickness(2, 2, 2, 2);
                            string fileName = "\\Videos\\" + imgName + ".gif";
                            videoImg.Source = ImageDownloader.GetInstance().GetImage(fileName);
                            videoImg.DataContext = videoLsit[i].Icon;
                            videoImg.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(videoImg_MouseDown);
                            videoImg.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(videoImg_MouseRightButtonDown);
                            wrapPanel1.Children.Add(videoImg);
                        }
                        catch (Exception ex)
                        {
                            string strMsg = ex.Message;
                        }

                    }
                }

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
            var videoName = video.DataContext.ToString();

            WebDownloader.GetInstance().DownloadFile(videoName, VideoDownloadComplete, this);
        } 
        

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (Main.videoCreat == null)
            {
                Main.videoCreat = new VideoCreat();
                Main.videoCreat.Show();
            }
        }
       
	}
}