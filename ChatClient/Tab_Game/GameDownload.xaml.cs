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

// 2014-01-21: GreenRose
using System.Windows.Threading;

namespace ChatClient.Tab_Game
{
    /// <summary>
    /// Interaction logic for GameDownload.xaml
    /// </summary>
    public partial class GameDownload : BaseWindow
    {
        DispatcherTimer flowGameImage = new DispatcherTimer();
        List<string> listGameImage = new List<string>();
        
        public GameDownload()
        {
            InitializeComponent();

            InitListGameImage();
        }

        private void InitListGameImage()
        {
            string strImageFile = "pack://application:,,,/Resources;component/image/GameImage/carGame.jpg";
            listGameImage.Add(strImageFile);

            strImageFile = "pack://application:,,,/Resources;component/image/GameImage/diceGame.jpg";
            listGameImage.Add(strImageFile);

            strImageFile = "pack://application:,,,/Resources;component/image/GameImage/horseGame.jpg";
            listGameImage.Add(strImageFile);

            strImageFile = "pack://application:,,,/Resources;component/image/GameImage/carGameResult.jpg";
            listGameImage.Add(strImageFile);

            strImageFile = "pack://application:,,,/Resources;component/image/GameImage/diceGameResult.jpg";
            listGameImage.Add(strImageFile);

            strImageFile = "pack://application:,,,/Resources;component/image/GameImage/horseGameResult.jpg";
            listGameImage.Add(strImageFile);
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            //this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void ProgressValue(int nVal)
        {
            this.progressUpdate.Value = nVal;
        }

        string bitUnit1 = null;
        string bitUnit2 = null;
        public void ProgrssStateDisplay(string _name, double _perBit, double _capacity, double _currentcap)
        {            
            if ((_capacity % 1000) > 0)
            {
                _capacity = _capacity / 1000;
                bitUnit1 = "MB";
            }
            else
                bitUnit1 = "KB";

            if ((_currentcap % 1000) > 0)
            {
                _currentcap = _currentcap / 1000;
                bitUnit2 = "MB";
            }
            else
                bitUnit2 = "KB";
            
            lblState.Content = "FileDownloading" + "   " + Math.Round(_capacity, 1) + bitUnit1 + "/" + Math.Round(_currentcap, 1) + bitUnit2;
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            flowGameImage.Interval = TimeSpan.FromSeconds(3);
            flowGameImage.Tick += new EventHandler(flowGameImage_Tick);

            if (listGameImage.Count > 0)
                flowGameImage.Start();
        }

        static int nStep = 0;
        void flowGameImage_Tick(object sender, EventArgs e)
        {
            if (nStep + 1 > listGameImage.Count)
            {
                nStep = 0;
            }

            imgGame.Source = new BitmapImage(new Uri(listGameImage[nStep], UriKind.RelativeOrAbsolute));
            nStep++;
        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
