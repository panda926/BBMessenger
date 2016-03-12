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
                    Main.SelectUserInfoList.Add(SelectUserInfo);
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
                DataGrid memberGrid = new DataGrid();
                memberGrid.MouseUp += new MouseButtonEventHandler(memberGrid_MouseUp);
                DataGridTextColumn col1 = new DataGridTextColumn();
                DataGridTextColumn col2 = new DataGridTextColumn();
                DataGridTextColumn col3 = new DataGridTextColumn();
                DataGridTextColumn col4 = new DataGridTextColumn();
                DataGridTextColumn col5 = new DataGridTextColumn();
                DataGridTextColumn col6 = new DataGridTextColumn();
                DataGridTextColumn col7 = new DataGridTextColumn();

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
                        memberGrid.Items.Add(new ListData
                        {
                            id = user.Partners[j].Id,
                            nicName = user.Partners[j].Nickname,
                            cash = user.Partners[j].Cash,
                            chatSum = user.Partners[j].ChatSum,
                            gameSum = user.Partners[j].GameSum,
                            point = user.Partners[j].Point,
                            state = user.Partners[j].LoginTime
                        });
                    }
                }
                col1.Header = "아이디";
                col2.Header = "닉네임";
                col3.Header = "보유캐쉬";
                col4.Header = "채팅";
                col5.Header = "게임";
                col6.Header = "포인트";
                col7.Header = "상태";


                memberList.Children.Add(memberGrid);
           
		}

        void memberGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (((ListData)grid.SelectedItem).state == "")
            {
                MessageBox.Show("아직 등록되지 않은 사용자입니다.*");
            }
            else
            {
                Main.selectInfo = Main.myhome.GetByUserInfo(((ListData)grid.SelectedItem).id);
                Main._ClientEngine.Send(NotifyType.Request_PartnerDetail, Main.selectInfo);
            }
            
        }
              
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