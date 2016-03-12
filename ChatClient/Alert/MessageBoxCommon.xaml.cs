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
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBoxCommon : Window
    {
        public MessageBoxType _type;
        public MessageBoxReply _result;
        public string _content = string.Empty;

        public static MessageBoxReply Show( string content, MessageBoxType boxType )
        {
            MessageBoxCommon messageBox = new MessageBoxCommon();
            messageBox._type = boxType;
            messageBox._content = content;

            messageBox.ShowDialog();

            return messageBox._result;
        }

        public MessageBoxCommon()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch( _type )
            {
                case MessageBoxType.Ok:
                    {
                        yesBt.Visibility = Visibility.Hidden;
                        noBt.Visibility = Visibility.Hidden;

                        _result = MessageBoxReply.Ok;
                    }
                    break;

                case MessageBoxType.YesNo:
                    {
                        okBt.Visibility = Visibility.Hidden;

                        _result = MessageBoxReply.No;
                    }
                    break;
            }

            msgBoxlabel.Content = _content;
        }

        private void yesBt_Click(object sender, RoutedEventArgs e)
        {
            _result = MessageBoxReply.Yes;
            this.Close();
        }

        private void okBt_Click(object sender, RoutedEventArgs e)
        {
            _result = MessageBoxReply.Ok;
            this.Close();
        }

        private void noBt_Click(object sender, RoutedEventArgs e)
        {
            _result = MessageBoxReply.No;
            this.Close();
        }
    }

    public enum MessageBoxType
    {
        Ok,
        YesNo
    }

    public enum MessageBoxReply
    {
        Ok,
        Yes,
        No
    }
}
