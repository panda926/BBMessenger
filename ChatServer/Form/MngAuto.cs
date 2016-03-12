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
    public partial class MngAuto : Form
    {
        public UserKind _Kind = UserKind.Buyer;

        public bool _IsRecommenderBuyer = true;

        public MngAuto()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            RefreshUserList();
        }

        public void RefreshUserList()
        {
            string queryStr = string.Format("select * from tblUser where Auto > 0 " );

            List<UserInfo> userList = Database.GetInstance().GetUserList(queryStr);

            if (userList == null)
            {
                MessageBox.Show("不能在资料库获取会员目录.");
                return;
            }

            UserView.Rows.Clear();

            int gameSum = 0;

            for (int i = 0; i < userList.Count; i++)
            {
                UserInfo userInfo = userList[i];

                gameSum += userInfo.GameSum;

                UserView.Rows.Add(
                    userInfo.Id,
                    userInfo.Nickname,
                    userInfo.Auto,
                    userInfo.GameSum,
                    userInfo.LoginTime
                    );

                UserView.Rows[UserView.Rows.Count - 1].Tag = userInfo;

                if (userInfo.GameSum < 0 )
                    UserView.Rows[UserView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Blue;
            }

            labelSummary.Text = string.Format("总电脑数: {0}", userList.Count);
            labelGameSum.Text = string.Format("总金额: {0}", gameSum);
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser._Kind = (int)UserKind.Buyer;
            addUser._IsAuto = true;

            if (addUser.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("会员登陆成功.");
                RefreshUserList();
            }
        }

        private void buttonEditUser_Click(object sender, EventArgs e)
        {
            if (UserView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需修改的会员帐号.");
                return;
            }

            string userId = UserView.SelectedRows[0].Cells["Id"].Value.ToString();

            AddUser editUser = new AddUser();

            editUser._IsEditMode = true;
            editUser._UserId = userId;
            editUser._Kind = (int)UserKind.Buyer;
            editUser._IsAuto = true;

            if (editUser.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("修改会员帐号成功.");
                RefreshUserList();
            }
        }

        private void buttonDelUser_Click(object sender, EventArgs e)
        {
            if (UserView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需删除的电脑人.");
                return;
            }

            UserInfo userInfo = (UserInfo)UserView.SelectedRows[0].Tag;

            string question = string.Format("真的要删除 {0} 电脑人吗?", userInfo.Id);

            if (MessageBox.Show(question, "删除提问", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (Database.GetInstance().DelUser(userInfo.Id) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            string resultStr = string.Format("{0} 电脑人已被删除.", userInfo.Id);
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
    }
}
