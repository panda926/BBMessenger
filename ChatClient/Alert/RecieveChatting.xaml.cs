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

using ControlExs;

namespace ChatClient
{
	/// <summary>
	/// RecieveChatting.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class RecieveChatting : Window
	{
        string reciveId = "";
		public RecieveChatting()
		{
			this.InitializeComponent();
			
			// 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.
		}
        public void ChattingInfo(string name, string icon, string price, string content, string id, double mark)
        {
            reciveId = id;
            //녀성사진
            ImageBrush faceImage = new ImageBrush();
            faceImage.Stretch = Stretch.Fill;
            faceImage.ImageSource = ImageDownloader.GetInstance().GetImage(icon);
            myPicture.Fill = faceImage;

            nickName.Content = name;
            pricestate.Content = price;
            marklabel.Content = mark.ToString() + "%";
            textBlock1.Text = content;

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Login._AskChatInfo.TargetId = reciveId;
            Login._AskChatInfo.Agree = 1;
            if (Login._UserInfo.Cash > Login._AskChatInfo.Price)
            {
                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_GameList, Login._UserInfo);
                Login._ClientEngine.Send(ChatEngine.NotifyType.Reply_EnterMeeting, Login._AskChatInfo);

            }
            else
            {               
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "您的余额不足不能进行聊天.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
            }
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //Login._AskChatInfo.Agree = 0;
            //Login._ClientEngine.Send(ChatEngine.NotifyType.Request_EnterMeeting, Login._AskChatInfo);
            this.Close();
        }

       

        
	}
}