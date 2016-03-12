using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Windows;

namespace ChatClient
{
    delegate void GameDownloadCompleteHandler( string filePath );

    class GameDownloadInfo
    {
        public string _filePath = string.Empty;
        public GameDownloadCompleteHandler _completeHandler;
        public Window _window;
    }

    class GameWebDownloader
    {
        static GameWebDownloader _instance = null;
        double currentByte = 0;
        string fileName = null;

        static public GameWebDownloader GetInstance()
        {
            if (_instance == null)
                _instance = new GameWebDownloader();

            return _instance;
        }

        public static ChatClient.Tab_Game.GameDownload gameDownloadWnd = null;        
        WebClient _Client = null;

        public bool DownloadFile(string filePath, GameDownloadCompleteHandler completeHandler, Control control)
        {
            GameDownloadInfo newInfo = new GameDownloadInfo();
            newInfo._filePath = filePath;
            newInfo._completeHandler = completeHandler;
            newInfo._window = Window.GetWindow(control);
            
            DownloadAsync(newInfo);

            return true;
        }

        void _window_Closed(object sender, EventArgs e)
        {
            Window window = sender as Window;
        }

        private void DownloadAsync(GameDownloadInfo newInfo)
        {
            _Client = null;

            GameDownloadInfo downloadInfo = newInfo;
            
            string exePath = System.Windows.Forms.Application.StartupPath + "\\";
            string localPath = exePath + downloadInfo._filePath.Replace('/', '\\');

            if (File.Exists(localPath) == true)
            {
                try
                {
                    File.Delete(localPath);
                }
                catch (Exception ex)
                {
                    string strError = ex.ToString();
                    ErrorCollection.GetInstance().SetErrorInfo(strError);
                }
            }

            string[] folderNames = localPath.Split('\\');
            string folderPath = string.Empty;
            fileName = folderNames[folderNames.Length - 1];
            for (int i = 0; i < folderNames.Length - 1; i++)
            {
                folderPath += folderNames[i];

                if (Directory.Exists(folderPath) == false)
                    Directory.CreateDirectory(folderPath);

                folderPath += '\\';
            }

            _Client = new WebClient();
            _Client.Proxy = null;

            _Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            _Client.DownloadFileCompleted += DownloadFileCompleted(localPath, downloadInfo);

            var url = Login._ServerGamePath + downloadInfo._filePath;

            //System.Threading.Thread.Sleep(1000);
            _Client.DownloadFileAsync(new Uri(url), localPath);

            //_ProgressBarWindow.Show();
            //_ProgressBarWindow.Left = downloadInfo._window.Left + downloadInfo._window.Width / 2;
            //_ProgressBarWindow.Top = downloadInfo._window.Top + downloadInfo._window.Height / 2;

            return;

            //_ProgressBarWindow.Hide();
        }

        void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            currentByte = bytesIn - currentByte;
            //gameDownloadWnd.ProgressValue( int.Parse(Math.Truncate(percentage).ToString()));

            if( gameDownloadWnd != null )
                gameDownloadWnd.ProgrssStateDisplay(fileName, currentByte / 1000, totalBytes / 1000, bytesIn / 1000);

            currentByte = bytesIn;
        }

        private AsyncCompletedEventHandler DownloadFileCompleted(string strFileName, GameDownloadInfo downloadInfo)
        {
            Action<object,AsyncCompletedEventArgs> action = (sender,e) =>
            {
                var _filename = strFileName;

                if (e.Error != null)
                {
                    //throw e.Error;
                }

                if (downloadInfo._completeHandler != null)
                    downloadInfo._completeHandler(strFileName);
            };

            return new AsyncCompletedEventHandler(action);
        }

    }
}
