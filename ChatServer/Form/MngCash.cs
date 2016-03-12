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
    public partial class MngCash : Form
    {
        public string _UserId = null;

        public MngCash()
        {
            InitializeComponent();
        }

        private void MngCash_Load(object sender, EventArgs e)
        {
            UserInfo userInfo = Database.GetInstance().FindUser(_UserId);

            if (userInfo == null)
                return;

            int cashTotal = 0;
            int chatTotal = 0;
            int gameTotal = 0;
            int presentTotal = 0;

            List<ChatHistoryInfo> chatHistoryList = Database.GetInstance().GetAllChatHistories(_UserId);

            if (chatHistoryList != null)
            {
                for (int i = 0; i < chatHistoryList.Count; i++)
                {
                    ChatHistoryInfo historyInfo = chatHistoryList[i];

                    int total = 0;
                    string targetId = "";

                    switch (userInfo.Kind)
                    {
                        case (int)UserKind.Buyer:
                        case (int)UserKind.Recommender:
                        case (int)UserKind.RecommendOfficer:
                            total = historyInfo.BuyerTotal;
                            targetId = historyInfo.ServicemanId;
                            break;

                        case (int)UserKind.ServiceWoman:
                            total = historyInfo.ServicemanTotal;
                            targetId = historyInfo.BuyerId;
                            break;

                        case (int)UserKind.ServiceOfficer:
                            total = historyInfo.ServiceOfficerTotal;
                            targetId = historyInfo.ServicemanId;
                            break;

                        case (int)UserKind.Manager:
                            total = historyInfo.ManagerTotal;
                            targetId = historyInfo.ServicemanId;
                            break;
                    }

                    TimeSpan duration = historyInfo.EndTime - historyInfo.StartTime;

                    chatTotal += total;

                    ChatHistoryView.Rows.Add(
                        targetId,
                        string.Format( "{0}:{1}:{2}", duration.Hours, duration.Minutes, duration.Seconds ),
                        total,
                        BaseInfo.ConvDateToString( historyInfo.StartTime )
                    );
                }
            }

            List<ChargeHistoryInfo> chargeHistoryList = Database.GetInstance().GetAllChargeList(_UserId);

            if (chargeHistoryList != null)
            {
                for (int i = 0; i < chargeHistoryList.Count; i++)
                {
                    ChargeHistoryInfo historyInfo = chargeHistoryList[i];

                    if( historyInfo.Complete != (int)CompleteKind.Agree )
                        continue;

                    cashTotal += historyInfo.Cash;

                    ChargeHistoryView.Rows.Add(
                        historyInfo.KindString,
                        historyInfo.Cash,
                        BaseInfo.ConvDateToString( historyInfo.StartTime ),
                        BaseInfo.ConvDateToString(historyInfo.EndTime)
                    );
                }
            }

            List<PresentHistoryInfo> presentHistoryList = Database.GetInstance().GetAllPresentHistories(_UserId);

            if (presentHistoryList != null)
            {
                for (int i = 0; i < presentHistoryList.Count; i++)
                {
                    PresentHistoryInfo presentHistoryInfo = presentHistoryList[i];

                    string sendId = presentHistoryInfo.SendId;
                    string receiveId = presentHistoryInfo.ReceiveId;
                    int cash = presentHistoryInfo.Cash;

                    if (sendId == _UserId)
                    {
                        sendId = "";
                        cash = -cash;
                    }
                    else
                    {
                        receiveId = "";
                    }

                    PresentHistoryView.Rows.Add(
                        BaseInfo.ConvDateToString( presentHistoryInfo.SendTime ),
                        sendId,
                        receiveId,
                        cash
                    );

                    presentTotal += cash;
                }
            }

            labelChargeSum.Text = "合计: " + cashTotal;
            labelChatSum.Text = "合计: " + chatTotal;
            labelGameSum.Text = "合计: " + gameTotal;
            labelPresentSum.Text = "合计: " + presentTotal;

        }

        private void TotalView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
