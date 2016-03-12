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

        BitmapImage bi = null;
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
                string filePath = string.Format("/Resources;component/image/face/{0}", i + ".gif");
                //icon.DataContext = filePath;
                icon.DataContext = string.Format("image/face/{0}", i + ".gif");

                bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
                bi.EndInit();

                icon.Source = bi;
                //icon.Source = ImageDownloader.GetInstance().GetImage(filePath);
                //icon.Source = ImageDownloader.GetInstance().GetImage(Login._UserInfo.Icon);
                icon.MouseLeftButtonDown += new MouseButtonEventHandler(icon_MouseUp);
                faceGrid.Children.Add(icon);
                iconwrapPanel.Children.Add(faceGrid);
            }
        }

        void icon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectImage = sender as Image;
            string imgIcon = selectImage.DataContext.ToString();

            Login._UserInfo.Icon = imgIcon;
            Login._ClientEngine.Send(NotifyType.Request_UpdateUser, Login._UserInfo);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
