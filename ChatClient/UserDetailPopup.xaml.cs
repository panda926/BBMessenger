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
using System.Globalization;
using ChatEngine;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for UserDetailPopup.xaml
    /// </summary>
    public partial class UserDetailPopup : Window
    {
        private UserInfo selecGridtUser = null;
        //private WomanUserControl womanUserControl = null;

        public UserDetailPopup()
        {
            InitializeComponent();
        }

        public void selectUserInfo(UserInfo userinfo)
        {
            selecGridtUser = userinfo;
            Image userImg = new Image();
            //userImg.MouseUp += new MouseButtonEventHandler(userImg_MouseUp);
            userImg.Cursor = Cursors.Hand;
            userImg.Stretch = Stretch.Fill;
           
            userImg.Source = ImageDownloader.GetInstance().GetImage(userinfo.Icon);
            imageGrid.Children.Add(userImg);

            userSign.Text = "[" + userinfo.Id + "]" + " " + userinfo.Nickname;
            userName.Content = userinfo.Nickname;
            ago.Content = (DateTime.Now.Year - userinfo.Year).ToString();
            address.Content = userinfo.Address;

            if (userinfo.Visitors != 0)
                count.Content = (int)((userinfo.Evaluation / (double)userinfo.Visitors) * 100) + "%";
            else
                count.Content = 0 + "%";
            textBlock1.Text = userinfo.Sign;
        }

        //void userImg_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    Main.selectUserInfo = selecGridtUser;
        //    Window1.selectInfo = Main.selectUserInfo;
        //    Window1._ClientEngine.Send(NotifyType.Request_PartnerDetail, Main.selectUserInfo);
        //    this.Close();
        //}

        //private void Window_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    womanUserControl = new WomanUserControl();
        //    womanUserControl.popupState = true;
        //    this.Close();
        //}
      
       
    }
}
