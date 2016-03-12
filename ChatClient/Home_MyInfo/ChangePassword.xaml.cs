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
using System.Windows.Navigation;
using System.Windows.Shapes;

using ControlExs;
using ChatEngine;

namespace ChatClient.Home_MyInfo
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : UserControl
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.rePass.Password != Login._UserInfo.Password)
                {
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "原始密码输入有误.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    return;
                }
                else
                {
                    if (newPass.Password != newRePass.Password)
                    {
                        TempWindowForm tempWindowForm = new TempWindowForm();
                        QQMessageBox.Show(tempWindowForm, "2次输入密码有误请重新输入.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        Login._UserInfo.Password = newPass.Password;
                        Login._ClientEngine.Send(NotifyType.Request_UpdateUser, Login._UserInfo);
                    }
                }
            }
            catch (Exception)
            { }
        }
    }
}
