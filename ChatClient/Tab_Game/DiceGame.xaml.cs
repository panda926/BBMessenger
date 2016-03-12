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
using ChatEngine;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Media.Animation;
using System.Collections;
using System.Media;

using ControlExs;
using System.Runtime.InteropServices;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for DiceGame.xaml
    /// </summary>
    public partial class DiceGame : Window
    {
        public GameInfo _GameInfo = null;
        public bool soundState = false;
        private BettingInfo _BettingInfo = new BettingInfo();
        public DiceInfo _DiceInfo = null;
        private List<UserInfo> _PlayerList = null;
        private List<UserInfo> _initPlayerList = null;
        private List<int> _PlayerTotal = new List<int>();
        private int[] _userScore = null;
        private int[] _winScore = null;
        private sortMoney[] s_totalScoreList = new sortMoney[0];
        private int[] _totalWinScore = new int[100];
        private int[] _userCard = null;
        private int[,] _mlUserSocre = null;
        private int[,] _initmlUserSocre = null;
        

        private List<Image> _Image = new List<Image>();

        private int currentBettingMoney;
        private int bettingMoney;
        private int selfBettingMoney;
        private double deltaLeft;
        private double deltaTop;
        private double deltaRight;
        private double deltaBottom;
        private int dice1;
        private int dice2;
        private int dice3;
        private int totalDice;
        private int score = 0;
        private int myCash;
        private int scoreResult;
        //private int counter = 0;
        private int roundCount = 0;
        private int addroundCount = 0;
        private int bettingTotalmoney = 0;
        //private int bettingTimeReduce = 0;


        int pageIndex = 0;
        int currentPage = 0;

        

        private bool firstUserList = true;

        //private bool firstRoound = true;
        private bool secondRoound = true;

        private bool userListflag = true;

        

        private System.Windows.Threading.DispatcherTimer bettingClock = null;
        //private System.Windows.Threading.DispatcherTimer gameClock = null;
        //private System.Windows.Threading.DispatcherTimer rollClock = null;
        private System.Windows.Threading.DispatcherTimer sameClock = null;

        private int _bettingTime = 0;

        private Storyboard storyboard_da = null;
        private Storyboard storyboard_xiao = null;
        private Storyboard storyboard_shuang = null;
        private Storyboard storyboard_dan = null;
        private Storyboard storyboard_clock = null;

        public DiceGame(Window LoginWindow)
            : this()
        {
            var b = new Binding("Left");
            b.Converter = new MoveLeftValueConverter();
            b.ConverterParameter = LoginWindow;
            b.Mode = BindingMode.TwoWay;
            b.Source = LoginWindow;

            BindingOperations.SetBinding(this, LeftProperty, b);

            b = new Binding("Top");
            b.Converter = new MoveTopValueConverter();
            b.ConverterParameter = LoginWindow;
            b.Mode = BindingMode.TwoWay;
            b.Source = LoginWindow;

            BindingOperations.SetBinding(this, TopProperty, b);
        }

        struct sortMoney
        {
            public string s_userId;
            public int s_money;
        }

        public DiceGame()
        {
            InitializeComponent();


            //userIcon.Source = ImageDownloader.GetInstance().GetImage(Login._UserInfo.Icon);
            WebDownloader.GetInstance().DownloadFile(Login._UserInfo.Icon, IconDownloadComplete, this);

            userName_label.Content = Login._UserInfo.Nickname + " [" + Login._UserInfo.Id + "]";
            secondRoound = false;
            firstUserList = true;

            da_image.Visibility = Visibility.Hidden;
            dan_image.Visibility = Visibility.Hidden;
            xiao_image.Visibility = Visibility.Hidden;
            shuang_image.Visibility = Visibility.Hidden;

            bettingClock = new System.Windows.Threading.DispatcherTimer();
            bettingClock.Interval = new TimeSpan(0, 0, 0, 1);
            bettingClock.Tick += new EventHandler(BettingTimer_Tick);

            sameClock = new System.Windows.Threading.DispatcherTimer();
            sameClock.Interval = new TimeSpan(0, 0, 0, 0, 250);
            sameClock.Tick += new EventHandler(sameClock_Tick);

            Initsetting();
            InitSoundSetting();
            Login._ClientEngine.AttachHandler(NotifyOccured);
        }

        public void IconDownloadComplete(string filePath)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(filePath);
            bi.EndInit();

            userIcon.Source = bi;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
        }

        private void Initsetting()
        {
            
            ClearBrttingChip();

            if (pageIndex > 0)
                next_label.Visibility = Visibility.Visible;
            else
            {
                next_label.Visibility = Visibility.Hidden;
                back_label.Visibility = Visibility.Hidden;
            }
            betting_time_label.Content = "0";
            betting_time_label.Visibility = Visibility.Hidden;
            dice1 = 0;
            dice2 = 0;
            dice3 = 0;
            totalDice = 0;
            currentBettingMoney = 0;
            bettingMoney = 0;
            deltaLeft = 0;
            deltaTop = 0;
            deltaRight = 0;
            deltaBottom = 0;
            selfBettingMoney = 0;
            //myCash = 0;
            scoreResult = 0;
            roundCount = 0;
            addroundCount = 0;
            bettingTotalmoney = 0;

            self_da.Content = "0";
            self_dan.Content = "0";
            self_xiao.Content = "0";
            self_shuang.Content = "0";

            total_da.Content = "0";
            total_dan.Content = "0";
            total_xiao.Content = "0";
            total_shuang.Content = "0";

            current_money_label.Content = "0";

            betting_grid.Cursor = Cursors.Arrow;

            money_5.IsEnabled = true;
            money_10.IsEnabled = true;
            money_50.IsEnabled = true;
            money_100.IsEnabled = true;
            money_500.IsEnabled = true;

            da_img.Opacity = 0.4;
            dan_img.Opacity = 0.4;
            xiao_img.Opacity = 0.4;
            shuang_img.Opacity = 0.4;

            money_5.Visibility = Visibility.Hidden;
            money_10.Visibility = Visibility.Hidden;
            money_50.Visibility = Visibility.Hidden;
            money_100.Visibility = Visibility.Hidden;
            money_500.Visibility = Visibility.Hidden;

            self_da.Visibility = Visibility.Hidden;
            self_dan.Visibility = Visibility.Hidden;
            self_xiao.Visibility = Visibility.Hidden;
            self_shuang.Visibility = Visibility.Hidden;

            total_da.Visibility = Visibility.Hidden;
            total_dan.Visibility = Visibility.Hidden;
            total_xiao.Visibility = Visibility.Hidden;
            total_shuang.Visibility = Visibility.Hidden;

            x_Label.Visibility = Visibility.Hidden;
            x_value.Visibility = Visibility.Hidden;

            clock_image.Visibility = Visibility.Hidden;

            betting_grid.Cursor = Cursors.Arrow;

            
        }

        private void InitSoundSetting()
        {
            mediaElement1.Source = Main._MediaElement1.Source;
            mediaElement2.Source = Main._MediaElement2.Source;
            mediaElement3.Source = Main._MediaElement3.Source;
            mediaElement4.Source = Main._MediaElement4.Source;
            mediaElement5.Source = Main._MediaElement5.Source;
            mediaElement6.Source = Main._MediaElement6.Source;
            mediaElement7.Source = Main._MediaElement7.Source;
            mediaElement8.Source = Main._MediaElement8.Source;
            mediaElement9.Source = Main._MediaElement9.Source;
            mediaElement10.Source = Main._MediaElement10.Source;


        }

        private void ClearBrttingChip()
        {
            for (int i = 0; i < _Image.Count; i++)
            {
                betting_grid.Children.Remove(_Image[i]);
            }
            _Image.Clear();
        }

        private void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_TableDetail:
                    {

                        if (!(baseInfo is DiceInfo))
                            return;

                        DiceInfo diceInfo = (DiceInfo)baseInfo;

                        //if (diceInfo._RoundIndex == 2)
                        //    ++counter;
                        //if (secondRoound == false && counter == 1)
                        //    return;
                        //else
                        //    secondRoound = true;
                            

                        if (_DiceInfo != null)
                        {
                            // 이전정보와 같은 회전정보이면 무시
                            if (_DiceInfo._RoundIndex == diceInfo._RoundIndex)
                                return;
                        }
                        else
                        {
                            // 처음에는 변수 및 베팅표시를 초기화하는 처리만을 진행한다.
                            if (diceInfo._RoundIndex == 1)
                            {
                                secondRoound = true;
                                _initmlUserSocre = diceInfo.m_lUserScore;
                                _initPlayerList = diceInfo._Players;

                                InitBettingDisplay();
                            }
                            else if (diceInfo._RoundIndex == 2)
                            {
                                secondRoound = true;
                                return;
                            }
                        }

                        _DiceInfo = diceInfo;

                        // 서버로부터 받은 정보로 유저관련변수들을 갱신한다.
                        if (_DiceInfo._RoundIndex == 2)
                        {
                            int[] totalWinScore = new int[100];

                            for (int i = 0; i < _DiceInfo._Players.Count; i++)
                            {
                                UserInfo newInfo = _DiceInfo._Players[i];
                                int newScore = _DiceInfo.m_lUserWinScore[i];

                                if (_PlayerList != null)
                                {
                                    for (int k = 0; k < _PlayerList.Count; k++)
                                    {
                                        UserInfo oldInfo = _PlayerList[k];
                                        int oldScore =  _totalWinScore[k];

                                        if (newInfo.Id == oldInfo.Id)
                                        {
                                            newScore += oldScore;
                                            break;
                                        }
                                    }
                                }
                                totalWinScore[i] = newScore;
                            }

                            _totalWinScore = totalWinScore;
                           
                        }

                        _PlayerList = _DiceInfo._Players;
                        _userScore = _DiceInfo.m_bWinner;
                        _userCard = _DiceInfo.m_enCards;
                        _winScore = _DiceInfo.m_lUserWinScore;
                        _mlUserSocre = _DiceInfo.m_lUserScore;

                        // 첫표시일때에는 현재 사용자의 보유캐시를 현시하고 유저정보목록을 현시한다.
                        if (firstUserList == true)
                        {
                            self_money_label.Content = Login._UserInfo.Cash;
                            myCash = Login._UserInfo.Cash;
                            DisplayUserList(currentPage);
                        }

                        switch (_DiceInfo._RoundIndex)
                        {
                            case 0:
                                {
                                    r_index.Content = _DiceInfo._RoundIndex;
                                    userListflag = false;
                                    secondRoound = true;
                                    Initsetting();
                                    
                                }
                                break;

                            case 1:
                                {
                                    r_index.Content = _DiceInfo._RoundIndex;
                                    userListflag = false;
                                    betting_time_label.Visibility = Visibility.Visible;
                                    clock_image.Visibility = Visibility.Visible;

                                    bettingStateMoneyChip();
                                    BettingTimer();
                                }
                                break;

                            case 2:
                                {
                                    r_index.Content = _DiceInfo._RoundIndex;
                                    userListflag = true;
                                    StopBetting();
                                    for (int i = 0; i < _userScore.Length; i++)
                                    {
                                        if (_userScore[i] == 1)
                                            addroundCount = i + 1;
                                    }
                                    
                                    VictoryResult(_userCard);
                                    displayScore(_winScore);

                                }
                                break;
                        }

                    }
                    break;
                case NotifyType.Reply_Betting:
                    {
                        Thread.Sleep(10);
                        _BettingInfo = (BettingInfo)baseInfo;
                        BettingResult(_BettingInfo);
                    }
                    break;

            }
        }

        // 게임결과(대,소, 쌍, 단의 점수)를 표시한다.
        private void displayScore(int[] m_lScore)
        {
            for (int k = 0; k < _PlayerList.Count; k++)
            {
                if (_PlayerList[k].Id == Login._UserInfo.Id)
                {
                    // 현재 로그인한 유저정보를 얻는다.
                    scoreResult = m_lScore[k];

                    // 베팅점수가 있을 경우 점수표시라벨의 색갈을 노란색으로 한다.
                    if (scoreResult > 0)
                    {
                        markDisplay.Foreground = new SolidColorBrush(Colors.Yellow);
                        markDisplay.Content = "+" + m_lScore[k];

                        da_label.Foreground = new SolidColorBrush(Colors.Yellow);
                        xiao_label.Foreground = new SolidColorBrush(Colors.Yellow);
                        shuang_label.Foreground = new SolidColorBrush(Colors.Yellow);
                        dan_label.Foreground = new SolidColorBrush(Colors.Yellow);


                        if (_winScore[k] > 0)
                            x_value.Content = "+" + _winScore[k];
                        if (_winScore[k] == 0)
                            x_value.Content = 0;
                    }
                    else
                    {
                        markDisplay.Foreground = new SolidColorBrush(Colors.Black);
                        markDisplay.Content = m_lScore[k];

                        da_label.Foreground = new SolidColorBrush(Colors.Black);
                        xiao_label.Foreground = new SolidColorBrush(Colors.Black);
                        shuang_label.Foreground = new SolidColorBrush(Colors.Black);
                        dan_label.Foreground = new SolidColorBrush(Colors.Black);


                        if (_winScore[k] < 0)
                            x_value.Content = _winScore[k];
                        if (_winScore[k] == 0)
                            x_value.Content = 0;
                    }

                    if (_mlUserSocre[k, 0] != 0)
                    {
                        if (_userScore[0] == 1)
                            da_label.Content = "+" + _mlUserSocre[k, 0];
                        else
                            da_label.Content = "-" + _mlUserSocre[k, 0];
                    }
                    else
                        da_label.Content = 0;

                    if (_mlUserSocre[k, 1] != 0)
                    {
                        if (_userScore[1] == 1)
                            xiao_label.Content = "+" + _mlUserSocre[k, 1];
                        else
                            xiao_label.Content = "-" + _mlUserSocre[k, 1];
                    }
                    else
                        xiao_label.Content = 0;

                    if (_mlUserSocre[k, 2] != 0)
                    {
                        if (_userScore[2] == 1)
                            dan_label.Content = "+" + _mlUserSocre[k, 2];
                        else
                            dan_label.Content = "-" + _mlUserSocre[k, 2];
                    }
                    else
                        dan_label.Content = 0;

                    if (_mlUserSocre[k, 3] != 0)
                    {
                        if (_userScore[3] == 1)
                            shuang_label.Content = "+" + _mlUserSocre[k, 3];
                        else
                            shuang_label.Content = "-" + _mlUserSocre[k, 3];
                    }
                    else
                        shuang_label.Content = 0;

                    score = score + m_lScore[k];
                    myCash = _PlayerList[k].Cash;

                }
            }

        }

        // 오른쪽밑단 유저리스트패널에 게임을 놀고있는 유저들의 회전점수를 표시한다.
        private void DisplayUserList(int _currentPage)
        {
            if (userListflag == true)
            {
                s_totalScoreList = new sortMoney[_PlayerList.Count];

                for (int k = 0; k < _PlayerList.Count; k++)
                {
                    s_totalScoreList[k] = new sortMoney()
                    {
                        s_userId = _PlayerList[k].Id,
                        s_money = _winScore[k]
                    };
                }

                Array.Sort<sortMoney>(s_totalScoreList, (x, y) => y.s_money.CompareTo(x.s_money));
            }

            firstUserList = false;
            stackPanel1.Children.Clear();

            // 한페이지에 5명의 정보를 표시한다.
            pageIndex = (int)(_DiceInfo._Players.Count / 5);

            total_member.Content = "总会员数:" + s_totalScoreList.Length;

            int toIndex = _currentPage * 5;
            int fromIndex = toIndex + 5;

            if (s_totalScoreList.Length == 0)
                return;

            if (fromIndex <= s_totalScoreList.Length)
            {
                for (int i = toIndex; i < fromIndex; i++)
                {
                    Grid initGrid = new Grid();
                    initGrid.VerticalAlignment = VerticalAlignment.Top;

                    Label inituserId = new Label();
                    inituserId.Margin = new Thickness(2, 0, 70, 0);

                    if (s_totalScoreList[i].s_money <= 0)
                        inituserId.Foreground = new SolidColorBrush(Colors.Black);
                    else
                        inituserId.Foreground = new SolidColorBrush(Colors.Brown);

                    inituserId.Content = s_totalScoreList[i].s_userId;

                    Label inituserScore = new Label();
                    inituserScore.Margin = new Thickness(75, 0, 2, 0);

                    if (s_totalScoreList[i].s_money <= 0)
                        inituserScore.Foreground = new SolidColorBrush(Colors.Black);
                    else
                        inituserScore.Foreground = new SolidColorBrush(Colors.Red);

                    inituserScore.Content = s_totalScoreList[i].s_money;

                    initGrid.Children.Add(inituserId);
                    initGrid.Children.Add(inituserScore);
                    stackPanel1.Children.Add(initGrid);
                }
            }
            else
            {
                for (int i = toIndex; i < _PlayerList.Count; i++)
                {
                    Grid initGrid = new Grid();
                    initGrid.VerticalAlignment = VerticalAlignment.Top;
                    Label inituserId = new Label();
                    inituserId.Margin = new Thickness(2, 0, 70, 0);
                    if (s_totalScoreList[i].s_money <= 0)
                        inituserId.Foreground = new SolidColorBrush(Colors.Black);
                    else
                        inituserId.Foreground = new SolidColorBrush(Colors.Brown);
                    inituserId.Content = s_totalScoreList[i].s_userId;

                    Label inituserScore = new Label();
                    inituserScore.Margin = new Thickness(75, 0, 2, 0);
                    if (s_totalScoreList[i].s_money <= 0)
                        inituserScore.Foreground = new SolidColorBrush(Colors.Black);
                    else
                        inituserScore.Foreground = new SolidColorBrush(Colors.Red);
                    inituserScore.Content = s_totalScoreList[i].s_money;


                    initGrid.Children.Add(inituserId);
                    initGrid.Children.Add(inituserScore);
                    stackPanel1.Children.Add(initGrid);
                }
            }

        }

        // 下一页단추의 사건처리부
        private void next_label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            back_label.Visibility = Visibility.Visible;

            ++currentPage;

            if (pageIndex + 1 > currentPage)
                DisplayUserList(currentPage);
            else if (pageIndex + 1 == currentPage)
            {
                DisplayUserList(currentPage);
                next_label.Visibility = Visibility.Hidden;
            }
            else
            {
                currentPage = pageIndex + 1;
                next_label.Visibility = Visibility.Hidden;
            }

        }

        // 前一页단추의 사건처리부
        private void back_label_MouseDown(object sender, MouseButtonEventArgs e)
        {

            next_label.Visibility = Visibility.Visible;
            --currentPage;
            if (currentPage > 0)
                DisplayUserList(currentPage);
            else if (currentPage == 0)
            {
                DisplayUserList(currentPage);
                back_label.Visibility = Visibility.Hidden;
            }
            else
            {
                currentPage = 0;
                back_label.Visibility = Visibility.Hidden;
            }

        }

        public void SetGameInfo(GameInfo gameInfo)
        {
            _GameInfo = gameInfo;
        }        
        

        //private void CurrentPageList(List<int> c_userList)
        //{
        //    //int toPage = currentPage * 10;
        //    //int fromPage = (int)(c_userList.Count % 10) + toPage * 10;
        //    stackPanel1.Children.Clear();
        //    for (int i = 0; i < _PlayerList.Count; i++)
        //    {
        //        Grid inserGrid = new Grid();
        //        Label userId = new Label();
        //        userId.Margin = new Thickness(2, 2, 70, 2);
        //        userId.Content = _PlayerList[i].Id;
        //        Label userScore = new Label();
        //        userScore.Margin = new Thickness(75, 2, 2, 2);
        //        userScore.Content = _winScore[i];//Convert.ToInt32(userScore.Content) + c_userList[j];

        //        inserGrid.Children.Add(userId);
        //        inserGrid.Children.Add(userScore);
        //        stackPanel1.Children.Add(inserGrid);
        //    }
        //}       

        // 베팅을 시작한다.
        private void BettingTimer()
        {
            betting_time_label.Content = "0";

            _bettingTime = DiceDefine.BET_TIME - _DiceInfo._RoundDelayTime;

            betting_time_label.Content = _bettingTime;

            bettingClock.Start();
        }

        // 베팅시간을 현시하고 상태에 따라 종소리를 울린다. 베팅시간이 끝나면 베팅중지상태로 한다.
        void BettingTimer_Tick(object sender, EventArgs e)
        {
            state_label.Content = "游戏开始.."; // 게임시작

            int bettingContentTime = Convert.ToInt32(betting_time_label.Content);
            bettingContentTime--;
            

            if (bettingContentTime == 60)
            {
                storyboard_clock = (Storyboard)this.Resources["clock_Storyboard"];
                storyboard_clock.RepeatBehavior = new RepeatBehavior(6);
                storyboard_clock.Begin(this);

                if (soundState == true)
                {
                    mediaElement1.Stop();
                    mediaElement1.Play();
                }
            }
            if (bettingContentTime == 15 || bettingContentTime == 55)
            {              
                storyboard_clock = (Storyboard)this.Resources["clock_Storyboard"];
                storyboard_clock.RepeatBehavior = new RepeatBehavior(6);
                storyboard_clock.Begin(this);
                
                if (soundState == true)
                {
                    mediaElement2.Stop();
                    mediaElement2.Play();
                }
            }

            if (bettingContentTime < 10)
                betting_time_label.Content = "0" + bettingContentTime.ToString();
            else
                betting_time_label.Content = bettingContentTime.ToString();

            if (bettingContentTime < 0)
            {
                StopBetting();
                return;
            }
        }

        // 현재상태를 베팅상태에서 대기상태로 설정한다.
        void StopBetting()
        {
            state_label.Content = "游戏结束.."; // 게임끝

            betting_time_label.Content = "0";

            betting_time_label.Visibility = Visibility.Hidden;
            clock_image.Visibility = Visibility.Hidden;

            money_5.Visibility = Visibility.Hidden;
            money_10.Visibility = Visibility.Hidden;
            money_50.Visibility = Visibility.Hidden;
            money_100.Visibility = Visibility.Hidden;
            money_500.Visibility = Visibility.Hidden;

            if (bettingClock.IsEnabled == false)
                return;

            bettingClock.Stop();
        }

        void TimerClock_Tick(object sender, EventArgs e)
        {
            state_label.Content = "等待结果.";  // 결과대기중..

            if (soundState == true)
            {
                mediaElement3.Stop();
                mediaElement3.Play();
            }

            bettingClock.Stop();
            bettingClock = null;
        }

        // 베팅상태를 설정한다.
        private void bettingStateMoneyChip()
        {
            da_img.Opacity = 1;
            dan_img.Opacity = 1;
            xiao_img.Opacity = 1;
            shuang_img.Opacity = 1;

            money_5.Visibility = Visibility.Visible;
            money_10.Visibility = Visibility.Visible;
            money_50.Visibility = Visibility.Visible;
            money_100.Visibility = Visibility.Visible;
            money_500.Visibility = Visibility.Visible;
        }

        // 현재 베팅금액총액에 해당한 칩을 그린다.
        private void InitBettingDisplay()
        {
            int daSum = 0;
            int xiaoSum = 0;
            int danSum = 0;
            int shuangSum = 0;

            for (int i = 0; i < _initPlayerList.Count; i++)
            {
                daSum = daSum + _initmlUserSocre[i, 0];
                xiaoSum = xiaoSum + _initmlUserSocre[i, 1];                
                danSum = danSum + _initmlUserSocre[i, 2];
                shuangSum = shuangSum + _initmlUserSocre[i, 3];
            }

            if (daSum > 0)
            {
                total_da.Visibility = Visibility.Visible;
                total_da.Content = Convert.ToInt32(total_da.Content) + daSum;

                initBettingMoneyView(daSum, 0);
            }
            if (xiaoSum > 0)
            {
                total_xiao.Visibility = Visibility.Visible;
                total_xiao.Content = Convert.ToInt32(total_xiao.Content) + xiaoSum;

                initBettingMoneyView(xiaoSum, 1);
            }
            if (shuangSum > 0)
            {
                total_shuang.Visibility = Visibility.Visible;
                total_shuang.Content = Convert.ToInt32(total_shuang.Content) + shuangSum;

                initBettingMoneyView(shuangSum, 2);
            }
            if (danSum > 0)
            {
                total_dan.Visibility = Visibility.Visible;
                total_dan.Content = Convert.ToInt32(total_dan.Content) + danSum;

                initBettingMoneyView(danSum, 3);
            }

        }

        // score에 해당한 판에 칩을 그린다.
        private void initBettingMoneyView(int total, int score)
        {
            if (total >= 500)
            {
                while (total >= 500)
                {
                    Thread.Sleep(10);

                    bettingMoney = 500;
                    if (score == 0)
                    {
                        BettingPosition(0);
                        BettingAreaDaDisplay();
                    }
                    else if (score == 1)
                    {
                        BettingPosition(1);
                        BettingAreaXiaoDisplay();
                    }
                    else if (score == 2)
                    {
                        BettingPosition(2);
                        BettingAreaDanDisplay();
                    }
                    else
                    {
                        BettingPosition(3);
                        BettingShuangDanDisplay();
                    }

                    total = total - 500;

                }
            }
            if (total >= 100)
            {
                while (total >= 100)
                {
                    Thread.Sleep(10);

                    bettingMoney = 100;
                    if (score == 0)
                    {
                        BettingPosition(0);
                        BettingAreaDaDisplay();
                    }
                    else if (score == 1)
                    {
                        BettingPosition(1);
                        BettingAreaXiaoDisplay();
                    }
                    else if (score == 2)
                    {
                        BettingPosition(2);
                        BettingAreaDanDisplay();
                    }
                    else
                    {
                        BettingPosition(3);
                        BettingShuangDanDisplay();
                    }

                    total = total - 100;

                }
            }
            if (total >= 50)
            {
                while (total >= 50)
                {
                    Thread.Sleep(10);

                    bettingMoney = 50;
                    if (score == 0)
                    {
                        BettingPosition(0);
                        BettingAreaDaDisplay();
                    }
                    else if (score == 1)
                    {
                        BettingPosition(1);
                        BettingAreaXiaoDisplay();
                    }
                    else if (score == 2)
                    {
                        BettingPosition(2);
                        BettingAreaDanDisplay();
                    }
                    else
                    {
                        BettingPosition(3);
                        BettingShuangDanDisplay();
                    }

                    total = total - 50;

                }
            }
            if (total >= 10)
            {
                while (total >= 10)
                {
                    Thread.Sleep(10);

                    bettingMoney = 10;
                    if (score == 0)
                    {
                        BettingPosition(0);
                        BettingAreaDaDisplay();
                    }
                    else if (score == 1)
                    {
                        BettingPosition(1);
                        BettingAreaXiaoDisplay();
                    }
                    else if (score == 2)
                    {
                        BettingPosition(2);
                        BettingAreaDanDisplay();
                    }
                    else
                    {
                        BettingPosition(3);
                        BettingShuangDanDisplay();
                    }

                    total = total - 10;

                }
            }
            if (total >= 5)
            {
                while (total >= 5)
                {
                    Thread.Sleep(10);

                    bettingMoney = 5;
                    if (score == 0)
                    {
                        BettingPosition(0);
                        BettingAreaDaDisplay();
                    }
                    else if (score == 1)
                    {
                        BettingPosition(1);
                        BettingAreaXiaoDisplay();
                    }
                    else if (score == 2)
                    {
                        BettingPosition(2);
                        BettingAreaDanDisplay();
                    }
                    else
                    {
                        BettingPosition(3);
                        BettingShuangDanDisplay();
                    }

                    total = total - 5;

                }
            }
           
        }

        private DateTime _prevTime = DateTime.Now;

        // 베팅결과를 반영한다.
        private void BettingResult(BettingInfo bettingInfo)
        {
            if (soundState == true)
            {
                if (( DateTime.Now - _prevTime ).TotalMilliseconds >= 200)
                {
                    _prevTime = DateTime.Now;
                    mediaElement4.Stop();
                    mediaElement4.Play();
                }
            }
           
            bettingMoney = bettingInfo._Score;

            if (bettingMoney != 0)
            {
                switch (bettingInfo._Area)
                {
                    case 0:
                        {
                            total_da.Visibility = Visibility.Visible;
                            total_da.Content = Convert.ToInt32(total_da.Content) + bettingInfo._Score;

                            BettingPosition(0);
                            BettingAreaDaDisplay();
                        }
                        break;
                    case 1:
                        {
                            total_xiao.Visibility = Visibility.Visible;
                            total_xiao.Content = Convert.ToInt32(total_xiao.Content) + bettingInfo._Score;

                            BettingPosition(1);                            
                            BettingAreaXiaoDisplay();
                        }
                        break;
                    case 2:
                        {
                            total_dan.Visibility = Visibility.Visible;
                            total_dan.Content = Convert.ToInt32(total_dan.Content) + bettingInfo._Score;

                            BettingPosition(2);                            
                            BettingAreaDanDisplay();
                        }
                        break;
                    case 3:
                        {
                            total_shuang.Visibility = Visibility.Visible;
                            total_shuang.Content = Convert.ToInt32(total_shuang.Content) + bettingInfo._Score;

                            BettingPosition(3);                            
                            BettingShuangDanDisplay();
                        }
                        break;
                }
            }
        }

        // 칩을 그릴 위치를 랜덤발생하여 얻는다.
        private void BettingPosition(int position)
        {
            Random random = new Random();
            switch (position)
            {
                case 0:
                    {
                        deltaLeft = random.Next(70, 140);
                        deltaTop = random.Next(20, 75);
                    }
                    break;
                case 1:
                    {
                        deltaLeft = random.Next(190, 270);
                        deltaTop = random.Next(20, 75);
                    }
                    break;
                case 2:
                    {
                        deltaLeft = random.Next(60, 120);
                        deltaTop = random.Next(120, 180);
                    }
                    break;
                case 3:
                    {
                        deltaLeft = random.Next(180, 270);
                        deltaTop = random.Next(120, 180);
                    }
                    break;
            }
        }

        // bettingMoney값에 해당한 칩을 (大)판에 그린다.
        private void BettingAreaDaDisplay()
        {
            deltaRight = 325 - deltaLeft - 34;
            deltaBottom = 218 - deltaTop - 33;

            Image daImage = new Image();
            daImage.Margin = new Thickness(deltaLeft, deltaTop, deltaRight, deltaBottom);
            daImage.Width = 20;
            daImage.Height = 17;
            daImage.Name = "_da";

            BitmapImage da_bit = new BitmapImage();
            da_bit.BeginInit();

            switch (bettingMoney)
            {
                case 5:
                    da_bit.UriSource = new Uri("/Resources;component/image/5.png", UriKind.RelativeOrAbsolute);
                    break;
                case 10:
                    da_bit.UriSource = new Uri("/Resources;component/image/10.png", UriKind.RelativeOrAbsolute);
                    break;
                case 50:
                    da_bit.UriSource = new Uri("/Resources;component/image/50.png", UriKind.RelativeOrAbsolute);
                    break;
                case 100:
                    da_bit.UriSource = new Uri("/Resources;component/image/100.png", UriKind.RelativeOrAbsolute);
                    break;
                case 500:
                    da_bit.UriSource = new Uri("/Resources;component/image/500.png", UriKind.RelativeOrAbsolute);
                    break;
            }

            da_bit.EndInit();

            daImage.Source = da_bit;
            
            _Image.Add(daImage);
            
            betting_grid.Children.Add(daImage);
        }

        // bettingMoney값에 해당한 칩을 (小)판에 그린다.
        private void BettingAreaXiaoDisplay()
        {
            deltaRight = 325 - deltaLeft - 34;
            deltaBottom = 218 - deltaTop - 33;

            Image xiaoImage = new Image();
            xiaoImage.Margin = new Thickness(deltaLeft, deltaTop, deltaRight, deltaBottom);
            xiaoImage.Width = 20;
            xiaoImage.Height = 17;
            xiaoImage.Name = "_xiao";

            BitmapImage xiao_bit = new BitmapImage();
            xiao_bit.BeginInit();

            switch (bettingMoney)
            {
                case 5:
                    xiao_bit.UriSource = new Uri("/Resources;component/image/5.png", UriKind.RelativeOrAbsolute);
                    break;
                case 10:
                    xiao_bit.UriSource = new Uri("/Resources;component/image/10.png", UriKind.RelativeOrAbsolute);
                    break;
                case 50:
                    xiao_bit.UriSource = new Uri("/Resources;component/image/50.png", UriKind.RelativeOrAbsolute);
                    break;
                case 100:
                    xiao_bit.UriSource = new Uri("/Resources;component/image/100.png", UriKind.RelativeOrAbsolute);
                    break;
                case 500:
                    xiao_bit.UriSource = new Uri("/Resources;component/image/500.png", UriKind.RelativeOrAbsolute);
                    break;
            }

            xiao_bit.EndInit();
            xiaoImage.Source = xiao_bit;
            _Image.Add(xiaoImage);
            betting_grid.Children.Add(xiaoImage);
        }

        // bettingMoney값에 해당한 칩을 (单)판에 그린다.
        private void BettingAreaDanDisplay()
        {
            deltaRight = 325 - deltaLeft - 34;
            deltaBottom = 218 - deltaTop - 33;

            Image danImage = new Image();
            danImage.Margin = new Thickness(deltaLeft, deltaTop, deltaRight, deltaBottom);
            danImage.Width = 20;
            danImage.Height = 17;
            danImage.Name = "_dan";

            BitmapImage dan_bit = new BitmapImage();
            dan_bit.BeginInit();

            switch (bettingMoney)
            {
                case 5:
                    dan_bit.UriSource = new Uri("/Resources;component/image/5.png", UriKind.RelativeOrAbsolute);
                    break;
                case 10:
                    dan_bit.UriSource = new Uri("/Resources;component/image/10.png", UriKind.RelativeOrAbsolute);
                    break;
                case 50:
                    dan_bit.UriSource = new Uri("/Resources;component/image/50.png", UriKind.RelativeOrAbsolute);
                    break;
                case 100:
                    dan_bit.UriSource = new Uri("/Resources;component/image/100.png", UriKind.RelativeOrAbsolute);
                    break;
                case 500:
                    dan_bit.UriSource = new Uri("/Resources;component/image/500.png", UriKind.RelativeOrAbsolute);
                    break;
            }

            dan_bit.EndInit();
            danImage.Source = dan_bit;

            _Image.Add(danImage);
            betting_grid.Children.Add(danImage);
        }

        // bettingMoney값에 해당한 칩을 (双)판에 그린다.
        private void BettingShuangDanDisplay()
        {
            deltaRight = 325 - deltaLeft - 34;
            deltaBottom = 218 - deltaTop - 33;

            Image shuangImage = new Image();
            shuangImage.Margin = new Thickness(deltaLeft, deltaTop, deltaRight, deltaBottom);
            shuangImage.Width = 20;
            shuangImage.Height = 17;
            shuangImage.Name = "_shuang";

            BitmapImage shaung_bit = new BitmapImage();
            shaung_bit.BeginInit();

            switch (bettingMoney)
            {
                case 5:
                    shaung_bit.UriSource = new Uri("/Resources;component/image/5.png", UriKind.RelativeOrAbsolute);
                    break;
                case 10:
                    shaung_bit.UriSource = new Uri("/Resources;component/image/10.png", UriKind.RelativeOrAbsolute);
                    break;
                case 50:
                    shaung_bit.UriSource = new Uri("/Resources;component/image/50.png", UriKind.RelativeOrAbsolute);
                    break;
                case 100:
                    shaung_bit.UriSource = new Uri("/Resources;component/image/100.png", UriKind.RelativeOrAbsolute);
                    break;
                case 500:
                    shaung_bit.UriSource = new Uri("/Resources;component/image/500.png", UriKind.RelativeOrAbsolute);
                    break;
            }

            shaung_bit.EndInit();
            shuangImage.Source = shaung_bit;

            _Image.Add(shuangImage);
            betting_grid.Children.Add(shuangImage);
        }

        // 어느 칩을 선택했는가에 따라 마우스카셜의 이미지를 바꾼다.
        #region MoneyBt_Click Event

        private void money_5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentBettingMoney = 5;
            selfBettingMoney = 5;
            Stream cursorStream5 =
                Application.GetResourceStream(new Uri("/Resources;component/image/SCORE_5.cur", UriKind.RelativeOrAbsolute)).Stream;
            betting_grid.Cursor = new Cursor(cursorStream5);
        }
       
        private void money_10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentBettingMoney = 10;
            selfBettingMoney = 10;
            Stream cursorStream10 =
                Application.GetResourceStream(new Uri("/Resources;component/image/SCORE_10.cur", UriKind.RelativeOrAbsolute)).Stream;
            betting_grid.Cursor = new Cursor(cursorStream10);
        }

        private void money_50_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentBettingMoney = 50;
            selfBettingMoney = 50;
            Stream cursorStream50 =
                Application.GetResourceStream(new Uri("/Resources;component/image/SCORE_50.cur", UriKind.RelativeOrAbsolute)).Stream;
            betting_grid.Cursor = new Cursor(cursorStream50);
        }

        private void money_100_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentBettingMoney = 100;
            selfBettingMoney = 100;
            Stream cursorStream100 =
                Application.GetResourceStream(new Uri("/Resources;component/image/SCORE_100.cur", UriKind.RelativeOrAbsolute)).Stream;
            betting_grid.Cursor = new Cursor(cursorStream100);
        }

        private void money_500_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentBettingMoney = 500;
            selfBettingMoney = 500;
            Stream cursorStream500 =
                Application.GetResourceStream(new Uri("/Resources;component/image/SCORE_500.cur", UriKind.RelativeOrAbsolute)).Stream;
            betting_grid.Cursor = new Cursor(cursorStream500);
        }

        #endregion

        #region BettingImagePan Event

        // (双)판의 마우스사건처리부
        void shuang_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_DiceInfo._RoundIndex == 1 && betting_grid.Cursor != Cursors.Arrow)
                {
                    // 현재 총베팅금액에 선택된 칩에 해당한 금액을 더한다.
                    bettingTotalmoney = bettingTotalmoney + currentBettingMoney;
                    
                    // 현재 보유금액이 베팅금액보다 큰 경우에만 칩을 표시한다.
                    if (bettingTotalmoney < Convert.ToInt32(self_money_label.Content) && bettingTotalmoney < 9999)
                    {
                        BettingPosition(3);

                        _BettingInfo._Area = 3;
                        _BettingInfo._Score = currentBettingMoney;

                        // 현재 보유금액을 현시한다.
                        self_shuang.Visibility = Visibility.Visible;
                        self_shuang.Content = Convert.ToInt32(self_shuang.Content) + selfBettingMoney;

                        // 현재 베팅금액을 현시한다.
                        current_money_label.Content = Convert.ToInt32(current_money_label.Content) + currentBettingMoney;

                        // 서버에 현재 베팅정보를 전송한다.
                        Login._ClientEngine.Send(NotifyType.Request_Betting, _BettingInfo);
                    }
                    else
                    {
                        // 베팅금액이 만웬을 넘어나는 경우
                        if (bettingTotalmoney > 9999)
                        {
                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "Overflow Betting Money.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                        }
                        // 현재 보유금액이 베팅금액보다 작은 경우
                        else
                        {                            
                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "帐号元宝不足.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                        }

                        bettingTotalmoney = bettingTotalmoney - currentBettingMoney;
                    }
                }
                else
                {
                    if (soundState == true)
                    {
                        mediaElement5.Stop();
                        mediaElement5.Play();
                        //mediaElement5.Position = TimeSpan.Zero;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        void dan_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_DiceInfo._RoundIndex == 1 && betting_grid.Cursor != Cursors.Arrow)
                {
                    bettingTotalmoney = bettingTotalmoney + currentBettingMoney;

                    if (bettingTotalmoney < Convert.ToInt32(self_money_label.Content) && bettingTotalmoney < 9999)
                     {
                         BettingPosition(2);

                         _BettingInfo._Area = 2;
                         _BettingInfo._Score = currentBettingMoney;
                         self_dan.Visibility = Visibility.Visible;
                         self_dan.Content = Convert.ToInt32(self_dan.Content) + selfBettingMoney;
                         current_money_label.Content = Convert.ToInt32(current_money_label.Content) + currentBettingMoney;
                         Login._ClientEngine.Send(NotifyType.Request_Betting, _BettingInfo);
                     }
                     else
                     {
                         if (bettingTotalmoney > 9999)
                         {                             
                             TempWindowForm tempWindowForm = new TempWindowForm();
                             QQMessageBox.Show(tempWindowForm, "Overflow Betting Money.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                         }
                         else
                         {                             
                             TempWindowForm tempWindowForm = new TempWindowForm();
                             QQMessageBox.Show(tempWindowForm, "帐号元宝不足.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                         }
                         bettingTotalmoney = bettingTotalmoney - currentBettingMoney;
                     }
                }
                else
                {
                    if (soundState == true)
                    {
                        mediaElement5.Stop();
                        mediaElement5.Play();
                        //mediaElement5.Position = TimeSpan.Zero;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        void xiao_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_DiceInfo._RoundIndex == 1 && betting_grid.Cursor != Cursors.Arrow)
                {
                    bettingTotalmoney = bettingTotalmoney + currentBettingMoney;

                    if (bettingTotalmoney < Convert.ToInt32(self_money_label.Content) && bettingTotalmoney < 9999)
                    {
                        BettingPosition(1);

                        _BettingInfo._Area = 1;
                        _BettingInfo._Score = currentBettingMoney;
                        self_xiao.Visibility = Visibility.Visible;
                        self_xiao.Content = Convert.ToInt32(self_xiao.Content) + selfBettingMoney;
                        current_money_label.Content = Convert.ToInt32(current_money_label.Content) + currentBettingMoney;
                        Login._ClientEngine.Send(NotifyType.Request_Betting, _BettingInfo);
                    }
                    else
                    {
                        if (bettingTotalmoney > 9999)
                        {
                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "Overflow Betting Money.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                        }
                        else
                        {                            
                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "帐号元宝不足.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                        }
                        bettingTotalmoney = bettingTotalmoney - currentBettingMoney;
                    }
                }
                else
                {
                    if (soundState == true)
                    {
                        mediaElement5.Stop();
                        mediaElement5.Play();
                        //mediaElement5.Position = TimeSpan.Zero;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        void da_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_DiceInfo._RoundIndex == 1 && betting_grid.Cursor != Cursors.Arrow)
                {

                    bettingTotalmoney = bettingTotalmoney + currentBettingMoney;

                    if (bettingTotalmoney < Convert.ToInt32(self_money_label.Content) && bettingTotalmoney < 9999)
                    {
                        BettingPosition(0);

                        _BettingInfo._Area = 0;
                        _BettingInfo._Score = currentBettingMoney;
                        self_da.Visibility = Visibility.Visible;
                        self_da.Content = Convert.ToInt32(self_da.Content) + selfBettingMoney;
                        current_money_label.Content = Convert.ToInt32(current_money_label.Content) + currentBettingMoney;
                        Login._ClientEngine.Send(NotifyType.Request_Betting, _BettingInfo);
                    }
                    else
                    {
                        if (bettingTotalmoney > 9999)
                        {
                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "Overflow Betting Money.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                        }
                        else
                        {                            
                            TempWindowForm tempWindowForm = new TempWindowForm();
                            QQMessageBox.Show(tempWindowForm, "帐号元宝不足.", "提示", QQMessageBoxIcon.OK, QQMessageBoxButtons.OK);
                        }

                        bettingTotalmoney = bettingTotalmoney - currentBettingMoney;
                    }
                }
                else
                {
                    if (soundState == true)
                    {
                        mediaElement5.Stop();
                        mediaElement5.Play();
                        //mediaElement5.Position = TimeSpan.Zero;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        #endregion        

        // 주사위의 결과상태를 그린다.
        private void VictoryResult(int[] card)
        {
            betting_grid.Cursor = Cursors.Arrow;
            money_5.IsEnabled = false;
            money_10.IsEnabled = false;
            money_50.IsEnabled = false;
            money_100.IsEnabled = false;
            money_500.IsEnabled = false;

            dice1 = card[0];
            dice2 = card[1];
            dice3 = card[2];

            //dice1 = 6;
            //dice2 = 6;
            //dice3 = 6;
            totalDice = dice1 + dice2 + dice3;

            BitmapImage diceBit1 = new BitmapImage();
            diceBit1.BeginInit();
            switch (dice1)
            {
                case 1:
                    diceBit1.UriSource = new Uri("/Resources;component/image/dice1.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 2:
                    diceBit1.UriSource = new Uri("/Resources;component/image/dice2.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 3:
                    diceBit1.UriSource = new Uri("/Resources;component/image/dice3.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 4:
                    diceBit1.UriSource = new Uri("/Resources;component/image/dice4.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 5:
                    diceBit1.UriSource = new Uri("/Resources;component/image/dice5.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 6:
                    diceBit1.UriSource = new Uri("/Resources;component/image/dice6.gif", UriKind.RelativeOrAbsolute);
                    break;
            }
            diceBit1.EndInit();
            diceImage1.Source = diceBit1;

            BitmapImage diceBit2 = new BitmapImage();
            diceBit2.BeginInit();
            switch (dice2)
            {
                case 1:
                    diceBit2.UriSource = new Uri("/Resources;component/image/dice1.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 2:
                    diceBit2.UriSource = new Uri("/Resources;component/image/dice2.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 3:
                    diceBit2.UriSource = new Uri("/Resources;component/image/dice3.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 4:
                    diceBit2.UriSource = new Uri("/Resources;component/image/dice4.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 5:
                    diceBit2.UriSource = new Uri("/Resources;component/image/dice5.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 6:
                    diceBit2.UriSource = new Uri("/Resources;component/image/dice6.gif", UriKind.RelativeOrAbsolute);
                    break;
            }
            diceBit2.EndInit();
            diceImage2.Source = diceBit2;

            BitmapImage diceBit3 = new BitmapImage();
            diceBit3.BeginInit();
            switch (dice3)
            {
                case 1:
                    diceBit3.UriSource = new Uri("/Resources;component/image/dice1.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 2:
                    diceBit3.UriSource = new Uri("/Resources;component/image/dice2.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 3:
                    diceBit3.UriSource = new Uri("/Resources;component/image/dice3.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 4:
                    diceBit3.UriSource = new Uri("/Resources;component/image/dice4.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 5:
                    diceBit3.UriSource = new Uri("/Resources;component/image/dice5.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 6:
                    diceBit3.UriSource = new Uri("/Resources;component/image/dice6.gif", UriKind.RelativeOrAbsolute);
                    break;
            }
            diceBit3.EndInit();
            diceImage3.Source = diceBit3;

            // 모든 주사위의 상태가 같게 나오는 경우
            if (dice1 == dice2 && dice2 == dice3)
            {
                x_Label.Visibility = Visibility.Visible;
                x_value.Visibility = Visibility.Visible;

                switch (dice1)
                {
                    case 1:
                        x_Label.Content = "X1:";
                        break;
                    case 2:
                        x_Label.Content = "X2:";
                        break;
                    case 3:
                        x_Label.Content = "X3:";
                        break;
                    case 4:
                        x_Label.Content = "X4:";
                        break;
                    case 5:
                        x_Label.Content = "X5:";
                        break;
                    case 6:
                        x_Label.Content = "X6:";
                        break;
                }

                Storyboard sameStoryboard = (Storyboard)this.Resources["diceStoryboard2"];
                sameStoryboard.Completed += new EventHandler(sameStoryboard_Completed);
                sameStoryboard.Begin(this);
            }
            else
            {
                Storyboard diceStoryboard = (Storyboard)this.Resources["diceStoryboard1"];
                diceStoryboard.Completed += new EventHandler(diceStoryboard_Completed);
                diceStoryboard.Begin(this);
            }
        }

        // 주사위의 상태가 다 같지 않을때
        void diceStoryboard_Completed(object sender, EventArgs e)
        {
            // 주사위의 결과상태를 반영한다.
            result_label.Content = totalDice.ToString();

            Storyboard storyboard1;
            storyboard1 = (Storyboard)this.Resources["Storyboard1"];
            storyboard1.Begin(this);

            Storyboard bigStoryBorad = null;

            if (totalDice >= 11 && totalDice <= 17)
                bigStoryBorad = (Storyboard)this.Resources["BigStoryboard"];
            else
                bigStoryBorad = (Storyboard)this.Resources["SmallStoryboard"];


            Storyboard evenStoryBoard = null;

            if (totalDice % 2 == 0)
                evenStoryBoard = (Storyboard)this.Resources["EvenStoryboard"];
            else
                evenStoryBoard = (Storyboard)this.Resources["OddStoryboard"];

            bigStoryBorad.Completed += moveClock_Tick;

            bigStoryBorad.Begin(this);
            evenStoryBoard.Begin(this);
        }

        
        void moveClock_Tick(object sender, EventArgs e)
        {
            if (secondRoound == true)
            {
                Storyboard mark_storyboard;
                mark_storyboard = (Storyboard)this.Resources["mark_Storyboard"];
                mark_storyboard.Begin(this);
            }

            if (soundState == true)
            {
                if (scoreResult > 0)
                {
                    mediaElement6.Stop();
                    mediaElement6.Play();
                }
                else if (scoreResult < 0)
                {
                    mediaElement7.Stop();
                    mediaElement7.Play();
                }
            }

            DisplayUserList(currentPage);

            // 게임에서 따거나 떼운돈을 현시한다.
            totoal_money_label.Content = score.ToString();

            // 현재 보유금액을 현시한다.
            self_money_label.Content = myCash;

            if (Main.multyChatRoom != null)
            {
                var userToolTip = new System.Windows.Controls.ToolTip();

                // 현재보유금액을 현시한다.
                Main.multyChatRoom.lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + myCash;

                // 현재 로그인한 유저닉네임과 아이디, 캐시값을 현시한다.
                userToolTip.Content = Login._UserInfo.Nickname + " " + "(" + Login._UserInfo.Id + ")" + " " + myCash;

                Main.multyChatRoom.lblCurReLoginPrice.ToolTip = userToolTip;
            }

            MoveTo(totalDice);
        }

        // 결과에 따라 칩을 이동시키는 함수
        private void MoveTo(int _total)
        {

            if (_total >= 11 && _total <= 17)
            {
                BigToSmall();
            }
            if (_total >= 4 && _total <= 10)
            {
                SmallToBig();
            }
            if (_total % 2 == 0)
            {
                OddToEven();

            }
            if (_total % 2 != 0)
            {
                EvenToOdd();
            }

            //moveClock.Stop();
            //moveClock = null;
        }

        private void BigToSmall()
        {
            for (int i = 0; i < _Image.Count; i++)
            {
                if (_Image[i].Name == "_xiao")
                {
                    if (soundState == true && bettingMoney != 0)
                    {
                        mediaElement8.Stop();
                        mediaElement8.Play();
                        
                    }

                    TranslateTransform datrans = new TranslateTransform();
                    _Image[i].RenderTransform = datrans;

                    Duration danduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation daanim2 = new DoubleAnimation(0, -130, danduration);
                    datrans.BeginAnimation(TranslateTransform.XProperty, daanim2);
                }
            }

            total_da.Visibility = Visibility.Visible;
            total_da.Content = (Convert.ToInt32(total_da.Content) + Convert.ToInt32(total_xiao.Content)).ToString();
            total_xiao.Visibility = Visibility.Hidden;
            total_xiao.Content = "0";
        }
        private void SmallToBig()
        {
            for (int i = 0; i < _Image.Count; i++)
            {
                if (_Image[i].Name == "_da")
                {
                    if (soundState == true && bettingMoney != 0)
                    {
                        mediaElement8.Stop();
                        mediaElement8.Play();
                    }

                    TranslateTransform xiaotrans = new TranslateTransform();
                    _Image[i].RenderTransform = xiaotrans;


                    Duration xiaoduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation xiaoanim2 = new DoubleAnimation(0, 130, xiaoduration);
                    xiaotrans.BeginAnimation(TranslateTransform.XProperty, xiaoanim2);
                }
            }
            total_xiao.Visibility = Visibility.Visible;
            total_xiao.Content = (Convert.ToInt32(total_da.Content) + Convert.ToInt32(total_xiao.Content)).ToString();
            total_da.Visibility = Visibility.Hidden;
            total_da.Content = "0";

        }
        private void OddToEven()
        {
            for (int i = 0; i < _Image.Count; i++)
            {
                if (_Image[i].Name == "_dan")
                {
                    if (soundState == true && bettingMoney != 0)
                    {
                        mediaElement8.Stop();
                        mediaElement8.Play();
                    }

                    TranslateTransform xiaotrans = new TranslateTransform();
                    _Image[i].RenderTransform = xiaotrans;

                    Duration xiaoduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation xiaoanim2 = new DoubleAnimation(0, 130, xiaoduration);
                    xiaotrans.BeginAnimation(TranslateTransform.XProperty, xiaoanim2);
                }
            }
            total_shuang.Visibility = Visibility.Visible;
            total_shuang.Content = (Convert.ToInt32(total_shuang.Content) + Convert.ToInt32(total_dan.Content)).ToString();
            total_dan.Visibility = Visibility.Hidden;
            total_dan.Content = "0";
        }
        private void EvenToOdd()
        {
            for (int i = 0; i < _Image.Count; i++)
            {
                if (_Image[i].Name == "_shuang")
                {
                    if (soundState == true && bettingMoney != 0)
                    {
                        mediaElement8.Stop();
                        mediaElement8.Play();
                    }

                    TranslateTransform dantrans = new TranslateTransform();
                    _Image[i].RenderTransform = dantrans;

                    Duration danduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation dananim2 = new DoubleAnimation(0, -150, danduration);
                    dantrans.BeginAnimation(TranslateTransform.XProperty, dananim2);
                }
            }
            total_dan.Visibility = Visibility.Visible;
            total_dan.Content = (Convert.ToInt32(total_shuang.Content) + Convert.ToInt32(total_dan.Content)).ToString();
            total_shuang.Visibility = Visibility.Hidden;
            total_shuang.Content = "0";
        }


        // 모든 주사위의 상태가 같을때
        void sameStoryboard_Completed(object sender, EventArgs e)
        {
            switch (dice1)
            {
                case 1:
                    welcom.Content = "x1";
                    break;
                case 2:
                    welcom.Content = "x2";
                    break;
                case 3:
                    welcom.Content = "x3";
                    break;
                case 4:
                    welcom.Content = "x4";
                    break;
                case 5:
                    welcom.Content = "x5";
                    break;
                case 6:
                    welcom.Content = "x6";
                    break;
            }
            sameClock.IsEnabled = true;

            welcom.Visibility = Visibility.Visible;

            if (soundState == true)
            {
                mediaElement10.Stop();
                mediaElement10.Play();
            }
            
            Storyboard storyboard_X;
            storyboard_X = (Storyboard)this.Resources["X_Storyboard"];
            storyboard_X.RepeatBehavior = new RepeatBehavior(6);
            storyboard_X.Completed += new EventHandler(storyboard_X_Completed);
            storyboard_X.Begin(this);
        }

        // X상태표시라벨을 안보이게한다.
        void storyboard_X_Completed(object sender, EventArgs e)
        {            
            welcom.Visibility = Visibility.Hidden;
        }
        
        void sameClock_Tick(object sender, EventArgs e)
        {

            if (soundState == true)
            {
                mediaElement9.Stop();
                mediaElement9.Play();
            }

            int indexRound = roundCount % 4;
            
            switch (indexRound)
            {
                case 0:
                    {
                        storyboard_da = (Storyboard)this.Resources["da_Storyboard"];
                        storyboard_da.RepeatBehavior = new RepeatBehavior(1);
                        storyboard_da.Begin(this);
                    }
                    break;
                case 1:
                    {
                        storyboard_xiao = (Storyboard)this.Resources["xiao_Storyboard"];
                        storyboard_xiao.RepeatBehavior = new RepeatBehavior(1);
                        storyboard_xiao.Begin(this);
                    }
                    break;
                case 2:
                    {
                        storyboard_shuang = (Storyboard)this.Resources["shuang_Storyboard"];
                        storyboard_shuang.RepeatBehavior = new RepeatBehavior(1);
                        storyboard_shuang.Begin(this);
                    }
                    break;
                case 3:
                    {
                        storyboard_dan = (Storyboard)this.Resources["dan_Storyboard"];
                        storyboard_dan.RepeatBehavior = new RepeatBehavior(1);
                        storyboard_dan.Begin(this);
                    }
                    break;
            }

            ++roundCount;

            if (roundCount >= 20 + addroundCount)
            {
                sameClock.IsEnabled = false;

                switch (addroundCount)
                {
                    case 1:
                        {
                            storyboard_da = (Storyboard)this.Resources["da_Storyboard"];
                            storyboard_da.RepeatBehavior = new RepeatBehavior(18);
                            storyboard_da.Begin(this);
                            DaVictory();
                        }
                        break;
                    case 2:
                        {
                            storyboard_xiao = (Storyboard)this.Resources["xiao_Storyboard"];
                            storyboard_xiao.RepeatBehavior = new RepeatBehavior(18);
                            storyboard_xiao.Begin(this);
                            XiaoVictory();
                        }
                        break;
                    case 3:
                        {
                            storyboard_dan = (Storyboard)this.Resources["dan_Storyboard"];
                            storyboard_dan.RepeatBehavior = new RepeatBehavior(18);
                            storyboard_dan.Begin(this);
                            DanVictory();
                        }
                        break;
                    case 4:
                        {
                            storyboard_shuang = (Storyboard)this.Resources["shuang_Storyboard"];
                            storyboard_shuang.RepeatBehavior = new RepeatBehavior(18);
                            storyboard_shuang.Begin(this);
                            ShuangVictory();
                        }
                        break;
                }
            }
          
        }

        // X상태인 경우에 (单)판에 칩이미지를 현시하는
        private void DaVictory()
        {

            dan_image.Visibility = Visibility.Visible;

            if (soundState == true && bettingMoney != 0)
            {
                mediaElement8.Stop();
                mediaElement8.Play();
                
            }
            
            for (int i = 0; i < _Image.Count; i++)
            {
                if (_Image[i].Name == "_xiao")
                {
                    TranslateTransform xiaotrans = new TranslateTransform();
                    _Image[i].RenderTransform = xiaotrans;

                    Duration xiaoduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation xiaoanim = new DoubleAnimation(0, -130, xiaoduration);
                    xiaotrans.BeginAnimation(TranslateTransform.XProperty, xiaoanim);
                }
                else if (_Image[i].Name == "_dan")
                {
                    TranslateTransform dantrans = new TranslateTransform();
                    _Image[i].RenderTransform = dantrans;

                    Duration danduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation dananim = new DoubleAnimation(0, -110, danduration);
                    dantrans.BeginAnimation(TranslateTransform.YProperty, dananim);
                }
                else if (_Image[i].Name == "_shuang")
                {
                    TranslateTransform shuangtrans = new TranslateTransform();
                    _Image[i].RenderTransform = shuangtrans;

                    Duration shuangduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation dananim1 = new DoubleAnimation(0, -110, shuangduration);
                    DoubleAnimation dananim2 = new DoubleAnimation(0, -130, shuangduration);
                    shuangtrans.BeginAnimation(TranslateTransform.YProperty, dananim1);
                    shuangtrans.BeginAnimation(TranslateTransform.XProperty, dananim2);
                }
            }

            //  (单)판에 베팅금액표시라벨에 4개의 판에 베팅한 금액을 총합을 보여주고 다른 라벨들은 숨긴다.
            total_da.Visibility = Visibility.Visible;
            total_da.Content = (Convert.ToInt32(total_da.Content) + Convert.ToInt32(total_xiao.Content) + Convert.ToInt32(total_dan.Content) + Convert.ToInt32(total_shuang.Content)).ToString();
            total_xiao.Visibility = Visibility.Hidden;
            total_dan.Visibility = Visibility.Hidden;
            total_shuang.Visibility = Visibility.Hidden;
            total_xiao.Content = "0";
            total_dan.Content = "0";
            total_shuang.Content = "0";

            if (soundState == true)
            {
                if (scoreResult > 0)
                {
                    mediaElement6.Stop();
                    mediaElement6.Play();
                }
                else if (scoreResult < 0)
                {
                    mediaElement7.Stop();
                    mediaElement7.Play();
               }
            }

            sameResultUserList();
            if (secondRoound == true)
            {
                Storyboard mark_storyboard;
                mark_storyboard = (Storyboard)this.Resources["mark_Storyboard"];
                mark_storyboard.Begin(this);
            }

        }

        private void XiaoVictory()
        {
            xiao_image.Visibility = Visibility.Visible;

            if (soundState == true && bettingMoney != 0)
            {
                mediaElement8.Stop();
                mediaElement8.Play();                
            }

            for (int i = 0; i < _Image.Count; i++)
            {
                if (_Image[i].Name == "_da")
                {
                    TranslateTransform datrans = new TranslateTransform();
                    _Image[i].RenderTransform = datrans;

                    Duration daduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation daanim = new DoubleAnimation(0, 130, daduration);
                    datrans.BeginAnimation(TranslateTransform.XProperty, daanim);
                }
                else if (_Image[i].Name == "_dan")
                {
                    TranslateTransform dantrans = new TranslateTransform();
                    _Image[i].RenderTransform = dantrans;

                    Duration danduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation dananim1 = new DoubleAnimation(0, -110, danduration);
                    DoubleAnimation dananim2 = new DoubleAnimation(0, 130, danduration);
                    dantrans.BeginAnimation(TranslateTransform.YProperty, dananim1);
                    dantrans.BeginAnimation(TranslateTransform.XProperty, dananim2);
                }
                else if (_Image[i].Name == "_shuang")
                {
                    TranslateTransform shuangtrans = new TranslateTransform();
                    _Image[i].RenderTransform = shuangtrans;

                    Duration shuangduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation dananim1 = new DoubleAnimation(0, -110, shuangduration);
                    shuangtrans.BeginAnimation(TranslateTransform.YProperty, dananim1);
                }
            }
            total_xiao.Visibility = Visibility.Visible;
            total_xiao.Content = (Convert.ToInt32(total_da.Content) + Convert.ToInt32(total_xiao.Content) + Convert.ToInt32(total_dan.Content) + Convert.ToInt32(total_shuang.Content)).ToString();
            total_da.Visibility = Visibility.Hidden;
            total_dan.Visibility = Visibility.Hidden;
            total_shuang.Visibility = Visibility.Hidden;
            total_da.Content = "0";
            total_dan.Content = "0";
            total_shuang.Content = "0";

            if (soundState == true)
            {
                if (scoreResult > 0)
                {
                    mediaElement6.Stop();
                    mediaElement6.Play();
                   
                }
                else if (scoreResult < 0)
                {
                    mediaElement7.Stop();
                    mediaElement7.Play();
                    
                }
            }
            sameResultUserList();
            if (secondRoound == true)
            {
                Storyboard mark_storyboard;
                mark_storyboard = (Storyboard)this.Resources["mark_Storyboard"];
                mark_storyboard.Begin(this);
            }

        }

        private void DanVictory()
        {
            dan_image.Visibility = Visibility.Visible;
            
            if (soundState == true && bettingMoney != 0)
            {
                mediaElement8.Stop();
                mediaElement8.Play();
            }
            for (int i = 0; i < _Image.Count; i++)
            {
                if (_Image[i].Name == "_da")
                {
                    TranslateTransform datrans = new TranslateTransform();
                    _Image[i].RenderTransform = datrans;

                    Duration daduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation daanim1 = new DoubleAnimation(0, 100, daduration);
                    DoubleAnimation daanim2 = new DoubleAnimation(0, -20, daduration);
                    datrans.BeginAnimation(TranslateTransform.YProperty, daanim1);
                    datrans.BeginAnimation(TranslateTransform.XProperty, daanim2);
                }
                else if (_Image[i].Name == "_xiao")
                {
                    TranslateTransform xiaotrans = new TranslateTransform();
                    _Image[i].RenderTransform = xiaotrans;

                    Duration xiaoduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation xiaoanim1 = new DoubleAnimation(0, 100, xiaoduration);
                    DoubleAnimation xiaoanim2 = new DoubleAnimation(0, -150, xiaoduration);
                    xiaotrans.BeginAnimation(TranslateTransform.YProperty, xiaoanim1);
                    xiaotrans.BeginAnimation(TranslateTransform.XProperty, xiaoanim2);
                }
                else if (_Image[i].Name == "_shuang")
                {
                    TranslateTransform shuangtrans = new TranslateTransform();
                    _Image[i].RenderTransform = shuangtrans;

                    Duration shuangduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation dananim1 = new DoubleAnimation(0, -150, shuangduration);
                    shuangtrans.BeginAnimation(TranslateTransform.XProperty, dananim1);
                }
            }
            total_dan.Visibility = Visibility.Visible;
            total_dan.Content = (Convert.ToInt32(total_da.Content) + Convert.ToInt32(total_xiao.Content) + Convert.ToInt32(total_dan.Content) + Convert.ToInt32(total_shuang.Content)).ToString();
            total_da.Visibility = Visibility.Hidden;
            total_xiao.Visibility = Visibility.Hidden;
            total_shuang.Visibility = Visibility.Hidden;
            total_da.Content = "0";
            total_xiao.Content = "0";
            total_shuang.Content = "0";

            if (soundState == true)
            {
                if (scoreResult > 0)
                {
                    mediaElement6.Stop();
                    mediaElement6.Play();
                    
                }
                else if (scoreResult < 0)
                {
                    mediaElement7.Stop();
                    mediaElement7.Play();
                    
                }
            }
            sameResultUserList();
            if (secondRoound == true)
            {
                Storyboard mark_storyboard;
                mark_storyboard = (Storyboard)this.Resources["mark_Storyboard"];
                mark_storyboard.Begin(this);
            }

        }

        private void ShuangVictory()
        {
            shuang_image.Visibility = Visibility.Visible;
            
            if (soundState == true && bettingMoney != 0)
            {
                mediaElement8.Stop();
                mediaElement8.Play();
            }
            for (int i = 0; i < _Image.Count; i++)
            {
                if (_Image[i].Name == "_da")
                {
                    TranslateTransform datrans = new TranslateTransform();
                    _Image[i].RenderTransform = datrans;

                    Duration daduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation daanim1 = new DoubleAnimation(0, 100, daduration);
                    DoubleAnimation daanim2 = new DoubleAnimation(0, 130, daduration);
                    datrans.BeginAnimation(TranslateTransform.YProperty, daanim1);
                    datrans.BeginAnimation(TranslateTransform.XProperty, daanim2);
                }
                else if (_Image[i].Name == "_xiao")
                {
                    TranslateTransform xiaotrans = new TranslateTransform();
                    _Image[i].RenderTransform = xiaotrans;

                    Duration xiaoduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation xiaoanim1 = new DoubleAnimation(0, 100, xiaoduration);
                    xiaotrans.BeginAnimation(TranslateTransform.YProperty, xiaoanim1);
                }
                else if (_Image[i].Name == "_dan")
                {
                    TranslateTransform shuangtrans = new TranslateTransform();
                    _Image[i].RenderTransform = shuangtrans;

                    Duration shuangduration = new Duration(new TimeSpan(0, 0, 0, 1, 0));
                    DoubleAnimation dananim1 = new DoubleAnimation(0, 130, shuangduration);
                    shuangtrans.BeginAnimation(TranslateTransform.XProperty, dananim1);
                }
            }
            total_shuang.Visibility = Visibility.Visible;
            total_shuang.Content = (Convert.ToInt32(total_da.Content) + Convert.ToInt32(total_xiao.Content) + Convert.ToInt32(total_dan.Content) + Convert.ToInt32(total_shuang.Content)).ToString();
            total_da.Visibility = Visibility.Hidden;
            total_xiao.Visibility = Visibility.Hidden;
            total_dan.Visibility = Visibility.Hidden;
            total_da.Content = "0";
            total_xiao.Content = "0";
            total_dan.Content = "0";

            if (soundState == true)
            {
                if (scoreResult > 0)
                {
                    mediaElement6.Stop();
                    mediaElement6.Play();
                    
                }
                else if (scoreResult < 0)
                {
                    mediaElement7.Stop();
                    mediaElement7.Play();
                }
            }
            sameResultUserList();
            if (secondRoound == true)
            {
                Storyboard mark_storyboard;
                mark_storyboard = (Storyboard)this.Resources["mark_Storyboard"];
                mark_storyboard.Begin(this);
            }

        }

        // 현재 게임을 놀고있는 유저목록을 현시하고, 현재 로그인한 유저의 딴돈/떼운돈, 현재보유금액을 표시한다.
        private void sameResultUserList()
        {
            DisplayUserList(currentPage);

            // 게임에서 따거나 떼운 총금액과 현재보유금액을 현시한다.
            totoal_money_label.Content = score.ToString();
            self_money_label.Content = myCash;

            // 메인화면이 떠있는 상태이면 현재보유금액을 표시한다.
            if (Main.multyChatRoom != null)
            {
                var userToolTip = new System.Windows.Controls.ToolTip();
                Main.multyChatRoom.lblCurReLoginPrice.Content = LabelContents.CUR_RELogin_PRICE_LBL + myCash;
                userToolTip.Content = Login._UserInfo.Nickname + " " + "(" + Login._UserInfo.Id + ")" + " " + myCash;
                Main.multyChatRoom.lblCurReLoginPrice.ToolTip = userToolTip;
            }
        }

        private void sound_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BitmapImage bitSound = new BitmapImage();

            bitSound.BeginInit();
            
            if (soundState == true)
            {
                bitSound.UriSource = new Uri("/Resources;component/image/sound2.gif", UriKind.RelativeOrAbsolute);
                soundState = false;
            }
            else
            {
                soundState = true;
                bitSound.UriSource = new Uri("/Resources;component/image/sound1.gif", UriKind.RelativeOrAbsolute);
            }
            bitSound.EndInit();
            sound.Source = bitSound;
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TempWindowForm tempWindowForm = new TempWindowForm();
            if (Main.activeGame != null)
            {
                //if (MessageBoxCommon.Show("Do you want to exit a game?", MessageBoxType.YesNo) == MessageBoxReply.No)
                if (QQMessageBox.Show(tempWindowForm, "您想离开游戏吗？", "提示", QQMessageBoxIcon.Question, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            
            
            if (Main.multyChatRoom != null)
            {
                e.Cancel = true;
                this.Hide();
                BitmapImage eBit = new BitmapImage();
                eBit.BeginInit();
                eBit.UriSource = new Uri("/Resources;component/image/game2.gif", UriKind.RelativeOrAbsolute);
                eBit.EndInit();
                Main.multyChatRoom.gameImg.Source = eBit;
                Main.multyChatRoom.m_gamestate = false;
            }
            else
            {
                if (bettingClock != null)
                    bettingClock.Stop();
                Main._DiceGame = null;
                Main.activeGame = null;
                Main.u_gamrId = null;
                Login._ClientEngine.Send(NotifyType.Request_OutGame, Login._UserInfo);
                Login._ClientEngine.DetachHandler(NotifyOccured);
            }
            
        }
    }

    public class MoveLeftValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ok, this is simple, it only demonstrates what happens
            if (value is double && parameter is Window)
            {
                var left = (double)value;
                var window = (Window)parameter;
                // here i must check on which side the window sticks on
                return left + window.ActualWidth + 7;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class MoveTopValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ok, this is simple, it only demonstrates what happens
            if (value is double && parameter is Window)
            {
                var top = (double)value;
                var window = (Window)parameter;
                // here i must check on which side the window sticks on
                return top + 25;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
