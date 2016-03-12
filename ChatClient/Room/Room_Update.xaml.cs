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
using System.Net;
using System.Net.Sockets;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for Room_Make.xaml
    /// </summary>
    public partial class Room_Update : Window
    {
        public string roomUpowner = null;
        private string selectRoomId = null;

        public Room_Update()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window1._ClientEngine.AttachHandler(NotifyOccured);
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_UpdateRoom:
                    {
                        this.Close();
                        Window1._ClientEngine.Send(NotifyType.Request_RoomList, Window1._RoomInfo);
                    }
                    break;
                case NotifyType.Reply_Error:
                    {
                        ResultInfo errorInfo = (ResultInfo)baseInfo;
                        ErrorType errorType = errorInfo.ErrorType;

                        //Window1.ShowError(errorType);
                    }
                    break;
            }

        }

        public void SetRoom(string roomId)
        {
            for (int i = 0; i < Window1.allRoomlist.Count; i++)
            {
                if (roomId == Window1.allRoomlist[i].Id)
                {
                    roomBox.Text = Window1.allRoomlist[i].Name;
                    selectRoomId = Window1.allRoomlist[i].Id;
                    //roomIdLabel.Content = Window1.allRoomlist[i].Id;
                    //roomKind.SelectedIndex = Window1.allRoomlist[i].Kind;
                    //roomPrice.Text = Window1.allRoomlist[i].Cash.ToString();
                    //maxUser.Text = Window1.allRoomlist[i].MaxUsers.ToString();
                    roomPass.Password = Window1.allRoomlist[i].RoomPass;
                }
            }
                
        }

        private void Update_Room(object sender, RoutedEventArgs e)
        {
            // IniFileEdit iniFileEdit = new IniFileEdit(Window1._UserPath);
            // string strPrice = System.Text.RegularExpressions.Regex.Replace(roomPrice.Text, @"[0-9]", "");
            // string strMaxUser = System.Text.RegularExpressions.Regex.Replace(maxUser.Text, @"[0-9]", "");
            //if (strPrice.Length > 0 && strMaxUser.Length > 0)
            //    MessageBoxCommon.Show(iniFileEdit.GetIniValue("StringMessageBox", "Invalid_Number"), MessageBoxType.Ok);
            //else
            //{
            if (roomPass.Password == veriPass.Password)
            {
                Window1._RoomInfo.Name = roomBox.Text; //방이름
                Window1._RoomInfo.Id = selectRoomId;
                Window1._RoomInfo.Kind = 0;
                //if (roomPrice.Text == "")
                    Window1._RoomInfo.Cash = 0;
                //else
                //    Window1._RoomInfo.Cash = Convert.ToInt32(roomPrice.Text);

                //if (maxUser.Text == "")
                    Window1._RoomInfo.MaxUsers = 0;
                //else
                //    Window1._RoomInfo.MaxUsers = Convert.ToInt32(maxUser.Text);
                Window1._RoomInfo.Owner = roomUpowner;
                Window1._RoomInfo.RoomPass = roomPass.Password;
                Window1._ClientEngine.Send(NotifyType.Request_UpdateRoom, Window1._RoomInfo);
                this.Close();
                //}
            }
            else
                MessageBoxCommon.Show("암호가 일치하지 않습니다.", MessageBoxType.Ok);
        }
        
    }
}
