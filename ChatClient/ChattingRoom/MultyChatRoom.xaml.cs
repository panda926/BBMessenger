using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.IO;
using System.Threading;
using ChatEngine;
using System.Net.Sockets;
using System.Net;
//using AForge.Video;
//using AForge.Video.DirectShow;

//using NSpeex;
//using NAudio.Wave;

//using iConfClient.NET;
using System.Windows.Media.Animation;

// 작성자   : GreenRose
// 설명     : 채팅방을 창조하며 비디오통화와 음성통화 본문전송을 진행한다.
// 작성기간 : 2013-03-20부터 ~ 

namespace ChatClient
{
    //class myComboBoxItem
    //{
    //    protected string strText;
    //    protected string strItemData;
    //    protected string strItemData2;
    //    protected string strDispalyValue;

    //    //Adding only the visible text.
    //    public myComboBoxItem(string strTextToAdd)
    //    {
    //        strText = strTextToAdd;
    //        strDispalyValue = strTextToAdd;
    //        strItemData = "0";
    //    }
    //    //Adding the text and ItemData
    //    public myComboBoxItem(string strTextToAdd, string strItemDataToAdd)
    //    {
    //        strText = strTextToAdd;
    //        strItemData = strItemDataToAdd;
    //        strDispalyValue = strTextToAdd;
    //    }
    //    //Adding the text and ItemData
    //    public myComboBoxItem(string strTextToAdd, string strItemDataToAdd, string strItemDataToAdd2)
    //    {
    //        strText = strTextToAdd;
    //        strItemData = strItemDataToAdd;
    //        strItemData2 = strItemDataToAdd2;
    //        strDispalyValue = strTextToAdd;
    //    }

    //    public string ItemData
    //    {
    //        get { return strItemData; }
    //        set { strItemData = value; }
    //    }

    //    public string ItemData2
    //    {
    //        get { return strItemData2; }
    //        set { strItemData2 = value; }
    //    }

    //    public string DisplayValue
    //    {
    //        get { return strDispalyValue; }
    //        set { strDispalyValue = value; }
    //    }
    //    //This is text that will be added in to the ComboBox (display to user)
    //    public override string ToString()
    //    {
    //        return strText;
    //    }

    //}

    public partial class MultyChatRoom : Window
    {
        #region Varialble

        System.Windows.Threading.DispatcherTimer disapthcerTimer = null;
        System.Windows.Threading.DispatcherTimer dispatcherVideoTimer = null;
        System.Windows.Threading.DispatcherTimer dispatcherOtherTimer = null;


        // Font정보보관변수들...

        string                              m_strFontName = "";
        float                               m_fFontSize = 12;
        System.Drawing.FontStyle            m_fontStyle = new System.Drawing.FontStyle();
        System.Drawing.Color                m_fontColor = new System.Drawing.Color();


        // 기타 성원변수들...

        FlowDocument                        m_flowDoc = new FlowDocument();
        bool                                m_videoSenBtnState = false;                // 비디오전송단추가 클릭되였는가 식별하는 변수값.
        bool                                m_VoiceSendBtnState = false;

        List<IconInfo>                      m_listEmoticons = new List<IconInfo>(); // 서버로부터 보내여지는 Emoticon정보목록
        List<IconInfo>                      m_listPresents = new List<IconInfo>();  // 서버로부터 보내여지는 Present정보목록

        List<UserInfo>                      m_sendpresentUserList = new List<UserInfo>();
        

        public RoomInfo                     m_roomInfo = new RoomInfo();       // 방정보

        VoiceInfo                           m_SendVoiceInfo = new VoiceInfo();
        VoiceInfo                           m_voiceInfo = new VoiceInfo();
        VideoInfo                           m_videoInfo = new VideoInfo();

        bool                                m_bFirstVideoFrame = true;

        RoomDetailInfo                      m_roomDetailInfo = new RoomDetailInfo();
        bool                                m_bRoomStateChange = false;
        bool                                m_bEndChatFlag = false;

        string                              m_strPreviousText = "";

        UserInfo                            m_userFemaleInfo = new UserInfo();
        //GameWindow                          m_gameWindow = null;
        
        
        
        int                                 m_nVideoEndStae = 0;

        //VideoCaptureDevice                  m_localWebCam = null;
        //FilterInfoCollection                m_localWebCamCollection = null;

        //System.Windows.Controls.ComboBox    m_comboCameraTypes = new System.Windows.Controls.ComboBox();

        List<String>                        m_MessageQue = new List<String>();

        private presentMoney[] s_totalScoreList = new presentMoney[100];
        List<string> m_presentSendId = new List<string>();
        List<int> m_presentValue = new List<int>();
        private static int moneyIndex = 0;
                    
        bool m_presentFlag = false;

        //private SpeexEncoder                encoder;

        //private bool                        isRecording;
        //private bool                        isPlaying;

        //private NAudio.Wave.WaveFormat      waveFormat;
        //private IWaveIn                     waveIn;
        //private IWavePlayer                 waveOut;

        //private JitterBufferWaveProvider    waveProvider;

        //private iConfServerDotNet icServer;
        //private iConfClientDotNet icClient;

        //string strUserID = null;
        //string strUserPass = null;
        //string strServerUrl = null;
        //string strCentralPort = null;

        public string m_strVideoChatPass = "VideoChatStart+GIT";
        public string m_strVideoChatEnd = "VideoChatEnd+GIT";

        public string m_strVoiceChatPass = "VoiceChatStart+GIT";
        public string m_strVoiceChatEnd = "VoiceChatEnd+Git";

        public bool m_bMakingRoom = false;

        //private iConfClientDotNet m_iconfClient;

        public string ID { get; set; }
        public string UUri { get; set; }

        public List<IconInfo> listPresentInfo { get; set; }

        public event EventHandler PresentSelected = delegate { };
        //public string UUri { get; set; }

        private System.Windows.Threading.DispatcherTimer mutiChatClock = null;

        private bool m_bShowPresent = false;

        WebCamView webCamView = null;

        private static bool songState = true;

        bool m_initRoomEnter = true;

        TextBox roomValue = new TextBox();
        Button roomBt = new Button();
        MusicPlayer musicPlayer = null;

        int _MusicState = 0;

        #endregion


        public MultyChatRoom()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                nHour = 0;
                nMinute = 0;
                nSecond = 0;

                // 2013-05-24: GreenRose
                // IConf.net sdk
                InitAllSetting();

                //Main.m_icServer.InitializeAudioSystem(iConfServer.NET.iConfServerDotNet.audioType.DirectSound, -1, -1, 16000, 5);
                //Main.m_icServer.RemoveNoiseFromOutgoingAudio(true);


                // messageEditBox에 초점을 맞춘다.+
                this.messageEditBox.Focus();


                // 항시적으로 비디오가 들어오는가를 감시한다.

                CheckVideoFrame();


                // 항시적으로 상태를 감시한다.

                CheckOtherState();


                // 환영 메세지를 출력한다.

                ShowErrorOrMessage(ManagerMessage.MESSAGE_WELCOME);


                // Notify

                Login._ClientEngine.AttachHandler(OnReceive);
                Login._VideoEngine.AttachUdpHandler(OnVideoReceive);


                mutiChatClock = new System.Windows.Threading.DispatcherTimer();
                mutiChatClock.Interval = new TimeSpan(0, 0, 0, 1);
                mutiChatClock.Tick += new EventHandler(mutiChatClock_Tick);
                mutiChatClock.IsEnabled = true;


                // 사용자가 여자일때 웹캠전송을 진행한다.

                //if (Login._UserInfo.Kind == 1)
                //{
                //    this.numberMark.IsEnabled = false;
                //    this.btnSendMark.IsEnabled = false;
                //    SetVideoChat();
                //}


                // GameWindow창을 띄운다.

                //m_gameWindow = new GameWindow(this);
               // m_gameWindow.SetGameInfo(Login._ChatGameInfo);
               // m_gameWindow.Show();

                


                // 음성채팅가능.

                //EnableoVoiceChat();


                //NAudio.Wave.AudioFileReader waveReader = new NAudio.Wave.AudioFileReader("c:\\1.mp3");
                //var waveOut = new NAudio.Wave.WaveOut();
                //int nCount = NAudio.Wave.WaveOut.DeviceCount;
                //waveOut.DeviceNumber = 4;
                //waveOut.Init(waveReader);
                //waveOut.Play();

                //NAudio.Wave.DirectSoundOut output = null;
                //output = waveOut;
                //output.Init(waveReader);
                //output.Play();

                //////////
                //encoder = new SpeexEncoder(BandMode.Narrow);

                //waveProvider = new JitterBufferWaveProvider();
                //waveProvider.VolumeUpdated += HandleVolumeUpdated;


                //waveOut = new WaveOut();

                //waveOut.Init(waveProvider);
                //waveOut.PlaybackStopped += waveOut_PlaybackStopped;
                //waveOut.Volume = 1.0f;

                //m_SendVoiceInfo.UserId = Login._UserInfo.Id;

                //StartPlaying();


                // 2013-06-17: GreenRose
                //ShowPresent();
                PresentSelected += (s1, e1) => OnPresentSelected(((MultyChatRoom)s1).UUri, ((MultyChatRoom)s1).ID);
            }
            catch (Exception ex)
            { }
        }

        private void ShowPresent()
        {
            try
            {
                foreach(IconInfo presentIconInfo in m_listPresents)
                {
                    //var present = new ChatClient.Present.Present();
                    System.Windows.Controls.Button btImg = new System.Windows.Controls.Button();
                    btImg.Margin = new Thickness(1,1,1,1);
                    btImg.Content = presentIconInfo.Name;
                    btImg.Foreground = new SolidColorBrush(Colors.Black);
                    btImg.Cursor = System.Windows.Input.Cursors.Hand;

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
                if (MessageBoxCommon.Show(ManagerMessage.ALERT_PRESENT_SENDING, MessageBoxType.YesNo) == MessageBoxReply.No)
                    return;

                InsertPresentToRichTextBox(strUri, strPoint);
            }
            else
            {
                MessageBoxCommon.Show("帐号元宝不足.", MessageBoxType.Ok);
            }
        }


        static int nHour    = 0;
        static int nMinute  = 0;
        static int nSecond  = 0;

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

            string strSecond    = string.Empty;
            string strMinute    = string.Empty;
            string strHour      = string.Empty;

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

        
        // 2013-05-24: GreenRose Created
        private void InitAllSetting()
        {
            //Main.m_icServer.ViewUserResponseReceived += new iConfServer.NET.iConfServerDotNet.ViewUserResponseReceivedDelegate(this.icServer_ViewUserResponseReceived);

            //Main.m_icServer.StopPreview();
            //timeChat.Content = "0";
            //Main.m_icServer.JoinConferenceRoom(m_roomInfo.Id);
            //Main.m_icServer.StopTransmittingAudio();

            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {
                //ics.Child = Main.m_icServer;
                //Main.m_icServer.Show();
                
                //button3.IsEnabled = false;
                //button4.IsEnabled = false;
                //numberMark.IsEnabled = false;
                this.goodImg.IsEnabled = false;
                this.badImg.IsEnabled = false;
                this.profilImg.IsEnabled = false;
                this.screenImg.IsEnabled = false;

                songState = true;
                BitmapImage songBit = new BitmapImage();
                songBit.BeginInit();
                songBit.UriSource = new Uri("/Resources;component/image/song.png", UriKind.RelativeOrAbsolute);
                songBit.EndInit();
                mp3Player.Source = songBit;

                
                roomValue.Width = 60;
                roomValue.Text = "房价格";
                roomValue.Height = 23;
                roomValue.MaxLength = 3;
                roomValue.VerticalAlignment = VerticalAlignment.Center;
                roomValue.HorizontalAlignment = HorizontalAlignment.Left;
                roomValue.MouseEnter += new MouseEventHandler(roomValue_MouseEnter);

                roomBt.Content = "Setting";
                roomBt.Height = 23;
                roomBt.VerticalAlignment = VerticalAlignment.Center;
                roomBt.HorizontalAlignment = HorizontalAlignment.Right;
                roomBt.Click += new RoutedEventHandler(roomBt_Click);

                stateGrid.Children.Add(roomValue);
                stateGrid.Children.Add(roomBt);

                resetBt.Visibility = Visibility.Hidden;

            }
            else
            {
                songState = false;
                BitmapImage soundBit = new BitmapImage();
                soundBit.BeginInit();
                soundBit.UriSource = new Uri("/Resources;component/image/sound4.png", UriKind.RelativeOrAbsolute);
                soundBit.EndInit();
                mp3Player.Source = soundBit;

                //m_iconfClient = null;
                //m_iconfClient = new iConfClientDotNet();
                //m_iconfClient.ClearImage();

                //m_iconfClient.NewVideoFrameBytesAvailable += new iConfClient.NET.iConfClientDotNet.NewVideoFrameBytesAvailableDelegate(this.icc_NewVideoFrameBytesAvailable);

                //buttonSendMyVideo.IsEnabled = false;
                string strOwnerName = m_roomInfo.Owner;

                //ics.Child = m_iconfClient;
                //m_iconfClient.Show();
                //Main.m_icClient = m_iconfClient;
                //Main.m_icServer.ViewUser(strOwnerName);

                ics.Visibility = Visibility.Visible;

                BitmapImage vBit = new BitmapImage();
                vBit.BeginInit();
                vBit.UriSource = new Uri("/Resources;component/image/camera.gif", UriKind.RelativeOrAbsolute);
                vBit.EndInit();
                videoBt.Source = vBit;

                

                

                //receiveImage.Visibility = Visibility.Hidden;
            }

            ImageBrush myIconImg = new ImageBrush();
            myIconImg.Stretch = Stretch.Fill;
            myIconImg.ImageSource = ImageDownloader.GetInstance().GetImage(Login._UserInfo.Icon);
            userIcon.Fill = myIconImg;

            roomName.Content = Login._RoomInfo.Name + "(" + Login._RoomInfo.Id + ")";

            System.Windows.Controls.ToolTip roomTip = new System.Windows.Controls.ToolTip();
            roomTip.Content = roomName.Content;
            roomName.ToolTip = roomTip;

            if (Login._RoomInfo.Point > 0)
            {
                gameImg.IsEnabled = false;
                gameImg.Opacity = 0.5;
                gameImg.Cursor = System.Windows.Input.Cursors.Arrow;

                roomValue.Visibility = Visibility.Hidden;
                roomBt.Visibility = Visibility.Hidden;
            }
            else
            {
                Main._DiceGame = new DiceGame(this);
                Main._DiceGame.SetGameInfo(Login._ChatGameInfo);
                Main._DiceGame.Hide();

            }
           
        }

        void roomValue_MouseEnter(object sender, MouseEventArgs e)
        {
            roomValue.Text = "";
            roomValue.Focusable = true;
        }

        //방값을 설정한다.
        void roomBt_Click(object sender, RoutedEventArgs e)
        {
            string strPrice = System.Text.RegularExpressions.Regex.Replace(roomValue.Text, @"[0-9]", "");
            if (strPrice.Length > 0)
            {
                IniFileEdit iniFileEdit = new IniFileEdit(Login._UserPath);
                MessageBoxCommon.Show(iniFileEdit.GetIniValue("StringMessageBox", "Invalid_Number"), MessageBoxType.Ok);
                return;
            }
            else
            {
                try
                {
                    m_roomInfo.Cash = Convert.ToInt32(roomValue.Text);
                }
                catch (System.Exception)
                {
                    MessageBoxCommon.Show("방값을 정확히 입력하세요.", MessageBoxType.Ok);
                    return;
                }

                if (MessageBoxCommon.Show("设定费用为" + roomValue.Text + "元吗？" , MessageBoxType.YesNo) == MessageBoxReply.Yes)
                {    
                    Login._ClientEngine.Send(NotifyType.Request_RoomInfo, m_roomInfo);
                    var selbt = sender as Button;
                    selbt.IsEnabled = false;
                    roomValue.IsEnabled = false;
                }
            }
        }

        private void resetBt_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxCommon.Show("确定开始付费聊天吗？ 费用为:" + Login._RoomInfo.Cash + "元", MessageBoxType.YesNo) == MessageBoxReply.Yes)
            {
                ics.Visibility = Visibility.Visible;
                resetBt.IsEnabled = false;
                Login._ClientEngine.Send(NotifyType.Request_RoomPrice, Login._RoomInfo);
            }
            else
            {
                resetBt.IsEnabled = true;
            }
        }
        

        //private void icc_NewVideoFrameBytesAvailable(byte[] bitmapbytes)
        //{

        //}

        //private void icServer_ViewUserResponseReceived(object sender, System.String userName, System.Int32 port,
        //                                          System.Int32 videoWidth, System.Int32 videoHeight,
        //                                          System.String videoCodec)
        //{
        //    this.Dispatcher.Invoke(new MethodInvoker(delegate
        //    {
        //        strServerUrl = Login._ServerServiceUri;
        //        m_iconfClient.Call(strServerUrl, port, videoWidth, videoHeight, "n/a", "n/a", "n/a", 0, 0, 0, videoCodec);
        //    }
        //    ));

        //}

        // 2013-05-24: Modify Ending
        

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

                //if (m_bFirstVideoFrame && m_videoInfo.Data != null)
                //{
                //    m_bFirstVideoFrame = false;
                //}


                // 비디오가 들어온다면...

                if (m_videoInfo.Data != null)
                {
                    if (m_videoInfo.IsEnd == 1)
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri("/Resources;component/image/NoCamera.png", UriKind.RelativeOrAbsolute);
                        bitmapImage.EndInit();

                        //this.receiveImage.Source = bitmapImage;

                        m_bFirstVideoFrame = true;
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

                StringInfo messageInfo = new StringInfo();
                messageInfo.UserId = Login._UserInfo.Id;
                messageInfo.FontSize = 12;
                messageInfo.FontName = "Arial";
                messageInfo.FontStyle = "";
                messageInfo.FontWeight = "";
                messageInfo.FontColor = "Black";

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

        private void InitLabelCaption(RoomDetailInfo roomDetailInfo)
        {
            try
            {
                UserInfo userInfo = new UserInfo();
                for (int i = 0; i < roomDetailInfo.Users.Count; i++)
                {
                    if (Login._UserInfo.Id.Equals(roomDetailInfo.Users[i].Id))
                    {
                        userInfo = roomDetailInfo.Users[i];
                        break;
                    }
                }

                if (Login._RoomInfo.Point == 0)
                {
                    if (userInfo.Cash <= m_roomInfo.Cash && userInfo.Kind == 0)
                    {
                        ShowErrorOrMessage(ManagerMessage.NOT_ENOUGH_CASH);
                    }
                    var userToolTip = new System.Windows.Controls.ToolTip();
                    
                    lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + userInfo.Cash.ToString();
                    userToolTip.Content = userInfo.Nickname + " " + "(" + userInfo.Id + ")" + " " + userInfo.Cash;
                    lblCurReLoginPrice.ToolTip = userToolTip;
                    lblPricePerSecond.Content = LabelContents.PRICE_PER_SECOND + m_roomInfo.Cash.ToString();
                    
                    if (Main._DiceGame != null)
                    {
                        Main._DiceGame.self_money_label.Content = userInfo.Cash.ToString();
                    }

                    if (Login._RoomInfo.Cash > 0 && m_initRoomEnter == true)
                    {
                        resetBt.IsEnabled = true;
                        ics.Visibility = Visibility.Hidden;
                    }

                    m_initRoomEnter = false;
                    
                }
                else if (Login._RoomInfo.Point > 0)
                {
                    if (userInfo.Point <= m_roomInfo.Point && userInfo.Kind == 0)
                    {
                        ShowErrorOrMessage(ManagerMessage.NOT_ENOUGH_POINT);
                    }

                    lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_POINT_LBL + userInfo.Point.ToString();

                    var userPointToolTip = new System.Windows.Controls.ToolTip();
                    userPointToolTip.Content = userInfo.Nickname + " " + "(" + userInfo.Id + ")" + " " + userInfo.Point;
                    lblCurReLoginPrice.ToolTip = userPointToolTip;

                    lblPricePerSecond.Content = LabelContents.PRICE_PER_SECOND + m_roomInfo.Point.ToString();

                    
                }
            }
            catch (Exception)
            { }
        }


        // 서버로부터 응답이 들어올때...

        public void OnReceive(NotifyType type, Socket socket, BaseInfo baseInfo)
        {
            try
            {
                switch (type)
                {
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
                            AddMessageToHistoryTextBox(strInfo);
                        }
                        break;



                    case NotifyType.Reply_OutMeeting:
                        {
                            UserInfo userInfo = (UserInfo)baseInfo;
                            UserOut(userInfo);
                        }
                        break;
                    case NotifyType.Reply_MusiceInfo:
                        {
                            MusiceInfo musiceInfo = (MusiceInfo)baseInfo;
                            if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
                            {
                                string path = string.Format("c:\\{0}", musiceInfo.MusiceName);
                                if (File.Exists(path))
                                    break;
                                else
                                {
                                    try
                                    {
                                        File.WriteAllBytes(path, musiceInfo.MusiceData);
                                    }
                                    catch (Exception ex)
                                    {
                                        string strMsg = ex.Message;
                                    }
                                }
                            }
                            
                        }
                        break;

                    case NotifyType.Reply_MusiceStateInfo:
                        {
                            MusiceStateInfo musiceStateInfo = (MusiceStateInfo)baseInfo;
                            if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
                            {
                                _MusicState = musiceStateInfo.M_Kind;

                                WebDownloader.GetInstance().DownloadFile( "MusicFile/" + musiceStateInfo.MusiceName, MusicDownloadComplete, this );
                            }
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
                            UserInfo userInfo = new UserInfo();
                            PresentHistoryInfo presentHistoryInfo = (PresentHistoryInfo)baseInfo;

                            //if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                            //{
                            //    m_nCurCash = m_nCurCash + presentHistoryInfo.Cash;
                            //    lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + m_nCurCash.ToString();
                            //}

                            //if (Login._UserInfo.Id == presentHistoryInfo.SendId)
                            //{
                            //    m_nCurCash = m_nCurCash - presentHistoryInfo.Cash;
                            //    lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + m_nCurCash.ToString();
                            //}

                            ShowPresentHistory(presentHistoryInfo);
                        }
                        break;

                    case NotifyType.Reply_RoomInfo:
                        {
                            RoomInfo roomInfo = (RoomInfo)baseInfo;
                            Login._RoomInfo = roomInfo;
                            if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
                            {
                                ics.Visibility = Visibility.Hidden;
                                if (MessageBoxCommon.Show("接受视频邀请吗？ 价格：" + roomInfo.Cash, MessageBoxType.YesNo) == MessageBoxReply.Yes)
                                {
                                    ics.Visibility = Visibility.Visible;
                                    //RoomPrice roomPrice = new RoomPrice();
                                    //roomPrice.UserId = Login._UserInfo.Id;
                                    //roomPrice.RoomId = m_roomInfo.Id;
                                    //roomPrice.RoomValue = roomInfo.Cash;
                                    //roomPrice.ReceiveId = roomInfo.Owner;

                                    Login._ClientEngine.Send(NotifyType.Request_RoomPrice, roomInfo);
                                }
                                else
                                {
                                    resetBt.IsEnabled = true;
                                }
                            }
                        }
                        break;

                    case NotifyType.Reply_PartnerDetail:
                        {
                            if (Main.selectUserHome == null)
                            {
                                Main.selectUserHome = new SelectUserHome();
                                Main.selectUserHome.selUserDetail = Login.selectUserDetail;
                                Main.selectUserHome.InitTabSetting(Login.selectInfo);
                                Main.selectUserHome.Show();
                            }
                        }
                        break;

                    
                }
            }
            catch (Exception)
            { }
        }

        public void MusicDownloadComplete(string filePath)
        {
            Main.novelPlayStateFlag = false;

            switch (_MusicState)
            {
                case 0:
                    {
                        mediaElement1.Source = new Uri(filePath, UriKind.RelativeOrAbsolute);
                        mediaElement1.Play();
                        mediaElement1.Volume = (double)sliderVolume.Value;
                    }
                    break;
                case 1:
                    {
                        mediaElement1.Pause();
                    }
                    break;
                case 2:
                    {
                        mediaElement1.Stop();
                    }
                    break;
            }
        }

        // 사용자가 채팅을 종료하였을때...

        private void UserOut(UserInfo userInfo)
        {
            try
            {
                if (userInfo.Id.Equals(Login._UserInfo.Id))
                {
                    if (m_bEndChatFlag == false)    
                    {
                        ShowErrorOrMessage(ManagerMessage.ALERT_SERVICE_OUT);
                        m_bEndChatFlag = true;
                    }

                    Login._UserInfo = userInfo;
                    Main.chatRoom = null;

                    var timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(3);
                    timer.Tick += delegate { this.Close(); };
                    timer.Start();
                }
                else
                {
                    string strMessage = userInfo.Id + ManagerMessage.ALERT_USER_OUT;
                    ShowErrorOrMessage(strMessage);
                }
            }
            catch (Exception)
            { }
            //try
            //{
            //    if (userInfo.Id.Equals(Login._UserInfo.Id))
            //    {
            //        ShowErrorOrMessage(ManagerMessage.ALERT_END_CHAT);
            //        //Thread.Sleep(2000);

            //        Login._UserInfo = userInfo;
            //        Main.multyChatRoom = null;
            //        this.Close();
            //    }
            //    else
            //    {
            //        string strMessage = userInfo.Id + ManagerMessage.ALERT_USER_OUT;
            //        ShowErrorOrMessage(strMessage);
            //    }
            //}
            //catch (Exception)
            //{ }
        }


        // 서버로부터 UDP응답이 들어올때...
        private List<string> _LogStrings = new List<String>();
        //private DateTime prevTime;

        public void OnVideoReceive(NotifyType type, IPEndPoint ipEndPoint, BaseInfo baseInfo)
        {
            try
            {
                switch (type)
                {
                    case NotifyType.Reply_VideoChat:
                        {
                            VideoInfo videoInfo = (VideoInfo)baseInfo;
                            m_videoInfo = videoInfo;
                        }
                        break;

                    case NotifyType.Reply_VoiceChat:
                        {
                            m_voiceInfo = (VoiceInfo)baseInfo;
                            
                                //ReceiveVoiceInfo(m_voiceInfo);

                            
                                //DateTime curTime = DateTime.Now;
                                //TimeSpan delay = curTime - prevTime;
                                //string logString = string.Format("Receive : {0} : ip-{1}, port-{2}, size-{3}", delay.TotalMilliseconds, ipEndPoint.Address, ipEndPoint.Port, m_voiceInfo.GetSize());
                                //_LogStrings.Add(logString);

                                //prevTime = curTime;
                            
                        }
                        break;
                }
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
                stackPanel1.Children.Clear();
                // 방에 입장한 사용자들의 정보를 유저리스트에 보여준다.

                if (roomDetailInfo.Users.Count < 1 || roomDetailInfo.Users == null)
                    return;

                // 선물을 보낸 사용자목록
                List<UserInfo> sendGiveUserInfo = new List<UserInfo>();

                for (int i = 0; i < roomDetailInfo.Users.Count; i++)
                {
                    if (roomDetailInfo.Users[i].Kind == 1)
                    {
                        m_userFemaleInfo = roomDetailInfo.Users[i];

                        int nCount = 0;
                        nCount = m_userFemaleInfo.Evaluation;

                        //if (nCount > 0)
                        //    this.goodCount.Content = nCount.ToString();
                        //else
                        //    this.goodCount.Content = string.Empty;


                        nCount = m_userFemaleInfo.Visitors - m_userFemaleInfo.Evaluation;

                        //if (nCount > 0)
                        //    this.badCount.Content = nCount.ToString();
                        //else
                        //    this.badCount.Content = string.Empty;

                        this.lblTotalPresent.Content = LabelContents.TOTAL_PRESENT_PRICE + " " + m_userFemaleInfo.ReceiveSum.ToString();
                    }

                    if (Login._UserInfo.Id == roomDetailInfo.Users[i].Id)
                        continue;

                    Grid userGrid = new Grid();
                    userGrid.Width = 200;
                    userGrid.Height = 20;
                    userGrid.Margin = new Thickness(0, 2, 0, 2);

                    Image img = new Image();
                    img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    img.Stretch = Stretch.Fill;

                    img.Source = ImageDownloader.GetInstance().GetImage(roomDetailInfo.Users[i].Icon);
                    img.Width = 20;
                    img.Height = 20;


                    TextBlock textBlock = new TextBlock();
                    textBlock.Margin = new Thickness(30, 0, 0, 0);
                    textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                    if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                    {
                        textBlock.Text = roomDetailInfo.Users[i].Nickname + "( " + roomDetailInfo.Users[i].Cash + " )";
                    }
                    else
                        textBlock.Text = roomDetailInfo.Users[i].Nickname;

                    System.Windows.Controls.ToolTip userNameTip = new System.Windows.Controls.ToolTip();
                    userNameTip.Content = roomDetailInfo.Users[i].Nickname + "( " + roomDetailInfo.Users[i].Id + " )";
                    textBlock.ToolTip = userNameTip;

                    userGrid.Children.Add(img);
                    userGrid.Children.Add(textBlock);

                    stackPanel1.Children.Add(userGrid);


                    // 선물보낸 사람들의 목록을 Sort하여 보여준다.
                    if (roomDetailInfo.Users[i].SendSum != 0 && roomDetailInfo.Users[i].Kind != (int)UserKind.ServiceWoman)
                        sendGiveUserInfo.Add(roomDetailInfo.Users[i]);
                }

                if (sendGiveUserInfo.Count > 0)
                    ShowSendGiveUserInfo(sendGiveUserInfo);


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

                InitLabelCaption(roomDetailInfo);
            }
            catch (Exception)
            { }
        }


        private void ShowSendGiveUserInfo(List<UserInfo> listUserInfo)
        {
            //var SortedUserInfos = from userInfo in listUserInfo orderby userInfo.SendSum ascending select userInfo;

            Array.Sort<presentMoney>(s_totalScoreList, (x, y) => y.s_money.CompareTo(x.s_money));

            sendGiveUserInfo.Children.Clear();
            for (int j = 0; j < s_totalScoreList.Length;j++ )
            {
                Grid userGrid = new Grid();
                userGrid.Width = 200;
                userGrid.Height = 20;
                userGrid.Margin = new Thickness(0, 2, 0, 2);

                


                TextBlock textBlock = new TextBlock();
                textBlock.Margin = new Thickness(30, 0, 0, 0);
                textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                for (int i = 0; i < listUserInfo.Count; i++)
                {
                    if (s_totalScoreList[j].s_userId == listUserInfo[i].Id)
                    {
                        Image img = new Image();
                        img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        img.Stretch = Stretch.Fill;

                        img.Source = ImageDownloader.GetInstance().GetImage(listUserInfo[i].Icon);
                        img.Width = 20;
                        img.Height = 20;

                        textBlock.Text = listUserInfo[i].Nickname + "( " + s_totalScoreList[j].s_money + " )";

                        userGrid.Children.Add(img);
                        userGrid.Children.Add(textBlock);

                        sendGiveUserInfo.Children.Add(userGrid);
                    }
                }
            }
        }
        struct presentMoney
        {
            public string s_userId;
            public int s_money;
        }

        private void ShowPresentHistory(PresentHistoryInfo presentHistoryInfo)
        {
            try
            {
                //s_totalScoreList

                bool sameFlag = false;
                Grid userGrid = new Grid();
                userGrid.Width = 200;
                userGrid.Height = 20;
                userGrid.Margin = new Thickness(0, 2, 0, 2);

                Image img = new Image();
                img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                img.Stretch = Stretch.Fill;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("/Resources;component/image/present/" + presentHistoryInfo.Descripiton, UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();

                img.Source = bitmapImage;
                img.Width = 20;
                img.Height = 20;


                TextBlock textBlock = new TextBlock();
                textBlock.Margin = new Thickness(30, 0, 0, 0);
                textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                textBlock.Text = presentHistoryInfo.SendId;

                if (m_presentFlag == false)
                {
                    s_totalScoreList[moneyIndex] = new presentMoney()
                    {
                        s_userId = presentHistoryInfo.SendId,
                        s_money = presentHistoryInfo.Cash
                    };
                    
                    m_presentFlag = true;
                }
                else
                {
                    for (int i = 0; i < s_totalScoreList.Length; i++)
                    {
                        if (presentHistoryInfo.SendId == s_totalScoreList[i].s_userId)
                        {
                            s_totalScoreList[i].s_money = s_totalScoreList[i].s_money + presentHistoryInfo.Cash;
                            sameFlag = true;
                        }
                        
                    }
                    if (sameFlag == false)
                    {
                        s_totalScoreList[moneyIndex] = new presentMoney()
                        {
                            s_userId = presentHistoryInfo.SendId,
                            s_money = presentHistoryInfo.Cash
                        };
                    }
                }

                ++moneyIndex;

                userGrid.Children.Add(img);
                userGrid.Children.Add(textBlock);

                presentHistory.Children.Add(userGrid);

                presentHistoryScroll.ScrollToBottom();
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
                //m_localWebCam.DesiredFrameSize = new System.Drawing.Size(480, 320);
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
        //    { }
        //}


        // 폼이 닫길때...

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_bEndChatFlag == false)
            {
                if (MessageBoxCommon.Show("Do you want to exit the room?", MessageBoxType.YesNo) == MessageBoxReply.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            
            FinalProcess();
        }


        // 폼클로즈시 마감처리를 진행한다.

        private void FinalProcess()
        {
            try
            {
                if (disapthcerTimer != null)
                    disapthcerTimer.Stop();

                if (dispatcherVideoTimer != null)
                    dispatcherVideoTimer.Stop();


                //if (m_localWebCam != null)
                //{
                //    if (m_localWebCam.IsRunning)
                //        m_localWebCam.Stop();

                //    m_localWebCam = null;
                //}

                //Main.m_icServer.Listen(false, "", 0, 0, 0);
                //Main.m_icServer.StopPreview();
                //Main.m_icServer.LeaveConference();
                //Main.m_icServer.LeaveCurrentConferenceRoom();
                //if (m_iconfClient != null)
                //{
                //    m_iconfClient.Disconnect();
                //    m_iconfClient.Dispose();                    
                //}

                if (musicPlayer != null)
                    musicPlayer.Close();

                Thread.Sleep(1000);

                // VoiceChat Stop
                //StopPlaying();
                //StopRecording();

                if (Convert.ToInt32(m_roomInfo.Id) < 1000)
                    Login._ClientEngine.Send(NotifyType.Request_OutMeeting, m_roomInfo);
                else
                {
                    if (mutiChatClock != null)
                    {
                        //mutiChatClock.Stop();
                        mutiChatClock.IsEnabled = false;
                        mutiChatClock = null;
                    }
                    
                    Login._ClientEngine.Send(NotifyType.Request_OutRoom, m_roomInfo);
                }

                Main.multyChatRoom = null;

                if (Main._DiceGame != null)
                {
                    Login._ClientEngine.Send(NotifyType.Request_OutGame, Main._DiceClient._GameInfo);
                    Main._DiceGame.Close();
                }

                if (webCamView != null)
                    webCamView.Close();

                if (mutiChatClock != null)
                    mutiChatClock.Stop();

                Main._nTotalBytesPerSecond = -1;
                

                Login._ClientEngine.DetachHandler(OnReceive);
                Login._VideoEngine.DetachUdpHander(OnVideoReceive);
            }
            catch (Exception ex)
            {
                ShowErrorOrMessage(ManagerMessage.ERROR_MICROPHONE);
            }
        }


        // 캐쉬충전단추를 클릭하였을때...

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("control", "mmsys.cpl,,1");
        }


        // 폰트설정단추를 클릭하였을때...
        private void Image_MouseUp_2(object sender, MouseButtonEventArgs e)
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
            catch (Exception ex)
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


        private void videoBt_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetVideoChat();
        }

        // 비디오전송단추를 클릭한후의 처리...

        private void SetVideoChat()
        {
            if (!m_videoSenBtnState)
            {
                 m_videoSenBtnState = true;

                 BitmapImage vBit = new BitmapImage();
                 vBit.BeginInit();
                 vBit.UriSource = new Uri("/Resources;component/image/camera.gif", UriKind.RelativeOrAbsolute);
                 vBit.EndInit();
                 videoBt.Source = vBit;

                //receiveImage.Visibility = Visibility.Hidden;
                ics.Visibility = Visibility.Visible;
                //Main.m_icServer.StartPreview(0);
            }
            else
            {
                //receiveImage.Visibility = Visibility.Visible;
                ics.Visibility = Visibility.Hidden;
                //Main.m_icServer.StopPreview();

                BitmapImage veBit = new BitmapImage();
                veBit.BeginInit();
                veBit.UriSource = new Uri("/Resources;component/image/camera1.gif", UriKind.RelativeOrAbsolute);
                veBit.EndInit();
                videoBt.Source = veBit;

                m_videoSenBtnState = false;
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

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!m_VoiceSendBtnState)
                {
                    //Main.m_icServer.StartTransmittingAudio();

                    BitmapImage sBit = new BitmapImage();
                    sBit.BeginInit();
                    sBit.UriSource = new Uri("/Resources;component/image/sound1.png", UriKind.RelativeOrAbsolute);
                    sBit.EndInit();
                    soundBt.Source = sBit;
                    

                    m_VoiceSendBtnState = true;

                    SendMessage(m_strVoiceChatPass);
                }
                else
                {
                    BitmapImage eBit = new BitmapImage();
                    eBit.BeginInit();
                    eBit.UriSource = new Uri("/Resources;component/image/sound2.png", UriKind.RelativeOrAbsolute);
                    eBit.EndInit();
                    soundBt.Source = eBit;

                    SendMessage(m_strVoiceChatEnd);

                    m_VoiceSendBtnState = false;
                    //Main.m_icServer.StopTransmittingAudio();
                }
               

            }
            catch (Exception ex)
            {
                ShowErrorOrMessage(ManagerMessage.ERROR_MICROPHONE);
            }
        }


        // Emoticon단추를 클릭하였을때...

        private void Image_MouseUp_1(object sender, MouseButtonEventArgs e)
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
                WpfAnimatedGif.ImageBehavior.SetRepeatBehavior(img, RepeatBehavior.Forever);
                

                InlineUIContainer ui = new InlineUIContainer(img);

                tp.Paragraph.Inlines.Add(ui);

                messageEditBox.CaretPosition = messageEditBox.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);

                messageEditBox.Focus();
            }
            catch (Exception)
            {
            }
        }


        private void InsertPresentToRichTextBox(string strUri, string strPoint)
        {
            try
            {
                PresentHistoryInfo presentHistoryInfo = new PresentHistoryInfo();
                presentHistoryInfo.Cash = Convert.ToInt32(strPoint);
                presentHistoryInfo.Descripiton = strUri;

                for (int i = 0; i < m_roomDetailInfo.Users.Count; i++)
                {
                    if (m_roomDetailInfo.Users[i].Kind == (int)UserKind.ServiceWoman)
                    {
                        presentHistoryInfo.ReceiveId = m_roomDetailInfo.Users[i].Id;
                        break;
                    }
                }

                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_Give, presentHistoryInfo);
                //strUri = "/Resources;component/image/present/" + strUri;

                //string strMessage = "<" + strUri + ">" + "선물을 보냅니다.";
                //SendMessage(strMessage);
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
                    //this.btnSendMark.IsEnabled = false;
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

                   // numberMark.Opacity = 1;
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
                int nFontSize = 0;
                string strFontName = "";
                string strFontStyle = "";
                string strFontWeight = "";
                string strFontColor = "";

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

                StringInfo messageInfo = new StringInfo();
                messageInfo.UserId = Login._UserInfo.Id;
                messageInfo.String = strMessage;
                messageInfo.FontSize = nFontSize;
                messageInfo.FontName = strFontName;
                messageInfo.FontStyle = strFontStyle;
                messageInfo.FontWeight = strFontWeight;
                messageInfo.FontColor = strFontColor;

                Login._ClientEngine.Send(NotifyType.Request_StringChat, messageInfo);
            }
            catch (Exception)
            { }
        }


        private void AddMessageToHistoryTextBox(StringInfo messageInfo)
        {
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
                    WpfAnimatedGif.ImageBehavior.SetRepeatBehavior(img, RepeatBehavior.Forever);

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
            disapthcerTimer = new System.Windows.Threading.DispatcherTimer();
            disapthcerTimer.Interval = new TimeSpan(0, 0, 0, 0, 70);
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
            try
            {
                //RenderTargetBitmap bmp = new RenderTargetBitmap((int)(318), (int)(238), 96, 96, PixelFormats.Pbgra32);
                //bmp.Render(drawingVisual);

                //FormatConvertedBitmap bitmap = new FormatConvertedBitmap();
                //bitmap.BeginInit();
                //bitmap.Source = bmp;
                //bitmap.DestinationFormat = PixelFormats.Indexed8;
                //BitmapPalette palette = BitmapPalettes.WebPalette;
                //bitmap.DestinationPalette = palette;
                //bitmap.EndInit(); 

                //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                //encoder.Frames.Add(BitmapFrame.Create(bmp));
                //encoder.QualityLevel = 25;


                byte[] bit = Encoding.ASCII.GetBytes("VideoChating");

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
            catch (Exception ex)
            {
                string strMsg = ex.Message;
            }
        }

        private void ReceiveBit(byte[] receiveData)
        {
            string errorMessage = null;

            try
            {
                if (m_bFirstVideoFrame)
                {
                    //Main.m_icServer.ViewUser(m_roomInfo.Id);

                    m_bFirstVideoFrame = false;
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

        //public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        //{
        //    byte[] buffer = new byte[2000];
        //    int len;
        //    while ((len = input.Read(buffer, 0, 2000)) > 0)
        //    {
        //        output.Write(buffer, 0, len);
        //    }
        //    output.Flush();
        //}

        //static byte[] GetBytes(string str)
        //{
        //    byte[] bytes = new byte[str.Length * sizeof(char)];
        //    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        //    return bytes;
        //}

        //static string GetString(byte[] bytes)
        //{
        //    char[] chars = new char[bytes.Length / sizeof(char)];
        //    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        //    return new string(chars);
        //}

        //public static void CopyTo(Stream src, Stream dest)
        //{
        //    byte[] bytes = new byte[4096];

        //    int cnt;

        //    while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        //    {
        //        dest.Write(bytes, 0, cnt);
        //    }
        //}

        //public static byte[] Zip(string str)
        //{
        //    var bytes = Encoding.UTF8.GetBytes(str);

        //    using (var msi = new MemoryStream(bytes))
        //    using (var mso = new MemoryStream())
        //    {
        //        using (var gs = new GZipStream(mso, CompressionMode.Compress))
        //        {
        //            //msi.CopyTo(gs);
        //            CopyTo(msi, gs);
        //        }

        //        return mso.ToArray();
        //    }
        //}

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

        //    if (waveIn == null)
        //        return;

        //    waveIn.StopRecording();
        //    waveIn = null;
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
        //        m_MessageQue.Add(strMessage);
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
        //    System.Buffer.BlockCopy(e.Buffer, 0, data, 0, e.BytesRecorded);

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

        //            //DateTime curTime = DateTime.Now;
        //            //TimeSpan delay = curTime - sendPrevTime;
        //            //string logString = string.Format("Send : {0} ", delay.TotalMilliseconds);
        //            //_LogStrings.Add(logString);

        //            //sendPrevTime = curTime;
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

            if (strSignal.Equals(m_strVoiceChatPass))
            {
                if (!strUserName.Equals(strID))
                {
                    string strMessage = strID + ManagerMessage.ALERT_VOICE_MESSAGE;
                    m_MessageQue.Add(strMessage);
                }

                return false;
            }

            if (strSignal.Equals(m_strVoiceChatEnd))
            {
                if (!strUserName.Equals(strID))
                {
                    string strMessage = strID + ManagerMessage.ALERT_END_VOICE;
                    m_MessageQue.Add(strMessage);
                }
                return false;
            }

            return true;
        }

        private void profilImg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
            {
                for (int i = 0; i < m_roomDetailInfo.Users.Count; i++)
                {
                    if (m_roomDetailInfo.Users[i].Kind == (int)UserKind.ServiceWoman)
                    {
                        Login.selectInfo = m_roomDetailInfo.Users[i];
                        Login._ClientEngine.Send(NotifyType.Request_PartnerDetail, Login.selectInfo);
                    }
                }
            }
        }

        private void goodImg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EvaluateWoman(true);
        }

        private void badImg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EvaluateWoman(false);
        }

        private void screenImg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            webCamView = new WebCamView();
            //webCamView.m_iconfClient = Main.m_icClient;
            webCamView.m_multiChatRoom = this;
            webCamView.Show();
        }


        //public void ShowWomanWebCam(iConfClientDotNet iconfClient)
        //{
        //    ics.Child = iconfClient;
        //}


        // 점수보내기 함수
        private void EvaluateWoman(bool bFeeling)
        {
            try
            {
                int nNum;
                if (bFeeling)
                    nNum = 1;
                else
                    nNum = 0;

                EvalHistoryInfo evalHistoryInfo = new EvalHistoryInfo();
                evalHistoryInfo.OwnId = m_userFemaleInfo.Id;
                evalHistoryInfo.Value = nNum;
                evalHistoryInfo.BuyerId = Login._UserInfo.Id;
                Login._ClientEngine.Send(NotifyType.Request_Evaluation, evalHistoryInfo);
            }
            catch (Exception)
            { }
        }

        // 2013-06-13: GreenRose
        // 이미지를 이동시키는 함수.
        public void MoveTo(Image target, double newX, double newY)
        {
            var top = Canvas.GetTop(target);
            var left = Canvas.GetLeft(target);

            //var imgSource = (BitmapImage)WpfAnimatedGif.ImageBehavior.GetAnimatedSource(target);

            Image image = new Image();
            //image.Source = target.Source;
            //BitmapImage bitmapImage = new BitmapImage();
            //bitmapImage.BeginInit();
            //bitmapImage.UriSource = new Uri(imgSource.UriSource.ToString, UriKind.RelativeOrAbsolute);
            //bitmapImage.EndInit();

            image.BeginInit();
            image.Source = target.Source;
            image.EndInit();

            top = 100;
            left = 100;
            TranslateTransform trans = new TranslateTransform();
            image.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(top, newY - top, TimeSpan.FromSeconds(10));
            DoubleAnimation anim2 = new DoubleAnimation(left, newX - left, TimeSpan.FromSeconds(10));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);
        }

       public bool m_gamestate = false;
        
        private void gameImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!m_gamestate)
                {
                    BitmapImage gBit = new BitmapImage();
                    gBit.BeginInit();
                    gBit.UriSource = new Uri("/Resources;component/image/game1.gif", UriKind.RelativeOrAbsolute);
                    gBit.EndInit();
                    gameImg.Source = gBit;

                    m_gamestate = true;

                    if (Main._DiceGame != null)
                    {
                        Main._DiceGame.soundState = true;
                        Main._DiceGame.Show();
                    }
                }
                else
                {
                    BitmapImage eBit = new BitmapImage();
                    eBit.BeginInit();
                    eBit.UriSource = new Uri("/Resources;component/image/game2.gif", UriKind.RelativeOrAbsolute);
                    eBit.EndInit();
                    gameImg.Source = eBit;

                    m_gamestate = false;

                    if (Main._DiceGame != null)
                    {
                        Main._DiceGame.soundState = false;
                        Main._DiceGame.Hide();
                    }
                }


            }
            catch (Exception ex)
            {
                ShowErrorOrMessage(ManagerMessage.ERROR_MICROPHONE);
            }
        }
        bool volumeState = true;
        private void mp3Player_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (songState == true)
            {
                musicPlayer = new MusicPlayer();
                musicPlayer.Show();
            }
            else
            {
                if (volumeState == true)
                {
                    volumeState = false;
                    sliderVolume.Visibility = Visibility.Visible;
                }
                else
                {
                    volumeState = true;
                    sliderVolume.Visibility = Visibility.Hidden;
                }
            }
        }
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaElement1.Volume = (double)sliderVolume.Value;
        }

       

        

        
        
        //private void image_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.MoveTo(image, 0, 0);
        //}

        //private void present1_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    //this.MoveTo(present1, 0, 0);
        //}

        //private void image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        //{
        //    MoveTo(image, 100, 100);
        //}
       
    }

}

