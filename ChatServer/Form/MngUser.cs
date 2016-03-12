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
    public partial class MngUser : Form
    {
        public MngUser()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            //this.Text = UserInfo.KindList[(int)_Kind] + " 목록";

            comboKind.Items.Add("会员帐号");
            comboKind.Items.Add("名字");
            comboKind.Items.Add("推荐人");

            comboKind.SelectedIndex = 0;

            RefreshUserList();
        }

        public void RefreshUserList()
        {
            string queryStr = string.Format("select * from tblUser where Kind = {0} and Auto=0 ", (int)UserKind.Buyer );

            string contentStr = textContent.Text.Trim();

            if (contentStr.Length > 0)
            {
                string field = "";

                switch (comboKind.SelectedIndex)
                {
                    case 0:
                        field += string.Format("and Id like '%{0}%'", contentStr);
                        break;

                    case 1:
                        field += string.Format("and  Nickname like '%{0}%'", contentStr);
                        break;

                    case 2:
                        field += string.Format("and  Recommender like '%{0}%'", contentStr);
                        break;
                }

                queryStr += field;
            }

            List<UserInfo> userList = Database.GetInstance().GetUserList(queryStr);

            if (userList == null)
            {
                MessageBox.Show("不能在资料库获取会员目录.");
                return;
            }

            UserView.Rows.Clear();

            for (int i = 0; i < userList.Count; i++)
            {
                UserInfo userInfo = userList[i];

                string checkSum = string.Empty;

                if (userInfo.Cash != userInfo.ChargeSum + userInfo.DischargeSum + userInfo.ChatSum + userInfo.GameSum + userInfo.SendSum + userInfo.ReceiveSum)
                    checkSum = "有";

                //List<UserInfo> partners = Database.GetInstance().GetPartners(userInfo);

                UserView.Rows.Add(
                    userInfo.Id,
                    userInfo.Nickname,
                    string.Format("{0} / {1}", userInfo.Cash, userInfo.Point ),
                    userInfo.Recommender,
                    userInfo.ChargeSum,
                    userInfo.DischargeSum,
                    userInfo.ChatSum,
                    userInfo.GameSum,
                    userInfo.SendSum + userInfo.ReceiveSum,
                    checkSum,
                    userInfo.LoginTime
                    );

                UserView.Rows[UserView.Rows.Count - 1].Tag = userInfo;

                if (string.IsNullOrEmpty(checkSum) == false)
                    UserView.Rows[UserView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;

                if (userInfo.ChargeSum + userInfo.DischargeSum < 0 )
                    UserView.Rows[UserView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
            }

            labelSummary.Text = string.Format("总数: {0}", userList.Count);
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser._Kind = (int)UserKind.Buyer;

            if (addUser.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");
                RefreshUserList();
            }
        }

        private void buttonEditUser_Click(object sender, EventArgs e)
        {
            if (UserView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            string userId = UserView.SelectedRows[0].Cells["Id"].Value.ToString();

            AddUser editUser = new AddUser();

            editUser._IsEditMode = true;
            editUser._UserId = userId;

            if (editUser.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");
                RefreshUserList();
            }
        }

        private void buttonDelUser_Click(object sender, EventArgs e)
        {
            if (UserView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            string userId = UserView.SelectedRows[0].Cells["Id"].Value.ToString();

            string question = string.Format("真要删除 {0} 吗?", userId);

            if (MessageBox.Show(question, "删除提问", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (Database.GetInstance().DelUser(userId) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            string resultStr = string.Format("{0} 已被删除.", userId);
            MessageBox.Show(resultStr);

            RefreshUserList();

        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            RefreshUserList();
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if (UserView.SelectedRows.Count == 0)
            {
                MessageBox.Show("내역을 볼 유저를 먼저 선택하십시오.");
                return;
            }

            string userId = UserView.SelectedRows[0].Cells["Id"].Value.ToString();

            MngCash mngCash = new MngCash();
            mngCash._UserId = userId;

            mngCash.ShowDialog();

        }
    }
}
