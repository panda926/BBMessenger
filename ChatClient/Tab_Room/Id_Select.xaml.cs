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
    /// Interaction logic for Id_Select.xaml
    /// </summary>
    public partial class Id_Select : Window
    {
        public Id_Select()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            for (int i = 0; i < 24; i++)
            {
                Button randomBt = new Button();
                
                int randomNum = random.Next(11111111, 99999999);
                randomBt.Content = randomNum.ToString();
                randomBt.Margin = new Thickness(3);
                randomBt.Height = 23;
                randomBt.Width = 65;
                randomBt.Background = new SolidColorBrush(Colors.Transparent);
                randomBt.BorderBrush = new SolidColorBrush(Colors.Transparent);
                randomBt.Foreground = new SolidColorBrush(Colors.Black);
                randomBt.BorderThickness = new Thickness(0);
                randomBt.Click += new RoutedEventHandler(randomBt_Click);
                randomPan.Children.Add(randomBt);
            }
        }

        void randomBt_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
            Button selectBt = sender as Button;
            var btClick = selectBt.Content;
            this.Content = btClick;
        }

        private void findId_Click(object sender, RoutedEventArgs e)
        {

        }
      
    }
}
