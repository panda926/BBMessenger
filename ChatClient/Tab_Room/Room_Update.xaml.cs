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

using ControlExs;

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
            Login._ClientEngine.AttachHandler(NotifyOccured);
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_UpdateRoom:
                    {
                        this.Close();
                        Login._ClientEngine.Send(NotifyType.Request_RoomList, Login._RoomInfo);
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

        public void SetRoom(string roomId)
        {
            for (int i = 0; i < Login.allRoomlist.Count; i++)
            {
                if (roomId == Login.allRoomlist[i].Id)
                {
                    roomBox.Text = Login.allRoomlist[i].Name;
                    selectRoomId = Login.allRoomlist[i].Id;
                    //roomIdLabel.Content = Login.allRoomlist[i].Id;
                    //roomKind.SelectedIndex = Login.allRoomlist[i].Kind;
                    //roomPrice.Text = Login.allRoomlist[i].Cash.ToString();
                    //maxUser.Text = Login.allRoomlist[i].MaxUsers.ToString();
                    roomPass.Password = Login.allRoomlist[i].RoomPass;
                }
            }
                
        }

        private void Update_Room(object sender, RoutedEventArgs e)
        {
            // IniFileEdit iniFileEdit = new IniFileEdit(Login._UserPath);
            // string strPrice = System.Text.RegularExpressions.Regex.Replace(roomPrice.Text, @"[0-9]", "");
            // string strMaxUser = System.Text.RegularExpressions.Regex.Replace(maxUser.Text, @"[0-9]", "");
            //if (strPrice.Length > 0 && strMaxUser.Length > 0)
            //    MessageBoxCommon.Show(iniFileEdit.GetIniValue("StringMessageBox", "Invalid_Number"), MessageBoxType.Ok);
            //else
            //{
            if (roomPass.Password == veriPass.Password)
            {
                Login._RoomInfo.Name = roomBox.Text; //방이름
                Login._RoomInfo.Id = selectRoomId;
                Login._RoomInfo.Kind = 0;
                //if (roomPrice.Text == "")
                Login._RoomInfo.Cash = 0;
                //else
                //    Login._RoomInfo.Cash = Convert.ToInt32(roomPrice.Text);

                //if (maxUser.Text == "")
                Login._RoomInfo.MaxUsers = 0;
                //else
                //    Login._RoomInfo.MaxUsers = Convert.ToInt32(maxUser.Text);
                Login._RoomInfo.Owner = roomUpowner;
                Login._RoomInfo.RoomPass = roomPass.Password;
                Login._ClientEngine.Send(NotifyType.Request_UpdateRoom, Login._RoomInfo);
                this.Close();
                //}
            }
            else
            {                
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "암호가 일치하지 않습니다.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
            }
        }
        
    }
}
