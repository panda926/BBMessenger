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

namespace ChatClient
{
	/// <summary>
	/// MyHome.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class SelectUserHome : BaseWindow
	{
        UserInfo selectUser = null;
        public UserDetailInfo selUserDetail = null;
        public SelectUserHome()
		{
			this.InitializeComponent();
            // 개체 만들기에 필요한 코드를 이 지점 아래에 삽입하십시오.

		}
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listBox1.SelectedIndex = 0;
            contentsGrid.Children.Clear();
            SelectUserInfoControl selectUserInfoControl = new SelectUserInfoControl();
            selectUserInfoControl.InitMyInfo(selectUser);
            contentsGrid.Children.Add(selectUserInfoControl);
        }

        public void InitTabSetting(UserInfo userInfo)
        {
            if (userInfo != null)
            {
                selectUser = userInfo;

                ImageBrush faceImage = new ImageBrush();
                faceImage.Stretch = Stretch.Fill;
                faceImage.ImageSource = ImageDownloader.GetInstance().GetImage(userInfo.Icon);
                myPicture.Fill = faceImage;

                ListBoxItem item1 = new ListBoxItem();
                item1.Content = "信息";
                item1.MouseUp += new MouseButtonEventHandler(item1_MouseUp);
                ListBoxItem item2 = new ListBoxItem();
                item2.Content = "相 册";
                item2.MouseUp += new MouseButtonEventHandler(item2_MouseUp);
                //ListBoxItem item3 = new ListBoxItem();
                //item3.Content = "评 价";
                //item3.MouseUp += new MouseButtonEventHandler(item3_MouseUp);
                ListBoxItem item4 = new ListBoxItem();

                listBox1.Items.Add(item1);

                if (userInfo.Kind == (int)UserKind.ServiceWoman)
                {
                    //item4.Content = "视频";
                    //item4.MouseUp += new MouseButtonEventHandler(item4_MouseUp);
                    listBox1.Items.Add(item2);
                    //listBox1.Items.Add(item3);
                    //listBox1.Items.Add(item4);
                }
            }
        }


        void item4_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                contentsGrid.Children.Clear();
                PictureControl pictureControl = new PictureControl();
                pictureControl.button4.Visibility = Visibility.Hidden;
                pictureControl.InitVideoList(selUserDetail.Faces);
                contentsGrid.Children.Add(pictureControl);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        void item3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                contentsGrid.Children.Clear();
                SelectLetterControl selectLetterControl = new SelectLetterControl();
                selectLetterControl.markInfo(selUserDetail);
                contentsGrid.Children.Add(selectLetterControl);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        void item2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                contentsGrid.Children.Clear();
                SelectAlbermControl selectAlbermControl = new SelectAlbermControl();
                selectAlbermControl.selPictureList(selUserDetail);
                contentsGrid.Children.Add(selectAlbermControl);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        void item1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                contentsGrid.Children.Clear();
                SelectUserInfoControl selectUserInfoControl = new SelectUserInfoControl();
                selectUserInfoControl.InitMyInfo(selectUser);
                contentsGrid.Children.Add(selectUserInfoControl);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.selectUserHome = null;
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
	}
}