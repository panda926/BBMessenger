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

// 2013-12-17: GrenRose
using ChatEngine;
using System.Net.Sockets;
using ControlExs;

namespace ChatClient
{
	/// <summary>
	/// BankAccountReg.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class Payment : UserControl
	{
        int nInputChangeMoney = 0;

        public Payment()
		{
			this.InitializeComponent();
            Login._ClientEngine.AttachHandler(NotifyOccured);
		}

        public void InitAllValues()
        {
            this.txtBankAccountID.Text = Login._UserInfo.strAccountID;
            this.txtBankAccountNumber.Text = Login._UserInfo.strAccountNumber;
        }

        private void btnPayment_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int nChangeMoney = Login._UserInfo.Cash / 100 * 90;
            nInputChangeMoney = 0;

            if (!this.txtBankAccountNumber.Text.Equals(Login._UserInfo.strAccountNumber))
            {
                //MessageBoxCommon.Show("계좌번호가 일치하지 않습니다.", MessageBoxType.Ok);
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "계좌번호가 일치하지 않습니다.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                this.txtBankAccountNumber.Text = string.Empty;
                this.txtBankAccountNumber.Focus();
                return;
            }

            if (!this.txtBankAccountID.Text.Equals(Login._UserInfo.strAccountID))
            {
                //MessageBoxCommon.Show("계좌아이디가 일치하지 않습니다.", MessageBoxType.Ok);
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "계좌아이디가 일치하지 않습니다.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                this.txtBankAccountID.Text = string.Empty;
                this.txtBankAccountID.Focus();
                return;
            }

            try
            {
                nInputChangeMoney = Convert.ToInt32(this.txtChangeMoney.Text);
                if (nInputChangeMoney > nChangeMoney)
                {
                    //MessageBoxCommon.Show("변한금액량이 너무 많습니다.", MessageBoxType.Ok);
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "结算金额有误请重新输入", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    this.txtChangeMoney.Text = string.Empty;
                    this.txtChangeMoney.Focus();
                    return;
                }

                if (!this.txtPassword.Password.Equals(Login._UserInfo.strAccountPass))
                {
                    //MessageBoxCommon.Show("비밀번호가 일치하지 않습니다.", MessageBoxType.Ok);
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "次输入密码有误请重新输入", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    this.txtPassword.Password = string.Empty;
                    this.txtPassword.Focus();
                    return;
                }
            }
            catch (System.Exception)
            {
                //MessageBoxCommon.Show("변환금액을 정확히 입력해주세요.", MessageBoxType.Ok);
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "结算金额有误请重新输入", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                return;
            }

            RequestPaymentToServer();
        }

        private void RequestPaymentToServer()
        {
            PaymentInfo paymentInfo = new PaymentInfo();

            paymentInfo.strID = Login._UserInfo.Id;
            paymentInfo.strAccountID = this.txtBankAccountID.Text;
            paymentInfo.strAccountNumber = this.txtBankAccountNumber.Text;
            paymentInfo.nPaymentMoney = nInputChangeMoney;

            Login._ClientEngine.Send(NotifyType.Request_PaymentInfo, paymentInfo);
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_PaymentInfo:
                    {
                        //MessageBoxCommon.Show("조작이 성공하였습니다.", MessageBoxType.Ok);
                        TempWindowForm tempWindowForm = new TempWindowForm();
                        QQMessageBox.Show(tempWindowForm, "结算成功", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                        this.txtChangeMoney.Text = "";
                        this.txtPassword.Password = "";
                    }
                    break;
            }
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
                        txtChangeMoney.Focus();
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

        private void txtChangeMoney_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txtChangeMoney.Text != string.Empty)
                    {
                        txtPassword.Focus();
                    }
                    else
                    {
                        txtChangeMoney.Focus();
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
                        btnPayment.Focus();
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
	}
}