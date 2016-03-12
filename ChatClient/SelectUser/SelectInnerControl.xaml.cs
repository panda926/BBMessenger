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
            if (chargePan.Children.Count > 0)
                return;

            detailInfo = userDetail;
            DataGrid chatGrid = new DataGrid();

            chatGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            chatGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
            chatGrid.RowStyle = (Style)FindResource("rowStyle");
            chatGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            chatGrid.Background = new SolidColorBrush(Colors.Transparent);
            chatGrid.Width = 385;
            chatGrid.Height = 285;

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Width = 120;
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 100;
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = new DataGridLength(165, DataGridLengthUnitType.Star);
            
            
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
                    if (Window1._UserInfo.Kind == (int)UserKind.Buyer)
                    {
                        chatGrid.Items.Add(new ChatData
                        {
                            nicName = userDetail.ChatHistories[j].ServicemanId,
                            price = userDetail.ChatHistories[j].ServicePrice.ToString(),
                            time = userDetail.ChatHistories[j].StartTime.ToString() + "-" + userDetail.ChatHistories[j].EndTime.ToString()
                        });
                    }
                    else if (Window1._UserInfo.Kind == (int)UserKind.ServiceWoman)
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
            col1.Header = "聊天状态";
            col2.Header = "聊天消费额";
            col3.Header = "聊天时长";


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
            if (chargePan.Children.Count > 0)
                return;
            chargePan.Children.Clear();

            DataGrid moneyGrid = new DataGrid();

            moneyGrid.Height = 300;

            moneyGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            moneyGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
            moneyGrid.RowStyle = (Style)FindResource("rowStyle");
            moneyGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            moneyGrid.Background = new SolidColorBrush(Colors.Transparent);
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Width = 65;
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 72;
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = 50;
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Width = 65;
            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Width = new DataGridLength(129, DataGridLengthUnitType.Star);
            

            moneyGrid.Columns.Add(col1);
            moneyGrid.Columns.Add(col2);
            moneyGrid.Columns.Add(col3);
            moneyGrid.Columns.Add(col4);
            moneyGrid.Columns.Add(col5);


            col1.Binding = new Binding("kind");
            col2.Binding = new Binding("cash");
            col3.Binding = new Binding("time");
            col4.Binding = new Binding("complete");
            col5.Binding = new Binding("complete");

            if (detailInfo != null)
            {
                for (int j = 0; j < detailInfo.ChargeHistories.Count; j++)
                {

                    moneyGrid.Items.Add(new ChargeData
                    {
                        kind = detailInfo.ChargeHistories[j].KindString,
                        cash = detailInfo.ChargeHistories[j].Cash.ToString(),
                        time = detailInfo.ChargeHistories[j].EndTime.ToString(),
                        complete = detailInfo.ChargeHistories[j].CompleteString,
                        bankAccount = detailInfo.ChargeHistories[j].BankAccount
                    });

                }
            }
            col1.Header = "结算种类";
            col2.Header = "结算金额";
            col3.Header = "结算日期";
            col4.Header = "结算状态";
            col5.Header = "结算状态";


            chargePan.Children.Add(moneyGrid);
        }
        public struct ChargeData
        {
            public string kind { set; get; }
            public string cash { set; get; }
            public string time { set; get; }
            public string complete { set; get; }
            public string bankAccount { set; get; }
        }
//평점탭
        private void hunjonTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (chargePan.Children.Count > 0)
                return;
            hunjonPan.Children.Clear();

            DataGrid hunjonGrid = new DataGrid();

            hunjonGrid.Height = 300;

            hunjonGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            hunjonGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
            hunjonGrid.RowStyle = (Style)FindResource("rowStyle");
            hunjonGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            hunjonGrid.Background = new SolidColorBrush(Colors.Transparent);

            hunjonGrid.IsReadOnly = true;
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Width = 65;
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 72;
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = 65;
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Width = 50;
            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Width = new DataGridLength(129, DataGridLengthUnitType.Star);

            hunjonGrid.Columns.Add(col1);
            hunjonGrid.Columns.Add(col2);
            hunjonGrid.Columns.Add(col3);
            hunjonGrid.Columns.Add(col4);
            hunjonGrid.Columns.Add(col5);


            col1.Binding = new Binding("no");
            col2.Binding = new Binding("ownId");
            col3.Binding = new Binding("value");
            col4.Binding = new Binding("buyId");
            col5.Binding = new Binding("evalTime");

            if (detailInfo.EvalHistories != null)
            {
                for (int j = 0; j < detailInfo.EvalHistories.Count; j++)
                {

                    hunjonGrid.Items.Add(new EvalData
                    {
                        no = j + 1,
                        ownId = detailInfo.EvalHistories[j].OwnId,
                        value = detailInfo.EvalHistories[j].Value.ToString(),
                        buyId = detailInfo.EvalHistories[j].BuyerId,
                        evalTime = detailInfo.EvalHistories[j].EvalTime.ToString()
                    });

                }
            }
            col1.Header = "帐号";
            col2.Header = "宝贝";
            col3.Header = "评价";
            col4.Header = "评价信息";
            col5.Header = "评价时间";


            hunjonPan.Children.Add(hunjonGrid);
        }
        public struct EvalData
        {
            public int no { set; get; }
            public string ownId { set; get; }
            public string value { set; get; }
            public string buyId { set; get; }
            public string evalTime { set; get; }
        }
//선물탭

        private void presentTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (chargePan.Children.Count > 0)
                return;
            sonmulPan.Children.Clear();

            DataGrid sonmulGrid = new DataGrid();
            sonmulGrid.Height = 300;

            sonmulGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            sonmulGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
            sonmulGrid.RowStyle = (Style)FindResource("rowStyle");
            sonmulGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            sonmulGrid.Background = new SolidColorBrush(Colors.Transparent);

            sonmulGrid.IsReadOnly = true;
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Width = 65;
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 72;
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = 50;
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Width = 65;
            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Width = new DataGridLength(129, DataGridLengthUnitType.Star);

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
            col1.Header = "送礼信息";
            col2.Header = "收礼信息";
            col3.Header = "元宝";
            col4.Header = "信息";
            col5.Header = "送礼时间";


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
            if (gamePan.Children.Count > 0)
                return;

            gamePan.Children.Clear();

            DataGrid gamelGrid = new DataGrid();
            gamelGrid.Height = 290;

            gamelGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            gamelGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;

            gamelGrid.IsReadOnly = true;
            gamelGrid.CellStyle = (Style)FindResource("cellStyle");
            gamelGrid.RowStyle = (Style)FindResource("rowStyle");
            gamelGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            gamelGrid.Background = new SolidColorBrush(Colors.Transparent);

            gamelGrid.SelectionMode = DataGridSelectionMode.Single;
            gamelGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            gamelGrid.LoadingRow += gameGrid_LoadingRow;

            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            DataGridTextColumn col3 = new DataGridTextColumn();

            col1.Width = 120;
            col2.Width = 75;
            col3.Width = new DataGridLength(165, DataGridLengthUnitType.Star);

            gamelGrid.Columns.Add(col1);
            gamelGrid.Columns.Add(col2);
            gamelGrid.Columns.Add(col3);


            col1.Binding = new Binding("gameId");
            col2.Binding = new Binding("price");
            col3.Binding = new Binding("elaspeTime");

            int gTotalPrice = 0;

            if (Window1.myhome.gameHistoryList != null)
            {
                for (int j = 0; j < Window1.myhome.gameHistoryList.Count; j++)
                {
                    int gamePrice = 0;

                    switch ((UserKind)Window1._UserInfo.Kind)
                    {
                        case UserKind.Buyer:
                        case UserKind.ServiceWoman:
                        case UserKind.ServiceOfficer:
                            {
                                gamePrice = Window1.myhome.gameHistoryList[j].BuyerTotal;
                            }
                            break;

                        case UserKind.Recommender:
                            {
                                gamePrice = Window1.myhome.gameHistoryList[j].RecommenderTotal;
                            }
                            break;

                        case UserKind.RecommendOfficer:
                            {
                                gamePrice = Window1.myhome.gameHistoryList[j].RecommendOfficerTotal;
                            }
                            break;

                        case UserKind.Manager:
                            {
                                gamePrice = Window1.myhome.gameHistoryList[j].ManagerTotal;
                            }
                            break;
                    }

                    string timeStr = Window1.myhome.gameHistoryList[j].StartTime.ToString("yy/MM/dd HH:mm") + " - ";

                    if (Window1.myhome.gameHistoryList[j].StartTime.Day == Window1.myhome.gameHistoryList[j].EndTime.Day)
                        timeStr += Window1.myhome.gameHistoryList[j].EndTime.ToString("HH:mm");
                    else
                        timeStr += Window1.myhome.gameHistoryList[j].EndTime.ToString("yy/MM/dd HH:mm");


                    gamelGrid.Items.Add(new GameData
                    {
                        gameId = Window1.myhome.gameHistoryList[j].GameId,
                        price = gamePrice,
                        elaspeTime = timeStr
                    });

                    gTotalPrice += gamePrice;
                }
            }
            col1.Header = "游戏信息";
            col2.Header = "游戏元宝";
            col3.Header = "游戏时间";


            gamePan.Children.Add(gamelGrid);

            gamePan.Orientation = Orientation.Vertical;

            //StackPanel stackPanel = new StackPanel();
            //stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

            Label gameTotalPrice = new Label();
            gameTotalPrice.HorizontalAlignment = HorizontalAlignment.Center;
            gameTotalPrice.Foreground = new SolidColorBrush(Colors.Red);
            gameTotalPrice.Content = "总: " + gTotalPrice.ToString();

            gamePan.Children.Add(gameTotalPrice);
        }

        void gameGrid_LoadingRow(object sender, Microsoft.Windows.Controls.DataGridRowEventArgs e)
        {
            // Get the DataRow corresponding to the DataGridRow that is loading.
            GameData item = e.Row.Item as GameData;
            if (item != null)
            {
                if (item.price > 0)
                    e.Row.Foreground = new SolidColorBrush(Colors.Green);
            }
        }

        private class GameData
        {
            public string gameId { set; get; }
            public int price { set; get; }
            public string elaspeTime { set; get; }

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