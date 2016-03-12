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
using System.Windows.Media.Animation;
using ChatEngine;
using System.Net;
using System.Net.Sockets;

namespace ChatClient
{
	/// <summary>
	/// ChattingCall.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ChattingCall : Window
	{

        string _callID = null;

		public ChattingCall()
		{
			this.InitializeComponent();
            this.Close();
			// 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.
            
		}

        public void ChattingCallName( UserInfo callName, string callId)
        {
            _callID = callId;
            chattingContent.Text = "您要对 " + callName.Nickname + "申请聊天吗?";
        }

        
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Login._AskChatInfo.TargetId = _callID;
            Login._ClientEngine.Send(NotifyType.Request_EnterMeeting, Login._AskChatInfo);
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       
       
	}
}