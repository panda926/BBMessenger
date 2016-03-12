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

using ControlExs;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for WomanUserControl.xaml
    /// </summary>
    public partial class WomanUserControl : UserControl
    {
        public string _userId = null;
        //ChattingCall chattingCall = null;
        public int index = 0;
        //private bool chatState = true;
        public bool popupState = true;
        UserDetailPopup userDetailPopup = null;
        UserInfo getUserInfo = null;//topUserList userList

        public Storyboard sb = null;

        public WomanUserControl()
        {
            InitializeComponent();
        }

        public void InitStoryBoard()
        {
            Duration duration = new Duration(TimeSpan.FromMilliseconds(500));
            ThicknessAnimation animation = new ThicknessAnimation();
            animation.Duration = duration;

            sb = new Storyboard();
            sb.Children.Add(animation);

            Storyboard.SetTarget(animation, grdUserIcon);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            Storyboard.SetTargetName(animation, "grdUserIcon");
            animation.To = new Thickness(0, 0, 0, 4);
            
            try
            {
                Storyboard storyboard = this.FindResource("shake") as Storyboard;
            }
            catch (Exception)
            {
                this.Resources.Add("shake", sb);
            }
            
            sb.RepeatBehavior = RepeatBehavior.Forever;
        }

        public void UserListGrid(UserInfo userInfo)
        {
            Main.selectUserInfo = userInfo;
            ImageBrush iconUser = new ImageBrush();
            iconUser.Stretch = Stretch.Fill;
            iconUser.ImageSource = ImageDownloader.GetInstance().GetImage(userInfo.Icon);
            //userIcon.Fill = iconUser;            
            //userIcon.Background = iconUser;            
            brUserIcon.Background = iconUser;

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
            

            //ToolTip nameToolTip = new ToolTip();
            //nameToolTip.Content = userInfo.Id + "   " + "(" + userInfo.Nickname + ")";
            //userNick.ToolTip = nameToolTip;
            //userNick.Content = userInfo.Id + "   " + "(" + userInfo.Nickname + ")";
            //userNick.Content = userInfo.Nickname + "   " + "(" + userInfo.Id + ")";
            userNick.Content = userInfo.Nickname;

            ToolTip singToolTip = new ToolTip();
            singToolTip.Content = userInfo.Sign;
            //userSign.ToolTip = singToolTip;
            userSign.Content = userInfo.Sign;
            //userVisite.Content = userInfo.Visitors.ToString();

            _userId = userInfo.Id;

            BitmapImage bi = new BitmapImage();
            switch (userInfo.nUserState)
            {
                case 0:
                    {
                        bi.BeginInit();
                        bi.UriSource = new Uri("/Resources;component/Icons/imonline.ico", UriKind.RelativeOrAbsolute);
                        bi.EndInit();

                        imgUserState.Source = bi;
                    }
                    break;

                case 1:
                    {
                        bi.BeginInit();
                        bi.UriSource = new Uri("/Resources;component/Icons/imoffline.ico", UriKind.RelativeOrAbsolute);
                        bi.EndInit();

                        imgUserState.Source = bi;
                    }
                    break;

                case 2:
                    {
                        bi.BeginInit();
                        bi.UriSource = new Uri("/Resources;component/Icons/busy.ico", UriKind.RelativeOrAbsolute);
                        bi.EndInit();

                        imgUserState.Source = bi;
                    }
                    break;

                case 3:
                    {
                        bi.BeginInit();
                        bi.UriSource = new Uri("/Resources;component/Icons/away.ico", UriKind.RelativeOrAbsolute);
                        bi.EndInit();

                        imgUserState.Source = bi;
                    }
                    break;
            }
        }


        private void userGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            //stateGrid.Visibility = Visibility.Visible;
            userGrid.Background = (System.Windows.Media.LinearGradientBrush)FindResource("GradientColor");
            //userGrid.Background = new SolidColorBrush(Colors.LightSkyBlue);
            Login.selectInfo = Main.selectUserInfo;
            //Login._ClientEngine.Send(NotifyType.Request_PartnerDetail, Main.selectUserInfo);
        }

        private void userGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            //stateGrid.Visibility = Visibility.Hidden;
            userGrid.Background = new SolidColorBrush(Colors.Transparent);
            if (userDetailPopup != null)
            {
                userDetailPopup.Close();
                popupState = true;
            }
        }

        private UserInfo GetUserInfoById(string id_call)
        {
            
            for (int i = 0; i < Login.userList.Count; i++)
            {
                if (Login.userList[i].Id == id_call)
                {
                    getUserInfo = Login.userList[i];
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
            //if (Main.chatRoom != null)
            //{                
            //    TempWindowForm tempWindowForm = new TempWindowForm();
            //    QQMessageBox.Show(tempWindowForm, "현재 채팅을 종료하셔야 합니다.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
            //    return;
            //}
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
                //Main.selectUserHome = new SelectUserHome();
                //Main.selectUserHome.selUserDetail = Login.selectUserDetail;                
                //Main.selectUserHome.InitTabSetting(Main.selectUserInfo);
                //Main.selectUserHome.Show();

                Login._ClientEngine.Send(NotifyType.Request_PartnerDetail, Main.selectUserInfo);
            }
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sb != null)
            {
                sb.Stop();
                sb = null;
            }

            if (Login._UserInfo.Cash <= 0)
            {
                //TempWindowForm tempWindowForm = new TempWindowForm();
                //QQMessageBox.Show(tempWindowForm, "没有钱", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                //return;
            }

            //Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            //{
            //    if (Main.loadWnd == null)
            //    {
            //        Main.loadWnd = new LoadingWnd();
            //        Main.loadWnd.Owner = GetParentWindow(this);
            //        Main.loadWnd.ShowDialog();
            //    }
            //}));

            //if (Login._UserInfo.nVIP == 1)
            {
                Login._AskChatInfo.AskingID = Login._UserInfo.Id;
                Login._AskChatInfo.TargetId = _userId;
                Login._AskChatInfo.Agree = 1;
                Login._ClientEngine.Send(NotifyType.Reply_EnterMeeting, Login._AskChatInfo);
            }
            //else
            //{
            //    TempWindowForm tempWindowForm = new TempWindowForm();
            //    QQMessageBox.Show(tempWindowForm, "只限vip客户使用权限", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Main.selectUserHome == null)
            {
                //Main.selectUserHome = new SelectUserHome();
                //Main.selectUserHome.selUserDetail = Login.selectUserDetail;                
                //Main.selectUserHome.InitTabSetting(Main.selectUserInfo);
                //Main.selectUserHome.Show();
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                {
                    if (Main.loadWnd == null)
                    {
                        Main.loadWnd = new LoadingWnd();
                        Main.loadWnd.Owner = GetParentWindow(this);
                        Main.loadWnd.ShowDialog();
                    }
                }));

                Main.selectUserInfo = GetUserInfoById(_userId);
                Login._ClientEngine.Send(NotifyType.Request_PartnerDetail, Main.selectUserInfo);
            }
        }

        public static Window GetParentWindow(DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            Window parent = parentObject as Window;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return GetParentWindow(parentObject);
            }
        }

        // 2014-04-05: GreenRose
        // 영상채팅단추를눌렀을경우.
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UserInfo userInfo = GetUserInfoById(_userId);

            if (userInfo.nVIP == 0)
            {
                if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman && Login._UserInfo.nVIP != 1)
                {
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "只限vip客户使用权限", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);

                    return;
                }
            }

            try
            {                
                if (userInfo.nUserState == 20)
                {
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "1:1聊天进行中...", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);

                    return;
                }

                if (CheckOpened("TempSoloVideoForm"))
                {
                    //GG.SoloVideoForm soloVideoForm = new GG.SoloVideoForm(Login._UserInfo.Id, _userId, "", false, Main._main);
                    ManVideoFormcs soloVideoForm = new ManVideoFormcs(Login._UserInfo.Id, _userId, "", false, Main._main);
                    soloVideoForm.Text = _userId;
                    soloVideoForm.Tag = "TempSoloVideoForm";
                    soloVideoForm.Show();
                }
            }
            catch (Exception)
            { }
        }

        // 2014-03-29: GreenRose
        // Check videoForm is opened
        private bool CheckOpened(string strTag)
        {
            System.Windows.Forms.FormCollection fc = System.Windows.Forms.Application.OpenForms;

            foreach (System.Windows.Forms.Form frm in fc)
            {
                if (frm.Tag.ToString() == strTag)
                {
                    frm.Close(); // 폼을 클로즈한다.                    
                }
            }

            return true;
        }
    }
}
