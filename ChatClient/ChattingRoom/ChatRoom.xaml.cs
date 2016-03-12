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
using System.Windows.Navigation;
using System.Windows.Shapes;

//using System.Windows.Forms;
using System.Reflection;
using ChatClient;
using ChatEngine;
using System.Net.Sockets;
using System.Net;
using System.Collections;
//using AForge.Video;
//using AForge.Video.DirectShow;
using System.Diagnostics;

//using NSpeex;
//using NAudio.CoreAudioApi;
//using NAudio.Wave;

using ANYCHATAPI;
using System.IO;
using System.Threading;

using ControlExs;
using System.Runtime.InteropServices;

//using iConfServer.NET;
//using iConfClient.NET;

// 작성자   : GreenRose
// 설명     : 채팅방을 창조하며 비디오통화와 음성통화 본문전송을 진행한다.
// 작성기간 : 2013-03-20부터 ~ 

namespace ChatClient
{

    #region StaticClass

    public static class ManagerMessage
    {
        public const string ERROR_FONT_SETTING      = "字体设置出现错误 请从新设置.";
        public const string ERROR_WEB_CAM           = "您的电脑没有安装视频或视频设置出现错误 请检查视频链接.";
        public const string MESSAGE_WELCOME         = "欢迎进入此房间. 祝您游戏愉快.";
        public const string ERROR_MICROPHONE        = "您的电脑没有安装麦克风或麦克风设置出现错误 请检查麦克风链接";
        public const string ERROR_MANY_CHAT         = "3000字以上的文字不能发送. 请修改发送的内容.";
        public const string ALERT_VOICE_MESSAGE     = " 正在对你发送语音信息.";
        public const string ALERT_END_VOICE         = "语音信息接收失败.";
        public const string NOT_ENOUGH_CASH         = "金币余额不足，请冲值";
        public const string NOT_ENOUGH_POINT        = "目前您的余额不足2分钟后将要退出此频道.";
        public const string ALERT_END_CHAT          = "将要退出此频道. 如需继续聊天 请充值元宝.";
        public const string ALERT_USER_OUT          = "0从此频道退出.";
        public const string ALERT_END_CHATTING      = "因对方退出频道 结束聊天.";
        public const string ALERT_SERVICE_OUT       = "因宝贝退出频道 结束聊天.";

        public const string ALERT_PRESENT_SENDING = "确定赠送？";
    }


    public static class FontInfo
    {
        public const string FONT_STYLE_ITALIC       = "a";
        public const string FONT_STYLE_NORMAL       = "b";
        public const string FONT_WEIGHT_BOLD        = "c";
        public const string FONT_WEIGHT_NORMAL      = "d";
    }


    public static class ButtonContent
    {
        public const string VIDEO_SEND_BUTTON       = "连接我的视频";
        public const string VIDEO_END_BUTTON        = "视频传送中断";
        public const string VOICE_SEND_BUTTON       = "连接麦克风";
        public const string VOICE_END_BUTTON        = "语音传送中断";
    }


    public static class LabelContents
    {
        public const string CUR_RELogin_PRICE_LBL = "现有元宝:";
        public const string CUR_USE_PRICE_LBL       = "";

        public const string CUR_RELogin_POINT_LBL    = "";
        public const string CUR_USE_POINT_LBL       = "";

        public const string PRICE_PER_SECOND = "每分中聊天元宝:";
        public const string MY_CAM_LBL = "我的视频";
        public const string YOU_CAM_LBL = "对方视频";

        public const string TOTAL_PRESENT_PRICE = "礼物总金额:";

        public const string TOTAL_GET_CASH = "聊天元宝:";

        public const string TOTAL_GET_POINT = "积分元宝:";
    }

    #endregion


    //#region VoiceClass

    //public class VolumeUpdatedEventArgs : EventArgs
    //{
    //    public int Volume { get; set; }
    //}

    //public class JitterBufferWaveProvider : NAudio.Wave.WaveStream
    //{
    //    private readonly SpeexDecoder decoder = new SpeexDecoder(BandMode.Narrow, true);
    //    private readonly SpeexJitterBuffer jitterBuffer;

    //    private readonly NAudio.Wave.WaveFormat waveFormat;
    //    private readonly object readWriteLock = new object();

    //    public JitterBufferWaveProvider()
    //    {
    //        waveFormat = new NAudio.Wave.WaveFormat(decoder.FrameSize * 50, 16, 1);
    //        jitterBuffer = new SpeexJitterBuffer(decoder);
    //    }

    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        int peakVolume = 0;
    //        int bytesRead = 0;
    //        lock (readWriteLock)
    //        {
    //            while (bytesRead < count)
    //            {
    //                if (exceedingBytes.Count != 0)
    //                {
    //                    buffer[bytesRead++] = exceedingBytes.Dequeue();
    //                }
    //                else
    //                {
    //                    short[] decodedBuffer = new short[decoder.FrameSize * 2];
    //                    jitterBuffer.Get(decodedBuffer);
    //                    for (int i = 0; i < decodedBuffer.Length; ++i)
    //                    {
    //                        if (bytesRead < count)
    //                        {
    //                            short currentSample = decodedBuffer[i];
    //                            peakVolume = currentSample > peakVolume ? currentSample : peakVolume;
    //                            BitConverter.GetBytes(currentSample).CopyTo(buffer, offset + bytesRead);
    //                            bytesRead += 2;
    //                        }
    //                        else
    //                        {
    //                            var bytes = BitConverter.GetBytes(decodedBuffer[i]);
    //                            exceedingBytes.Enqueue(bytes[0]);
    //                            exceedingBytes.Enqueue(bytes[1]);
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        OnVolumeUpdated(peakVolume);

    //        return bytesRead;
    //    }

    //    public override void Write(byte[] buffer, int offset, int count)
    //    {
    //        lock (readWriteLock)
    //        {
    //            jitterBuffer.Put(buffer);
    //        }
    //    }

    //    public override long Length
    //    {
    //        get { return 1; }
    //    }

    //    public override long Position
    //    {
    //        get { return 0; }
    //        set { throw new NotImplementedException(); }
    //    }

    //    public override NAudio.Wave.WaveFormat WaveFormat
    //    {
    //        get
    //        {
    //            return waveFormat;
    //        }
    //    }

    //    public EventHandler<VolumeUpdatedEventArgs> VolumeUpdated;

    //    private void OnVolumeUpdated(int volume)
    //    {
    //        var eventHandler = VolumeUpdated;
    //        if (eventHandler != null)
    //        {
    //            eventHandler.BeginInvoke(this, new VolumeUpdatedEventArgs { Volume = volume }, null, null);
    //        }
    //    }

    //    private readonly Queue<byte> exceedingBytes = new Queue<byte>();
    //}

    //#endregion

    public partial class ChatRoom : BaseWindow
    {
        #region Varialble

        System.Windows.Threading.DispatcherTimer disapthcerTimer        = null;
        System.Windows.Threading.DispatcherTimer dispatcherVideoTimer   = null;
        System.Windows.Threading.DispatcherTimer dispatcherOtherTimer   = null;


        // Font정보보관변수들...

        string                      m_strFontName       = "";
        float                       m_fFontSize         = 12;
        System.Drawing.FontStyle    m_fontStyle         = new System.Drawing.FontStyle();
        System.Drawing.Color        m_fontColor         = new System.Drawing.Color();


        // 기타 성원변수들...

        FlowDocument                m_flowDoc           = new FlowDocument();
        bool                        m_videoSenBtnState  = false;                // 비디오전송단추가 클릭되였는가 식별하는 변수값.
        bool                        m_VoiceSendBtnState = false;

        List<IconInfo>              m_listEmoticons     = new List<IconInfo>(); // 서버로부터 보내여지는 Emoticon정보목록
        List<IconInfo>              m_listPresents = new List<IconInfo>();      // 서버로부터 보내여지는 Present정보목록
        public RoomInfo             m_roomInfo          = new RoomInfo();       // 방정보

        VoiceInfo                   m_SendVoiceInfo     = new VoiceInfo();
        VoiceInfo                   m_voiceInfo         = new VoiceInfo();
        VideoInfo                   m_videoInfo         = new VideoInfo();

        bool                        m_bFirstVideoFrame  = true;
        bool                        m_bFirstVideoFrame1 = true;

        RoomDetailInfo              m_roomDetailInfo    = new RoomDetailInfo();
        bool                        m_bRoomStateChange  = false;
        bool                        m_bEndChatFlag      = false;

        string                      m_strPreviousText   = "";

        UserInfo                    m_userFemaleInfo    = new UserInfo();
        DiceGame                    m_diceGame = null;


        int                         m_nVideoEndStae     = 0;
        int                         m_getMoney = 0;

        //VideoCaptureDevice          m_localWebCam           = null;
        //FilterInfoCollection        m_localWebCamCollection = null;

        System.Windows.Controls.ComboBox m_comboCameraTypes = new System.Windows.Controls.ComboBox();

        List<String> m_MessageQue = new List<String>();

        //private SpeexEncoder encoder;

        //private bool isRecording;
        //private bool isPlaying;

        //private NAudio.Wave.WaveFormat waveFormat;
        //private IWaveIn waveIn;
        //private IWavePlayer waveOut;

        //private JitterBufferWaveProvider waveProvider;


        public string m_strVideoChatPass = "VideoChatStart+GIT";
        public string m_strVideoChatEnd = "VideoChatEnd+GIT";

        public string m_strVoiceChatPass = "VoiceChatStart+GIT";
        public string m_strVoiceChatEnd = "VoiceChatEnd+Git";

        public bool m_bMakingRoom = false;

        private System.Windows.Threading.DispatcherTimer mutiChatClock = null;

        public event EventHandler PresentSelected = delegate { };

        public string ID { get; set; }
        public string UUri { get; set; }

        private bool m_bShowPresent = false;

        OtherWebCamView otherWebCamView = null;

        #endregion


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

        // 2013-12-29: GreenRose
        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr handle, bool invert); 
        
        public ChatRoom()
        {
            InitializeComponent();

            btnMin.Click += new System.Windows.RoutedEventHandler(this.btnMin_Click);
            btnClose.Click += new System.Windows.RoutedEventHandler(this.btnClose_Click);

            btnEmoticon.Click += new System.Windows.RoutedEventHandler(this.btnEmoticon_Click);
            btnFont.Click += new System.Windows.RoutedEventHandler(this.btnFont_Click);

            // 환영 메세지를 출력한다.
            ShowErrorOrMessage(ManagerMessage.MESSAGE_WELCOME);
            Login._ClientEngine.AttachHandler(OnReceive);

            // 2013-12-29: GreenRose
            // remoteVideoImage SourceChangeEvent
            Image image = new Image();
            System.Windows.Application.Current.Properties["OtherWebCamView"] = image;
            var prop = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(Image.SourceProperty, typeof(Image));
            prop.AddValueChanged(remoteVideoImage, SourceChangedHandler);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnEmoticon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Point pos = PointToScreen(Mouse.GetPosition(this));

                var selector = new ChatClient.Emoticon.EmoticonSelector();
                selector.listEmoticonInfo = m_listEmoticons;

                selector.EmoticonSelected += (s1, e1) => OnEmoticonSelected(((ChatClient.Emoticon.EmoticonSelector)s1).UUri);
                selector.EmoticonClosing += OnEmoticonClosing;
                selector.Top = pos.Y;
                selector.Left = pos.X;
                selector.Show();
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void btnFont_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingFont();
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
            finally
            {
                this.messageEditBox.Focus();
            }
        }

        // 2013-12-29: GreenRose
        // remoteVideoImage SourceChangeEvent
        private void SourceChangedHandler(object obj, EventArgs e)
        {
            screenImg.Visibility = Visibility.Visible;
            Image image = obj as Image;
            System.Windows.Application.Current.Properties["OtherWebCamView"] = image;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitAllSetting();
            try
            {
                InitLabelCaption(Login._UserInfo);

                nHour = 0;
                nMinute = 0;
                nSecond = 0;

                if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
                {
                    //button2.IsEnabled = false;
                    //button3.IsEnabled = false;
                    //button4.IsEnabled = false;
                    //numberMark.IsEnabled = false;
                    roomValue.Visibility = Visibility.Hidden;
                    roomValueBt.Visibility = Visibility.Hidden;
                }

                lblWebCam1.Content = LabelContents.MY_CAM_LBL;
                lblWebCam2.Content = LabelContents.YOU_CAM_LBL;

                m_getMoney = Login._UserInfo.Cash;
                // MessageEditBox에 초점을 맞추어놓는다.

                this.messageEditBox.Focus();


                // 항시적으로 비디오가 들어오는가를 감시한다.

                //CheckVideoFrame();


                // 항시적으로 상태를 감시한다.

                CheckOtherState();


                // 환영 메세지를 출력한다.

                //ShowErrorOrMessage(ManagerMessage.MESSAGE_WELCOME);


                // Notify

                //Login._ClientEngine.AttachHandler(OnReceive);
                //Login._VideoEngine.AttachUdpHandler(OnVideoReceive);


                // 사용자가 여자일때 웹캠전송을 진행한다.

                //if (Login._UserInfo.Kind == 1)
                //{
                //    this.numberMark.IsEnabled = false;
                //    this.btnSendMark.IsEnabled = false;
                //    SetVideoChat();
                //}


                // GameWindow창을 띄운다.

                //m_gameWindow = new GameWindow(this);
                //m_gameWindow.SetGameInfo(Login._GameInfo);
                //m_gameWindow.Show();


                // 음성채팅가능.

                EnableoVoiceChat();


                //////////
                //encoder = new SpeexEncoder(BandMode.Narrow);

                //waveProvider = new JitterBufferWaveProvider();
                //waveProvider.VolumeUpdated += HandleVolumeUpdated;
                //waveOut = new WaveOut();
                ////waveOut = new WasapiOut(AudioClientShareMode.Shared, 20);

                //waveOut.Init(waveProvider);

                //waveOut.PlaybackStopped += waveOut_PlaybackStopped;
                //waveOut.Volume = 1.0f;

                m_SendVoiceInfo.UserId = Login._UserInfo.Id;



                mutiChatClock = new System.Windows.Threading.DispatcherTimer();
                mutiChatClock.Interval = new TimeSpan(0, 0, 0, 1);
                mutiChatClock.Tick += new EventHandler(mutiChatClock_Tick);
                mutiChatClock.IsEnabled = true;


                // 2013-06-17: GreenRose
                //ShowPresent();
                PresentSelected += (s1, e1) => OnPresentSelected(((ChatRoom)s1).UUri, ((ChatRoom)s1).ID);

                // 2013-12-26: GreenRose
                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_RoomDetail, this.m_roomInfo);
            }
            catch (Exception)
            { }

            //StartPlaying();
        }


        static int nHour = 0;
        static int nMinute = 0;
        static int nSecond = 0;

        void mutiChatClock_Tick(object sender, EventArgs e)
        {
            if (nSecond >= 59)
            {
                nSecond = -1;
                nMinute++;
            }

            if (nMinute > 59)
            {
                nMinute = 0;
                nHour++;
            }

            if (nHour > 23)
                nHour = 0;

            nSecond++;

            string strSecond = string.Empty;
            string strMinute = string.Empty;
            string strHour = string.Empty;

            if (nSecond < 10)
            {
                strSecond = "0" + nSecond.ToString();
            }
            else
                strSecond = nSecond.ToString();

            if (nMinute < 10)
            {
                strMinute = "0" + nMinute.ToString();
            }
            else
                strMinute = nMinute.ToString();

            if (nHour < 10)
            {
                strHour = "0" + nHour.ToString();
            }
            else
                strHour = nHour.ToString();

            timeChat.Content = strHour + ":" + strMinute + ":" + strSecond;


            // bit per second를 현시한다.
            if (Main._nTotalBytesPerSecond != -1)
            {
                if (Main._nTotalBytesPerSecond != -1)
                {
                    if (this.lblBitPerSecond.Visibility == Visibility.Hidden)
                        this.lblBitPerSecond.Visibility = Visibility.Visible;

                    decimal dBitPerSecond = Main._nTotalBytesPerSecond / 1024;

                    string specifier = "0.00";

                    this.lblBitPerSecond.Content = dBitPerSecond.ToString(specifier) + "/bps";
                    dBitPerSecond = 0;

                    Main._nTotalBytesPerSecond = 0;
                }
            }
        }


        private void ShowPresent()
        {
            try
            {
                foreach (IconInfo presentIconInfo in m_listPresents)
                {
                    //var present = new ChatClient.Present.Present();
                    System.Windows.Controls.Button btImg = new System.Windows.Controls.Button();
                    btImg.Margin = new Thickness(1, 1, 1, 1);
                    btImg.Content = presentIconInfo.Name;
                    btImg.Foreground = new SolidColorBrush(Colors.Black);
                    btImg.Cursor = System.Windows.Input.Cursors.Hand;

                    btImg.Style = (Style)FindResource("NormalButtonStyle");

                    //present.DataContext = new ChatClient.Present.Present(new Uri("/Resources;component/image/present/" + presentIconInfo.Icon, UriKind.RelativeOrAbsolute), presentIconInfo.Name + "( " + presentIconInfo.Point.ToString() + " )", presentIconInfo.Id);

                    //present.DataContext = new ChatClient.Present.Present(presentIconInfo.Name, presentIconInfo.Name + "( " + presentIconInfo.Point.ToString() + " )", presentIconInfo.Id);
                    if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman || m_roomInfo.Point > 0)
                    {
                        btImg.IsEnabled = false;
                    }

                    int id = presentIconInfo.Point;
                    string strUri = presentIconInfo.Icon;

                    btImg.Click += (send, ex) =>
                    {
                        ID = id.ToString();
                        UUri = strUri;
                        PresentSelected(this, EventArgs.Empty);
                    };

                    uniformGrid1.Children.Add(btImg);
                }
            }
            catch (Exception)
            {
            }
        }

        private void OnPresentSelected(string strUri, string strPoint)
        {
            if (Convert.ToInt32(strPoint) < Login._UserInfo.Cash)
            {
                //if (MessageBoxCommon.Show(ManagerMessage.ALERT_PRESENT_SENDING, MessageBoxType.YesNo) == MessageBoxReply.No)
                //    return;
                TempWindowForm tempWindowForm = new TempWindowForm();
                if (QQMessageBox.Show(tempWindowForm, ManagerMessage.ALERT_PRESENT_SENDING, "提示", QQMessageBoxIcon.Question, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                    return;

                InsertPresentToRichTextBox(strUri, strPoint);
            }
            else
            {
                //MessageBoxCommon.Show("帐号元宝不足.", MessageBoxType.Ok);
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, ManagerMessage.NOT_ENOUGH_CASH, "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK) ;
            }
        }

        private void InsertPresentToRichTextBox(string strUri, string strPoint)
        {
            try
            {
                //if (messageEditBox.CaretPosition.Paragraph == null)
                //    messageEditBox.CaretPosition.InsertParagraphBreak();

                //TextPointer tp = messageEditBox.CaretPosition;
                //Image img = new Image();

                //BitmapImage bitMapImage = new BitmapImage();
                //bitMapImage.BeginInit();
                //bitMapImage.UriSource = new Uri("/Resources;component/image/present/" + strUri, UriKind.RelativeOrAbsolute);
                //bitMapImage.EndInit();

                ////img.Source = bitMapImage;
                //img.Width = 25;
                //WpfAnimatedGif.ImageBehavior.SetAnimatedSource(img, bitMapImage);
                //WpfAnimatedGif.ImageBehavior.SetRepeatBehavior(img, System.Windows.Media.Animation.RepeatBehavior.Forever);

                //InlineUIContainer ui = new InlineUIContainer(img);

                //tp.Paragraph.Inlines.Add(ui);


                //Run run = new Run("선물을 보냅니다.");
                //tp.Paragraph.Inlines.Add(run);

                //messageEditBox.CaretPosition = messageEditBox.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);

                //messageEditBox.Focus();

                PresentHistoryInfo presentHistoryInfo = new PresentHistoryInfo();
                presentHistoryInfo.Cash = Convert.ToInt32(strPoint);
                for (int i = 0; i < m_roomDetailInfo.Users.Count; i++)
                {
                    if (m_roomDetailInfo.Users[i].Kind == (int)UserKind.ServiceWoman)
                    {
                        presentHistoryInfo.ReceiveId = m_roomDetailInfo.Users[i].Id;
                        break;
                    }
                }

                strUri = "/Resources;component/image/present/" + strUri;
                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_Give, presentHistoryInfo);

                string strMessage = "<" + strUri + ">" + "礼物赠送成功";
                SendMessage(strMessage);
            }
            catch (Exception)
            {
            }
        }

        // 2013-05-24: GreenRose Created
        private void InitAllSetting()
        {
            //Main.m_icServer.StopPreview();
            //Main.m_icServer.ViewUserResponseReceived += new iConfServer.NET.iConfServerDotNet.ViewUserResponseReceivedDelegate(this.icServer_ViewUserResponseReceived);

            //icClient.Child = Main.m_icServer;
            //Main.m_icServer.Show();

            //Main.m_icServer.StopTransmittingAudio();

            //icServer.Child = Main.m_icServer;
            //Main.m_icServer.Show();

            //m_iconfClient = new iConfClient.NET.iConfClientDotNet();
            //icClient.Child = m_iconfClient;
            //m_iconfClient.Show();

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
            //AnyChatCoreSDK.Login("WPF", "", 0);
            AnyChatCoreSDK.Login(m_roomInfo.Id, "", 0);
            //AnyChatCoreSDK.EnterRoom(3, "", 0);
            AnyChatCoreSDK.EnterRoom(Convert.ToInt32(m_roomInfo.Id), "", 0);


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
                        //AnyChatCoreSDK.UserSpeakControl(-1, true);
                        //AnyChatCoreSDK.UserCameraControl(-1, true);
                    }
                    break;
                case AnyChatCoreSDK.WM_GV_ONLINEUSER:
                    {
                        OpenRemoteUserVideo(true);
                        OpenRemoteUserSpeak(true);
                    }
                    break;
                case AnyChatCoreSDK.WM_GV_USERATROOM:
                    if (lParam != 0)    
                    {
                        OpenRemoteUserVideo(true);
                        OpenRemoteUserSpeak(true);
                    }
                    else                
                    {
                        if (wParam == g_otherUserId)
                        {
                            g_otherUserId = -1;
                            OpenRemoteUserVideo(true);
                            OpenRemoteUserSpeak(true);
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

            string strAppPath = ".\\";
            string strFilePath = strAppPath + fi.Name;

            fi.MoveTo(strFilePath);
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
                        localVideoImage.Source = RotateBitmap;
                    }
                    else if (userId == g_otherUserId)
                    {
                        remoteVideoImage.Source = RotateBitmap;
                    }
                }), null);
            });
            action.BeginInvoke(null, null);
        }

        public void OpenRemoteUserVideo(bool bFlag)
        {
            //if (g_otherUserId != -1)
            //    return;
            
            int usercount = 0;
            AnyChatCoreSDK.GetOnlineUser(null, ref usercount);

            //System.Threading.Thread.Sleep(2000);
            if (usercount > 0)
            {
                int[] useridarray = new int[usercount];
                AnyChatCoreSDK.GetOnlineUser(useridarray, ref usercount);
                for (int i = 0; i < usercount; i++)
                {
                    //int usercamerastatus = 0;
                    //if (AnyChatCoreSDK.QueryUserState(useridarray[i], AnyChatCoreSDK.BRAC_USERSTATE_CAMERA, ref usercamerastatus, sizeof(int)) != 0)
                    //    continue;
                    //if (usercamerastatus == 2)
                    {
                        //AnyChatCoreSDK.UserSpeakControl(useridarray[i], true);
                        AnyChatCoreSDK.UserCameraControl(useridarray[i], bFlag);
                        g_otherUserId = useridarray[i];
                        break;
                    }
                }
            }
        }

        public void OpenRemoteUserSpeak(bool bFlag)
        {
            //if (g_otherUserId != -1)
            //    return;

            int usercount = 0;
            AnyChatCoreSDK.GetOnlineUser(null, ref usercount);

            //System.Threading.Thread.Sleep(2000);
            if (usercount > 0)
            {
                int[] useridarray = new int[usercount];
                AnyChatCoreSDK.GetOnlineUser(useridarray, ref usercount);
                for (int i = 0; i < usercount; i++)
                {
                    //int usercamerastatus = 0;
                    //if (AnyChatCoreSDK.QueryUserState(useridarray[i], AnyChatCoreSDK.BRAC_USERSTATE_CAMERA, ref usercamerastatus, sizeof(int)) != 0)
                    //    continue;
                    //if (usercamerastatus == 2)
                    {
                        AnyChatCoreSDK.UserSpeakControl(useridarray[i], bFlag);
                        //AnyChatCoreSDK.UserCameraControl(useridarray[i], true);
                        g_otherUserId = useridarray[i];
                        break;
                    }
                }
            }
        }

        //private void icServer_ViewUserResponseReceived(object sender, System.String userName, System.Int32 port,
        //                                          System.Int32 videoWidth, System.Int32 videoHeight,
        //                                          System.String videoCodec)
        //{
        //    this.Dispatcher.Invoke(new MethodInvoker(delegate
        //    {
        //        m_iconfClient.Call(Login._ServerServiceUri, port, videoWidth, videoHeight, "n/a", "n/a", "n/a", 0, 0, 0, videoCodec);
        //    }
        //    ));
        //}



        // 음성장치가 있으면 초기화를 진행한다.

        private void EnableoVoiceChat()
        {
            return;

            //try
            //{
            //    StartRecording();
            //    StopRecording();
            //}
            //catch (Exception ex)
            //{
            //    ShowErrorOrMessage(ManagerMessage.ERROR_MICROPHONE);
            //}
        }


        // 30ms간격으로 비디오가 들어오는가를 검사한다.

        private void CheckVideoFrame()
        {
            dispatcherVideoTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherVideoTimer.Interval = new TimeSpan(0, 0, 0, 0, 80);
            dispatcherVideoTimer.Tick += new EventHandler(Each_Video_Tick);
            dispatcherVideoTimer.Start();
        }


        // 50ms간격으로 상태변화가 있는가를 검사한다.

        private void CheckOtherState()
        {
            dispatcherOtherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherOtherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherOtherTimer.Tick += new EventHandler(Each_State_Tick);
            dispatcherOtherTimer.Start();
        }


        // 비디오검사

        private void Each_Video_Tick(Object obj, EventArgs e)
        {
            try
            {
                // 첫 비디오프레임이 들어온다면...

                if (m_bFirstVideoFrame && m_videoInfo.Data != null)
                {
                    lblWebCam1.Content = LabelContents.MY_CAM_LBL + "(" + m_videoInfo.UserId + ")";
                    m_bFirstVideoFrame = false;
                }


                // 비디오가 들어온다면...

                if(m_videoInfo.Data != null)
                {
                    if (m_videoInfo.IsEnd == 1)
                    {
                        //BitmapImage bitmapImage = new BitmapImage();
                        //bitmapImage.BeginInit();
                        //bitmapImage.UriSource = new Uri("/Resources;component/image/NoCamera.png", UriKind.RelativeOrAbsolute);
                        //bitmapImage.EndInit();

                        //this.receiveImage.Source = bitmapImage;
                        lblWebCam1.Content = LabelContents.MY_CAM_LBL;

                        //m_bFirstVideoFrame = true;
                    }
                    else
                        ReceiveBit(m_videoInfo.Data);
                }
            }
            catch (Exception)
            { }
            
        }


        // 상태검사.

        private void Each_State_Tick(Object obj, EventArgs e)
        {
            try
            {
                // 방의 상태가 변한다면...

                if (m_bRoomStateChange == true)
                {
                    m_bRoomStateChange = false;
                    InitRoomState(m_roomDetailInfo);
                }

                int messageCount = m_MessageQue.Count;

                for (int i = 0; i < messageCount; i++)
                    ShowErrorOrMessage(m_MessageQue[i]);

                m_MessageQue.RemoveRange(0, messageCount);

                StringInfo messageInfo  = new StringInfo();
                messageInfo.UserId      = Login._UserInfo.Id;
                messageInfo.FontSize    = 12;
                messageInfo.FontName    = "Arial";
                messageInfo.FontStyle   = "";
                messageInfo.FontWeight  = "";
                messageInfo.FontColor   = "Black";

                int logCount = _LogStrings.Count;

                for (int i = 0; i < logCount; i++)
                {
                    messageInfo.String = _LogStrings[i];
                    AddMessageToHistoryTextBox(messageInfo);
                }

                _LogStrings.RemoveRange(0, logCount);

            }
            catch (Exception)
            { }
        }


        // 방입장시 정보현시.

        static int m_nCurCash = 0;

        //private void InitLabelCaption(RoomDetailInfo roomDetailInfo)
        private void InitLabelCaption(UserInfo userInfo)
        {
            try
            {
                //UserInfo userInfo = new UserInfo();
                //for (int i = 0; i < roomDetailInfo.Users.Count; i++)
                //{
                //    if (Login._UserInfo.Id.Equals(roomDetailInfo.Users[i].Id))
                //    {
                //        userInfo = roomDetailInfo.Users[i];
                //        break;
                //    }
                //}

                if (userInfo.Cash != 0)
                {
                    if (userInfo.Cash <= m_roomInfo.Cash && userInfo.Kind == 0)
                    {
                        ShowErrorOrMessage(ManagerMessage.NOT_ENOUGH_CASH);
                    }
                    getMoney.Content = LabelContents.TOTAL_GET_CASH + (userInfo.Cash - m_getMoney).ToString();
                    lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + userInfo.Cash.ToString();
                    lblPricePerSecond.Content = LabelContents.PRICE_PER_SECOND + m_roomInfo.Cash.ToString();

                    m_nCurCash = userInfo.Cash;
                }
                else if (userInfo.Point != 0)
                {
                    if (userInfo.Point <= m_roomInfo.Point && userInfo.Kind == 0)
                    {
                        ShowErrorOrMessage(ManagerMessage.NOT_ENOUGH_POINT);
                    }
                    getMoney.Content = LabelContents.TOTAL_GET_POINT + (userInfo.Cash - m_getMoney).ToString();
                    lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_POINT_LBL + userInfo.Point.ToString();
                    lblPricePerSecond.Content = LabelContents.PRICE_PER_SECOND + m_roomInfo.Point.ToString();
                }
            }
            catch (Exception)
            { }
        }

        

              //방값을 설정한다.
        private void roomValueBt_Click(object sender, RoutedEventArgs e)
        {
            string strPrice = System.Text.RegularExpressions.Regex.Replace(roomValue.Text, @"[0-9]", "");
            if (strPrice.Length > 0)
            {
                IniFileEdit iniFileEdit = new IniFileEdit(Login._UserPath);
                //MessageBoxCommon.Show(iniFileEdit.GetIniValue("StringMessageBox", "Invalid_Number"), MessageBoxType.Ok);
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, iniFileEdit.GetIniValue("StringMessageBox", "Invalid_Number"), "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                return;
            }
            else
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
                try
                {
                    m_roomInfo.Cash = Convert.ToInt32(roomValue.Text);
                }
                catch (System.Exception)
                {
                                    //MessageBoxCommon.Show("방값을 정확히 입력하세요.", MessageBoxType.Ok);
                    QQMessageBox.Show(tempWindowForm, "방값을 정확히 입력하세요.", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);
                    return;
                }

                            //if (MessageBoxCommon.Show("방값을 " + roomValue.Text + "원으로 설정하시겠습니까?", MessageBoxType.YesNo) == MessageBoxReply.Yes)
                if (QQMessageBox.Show(tempWindowForm, "确定   赠送 " + roomValue.Text + "金币 礼物吗？", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    Login._ClientEngine.Send(NotifyType.Request_RoomInfo, m_roomInfo);
                    var selbt = sender as System.Windows.Controls.Button;
                    selbt.IsEnabled = false;
                    roomValue.IsEnabled = false;
                }
            }
        }
                // 서버로부터 응답이 들어올때...

        public void OnReceive(NotifyType type, Socket socket, BaseInfo baseInfo)
        {
            try
            {
                switch (type)
                {
                    case NotifyType.Reply_UserInfo:
                        {
                            UserInfo userInfo = (UserInfo)baseInfo;
                            InitLabelCaption(userInfo);
                        }
                        break;
                    case NotifyType.Reply_RoomDetail:
                        {
                            RoomDetailInfo roomDetailInfo = (RoomDetailInfo)baseInfo;
                            m_roomDetailInfo = roomDetailInfo;
                            m_bRoomStateChange = true;
                        }
                        break;

                    case NotifyType.Reply_StringChat:
                        {
                            StringInfo strInfo = (StringInfo)baseInfo;
                            if (strInfo.UserId != Login._UserInfo.Id)
                            {
                                System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(this);
                                FlashWindow(helper.Handle, true);
                            }

                            AddMessageToHistoryTextBox(strInfo);
                        }
                        break;

                    

                    case NotifyType.Reply_OutMeeting:
                        {
                            UserInfo userInfo = (UserInfo)baseInfo;
                            UserOut(userInfo);
                        }
                        break;

                    case NotifyType.Reply_OutRoom:
                        {
                            UserInfo userInfo = (UserInfo)baseInfo;
                            UserOut(userInfo);
                        }
                        break;

                    case NotifyType.Reply_Give:
                        {
                            PresentHistoryInfo presentHistoryInfo = (PresentHistoryInfo)baseInfo;

                            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                            {
                                m_nCurCash = m_nCurCash + presentHistoryInfo.Cash;
                                lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + m_nCurCash.ToString();
                            }

                            if (Login._UserInfo.Id == presentHistoryInfo.SendId)
                            {
                                m_nCurCash = m_nCurCash - presentHistoryInfo.Cash;
                                lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + m_nCurCash.ToString();
                            }
                        }
                        break;

                    case NotifyType.Reply_RoomInfo:
                        {
                            RoomInfo roomInfo = (RoomInfo)baseInfo;
                            if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
                            {
                                TempWindowForm tempWindowForm = new TempWindowForm();

                                //if (MessageBoxCommon.Show("유로채팅을 시작하겠습니까? 방값:" + roomInfo.Cash, MessageBoxType.YesNo) == MessageBoxReply.Yes)
                                if (QQMessageBox.Show(tempWindowForm, "接受视频邀请吗？ 价格：" + roomInfo.Cash, "提示", QQMessageBoxIcon.Question, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                                {
                                    Login._ClientEngine.Send(NotifyType.Request_RoomPrice, roomInfo);
                                }
                                else
                                {
                                    this.Close();
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            { }
        }


        // 사용자가 채팅을 종료하였을때...

        private void UserOut(UserInfo userInfo)
        {
            try
            {
                if (userInfo.Id.Equals(Login._UserInfo.Id) && userInfo.RoomId.Equals(this.m_roomInfo.Id))
                {
                    if (m_bEndChatFlag == false)
                    {
                        remoteVideoImage.Source = new TransformedBitmap();
                        ShowErrorOrMessage(ManagerMessage.ALERT_END_CHATTING);
                        m_bEndChatFlag = true;
                    }

                    Login._UserInfo = userInfo;

                    remoteVideoImage.Visibility = Visibility.Hidden;
                    screenImg.Visibility = Visibility.Hidden;
                    //Main.chatRoom = null;

                    //var timer = new System.Windows.Threading.DispatcherTimer();
                    //timer.Interval = TimeSpan.FromSeconds(5);
                    //timer.Tick += delegate { this.Close(); };
                    //timer.Start();
                }
                else
                {
                    //string strMessage = userInfo.Id + ManagerMessage.ALERT_USER_OUT;
                    //ShowErrorOrMessage(strMessage);
                }
            }
            catch (Exception)
            { }
        }


        // 서버로부터 UDP응답이 들어올때...
        private List<string> _LogStrings = new List<String>();
        private DateTime prevTime;

        public void OnVideoReceive(NotifyType type, IPEndPoint ipEndPoint, BaseInfo baseInfo)
        {   
            try
            {
                //switch (type)
                //{
                //    case NotifyType.Reply_VideoChat:
                //        {
                //            VideoInfo videoInfo = (VideoInfo)baseInfo;
                //            m_videoInfo = videoInfo;
                //        }
                //        break;

                //    case NotifyType.Reply_VoiceChat:
                //        {
                //            m_voiceInfo = (VoiceInfo)baseInfo;
                //            ReceiveVoiceInfo(m_voiceInfo);

                //            DateTime curTime = DateTime.Now;
                //            TimeSpan delay = curTime - prevTime;
                //            string logString = string.Format("Receive : {0} : ip-{1}, port-{2}, size-{3}", delay.TotalMilliseconds, ipEndPoint.Address, ipEndPoint.Port, m_voiceInfo.GetSize());
                //            //_LogStrings.Add(logString);   

                //            prevTime = curTime;
                //        }
                //        break;
                //}
            }
            catch (Exception ex)
            {
                string strMessage = ex.Message;
            }
        }


        // 채팅방의 초기상태를 설정한다.

        private void InitRoomState(RoomDetailInfo roomDetailInfo)
        {
            try
            {
                // 방에 입장한 사용자들의 정보를 유저리스트에 보여준다.

                if (roomDetailInfo.Users.Count < 1 || roomDetailInfo.Users == null)
                    return;

                //this.userList.Items.Clear();
                //for (int i = 0; i < roomDetailInfo.Users.Count; i++)
                //{
                //    if (roomDetailInfo.Users[i].Kind == 1)
                //    {
                //        m_userFemaleInfo = roomDetailInfo.Users[i];
                //    }

                //    StackPanel stackPanel = new StackPanel();
                //    stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

                //    Image img = new Image();
                //    string strImageSource = "http://" + Login._ServerRealUri + "/" + roomDetailInfo.Users[i].Icon;

                //    BitmapImage bitMapImage = new BitmapImage();
                //    bitMapImage.BeginInit();
                //    bitMapImage.UriSource = new Uri(strImageSource, UriKind.RelativeOrAbsolute);
                //    bitMapImage.EndInit();

                //    img.Source = bitMapImage;
                //    img.Width = 25;

                //    TextBlock textBlock = new TextBlock();
                //    textBlock.Text = "  " + roomDetailInfo.Users[i].Id;

                //    stackPanel.Children.Add(img);
                //    stackPanel.Children.Add(textBlock);

                //    this.userList.Items.Add(stackPanel);
                //    //this.userList.Items.Add(roomDetailInfo.Users[i].Id);
                //}


                // Emoticon들을 얻어 Emoticon컨트롤에 배치하여준다.

                m_listEmoticons = new List<IconInfo>();
                m_listEmoticons = roomDetailInfo.Emoticons;

                m_listPresents = new List<IconInfo>();
                m_listPresents = roomDetailInfo.Presents;

                if (!m_bShowPresent)
                {
                    ShowPresent();
                    m_bShowPresent = true;
                }

                //InitLabelCaption(roomDetailInfo);
            }
            catch (Exception)
            { }
        }


        // 웹캠을 설정한다.

        private bool InitWPFMediaKit()
        {
            try
            {
                //m_localWebCamCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                //m_localWebCam = new VideoCaptureDevice(m_localWebCamCollection[0].MonikerString);
                //m_localWebCam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
                //m_localWebCam.DesiredFrameSize = new System.Drawing.Size(480,320);
                //m_localWebCam.Start();

                return true;
            }
            catch (Exception)
            {
                string strMessage = ManagerMessage.ERROR_WEB_CAM;
                ShowErrorOrMessage(strMessage);
                return false;
            }
        }


        // 캠으로부터 화상을 얻어 현시한다.

        //private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        //{
        //    try
        //    {
        //        System.Drawing.Image img = (System.Drawing.Bitmap)eventArgs.Frame.Clone();

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
        //            //MyVideoCapElement.Source = bi;
        //        }));
        //    }
        //    catch (Exception)
        //    {}
        //}
        

        // 폼이 닫길때...

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_bEndChatFlag == false)
            {
                //if (MessageBoxCommon.Show("Do you want to exit a chaRoom?", MessageBoxType.YesNo) == MessageBoxReply.No)
                //{
                //    e.Cancel = true;
                //    return;
                //}

                TempWindowForm tempWindowForm = new TempWindowForm();
                if (QQMessageBox.Show(tempWindowForm, "确定结束视频通话吗？", "提示", QQMessageBoxIcon.Question, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Main.chatRoom = null;

            //Main.listChatRoom.Remove(this);
            FinalProcess();
        }


        // 폼클로즈시 마감처리를 진행한다.

        private void FinalProcess()
        {
            try
            {
                AnyChatCoreSDK.LeaveRoom(-1);
                AnyChatCoreSDK.Logout();
                AnyChatCoreSDK.Release();

                if (disapthcerTimer != null)
                    disapthcerTimer.Stop();

                if (dispatcherVideoTimer != null)
                    dispatcherVideoTimer.Stop();

                if (dispatcherOtherTimer != null)
                    dispatcherOtherTimer.Stop();

                //m_iconfClient.Disconnect();

                Thread.Sleep(1000);
                //Main.m_icServer.Listen(false, "", 0, 0, 0);
                //Main.m_icServer.StopPreview();
                //Main.m_icServer.LeaveConference();
                

                //if (m_localWebCam != null)
                //{
                //    if(m_localWebCam.IsRunning)
                //        m_localWebCam.Stop();

                //    m_localWebCam = null;
                //}


                if (mutiChatClock != null)
                    mutiChatClock.Stop();


                // VoiceChat Stop
                //StopPlaying();
                //StopRecording();

                if (Convert.ToInt32(m_roomInfo.Id) < 1000)
                    Login._ClientEngine.Send(NotifyType.Request_OutMeeting, m_roomInfo);
                else
                    Login._ClientEngine.Send(NotifyType.Request_OutRoom, m_roomInfo);

                if (m_diceGame != null)
                {
                    m_diceGame.Close();
                }

                if (otherWebCamView != null)
                    otherWebCamView.Close();

                Main._nTotalBytesPerSecond = -1;

                Login._ClientEngine.DetachHandler(OnReceive);
                //Login._VideoEngine.DetachHandler(OnVideoReceive);
            }
            catch (Exception ex)
            {
                ShowErrorOrMessage(ManagerMessage.ERROR_MICROPHONE);
            }
        }


        // 캐쉬충전단추를 클릭하였을때...

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start("control", "mmsys.cpl,,1");
            //System.Diagnostics.Process.Start("http://" + Login._ServerRealUri + "/Charge");
            System.Diagnostics.Process.Start("http://" + Login._ServerRealUri + "/halbin2013-05-29/bbs/chargeHistory.php");
        }


        // 폰트설정단추를 클릭하였을때...

        private void Image_MouseUp_2(object sender, EventArgs e)
        {
            try
            {
                SettingFont();
            }
            catch (Exception)
            {
                string strErrorMessage = ManagerMessage.ERROR_FONT_SETTING;
                ShowErrorOrMessage(strErrorMessage);
            }
            finally
            {
                this.messageEditBox.Focus();
            }
        }


        // 메세지편집창에서 Enter건을 클릭하였을때...

        private void messageEditBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                string strSendMessage = "";
                strSendMessage = InterpretMessage();

                if (strSendMessage.Length > 3000)
                {
                    ShowErrorOrMessage(ManagerMessage.ERROR_MANY_CHAT);
                    e.Handled = true;
                    return;
                }

                if (e.Key == Key.Enter)
                {
                    if (strSendMessage != null && strSendMessage != string.Empty)
                        SendMessage(strSendMessage);

                    TextRange textRange = new TextRange(messageEditBox.Document.ContentStart, messageEditBox.Document.ContentEnd);
                    textRange.Text = "";
                    messageEditBox.ScrollToHome();
                    messageEditBox.CaretPosition = messageEditBox.Document.ContentStart;

                    e.Handled = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && e.Key == Key.Insert)
                {
                    e.Handled = true;
                    return;
                }
            }
            catch (Exception)
            {
            }
        }


        // 오류를 현시하여주는 함수

        private void ShowErrorOrMessage(string strErrorMessage)
        {
            Paragraph para = new Paragraph();
            Run errorMessage = new Run(strErrorMessage);

            errorMessage.FontSize = 12;
            errorMessage.Foreground = Brushes.Red;
            para.LineHeight = 1;
            para.Inlines.Add(new Bold(errorMessage));
            m_flowDoc.Blocks.Add(para);

            this.messageHistoryBox.Document = m_flowDoc;
            this.messageHistoryBox.ScrollToEnd();
        }


        // 비디오전송단추를 클릭하였을때...

        private void buttonSendMyVideo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetVideoChat();
            //InitAllSetting();
        }


        // 비디오전송단추를 클릭한후의 처리...

        private void SetVideoChat()
        {
            if (!m_videoSenBtnState)
            {
                //if (InitWPFMediaKit())
                //{
                //    m_nVideoEndStae = 0;
                //    lblWebCam2.Content = Login._UserInfo.Id + LabelContents.WEB_CAM_LBL;
                //    this.webCamImage.Visibility = Visibility.Hidden;
                //    StartVideoCapture();
                //    this.messageEditBox.Focus();

                //    this.buttonSendMyVideo.Content = ButtonContent.VIDEO_END_BUTTON;
                //    m_videoSenBtnState = true;
                //}

                //icServer.Visibility = Visibility.Visible;
                lblWebCam1.Content = LabelContents.MY_CAM_LBL + "(" + Login._UserInfo.Id + ")";
                //this.webCamImage.Visibility = Visibility.Hidden;
                this.localVideoImage.Visibility = Visibility.Visible;
                //StartVideoCapture();

                SendMessage(m_strVideoChatPass);

                this.buttonSendMyVideo.Content = ButtonContent.VIDEO_END_BUTTON;
                m_videoSenBtnState = true;
                //Main.m_icServer.StartPreview(0);

                AnyChatCoreSDK.UserCameraControl(-1, true);
                
                // 2014-01-17: GreenRose
                int nRet = AnyChatCoreSDK.StreamRecordCtrl(g_selfUserId, true, 0, 0);
                nRet = AnyChatCoreSDK.SnapShot(g_selfUserId, 0, 0);
            }
            else
            {
                //m_nVideoEndStae = 1;
                //SendVideo();
                //lblWebCam2.Content = LabelContents.WEB_CAM_LBL;
                //this.buttonSendMyVideo.Content = ButtonContent.VIDEO_SEND_BUTTON;
                //webCamImage.Visibility = Visibility.Visible;

                //if (disapthcerTimer != null)
                //    disapthcerTimer.Stop();

                //if (m_localWebCam != null)
                //{
                //    if(m_localWebCam.IsRunning)
                //        m_localWebCam.Stop();

                //    m_localWebCam = null;
                //}

                //m_videoSenBtnState = false;

                //icServer.Visibility = Visibility.Hidden;
                lblWebCam1.Content = LabelContents.MY_CAM_LBL;
                this.buttonSendMyVideo.Content = ButtonContent.VIDEO_SEND_BUTTON;
                //webCamImage.Visibility = Visibility.Visible;
                localVideoImage.Visibility = Visibility.Hidden;

                if (disapthcerTimer != null)
                    disapthcerTimer.Stop();

                m_videoSenBtnState = false;

                SendMessage(m_strVideoChatEnd);

                AnyChatCoreSDK.UserCameraControl(-1, false);

                // 2014-01-17: GreenRose
                int nRet = AnyChatCoreSDK.StreamRecordCtrl(g_selfUserId, false, 0, 0);
            }
        }


        // Send단추를 클릭하였을때...

        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string strSendMessage = InterpretMessage();
            if (strSendMessage != null && strSendMessage != string.Empty)
                SendMessage(strSendMessage);

            TextRange textRange = new TextRange(messageEditBox.Document.ContentStart, messageEditBox.Document.ContentEnd);
            textRange.Text = "";
            messageEditBox.ScrollToHome();
            messageEditBox.CaretPosition = messageEditBox.Document.ContentStart;

            this.messageEditBox.Focus();
        }


        // 음성보내기 단추를 클릭하였을때...

        private void button6_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (!m_VoiceSendBtnState)
                {
                    //Main.m_icServer.StartTransmittingAudio();
                    if (!Main._bJoinedConference && !m_bMakingRoom)
                    {
                        ArrayList userList = new ArrayList();

                        for (int i = 0; i < m_roomDetailInfo.Users.Count; i++)
                        {
                            string strUserName = Login._UserInfo.Id;
                            string strCompName = m_roomDetailInfo.Users[i].Id;

                            if (!strUserName.Equals(strCompName))
                            {
                                userList.Add(strCompName);
                            }
                        }
                        //Main.m_icServer.StartConference(userList);
                        
                        m_bMakingRoom = true;
                    }
                    AnyChatCoreSDK.UserSpeakControl(-1, true);
                    this.button6.Content = ButtonContent.VOICE_END_BUTTON;

                    m_VoiceSendBtnState = true;

                    SendMessage(m_strVoiceChatPass);
                }
                else
                {
                    AnyChatCoreSDK.UserSpeakControl(-1, false);

                    this.button6.Content = ButtonContent.VOICE_SEND_BUTTON;
                    SendMessage(m_strVoiceChatEnd);

                    m_VoiceSendBtnState = false;
                    //Main.m_icServer.StopTransmittingAudio();
                }
            }
            catch (Exception)
            {
                ShowErrorOrMessage(ManagerMessage.ERROR_MICROPHONE);
            }
        }


        // Emoticon단추를 클릭하였을때...

        private void Image_MouseUp_1(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Point pos = PointToScreen(Mouse.GetPosition(this));

                var selector = new ChatClient.Emoticon.EmoticonSelector();
                selector.listEmoticonInfo = m_listEmoticons;

                selector.EmoticonSelected += (s1, e1) => OnEmoticonSelected(((ChatClient.Emoticon.EmoticonSelector)s1).UUri);
                selector.EmoticonClosing += OnEmoticonClosing;
                selector.Top = pos.Y;
                selector.Left = pos.X;
                selector.Show();
            }
            catch (Exception)
            { }
        }


        // Emoticon폼이 클로즈되였을때 메세지편집창에 초점을 맞춘다.

        private void OnEmoticonClosing(Object obj, EventArgs e)
        {
            this.messageEditBox.Focus();
        }


        // Emoticon을 선택한후의 다음처리.

        private void OnEmoticonSelected(string strUri)
        {
            InsertImageToRichTextBox(strUri);
            messageEditBox.Focus();
        }


        // RichTextBox에 이미지를 보여준다.

        private void InsertImageToRichTextBox(string strUri)
        {
            try
            {
                if (messageEditBox.CaretPosition.Paragraph == null)
                    messageEditBox.CaretPosition.InsertParagraphBreak();

                TextPointer tp = messageEditBox.CaretPosition;
                Image img = new Image();

                BitmapImage bitMapImage = new BitmapImage();
                bitMapImage.BeginInit();
                bitMapImage.UriSource = new Uri("/Resources;component/image/emoticon/" + strUri, UriKind.RelativeOrAbsolute);
                bitMapImage.EndInit();

                //img.Source = bitMapImage;
                img.Width = 25;
                
                WpfAnimatedGif.ImageBehavior.SetAnimatedSource(img, bitMapImage);
                WpfAnimatedGif.ImageBehavior.SetRepeatBehavior(img, System.Windows.Media.Animation.RepeatBehavior.Forever);

                InlineUIContainer ui = new InlineUIContainer(img);
                tp.Paragraph.Inlines.Add(ui);

                messageEditBox.CaretPosition = messageEditBox.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);

                messageEditBox.Focus();
            }
            catch (Exception)
            {
            }
        }


        // 점수보내기 단추를 클릭하였을때...

        private void btnSendMark_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
               // int nNum = Convert.ToInt32(numberMark.Text);
                EvalHistoryInfo evalHistoryInfo = new EvalHistoryInfo();
                evalHistoryInfo.OwnId = m_userFemaleInfo.Id; 
                //evalHistoryInfo.Value = nNum;
                evalHistoryInfo.BuyerId = Login._UserInfo.Id;

               // numberMark.Text = "";

                Login._ClientEngine.Send(NotifyType.Request_Evaluation, evalHistoryInfo);
            }
            catch (Exception)
            { }
        }


        // 점수입력칸에 점수를 입력할때 수자만 입력가능하게 한다...

        private void numberMark_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(((System.Windows.Controls.TextBox)sender).Text))
                {
                    //numberMark.Opacity = 0.8;
                    m_strPreviousText = "";
                   // this.btnSendMark.IsEnabled = false;
                }
                else
                {
                    int num = 0;
                    bool success = int.TryParse(((System.Windows.Controls.TextBox)sender).Text, out num);
                    if (success & num > 0 && num <= 5)
                    {
                        ((System.Windows.Controls.TextBox)sender).Text.Trim();
                        m_strPreviousText = ((System.Windows.Controls.TextBox)sender).Text;
                    }
                    else
                    {
                        ((System.Windows.Controls.TextBox)sender).Text = m_strPreviousText;
                        ((System.Windows.Controls.TextBox)sender).SelectionStart = ((System.Windows.Controls.TextBox)sender).Text.Length;
                    }

                    //numberMark.Opacity = 1;
                   // this.btnSendMark.IsEnabled = true;
                }
            }
            catch (Exception)
            { }
        }


        #region FontSetting
        private void SettingFont()
        {
            using (var fontDialog = new System.Windows.Forms.FontDialog())
            {
                fontDialog.Font = new System.Drawing.Font(m_strFontName, m_fFontSize, m_fontStyle);
                fontDialog.ShowColor = true;

                fontDialog.Color = m_fontColor;

                if (fontDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                {
                    FontFamilyConverter ffc = new FontFamilyConverter();

                    this.messageEditBox.FontSize = fontDialog.Font.Size / 72 * 96;
                    this.messageEditBox.FontFamily = (System.Windows.Media.FontFamily)ffc.ConvertFromString(fontDialog.Font.Name);

                    if (fontDialog.Font.Bold)
                        this.messageEditBox.FontWeight = FontWeights.Bold;
                    else
                        this.messageEditBox.FontWeight = FontWeights.Normal;

                    if (fontDialog.Font.Italic)
                        this.messageEditBox.FontStyle = FontStyles.Italic;
                    else
                        this.messageEditBox.FontStyle = FontStyles.Normal;

                    BrushConverter bc = new BrushConverter();
                    this.messageEditBox.Foreground = (System.Windows.Media.Brush)bc.ConvertFromString(fontDialog.Color.Name);

                    m_strFontName = fontDialog.Font.Name;
                    m_fFontSize = fontDialog.Font.Size;
                    m_fontStyle = fontDialog.Font.Style;
                    m_fontColor = fontDialog.Color;
                }
            }
        }
        #endregion


        #region MessageFunction


        // 보내려는 메세지를 변환( 실레로 이미지가 있으면 이미지를 그에 해당한 문자로 변환)하여 서버로 전송

        private string InterpretMessage()
        {
            FlowDocument flowDocument = new FlowDocument();

            foreach (Block block in messageEditBox.Document.Blocks)
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
                                var imgSource = (BitmapImage)WpfAnimatedGif.ImageBehavior.GetAnimatedSource(img);
                                Run run = new Run("<" + imgSource.UriSource.ToString() + ">");

                                //Run run = new Run("<" + img.Source.ToString() + ">");
                                newPara.Inlines.Add(run);
                            }
                        }
                        else if (inLine is Run)
                        {
                            Run run = (Run)inLine;
                            newPara.Inlines.Add(run.Text);
                        }
                    }

                    flowDocument.Blocks.Add(newPara);
                }
            }

            TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);

            string strMessage = textRange.Text;

            return strMessage;
        }


        private void SendMessage(string strMessage)
        {
            try
            {
                int     nFontSize       = 0;
                string strFontName      = "";
                string strFontStyle     = "";
                string strFontWeight    = "";
                string strFontColor     = "";

                nFontSize = Convert.ToInt32(this.messageEditBox.FontSize);
                strFontName = messageEditBox.FontFamily.Source;

                if (this.messageEditBox.FontStyle == FontStyles.Italic)
                    strFontStyle = FontInfo.FONT_STYLE_ITALIC;
                else
                    strFontStyle = FontInfo.FONT_STYLE_NORMAL;

                if (this.messageEditBox.FontWeight == FontWeights.Bold)
                    strFontWeight = FontInfo.FONT_WEIGHT_BOLD;
                else
                    strFontWeight = FontInfo.FONT_WEIGHT_NORMAL;

                strFontColor = messageEditBox.Foreground.ToString();

                StringInfo messageInfo  = new StringInfo();
                messageInfo.UserId      = Login._UserInfo.Id;
                messageInfo.String      = strMessage;
                messageInfo.FontSize    = nFontSize;
                messageInfo.FontName    = strFontName;
                messageInfo.FontStyle   = strFontStyle;
                messageInfo.FontWeight  = strFontWeight;
                messageInfo.FontColor   = strFontColor;

                // 2013-12-25: GreenRose
                messageInfo.strRoomID = this.m_roomInfo.Id;

                Login._ClientEngine.Send(NotifyType.Request_StringChat, messageInfo);
            }
            catch (Exception)
            { }
        }


        public void AddMessageToHistoryTextBox(StringInfo messageInfo)
        {
            if(messageInfo.strRoomID.Equals(this.m_roomInfo.Id))
                InterpretServerMessgae(messageInfo.UserId, messageInfo.String, messageInfo.FontSize, messageInfo.FontName, messageInfo.FontStyle, messageInfo.FontWeight, messageInfo.FontColor);
        }


        private void InterpretServerMessgae(string strUserID, string strMessage, int nFontSize, string strFontName, string strFontStyle, string strFontWeight, string strFontColor)
        {
            double fontSize = Convert.ToDouble(nFontSize);
            FontFamily fontFamily = new FontFamily(strFontName);

            FontStyle fontStyle = new FontStyle();
            if (strFontStyle == FontInfo.FONT_STYLE_ITALIC)
                fontStyle = FontStyles.Italic;
            else
                fontStyle = FontStyles.Normal;

            FontWeight fontWeight = new FontWeight();
            if (strFontWeight == FontInfo.FONT_WEIGHT_BOLD)
                fontWeight = FontWeights.Bold;
            else
                fontWeight = FontWeights.Normal;

            BrushConverter bc = new BrushConverter();
            Brush fontColor = (Brush)bc.ConvertFrom(strFontColor);

            AddMessage(strUserID, strMessage, fontSize, fontFamily, fontStyle, fontWeight, fontColor);
        }


        private void AddMessage(string strID, string strReceiveMessage, double fontSize, FontFamily fontName, FontStyle fontStyle, FontWeight fontWeight, Brush fontColor)
        {
            if (!InterpretSignalText(strReceiveMessage, strID))
            {
                return;
            }

            // 아이디를 현시하여준다.

            Paragraph para = new Paragraph();
            Run userID = new Run(strID + "(" + DateTime.Now.ToShortTimeString() + ") : " + "\r\n");

            if (strID.Equals(Login._UserInfo.Id))
                userID.Foreground = System.Windows.Media.Brushes.Red;
            else
                userID.Foreground = System.Windows.Media.Brushes.Blue;

            userID.FontSize = 12;
            para.LineHeight = 5;
            para.Inlines.Add(new Bold(userID));
            m_flowDoc.Blocks.Add(para);


            // 메세지를 현시하여준다.

            string rtf = "";
            rtf = strReceiveMessage;

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
                    bitMapImage.UriSource = new Uri(strUri.Substring(0), UriKind.RelativeOrAbsolute);
                    bitMapImage.EndInit();

                    //img.Source = bitMapImage;
                    img.Width = 25;
                    WpfAnimatedGif.ImageBehavior.SetAnimatedSource(img, bitMapImage);
                    WpfAnimatedGif.ImageBehavior.SetRepeatBehavior(img, System.Windows.Media.Animation.RepeatBehavior.Forever);

                    InlineUIContainer ui = new InlineUIContainer(img);

                    para.Inlines.Add(ui);

                    rtf = rtf.Substring(rtf.IndexOf('>') + 1);
                    i = -1;
                }
                else
                {
                    if (rtf[i] != '\r' && rtf[i] != '\n')
                        strMessage += rtf[i];
                }
            }

            

            if (strMessage != string.Empty)
            {
                Run run = new Run(strMessage);
                run.FontSize = fontSize;
                run.FontFamily = fontName;
                run.FontWeight = fontWeight;
                run.FontStyle = fontStyle;
                run.Foreground = fontColor;
                para.Inlines.Add(run);
            }

            this.messageHistoryBox.Document = m_flowDoc;
            this.messageHistoryBox.ScrollToEnd();
        }

        #endregion


        #region VideoChat

        private void StartVideoCapture()
        {
            //Main.m_icServer.StartPreview(0);
            disapthcerTimer = new System.Windows.Threading.DispatcherTimer();
            disapthcerTimer.Interval = new TimeSpan(0, 0, 0, 0, 60);
            disapthcerTimer.Tick += new EventHandler(Each_Tick);
            disapthcerTimer.Start();
        }


        private void Each_Tick(Object obj, EventArgs ev)
        {
            try
            {
                SendVideo();
            }
            catch (Exception)
            {
            }
        }


        private void SendVideo()
        {
            //VisualBrush vb = new VisualBrush(MyVideoCapElement);
            //DrawingVisual drawingVisual = new DrawingVisual();
            //DrawingContext drawingContext = drawingVisual.RenderOpen();

            //using (drawingContext)
            //{
            //    drawingContext.DrawRectangle(vb, null, new Rect(0, 0, 318, 238));
            //}

            //double dpiX = 200;
            //double dpiY = 200;

            //RenderTargetBitmap bmp = new RenderTargetBitmap((int)(318 * dpiX / 96), (int)(238 * dpiY / 96), dpiX, dpiY, PixelFormats.Pbgra32);
            //RenderTargetBitmap bmp = new RenderTargetBitmap((int)(318), (int)(238), 96, 96, PixelFormats.Pbgra32);
            //bmp.Render(drawingVisual);

            //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(bmp));
            //encoder.QualityLevel = 30;

            //byte[] bit = new byte[0];

            byte[] bit = Encoding.ASCII.GetBytes("P2PVideoChat");

            using (MemoryStream stream = new MemoryStream())
            {
                //encoder.Save(stream);
                //bit = stream.ToArray();
                stream.Close();

                VideoInfo videoInfo = new VideoInfo();
                videoInfo.UserId = Login._UserInfo.Id;
                videoInfo.Data = bit;
                videoInfo.IsEnd = m_nVideoEndStae;

                Login._VideoEngine.Send(NotifyType.Request_VideoChat, videoInfo);
            }
        }

        private void ReceiveBit(byte[] receiveData)
        {
            string errorMessage = null;

            try
            {
                if (m_bFirstVideoFrame1)
                {
                    //for (int i = 0; i < m_roomDetailInfo.Users.Count; i++)
                    //{
                    //    string strCurName = Login._UserInfo.Id;
                    //    string strCompName = m_roomDetailInfo.Users[i].Id;

                    //    if (!strCurName.Equals(strCompName))
                    //    {
                    //        Main.m_icServer.ViewUser(strCompName);
                    //        break;
                    //    }
                    //}

                    //Main.m_icServer.ViewUser(m_videoInfo.UserId);
                    m_bFirstVideoFrame1 = false;
                }

                //MemoryStream ms = new MemoryStream(receiveData);

                //JpegBitmapDecoder decoder = new JpegBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                //BitmapSource bitmapSource = decoder.Frames[0];

                //receiveImage.Source = bitmapSource;

                //BitmapImage bitmapImage = new BitmapImage();
                //bitmapImage.BeginInit();
                //bitmapImage.StreamSource = ms;
                //bitmapImage.EndInit();

                //receiveImage.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }


        #endregion


        //#region VoiceChat

        //private void StartRecording()
        //{
        //    waveFormat = new NAudio.Wave.WaveFormat(encoder.FrameSize * 50, 16, 1);
        //    waveIn = new WaveIn { WaveFormat = waveFormat, BufferMilliseconds = 40, NumberOfBuffers = 2 };
        //    //waveIn = new WasapiCapture();

        //    waveIn.DataAvailable += waveIn_DataAvailable;
        //    waveIn.RecordingStopped += waveIn_RecordingStopped;

        //    waveIn.StartRecording();
        //    isRecording = true;

        //    this.messageEditBox.Focus();
        //    this.button6.Content = ButtonContent.VOICE_END_BUTTON;

        //    m_SendVoiceInfo.PlayState = (int)PlayState.Start;
        //    Login._VideoEngine.Send(NotifyType.Request_VoiceChat, m_SendVoiceInfo);
        //}

        //private void StopRecording()
        //{
        //    isRecording = false;
        //    this.button6.Content = ButtonContent.VOICE_SEND_BUTTON;

        //    //if (waveIn == null)
        //    //    return;

        //    //waveIn.StopRecording();
        //    //waveIn = null;

        //    //if (isRecording)
        //    //{
        //    //    waveOut.Stop();
        //    //    waveOut.Dispose();
        //    //    waveOut = null;
        //    //}
        //}

        //private void StartPlaying()
        //{
        //    //if (waveOut.PlaybackState == PlaybackState.Playing)
        //    //    return;
        //    if (isPlaying == true)
        //        return;

        //    waveOut.Play();
        //    isPlaying = true;
        //}

        //private void StopPlaying()
        //{
        //    if (waveOut == null)
        //        return;

        //    //if (isPlaying)
        //    {
        //        waveOut.Stop();
        //        waveOut.Dispose();
        //        //waveOut = null;
        //        isPlaying = false;
        //    }
        //}

        //private void ReceiveVoiceInfo(VoiceInfo voiceInfo)
        //{
        //    if (voiceInfo.PlayState == (int)PlayState.Start)
        //    {
        //        string strMessage = voiceInfo.UserId + ManagerMessage.ALERT_VOICE_MESSAGE;
        //        m_MessageQue.Add( strMessage );                   
        //    }
        //    else if (voiceInfo.PlayState == (int)PlayState.Stop)
        //    {
        //        string strMessage = m_voiceInfo.UserId + ManagerMessage.ALERT_END_VOICE;
        //        m_MessageQue.Add(strMessage);
        //    }
        //    else
        //    {
        //        StartPlaying();

        //        for (int i = 0; i < voiceInfo.Frames.Count; i++)
        //        {
        //            byte[] frame = voiceInfo.Frames[i];
        //            waveProvider.Write(frame, 0, frame.Length);
        //        }
        //    }
        //}

        //private DateTime sendPrevTime;

        //private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        //{
        //    if (isRecording == false)
        //        return;

        //    short[] data = new short[e.BytesRecorded / 2];
        //    Buffer.BlockCopy(e.Buffer, 0, data, 0, e.BytesRecorded);

        //    var encodedData = new byte[e.BytesRecorded];
        //    var encodedBytes = encoder.Encode(data, 0, data.Length, encodedData, 0, encodedData.Length);

        //    if (encodedBytes != 0)
        //    {
        //        var upstreamFrame = new byte[encodedBytes];
        //        Array.Copy(encodedData, upstreamFrame, encodedBytes);

        //        m_SendVoiceInfo.PlayState = (int)PlayState.Playing;
        //        m_SendVoiceInfo.Frames.Add(upstreamFrame);

        //        if (m_SendVoiceInfo.Frames.Count > 30)
        //        {
        //            Login._VideoEngine.Send(NotifyType.Request_VoiceChat, m_SendVoiceInfo);
        //            m_SendVoiceInfo.Frames.Clear();

        //            DateTime curTime = DateTime.Now;
        //            TimeSpan delay = curTime - sendPrevTime;
        //            string logString = string.Format("Send : {0} ", delay.TotalMilliseconds);
        //            //_LogStrings.Add(logString);

        //            sendPrevTime = curTime;
        //        }
        //    }
        //}

        //private void waveIn_RecordingStopped(object sender, EventArgs e)
        //{
        //    //isRecording = false;
        //    //RaiseCommands();

        //    m_SendVoiceInfo.PlayState = (int)PlayState.Stop;
        //    Login._VideoEngine.Send(NotifyType.Request_VoiceChat, m_SendVoiceInfo);

        //    m_SendVoiceInfo.PlayState = (int)PlayState.Ready;
        //}

        //private void waveOut_PlaybackStopped(object sender, EventArgs e)
        //{
        //    isPlaying = false;
        //    //RaiseCommands();
        //}

        //private void HandleVolumeUpdated(object sender, VolumeUpdatedEventArgs e)
        //{
        //    //if (!View.CheckAccess())
        //    //{
        //    //    View.Dispatcher.BeginInvoke(new ThreadStart(() =>
        //    //    {
        //    //        Volume = e.Volume;
        //    //    }));
        //    //}
        //}

        //#endregion

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            PresentHistoryInfo presentHistoryInfo = new PresentHistoryInfo();
            presentHistoryInfo.Cash = 100;
            for (int i = 0; i < m_roomDetailInfo.Users.Count; i++)
            {
                if (m_roomDetailInfo.Users[i].Kind == (int)UserKind.ServiceWoman)
                {
                    presentHistoryInfo.ReceiveId = m_roomDetailInfo.Users[i].Id;
                    break;
                }
            }


            SendMessage("100원을 선물하였습니다.");

            Login._ClientEngine.Send(ChatEngine.NotifyType.Request_Give, presentHistoryInfo);
        }


        private void button4_Click(object sender, RoutedEventArgs e)
        {
            PresentHistoryInfo presentHistoryInfo = new PresentHistoryInfo();
            presentHistoryInfo.Cash = 500;
            for (int i = 0; i < m_roomDetailInfo.Users.Count; i++)
            {
                if (m_roomDetailInfo.Users[i].Kind == (int)UserKind.ServiceWoman)
                {
                    presentHistoryInfo.ReceiveId = m_roomDetailInfo.Users[i].Id;
                    break;
                }
            }

            SendMessage("500원을 선물하였습니다.");

            Login._ClientEngine.Send(ChatEngine.NotifyType.Request_Give, presentHistoryInfo);
        }

        private bool InterpretSignalText(string strSignal, string strID)
        {
            string strUserName = Login._UserInfo.Id;
            if (strSignal.Equals(m_strVideoChatPass))
            {
                if(!strUserName.Equals(strID))
                {
                    //Main.m_icClient = m_iconfClient;
                    //Main.m_icServer.ViewUser(strID);
                    //receiveImage.Visibility = Visibility.Hidden;

                    //OpenRemoteUserVideo(true);
                    //icClient.Visibility = Visibility.Visible;
                    lblWebCam2.Content = LabelContents.YOU_CAM_LBL + "(" + strID + ")";
                    remoteVideoImage.Visibility = Visibility.Visible;
                    OpenRemoteUserVideo(true);
                }

                return false;
            }

            if (strSignal.Equals(m_strVideoChatEnd))
            {
                if (!strUserName.Equals(strID))
                {
                    //OpenRemoteUserVideo(false);
                    //icClient.Visibility = Visibility.Hidden;
                   // receiveImage.Visibility = Visibility.Visible;
                    lblWebCam2.Content = LabelContents.YOU_CAM_LBL;
                    remoteVideoImage.Visibility = Visibility.Hidden;
                    OpenRemoteUserVideo(false);

                    if (otherWebCamView.IsLoaded)
                        otherWebCamView.Close();

                    screenImg.Visibility = Visibility.Hidden;
                }

                return false; 
            }

            if (strSignal.Equals(m_strVoiceChatPass))
            {
                if(!strUserName.Equals(strID))
                {
                    string strMessage = strID + ManagerMessage.ALERT_VOICE_MESSAGE;
                    m_MessageQue.Add(strMessage);

                    OpenRemoteUserSpeak(true);
                }

                return false;
            }

            if (strSignal.Equals(m_strVoiceChatEnd))
            {
                if (!strUserName.Equals(strID))
                {
                    string strMessage = strID + ManagerMessage.ALERT_END_VOICE;
                    m_MessageQue.Add(strMessage);

                    OpenRemoteUserSpeak(false);
                }
                return false;
            }

            return true;
        }

        private void screenImg_MouseDown(object sender, MouseButtonEventArgs e)
        {

            otherWebCamView = new OtherWebCamView();
            otherWebCamView.Show();
        }

        //public void ShowWomanWebCam(iConfClientDotNet iconfClient, iConfServerDotNet iconfServer)
        //{
        //    icClient.Child = iconfClient;
        //    icServer.Child = iconfServer;
        //}

        //public void ShowWebCam(iConfServerDotNet iconfServer)
        //{
        //    icServer.Child = iconfServer;
        //}
    }

}

  