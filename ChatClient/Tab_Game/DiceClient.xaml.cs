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
using GameControls;

using ControlExs;
using System.Runtime.InteropServices;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for DiceClient.xaml
    /// </summary>
    public partial class DiceClient : Window
    {
        public GameInfo _GameInfo = null;

        private const int BTN_STATUS_NORMAL = 1;
        private const int BTN_STATUS_FOCUS = 2;
        private const int BTN_STATUS_DISABLE = 3;

        private DiceInfo m_DiceInfo = null;
        private BettingInfo m_BettingInfo = new BettingInfo();
        private List<Image> m_listChipImg = new List<Image>();
        private List<Image> m_listCoinImg = new List<Image>();

        private int m_wMeChairID = GameDefine.INVALID_CHAIR;
        
        private int m_nCurUserMoney = 0;
        private int m_nCurGainMoney = 0;
        private int m_nCurRoundGainMoney = 0;
        private int m_nCurScore = 0;

        private bool m_bSound = true;

        private System.Windows.Threading.DispatcherTimer timerBetting = null;
        private Microsoft.Windows.Controls.DataGrid dgUserList = new Microsoft.Windows.Controls.DataGrid(); 
        private FlowDocument m_flowDoc = new FlowDocument();

        private const int IDC_BTN_SENDMSG = 200;
        private const int CHAT_VIEW_LINE = 150;

        // added by usc at 2014/03/07
        private GameNotice gameNoticeWindow = null;
        private bool m_bNotice = false;

        private int[] m_aAreaBetAll = new int[4];
        private int[] m_lUserBetAll = new int[GameDefine.GAME_PLAYER];
        private int[] m_lUserBet = new int[4];

        private int m_nBettingTime = 0;
        private DateTime m_BetStartTime;

        private bool m_bFirstRound = true;

        private bool m_bFlicked = false;

        // added
        private int m_ChatViewLine = 0;
        private int m_nEndRoundStep = 0;

        Storyboard m_storybrdDicingResult;

        private struct UserBettingInfo
        {
            public string face { set; get; }
            public string id { set; get; }
            public int betscore { set; get; }
            public int score { set; get; }
        }

        public DiceClient(GameInfo gameInfo)
        {
            InitializeComponent();

            _GameInfo = gameInfo;

            string strGameType = string.Empty;

            if (gameInfo.nCashOrPointGame == 0)
                strGameType = "(余额)";
            else
                strGameType = "(积分)";

            lblTitle.Content = gameInfo.GameName + strGameType;
            imgIcon.Source = BitmapFrame.Create(new Uri(gameInfo.Icon, UriKind.RelativeOrAbsolute));

            InitControls();

            PrepareBetting();

            WebDownloader.GetInstance().DownloadFile(Login._UserInfo.Icon, IconDownloadComplete, this);
            Login._ClientEngine.AttachHandler(NotifyOccured);

            Login._ClientEngine.AttachHandler(OnReceive);            
        }

        private void InitControls()
        {
            meGameStart.Source = Main._MediaElement1.Source;
            meTimeWarning.Source = Main._MediaElement2.Source;
            meGameEnd.Source = Main._MediaElement3.Source;
            meBet.Source = Main._MediaElement4.Source;
            meAlert.Source = Main._MediaElement5.Source;
            meCheer.Source = Main._MediaElement6.Source;
            meGameLost.Source = Main._MediaElement7.Source;
            mePayCol.Source = Main._MediaElement8.Source;
            meMostCard.Source = Main._MediaElement9.Source;
            meProp.Source = Main._MediaElement10.Source;
            meBackground.Source = Main._MediaElement11.Source;

            timerBetting = new System.Windows.Threading.DispatcherTimer();
            timerBetting.Interval = new TimeSpan(0, 0, 0, 1);
            timerBetting.Tick += new EventHandler(BettingTimer_Tick);

            lblUserNick_Top.Content = Login._UserInfo.Nickname + " [" + Login._UserInfo.Id + "]";
            lblUserCash_Top.Content = Login._UserInfo.Cash;
            lblUserPoint_Top.Content = Login._UserInfo.Point;


            // added by usc at 2014/02/27

            if (Login._UserInfo.nCashOrPointGame == 0)
                m_nCurUserMoney = Login._UserInfo.Cash;
            else
                m_nCurUserMoney = Login._UserInfo.Point;

            lblCurGameMoney.Content = m_nCurUserMoney;

            // added by usc at 2014/03/25
            AddDataGrid(panUserListHeader, new Microsoft.Windows.Controls.DataGrid());
            AddDataGrid(panUserList, dgUserList);

            dgUserList.MouseDoubleClick += new MouseButtonEventHandler(dgUserList_MouseDoubleClick);

            SetButtonStatus(imgChip5, BTN_STATUS_DISABLE);
            SetButtonStatus(imgChip10, BTN_STATUS_DISABLE);
            SetButtonStatus(imgChip50, BTN_STATUS_DISABLE);
            SetButtonStatus(imgChip100, BTN_STATUS_DISABLE);
            SetButtonStatus(imgChip500, BTN_STATUS_DISABLE);

            // added by usc at 2014/03/12
            Array.Clear(m_aAreaBetAll, 0, 4);
            Array.Clear(m_lUserBetAll, 0, m_lUserBetAll.Length);
            Array.Clear(m_lUserBet, 0, 4);

            // added by usc at 2014/03/25
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Image imgCoin = new Image();
                    imgCoin.Margin = new Thickness(0, 0, 0, 11 * j + 2);
                    imgCoin.Width = 32;
                    imgCoin.Height = 21;

                    BitmapImage bmpImg = new BitmapImage();

                    bmpImg.BeginInit();

                    string strImgPath = string.Format("/Resources;component/image/Coin.png");
                    bmpImg.UriSource = new Uri(strImgPath, UriKind.RelativeOrAbsolute);
                    bmpImg.EndInit();

                    imgCoin.Source = bmpImg;
                    imgCoin.Name = string.Format("imgCoin{0}_{1}", i, j);

                    imgCoin.HorizontalAlignment = HorizontalAlignment.Center;
                    imgCoin.VerticalAlignment = VerticalAlignment.Bottom;
                    imgCoin.Visibility = Visibility.Hidden;
                    imgCoin.SetValue(Grid.ColumnProperty, i);

                    grdBettingGraph.Children.Add(imgCoin);

                    m_listCoinImg.Add(imgCoin);
                }
            }

            PlaySound(meBackground);

            m_storybrdDicingResult = (Storyboard)this.Resources["storybrdDicingResult"];
            m_storybrdDicingResult.Completed += new EventHandler(storybrdDicingResult_Completed);
        }

        private void AddDataGrid(StackPanel stackPanel, Microsoft.Windows.Controls.DataGrid dgGrid)
        {
            stackPanel.Children.Clear();

            dgGrid.HorizontalAlignment = HorizontalAlignment.Left;
            dgGrid.Margin = new System.Windows.Thickness(1, 1, 0, 1);
            dgGrid.BorderBrush = new SolidColorBrush(Colors.Transparent);
            dgGrid.HeadersVisibility = Microsoft.Windows.Controls.DataGridHeadersVisibility.Column;
            dgGrid.GridLinesVisibility = Microsoft.Windows.Controls.DataGridGridLinesVisibility.Horizontal;
            dgGrid.RowHeaderWidth = 0;
            dgGrid.RowHeight = 18;

            dgGrid.Style = (Style)FindResource("DataGridStyle");
            dgGrid.CellStyle = (Style)FindResource("dgCellStyle");
            dgGrid.RowStyle = (Style)FindResource("dgRowStyle");

            dgGrid.VerticalGridLinesBrush = (Brush)FindResource("TransparentGridLine");
            dgGrid.HorizontalGridLinesBrush = (Brush)FindResource("TransparentGridLine");
            dgGrid.Background = new SolidColorBrush(Color.FromRgb(251, 236, 166));
            dgGrid.Foreground = new SolidColorBrush(Color.FromRgb(29, 56, 117));
            dgGrid.FontSize = 11;
            dgGrid.IsReadOnly = true;
            dgGrid.SelectionMode = Microsoft.Windows.Controls.DataGridSelectionMode.Extended;
            dgGrid.SelectionUnit = Microsoft.Windows.Controls.DataGridSelectionUnit.FullRow;

            if (stackPanel.Name == "panUserList")
            {
                dgGrid.ColumnHeaderHeight = 0;
                dgGrid.HeadersVisibility = Microsoft.Windows.Controls.DataGridHeadersVisibility.None;
            }
            else
                dgGrid.ColumnHeaderStyle = (Style)FindResource("dgColumnHeaderStyle");

            Microsoft.Windows.Controls.DataGridTemplateColumn col1 = new Microsoft.Windows.Controls.DataGridTemplateColumn();
            col1.Header = "     ";

            FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Image));
            Binding b1 = new Binding("face");
            b1.Mode = BindingMode.TwoWay;
            factory1.SetValue(Image.SourceProperty, b1);

            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = factory1;

            col1.CellTemplate = cellTemplate1;
            col1.Width = 27;
            col1.MinWidth = 27;
            dgGrid.Columns.Add(col1);

            Microsoft.Windows.Controls.DataGridTextColumn col2 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col2.Header = "用户名";
            col2.Width = 88;
            col2.MinWidth = 88;

            col2.Binding = new Binding("id");
            dgGrid.Columns.Add(col2);

            Microsoft.Windows.Controls.DataGridTextColumn col3 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col3.Header = "押注金额";

            col3.Width = 45;// Microsoft.Windows.Controls.DataGridLength.Auto;
            col3.MinWidth = 45;
            col3.Binding = new Binding("betscore");
            col3.ElementStyle = (Style)FindResource("dgElementRightStyle");
            dgGrid.Columns.Add(col3);

            Microsoft.Windows.Controls.DataGridTextColumn col4 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col4.Header = "  金额";

            col4.Width = 45;// Microsoft.Windows.Controls.DataGridLength.Auto;
            col4.MinWidth = 45;
            col4.Binding = new Binding("score");
            col4.ElementStyle = (Style)FindResource("dgElementRightStyle");
            dgGrid.Columns.Add(col4);

            Microsoft.Windows.Controls.DataGridTextColumn col5 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col5.Header = "";

            col5.Width = 25;
            dgGrid.Columns.Add(col5);

            dgGrid.Items.Clear();
            stackPanel.Children.Add(dgGrid);
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

                        m_wMeChairID = GameDefine.INVALID_CHAIR;

                        for (int i = 0; i < diceInfo._Players.Count; i++)
                        {
                            UserInfo userInfo = diceInfo._Players[i];

                            if (userInfo != null && userInfo.Id == Login._UserInfo.Id)
                            {
                                m_wMeChairID = i;
                                break;
                            }
                        }

                        if (diceInfo._RoundIndex > 0)
                        {
                            // added by usc at 2014/03/20
                            Array.Clear(m_lUserBetAll, 0, m_lUserBetAll.Length);

                            for (int i = 0; i < 4; i++)
                                m_aAreaBetAll[i] = diceInfo.m_lPlayerBetAll[i];

                            DrawBettingGraph();
                            
                            for (int i = 0; i < diceInfo._Players.Count; i++)
                            {
                                for (int j = 0; j < 4; j++)
                                    m_lUserBetAll[i] += diceInfo.m_lUserScore[i, j];
                            }

                            if (m_bFirstRound)
                            {
                                DrawUserList(diceInfo, false);

                                // added by usc at 2014/04/03
                                string strMsg = string.Format("欢迎 {0} 进入房间", Login._UserInfo.Nickname);

                                InterpretServerMessgae("AMDIN_MSG", strMsg, 12, "Segoe UI", "b", "d", "#9adf8c");

                                for (int i = 0; i < 4; i++)
                                    DrawChip_First(i, m_aAreaBetAll[i]);

                                DrawBettingScore();                                
                            }
                        }

                        if ((m_DiceInfo != null) && (m_DiceInfo._RoundIndex == diceInfo._RoundIndex))
                        {
                            // added by usc at 2014/03/14
                            m_DiceInfo = diceInfo;

                            DrawUserList(diceInfo, false);

                            m_bFirstRound = false;

                            return;
                        }

                        // added by usc at 2014/03/19
                        //if (m_wMeChairID == GameDefine.INVALID_CHAIR)
                        //    return;

                        m_DiceInfo = diceInfo;

                        switch (m_DiceInfo._RoundIndex)
                        {
                            case 0:
                                {                                    
                                    PrepareBetting();

                                    InitUserList();
                                }
                                break;
                            case 1:
                                {
                                    StartBetting();
                                }
                                break;
                            case 2:
                                {
                                    StopBetting();

                                    if (m_bFirstRound)
                                    {
                                        int nElapsedTime = m_DiceInfo._RoundDelayTime + (int)(Math.Round((DateTime.Now - m_DiceInfo._dtReceiveTime).TotalMilliseconds / 1000d));

                                        if (nElapsedTime < 5)
                                            m_nEndRoundStep = 0;
                                        else if (nElapsedTime < 10)
                                            m_nEndRoundStep = 1;
                                        else if (nElapsedTime < 15)
                                            m_nEndRoundStep = 2;
                                        else
                                            m_nEndRoundStep = 3;
                                    }

                                    DrawDicing(m_DiceInfo.m_enCards);

                                    SetResultInfo(m_DiceInfo.m_lUserWinScore);
                                }
                                break;
                        }

                        m_bFirstRound = false;
                    }
                    break;
                case NotifyType.Reply_Betting:
                    {
                        if (!(baseInfo is BettingInfo))
                            return;

                        if (m_DiceInfo == null)
                            return;

                        m_BettingInfo = (BettingInfo)baseInfo;

                        if (m_BettingInfo._UserIndex != m_wMeChairID)
                        {
                            DrawChip(m_BettingInfo._Area, m_BettingInfo._Score);

                            PlaySound(meBet);
                        }

                        // added by usc at 2014/03/11
                        m_lUserBetAll[m_BettingInfo._UserIndex] += m_BettingInfo._Score;
                        m_aAreaBetAll[m_BettingInfo._Area] += m_BettingInfo._Score;

                        DrawBettingGraph();
                        DrawUserList(m_DiceInfo, false);

                        // added by usc at 2014/03/22
                        DrawBettingScore();
                    }
                    break;
            }
        }

        private void PrepareBetting()
        {
            grdBettingPanel.Cursor = Cursors.Arrow;                      
            grdDicingResult.Visibility = Visibility.Hidden;

            imgDaPanel.Opacity = 0.4;
            imgXiaoPanel.Opacity = 0.4;
            imgDanPanel.Opacity = 0.4;
            imgShuangPanel.Opacity = 0.4;

            imgDaYellowPanel.Visibility = Visibility.Hidden;
            imgXiaoYellowPanel.Visibility = Visibility.Hidden;
            imgDanYellowPanel.Visibility = Visibility.Hidden;
            imgShuangYellowPanel.Visibility = Visibility.Hidden;

            imgLeaveTime1.Visibility = Visibility.Hidden;
            imgLeaveTime2.Visibility = Visibility.Hidden;

            imgDice1.Visibility = Visibility.Hidden;
            imgDice2.Visibility = Visibility.Hidden;
            imgDice3.Visibility = Visibility.Hidden;

            grdBettingGraph.Visibility = Visibility.Visible;

            lblCurBettingMoney.Content = 0;

            DrawBettingGraph();           
            
            //added
            ClearChips();

            // added by usc at 2014/03/12
            Array.Clear(m_aAreaBetAll, 0, 4);
            Array.Clear(m_lUserBetAll, 0, m_lUserBetAll.Length);
            Array.Clear(m_lUserBet, 0, 4);

            DrawBettingScore();

            m_bFlicked = false;
            m_nEndRoundStep = 0;

            // added by usc at 2014/04/03
            m_storybrdDicingResult.Stop();
        }

        private void StartBetting()
        {
            imgDaPanel.Opacity = 1;
            imgXiaoPanel.Opacity = 1;
            imgDanPanel.Opacity = 1;
            imgShuangPanel.Opacity = 1;

            int nElapsedTime = (int)(Math.Round((DateTime.Now - m_DiceInfo._dtReceiveTime).TotalMilliseconds / 1000d));
            m_nBettingTime = DiceDefine.BET_TIME - m_DiceInfo._RoundDelayTime - nElapsedTime;
            m_BetStartTime = DateTime.Now;

            DrawBettingTime();

            imgLeaveTime1.Visibility = Visibility.Visible;
            imgLeaveTime2.Visibility = Visibility.Visible;

            timerBetting.Start();

            if(m_nBettingTime > 5)
                UpdateButton();
        }

        private void StopBetting()
        {
            grdBettingPanel.Cursor = Cursors.Arrow;

            m_nBettingTime = 0;
            imgLeaveTime1.Visibility = Visibility.Hidden;
            imgLeaveTime2.Visibility = Visibility.Hidden;

            grdBettingGraph.Visibility = Visibility.Hidden;

            imgDice1.Visibility = Visibility.Visible;
            imgDice2.Visibility = Visibility.Visible;
            imgDice3.Visibility = Visibility.Visible;

            timerBetting.Stop();

            PlaySound(meGameEnd);

            SetButtonStatus(imgChip5, BTN_STATUS_DISABLE);
            SetButtonStatus(imgChip10, BTN_STATUS_DISABLE);
            SetButtonStatus(imgChip50, BTN_STATUS_DISABLE);
            SetButtonStatus(imgChip100, BTN_STATUS_DISABLE);
            SetButtonStatus(imgChip500, BTN_STATUS_DISABLE);      
      
            // added by usc at 2014/03/15
            Array.Clear(m_aAreaBetAll, 0, 4);
        }

        private void DrawDicing(int[] aDiceState)
        {
            for (int i = 0; i < 3; i++)
                DrawDice(i + 1, aDiceState[i]);

            if (m_nEndRoundStep < 1)
            {
                Storyboard storybrdDicing = (Storyboard)this.Resources["storybrdDicing"];
                storybrdDicing.RepeatBehavior = new RepeatBehavior(1);
                storybrdDicing.Completed += new EventHandler(storybrdDicing_Completed);
                storybrdDicing.Begin(this);
            }
            else
                storybrdDicing_Completed(null, null);
        }

        private void DrawDice(int nDiceNo, int nState)
        {
            BitmapImage bmpImg = new BitmapImage();

            bmpImg.BeginInit();
            switch (nState)
            {
                case 1:
                    bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/dice1.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 2:
                    bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/dice2.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 3:
                    bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/dice3.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 4:
                    bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/dice4.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 5:
                    bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/dice5.gif", UriKind.RelativeOrAbsolute);
                    break;
                case 6:
                    bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/dice6.gif", UriKind.RelativeOrAbsolute);
                    break;
            }
            bmpImg.EndInit();

            switch (nDiceNo)
            {
                case 1:
                    imgDice1.Source = bmpImg;
                    break;
                case 2:
                    imgDice2.Source = bmpImg;
                    break;
                case 3:
                    imgDice3.Source = bmpImg;
                    break;
            }
            
        }      

        private void DrawBettingGraph()
        {
            int nMaxSum = GetMaxValue(m_aAreaBetAll);

            if (nMaxSum == 0)
            {
                for (int i = 0; i < m_listCoinImg.Count; i++)
                    m_listCoinImg[i].Visibility = Visibility.Hidden;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    int nPercent = (int)((double)m_aAreaBetAll[i] / (double)nMaxSum * 100d);

                    for (int j = 0; j < 10; j++)
                    {
                        string strImgName = string.Format("imgCoin{0}_{1}", i, j);

                        if (j < nPercent / 10)
                            SetCoinVisibility(strImgName, true);
                        else
                            SetCoinVisibility(strImgName, false);
                    }
                }
            }
        }

        private Image SetCoinVisibility(string strCoinName, bool bVisible)
        {
            Image retImage = null;

            for (int i = 0; i < m_listCoinImg.Count; i++)
            {
                if (m_listCoinImg[i].Name == strCoinName)
                {
                    if (bVisible)
                        m_listCoinImg[i].Visibility = Visibility.Visible;
                    else
                        m_listCoinImg[i].Visibility = Visibility.Hidden;

                    break;
                }
            }

            return retImage;
        }

        private void DrawChip_First(int nArea, int nScore)
        {
            int nCurScore = nScore;

            int[] lScoreIndex = new int[] { 500000, 100000, 10000, 1000, 100 };

            for (int i = 0; i < lScoreIndex.Length; i++)
            {
                int lCellCount = nCurScore / lScoreIndex[i];

                if (lCellCount == 0) continue;

                for (int j = 0; j < lCellCount; j++)
                {
                    DrawChip(nArea, lScoreIndex[i]);
                }

                nCurScore -= lCellCount * lScoreIndex[i];
            }
        }

        Random _rnd = new Random();

        private void DrawChip(int nArea, int nScore)
        {

            string strImgName = string.Empty;
            int nLeft = 0, nTop = 0, nRight, nBottom;

            switch (nArea)
            {
                case 0:
                    {
                        strImgName = "imgChipDa";
                        nLeft = _rnd.Next(10, (int)imgDaPanel.Width - 50);
                        nTop = _rnd.Next(10, (int)imgDaPanel.Height - 100);
                    }
                    break;
                case 1:
                    {
                        strImgName = "imgChipXiao";
                        nLeft = _rnd.Next((int)imgDaPanel.Width + 13, (int)imgDaPanel.Width * 2 + 3 - 50);
                        nTop = _rnd.Next(10, (int)imgDaPanel.Height - 100);
                    }
                    break;
                case 2:
                    {
                        strImgName = "imgChipDan";
                        nLeft = _rnd.Next(10, (int)imgDanPanel.Width - 50);
                        nTop = _rnd.Next((int)imgDaPanel.Height + 13, (int)imgDaPanel.Height * 2 + 3 - 100);
                    }
                    break;
                case 3:
                    {
                        strImgName = "imgChipShuang";
                        nLeft = _rnd.Next((int)imgDaPanel.Width + 13, (int)imgDaPanel.Width * 2 + 3 - 50);
                        nTop = _rnd.Next((int)imgDaPanel.Height + 13, (int)imgDaPanel.Height * 2 + 3 - 100);
                    }
                    break;
            }

            nRight = (int)grdBettingPanel.Width - nLeft - 40;
            nBottom = (int)grdBettingPanel.Height - nTop - 40;

            Image imgChip = new Image();
            imgChip.Margin = new Thickness(nLeft, nTop, nRight, nBottom);
            imgChip.Width = 40;
            imgChip.Height = 40;

            BitmapImage bmpImg = new BitmapImage();

            bmpImg.BeginInit();

            string strImgPath = string.Format("/Resources;component/image/diceGame/Chip{0}.png", nScore);
            bmpImg.UriSource = new Uri(strImgPath, UriKind.RelativeOrAbsolute);
            bmpImg.EndInit();

            imgChip.Source = bmpImg;
            imgChip.Name = strImgName;
            imgChip.Visibility = Visibility.Visible;

            grdBettingPanel.Children.Add(imgChip);

            m_listChipImg.Add(imgChip);
        }

        private void BettingTimer_Tick(object sender, EventArgs e)
        {
            int nLeaveTime = GetBetLeaveTime();

            if (nLeaveTime == DiceDefine.BET_TIME)
            {
                PlaySound(meGameStart);
            }
            else if (nLeaveTime > 0 && nLeaveTime <= 5)
            {
                PlaySound(meTimeWarning);

                grdBettingPanel.Cursor = Cursors.Arrow;

                SetButtonStatus(imgChip5, BTN_STATUS_DISABLE);
                SetButtonStatus(imgChip10, BTN_STATUS_DISABLE);
                SetButtonStatus(imgChip50, BTN_STATUS_DISABLE);
                SetButtonStatus(imgChip100, BTN_STATUS_DISABLE);
                SetButtonStatus(imgChip500, BTN_STATUS_DISABLE);
            }
            else if (nLeaveTime < 0)
                timerBetting.Stop();

            DrawBettingTime();
        }

        private int GetBetLeaveTime()
        {
            return m_nBettingTime - (int)(DateTime.Now - m_BetStartTime).TotalSeconds;
        }

        private void DrawBettingTime()
        {
            string strNumPath1;
            string strNumPath2;

            strNumPath1 = "";
            strNumPath2 = "";

            int nLeaveTime = GetBetLeaveTime();

            if (nLeaveTime > 0)
            {
                strNumPath1 = string.Format("/Resources;component/image/diceGame/Num{0}.png", nLeaveTime / 10);
                strNumPath2 = string.Format("/Resources;component/image/diceGame/Num{0}.png", nLeaveTime % 10);
            }
            else
            {
                strNumPath1 = "/Resources;component/image/diceGame/Num0.png";
                strNumPath2 = "/Resources;component/image/diceGame/Num0.png";
            }

            BitmapImage bmpImg = new BitmapImage();
            bmpImg.BeginInit();
            bmpImg.UriSource = new Uri(strNumPath1, UriKind.RelativeOrAbsolute);
            bmpImg.EndInit();

            imgLeaveTime1.Source = bmpImg;

            bmpImg = new BitmapImage();
            bmpImg.BeginInit();
            bmpImg.UriSource = new Uri(strNumPath2, UriKind.RelativeOrAbsolute);
            bmpImg.EndInit();

            imgLeaveTime2.Source = bmpImg;
        }

        void storybrdDicing_Completed(object sender, EventArgs e)
        {
            if (m_DiceInfo._RoundIndex < 2)
                return;

            int nDice1 = 0, nDice2 = 0, nDice3 = 0;
            int nTotalDice = 0;

            nDice1 = m_DiceInfo.m_enCards[0];
            nDice2 = m_DiceInfo.m_enCards[1];
            nDice3 = m_DiceInfo.m_enCards[2];

            nTotalDice = nDice1 + nDice2 + nDice3;
                        
            if ((nDice1 == nDice2) && (nDice2 == nDice3))
            {
                if (m_nEndRoundStep < 2)
                {
                    lblXResultLavel.Visibility = Visibility.Visible;
                    lblXResultLavel.Content = string.Format("X{0}", nDice1);
                    lblXResultValue.Visibility = Visibility.Visible;


                    lblXLabel.Visibility = Visibility.Visible;
                    lblXLabel.Content = string.Format("X{0}", nDice1);

                    PlaySound(meMostCard);

                    Storyboard storybrdXLabel;
                    storybrdXLabel = (Storyboard)this.Resources["storybrdXLabel"];
                    storybrdXLabel.RepeatBehavior = new RepeatBehavior(6);
                    storybrdXLabel.Completed += new EventHandler(storybrdXLabel_Completed);


                    Storyboard storybrdXFlicker;
                    storybrdXFlicker = (Storyboard)this.Resources["storybrdXFlicker"];
                    storybrdXFlicker.RepeatBehavior = new RepeatBehavior(10);
                    storybrdXFlicker.Completed += new EventHandler(storybrdXFlicker_Completed);

                    //PlaySound(meProp);

                    storybrdXLabel.Begin(this);
                    storybrdXFlicker.Begin(this);
                }
                else
                    storybrdXFlicker_Completed(null, null);
                               
            }
            else
            {
                if (m_nEndRoundStep < 2)
                {
                    Storyboard storybrdFlicker1 = null;
                    Storyboard storybrdFlicker2 = null;

                    if (nTotalDice >= 11 && nTotalDice <= 17)
                        storybrdFlicker1 = (Storyboard)this.Resources["storybrdDaPanelFlicker"];
                    else if (nTotalDice >= 4 && nTotalDice <= 10)
                        storybrdFlicker1 = (Storyboard)this.Resources["storybrdXiaoPanelFlicker"];


                    if (nTotalDice % 2 == 0)
                        storybrdFlicker2 = (Storyboard)this.Resources["storybrdShuangPanelFlicker"];
                    else
                        storybrdFlicker2 = (Storyboard)this.Resources["storybrdDanPanelFlicker"];

                    storybrdFlicker2.Completed += storybrdFlicker_Completed;

                    storybrdFlicker1.Begin(this);
                    storybrdFlicker2.Begin(this);
                }
                else
                    storybrdFlicker_Completed(null, null);
            }
        }

        void storybrdXLabel_Completed(object sender, EventArgs e)
        {
            lblXLabel.Visibility = Visibility.Hidden;
        }

        void storybrdXFlicker_Completed(object sender, EventArgs e)
        {
            if (m_bFlicked)
                return;

            m_bFlicked = true;

            if (m_DiceInfo._RoundIndex < 2)
                return;

            PlaySound(meProp);

            lblXResultLavel.Visibility = Visibility.Visible;
            lblXResultValue.Visibility = Visibility.Visible;

            if (m_nEndRoundStep < 2)
            {
                int nFlickerArea = 0;
                Storyboard storybrd = null;

                for (int i = 0; i < m_DiceInfo.m_bWinner.Length; i++)
                {
                    if (m_DiceInfo.m_bWinner[i] == 1)
                    {
                        nFlickerArea = i;
                        break;
                    }
                }


                if (m_listChipImg.Count > 0)
                    PlaySound(mePayCol);

                switch (nFlickerArea)
                {
                    case 0:
                        {
                            storybrd = (Storyboard)this.Resources["storybrdXDaFlicker"];
                            storybrd.Begin(this);

                            MoveAllChips_To_DaPanel();
                        }
                        break;
                    case 1:
                        {
                            storybrd = (Storyboard)this.Resources["storybrdXXiaoFlicker"];
                            storybrd.Begin(this);

                            MoveAllChips_To_XiaoPanel();
                        }
                        break;
                    case 2:
                        {
                            storybrd = (Storyboard)this.Resources["storybrdXDanFlicker"];
                            storybrd.Begin(this);

                            MoveAllChips_To_DanPanel();
                        }
                        break;
                    case 3:
                        {
                            storybrd = (Storyboard)this.Resources["storybrdXShuangFlicker"];
                            storybrd.Begin(this);

                            MoveAllChips_To_ShuangPanel();
                        }
                        break;
                }
            }

            if(m_wMeChairID != GameDefine.INVALID_CHAIR)
                DrawResult();

            DrawUserList(m_DiceInfo, true);
        }               

        void storybrdFlicker_Completed(object sender, EventArgs e)
        {
            if (m_bFlicked)
                return;

            m_bFlicked = true;

            if (m_DiceInfo._RoundIndex < 2)
                return;

            if (m_nEndRoundStep < 3)
            {
                if (m_listChipImg.Count > 0)
                    PlaySound(mePayCol);

                MoveChips();
            }

            if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                DrawResult();

            DrawUserList(m_DiceInfo, true);
        }

        private void MoveChips()
        {
            int nDice1 = 0, nDice2 = 0, nDice3 = 0;
            int nTotalDice = 0;

            nDice1 = m_DiceInfo.m_enCards[0];
            nDice2 = m_DiceInfo.m_enCards[1];
            nDice3 = m_DiceInfo.m_enCards[2];

            nTotalDice = nDice1 + nDice2 + nDice3;

            if (nTotalDice >= 11 && nTotalDice <= 17)
            {
                MoveXiaoChips_To_DaPanel();
            }
            else if (nTotalDice >= 4 && nTotalDice <= 10)
            {
                MoveDaChips_To_XiaoPanel();
            }

            if (nTotalDice % 2 == 0)
            {
                MoveDanChips_To_ShuangPanel();
            }
            else
            {
                MoveShuangChips_To_DanPanel();                
            }
        }

        private void MoveXiaoChips_To_DaPanel()
        {
            DrawChipsMoving("imgChipXiao", TranslateTransform.XProperty, -(int)imgDanPanel.Width);
        }

        private void MoveDaChips_To_XiaoPanel()
        {
            DrawChipsMoving("imgChipDa", TranslateTransform.XProperty, (int)imgDanPanel.Width);
        }

        private void MoveShuangChips_To_DanPanel()
        {
            DrawChipsMoving("imgChipShuang", TranslateTransform.XProperty, -(int)imgDanPanel.Width);
        }

        private void MoveDanChips_To_ShuangPanel()
        {
            DrawChipsMoving("imgChipDan", TranslateTransform.XProperty, (int)imgDanPanel.Width);
        }

        private void DrawChipsMoving(string strChipName, DependencyProperty dp, int nValue)
        {
            for (int i = 0; i < m_listChipImg.Count; i++)
            {
                if (m_listChipImg[i].Name == strChipName)
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 1, 500));
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0, nValue, dur);
                    tt.BeginAnimation(dp, doubleAnimation);
                }
            }
        }

        private void MoveAllChips_To_DaPanel()
        {
            for (int i = 0; i < m_listChipImg.Count; i++)
            {
                if (m_listChipImg[i].Name == "imgChipXiao")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0, -(int)imgDanPanel.Width, dur);
                    tt.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
                }
                else if (m_listChipImg[i].Name == "imgChipDan")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0, -(int)imgDanPanel.Height, dur);
                    tt.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);

                }
                else if (m_listChipImg[i].Name == "imgChipShuang")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation1 = new DoubleAnimation(0, -(int)imgDanPanel.Height, dur);
                    DoubleAnimation doubleAnimation2 = new DoubleAnimation(0, -(int)imgDanPanel.Width, dur);
                    tt.BeginAnimation(TranslateTransform.YProperty, doubleAnimation1);
                    tt.BeginAnimation(TranslateTransform.XProperty, doubleAnimation2);
                }
            }
        }

        private void MoveAllChips_To_XiaoPanel()
        {
            for (int i = 0; i < m_listChipImg.Count; i++)
            {
                if (m_listChipImg[i].Name == "imgChipDa")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0, (int)imgDanPanel.Width, dur);
                    tt.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
                }
                else if (m_listChipImg[i].Name == "imgChipShuang")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0, -(int)imgDanPanel.Height, dur);
                    tt.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);

                }
                else if (m_listChipImg[i].Name == "imgChipDan")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation1 = new DoubleAnimation(0, -(int)imgDanPanel.Height, dur);
                    DoubleAnimation doubleAnimation2 = new DoubleAnimation(0, (int)imgDanPanel.Width, dur);
                    tt.BeginAnimation(TranslateTransform.YProperty, doubleAnimation1);
                    tt.BeginAnimation(TranslateTransform.XProperty, doubleAnimation2);
                }
            }
        }

        private void MoveAllChips_To_DanPanel()
        {
            for (int i = 0; i < m_listChipImg.Count; i++)
            {
                if (m_listChipImg[i].Name == "imgChipShuang")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0, -(int)imgDanPanel.Width, dur);
                    tt.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
                }
                else if (m_listChipImg[i].Name == "imgChipDa")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0, (int)imgDanPanel.Height, dur);
                    tt.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);

                }
                else if (m_listChipImg[i].Name == "imgChipXiao")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation1 = new DoubleAnimation(0, (int)imgDanPanel.Height, dur);
                    DoubleAnimation doubleAnimation2 = new DoubleAnimation(0, -(int)imgDanPanel.Width, dur);
                    tt.BeginAnimation(TranslateTransform.YProperty, doubleAnimation1);
                    tt.BeginAnimation(TranslateTransform.XProperty, doubleAnimation2);
                }
            }
        }

        private void MoveAllChips_To_ShuangPanel()
        {
            for (int i = 0; i < m_listChipImg.Count; i++)
            {
                if (m_listChipImg[i].Name == "imgChipDan")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0, (int)imgDanPanel.Width, dur);
                    tt.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
                }
                else if (m_listChipImg[i].Name == "imgChipXiao")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation = new DoubleAnimation(0, (int)imgDanPanel.Height, dur);
                    tt.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);

                }
                else if (m_listChipImg[i].Name == "imgChipDa")
                {
                    TranslateTransform tt = new TranslateTransform();
                    m_listChipImg[i].RenderTransform = tt;

                    Duration dur = new Duration(new TimeSpan(0, 0, 0, 2, 0));
                    DoubleAnimation doubleAnimation1 = new DoubleAnimation(0, (int)imgDanPanel.Height, dur);
                    DoubleAnimation doubleAnimation2 = new DoubleAnimation(0, (int)imgDanPanel.Width, dur);
                    tt.BeginAnimation(TranslateTransform.YProperty, doubleAnimation1);
                    tt.BeginAnimation(TranslateTransform.XProperty, doubleAnimation2);
                }
            }
        }

        private void SetResultInfo(int[] aUserWinScore)
        {
            for (int i = 0; i < m_DiceInfo._Players.Count; i++)
            {
                if (m_DiceInfo._Players[i].Id == Login._UserInfo.Id)
                {
                    int nResultScore = aUserWinScore[i];

                    if (nResultScore > 0)
                    {
                        lblResultScore.Foreground = new SolidColorBrush(Colors.Yellow);
                        lblResultScore.Content = "+" + nResultScore;

                        lblResultDa.Foreground = new SolidColorBrush(Colors.Yellow);
                        lblResultXiao.Foreground = new SolidColorBrush(Colors.Yellow);
                        lblResultDan.Foreground = new SolidColorBrush(Colors.Yellow);
                        lblResultShuang.Foreground = new SolidColorBrush(Colors.Yellow);

                        lblXResultValue.Content = "+" + nResultScore;
                    }
                    else
                    {
                        lblResultScore.Foreground = new SolidColorBrush(Colors.Black);
                        lblResultScore.Content = nResultScore;

                        lblResultDa.Foreground = new SolidColorBrush(Colors.Black);
                        lblResultXiao.Foreground = new SolidColorBrush(Colors.Black);
                        lblResultDan.Foreground = new SolidColorBrush(Colors.Black);
                        lblResultShuang.Foreground = new SolidColorBrush(Colors.Black);

                        lblXResultValue.Content = nResultScore;
                    }


                    if (m_DiceInfo.m_lUserScore[i, 0] != 0)
                    {
                        if (m_DiceInfo.m_bWinner[0] == 1)
                            lblResultDa.Content = "+" + m_DiceInfo.m_lUserScore[i, 0];
                        else
                            lblResultDa.Content = "-" + m_DiceInfo.m_lUserScore[i, 0];
                    }
                    else
                        lblResultDa.Content = 0;

                    if (m_DiceInfo.m_lUserScore[i, 1] != 0)
                    {
                        if (m_DiceInfo.m_bWinner[1] == 1)
                            lblResultXiao.Content = "+" + m_DiceInfo.m_lUserScore[i, 1];
                        else
                            lblResultXiao.Content = "-" + m_DiceInfo.m_lUserScore[i, 1];
                    }
                    else
                        lblResultXiao.Content = 0;

                    if (m_DiceInfo.m_lUserScore[i, 2] != 0)
                    {
                        if (m_DiceInfo.m_bWinner[2] == 1)
                            lblResultDan.Content = "+" + m_DiceInfo.m_lUserScore[i, 2];
                        else
                            lblResultDan.Content = "-" + m_DiceInfo.m_lUserScore[i, 2];
                    }
                    else
                        lblResultDan.Content = 0;

                    if (m_DiceInfo.m_lUserScore[i, 3] != 0)
                    {
                        if (m_DiceInfo.m_bWinner[3] == 1)
                            lblResultShuang.Content = "+" + m_DiceInfo.m_lUserScore[i, 3];
                        else
                            lblResultShuang.Content = "-" + m_DiceInfo.m_lUserScore[i, 3];
                    }
                    else
                        lblResultShuang.Content = 0;

                    //
                    if (Login._UserInfo.nCashOrPointGame == 0)
                        m_nCurUserMoney = m_DiceInfo._Players[i].Cash;
                    else
                        m_nCurUserMoney = m_DiceInfo._Players[i].Point;

                    m_nCurRoundGainMoney = nResultScore;
                    m_nCurGainMoney += nResultScore;

                }
            }
        }

        private void DrawResult()
        {
            if (m_DiceInfo._RoundIndex < 2)
                return;

            if (Login._UserInfo.nCashOrPointGame == 0)
                lblUserCash_Top.Content = m_nCurUserMoney;
            else
                lblUserPoint_Top.Content = m_nCurUserMoney;

            lblCurGameMoney.Content = m_nCurUserMoney;
            lblRoundGainMoney.Content = m_nCurRoundGainMoney;
            lblCurGainMoney.Content = m_nCurGainMoney;

            ChatEngine.StringInfo messageInfo = new ChatEngine.StringInfo();

            // added by usc at 2014/03/04
            messageInfo.UserId = Login._UserInfo.Id;
            messageInfo.strRoomID = Login._UserInfo.GameId;

            Login._ClientEngine.Send(NotifyType.Request_GameResult, messageInfo);
    

            int nResultScore = Convert.ToInt32(lblResultScore.Content);

            if (nResultScore > 0)       // 돈을 땄을때
                PlaySound(meCheer);
            else if (nResultScore < 0)   // 돈을 떼웠을때
                PlaySound(meGameLost);  

            
            
            m_storybrdDicingResult.Begin(this);
        }

        void storybrdDicingResult_Completed(object sender, EventArgs e)
        {
            lblXResultLavel.Visibility = Visibility.Hidden;
            lblXResultValue.Visibility = Visibility.Hidden;            

            ClearChips();

            // added by usc at 2014/03/12
            Array.Clear(m_aAreaBetAll, 0, 4);
            Array.Clear(m_lUserBetAll, 0, m_lUserBetAll.Length);
            Array.Clear(m_lUserBet, 0, 4);

            DrawBettingScore();

            lblDaAllScore.Visibility = Visibility.Hidden;
            lblXiaoAllScore.Visibility = Visibility.Hidden;   
            lblDanAllScore.Visibility = Visibility.Hidden;  
            lblShuangAllScore.Visibility = Visibility.Hidden;

            lblDaSlash.Visibility = Visibility.Hidden;
            lblXiaoSlash.Visibility = Visibility.Hidden;
            lblDanSlash.Visibility = Visibility.Hidden;
            lblShuangSlash.Visibility = Visibility.Hidden;

            lblDaScore.Visibility = Visibility.Hidden;
            lblXiaoScore.Visibility = Visibility.Hidden;
            lblDanScore.Visibility = Visibility.Hidden;
            lblShuangScore.Visibility = Visibility.Hidden;
    }

        private void ClearChips()
        {
            for (int i = 0; i < m_listChipImg.Count; i++)
                grdBettingPanel.Children.Remove(m_listChipImg[i]);

            m_listChipImg.Clear();
        }

        private void PlaySound(MediaElement mediaElement)
        {
            if (m_bSound)
            {
                mediaElement.Stop();
                mediaElement.Play();
            }
        }

        private int GetMaxValue(int[] aNumber)
        {
            int nMaxValue = aNumber[0];

            for (int i = 1; i < 4; i++)
                if (nMaxValue < aNumber[i])
                    nMaxValue = aNumber[i];

            return nMaxValue;
        }

        private void InitUserList()
        {
            dgUserList.Items.Clear();

            for (int i = 0; i < m_DiceInfo._Players.Count; i++)
            {
                string strImgPath = string.Format("/Resources;Component/{0}", m_DiceInfo._Players[i].Icon);

                dgUserList.Items.Add(new UserBettingInfo { face = strImgPath, id = m_DiceInfo._Players[i].Nickname, betscore = 0, score = 0 });
            }            
        }

        private void DrawUserList(DiceInfo curDiceInfo, bool bWinScore)
        {
            dgUserList.Items.Clear();

            for (int i = 0; i < curDiceInfo._Players.Count; i++)
            {
                string strImgPath = string.Format("/Resources;Component/{0}", curDiceInfo._Players[i].Icon);

                if (bWinScore)
                {
                    dgUserList.Items.Add(new UserBettingInfo { face = strImgPath, id = curDiceInfo._Players[i].Nickname, betscore = m_lUserBetAll[i], score = curDiceInfo.m_lUserWinScore[i] });

                    if (curDiceInfo.m_lUserWinScore[i] > 0)
                    {
                        string strMsg = string.Format("恭喜 {0} 赢得{1}金币", curDiceInfo._Players[i].Nickname, curDiceInfo.m_lUserWinScore[i]);

                        InterpretServerMessgae("PROMPT_MSG", strMsg, 12, "Segoe UI", "b", "d", "#ffff00");
                    }
                }
                else
                    dgUserList.Items.Add(new UserBettingInfo { face = strImgPath, id = curDiceInfo._Players[i].Nickname, betscore = m_lUserBetAll[i], score = 0 });

            }            
        }

        private void UpdateButton()
        {
            int curBettingMoney = Convert.ToInt32(lblCurBettingMoney.Content);

            if (Login._UserInfo.nCashOrPointGame == 0)
                curBettingMoney = Convert.ToInt32(lblCurBettingMoney.Content);
            else
                curBettingMoney = Convert.ToInt32(lblCurBettingMoney.Content);

            double nCommissionMulti = (100 + _GameInfo.Commission) / 100d;

            if (m_nCurUserMoney < 100 * nCommissionMulti || curBettingMoney >= (GameDefine.MAX_BETTING_MONEY - 100) * nCommissionMulti)
            {
                SetButtonStatus(imgChip5, BTN_STATUS_DISABLE);

                if (m_nCurScore == 100 && grdBettingPanel.Cursor != Cursors.Arrow)
                {
                    grdBettingPanel.Cursor = Cursors.Arrow;
                    m_nCurScore = 0;
                }
            }
            else
                SetButtonStatus(imgChip5, BTN_STATUS_NORMAL);

            if (m_nCurUserMoney < 1000 * nCommissionMulti || curBettingMoney > (GameDefine.MAX_BETTING_MONEY - 1000) * nCommissionMulti)
            {
                SetButtonStatus(imgChip10, BTN_STATUS_DISABLE);

                if (m_nCurScore == 1000 && grdBettingPanel.Cursor != Cursors.Arrow)
                {
                    grdBettingPanel.Cursor = Cursors.Arrow;
                    m_nCurScore = 0;
                }
            }
            else
                SetButtonStatus(imgChip10, BTN_STATUS_NORMAL);

            if (m_nCurUserMoney < 10000 * nCommissionMulti || curBettingMoney > (GameDefine.MAX_BETTING_MONEY - 10000) * nCommissionMulti)
            {
                SetButtonStatus(imgChip50, BTN_STATUS_DISABLE);

                if (m_nCurScore == 10000 && grdBettingPanel.Cursor != Cursors.Arrow)
                {
                    grdBettingPanel.Cursor = Cursors.Arrow;
                    m_nCurScore = 0;
                }
            }
            else
                SetButtonStatus(imgChip50, BTN_STATUS_NORMAL);

            if (m_nCurUserMoney < 100000 * nCommissionMulti || curBettingMoney > (GameDefine.MAX_BETTING_MONEY - 100000) * nCommissionMulti)
            {
                SetButtonStatus(imgChip100, BTN_STATUS_DISABLE);

                if (m_nCurScore == 100000 && grdBettingPanel.Cursor != Cursors.Arrow)
                {
                    grdBettingPanel.Cursor = Cursors.Arrow;
                    m_nCurScore = 0;
                }
            }
            else
                SetButtonStatus(imgChip100, BTN_STATUS_NORMAL);

            if (m_nCurUserMoney < 500000 * nCommissionMulti || curBettingMoney > (GameDefine.MAX_BETTING_MONEY - 500000) * nCommissionMulti)
            {
                SetButtonStatus(imgChip500, BTN_STATUS_DISABLE);

                if (m_nCurScore == 500000 && grdBettingPanel.Cursor != Cursors.Arrow)
                {
                    grdBettingPanel.Cursor = Cursors.Arrow;
                    m_nCurScore = 0;
                }
            }
            else
                SetButtonStatus(imgChip500, BTN_STATUS_NORMAL);
        }

        private void SetButtonStatus(Image imgCtrl, int nStatus)
        {
            BitmapImage bmpImg = new BitmapImage();
            bmpImg.BeginInit();

            string strImgMoney = imgCtrl.Name.Substring(7, imgCtrl.Name.Length - 7);
            string strUri = string.Format("/Resources;component/image/diceGame/BT_JETTON_{0}_{1}.png", strImgMoney, nStatus);

            bmpImg.UriSource = new Uri(strUri, UriKind.Relative);
            bmpImg.EndInit();

            imgCtrl.Source = bmpImg;

            if (nStatus == BTN_STATUS_DISABLE)
                imgCtrl.IsEnabled = false;
            else
                imgCtrl.IsEnabled = true;
        }

#region imgChip MouseEvent

        private void imgChip5_MouseEnter(object sender, MouseEventArgs e)
        {
            if(imgChip5.IsEnabled)
                SetButtonStatus(imgChip5, BTN_STATUS_FOCUS);
        }

        private void imgChip5_MouseLeave(object sender, MouseEventArgs e)
        {
            if (imgChip5.IsEnabled)
                SetButtonStatus(imgChip5, BTN_STATUS_NORMAL);
        }

        private void imgChip5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Stream streamCursor = Application.GetResourceStream(new Uri("/Resources;component/image/diceGame/SCORE_5.cur", UriKind.RelativeOrAbsolute)).Stream;
            grdBettingPanel.Cursor = new Cursor(streamCursor);

            m_nCurScore = 100;
        }

        private void imgChip10_MouseEnter(object sender, MouseEventArgs e)
        {
            if (imgChip10.IsEnabled)
                SetButtonStatus(imgChip10, BTN_STATUS_FOCUS);
        }

        private void imgChip10_MouseLeave(object sender, MouseEventArgs e)
        {
            if (imgChip10.IsEnabled)
                SetButtonStatus(imgChip10, BTN_STATUS_NORMAL);
        }

        private void imgChip10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Stream streamCursor = Application.GetResourceStream(new Uri("/Resources;component/image/diceGame/SCORE_10.cur", UriKind.RelativeOrAbsolute)).Stream;
            grdBettingPanel.Cursor = new Cursor(streamCursor);

            m_nCurScore = 1000;
        }

        private void imgChip50_MouseEnter(object sender, MouseEventArgs e)
        {
            if (imgChip50.IsEnabled)
                SetButtonStatus(imgChip50, BTN_STATUS_FOCUS);
        }

        private void imgChip50_MouseLeave(object sender, MouseEventArgs e)
        {
            if (imgChip50.IsEnabled)
                SetButtonStatus(imgChip50, BTN_STATUS_NORMAL);
        }

        private void imgChip50_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Stream streamCursor = Application.GetResourceStream(new Uri("/Resources;component/image/diceGame/SCORE_50.cur", UriKind.RelativeOrAbsolute)).Stream;
            grdBettingPanel.Cursor = new Cursor(streamCursor);

            m_nCurScore = 10000;
        }

        private void imgChip100_MouseEnter(object sender, MouseEventArgs e)
        {
            if (imgChip100.IsEnabled)
                SetButtonStatus(imgChip100, BTN_STATUS_FOCUS);
        }

        private void imgChip100_MouseLeave(object sender, MouseEventArgs e)
        {
            if (imgChip100.IsEnabled)
                SetButtonStatus(imgChip100, BTN_STATUS_NORMAL);
        }

        private void imgChip100_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Stream streamCursor = Application.GetResourceStream(new Uri("/Resources;component/image/diceGame/SCORE_100.cur", UriKind.RelativeOrAbsolute)).Stream;
            grdBettingPanel.Cursor = new Cursor(streamCursor);

            m_nCurScore = 100000;
        }

        private void imgChip500_MouseEnter(object sender, MouseEventArgs e)
        {
            if (imgChip500.IsEnabled)
                SetButtonStatus(imgChip500, BTN_STATUS_FOCUS);
        }

        private void imgChip500_MouseLeave(object sender, MouseEventArgs e)
        {
            if (imgChip500.IsEnabled)
                SetButtonStatus(imgChip500, BTN_STATUS_NORMAL);
        }

        private void imgChip500_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Stream streamCursor = Application.GetResourceStream(new Uri("/Resources;component/image/diceGame/SCORE_500.cur", UriKind.RelativeOrAbsolute)).Stream;
            grdBettingPanel.Cursor = new Cursor(streamCursor);

            m_nCurScore = 500000;
        }

#endregion

#region BettingPanel MouseEvent

        private void imgDaPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PutChip(0);
        }

        private void imgXiaoPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PutChip(1);
        }

        private void imgDanPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PutChip(2);
        }

        private void imgShuangPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PutChip(3);
        }

        private void PutChip(int nArea)
        {
            if (m_DiceInfo != null && 
                m_DiceInfo._RoundIndex == 1 && 
                grdBettingPanel.Cursor != Cursors.Arrow)
            {
                double CommissionMulti = (100 + _GameInfo.Commission) / 100d;

                int curBettingMoney = Convert.ToInt32(lblCurBettingMoney.Content);

                // 2만이상 베팅할수 없다.
                if (curBettingMoney + m_nCurScore > GameDefine.MAX_BETTING_MONEY)
                {
                    PlaySound(meAlert);
                    return;
                }
                // 돈이 부족하면 베팅할수 없다.
                else if (m_nCurUserMoney - m_nCurScore * CommissionMulti < 0)
                {
                    PlaySound(meAlert);
                    return;
                }
                else
                {
                    // 최대값과 최소값차가 5만을 넘을수 없다.
                    int nMinScore = m_aAreaBetAll[0];

                    for (int i = 1; i < 4; i++)
                    {
                        if (m_aAreaBetAll[i] < nMinScore)
                            nMinScore = m_aAreaBetAll[i];
                    }

                    int nNewScore = m_aAreaBetAll[nArea] + m_nCurScore;
                    
                    if (nNewScore  - nMinScore > 50000)
                    {
                        PlaySound(meAlert);
                        return;
                    }                        
                }

                if (m_wMeChairID == GameDefine.INVALID_CHAIR)
                    return;

                int commissionCash = m_nCurScore * _GameInfo.Commission / 100;

                curBettingMoney += m_nCurScore;
                m_nCurUserMoney -= m_nCurScore + commissionCash;

                if (Login._UserInfo.nCashOrPointGame == 0)
                    lblUserCash_Top.Content = m_nCurUserMoney;
                else
                    lblUserPoint_Top.Content = m_nCurUserMoney;

                lblCurGameMoney.Content = m_nCurUserMoney;
                lblCurBettingMoney.Content = curBettingMoney;

                // added by usc at 2014/03/22
                m_lUserBet[nArea] += m_nCurScore;

                DrawChip(nArea, m_nCurScore);
                PlaySound(meBet);

                m_BettingInfo._Area = nArea;
                m_BettingInfo._Score = m_nCurScore;
                m_BettingInfo._UserIndex = m_wMeChairID;

                // 서버에 현재 베팅정보를 전송한다.
                Login._ClientEngine.Send(NotifyType.Request_Betting, m_BettingInfo);

                UpdateButton();
            }
            else
            {
                PlaySound(meAlert);
            }
        }

#endregion     

        public void IconDownloadComplete(string filePath)
        {
            if (filePath == string.Empty)
                return;

            try
            {
                BitmapImage bImg = new BitmapImage();
                bImg.BeginInit();
                bImg.UriSource = new Uri(filePath);
                bImg.EndInit();

                imgCurUserFace.Source = bImg;
            }
            catch { }
        }

        private void imgVolumn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            m_bSound = !m_bSound;

            BitmapImage bmpImg = new BitmapImage();
            bmpImg.BeginInit();

            if (m_bSound)
                bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/VolumnOpen.png", UriKind.RelativeOrAbsolute);
            else
                bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/VolumnClose.png", UriKind.RelativeOrAbsolute);

            bmpImg.EndInit();

            imgVolumn.Source = bmpImg;

            if (!m_bSound)
                meBackground.Stop();
            else
                meBackground.Play();
        }

        private void rtxtMessageEdit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string strSendMessage = "";
                strSendMessage = InterpretMessage();

                if (strSendMessage.Length > 3000)
                {
                    //ShowErrorOrMessage(ManagerMessage.ERROR_MANY_CHAT);
                    e.Handled = true;
                    return;
                }

                if (e.Key == Key.Enter)
                {
                    if (strSendMessage != null && strSendMessage != string.Empty && strSendMessage != "\r\n")
                        SendMessage(strSendMessage);

                    TextRange textRange = new TextRange(rtxtMessageEdit.Document.ContentStart, rtxtMessageEdit.Document.ContentEnd);
                    textRange.Text = "";
                    rtxtMessageEdit.ScrollToHome();
                    rtxtMessageEdit.CaretPosition = rtxtMessageEdit.Document.ContentStart;

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
                string strError = ex.ToString();
                ErrorCollection.GetInstance().SetErrorInfo(strError);
            }
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

                nFontSize = Convert.ToInt32(this.rtxtMessageEdit.FontSize);
                strFontName = rtxtMessageEdit.FontFamily.Source;

                if (this.rtxtMessageEdit.FontStyle == FontStyles.Italic)
                    strFontStyle = FontInfo.FONT_STYLE_ITALIC;
                else
                    strFontStyle = FontInfo.FONT_STYLE_NORMAL;

                if (this.rtxtMessageEdit.FontWeight == FontWeights.Bold)
                    strFontWeight = FontInfo.FONT_WEIGHT_BOLD;
                else
                    strFontWeight = FontInfo.FONT_WEIGHT_NORMAL;

                strFontColor = rtxtMessageEdit.Foreground.ToString();

                ChatEngine.StringInfo messageInfo = new ChatEngine.StringInfo();

                messageInfo.UserId = Login._UserInfo.Id;
                messageInfo.String = strMessage;
                messageInfo.FontSize = nFontSize;
                messageInfo.FontName = strFontName;
                messageInfo.FontStyle = strFontStyle;
                messageInfo.FontWeight = strFontWeight;
                messageInfo.FontColor = strFontColor;
                messageInfo.strRoomID = Login._UserInfo.GameId;

                Login._ClientEngine.Send(NotifyType.Request_GameChat, messageInfo);
            }
            catch (Exception)
            { }
        }

        private string InterpretMessage()
        {
            FlowDocument flowDocument = new FlowDocument();

            foreach (Block block in rtxtMessageEdit.Document.Blocks)
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

        public void OnReceive(NotifyType type, Socket socket, BaseInfo baseInfo)
        {
            try
            {
                switch (type)
                {
                    case NotifyType.Reply_GameChat:
                        {
                            ChatEngine.StringInfo strInfo = (ChatEngine.StringInfo)baseInfo;

                            AddMessageToHistoryTextBox(strInfo);
                        }
                        break;
                    case NotifyType.Reply_Betting:
                        {
                            if (!(baseInfo is BettingInfo))
                                return;

                            BettingInfo bettingInfo = (BettingInfo)baseInfo;

                            if (m_wMeChairID== bettingInfo._UserIndex || m_DiceInfo == null)
                                return;

                            string strMsg = string.Format("{0} <{1}> 下注", m_DiceInfo._Players[bettingInfo._UserIndex].Nickname, GetUriString(bettingInfo._Score, 0));

                            InterpretServerMessgae("AMDIN_MSG", strMsg, 12, "Segoe UI", "b", "d", "#FFFFFF");
                        }
                        break;
                }
            }
            catch (Exception)
            { }
        }

        private string GetUriString(int nScore, int nType)
        {
            string strUri = string.Empty;
            string strAddUri = string.Empty;

            if (nType > 0)
                strAddUri = string.Format("_{0}", nType);

            strUri = string.Format("/Resources;component/image/Chip{0}{1}.png", nScore, strAddUri);

            return strUri;
        }

        public void AddMessageToHistoryTextBox(ChatEngine.StringInfo messageInfo)
        {
            if (messageInfo.strRoomID.Equals(Login._UserInfo.GameId))
                InterpretServerMessgae(messageInfo.UserId, messageInfo.String, messageInfo.FontSize, messageInfo.FontName, messageInfo.FontStyle, messageInfo.FontWeight, messageInfo.FontColor);
            
        }

        private void InterpretServerMessgae(string strUserID, string strMessage, int nFontSize, string strFontName, string strFontStyle, string strFontWeight, string strFontColor)
        {
            m_ChatViewLine++;

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

        private void AddMessage(string strSenderId, string strReceiveMessage, double fontSize, FontFamily fontName, FontStyle fontStyle, FontWeight fontWeight, Brush fontColor)
        {
            Paragraph para = new Paragraph();

            if (strSenderId.Equals("AMDIN_MSG"))
            {
                Image img = new Image();

                BitmapImage bitMapImage = new BitmapImage();
                bitMapImage.BeginInit();
                bitMapImage.UriSource = new Uri("/Resources;component/image/AdminNotice.png", UriKind.RelativeOrAbsolute);
                bitMapImage.EndInit();

                //img.Source = bitMapImage;
                img.Width = 59;
                WpfAnimatedGif.ImageBehavior.SetAnimatedSource(img, bitMapImage);
                WpfAnimatedGif.ImageBehavior.SetRepeatBehavior(img, System.Windows.Media.Animation.RepeatBehavior.Forever);

                InlineUIContainer ui = new InlineUIContainer(img);

                para.LineHeight = 5;
                para.Inlines.Add(ui);

                Run space = new Run(" ");
                para.Inlines.Add(new Bold(space));

                if (m_ChatViewLine > CHAT_VIEW_LINE)
                    rtxtMessageView.Document.Blocks.Remove(rtxtMessageView.Document.Blocks.FirstBlock);

                m_flowDoc.Blocks.Add(para);
            }
            else if (strSenderId.Equals("PROMPT_MSG"))
            {
                Image img = new Image();

                BitmapImage bitMapImage = new BitmapImage();
                bitMapImage.BeginInit();
                bitMapImage.UriSource = new Uri("/Resources;component/image/PromptNotice.png", UriKind.RelativeOrAbsolute);
                bitMapImage.EndInit();

                img.Width = 59;
                WpfAnimatedGif.ImageBehavior.SetAnimatedSource(img, bitMapImage);
                WpfAnimatedGif.ImageBehavior.SetRepeatBehavior(img, System.Windows.Media.Animation.RepeatBehavior.Forever);

                InlineUIContainer ui = new InlineUIContainer(img);

                para.LineHeight = 5;
                para.Inlines.Add(ui);

                Run space = new Run(" ");
                para.Inlines.Add(new Bold(space));

                if (m_ChatViewLine > CHAT_VIEW_LINE)
                    rtxtMessageView.Document.Blocks.Remove(rtxtMessageView.Document.Blocks.FirstBlock);

                m_flowDoc.Blocks.Add(para);
            }
            else
            {
                string strSenderName = string.Empty;

                int nSeatIndex = GameDefine.INVALID_CHAIR;

                for (int i = 0; i < m_DiceInfo._Players.Count; i++)
                {
                    UserInfo userInfo = m_DiceInfo._Players[i];

                    if (userInfo != null && userInfo.Id == strSenderId)
                    {
                        nSeatIndex = i;
                        strSenderName = "[" + userInfo.Nickname + "] ";
                        break;
                    }
                }

                if (nSeatIndex == GameDefine.INVALID_CHAIR)
                {
                    for (int i = 0; i < m_DiceInfo._Viewers.Count; i++)
                    {
                        UserInfo userInfo = m_DiceInfo._Viewers[i];

                        if (userInfo != null && userInfo.Id == strSenderId)
                        {
                            nSeatIndex = i;
                            strSenderName = "[" + userInfo.Nickname + "] ";
                            break;
                        }
                    }
                }

                if (nSeatIndex == GameDefine.INVALID_CHAIR)
                    return;

                Image img = new Image();

                BitmapImage bitMapImage = new BitmapImage();
                bitMapImage.BeginInit();
                bitMapImage.UriSource = new Uri("/Resources;component/image/UserNotice.png", UriKind.RelativeOrAbsolute);
                bitMapImage.EndInit();

                img.Width = 59;
                WpfAnimatedGif.ImageBehavior.SetAnimatedSource(img, bitMapImage);
                WpfAnimatedGif.ImageBehavior.SetRepeatBehavior(img, System.Windows.Media.Animation.RepeatBehavior.Forever);

                InlineUIContainer ui = new InlineUIContainer(img);

                para.LineHeight = 5;
                para.Inlines.Add(ui);

                Run space = new Run(" ");
                para.Inlines.Add(new Bold(space));

                Run userNickname = new Run(strSenderName);

                if (strSenderId.Equals(Login._UserInfo.Id))
                    userNickname.Foreground = new SolidColorBrush(Color.FromRgb(100, 255, 0));
                else
                    userNickname.Foreground = new SolidColorBrush(Color.FromRgb(235, 0, 235));

                userNickname.FontSize = 12;
                para.LineHeight = 5;
                para.Inlines.Add(new Bold(userNickname));

                if (m_ChatViewLine > CHAT_VIEW_LINE)
                    rtxtMessageView.Document.Blocks.Remove(rtxtMessageView.Document.Blocks.FirstBlock);

                m_flowDoc.Blocks.Add(para);
            }

            string rtf = strReceiveMessage;

            string strMessage = "";

            for (int i = 0; i < rtf.Length; i++)
            {
                if (rtf[i] == '<')
                {
                    if (strMessage != string.Empty)
                    {
                        Run run = new Run(strMessage);
                        run.FontSize = 12;
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

                    img.Width = 20;
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

            this.rtxtMessageView.Document = m_flowDoc;
            this.rtxtMessageView.ScrollToEnd();
        }

        private void btnSendMsg_Click(object sender, RoutedEventArgs e)
        {
            string strSendMessage = InterpretMessage();

            if (strSendMessage != null && strSendMessage != string.Empty && strSendMessage != "\r\n")
                SendMessage(strSendMessage);

            TextRange textRange = new TextRange(rtxtMessageEdit.Document.ContentStart, rtxtMessageEdit.Document.ContentEnd);
            textRange.Text = "";
            rtxtMessageEdit.ScrollToHome();
            rtxtMessageEdit.CaretPosition = rtxtMessageEdit.Document.ContentStart;

            this.rtxtMessageEdit.Focus();
        }

        private void dgUserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Windows.Controls.DataGrid dgGrid = (Microsoft.Windows.Controls.DataGrid)sender;

            if (dgGrid.CurrentColumn.DisplayIndex != 1)
                return;

            try
            {
                FlowDocument flowDoc = new FlowDocument();

                DependencyObject dep = (DependencyObject)e.OriginalSource;
                string strUserID = ((TextBlock)dep).Text;

                if (strUserID.Trim() == Login._UserInfo.Nickname.Trim())
                    return;

                Paragraph para = new Paragraph();
                Run userID = new Run(strUserID + ", ");

                para.Inlines.Add(new Bold(userID));
                flowDoc.Blocks.Add(para);

                rtxtMessageEdit.Document = flowDoc;
                rtxtMessageEdit.Focus();
            }
            catch (System.Exception) { }
        }

        private void imgNotice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_bNotice)
            {
                m_bNotice = true;

                System.Windows.Point pos = PointToScreen(Mouse.GetPosition(this));

                gameNoticeWindow = new GameNotice();
                gameNoticeWindow.SetGameNotice(_GameInfo);
                gameNoticeWindow.Left = pos.X;
                gameNoticeWindow.Top = pos.Y + 29;
                gameNoticeWindow.Show();
            }
            else
            {
                m_bNotice = false;

                if (gameNoticeWindow != null)
                    gameNoticeWindow.Close();
            }
        }

        private void imgNotice_MouseLeave(object sender, MouseEventArgs e)
        {
            m_bNotice = false;

            if (gameNoticeWindow != null)
                gameNoticeWindow.Close();
        }

        private void DrawBettingScore()
        {
            if (m_aAreaBetAll[0] > 0)
            {
                lblDaAllScore.Content = m_aAreaBetAll[0];
                lblDaAllScore.Visibility = Visibility.Visible;

                lblDaSlash.Visibility = Visibility.Visible;

                lblDaScore.Content = m_lUserBet[0];
                lblDaScore.Visibility = Visibility.Visible;
            }
            else
            {
                lblDaAllScore.Visibility = Visibility.Hidden;
                lblDaSlash.Visibility = Visibility.Hidden;
                lblDaScore.Visibility = Visibility.Hidden;
            }

            if (m_aAreaBetAll[1] > 0)
            {
                lblXiaoAllScore.Content = m_aAreaBetAll[1];
                lblXiaoAllScore.Visibility = Visibility.Visible;

                lblXiaoSlash.Visibility = Visibility.Visible;

                lblXiaoScore.Content = m_lUserBet[1];
                lblXiaoScore.Visibility = Visibility.Visible;
            }
            else
            {
                lblXiaoAllScore.Visibility = Visibility.Hidden;
                lblXiaoSlash.Visibility = Visibility.Hidden;
                lblXiaoScore.Visibility = Visibility.Hidden;
            }

            if (m_aAreaBetAll[2] > 0)
            {
                lblDanAllScore.Content = m_aAreaBetAll[2];
                lblDanAllScore.Visibility = Visibility.Visible;

                lblDanSlash.Visibility = Visibility.Visible;

                lblDanScore.Content = m_lUserBet[2];
                lblDanScore.Visibility = Visibility.Visible;
            }
            else
            {
                lblDanAllScore.Visibility = Visibility.Hidden;
                lblDanSlash.Visibility = Visibility.Hidden;
                lblDanScore.Visibility = Visibility.Hidden;
            }

            if (m_aAreaBetAll[3] > 0)
            {
                lblShuangAllScore.Content = m_aAreaBetAll[3];
                lblShuangAllScore.Visibility = Visibility.Visible;

                lblShuangSlash.Visibility = Visibility.Visible;

                lblShuangScore.Content = m_lUserBet[3];
                lblShuangScore.Visibility = Visibility.Visible;
            }
            else
            {
                lblShuangAllScore.Visibility = Visibility.Hidden;
                lblShuangSlash.Visibility = Visibility.Hidden;
                lblShuangScore.Visibility = Visibility.Hidden;
                }
        }

        private void meBackground_MediaEnded(object sender, RoutedEventArgs e)
        {
             PlaySound(meBackground);
        }

        private bool IsLookonMode()
        {
            return m_wMeChairID == GameDefine.INVALID_CHAIR ? true: false;
        }

        private void btnMinimized_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TempWindowForm tempWindowForm = new TempWindowForm();

            if (Main.activeGame != null)
            {
                if (QQMessageBox.Show(tempWindowForm, "您想离开游戏吗？", "提示", QQMessageBoxIcon.Question, QQMessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // 2014-04-01: GreenRose
            // 게임방에서 나간상태를 알린다.
            Login._UserInfo.nUserState = 13;
            Login._ClientEngine.Send(NotifyType.Request_UserState, Login._UserInfo);

            if (timerBetting != null)
            {
                timerBetting.Stop();
                timerBetting = null;
            }

            Main._DiceClient = null;
            Main.activeGame = null;
            Main.u_gamrId = null;

            Login._ClientEngine.Send(NotifyType.Request_OutGame, Login._UserInfo);
            Login._ClientEngine.DetachHandler(NotifyOccured);
            Login._ClientEngine.DetachHandler(OnReceive);

            // added by usc
            meBackground.Stop();
        }
    }
}
