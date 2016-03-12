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
    public partial class MngChargeApprove : Form
    {
        public MngChargeApprove()
        {
            InitializeComponent();
        }

        private void MngUser_Load(object sender, EventArgs e)
        {
            RefreshChargeHistoryList();
        }

        public void RefreshChargeHistoryList()
        {
            List<ChargeHistoryInfo> chargeList = Database.GetInstance().GetChargeRequestList();

            if (chargeList == null)
            {
                MessageBox.Show( "不能在资料库获取结帐信息.");
                return;
            }

            HistoryView.Rows.Clear();

            foreach ( ChargeHistoryInfo historyInfo in chargeList)
            {
                UserInfo userInfo = Database.GetInstance().FindUser(historyInfo.OwnId);

                if (userInfo == null)
                    continue;

                HistoryView.Rows.Add(
                        historyInfo.OwnId,
                        userInfo.Nickname,
                        historyInfo.BankAccount,
                        historyInfo.Cash,
                        BaseInfo.ConvDateToString(historyInfo.StartTime)
                    );

                HistoryView.Rows[HistoryView.Rows.Count - 1].Tag = historyInfo;
            }

            labelSummary.Text = string.Format("申请总数量:: {0}", chargeList.Count);
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            RefreshChargeHistoryList();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddCharge addCharge = new AddCharge();

            if (addCharge.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("设置成功");
                RefreshChargeHistoryList();
            }
        }

        private void buttonApprove_Click(object sender, EventArgs e)
        {
            //if (HistoryView.CurrentCell == null)
            //    return;

            //ChargeHistoryInfo historyInfo = (ChargeHistoryInfo)HistoryView.Rows[HistoryView.CurrentCell.RowIndex].Tag;

            //if (historyInfo == null)
            //    return;

            //if (Cash.GetInstance().ProcessCharge(historyInfo.Id, 1) == true)
            //{
            //    MessageBox.Show("申请通过.");
            //    RefreshChargeHistoryList();

            //    List<ChargeHistoryInfo> historyList = Database.GetInstance().GetAllChargeList(historyInfo.OwnId);

            //    if (historyList.Count == 1)
            //    {
            //        UserInfo userInfo = Database.GetInstance().FindUser(historyInfo.OwnId);

            //        if (userInfo != null && string.IsNullOrEmpty(userInfo.Friend) == false )
            //        {
            //            UserInfo recommender = Database.GetInstance().FindUser(userInfo.Friend);

            //            if (recommender != null)
            //            {
            //                EnvInfo envInfo = Database.GetInstance().GetEnviroment();

            //                recommender.Point += envInfo.ChargeBonusPoint;
            //                Database.GetInstance().UpdateUser(recommender);

            //                UserInfo liveInfo = Users.GetInstance().FindUser(recommender.Id);

            //                if (liveInfo != null)
            //                    liveInfo.Point += envInfo.ChargeBonusPoint;
            //            }
            //        }
            //    }
            //}

        }

    }
}
