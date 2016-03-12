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


namespace ChatClient
{
	/// <summary>
	/// MyInfoControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MyInfoControl : UserControl
	{
		public MyInfoControl()
		{
			this.InitializeComponent();

            for (int i = DateTime.Now.Year - 100; i <= DateTime.Now.Year - 15; i++)
            {
                userYear.Items.Add(i);
            }
            for (int j = 1; j <= 12; j++)
            {
                userMonth.Items.Add(j);
            }
            for (int k = 1; k <= 31; k++)
            {
                userDay.Items.Add(k);
            }
		}

        public void InitMyInfo(UserInfo users)
        {
            userId.Content = users.Id;
            userNicname.Text = users.Nickname;
            userAddress.Text = users.Address;
            rePass.Password = users.Password;
            userYear.SelectedValue = users.Year;
            userMonth.SelectedValue = users.Month;
            userDay.SelectedValue = users.Day;
            signBox.Text = users.Sign;

            chatprecent.Content = users.Cash;
            gamepercent.Content = users.Point;

            buttonSave.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (userNicname.Text == "")
                MessageBoxCommon.Show("请输入您的称号.", MessageBoxType.Ok);
            else
            {
                Window1._UserInfo.Id = userId.Content.ToString();
                Window1._UserInfo.Nickname = userNicname.Text;
                Window1._UserInfo.Address = userAddress.Text;
                Window1._UserInfo.Sign = signBox.Text;

                try
                {
                    Window1._UserInfo.Year = Convert.ToInt32(userYear.SelectedItem.ToString());
                    Window1._UserInfo.Month = Convert.ToInt32(userMonth.SelectedItem.ToString());
                    Window1._UserInfo.Day = Convert.ToInt32(userDay.SelectedItem.ToString());
                }
                catch { }

                Window1._UserInfo.Password = rePass.Password;
                Window1._ClientEngine.Send(NotifyType.Request_UpdateUser, Window1._UserInfo);
                
            }
        }

        private void userNicname_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
        }

        private void userYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
        }

        private void userMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
        }

        private void userDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
        }

        private void userAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
        }

        private void signBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
        }

        private void rePass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            buttonSave.IsEnabled = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Window1._ClientEngine.Send(ChatEngine.NotifyType.Request_ChargeSiteUrl, Window1._UserInfo);
        }

        //private void hunjon_Click(object sender, RoutedEventArgs e)
        //{
        //    HunJon hunJon = new HunJon();
        //    hunJon.ShowDialog();
           
        //}
	}
}