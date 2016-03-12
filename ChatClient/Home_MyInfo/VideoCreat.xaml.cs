using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
//using AForge.Video;
//using AForge.Video.DirectShow;
//using AForge.Video.FFMPEG;
using ChatEngine;
using System.Windows.Media;

using ControlExs;
using ANYCHATAPI;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for VideoCreat.xaml
    /// </summary>
    public partial class VideoCreat : BaseWindow
    {
        //public VideoCaptureDevice localWebCam = null;
        //FilterInfoCollection localWebCamCollection = null;
        System.Drawing.Bitmap _bitmap = null;
        //VideoFileWriter _writer;
        System.Windows.Threading.DispatcherTimer disapthcerTimer;
        Image cameraImg = null;
        bool firstcapture = false;
        
        // 2013-12-30: GreenRose
        public string _strRecordFilePath = string.Empty;

        // 2013-12-31: GreenRose
        System.Windows.Threading.DispatcherTimer _stopRecTimer = null;

        // 2013-12-18: GreenRose
        static AnyChatCoreSDK.NotifyMessage_CallBack OnNotifyMessageCallback = new
            AnyChatCoreSDK.NotifyMessage_CallBack(NotifyMessage_CallBack);

        static AnyChatCoreSDK.VideoData_CallBack OnVideoDataCallback = new
            AnyChatCoreSDK.VideoData_CallBack(VideoData_CallBack);

        // 2014-01-17: GreenRose
        static AnyChatCoreSDK.RecordCallBack OnRecordCallBack = new
            AnyChatCoreSDK.RecordCallBack(RecordData_CallBack);

        // 委托句柄定义
        public static AnyChatCoreSDK.NotifyMessage_CallBack NotifyMessageHandler = null;
        public static AnyChatCoreSDK.VideoData_CallBack VideoDataHandler = null;

        // 2014-01-17: GrenRose
        public static AnyChatCoreSDK.RecordCallBack RecordHandler = null;

        public static int g_selfUserId = -1;
        public static int g_otherUserId = -1;

        string _strFileName = string.Empty;

        // 2014-02-09: GreenRose
        public MyHome myHome = null;

        public VideoCreat()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            disapthcerTimer = new System.Windows.Threading.DispatcherTimer();
            disapthcerTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);
            disapthcerTimer.Tick += new EventHandler(Each_Tick);

            // 2013-12-31: GreenRose
            _stopRecTimer = new System.Windows.Threading.DispatcherTimer();
            _stopRecTimer.Interval = new TimeSpan(0, 0, 0, 0, 60000);
            _stopRecTimer.Tick += new EventHandler(StopRecTimer_Tick);

            //if (localWebCam != null)
            //{
            //    if (localWebCam.IsRunning)
            //        localWebCam.Stop();
            //    localWebCam = null;
            //}
        }

        // 2013-05-24: GreenRose Created
        private void InitAllSetting()
        {
            AnyChatCoreSDK.SetNotifyMessageCallBack(OnNotifyMessageCallback, 0);
            AnyChatCoreSDK.SetVideoDataCallBack(AnyChatCoreSDK.PixelFormat.BRAC_PIX_FMT_RGB24, OnVideoDataCallback, 0);

            // 2014-01-17: GreenRose
            AnyChatCoreSDK.SetRecordCallBack(OnRecordCallBack, 0);

            ulong dwFuncMode = AnyChatCoreSDK.BRAC_FUNC_VIDEO_CBDATA | AnyChatCoreSDK.BRAC_FUNC_AUDIO_AUTOPLAY | AnyChatCoreSDK.BRAC_FUNC_CHKDEPENDMODULE
                | AnyChatCoreSDK.BRAC_FUNC_AUDIO_VOLUMECALC | AnyChatCoreSDK.BRAC_FUNC_NET_SUPPORTUPNP | AnyChatCoreSDK.BRAC_FUNC_FIREWALL_OPEN
                | AnyChatCoreSDK.BRAC_FUNC_AUDIO_AUTOVOLUME | AnyChatCoreSDK.BRAC_FUNC_CONFIG_LOCALINI;

            AnyChatCoreSDK.InitSDK(IntPtr.Zero, dwFuncMode);
            //AnyChatCoreSDK.Connect("demo.anychat.cn", 8906);
            //AnyChatCoreSDK.Connect("98.126.164.98", 8906);
            //AnyChatCoreSDK.Connect("192.168.0.2", 8906);            
            AnyChatCoreSDK.Connect(Login._ServerServiceUri, 8906);            
            AnyChatCoreSDK.Login("1", "", 0);
            AnyChatCoreSDK.EnterRoom(1, "", 0);


            NotifyMessageHandler = new AnyChatCoreSDK.NotifyMessage_CallBack(NotifyMessageCallbackDelegate);
            VideoDataHandler = new AnyChatCoreSDK.VideoData_CallBack(VideoDataCallbackDelegate);
            RecordHandler = new AnyChatCoreSDK.RecordCallBack(RecordDataCallBackDelegate);
        }

        public static void NotifyMessage_CallBack(int dwNotifyMsg, int wParam, int lParam, int userValue)
        {
            if (NotifyMessageHandler != null)
                NotifyMessageHandler(dwNotifyMsg, wParam, lParam, userValue);
        }

        public void NotifyMessageCallbackDelegate(int dwNotifyMsg, int wParam, int lParam, int userValue)
        {

            switch (dwNotifyMsg)
            {
                case AnyChatCoreSDK.WM_GV_CONNECT:
                    if (wParam != 0)
                    {
                        //msglabel.Content = "连接服务器成功";
                    }
                    else
                    {
                        //msglabel.Content = "连接服务器失败";
                    }
                    break;
                case AnyChatCoreSDK.WM_GV_LOGINSYSTEM:
                    if (lParam == 0)
                    {
                        g_selfUserId = wParam;
                    }
                    break;
                case AnyChatCoreSDK.WM_GV_ENTERROOM:
                    if (lParam == 0)
                    {
                        AnyChatCoreSDK.UserSpeakControl(-1, true);
                        AnyChatCoreSDK.UserCameraControl(-1, true);
                    }
                    break;
                case AnyChatCoreSDK.WM_GV_ONLINEUSER:
                    {
                        //OpenRemoteUserVideo(true);
                        //OpenRemoteUserSpeak(true);
                    }
                    break;
                case AnyChatCoreSDK.WM_GV_USERATROOM:
                    if (lParam != 0)
                    {
                        //OpenRemoteUserVideo(true);
                        //OpenRemoteUserSpeak(true);
                    }
                    else
                    {
                        if (wParam == g_otherUserId)
                        {
                            g_otherUserId = -1;
                            //OpenRemoteUserVideo(true);
                            //OpenRemoteUserSpeak(true);
                        }
                    }
                    break;
                case AnyChatCoreSDK.WM_GV_LINKCLOSE:
                    {
                        //AnyChatCoreSDK.LeaveRoom(Convert.ToInt32(m_roomInfo.Id));
                        //AnyChatCoreSDK.Logout();
                        //AnyChatCoreSDK.Release();

                        //InitAllSetting();
                    }
                    break;
                default:
                    break;
            }
        }

        public static void VideoData_CallBack(int userId, IntPtr buf, int len, AnyChatCoreSDK.BITMAPINFOHEADER bitMap, int userValue)
        {
            if (VideoDataHandler != null)
                VideoDataHandler(userId, buf, len, bitMap, userValue);
        }

        // 2014-01-17: GreenRose
        public static void RecordData_CallBack(int userId, string filePath, int param, bool recordType, int userValue)
        {
            if (RecordHandler != null)
                RecordHandler(userId, filePath, param, recordType, userValue);
        }

        // 2014-01-17: GreenRose
        public void RecordDataCallBackDelegate(int userId, string filePath, int param, bool recordType, int userValue)
        {
            FileInfo fi = new FileInfo(filePath);

            string path = System.Windows.Forms.Application.StartupPath + "\\Record\\";
            //_strRecordFilePath = path + Login._UserInfo.Id + System.DateTime.Now.ToString("yyyyMMddHHmm") + fi.Extension;
            _strRecordFilePath = path + Login._UserInfo.Id + _strFileName + fi.Extension;

            fi.MoveTo(_strRecordFilePath);
        }

        public void VideoDataCallbackDelegate(int userId, IntPtr buf, int len, AnyChatCoreSDK.BITMAPINFOHEADER bitMap, int userValue)
        {
            int stride = bitMap.biWidth * 3;
            BitmapSource bs = BitmapSource.Create(bitMap.biWidth, bitMap.biHeight, 96, 96, PixelFormats.Bgr24, null, buf, len, stride);

            TransformedBitmap RotateBitmap = new TransformedBitmap();
            RotateBitmap.BeginInit();
            RotateBitmap.Source = bs;
            RotateBitmap.Transform = new RotateTransform(180);
            RotateBitmap.EndInit();
            RotateBitmap.Freeze();

            Action action = new Action(delegate()
            {
                Dispatcher.BeginInvoke(new Action(delegate()
                {
                    if (userId == g_selfUserId)
                    {
                        cameraImg.Source = RotateBitmap;
                    }                    
                }), null);
            });
            action.BeginInvoke(null, null);
        }

        public void OpenRemoteUserVideo(bool bFlag)
        {
            int usercount = 0;
            AnyChatCoreSDK.GetOnlineUser(null, ref usercount);

            if (usercount > 0)
            {
                int[] useridarray = new int[usercount];
                AnyChatCoreSDK.GetOnlineUser(useridarray, ref usercount);
                for (int i = 0; i < usercount; i++)
                {                    
                    AnyChatCoreSDK.UserCameraControl(useridarray[i], bFlag);
                    g_otherUserId = useridarray[i];
                    break;
                }
            }
        }

        public void OpenRemoteUserSpeak(bool bFlag)
        {
            int usercount = 0;
            AnyChatCoreSDK.GetOnlineUser(null, ref usercount);

            if (usercount > 0)
            {
                int[] useridarray = new int[usercount];
                AnyChatCoreSDK.GetOnlineUser(useridarray, ref usercount);
                for (int i = 0; i < usercount; i++)
                {
                    AnyChatCoreSDK.UserSpeakControl(useridarray[i], bFlag);
                    g_otherUserId = useridarray[i];
                    break;
                }
            }
        }

        private void InitWebCam()
        {
            cameraImg = new Image();
            cameraImg.Margin = new Thickness(0);
            BitmapImage reveri = new BitmapImage();
            reveri.BeginInit();
            reveri.UriSource = new Uri("/Resources;component/image/NoCamera.png", UriKind.RelativeOrAbsolute);
            reveri.EndInit();
            cameraImg.Source = reveri;
            videoGrid.Children.Add(cameraImg);

            // 2014-01-17: GreenRose
            InitAllSetting();

            //try
            //{
            //    localWebCamCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //    //this.videoCapDevices.ItemsSource = localWebCamCollection;
            //    for (int i = 0; i < localWebCamCollection.Count; i++)
            //    {
            //        this.videoCapDevices.Items.Add(localWebCamCollection[i].MonikerString);
            //    }

            //    localWebCam = new VideoCaptureDevice(localWebCamCollection[0].MonikerString);
            //    localWebCam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
            //    localWebCam.DesiredFrameSize = new System.Drawing.Size(350, 229);
            //    localWebCam.Start();
            //}
            //catch (Exception)
            //{ }
        }

        // 캠으로부터 화상을 얻어 현시한다.

        //private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        //{
        //    try
        //    {
        //        System.Drawing.Image img = (System.Drawing.Bitmap)eventArgs.Frame.Clone();
        //        _bitmap = (System.Drawing.Bitmap)eventArgs.Frame.Clone();

        //        MemoryStream ms = new MemoryStream();
        //        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        BitmapImage bi = new BitmapImage();
        //        bi.BeginInit();
        //        bi.StreamSource = ms;
        //        bi.EndInit();

        //        bi.Freeze();
        //        Dispatcher.BeginInvoke(new ThreadStart(delegate
        //        {
        //            cameraImg.Source = bi;
        //        }));
        //    }
        //    catch (Exception ex)
        //    {
        //        string strMessage = ex.Message;
        //    }
        //}

        private void Redy_Click(object sender, RoutedEventArgs e)
        {
            redy.IsEnabled = false;
            //videoCapDevices.IsEnabled = true;
            start.IsEnabled = true;
            preview.IsEnabled = false;
            save.IsEnabled = false;
            stop.IsEnabled = false;
            //firstcapture = true;

            videoGrid.Children.Clear();
            InitWebCam();

            //SelectVideoView selectVideoView = new SelectVideoView();
            //selectVideoView.Show();
            
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            _stopRecTimer.IsEnabled = true;
            firstcapture = true;
            stop.IsEnabled = true;
            start.IsEnabled = false;
            StartProc();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            if (_stopRecTimer != null)
                _stopRecTimer.IsEnabled = false;

            start.IsEnabled = true;
            stop.IsEnabled = false;
            preview.IsEnabled = true;
            save.IsEnabled = true;

            AnyChatCoreSDK.StreamRecordCtrl(-1, false, 0, 0);
            //disapthcerTimer.IsEnabled = false;
            //_writer.Close();
        }

        private void preview_Click(object sender, RoutedEventArgs e)
        {
            stop.IsEnabled = false;
            save.IsEnabled = true;
            start.IsEnabled = false;
            redy.IsEnabled = true;

            //string path = @"c:\\Record\" + Login._UserInfo.Id + System.DateTime.Now.ToLongDateString() + ".avi";

            if (File.Exists(_strRecordFilePath))
            {
                //MediaElement media = new MediaElement();
                //media.Margin = new Thickness(0);
                //videoGrid.Children.Clear();
                //videoGrid.Children.Add(media);
                //media.LoadedBehavior = MediaState.Manual;
                //media.UnloadedBehavior = MediaState.Stop;
                //media.Source = new Uri(_strRecordFilePath, UriKind.Absolute);
                //media.Play();

                Utils.FileExecute(_strRecordFilePath);

            }
        }

        
        private void save_Click(object sender, RoutedEventArgs e)
        {
            //string filePath = "C:\\Record\\" + Login._UserInfo.Id + ".avi";

            //Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();

            //open.Multiselect = false;
            //open.FileName = filePath;
            //open.Filter = "AVI Files (*.avi)|*.avi";

            //if ((bool)open.ShowDialog())
            //{
            //    if (filePath == open.FileName)
            //    {
            //        Uploader(open.FileName, open.OpenFile());
            //    }

            //}

            try
            {
                //System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
                //fileDialog.Multiselect = false;
                //fileDialog.Filter = "AVI Files (*.avi)|*.avi|WMV Files (*.wmv)|*.wmv|ALL Files (*.*)|*.*";
                //fileDialog.Title = "비디오파일선택";

                //if (System.Windows.Forms.DialogResult.OK == fileDialog.ShowDialog())
                //{                    
                    //string fileName = fileDialog.FileName;
                string fileName = _strRecordFilePath;
                System.IO.FileInfo fileSize = new System.IO.FileInfo(fileName);
                if (fileSize.Length > 1024 * 10 * 10 * 10 * 10 / 2)
                {
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "文件大小必须小于 5MB.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);

                    return;
                }

                string strImageFile = fileName.Replace(".mp4", ".jpg");

                string strUri = Login._ServerPath;
                if (strUri[strUri.Length - 1] != '/')
                {
                    strUri = strUri + "/";
                }

                // 2014-02-09: GReenRose
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                {
                    if (Main.loadWnd == null)
                    {
                        Main.loadWnd = new LoadingWnd();
                        Main.loadWnd.Owner = myHome;
                        Main.loadWnd.ShowDialog();
                    }
                }));

                strUri += "FaceUpload.php";
                WebUploader.GetInstance().UploadFile(fileName, strUri, FileUploadComplete, this);

                _strImageFile = strImageFile;
                _strUri = strUri;
                //WebUploader.GetInstance().UploadFile(strImageFile, strUri, ImageFileUploadComplete, this);

                this.Close();
                //if (UpdateFile(fileName))
                //{
                //    if (UpdateFile(strImageFile))
                //    {
                //        byte[] bit = new byte[0];

                //        VideoInfo videoInfo = new VideoInfo();
                //        videoInfo.Data = bit;
                //        videoInfo.UserId = Login._UserInfo.Id;
                //        videoInfo.ImgData = bit;
                //        int nIndex = fileName.LastIndexOf("\\");
                //        videoInfo.ImgName = fileName.Substring(nIndex + 1);
                //        Login._ClientEngine.Send(NotifyType.Request_VideoUpload, videoInfo);
                //    }                
                //}
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        string _strImageFile = string.Empty;
        string _strUri = string.Empty;

        // 2013-12-31: GreenRose
        private void FileUploadComplete(string strFileName)
        {
            //byte[] bit = new byte[0];

            //VideoInfo videoInfo = new VideoInfo();
            //videoInfo.Data = bit;
            //videoInfo.UserId = Login._UserInfo.Id;
            //videoInfo.ImgData = bit;
            //int nIndex = strFileName.LastIndexOf("\\");
            //videoInfo.ImgName = strFileName.Substring(nIndex + 1);
            //Login._ClientEngine.Send(NotifyType.Request_VideoUpload, videoInfo);

            WebUploader.GetInstance().UploadFile(_strImageFile, _strUri, ImageFileUploadComplete, this);
        }

        // 2013-12-31: GreenRose
        private void ImageFileUploadComplete(string strFileName)
        {
            byte[] bit = new byte[0];

            //strFileName = strFileName.Replace(".gif", ".avi");
            strFileName = strFileName.Replace(".jpg", ".mp4");
            VideoInfo videoInfo = new VideoInfo();
            videoInfo.Data = bit;
            videoInfo.UserId = Login._UserInfo.Id;
            videoInfo.ImgData = bit;
            int nIndex = strFileName.LastIndexOf("\\");
            videoInfo.ImgName = strFileName.Substring(nIndex + 1);
            Login._ClientEngine.Send(NotifyType.Request_VideoUpload, videoInfo);
        }

        // 2013-12-29: GreenRose
        private bool UpdateFile(string strFileName)
        {
            try
            {
                string strUri = Login._ServerPath;
                if (strUri[strUri.Length - 1] != '/')
                {
                    strUri = strUri + "/";
                }

                strUri += "FaceUpload.php";

                System.Net.WebClient wc = new System.Net.WebClient();
                wc.Credentials = System.Net.CredentialCache.DefaultCredentials;
                wc.UploadFile(strUri, strFileName);
                wc.Dispose();
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);

                return false;
            }

            return true;
        }

        private static void Uploader(string filename, Stream Data)
        {
            byte[] bit = new byte[0];

            BinaryReader reader = new BinaryReader(Data);
            string path = @"c:\\Record\" + Login._UserInfo.Id + ".avi";
            char[] delimiterChars = { ':', '\\' };
            string[] words = path.Split(delimiterChars);
            int count = words.Length;

            //FileStream fstream = new FileStream(path, FileMode.Open);

            bit = reader.ReadBytes((int)Data.Length);
            //fstream.Close();

            VideoInfo videoInfo = new VideoInfo();
            videoInfo.Data = bit;
            videoInfo.UserId = words[count - 1].ToString();


            ///////////////////////////
            Image myImg = new Image();
            string imgPath = @"c:\\Record\" + Login._UserInfo.Id + ".gif";
            BitmapImage myBit = new BitmapImage();
            myBit.BeginInit();
            myBit.UriSource = new Uri(imgPath, UriKind.Absolute);
            myBit.EndInit();
            /*********************************/
            char[] imgChars = { ':', '\\' };
            string[] imgwords = imgPath.Split(delimiterChars);
            int imgcount = imgwords.Length;
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(myBit));

            byte[] imgbit = new byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                imgbit = stream.ToArray();
                stream.Close();

                videoInfo.ImgData = imgbit;
                videoInfo.ImgName = imgwords[count - 1].ToString();
            }
            //////////////////////////////

            Login._ClientEngine.Send(NotifyType.Request_VideoUpload, videoInfo);

        }

        public void StartProc()
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\Record\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            
            try
            {
                //_writer = new VideoFileWriter();

                //_strRecordFilePath = path + Login._UserInfo.Id + System.DateTime.Now.ToString("yyyyMMddHHmm") + ".avi";
                //_writer.Open(_strRecordFilePath, 640, 480, 25, VideoCodec.MPEG4);

                //StartVideCapture();

                _strFileName = System.DateTime.Now.ToString("yyyyMMddHHmm");
                AnyChatCoreSDK.StreamRecordCtrl(-1, true, 0, 0);
                AnyChatCoreSDK.SnapShot(-1, 0, 0);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
           
        }

        private void StartVideCapture()
        {
            disapthcerTimer.IsEnabled = true;
        }

        private void Each_Tick(Object obj, EventArgs ev)
        {
            try
            {
                //if (firstcapture == true)
                //{
                //    var encoder = new BmpBitmapEncoder();
                //    RenderTargetBitmap bitmap = new RenderTargetBitmap(320, 240, 96, 96, PixelFormats.Pbgra32);
                //    bitmap.Render(cameraImg);
                //    BitmapFrame frame = BitmapFrame.Create(bitmap);
                //    encoder.Frames.Add(frame);

                //    using (var stream = File.Create(_strRecordFilePath.Replace(".avi", ".gif")))
                //    {
                //        encoder.Save(stream);
                //    }
                //    firstcapture = false;
                //}


                //_writer.WriteVideoFrame(_bitmap);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        // 2013-12-31: GreenRose
        private void StopRecTimer_Tick(Object obj, EventArgs ev)
        {
            try
            {
                _stopRecTimer.IsEnabled = false;

                start.IsEnabled = true;
                stop.IsEnabled = false;
                preview.IsEnabled = true;
                save.IsEnabled = true;

                AnyChatCoreSDK.StreamRecordCtrl(-1, false, 0, 0);
                //disapthcerTimer.IsEnabled = false;
                //_writer.Close();
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                //if (localWebCam != null)
                //{
                //    if (localWebCam.IsRunning)
                //        localWebCam.Stop();
                //}

                if (disapthcerTimer != null)
                {
                    disapthcerTimer.IsEnabled = false;
                }

                AnyChatCoreSDK.LeaveRoom(-1);
                AnyChatCoreSDK.Logout();
                AnyChatCoreSDK.Release();

                //if (_writer != null)
                //    _writer.Close();

                Main.videoCreat = null;
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void videoCapDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (localWebCam != null)
            //    localWebCam.Stop();

            //localWebCam = new VideoCaptureDevice(this.videoCapDevices.SelectedValue.ToString());
            //localWebCam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
            //localWebCam.DesiredFrameSize = new System.Drawing.Size(350, 229);
            //localWebCam.Start();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
