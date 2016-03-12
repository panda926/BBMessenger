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

using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using com.hitops.MOP.YTD.YardTruckDriver;
using ChatClient;
using ChatEngine;
using System.Net.Sockets;
using System.Net;
using Voice;
using System.Collections;


// 작성자   : GreenRose
// 설명     : 채팅방을 창조하며 비디오통화와 음성통화 본문전송을 진행한다.
// 작성기간 : 2013-03-20부터 ~ 

namespace Sample
{
    public partial class CommRoom : Window
    {
        #region Varialble

        System.Windows.Threading.DispatcherTimer disapthcerTimer        = null;
        System.Windows.Threading.DispatcherTimer dispatcherVideoTimer   = null;


        // Font정보보관변수들...

        string                      m_strFontName       = "";
        float                       m_fFontSize         = 12;
        System.Drawing.FontStyle    m_fontStyle         = new System.Drawing.FontStyle();
        System.Drawing.Color        m_fontColor         = new System.Drawing.Color();


        // 기타 성원변수들...

        FlowDocument                m_flowDoc           = new FlowDocument();
        bool                        m_videoSenBtnState  = false;                // 비디오전송단추가 클릭되였는가 식별하는 변수값.
        bool                        m_voiceSendBtnState = false;                // 음성전송단추가 클릭되였는가 식별하는 변수값. 
        List<IconInfo>              m_listEmoticons     = new List<IconInfo>(); // 서버로부터 보내여지는 Emoticon정보목록
        public RoomInfo             m_roomInfo          = new RoomInfo();       // 방정보

        private WaveOutPlayer       m_Player;
        private WaveInRecorder      m_Recorder;
        private FifoStream          m_Fifo              = new FifoStream();
        VoiceInfo                   m_voiceInfo         = new VoiceInfo();

        private byte[]              m_PlayBuffer;
        private byte[]              m_RecBuffer;

        bool                        m_bVoicePlayerFlag  = false;
        bool                        m_bFirstVideoFrame  = true;
        bool                        m_bFirstVoiceFrame  = true;

        Queue                       queue               = new Queue();
        RoomDetailInfo              m_roomDetailInfo    = new RoomDetailInfo();
        bool                        m_bRoomStateChange  = false;

        string                      m_strPreviousText   = "";

        UserInfo                    m_userFemaleInfo    = new UserInfo();


        #endregion


        public CommRoom()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblWebCam1.Content = LabelContents.WEB_CAM_LBL;
            lblWebCam2.Content = LabelContents.WEB_CAM_LBL;


            // MessageEditBox에 초점을 맞추어놓는다.

            this.messageEditBox.Focus();


            // 항시적으로 비디오가 들어오는가를 감시한다.

            CheckVideoFrame();


            // 환영 메세지를 출력한다.

            ShowErrorOrMessage(ManagerMessage.MESSAGE_WELCOME);


            // 음성채팅가능.
            Stop();
            WaveFormat fmt = new WaveFormat(44100, 16, 2);
            m_Player = new WaveOutPlayer(-1, fmt, 3000, 3, new BufferFillEventHandler(Filler));
            m_bVoicePlayerFlag = true;


            // Notify

            Main._ClientEngine.AttachHandler(OnReceive);

            Main._VideoEngine.AttachUdpHandler(OnVideoReceive);


            // 사용자가 여자일때 웹캠전송을 진행한다.

            if (Main._UserInfo.Kind == 1)
            {
                this.numberMark.IsEnabled = false;
                this.btnSendMark.IsEnabled = false;
                SetVideoChat();
            }
        }


        // 30ms간격으로 비디오가 들어오는가를 검사한다.

        private void CheckVideoFrame()
        {
            dispatcherVideoTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherVideoTimer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            dispatcherVideoTimer.Tick += new EventHandler(Each_Video_Tick);
            dispatcherVideoTimer.Start();
        }


        // 비디오검사

        private void Each_Video_Tick(Object obj, EventArgs e)
        {
            try
            {
                // 첫 비디오프레임이 들어온다면...

                if (m_bFirstVideoFrame && queue.Count > 0)
                {
                    VideoInfo videoInfo = new VideoInfo();
                    videoInfo = (VideoInfo)queue.Dequeue();

                    lblWebCam1.Content = videoInfo.UserId + LabelContents.WEB_CAM_LBL;
                    m_bFirstVideoFrame = false;
                }


                // 비디오가 들어온다면...

                if (queue.Count > 0)
                {
                    VideoInfo videoInfo = new VideoInfo();
                    videoInfo = (VideoInfo)queue.Dequeue();

                    ReceiveBit(videoInfo.Data);
                }


                // 보이스정보가 들어온다면...

                if (m_Fifo.Length > 0 && m_bFirstVoiceFrame)
                {
                    m_bFirstVoiceFrame = false;
                    string strMessage = m_voiceInfo.UserId + ManagerMessage.ALERT_VOICE_MESSAGE;
                    ShowErrorOrMessage(strMessage);
                }


                // 방의 상태가 변한다면...

                if (m_bRoomStateChange == true)
                {
                    m_bRoomStateChange = false;
                    InitRoomState(m_roomDetailInfo);
                }
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
                    if (Main._UserInfo.Id.Equals(roomDetailInfo.Users[i].Id))
                    {
                        userInfo = roomDetailInfo.Users[i];
                        break;
                    }
                }

                if (userInfo.Cash != 0)
                {
                    if (userInfo.Cash <= m_roomInfo.Cash && userInfo.Kind == 0)
                    {
                        ShowErrorOrMessage(ManagerMessage.NOT_ENOUGH_CASH);
                    }

                    lblCurRemainPrice.Content = LabelContents.CUR_REMAIN_PRICE_LBL + userInfo.Cash.ToString();
                    lblCurUsingPrice.Content = LabelContents.CUR_USE_PRICE_LBL + m_roomInfo.Cash.ToString();
                    lblPricePerSecond.Content = LabelContents.PRICE_PER_SECOND + m_roomInfo.Cash.ToString();
                }
                else if (userInfo.Point != 0)
                {
                    if (userInfo.Point <= m_roomInfo.Point && userInfo.Kind == 0)
                    {
                        ShowErrorOrMessage(ManagerMessage.NOT_ENOUGH_POINT);
                    }

                    lblCurRemainPrice.Content = LabelContents.CUR_REMAIN_POINT_LBL + userInfo.Point.ToString();
                    lblCurUsingPrice.Content = LabelContents.CUR_USE_POINT_LBL + m_roomInfo.Point.ToString();
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

                    case NotifyType.Reply_VoiceChat:
                        {
                            VoiceInfo voiceInfo = (VoiceInfo)baseInfo;
                            m_voiceInfo = voiceInfo;
                            ReceiveVoiceInfo(voiceInfo.Data);
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
                if (userInfo.Id.Equals(Main._UserInfo.Id))
                {
                    ShowErrorOrMessage(ManagerMessage.ALERT_END_CHAT);
                    Thread.Sleep(2000);

                    Main._UserInfo = userInfo;
                    this.Close();
                }
                else
                {
                    string strMessage = userInfo.Id + ManagerMessage.ALERT_USER_OUT;
                    ShowErrorOrMessage(strMessage);
                }
            }
            catch (Exception)
            { }
        }


        // 서버로부터 UDP응답이 들어올때...

        public void OnVideoReceive(NotifyType type, IPAddress ipAddress, BaseInfo baseInfo)
        {
            try
            {
                switch (type)
                {
                    case NotifyType.Reply_VideoChat:
                        {
                            VideoInfo videoInfo = (VideoInfo)baseInfo;
                            queue.Enqueue(videoInfo);
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
                // 방에 입장한 사용자들의 정보를 유저리스트에 보여준다.

                if (roomDetailInfo.Users.Count < 1 || roomDetailInfo.Users == null)
                    return;

                this.userList.Items.Clear();
                for (int i = 0; i < roomDetailInfo.Users.Count; i++)
                {
                    if (roomDetailInfo.Users[i].Kind == 1)
                    {
                        m_userFemaleInfo = roomDetailInfo.Users[i];
                    }

                    this.userList.Items.Add(roomDetailInfo.Users[i].Id);
                }


                // Emoticon들을 얻어 Emoticon컨트롤에 배치하여준다.

                m_listEmoticons = new List<IconInfo>();
                m_listEmoticons = roomDetailInfo.Emoticons;

                InitLabelCaption(roomDetailInfo);
            }
            catch (Exception)
            { }
        }


        // 웹캠을 설정한다.

        private bool InitWPFMediaKit()
        {
            try
            {
                MyVideoCapElement.VideoCaptureDevice = WPFMediaKit.DirectShow.Controls.MultimediaUtil.VideoInputDevices[0]; // 웹캠을 플래이하기위한 장치 설정.
                return true;
            }
            catch (Exception)
            {
                string strMessage = ManagerMessage.ERROR_WEB_CAM;
                ShowErrorOrMessage(strMessage);
                return false;
            }
        }
        

        // 폼이 닫길때...

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FinalProcess();
        }


        // 폼클로즈시 마감처리를 진행한다.

        private void FinalProcess()
        {
            if (disapthcerTimer != null)
                disapthcerTimer.Stop();

            if (dispatcherVideoTimer != null)
                dispatcherVideoTimer.Stop();

            if (MyVideoCapElement.VideoCaptureDevice != null)
            {
                MyVideoCapElement.Stop();
                MyVideoCapElement.VideoCaptureDevice = null;
            }


            // VoiceChat Stop

            m_bVoicePlayerFlag = false;
            Stop();

            Main._ClientEngine.Send(NotifyType.Request_OutRoom, m_roomInfo);
        }


        // 캐쉬충전단추를 클릭하였을때...

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("control", "mmsys.cpl,,1");
        }


        // 폰트설정단추를 클릭하였을때...

        private void Button_Click(object sender, RoutedEventArgs e)
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
        }


        // 비디오전송단추를 클릭한후의 처리...

        private void SetVideoChat()
        {
            if (!m_videoSenBtnState)
            {
                if (InitWPFMediaKit())
                {
                    lblWebCam2.Content = Main._UserInfo.Id + LabelContents.WEB_CAM_LBL;
                    this.webCamImage.Visibility = Visibility.Hidden;
                    StartVideoCapture();
                    this.messageEditBox.Focus();

                    this.buttonSendMyVideo.Content = ButtonContent.VIDEO_END_BUTTON;
                    m_videoSenBtnState = true;
                }
            }
            else
            {
                lblWebCam2.Content = LabelContents.WEB_CAM_LBL;
                this.buttonSendMyVideo.Content = ButtonContent.VIDEO_SEND_BUTTON;
                webCamImage.Visibility = Visibility.Visible;

                if (disapthcerTimer != null)
                    disapthcerTimer.Stop();

                if (MyVideoCapElement.VideoCaptureDevice != null)
                {
                    MyVideoCapElement.Stop();
                    MyVideoCapElement.VideoCaptureDevice = null;
                }

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

        private void button6_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (!m_voiceSendBtnState)
                {
                    Start();
                    this.messageEditBox.Focus();

                    this.button6.Content = ButtonContent.VOICE_END_BUTTON;
                    m_voiceSendBtnState = true;
                }
                else
                {
                    Stop();
                    this.button6.Content = ButtonContent.VOICE_SEND_BUTTON;
                    m_voiceSendBtnState = false;
                }   
            }
            catch (Exception)
            {
                ShowErrorOrMessage(ManagerMessage.ERROR_MICROPHONE);
            }
        }


        // Emoticon단추를 클릭하였을때...

        private void Emoticon_Click(object sender, System.Windows.RoutedEventArgs e)
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
                bitMapImage.UriSource = new Uri(strUri, UriKind.RelativeOrAbsolute);
                bitMapImage.EndInit();

                img.Source = bitMapImage;
                img.Width = 25;
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
                int nNum = Convert.ToInt32(numberMark.Text);
                EvalHistoryInfo evalHistoryInfo = new EvalHistoryInfo();
                evalHistoryInfo.OwnId = Main._UserInfo.Id;
                evalHistoryInfo.Value = nNum;
                evalHistoryInfo.BuyerId = m_userFemaleInfo.Id;

                Main._ClientEngine.Send(NotifyType.Request_Evaluation, evalHistoryInfo);
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
                    numberMark.Opacity = 0.5;
                    m_strPreviousText = "";
                    this.btnSendMark.IsEnabled = false;
                }
                else
                {
                    int num = 0;
                    bool success = int.TryParse(((System.Windows.Controls.TextBox)sender).Text, out num);
                    if (success & num > 0)
                    {
                        ((System.Windows.Controls.TextBox)sender).Text.Trim();
                        m_strPreviousText = ((System.Windows.Controls.TextBox)sender).Text;
                    }
                    else
                    {
                        ((System.Windows.Controls.TextBox)sender).Text = m_strPreviousText;
                        ((System.Windows.Controls.TextBox)sender).SelectionStart = ((System.Windows.Controls.TextBox)sender).Text.Length;
                    }

                    numberMark.Opacity = 1;
                    this.btnSendMark.IsEnabled = true;
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
                                Run run = new Run("<" + img.Source.ToString() + ">");
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
                messageInfo.UserId      = Main._UserInfo.Id;
                messageInfo.String      = strMessage;
                messageInfo.FontSize    = nFontSize;
                messageInfo.FontName    = strFontName;
                messageInfo.FontStyle   = strFontStyle;
                messageInfo.FontWeight  = strFontWeight;
                messageInfo.FontColor   = strFontColor;

                Main._ClientEngine.Send(NotifyType.Request_StringChat, messageInfo);
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
            // 아이디를 현시하여준다.

            Paragraph para = new Paragraph();
            Run userID = new Run(strID + "(" + DateTime.Now.ToShortTimeString() + ") : " + "\r\n");

            if (strID.Equals(Main._UserInfo.Id))
                userID.Foreground = System.Windows.Media.Brushes.YellowGreen;
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
            disapthcerTimer.Interval = new TimeSpan(0, 0, 0, 0, 33);
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
            VisualBrush vb = new VisualBrush(MyVideoCapElement);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            using (drawingContext)
            {
                drawingContext.DrawRectangle(vb, null, new Rect(0, 0, 318, 238));
            }

            double dpiX = 200;
            double dpiY = 200;

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)(318 * dpiX / 96), (int)(238 * dpiY / 96), dpiX, dpiY, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.QualityLevel = 50;

            byte[] bit = new byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                bit = stream.ToArray();
                stream.Close();

                VideoInfo videoInfo = new VideoInfo();
                videoInfo.UserId = Main._UserInfo.Id;
                videoInfo.Data = bit;

                Main._VideoEngine.Send(NotifyType.Request_VideoChat, videoInfo);
            }
        }

        private void ReceiveBit(byte[] receiveData)
        {
            string errorMessage = null;

            try
            {
                MemoryStream ms = new MemoryStream(receiveData);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                receiveImage.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
        #endregion


        #region VoiceChat

        private void Start()
        {
            Stop();
            try
            {
                SoundUtils.SetVolumePercent(100);

                WaveFormat fmt = new WaveFormat(44100, 16, 2);
                if(!m_bVoicePlayerFlag)
                    m_Player = new WaveOutPlayer(-1, fmt, 3000, 3, new BufferFillEventHandler(Filler));
                m_Recorder = new WaveInRecorder(-1, fmt, 3000, 3, new BufferDoneEventHandler(Voice_Out));

                m_bVoicePlayerFlag = false;
            }
            catch
            {
                Stop();
                throw;
            }
        }

        private void Stop()
        {
            if (m_Player != null && m_bVoicePlayerFlag == false)
                try
                {
                    m_Player.Dispose();
                }
                finally
                {
                    m_Player = null;
                }
            if (m_Recorder != null)
                try
                {
                    m_Recorder.Dispose();
                }
                finally
                {
                    m_Recorder = null;
                }
            m_Fifo.Flush(); // clear all pending data
        }


        private void Filler(IntPtr data, int size)
        {
            if (m_PlayBuffer == null || m_PlayBuffer.Length < size)
                m_PlayBuffer = new byte[size];
            if (m_Fifo.Length >= size)
                m_Fifo.Read(m_PlayBuffer, 0, size);
            else
                for (int i = 0; i < m_PlayBuffer.Length; i++)
                    m_PlayBuffer[i] = 0;
            System.Runtime.InteropServices.Marshal.Copy(m_PlayBuffer, 0, data, size);
        }


        private void Voice_Out(IntPtr data, int size)
        {
            //for Recorder
            if (m_RecBuffer == null || m_RecBuffer.Length < size)
                m_RecBuffer = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(data, m_RecBuffer, 0, size);

            VoiceInfo voiceInfo = new VoiceInfo();
            voiceInfo.UserId = Main._UserInfo.Id;
            voiceInfo.Data = m_RecBuffer;

            Main._ClientEngine.Send(NotifyType.Request_VoiceChat, voiceInfo);         
        }

        private void ReceiveVoiceInfo(byte[] voiceData)
        {
            m_Fifo.Write(voiceData, 0, voiceData.Length);
        }


        #endregion
    }

}

  