using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using ChatEngine;
using System.Threading;
using System.ComponentModel;
using System.Net;

using ControlExs;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MusicPlayer.xaml
    /// </summary>
    public partial class MusicPlayer : Window
    {
        int selectMusic = 0;
        List<string> musicList = new List<string>();
        List<string> playList = new List<string>();
        bool playState = true;
        MusiceStateInfo musiceStateInfo = null;
        BackgroundWorker _LoadWorker = new System.ComponentModel.BackgroundWorker();
        bool sendMusiceFlag = true;
        bool selectMusicFlag = false;
        string musicname = null;

        public MusicPlayer()
        {
            InitializeComponent();
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


           
            
        }
        

        private void lstMediaItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectMusic = lstMediaItems.SelectedIndex + 1;
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaPlayerMain.Volume = (double)sliderVolume.Value;
        }
        
        private void play_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (selectMusic > 0)
            {
                if (playState == true)
                {
                    playState = false;
                    musicStop.IsEnabled = true;
                    musicStop.Opacity = 1;
                    BitmapImage musicBit = new BitmapImage();
                    musicBit.BeginInit();
                    musicBit.UriSource = new Uri("/Resources;component/image/pause.png", UriKind.RelativeOrAbsolute);
                    musicBit.EndInit();
                    play.Source = musicBit;

                    if (selectMusic < musicList.Count)
                    {
                        musicNext.Opacity = 1;
                        musicNext.IsEnabled = true;
                        musicPreNext.Opacity = 1;
                        musicPreNext.IsEnabled = true;
                    }
                    musicStop.IsEnabled = true;
                    musicStop.Opacity = 1;

                    mediaPlayerMain.Source = new Uri(musicList[selectMusic - 1], UriKind.RelativeOrAbsolute);
                    mediaPlayerMain.Play();
                    mediaPlayerMain.Volume = (double)sliderVolume.Value;

                    musiceStateInfo = new MusiceStateInfo();
                    musiceStateInfo.MusiceName = lstMediaItems.SelectedItem.ToString();
                    musiceStateInfo.M_Kind = (int)MusiceKind.Play;
                    Login._ClientEngine.Send(NotifyType.Request_MusiceStateInfo, musiceStateInfo);
                    
                }
                else
                {
                    playState = true;
                    musicStop.IsEnabled = false;
                    musicStop.Opacity = 0.5;

                    BitmapImage musicBit2 = new BitmapImage();
                    musicBit2.BeginInit();
                    musicBit2.UriSource = new Uri("/Resources;component/image/play.png", UriKind.RelativeOrAbsolute);
                    musicBit2.EndInit();
                    play.Source = musicBit2;

                    mediaPlayerMain.Pause();

                    musiceStateInfo.MusiceName = lstMediaItems.SelectedItem.ToString();
                    musiceStateInfo.M_Kind = (int)MusiceKind.Pause;
                    Login._ClientEngine.Send(NotifyType.Request_MusiceStateInfo, musiceStateInfo);
                    
                }
            }
        }

        
        private void musicNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectMusic = selectMusic + 1;
            if (selectMusic > musicList.Count)
                selectMusic = 1;

            playState = false;
            lstMediaItems.SelectedIndex = selectMusic - 1;
            BitmapImage musicBit4 = new BitmapImage();
            musicBit4.BeginInit();
            musicBit4.UriSource = new Uri("/Resources;component/image/pause.png", UriKind.RelativeOrAbsolute);
            musicBit4.EndInit();
            play.Source = musicBit4;

            musicStop.IsEnabled = true;
            musicStop.Opacity = 1;

            mediaPlayerMain.Source = new Uri(musicList[selectMusic - 1], UriKind.RelativeOrAbsolute);
            mediaPlayerMain.Play();
            mediaPlayerMain.Volume = (double)sliderVolume.Value;

            musiceStateInfo.MusiceName = lstMediaItems.SelectedItem.ToString();
            musiceStateInfo.M_Kind = (int)MusiceKind.Play;
            Login._ClientEngine.Send(NotifyType.Request_MusiceStateInfo, musiceStateInfo);
            
        }

        private void musicPreNext_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectMusic = selectMusic - 1;
            if (selectMusic < 1)
                selectMusic = musicList.Count;

            playState = true;
            lstMediaItems.SelectedIndex = selectMusic - 1;
            BitmapImage musicBit6 = new BitmapImage();
            musicBit6.BeginInit();
            musicBit6.UriSource = new Uri("/Resources;component/image/pause.png", UriKind.RelativeOrAbsolute);
            musicBit6.EndInit();
            play.Source = musicBit6;

            musicStop.IsEnabled = true;
            musicStop.Opacity = 1;

            mediaPlayerMain.Source = new Uri(musicList[selectMusic - 1], UriKind.RelativeOrAbsolute);
            mediaPlayerMain.Play();
            mediaPlayerMain.Volume = (double)sliderVolume.Value;

            musiceStateInfo.MusiceName = lstMediaItems.SelectedItem.ToString();
            musiceStateInfo.M_Kind = (int)MusiceKind.Play;
            Login._ClientEngine.Send(NotifyType.Request_MusiceStateInfo, musiceStateInfo);
            
        }

        private void musicStop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            playState = true;
            musicStop.IsEnabled = false;
            musicStop.Opacity = 0.5;

            BitmapImage musicBit3 = new BitmapImage();
            musicBit3.BeginInit();
            musicBit3.UriSource = new Uri("/Resources;component/image/play.png", UriKind.RelativeOrAbsolute);
            musicBit3.EndInit();
            play.Source = musicBit3;

            mediaPlayerMain.Stop();

            musiceStateInfo.MusiceName = lstMediaItems.SelectedItem.ToString();
            musiceStateInfo.M_Kind = (int)MusiceKind.Stop;
            Login._ClientEngine.Send(NotifyType.Request_MusiceStateInfo, musiceStateInfo);
        }

        private void open_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _LoadWorker = new BackgroundWorker();
            _LoadWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadWorker_DoWork);
            _LoadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_LoadWorker_RunWorkerCompleted);

            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "mp3 (*.mp3)|*.mp3|wma (*.wma)|*.wma";

            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {

                System.IO.FileInfo fileSize = new System.IO.FileInfo(ofd.FileName);
                if (fileSize.Length > 5000000)
                {
                    //MessageBoxCommon.Show("Overflow mp3 Size.", MessageBoxType.Ok);
                    //return;

                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "Overflow mp3 Size.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                }

                char[] delimiterChars = { ':', '\\' };
                string[] words = ofd.FileName.Split(delimiterChars);
                int count = words.Length;
                musicname = words[count - 1].ToString();
                string filename = ofd.FileName;
                lstMediaItems.Items.Add(musicname);
                musicList.Add(filename);

                
                _LoadWorker.RunWorkerAsync();
               

            }
        }
        private void LoadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //selectMusicFlag = false;
            playList.Add(musicname);
            UpdateFile(musicname);
        }

        void _LoadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            selectMusicFlag = true;
            
        }

        private bool UpdateFile(string strFileName)
        {
            try
            {
                string strUri = Login._ServerPath;
                strUri += "MusicUpload.php";
                

                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.UploadFile(strUri, strFileName);
                wc.Dispose();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (selectMusic > 1)
            {
                musicList.RemoveAt(selectMusic - 1);
            }
        }


        private void Element_MediaOpened(object sender, EventArgs e)
        {
            mediaPlayerMain.Play();
            
        }

        private void Element_MediaEnded(object sender, EventArgs e)
        {
            mediaPlayerMain.Stop();

            ++selectMusic;

            if (selectMusic > musicList.Count)
                selectMusic = 1;
            

            lstMediaItems.SelectedIndex = selectMusic - 1;

            mediaPlayerMain.Source = new Uri(musicList[selectMusic - 1], UriKind.RelativeOrAbsolute);
            mediaPlayerMain.Play();

            musiceStateInfo = new MusiceStateInfo();
            musiceStateInfo.MusiceName = lstMediaItems.SelectedItem.ToString();
            musiceStateInfo.M_Kind = (int)MusiceKind.Play;
            Login._ClientEngine.Send(NotifyType.Request_MusiceStateInfo, musiceStateInfo);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (musiceStateInfo != null)
            {
                if (lstMediaItems.Items.Count != 0)
                    musiceStateInfo.MusiceName = lstMediaItems.SelectedItem.ToString();
                musiceStateInfo.M_Kind = (int)MusiceKind.Stop;
                Login._ClientEngine.Send(NotifyType.Request_MusiceStateInfo, musiceStateInfo);
            }
        }

        
    }
}
