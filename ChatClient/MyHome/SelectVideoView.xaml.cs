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
    public partial class SelectVideoView : Window
    {
        public SelectVideoView()
        {
            InitializeComponent();
        }

        public void LoadVideo(string videoName)
        {
            mediaElement1.Source = new Uri(Window1._ServerPath + videoName, UriKind.RelativeOrAbsolute);
            mediaElement1.Play();
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            mediaElement1.Stop();
            mediaElement1.Play();
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            mediaElement1.Pause();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            mediaElement1.Stop();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Main.selectVideoView = null;
        }
    }
}
