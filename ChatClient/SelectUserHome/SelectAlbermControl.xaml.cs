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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatEngine;

namespace ChatClient
{
	/// <summary>
	/// AlbermControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class SelectAlbermControl : UserControl
	{
        public SelectAlbermControl()
		{
			this.InitializeComponent();
		}

        public void InnerAlberm(UserDetailInfo user)
        {
            picturePanel.Children.Clear();
            for (int i = 0; i < user.Faces.Count; i++)
            {
                var gameGrid = new Grid();
                gameGrid.Width = 82;
                gameGrid.Height = 81;
                gameGrid.Margin = new Thickness(0);

                Image finalImage = new Image();
                finalImage.Cursor = Cursors.Hand;
                finalImage.Margin = new Thickness(2, 1, 2, 1);
                BitmapImage veri = new BitmapImage();
                veri.BeginInit();
                
                    veri.UriSource = new Uri(Main._ServerPath + user.Faces[i].Icon, UriKind.RelativeOrAbsolute);
                    finalImage.DataContext = user.Faces[i].Id;
               

                veri.EndInit();
                finalImage.Source = veri;
                //finalImage.MouseUp += new MouseButtonEventHandler(EnterGame_MouseUp);
                gameGrid.Children.Add(finalImage);
                picturePanel.Children.Add(gameGrid);
            }
        }
        
	}
}