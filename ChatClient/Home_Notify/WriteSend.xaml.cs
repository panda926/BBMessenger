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

using ControlExs;

namespace ChatClient
{
	/// <summary>
	/// WriteSend.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class WriteSend : BaseWindow
	{
        public bool btFlag = true;
		public WriteSend()
		{
			this.InitializeComponent();
			
			// 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (btFlag == true)
            {
                if (title.Text != string.Empty)
                {
                    string sendNameStr = sendName.Text;
                    char[] delimiterChars = { ',' };
                    string[] words = sendNameStr.Split(delimiterChars);
                    int count = words.Length;
                    if (words[0].ToString() == "管理人")
                    {
                        Login._BoardInfo.Title = title.Text;
                        Login._BoardInfo.Content = contents.Text;
                        Login._BoardInfo.UserId = Login._UserInfo.Id;
                        Login._BoardInfo.UserKind = Login._UserInfo.Kind;
                        Login._BoardInfo.SendId = "admin";
                        Login._ClientEngine.Send(NotifyType.Request_SendLetter, Login._BoardInfo);
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (words[i].ToString() != "")
                            {
                                Login._BoardInfo.Title = title.Text;
                                Login._BoardInfo.Content = contents.Text;
                                Login._BoardInfo.UserId = Login._UserInfo.Id;
                                Login._BoardInfo.UserKind = Login._UserInfo.Kind;
                                Login._BoardInfo.SendId = words[i].ToString();
                                Login._ClientEngine.Send(NotifyType.Request_SendLetter, Login._BoardInfo);
                            }
                        }
                    }
                }
                else
                {                    
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "请输入标题", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                }
            }
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.writeSend = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (btFlag == true)
                stateBt.Content = "发送";
            else
            {
                stateBt.Content = "关闭";
                sendName.IsEnabled = false;
                checkBt.IsEnabled = false;
            }

            title.KeyDown += title_KeyDown;
            contents.AcceptsReturn = true;
        }

        public void viewLetter()
        {

        }

        private void title_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter )
            {
                if (title.Text != string.Empty)
                {
                    contents.Focus();
                }
                
            }
        }

        private void checkBt_Click(object sender, RoutedEventArgs e)
        {
            CheckUserList checkUserList = new CheckUserList();
            checkUserList.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

	}
}