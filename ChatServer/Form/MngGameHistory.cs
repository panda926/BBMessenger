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
    public partial class MngGameHistory : Form
    {
        public MngGameHistory()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            comboYear.Items.Add("全部");
            comboMonth.Items.Add("全部");
            comboDay.Items.Add("全部");

            for (int i = DateTime.Now.Year - 5; i < DateTime.Now.Year + 5; i++)
            {
                comboYear.Items.Add(i);

                if (i == DateTime.Now.Year)
                    comboYear.SelectedIndex = comboYear.Items.Count - 1;
            }

            for (int i = 1; i <= 12; i++)
            {
                comboMonth.Items.Add(i);

                if (i == DateTime.Now.Month)
                    comboMonth.SelectedIndex = comboMonth.Items.Count - 1;
            }

            for (int i = 1; i <= 31; i++)
            {
                comboDay.Items.Add(i);

                if (i == DateTime.Now.Day)
                    comboDay.SelectedIndex = comboDay.Items.Count - 1;
            }

            if (Users.ManagerInfo.Kind == (int)UserKind.RecommendOfficer)
            {
                HistoryView.Columns[HistoryView.Columns.Count - 1].Width = 0;
                HistoryView.Columns[HistoryView.Columns.Count - 2].Width = 0;

                SumView.Columns[SumView.Columns.Count - 1].Width = 0;
                SumView.Columns[SumView.Columns.Count - 2].Width = 0;
            }

            RefreshUserList();
        }

        public void RefreshUserList()
        {
            int year = 0;
            int month = 0;
            int day = 0;

            try
            {
                year = Convert.ToInt32(comboYear.Text);
                month = Convert.ToInt32(comboMonth.Text);
                day = Convert.ToInt32(comboDay.Text);
            }
            catch
            {
            }

            List<GameHistoryInfo> gameHistoryList = Database.GetInstance().GetDayGameHistories(year, month, day);

            if (gameHistoryList == null)
            {
                MessageBox.Show("不能在资料库获取游戏信息.");
                return;
            }

            HistoryView.Rows.Clear();
            SumView.Rows.Clear();

            int sumBuyerTotal = 0;
            int RecommenderTotal = 0;
            int officerTotal = 0;
            int sumManagerTotal = 0;

            for (int i = 0; i < gameHistoryList.Count; i++)
            {
                GameHistoryInfo historyInfo = gameHistoryList[i];

                TimeSpan duration = historyInfo.EndTime - historyInfo.StartTime;

                //string managerId = Users.ManagerInfo.Id;
                //int managerCash = 0;

                //switch (Users.ManagerInfo.Kind)
                //{
                //    case (int)UserKind.Manager:
                //        managerCash = historyInfo.ManagerTotal;
                //        break;

                //    case (int)UserKind.RecommendOfficer:
                //        managerCash = historyInfo.RecommendOfficerTotal;
                //        break;
                //}

                HistoryView.Rows.Add(
                    BaseInfo.ConvDateToString( historyInfo.StartTime ),
                    string.Format( "{0:00}:{1:00}:{2:00}", duration.Hours, duration.Minutes, duration.Seconds ),
                    historyInfo.GameId,
                    historyInfo.BuyerId,
                    historyInfo.BuyerTotal,
                    historyInfo.RecommenderId,
                    historyInfo.RecommenderTotal,
                    historyInfo.OfficerId,
                    historyInfo.RecommendOfficerTotal,
                    historyInfo.ManagerId,
                    historyInfo.ManagerTotal
                    );

                sumBuyerTotal += historyInfo.BuyerTotal;
                RecommenderTotal += historyInfo.RecommenderTotal;
                officerTotal += historyInfo.RecommendOfficerTotal;
                sumManagerTotal += historyInfo.ManagerTotal;
            }

            SumView.Rows.Add(
                "",
                "",
                "",
                "",
                sumBuyerTotal,
                "",
                RecommenderTotal,
                "",
                officerTotal,
                "",
                sumManagerTotal);

            labelSummary.Text = string.Format("总数: {0}", gameHistoryList.Count);
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser();

            if (addUser.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("会员帐号登陆成功.");
                RefreshUserList();
            }
        }

        private void buttonEditUser_Click(object sender, EventArgs e)
        {
            if (HistoryView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            string userId = HistoryView.SelectedRows[0].Cells["Id"].Value.ToString();

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
            if (HistoryView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            string userId = HistoryView.SelectedRows[0].Cells["Id"].Value.ToString();

            string question = string.Format("真要删除 {0} 吗?", userId);

            if (MessageBox.Show(question, "删除提问", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (Database.GetInstance().DelUser(userId) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return;
            }

            string resultStr = string.Format("{0} 已被删除成功.", userId);
            MessageBox.Show(resultStr);

            RefreshUserList();

        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            RefreshUserList();
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if (HistoryView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择会员帐号.");
                return;
            }

            string userId = HistoryView.SelectedRows[0].Cells["Id"].Value.ToString();

            MngCash mngCash = new MngCash();
            mngCash._UserId = userId;

            mngCash.ShowDialog();

        }
    }
}
