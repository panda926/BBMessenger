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
using System.Windows.Forms.Integration;
using ChatEngine;
//using SicboClient;
//using DzCardClient;
//using HorseClient;
//using BumperCarClient;
using GameControls;
using System.Net.Sockets;

using ControlExs;
using System.Runtime.InteropServices;
using System.Windows.Forms;
//using FishClient;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for GameRoom.xaml
    /// </summary>
    public partial class GameRoom : Window
    {
        public GameInfo _GameInfo;
        public GameView _GameView;

        private Microsoft.Windows.Controls.DataGrid dgUserList = new Microsoft.Windows.Controls.DataGrid();
        private FlowDocument m_flowDoc = new FlowDocument();
        private bool _AutoClose = false;

        private int m_wMeChairID = GameDefine.INVALID_CHAIR;
        private TableInfo m_tableinfo = null;

        private int[] m_aAreaSum = null;

        private GameNotice gameNoticeWindow = null;
        private bool m_bNotice = false;

        private int[] m_lUserBetAll = new int[GameDefine.GAME_PLAYER];

        private const int CHAT_VIEW_LINE = 150;
        private int m_ChatViewLine = 0;

        // added
        private List<Image> m_listCoinImg = new List<Image>();

        Timer m_timer;

        private struct UserBettingInfo
        {
            public string face { set; get; }
            public string id { set; get; }
            public int betscore { set; get; }
            public int score { set; get; }
        }

        public GameRoom(GameInfo gameInfo)
        {
            InitializeComponent();

            _GameInfo = gameInfo;

            this.Width = _GameInfo.Width + 254;
            grdMainParent.Width = this.Width;
            grdMain.Width = _GameInfo.Width + 245;

            this.Height = _GameInfo.Height + 36;
            grdMainParent.Height = this.Height;
            grdMain.Height = _GameInfo.Height;

            string strGameType = string.Empty;

            if (_GameInfo.nCashOrPointGame == 0)
                strGameType = "(余额)";
            else
                strGameType = "(积分)";

            //this.Title = _GameInfo.GameName + strGameType;
            //this.Icon = BitmapFrame.Create(new Uri(_GameInfo.Icon, UriKind.RelativeOrAbsolute));

            lblTitle.Content = _GameInfo.GameName + strGameType;
            imgIcon.Source = BitmapFrame.Create(new Uri(_GameInfo.Icon, UriKind.RelativeOrAbsolute));

            if (_GameInfo.Source == "BumperCar")
                imgNotice.Visibility = Visibility.Hidden;

            int nLeft = (int)(grdMainParent.Width / 2 - imgLoading.Width / 2);

            imgLoading.Margin = new Thickness(nLeft, 0, 0, 0);
        }


        private void InitControls()
        {
            AddDataGrid(panUserListHeader, new Microsoft.Windows.Controls.DataGrid());
            AddDataGrid(panUserList, dgUserList);

            dgUserList.MouseDoubleClick += new MouseButtonEventHandler(dgUserList_MouseDoubleClick);

            // added by usc at 2014/03/25
            if (_GameInfo.Source == "Horse")
            {
                for (int i = 0; i < HorseDefine.AREA_COUNT; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Image imgCoin = new Image();
                        imgCoin.Margin = new Thickness(0, 0, 0, 11 * j + 1);
                        imgCoin.Width = 32;
                        imgCoin.Height = 21;

                        BitmapImage bmpImg = new BitmapImage();

                        bmpImg.BeginInit();

                        string strImgPath = string.Format("/Resources;component/image/Coin.png");
                        bmpImg.UriSource = new Uri(strImgPath, UriKind.RelativeOrAbsolute);
                        bmpImg.EndInit();

                        imgCoin.Source = bmpImg;
                        imgCoin.Name = string.Format("imgCoin{0}_{1}", i, j);

                        imgCoin.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        imgCoin.VerticalAlignment = VerticalAlignment.Bottom;
                        imgCoin.Visibility = Visibility.Hidden;
                        imgCoin.SetValue(Grid.ColumnProperty, i);

                        grdHorseGraph.Children.Add(imgCoin);

                        m_listCoinImg.Add(imgCoin);
                    }
                }
            }
            else
            {
                for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Image imgCoin = new Image();
                        imgCoin.Margin = new Thickness(0, 0, 0, 9 * j + 1);
                        imgCoin.Width = 26;
                        imgCoin.Height = 18;

                        BitmapImage bmpImg = new BitmapImage();

                        bmpImg.BeginInit();

                        string strImgPath = string.Format("/Resources;component/image/Coin.png");
                        bmpImg.UriSource = new Uri(strImgPath, UriKind.RelativeOrAbsolute);
                        bmpImg.EndInit();

                        imgCoin.Source = bmpImg;
                        imgCoin.Name = string.Format("imgCoin{0}_{1}", i, j);

                        imgCoin.VerticalAlignment = VerticalAlignment.Bottom;
                        imgCoin.Visibility = Visibility.Hidden;
                        imgCoin.SetValue(Grid.ColumnProperty, i);

                        grdBumperCarGraph.Children.Add(imgCoin);

                        m_listCoinImg.Add(imgCoin);
                    }
                }
            }
        }

        private void AddDataGrid(StackPanel stackPanel, Microsoft.Windows.Controls.DataGrid dgGrid)
        {
            stackPanel.Children.Clear();

            dgGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
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
            System.Windows.Data.Binding b1 = new System.Windows.Data.Binding("face");
            b1.Mode = BindingMode.TwoWay;
            factory1.SetValue(Image.SourceProperty, b1);

            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = factory1;

            col1.CellTemplate = cellTemplate1;
            col1.Width = 28;
            col1.MinWidth = 28;
            dgGrid.Columns.Add(col1);

            Microsoft.Windows.Controls.DataGridTextColumn col2 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col2.Header = "用户名";
            col2.Width = 90;
            col2.MinWidth = 90;

            col2.Binding = new System.Windows.Data.Binding("id");
            dgGrid.Columns.Add(col2);

            Microsoft.Windows.Controls.DataGridTextColumn col3 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col3.Header = "押注金额";

            col3.Width = 45;// Microsoft.Windows.Controls.DataGridLength.Auto;
            col3.MinWidth = 45;
            col3.Binding = new System.Windows.Data.Binding("betscore");
            col3.ElementStyle = (Style)FindResource("dgElementRightStyle");
            dgGrid.Columns.Add(col3);

            Microsoft.Windows.Controls.DataGridTextColumn col4 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col4.Header = "  金额";

            col4.Width = 50;// Microsoft.Windows.Controls.DataGridLength.Auto;
            col4.MinWidth = 50;
            col4.Binding = new System.Windows.Data.Binding("score");
            col4.ElementStyle = (Style)FindResource("dgElementRightStyle");
            dgGrid.Columns.Add(col4);

            Microsoft.Windows.Controls.DataGridTextColumn col5 = new Microsoft.Windows.Controls.DataGridTextColumn();
            col5.Header = "";

            col5.Width = 25;
            dgGrid.Columns.Add(col5);

            dgGrid.Items.Clear();
            stackPanel.Children.Add(dgGrid);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_AutoClose == false)
            {
                TempWindowForm tempWindowForm = new TempWindowForm();
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

            _GameView.CloseView();

            Main._GameRoom = null;
            Main.activeGame = null;
            Main.u_gamrId = null;

            Login._ClientEngine.Send(NotifyType.Request_OutGame, Login._UserInfo);
            Login._ClientEngine.DetachHandler(_GameView.NotifyOccured);

            Login._ClientEngine.DetachHandler(NotifyOccured);
            Login._ClientEngine.DetachHandler(OnReceive);

            _GameView.SetFrameWindowProc(null);
            _GameView.Dispose();
            _GameView = null;

            host.Child = null;
        }

        public bool GameRoomProc(FrameWindowMessage msgType, object data)
        {
            switch (msgType)
            {
                case FrameWindowMessage.Frame_Close:
                    _AutoClose = true;
                    this.Close();
                    break;
            }

            return true;
        }

        private void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_TableDetail:
                    {
                        if (!(baseInfo is TableInfo))
                            return;

                        TableInfo tableInfo = (TableInfo)baseInfo;

                        m_wMeChairID = GameDefine.INVALID_CHAIR;

                        for (int i = 0; i < tableInfo._Players.Count; i++)
                        {
                            UserInfo userInfo = tableInfo._Players[i];

                            if (userInfo != null && userInfo.Id == Login._UserInfo.Id)
                            {
                                m_wMeChairID = i;
                                break;
                            }
                        }

                        if (tableInfo._RoundIndex > 0)
                        {
                            // added by usc at 2014/03/11
                            Array.Clear(m_lUserBetAll, 0, m_lUserBetAll.Length);

                            if (_GameInfo.Source == "Horse")
                            {
                                HorseInfo horseInfo = (HorseInfo)tableInfo;

                                for (int i = 0; i < HorseDefine.AREA_COUNT; i++)
                                    m_aAreaSum[i] = horseInfo.m_lPlayerBetAll[i];

                                DrawBettingGraph(m_aAreaSum);

                                // added by usc at 2014/03/11
                                for (int i = 0; i < horseInfo._Players.Count; i++)
                                {
                                    for (int j = 0; j < HorseDefine.AREA_COUNT; j++)
                                        m_lUserBetAll[i] += horseInfo.m_lPlayerBet[i, j];
                                }
                            }
                            else
                            {
                                BumperCarInfo bumpercarInfo = (BumperCarInfo)tableInfo;

                                for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
                                    m_aAreaSum[i] = bumpercarInfo.m_lAllJettonScore[i + 1];

                                DrawBettingGraph(m_aAreaSum);

                                // added by usc at 2014/03/11
                                for (int i = 0; i < bumpercarInfo._Players.Count; i++)
                                {
                                    for (int j = 0; j < BumperCarDefine.AREA_COUNT; j++)
                                        m_lUserBetAll[i] += bumpercarInfo.m_lUserJettonScore[j + 1, i];
                                }
                            }

                            if (m_tableinfo == null)
                            {
                                DrawUserList(tableInfo, false);

                                // added by usc at 2014/04/03
                                string strMsg = string.Format("欢迎 {0} 进入房间", Login._UserInfo.Nickname);

                                InterpretServerMessgae("AMDIN_MSG", strMsg, 12, "Segoe UI", "b", "d", "#9adf8c");
                            }
                        }

                        if (m_tableinfo != null && m_tableinfo._RoundIndex == tableInfo._RoundIndex)
                        {
                            m_tableinfo = tableInfo;

                            DrawUserList(tableInfo, false);

                            return;
                        }

                        // added by usc at 2014/03/19
                        //if (m_wMeChairID == GameDefine.INVALID_CHAIR)
                        //    return;

                        m_tableinfo = tableInfo;

                        if (tableInfo._RoundIndex == 0)
                        {
                            // added by usc at 2014/03/11
                            Array.Clear(m_aAreaSum, 0, m_aAreaSum.Length);
                            Array.Clear(m_lUserBetAll, 0, m_lUserBetAll.Length);

                            DrawBettingGraph(m_aAreaSum);

                            InitUserList(tableInfo);
                        }


                    }
                    break;
                case NotifyType.Reply_GameResult:
                    {
                        ChatEngine.StringInfo strInfo = (ChatEngine.StringInfo)baseInfo;

                        if (strInfo != null)
                        {
                            if (strInfo.UserId == Login._UserInfo.Id)
                                DrawUserList(m_tableinfo, true);
                        }
                    }
                    break;
                case NotifyType.Reply_Betting:
                    {
                        if (!(baseInfo is BettingInfo))
                            return;

                        BettingInfo bettingInfo = (BettingInfo)baseInfo;

                        int nArea = 0;
                        if (_GameInfo.Source == "Horse")
                            nArea = bettingInfo._Area;
                        else if (_GameInfo.Source == "BumperCar")
                            nArea = bettingInfo._Area - 1;

                        m_aAreaSum[nArea] += bettingInfo._Score;

                        // added by usc at 2014/03/11
                        m_lUserBetAll[bettingInfo._UserIndex] += bettingInfo._Score;

                        DrawBettingGraph(m_aAreaSum);

                        DrawUserList(m_tableinfo, false);
                    }
                    break;
            }
        }

        private void InitUserList(TableInfo curTableInfo)
        {
            dgUserList.Items.Clear();

            for (int i = 0; i < curTableInfo._Players.Count; i++)
            {
                string strImgPath = string.Format("/Resources;Component/{0}", curTableInfo._Players[i].Icon);

                dgUserList.Items.Add(new UserBettingInfo { face = strImgPath, id = curTableInfo._Players[i].Nickname, betscore = 0, score = 0 });
            }
        }

        private void DrawUserList(TableInfo curTableInfo, bool bWinScore)
        {
            if (curTableInfo == null)
                return;

            dgUserList.Items.Clear();

            if (_GameInfo.Source == "Horse")
            {
                HorseInfo horseInfo = (HorseInfo)curTableInfo;

                for (int i = 0; i < horseInfo._Players.Count; i++)
                {
                    string strImgPath = string.Format("/Resources;Component/{0}", horseInfo._Players[i].Icon);

                    if (bWinScore)
                        dgUserList.Items.Add(new UserBettingInfo { face = strImgPath, id = horseInfo._Players[i].Nickname, betscore = m_lUserBetAll[i], score = horseInfo.m_lUserWinScore[i] });
                    else
                        dgUserList.Items.Add(new UserBettingInfo { face = strImgPath, id = horseInfo._Players[i].Nickname, betscore = m_lUserBetAll[i], score = 0 });
                }
            }
            else
            {
                BumperCarInfo bumperCarInfo = (BumperCarInfo)curTableInfo;

                for (int i = 0; i < bumperCarInfo._Players.Count; i++)
                {
                    string strImgPath = string.Format("/Resources;Component/{0}", bumperCarInfo._Players[i].Icon);

                    if (bWinScore)
                        dgUserList.Items.Add(new UserBettingInfo { face = strImgPath, id = bumperCarInfo._Players[i].Nickname, betscore = m_lUserBetAll[i], score = bumperCarInfo.m_lUserWinScore[i] });
                    else
                        dgUserList.Items.Add(new UserBettingInfo { face = strImgPath, id = bumperCarInfo._Players[i].Nickname, betscore = m_lUserBetAll[i], score = 0 });
                }
            }

            if (bWinScore)
            {
                for (int i = 0; i < curTableInfo._Players.Count; i++)
                {
                    if (curTableInfo.m_lUserWinScore[i] > 0)
                    {
                        string strMsg = string.Format("恭喜 {0} 赢得{1}金币", curTableInfo._Players[i].Nickname, curTableInfo.m_lUserWinScore[i]);

                        InterpretServerMessgae("PROMPT_MSG", strMsg, 12, "Segoe UI", "b", "d", "#ffff00");
                    }
                }
            }
        }

        private void DrawBettingGraph(int[] aAreaSum)
        {
            int nMaxSum = GetMaxValue(aAreaSum);

            if (nMaxSum == 0)
            {
                for (int i = 0; i < m_listCoinImg.Count; i++)
                    m_listCoinImg[i].Visibility = Visibility.Hidden;

            }
            else
            {
                for (int i = 0; i < aAreaSum.Length; i++)
                {
                    int nPercent = (int)((double)aAreaSum[i] / (double)nMaxSum * 100d);

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

        private int GetMaxValue(int[] aNumber)
        {
            int nMaxValue = aNumber[0];

            for (int i = 1; i < aNumber.Length; i++)
                if (nMaxValue < aNumber[i])
                    nMaxValue = aNumber[i];

            return nMaxValue;
        }

        private void rtxtMessageEdit_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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

                            if (m_wMeChairID == bettingInfo._UserIndex || m_tableinfo == null)
                                return;

                            string strMsg = string.Format("{0} <{1}> 下注", m_tableinfo._Players[bettingInfo._UserIndex].Nickname, GetUriString(bettingInfo._Score));

                            InterpretServerMessgae("AMDIN_MSG", strMsg, 12, "Segoe UI", "b", "d", "#FFFFFF");
                        }
                        break;
                }
            }
            catch (Exception)
            { }
        }

        private string GetUriString(int nScore)
        {
            string strUri = string.Empty;
            string strAddUri = string.Empty;

            if (_GameInfo.Source == "BumperCar")
                strAddUri = "_1";

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

                for (int i = 0; i < m_tableinfo._Players.Count; i++)
                {
                    UserInfo userInfo = m_tableinfo._Players[i];

                    if (userInfo != null && userInfo.Id == strSenderId)
                    {
                        nSeatIndex = i;
                        strSenderName = "[" + userInfo.Nickname + "] ";
                        break;
                    }
                }

                if (nSeatIndex == GameDefine.INVALID_CHAIR)
                {
                    for (int i = 0; i < m_tableinfo._Viewers.Count; i++)
                    {
                        UserInfo userInfo = m_tableinfo._Viewers[i];

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
                gameNoticeWindow.Left = pos.X - gameNoticeWindow.Width;
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

        private void imgNotice_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            m_bNotice = false;

            if (gameNoticeWindow != null)
                gameNoticeWindow.Close();
        }

        private void imgVolumn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _GameView.m_bGameSound = !_GameView.m_bGameSound;

            BitmapImage bmpImg = new BitmapImage();
            bmpImg.BeginInit();

            if (_GameView.m_bGameSound)
                bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/VolumnOpen.png", UriKind.RelativeOrAbsolute);
            else
                bmpImg.UriSource = new Uri("/Resources;component/image/diceGame/VolumnClose.png", UriKind.RelativeOrAbsolute);

            bmpImg.EndInit();

            imgVolumn.Source = bmpImg;
        }

        private bool IsLookonMode()
        {
            return m_wMeChairID == GameDefine.INVALID_CHAIR ? true : false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_timer = new System.Windows.Forms.Timer();

            m_timer.Interval = 1000;
            m_timer.Tick += new System.EventHandler(this.Timer_Tick);
            m_timer.Start();
        }

        protected void Timer_Tick(object sender, EventArgs e)
        {
            if (m_timer != null)
            {
                LoadGame();

                m_timer.Stop();
                m_timer = null;
            }
        }

        WindowsFormsHost host = new WindowsFormsHost();

        private void LoadGame()
        {
            string strStartupPath = System.Windows.Forms.Application.StartupPath;
            string strDllPath = string.Empty;

            strDllPath = string.Format("{0}\\Games\\{1}\\{1}Client.dll", strStartupPath, _GameInfo.Source);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(strDllPath);

            string assamblName = string.Format("{0}Client.{0}View", _GameInfo.Source);
            _GameView = assembly.CreateInstance(assamblName) as GameView;

            if (_GameView == null)
                return;

            InitControls();

            switch (_GameInfo.Source)
            {
                case "Horse":
                    {
                        grdGameNotice.Visibility = Visibility.Visible;
                        grdBumperCarGraph.Visibility = Visibility.Hidden;
                        grdHorseGraph.Visibility = Visibility.Visible;
                        grdUserList.Visibility = Visibility.Visible;
                        grdChatEdit.Visibility = Visibility.Visible;

                        m_aAreaSum = new int[HorseDefine.AREA_COUNT];
                    }
                    break;

                case "BumperCar":
                    {
                        grdGameNotice.Visibility = Visibility.Visible;
                        grdHorseGraph.Visibility = Visibility.Hidden;
                        grdBumperCarGraph.Visibility = Visibility.Visible;
                        grdUserList.Visibility = Visibility.Visible;
                        grdChatEdit.Visibility = Visibility.Visible;                        

                        m_aAreaSum = new int[BumperCarDefine.AREA_COUNT];                        
                    }
                    break;
            }

            _GameView.Width = _GameInfo.Width;
            _GameView.Height = _GameInfo.Height;

            // added by usc at 2014/03/19
            _GameView.m_bGameSound = true;

            host.Child = _GameView;

            MyExpander.Content = host;
            MyExpander.IsExpanded = true;

            // added by usc at 2014/01/17
            MyExpander.Margin = new System.Windows.Thickness(0, -22, 0, 0);

            //this.Content = MyExpander;
            imgLoading.Visibility = Visibility.Hidden;

            _GameView.SetClientSocket(Login._ClientEngine);
            _GameView.SetUserInfo(Login._UserInfo);
            _GameView.SetGameInfo(_GameInfo);
            _GameView.SetFrameWindowProc(GameRoomProc);

            Login._ClientEngine.AttachHandler(OnReceive);
            Login._ClientEngine.AttachHandler(NotifyOccured);

            Login._ClientEngine.Send(NotifyType.Request_PlayerEnter, Login._UserInfo);  
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
    }
}
