using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Media.Animation;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for WomanUserControl.xaml
    /// </summary>
    public partial class WomanUserControl : UserControl
    {
        private string _userId = null;
        //ChattingCall chattingCall = null;
        public int index = 0;
        //private bool chatState = true;
        public bool popupState = true;
        UserDetailPopup userDetailPopup = null;
        UserInfo getUserInfo = null;//topUserList userList

        public WomanUserControl()
        {
            InitializeComponent();
        }
        public void UserListGrid(UserInfo userInfo)
        {
            Main.selectUserInfo = userInfo;
            ImageBrush iconUser = new ImageBrush();
            iconUser.Stretch = Stretch.Fill;

            iconUser.ImageSource = ImageDownloader.GetInstance().GetImage(userInfo.Icon);
            userIcon.Fill = iconUser;


            //if (userInfo.RoomId == "")
            //{
            //    chattingState.Content = "等候";
            //    chattingState.Foreground = new SolidColorBrush(Colors.Green);
            //}
            //else
            //{
            //    chatState = false;
            //    chattingState.Content = "聊天中";
            //    chattingState.Foreground = new SolidColorBrush(Colors.Red);
            //    chatIcon.Opacity = 0.5;
            //    chatIcon.IsEnabled = false;
            //}
            

            ToolTip nameToolTip = new ToolTip();
            nameToolTip.Content = userInfo.Id + "   " + "(" + userInfo.Nickname + ")";
            userNick.ToolTip = nameToolTip;
            userNick.Content = userInfo.Id + "   " + "(" + userInfo.Nickname + ")";

            ToolTip singToolTip = new ToolTip();
            singToolTip.Content = userInfo.Sign;
            userSign.ToolTip = singToolTip;
            userSign.Content = userInfo.Sign;
            userVisite.Content = userInfo.Visitors.ToString();

            _userId = userInfo.Id;
        }


        private void userGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            stateGrid.Visibility = Visibility.Visible;
            userGrid.Background = new SolidColorBrush(Colors.LightSkyBlue);
            Window1.selectInfo = Main.selectUserInfo;
            Window1._ClientEngine.Send(NotifyType.Request_PartnerDetail, Main.selectUserInfo);
        }

        private void userGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            stateGrid.Visibility = Visibility.Hidden;
            userGrid.Background = new SolidColorBrush(Colors.Transparent);
            if (userDetailPopup != null)
            {
                userDetailPopup.Close();
                popupState = true;
            }
        }

        private UserInfo GetUserInfoById(string id_call)
        {
            
            for (int i = 0; i < Window1.userList.Count; i++)
            {
                if (Window1.userList[i].Id == id_call)
                {
                    getUserInfo = Window1.userList[i];
                    break;
                }
            }
            return getUserInfo;
        }

        private void userGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {

            //if (Main.selectUserInfo.RoomId == "" && Main.selectUserInfo.GameId == "")
            //{
            //    UserInfo getUser = GetUserInfoById(_userId);
            //    chattingCall = new ChattingCall();
            //    chattingCall.ChattingCallName(getUser, _userId);
            //    chattingCall.ShowDialog();
            //}
            //else if (Main.selectUserInfo.RoomId != "" && Main.selectUserInfo.GameId != "")
            //{
            //    Main main = new Main();
            //    main.tabControl1.SelectedIndex = 1;
            //}

        }


        private void userIcon_MouseEnter_1(object sender, MouseEventArgs e)
        {

            GetUserInfoById(_userId);
            System.Windows.Point pos = PointToScreen(Mouse.GetPosition(this));
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            Point mousePoint = Mouse.GetPosition(this);
            if (popupState == true)
            {
                popupState = false;
                userDetailPopup = new UserDetailPopup();
                userDetailPopup.selectUserInfo(getUserInfo);
                userDetailPopup.Left = pos.X - userDetailPopup.Width - mousePoint.X - 15;
                userDetailPopup.Top = pos.Y - mousePoint.Y;
                userDetailPopup.Show();
            }
        }

        private void userIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window1._AskChatInfo.TargetId = _userId;
            Window1._AskChatInfo.Agree = 1;
            //Window1._ClientEngine.Send(ChatEngine.NotifyType.Request_GameList, Window1._UserInfo);
            Window1._ClientEngine.Send(NotifyType.Reply_EnterMeeting, Window1._AskChatInfo);

        }

        //private void chatIcon_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (chatState == true && chattingCall == null)
        //    {
        //        UserInfo getUser = GetUserInfoById(_userId);
        //        chattingCall = new ChattingCall();
        //        chattingCall.ChattingCallName(getUser, _userId);
        //        chattingCall.Show();
        //    }
        //}

        //private void paperIcon_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (Main.mPage == null)
        //    {
        //        Main.mPage = new MemoPage();
        //        Main.mPage.sendId = _userId;
        //        Main.mPage.Show();
        //    }
        //}

        private void profilIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Main.selectUserHome == null)
            {
                Main.selectUserHome = new SelectUserHome();
                Main.selectUserHome.selUserDetail = Window1.selectUserDetail;
                Main.selectUserHome.InitTabSetting(Window1.selectInfo);
                Main.selectUserHome.Show();
            }
        }

      
        
    }
}
