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
    delegate void DownloadCompleteHandler( string filePath );

    class DownloadInfo
    {
        public string _filePath = string.Empty;
        public DownloadCompleteHandler _completeHandler;
        public Window _window;
    }

    class WebDownloader
    {
        static WebDownloader _instance = null;
        double currentByte = 0;
        string fileName = null;

        static public WebDownloader GetInstance()
        {
            if (_instance == null)
                _instance = new WebDownloader();

            return _instance;
        }

        ProgressBarWindow _ProgressBarWindow = new ProgressBarWindow();
        WebClient _Client = null;

        List<DownloadInfo> _downloadList = new List<DownloadInfo>();

        public bool DownloadFile(string filePath, DownloadCompleteHandler completeHandler, Control control )
        {
            //foreach (DownloadInfo downloadInfo in _downloadList)
            //{
            //    if (downloadInfo._filePath == filePath)
            //    {
            //        return true;
            //    }
            //}

            DownloadInfo newInfo = new DownloadInfo();
            newInfo._filePath = filePath;
            newInfo._completeHandler = completeHandler;
            newInfo._window = Window.GetWindow(control);

            newInfo._window.Closed += new EventHandler(_window_Closed);
            
            _downloadList.Add(newInfo);

            if( _downloadList.Count <= 1 )
                DownloadAsync();

            return true;
        }

        public void CancelDownload(Window window)
        {
            if (_downloadList.Count == 0)
                return;

            if (_Client != null)
            {
                DownloadInfo downloadInfo = _downloadList[0];

                if (downloadInfo._window == window)
                {
                    _Client.CancelAsync();
                }
            }

            int i = 0;
            while (i < _downloadList.Count)
            {
                DownloadInfo nextInfo = _downloadList[i];

                if (nextInfo._window == window)
                {
                    _downloadList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        void _window_Closed(object sender, EventArgs e)
        {
            Window window = sender as Window;

            CancelDownload(window);
        }

        private void DownloadAsync()
        {
            _Client = null;

            if (_downloadList.Any())
            {
                DownloadInfo downloadInfo = _downloadList[0];

                string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string localPath = exePath + "\\" + downloadInfo._filePath.Replace('/', '\\');

                if (File.Exists(localPath) == true)
                {
                    _downloadList.RemoveAt(0);

                    if (downloadInfo._completeHandler != null)
                        downloadInfo._completeHandler(localPath);

                    DownloadAsync();
                    return;
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
                _Client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

                var url = Window1._ServerPath + downloadInfo._filePath;

                _Client.DownloadFileAsync(new Uri(url), localPath);

                _ProgressBarWindow.Show();
                _ProgressBarWindow.Left = downloadInfo._window.Left + downloadInfo._window.Width / 2;
                _ProgressBarWindow.Top = downloadInfo._window.Top + downloadInfo._window.Height / 2;

                return;
            }

            _ProgressBarWindow.Hide();
        }

        void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            currentByte = bytesIn - currentByte;
            _ProgressBarWindow.ProgressValue( int.Parse(Math.Truncate(percentage).ToString()));
            _ProgressBarWindow.ProgrssStateDisplay(fileName, currentByte / 1000, totalBytes / 1000, bytesIn / 1000);
            currentByte = bytesIn;
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle error scenario
                //throw e.Error;
            }

            if (e.Cancelled)
            {
                // handle cancelled scenario
            }
            else
            {
                if (_downloadList.Count > 0)
                {
                    DownloadInfo downloadInfo = _downloadList[0];

                    string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string localPath = exePath + "\\" + downloadInfo._filePath.Replace('/', '\\');

                    _downloadList.RemoveAt(0);

                    if (downloadInfo._completeHandler != null)
                        downloadInfo._completeHandler(localPath);
                }
            }

            DownloadAsync();
        }

    }
}
