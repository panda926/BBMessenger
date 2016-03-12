using System;
using System.Collections.Generic;
using System.Windows;
using System.Threading;
using ChatEngine;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net.Sockets;
using System.Windows.Controls;
using System.Diagnostics;

using System.ComponentModel;
using ControlExs;
using System.IO;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : BaseWindow
    {
        #region Variable

        static public string _ServerUri = "";
        static public string _ServerServiceUri = "";
        static public string _ServerRealUri = "";
        static public string _ServerPort = "";
        static public string _ServerPath = "";
        static public string _ServerGamePath = "";
        static public string _UserPath = System.IO.Path.GetFullPath("UserInfo.ini");
        static public Client _ClientEngine;
        static public UdpServerClient _VideoEngine = new UdpServerClient();
        static public HomeInfo _HomeInfo = new HomeInfo();
        static public BoardInfo _BoardInfo = new BoardInfo();
        static public UserInfo _UserInfo = new UserInfo();
        static public UserDetailInfo _UserDetailInfo = new UserDetailInfo();
        static public GameInfo _ChatGameInfo = new GameInfo();
        static public RoomInfo _RoomInfo = new RoomInfo();
        static public AskChatInfo _AskChatInfo = new AskChatInfo();
        static public GameListInfo _GameListInfo = new GameListInfo();
        static public UserListInfo _UserListInfo = new UserListInfo();
        static public RoomListInfo _RoomListInfo = new RoomListInfo();
        static public StringInfo _StringInfo = new StringInfo();
        static public ClassListInfo _ClassListInfo = new ClassListInfo();
        static public ClassTypeListInfo _ClassTypeListInfo = new ClassTypeListInfo();
        static public ClassInfo _ClassInfo = new ClassInfo();
        static public ClassTypeInfo _ClassTypeInfo = new ClassTypeInfo();
        static public ClassPictureDetailInfo _ClassPictureDetailInfo = new ClassPictureDetailInfo();

        static public List<UserInfo> userList = null;
        static public List<UserInfo> topUserList = null;
        static public List<RoomInfo> allRoomlist = null;
        static public UserDetailInfo selectUserDetail = null;
        static public UserInfo selectInfo = null;
        static public List<UserInfo> SelectUserInfoList = new List<UserInfo>();
        static public MyHome myhome = null;
        static public FriendIDCreate friendIDCreate= null;
        static public List<BoardInfo> noticeList = null;
        static public List<BoardInfo> boardList = null;

        static public Main main = null;
        static public int m_nCurUserCount = 0;
        string icon = null;
        IniFileEdit _IniFileEdit = new IniFileEdit(_UserPath);

        // 2013-12-16: GreenRose
        // 돈가방단추를 눌렀는가 아니면 사용자정보단추를 눌렀는가를 따져보고 Request_UserDetail을 보낸후 요청을 받을때 필요한 변수.
        static public int m_nFlagMoneyBagBtnOrUserDetail;

        // 2014-01-20: GreenRose
        // MainResourceDictionary Source
        static public string _strMainResourceDicSource = string.Empty;

        #endregion               

        public Login()
        {
            InitializeComponent();

            this.SetLanguageDictionary();

            InitSetting();
            InitMainResourceDic(); // WindowStyle Setting
            
            CheckUpdate();

            _ClientEngine = new Client(InvokeSocket);

            if (!_ClientEngine.Connect(_ServerUri, _ServerPort, ProtocolType.Tcp))
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
                QQMessageBox.Show(tempWindowForm, "服务器连接失败", "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);

                Environment.Exit(0);
            }

            _ClientEngine.AttachHandler(NotifyOccured);

            StartTimer();            

            ImageBrush myIconImg = new ImageBrush();
            myIconImg.Stretch = Stretch.Fill;

            icon = _IniFileEdit.GetIniValue("UserInfo", "userImage");

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(icon, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            
            myIconImg.ImageSource = ImageDownloader.GetInstance().GetImage(icon);
            logimg.Fill = myIconImg;

            txtUserName.Focus();


            // 2014-06-15: GreenRose
            DeleteDataFolder();
        }

        private void DeleteDataFolder()
        {
            try
            {
                string exePath = System.Windows.Forms.Application.StartupPath;

                System.IO.DirectoryInfo directoryInfo = null;

                directoryInfo = new System.IO.DirectoryInfo(exePath + "\\Data");
                EmptyFolder(directoryInfo);

                directoryInfo = new System.IO.DirectoryInfo(exePath + "\\Images\\Face");
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception)
                    { }
                }
            }
            catch (Exception)
            { }
        }

        // 2014-05-16: GreenRose
        private void EmptyFolder(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                try
                {                    
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

        void CheckUpdate()
        {
            string sourceFolder = AppDomain.CurrentDomain.BaseDirectory + "NewUpdate";
            string targetFolder = AppDomain.CurrentDomain.BaseDirectory + "Update";
 
            if (Directory.Exists(sourceFolder) == false)
                return;

            if (Directory.Exists(targetFolder) == false)
                return;

            DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);
            DirectoryInfo targetFolderInfo = new DirectoryInfo(targetFolder);

            foreach (FileInfo sourceFileInfo in sourceFolderInfo.GetFiles())
            {
                string targetFileName = string.Format("{0}\\{1}", targetFolder, sourceFileInfo.Name);

                File.Copy(sourceFileInfo.FullName, targetFileName, true);
            }

            Directory.Delete(sourceFolder, true);
        }

        // 2014-01-20: GreenRose
        private void InitMainResourceDic()
        {
            LoadStyle loadStyle = new LoadStyle();
            loadStyle.LoadCurrentStyle(_strMainResourceDicSource, _strMainResourceDicSource, "MainResource");
        }

        public void InvokeSocket()
        {
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                _ClientEngine.NotifyReceiveData();
            }));
        }

        private void StartTimer()
        {
            System.Windows.Threading.DispatcherTimer TimerClock = new System.Windows.Threading.DispatcherTimer();

            Random rand = new Random();
            m_nCurUserCount = rand.Next(100, 200);

            TimerClock.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 1 second
            TimerClock.IsEnabled = true;
            TimerClock.Tick += new EventHandler(TimerClock_Tick);
        }

        static int counter = 0;

        void TimerClock_Tick(object sender, EventArgs e)
        {            
            if (main == null)
                return;

            Random rand = new Random();

            double nSign = Math.Pow(-1, rand.Next() % 2);
            int nDeltaCount = rand.Next(1, 3);

            int nChangeFlag = rand.Next() % 10;

            if (m_nCurUserCount + nDeltaCount >= 175)
                nSign = -1;

            if (nChangeFlag == 0)
            {
                m_nCurUserCount = m_nCurUserCount + (int)nSign * nDeltaCount;

                main.txtUserCount.Text = string.Format("({0}人在线)", m_nCurUserCount);
            }
            
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {                
                case NotifyType.Reply_Login:
                    {
                        _UserInfo = (UserInfo)baseInfo;                                               

                        if (main != null)
                            return;

                        SavePass();

                        PB9.Visibility = Visibility.Hidden;
                        lblWaiting.Visibility = Visibility.Visible;
                        

                        main = new Main();

                        if (_UserInfo.Nickname == string.Empty)
                            main.nickName.Content = "未知昵称";
                        else
                            main.nickName.Content = _UserInfo.Nickname;
                        main.nickName.Content += "(" + _UserInfo.Id + ")";
                        ToolTip tt = new ToolTip();
                        tt.Content = _UserInfo.Nickname;
                        main.nickName.ToolTip = tt;

                        main.txtCash.Text = _UserInfo.Cash.ToString();
                        main.txtPoint.Text = _UserInfo.Point.ToString();
                        main.txtRenminbi.Text = (Login._UserInfo.Cash / 100 + Login._UserInfo.Point / 10000).ToString();

                        ImageBrush myIconImg = new ImageBrush();

                        myIconImg.Stretch = Stretch.Fill;
                        myIconImg.ImageSource = ImageDownloader.GetInstance().GetImage(_UserInfo.Icon);

                        main.memberImg.Fill = myIconImg;

                        ToolTip sign = new ToolTip();
                        if (_UserInfo.Sign == string.Empty)
                            _UserInfo.Sign = "未知的标志";
                                                
                        System.Windows.Documents.FlowDocument fd = new System.Windows.Documents.FlowDocument();
                        fd.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run(_UserInfo.Sign)));
                        main.singBox.Document = fd;

                        _IniFileEdit.SetIniValue("UserInfo", "userImage", _UserInfo.Icon);
                        
                        Login._ClientEngine.Send(NotifyType.Request_Home, _UserInfo);                                         

                        main.Show();

                        this.Hide();                        
                    }
                    break;

                case NotifyType.Reply_AdminNotice:
                    {
                        BoardInfo boardInfo = (BoardInfo)baseInfo;
                        if (main != null)
                        {
                            if (boardInfo != null)
                            {
                                if (boardInfo.Content != "")
                                {
                                    main.searchTextBox.Text = boardInfo.Content;
                                    main.RightToLeftMarquee();
                                }
                                else
                                {
                                    main.searchTextBox.Text = "Unknown Notice";
                                    main.RightToLeftMarquee();
                                }
                            }
                        }
                    }
                    break;

                case NotifyType.Reply_Error:
                    {
                        if (Main.loadWnd != null)
                        {
                            Main.loadWnd.Close();
                            Main.loadWnd = null;
                        }

                        btnLogIn.IsEnabled = true;
                        ResultInfo errorInfo = (ResultInfo)baseInfo;
                        ErrorType errorType = errorInfo.ErrorType;
                        
                        if(errorType == ErrorType.Duplicate_logout)
                        {
                            Login._ClientEngine._NotifyHandler(NotifyType.Notify_Duplicate, null, null);
                            break;
                        }

                        Login.ShowError(errorType);
                    }
                    break;
            }
        }
        
        static public void ShowError(ErrorType errorType)
        {
            IniFileEdit iniFileEdit = new IniFileEdit(_UserPath);
            string errorString = iniFileEdit.GetIniValue("MessageString", errorType.ToString());
            if (errorString == "")
                return;
            else
            {                
                TempWindowForm tempWindowForm = new TempWindowForm();                
                QQMessageBox.Show(tempWindowForm, errorString, "提示", QQMessageBoxIcon.Error, QQMessageBoxButtons.OK);

                if (errorType == ErrorType.Unknown_User)
                {
                    Main tempMain = Main._main;
                    if(tempMain != null)
                        tempMain.ExitProgram();
                }
            }
        }

        private void InitSetting()
        {
            IniFileEdit _FileEdit = new IniFileEdit(_UserPath);

            _ServerRealUri = _FileEdit.GetIniValue("ServerInfo", "ServerRealUri");      // 이미지,동영상 컨텐츠서버
            _ServerUri = _FileEdit.GetIniValue("ServerInfo", "ServerUri");              // 채팅 서버
            _ServerPort = _FileEdit.GetIniValue("ServerInfo", "ServerPort");            // 채팅 서버 포트
            _ServerServiceUri = _FileEdit.GetIniValue("ServerInfo", "ServerServiceUri");
            _ServerGamePath = "http://" + _FileEdit.GetIniValue("ServerInfo", "ServerGamePath");    // 다운로드 서버
            _ServerPath = "http://" + _ServerRealUri + "/";

            _strMainResourceDicSource = _FileEdit.GetIniValue("StyleInfo", "MainResource");
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {            
            _UserInfo.Id = txtUserName.Text;
            _UserInfo.Password = txtPassword.Password;

             _ClientEngine.Send(NotifyType.Request_Login, _UserInfo);

            btnLogIn.IsEnabled = false;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Activate();

            lblWaiting.Visibility = Visibility.Hidden;
            PB9.Visibility = Visibility.Visible;
            this.UpdateLayout();

            // Show Password And UserID
            CheckRememberState();
        }

        // 2014-02-19: GreenRose
        private void CheckRememberState()
        {
            try
            {
                string strPassword = _IniFileEdit.GetIniValue("UserInfo", "userPass");
                string strID = _IniFileEdit.GetIniValue("UserInfo", "userId");

                if (strPassword != string.Empty && strID != string.Empty)
                {
                    this.txtUserName.Text = strID;
                    this.txtPassword.Password = strPassword;

                    chkStorePass.IsChecked = true;
                }
                else
                    chkStorePass.IsChecked = false;
            }
            catch (Exception)
            { }
        }

        // 2014-02-19: GreenRose
        private void SavePass()
        {
            try
            {
                if (chkStorePass.IsChecked == true)
                {
                    string strID = this.txtUserName.Text;
                    string strPassword = this.txtPassword.Password;

                    _IniFileEdit.SetIniValue("UserInfo", "userId", strID);
                    _IniFileEdit.SetIniValue("UserInfo", "userPass", strPassword);
                }
                else
                {
                    _IniFileEdit.SetIniValue("UserInfo", "userId", string.Empty);
                    _IniFileEdit.SetIniValue("UserInfo", "userPass", string.Empty);
                }
            }
            catch (Exception)
            { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_ClientEngine != null)
            {                
                _ClientEngine.DetachHandler(NotifyOccured);
                _ClientEngine.Disconnect();
            }
        }

        private void txtUserName_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txtUserName.Text != string.Empty)
                    {                        
                        txtPassword.Focus();
                    }
                    else
                    {
                        txtUserName.Focus();
                    }
                }   
            }
            catch (Exception ex)
            {
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
        }

        private void txtPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtPassword.Password != string.Empty)
                {
                    btnLogIn.Focus();
                }
                else
                {
                    txtPassword.Focus();
                }
            }
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void linkSiteUrl_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.OriginalString));
                e.Handled = true;
            }
            catch (System.Exception ex)
            {
                string strErrorMsg = ex.ToString();
            }
        }
    }
}
