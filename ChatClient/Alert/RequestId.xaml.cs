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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for RequestId.xaml
    /// </summary>
    public partial class RequestId : Window
    {
        public RequestId()
        {
            InitializeComponent();
        }

        private void yes_Click(object sender, RoutedEventArgs e)
        {
            Login._ClientEngine.Send(ChatEngine.NotifyType.Request_NewID, Login._UserInfo);
            this.Close();
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
