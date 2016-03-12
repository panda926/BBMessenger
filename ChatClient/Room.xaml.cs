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
//using System.Drawing;

using System.Net.Sockets;
using ChatEngine;

//using System.Windows.Forms;
using Microsoft.DirectX.DirectSound;
using System.Threading;
using System.IO;
using g711audio;
using System.Diagnostics;

using WPFMediaKit.DirectShow;

// 작성날자: 2013-03-05
// 작성자  : GreenRose
// 내용    : 사용자가 선택한 방에서의 기능구현
namespace ChatClient
{
    public partial class Room : Window
    {
        public RoomInfo m_roomInfo = new RoomInfo();
        public RoomDetailInfo m_roomDetailInfo = new RoomDetailInfo();

        FlowDocument m_flowDoc = new FlowDocument();
        List<IconInfo> m_listPresentInfo = new List<IconInfo>();      // 서버로부터 보내여지는 선물목록
        List<IconInfo> m_listEmotionInfo = new List<IconInfo>();      // 서버로부터 보내여지는 Emoticon정보목록

        System.Windows.Threading.DispatcherTimer disapthcerTimer = null;

        public Room()
        {
            InitializeComponent();
        }

        // 초기설정을 진행한다.
        private void InitComponentAndHandler()
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;  // 현재 이 프로그램이 실행되는 등록부의 경로.

            Initialize();   // 음성채팅을 필요한 초기화설정

            Call();         // 음성채팅의 형태설정.

            this.richTextMessage.CaretPosition = this.richTextMessage.Document.ContentStart; // MessageEdit창의 카숄을 첫위치로 옮긴다.

            try
            {
                videoCapElement.VideoCaptureDevice = WPFMediaKit.DirectShow.Controls.MultimediaUtil.VideoInputDevices[0]; // 웹캠을 플래이하기위한 장치 설정.
            }
            catch (Exception ex)
            {
                this.VidoeChatStart.IsEnabled = false;
            }

            this.audioLevel.Items.Clear();                  // AudioLevel을 설정해주는 ComboBox를 초기화해준다.
            this.audioLevel.Items.Add("A-Law");
            this.audioLevel.Items.Add("u-Law");
            this.audioLevel.Items.Add("None");
            this.audioLevel.SelectedIndex = 0;

            this.Count.Content = "UserCount: " + m_roomInfo.UserCount;              // 방에 입장한 사용자의 수

            this.title.Content = "RoomID: " + m_roomInfo.Id;                        // 방의 아이디

            this.owner.Content = "RoomOwner: " + m_roomInfo.Owner;                  // 방장을 현시한다.
            this.UserID.Content = "UserID: " + Main_Pan._UserInfo.Id;               // 현재사용자의 아이디를 현시한다.
            this.Point.Content = "Point: " + Main_Pan._UserInfo.Point.ToString();   // 현재사용자의 포인트


            if (m_roomInfo.Icon != string.Empty)   // Room의 아이콘을 설정한다.             
            {
                BitmapImage roomIcon = new BitmapImage(new Uri("\\\\" + Main_Pan._ServerUri + "\\" + m_roomInfo.Icon, UriKind.RelativeOrAbsolute));
                this.RoomIcon.Source = roomIcon;
            }

            if (!Main_Pan._UserInfo.Id.Equals(m_roomInfo.Owner)) // 현재의 사용자가 입장한 방이 자기가 창조한 방이 아니라면 VideoChatStart단추를 비능동으로 한다.
            {
                this.VidoeChatStart.IsEnabled = false;
            }

            BitmapImage bitmapVoice = new BitmapImage(new Uri(strPath + "\\voice.png", UriKind.Relative));
            VoiceChatStartImage.Source = bitmapVoice;

            BitmapImage bitmapVideo = new BitmapImage(new Uri(strPath + "\\video.png", UriKind.Relative));
            VidoeChatStartImage.Source = bitmapVideo;
        }

        // WebCam으로부터 이미지를 캡쳐하여 바이트형으로 변환한다.
        private void SendVideo()
        {
            double dpiX = 190;
            double dpiY = 190;

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)(videoCapElement.Width * dpiX / 96), (int)(videoCapElement.Height * dpiY / 96), dpiX, dpiY, PixelFormats.Pbgra32);
            bmp.Render(videoCapElement);

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.QualityLevel = 15;

            byte[] bit = new byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                bit = stream.ToArray();
                stream.Close();

                VideoInfo videoInfo = new VideoInfo();
                videoInfo.Data = bit;
                videoInfo.UserId = Main_Pan._UserInfo.Id;

                Main_Pan._ClientEngine.Send(NotifyType.Request_VideoChat, videoInfo);
            }
        }

        // 서버로부터 받은 비디오데이터를 화면에 현시한다.
        private void ReceiveVideo(byte[] receiveData)
        {
            MemoryStream ms = new MemoryStream(receiveData);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();

            this.videoPlayer.Source = bitmapImage;
            this.videoPlayer.Visibility = Visibility.Visible;
            //this.border4.Visibility = Visibility.Visible;

            
        }

        public void OnReceive(NotifyType type, Socket socket, BaseInfo baseInfo)
        {
            switch (type)
            {
                case NotifyType.Reply_Error:        // 서버로부터 통신오류응답이 온경우.
                    { 
                    }
                    break;

                case NotifyType.Reply_StringChat:
                    {
                        StringInfo strInfo = (StringInfo)baseInfo;
                        AddMessageToHistoryTextBox(strInfo);
                    }
                    break;

                case NotifyType.Reply_RoomDetail:
                    {
                        m_roomDetailInfo = (RoomDetailInfo)baseInfo;
                        ShowUserList(m_roomDetailInfo);
                    }
                    break;

                case NotifyType.Reply_VoiceChat:
                    {
                        VoiceInfo voiceInfo = (VoiceInfo)baseInfo;
                        ReceiveVoiceInfo(voiceInfo.Data);
                    }
                    break;

                case NotifyType.Reply_VideoChat:
                    {
                        VideoInfo videoInfo = (VideoInfo)baseInfo;
                        ReceiveVideo(videoInfo.Data);
                    }
                    break;

                case NotifyType.Reply_Give:
                    {
                        GiveInfo giveInfo = (GiveInfo)baseInfo;
                        SendPresent(giveInfo.PresentId);
                    }
                    break;
            }
        }

        // 서버로부터 들어오는 메세지를 표시하여준다.
        public void AddMessageToHistoryTextBox(StringInfo strInfo)
        {
            // 아이디를 현시하여준다.
            Paragraph para = new Paragraph();
            Run userID = new Run(strInfo.UserId + " : " + "\n");
            userID.Foreground = System.Windows.Media.Brushes.YellowGreen;
            userID.FontSize = 14;
            para.Inlines.Add(new Bold(userID));

            para.LineHeight = 2;
            m_flowDoc.Blocks.Add(para);

            // 메세지를 현시하여준다.
            MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(strInfo.String));

            FlowDocument flowDocument = new FlowDocument();
            TextRange otherTextRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            otherTextRange.Load(ms, DataFormats.Rtf);
            string rtf = "";
            //rtf = ASCIIEncoding.Default.GetString(ms.ToArray());
            rtf = otherTextRange.Text;

            Paragraph newPara = new Paragraph();

            string strMessage = "";
            for (int i = 0; i < rtf.Length; i++)
            {
                if (rtf[i] == '<')
                {
                    if (strMessage != string.Empty)
                    {
                        Run run = new Run(strMessage);
                        run.FontSize = 16;
                        para.Inlines.Add(run);
                        strMessage = "";
                    }

                    string strUri = "";
                    for (int j = i + 1; j < rtf.IndexOf('>'); j++)
                    {
                        strUri += rtf[j];
                    }
                    Image img = new Image();

                    BitmapImage bitMapImage = new BitmapImage();
                    bitMapImage.BeginInit();
                    bitMapImage.UriSource = new Uri(strUri.Substring(5), UriKind.RelativeOrAbsolute);
                    bitMapImage.EndInit();

                    img.Source = bitMapImage;
                    img.Width = 25;
                    InlineUIContainer ui = new InlineUIContainer(img);

                    para.Inlines.Add(ui);

                    rtf = rtf.Substring(rtf.IndexOf('>') + 1);
                    i = -1;
                }
                else
                {
                    if(rtf[i] != '\r' && rtf[i] != '\n')
                        strMessage += rtf[i];
                }
            }

            if (strMessage != string.Empty)
            {
                Run run = new Run(strMessage);
                run.FontSize = 16;
                para.Inlines.Add(run);
            }

            //FlowDocument flowDocument = new FlowDocument();
            //TextRange otherTextRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            //otherTextRange.Load(ms, DataFormats.Rtf);

            //RichTextBox richTextBox = new RichTextBox();
            //richTextBox.Document = flowDocument;

            //foreach (Block block in richTextBox.Document.Blocks)
            //{
            //    if (block is Paragraph)
            //    {
            //        Paragraph newPara = new Paragraph();

            //        Paragraph otherPara = (Paragraph)block;
            //        foreach (Inline inLine in otherPara.Inlines)
            //        {
            //            //if (inLine is Run)
            //            //{
            //                Run run = (Run)inLine;
            //                string strText = run.Text;

            //                if(strText.Substring(0, 3).Equals("file:"))
            //                {
            //                    Image img = new Image();

            //                    BitmapImage bitMapImage = new BitmapImage();
            //                    bitMapImage.BeginInit();
            //                    bitMapImage.UriSource = new Uri(strText.Substring(2), UriKind.RelativeOrAbsolute);
            //                    bitMapImage.EndInit();

            //                    img.Source = bitMapImage;
            //                    img.Width = 30;
            //                    InlineUIContainer ui = new InlineUIContainer(img);

            //                    newPara.Inlines.Add(ui);
            //                }

            //                newPara.Inlines.Add(run.Text);
            //            //}
            //        }

            //        m_flowDoc.Blocks.Add(newPara);
            //    }
            //}
            //textRange.Load(ms, DataFormats.Rtf);
            
            historyTextBox.Document = m_flowDoc;
            this.historyTextBox.ScrollToEnd();
        }

        // 사용자가 방에 입장할때.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitComponentAndHandler();
            
            Main_Pan._ClientEngine.AttachHandler(OnReceive);            
        }

        private void ShowUserList(RoomDetailInfo roomDetailInfo)
        {
            // Contacts목록에 현재 방에 입장한 유저들의 목록을 현시한다.
            this.UserTreeView.Items.Clear();
            TreeViewItem treeViewItem = new TreeViewItem();
            treeViewItem.Header = "Users";
            treeViewItem.Foreground = Brushes.Brown;
            treeViewItem.FontSize = 14;
            treeViewItem.IsExpanded = true;
            for (int i = 0; i < roomDetailInfo.Users.Count; i++)
            {
                TreeViewItem treeViewChildItem = new TreeViewItem();
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;

                Image icon = new Image();
                icon = GetLevelIcon(roomDetailInfo.Users[i].LevelNum);
                icon.Width = 20;
                icon.Height = 20;

                Label lbl = new Label();
                lbl.Content = roomDetailInfo.Users[i].Id;
                lbl.Foreground = Brushes.Brown;
                lbl.FontSize = 14;

                stackPanel.Children.Add(icon);
                stackPanel.Children.Add(lbl);

                treeViewChildItem.Header = stackPanel;
                treeViewItem.Items.Add(treeViewChildItem);
            }

            UserTreeView.Items.Add(treeViewItem);

            // Present들을 얻어 Present컨트롤에 배치하여준다.
            m_listPresentInfo = new List<IconInfo>();
            m_listPresentInfo = roomDetailInfo.Presents;

            BitmapImage bitmapPresentImage = new BitmapImage(new Uri("\\\\" + Main_Pan._ServerUri + "\\" + m_listPresentInfo[0].Icon, UriKind.RelativeOrAbsolute));
            PresentImage.Source = bitmapPresentImage;

            // Emoticon들을 얻어 Emoticon컨트롤에 배치하여준다.
            m_listEmotionInfo = new List<IconInfo>();
            m_listEmotionInfo = roomDetailInfo.Emoticons;

            BitmapImage bitmapEmoticonImage = new BitmapImage(new Uri("\\\\" + Main_Pan._ServerUri + "\\" + m_listEmotionInfo[0].Icon, UriKind.RelativeOrAbsolute));
            EmoticonImage.Source = bitmapEmoticonImage;

            // 사용자가 입장하거나 퇴장할때마다 현재방의 사용자수를 나타낸다.
            this.Count.Content = "UserCount: " + roomDetailInfo.Users.Count.ToString();

            // richTextMessage에 초점을 맞춘다.
            this.richTextMessage.Focus();
        }

        private Image GetLevelIcon(int levelNum)
        {
            Image levelImg = new Image();
            levelImg.Width = 19;
            levelImg.Height = 16;
            levelImg.Margin = new Thickness(0);
            levelImg.HorizontalAlignment = HorizontalAlignment.Left;

            BitmapImage levelBit = new BitmapImage();
            levelBit.BeginInit();

            if (levelNum == 1)
            {
                levelBit.UriSource = new Uri("../images/grade/16.PNG", UriKind.Relative);
            }
            else if (levelNum == 2)
            {
                levelBit.UriSource = new Uri("../images/grade/6.PNG", UriKind.Relative);
            }
            else if (levelNum == 3)
            {
                levelBit.UriSource = new Uri("../images/grade/5.PNG", UriKind.Relative);
            }
            else if (levelNum >= 4 && levelNum <= 7)
            {
                levelBit.UriSource = new Uri("../images/grade/17.PNG", UriKind.Relative);
            }
            else if (levelNum >= 8 && levelNum <= 11)
            {
                levelBit.UriSource = new Uri("../images/grade/7.PNG", UriKind.Relative);
            }
            else if (levelNum >= 12 && levelNum <= 15)
            {
                levelBit.UriSource = new Uri("../images/grade/4.PNG", UriKind.Relative);
            }
            else if (levelNum >= 16 && levelNum <= 31)
            {
                levelBit.UriSource = new Uri("../images/grade/18.PNG", UriKind.Relative);
            }
            else if (levelNum >= 32 && levelNum <= 47)
            {
                levelBit.UriSource = new Uri("../images/grade/8.PNG", UriKind.Relative);
            }
            else if (levelNum >= 48 && levelNum <= 63)
            {
                levelBit.UriSource = new Uri("../images/grade/3.PNG", UriKind.Relative);
            }
            else if (levelNum >= 64 && levelNum <= 127)
            {
                levelBit.UriSource = new Uri("../images/grade/19.PNG", UriKind.Relative);
            }
            else if (levelNum >= 128 && levelNum <= 191)
            {
                levelBit.UriSource = new Uri("../images/grade/9.PNG", UriKind.Relative);
            }
            else if (levelNum >= 192 && levelNum <= 255)
            {
                levelBit.UriSource = new Uri("../images/grade/2.PNG", UriKind.Relative);
            }
            else if (levelNum >= 256 && levelNum <= 511)
            {
                levelBit.UriSource = new Uri("../images/grade/20.PNG", UriKind.Relative);
            }
            else if (levelNum >= 512 && levelNum <= 767)
            {
                levelBit.UriSource = new Uri("../images/grade/10.PNG", UriKind.Relative);
            }
            else if (levelNum >= 768 && levelNum <= 1024)
            {
                levelBit.UriSource = new Uri("../images/grade/1.PNG", UriKind.Relative);
            }

            levelBit.EndInit();
            levelImg.Source = levelBit;

            return levelImg;
        }

        // 비디오채팅단추를 눌렀을때의 기능구현.
        private void VidoeChatStart_Click(object sender, RoutedEventArgs e)
        {
            disapthcerTimer = new System.Windows.Threading.DispatcherTimer();
            disapthcerTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);
            disapthcerTimer.Tick += new EventHandler(Each_Tick);
            disapthcerTimer.Start();
        }

        private void Each_Tick(Object obj, EventArgs ev)
        {
            try
            {
                SendVideo();
            }
            catch (Exception ex)
            { 
            }
        }

        // 메세지편집창에서 메세지를 보내는 함수.
        private void richTextMessage_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                richTextMessage.ScrollToHome();
                richTextMessage.CaretPosition = richTextMessage.Document.ContentStart;
                richTextMessage.Focus();
            }
        }

        private void richTextMessage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FlowDocument flowDocument = new FlowDocument();
                
                foreach (Block block in richTextMessage.Document.Blocks)
                {
                    if (block is Paragraph)
                    {
                        Paragraph newPara = new Paragraph();

                        Paragraph para = (Paragraph)block;
                        foreach (Inline inLine in para.Inlines)
                        {
                            if (inLine is InlineUIContainer)
                            {
                                InlineUIContainer uiContainer = (InlineUIContainer)inLine;
                                if (uiContainer.Child is Image)
                                {
                                    Image img = (Image)uiContainer.Child;
                                    Run run = new Run("<" + img.Source.ToString() + ">");
                                    newPara.Inlines.Add(run);
                                }
                            }
                            else if(inLine is Run)
                            {
                                Run run = (Run)inLine;
                                newPara.Inlines.Add(run.Text);
                            }
                        }

                        flowDocument.Blocks.Add(newPara);
                    }
                }

                TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);

                if (textRange.Text != string.Empty)
                {
                    MemoryStream ms = new MemoryStream();
                    textRange.Save(ms, DataFormats.Rtf);

                    string strMessage = Encoding.Default.GetString(ms.ToArray());

                    SendMessage(strMessage);
                }

                TextRange messageTextRange = new TextRange(richTextMessage.Document.ContentStart, richTextMessage.CaretPosition);
                messageTextRange.Text = "";
                //richTextMessage.Document.Blocks.Clear();
                richTextMessage.ScrollToHome();
                richTextMessage.CaretPosition = richTextMessage.Document.ContentStart;
            }
        }

        // 메세지를 서버에 전달한다.
        public void SendMessage(string strMessage)
        {
            StringInfo messageInfo = new StringInfo();
            messageInfo.String = strMessage;
            messageInfo.UserId = Main_Pan._UserInfo.Id;

            Main_Pan._ClientEngine.Send(NotifyType.Request_StringChat, messageInfo);
        }

        // Emoticon을 보내려할때.
        private void Emoticon_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Point pos = PointToScreen(Mouse.GetPosition(this));

            var selector = new Emoticon.EmoticonSelector();
            selector.listEmoticonInfo = m_listEmotionInfo;

            selector.EmoticonSelected += (s1, e1) => OnEmoticonSelected(((Emoticon.EmoticonSelector)s1).UUri);

            selector.Top = pos.Y;
            selector.Left = pos.X;

            //selector.m_room = this;
            
            selector.Show();
        }

        // 선물을 보내려할때.
        private void Present_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Point pos = PointToScreen(Mouse.GetPosition(this));
            
            var selector = new Present.PresentSelector();
            selector.listPresentInfo = m_listPresentInfo;
            selector.PresentSelected += (s1, e1) => OnPresentSelected(((Present.PresentSelector)s1).UUri, ((Present.PresentSelector)s1).ID);

            selector.Top = pos.Y;
            selector.Left = pos.X;
            selector.Show();
        }

        // Emoticon을 선택한후의 다음처리.
        private void OnEmoticonSelected(string strUri)
        {
            InsertImageToRichTextBox(strUri);
            richTextMessage.Focus();
        }

        // 선물을 보낸다.
        private void SendPresent(string strPresentID)
        {
            foreach (IconInfo presentInfo in m_listPresentInfo)
            {
                if (presentInfo.Id.Equals(strPresentID))
                {
                    InsertImageToRichTextBox("\\\\" + Main_Pan._ServerUri + "\\" + presentInfo.Icon);
                    Main_Pan._UserInfo.Point = Main_Pan._UserInfo.Point - presentInfo.Point;
                    this.Point.Content = Main_Pan._UserInfo.Point.ToString();
                }
            }
        }

        // RichTextBox에 이미지를 보여준다.
        private void InsertImageToRichTextBox(string strUri)
        {
            try
            {
                TextPointer tp = richTextMessage.CaretPosition;
                Image img = new Image();

                BitmapImage bitMapImage = new BitmapImage();
                bitMapImage.BeginInit();
                bitMapImage.UriSource = new Uri(strUri, UriKind.RelativeOrAbsolute);
                bitMapImage.EndInit();

                img.Source = bitMapImage;
                img.Width = 25;
                InlineUIContainer ui = new InlineUIContainer(img);
                tp.Paragraph.Inlines.Add(ui);

                richTextMessage.CaretPosition = richTextMessage.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);

                richTextMessage.Focus();
            }
            catch (Exception ex)
            {
                
            }
        }

        // 선물을 선택한후의 다음처리.
        private void OnPresentSelected(string strUri, string strID)
        {
            GiveInfo giveInfo = new GiveInfo();
            giveInfo.UserId = Main_Pan._UserInfo.Id;
            giveInfo.PresentId = strID;

            Main_Pan._ClientEngine.Send(NotifyType.Request_Give, giveInfo);
        }

        // 음성채팅시작
        // 작성날자: 2013-03-07
        // 작성자  : GreenRose

        private Microsoft.DirectX.DirectSound.CaptureBufferDescription captureBufferDescription;
        private AutoResetEvent autoResetEvent;
        private WaveFormat waveFormat;
        private Capture capture;
        private int bufferSize;
        private CaptureBuffer captureBuffer;
        private Device device;
        private SecondaryBuffer playbackBuffer;
        private BufferDescription playbackBufferDescription;
        private volatile bool bIsCallActive;                 //Tells whether we have an active c
        private Vocoder vocoder;
        private byte[] byteData = new byte[1024];   //Buffer to store the data received.
        private volatile int nUdpClientFlag;                 //Flag used to close the ud
        private bool bStop;                         //Flag to end the Start and Receive threads.
        private Notify notify;

        private void VoiceChartStart_Click(object sender, RoutedEventArgs e)
        {
            //Call();
            InitializeCall();
        }

        private void Initialize()
        {
            try
            {
                device = new Device();
                System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(this);
                device.SetCooperativeLevel(helper.Handle, CooperativeLevel.Normal);

                CaptureDevicesCollection captureDeviceCollection = new CaptureDevicesCollection();

                DeviceInformation deviceInfo = captureDeviceCollection[0];

                capture = new Capture(deviceInfo.DriverGuid);

                short channels = 1; //Stereo.
                short bitsPerSample = 16; //16Bit, alternatively use 8Bits.
                int samplesPerSecond = 22050; //11KHz use 11025 , 22KHz use 22050, 44KHz use 44100 etc.

                //Set up the wave format to be captured.
                waveFormat = new WaveFormat();
                waveFormat.Channels = channels;
                waveFormat.FormatTag = WaveFormatTag.Pcm;
                waveFormat.SamplesPerSecond = samplesPerSecond;
                waveFormat.BitsPerSample = bitsPerSample;
                waveFormat.BlockAlign = (short)(channels * (bitsPerSample / (short)8));
                waveFormat.AverageBytesPerSecond = waveFormat.BlockAlign * samplesPerSecond;

                captureBufferDescription = new Microsoft.DirectX.DirectSound.CaptureBufferDescription();
                captureBufferDescription.BufferBytes = waveFormat.AverageBytesPerSecond / 5;//approx 200 milliseconds of PCM data.
                captureBufferDescription.Format = waveFormat;

                playbackBufferDescription = new BufferDescription();
                playbackBufferDescription.BufferBytes = waveFormat.AverageBytesPerSecond / 5;
                playbackBufferDescription.Format = waveFormat;
                playbackBuffer = new SecondaryBuffer(playbackBufferDescription, device);

                bufferSize = captureBufferDescription.BufferBytes;

                bIsCallActive = false;
                nUdpClientFlag = 0;
            }
            catch (Exception ex)
            {
                this.VoiceChartStart.IsEnabled = false;
            }
        }

        private void Call()
        {
            try
            {
                if (audioLevel.SelectedIndex == 0)
                {
                    vocoder = Vocoder.ALaw;
                }
                else if (audioLevel.SelectedIndex == 1)
                {
                    vocoder = Vocoder.uLaw;
                }
                else if (audioLevel.SelectedIndex == 2)
                {
                    vocoder = Vocoder.None;
                }

                //InitializeCall();
            }
            catch (Exception ex)
            {
                
            }
        }

        private void InitializeCall()
        {
            Thread senderThread = new Thread(new ThreadStart(SendVoiceInfo));
            senderThread.Start();
        }

        // 음성정보를 전달한다.
        private void SendVoiceInfo()
        {
            try
            {
                captureBuffer = new CaptureBuffer(captureBufferDescription, capture);

                CreateNotifyPositions();

                int halfBuffer = bufferSize / 2;
                captureBuffer.Start(true);

                bool readFirstBufferPart = true;
                int offset = 0;

                MemoryStream memStream = new MemoryStream(halfBuffer);
                bStop = false;

                VoiceInfo voiceInfo = new VoiceInfo();

                while (!bStop)
                {
                    autoResetEvent.WaitOne();
                    memStream.Seek(0, SeekOrigin.Begin);
                    captureBuffer.Read(offset, memStream, halfBuffer, LockFlag.None);
                    readFirstBufferPart = !readFirstBufferPart;
                    offset = readFirstBufferPart ? 0 : halfBuffer;

                    if (vocoder == Vocoder.ALaw)
                    {
                        byte[] dataToWrite = ALawEncoder.ALawEncode(memStream.GetBuffer());

                        voiceInfo.UserId = Main_Pan._UserInfo.Id;
                        voiceInfo.Data = dataToWrite;

                        Main_Pan._ClientEngine.Send(NotifyType.Request_VoiceChat, voiceInfo);
                    }
                    else if (vocoder == Vocoder.uLaw)
                    {
                        byte[] dataToWrite = MuLawEncoder.MuLawEncode(memStream.GetBuffer());

                        voiceInfo.UserId = Main_Pan._UserInfo.Id;
                        voiceInfo.Data = dataToWrite;

                        Main_Pan._ClientEngine.Send(NotifyType.Request_VoiceChat, voiceInfo);
                    }
                    else
                    {
                        byte[] dataToWrite = memStream.GetBuffer();

                        voiceInfo.UserId = Main_Pan._UserInfo.Id;
                        voiceInfo.Data = dataToWrite;

                        Main_Pan._ClientEngine.Send(NotifyType.Request_VoiceChat, voiceInfo);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                captureBuffer.Stop();
                nUdpClientFlag = 0;
            }   
        }

        // 서버로부터 음성정보를 전달받는다.
        private void ReceiveVoiceInfo(byte[] byteData)
        {
            try
            {
                //Initialize();
                bStop = false;

                //G711 compresses the data by 50%, so we allocate a buffer of double
                //the size to store the decompressed data.
                byte[] byteDecodedData = new byte[byteData.Length * 2];

                //Decompress data using the proper vocoder.
                if (vocoder == Vocoder.ALaw)
                {
                    ALawDecoder.ALawDecode(byteData, out byteDecodedData);
                }
                else if (vocoder == Vocoder.uLaw)
                {
                    MuLawDecoder.MuLawDecode(byteData, out byteDecodedData);
                }
                else
                {
                    byteDecodedData = new byte[byteData.Length];
                    byteDecodedData = byteData;
                }


                //Play the data received to the user.
                playbackBuffer = new SecondaryBuffer(playbackBufferDescription, device);
                playbackBuffer.Write(0, byteDecodedData, LockFlag.None);
                playbackBuffer.Play(0, BufferPlayFlags.Default);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                nUdpClientFlag += 1;
            }
        }

        private void CreateNotifyPositions()
        {
            try
            {
                autoResetEvent = new AutoResetEvent(false);
                notify = new Notify(captureBuffer);
                BufferPositionNotify bufferPositionNotify1 = new BufferPositionNotify();
                bufferPositionNotify1.Offset = bufferSize / 2 - 1;
                bufferPositionNotify1.EventNotifyHandle = autoResetEvent.SafeWaitHandle.DangerousGetHandle();
                BufferPositionNotify bufferPositionNotify2 = new BufferPositionNotify();
                bufferPositionNotify2.Offset = bufferSize - 1;
                bufferPositionNotify2.EventNotifyHandle = autoResetEvent.SafeWaitHandle.DangerousGetHandle();

                notify.SetNotificationPositions(new BufferPositionNotify[] { bufferPositionNotify1, bufferPositionNotify2 });
            }
            catch (Exception ex)
            {
                
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            RoomInfo roomInfo = m_roomInfo;
            if(disapthcerTimer != null)
                disapthcerTimer.Stop();

            //sound.StopLoop = true;
            bStop = true;

            Main_Pan._ClientEngine.Send(NotifyType.Request_OutRoom, roomInfo);
        }


        //private DirectSoundHelper sound;
        //private byte[] buffer = new byte[2205];
        //private Thread th;

        //Stopwatch m_watch = new Stopwatch();

        //private void VoiceChartStart_Click(object sender, RoutedEventArgs e)
        //{
        //    sound = new DirectSoundHelper();
        //    sound.OnBufferFulfill += new EventHandler(SendVoiceBuffer);

        //    th = new Thread(new ThreadStart(sound.StartCapturing));
        //    th.IsBackground = true;
        //    th.Start();

        //    sound.StopLoop = false;
        //}

        //void SendVoiceBuffer(object VoiceBuffer, EventArgs e)
        //{
        //    byte[] Buffer = (byte[])VoiceBuffer;

        //    VoiceInfo voiceInfo = new VoiceInfo();
        //    voiceInfo.UserId = Main_Pan._UserInfo.Id;
        //    voiceInfo.Data = Buffer;

        //    m_watch.Start();
        //    Main_Pan._ClientEngine.Send(NotifyType.Request_VoiceChat, voiceInfo);
        //}

        //private void ReceiveVoiceInfo(byte[] byteData)
        //{
        //    try
        //    {
        //        //m_watch.Stop();
        //        //MessageBox.Show(m_watch.Elapsed.Milliseconds.ToString());
        //        //Stopwatch watch = new Stopwatch();
        //        //watch.Start();
        //        sound.PlayReceivedVoice(byteData);
        //        //watch.Stop();

        //        //this.title.Content = watch.Elapsed.Milliseconds.ToString() + ":" + m_watch.Elapsed.Milliseconds.ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }

    enum Vocoder
    {
        ALaw,   //A-Law vocoder.
        uLaw,   //u-Law vocoder.
        None,   //Don't use any vocoder.
    }
}
