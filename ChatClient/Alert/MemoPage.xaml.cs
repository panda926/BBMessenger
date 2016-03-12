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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MemoPage.xaml
    /// </summary>
    public partial class MemoPage : Window
    {
        public string sendId = null;
        List<string> userName = new List<string>();
        List<string> stringMsg = new List<string>();
        bool userFlag = true;
        IniFileEdit _IniFileEdit = new IniFileEdit(Login._UserPath);

        public MemoPage()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                grid2.Visibility = Visibility.Visible;
            else
                grid1.Visibility = Visibility.Visible;
        }

        public void InsertString(StringInfo stringMemo)
        {
            TextBlock recieveId = new TextBlock();
            recieveId.Margin = new Thickness(0);

            TextBlock recieveContent = new TextBlock();
            recieveContent.Margin = new Thickness(0);

            stringMsg.Add(stringMemo.String);

            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {

                if (userName.Count == 0)
                {
                    UsercomboBox.Items.Add(stringMemo.UserId);
                    userName.Add(stringMemo.UserId);
                }
                else
                {

                    for (int i = 0; i < userName.Count; i++)
                    {
                        if (userName[i] == stringMemo.UserId)
                        {
                            userFlag = false;
                            break;
                        }
                    }
                    if (userFlag == true)
                    {
                        UsercomboBox.Items.Add(stringMemo.UserId);
                        userName.Add(stringMemo.UserId);
                    }

                    UsercomboBox.SelectedValue = stringMemo.UserId;
                }

                recieveId.Text = "[" + stringMemo.UserId + "]" + " " + DateTime.Now.ToString();
                recieveContent.Text = stringMemo.String;
                stackPanel2.Children.Add(recieveId);
                stackPanel2.Children.Add(recieveContent);
                scrollViewer2.ScrollToEnd();
            }
            else
            {
                recieveId.Text = "[" + stringMemo.UserId + "]" + " " + DateTime.Now.ToString();
                recieveContent.Text = stringMemo.String;
                stackPanel1.Children.Add(recieveId);
                stackPanel1.Children.Add(recieveContent);
                scrollViewer1.ScrollToEnd();
            }
            sendId = stringMemo.UserId;
        }

        private void messageEditBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (messageEditBox.Text != string.Empty)
                    {
                        TextBlock userIdtxt = new TextBlock();
                        userIdtxt.Margin = new Thickness(0);
                        userIdtxt.Text = "[" + Login._UserInfo.Id + "]" + " " + DateTime.Now.ToString();
                        userIdtxt.Foreground = new SolidColorBrush(Colors.Red);

                        TextBlock contenttxt = new TextBlock();
                        contenttxt.Margin = new Thickness(0);
                        contenttxt.Text = messageEditBox.Text;
                        contenttxt.Background = new SolidColorBrush(Colors.LightSkyBlue);


                        if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                        {
                            stackPanel2.Children.Add(userIdtxt);
                            stackPanel2.Children.Add(contenttxt);
                        }
                        else
                        {
                            stackPanel1.Children.Add(userIdtxt);
                            stackPanel1.Children.Add(contenttxt);
                        }


                        Login._StringInfo.UserId = sendId;
                        Login._StringInfo.String = messageEditBox.Text;
                        Login._ClientEngine.Send(NotifyType.Request_Message, Login._StringInfo);

                        messageEditBox.Clear();
                    }
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && e.Key == Key.Insert)
                {
                    e.Handled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string endMemo = _IniFileEdit.GetIniValue("StringMessageBox", "EndMemo");
            
            Login._StringInfo.UserId = sendId;
            Login._StringInfo.String = endMemo;
            Login._ClientEngine.Send(NotifyType.Request_Message, Login._StringInfo);
            Main.mPage = null;
            
           
        }

        private void UsercomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sendId = UsercomboBox.SelectedItem.ToString();
        }

        

    }
}
