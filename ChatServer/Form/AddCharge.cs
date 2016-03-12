using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChatEngine;

namespace ChatServer
{
    public partial class AddCharge : Form
    {
        EnvInfo _envInfo;

        public AddCharge()
        {
            InitializeComponent();

            _envInfo = Database.GetInstance().GetEnviroment();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string userID = textUserId.Text.Trim();

            if (userID.Length == 0 )
            {
                MessageBox.Show("请输入会员帐号.");
                return;
            }

            UserInfo userInfo = Database.GetInstance().FindUser(userID);

            if (userInfo == null )
            {
                MessageBox.Show("不准确帐号.");
                return;
            }

            int cash = 0;

            try
            {
                cash = Convert.ToInt32(textMoney.Text);
            }
            catch
            {
            }

            if (cash == 0)
            {
                MessageBox.Show("请准确输入金额.");
                return;
            }

            if (cash < 0 && userInfo.Cash + cash * _envInfo.CashRate < 0)
            {
                string errStr = string.Format("持有缓存已满。当前缓存是{0}", userInfo.Cash);
                MessageBox.Show(errStr);
                return;
            }

            if (Cash.GetInstance().ProcessCharge(userID, cash) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            if( cash > 0 )
                MessageBox.Show("充电成功");
            else
                MessageBox.Show("兑换成功");

            this.DialogResult = DialogResult.OK;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            textUserId.Select();
            textUserId.Focus();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void textMoney_TextChanged(object sender, EventArgs e)
        {
            int money = 0;

            try
            {
                money = Convert.ToInt32(textMoney.Text);
            }
            catch { }

            labelCash.Text = string.Format("( 游戏货币: {0} )", money * _envInfo.CashRate);

            int serviceMoney = money * _envInfo.ChargeGivePercent / 100;
            int realMoney = money - serviceMoney;

            textServiceMoney.Text = serviceMoney.ToString();
            textRealMoney.Text = realMoney.ToString();
        }

    }
}
