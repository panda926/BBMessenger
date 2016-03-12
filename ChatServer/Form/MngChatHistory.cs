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
    public partial class MngChatHistory : Form
    {
        public MngChatHistory()
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

            if (Users.ManagerInfo.Kind == (int)UserKind.ServiceOfficer)
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

            List<ChatHistoryInfo> chatHistoryList = Database.GetInstance().GetDayChatHistories(year, month, day);

            if (chatHistoryList == null)
            {
                MessageBox.Show("不能在资料库获取聊天记录目录.");
                return;
            }

            HistoryView.Rows.Clear();
            SumView.Rows.Clear();

            int sumBuyerTotal = 0;
            int sumServicemanTotal = 0;
            int sumOfficerTotal = 0;
            int sumManagerTotal = 0;

            for (int i = 0; i < chatHistoryList.Count; i++)
            {
                ChatHistoryInfo historyInfo = chatHistoryList[i];

                TimeSpan duration = historyInfo.EndTime - historyInfo.StartTime;


                //string managerId = Users.ManagerInfo.Id;
                //int managerCash = 0;

                //switch (Users.ManagerInfo.Kind)
                //{
                //    case (int)UserKind.Manager:
                //        managerCash = historyInfo.ManagerTotal;
                //        break;

                //    case (int)UserKind.ServiceOfficer:
                //        managerCash = historyInfo.ServiceOfficerTotal;
                //        break;
                //}

                HistoryView.Rows.Add(
                    BaseInfo.ConvDateToString( historyInfo.StartTime ),
                    string.Format( "{0:00}:{1:00}:{2:00}", duration.Hours, duration.Minutes, duration.Seconds ),
                    historyInfo.ServicePrice,
                    historyInfo.BuyerId,
                    historyInfo.BuyerTotal,
                    historyInfo.ServicemanId,
                    historyInfo.ServicemanTotal,
                    historyInfo.OfficerId,
                    historyInfo.ServiceOfficerTotal,
                    historyInfo.ManagerId,
                    historyInfo.ManagerTotal
                    );

                sumBuyerTotal += historyInfo.BuyerTotal;
                sumServicemanTotal += historyInfo.ServicemanTotal;
                sumOfficerTotal += historyInfo.ServiceOfficerTotal;
                sumManagerTotal += historyInfo.ManagerTotal;
            }

            SumView.Rows.Add(
                "",
                "",
                "",
                "",
                sumBuyerTotal,
                "",
                sumServicemanTotal,
                "",
                sumOfficerTotal,
                "",
                sumManagerTotal);

            labelSummary.Text = string.Format("总数: {0}", chatHistoryList.Count);
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            RefreshUserList();
        }
    }
}
