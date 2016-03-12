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
//using iConfClient.NET;
//using iConfServer.NET;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for WebCamView.xaml
    /// </summary>
    public partial class OtherWebCamView : BaseWindow
    {
        //public iConfClientDotNet m_iconfClient;
        //public iConfServerDotNet m_iconfServer;
        
        //public ChatRoom m_chatRoom = new ChatRoom();
        //public Image _webCamImage = new Image();

        public OtherWebCamView()
        {
            InitializeComponent();

            // 2013-12-29: GreenRose
            // remoteVideoImage SourceChangeEvent
            Image image = System.Windows.Application.Current.Properties["OtherWebCamView"] as Image;
            var prop = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(Image.SourceProperty, typeof(Image));
            prop.AddValueChanged(image, SourceChangedHandler);
        }

        private void SourceChangedHandler(object obj, EventArgs e)
        {
            Image image = obj as Image;
            webCamImage.Source = image.Source;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //ics.Visibility = Visibility.Visible;
                //m_iconfClient.ClearImage();

                //ics.Child = m_iconfClient;
                //m_iconfClient.Show();

                //m_chatRoom.ShowWebCam(m_iconfServer);
                //this.webCamImage.Source = _webCamImage.Source;
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }
            
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //m_chatRoom.ShowWomanWebCam(m_iconfClient, m_iconfServer);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
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
