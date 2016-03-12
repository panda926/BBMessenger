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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for PreviewImage.xaml
    /// </summary>
    public partial class PreviewImage : BaseWindow
    {
        public PreviewImage()
        {
            InitializeComponent();
        }

        public void ViewImage(string imageName)
        {
            imageGrid.Children.Clear();
            Image img = new Image();
            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri(imageName, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            img.Source = bi;
            double widthImg = img.Source.Width;
            double heightImg = img.Source.Height;
            imageGrid.Children.Add(img);

            this.Width = widthImg;
            this.Height = heightImg;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.previewImage = null;
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
