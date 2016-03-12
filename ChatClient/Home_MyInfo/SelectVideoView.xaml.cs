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
    /// Interaction logic for SelectVideoView.xaml
    /// </summary>
    public partial class SelectVideoView : BaseWindow
    {
        public string _strMediaFile = string.Empty;
        public SelectVideoView()
        {
            InitializeComponent();

            this.play.IsEnabled = true;
            this.pause.IsEnabled = false;
            this.stop.IsEnabled = false;
        }

        public void LoadVideo(string videoName)
        {
            mediaElement1.Source = new Uri(Login._ServerPath + videoName, UriKind.RelativeOrAbsolute);
            mediaElement1.Play();
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            lblWait.Visibility = Visibility.Visible;

            mediaElement1.MediaFailed += MediaElement_Failed; 
            mediaElement1.Source = new Uri(_strMediaFile, UriKind.RelativeOrAbsolute);
            //mediaElement1.Stop();
            mediaElement1.Play();

            this.play.IsEnabled = false;
            this.pause.IsEnabled = true;
            this.stop.IsEnabled = true;
        }

        private void MediaElement_Failed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(e.ErrorException.Message);
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            mediaElement1.Pause();

            this.play.IsEnabled = true;
            this.pause.IsEnabled = false;
            this.stop.IsEnabled = false;
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            mediaElement1.Stop();

            this.play.IsEnabled = true;
            this.pause.IsEnabled = false;
            this.stop.IsEnabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Main.selectVideoView = null;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
