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
    public partial class MngChargeHistory : Form
    {
        public int _Kind = (int)ChargeKind.Charge;
        
        private List<ChargeHistoryInfo> _ChargeList = new List<ChargeHistoryInfo>();
        private int _ChargeSelId = -1;

        public MngChargeHistory()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
//            if (_Kind == (int)ChargeKind.Discharge)
//                this.Text = "환전이력";

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

            RefreshChargeHistoryList();
        }

        public void RefreshChargeHistoryList()
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

            _ChargeList = Database.GetInstance().GetChargeDayList(_Kind, 1, year, month, day);

            if (_ChargeList == null)
            {
                MessageBox.Show("不能在资料库获取结算信息.");
                return;
            }

            HistoryView.Rows.Clear();
            SumView.Rows.Clear();

            int totalSum = 0;

            for (int i = 0; i < _ChargeList.Count; i++)
            {
                ChargeHistoryInfo historyInfo = _ChargeList[i];

                UserInfo userInfo = Database.GetInstance().FindUser(historyInfo.OwnId);

                if (userInfo == null)
                    continue;

                HistoryView.Rows.Add(
                        historyInfo.OwnId,
                        userInfo.Nickname,
                        historyInfo.BankAccount,
                        historyInfo.Cash,
                        historyInfo.ApproveId,
                        BaseInfo.ConvDateToString(historyInfo.EndTime)
                    );

                totalSum += historyInfo.Cash;

                if (historyInfo.Id == _ChargeSelId)
                {
                    HistoryView.CurrentCell = HistoryView[0, HistoryView.Rows.Count - 1];
                }
            }
            _ChargeSelId = -1;

            SumView.Rows.Add( "", "", "", totalSum, "", "");

            labelSummary.Text = string.Format("总数: {0}", _ChargeList.Count);
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            RefreshChargeHistoryList();
        }

    }
}
