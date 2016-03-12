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
    public partial class SelectUserInfoControl : UserControl
	{
        public SelectUserInfoControl()
		{
			this.InitializeComponent();
            for (int i = 1953; i <= DateTime.Now.Year - 15; i++)
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
            userYear.SelectedValue = users.Year;
            userMonth.SelectedValue = users.Month;
            userDay.SelectedValue = users.Day;
            signBox.Text = users.Sign;
            label2.Content = users.Cash;
            label4.Content = users.Point;
        }
       
	}
}