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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient.ChattingRoom
{
    /// <summary>
    /// Interaction logic for VideoCalling.xaml
    /// </summary>
    public partial class VideoCalling : UserControl
    {
        public VideoCalling()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler AcceptButton_Click;
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            this.lblVideoChatting.Content = "视频聊天进步";
            this.btnAccept.Visibility = System.Windows.Visibility.Hidden;
            this.btnReject.Visibility = System.Windows.Visibility.Hidden;
            if (AcceptButton_Click != null)
                AcceptButton_Click(sender, e);
        }

        public event RoutedEventHandler RejectButton_Click;
        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            this.lblVideoChatting.Content = "视频聊天结束";
            this.btnAccept.Visibility = System.Windows.Visibility.Hidden;
            this.btnReject.Visibility = System.Windows.Visibility.Hidden;
            
            if (RejectButton_Click != null)
                RejectButton_Click(sender, e);
        }
    }
}
