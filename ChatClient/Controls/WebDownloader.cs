using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Windows;
using System.Security.Cryptography;

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


        // 2014-04-07: GreenRose
        public AlbermControl ctrlAlbum = null;
        public SelectAlbermControl ctrlSelectAlbum = null;

        public bool DownloadFile(string filePath, DownloadCompleteHandler completeHandler, Control control )
        {
            DownloadInfo newInfo = new DownloadInfo();
            newInfo._filePath = filePath;
            newInfo._completeHandler = completeHandler;
            newInfo._window = Window.GetWindow(control);

            //newInfo._window.Closed += new EventHandler(_window_Closed);
            DownloadAsync(newInfo);

            return true;
        }

        void _window_Closed(object sender, EventArgs e)
        {
            Window window = sender as Window;
        }

        private void DownloadAsync(DownloadInfo newInfo)
        {
            _Client = null;

            DownloadInfo downloadInfo = newInfo;

            //string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string exePath = System.Windows.Forms.Application.StartupPath;
            string localPath = exePath + "\\" + downloadInfo._filePath.Replace('/', '\\');

            if (File.Exists(localPath) == true)
            {
                if (downloadInfo._completeHandler != null)
                    downloadInfo._completeHandler(localPath);

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
            _Client.DownloadFileCompleted += DownloadFileCompleted(localPath, downloadInfo);

            var url = Login._ServerPath + downloadInfo._filePath;

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

            if (ctrlAlbum != null)
                ctrlAlbum.progressUpdate.Value = int.Parse(Math.Truncate(percentage).ToString());

            if (ctrlSelectAlbum != null)
                ctrlSelectAlbum.progressUpdate.Value = int.Parse(Math.Truncate(percentage).ToString());

            //_ProgressBarWindow.ProgressValue( int.Parse(Math.Truncate(percentage).ToString()));
            //_ProgressBarWindow.ProgrssStateDisplay(fileName, currentByte / 1000, totalBytes / 1000, bytesIn / 1000);
            currentByte = bytesIn;
        }

        private AsyncCompletedEventHandler DownloadFileCompleted(string strFileName, DownloadInfo downloadInfo)
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

        public string DecryptFile( string strFileName )
        {
            FileInfo fileInfo = new FileInfo( strFileName );

            string decName = string.Format("{0}\\{1}_dec{2}", fileInfo.Directory, fileInfo.Name.Substring(0, fileInfo.Name.Length-fileInfo.Extension.Length), fileInfo.Extension);

            if (File.Exists(decName) == false )
            {
                //File.SetAttributes(decName, FileAttributes.Normal); 
                //File.Delete(decName);

                Decrypt(fileInfo.FullName, decName);
            }


            //fileInfo.IsReadOnly = false;
            //fileInfo.Delete();

            //File.Move(decName, fileInfo.FullName);
            return decName;
        }

        private void Decrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "PAKCHOLJIN1977530";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }

    }
}
