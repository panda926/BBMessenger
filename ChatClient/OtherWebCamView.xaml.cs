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
using ChatEngine;
using iConfClient.NET;
using iConfServer.NET;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for WebCamView.xaml
    /// </summary>
    public partial class OtherWebCamView : Window
    {
        public iConfClientDotNet m_iconfClient;
        public iConfServerDotNet m_iconfServer;
        
        public ChatRoom m_chatRoom = new ChatRoom();
        

        public OtherWebCamView()
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

                m_chatRoom.ShowWebCam(m_iconfServer);
            }
            catch (Exception)
            { }
        }
            
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                m_chatRoom.ShowWomanWebCam(m_iconfClient, m_iconfServer);
            }
            catch (Exception)
            { }
        }
    }
}
