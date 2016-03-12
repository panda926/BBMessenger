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
    public partial class Room_Make : Window
    {
        static public RoomListInfo _RoomListInfo = new RoomListInfo();
        public string roomOwner = null;

        public Room_Make()
        {
            InitializeComponent();
            //useId.SelectedIndex = 0;
        }
       
        private void goRegistor_Click(object sender, RoutedEventArgs e)
        {
            //IniFileEdit iniFileEdit = new IniFileEdit(Window1._UserPath);

            
            //string strPrice = System.Text.RegularExpressions.Regex.Replace(roomPass.Password, @"[0-9]", "");
            //string strMaxUser = System.Text.RegularExpressions.Regex.Replace(veriPass.Password, @"[0-9]", "");
            //if (strPrice.Length > 0 || strMaxUser.Length > 0)
            //{
            //    MessageBoxCommon.Show(iniFileEdit.GetIniValue("StringMessageBox", "Invalid_Number"), MessageBoxType.Ok);
            //    return;
            //}
            //else
            //{
                if (roomPass.Password == veriPass.Password)
                {
                    RoomInfo roomInfo = new RoomInfo();
                    roomInfo.Name = roomBox.Text; //방이름
                    Random userId = new Random();
                    roomInfo.Id = userId.Next(11111111, 99999999).ToString(); //방아이디
                    roomInfo.Kind = 0; //방종류
                    //if (price.Text == "")
                    roomInfo.Cash = 0;
                    //else
                    //    roomInfo.Cash = Convert.ToInt32(price.Text);
                    //if (maxUser.Text == "")
                    roomInfo.MaxUsers = 0;
                    //else
                    //    roomInfo.MaxUsers = Convert.ToInt32(maxUser.Text);
                    roomInfo.Owner = roomOwner;
                    roomInfo.RoomPass = roomPass.Password;
                    roomInfo.Icon = "Images\\Room\\default.gif";

                    Window1._ClientEngine.Send(NotifyType.Request_MakeRoom, roomInfo);
                    this.Close();
                }
                else
                    MessageBoxCommon.Show("암호가 일치하지 않습니다.", MessageBoxType.Ok);
            //}
        }

        //private void useId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ComboBoxItem currentItemType = (ComboBoxItem)useId.SelectedItem;
        //    if (currentItemType.Content.Equals("自主选号"))
        //    {
        //        Id_Select id_select = new Id_Select();
        //        id_select.ShowDialog();
        //        if (id_select.DialogResult == true)
        //        {
        //            idLook.Content = id_select.Content;
        //            veri_image.Visibility = Visibility.Visible;
                    
        //        }
        //    }
        //    else
        //    {
        //        idLook.Content = "";
        //        veri_image.Visibility = Visibility.Hidden;
        //    }
        //}

       
    }
}
