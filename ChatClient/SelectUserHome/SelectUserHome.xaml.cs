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
	public partial class SelectUserHome : Window
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
           
            
        }
        public void InitTabSetting(UserInfo userInfo)
        {
            selectUser = userInfo;
            Image faceImage = new Image();
            faceImage.Margin = new Thickness(2, 2, 2, 2);
            faceImage.Stretch = Stretch.Fill;
            BitmapImage face = new BitmapImage();
            face.BeginInit();
            face.UriSource = new Uri(Main._ServerPath + userInfo.Icon, UriKind.RelativeOrAbsolute);
            face.EndInit();
            faceImage.Source = face;
            userPicture.Children.Add(faceImage);


            userName.Content = userInfo.Nickname;
            userAge.Content = (DateTime.Now.Year - userInfo.Year).ToString();
            if (userInfo.Kind == (int)UserKind.ServiceWoman)
            {
                cash_mark.Content = "+ 评 分:";
                if (userInfo.Visitors != 0)
                    userVisit.Content = (userInfo.Evaluation / userInfo.Visitors).ToString();
                else
                    userVisit.Content = "0";
               
            }
            else
            {
                cash_mark.Content = "+ 现 金:";
                userVisit.Content = userInfo.Cash.ToString();
            }

            var tabGrid = new Grid();
            ColumnDefinition tabCol1 = new ColumnDefinition();
            tabCol1.Width = GridLength.Auto;
            ColumnDefinition tabCol2 = new ColumnDefinition();
            tabCol2.Width = GridLength.Auto;
            ColumnDefinition tabCol3 = new ColumnDefinition();
            tabCol3.Width = GridLength.Auto;
            ColumnDefinition tabCol4 = new ColumnDefinition();
            tabCol4.Width = GridLength.Auto;
            ColumnDefinition tabCol5 = new ColumnDefinition();
            tabCol5.Width = GridLength.Auto;
            


            tabGrid.ColumnDefinitions.Add(tabCol1);
            tabGrid.ColumnDefinitions.Add(tabCol2);
            tabGrid.ColumnDefinitions.Add(tabCol3);
            tabGrid.ColumnDefinitions.Add(tabCol4);
            tabGrid.ColumnDefinitions.Add(tabCol5);
            


            Label tabLabel1 = new Label();
            tabLabel1.Style = (Style)FindResource("SimpleLabel");
            tabLabel1.Foreground = new SolidColorBrush(Colors.White);
            tabLabel1.FontWeight = FontWeights.Bold;
            tabLabel1.HorizontalAlignment = HorizontalAlignment.Center;
            tabLabel1.VerticalAlignment = VerticalAlignment.Center;
            tabLabel1.Cursor = Cursors.Hand;
            tabLabel1.Content = "信息";
            tabLabel1.MouseDown += new MouseButtonEventHandler(selectInfoChange_MouseDown);
            

            Label tabLabel2 = new Label();
            tabLabel2.Style = (Style)FindResource("SimpleLabel");
            tabLabel2.Foreground = new SolidColorBrush(Colors.White);
            tabLabel2.FontWeight = FontWeights.Bold;
            tabLabel2.HorizontalAlignment = HorizontalAlignment.Center;
            tabLabel2.VerticalAlignment = VerticalAlignment.Center;
            tabLabel2.Cursor = Cursors.Hand;
            tabLabel2.Content = "照  片"; 
            tabLabel2.MouseDown += new MouseButtonEventHandler(albermBt_MouseDown);
           

            Label tabLabel3 = new Label();
            tabLabel3.Style = (Style)FindResource("SimpleLabel");
            tabLabel3.Foreground = new SolidColorBrush(Colors.White);
            tabLabel3.FontWeight = FontWeights.Bold;
            tabLabel3.HorizontalAlignment = HorizontalAlignment.Center;
            tabLabel3.VerticalAlignment = VerticalAlignment.Center;
            tabLabel3.Cursor = Cursors.Hand;
            tabLabel3.Content = "评  分";
            if (userInfo.Kind == (int)UserKind.ServiceWoman)
            {
                tabLabel3.IsEnabled = true;
                tabLabel3.MouseDown += new MouseButtonEventHandler(mark_MouseDown);
            }
            else
                tabLabel3.IsEnabled = false;

            Label tabLabel4 = new Label();
            tabLabel4.Style = (Style)FindResource("SimpleLabel");
            tabLabel4.Foreground = new SolidColorBrush(Colors.White);
            tabLabel4.FontWeight = FontWeights.Bold;
            tabLabel4.HorizontalAlignment = HorizontalAlignment.Center;
            tabLabel4.VerticalAlignment = VerticalAlignment.Center;
            tabLabel4.Cursor = Cursors.Hand;
            tabLabel4.Content = "성원목록";
            tabLabel4.MouseDown += new MouseButtonEventHandler(listMemberLabel_MouseDown);
            



            Label tabLabel5 = new Label();
            tabLabel5.Style = (Style)FindResource("SimpleLabel");
            tabLabel5.Foreground = new SolidColorBrush(Colors.White);
            tabLabel5.FontWeight = FontWeights.Bold;
            tabLabel5.HorizontalAlignment = HorizontalAlignment.Center;
            tabLabel5.VerticalAlignment = VerticalAlignment.Center;
            tabLabel5.Cursor = Cursors.Hand;
            tabLabel5.Content = "细目官吏";
            tabLabel5.MouseDown += new MouseButtonEventHandler(innerMag_MouseDown);
            

            

            Image tabImg1 = new Image();
            tabImg1.Width = 68;
            tabImg1.Height = 26;
            tabImg1.Stretch = Stretch.Fill;
            BitmapImage tabBit1 = new BitmapImage();
            tabBit1.BeginInit();
            tabBit1.UriSource = new Uri("../image/tab1.gif", UriKind.RelativeOrAbsolute);
            tabBit1.EndInit();
            tabImg1.Source = tabBit1;

            Image tabImg2 = new Image();
            tabImg2.Width = 68;
            tabImg2.Height = 26;
            tabImg2.Stretch = Stretch.Fill;
            BitmapImage tabBit2 = new BitmapImage();
            tabBit2.BeginInit();
            tabBit2.UriSource = new Uri("../image/tab1.gif", UriKind.RelativeOrAbsolute);
            tabBit2.EndInit();
            tabImg2.Source = tabBit2;

            Image tabImg3 = new Image();
            tabImg3.Width = 68;
            tabImg3.Height = 26;
            tabImg3.Stretch = Stretch.Fill;
            BitmapImage tabBit3 = new BitmapImage();
            tabBit3.BeginInit();
            if (userInfo.Kind == (int)UserKind.ServiceWoman)
                tabBit3.UriSource = new Uri("../image/tab1.gif", UriKind.RelativeOrAbsolute);
            else
                tabBit3.UriSource = new Uri("../image/tab2.gif", UriKind.RelativeOrAbsolute);
            tabBit3.EndInit();
            tabImg3.Source = tabBit3;

            Image tabImg4 = new Image();
            tabImg4.Width = 68;
            tabImg4.Height = 26;
            tabImg4.Stretch = Stretch.Fill;
            BitmapImage tabBit4 = new BitmapImage();
            tabBit4.BeginInit();
            tabBit4.UriSource = new Uri("../image/tab1.gif", UriKind.RelativeOrAbsolute);
            tabBit4.EndInit();
            tabImg4.Source = tabBit4;

            Image tabImg5 = new Image();
            tabImg5.Width = 68;
            tabImg5.Height = 26;
            tabImg5.Stretch = Stretch.Fill;
            BitmapImage tabBit5 = new BitmapImage();
            tabBit5.BeginInit();
            tabBit5.UriSource = new Uri("../image/tab1.gif", UriKind.RelativeOrAbsolute);
            tabBit5.EndInit();
            tabImg5.Source = tabBit5;

                        

            Grid.SetRow(tabImg1, 0);
            Grid.SetColumn(tabImg1, 0);

            Grid.SetRow(tabImg2, 0);
            Grid.SetColumn(tabImg2, 1);

            Grid.SetRow(tabImg3, 0);
            Grid.SetColumn(tabImg3, 2);

            Grid.SetRow(tabImg4, 0);
            Grid.SetColumn(tabImg4, 3);

            Grid.SetRow(tabImg5, 0);
            Grid.SetColumn(tabImg5, 4);

            

            Grid.SetRow(tabLabel1, 0);
            Grid.SetColumn(tabLabel1, 0);

            Grid.SetRow(tabLabel2, 0);
            Grid.SetColumn(tabLabel2, 1);

            Grid.SetRow(tabLabel3, 0);
            Grid.SetColumn(tabLabel3, 2);

            Grid.SetRow(tabLabel4, 0);
            Grid.SetColumn(tabLabel4, 3);

            Grid.SetRow(tabLabel5, 0);
            Grid.SetColumn(tabLabel5, 4);

            

            tabGrid.Children.Add(tabImg1);
            tabGrid.Children.Add(tabImg2);
            tabGrid.Children.Add(tabImg3);
            tabGrid.Children.Add(tabImg4);
            tabGrid.Children.Add(tabImg5);
            tabGrid.Children.Add(tabLabel1);
            tabGrid.Children.Add(tabLabel2);
            tabGrid.Children.Add(tabLabel3);
            tabGrid.Children.Add(tabLabel4);
            tabGrid.Children.Add(tabLabel5);
            
            

            grid1.Children.Add(tabGrid);
        }

        
        
        private void selectInfoChange_MouseDown(object sender, MouseButtonEventArgs e)
        {
            titleName.Content = "信息";
            contentsGrid.Children.Clear();
            SelectUserInfoControl selectUserInfoControl = new SelectUserInfoControl();
            selectUserInfoControl.InitMyInfo(selectUser);
            contentsGrid.Children.Add(selectUserInfoControl);
        }

        private void albermBt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            titleName.Content = "照  片";
            contentsGrid.Children.Clear();
            SelectAlbermControl selectAlbermControl = new SelectAlbermControl();
            selectAlbermControl.InnerAlberm(selUserDetail);
            contentsGrid.Children.Add(selectAlbermControl);
        }

        private void mark_MouseDown(object sender, MouseButtonEventArgs e)
        {
            titleName.Content = "评  分";
            contentsGrid.Children.Clear();
            SelectLetterControl selectLetterControl = new SelectLetterControl();
            selectLetterControl.markInfo(selUserDetail);
            contentsGrid.Children.Add(selectLetterControl);
        }

        private void listMemberLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            titleName.Content = "성원목록";
            contentsGrid.Children.Clear();
            SelectNoticeMemberControl selectNoticeMemberControl = new SelectNoticeMemberControl();
            selectNoticeMemberControl.listMember(selUserDetail);
            contentsGrid.Children.Add(selectNoticeMemberControl);
        }

        private void innerMag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            titleName.Content = "细目官吏";
            contentsGrid.Children.Clear();
            SelectInnerControl selectInnerControl = new SelectInnerControl();
            selectInnerControl.InnerChatting(selUserDetail);
            contentsGrid.Children.Add(selectInnerControl);
        }

       
        
	}
}