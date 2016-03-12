using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ChatEngine;
using System.Windows.Media;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for UserIconControl.xaml
    /// </summary>
    public partial class UserIconControl : UserControl
    {
        public UserIconControl()
        {
            InitializeComponent();
            IconListView();
        }
        private void IconListView()
        {
            iconwrapPanel.Children.Clear();

            for (int i = 1; i <= 60; i++)
            {
                var faceGrid = new Grid();
                faceGrid.Width = 58;
                faceGrid.Height = 58;
                faceGrid.Margin = new Thickness(2, 2, 1, 2);

                Image icon = new Image();
                icon.Margin = new Thickness(0);
                icon.Width = 58;
                icon.Height = 58;
                icon.Cursor = Cursors.Hand;
                string filePath = string.Format("image/face/{0}", i + ".gif");
                icon.DataContext = filePath;
                icon.Source = ImageDownloader.GetInstance().GetImage(filePath);
                //icon.Source = ImageDownloader.GetInstance().GetImage(Window1._UserInfo.Icon);
                icon.MouseLeftButtonDown += new MouseButtonEventHandler(icon_MouseUp);
                
                
                faceGrid.Children.Add(icon);
                iconwrapPanel.Children.Add(faceGrid);
            }
        }

        void icon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectImage = sender as Image;
            string imgIcon = selectImage.DataContext.ToString();

            Window1._UserInfo.Icon = imgIcon;
            Window1._ClientEngine.Send(NotifyType.Request_UpdateUser, Window1._UserInfo);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
