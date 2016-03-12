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

using ControlExs;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for ChattingReciev.xaml
    /// </summary>
    public partial class ChattingReciev : Window
    {
        string reqId = "";
        public ChattingReciev()
        {
            this.InitializeComponent();
        }
        public void RequestName(UserInfo user)
        {
            label1.Content = user.Nickname + "对你申请聊天.";
            reqId = user.Id;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Text.RegularExpressions.Regex emailregex = new System.Text.RegularExpressions.Regex(@"[0-9]");
            Boolean ismatch = emailregex.IsMatch(textBox1.Text);
            if (!ismatch)
            {               
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "请把房间价格用数字填写.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
            }
            else
            {
                Login._AskChatInfo.Agree = 1;
                Login._AskChatInfo.Price = Convert.ToInt32(textBox1.Text);
                Login._AskChatInfo.AskContent = textBlock1.Text;
                Login._AskChatInfo.TargetId = reqId;

                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_EnterMeeting, Login._AskChatInfo);
                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_GameList, Login._UserInfo);
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBlock1.AcceptsReturn = true;
        }
    }
}
