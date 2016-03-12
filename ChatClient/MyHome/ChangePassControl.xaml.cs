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
	/// ChangePassControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ChangePassControl : UserControl
	{
		public ChangePassControl()
		{
			this.InitializeComponent();
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (prePass.Password == Main._UserInfo.Password)
            {
                if (newPass.Password == veriPass.Password)
                {
                    Main._UserInfo.Password = newPass.Password;
                    Main._ClientEngine.Send(NotifyType.Request_UpdateUser, Main._UserInfo);
                }
                else
                    MessageBox.Show("암호를 다시 확인 하세요.");
            }
            else
            {
                MessageBox.Show("기존 암호와 일치하지 않습니다.");
            }
        }
	}
}