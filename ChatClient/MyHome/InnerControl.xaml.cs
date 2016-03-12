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
	public partial class InnerControl : UserControl
	{
		public InnerControl()
		{
			this.InitializeComponent();

            //if (Window1._UserInfo.Kind == (int)UserKind.ServiceWoman)
            //{
                DetailTab.Items.RemoveAt(4);
            //}
		}
//채팅탭
        public void InnerChatting(List<ChatHistoryInfo> chatHistoryList)
        {
            if (chatPan.Children.Count > 0)
                return;

            DataGrid chatGrid = new DataGrid();

            chatGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            chatGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;

            chatGrid.IsReadOnly = true;
            chatGrid.CellStyle = (Style)FindResource("cellStyle");
            chatGrid.RowStyle = (Style)FindResource("rowStyle");
            chatGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            chatGrid.Background = new SolidColorBrush(Colors.Transparent);

            chatGrid.SelectionMode = DataGridSelectionMode.Single;
            chatGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            //chatGrid.LoadingRow += dataGrid_LoadingRow;

            chatGrid.Width = 385;
            chatGrid.Height = 285;

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Width = 120;
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 75;
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = new DataGridLength(165, DataGridLengthUnitType.Star);
            
            chatGrid.Columns.Add(col1);
            chatGrid.Columns.Add(col2);
            chatGrid.Columns.Add(col3);

           
            col1.Binding = new Binding("nicName");
            col2.Binding = new Binding("price");
            col3.Binding = new Binding("time");

            int nTotalPrice = 0;

            if (chatHistoryList != null)
            {
                for (int j = 0; j < chatHistoryList.Count; j++)
                {
                    string timeStr = chatHistoryList[j].StartTime.ToString("yy/MM/dd HH:mm") + " - ";

                    if( chatHistoryList[j].StartTime.Day == chatHistoryList[j].EndTime.Day )
                        timeStr += chatHistoryList[j].EndTime.ToString("HH:mm");
                    else
                        timeStr += chatHistoryList[j].EndTime.ToString("yy/MM/dd HH:mm");

                    string userName = string.Empty;
                    int userPrice = 0;

                    switch((UserKind)Window1._UserInfo.Kind)
                    {
                        case UserKind.Buyer:
                        case UserKind.Recommender:
                        case UserKind.RecommendOfficer:
                            {
                                userName = chatHistoryList[j].ServicemanId;
                                userPrice = chatHistoryList[j].BuyerTotal;
                            }
                            break;

                        case UserKind.ServiceWoman:
                            {
                                userName = chatHistoryList[j].BuyerId;
                                userPrice = chatHistoryList[j].ServicemanTotal;
                            }
                            break;

                        case UserKind.ServiceOfficer:
                            {
                                userName = chatHistoryList[j].ServicemanId;
                                userPrice = chatHistoryList[j].ServiceOfficerTotal;
                            }
                            break;

                        case UserKind.Manager:
                            {
                                userName = chatHistoryList[j].ServicemanId;
                                userPrice = chatHistoryList[j].ManagerTotal;
                            }
                            break;
                    }

                    chatGrid.Items.Add(new ChatData
                    {
                        nicName = userName,
                        price = userPrice.ToString(),
                        time = timeStr
                    });

                    nTotalPrice += userPrice;
                }
            }
            col1.Header = "聊天状态";
            col2.Header = "聊天元宝";
            col3.Header = "聊天时间";

            chatPan.Children.Add(chatGrid);
            chatPan.Orientation = Orientation.Vertical;

            //StackPanel stackPanel = new StackPanel();
            //stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

            Label labelTotalPrice = new Label();
            labelTotalPrice.HorizontalAlignment = HorizontalAlignment.Center;
            labelTotalPrice.Foreground = new SolidColorBrush(Colors.Red);
            labelTotalPrice.Content = "总: " + nTotalPrice.ToString();

            chatPan.Children.Add(labelTotalPrice);
            
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

            moneyGrid.IsReadOnly = true;
            moneyGrid.CellStyle = (Style)FindResource("cellStyle");
            moneyGrid.RowStyle = (Style)FindResource("rowStyle");
            moneyGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            moneyGrid.Background = new SolidColorBrush(Colors.Transparent);

            moneyGrid.SelectionMode = DataGridSelectionMode.Single;
            moneyGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Width = 65;
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 72;
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = 70;
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Width = 110;
            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Width = new DataGridLength(65, DataGridLengthUnitType.Star);
            

            moneyGrid.Columns.Add(col1);
            moneyGrid.Columns.Add(col2);
            moneyGrid.Columns.Add(col3);
            moneyGrid.Columns.Add(col4);
            moneyGrid.Columns.Add(col5);


            col1.Binding = new Binding("kind");
            col2.Binding = new Binding("cash");
            col3.Binding = new Binding("time");
            col4.Binding = new Binding("bankAccount");
            col5.Binding = new Binding("complete");

            if (Window1.myhome.chargeHistoryList != null)
            {
                for (int j = 0; j < Window1.myhome.chargeHistoryList.Count; j++)
                {

                    moneyGrid.Items.Add(new ChargeData
                    {
                        kind = Window1.myhome.chargeHistoryList[j].KindString,
                        cash = Window1.myhome.chargeHistoryList[j].Cash.ToString(),
                        time = Window1.myhome.chargeHistoryList[j].EndTime.ToString("MM/dd"),
                        bankAccount = Window1.myhome.chargeHistoryList[j].BankAccount,
                        complete = Window1.myhome.chargeHistoryList[j].CompleteString,
                        state = Window1.myhome.chargeHistoryList[j].Complete
                    });

                }
            }
            col1.Header = "结算种类";
            col2.Header = "结算金额";
            col3.Header = "结算日期";
            col4.Header = "银行";
            col5.Header = "结算状态";

            chargePan.Children.Add(moneyGrid);
        }

       

        void chargeGrid_LoadingRow(object sender, Microsoft.Windows.Controls.DataGridRowEventArgs e)
        {
            // Get the DataRow corresponding to the DataGridRow that is loading.
            ChargeData item = e.Row.Item as ChargeData;
            if (item != null)
            {
                if (item.state == 0 )
                    e.Row.Foreground = new SolidColorBrush(Colors.Blue);
                else if( item.state == 2 )
                    e.Row.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        public class ChargeData
        {
            public string kind { set; get; }
            public string cash { set; get; }
            public string time { set; get; }
            public string complete { set; get; }
            public string bankAccount { set; get; }
            public int state { set; get; }
        }
//평점탭
        private void hunjonTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (markPan.Children.Count > 0)
                return;

            markPan.Children.Clear();

            DataGrid markGrid = new DataGrid();
            markGrid.Height = 300;

            markGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            markGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
            markGrid.CellStyle = (Style)FindResource("cellStyle");
            markGrid.RowStyle = (Style)FindResource("rowStyle");
            markGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            markGrid.Background = new SolidColorBrush(Colors.Transparent);

            markGrid.IsReadOnly = true;
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
            

            markGrid.Columns.Add(col1);
            markGrid.Columns.Add(col2);
            markGrid.Columns.Add(col3);
            markGrid.Columns.Add(col4);
            markGrid.Columns.Add(col5);


            col1.Binding = new Binding("no");
            col2.Binding = new Binding("ownId");
            col3.Binding = new Binding("value");
            col4.Binding = new Binding("buyId");
            col5.Binding = new Binding("evalTime");

            if (Window1.myhome.evalHistoryList != null)
            {
                for (int j = 0; j < Window1.myhome.evalHistoryList.Count; j++)
                {

                    markGrid.Items.Add(new EvalData
                    {
                        no = j + 1,
                        ownId = Window1.myhome.evalHistoryList[j].OwnId,
                        value = Window1.myhome.evalHistoryList[j].Value.ToString(),
                        buyId = Window1.myhome.evalHistoryList[j].BuyerId,
                        evalTime = Window1.myhome.evalHistoryList[j].EvalTime.ToString()
                    });

                }
            }
            col1.Header = "帐号";
            col2.Header = "宝贝";
            col3.Header = "评价";
            col4.Header = "评价信息";
            col5.Header = "评价时间";


            markPan.Children.Add(markGrid);
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
            if (sonmulPan.Children.Count > 0)
                return;

            sonmulPan.Children.Clear();

            DataGrid sonmulGrid = new DataGrid();
            sonmulGrid.Height = 285;

            sonmulGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            sonmulGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
            sonmulGrid.CellStyle = (Style)FindResource("cellStyle");
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

            int nTotalPresent = 0;

            if (Window1.myhome.presentHistoryList != null)
            {
               for (int j = 0; j < Window1.myhome.presentHistoryList.Count; j++)
                {

                    sonmulGrid.Items.Add(new PresentData
                    {
                        sendId = Window1.myhome.presentHistoryList[j].SendId,
                        receiveId = Window1.myhome.presentHistoryList[j].ReceiveId,
                        cash = Window1.myhome.presentHistoryList[j].Cash,
                        descripiton = Window1.myhome.presentHistoryList[j].Descripiton,
                        sendTime = Window1.myhome.presentHistoryList[j].SendTime.ToString()
                    });

                    nTotalPresent += Window1.myhome.presentHistoryList[j].Cash;
                }
            }
            col1.Header = "送礼信息";
            col2.Header = "收礼信息";
            col3.Header = "元宝";
            col4.Header = "信息";
            col5.Header = "送礼时间";


            sonmulPan.Children.Add(sonmulGrid);


            Label labelTotalPresent = new Label();
            labelTotalPresent.HorizontalAlignment = HorizontalAlignment.Center;
            labelTotalPresent.Foreground = new SolidColorBrush(Colors.Red);
            labelTotalPresent.Content = "总: " + nTotalPresent.ToString();

            sonmulPan.Children.Add(labelTotalPresent);
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
            gamelGrid.Height = 285;

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