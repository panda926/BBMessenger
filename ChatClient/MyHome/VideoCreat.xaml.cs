using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using ChatEngine;
using System.Windows.Media;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for VideoCreat.xaml
    /// </summary>
    public partial class VideoCreat : Window
    {
        public VideoCaptureDevice localWebCam = null;
        FilterInfoCollection localWebCamCollection = null;
        System.Drawing.Bitmap _bitmap = null;
        VideoFileWriter _writer;
        System.Windows.Threading.DispatcherTimer disapthcerTimer;
        Image cameraImg = null;
        bool firstcapture = true;
        

        public VideoCreat()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            disapthcerTimer = new System.Windows.Threading.DispatcherTimer();
            disapthcerTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);
            disapthcerTimer.Tick += new EventHandler(Each_Tick);

            if (localWebCam != null)
            {
                if (localWebCam.IsRunning)
                    localWebCam.Stop();
                localWebCam = null;
            }
        }

        private void InitWebCam()
        {
            cameraImg = new Image();
            cameraImg.Margin = new Thickness(0);
            BitmapImage reveri = new BitmapImage();
            reveri.BeginInit();
            reveri.UriSource = new Uri("../image/NoCamera.png", UriKind.RelativeOrAbsolute);
            reveri.EndInit();
            cameraImg.Source = reveri;
            videoGrid.Children.Add(cameraImg);

            try
            {
                localWebCamCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                localWebCam = new VideoCaptureDevice(localWebCamCollection[0].MonikerString);
                localWebCam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
                localWebCam.DesiredFrameSize = new System.Drawing.Size(350, 229);
                localWebCam.Start();
            }
            catch (Exception)
            { }
        }

        // 캠으로부터 화상을 얻어 현시한다.

        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (System.Drawing.Bitmap)eventArgs.Frame.Clone();
                _bitmap = (System.Drawing.Bitmap)eventArgs.Frame.Clone();

                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                bi.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    cameraImg.Source = bi;
                }));
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
            }
        }

        private void Redy_Click(object sender, RoutedEventArgs e)
        {
            start.IsEnabled = true;
            firstcapture = true;

            videoGrid.Children.Clear();
            InitWebCam();
            
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            stop.IsEnabled = true;
            start.IsEnabled = false;
            StartProc();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            stop.IsEnabled = false;
            preview.IsEnabled = true;
            disapthcerTimer.IsEnabled = false;
            _writer.Close();
        }

        private void preview_Click(object sender, RoutedEventArgs e)
        {
            stop.IsEnabled = false;
            save.IsEnabled = true;

            string path = @"c:\\Record\" + Window1._UserInfo.Id + ".avi";

            if (File.Exists(path))
            {
                MediaElement media = new MediaElement();
                media.Margin = new Thickness(0);
                videoGrid.Children.Clear();
                videoGrid.Children.Add(media);
                media.LoadedBehavior = MediaState.Manual;
                media.UnloadedBehavior = MediaState.Stop;
                media.Source = new Uri(path, UriKind.Absolute);
                media.Play();

            }
        }

        
        private void save_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "C:\\Record\\" + Window1._UserInfo.Id + ".avi";

            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();

            open.Multiselect = false;
            open.FileName = filePath;
            open.Filter = "AVI Files (*.avi)|*.avi";

            if ((bool)open.ShowDialog())
            {
                if (filePath == open.FileName)
                {
                    Uploader(open.FileName, open.OpenFile());
                }

            }
        }

        private static void Uploader(string filename, Stream Data)
        {
            byte[] bit = new byte[0];

            BinaryReader reader = new BinaryReader(Data);
            string path = @"c:\\Record\" + Window1._UserInfo.Id + ".avi";
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
            string imgPath = @"c:\\Record\" + Window1._UserInfo.Id + ".gif";
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

            Window1._ClientEngine.Send(NotifyType.Request_VideoUpload, videoInfo);

        }

        public void StartProc()
        {
            string path = "c:\\Record";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            try
            {
                _writer = new VideoFileWriter();

                _writer.Open("c:\\Record\\" + Window1._UserInfo.Id + ".avi", 640, 480, 25, VideoCodec.WMV2);

                StartVideCapture();
            }
            catch (Exception ex)
            {
                string strMsg = ex.Message;
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
                if (firstcapture == true)
                {
                    var encoder = new BmpBitmapEncoder();
                    RenderTargetBitmap bitmap = new RenderTargetBitmap(320, 240, 96, 96, PixelFormats.Pbgra32);
                    bitmap.Render(cameraImg);
                    BitmapFrame frame = BitmapFrame.Create(bitmap);
                    encoder.Frames.Add(frame);

                    using (var stream = File.Create("c:\\Record\\" + Window1._UserInfo.Id + ".gif"))
                    {
                        encoder.Save(stream);
                    }
                    firstcapture = false;
                }


                _writer.WriteVideoFrame(_bitmap);
            }
            catch (Exception ex)
            {
                string strMsg = ex.Message;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (disapthcerTimer != null)
            {
                disapthcerTimer.IsEnabled = false;
            }
            if (_writer != null)
                _writer.Close();
            Main.videoCreat = null;
        }

        
    }
}
