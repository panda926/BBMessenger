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

        public string _strDownloadUrl = string.Empty;

        public NoticeMemberControl()
        {
            this.InitializeComponent();
        }
        private void userList(List<UserInfo> userInfoList)
        {
            Login.SelectUserInfoList.Clear();
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
                Login.SelectUserInfoList.Add(SelectUserInfo);
            }
            
        }
        public void listMember(List<UserInfo> userInfo)
		{
            this.userList(userInfo);

            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman || Login._UserInfo.Kind == (int)UserKind.Buyer)
            {
                label1.Visibility = Visibility.Hidden;
                buyers.Visibility = Visibility.Hidden;
            }
            else if (Login._UserInfo.Kind == (int)UserKind.Recommender)
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
            buyers.Content = (Login._UserInfo.MaxBuyers - userInfo.Count).ToString();
            reqBuyer.Content = userInfo.Count.ToString();


            Microsoft.Windows.Controls.DataGrid memberGrid = new Microsoft.Windows.Controls.DataGrid();

            memberGrid.Width = 390;
            memberGrid.Height = 230;
            memberGrid.HeadersVisibility = Microsoft.Windows.Controls.DataGridHeadersVisibility.Column;
            memberGrid.GridLinesVisibility = Microsoft.Windows.Controls.DataGridGridLinesVisibility.Horizontal;

            memberGrid.IsReadOnly = true;
            memberGrid.Style = (Style)FindResource("DataGridStyle");
            memberGrid.CellStyle = (Style)FindResource("cellStyle");
            memberGrid.RowStyle = (Style)FindResource("rowStyle");
            memberGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
            memberGrid.Background = new SolidColorBrush(Colors.Transparent);

            memberGrid.SelectionMode = Microsoft.Windows.Controls.DataGridSelectionMode.Single;
            memberGrid.SelectionUnit = Microsoft.Windows.Controls.DataGridSelectionUnit.FullRow;
            memberGrid.LoadingRow += dataGrid_LoadingRow;

            //memberGrid.MouseLeftButtonUp += new MouseButtonEventHandler(memberGrid_MouseUp);
            Microsoft.Windows.Controls.DataGridTextColumn col1 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col1.Width = 100;
            Microsoft.Windows.Controls.DataGridTextColumn col2 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col2.Width = 150;
            //Microsoft.Windows.Controls.DataGridTextColumn col3 = new Microsoft.Windows.Controls.DataGridTextColumn();
            //col3.Width = 55;
            //Microsoft.Windows.Controls.DataGridTextColumn col4 = new Microsoft.Windows.Controls.DataGridTextColumn();
            //col4.Width = 55;
            //Microsoft.Windows.Controls.DataGridTextColumn col5 = new Microsoft.Windows.Controls.DataGridTextColumn();
            //col5.Width = 55;
            //Microsoft.Windows.Controls.DataGridTextColumn col6 = new Microsoft.Windows.Controls.DataGridTextColumn();
            //col6.Width = 40;
            Microsoft.Windows.Controls.DataGridTextColumn col7 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col7.Width = new Microsoft.Windows.Controls.DataGridLength(57, Microsoft.Windows.Controls.DataGridLengthUnitType.Star);
            
            
            memberGrid.Columns.Add(col1);
            memberGrid.Columns.Add(col2);
            //memberGrid.Columns.Add(col3);
            //memberGrid.Columns.Add(col4);
            //memberGrid.Columns.Add(col5);
            //memberGrid.Columns.Add(col6);
            memberGrid.Columns.Add(col7);
            

            col1.Binding = new Binding("id");
            col2.Binding = new Binding("nicName");
            //col3.Binding = new Binding("cash");
            //col4.Binding = new Binding("chatSum");
            //col5.Binding = new Binding("gameSum");
            //col6.Binding = new Binding("point");
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
                        //cash = userInfo[j].Cash,
                        //chatSum = userInfo[j].ChatSum,
                        //gameSum = userInfo[j].GameSum,
                        //point = userInfo[j].Point,
                        state = stateStr
                    });
                }
            }
            col1.Header = "帐号";
            col2.Header = "称号";
            //col3.Header = "元宝";
            //col4.Header = "聊天";
            //col5.Header = "游戏";
            //col6.Header = "积分";
            col7.Header = "状态";

            memberGrid.Margin = new Thickness(5);
            memberList.Children.Add(memberGrid);

            // 2014-03-24: 그린로즈
            //this.linkSiteUrl.NavigateUri = new Uri(_strDownloadUrl, UriKind.RelativeOrAbsolute);
            this.txtDownloadLink.Text = _strDownloadUrl;
            
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
            Login._ClientEngine.Send(ChatEngine.NotifyType.Request_NewID, Login._UserInfo);
            //RequestId requestId = new RequestId();
            //requestId.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDownloadLink.Text.Trim() == string.Empty)
                    return;

                Clipboard.SetText(txtDownloadLink.Text);
            }
            catch (System.Exception)
            {
            	
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtDownloadLink.Focus();
            this.txtDownloadLink.SelectAll();            
        }

        //private void linkSiteUrl_RequestNavigate(object sender, RequestNavigateEventArgs e)
        //{
        //    try
        //    {
        //        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.OriginalString));
        //        e.Handled = true;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        string strErrorMsg = ex.ToString();
        //    }
        //}
	}
}