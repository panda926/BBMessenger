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

using ControlExs;

namespace ChatClient
{
	/// <summary>
	/// MyInfoControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MyInfoControl : UserControl
	{
        public bool _bChanged = false;
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
            //userAddress.Text = users.Address;
            //rePass.Password = users.Password;
            userYear.SelectedValue = users.Year;
            userMonth.SelectedValue = users.Month;
            userDay.SelectedValue = users.Day;
            signBox.Text = users.Sign;
			
			userEmailAddress.Text = users.Address;

            chatprecent.Content = users.Cash;
            gamepercent.Content = users.Point;

            if (users.Kind == (int)UserKind.ServiceWoman)
            {
                this.chkAllGuest.Visibility = Visibility.Visible;
                this.txtAllGuest.Visibility = Visibility.Visible;

                if (users.nVIP == 2)
                    this.chkAllGuest.IsEnabled = false;

                if (users.nVIP == 0)
                {
                    this.chkAllGuest.IsChecked = false;
                }
                else
                    this.chkAllGuest.IsChecked = true;
            }

            //buttonSave.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _bChanged = true;
            if (userNicname.Text == "")
            {
                //MessageBoxCommon.Show("请输入您的称号.", MessageBoxType.Ok);
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "请输入您的称号.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
            }
            else
            {
                Login._UserInfo.Id = userId.Content.ToString();
                Login._UserInfo.Nickname = userNicname.Text;
                Login._UserInfo.Address = userEmailAddress.Text;
                Login._UserInfo.Sign = signBox.Text;

                if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                {
                    if (this.chkAllGuest.IsChecked == true)
                        Login._UserInfo.nVIP = 1;
                    else
                        Login._UserInfo.nVIP = 0;
                }

                try
                {
                    Login._UserInfo.Year = Convert.ToInt32(userYear.SelectedItem.ToString());
                    Login._UserInfo.Month = Convert.ToInt32(userMonth.SelectedItem.ToString());
                    Login._UserInfo.Day = Convert.ToInt32(userDay.SelectedItem.ToString());
                }
                catch (Exception)
                {                    
                }

                //if(!rePass.Password.Equals(Login._UserInfo.Password))
                //{
                //    TempWindowForm tempWindowForm = new TempWindowForm();
                //    QQMessageBox.Show(tempWindowForm, "原始密码输入有误.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                //    return;
                //}

                //if (!CheckPassword(newPass.Password, userId.Content.ToString()))
                //{
                //    return;
                //}

                //Login._UserInfo.Password = newPass.Password;
                Login._ClientEngine.Send(NotifyType.Request_UpdateUser, Login._UserInfo);

            }
        }

        // 2013-12-12: GreenRose
        // 패스워드 유효성검사하는 함수
        private bool CheckPassword(string strPassword, string strUserID)
        {
            try
            {
                if (strPassword.Length < 6)
                {
                    //MessageBoxCommon.Show("비밀번호는 문자, 수자 조합으로 6~16자리로 입력해주세요.", MessageBoxType.Ok);
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "密码由字母和数字组合6-16位 请重新输入", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    return false;
                }

                System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"[a-zA-Z]");
                System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@"[0-9]");
                if (!regex1.IsMatch(strPassword) || !regex2.IsMatch(strPassword))
                {
                    //MessageBoxCommon.Show("비밀번호는 문자, 수자 조합으로 6~16자리로 입력해주세요.", MessageBoxType.Ok);
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "密码由字母和数字组合6-16位 请重新输入", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    return false;
                }

                if (strPassword.IndexOf(strUserID) > -1)
                {
                    //MessageBoxCommon.Show("비밀번호에 아이디를 사용할수 없습니다.", MessageBoxType.Ok);
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "密码不能和ID重复密码过于简单", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    return false;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);

                return false;
            }

            return true;
        }

        private void userNicname_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
            _bChanged = true;
        }

        private void userYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
            _bChanged = true;
        }

        private void userMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
            _bChanged = true;
        }

        private void userDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
            _bChanged = true;
        }

        private void userAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
            _bChanged = true;
        }

        private void signBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
            _bChanged = true;
        }

        //private void rePass_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    buttonSave.IsEnabled = true;
        //    _bChanged = true;
        //}

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Login._ClientEngine.Send(ChatEngine.NotifyType.Request_ChargeSiteUrl, Login._UserInfo);
        }

        //private void hunjon_Click(object sender, RoutedEventArgs e)
        //{
        //    HunJon hunJon = new HunJon();
        //    hunJon.ShowDialog();
           
        //}
	}
}