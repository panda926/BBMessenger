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

// 2013-12-16: GreenRose

using ChatEngine;
using ControlExs;

namespace ChatClient
{
	/// <summary>
	/// BankAccountReg.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class BankAccountReg : UserControl
	{
        int nFlag = 0;
		public BankAccountReg()
		{
			this.InitializeComponent();
            InitAllValues();
		}

        private void InitAllValues()
        {
            this.txtBankAccountNumber.Text = string.Empty;
            this.txtBankAccountID.Text = string.Empty;
            this.txtPassword.Password = string.Empty;
            this.txtConfirmPassword.Password = string.Empty;

            this.txtBankAccountNumber.Focus();
        }

        public void InitAllControlValues(bool bUpdate)
        {
            nFlag = 0;

            if (bUpdate)
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "资料注册成功", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                //MessageBoxCommon.Show("조작이 성공하였습니다.", MessageBoxType.Ok);
            }

            if (!string.IsNullOrEmpty(Login._UserInfo.strAccountID))
                this.txtBankAccountID.Text = Login._UserInfo.strAccountID;

            if(!string.IsNullOrEmpty(Login._UserInfo.strAccountNumber))
                this.txtBankAccountNumber.Text = Login._UserInfo.strAccountNumber;

            if (!string.IsNullOrEmpty(Login._UserInfo.strAccountID) && !string.IsNullOrEmpty(Login._UserInfo.strAccountNumber))
            {
                nFlag = 1;

                this.txtBankAccountID.IsEnabled = false;
                this.txtBankAccountNumber.IsEnabled = false;

                this.lblPassword1.Content = "原始密码";
                this.lblPassword2.Content = "新密码";

                this.btnSaveOrChange.Content = "变更";
            }
        }

        private void btnSaveOrChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!CheckValidForValues())
                return;

            Login._UserInfo.strAccountID = this.txtBankAccountID.Text;
            Login._UserInfo.strAccountNumber = this.txtBankAccountNumber.Text;

            if (nFlag == 0)
                Login._UserInfo.strAccountPass = this.txtPassword.Password;
            else
                Login._UserInfo.strAccountPass = this.txtConfirmPassword.Password;

            Login._ClientEngine.Send(NotifyType.Request_UpdateUser, Login._UserInfo);

            //InitAllValues();
            
        }

        private bool CheckValidForValues()
        {
            try
            {
                Convert.ToInt32(this.txtBankAccountNumber.Text);
            }
            catch (System.Exception)
            {
                //MessageBoxCommon.Show("계좌번호는 수자가 되여야 합니다.", MessageBoxType.Ok);
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "银行帐号请填写正确数字", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                this.txtBankAccountNumber.Text = string.Empty;
                this.txtBankAccountNumber.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(this.txtBankAccountID.Text))
            {
                //MessageBoxCommon.Show("계좌아이디를 입력하세요.", MessageBoxType.Ok);
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "银行类型 请输入正确帐号", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                this.txtBankAccountID.Text = string.Empty;
                this.txtBankAccountID.Focus();
            }

            if (nFlag == 0)
            {
                if (!CheckPassword(this.txtPassword.Password, this.txtBankAccountID.Text))
                {
                    this.txtPassword.Password = string.Empty;
                    this.txtConfirmPassword.Password = string.Empty;
                    return false;
                }

                if (!this.txtPassword.Password.Equals(this.txtConfirmPassword.Password))
                {
                    //MessageBoxCommon.Show("비밀번호가 일치하지 않습니다.", MessageBoxType.Ok);
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "2次输入密码有误请重新输入", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    this.txtPassword.Password = string.Empty;
                    this.txtConfirmPassword.Password = string.Empty;
                    this.txtPassword.Focus();
                    return false;
                }
            }
            else
            {
                if (!this.txtPassword.Password.Equals(Login._UserInfo.strAccountPass))
                {
                    //MessageBoxCommon.Show("이전암호가 정확하지 않습니다.", MessageBoxType.Ok);
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "原始密码输入有误", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    this.txtPassword.Password = string.Empty;
                    this.txtConfirmPassword.Password = string.Empty;
                    this.txtPassword.Focus();

                    return false;
                }

                if (!CheckPassword(this.txtConfirmPassword.Password, this.txtBankAccountID.Text))
                {
                    this.txtPassword.Password = string.Empty;
                    this.txtConfirmPassword.Password = string.Empty;
                    return false;
                }
            }

            return true;
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

        private void txtBankAccountNumber_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        	try
        	{
                if (e.Key == Key.Enter)
                {
                    if (txtBankAccountNumber.Text != string.Empty)
                    {
                       txtBankAccountID.Focus();
                    }
                    else
                    {
                        txtBankAccountNumber.Focus();
                    }
                }   
        	}
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void txtBankAccountID_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txtBankAccountID.Text != string.Empty)
                    {
                        txtPassword.Focus();
                    }
                    else
                    {
                        txtBankAccountID.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void txtPassword_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txtPassword.Password != string.Empty)
                    {
                        txtConfirmPassword.Focus();
                    }
                    else
                    {
                        txtPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void txtConfirmPassword_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txtConfirmPassword.Password != string.Empty)
                    {
                        btnSaveOrChange.Focus();
                    }
                    else
                    {
                        txtConfirmPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }
	}
}