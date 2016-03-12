
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ChatEngine;
using Microsoft.Windows.Controls;
using System.IO;

using System.Windows.Media.Animation;
using System.ComponentModel;
using ControlExs;

// 2013-12-25: GreenRose
// AutoHideWindow;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using ChatClient.Properties;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>

    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 1.0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] is double)
                    result *= (double)values[i];
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }

    public partial class Main : BaseWindow
    {
        // 2014-05-01: GreenRose
        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int Init(StringBuilder pUserID, StringBuilder pServerAddress);

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int Destroy();


        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int GetCameraCount();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        extern public static IntPtr GetCameraName(int index);


        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StartVideoCapture(int index);

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StartAudioCapture();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StopVideoCapture();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StopAudioCapture();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int SetCaptureWindow(IntPtr hWnd);


        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StartSend();

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StopSend();


        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StartReceive(StringBuilder pUserID, IntPtr hWnd);

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int StopReceive(StringBuilder pUserID);

        [DllImport("VideoComm.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        extern public static IntPtr GetLastErrorString();

        #region Variable

        static public UserInfo selectUserInfo = null;        
        static public NewChatRoom chatRoom = null;                
        static public WritePaper writePaper = null;
        static public PreviewImage previewImage = null;
        static public SelectUserHome selectUserHome = null;
        static public SelectVideoView selectVideoView = null;
        static public VideoCreat videoCreat = null;

        //2014-04-05: GreenRose
        public static Main _main = null;
        
        // 2014-02-09: GreenRose
        static public LoadingWnd loadWnd = null;        

        static public WriteSend writeSend = null;
        static public int boardView = 0;
        static public int noticeView = 0;

        static public bool notice_board = true;
        
        static public Window activeGame = null;

        List<GameInfo> allGameList = null;
        public static GameRoom _GameRoom = null;
        public static DiceClient _DiceClient = null;

        public static MediaElement _MediaElement1 = new MediaElement();
        public static MediaElement _MediaElement2 = new MediaElement();
        public static MediaElement _MediaElement3 = new MediaElement();
        public static MediaElement _MediaElement4 = new MediaElement();
        public static MediaElement _MediaElement5 = new MediaElement();
        public static MediaElement _MediaElement6 = new MediaElement();
        public static MediaElement _MediaElement7 = new MediaElement();
        public static MediaElement _MediaElement8 = new MediaElement();
        public static MediaElement _MediaElement9 = new MediaElement();
        public static MediaElement _MediaElement10 = new MediaElement();
        public static MediaElement _MediaElement11 = new MediaElement();

        WomanUserControl womanUserControl = null;
        List<WomanUserControl> womanUserControlList = new List<WomanUserControl>();
        RoomUserControl roomUserControl = null;
        List<RoomUserControl> roomUserControlList = new List<RoomUserControl>();

        public static string u_gamrId = null;

        public static int selectIndex;                

        ChatEngine.IniFileEdit messageInfo = new ChatEngine.IniFileEdit(Login._UserPath);

        public static bool _bJoinedConference = false;

        private System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
        private System.ComponentModel.BackgroundWorker bw1 = new System.ComponentModel.BackgroundWorker();

        public static bool _bFirstRequestGameInfo = false;
        public static bool _bFirstRequestRoomInfo = false;
        public static bool _bFirstRequestPNFInfo = false;
        public static int _nTotalBytesPerSecond = -1;
        public static PNFWindowsView pnfWindowsView = null;
        public static PictureView pictureView = null;
        public static string pictureName = "";
        public static bool novelPlayStateFlag = false;

        public static List<String> m_listPreviewingImgFolder = new List<String>();

        public static bool m_bGameLoading = false;
        public static bool m_bGameRunning = false;               

        // 2013-12-25: GreenRose
        // Chat 신청.
        private System.Drawing.Icon blank = ChatClient.Properties.Resources.blank;
        private System.Drawing.Icon striped = ChatClient.Properties.Resources.striped;

        MediaElement alertMediaElement = null;

        // 2013-12-25: GreenRose
        // 여러명과 채팅을 하려고한다.        
        public static List<NewChatRoom> listChatRoom = new List<NewChatRoom>();

        // 2013-12-26: GreenRose
        // ShowMoneyBag
        MoneyBag wndMoneyBag = null;
        
        // 2014-01-20: GreenRose
        List<string> _listStyle = new List<string>();
        static int _nStyleNumber = 0;

        private DispatcherTimer _dtCheckGoAway = null;

        public static int _nCameraIndex = 0;

        #endregion

        public Main()
        {
            InitializeComponent();
            this.btnCamOrNotify.Click += new System.Windows.RoutedEventHandler(btnCmaOrNotify_Click);

            InitStyleList();
            InitMenuIcon();

            Login._ClientEngine.AttachHandler(NotifyOccured);
            InitDiceSoundSetting();

            this.menuImAway.Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click_StateChange);
            this.menuImOnline.Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click_StateChange);
            this.ImBusy.Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click_StateChange);
            this.ImOffline.Click += new System.Windows.RoutedEventHandler(this.MenuItem_Click_StateChange);


            // 2014-04-05: GreenRose
            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/Resources;component/image/video.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();

                imgCamOrNotify.Source = bi;
                btnCamOrNotify.ToolTip = "开启公麦";

                btnCamOrNotify.Visibility = Visibility.Hidden;
            }

            _main = this;

            if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
            { InitAllMainCustomerPan(); }
        }

        // 2014-06-10: GreenRose
        private StackPanel allCustomerPan = new StackPanel();
        private StackPanel stackPanelVip = new StackPanel();

        private Expander vipExpander = new Expander();
        private Expander customerExpander = new Expander();
        private void InitAllMainCustomerPan()
        {
            vipExpander = new Expander();
            vipExpander.Header = "在线客服";
            vipExpander.VerticalAlignment = VerticalAlignment.Top;
            vipExpander.OverridesDefaultStyle = true;
            vipExpander.FontSize = 12;
            vipExpander.Template = (ControlTemplate)FindResource("RevealExpanderTemp");
            vipExpander.FontFamily = new FontFamily("Microsoft YaHei");
            vipExpander.IsExpanded = true;

            Grid gridVip = new Grid();

            stackPanelVip = new StackPanel();

            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0, 0.5);
            gradientBrush.EndPoint = new Point(1, 0.5);
            
            gradientBrush.GradientStops.Add(new GradientStop((Color)System.Windows.Media.ColorConverter.ConvertFromString("#B2FFFFFF"), 0));
            gradientBrush.GradientStops.Add(new GradientStop(new Color(), 0));
            gradientBrush.GradientStops.Add(new GradientStop((Color)System.Windows.Media.ColorConverter.ConvertFromString("#55FFFFFF"), 0.257));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.493));

            stackPanelVip.Background = gradientBrush;

            gridVip.Children.Add(stackPanelVip);
            vipExpander.Content = gridVip;




            customerExpander = new Expander();
            customerExpander.Header = "在线宝贝";
            customerExpander.VerticalAlignment = VerticalAlignment.Top;
            customerExpander.OverridesDefaultStyle = true;
            customerExpander.FontSize = 12;
            customerExpander.Template = (ControlTemplate)FindResource("RevealExpanderTemp");
            customerExpander.FontFamily = new FontFamily("Microsoft YaHei");
            customerExpander.IsExpanded = true;

            Grid gridCustomer = new Grid();

            allCustomerPan = new StackPanel();

            LinearGradientBrush gradientBrush1 = new LinearGradientBrush();
            gradientBrush1.StartPoint = new Point(0, 0.5);
            gradientBrush1.EndPoint = new Point(1, 0.5);

            gradientBrush1.GradientStops.Add(new GradientStop((Color)System.Windows.Media.ColorConverter.ConvertFromString("#B2FFFFFF"), 0));
            gradientBrush1.GradientStops.Add(new GradientStop(new Color(), 0));
            gradientBrush1.GradientStops.Add(new GradientStop((Color)System.Windows.Media.ColorConverter.ConvertFromString("#55FFFFFF"), 0.257));
            gradientBrush1.GradientStops.Add(new GradientStop(Colors.Transparent, 0.493));

            allCustomerPan.Background = gradientBrush1;

            gridCustomer.Children.Add(allCustomerPan);
            customerExpander.Content = gridCustomer;


            allMainCustomerPan.Children.Add(vipExpander);
            allMainCustomerPan.Children.Add(customerExpander);
        }

        // 2014-04-05: GreenRose        
        private void btnCmaOrNotify_Click(object obj, RoutedEventArgs e)
        {
            try
            {
                if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                {
                    ShowWebCam();
                }
            }
            catch (Exception)
            { }
        }

        // 2014-04-14: GreenRose
        private void MenuItem_Click_StateChange(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem item = sender as System.Windows.Controls.MenuItem;
            ChangeLoginStateImage(item.Tag.ToString(), imgLoginState);
        }

        public void ChangeLoginStateImage(string imageTag, System.Windows.Controls.Image imgLoginState)
        {
            string imageUrl = "";
            string tag = "";
            switch (imageTag)
            {
                case "imonline":
                    {
                        imageUrl = "pack://application:,,,/Resources;component/Icons/imonline.ico";
                        tag = "1";
                        SendIMOnLine();
                    }
                    break;
                case "away":
                    {
                        imageUrl = "pack://application:,,,/Resources;component/Icons/away.ico";
                        tag = "3";
                        SendIMGoAway();
                    }
                    break;
                case "busy":
                    {
                        imageUrl = "pack://application:,,,/Resources;component/Icons/busy.ico";
                        tag = "2";
                        SendIMBusy();
                    }
                    break;
                case "offline":
                    {
                        imageUrl = "pack://application:,,,/Resources;component/Icons/imoffline.ico";
                        tag = "7";
                        SendIMOffline();
                    }
                    break;
                default:
                    break;
            }

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imageUrl, UriKind.RelativeOrAbsolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            ImageSource bi = bitmap;
            imgLoginState.Tag = tag;
            imgLoginState.Source = bi;            
        }

        // 2014-04-05: GreenRose
        // 캠화면을 보인다.
        //public GG.SoloVideoForm soloVideoForm = null;
        public ManVideoFormcs soloVideoForm = null;
        public void ShowWebCam()
        {
            try
            {
                if (soloVideoForm == null)
                {
                    System.Windows.Forms.FormCollection fc = System.Windows.Forms.Application.OpenForms;

                    foreach (System.Windows.Forms.Form frm in fc)
                    {
                        if (frm.Tag.ToString() == "P2pVideoChat")
                        {
                            return;
                        }
                    }

                    if (ChatClient.Login._UserInfo.nUserState != 0)
                    {
                        ChatClient.Login._UserInfo.nUserState = 0;
                        ChatClient.Login._ClientEngine.Send(ChatEngine.NotifyType.Request_UserState, ChatClient.Login._UserInfo);
                    }

                    //soloVideoForm = new GG.SoloVideoForm(Login._UserInfo.Id, string.Empty, string.Empty, false, this);
                    soloVideoForm = new ManVideoFormcs(Login._UserInfo.Id, string.Empty, string.Empty, false, this);
                    soloVideoForm.Show();
                }
                else
                {
                    soloVideoForm.Close();
                }
            }
            catch (Exception)
            { }
        }

        private void expander1_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void expander2_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void expander3_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            e.Handled = true;
        }

        // 2014-01-20: GreenRose
        private void InitStyleList()
        {
            string strStyle = string.Empty;

            strStyle = "/ChatClient;component/QQStyle/MainResourceDictionary.xaml";
            _listStyle.Add(strStyle);

            strStyle = "/ChatClient;component/QQStyle/TreeMainResourceDictionary.xaml";
            _listStyle.Add(strStyle);

            strStyle = "/ChatClient;component/QQStyle/GreenMainResourceDictionary.xaml";
            _listStyle.Add(strStyle);

            strStyle = "/ChatClient;component/QQStyle/BlueMainResourceDictionary.xaml";
            _listStyle.Add(strStyle);

            strStyle = "/ChatClient;component/QQStyle/RedMainResourceDictionary.xaml";
            _listStyle.Add(strStyle);
        }

        public class ShortCutModel
        {
            public bool IsVisible
            { get; set; }

            public string ImageUrl
            { get; set; }

            public string Description
            { get; set; }

            public string ToolTip
            { get; set; }

            public string number
            { get; set; }

            public string IsCollapse
            { get; set; }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Activate();
            
            MyNotifyIcon.ToolTipText = Login._UserInfo.Id;
            
            this._dtCheckGoAway = new DispatcherTimer();
            this._dtCheckGoAway.Interval = TimeSpan.FromMilliseconds(1000);
            this._dtCheckGoAway.Tick += new EventHandler(CheckGoAway);
            this._dtCheckGoAway.Start();

            ConnectMediaServer();
        }

        private void ConnectMediaServer()
        {
            StringBuilder strID = new StringBuilder(Login._UserInfo.Id);
            StringBuilder strServerIP = new StringBuilder(Login._ServerServiceUri);

            int nResult = Init(strID, strServerIP);
            if (nResult == 0)
            {
                IntPtr result = GetLastErrorString();
                string strError = Marshal.PtrToStringAnsi(result);

                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }

            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {
                this.btnCamOrNotify.Visibility = Visibility.Visible;
                ShowWebCam();

                //Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                //{
                //    this.btnCamOrNotify.Visibility = Visibility.Visible;
                //    ShowWebCam();
                //}));
            }
        }                

        int _nCheckGoAway = 0;
        bool _bIMOnline = true;
        private void CheckGoAway(object sender, EventArgs e)
        {
            if(Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.IsActive) == null)
            {
                if (_bIMOnline)
                {
                    if (_nCheckGoAway == 60 * 15)
                    {
                        if (Login._UserInfo.nUserState == 0)
                        {
                            SendIMGoAway();
                        }

                        _nCheckGoAway = 0;
                        _bIMOnline = false;
                    }
                    else
                        _nCheckGoAway++;
                }
            }
            else
            {
                if (_bIMOnline == false)
                {
                    if (Login._UserInfo.nUserState == 3)
                    {
                        SendIMOnLine();
                    }
                    _nCheckGoAway = 0;
                    _bIMOnline = true;
                }
            }
        }

        private void InitMenuIcon()
        {
            ShortCutModel mode = new ShortCutModel();
            mode.IsVisible = true;
            mode.ImageUrl = "pack://application:,,,/Resources;component/image/menuicon/purse18.png";
            mode.IsCollapse = "Visible";

            btnPayment.DataContext = mode;

            mode = new ShortCutModel();
            mode.IsVisible = true;
            mode.ImageUrl = "pack://application:,,,/Resources;component/image/menuicon/Tools.png";
            mode.IsCollapse = "Visible";

            btnMyInfo.DataContext = mode;

            mode = new ShortCutModel();
            mode.IsVisible = true;
            mode.ImageUrl = "pack://application:,,,/Resources;component/image/vip.gif";
            mode.IsCollapse = "Visible";

            btnSendMessage.DataContext = mode;

            mode = new ShortCutModel();
            mode.IsVisible = true;
            mode.ImageUrl = "pack://application:,,,/Resources;component/image/menuicon/mail.png";
            mode.IsCollapse = "Visible";

            btnReceiveMessage.DataContext = mode;

            mode = new ShortCutModel();
            mode.IsVisible = true;
            mode.ImageUrl = "pack://application:,,,/Resources;component/image/menuicon/colour.png";
            mode.IsCollapse = "Visible";

            btnStyleChange.DataContext = mode;
        }


        #region NotifyOccured
        private void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {                
                case NotifyType.Notify_Socket:
                    {
                        try
                        {
                            if (Main.loadWnd != null)
                            {
                                Main.loadWnd.Close();
                                Main.loadWnd = null;
                            }

                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "服务器连接失败", "消息", QQMessageBoxIcon.Warning, QQMessageBoxButtons.OK);
                        }
                        catch(Exception ex)
                        {
                            string strError = ex.ToString();
                            ErrorCollection.GetInstance().SetErrorInfo(strError);
                        }

                        ExitProgram();
                    }
                    break;

                // 2014-04-05: GreenRose
                // 같은 아이디로 로그인하였을경우 이미 로그인을 로그아웃시킨다.
                case NotifyType.Notify_Duplicate:
                    {
                        try
                        {
                            if (Main.loadWnd != null)
                            {
                                Main.loadWnd.Close();
                                Main.loadWnd = null;
                            }

                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "您的账号在另一地点登陆，您被迫下线", "消息", QQMessageBoxIcon.Warning, QQMessageBoxButtons.OK);
                        }
                        catch (Exception ex)
                        {
                            string strError = ex.ToString();
                            ErrorCollection.GetInstance().SetErrorInfo(strError);
                        }

                        ExitProgram();
                    }
                    break;
                
                case NotifyType.Reply_UserInfo:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        Login._UserInfo = (UserInfo)baseInfo;
                        this.txtCash.Text = Login._UserInfo.Cash.ToString();
                        this.txtPoint.Text = Login._UserInfo.Point.ToString();
                        this.txtRenminbi.Text = (Login._UserInfo.Cash / 100 + Login._UserInfo.Point / 10000).ToString();
                    }
                    break;
                case NotifyType.Reply_Home:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        Login._HomeInfo = (HomeInfo)baseInfo;
                        Login.boardList = Login._HomeInfo.Notices;
                        Login.noticeList = Login._HomeInfo.Letters;
                        displayBord();
                    }
                    break;
                case NotifyType.Reply_UserList:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        UserListInfo userListInfo = (UserListInfo)baseInfo;
                        ShowBallon(Login.userList, userListInfo._Users);


                        Login._UserListInfo = (UserListInfo)baseInfo;

                        if (Login.userList != null)
                            Login.userList.Clear();

                        if (Login._UserListInfo != null)
                        {
                            Login.userList = Login._UserListInfo._Users;
                            CustomerList(Login.userList, Login._UserInfo.Kind);
                        }
                    }
                    break;
                case NotifyType.Reply_UserState:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }
                        
                        UserInfo userInfo = (UserInfo)baseInfo;

                        ChangeUserState(userInfo, Login._UserInfo.Kind);
                    }
                    break;

                case NotifyType.Reply_GameList:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        Login._GameListInfo = (GameListInfo)baseInfo;
                        if (Login._GameListInfo != null)
                        {
                            allGameList = Login._GameListInfo.Games;
                            PlayGameList(allGameList);
                        }
                    }
                    break;

                case NotifyType.Reply_RoomList:
                    {
                        Login._RoomListInfo = (RoomListInfo)baseInfo;
                        if (Login._RoomListInfo != null)
                        {
                            Login.allRoomlist = Login._RoomListInfo.Rooms;
                            RoomInfoList(Login.allRoomlist);
                        }
                    }
                    break;

                case NotifyType.Reply_EnterGame:
                    {
                        m_bGameRunning = false;

                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        // 만일 사용자가 여자라면 비디오창을 닫는다.
                        if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                        {
                            if (soloVideoForm != null)
                                soloVideoForm.Close();
                        }

                        GameInfo enterGameInfo = (GameInfo)baseInfo;

                        if (enterGameInfo.Source == "Sicbo" ||
                            enterGameInfo.Source == "DzCard" ||
                            enterGameInfo.Source == "Horse" ||
                            enterGameInfo.Source == "BumperCar" ||
                            enterGameInfo.Source == "Fish")
                        {
                            _GameRoom = new GameRoom(enterGameInfo);

                            activeGame = _GameRoom;

                            // 2014-04-01: GreenRose
                            // 게임방에 들어온 상태를 알려준다.
                            if (enterGameInfo.Source == "Horse")
                                Login._UserInfo.nUserState = 11;
                            else if (enterGameInfo.Source == "BumperCar")
                                Login._UserInfo.nUserState = 12;

                            Login._ClientEngine.Send(NotifyType.Request_UserState, Login._UserInfo);

                            _GameRoom.Show();
                        }                        
                        else
                        {
                            _DiceClient = new DiceClient(enterGameInfo);

                            Login._ClientEngine.Send(NotifyType.Request_PlayerEnter, Login._UserInfo);  

                            activeGame = _DiceClient;

                            // 2014-04-01: GreenRose
                            // 게임방에 들어온 상태를 알려준다.
                            Login._UserInfo.nUserState = 10;
                            Login._ClientEngine.Send(NotifyType.Request_UserState, Login._UserInfo);

                            _DiceClient.Show();
                        }
                    }
                    break;
                
                case NotifyType.Reply_EnterMeeting:
                    {
                        Login._AskChatInfo = (AskChatInfo)baseInfo;

                        NewChatRoom tempChatRoom = null;
                        tempChatRoom = listChatRoom.Find(item => item.m_roomInfo.Id == Login._AskChatInfo.MeetingInfo.Id);

                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        if (!Login._UserInfo.Id.Equals(Login._AskChatInfo.TargetId))
                        {                            
                            if (tempChatRoom == null)
                            {
                                tempChatRoom = null;
                                tempChatRoom = new NewChatRoom();
                                tempChatRoom.m_roomInfo = Login._AskChatInfo.MeetingInfo;

                                UserInfo userInfo = GetUserInfoByID(Login._AskChatInfo.TargetId);
                                if(userInfo != null)
                                {
                                    tempChatRoom.SetRemoteUserInfo(userInfo);
                                }

                                listChatRoom.Add(tempChatRoom);
                                tempChatRoom.Show();
                                tempChatRoom.Activate();
                            }
                            else
                            {
                                if (tempChatRoom.IsLoaded)
                                    tempChatRoom.Activate();
                                else
                                {
                                    tempChatRoom.Show();
                                    tempChatRoom.Activate();
                                }
                            }

                            Hardcodet.Wpf.TaskbarNotification.TaskbarIcon taskBarIcon = System.Windows.Application.Current.Properties[Login._AskChatInfo.TargetId + "tag"] as Hardcodet.Wpf.TaskbarNotification.TaskbarIcon;
                            if (taskBarIcon != null)
                                taskBarIcon.Dispose();
                        }                       
                    }
                    break;      
                    // 2013-12-26: GreenRose
                    // 사용자가 메세지를 보낼때
                case NotifyType.Reply_StringChat:
                    {
                        ChatEngine.StringInfo stringInfo = (ChatEngine.StringInfo)baseInfo;

                        if (!InterpretSignalText(stringInfo.String, stringInfo.UserId))
                            return;

                        NewChatRoom tempChatRoom = null;
                        tempChatRoom = listChatRoom.Find(item => item.m_roomInfo.Id == stringInfo.strRoomID);

                        if (Login._UserInfo.Id.Equals(stringInfo.UserId))
                        {
                            if (tempChatRoom == null)
                            {
                                tempChatRoom = null;
                                tempChatRoom = new NewChatRoom();
                                tempChatRoom.m_roomInfo.Id = stringInfo.strRoomID;
                                tempChatRoom.AddMessageToHistoryTextBox(stringInfo);

                                listChatRoom.Add(tempChatRoom);
                                tempChatRoom.Show();
                                tempChatRoom.Activate();
                            }
                            else
                            {
                                tempChatRoom.Activate();
                            }
                        }
                        else
                        {
                            if (tempChatRoom == null)
                            {
                                tempChatRoom = null;                                
                                tempChatRoom = new NewChatRoom();
                                tempChatRoom.m_roomInfo.Id = stringInfo.strRoomID;

                                UserInfo userInfo = GetUserInfoByID(stringInfo.UserId);
                                if (userInfo != null)
                                    tempChatRoom.SetRemoteUserInfo(userInfo);

                                tempChatRoom.AddMessageToHistoryTextBox(stringInfo);


                                if (tempChatRoom.IsLoaded)
                                {                                    
                                    listChatRoom.Add(tempChatRoom);
                                    return;
                                }

                                if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
                                {
                                    foreach (UIElement child in allCustomerPan.Children)
                                    {
                                        WomanUserControl womanUserCtrl = child as WomanUserControl;
                                        if (womanUserCtrl != null)
                                        {
                                            if (womanUserCtrl._userId == stringInfo.UserId)
                                            {
                                                if (womanUserCtrl.sb == null)
                                                {
                                                    womanUserCtrl.InitStoryBoard();
                                                    womanUserCtrl.sb.Begin();
                                                }
                                            }
                                        }
                                    }
                                }

                                listChatRoom.Add(tempChatRoom);

                                System.Windows.Application.Current.Properties[stringInfo.UserId] = tempChatRoom;

                                // AlertSetting
                                alertMediaElement = new MediaElement();
                                alertMediaElement.LoadedBehavior = MediaState.Manual;
                                alertMediaElement.UnloadedBehavior = MediaState.Manual;
                                alertMediaElement.Source = new Uri("sound/msg.WAV", UriKind.RelativeOrAbsolute);
                                alertMediaElement.Volume = 100;
                                alertMediaElement.Play();

                                DispatcherTimer _BlinkNotifyTimer;
                                _BlinkNotifyTimer = new DispatcherTimer();
                                _BlinkNotifyTimer.Interval = TimeSpan.FromMilliseconds(500);

                                // notifyIcon setting
                                Hardcodet.Wpf.TaskbarNotification.TaskbarIcon taskBarIcon = new Hardcodet.Wpf.TaskbarNotification.TaskbarIcon();
                                taskBarIcon.Tag = stringInfo.UserId;

                                System.Windows.Application.Current.Properties[stringInfo.UserId + "tag"] = taskBarIcon; 

                                UserInfo askUserInfo = null;
                                askUserInfo = Login._UserListInfo._Users.Find(item => item.Id == stringInfo.UserId);

                                System.Drawing.Icon icon = null;
                                if (askUserInfo != null)
                                {
                                    BitmapImage bitmapImage = new BitmapImage();
                                    bitmapImage = ImageDownloader.GetInstance().GetImage(askUserInfo.Icon);
                                    System.Drawing.Bitmap bitmap = BitmapImage2Bitmap(bitmapImage);

                                    System.IntPtr iconHandle = bitmap.GetHicon();
                                    icon = System.Drawing.Icon.FromHandle(iconHandle);
                                }

                                bool blink = false;
                                taskBarIcon.TrayMouseDoubleClick += (sender, args) =>
                                {
                                    _BlinkNotifyTimer.Stop();
                                    //ChatRoom tempChatRoomTimer = System.Windows.Application.Current.Properties[taskBarIcon.Tag] as ChatRoom;

                                    foreach (UIElement child in allCustomerPan.Children)
                                    {
                                        if (Login._UserInfo.Kind != (int)UserKind.ServiceWoman)
                                        {
                                            WomanUserControl womanUserCtrl = child as WomanUserControl;
                                            if (womanUserCtrl != null)
                                            {
                                                if (womanUserCtrl._userId == taskBarIcon.Tag.ToString())
                                                {
                                                    if(womanUserCtrl.sb != null)
                                                        womanUserCtrl.sb.Stop();
                                                    womanUserCtrl.sb = null;
                                                }
                                            }
                                        }
                                    }

                                    if (tempChatRoom.IsLoaded)
                                    {
                                        tempChatRoom.Activate();
                                    }
                                    else
                                    {
                                        tempChatRoom.Show();
                                        tempChatRoom.Activate();
                                    }

                                    taskBarIcon.Dispose();
                                    alertMediaElement.Stop();
                                };

                                Samples.FancyBalloon balloon = new Samples.FancyBalloon();
                                balloon.BalloonText = stringInfo.UserId + "像您发送聊天";
                                taskBarIcon.ToolTipText = balloon.BalloonText;
                                taskBarIcon.ShowCustomBalloon(balloon, System.Windows.Controls.Primitives.PopupAnimation.Slide, 3000);


                                _BlinkNotifyTimer.Tick += (sender, args) =>
                                {
                                    if (!blink)
                                    {
                                        if (icon != null)
                                            taskBarIcon.Icon = icon;
                                        else
                                            taskBarIcon.Icon = striped;
                                    }
                                    else
                                    {
                                        taskBarIcon.Icon = blank;
                                    }
                                    blink = !blink;
                                };
                                _BlinkNotifyTimer.Start();
                            }
                        }
                    }
                    break;

                case NotifyType.Reply_Message:
                    {
                        //Login._StringInfo = (ChatEngine.StringInfo)baseInfo;

                        //string endMemo = messageInfo.GetIniValue("StringMessageBox", "EndMemo");
                        //if (endMemo != Login._StringInfo.String)
                        //{
                        //    if (mPage == null)
                        //    {
                        //        mPage = new MemoPage();
                        //        mPage.InsertString(Login._StringInfo);
                        //        mPage.Show();
                        //    }
                        //    else
                        //        mPage.InsertString(Login._StringInfo);

                        //}
                        //else
                        //{
                        //    if (mPage != null)
                        //    {
                        //        mPage.InsertString(Login._StringInfo);

                        //        var timer = new System.Windows.Threading.DispatcherTimer();
                        //        timer.Interval = TimeSpan.FromSeconds(2);
                        //        timer.Tick += delegate { mPage.Close(); timer.Stop(); };
                        //        timer.Start();
                                
                        //    }
                        //}
                        
                    }
                    break;

                case NotifyType.Reply_DelRoom:
                    {
                        Login._ClientEngine.Send(NotifyType.Request_RoomList, Login._UserInfo);
                    }
                    break;

                case NotifyType.Reply_UserDetail:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        Login._UserDetailInfo = (UserDetailInfo)baseInfo;
                        if (Login.m_nFlagMoneyBagBtnOrUserDetail == 0)          // 돈가방단추를 눌렀다면.
                        {
                            if (Login.myhome == null)
                            {
                                Login.myhome = new MyHome();
                                Login.myhome.InitTabSetting(Login._UserInfo);
                                Login.myhome.chargeHistoryList = Login._UserDetailInfo.ChargeHistories;
                                Login.myhome.chatHistoryList = Login._UserDetailInfo.ChatHistories;
                                Login.myhome.evalHistoryList = Login._UserDetailInfo.EvalHistories;
                                Login.myhome.iconInfoList = Login._UserDetailInfo.Faces;
                                Login.myhome.gameHistoryList = Login._UserDetailInfo.GameHistories;
                                Login.myhome.userInfoList = Login._UserDetailInfo.Partners;
                                Login.myhome.presentHistoryList = Login._UserDetailInfo.PresentHistories;
                                Login.myhome.roomInfoList = Login._UserDetailInfo.Rooms;
                            }

                            if (Login._UserInfo != null )
                            {
                                if (wndMoneyBag == null || !wndMoneyBag.IsLoaded)
                                {
                                    wndMoneyBag = new MoneyBag();
                                    wndMoneyBag.InitTabSetting(Login._UserInfo);
                                    wndMoneyBag.chatHistoryList = Login._UserDetailInfo.ChatHistories;

                                    wndMoneyBag.Show();
                                }
                                else
                                {
                                    if (wndMoneyBag.IsLoaded)
                                        wndMoneyBag.Activate();
                                }
                            }
                        }
                        else if (Login.m_nFlagMoneyBagBtnOrUserDetail == 1)     // 사용자정보보기단추를 눌렀다면.
                        {
                            if (Login.myhome == null)
                            {
                                Login.myhome = new MyHome();
                                Login.myhome.InitTabSetting(Login._UserInfo);
                                Login.myhome.chargeHistoryList = Login._UserDetailInfo.ChargeHistories;
                                Login.myhome.chatHistoryList = Login._UserDetailInfo.ChatHistories;
                                Login.myhome.evalHistoryList = Login._UserDetailInfo.EvalHistories;
                                Login.myhome.iconInfoList = Login._UserDetailInfo.Faces;
                                Login.myhome.gameHistoryList = Login._UserDetailInfo.GameHistories;
                                Login.myhome.userInfoList = Login._UserDetailInfo.Partners;
                                Login.myhome.presentHistoryList = Login._UserDetailInfo.PresentHistories;
                                Login.myhome.roomInfoList = Login._UserDetailInfo.Rooms;
                                Login.myhome.Show();
                            }
                            else
                            {
                                if (Login.myhome.IsLoaded)
                                    Login.myhome.Activate();
                                else
                                    Login.myhome.Show();
                            }
                        }
                        else if (Login.m_nFlagMoneyBagBtnOrUserDetail == 2)   // 아이디 생성단추를 클릭하였을때
                        {
                            if (Login.friendIDCreate == null)
                            {
                                Login.friendIDCreate = new FriendIDCreate();
                                Login.friendIDCreate.userInfoList = Login._UserDetailInfo.Partners;
                                Login.friendIDCreate._strDownloadUrl = Login._UserDetailInfo._strDownUrl;
                                Login.friendIDCreate.Show();
                            }
                            else
                            {
                                if (Login.friendIDCreate.IsLoaded)
                                    Login.friendIDCreate.Activate();
                                else
                                    Login.friendIDCreate.Show();
                            }
                        }
                    }
                    break;

                case NotifyType.Reply_PartnerDetail:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        Login.selectUserDetail = null;
                        Login.selectUserDetail = (UserDetailInfo)baseInfo;
                        if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
                        {
                            selectUserHome = new SelectUserHome();
                            selectUserHome.selUserDetail = Login.selectUserDetail;
                            selectUserHome.InitTabSetting(Login.selectInfo);
                            selectUserHome.Show();
                        }
                        else
                        {
                            Main.selectUserHome = new SelectUserHome();
                            Main.selectUserHome.selUserDetail = Login.selectUserDetail;
                            Main.selectUserHome.InitTabSetting(Main.selectUserInfo);
                            Main.selectUserHome.Show();
                        }
                    }
                    break;

                case NotifyType.Reply_LetterList:
                    {
                        Login._HomeInfo = (HomeInfo)baseInfo;
                        if (Login.noticeList != null)
                            Login.noticeList.Clear();
                        Login.noticeList = Login._HomeInfo.Letters;
                        displayBord();
                        if (writePaper != null && notice_board == false)
                            writePaper.noticeListView(Login.noticeList);
                        
                    }
                    break;

                case NotifyType.Reply_NoticeList:
                    {
                        Login._HomeInfo = (HomeInfo)baseInfo;
                        if (Login.boardList != null)
                                Login.boardList.Clear();
                            Login.boardList = Login._HomeInfo.Notices;
                        displayBord();
                        
                        if (writePaper != null && notice_board == true)
                            writePaper.noticeListView(Login.boardList);
                    }
                    break;

                case NotifyType.Reply_ChargeSiteUrl:
                    {
                        ChatEngine.StringInfo stringInfo = (ChatEngine.StringInfo)baseInfo;

                        string siteUrl = stringInfo.String;

                        if (string.IsNullOrEmpty(siteUrl))
                            return;

                        System.Diagnostics.Process.Start(siteUrl);
                    }
                    break;

                case NotifyType.Reply_ClassInfo:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        Login._ClassListInfo = (ClassListInfo)baseInfo;
                        NVPInfoList();
                    }
                    break;

                case NotifyType.Reply_ClassTypeInfo:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        Login._ClassTypeListInfo = (ClassTypeListInfo)baseInfo;
                        if (Login._ClassTypeListInfo.ClassType.Count == 0)
                        {                            
                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "没有找到您所查找的资料", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                        }
                        else
                        {
                            if (pnfWindowsView == null)
                            {
                                pnfWindowsView = new PNFWindowsView();
                                pnfWindowsView.Show();
                                pnfWindowsView.InitPNFView(Login._ClassTypeListInfo);
                            }
                            else
                            {
                                pnfWindowsView.InitPNFView(Login._ClassTypeListInfo);
                            }
                        }
                    }
                    break;

                case NotifyType.Reply_ClassPictureDetail:
                    {
                        Login._ClassPictureDetailInfo = (ClassPictureDetailInfo)baseInfo;

                        // 2013-12-13: GreenRose
                        // 사용자가 이미지보기에서 여러번 이미지보기를 클릭하였을때 그림보기창이 여러개가 펼쳐져야 한다.
                        if (Login._ClassPictureDetailInfo.ClassType == null)
                        {
                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "没有找到您所查找的资料", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                            break;
                        }
                        else if(Login._ClassPictureDetailInfo.ClassType.Count < 1)
                        {
                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "没有找到您所查找的资料", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                            break;
                        }

                        if (!m_listPreviewingImgFolder.Contains(Login._ClassPictureDetailInfo.ClassType[0].Class_Img_Folder))
                        {
                            PictureView frmPictureView = new PictureView();
                            frmPictureView.InitPictureView(Login._ClassPictureDetailInfo);
                            frmPictureView.Show();

                            m_listPreviewingImgFolder.Add(Login._ClassPictureDetailInfo.ClassType[0].Class_Img_Folder);
                        }
                    }
                    break;
            }
        }

        #endregion

        private System.Drawing.Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                // return bitmap; <-- leads to problems, stream is closed/closing ...
                return new System.Drawing.Bitmap(bitmap);
            }
        }

        private void InitDiceSoundSetting()
        {

            _MediaElement1.Source = new Uri("sound/GAME_START.WAV", UriKind.RelativeOrAbsolute);
            _MediaElement2.Source = new Uri("sound/TIME_WARIMG.wav", UriKind.RelativeOrAbsolute);
            _MediaElement3.Source = new Uri("sound/GAME_END.WAV", UriKind.RelativeOrAbsolute);
            _MediaElement4.Source = new Uri("sound/Bet.WAV", UriKind.RelativeOrAbsolute);
            _MediaElement5.Source = new Uri("sound/Alert.WAV", UriKind.RelativeOrAbsolute);
            _MediaElement6.Source = new Uri("sound/CHEER2.WAV", UriKind.RelativeOrAbsolute);
            _MediaElement7.Source = new Uri("sound/IDC_SND_GAMELOST.wav", UriKind.RelativeOrAbsolute);
            _MediaElement8.Source = new Uri("sound/paycol.WAV", UriKind.RelativeOrAbsolute);
            _MediaElement9.Source = new Uri("sound/MOST_CARD.WAV", UriKind.RelativeOrAbsolute);
            _MediaElement10.Source = new Uri("sound/prop.wav", UriKind.RelativeOrAbsolute);
            _MediaElement11.Source = new Uri("sound/BACK_GROUND.wav", UriKind.RelativeOrAbsolute);
        }

        private void displayBord()
        {
            int noView = 0;

            if (Login.boardList != null)
            {
                for (int i = 0; i < Login.boardList.Count; i++)
                {
                    if (Login.boardList[i].Readed == 0)
                        ++noView;
                }

                if (noView > 0)
                {
                    ShortCutModel mode = new ShortCutModel();
                    mode.IsVisible = true;
                    mode.ImageUrl = "pack://application:,,,/Resources;component/image/menuicon/mail.png";
                    mode.number = noView.ToString();

                    btnReceiveMessage.DataContext = mode;
                }
            }
        }

        // 2014-02-03: GreenRose
        private void ChangeUserState(UserInfo userInfo, int nKind)
        {
            try
            {
                for (int i = 0; i < Login._UserListInfo._Users.Count; i++)
                {
                    if (Login._UserListInfo._Users[i].Id == userInfo.Id)
                    {
                        Login._UserListInfo._Users[i].nUserState = userInfo.nUserState;
                        break;
                    }
                }

                if (nKind != (int)UserKind.ServiceWoman)
                {
                    foreach (UIElement child in allCustomerPan.Children)
                    {
                        WomanUserControl womanUserCtrl = child as WomanUserControl;
                        if (womanUserCtrl._userId == userInfo.Id)
                        {
                            BitmapImage bi = new BitmapImage();
                            switch (userInfo.nUserState)
                            {
                                case 0:
                                case 13:
                                    {
                                        bi.BeginInit();
                                        bi.UriSource = new Uri("/Resources;component/Icons/imonline.ico", UriKind.RelativeOrAbsolute);
                                        bi.EndInit();

                                        womanUserCtrl.imgUserState.Source = bi;
                                    }
                                    break;

                                case 1:
                                    {
                                        bi.BeginInit();
                                        bi.UriSource = new Uri("/Resources;component/Icons/imoffline.ico", UriKind.RelativeOrAbsolute);
                                        bi.EndInit();

                                        womanUserCtrl.imgUserState.Source = bi;
                                    }
                                    break;

                                case 2:
                                    {
                                        bi.BeginInit();
                                        bi.UriSource = new Uri("/Resources;component/Icons/busy.ico", UriKind.RelativeOrAbsolute);
                                        bi.EndInit();

                                        womanUserCtrl.imgUserState.Source = bi;
                                    }
                                    break;

                                case 3:
                                    {
                                        bi.BeginInit();
                                        bi.UriSource = new Uri("/Resources;component/Icons/away.ico", UriKind.RelativeOrAbsolute);
                                        bi.EndInit();

                                        womanUserCtrl.imgUserState.Source = bi;
                                    }
                                    break;

                                case 10:
                                    {
                                        bi.BeginInit();
                                        bi.UriSource = new Uri("/Resources;component/Icons/DiceClient.ico", UriKind.RelativeOrAbsolute);
                                        bi.EndInit();

                                        womanUserCtrl.imgUserState.Source = bi;
                                    }
                                    break;

                                case 11:
                                    {
                                        bi.BeginInit();
                                        bi.UriSource = new Uri("/Resources;component/Icons/HorseClient.ico", UriKind.RelativeOrAbsolute);
                                        bi.EndInit();

                                        womanUserCtrl.imgUserState.Source = bi;


                                    }
                                    break;

                                case 12:
                                    {
                                        bi.BeginInit();
                                        bi.UriSource = new Uri("/Resources;component/Icons/BumperCarClient.ico", UriKind.RelativeOrAbsolute);
                                        bi.EndInit();

                                        womanUserCtrl.imgUserState.Source = bi;
                                    }
                                    break;

                                case 20:
                                    {
                                        bi.BeginInit();
                                        bi.UriSource = new Uri("/Resources;component/image/video.png", UriKind.RelativeOrAbsolute);
                                        bi.EndInit();

                                        womanUserCtrl.imgUserState.Source = bi;

                                        CheckOpened(userInfo.Id);
                                    }
                                    break;

                                case 21:
                                    {
                                        womanUserCtrl.btnCam.Width = 17;
                                        womanUserCtrl.btnCam.IsEnabled = true;
                                    }
                                    break;

                                case 22:
                                    {
                                        womanUserCtrl.btnCam.Width = 0;
                                        womanUserCtrl.btnCam.IsEnabled = false;

                                        CheckOpened(userInfo.Id);
                                    }
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < myDataGrid.Items.Count; i++)
                    {
                        if (((MyData)myDataGrid.Items[i]).id == userInfo.Id)
                        {
                            switch (userInfo.nUserState)
                            {
                                case 0:
                                case 13:
                                    {
                                        ((MyData)myDataGrid.Items[i]).state = "/Resources;component/Icons/imonline.ico";
                                    }
                                    break;

                                case 1:
                                    {
                                        ((MyData)myDataGrid.Items[i]).state = "/Resources;component/Icons/imoffline.ico";
                                    }
                                    break;

                                case 2:
                                    {
                                        ((MyData)myDataGrid.Items[i]).state = "/Resources;component/Icons/busy.ico";
                                    }
                                    break;

                                case 3:
                                    {
                                        ((MyData)myDataGrid.Items[i]).state = "/Resources;component/Icons/away.ico";
                                    }
                                    break;

                                case 10:
                                    {
                                        ((MyData)myDataGrid.Items[i]).state = "/Resources;component/Icons/DiceClient.ico";
                                    }
                                    break;

                                case 11:
                                    {
                                        ((MyData)myDataGrid.Items[i]).state = "/Resources;component/Icons/HorseClient.ico";
                                    }
                                    break;

                                case 12:
                                    {
                                        ((MyData)myDataGrid.Items[i]).state = "/Resources;component/Icons/BumperCarClient.ico";
                                    }
                                    break;
                            }

                            break;
                        }
                    }
                }
            }
            catch(Exception)
            {            
            }
        }

        // 2014-03-29: GreenRose
        // Check videoForm is opened
        private bool CheckOpened(string strText)
        {
            System.Windows.Forms.FormCollection fc = System.Windows.Forms.Application.OpenForms;

            foreach (System.Windows.Forms.Form frm in fc)
            {
                if (frm.Text.ToString() == strText)
                {
                    frm.Close(); // 폼을 클로즈한다.      
                    return true;
                }
            }

            return true;
        }

        //모든고객정보를 현시한다.
        #region CustomerList

        public Microsoft.Windows.Controls.DataGrid myDataGrid = null;

        private void CustomerList(List<UserInfo> userList, int kind)
        {            
            if (kind != (int)UserKind.ServiceWoman)
            {
                allCustomerPan.Children.Clear();
                stackPanelVip.Children.Clear();
                womanUserControlList.Clear();

                //TreeView treeView = new TreeView();
                //treeView.Background = Brushes.Transparent;
                //TreeViewItem treeViewItem1 = new TreeViewItem();
                //treeViewItem1.Header = "VipMaker";
                //WomanUserControl vipUserControl = new WomanUserControl();
                //vipUserControl.Margin = new Thickness(2);
                //if (userList.Count != 0)
                //    vipUserControl.UserListGrid(userList[0]);

                //TreeViewItem treeViewItem11 = new TreeViewItem();
                //treeViewItem11.Header = vipUserControl;

                //treeViewItem1.Items.Add(treeViewItem11);
                //treeView.Items.Add(treeViewItem1);

                //allCustomerPan.Children.Add(treeView);
                
                for (var i = 0; i < userList.Count; i++)
                {
                    womanUserControl = new WomanUserControl();
                    womanUserControl.Margin = new Thickness(2);
                    womanUserControlList.Add(womanUserControl);
                    womanUserControl.UserListGrid(userList[i]);

                    if(userList[i].nVIP == 2)
                    {
                        stackPanelVip.Children.Add(womanUserControl);
                    }
                    else
                        allCustomerPan.Children.Add(womanUserControl);

                    ChangeUserState(userList[i], kind);
                }

                if (stackPanelVip.Children.Count < 1)
                    vipExpander.IsExpanded = false;
                else
                    vipExpander.IsExpanded = true;

                if (allCustomerPan.Children.Count < 1)
                    customerExpander.IsExpanded = false;
                else
                    customerExpander.IsExpanded = true;
            }
            else
            {
                allMainCustomerPan.Children.Clear();
                myDataGrid = new Microsoft.Windows.Controls.DataGrid();

                myDataGrid.HeadersVisibility = Microsoft.Windows.Controls.DataGridHeadersVisibility.Column;                
                myDataGrid.RowHeaderWidth = 0;

                myDataGrid.Style = (Style)FindResource("DataGridStyle");
                myDataGrid.CellStyle = (Style)FindResource("cellStyle");
                myDataGrid.RowStyle = (Style)FindResource("rowStyle");                
                myDataGrid.ColumnHeaderStyle = (Style)FindResource("columnHeaderStyle");
                myDataGrid.Background = new SolidColorBrush(Colors.Transparent);
                myDataGrid.CommitEdit(Microsoft.Windows.Controls.DataGridEditingUnit.Cell, true);

                myDataGrid.Height = 450;
                myDataGrid.IsReadOnly = true;

                myDataGrid.SelectionMode = Microsoft.Windows.Controls.DataGridSelectionMode.Extended;
                myDataGrid.SelectionUnit = Microsoft.Windows.Controls.DataGridSelectionUnit.FullRow;

                myDataGrid.MouseRightButtonDown += new MouseButtonEventHandler(myDataGrid_MouseRightButtonUp);
                myDataGrid.LoadingRow += dataGrid_LoadingRow;

                myDataGrid.MouseDoubleClick += new MouseButtonEventHandler(myDataGrid_MouseDoubleClick);

                Microsoft.Windows.Controls.DataGridTemplateColumn col5 = new Microsoft.Windows.Controls.DataGridTemplateColumn();
                col5.Header = "  ";

                FrameworkElementFactory factory5 = new FrameworkElementFactory(typeof(Image));
                Binding bmp = new Binding("icon");                
                bmp.Mode = BindingMode.TwoWay;                
                factory5.SetValue(Image.SourceProperty, bmp);

                DataTemplate cellTemplate5 = new DataTemplate();
                cellTemplate5.VisualTree = factory5;

                col5.CellTemplate = cellTemplate5;
                col5.Width = Microsoft.Windows.Controls.DataGridLength.Auto;
                col5.Width = 12;
                myDataGrid.Columns.Add(col5);

                Microsoft.Windows.Controls.DataGridTemplateColumn colState = new Microsoft.Windows.Controls.DataGridTemplateColumn();
                colState.Header = "  ";

                FrameworkElementFactory factoryState = new FrameworkElementFactory(typeof(Image));
                Binding bmpState = new Binding("state");
                bmpState.Mode = BindingMode.TwoWay;
                factoryState.SetValue(Image.SourceProperty, bmpState);                

                DataTemplate cellTemplateState = new DataTemplate();
                cellTemplateState.VisualTree = factoryState;

                colState.CellTemplate = cellTemplateState;                
                colState.Width = 6;
                colState.Width = 6;
                myDataGrid.Columns.Add(colState);

                Microsoft.Windows.Controls.DataGridTextColumn col4 = new Microsoft.Windows.Controls.DataGridTextColumn();
                col4.Header = "帐号";
                col4.Width = 50;

                col4.Binding = new Binding("id");                
                myDataGrid.Columns.Add(col4);

                Microsoft.Windows.Controls.DataGridTextColumn col1 = new Microsoft.Windows.Controls.DataGridTextColumn();
                col1.Header = "称号";
                
                col1.Width = Microsoft.Windows.Controls.DataGridLength.Auto;
                col1.MinWidth = 110;
                col1.Binding = new Binding("nicName");
                myDataGrid.Columns.Add(col1);

                Microsoft.Windows.Controls.DataGridTextColumn col2 = new Microsoft.Windows.Controls.DataGridTextColumn();
                col2.Header = "金币";
                col2.Width = Microsoft.Windows.Controls.DataGridLength.Auto;
                col2.MinWidth = 50;
                col2.Binding = new Binding("cash");
                myDataGrid.Columns.Add(col2);

                Microsoft.Windows.Controls.DataGridTemplateColumn col3 = new Microsoft.Windows.Controls.DataGridTemplateColumn();
                col3.Header = "级别";

                FrameworkElementFactory factory3 = new FrameworkElementFactory(typeof(Image));
                Binding b3 = new Binding("level");
                b3.Mode = BindingMode.TwoWay;
                factory3.SetValue(Image.SourceProperty, b3);

                DataTemplate cellTemplate3 = new DataTemplate();
                cellTemplate3.VisualTree = factory3;

                col3.CellTemplate = cellTemplate3;                           
                col3.Width = 60;
                col3.MinWidth = 60;                                
                myDataGrid.Columns.Add(col3);


                myDataGrid.Items.Clear();
                if (userList != null)
                {
                    for (int j = 0; j < userList.Count; j++)
                    {                        
                        if (userList[j].Icon == "Images\\Face\\1.gif" || userList[j].Icon == string.Empty)
                        {
                            userList[j].Icon = "image/face/DefaultHeadImage.png";
                        }

                        string strImgPath = string.Empty;

                        if (userList[j].nVIP == 0)
                        {
                            int nLevel = 0;

                            if (userList[j].Cash > 100 && userList[j].Cash <= 500)
                                nLevel = 1;
                            else if (userList[j].Cash > 500 && userList[j].Cash <= 1000)
                                nLevel = 2;
                            else if (userList[j].Cash > 1000 && userList[j].Cash <= 5000)
                                nLevel = 3;
                            else if (userList[j].Cash > 5000 && userList[j].Cash <= 10000)
                                nLevel = 4;
                            else if (userList[j].Cash > 10000)
                                nLevel = 5;
                            else
                                nLevel = 0;

                            strImgPath = string.Format("/Resources;Component/image/star{0}.png", nLevel);
                        }
                        else
                            strImgPath = "/Resources;Component/image/new_vip.png";


                        myDataGrid.Items.Add(new MyData { icon = string.Format("/Resources;Component/{0}", userList[j].Icon), state=string.Empty, id = userList[j].Id, nicName = userList[j].Nickname, cash = userList[j].Cash, point = userList[j].Point, level = strImgPath });
                        ChangeUserState(userList[j], Login._UserInfo.Kind);
                    }
                }

                allMainCustomerPan.Children.Add(myDataGrid);
            }
        }

        void dataGrid_LoadingRow(object sender, Microsoft.Windows.Controls.DataGridRowEventArgs e)
        {
            // Get the DataRow corresponding to the Microsoft.Windows.Controls.DataGridRow that is loading.
            MyData item = e.Row.Item as MyData;
            if (item != null)
            {
                if( item.level != "" )
                    e.Row.Foreground = new SolidColorBrush(Colors.Black);
            }
        }


        #endregion


        public class MyData : INotifyPropertyChanged 
        {
            private string _icon;
            public string icon
            {
                get { return _icon;}
                set
                {
                    if(value != this._icon)
                    {
                        this._icon = value;
                        NotifyPropertyChanged("icon");
                    }
                }
            }

            private string _state;
            public string state
            {
                get { return _state;}
                set
                {
                    if(value != this._state)
                    {
                        this._state = value;
                        NotifyPropertyChanged("state");
                    }
                }
            }

            private string _nickname;
            public string nicName
            {
                get { return _nickname; }
                set
                {
                    if(value != _nickname)
                    {
                        this._nickname = value;
                        NotifyPropertyChanged("nicName");
                    }
                }
            }

            private int _cash;
            public int cash
            {
                get { return _cash; }
                set
                {
                    if(value != this._cash)
                    {
                        this._cash = value;
                        NotifyPropertyChanged("cash");
                    }
                }
            }

            private int _point;
            public int point
            {
                get { return _point; }
                set
                {
                    if(value != _point)
                    {
                        this._point = value;
                        NotifyPropertyChanged("point");
                    }
                }
            }

            private string _id;
            public string id
            {
                get { return _id;}
                set
                {
                    if(value != this._id)
                    {
                        this._id = value;
                        NotifyPropertyChanged("id");
                    }
                }
            }

            private string _level;
            public string level
            {
                get { return _level; }
                set
                {
                    if(value != this._level)
                    {
                        this._level = value;
                        NotifyPropertyChanged("level");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged; 
            private void NotifyPropertyChanged(string propertyName) 
            { 
                if (PropertyChanged != null) 
                { 
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); 
                } 
            }
        }


        void myDataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Microsoft.Windows.Controls.DataGrid dataGrid = sender as Microsoft.Windows.Controls.DataGrid;
                Microsoft.Windows.Controls.DataGridRow row = (Microsoft.Windows.Controls.DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                Microsoft.Windows.Controls.DataGridCell RowColumn = dataGrid.Columns[2].GetCellContent(row).Parent as Microsoft.Windows.Controls.DataGridCell;                

                string userId = ((TextBlock)RowColumn.Content).Text;                
                ContextMenu contextMenu = new ContextMenu();

                for (int i = 0; i < Login.userList.Count; i++)
                {
                    if (userId == Login.userList[i].Id)
                    {
                        MenuItem selectItem3 = new MenuItem();
                        selectItem3.Header = "查看个人资料";
                        selectItem3.DataContext = i.ToString();
                        selectItem3.Click += new RoutedEventHandler(selectItem3_Click);
                        contextMenu.Items.Add(selectItem3);

                        contextMenu.IsOpen = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        void myDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Microsoft.Windows.Controls.DataGrid dataGrid = sender as Microsoft.Windows.Controls.DataGrid;
                Microsoft.Windows.Controls.DataGridRow row = (Microsoft.Windows.Controls.DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                Microsoft.Windows.Controls.DataGridCell RowColumn = dataGrid.Columns[2].GetCellContent(row).Parent as Microsoft.Windows.Controls.DataGridCell;
                
                string userId = ((TextBlock)RowColumn.Content).Text;                

                for (int i = 0; i < Login.userList.Count; i++)
                {
                    if (userId == Login.userList[i].Id)
                    {
                        Login._AskChatInfo.AskingID = Login._UserInfo.Id;
                        Login._AskChatInfo.TargetId = userId;
                        Login._AskChatInfo.Agree = 1;
                        Login._ClientEngine.Send(NotifyType.Reply_EnterMeeting, Login._AskChatInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        void selectItem3_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem3 = sender as MenuItem;
            string proNum = menuItem3.DataContext.ToString();

            Login.selectInfo = Login.userList[Convert.ToInt32(proNum)];
            Login._ClientEngine.Send(NotifyType.Request_PartnerDetail, Login.selectInfo);

        }

        //게임목록현시
        public void PlayGameList(List<GameInfo> topGame)
        {
            allGamePan.Children.Clear();

            for (int j = 0; j < topGame.Count; j++)
            {
                var gridGame = new Grid();                           
                gridGame.Width = 120;
                gridGame.Height = 60;
                gridGame.Margin = new Thickness(3, 3, 3, 3);
                gridGame.MouseEnter += new MouseEventHandler(gridGame_MouseEnter);
                gridGame.MouseLeave += new MouseEventHandler(gridGame_MouseLeave);

                Border border = new Border();
                border.BorderThickness = new Thickness(0.2);
                border.BorderBrush = Brushes.Gray;
                border.Margin = new Thickness(1);
                border.CornerRadius = new CornerRadius(5);

                GameInfoUserCtrl gameInfoUserCtrl = new GameInfoUserCtrl();
                gameInfoUserCtrl.InitUserCtrl(topGame[j]);
                border.Child = gameInfoUserCtrl;
                gridGame.Children.Add(border);
                allGamePan.Children.Add(gridGame);                            
            }

        }

        void gridGame_MouseLeave(object sender, MouseEventArgs e)
        {
            var gridLeave = sender as Grid;
            gridLeave.Background = new SolidColorBrush(Colors.Transparent);
        }

        void gridGame_MouseEnter(object sender, MouseEventArgs e)
        {
            var gridEnter = sender as Grid;            
            gridEnter.Background = (System.Windows.Media.LinearGradientBrush)FindResource("GradientColor");
        }

        void gameImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }


        
        //방목록리스트 현시
        #region RoomInfoList

        private void RoomInfoList(List<RoomInfo> roomlist)
        {
            roomPan.Children.Clear();
            roomUserControlList.Clear();
            for (int i = 0; i < roomlist.Count; i++)
            {
                roomUserControl = new RoomUserControl();
                roomUserControlList.Add(roomUserControl);
                roomUserControl.RoomListGrid(roomlist[i]);

                roomPan.Children.Add(roomUserControl);
            }
        }

        #endregion

        //소설,비데오,그림리스트 현시
        #region NVPInfoList
        private bool selectPNF = true;
        private void NVPInfoList()
        {
            if (selectPNF == false)
                return;

            selectPNF = false;

            for (int i = 0; i < Login._ClassListInfo.Classes.Count; i++)
            {
                PNF_UserControl pnfControl = new PNF_UserControl();

                if (Login._ClassListInfo.Classes[i].ClassInfo_Id == 0)
                {
                    pnfControl.Width = picturePanel.Width;
                    pnfControl.label1.Width = pnfControl.Width / 2;
                    pnfControl.label2.Width = pnfControl.Width / 2;
                    pnfControl.InitPNFSetting(Login._ClassListInfo.Classes[i]);
                    pnfControl.FocusVisualStyle = null;
                    picturePanel.Children.Add(pnfControl);
                }

                if (Login._ClassListInfo.Classes[i].ClassInfo_Id == 1)
                {

                }

                if (Login._ClassListInfo.Classes[i].ClassInfo_Id == 2)
                {
                    pnfControl.Width = filmPanel.Width;
                    pnfControl.label1.Width = pnfControl.Width / 2;
                    pnfControl.label2.Width = pnfControl.Width / 2;
                    pnfControl.InitPNFSetting(Login._ClassListInfo.Classes[i]);
                    pnfControl.FocusVisualStyle = null;
                    filmPanel.Children.Add(pnfControl);
                }

                if (Login._ClassListInfo.Classes[i].ClassInfo_Id == 3)
                {
                    Tab_PNF.SiteURLUserCtrl siteUrlCtrl = new Tab_PNF.SiteURLUserCtrl();
                    siteUrlCtrl.Width = filmPanel.Width;
                    siteUrlCtrl.InitSiteUrlCtrl(Login._ClassListInfo.Classes[i]);
                    siteUrlCtrl.FocusVisualStyle = null;
                    sitePanel.Children.Add(siteUrlCtrl);
                }
            }
        }

        #endregion        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


        public void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ExitProgram();
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void chargeBt_MouseUp(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                if (Main.loadWnd == null)
                {
                    Main.loadWnd = new LoadingWnd();
                    Main.loadWnd.Owner = this;
                    loadWnd.ShowDialog();
                }
            }));
            
            Login.m_nFlagMoneyBagBtnOrUserDetail = 0;
            Login._ClientEngine.Send(NotifyType.Request_UserDetail, Login._UserInfo);
        }

        private void button2_MouseUp(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                if (Main.loadWnd == null)
                {
                    Main.loadWnd = new LoadingWnd();
                    Main.loadWnd.Owner = this;
                    loadWnd.ShowDialog();
                }
            }));
            Login.m_nFlagMoneyBagBtnOrUserDetail = 1;
            Login._ClientEngine.Send(NotifyType.Request_UserDetail, Login._UserInfo);
        }

        private void button3_MouseUp(object sender, EventArgs e)
        {
            if (boardView > 0)
                return;
            ++boardView;
            notice_board = true;
            if (writePaper != null)
            {
                writePaper.Close();

                writePaper = new WritePaper();
                writePaper.noticeListView(Login.boardList);
                writePaper.Show();
            }
            else
            {
                writePaper = new WritePaper();
                writePaper.noticeListView(Login.boardList);
                writePaper.Show();
            }
        }

        private void button4_MouseUp(object sender, EventArgs e)
        {
            if (noticeView > 0)
                return;
            ++noticeView;
            notice_board = false;
            if (writePaper != null)
            {
                writePaper.Close();

                writePaper = new WritePaper();
                writePaper.noticeListView(Login.noticeList);
                writePaper.Show();
               
            }
            else
            {
                writePaper = new WritePaper();
                writePaper.noticeListView(Login.noticeList);
                writePaper.Show();
            }
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl))
                return;

            int tabInsex = tabControl1.SelectedIndex;
            switch (tabInsex)
            {
                case 0:
                    {
                        Storyboard memberStoryboard;
                        memberStoryboard = (Storyboard)this.Resources["StoryMember"];
                        memberStoryboard.Begin(this);
                    }
                    break;

                case 1:
                    {
                        
                    }
                    break;

                case 2:
                    {
                       
                        Storyboard gameStoryboard;
                        gameStoryboard = (Storyboard)this.Resources["StoryGame"];
                        gameStoryboard.Begin(this);

                        if (!_bFirstRequestGameInfo || Login._GameListInfo.Games.Count == 0)
                        {
                            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                            {
                                if (Main.loadWnd == null)
                                {
                                    Main.loadWnd = new LoadingWnd();
                                    Main.loadWnd.Owner = this;
                                    loadWnd.ShowDialog();
                                }
                            }));

                            Login._ClientEngine.Send(NotifyType.Request_GameList, Login._UserInfo);
                            _bFirstRequestGameInfo = true;
                        }
                        else
                        {
                            if (Login._GameListInfo != null)
                            {
                                
                                for(int i=0;i<Login._GameListInfo.Games.Count;i++)
                                {
                                    if (Login._GameListInfo.Games[i].Source == "Sicbo")
                                    {
                                        Login._ChatGameInfo = Login._GameListInfo.Games[i];
                                        allGameList = Login._GameListInfo.Games;
                                        PlayGameList(allGameList);
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    break;

                case 3:
                    {
                        Storyboard pnfStoryboard;
                        pnfStoryboard = (Storyboard)this.Resources["pnfStoryboard1"];
                        pnfStoryboard.Begin(this);

                        if (!_bFirstRequestPNFInfo || Login._ClassListInfo.Classes.Count == 0)
                        {
                            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                            {
                                if (Main.loadWnd == null)
                                {
                                    Main.loadWnd = new LoadingWnd();
                                    Main.loadWnd.Owner = this;
                                    loadWnd.ShowDialog();
                                }
                            }));

                            Login._ClientEngine.Send(NotifyType.Request_ClassInfo, Login._UserInfo);
                            _bFirstRequestPNFInfo = true;
                        }
                        else
                        {
                            NVPInfoList();
                        }
                    }
                    break;
            }

        }

        // 2013-05-18: GreenRose
        // 설명: TrayIcon의 ContextMenu에서 LogOut을 눌렀을때의 처리이다.
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {                        
            ExitProgram();
        }

        // 2014-05-16: GreenRose
        private void EmptyFolder(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                try
                {
                    if (file.Name == "Temp.jpg")
                        continue;

                    file.Delete();
                }
                catch (Exception)
                { }
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                EmptyFolder(subfolder);
            }
        }

        public void ExitProgram()
        {
            Login._ClientEngine.Send(NotifyType.Request_Logout, Login._UserInfo);
            Login._ClientEngine.DetachHandler(NotifyOccured);            
            
            //try
            //{
            //    string exePath = System.Windows.Forms.Application.StartupPath;

            //    System.IO.DirectoryInfo directoryInfo = null;

            //    directoryInfo = new System.IO.DirectoryInfo(exePath + "\\Data");
            //    EmptyFolder(directoryInfo);
            //    //foreach (FileInfo file in directoryInfo.GetFiles())
            //    //{
            //    //    try
            //    //    {
            //    //        if (file.Name == "Temp.jpg")
            //    //            continue;

            //    //        file.Delete();
            //    //    }
            //    //    catch (Exception)
            //    //    { }
            //    //}

            //    //directoryInfo = new System.IO.DirectoryInfo(exePath + "\\ImageFile");
            //    //foreach (FileInfo file in directoryInfo.GetFiles())
            //    //{
            //    //    try
            //    //    {
            //    //        file.Delete();
            //    //    }
            //    //    catch (Exception ex)
            //    //    {
            //    //        string strError = ex.ToString();                        
            //    //    }
            //    //}

            //    directoryInfo = new System.IO.DirectoryInfo(exePath + "\\Images\\Face");
            //    foreach (FileInfo file in directoryInfo.GetFiles())
            //    {
            //        try
            //        {
            //            file.Delete();
            //        }
            //        catch (Exception)
            //        { }
            //    }

                
            //}
            //catch (Exception)
            //{ }
            
            Destroy();

            MyNotifyIcon.Dispose();

            SendErrorFile();
            
            if(bSendErrorFileSuccess)
                Application.Current.Shutdown();
        }

        // 2014-02-05: GreenRose
        // 에러파일을 서버에 전송한다.

        bool bSendErrorFileSuccess = true;
        private void SendErrorFile()
        {
            try
            {
                string strErrorFilePath = Path.GetFullPath("Error.txt");
                FileInfo fileInfo = new FileInfo(strErrorFilePath);

                strErrorFilePath = fileInfo.DirectoryName + "\\" + System.DateTime.Now.ToString("hhmmss") + "-Error.txt";

                if (fileInfo.Exists)
                {
                    bSendErrorFileSuccess = false;
                    fileInfo.MoveTo(strErrorFilePath);

                    string strUri = Login._ServerPath;
                    if (strUri[strUri.Length - 1] != '/')
                    {
                        strUri = strUri + "/";
                    }

                    strUri += "ErrorUpload.php";
                    WebUploader.GetInstance().UploadFile(strErrorFilePath, strUri, FileUploadComplete, this);
                }
            }
            catch (Exception)
            {
                bSendErrorFileSuccess = true;
            }
        }

        // 2013-12-31: GreenRose
        private void FileUploadComplete(string strFileName)
        {
            File.Delete(strFileName);
            Application.Current.Shutdown();
        }

        private void showItem_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
        }


        private void ShowBallon(List<UserInfo> orgUserInfos, List<UserInfo> curUserInfos)
        {
            if (orgUserInfos == null || curUserInfos == null)
                return;

            foreach (UserInfo userInfo in curUserInfos)
            {
                bool bExist = false;
                for (int i = 0; i < orgUserInfos.Count; i++)
                {
                    if (userInfo.Id.Equals(orgUserInfos[i].Id))
                    {
                        bExist = true;
                        break;
                    }
                }

                if (!bExist)
                {
                    Samples.FancyBalloon balloon = new Samples.FancyBalloon();
                    balloon.BalloonText = userInfo.Id + "登陆";

                    MyNotifyIcon.ShowCustomBalloon(balloon, System.Windows.Controls.Primitives.PopupAnimation.Slide, 6000);
                }
            }
        }

        private void singBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {

                TextRange documentTextRange = new TextRange(this.singBox.Document.ContentStart, singBox.Document.ContentEnd);
                string text = documentTextRange.Text;
            }
            else
            {
                TextRange documentTextRange = new TextRange(this.singBox.Document.ContentStart, singBox.Document.ContentEnd);
                string text = documentTextRange.Text;

                System.Windows.Documents.FlowDocument fd = new System.Windows.Documents.FlowDocument();
                fd.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run(text)));
                singBox.Document = fd;

                Login._UserInfo.Sign = text;
                Login._ClientEngine.Send(NotifyType.Request_UpdateUser, Login._UserInfo);

                singBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void singBox_MouseEnter(object sender, MouseEventArgs e)
        {
            TextRange documentTextRange = new TextRange(this.singBox.Document.ContentStart, singBox.Document.ContentEnd);
            string text = documentTextRange.Text;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            ExitProgram();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {            
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                if (Main.loadWnd == null)
                {
                    Main.loadWnd = new LoadingWnd();
                    Main.loadWnd.Owner = this;
                    loadWnd.ShowDialog();
                }
            }));

            Login.m_nFlagMoneyBagBtnOrUserDetail = 1;
            Login._ClientEngine.Send(NotifyType.Request_UserDetail, Login._UserInfo);
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                if (Main.loadWnd == null)
                {
                    Main.loadWnd = new LoadingWnd();
                    Main.loadWnd.Owner = this;
                    loadWnd.ShowDialog();
                }
            }));

            Login.m_nFlagMoneyBagBtnOrUserDetail = 0;
            Login._ClientEngine.Send(NotifyType.Request_UserDetail, Login._UserInfo);
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (boardView > 0)
                return;
            ++boardView;
            notice_board = true;
            if (writePaper != null)
            {
                writePaper.Close();

                writePaper = new WritePaper();
                writePaper.noticeListView(Login.boardList);
                writePaper.Show();
            }
            else
            {
                writePaper = new WritePaper();
                writePaper.noticeListView(Login.boardList);
                writePaper.Show();
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            if (noticeView > 0)
                return;
            ++noticeView;
            notice_board = false;
            if (writePaper != null)
            {
                writePaper.Close();

                writePaper = new WritePaper();
                writePaper.noticeListView(Login.noticeList);
                writePaper.Show();

            }
            else
            {
                writePaper = new WritePaper();
                writePaper.noticeListView(Login.noticeList);
                writePaper.Show();
            }
        }

        private void memberImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                if (Main.loadWnd == null)
                {
                    Main.loadWnd = new LoadingWnd();
                    Main.loadWnd.Owner = this;
                    loadWnd.ShowDialog();
                }
            }));
            Login.m_nFlagMoneyBagBtnOrUserDetail = 1;
            Login._ClientEngine.Send(NotifyType.Request_UserDetail, Login._UserInfo);
        }

        // 2013-12-11: GreenRose
        // 트레이아이콘을 두번클릭하였을때의 처리 진행.
        private void MyNotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void notify_iconImonline_Click(object sender, RoutedEventArgs e)
        {
            SendIMOnLine();
        }

        private void SendIMOnLine()
        {
            // 만일 사용자가 여자일때 비디오창이 켜져있으면 닫는다.
            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {
                if (soloVideoForm != null)
                {
                    soloVideoForm.Close();
                }
            }

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri("/Resources;component/Icons/imonline.ico", UriKind.RelativeOrAbsolute);
            bi.EndInit();

            this.imgLoginState.Source = bi;
            Login._UserInfo.nUserState = 0;
            Login._ClientEngine.Send(NotifyType.Request_UserState, Login._UserInfo);
        }

        private void notify_iconImoffline_Click(object sender, RoutedEventArgs e)
        {
            SendIMOffline();
        }

        private void SendIMOffline()
        {
            // 만일 사용자가 여자일때 비디오창이 켜져있으면 닫는다.
            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {
                if (soloVideoForm != null)
                {
                    soloVideoForm.Close();
                }
            }

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri("/Resources;component/Icons/imoffline.ico", UriKind.RelativeOrAbsolute);
            bi.EndInit();

            this.imgLoginState.Source = bi;
            Login._UserInfo.nUserState = 1;
            Login._ClientEngine.Send(NotifyType.Request_UserState, Login._UserInfo);
        }

        private void notify_iconImbusy_Click(object sender, RoutedEventArgs e)
        {
            SendIMBusy();
        }

        private void SendIMBusy()
        {
            // 만일 사용자가 여자일때 비디오창이 켜져있으면 닫는다.
            if(Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {
                if(soloVideoForm != null)
                {
                    soloVideoForm.Close();
                }
            }

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri("/Resources;component/Icons/busy.ico", UriKind.RelativeOrAbsolute);
            bi.EndInit();

            this.imgLoginState.Source = bi;
            Login._UserInfo.nUserState = 2;
            Login._ClientEngine.Send(NotifyType.Request_UserState, Login._UserInfo);
        }

        private void notify_iconImaway_Click(object sender, RoutedEventArgs e)
        {
            SendIMGoAway();
        }

        private void SendIMGoAway()
        {
            // 만일 사용자가 여자일때 비디오창이 켜져있으면 닫는다.
            if (Login._UserInfo.Kind == (int)UserKind.ServiceWoman)
            {
                if (soloVideoForm != null)
                {
                    soloVideoForm.Close();
                }
            }

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri("/Resources;component/Icons/away.ico", UriKind.RelativeOrAbsolute);
            bi.EndInit();

            this.imgLoginState.Source = bi;
            Login._UserInfo.nUserState = 3;
            Login._ClientEngine.Send(NotifyType.Request_UserState, Login._UserInfo);
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void singBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextRange documentTextRange = new TextRange(this.singBox.Document.ContentStart, singBox.Document.ContentEnd);
            string text = documentTextRange.Text;
            if (text == "编辑个性签名\r\n")
            {
                FlowDocument fd = new FlowDocument();
                fd.Blocks.Add(new Paragraph(new Run("")));
                this.singBox.Document = fd;
            }
        }

        private void singBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextRange documentTextRange = new TextRange(this.singBox.Document.ContentStart, singBox.Document.ContentEnd);
            string text = documentTextRange.Text;
            if (text.Trim() == "")
            {
                FlowDocument fd = new FlowDocument();
                fd.Blocks.Add(new Paragraph(new Run("编辑个性签名")));
                this.singBox.Document = fd;
            }
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                if (Main.loadWnd == null)
                {
                    Main.loadWnd = new LoadingWnd();
                    Main.loadWnd.Owner = this;
                    loadWnd.ShowDialog();
                }
            }));
            Login.m_nFlagMoneyBagBtnOrUserDetail = 0;
            Login._ClientEngine.Send(NotifyType.Request_UserDetail, Login._UserInfo);
        }

        private void btnMyInfo_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                if (Main.loadWnd == null)
                {
                    Main.loadWnd = new LoadingWnd();
                    Main.loadWnd.Owner = this;
                    loadWnd.ShowDialog();
                }
            }));
            Login.m_nFlagMoneyBagBtnOrUserDetail = 1;
            Login._ClientEngine.Send(NotifyType.Request_UserDetail, Login._UserInfo);
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
            {
                if (Main.loadWnd == null)
                {
                    Main.loadWnd = new LoadingWnd();
                    Main.loadWnd.Owner = this;
                    loadWnd.ShowDialog();
                }
            }));
            Login.m_nFlagMoneyBagBtnOrUserDetail = 2;
            Login._ClientEngine.Send(NotifyType.Request_UserDetail, Login._UserInfo);
        }

        private void btnReceiveMessage_Click(object sender, RoutedEventArgs e)
        {
            if (boardView > 0)
                return;
            ++boardView;
            notice_board = true;
            if (writePaper != null)
            {
                writePaper.Close();

                writePaper = new WritePaper();
                writePaper.noticeListView(Login.boardList);
                writePaper.Show();
            }
            else
            {


                writePaper = new WritePaper();
                writePaper.noticeListView(Login.boardList);
                writePaper.Show();
            }
        }

        private void btnStyleChange_Click(object sender, RoutedEventArgs e)
        {
            LoadStyle loadStyle = new LoadStyle();
            
            _nStyleNumber = _listStyle.FindIndex(item => item == Login._strMainResourceDicSource);
            _nStyleNumber++;

            if (_nStyleNumber + 1 > _listStyle.Count)
                _nStyleNumber = 0;            

            loadStyle.LoadCurrentStyle(Login._strMainResourceDicSource, _listStyle[_nStyleNumber], "MainResource");
            Login._strMainResourceDicSource = _listStyle[_nStyleNumber];            
        }

        // 2014-02-09: GreenRose
        public static Window GetParentWindow(DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            Window parent = parentObject as Window;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return GetParentWindow(parentObject);
            }
        }

        // 2014-02-13: GreenRose
        public void RightToLeftMarquee()
        {
            double height = canMain.ActualHeight - searchTextBox.ActualHeight;
            searchTextBox.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = canMain.ActualWidth;
            doubleAnimation.To = -MeasureString(searchTextBox.Text).Width;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            int nCount = 1;
            if ((int)doubleAnimation.To / 100 * (-1) < 1)
            {
                nCount = 1;
            }
            else
            {
                nCount = (int)doubleAnimation.To / 100 * (-1);
            }

            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(nCount * 5));
            searchTextBox.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }

        private Size MeasureString(string candidate)
        {
            var formattedText = new FormattedText(
                candidate,
                System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(this.searchTextBox.FontFamily, this.searchTextBox.FontStyle, this.searchTextBox.FontWeight, this.searchTextBox.FontStretch),
                this.searchTextBox.FontSize,
                Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

        private UserInfo GetUserInfoByID(string strID)
        {
            UserInfo userInfo = null;

            userInfo = Login._UserListInfo._Users.Find(item => item.Id == strID);

            return userInfo;
        }


        // 2014-03-20: GreenRose
        public string m_strWritingStart = "WritingStart+GIT";
        public string m_strWritingEnd = "WritingEnd+GIT";
        public string m_strShakeWindow = "ShakeWindow+GIT";
        public string m_strVideoChatPass = "VideoChatStart+GIT";
        public string m_strVideoAccept = "VideoChatAccept+GIT";
        public string m_strVideoReject = "VideoChatReject+GIT";
        public string m_strVideoChatEnd = "VideoChatEnd+GIT";

        private bool InterpretSignalText(string strSignal, string strID)
        {
            string strUserName = Login._UserInfo.Id;

            if (strSignal.Equals(m_strShakeWindow))
            {
                if (!strUserName.Equals(strID))
                {
                }

                return false;
            }

            if (strSignal.Equals(m_strWritingStart))
            {
                if (!strUserName.Equals(strID))
                {
                }

                return false;
            }

            if (strSignal.Equals(m_strWritingEnd))
            {
                if (!strUserName.Equals(strID))
                {
                }

                return false;
            }

            if (strSignal.Equals(m_strVideoChatEnd))
            {
                if (!strUserName.Equals(strID))
                {
                }

                return false;
            }

            return true;
        }
    }
}
