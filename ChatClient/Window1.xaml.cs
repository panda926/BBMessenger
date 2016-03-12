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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        #region Variable

        static public string _ServerUri = "";
        static public string _ServerServiceUri = "";
        static public string _ServerRealUri = "";
        static public string _ServerPort = "";
        static public string _ServerPath = "";
        static public string _UserPath = System.IO.Path.GetFullPath("UserInfo.ini");
        static public Client _ClientEngine = new Client();
        static public UdpServerClient _VideoEngine = new UdpServerClient();
        static public HomeInfo _HomeInfo = new HomeInfo();
        static public BoardInfo _BoardInfo = new BoardInfo();
        static public UserInfo _UserInfo = new UserInfo();
        static public UserDetailInfo _UserDetailInfo = new UserDetailInfo();
        static public GameInfo _GameInfo = new GameInfo();
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
        static public List<BoardInfo> noticeList = null;
        static public List<BoardInfo> boardList = null;

        static public Main main = null;
        string icon = null;
        IniFileEdit _IniFileEdit = new IniFileEdit(_UserPath);

        #endregion

        public Window1()
        {
            InitializeComponent();
            this.SetLanguageDictionary();

            InitSetting();

            _ClientEngine.Connect(_ServerUri, _ServerPort, ProtocolType.Tcp);
            _ClientEngine.AttachHandler(NotifyOccured);

            _VideoEngine.Connect(_ServerUri, Convert.ToInt32(_ServerPort), 1);

            StartTimer();
            //_ClientEngine.Send(NotifyType.Request_Home, _UserInfo);

            ImageBrush myIconImg = new ImageBrush();
            myIconImg.Stretch = Stretch.Fill;

            icon = _IniFileEdit.GetIniValue("UserInfo", "userImage");
            myIconImg.ImageSource = ImageDownloader.GetInstance().GetImage(icon);
            logimg.Fill = myIconImg;

            txtUserName.Focus();
        }

        private void StartTimer()
        {
            System.Windows.Threading.DispatcherTimer TimerClock = new System.Windows.Threading.DispatcherTimer();

            TimerClock.Interval = new TimeSpan(0, 0, 0, 0, 200); // 300 milliseconds
            TimerClock.IsEnabled = true;
            TimerClock.Tick += new EventHandler(TimerClock_Tick);
        }

        static int counter = 0;

        void TimerClock_Tick(object sender, EventArgs e)
        {
            //try
            //{
                _ClientEngine.NotifyReceiveData();

                if (_UserInfo != null)
                {
                    counter++;

                    if (counter >= 10)
                    {
                        _VideoEngine.Ping(_UserInfo);
                        counter = 0;
                    }
                }
            //}
            //catch { }
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_Login:
                    {
                        PB9.Visibility = Visibility.Hidden;
                        lblWaiting.Visibility = Visibility.Visible;
                        _UserInfo = (UserInfo)baseInfo;

                        main = new Main();
                        main.nickName.Content = _UserInfo.Nickname;
                        ToolTip tt = new ToolTip();
                        tt.Content = _UserInfo.Nickname;
                        main.nickName.ToolTip = tt;

                        ImageBrush myIconImg = new ImageBrush();

                        myIconImg.Stretch = Stretch.Fill;
                        myIconImg.ImageSource = ImageDownloader.GetInstance().GetImage(_UserInfo.Icon);

                        main.memberImg.Fill = myIconImg;

                        ToolTip sign = new ToolTip();
                        sign.Content = _UserInfo.Sign;
                        main.singBox.ToolTip = sign;
                        main.singBox.Text = _UserInfo.Sign;

                        _IniFileEdit.SetIniValue("UserInfo", "userImage", _UserInfo.Icon);

                        Window1._ClientEngine.Send(NotifyType.Request_UserList, _UserInfo);
                        Window1._ClientEngine.Send(NotifyType.Request_Home, _UserInfo);

                        Window1._ClientEngine.Send(NotifyType.Request_GameList, _UserInfo);
                        Window1._ClientEngine.Send(NotifyType.Request_RoomList, _UserInfo);

                        main.Show();

                        this.Hide();
                    }
                    break;
                case NotifyType.Reply_Error:
                    {
                        ResultInfo errorInfo = (ResultInfo)baseInfo;
                        ErrorType errorType = errorInfo.ErrorType;

                        Window1.ShowError(errorType);
                    }
                    break;
            }
        }

        private void OpenForm()
        {
            main = new Main();
            main.nickName.Content = _UserInfo.Nickname;
            ToolTip tt = new ToolTip();
            tt.Content = _UserInfo.Nickname;
            main.nickName.ToolTip = tt;

            ImageBrush myIconImg = new ImageBrush();

            myIconImg.Stretch = Stretch.Fill;
            myIconImg.ImageSource = ImageDownloader.GetInstance().GetImage(_UserInfo.Icon);

            main.memberImg.Fill = myIconImg;

            ToolTip sign = new ToolTip();
            sign.Content = _UserInfo.Sign;
            main.singBox.ToolTip = sign;
            main.singBox.Text = _UserInfo.Sign;

            _IniFileEdit.SetIniValue("UserInfo", "userImage", _UserInfo.Icon);

            Window1._ClientEngine.Send(NotifyType.Request_UserList, _UserInfo);
            Window1._ClientEngine.Send(NotifyType.Request_Home, _UserInfo);

            main.Show();

            this.Hide();
        }

        static public void ShowError(ErrorType errorType)
        {
            IniFileEdit iniFileEdit = new IniFileEdit(_UserPath);
            string errorString = iniFileEdit.GetIniValue("MessageString", errorType.ToString());
            if (errorString == "")
                return;
            else
                MessageBoxCommon.Show(errorString, MessageBoxType.Ok);
        }

        private void InitSetting()
        {
            IniFileEdit _FileEdit = new IniFileEdit(_UserPath);

            _ServerRealUri = _FileEdit.GetIniValue("ServerInfo", "ServerRealUri");
            _ServerUri = _FileEdit.GetIniValue("ServerInfo", "ServerUri");
            _ServerPort = _FileEdit.GetIniValue("ServerInfo", "ServerPort");
            _ServerServiceUri = _FileEdit.GetIniValue("ServerInfo", "ServerServiceUri");
            _ServerPath = "http://" + _ServerRealUri + "/";
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //progressBar1.Visibility = Visibility.Visible;
            //Process();

            _UserInfo.Id = txtUserName.Text;
            _UserInfo.Password = txtPassword.Password;
            _ClientEngine.Send(NotifyType.Request_Login, _UserInfo);

            
        }

        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);


        private void Process()
        {
            //Configure the ProgressBar
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 15000;
            progressBar1.Value = 0;

            //Stores the value of the ProgressBar
            double value = 0;

            //Create a new instance of our ProgressBar Delegate that points
            //  to the ProgressBar's SetValue method.
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);

            //Tight Loop:  Loop until the ProgressBar.Value reaches the max
            do
            {
                value += 1;

                /*Update the Value of the ProgressBar:
                  1)  Pass the "updatePbDelegate" delegate that points to the ProgressBar1.SetValue method
                  2)  Set the DispatcherPriority to "Background"
                  3)  Pass an Object() Array containing the property to update (ProgressBar.ValueProperty) and the new value */
                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, value });

            }
            while (progressBar1.Value != progressBar1.Maximum);

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
            lblWaiting.Visibility = Visibility.Hidden;
            PB9.Visibility = Visibility.Visible;
            this.UpdateLayout();

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
                if (e.Key == Key.Enter || e.Key == Key.Tab)
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
            catch (Exception)
            { }
        }

        private void txtPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
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
    }
}
