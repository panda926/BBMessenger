using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ChatEngine;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for RecievePaper.xaml
    /// </summary>
    public partial class RecievePaper : Window
    {
        public RecievePaper()
        {
            InitializeComponent();
        }

        public void paperList(List<BoardInfo> boardList)
        {
            if (boardList != null)
            {
                for (int i = 0; i < boardList.Count; i++)
                {
                    if (boardList[i].UserKind == Window1._UserInfo.Kind)
                    {
                        StackPanel stackPanel = new StackPanel();
                        stackPanel.Margin = new Thickness(0);
                        stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                        stackPanel.MouseUp += new MouseButtonEventHandler(stackPanel_MouseUp);
                        stackPanel.DataContext = i;

                        Image img = new Image();
                        string strImageSource = "image/arror.png";

                        BitmapImage bitMapImage = new BitmapImage();
                        bitMapImage.BeginInit();
                        bitMapImage.UriSource = new Uri(strImageSource, UriKind.RelativeOrAbsolute);
                        bitMapImage.EndInit();

                        img.Source = bitMapImage;
                        img.Width = 25;

                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = "  " + boardList[i].Title;

                        stackPanel.Children.Add(img);
                        stackPanel.Children.Add(textBlock);

                        this.listBox1.Items.Add(stackPanel);
                    }
                }
            }
            else
                textBlock1.Text = "관리자로부터 받은 편가 없습니다.";
        }

        void stackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StackPanel index = sender as StackPanel;
            string listIndex = index.DataContext.ToString();
            string contents = Window1.boardList[Convert.ToInt32(listIndex)].Content;
            textBlock1.Text = contents;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.recievePaper = null;
        }
    }
}
