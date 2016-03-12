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
    public partial class MngChatPercent : Form
    {
        private static MngChatPercent _instance = null;
        public string _searchId = string.Empty;

        public bool _isSaveUserList = false;
        public List<UserInfo> _saveList = new List<UserInfo>();

        public static MngChatPercent GetInstance()
        {
            if( _instance == null )
                _instance = new MngChatPercent();

            return _instance;
        }

        public MngChatPercent()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            if (Users.ManagerInfo.Kind == (int)UserKind.ServiceOfficer)
                comboRecommender.Enabled = false;
            else
                RefreshRecommenderList();

            RefreshUserList();
        }

        public void RefreshRecommenderList()
        {
            comboRecommender.Items.Clear();

            string queryStr = string.Format("select * from tblUser where Kind = {0} ", (int)UserKind.ServiceOfficer);

            queryStr += " order by Id";

            List<UserInfo> recommenderList = Database.GetInstance().GetUserList(queryStr);

            if (recommenderList != null)
            {
                foreach (UserInfo userInfo in recommenderList)
                {
                    comboRecommender.Items.Add(userInfo.Id);
                }
            }
        }

        public void RefreshUserList()
        {
            string queryStr = string.Format("select * from tblUser where Kind != {0} and Kind != {1} and Kind != {2} and Kind != {3}", (int)UserKind.Manager, (int)UserKind.RecommendOfficer, (int)UserKind.Recommender, (int)UserKind.Buyer);

            if (Users.ManagerInfo.Kind == (int)UserKind.ServiceOfficer)
            {
                queryStr += string.Format(" and Recommender = '{0}'", Users.ManagerInfo.Id);
            }
            else
            {
                string contentStr = comboRecommender.Text.Trim();

                if (contentStr.Length > 0)
                {
                    queryStr += string.Format(" and Recommender like '%{0}%'", contentStr);
                }
            }

            queryStr += " order by Kind";

            List<UserInfo> userList = Database.GetInstance().GetUserList(queryStr);

            if (userList == null)
            {
                MessageBox.Show("不能在资料库获取宝贝信息.");
                return;
            }

            UserView.Rows.Clear();

            for (int i = 0; i < userList.Count; i++)
            {
                UserInfo userInfo = userList[i];

                int officerPercent = 0;

                UserInfo officerInfo = Database.GetInstance().FindUser( userInfo.Recommender );

                if( officerInfo != null )
                    officerPercent = officerInfo.ChatPercent;

                decimal average = 0;

                if (userInfo.Visitors > 0)
                    average = (decimal)userInfo.Evaluation / userInfo.Visitors;

                UserView.Rows.Add(
                    userInfo.KindString,
                    userInfo.Id,
                    userInfo.Nickname,
                    userInfo.ChatPercent,
                    officerPercent,
                    userInfo.Recommender,
                    string.Format( "{0: #.0} / {1} ", average, userInfo.Visitors ),
                    string.Format("{0}", userInfo.Cash),
                    userInfo.ChargeSum + userInfo.DischargeSum,
                    userInfo.ChatSum,
                    userInfo.SendSum + userInfo.ReceiveSum
                    );

                UserView.Rows[UserView.Rows.Count - 1].Tag = userInfo;

                bool incorrect = false;

                if (userInfo.ChatPercent <= 0)
                {
                    incorrect = true;
                }
                else if (userInfo.Kind == (int)UserKind.ServiceOfficer && userInfo.ChatPercent > 100)
                {
                    incorrect = true;
                }
                else if (userInfo.Kind == (int)UserKind.ServiceWoman )
                {
                    if( officerInfo == null )
                        incorrect = true;
                    else if( userInfo.ChatPercent >= officerInfo.ChatPercent )
                        incorrect = true;
                }


                if( incorrect == true )
                    UserView.Rows[UserView.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;

            }

            labelSummary.Text = string.Format("总会员数: {0}", userList.Count);

            if (string.IsNullOrEmpty(_searchId) == false)
            {
                SearchUser(_searchId);
                _searchId = string.Empty;
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            if (buttonSave.Enabled == true)
            {
                DialogResult result = MessageBox.Show("Do you save any changed?", "Confirm", MessageBoxButtons.YesNoCancel);

                if( result == DialogResult.Cancel )
                    return;

                if (result == DialogResult.Yes)
                    SavePercent();
            }

            RefreshUserList();
        }

        private void UserView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UserInfo userInfo = (UserInfo)UserView.Rows[ UserView.CurrentCell.RowIndex].Tag;

            try
            {
                int officePercent = Convert.ToInt32( UserView[UserView.CurrentCell.ColumnIndex + 1, UserView.CurrentCell.RowIndex ].Value );

                int chatPercent = Convert.ToInt32(UserView.CurrentCell.Value);

                if( chatPercent >= officePercent )
                    throw new Exception();

                userInfo.ChatPercent = chatPercent;
            }
            catch
            {
                MessageBox.Show("please input percent correctly");
                UserView.CurrentCell.Value = userInfo.ChatPercent.ToString();
                return;
            }

            bool isAlready = false;

            foreach (UserInfo saveInfo in _saveList)
            {
                if (saveInfo == userInfo)
                {
                    isAlready = true;
                    break;
                }
            }

            if (isAlready == false)
                _saveList.Add(userInfo);


            _isSaveUserList = true;
            buttonSave.Enabled = true;
        }

        private void SavePercent()
        {
            foreach( UserInfo saveInfo in _saveList )
            {
                if( Database.GetInstance().UpdateUser(saveInfo) == false )
                {
                    MessageBox.Show( "Fail to save chat percent");
                    return;
                }
            }

            MessageBox.Show("saved successfully");

            _saveList.Clear();
            _isSaveUserList = false;
            buttonSave.Enabled = false;

            RefreshUserList();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SavePercent();
        }

        private void MngChatPercent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (buttonSave.Enabled == true)
            {
                DialogResult result = MessageBox.Show("Do you save any changed?", "Confirm", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                if (result == DialogResult.Yes)
                    SavePercent();
            }

            _instance = null;
        }

        private void buttonEvaluation_Click(object sender, EventArgs e)
        {
            if( UserView.CurrentCell == null )
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            UserInfo userInfo = (UserInfo)UserView.Rows[UserView.CurrentCell.RowIndex].Tag;

            MngEvaluation managerEvaluation = new MngEvaluation();
            managerEvaluation._UserId = userInfo.Id;

            managerEvaluation.ShowDialog();
        }

        private void buttonHistory_Click_1(object sender, EventArgs e)
        {
            if (UserView.CurrentCell == null)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            UserInfo userInfo = (UserInfo)UserView.Rows[UserView.CurrentCell.RowIndex].Tag;

            MngCash mngCash = new MngCash();
            mngCash._UserId = userInfo.Id;

            mngCash.ShowDialog();
        }

        public void SearchUser( string userId )
        {
            foreach (DataGridViewRow viewRow in UserView.Rows)
            {
                UserInfo userInfo = (UserInfo)viewRow.Tag;

                if (userInfo.Id == userId)
                {
                    UserView.CurrentCell = viewRow.Cells[3];
                    break;
                }
            }
        }
    }
}
