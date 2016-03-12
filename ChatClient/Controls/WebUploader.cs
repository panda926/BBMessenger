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
    delegate void UploadCompleteHandler( string uploadPath );

    class UploadInfo
    {
        public string _filePath = string.Empty;
        public string _uploadUrl = string.Empty;

        public UploadCompleteHandler _completeHandler;
        public Window _window;
    }

    class WebUploader
    {
        static WebUploader _instance = null;

        static public WebUploader GetInstance()
        {
            if (_instance == null)
                _instance = new WebUploader();

            return _instance;
        }

        ProgressBarWindow _ProgressBarWindow = new ProgressBarWindow();
        double currentByte = 0;
        string fileName = null;

        WebClient _Client = null;

        List<UploadInfo> _uploadList = new List<UploadInfo>();

        // 2014-04-07: GreenRose
        public AlbermControl ctrlAlbum = null;

        public bool UploadFile(string filePath, string uploadUrl, UploadCompleteHandler completeHandler, Control control )
        {
            if (File.Exists(filePath) == false)
                return false;

            //long uploadSize = (new FileInfo(filePath)).Length;


            //string[] folderNames = uploadUrl.Split('/');
            //string folderPath = Utils.GetExePath();

            //for (int i = 0; i < folderNames.Length; i++)
            //{
            //    folderPath = string.Format( "{0}\\{1}", folderPath, folderNames[i] );

            //    if (Directory.Exists(folderPath) == false)
            //        Directory.CreateDirectory(folderPath);
            //}


            //string fileName = Utils.GetFileName( filePath );

            //while (true)
            //{
            //    string path = string.Format("{0}\\{1}", folderPath, fileName);

            //    if (File.Exists(path))
            //    {
            //        FileInfo fileInfo = new FileInfo(path);

            //        if (fileInfo.Length == uploadSize)
            //            break;

            //        string[] part = fileName.Split('.');
            //        fileName = part[0] + "1." + part[1];
            //        continue;
            //    }

            //    File.Copy(filePath, path);
            //    filePath = path;
            //    break;
            //}


            UploadInfo newInfo = new UploadInfo();
            newInfo._filePath = filePath;
            newInfo._uploadUrl = uploadUrl;
            newInfo._completeHandler = completeHandler;
            newInfo._window = Window.GetWindow(control);

            //newInfo._window.Closed += new EventHandler(_window_Closed);
            
            _uploadList.Add(newInfo);

            if (_uploadList.Count <= 1)
                UploadAsync();

            return true;
        }

        public void CancelUpload(Window window)
        {
            if (_uploadList.Count == 0)
                return;

            if (_Client != null)
            {
                UploadInfo uploadInfo = _uploadList[0];

                if (uploadInfo._window == window)
                {
                    _Client.CancelAsync();
                }
            }

            int i = 0;
            while (i < _uploadList.Count)
            {
                UploadInfo nextInfo = _uploadList[i];

                if (nextInfo._window == window)
                {
                    _uploadList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        void _window_Closed(object sender, EventArgs e)
        {
            Window window = sender as Window;

            CancelUpload(window);
        }

        private void UploadAsync()
        {
            _Client = null;

            if (_uploadList.Any())
            {
                UploadInfo uploadInfo = _uploadList[0];

                _Client = new WebClient();
                _Client.Credentials = System.Net.CredentialCache.DefaultCredentials;
                _Client.Proxy = null;

                _Client.UploadProgressChanged += new UploadProgressChangedEventHandler(_Client_UploadProgressChanged);
                _Client.UploadFileCompleted += new UploadFileCompletedEventHandler(_Client_UploadFileCompleted);

                var url = Login._ServerPath + "upload.php";

                //_Client.UploadFileAsync( new Uri(url), "POST", uploadInfo._filePath, uploadInfo._uploadUrl);
                _Client.UploadFileAsync(new Uri(uploadInfo._uploadUrl), "POST", uploadInfo._filePath, uploadInfo._uploadUrl);
                //_Client.UploadFile(new Uri(uploadInfo._uploadUrl), uploadInfo._filePath);

                //_ProgressBarWindow.Show();
                //_ProgressBarWindow.Left = uploadInfo._window.Left + uploadInfo._window.Width / 2;
                //_ProgressBarWindow.Top = uploadInfo._window.Top + uploadInfo._window.Height / 2;

                return;
            }

            //_ProgressBarWindow.Hide();
        }

        void _Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
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
                if (_uploadList.Count > 0)
                {
                    UploadInfo uploadInfo = _uploadList[0];

                    //string uploadPath = System.Text.Encoding.UTF8.GetString(e.Result);
                    string uploadPath = uploadInfo._filePath;

                    _uploadList.RemoveAt(0);

                    if (uploadInfo._completeHandler != null)
                        uploadInfo._completeHandler(uploadPath);
                }
            }

            UploadAsync();
        }

        void _Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesSent.ToString());
            double totalBytes = double.Parse(e.TotalBytesToSend.ToString());
            double percentage = bytesIn / totalBytes * 100;
            currentByte = bytesIn - currentByte;

            if (ctrlAlbum != null)
                ctrlAlbum.progressUpdate.Value = int.Parse(Math.Truncate(percentage).ToString());

            //_ProgressBarWindow.ProgressValue( int.Parse(Math.Truncate(percentage).ToString()));
            //_ProgressBarWindow.ProgrssStateDisplay(fileName, currentByte / 1000, totalBytes / 1000, bytesIn / 1000);
            currentByte = bytesIn;
        }

    }
}
