using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatEngine;
using System.IO;

namespace ChatClient
{
	/// <summary>
	/// AlbermControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class SelectAlbermControl : UserControl
	{
        public SelectAlbermControl()
		{
			this.InitializeComponent();
		}

        int nTotalCount = 0;
        public void selPictureList(UserDetailInfo user)
        {
            try
            {
                wrapPanel1.Children.Clear();

                nTotalCount = user.Faces.Count;

                if (user.Faces.Count == 0)
                {
                    this.titleName.Visibility = Visibility.Visible;
                    return;
                }

                for (int i = 0; i < user.Faces.Count; i++)
                {
                    //string video = user.Faces[i].Icon;
                    //char[] delimiterChars = { ':', '\\' };
                    //string[] words = video.Split(delimiterChars);
                    //int count = words.Length;

                    //if (words[count - 2].ToString() == "Face")
                    {
                        user.Faces[i].Icon = user.Faces[i].Icon.Replace("\\", "/");
                        WebDownloader.GetInstance().ctrlSelectAlbum = this;
                        progressUpdate.Visibility = Visibility.Visible;
                        WebDownloader.GetInstance().DownloadFile(user.Faces[i].Icon, TitleDownloadComplete, this);

                        //var faceGrid = new Grid();
                        //faceGrid.Width = 82;
                        //faceGrid.Height = 81;
                        //faceGrid.Margin = new Thickness(0);

                        //Image finalImage = new Image();
                        //finalImage.Cursor = Cursors.Hand;
                        //finalImage.Margin = new Thickness(2, 1, 2, 1);
                        //finalImage.DataContext = user.Faces[i].Icon;
                        //finalImage.Source = ImageDownloader.GetInstance().GetImage(user.Faces[i].Icon);
                        //finalImage.MouseLeftButtonUp += new MouseButtonEventHandler(finalImage_MouseLeftButtonUp);
                        //faceGrid.Children.Add(finalImage);
                        //wrapPanel1.Children.Add(faceGrid);
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        // 2013-12-29: GreenRose
        int nStep = 0;
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
                finalImage.Cursor = Cursors.Hand;
                finalImage.Margin = new Thickness(2, 1, 2, 1);
                finalImage.DataContext = filePath;
                finalImage.Source = bi;
                finalImage.MouseLeftButtonUp += new MouseButtonEventHandler(finalImage_MouseLeftButtonUp);
                faceGrid.Children.Add(finalImage);
                wrapPanel1.Children.Add(faceGrid);

                
                nStep++;

                if (nStep == nTotalCount)
                {
                    progressUpdate.Visibility = Visibility.Hidden;
                    WebDownloader.GetInstance().ctrlSelectAlbum = null;

                    nStep = 0;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        void finalImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var getIcon = sender as Image;
                string iconName = getIcon.DataContext.ToString();

                if (Main.previewImage == null)
                    Main.previewImage = new PreviewImage();
                Main.previewImage.ViewImage(iconName);
                Main.previewImage.Show();
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }
	}
}