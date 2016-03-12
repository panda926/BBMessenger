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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for RoomUserControl.xaml
    /// </summary>
    public partial class RoomUserControl : UserControl
    {
        private string _roomId = null;
        private string _ownerId = null;
        private string _roomPass = null;
        private bool _roomGrid = false;
        private RoomInfo _roomInfo = null;

        public RoomUserControl()
        {
            InitializeComponent();
        }

        public void RoomListGrid(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;
            if (roomInfo.Point == 0)
            {
                if (Window1._UserInfo.Kind == (int)UserKind.ServiceWoman)
                {
                    BitmapImage roomBit = new BitmapImage();
                    roomBit.BeginInit();
                    roomBit.UriSource = new Uri("image/default.gif", UriKind.RelativeOrAbsolute);
                    roomBit.EndInit();
                    roomIcon.Source = roomBit;
                }
                else
                {
                    for (int i = 0; i < Window1.userList.Count; i++)
                    {
                        if (Window1.userList[i].Id == roomInfo.Owner)
                        {
                            roomIcon.Source = ImageDownloader.GetInstance().GetImage(Window1.userList[i].Icon);
                            break;
                        }
                    }
                }


                _roomPass = roomInfo.RoomPass;
                _roomId = roomInfo.Id;
                _ownerId = roomInfo.Owner;

                ToolTip roomToolTip = new ToolTip();
                roomToolTip.Content = roomInfo.Name + "(" + roomInfo.Id + ")";
                roomName.ToolTip = roomToolTip;

                roomName.Content = roomInfo.Name;
                roomCount.Content = roomInfo.UserCount.ToString();
                //roomCount.Content = roomInfo.UserCount.ToString() + "/" + roomInfo.MaxUsers.ToString();
                //roomValue.Content = roomInfo.Cash.ToString();
            }
            else
            {
                BitmapImage roomBit = new BitmapImage();
                roomBit.BeginInit();
                roomBit.UriSource = new Uri("image/pointRoom.gif", UriKind.RelativeOrAbsolute);
                roomBit.EndInit();
                roomIcon.Source = roomBit;

                _roomId = roomInfo.Id;
                _ownerId = roomInfo.Id;

                ToolTip roomToolTip = new ToolTip();
                roomToolTip.Content = roomInfo.Name + "(" + roomInfo.Id + ")";
                roomName.ToolTip = roomToolTip;

                roomName.Content = roomInfo.Name;
                roomCount.Content = roomInfo.UserCount.ToString();
                //roomCount.Content = roomInfo.UserCount.ToString() + "/" + roomInfo.MaxUsers.ToString();
                //roomValue.Content = roomInfo.Point;
            }
        }

       
        private void roomModify_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_roomGrid == true)
            {
                _roomGrid = true;
                Room_Update updateRoom = new Room_Update();
                updateRoom.roomUpowner = _ownerId;
                updateRoom.SetRoom(_roomId);
                updateRoom.ShowDialog();
            }
        }

        private void roomDelete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_roomGrid == true)
            {
                _roomGrid = true;
                if (MessageBoxCommon.Show("Are you sure?",  MessageBoxType.YesNo) == MessageBoxReply.No)
                    return;

                _roomInfo.Id = _roomId;
                Window1._ClientEngine.Send(NotifyType.Request_DelRoom, _roomInfo);
            }
        }

        private void roomGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            _roomGrid = false;
            roomGrid.Background = new SolidColorBrush(Colors.LightSkyBlue);
            if (_ownerId == Window1._UserInfo.Id)
                roomSetting.Visibility = Visibility.Visible;
        }

        private void roomGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            roomGrid.Background = new SolidColorBrush(Colors.Transparent);
            if (_ownerId == Window1._UserInfo.Id)
                roomSetting.Visibility = Visibility.Hidden;
        }

        bool _isFirstmousclick = false;


        private void roomSetting_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void roomSetting_MouseEnter(object sender, MouseEventArgs e)
        {
            _roomGrid = true;
        }

        private void roomGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isFirstmousclick == true)
                return;

            _roomInfo.Id = _roomId;
            if (Main._DiceGame != null)
                MessageBoxCommon.Show("请稍候......", MessageBoxType.Ok);

            else
            {
                if (Window1._UserInfo.Kind == (int)UserKind.ServiceWoman)
                {
                    if (Main.multyChatRoom == null && Main.chatRoom == null &&
                        _ownerId == Window1._UserInfo.Id && _roomGrid == false || _roomId == "1000000")
                    {
                        Window1._ClientEngine.Send(NotifyType.Request_EnterRoom, _roomInfo);
                    }
                }
                else
                {
                    
                    //if (MessageBoxCommon.Show("您确定要进入此房间吗？ 방값:" + Window1._RoomInfo.Cash, MessageBoxType.YesNo) == MessageBoxReply.Yes)
                    //{
                        if (_roomPass == null || _roomPass == "")
                            Window1._ClientEngine.Send(NotifyType.Request_EnterRoom, _roomInfo);
                        else
                        {
                            RoomPassVerify roomPassVerify = new RoomPassVerify();
                            roomPassVerify.VerifyRoomInfo(_roomInfo);
                            roomPassVerify.Show();
                        }
                    //}
                    
                }
            }
        }
    }
}
