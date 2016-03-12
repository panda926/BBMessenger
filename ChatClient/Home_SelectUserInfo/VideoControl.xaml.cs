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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatEngine;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class VideoControl : UserControl
    {
        public VideoControl()
        {
            InitializeComponent();
        }

        //public void Initevideo(UserDetailInfo user)
        //{
        //    for (int i = 0; i < user.Faces.Count; i++)
        //    {
        //        if (videoLsit[i].Icon.Contains("Videos") == true)
        //        {
        //            try
        //            {
        //                char[] delimiterChars = { '\\', '.' };
        //                string[] imgwords = videoLsit[i].Icon.Split(delimiterChars);
        //                int imgcount = imgwords.Length;
        //                string imgName = imgwords[imgcount - 2].ToString();

        //                Image videoImg = new Image();
        //                videoImg.Width = 85;
        //                videoImg.Height = 63;
        //                videoImg.Cursor = System.Windows.Input.Cursors.Hand;
        //                videoImg.Margin = new Thickness(2, 2, 2, 2);
        //                BitmapImage videoBit = new BitmapImage();
        //                videoBit.BeginInit();
        //                videoBit.UriSource = new Uri(Login._ServerPath + "\\Videos\\" + imgName + ".png", UriKind.RelativeOrAbsolute);
        //                videoBit.EndInit();
        //                videoImg.Source = videoBit;
        //                videoImg.DataContext = videoLsit[i].Icon;
        //                videoImg.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(videoImg_MouseDown);
        //                videoImg.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(videoImg_MouseRightButtonDown);
        //                wrapPanel1.Children.Add(videoImg);
        //            }
        //            catch (Exception ex)
        //            {
        //                string strMsg = ex.Message;
        //            }

        //        }
        //    }
        //}
       
    }
}
