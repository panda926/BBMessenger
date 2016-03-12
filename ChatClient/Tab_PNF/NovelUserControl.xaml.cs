using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChatEngine;
using System.ComponentModel;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for FilmUserControl.xaml
    /// </summary>
    public partial class NovelUserControl : UserControl
    {
        public NovelUserControl()
        {
            InitializeComponent();
        }

        public void InitNovelView(ClassTypeInfo classTypeInfo)
        {
            dateLabel.Content = classTypeInfo.Class_File_Date.ToString("yyyy-MM-dd");
            char[] delimiterChars = { '.' };
            string[] words = classTypeInfo.Class_File_Name.Split(delimiterChars);
            int count = words.Length;
            titleLabel.Content = words[0].ToString();
            novelGrid.DataContext = classTypeInfo.Class_File_Name;
        }

        public void NovelDownloadComplete(string filePath)
        {
            Utils.FileExecute(filePath);
        }

        private void novelGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var itemGrid = sender as Grid;

            WebDownloader.GetInstance().DownloadFile("NovelFile/" + itemGrid.DataContext.ToString(), NovelDownloadComplete, this);
            return;
        }

        private void novelGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            novelGrid.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightSkyBlue);
        }

        private void novelGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            novelGrid.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
        }
        
    }
}
