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
using System.Windows.Shapes;
using ChatEngine;
using System.Net;
using System.Net.Sockets;

namespace ChatClient
{
	/// <summary>
	/// Letter.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class Letter : Window
	{
        string sendId = null;
		public Letter()
		{
			this.InitializeComponent();
			// 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.
		}
       
        //public void LetterSend(UserInfo user, string content)
        //{
        //    sendId = user.Id;
        //    userName.Content = user.Nickname;
        //    contents.Text = content;
        //    if (user.Kind != (int)UserKind.ServiceWoman)
        //    {
        //        label.Content = "现  金:";
        //        cash_point.Content = user.Cash.ToString();
        //    }
        //    else
        //    {
        //        label.Content = "评  价:";
        //        if (user.Visitors == 0)
        //            cash_point.Content = "0";
        //        else
        //            cash_point.Content = ((user.Evaluation / (double)user.Visitors)*100).ToString("0.0")  + "%";
        //    }

        //    WebDownloader.GetInstance().DownloadFile(user.Icon, IconDownloadComplete, this);
        //}

        //public void IconDownloadComplete(string filePath)
        //{
        //    BitmapImage bi = new BitmapImage();
        //    bi.BeginInit();
        //    bi.UriSource = new Uri(filePath);
        //    bi.EndInit();

        //    ImageBrush faceImage = new ImageBrush();
        //    faceImage.Stretch = Stretch.Fill;
        //    faceImage.ImageSource = bi;
        //    myPicture.Fill = faceImage;
        //}
       
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Login._StringInfo.UserId = sendId;
            Login._StringInfo.String = contents.Text;
            Login._ClientEngine.Send(NotifyType.Request_Message, Login._StringInfo);
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}