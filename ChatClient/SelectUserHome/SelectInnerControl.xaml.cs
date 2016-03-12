using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatEngine;
using Microsoft.Windows.Controls;

namespace ChatClient
{
	/// <summary>
	/// InnerControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class SelectInnerControl : UserControl
	{
        private UserDetailInfo detailInfo = null;
        public SelectInnerControl()
		{
			this.InitializeComponent();
		}
//채팅탭
        public void InnerChatting(UserDetailInfo userDetail)
        {
            detailInfo = userDetail;
            DataGrid chatGrid = new DataGrid();
            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            DataGridTextColumn col3 = new DataGridTextColumn();
            
            chatGrid.Columns.Add(col1);
            chatGrid.Columns.Add(col2);
            chatGrid.Columns.Add(col3);
           

            col1.Binding = new Binding("nicName");
            col2.Binding = new Binding("price");
            col3.Binding = new Binding("time");

            if (userDetail != null)
            {
                for (int j = 0; j < userDetail.ChatHistories.Count; j++)
                {
                    if (Main._UserInfo.Kind == (int)UserKind.Buyer)
                    {
                        chatGrid.Items.Add(new ChatData
                        {
                            nicName = userDetail.ChatHistories[j].ServicemanId,
                            price = userDetail.ChatHistories[j].ServicePrice.ToString(),
                            time = userDetail.ChatHistories[j].StartTime.ToString() + "-" + userDetail.ChatHistories[j].EndTime.ToString()
                        });
                    }
                    else if (Main._UserInfo.Kind == (int)UserKind.ServiceWoman)
                    {
                        chatGrid.Items.Add(new ChatData
                        {
                            nicName = userDetail.ChatHistories[j].BuyerId,
                            price = userDetail.ChatHistories[j].ServicePrice.ToString(),
                            time = userDetail.ChatHistories[j].StartTime.ToString() + "-" + userDetail.ChatHistories[j].EndTime.ToString()
                        });
                    }
                }
            }
            col1.Header = "채팅상태";
            col2.Header = "채팅금액";
            col3.Header = "채팅시간";


            chatPan.Children.Add(chatGrid);
        }
        public struct ChatData
        {
            public string nicName { set; get; }
            public string price { set; get; }
            public string time { set; get; }
        }
//충전탭
        private void chargeTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            chargePan.Children.Clear();

            DataGrid moneyGrid = new DataGrid();
            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            DataGridTextColumn col3 = new DataGridTextColumn();
            DataGridTextColumn col4 = new DataGridTextColumn();

            moneyGrid.Columns.Add(col1);
            moneyGrid.Columns.Add(col2);
            moneyGrid.Columns.Add(col3);
            moneyGrid.Columns.Add(col4);


            col1.Binding = new Binding("kind");
            col2.Binding = new Binding("cash");
            col3.Binding = new Binding("time");
            col4.Binding = new Binding("complete");

            if (detailInfo != null)
            {
                for (int j = 0; j < detailInfo.ChargeHistories.Count; j++)
                {

                    moneyGrid.Items.Add(new ChargeData
                    {
                        kind = detailInfo.ChargeHistories[j].KindString,
                        cash = detailInfo.ChargeHistories[j].Cash.ToString(),
                        time = detailInfo.ChargeHistories[j].EndTime.ToString(),
                        complete = detailInfo.ChargeHistories[j].CompleteString
                    });

                }
            }
            col1.Header = "결제종류";
            col2.Header = "결제금액";
            col3.Header = "결제일";
            col4.Header = "결제상태";


            chargePan.Children.Add(moneyGrid);
        }
        public struct ChargeData
        {
            public string kind { set; get; }
            public string cash { set; get; }
            public string time { set; get; }
            public string complete { set; get; }
        }
//환전탭
        private void hunjonTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            hunjonPan.Children.Clear();

            DataGrid hunjonGrid = new DataGrid();
            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            DataGridTextColumn col3 = new DataGridTextColumn();
            DataGridTextColumn col4 = new DataGridTextColumn();

            hunjonGrid.Columns.Add(col1);
            hunjonGrid.Columns.Add(col2);
            hunjonGrid.Columns.Add(col3);
            hunjonGrid.Columns.Add(col4);


            col1.Binding = new Binding("ownId");
            col2.Binding = new Binding("value");
            col3.Binding = new Binding("buyId");
            col4.Binding = new Binding("evalTime");

            if (detailInfo != null)
            {
                for (int j = 0; j < detailInfo.EvalHistories.Count; j++)
                {

                    hunjonGrid.Items.Add(new EvalData
                    {
                        ownId = detailInfo.EvalHistories[j].OwnId,
                        value = detailInfo.EvalHistories[j].Value.ToString(),
                        buyId = detailInfo.EvalHistories[j].BuyerId,
                        evalTime = detailInfo.EvalHistories[j].EvalTime.ToString()
                    });

                }
            }
            col1.Header = "wwwww";
            col2.Header = "결제금액";
            col3.Header = "소유자";
            col4.Header = "결제일";


            hunjonPan.Children.Add(hunjonGrid);
        }
        public struct EvalData
        {
            public string ownId { set; get; }
            public string value { set; get; }
            public string buyId { set; get; }
            public string evalTime { set; get; }
        }
//선물탭
        private void presentTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            sonmulPan.Children.Clear();

            DataGrid sonmulGrid = new DataGrid();
            sonmulGrid.IsReadOnly = true;
            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            DataGridTextColumn col3 = new DataGridTextColumn();
            DataGridTextColumn col4 = new DataGridTextColumn();
            DataGridTextColumn col5 = new DataGridTextColumn();

            sonmulGrid.Columns.Add(col1);
            sonmulGrid.Columns.Add(col2);
            sonmulGrid.Columns.Add(col3);
            sonmulGrid.Columns.Add(col4);
            sonmulGrid.Columns.Add(col5);


            col1.Binding = new Binding("sendId");
            col2.Binding = new Binding("receiveId");
            col3.Binding = new Binding("cash");
            col4.Binding = new Binding("descripiton");
            col5.Binding = new Binding("sendTime");

            if (detailInfo != null)
            {
                for (int j = 0; j < detailInfo.PresentHistories.Count; j++)
                {

                    sonmulGrid.Items.Add(new PresentData
                    {
                        sendId = detailInfo.PresentHistories[j].SendId,
                        receiveId = detailInfo.PresentHistories[j].ReceiveId,
                        cash = detailInfo.PresentHistories[j].Cash,
                        descripiton = detailInfo.PresentHistories[j].Descripiton,
                        sendTime = detailInfo.PresentHistories[j].SendTime.ToString()
                    });

                }
            }
            col1.Header = "보낸사람";
            col2.Header = "받은사람";
            col3.Header = "캐쉬";
            col4.Header = "설명";
            col5.Header = "보낸날자";


            sonmulPan.Children.Add(sonmulGrid);
        }
        private struct PresentData
        {
            public string sendId { set; get; }
            public string receiveId { set; get; }
            public int cash { set; get; }
            public string descripiton { set; get; }
            public string sendTime { set; get; }
        }
//게임탭
        private void gameTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            gamePan.Children.Clear();

            DataGrid gameGrid = new DataGrid();
            gameGrid.IsReadOnly = true;
            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            DataGridTextColumn col3 = new DataGridTextColumn();
            DataGridTextColumn col4 = new DataGridTextColumn();
            DataGridTextColumn col5 = new DataGridTextColumn();
            DataGridTextColumn col6 = new DataGridTextColumn();
            DataGridTextColumn col7 = new DataGridTextColumn();
            DataGridTextColumn col8 = new DataGridTextColumn();
            DataGridTextColumn col9 = new DataGridTextColumn();

            gameGrid.Columns.Add(col1);
            gameGrid.Columns.Add(col2);
            gameGrid.Columns.Add(col3);
            gameGrid.Columns.Add(col4);
            gameGrid.Columns.Add(col5);
            gameGrid.Columns.Add(col6);
            gameGrid.Columns.Add(col7);
            gameGrid.Columns.Add(col8);
            gameGrid.Columns.Add(col9);


            col1.Binding = new Binding("gameId");
            col2.Binding = new Binding("buyerId");
            col3.Binding = new Binding("buyerTotal");
            col4.Binding = new Binding("managerTotal");
            col5.Binding = new Binding("recommenderId");
            col6.Binding = new Binding("recommenderTotal");
            col7.Binding = new Binding("recommendOfficerTotal");
            col8.Binding = new Binding("startTime");
            col9.Binding = new Binding("endTime");

            if (detailInfo != null)
            {
                for (int j = 0; j < detailInfo.GameHistories.Count; j++)
                {

                    gameGrid.Items.Add(new GameData
                    {
                        gameId = detailInfo.GameHistories[j].GameId,
                        buyerId = detailInfo.GameHistories[j].BuyerId,
                        buyerTotal = detailInfo.GameHistories[j].BuyerTotal,
                        managerTotal = detailInfo.GameHistories[j].ManagerTotal,
                        recommenderId = detailInfo.GameHistories[j].RecommenderId,
                        recommenderTotal = detailInfo.GameHistories[j].RecommenderTotal,
                        recommendOfficerTotal = detailInfo.GameHistories[j].RecommendOfficerTotal,
                        startTime = detailInfo.GameHistories[j].StartTime.ToString(),
                        endTime = detailInfo.GameHistories[j].EndTime.ToString()
                    });

                }
            }
            col1.Header = "gameId";
            col2.Header = "buyerId";
            col3.Header = "buyerTotal";
            col4.Header = "managerTotal";
            col5.Header = "recommenderId";
            col6.Header = "recommenderTotal";
            col7.Header = "recommendOfficerTotal";
            col8.Header = "startTime";
            col9.Header = "endTime";


            gamePan.Children.Add(gameGrid);
        }
        private struct GameData
        {
            public string gameId { set; get; }
            public string buyerId { set; get; }
            public int buyerTotal { set; get; }
            public int managerTotal { set; get; }
            public string recommenderId { set; get; }
            public int recommenderTotal { set; get; }
            public int recommendOfficerTotal { set; get; }
            public string startTime { set; get; }
            public string endTime { set; get; }
        }
	}
    

}