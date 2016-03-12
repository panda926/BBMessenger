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
    /// Interaction logic for ChattingReciev.xaml
    /// </summary>
    public partial class HunJon : Window
    {
        int hunMoney = 0;
        string m_strPreviousText;
        public HunJon()
        {
            this.InitializeComponent();
        }
        public void RequestHunjonMoney(int money)
        {
            hunMoney = money;
        }
        private void numberMark_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(((System.Windows.Controls.TextBox)sender).Text))
                {
                    numberMark.Opacity = 0.8;
                    m_strPreviousText = "";
                    this.button1.IsEnabled = false;
                }
                else
                {
                    int num = 0;
                    bool success = int.TryParse(((System.Windows.Controls.TextBox)sender).Text, out num);
                    if (success & num > 0 && num <= 5)
                    {
                        ((System.Windows.Controls.TextBox)sender).Text.Trim();
                        m_strPreviousText = ((System.Windows.Controls.TextBox)sender).Text;
                    }
                    else
                    {
                        ((System.Windows.Controls.TextBox)sender).Text = m_strPreviousText;
                        ((System.Windows.Controls.TextBox)sender).SelectionStart = ((System.Windows.Controls.TextBox)sender).Text.Length;
                    }

                    numberMark.Opacity = 1;
                    this.button1.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {

            //Login._UserInfo.co = Convert.ToInt32(numberMark.Text);

            //Login._ClientEngine.Send(ChatEngine.NotifyType.Reply_Give, Login._AskChatInfo);
            //this.Close();
            
        }
    }
}
