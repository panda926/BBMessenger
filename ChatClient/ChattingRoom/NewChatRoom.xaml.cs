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

using System.ComponentModel;
using ChatEngine;

using ANYCHATAPI;
using System.IO;
using System.Threading;

using ControlExs;
using System.Runtime.InteropServices;

using System.Reflection;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Diagnostics;

using System.Windows.Media.Animation;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for NewChatRoom.xaml
    /// </summary>
    /// 

    public static class ManagerMessage
    {
        public const string ERROR_FONT_SETTING = "字体设置出现错误 请从新设置.";
        public const string ERROR_WEB_CAM = "您的电脑没有安装视频或视频设置出现错误 请检查视频链接.";
        public const string MESSAGE_WELCOME = "欢迎进入此房间. 祝您游戏愉快.";
        public const string ERROR_MICROPHONE = "您的电脑没有安装麦克风或麦克风设置出现错误 请检查麦克风链接";
        public const string ERROR_MANY_CHAT = "3000字以上的文字不能发送. 请修改发送的内容.";
        public const string ALERT_VOICE_MESSAGE = " 正在对你发送语音信息.";
        public const string ALERT_END_VOICE = "语音信息接收失败.";
        public const string NOT_ENOUGH_CASH = "金币余额不足，请冲值";
        public const string NOT_ENOUGH_POINT = "目前您的余额不足2分钟后将要退出此频道.";
        public const string ALERT_END_CHAT = "将要退出此频道. 如需继续聊天 请充值元宝.";
        public const string ALERT_USER_OUT = "0从此频道退出.";
        public const string ALERT_END_CHATTING = "因对方退出频道 结束聊天.";
        public const string ALERT_SERVICE_OUT = "因宝贝退出频道 结束聊天.";

        public const string ALERT_PRESENT_SENDING = "确定赠送？";
    }


    public static class FontInfo
    {
        public const string FONT_STYLE_ITALIC = "a";
        public const string FONT_STYLE_NORMAL = "b";
        public const string FONT_WEIGHT_BOLD = "c";
        public const string FONT_WEIGHT_NORMAL = "d";
    }

    public partial class NewChatRoom : BaseWindow
    {
        #region Varialble

        System.Windows.Threading.DispatcherTimer dispatcherOtherTimer = null;


        // Font정보보관변수들...

        string m_strFontName = "";
        float m_fFontSize = 14;
        System.Drawing.FontStyle m_fontStyle = new System.Drawing.FontStyle();
        System.Drawing.Color m_fontColor = new System.Drawing.Color();


        // 기타 성원변수들...

        //FlowDocument m_flowDoc = new FlowDocument();
        CustomFlowDoc m_flowDoc = new CustomFlowDoc();
        //ChattingRoom.VideoCalling videoCalling = new ChattingRoom.VideoCalling();

        List<IconInfo> m_listEmoticons = new List<IconInfo>();      // 서버로부터 보내여지는 Emoticon정보목록
        List<IconInfo> m_listPresents = new List<IconInfo>();       // 서버로부터 보내여지는 Present정보목록
        public RoomInfo m_roomInfo = new RoomInfo();                // 방정보

        RoomDetailInfo m_roomDetailInfo = new RoomDetailInfo();
        bool m_bRoomStateChange = false;
        bool m_bEndChatFlag = false;

        int m_getMoney = 0;

        List<String> m_MessageQue = new List<String>();

        public event EventHandler PresentSelected = delegate { };

        public string ID { get; set; }
        public string UUri { get; set; }

        private bool m_bShowPresent = false;

        public string m_strShakeWindow = "ShakeWindow+GIT";

        public string m_strVideoChatPass = "VideoChatStart+GIT";
        public string m_strVideoAccept = "VideoChatAccept+GIT";
        public string m_strVideoReject = "VideoChatReject+GIT";
        public string m_strVideoChatEnd = "VideoChatEnd+GIT";
        public string m_strVideoSendingStop = "VideoSendingStop+GIT";

        public string m_strVoiceChatPass = "VoiceChatStart+GIT";
        public string m_strVoiceChatEnd = "VoiceChatEnd+Git";

        public string m_strWritingStart = "WritingStart+GIT";
        public string m_strWritingEnd = "WritingEnd+GIT";

        public int _nTotalPresentCount = 0;

        #endregion

        // 2013-12-29: GreenRose
        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr handle, bool invert); 


        // 2014-02-11: GreenRose
        public string strOwnIP = string.Empty;
        public string strRemoteIP = string.Empty;

        public string strRemoteID = string.Empty;
        //public GG.VideoForm videoForm = null;
        public WomanVideoFormcs videoForm = null;

        public NewChatRoom()
        {
            InitializeComponent();
            
            Login._ClientEngine.AttachHandler(OnReceive);

            this.btnVideoChat.IsEnabled = true;            
            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {
                this.btnVideoChat.IsEnabled = false;
            }
            
            InitCameraControl();

            // 2014-06-06: GreenRose
            // prevent pasting to messageEditBox control
            DataObject.AddPastingHandler(messageEditBox, new DataObjectPastingEventHandler(OnPaste));
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        // 2014-04-04: GreenRose
        private void InitCameraControl()
        {
            //try
            //{
            //    List<CameraInformation> listCameraInfos = OMCS.Tools.Camera.GetCameras();
                
            //    for (int i = 0; i < listCameraInfos.Count; i++)
            //    {
            //        cmbCamera.Items.Add(listCameraInfos[i].Name);
            //    }

            //    if (listCameraInfos.Count == 0)
            //        cmbCamera.Items.Add("没有");

            //    if (cmbCamera.Items.Count > 0)
            //        cmbCamera.SelectedIndex = 0;
            //}
            //catch (Exception)
            //{ }
        }

        private void btnClose1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMin1_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Emoticon Button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (strRemoteID == string.Empty)
                return;

            UserInfo usRemoteInfo = Login._UserListInfo._Users.Find(item => item.Id == strRemoteID);
            if (usRemoteInfo.Kind == (int)UserKind.ServiceWoman && usRemoteInfo.nVIP == 0)
            {
                if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman && Login._UserInfo.nVIP != 1)
                {
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "只限vip客户使用权限", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);

                    return;
                }
            }

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

        // Font Button
        private void Button_Click_1(object sender, RoutedEventArgs e)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                InitFontInfo();

                if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                {
                    this.btnGive.IsEnabled = false;
                    this.txtPresentCount.IsEnabled = false;
                }

                InitLabelCaption(Login._UserInfo);
                m_getMoney = Login._UserInfo.Cash;

                this.messageEditBox.Focus();

                CheckOtherState();

                // 2013-06-17: GreenRose
                //PresentSelected += (s1, e1) => OnPresentSelected(((ChatRoom)s1).UUri, ((ChatRoom)s1).ID);

                //// 2013-12-26: GreenRose
                //Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                //{
                //    if (Main.loadWnd == null)
                //    {
                //        Main.loadWnd = new LoadingWnd();
                //        Main.loadWnd.Owner = this;
                //        Main.loadWnd.ShowDialog();
                //    }
                //}));

                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_RoomDetail, this.m_roomInfo);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void OnPresentSelected(string strUri, string strPoint)
        {
            if (Convert.ToInt32(strPoint) < Login._UserInfo.Cash)
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
                if (QQMessageBox.Show(tempWindowForm, ManagerMessage.ALERT_PRESENT_SENDING, "提示", QQMessageBoxIcon.Question, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                    return;

                InsertPresentToRichTextBox(strUri, strPoint);
            }
            else
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, ManagerMessage.ALERT_PRESENT_SENDING, "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
            }
        }

        private void InsertPresentToRichTextBox(string strUri, string strPoint)
        {
            try
            {
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
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        // 50ms간격으로 상태변화가 있는가를 검사한다.

        private void CheckOtherState()
        {
            dispatcherOtherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherOtherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherOtherTimer.Tick += new EventHandler(Each_State_Tick);
            dispatcherOtherTimer.Start();
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
                {
                    //ShowErrorOrMessage(m_MessageQue[i]);
                }

                m_MessageQue.RemoveRange(0, messageCount);

                StringInfo messageInfo = new StringInfo();
                messageInfo.UserId = Login._UserInfo.Id;
                messageInfo.FontSize = 12;
                messageInfo.FontName = "Arial";
                messageInfo.FontStyle = "";
                messageInfo.FontWeight = "";
                messageInfo.FontColor = "Black";
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        // 방입장시 정보현시.

        static int m_nCurCash = 0;

        //private void InitLabelCaption(RoomDetailInfo roomDetailInfo)
        private void InitLabelCaption(UserInfo userInfo)
        {
            try
            {

                if (userInfo.Cash != 0)
                {
                    if (userInfo.Cash <= m_roomInfo.Cash && userInfo.Kind == 0)
                    {
                        //ShowErrorOrMessage(ManagerMessage.NOT_ENOUGH_CASH);
                    }

                    //getMoney.Content = LabelContents.TOTAL_GET_CASH + (userInfo.Cash - m_getMoney).ToString();
                    //lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + userInfo.Cash.ToString();
                    //lblPricePerSecond.Content = LabelContents.PRICE_PER_SECOND + m_roomInfo.Cash.ToString();

                    m_nCurCash = userInfo.Cash;
                }
                else if (userInfo.Point != 0)
                {
                    if (userInfo.Point <= m_roomInfo.Point && userInfo.Kind == 0)
                    {
                        //ShowErrorOrMessage(ManagerMessage.NOT_ENOUGH_POINT);
                    }
                    //getMoney.Content = LabelContents.TOTAL_GET_POINT + (userInfo.Cash - m_getMoney).ToString();
                    //lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_POINT_LBL + userInfo.Point.ToString();
                    //lblPricePerSecond.Content = LabelContents.PRICE_PER_SECOND + m_roomInfo.Point.ToString();
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        // 서버로부터 응답이 들어올때...

        MediaElement alertMediaElement = null;
        bool isHeadGray = false;
        public void OnReceive(NotifyType type, Socket socket, BaseInfo baseInfo)
        {
            try
            {
                switch (type)
                {
                    case NotifyType.Reply_UserInfo:
                        {
                            //UserInfo userInfo = (UserInfo)baseInfo;                            
                        }
                        break;
                    case NotifyType.Reply_UserList:
                        {
                            if (strRemoteID != string.Empty)
                            {
                                UserInfo userInfo = Login._UserListInfo._Users.Find(item => item.Id == strRemoteID);
                                if (userInfo == null && !isHeadGray)
                                {
                                    try
                                    {
                                        FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                                        newFormatedBitmapSource.BeginInit();
                                        newFormatedBitmapSource.Source = (BitmapImage)imageHead.Source;
                                        newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray32Float;
                                        newFormatedBitmapSource.EndInit();
                                        imageHead.Source = newFormatedBitmapSource;

                                        isHeadGray = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorCollection.GetInstance().SetErrorInfo(ex.ToString());
                                    }
                                    //this.Close();
                                }
                                else
                                {
                                    SetRemoteUserInfo(userInfo);
                                }
                            }
                        }
                        break;
                    case NotifyType.Reply_RoomDetail:
                        {
                            RoomDetailInfo roomDetailInfo = (RoomDetailInfo)baseInfo;
                            if (roomDetailInfo.strRoomID != m_roomInfo.Id)
                            {
                                if (Main.loadWnd != null)
                                {
                                    Main.loadWnd.Close();
                                    Main.loadWnd = null;
                                }

                                break;
                            }

                            for (int i = 0; i < roomDetailInfo.Users.Count; i++)
                            {
                                if (roomDetailInfo.Users[i].Id == Login._UserInfo.Id)
                                {
                                    this.strOwnIP = roomDetailInfo.Users[i].strOwnIP;
                                    continue;
                                }

                                this.strRemoteID = roomDetailInfo.Users[i].Id;
                                this.strRemoteIP = roomDetailInfo.Users[i].strOwnIP;
                            }

                            if (roomDetailInfo.strRoomID == m_roomInfo.Id)
                            {
                                m_roomDetailInfo = roomDetailInfo;
                                m_bRoomStateChange = true;
                            }

                            if (Main.loadWnd != null)
                            {
                                Main.loadWnd.Close();
                                Main.loadWnd = null;
                            }
                        }
                        break;

                    case NotifyType.Reply_StringChat:
                        {
                            StringInfo strInfo = (StringInfo)baseInfo;
                            if (strInfo.UserId != Login._UserInfo.Id)
                            {
                                System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(this);
                                FlashWindow(helper.Handle, true);

                                AddMessageToHistoryTextBox(strInfo);
                            }
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

                            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman && m_roomInfo.Id == presentHistoryInfo.strRoomID)
                            {
                                m_nCurCash = m_nCurCash + presentHistoryInfo.Cash;
                                StartHeartAnimation();
                                //lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + m_nCurCash.ToString();
                                _nTotalPresentCount += presentHistoryInfo.Cash / 100;
                                this.txtPresentCount.Text = _nTotalPresentCount.ToString();
                                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_RoomDetail, this.m_roomInfo);
                            }

                            if (Login._UserInfo.Id == presentHistoryInfo.SendId)
                            {
                                m_nCurCash = m_nCurCash - presentHistoryInfo.Cash;
                                //lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + m_nCurCash.ToString();
                            }

                            
                        }
                        break;

                    case NotifyType.Reply_RoomInfo:
                        {
                            RoomInfo roomInfo = (RoomInfo)baseInfo;
                            if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
                            {
                                TempWindowForm tempWindowForm = new TempWindowForm();                                
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
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
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
                        //ShowErrorOrMessage(ManagerMessage.ALERT_END_CHATTING);
                        m_bEndChatFlag = true;
                    }

                    Login._UserInfo = userInfo;               
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
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

                // UserIcon Show 
                // 2014-01-27: GreenRose
                foreach (UserInfo userInfo in roomDetailInfo.Users)
                {
                    if (userInfo.Id == Login._UserInfo.Id)
                    {
                        continue;
                    }
                    else
                    {
                        imageHead.Source = ImageDownloader.GetInstance().GetImage(userInfo.Icon);
                        tbNickName.Text = userInfo.Nickname;
                        tbPersonal.Text = userInfo.Id;

                        txtCash.Text = userInfo.Cash.ToString();
                        txtPoint.Text = userInfo.Point.ToString();
                    }
                }


                // Emoticon들을 얻어 Emoticon컨트롤에 배치하여준다.

                m_listEmoticons = new List<IconInfo>();
                m_listEmoticons = roomDetailInfo.Emoticons;

                m_listPresents = new List<IconInfo>();
                m_listPresents = roomDetailInfo.Presents;

                if (!m_bShowPresent)
                {                    
                    m_bShowPresent = true;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        public void SetRemoteUserInfo(UserInfo userInfo)
        {
            try
            {
                //if (!Login._UserListInfo._Users.Contains(userInfo))
                //{
                //    FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                //    newFormatedBitmapSource.BeginInit();
                //    newFormatedBitmapSource.Source = ImageDownloader.GetInstance().GetImage(userInfo.Icon);
                //    newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray32Float;
                //    newFormatedBitmapSource.EndInit();
                //    imageHead.Source = newFormatedBitmapSource;
                //}
                //else
                {
                    imageHead.Source = ImageDownloader.GetInstance().GetImage(userInfo.Icon);
                }

                tbNickName.Text = userInfo.Nickname;
                tbPersonal.Text = userInfo.Id;

                txtCash.Text = userInfo.Cash.ToString();
                txtPoint.Text = userInfo.Point.ToString();

                isHeadGray = false;
            }
            catch(Exception)
            { }
        }

        // 폼이 닫길때...

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_bEndChatFlag == false)
            {
            }

            Main.chatRoom = null;

            Main.listChatRoom.Remove(this);
            FinalProcess();
        }

        // 폼클로즈시 마감처리를 진행한다.

        private void FinalProcess()
        {
            try
            {
                if (videoForm != null)
                    videoForm.Close();

                //if(Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                //    ChatClient.Login.multimediaManager.Dispose();


                if(extemview.Visibility == Visibility.Hidden)
                {
                    SendMessage(m_strVideoReject);
                }

                //AnyChatCoreSDK.LeaveRoom(-1);
                //AnyChatCoreSDK.Logout();
                //AnyChatCoreSDK.Release();

                if (dispatcherOtherTimer != null)
                    dispatcherOtherTimer.Stop();

                Thread.Sleep(1000);

                if (Convert.ToInt32(m_roomInfo.Id) < 1000)
                    Login._ClientEngine.Send(NotifyType.Request_OutMeeting, m_roomInfo);
                else
                    Login._ClientEngine.Send(NotifyType.Request_OutRoom, m_roomInfo);

                Main._nTotalBytesPerSecond = -1;

                Login._ClientEngine.DetachHandler(OnReceive);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        // 2014-03-30: GreenRose
        public bool IsRichTextBoxEmpty(RichTextBox rtb)
        {
            var start = rtb.Document.ContentStart;
            var end = rtb.Document.ContentEnd;
            int difference = start.GetOffsetToPosition(end);

            if (difference <= 5)
                return true;

            return false;
        }

        bool _bWriting = false;
        private void messageEditBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string strSendMessage = "";
                strSendMessage = InterpretMessage();
                //strSendMessage = GetStringFromRichTextBox();

                if(_bWriting == false)
                {
                    if (!IsRichTextBoxEmpty(messageEditBox))
                    {
                        _bWriting = true;
                        SendMessage(m_strWritingStart);
                    }
                }
                else
                {
                    if (IsRichTextBoxEmpty(messageEditBox))
                    {
                        _bWriting = false;
                        SendMessage(m_strWritingEnd);
                    }
                }

                if (strSendMessage.Length > 3000)
                {
                    ShowErrorOrMessage(ManagerMessage.ERROR_MANY_CHAT);
                    e.Handled = true;
                    return;
                }

                if (e.Key == Key.Enter)
                {
                    if (strRemoteID != string.Empty)
                    {
                        UserInfo usRemoteInfo = Login._UserListInfo._Users.Find(item => item.Id == strRemoteID);
                        if (usRemoteInfo.Kind == (int)UserKind.ServiceWoman && usRemoteInfo.nVIP == 0)
                        {
                            if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman && Login._UserInfo.nVIP != 1)
                            {
                                TempWindowForm tempWindowForm = new TempWindowForm();
                                QQMessageBox.Show(tempWindowForm, "只限vip客户使用权限", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);

                                TextRange textRange1 = new TextRange(messageEditBox.Document.ContentStart, messageEditBox.Document.ContentEnd);
                                textRange1.Text = "";
                                messageEditBox.ScrollToHome();
                                messageEditBox.CaretPosition = messageEditBox.Document.ContentStart;
                                e.Handled = true;

                                return;
                            }
                        }

                        if (strSendMessage != null && strSendMessage.Trim() != string.Empty)
                        {
                            _bWriting = false;
                            SendMessage(strSendMessage);
                            Thread.Sleep(300);
                            SendMessage(m_strWritingEnd);
                        }

                        TextRange textRange = new TextRange(messageEditBox.Document.ContentStart, messageEditBox.Document.ContentEnd);
                        textRange.Text = "";
                        messageEditBox.ScrollToHome();
                        messageEditBox.CaretPosition = messageEditBox.Document.ContentStart;

                        e.Handled = true;
                    }
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && e.Key == Key.Insert)
                {
                    e.Handled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        // 오류를 현시하여주는 함수

        public void ShowErrorOrMessage(string strErrorMessage)
        {
            Paragraph para = new Paragraph();
            //Run errorMessage = new Run(strErrorMessage);

            //errorMessage.FontSize = 14;
            //errorMessage.Foreground = Brushes.Red;
            
            Label lblState = new Label();
            lblState.Foreground = new SolidColorBrush(Colors.BlueViolet);
            lblState.FontWeight = FontWeights.Bold;
            lblState.Content = strErrorMessage;

            System.Windows.Media.Effects.DropShadowEffect effect = new System.Windows.Media.Effects.DropShadowEffect();
            effect.Color = Colors.Green;
            effect.ShadowDepth = 1;
            effect.Direction = 297;
            effect.BlurRadius = 7;

            lblState.Effect = effect;

            para.LineHeight = 1;
            para.Inlines.Add(lblState);

            m_flowDoc.Blocks.Add(para);

            this.messageHistoryBox.Document = m_flowDoc;
            this.messageHistoryBox.ScrollToEnd();
        }

        // 2014-02-28: GreenRose
        private void ShowVideoCalling()
        {
            Paragraph para = new Paragraph();

            ChattingRoom.VideoCalling videoCalling = new ChattingRoom.VideoCalling();
            videoCalling.AcceptButton_Click += new RoutedEventHandler(AcceptButton_Click);
            videoCalling.RejectButton_Click += new RoutedEventHandler(RejectButton_Click);
            
            para.LineHeight = 1;
            para.Inlines.Add(videoCalling);

            m_flowDoc.Blocks.Add(para);
            this.messageHistoryBox.Document = m_flowDoc;
            this.messageHistoryBox.ScrollToEnd();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            ShowVideoChatWnd();
            SendMessage(m_strVideoAccept);
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(m_strVideoReject);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (strRemoteID == string.Empty)
                return;

            UserInfo usRemoteInfo = Login._UserListInfo._Users.Find(item => item.Id == strRemoteID);

            if (usRemoteInfo.Kind == (int)UserKind.ServiceWoman && usRemoteInfo.nVIP == 0)
            {
                if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman && Login._UserInfo.nVIP != 1)
                {
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "只限vip客户使用权限", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);

                    TextRange textRange1 = new TextRange(messageEditBox.Document.ContentStart, messageEditBox.Document.ContentEnd);
                    textRange1.Text = "";
                    messageEditBox.ScrollToHome();
                    messageEditBox.CaretPosition = messageEditBox.Document.ContentStart;

                    this.messageEditBox.Focus();

                    return;
                }
            }

            string strSendMessage = InterpretMessage();
            if (strSendMessage != null && strSendMessage != string.Empty)
                SendMessage(strSendMessage);

            TextRange textRange = new TextRange(messageEditBox.Document.ContentStart, messageEditBox.Document.ContentEnd);
            textRange.Text = "";
            messageEditBox.ScrollToHome();
            messageEditBox.CaretPosition = messageEditBox.Document.ContentStart;

            this.messageEditBox.Focus();
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
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
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

                    string strIniFilePath = System.Windows.Forms.Application.StartupPath + "\\userinfo.ini";
                    IniFileEdit iniFileEdit = new IniFileEdit(strIniFilePath);

                    iniFileEdit.SetIniValue("FontInfo", "FontName", m_strFontName);
                    iniFileEdit.SetIniValue("FontInfo", "FontSize", m_fFontSize.ToString());
                    iniFileEdit.SetIniValue("FontInfo", "FontStyle", ((int)m_fontStyle).ToString());
                    iniFileEdit.SetIniValue("FontInfo", "FontColor", m_fontColor.Name);
                }
            }
        }
        #endregion

        private void InitFontInfo()
        {
            try
            {
                string strIniFilePath = System.Windows.Forms.Application.StartupPath + "\\userinfo.ini";
                IniFileEdit iniFileEdit = new IniFileEdit(strIniFilePath);

                FontFamilyConverter ffc = new FontFamilyConverter();

                string strVal = iniFileEdit.GetIniValue("FontInfo", "FontName");
                this.messageEditBox.FontFamily = (System.Windows.Media.FontFamily)ffc.ConvertFromString(strVal);

                strVal = iniFileEdit.GetIniValue("FontInfo", "FontSize");
                this.messageEditBox.FontSize = Convert.ToDouble(strVal) / 72 * 96;

                strVal = iniFileEdit.GetIniValue("FontInfo", "FontStyle");
                int nStyle = Convert.ToInt32(strVal);

                if (nStyle == (int)System.Drawing.FontStyle.Bold)
                {
                    this.messageEditBox.FontWeight = FontWeights.Bold;
                }
                else if (nStyle == (int)System.Drawing.FontStyle.Regular)
                {
                    this.messageEditBox.FontWeight = FontWeights.Normal;
                }
                else if (nStyle == (int)System.Drawing.FontStyle.Italic)
                {
                    this.messageEditBox.FontStyle = FontStyles.Italic;
                }

                strVal = iniFileEdit.GetIniValue("FontInfo", "FontColor");
                BrushConverter bc = new BrushConverter();
                this.messageEditBox.Foreground = (System.Windows.Media.Brush)bc.ConvertFromString(strVal);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

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


        // 2014-03-19: GreenRose
        private string GetStringFromRichTextBox()
        {
            string strText = string.Empty;

            TextRange tr = new TextRange(messageEditBox.Document.ContentStart, messageEditBox.Document.ContentEnd);
            MemoryStream ms = new MemoryStream();
            tr.Save(ms, DataFormats.Rtf);

            strText = ASCIIEncoding.Default.GetString(ms.ToArray());

            return strText;
        }

        // 2014-03-19: GreenRose
        public MemoryStream GetMemoryStreamFromString( string s ) 
        { 
            if ( s == null || s.Length == 0) 
                return null; 
            MemoryStream m = new MemoryStream();
            StreamWriter sw = new StreamWriter(m); 
            sw.Write(s); 
            sw.Flush();
            return m; 
        } 


        public void SendMessage(string strMessage)
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

                // 2013-12-25: GreenRose
                messageInfo.strRoomID = this.m_roomInfo.Id;

                AddMessageToHistoryTextBox(messageInfo);
                Login._ClientEngine.Send(NotifyType.Request_StringChat, messageInfo);
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }


        string _strRoomID = string.Empty;
        public void AddMessageToHistoryTextBox(StringInfo messageInfo)
        {
            if (messageInfo.strRoomID.Equals(this.m_roomInfo.Id))
            {
                _strRoomID = messageInfo.strRoomID;
                InterpretServerMessgae(messageInfo.UserId, messageInfo.String, messageInfo.FontSize, messageInfo.FontName, messageInfo.FontStyle, messageInfo.FontWeight, messageInfo.FontColor);
            }
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
                return;            

            // AlertSetting
            if (this.IsActive == false)
            {                
                alertMediaElement = new MediaElement();
                alertMediaElement.LoadedBehavior = MediaState.Manual;
                alertMediaElement.UnloadedBehavior = MediaState.Manual;
                alertMediaElement.Source = new Uri("sound/msg.WAV", UriKind.RelativeOrAbsolute);
                alertMediaElement.Volume = 100;
                alertMediaElement.Play();                
            }

            if (strID == Login._UserInfo.Id)
            {
                if (Login._UserInfo.Nickname == string.Empty)
                    strID = "未知昵称";
                else
                    strID = Login._UserInfo.Nickname;
            }
            else
            {
                UserInfo usRemoteInfo = Login._UserListInfo._Users.Find(item => item.Id == strID);
                if (usRemoteInfo != null)
                {
                    if (usRemoteInfo.Nickname == string.Empty)
                        strID = "未知昵称";
                    else
                        strID = usRemoteInfo.Nickname;
                }
            }

            // 아이디를 현시하여준다.

            Paragraph para = new Paragraph();
            Run userID = new Run(strID + "(" + DateTime.Now.ToShortTimeString() + ") : " + "\r\n");

            if (strID.Equals(Login._UserInfo.Nickname))
                userID.Foreground = System.Windows.Media.Brushes.Red;
            else
                userID.Foreground = System.Windows.Media.Brushes.Blue;

            userID.FontSize = 14;
            para.LineHeight = 8;
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
                        run.FontSize = fontSize;
                        run.FontStyle = fontStyle;
                        run.FontWeight = fontWeight;
                        run.Foreground = fontColor;
                        run.FontFamily = fontName;
                        
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

            //TextRange tr = new TextRange(this.messageHistoryBox.Document.ContentEnd, this.messageHistoryBox.Document.ContentEnd);
            //MemoryStream ms = GetMemoryStreamFromString(strReceiveMessage);
            //tr.Load(ms, DataFormats.Rtf);
        }

        #endregion

        private bool InterpretSignalText(string strSignal, string strID)
        {
            if (m_roomInfo.Id != _strRoomID)
                return false;

            string strUserName = Login._UserInfo.Id;

            if (strSignal.Equals(m_strShakeWindow))
            {
                if (!strUserName.Equals(strID))
                {
                    ShakeWindow();
                    m_MessageQue.Add("ShakeWindow");
                }

                return false;
            }

            if (strSignal.Equals(m_strVideoChatPass))
            {
                //if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman && !strUserName.Equals(strID))
                //{
                //    ShowVideoChatWnd();
                //}

                if (!strUserName.Equals(strID))
                {
                    if (this.IsLoaded == false)
                    {
                        Hardcodet.Wpf.TaskbarNotification.TaskbarIcon taskBarIcon1 = System.Windows.Application.Current.Properties[strID + "tag"] as Hardcodet.Wpf.TaskbarNotification.TaskbarIcon;
                        if (taskBarIcon1 != null)
                            taskBarIcon1.Dispose();

                        this.Show();
                    }

                    //ShowVideoChatWnd();

                    //ShowVideoCalling();
                    extemview.Visibility = Visibility.Hidden;
                    videoCalling.Visibility = Visibility.Visible;
                }

                return false;
            }

            if (strSignal.Equals(m_strVideoAccept))
            {
                if (!strUserName.Equals(strID))
                {
                    //ShowErrorOrMessage("视频聊天已连接");
                    VisibelExtemView();                    
                    ShowVideoChatWnd();
                }

                return false;
            }

            if (strSignal.Equals(m_strVideoReject))
            {
                if (!strUserName.Equals(strID))
                {
                    //ShowErrorOrMessage("对方拒绝接受视频请求");
                    VisibelExtemView();
                }

                return false;
            }

            if (strSignal.Equals(m_strVideoChatEnd))
            {
                if (!strUserName.Equals(strID))
                {
                    //ShowErrorOrMessage("视频聊天已关闭");
                    if (videoForm != null)
                    {
                        videoForm.Close();
                        videoForm = null;
                    }
                }

                return false;
            }

            if (strSignal.Equals(m_strWritingStart))
            { 
                if (!strUserName.Equals(strID))
                {
                    imageWriting.Visibility = Visibility.Visible;
                }

                return false;
            }

            if(strSignal.Equals(m_strWritingEnd))
            {
                if (!strUserName.Equals(strID))
                {
                    imageWriting.Visibility = Visibility.Hidden;
                }

                return false;
            }

            if(strSignal.Equals(m_strVideoSendingStop))
            {
                if(!strUserName.Equals(strID))
                {
                    if(videoForm != null)
                        videoForm.ChangeRemotePanelBackground();
                }

                return false;
            }

            return true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int nCash = 0;
            int nCount = 0;
            try
            {
                nCount = Convert.ToInt32(txtPresentCount.Text);
                if (nCount == 0)
                    return;

                nCash = 100 * nCount;
            }
            catch (Exception)
            { }

            if (Login._UserInfo.Cash > 100 * nCount)
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
                if (QQMessageBox.Show(tempWindowForm, ManagerMessage.ALERT_PRESENT_SENDING, "提示", QQMessageBoxIcon.Question, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                    return;

                PresentHistoryInfo presentHistoryInfo = new PresentHistoryInfo();
                presentHistoryInfo.Cash = nCash;
                presentHistoryInfo.strRoomID = m_roomInfo.Id;
                presentHistoryInfo.SendTime = DateTime.Now;

                for (int i = 0; i < m_roomDetailInfo.Users.Count; i++)
                {
                    if (m_roomDetailInfo.Users[i].Kind == (int)UserKind.ServiceWoman)
                    {
                        presentHistoryInfo.ReceiveId = m_roomDetailInfo.Users[i].Id;
                        break;
                    }
                }

                string strUri = "/Resources;component/image/rose2.png";
                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_Give, presentHistoryInfo);
                Login._ClientEngine.Send(ChatEngine.NotifyType.Request_RoomDetail, this.m_roomInfo);

                string strMessage = "<" + strUri + ">" + "礼物赠送成功(" + nCount + ")";
                SendMessage(strMessage);

                StartHeartAnimation();
            }
            else
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, ManagerMessage.NOT_ENOUGH_CASH, "提示", QQMessageBoxIcon.Warning, QQMessageBoxButtons.OK);
            }
        }

        private void StartHeartAnimation()
        {
            Storyboard opacityStoryBoard = (Storyboard)FindResource("flyRoseStory1");
            opacityStoryBoard.Begin(this);
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            Storyboard storyBoard = (Storyboard)FindResource("myStoryboard");
            storyBoard.Begin(this);

            Storyboard storyBoard1 = (Storyboard)FindResource("myStoryboard1");
            storyBoard1.Begin(this);
        }

        bool _bFirst = false;
        bool _bShake = true;
        System.Windows.Threading.DispatcherTimer timerShakeWnd = null;
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (strRemoteID == string.Empty)
                return;

            UserInfo usRemoteInfo = Login._UserListInfo._Users.Find(item => item.Id == strRemoteID);
            if (usRemoteInfo.Kind == (int)UserKind.ServiceWoman && usRemoteInfo.nVIP == 0)
            {
                if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman && Login._UserInfo.nVIP != 1)
                {
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "只限vip客户使用权限", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);

                    return;
                }
            }

            if (_bFirst == false)
            {
                _bFirst = true;

                timerShakeWnd = new System.Windows.Threading.DispatcherTimer();
                timerShakeWnd.Interval = TimeSpan.FromSeconds(10);
                timerShakeWnd.Tick += new EventHandler(Each_Shake_Tick);
                timerShakeWnd.Start();
            }

            if (_bShake)
            {
                SendMessage(m_strShakeWindow);
                ShakeWindow();
            }
        }

        private void Each_Shake_Tick(Object obj, EventArgs e)
        {
            _bShake = true;
        }

        private void ShakeWindow()
        {           
            // 2014-03-19: GreenRose
            Point pOld = new Point(this.Left, this.Top);
            int radius = 3;//半径
            for (int n = 0; n < 3; n++) //旋转圈数
            {
                //右半圆逆时针
                for (int i = -radius; i <= radius; i++)
                {
                    int x = Convert.ToInt32(Math.Sqrt(radius * radius - i * i));
                    int y = -i;
                    
                    this.Left = pOld.X + x;
                    this.Top = pOld.Y + y;
                    Thread.Sleep(10);
                }
                //左半圆逆时针
                for (int j = radius; j >= -radius; j--)
                {
                    int x = -Convert.ToInt32(Math.Sqrt(radius * radius - j * j));
                    int y = -j;

                    this.Left = pOld.X + x;
                    this.Top = pOld.Y + y;
                    Thread.Sleep(10);
                }
            }
            //抖动完成，恢复原来位置
            this.Left = pOld.X;
            this.Top = pOld.Y;
        }

        //public VideoChatWnd _videoChatWnd = null;
        public bool _bSendVideoChat = false;
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (strRemoteID == string.Empty)
                return;

            UserInfo usRemoteInfo = Login._UserListInfo._Users.Find(item => item.Id == strRemoteID);
            if (usRemoteInfo.Kind == (int)UserKind.ServiceWoman && usRemoteInfo.nVIP == 0)
            {
                if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman && Login._UserInfo.nVIP != 1)
                {
                    TempWindowForm tempWindowForm = new TempWindowForm();
                    QQMessageBox.Show(tempWindowForm, "只限vip客户使用权限", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);

                    return;
                }
            }

            if (extemview.Visibility == Visibility.Hidden)
                return;
            //if (_bSendVideoChat == true)
            //    return;
            //if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            //    ShowVideoChatWnd();
            //else
            {
                SendMessage(m_strVideoChatPass);
                //ShowErrorOrMessage("视频聊天接受中...");

                extemview.Visibility = Visibility.Hidden;
                accepRejectCall.Visibility = Visibility.Hidden;

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("pack://application:,,,/Resources;component/image/ChatRoomIcon/HDVideoHangs.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();

                imgCallingIcon.Source = bi;
                txtCalling.Text = "拒绝";

                videoCalling.Visibility = Visibility.Visible;
            }

            _bSendVideoChat = true;
        }

        private void VisibelExtemView()
        {
            extemview.Visibility = Visibility.Visible;
            accepRejectCall.Visibility = Visibility.Visible;

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("pack://application:,,,/Resources;component/image/ChatRoomIcon/AV_Accept.png", UriKind.RelativeOrAbsolute);
            bi.EndInit();

            imgCallingIcon.Source = bi;
            txtCalling.Text = "接受";

            videoCalling.Visibility = Visibility.Hidden;
        }

        private void ShowVideoChatWnd()
        {
            //if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman && CheckOpened("VideoForm"))            
            //    return;            

            if (videoForm == null)
            {                                
                //videoForm = new GG.VideoForm(Login._UserInfo.Id, strRemoteID, "", false, this);                                
                videoForm = new WomanVideoFormcs(Login._UserInfo.Id, strRemoteID, "", false, this);                                
                videoForm.Show();
            }
        }

        // 2014-03-29: GreenRose
        // Check videoForm is opened
        private bool CheckOpened(string strTag)
        {
            System.Windows.Forms.FormCollection fc = System.Windows.Forms.Application.OpenForms;

            foreach (System.Windows.Forms.Form frm in fc)
            {
                if (frm.Tag.ToString() == strTag)
                {
                    return true;
                }
            }
            return false;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            SendMessage(m_strVideoChatPass);
            ShowVideoChatWnd();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtPresentCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void acceptVideoCall_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (txtCalling.Text == "接受")
            {
                VisibelExtemView();

                // 2014-04-06: GReenRose
                if (ChatClient.Login._UserInfo.Kind == (int)ChatEngine.UserKind.ServiceWoman)
                {
                    ChatClient.Main main = ChatClient.Main._main;
                    if (main.soloVideoForm != null)
                    {
                        main.soloVideoForm.Close();
                        main.soloVideoForm = null;
                    }                    

                    Login._UserInfo.nUserState = 20;
                    Login._ClientEngine.Send(NotifyType.Request_UserState, Login._UserInfo);
                }

                ShowVideoChatWnd();
                SendMessage(m_strVideoAccept);
            }
            else if (txtCalling.Text == "拒绝")
            {
                VisibelExtemView();
                SendMessage(m_strVideoReject);
            }
        }

        private void accepRejectCall_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisibelExtemView();
            SendMessage(m_strVideoReject);
        }

        private void acceptVideoCall_MouseEnter(object sender, MouseEventArgs e)
        {
            ret1.Stroke = Brushes.Red;
        }

        private void acceptVideoCall_MouseLeave(object sender, MouseEventArgs e)
        {
            ret1.Stroke = Brushes.Gray;
        }

        private void accepRejectCall_MouseEnter(object sender, MouseEventArgs e)
        {
            ret2.Stroke = Brushes.Red;
        }

        private void accepRejectCall_MouseLeave(object sender, MouseEventArgs e)
        {
            ret2.Stroke = Brushes.Gray;
        }

        // 2014-04-04: GreenRose
        private void cmbCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbCamera.SelectedIndex >= 0)
            //{
            //    Login.multimediaManager.CameraDeviceIndex = cmbCamera.SelectedIndex;
            //    List<CameraCapability> listCameraCapability = OMCS.Tools.Camera.GetCameraCapability(cmbCamera.SelectedIndex);

            //    if (listCameraCapability.Count > 0)
            //    {
            //        //multimediaManager.CameraVideoSize = listCameraCapability[listCameraCapability.Count - 1].VideoSize;
            //        Login.multimediaManager.CameraVideoSize = listCameraCapability[listCameraCapability.Count / 2].VideoSize;
            //    }
            //}
        }
    }
}
