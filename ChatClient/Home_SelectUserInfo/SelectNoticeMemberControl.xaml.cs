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
	public partial class SelectNoticeMemberControl : UserControl
	{
        UserInfo SelectUserInfo = null;
        string stateStr = null;
        public SelectNoticeMemberControl()
		{
			this.InitializeComponent();
		}
        private void userList(UserDetailInfo userList)
        {
            if (userList != null)
            {
                for (int i = 0; i < userList.Partners.Count; i++)
                {
                    SelectUserInfo = new UserInfo();
                    SelectUserInfo.Id = userList.Partners[i].Id;
                    SelectUserInfo.Nickname = userList.Partners[i].Nickname;
                    SelectUserInfo.Icon = userList.Partners[i].Icon;
                    SelectUserInfo.Password = userList.Partners[i].Password;
                    SelectUserInfo.Year = userList.Partners[i].Year;
                    SelectUserInfo.Month = userList.Partners[i].Month;
                    SelectUserInfo.Day = userList.Partners[i].Day;
                    SelectUserInfo.Address = userList.Partners[i].Address;
                    SelectUserInfo.Point = userList.Partners[i].Point;
                    SelectUserInfo.Cash = userList.Partners[i].Cash;
                    SelectUserInfo.Evaluation = userList.Partners[i].Evaluation;
                    SelectUserInfo.Visitors = userList.Partners[i].Visitors;
                    Login.SelectUserInfoList.Add(SelectUserInfo);
                }
            }
        }
        public void listMember(UserDetailInfo user)
		{
           
                this.userList(user);
                if (user != null)
                {
                    reqBuyer.Content = user.Partners.Count.ToString();
                }
                Microsoft.Windows.Controls.DataGrid memberGrid = new Microsoft.Windows.Controls.DataGrid();
                memberGrid.Style = (Style)FindResource("DataGridStyle");
                memberGrid.GridLinesVisibility = Microsoft.Windows.Controls.DataGridGridLinesVisibility.Horizontal;
                //memberGrid.MouseLeftButtonUp += new MouseButtonEventHandler(selMemberGrid_MouseUp);
                Microsoft.Windows.Controls.DataGridTextColumn col1 = new Microsoft.Windows.Controls.DataGridTextColumn();
                Microsoft.Windows.Controls.DataGridTextColumn col2 = new Microsoft.Windows.Controls.DataGridTextColumn();
                Microsoft.Windows.Controls.DataGridTextColumn col3 = new Microsoft.Windows.Controls.DataGridTextColumn();
                Microsoft.Windows.Controls.DataGridTextColumn col4 = new Microsoft.Windows.Controls.DataGridTextColumn();
                Microsoft.Windows.Controls.DataGridTextColumn col5 = new Microsoft.Windows.Controls.DataGridTextColumn();
                Microsoft.Windows.Controls.DataGridTextColumn col6 = new Microsoft.Windows.Controls.DataGridTextColumn();
                Microsoft.Windows.Controls.DataGridTextColumn col7 = new Microsoft.Windows.Controls.DataGridTextColumn();

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

                if (user != null)
                {
                    for (int j = 0; j < user.Partners.Count; j++)
                    {
                        if (user.Partners[j].LoginTime == "")
                            stateStr = "New ID";
                        else
                            stateStr = "Old ID";
                        memberGrid.Items.Add(new ListData
                        {
                            id = user.Partners[j].Id,
                            nicName = user.Partners[j].Nickname,
                            cash = user.Partners[j].Cash,
                            chatSum = user.Partners[j].ChatSum,
                            gameSum = user.Partners[j].GameSum,
                            point = user.Partners[j].Point,
                            state = stateStr
                        });
                    }
                }
                col1.Header = "账号";
                col2.Header = "称号";
                col3.Header = "元宝";
                col4.Header = "聊天";
                col5.Header = "游戏";
                col6.Header = "积分";
                col7.Header = "状态";


                memberList.Children.Add(memberGrid);
           
		}

        //void selMemberGrid_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    var grid = sender as Microsoft.Windows.Controls.DataGrid;
        //    if (((ListData)grid.SelectedItem).state == "New ID")
        //    {
        //        MessageBox.Show("아직 등록되지 않은 사용자입니다.*");
        //    }
        //    else
        //    {
        //        Login Login = new Login();
        //        //Login.selectInfo = Login.GetByUserInfo(((ListData)grid.SelectedItem).id);
        //        //Login._ClientEngine.Send(NotifyType.Request_PartnerDetail, Login.selectInfo);

        //        //ContextMenu contextMenu = new ContextMenu();
        //        //MenuItem proitem = new MenuItem();
        //        //proitem.Click += new RoutedEventHandler(proitem_Click);
        //        //proitem.Header = "프로필보기";
        //        //proitem.DataContext = ((ListData)grid.SelectedItem).id;

        //        //MenuItem cashitem = new MenuItem();
        //        //cashitem.Header = "캐쉬보내기";
        //        //cashitem.DataContext = ((ListData)grid.SelectedItem).id;
        //        //cashitem.Click += new RoutedEventHandler(cashitem_Click);

        //        //contextMenu.Items.Add(proitem);
        //        //contextMenu.Items.Add(cashitem);
        //        //contextMenu.IsOpen = true;
        //    }

        //}

        //void cashitem_Click(object sender, RoutedEventArgs e)
        //{
        //    var cashmenuId = sender as MenuItem;
        //    string cashselId = cashmenuId.DataContext.ToString();
        //    SendCash sendCash = new SendCash();
        //    sendCash.InitSetting(cashselId);
        //}

        //void proitem_Click(object sender, RoutedEventArgs e)
        //{
        //    var menuId = sender as MenuItem;
        //    string selId = menuId.DataContext.ToString();
        //    Login.selectInfo = Login.myhome.GetByUserInfo(selId);
        //    Login._ClientEngine.Send(NotifyType.Request_PartnerDetail, Login.selectInfo);
        //}
              
        private struct ListData
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
        
	}
}