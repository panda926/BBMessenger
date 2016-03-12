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
    public partial class MngRecommender : Form
    {
        public MngRecommender()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            comboKind.Items.Add("会员帐号");
            comboKind.Items.Add("名字");
            comboKind.SelectedIndex = 0;

            RefreshUserList();
        }

        public void RefreshUserList()
        {
            string queryStr = string.Format("select * from tblUser where Kind={0} ", (int)UserKind.Recommender);

            string contentStr = textContent.Text.Trim();

            if (contentStr.Length > 0)
            {
                switch (comboKind.SelectedIndex)
                {
                    case 0:
                        queryStr += string.Format(" and Id like '%{0}%'", contentStr);
                        break;

                    case 1:
                        queryStr += string.Format(" and Nickname like '%{0}%'", contentStr);
                        break;
                }
            }

            List<UserInfo> userList = Database.GetInstance().GetUserList(queryStr);

            if (userList == null)
            {
                MessageBox.Show("不能在资料库获取广告员资料.");
                return;
            }

            UserView.Rows.Clear();

            for (int i = 0; i < userList.Count; i++)
            {
                UserInfo userInfo = userList[i];

                List<UserInfo> partners = Database.GetInstance().GetPartners(userInfo);

                UserView.Rows.Add(
                    userInfo.Id,
                    userInfo.Nickname,
                    string.Format("{0} / {1}", userInfo.Cash, userInfo.Point),
                    string.Format("{0}", partners.Count ),
                    userInfo.ChargeSum + userInfo.DischargeSum,
                    userInfo.GamePercent,
                    userInfo.GameSum,
                    userInfo.SendSum + userInfo.ReceiveSum,
                    userInfo.LoginTime
                    );
            }

            labelSummary.Text = string.Format("总数: {0}", userList.Count);
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser._Kind = (int)UserKind.Recommender;

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
            editUser._Kind = (int)UserKind.Recommender;

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
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            string userId = UserView.SelectedRows[0].Cells["Id"].Value.ToString();

            MngCash mngCash = new MngCash();
            mngCash._UserId = userId;

            mngCash.ShowDialog();

        }

        private void buttonPrevilege_Click(object sender, EventArgs e)
        {
            if (UserView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            string userId = UserView.SelectedRows[0].Cells["Id"].Value.ToString();

            MngPercent mngPercent = new MngPercent();
            mngPercent._UserId = userId;

            if (mngPercent.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");
                RefreshUserList();
            }
        }

        private void buttonNewCount_Click(object sender, EventArgs e)
        {
            if (UserView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            string userId = UserView.SelectedRows[0].Cells["Id"].Value.ToString();

            MngPrivilege mngPrevilege = new MngPrivilege();
            mngPrevilege._UserId = userId;

            if (mngPrevilege.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功.");
                RefreshUserList();
            }
        }

    }
}
