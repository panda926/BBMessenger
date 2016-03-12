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
    public partial class AddPoint : Form
    {
        public AddPoint()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string userID = textUserId.Text.Trim();

            if (userID.Length == 0)
            {
                MessageBox.Show("请输入会员帐号.");
                return;
            }

            UserInfo userInfo = Database.GetInstance().FindUser(userID);

            if (userInfo == null)
            {
                MessageBox.Show("不准确的帐号.");
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
                MessageBox.Show("请准确输入积分金额.");
                return;
            }

            if (Cash.GetInstance().ProcessPoint(userID, cash) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

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

    }
}
