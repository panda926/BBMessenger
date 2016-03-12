using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChatEngine;

using System.IO;
using System.ComponentModel;
using System.Net;
using System.Windows.Media.Imaging;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for FilmUserControl.xaml
    /// </summary>
    public partial class FilmUserControl : UserControl
    {
        bool fpFlag = true;

        public FilmUserControl()
        {
            InitializeComponent();
        }

        public void InitFilmView(ClassTypeInfo classTypeInfo)
        {
            string strFilePath = string.Empty;

            if (classTypeInfo.Class_File_Type == 2)
            {
                strFilePath = "VideoFile/" + classTypeInfo.Class_Video_Title;
                image2.DataContext = classTypeInfo.Class_File_Name;
            }
            else
            {
                strFilePath = "ImageFile/" + classTypeInfo.Class_File_Name;
                image2.DataContext = classTypeInfo.Class_Img_Folder;
            }

            WebDownloader.GetInstance().DownloadFile(strFilePath, TitleDownloadComplete, this);

            dateLabel.Content = classTypeInfo.Class_File_Date.ToString("yyyy-MM-dd");

            char[] delimiterChars = { '.' };
            string[] words = classTypeInfo.Class_Video_Title.Split(delimiterChars);
            int count = words.Length;

            if (classTypeInfo.Class_File_Type == 2)
            {
                fpFlag = true;
                titleLabel.Content = words[0].ToString();
            }
            else
            {
                fpFlag = false;
                titleLabel.Content = classTypeInfo.Class_Img_Folder + "(" + classTypeInfo.Class_File_Count + ")";
            }

            for (int i = 0; i < Window1._ClassListInfo.Classes.Count; i++)
            {
                if (Window1._ClassListInfo.Classes[i].Class_Type_Id == classTypeInfo.Class_File_Area)
                {
                    positionLabel.Content = Window1._ClassListInfo.Classes[i].Class_Type_Name;
                    break;
                }
            }
            
        }

        public void TitleDownloadComplete(string filePath)
        {
            string strImagePath = filePath;
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri(strImagePath, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            image1.Source = bi;            
        }

        public void VideoDownloadComplete(string filePath)
        {
            Utils.FileExecute(filePath);
        }

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (fpFlag == true)
            {
                WebDownloader.GetInstance().DownloadFile("VideoFile/" + image2.DataContext.ToString(), VideoDownloadComplete, this);
                return;
            }
            else
            {
                for (int i = 0; i < Window1._ClassTypeListInfo.ClassType.Count; i++)
                {
                    string folderName = Window1._ClassTypeListInfo.ClassType[i].Class_Img_Folder;
                    if (folderName == image2.DataContext.ToString())
                    {
                        Window1._ClassTypeInfo = Window1._ClassTypeListInfo.ClassType[i];
                        Window1._ClientEngine.Send(NotifyType.Request_ClassPictureDetail, Window1._ClassTypeInfo);
                        break;
                    }
                }
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            gridFilm.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightSkyBlue);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            gridFilm.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
        }
    }
}
