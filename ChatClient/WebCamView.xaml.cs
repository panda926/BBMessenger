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


using iConfClient.NET;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for WebCamView.xaml
    /// </summary>
    public partial class WebCamView : Window
    {
        public iConfClientDotNet m_iconfClient;
        public MultyChatRoom m_multiChatRoom = new MultyChatRoom();

        public WebCamView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ics.Visibility = Visibility.Visible;
                m_iconfClient.ClearImage();

                ics.Child = m_iconfClient;
                m_iconfClient.Show();
            }
            catch (Exception)
            { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                m_multiChatRoom.ShowWomanWebCam(m_iconfClient);
            }
            catch (Exception)
            { }
        }
    }
}
