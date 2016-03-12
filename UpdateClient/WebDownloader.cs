using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Windows;


// 2014-01-21: GreenRose
namespace UpdateClient
{
    delegate void DownloadCompleteHandler( string filePath );

    class DownloadInfo
    {
        public string _filePath = string.Empty;
        public DownloadCompleteHandler _completeHandler;
        public string _strServerPath = string.Empty;
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

        public static ProgressForm progressForm = null;
        public static string localRoot = null;

        WebClient _Client = null;

        public bool DownloadFile(string filePath, DownloadCompleteHandler completeHandler, string strServerPath )
        {
            DownloadInfo newInfo = new DownloadInfo();
            newInfo._filePath = filePath;
            newInfo._completeHandler = completeHandler;
            newInfo._strServerPath = strServerPath;
            
            DownloadAsync(newInfo);

            return true;
        }        

        private void DownloadAsync(DownloadInfo newInfo)
        {
            _Client = null;

            DownloadInfo downloadInfo = newInfo;            
            
            string exePath = System.Windows.Forms.Application.StartupPath + "\\";
            if (localRoot != null)
                exePath = localRoot;

            string localPath = exePath + downloadInfo._filePath.Replace('/', '\\');

            if (File.Exists(localPath) == true)
            {
                try
                {
                    File.Delete(localPath);
                }
                catch (Exception)
                { }
            }

            string[] folders = downloadInfo._filePath.Split('\\');
            string fullPath = exePath;

            for (int i = 0; i < folders.Length - 1; i++)
            {
                if (folders[i] == "Update")
                    folders[i] = "NewUpdate";

                fullPath = string.Format("{0}\\{1}", fullPath, folders[i]);

                if (Directory.Exists(fullPath) == false)
                    Directory.CreateDirectory(fullPath);
            }

            _Client = new WebClient();
            _Client.Proxy = null;

            _Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            _Client.DownloadFileCompleted += DownloadFileCompleted(localPath, downloadInfo);

            var url = "http://" + downloadInfo._strServerPath + downloadInfo._filePath.Replace("\\", "/");

            _Client.DownloadFileAsync(new Uri(url), localPath);            

            return;            
        }

        void client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            currentByte = bytesIn - currentByte;
            progressForm.ProgressValue( int.Parse(Math.Truncate(percentage).ToString()));
            progressForm.ProgrssStateDisplay(fileName, currentByte / 1000, totalBytes / 1000, bytesIn / 1000);
            currentByte = bytesIn;
        }

        private AsyncCompletedEventHandler DownloadFileCompleted(string strFileName, DownloadInfo downloadInfo)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {
                var _filename = strFileName;

                if (e.Error != null)
                {
                }

                if (downloadInfo._completeHandler != null)
                    downloadInfo._completeHandler(strFileName);
            };

            return new AsyncCompletedEventHandler(action);
        }

    }
}
