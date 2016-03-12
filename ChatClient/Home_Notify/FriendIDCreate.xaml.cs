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
using System.Windows.Shapes;

using ChatEngine;
using System.Net.Sockets;
using ControlExs;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for FriendIDCreate.xaml
    /// </summary>
    public partial class FriendIDCreate : BaseWindow
    {
        public List<UserInfo> userInfoList = null;
        public string _strDownloadUrl = string.Empty;

        public FriendIDCreate()
        {
            InitializeComponent();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Login.friendIDCreate = null;
            this.Close();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Login._ClientEngine.AttachHandler(NotifyOccured);

            NoticeMemberControl noticeMemberControl = new NoticeMemberControl();
            noticeMemberControl._strDownloadUrl = _strDownloadUrl;
            noticeMemberControl.listMember(userInfoList);            
            grdFriendIDCreate.Children.Clear();
            grdFriendIDCreate.Children.Add(noticeMemberControl);
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_NewID:
                    {
                        UserInfo newUserInfo = (UserInfo)baseInfo;
                        TempWindowForm tempWindowForm = new TempWindowForm();
                        QQMessageBox.Show(tempWindowForm, "帐号申请通过." + newUserInfo.Id + "(首次登陆没有密码 登陆后请完善资料)", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                        userInfoList.Add(newUserInfo);

                        grdFriendIDCreate.Children.Clear();
                        NoticeMemberControl noticeMemberControl = new NoticeMemberControl();
                        noticeMemberControl.listMember(userInfoList);
                        grdFriendIDCreate.Children.Add(noticeMemberControl);
                    }
                    break;

                case NotifyType.Reply_Give:
                    {
                        PresentHistoryInfo presentHistoryInfo = (PresentHistoryInfo)baseInfo;

                        for (int i = 0; i < userInfoList.Count; i++)
                        {
                            if (userInfoList[i].Id == presentHistoryInfo.ReceiveId)
                            {
                                userInfoList[i].Cash = userInfoList[i].Cash + presentHistoryInfo.Cash;

                                grdFriendIDCreate.Children.Clear();
                                NoticeMemberControl noticeMemberControl = new NoticeMemberControl();
                                noticeMemberControl.listMember(userInfoList);
                                grdFriendIDCreate.Children.Add(noticeMemberControl);

                                break;
                            }
                        }
                    }
                    break;
            }
        }
    }
}
