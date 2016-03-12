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
using System.Windows.Shapes;
using ChatEngine;
using System.Net.Sockets;

using ControlExs;

namespace ChatClient
{
	/// <summary>
	/// MyHome.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MyHome : BaseWindow
	{
        public List<ChargeHistoryInfo> chargeHistoryList = null;
        public List<ChatHistoryInfo> chatHistoryList = null;
        public List<EvalHistoryInfo> evalHistoryList = null;
        public List<IconInfo> iconInfoList = null;
        public List<GameHistoryInfo> gameHistoryList = null;
        public List<UserInfo> userInfoList = null;
        public List<PresentHistoryInfo> presentHistoryList = null;
        public List<RoomInfo> roomInfoList = null;

        PictureControl pictureCtrl = null;
       
		public MyHome()
		{
			this.InitializeComponent();
            // 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.

		}
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Login._ClientEngine.AttachHandler(NotifyOccured);

            listBox1.SelectedIndex = 0;
            selectGrid.Children.Clear();
            MyInfoControl myInfoControl = new MyInfoControl();
            myInfoControl.InitMyInfo(Login._UserInfo);
            selectGrid.Children.Add(myInfoControl);
            
        }
        public void InitTabSetting(UserInfo userInfo)
        {
            
            ImageBrush faceImage = new ImageBrush();
            faceImage.Stretch = Stretch.Fill;
            faceImage.ImageSource = ImageDownloader.GetInstance().GetImage(userInfo.Icon);
            myPicture.Fill = faceImage;
            //myPicture.Cursor = Cursors.Hand;

            ListBoxItem item1 = new ListBoxItem();
            item1.Content = "我的信息";
            item1.FocusVisualStyle = null;
            item1.MouseUp += new MouseButtonEventHandler(item1_MouseUp);

            ListBoxItem item2 = new ListBoxItem();
            item1.FocusVisualStyle = null;
            if(Login._UserInfo.Kind  == (int)UserKind.ServiceWoman)
                item2.Content = "相 册";
            else
                item2.Content = "更换头像";
            item2.MouseUp += new MouseButtonEventHandler(item2_MouseUp);

            ListBoxItem item3 = new ListBoxItem();
            item3.Content = "修改密码";
            item3.MouseUp += new MouseButtonEventHandler(item3_MouseUp);

            //ListBoxItem item4 = new ListBoxItem();
                     //item4.Content = "会员信息";
            //item4.MouseUp += new MouseButtonEventHandler(item4_MouseUp);

            ListBoxItem item6 = new ListBoxItem();
            item6.Content = "视频录制";
            item6.MouseUp+=new MouseButtonEventHandler(item4_1_MouseUp);

            ListBoxItem item5 = new ListBoxItem();
            item5.Content = "视频设置";
            item5.MouseUp += new MouseButtonEventHandler(item5_MouseUp);

            ListBoxItem item7 = new ListBoxItem();
            item1.FocusVisualStyle = null;
            item7.Content = "更换头像";
            item7.MouseUp += new MouseButtonEventHandler(item7_MouseUp);

            listBox1.Items.Add(item1);
            listBox1.Items.Add(item3);
            listBox1.Items.Add(item2);
            if (userInfo.Kind == (int)UserKind.ServiceWoman)
            {
                //listBox1.Items.Add(item3);
                //listBox1.Items.Add(item6);
                listBox1.Items.Add( item7 );
            }
            listBox1.Items.Add(item5);
            //listBox1.Items.Add(item4);
            //listBox1.Items.Add(item5);
            
        }

        void item5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectGrid.Children.Clear();
            Home_MyInfo.WebCamSetting webCamSetting = new Home_MyInfo.WebCamSetting();            
            selectGrid.Children.Add(webCamSetting);
        }

        void item4_1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectGrid.Children.Clear();
            PictureControl innerControl = new PictureControl();
            innerControl.myHome = this;
            innerControl.InitVideoList(iconInfoList);
            selectGrid.Children.Add(innerControl);
        }

        void item4_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectGrid.Children.Clear();
            NoticeMemberControl noticeMemberControl = new NoticeMemberControl();
            noticeMemberControl.listMember(userInfoList);
            selectGrid.Children.Add(noticeMemberControl);
        }

        void item3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectGrid.Children.Clear();
            //LetterControl letterControl = new LetterControl();
            //letterControl.markInfo(evalHistoryList);
            //selectGrid.Children.Add(letterControl);

            ChatClient.Home_MyInfo.ChangePassword usCtrlChangePass = new Home_MyInfo.ChangePassword();
            selectGrid.Children.Add(usCtrlChangePass);
        }

        void item2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectGrid.Children.Clear();
            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {
                AlbermControl albermControl = new AlbermControl();
                albermControl.pictureList(iconInfoList);
                selectGrid.Children.Add(albermControl);
            }
            else
            {
                UserIconControl userIconControl = new UserIconControl();
                //userIconControl.IconList();

                selectGrid.Children.Add(userIconControl);
            }
        }

        void item7_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectGrid.Children.Clear();
            UserIconControl userIconControl = new UserIconControl();

            selectGrid.Children.Add(userIconControl);
        }

        void item1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectGrid.Children.Clear();
            MyInfoControl myInfoControl = new MyInfoControl();
            myInfoControl.InitMyInfo(Login._UserInfo);
            selectGrid.Children.Add(myInfoControl);
        }
        
        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
               case NotifyType.Reply_UpdateUser:
                    {
                        UserInfo userName = (UserInfo)baseInfo;
                        Login.main.nickName.Content = userName.Nickname;

                        ToolTip tt = new ToolTip();
                        tt.Content = userName.Nickname;
                        Login.main.nickName.ToolTip = tt;

                        IniFileEdit _IniFileEdit = new IniFileEdit(Login._UserPath);

                        ImageBrush updateImg = new ImageBrush();
                        updateImg.Stretch = Stretch.Fill;
                        _IniFileEdit.SetIniValue("UserInfo", "userImage", userName.Icon);

                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.UriSource = new Uri(userName.Icon, UriKind.RelativeOrAbsolute);
                        bi.EndInit();

                        //updateImg.ImageSource = bi;
                        updateImg.ImageSource = ImageDownloader.GetInstance().GetImage(userName.Icon);
                        myPicture.Fill = updateImg;
                        Login.main.memberImg.Fill = updateImg;

                        ToolTip sign = new ToolTip();
                        sign.Content = Login._UserInfo.Sign;
                        Login.main.singBox.ToolTip = sign;
                        //Login.main.singBox.Text = Login._UserInfo.Sign;

                        if (selectGrid.Children.Count > 0 && selectGrid.Children[0] is MyInfoControl)
                        {
                            MyInfoControl myInfoControl = (MyInfoControl)selectGrid.Children[0];

                            myInfoControl.buttonSave.IsEnabled = false;
                        }

                        //MessageBoxCommon.Show("更新成功", MessageBoxType.Ok);
                        TempWindowForm tempWindowForm = new TempWindowForm();
                        QQMessageBox.Show(tempWindowForm, "更新成功", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                    }
                    break;

               case NotifyType.Request_VideoUpload:
                    {
                        // 2014-02-09: GreenRose
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        //List<IconInfo> listVideoInfo = new List<IconInfo>();
                        IconInfo newIcon = (IconInfo)baseInfo;
                        //iconInfoList.Add(newIcon);
                        if (iconInfoList != null)
                        {
                            iconInfoList.Add(newIcon);
                        }
                        else
                        {
                            iconInfoList = new List<IconInfo>();
                            iconInfoList.Add(newIcon);
                        }

                        selectGrid.Children.Clear();
                        PictureControl pictureControl = new PictureControl();
                        pictureControl.InitVideoList(iconInfoList);
                        selectGrid.Children.Add(pictureControl);

                        TempWindowForm tempWindowForm = new TempWindowForm();
                        QQMessageBox.Show(tempWindowForm, "更新成功", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                    }
                    break;
              
               //case NotifyType.Reply_NewID:
               //     {
               //         UserInfo newUserInfo = (UserInfo)baseInfo;                        
               //         TempWindowForm tempWindowForm = new TempWindowForm();
               //         QQMessageBox.Show(tempWindowForm, "帐号申请通过." + newUserInfo.Id + "(首次登陆没有密码 登陆后请完善资料)", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
               //         userInfoList.Add(newUserInfo);

               //         selectGrid.Children.Clear();
               //         NoticeMemberControl noticeMemberControl = new NoticeMemberControl();
               //         noticeMemberControl.listMember(userInfoList);
               //         selectGrid.Children.Add(noticeMemberControl);
               //     }
               //     break;
               case NotifyType.Reply_IconUpload:
                    {
                        IconInfo newIcon = (IconInfo)baseInfo;

                        IconInfo iconInfo = iconInfoList.Find(item => item.Id == newIcon.Id);
                        if (iconInfo == null)
                        {
                            iconInfoList.Add(newIcon);

                            selectGrid.Children.Clear();
                            AlbermControl albermControl = new AlbermControl();
                            albermControl.pictureList(iconInfoList);
                            selectGrid.Children.Add(albermControl);
                        }
                    }
                    break;
               case NotifyType.Reply_Give:
                    {
                        PresentHistoryInfo presentHistoryInfo = (PresentHistoryInfo)baseInfo;
                        
                        presentHistoryList.Add(presentHistoryInfo);
                        selectGrid.Children.Clear();
                        InnerControl innerControl = new InnerControl();
                        innerControl.InnerChatting(chatHistoryList);
                        selectGrid.Children.Add(innerControl);

                        

                        for (int i = 0; i < userInfoList.Count; i++)
                        {
                            if (userInfoList[i].Id == presentHistoryInfo.ReceiveId)
                            {
                                userInfoList[i].Cash = userInfoList[i].Cash + presentHistoryInfo.Cash;

                                selectGrid.Children.Clear();
                                NoticeMemberControl noticeMemberControl = new NoticeMemberControl();
                                noticeMemberControl.listMember(userInfoList);
                                selectGrid.Children.Add(noticeMemberControl);

                                break;
                            }
                        }
                    }
                    break;
               case NotifyType.Reply_UpdatePercent:
                    {
                        UserInfo userPercent = (UserInfo)baseInfo;
                        if (userPercent.Id == Login._UserInfo.Id)
                        {
                            selectGrid.Children.Clear();
                            MyInfoControl myInfoControl = new MyInfoControl();
                            myInfoControl.InitMyInfo(Login._UserInfo);
                            selectGrid.Children.Add(myInfoControl);
                        }
                    }
                    break;
               case NotifyType.Reply_IconRemove:
                    {
                        IconInfo newIcon = (IconInfo)baseInfo;
                        for (int i = 0; i < iconInfoList.Count; i++)
                        {
                            if (iconInfoList[i].Icon == newIcon.Icon)
                            {
                                iconInfoList.Remove(iconInfoList[i]);
                            }

                        }

                        selectGrid.Children.Clear();
                        AlbermControl albermControl = new AlbermControl();
                        albermControl.pictureList(iconInfoList);
                        selectGrid.Children.Add(albermControl);
                    }
                    break;
               case NotifyType.Reply_Error:
                    {
                        ResultInfo errorInfo = (ResultInfo)baseInfo;
                        ErrorType errorType = errorInfo.ErrorType;

                        //Login.ShowError(errorType);
                    }
                    break;
            }
        }
        

        private void Window_Closed(object sender, EventArgs e)
        {
            //if (pictureCtrl != null)
            //{
            //    if (pictureCtrl.localWebCam != null)
            //    {
            //        if (pictureCtrl.localWebCam.IsRunning)
            //            pictureCtrl.localWebCam.Stop();
            //        pictureCtrl.localWebCam = null;
            //    }
            //}

            Login.myhome = null;
            Login._ClientEngine.DetachHandler(NotifyOccured);
            this.Close();
        }

        private void myPicture_MouseUp(object sender, MouseButtonEventArgs e)
        {
            listBox1.SelectedIndex = 1;

            item2_MouseUp(null, null);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        
       
	}
}