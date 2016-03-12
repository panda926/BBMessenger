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
using System.Data;

namespace ChatClient
{
	/// <summary>
	/// NoticeMemberControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class NoticeMemberControl : UserControl
	{
        UserInfo SelectUserInfo = null;
        string stateStr = null;
		public NoticeMemberControl()
		{
			this.InitializeComponent();
		}
        private void userList(List<UserInfo> userInfoList)
        {
            Window1.SelectUserInfoList.Clear();
            for (int i = 0; i < userInfoList.Count; i++)
            {
                SelectUserInfo = new UserInfo();
                SelectUserInfo.Id = userInfoList[i].Id;
                SelectUserInfo.Nickname = userInfoList[i].Nickname;
                SelectUserInfo.Icon = userInfoList[i].Icon;
                SelectUserInfo.Password = userInfoList[i].Password;
                SelectUserInfo.Year = userInfoList[i].Year;
                SelectUserInfo.Month = userInfoList[i].Month;
                SelectUserInfo.Day = userInfoList[i].Day;
                SelectUserInfo.Address = userInfoList[i].Address;
                SelectUserInfo.Point = userInfoList[i].Point;
                SelectUserInfo.Cash = userInfoList[i].Cash;
                SelectUserInfo.Evaluation = userInfoList[i].Evaluation;
                SelectUserInfo.Visitors = userInfoList[i].Visitors;
                Window1.SelectUserInfoList.Add(SelectUserInfo);
            }
            
        }
        public void listMember(List<UserInfo> userInfo)
		{
            this.userList(userInfo);

            if (Window1._UserInfo.Kind == (int)UserKind.ServiceWoman || Window1._UserInfo.Kind == (int)UserKind.Buyer)
            {
                label1.Visibility = Visibility.Hidden;
                buyers.Visibility = Visibility.Hidden;
            }
            else if (Window1._UserInfo.Kind == (int)UserKind.Recommender)
            {
                label1.Visibility = Visibility.Visible;
                buyers.Visibility = Visibility.Visible;
            }
            else
            {
                label1.Visibility = Visibility.Hidden;
                buyers.Visibility = Visibility.Hidden;
                label2.Visibility = Visibility.Hidden;
                reqBuyer.Visibility = Visibility.Hidden;
                idReqBt.Visibility = Visibility.Hidden;
            }
            buyers.Content = (Window1._UserInfo.MaxBuyers - userInfo.Count).ToString();
            reqBuyer.Content = userInfo.Count.ToString();


            DataGrid memberGrid = new DataGrid();

            memberGrid.Width = 390;
            memberGrid.Height = 300;
            memberGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            memberGrid.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;

            memberGrid.IsReadOnly = true;
            memberGrid.CellStyle = (Style)FindResource("cellStyle");
            memberGrid.RowStyle = (Style)FindResource("rowStyle");
            memberGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            memberGrid.Background = new SolidColorBrush(Colors.Transparent);

            memberGrid.SelectionMode = DataGridSelectionMode.Single;
            memberGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            memberGrid.LoadingRow += dataGrid_LoadingRow;

            //memberGrid.MouseLeftButtonUp += new MouseButtonEventHandler(memberGrid_MouseUp);
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Width = 55;
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Width = 70;
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Width = 55;
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Width = 55;
            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Width = 55;
            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Width = 40;
            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Width = new DataGridLength(57, DataGridLengthUnitType.Star);
            
            
            memberGrid.Columns.Add(col1);
            memberGrid.Columns.Add(col2);
            memberGrid.Columns.Add(col3);
            memberGrid.Columns.Add(col4);
            memberGrid.Columns.Add(col5);
            memberGrid.Columns.Add(col6);
            memberGrid.Columns.Add(col7);
            

            col1.Binding = new Binding("id");
            col2.Binding = new Binding("nicName");
            col3.Binding = new Binding("cash");
            col4.Binding = new Binding("chatSum");
            col5.Binding = new Binding("gameSum");
            col6.Binding = new Binding("point");
            col7.Binding = new Binding("state");

            if (userInfo != null)
            {
                for (int j = 0; j < userInfo.Count; j++)
                {
                    if (userInfo[j].LoginTime == "")
                        stateStr = "新帐号";
                    else
                        stateStr = "使用中的帐号";
                    memberGrid.Items.Add(new ListData
                    {
                        id = userInfo[j].Id,
                        nicName = userInfo[j].Nickname,
                        cash = userInfo[j].Cash,
                        chatSum = userInfo[j].ChatSum,
                        gameSum = userInfo[j].GameSum,
                        point = userInfo[j].Point,
                        state = stateStr
                    });
                }
            }
            col1.Header = "帐号";
            col2.Header = "称号";
            col3.Header = "元宝";
            col4.Header = "聊天";
            col5.Header = "游戏";
            col6.Header = "积分";
            col7.Header = "状态";
            

            memberList.Children.Add(memberGrid);
		}

        void dataGrid_LoadingRow(object sender, Microsoft.Windows.Controls.DataGridRowEventArgs e)
        {
            // Get the DataRow corresponding to the DataGridRow that is loading.
            ListData item = e.Row.Item as ListData;
            if (item != null)
            {
                if (item.state == "新帐号")
                    e.Row.Foreground = new SolidColorBrush(Colors.Blue);
            }
        }

        private class ListData
        {
            public string id { set; get; }
            public string nicName { set; get; }
            public int cash { set; get; }
            public int chatSum { set; get; }
            public int gameSum { set; get; }
            public int point { set; get; }
            public string state { set; get; }
            public string pingDate { set; get; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1._ClientEngine.Send(ChatEngine.NotifyType.Request_NewID, Window1._UserInfo);
            //RequestId requestId = new RequestId();
            //requestId.ShowDialog();
        }
	}
}