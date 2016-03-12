using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameControls;
using ChatEngine;
using System.Net.Sockets;
using System.IO;

namespace HorseClient
{
    public enum HorseGameStatus
    {
        GS_FREE = 0,
        GS_BET,
        GS_BET_END,
        GS_HORSES,
        GS_END,

        GS_EMPTY = 100
    }

    public partial class HorseView : GameView
    {
        //定时标识		
        const int IDI_HORSES_START = 101;							//赛马开始
        const int IDI_HORSES_END = 102;							//赛马结束
        const int IDI_PLAY_SOUNDS = 103;							//播放声音
        const int IDI_RENDER = 105;

        // added at 2014/01/06
        private const int IDC_CHIP_BUTTON_100 = 200;
        private const int IDC_CHIP_BUTTON_1W = 201;
        private const int IDC_CHIP_BUTTON_10W = 202;
        private const int IDC_CHIP_BUTTON_100W = 203;

        //基础信息
        int m_nStreak;							//场次
        int m_wKindID;							//游戏ID
        tagBASE m_tagBenchmark;						//基准结构
        int m_nBetEndTime;						//下注结束时间

        //游戏变量
        int m_wMeChairID;						//我的位置
        HorseGameStatus m_cbGameStatus;						//设置状态
        bool m_bRacing;							//正在赛马
        bool m_bMeOrAll;							//显示自己还是全部(true为Me, false为All)

        //游戏信息
        int[] m_nMultiple = new int[HorseDefine.AREA_ALL];				//区域倍数
        int[] m_bRanking = new int[HorseDefine.RANKING_NULL];			//名次

        //分数
        int m_nBetPlayerCount;					//下注人数
        int[] m_lPlayerBet = new int[HorseDefine.AREA_ALL];				//玩家下注
        int[] m_lPlayerBetAll = new int[HorseDefine.AREA_ALL];			//所有下注
        int m_lPlayerWinning;					//玩家输赢

        //大小
        CSize m_szTotalSize;						//总大小
        CSize m_szBackdrop;						//背景中大小
        CSize m_szBackdropHand;					//背景头大小
        CSize m_szBackdropTai;					//背景尾大小

        //马匹
        int[,] m_nHorsesSpeed = new int[HorseDefine.HORSES_ALL, HorseDefine.STEP_SPEED];	//每匹马的每秒速度(标准值)
        int[] m_nHorsesSpeedIndex = new int[HorseDefine.HORSES_ALL];	//每匹马速度索引
        int m_nHorsesBasicSpeed;				//马匹基础速度
        int m_nInFrameDelay;					//马匹换帧延迟
        int[] m_cbHorsesIndex = new int[HorseDefine.HORSES_ALL];		//马索引
        string[] m_szHorsesName = new string[HorseDefine.HORSES_ALL];	//马匹名字
        CPoint[] m_ptHorsesPos = new CPoint[HorseDefine.HORSES_ALL];			//马位置
        CMyD3DTexture[] m_ImageHorses = new CMyD3DTexture[HorseDefine.HORSES_ALL];			//马
        //CDirectSound			m_HorsesSound;						//马匹声音
        //CDirectSound			m_GameOverSound;					//马匹声音

        //游戏结束
        bool m_bGameOver;						//游戏结束
        CPoint m_ptGameOver;						//位置
        CMyD3DTexture m_ImageGameOver;					//地图
        CSkinButton m_btGameOver = new CSkinButton();						//按钮

        //背景
        CMyD3DTexture m_ImageBackdropHand;				//背景头
        CMyD3DTexture m_ImageBackdropTail;				//背景尾
        CMyD3DTexture m_ImageBackdrop;					//背景中
        CMyD3DTexture m_ImageDistanceFrame;				//距离

        //状态栏
        CMyD3DTexture m_ImageUserInfoL;					//左
        CMyD3DTexture m_ImageChipPanel;                  // Chip Panel
        //CMyD3DTexture m_ImageUserInfoM;					//中
        //CMyD3DTexture m_ImageUserInfoR;					//右
        //CMyD3DTexture m_ImageUserInfoShu;					//中竖线
        CMyD3DTexture m_ImageBettingPanel;                  // Betting Panel

        //遮掩栏
        CMyD3DTexture m_ImagePenHand;						//围栏头
        CMyD3DTexture m_ImagePenNum;						//围栏数字
        CMyD3DTexture m_ImagePenTail;						//围栏尾

        //时间
        CMyD3DTexture m_ImageTimeNumer;					//数字
        CMyD3DTexture m_ImageTimeBet;						//下注时间
        CMyD3DTexture m_ImageTimeBetEnd;					//下注结束
        CMyD3DTexture m_ImageTimeFree;					//空闲时间
        CMyD3DTexture m_ImageTimeHorseEnd;				//跑马结束
        CMyD3DTexture m_ImageTimeHorse;					//跑马

        //控件变量
        CDialogPlayBet m_DlgPlayBet = new CDialogPlayBet();						//下注窗口
        DialogRecord m_DlgRecord = new DialogRecord();						//游戏记录
        DialogStatistics m_DlgStatistics = new DialogStatistics();					//统计窗口
        DialogBetRecord m_DlgBetRecord = new DialogBetRecord();						//记录窗口
        //CUserFaceWnd			m_WndUserFace;						//玩家脸部
        DialogControl m_DlgControl = new DialogControl();						//控制窗口
        //CButton					m_btOpenControl = new CButton();					//系统控制

        //按钮
        CSkinButton m_btPlayerBetShow = new CSkinButton();					//显示个人投注
        CSkinButton m_btAllBetShow = new CSkinButton();						//显示全场投注

        CSkinButton m_btPlayerBet = new CSkinButton();						//个人投注
        CSkinButton m_btStatistics = new CSkinButton();						//统计
        //CSkinButton				m_btExplain = new CSkinButton();						//说明

        //CSkinButton				m_btRecord = new CSkinButton();							//历史记录
        //CSkinButton				m_btBetRecord = new CSkinButton();						//下注记录



        //背景资源
        CMyD3DTexture m_TextureBack;						//背景资源

        CMy3DFont m_D3DFont;

        const int INT_MAX = 2147483647;    /* maximum (signed) int value */
        const int INVALID_WORD = 0xFFFF;					//无效数值

        public HorseInfo m_ReceiveInfo = null;
        public TimerInfo m_RenderTimer = null;


        // added at 2014/01/04
        PictureButton m_btBet1000 = new PictureButton();				//下注100
        PictureButton m_btBet1W = new PictureButton();					//下注1000
        PictureButton m_btBet10W = new PictureButton();					//下注1W
        PictureButton m_btBet100W = new PictureButton();				//下注10W

        int m_nCurScore;				                                        //当前注数
        CRect[] m_RectArea = new CRect[6];
        CRect[] m_RectFlashArea = new CRect[6];

        int m_FlashCount = 0;

        // int[] m_lUserChipScore = new int[HorseDefine.AREA_ALL + 1];

        List<tagChipInfo>[] m_aChipInfos = new List<tagChipInfo>[HorseDefine.AREA_ALL];		//筹码数组

        CPngImage m_ImageChipView;
        string m_SoundFolder;

        // added by usc at 2014/03/11
        CMyD3DTexture m_ImageUserFace;
        CMyD3DTexture m_ImageCash;
        CMyD3DTexture m_ImagePoint;

        // added by usc at 2014/03/12
        int m_nHorseTime = 0;
        DateTime m_BetStartTime;

        // added by usc at 2014/03/21
        CMyD3DTexture m_ImageScoreNumber;					//数字
        CMyD3DTexture m_ImageRank1;
        CMyD3DTexture m_ImageRank2;

        // added by usc at 2014/04/01
        Image m_bmpBettingPanel;

        public string TEXT(string str)
        {
            return str;
        }

        public void ShowWindow(bool isVisible)
        {
            this.Visible = isVisible;
        }

        private Bitmap GetImage(string fileName)
        {
            Bitmap resultBmp = null;

            try
            {
                string fullPath = string.Format("{0}Games\\Horse\\image\\{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

                using (MemoryStream ms = FileEncoder.DecryptToMemory(fullPath))
                {
                    resultBmp = (Bitmap)Image.FromStream(ms);
                }

            }
            catch { }

            return resultBmp;
        }

        private Cursor GetCursor(string fileName)
        {
            Cursor resultCursor = null;

            try
            {
                string fullPath = string.Format("{0}Games\\Horse\\cursor\\{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

                using (MemoryStream ms = FileEncoder.DecryptToMemory(fullPath))
                {
                    resultCursor = GameGraphics.LoadCursorFromResource(ms.ToArray());
                }
            }
            catch { }            

            return resultCursor;
        }

        public HorseView()
        {
            InitializeComponent();

            m_SoundFolder = AppDomain.CurrentDomain.BaseDirectory + "Games\\Horse\\sound\\";

            //this.SetStyle(ControlStyles.DoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.UserPaint, true);

            //基础信息
            m_nStreak = INT_MAX;
            m_wKindID = INVALID_WORD;
            //ZeroMemory(&m_tagBenchmark, sizeof(m_tagBenchmark));
            m_nBetEndTime = 0;

            //游戏变量
            m_wMeChairID = GameDefine.INVALID_CHAIR;
            m_cbGameStatus = HorseGameStatus.GS_EMPTY;
            m_bMeOrAll = true;
            m_bRacing = false;

            //游戏信息
            for (int i = 0; i < m_nMultiple.Length; ++i)
                m_nMultiple[i] = 4;

            for (int i = 0; i < m_bRanking.Length; ++i)
                m_bRanking[i] = HorseDefine.HORSES_ALL;

            //马
            //ZeroMemory(m_cbHorsesIndex, sizeof(m_cbHorsesIndex));
            //ZeroMemory(m_ptHorsesPos, sizeof(m_ptHorsesPos));
            //ZeroMemory(m_nHorsesSpeed, sizeof(m_nHorsesSpeed));
            //ZeroMemory(m_nHorsesSpeedIndex, sizeof(m_nHorsesSpeedIndex));
            //ZeroMemory(m_szHorsesName, sizeof(m_szHorsesName));
            m_nHorsesBasicSpeed = HorseDefine.HAND_LENGTH;
            m_nInFrameDelay = 0;

            //游戏结束
            m_bGameOver = false;

            //游戏分数
            m_nBetPlayerCount = 0;
            //memset(m_lPlayerBet, 0, sizeof(m_lPlayerBet));
            //memset(m_lPlayerBetAll, 0, sizeof(m_lPlayerBetAll));
            m_lPlayerWinning = 0;

            m_ptHorsesPos[HorseDefine.HORSES_ONE].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_ONE_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_TWO].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_TWO_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_THREE].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_THREE_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_FOUR].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_FOUR_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_FIVE].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_FIVE_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_SIX].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_SIX_Y_POS);


            Control hInstance = this;
            CRect CreateRect = new CRect(0, 0, 0, 0);

            ////玩家脸部
            //if( m_WndUserFace.GetSafeHwnd() == null )
            //{
            //	m_WndUserFace.Create(null, null, WS_CHILD|WS_VISIBLE, CreateRect, this, 1001);
            //	m_WndUserFace.SetFrameView(this);
            //}

            //下注控件
            //if( m_DlgPlayBet.GetSafeHwnd() == null )
            //m_DlgPlayBet = new CDialogPlayBet();

            //历史记录
            //if( m_DlgRecord.GetSafeHwnd() == null )
            //m_DlgRecord = new DialogRecord();  

            //统计窗口
            //if( m_DlgStatistics.GetSafeHwnd() == null )
            //m_DlgStatistics = new DialogStatistics(); //.Create(IDD_DIALOG_TONGJI,this);

            //记录窗口
            //if( m_DlgBetRecord.GetSafeHwnd() == null )
            //m_DlgBetRecord = new DialogBetRecord(); //.Create(IDD_DIALOG_BET_LOG,this);

            //按钮
            m_btPlayerBetShow.Create(TEXT(""), true, false, CreateRect, this, 0);
            m_btAllBetShow.Create(TEXT(""), true, true, CreateRect, this, 0);
            m_btPlayerBet.Create(TEXT(""), true, true, CreateRect, this, 0);
            m_btStatistics.Create(TEXT(""), true, true, CreateRect, this, 0);
            //m_btExplain.Create(TEXT(""),true,true,CreateRect,this,0);
            //m_btRecord.Create(TEXT(""),true, true,CreateRect,this,0);
            m_btGameOver.Create(TEXT(""), false, true, CreateRect, this, 0);
            //m_btBetRecord.Create(TEXT(""),true, true, CreateRect,this,0);

            // added
            Rectangle rcCreate = new Rectangle();

            m_btBet1000.Create(true, false, rcCreate, this, IDC_CHIP_BUTTON_100);
            m_btBet1000.Click += OnBnClickedButton1000;
            m_btBet1W.Create(true, false, rcCreate, this, IDC_CHIP_BUTTON_1W);
            m_btBet1W.Click += OnBnClickedButton1w;
            m_btBet10W.Create(true, false, rcCreate, this, IDC_CHIP_BUTTON_10W);
            m_btBet10W.Click += OnBnClickedButton10w;
            m_btBet100W.Create(true, false, rcCreate, this, IDC_CHIP_BUTTON_100W);
            m_btBet100W.Click += OnBnClickedButton100w;

            /*
	        m_btPlayerBetShow.SetButtonImage(GetImage( "BT_GEREN,hInstance,false,false);
	        m_btAllBetShow.SetButtonImage(GetImage( "BT_QUANCHANG,hInstance,false,false);
	        m_btPlayerBet.SetButtonImage(GetImage( "BT_XIAZHU,hInstance,false,false);
	        m_btStatistics.SetButtonImage(GetImage( "BT_TONGJI,hInstance,false,false);
            */
            //m_btExplain.SetButtonImage(GetImage( "BT_SHUOMING,hInstance,false,false);
            //m_btRecord.SetButtonImage(GetImage( "BT_HISTORYSCORE,hInstance,false,false);
            //m_btGameOver.SetButtonImage(GetImage("BT_CLOSEGAMESCORE.bmp"),hInstance,false,false);
            //m_btBetRecord.SetButtonImage(GetImage( "BT_JILU,hInstance,false,false);

            // added at 2014/01/04
            m_btBet1000.SetButtonImage(GetImage("BT_JETTON_100.BMP"));
            m_btBet1W.SetButtonImage(GetImage("BT_JETTON_1000.BMP"));
            m_btBet10W.SetButtonImage(GetImage("BT_JETTON_10000.BMP"));
            m_btBet100W.SetButtonImage(GetImage("BT_JETTON_100000.BMP"));

            Color transColor = Color.FromArgb(255, 255, 255);

            m_btBet1000.SetTransparentColor(transColor);
            m_btBet1W.SetTransparentColor(transColor);
            m_btBet10W.SetTransparentColor(transColor);
            m_btBet100W.SetTransparentColor(transColor);

            m_ImageChipView.LoadFromResource(hInstance, GetImage("CHIP_VIEW.png"));
            m_ImageChipView.SetTransparentColor(transColor);


            m_btAllBetShow._control.Click += OnAllBetShow;
            m_btPlayerBetShow._control.Click += OnPlayerBetShow;
            m_btPlayerBet._control.Click += OnPlayerBet;
            //m_btRecord._control.Click += OnRecordShow;
            m_btStatistics._control.Click += OnStatistics;
            m_btGameOver._control.Click += OnGameOverClose;
            //m_btBetRecord._control.Click += OnBetRecordShow;
            //m_btExplain._control.Click += OnExplain;


            //马匹声音
            //m_HorsesSound.Create(TEXT("HOS"));
            //m_GameOverSound.Create(TEXT("WAV_GAME_OVER"));

            //m_btOpenControl.Create(null,true, true, new CRect(4,4,11,11),this,0);
            //m_btOpenControl.ShowWindow(false);
            //m_btOpenControl._control.Click += OpenControlWnd;

            // added at 2014/01/04           
            for (int i = 0; i < m_aChipInfos.Length; i++)
            {
                m_aChipInfos[i] = new List<tagChipInfo>();
            }

            m_bLoaded = true;
        }


        //重置界面
        void ResetGameView()
        {
            //效验数据
            //ASSERT(wDataSize == sizeof(CMD_S_HorsesEnd));
            //if (wDataSize != sizeof(CMD_S_HorsesEnd)) return false;

            //消息处理
            //CMD_S_HorsesEnd* pHorsesEnd = (CMD_S_HorsesEnd*)pBuffer;

            //设置时间
            //SetGameClock(GetMeChairID(), IDI_FREE, pHorsesEnd->nTimeLeave);
            SetUserClock(GetMeChairID(), HorseInfo.IDI_FREE, m_ReceiveInfo.m_nFreeTime);

            //保存记录
            if (m_ReceiveInfo.m_GameRecords[0].nStreak != 0)
            {
                bool bAdd = true;

                if (m_DlgRecord.m_GameRecords.Count > 0)
                {
                    if (m_DlgRecord.m_GameRecords[m_DlgRecord.m_GameRecords.Count - 1].nStreak == m_ReceiveInfo.m_GameRecords[0].nStreak)
                        bAdd = false;
                }

                if (bAdd == true)
                {
                    m_DlgRecord.m_GameRecords.Add(m_ReceiveInfo.m_GameRecords[0]);

                    if (m_DlgRecord.m_GameRecords.Count() > HorseDefine.MAX_SCORE_HISTORY)
                    {
                        m_DlgRecord.m_GameRecords.RemoveAt(0);
                    }
                    if (m_DlgRecord.GetSafeHwnd() && m_DlgRecord.IsWindowVisible())
                    {
                        m_DlgRecord.Invalidate(false);
                    }
                }
            }

            //保存下注记录
            if (m_wMeChairID != GameDefine.INVALID_CHAIR)
            {
                for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
                {
                    if (m_lPlayerBet[i] > 0)
                    {
                        BetRecordInfo Info = new BetRecordInfo();
                        Info.nStreak = m_ReceiveInfo.m_GameRecords[0].nStreak;
                        Info.nHours = m_ReceiveInfo.m_GameRecords[0].nHours;
                        Info.nMinutes = m_ReceiveInfo.m_GameRecords[0].nMinutes;
                        Info.nRanking = i;
                        Info.lBet = m_lPlayerBet[i];
                        Info.lWin = m_ReceiveInfo.m_lPlayerWinning[m_wMeChairID];
                        m_DlgBetRecord.AddInfo(Info);
                    }
                }
            }

            //设置场次
            SetStreak(m_ReceiveInfo.m_nStreak);

            //全天赢的场次
            m_DlgStatistics.SetWinCount(m_ReceiveInfo.m_nWinCount);

            //保存倍数
            //memcpy(m_nMultiple, pHorsesEnd->nMultiple, sizeof(m_nMultiple));
            // modified by usc at 2014/01/07
            //m_nMultiple = m_ReceiveInfo.m_nMultiple;

            //m_DlgPlayBet.SetMultiple(m_ReceiveInfo.m_nMultiple);

            //设置状态
            //SetGameStatus(GS_FREE);
            //m_GameClientView.SetGameStatus(GS_FREE);
            m_DlgPlayBet.SetCanBet(false);

            //更新马匹
            NweHorses();

            //游戏分数
            //memset(m_lPlayerBet, 0, sizeof(m_lPlayerBet));
            //memset(m_lPlayerBetAll, 0, sizeof(m_lPlayerBetAll));

            // added at 2014/01/04
            for (int i = 0; i < HorseDefine.AREA_ALL; i++)
                m_aChipInfos[i].Clear();

            // added by usc at 2014/04/01
            DrawBettingPanel();

            return;

            ////基础信息
            //m_wKindID = INVALID_WORD;

            ////游戏变量
            //m_bRacing = false;
            //m_tagBenchmark = new tagBASE();
            ////ZeroMemory(&m_tagBenchmark, sizeof(m_tagBenchmark));

            //for ( int i = 0; i < m_bRanking.Length; ++i)
            //    m_bRanking[i] = HorseDefine.HORSES_ALL;

            //m_nBetEndTime = 0;
            //m_bMeOrAll = true;

            ////马
            //ZeroMemory(m_cbHorsesIndex);
            //ZeroMemory(m_ptHorsesPos);
            //ZeroMemory(m_nHorsesSpeed);
            //ZeroMemory(m_nHorsesSpeedIndex);
            //m_nHorsesBasicSpeed = HorseDefine.HAND_LENGTH;

            ////游戏分数
            //m_nBetPlayerCount = 0;
            //ZeroMemory(m_lPlayerBet);
            //ZeroMemory(m_lPlayerBetAll);
            //m_lPlayerWinning = 0;

            ////游戏结束
            //m_bGameOver = false;
        }

        //调整控件
        void RectifyControl(int nWidth, int nHeight)
        {
            try
            {
                if (m_tagBenchmark.ptBase.x == m_szTotalSize.cx - m_tagBenchmark.nWidth)
                {
                    m_tagBenchmark.ptBase.x = m_szTotalSize.cx - nWidth;
                }
                m_tagBenchmark.nWidth = nWidth;
                m_tagBenchmark.nHeight = nHeight;


                //if ( m_DlgPlayBet.GetSafeHwnd() )
                m_DlgPlayBet.Visible = false;


                //if ( m_DlgRecord.GetSafeHwnd() )
                m_DlgRecord.Visible = false;

                //if ( m_DlgStatistics.GetSafeHwnd() )
                m_DlgStatistics.Visible = false;

                //if ( m_DlgBetRecord.GetSafeHwnd() )
                m_DlgBetRecord.Visible = false;

                ////脸部
                //if ( m_WndUserFace.GetSafeHwnd() )
                //	m_WndUserFace.MoveWindow(18, m_tagBenchmark.nHeight - m_ImageUserInfoL.GetHeight() + 20, 34, 34);

                //结束游戏位置
                m_ptGameOver.SetPoint(m_tagBenchmark.nWidth / 2 - m_ImageGameOver.GetWidth() / 2, m_tagBenchmark.nHeight / 2 - m_ImageGameOver.GetHeight() / 2);

                //按钮
                Rectangle rcButton = new Rectangle();
                IntPtr hDwp = GameGraphics.BeginDeferWindowPos(32);
                const uint uFlags = GameGraphics.SWP_NOACTIVATE | GameGraphics.SWP_NOZORDER | GameGraphics.SWP_NOCOPYBITS | GameGraphics.SWP_NOSIZE;

                //m_btPlayerBetShow.GetWindowRect(&rcButton);
                rcButton.Width = 63;
                rcButton.Height = 16;

                if (m_btPlayerBetShow._control != null)
                {
                    GameGraphics.DeferWindowPos(hDwp, m_btPlayerBetShow._control.Handle, 0, 190, nHeight - 104, 0, 0, uFlags);
                    GameGraphics.DeferWindowPos(hDwp, m_btAllBetShow._control.Handle, 0, 190 + rcButton.Width, nHeight - 104, 0, 0, uFlags);
                }

                //m_btPlayerBet.GetWindowRect(&rcButton);
                rcButton.Width = 107;
                rcButton.Height = 26;

                GameGraphics.DeferWindowPos(hDwp, m_btPlayerBet.Handle, 0, m_ImageUserInfoL.GetWidth() + (nWidth - m_ImageUserInfoL.GetWidth()) / 3 / 2 - rcButton.Width / 2, nHeight - 134, 0, 0, uFlags);
                GameGraphics.DeferWindowPos(hDwp, m_btStatistics.Handle, 0, m_ImageUserInfoL.GetWidth() + (nWidth - m_ImageUserInfoL.GetWidth()) / 3 * 1 + (nWidth - m_ImageUserInfoL.GetWidth()) / 3 / 2 - rcButton.Width / 2, nHeight - 134, 0, 0, uFlags);
                //GameGraphics.DeferWindowPos(hDwp, m_btExplain.Handle, 0, m_ImageUserInfoL.GetWidth() +  (nWidth - m_ImageUserInfoL.GetWidth()) / 3 * 2 + (nWidth - m_ImageUserInfoL.GetWidth()) / 3 / 2 - rcButton.Width/2, nHeight - 134, 0, 0, uFlags);

                GameGraphics.DeferWindowPos(hDwp, m_btGameOver.Handle, 0, m_ptGameOver.x + 493, m_ptGameOver.y + 43, 0, 0, uFlags);

                // added by usc at 2014/02/04
                //GameGraphics.DeferWindowPos(hDwp, m_btRecord.Handle, 0, nWidth - 115, 5, 0, 0, uFlags);
                //GameGraphics.DeferWindowPos(hDwp, m_btBetRecord.Handle, 0, 42, nHeight - 50, 0, 0, uFlags);

                // added by usc at 2014/01/04
                GameGraphics.DeferWindowPos(hDwp, m_btBet1000.Handle, 0, m_ImageUserInfoL.GetWidth() + m_ImageChipPanel.GetWidth() / 2 - m_btBet1000.Width / 2, nHeight - 134, 0, 0, uFlags);
                GameGraphics.DeferWindowPos(hDwp, m_btBet1W.Handle, 0, m_ImageUserInfoL.GetWidth() + m_ImageChipPanel.GetWidth() / 2 - m_btBet1000.Width / 2, nHeight - 101, 0, 0, uFlags);
                GameGraphics.DeferWindowPos(hDwp, m_btBet10W.Handle, 0, m_ImageUserInfoL.GetWidth() + m_ImageChipPanel.GetWidth() / 2 - m_btBet1000.Width / 2, nHeight - 68, 0, 0, uFlags);
                GameGraphics.DeferWindowPos(hDwp, m_btBet100W.Handle, 0, m_ImageUserInfoL.GetWidth() + m_ImageChipPanel.GetWidth() / 2 - m_btBet1000.Width / 2, nHeight - 35, 0, 0, uFlags);

                GameGraphics.EndDeferWindowPos(hDwp);

                double fRectWidth = (double)(m_ImageBettingPanel.GetWidth()) / 6.0 + 1;

                for (int i = 0; i < HorseDefine.AREA_ALL; i++)
                {
                    m_RectArea[i].top = nHeight - m_ImageChipPanel.GetHeight() + 20;
                    m_RectArea[i].left = m_ImageUserInfoL.GetWidth() + m_ImageChipPanel.GetWidth() + i * (int)fRectWidth;
                    m_RectArea[i].bottom = m_RectArea[i].top + 100;
                    m_RectArea[i].right = m_RectArea[i].left + (int)fRectWidth;

                    m_RectFlashArea[i].top = nHeight - m_ImageChipPanel.GetHeight() + 1;
                    m_RectFlashArea[i].left = m_ImageUserInfoL.GetWidth() + m_ImageChipPanel.GetWidth() + i * (int)fRectWidth + 1;
                    m_RectFlashArea[i].bottom = nHeight - 1;
                    m_RectFlashArea[i].right = m_RectFlashArea[i].left + (int)fRectWidth - 1;
                }
            }
            catch (Exception)
            {

            }
        }

        //绘画界面
        void DrawGameView(Graphics g, int nWidth, int nHeight)
        {
            m_tagBenchmark.nWidth = nWidth;
            m_tagBenchmark.nHeight = nHeight;

            //TRACE(TEXT("Horse DrawGameView\n"));

            //背景
            DrawBackdrop(g);

            //马
            DrawHorses(g);

            //时间提示
            DrawTimeClew(g);

            //玩家信息
            DrawUserInfo(g);

            // added by usc at 2014/04/01
            DrawBettingInfo(g);

            //结束信息
            DrawGameOver(g);            
        }

        //void DrawChips(Graphics pDC)
        //{
        //    for (int i = 0; i < HorseDefine.AREA_ALL; i++)
        //    {
        //        for (int j = 0; j < m_aChipInfos[i].Count; j++)
        //        {
        //            tagChipInfo pChipInfo = m_aChipInfos[i][j];

        //            int nChipIndex = 0;

        //            switch (pChipInfo.nScore)
        //            {
        //                case 100:
        //                    nChipIndex = 0;
        //                    break;
        //                case 1000:
        //                    nChipIndex = 1;
        //                    break;
        //                case 10000:
        //                    nChipIndex = 2;
        //                    break;
        //                case 100000:
        //                    nChipIndex = 3;
        //                    break;
        //            }

        //            //绘画界面
        //            m_ImageChipView.DrawImage(pDC, pChipInfo.nXPos, pChipInfo.nYPos, 30, 30, nChipIndex * 30, 0);
        //        }
        //    }
        //}

        //绘画背景
        void DrawBackdrop(Graphics g)
        {
            //背景
            m_ImageBackdropHand.DrawImage(g, m_tagBenchmark, 0, 0);
            int nBackX = m_ImageBackdropHand.GetWidth();

            for (int i = 0; i < HorseDefine.BACK_COUNT; ++i)
            {
                m_ImageBackdrop.DrawImage(g, m_tagBenchmark, nBackX, 0);
                nBackX += m_ImageBackdrop.GetWidth();
            }
            m_ImageBackdropTail.DrawImage(g, m_tagBenchmark, m_szTotalSize.cx - m_ImageBackdropTail.GetWidth(), 0);
        }

        //绘画马
        void DrawHorses(Graphics g)
        {
            //围栏起始
            int nPenX = 123;

            //马
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                int nHorsesIndex = HorseDefine.HORSES_ALL - nHorses - 1;
                m_ImageHorses[nHorsesIndex].DrawImage(g, m_tagBenchmark, m_ptHorsesPos[nHorsesIndex].x, m_ptHorsesPos[nHorsesIndex].y,
                                            m_ImageHorses[nHorsesIndex].GetWidth() / 5, m_ImageHorses[nHorsesIndex].GetHeight(),
                                            m_ImageHorses[nHorsesIndex].GetWidth() / 5 * m_cbHorsesIndex[nHorsesIndex], 0);

                //名字
                //if ( m_bRacing )
                //{
                //	DrawTextString(g, m_szHorsesName[nHorsesIndex], Color.FromArgb(255,20,20), Color.FromArgb(230,230,230),
                //		m_ptHorsesPos[nHorsesIndex].x + m_ImageHorses[nHorsesIndex].GetWidth() / 5 / 2 + 35, m_ptHorsesPos[nHorsesIndex].y + 8);
                //}

                //围栏数
                m_ImagePenNum.DrawImage(g, m_tagBenchmark, nPenX + m_ImagePenHand.GetWidth(), m_ptHorsesPos[nHorsesIndex].y + 75,
                                        m_ImagePenNum.GetWidth() / 6, m_ImagePenNum.GetHeight(),
                                        m_ImagePenNum.GetWidth() / 6 * nHorsesIndex, 0);

            }

            //距离
            int nXPos = (m_szTotalSize.cx - HorseDefine.HAND_LENGTH - HorseDefine.TAIL_LENGTH) / 12;
            for (int i = 0; i < 11; i++)
            {
                m_ImageDistanceFrame.DrawImage(g, m_tagBenchmark, nXPos, 429,
                    m_ImageDistanceFrame.GetWidth() / 11, m_ImageDistanceFrame.GetHeight(),
                    m_ImageDistanceFrame.GetWidth() / 11 * i, 0);
                nXPos += (m_szTotalSize.cx - HorseDefine.HAND_LENGTH - HorseDefine.TAIL_LENGTH) / 12;
            }

            //围栏头
            m_ImagePenHand.DrawImage(g, m_tagBenchmark, nPenX, 131);

            //围栏尾
            m_ImagePenTail.DrawImage(g, m_tagBenchmark, m_szTotalSize.cx - 239, 56);
        }

        //绘画时间提示
        void DrawTimeClew(Graphics g)
        {
            if (m_nStreak == INT_MAX)
                return;

            // modified by usc at 2014/03/12
            int nUserTime = GetUserClock(m_wMeChairID);

            //if (nUserTime < 0)
            //    return;

            if (m_cbGameStatus == HorseGameStatus.GS_FREE)
            {
                m_ImageTimeFree.DrawImage(g, 50, m_tagBenchmark.nHeight - 175);
                DrawNumber(g, 193, m_tagBenchmark.nHeight - 175, nUserTime / 60, true);
                DrawNumber(g, 253, m_tagBenchmark.nHeight - 175, nUserTime % 60, true);
                DrawNumber(g, 525, m_tagBenchmark.nHeight - 175, m_nStreak + 1);
            }
            else if (m_cbGameStatus == HorseGameStatus.GS_BET)
            {
                nUserTime = m_nHorseTime - (int)(DateTime.Now - m_BetStartTime).TotalSeconds;

                if (nUserTime < 0)
                    return;

                nUserTime += m_nBetEndTime;
                m_ImageTimeBet.DrawImage(g, 50, m_tagBenchmark.nHeight - 175);
                DrawNumber(g, 497, m_tagBenchmark.nHeight - 175, nUserTime / 60, true);
                DrawNumber(g, 558, m_tagBenchmark.nHeight - 175, nUserTime % 60, true);
                DrawNumber(g, 327, m_tagBenchmark.nHeight - 175, m_nStreak + 1);
            }
            else if (m_cbGameStatus == HorseGameStatus.GS_BET_END)
            {
                m_ImageTimeBetEnd.DrawImage(g, 50, m_tagBenchmark.nHeight - 175);
                DrawNumber(g, 497, m_tagBenchmark.nHeight - 175, nUserTime / 60, true);
                DrawNumber(g, 558, m_tagBenchmark.nHeight - 175, nUserTime % 60, true);
                DrawNumber(g, 327, m_tagBenchmark.nHeight - 175, m_nStreak + 1);
            }
            else// if (m_cbGameStatus == HorseGameStatus.GS_HORSES)
            {
                if (m_bRacing)
                {
                    m_ImageTimeHorse.DrawImage(g, 50, m_tagBenchmark.nHeight - 175);
                }
                else
                {
                    m_ImageTimeHorseEnd.DrawImage(g, 50, m_tagBenchmark.nHeight - 175);
                    DrawNumber(g, 194, m_tagBenchmark.nHeight - 175, nUserTime / 60, true);
                    DrawNumber(g, 255, m_tagBenchmark.nHeight - 175, nUserTime % 60, true);
                }
            }
        }

        public UserInfo GetClientUserItem(int m_wMeChairID)
        {
            if (m_wMeChairID == GameDefine.INVALID_CHAIR)
                return null;

            if (m_ReceiveInfo == null)
                return null;

            return m_ReceiveInfo._Players[m_wMeChairID];
        }

        //绘画玩家信息
        void DrawUserInfo(Graphics g)
        {
            //背景
            m_ImageUserInfoL.DrawImage(g, 0, m_tagBenchmark.nHeight - m_ImageUserInfoL.GetHeight());
            
            //m_ImageUserInfoLM.DrawImage(g, m_ImageUserInfoL.GetWidth() + 2, m_tagBenchmark.nHeight - m_ImageUserInfoLM.GetHeight());

            //for (int nX = m_ImageUserInfoL.GetWidth() + m_ImageUserInfoLM.GetWidth(); nX < m_tagBenchmark.nWidth; nX += m_ImageUserInfoM.GetWidth())
            //{
            //    m_ImageUserInfoM.DrawImage(g, nX, m_tagBenchmark.nHeight - m_ImageUserInfoM.GetHeight());
            //}
            //m_ImageUserInfoR.DrawImage(g, m_tagBenchmark.nWidth - m_ImageUserInfoR.GetWidth(), m_tagBenchmark.nHeight - m_ImageUserInfoR.GetHeight());

            //m_ImageUserInfoM1.DrawImage(g, m_ImageUserInfoL.GetWidth() + m_ImageUserInfoLM.GetWidth() + 2, m_tagBenchmark.nHeight - m_ImageUserInfoM1.GetHeight());

            //个人信息
            //if (m_wMeChairID == GameDefine.INVALID_CHAIR)
            //    return;

            // modified by usc at 2014/04/02
            //UserInfo pUserData = GetClientUserItem(m_wMeChairID);
            UserInfo pUserData = m_UserInfo;

            //m_WndUserFace.SetUserData(pUserData);
            if (pUserData == null)
                return;

            // added by usc at 2014/03/11
            Bitmap bmpFace = GetFaceImage(pUserData.Icon);

            if (bmpFace == null)
                bmpFace = GetFaceImage("image\\face\\DefaultHeadImage.png");

            m_ImageUserFace.LoadImage(this, bmpFace);

            m_ImageUserFace.DrawImage(g, 20, 5, 20, 20, 0, 0, bmpFace.Width, bmpFace.Height);


            m_ImageCash.DrawImage(g, 150, 5, 21, 21, 0, 0, 30, 30);
            m_ImagePoint.DrawImage(g, 250, 5, 20, 20, 0, 0, 30, 30);

            CRect rcUserInfo_Up = new CRect(30, 10, 150, m_tagBenchmark.nHeight - 20);

            m_D3DFont.DrawText(g, pUserData.Nickname, rcUserInfo_Up, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_LEFT, Color.FromArgb(255, 255, 255));

            //DrawUserAvatar(g,20,m_tagBenchmark.nHeight - m_ImageUserInfoL.GetHeight() + 21,32,32,pUserData);

            string szUserInfo = string.Empty;

            // modified by usc at 2013/07/21
            //CRect rcUserInfo = new CRect(60, m_tagBenchmark.nHeight - m_ImageUserInfoL.GetHeight() + 13, 190, m_tagBenchmark.nHeight - m_ImageUserInfoL.GetHeight() + 28);
            CRect rcUserInfo = new CRect(40, m_tagBenchmark.nHeight - m_ImageUserInfoL.GetHeight() + 40, 170, m_tagBenchmark.nHeight - m_ImageUserInfoL.GetHeight() + 55);

            szUserInfo = string.Format("用户名：{0}", pUserData.Nickname);
            m_D3DFont.DrawText(g, szUserInfo, rcUserInfo, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_LEFT, Color.FromArgb(255, 255, 255));


            // modified by usc at 2014/03/26
            //玩家总注
            int lUserAllBet = 0;            
            if (m_cbGameStatus != HorseGameStatus.GS_END)
            {
                for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
                    lUserAllBet += m_lPlayerBet[i];
            }

            rcUserInfo.OffsetRect(0, 20);

            // added by usc at 2014/04/10
            int nCommission = lUserAllBet * m_GameInfo.Commission / 100;

            szUserInfo = string.Format("游戏币：{0}", pUserData.GetGameMoney() - lUserAllBet - nCommission);

            m_D3DFont.DrawText(g, szUserInfo, rcUserInfo, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_LEFT, Color.FromArgb(255, 255, 255));

            // added by usc at 2014/03/12
            int nCash = 0;
            int nPoint = 0;

            if (pUserData.nCashOrPointGame == 0)
            {
                nCash = pUserData.Cash - lUserAllBet;
                nPoint = pUserData.Point;
            }
            else
            {
                nCash = pUserData.Cash;
                nPoint = pUserData.Point - lUserAllBet;
            }

            rcUserInfo_Up.OffsetRect(100, 0);
            m_D3DFont.DrawText(g, nCash.ToString(), rcUserInfo_Up, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_LEFT, Color.FromArgb(255, 255, 255));

            rcUserInfo_Up.OffsetRect(105, 0);
            m_D3DFont.DrawText(g, nPoint.ToString(), rcUserInfo_Up, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_LEFT, Color.FromArgb(255, 255, 255));


            rcUserInfo.OffsetRect(0, 18);
            szUserInfo = string.Format("总  注：{0}", lUserAllBet);
            m_D3DFont.DrawText(g, szUserInfo, rcUserInfo, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_LEFT, Color.FromArgb(255, 255, 255));

            //下注界面设置积分
            int lAllBet = 0;
            for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
                lAllBet += m_lPlayerBetAll[i];

            if (pUserData.nCashOrPointGame == 0)
            {
                m_DlgPlayBet.SetScore(pUserData.Cash - lUserAllBet);
                m_DlgRecord.SetScore(pUserData.Cash - lUserAllBet, lAllBet, m_nBetPlayerCount);
            }
            else
            {
                m_DlgPlayBet.SetScore(pUserData.Point - lUserAllBet);
                m_DlgRecord.SetScore(pUserData.Point - lUserAllBet, lAllBet, m_nBetPlayerCount);
            }

            /*
	        //区域名称
	        rcBetName.SetRect( ptBenchmark.x , ptBenchmark.y, ptBenchmark.x + sizeDisplayColumn.cx / 5, ptBenchmark.y + sizeDisplayColumn.cy / 6);
	        m_D3DFont.DrawText(g, TEXT("1 - 6"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("1 - 5"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("1 - 4"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("1 - 3"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("1 - 2"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));


	        rcBetName.SetRect( ptBenchmark.x , ptBenchmark.y + sizeDisplayColumn.cy / 6 * 2, ptBenchmark.x + sizeDisplayColumn.cx / 5, ptBenchmark.y + sizeDisplayColumn.cy / 6 * 3);
	        m_D3DFont.DrawText(g, TEXT("2 - 6"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("2 - 5"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("2 - 4"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("2 - 3"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("3 - 6"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));

	        rcBetName.SetRect( ptBenchmark.x , ptBenchmark.y + sizeDisplayColumn.cy / 6 * 4, ptBenchmark.x + sizeDisplayColumn.cx / 5, ptBenchmark.y + sizeDisplayColumn.cy / 6 * 5 );
	        m_D3DFont.DrawText(g, TEXT("3 - 5"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("3 - 4"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("4 - 6"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("4 - 5"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));
	        rcBetName.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        m_D3DFont.DrawText(g, TEXT("5 - 6"), rcBetName, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,0));


	        //区域下注
	        rcBetCount.SetRect( ptBenchmark.x , ptBenchmark.y + sizeDisplayColumn.cy / 6, ptBenchmark.x + sizeDisplayColumn.cx / 5, ptBenchmark.y + sizeDisplayColumn.cy / 6 * 2);
	        szBet = lBet[HorseDefine.AREA_1_6].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_1_5].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_1_4].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_1_3].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_1_2].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));


	        rcBetCount.SetRect( ptBenchmark.x , ptBenchmark.y + sizeDisplayColumn.cy / 6 * 3, ptBenchmark.x + sizeDisplayColumn.cx / 5, ptBenchmark.y + sizeDisplayColumn.cy / 6 * 4);
	        szBet = lBet[HorseDefine.AREA_2_6].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_2_5].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_2_4].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_2_3].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));;
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_3_6].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));

	        rcBetCount.SetRect( ptBenchmark.x , ptBenchmark.y + sizeDisplayColumn.cy / 6 * 5, ptBenchmark.x + sizeDisplayColumn.cx / 5, ptBenchmark.y + sizeDisplayColumn.cy );
	        szBet = lBet[HorseDefine.AREA_3_5].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_3_4].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_4_6].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_4_5].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
	        rcBetCount.OffsetRect(sizeDisplayColumn.cx / 5 , 0);
	        szBet = lBet[HorseDefine.AREA_5_6].ToString();
	        m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER, Color.FromArgb(255,255,255));
            */

        }

        // added newly
        void DrawBettingInfo(Graphics g)
        {
            m_ImageChipPanel.DrawImage(g, m_ImageUserInfoL.GetWidth(), m_tagBenchmark.nHeight - m_ImageChipPanel.GetHeight());

            g.DrawImage(m_bmpBettingPanel, m_ImageUserInfoL.GetWidth() + m_ImageChipPanel.GetWidth(), m_tagBenchmark.nHeight - m_ImageBettingPanel.GetHeight());

            //下注信息----------------------------------------------------------
            //基准位置
            CPoint ptBenchmark = new CPoint();

            //显示大小
            CSize sizeDisplayColumn = new CSize();

            ptBenchmark.SetPoint(m_ImageUserInfoL.GetWidth() + m_ImageChipPanel.GetWidth(), m_tagBenchmark.nHeight - m_ImageUserInfoL.GetHeight());
            sizeDisplayColumn.SetSize(m_tagBenchmark.nWidth - m_ImageUserInfoL.GetWidth() - m_ImageChipPanel.GetWidth(), 84);

            //竖线
            //for (int i = 1; i < 6; ++i)
            //{
            //    m_ImageUserInfoShu.DrawImage(g, ptBenchmark.x + sizeDisplayColumn.cx / 6 * i, ptBenchmark.y);
            //}

            //显示下注
            CRect rcBetMultiple = new CRect();
            CRect rcBetCount = new CRect();
            //下注
            // deleted by usc at 2014/03/21
            //int[] lBet = new int[HorseDefine.AREA_ALL];	        

            //if ( m_bMeOrAll )
            //    memcpy(lBet, m_lPlayerBet );
            //else
            //    memcpy(lBet, m_lPlayerBetAll);

            string szBet = string.Empty;

            for (int i = 0; i < 6; ++i)
            {
                szBet = string.Format("X {0}", m_nMultiple[i]);

                rcBetMultiple.SetRect(ptBenchmark.x + sizeDisplayColumn.cx / 6 * i + 3,
                                            m_tagBenchmark.nHeight - m_ImageBettingPanel.GetHeight() + 10,
                                            ptBenchmark.x + sizeDisplayColumn.cx / 6 * i + 35,
                                            m_tagBenchmark.nHeight - m_ImageBettingPanel.GetHeight() + 20);

                m_D3DFont.DrawText(g, szBet, rcBetMultiple, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_CENTER, Color.FromArgb(255, 255, 255));

                rcBetCount.SetRect(         ptBenchmark.x + sizeDisplayColumn.cx / 6 * i + sizeDisplayColumn.cx / 24 + 3, 
                                            m_tagBenchmark.nHeight - 15, 
                                            ptBenchmark.x + sizeDisplayColumn.cx / 6 * i + sizeDisplayColumn.cx * 3 / 24, 
                                            m_tagBenchmark.nHeight - 5);

                szBet = string.Format("{0}", m_lPlayerBet[i]);
                m_D3DFont.DrawText(g, szBet, rcBetCount, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_CENTER, Color.FromArgb(255, 255, 255));

                // added by usc at 2014/3/21
                if (m_lPlayerBetAll[i] > 0)
                {
                    int nWidth = GetNumberLen(m_lPlayerBetAll[i] * m_ImageScoreNumber.GetWidth() / 11);

                    DrawScoreNumber(g, ptBenchmark.x + sizeDisplayColumn.cx / 6 * i + (sizeDisplayColumn.cx / 6 - nWidth) / 2, m_tagBenchmark.nHeight - m_ImageBettingPanel.GetHeight() / 3, m_lPlayerBetAll[i]);
                }
            }
        }

        //绘画结束信息
        void DrawGameOver(Graphics g)
        {
            if (!m_bGameOver)
                return;

            if (m_wMeChairID != GameDefine.INVALID_CHAIR)
            {
                m_ImageGameOver.DrawImage(g, m_ptGameOver.x, m_ptGameOver.y);

                CRect rcet = new CRect();
                string szInfo = string.Empty;
                rcet.SetRect(m_ptGameOver.x + 170, m_ptGameOver.y + 105, m_ptGameOver.x + 255, m_ptGameOver.y + 120);

                for (int i = 0; i < CountArray(m_bRanking); ++i)
                {
                    if (m_bRanking[i] >= HorseDefine.HORSES_ALL)
                    {
                        //ASSERT(false);
                        continue;
                    }

                    if (m_bRanking[i] == HorseDefine.HORSES_ONE)
                    {
                        if (_tcscmp(TEXT("一号马"), m_szHorsesName[m_bRanking[i]]) == 0)
                            szInfo = TEXT("一号马");
                        else
                            szInfo = string.Format("一号：{0}", m_szHorsesName[m_bRanking[i]]);
                    }
                    else if (m_bRanking[i] == HorseDefine.HORSES_TWO)
                    {
                        if (_tcscmp(TEXT("二号马"), m_szHorsesName[m_bRanking[i]]) == 0)
                            szInfo = TEXT("二号马");
                        else
                            szInfo = string.Format("二号：{0}", m_szHorsesName[m_bRanking[i]]);
                    }
                    else if (m_bRanking[i] == HorseDefine.HORSES_THREE)
                    {
                        if (_tcscmp(TEXT("三号马"), m_szHorsesName[m_bRanking[i]]) == 0)
                            szInfo = TEXT("三号马");
                        else
                            szInfo = string.Format("三号：{0}", m_szHorsesName[m_bRanking[i]]);
                    }
                    else if (m_bRanking[i] == HorseDefine.HORSES_FOUR)
                    {
                        if (_tcscmp(TEXT("四号马"), m_szHorsesName[m_bRanking[i]]) == 0)
                            szInfo = TEXT("四号马");
                        else
                            szInfo = string.Format("四号：{0}", m_szHorsesName[m_bRanking[i]]);
                    }
                    else if (m_bRanking[i] == HorseDefine.HORSES_FIVE)
                    {
                        if (_tcscmp(TEXT("五号马"), m_szHorsesName[m_bRanking[i]]) == 0)
                            szInfo = TEXT("五号马");
                        else
                            szInfo = string.Format("五号：{0}", m_szHorsesName[m_bRanking[i]]);
                    }
                    else if (m_bRanking[i] == HorseDefine.HORSES_SIX)
                    {
                        if (_tcscmp(TEXT("六号马"), m_szHorsesName[m_bRanking[i]]) == 0)
                            szInfo = "六号马";
                        else
                            szInfo = string.Format("六号：{0}", m_szHorsesName[m_bRanking[i]]);
                    }

                    m_D3DFont.DrawText(g, szInfo, rcet, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_LEFT, Color.FromArgb(255, 255, 255));

                    if (i % 2 == 0)
                        rcet.OffsetRect(110, 0);
                    else
                        rcet.OffsetRect(-110, 40);
                }

                //if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                //{
                    //UserInfo pUserData = GetClientUserItem(m_wMeChairID);
                    UserInfo pUserData = m_UserInfo;

                    if (pUserData != null)
                    {
                        string strNickname = pUserData.Nickname;

                        if (strNickname.Length > 12)
                            strNickname = strNickname.Substring(0, 12);

                        rcet.SetRect(m_ptGameOver.x + 411, m_ptGameOver.y + 122, m_ptGameOver.x + 500, m_ptGameOver.y + 142);
                        m_D3DFont.DrawText(g, strNickname, rcet, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_LEFT, Color.FromArgb(255, 255, 0));

                        rcet.SetRect(m_ptGameOver.x + 390, m_ptGameOver.y + 155, m_ptGameOver.x + 500, m_ptGameOver.y + 175);
                        szInfo = m_lPlayerWinning.ToString();
                        m_D3DFont.DrawText(g, szInfo, rcet, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_LEFT, Color.FromArgb(255, 255, 0));

                    }
                //}
            }

            // added by usc at 2014/03/05
            int nRankOneArea = m_bRanking[0];
            int nRankTwoArea = m_bRanking[1];

            m_FlashCount++;

            if (m_FlashCount > 3)
            {
                if (nRankOneArea < HorseDefine.HORSES_ALL && nRankTwoArea < HorseDefine.HORSES_ALL)
                {
                    g.DrawRectangle(new Pen(Color.FromArgb(254, 207, 13), 3), m_RectFlashArea[nRankOneArea].left, m_RectFlashArea[nRankOneArea].top, m_RectFlashArea[nRankOneArea].Width(), m_RectFlashArea[nRankOneArea].Height());
                    g.DrawRectangle(new Pen(Color.FromArgb(235, 235, 235), 2), m_RectFlashArea[nRankTwoArea].left, m_RectFlashArea[nRankTwoArea].top, m_RectFlashArea[nRankTwoArea].Width(), m_RectFlashArea[nRankTwoArea].Height());
                }

                if (m_FlashCount > 6)
                    m_FlashCount = 0;
            }

            // added by usc at 2014/03/22
            DrawRankFlash(g, 1, nRankOneArea);
            DrawRankFlash(g, 2, nRankTwoArea);
        }

        //艺术字体
        void DrawTextString(Graphics g, string pszString, Color crText, Color crFrame, int nXPos, int nYPos)
        {
            //减去基准点
            nXPos -= m_tagBenchmark.ptBase.x;

            if (nXPos < 0 || nXPos > m_tagBenchmark.nWidth)
                return;

            //变量定义
            int nStringLength = pszString.Length;
            int[] nXExcursion = new int[] { 1, 1, 1, 0, -1, -1, -1, 0 };
            int[] nYExcursion = new int[] { -1, 0, 1, 1, 1, 0, -1, -1 };

            //绘画边框
            for (int i = 0; i < CountArray(nXExcursion); i++)
            {
                m_D3DFont.DrawText(g, pszString, nXPos + nXExcursion[i], nYPos + nYExcursion[i], CDC.TA_CENTER | CDC.DT_TOP, crFrame);
            }

            //绘画字体
            m_D3DFont.DrawText(g, pszString, nXPos, nYPos, CDC.TA_CENTER | CDC.DT_TOP, crText);
        }

        //绘画数字
        void DrawNumber(Graphics g, int nXPos, int nYPos, int nNumer)
        {
            DrawNumber(g, nXPos, nYPos, nNumer, false);
        }

        void DrawNumber(Graphics g, int nXPos, int nYPos, int nNumer, bool bTime)
        {
            //加载资源
            int nNumberHeight = m_ImageTimeNumer.GetHeight();
            int nNumberWidth = m_ImageTimeNumer.GetWidth() / 10;

            //计算数目
            int lNumberCount = 0;
            int lNumberTemp = nNumer;
            do
            {
                lNumberCount++;
                lNumberTemp /= 10;
            } while (lNumberTemp > 0);

            //位置定义
            int nYDrawPos = nYPos;
            int nXDrawPos = nXPos + lNumberCount * nNumberWidth / 2 - nNumberWidth;
            if (bTime && lNumberCount == 1)
                nXDrawPos = nXPos + 2 * nNumberWidth / 2 - nNumberWidth;

            //绘画桌号
            for (int i = 0; i < lNumberCount; i++)
            {
                //绘画号码
                int lCellNumber = nNumer % 10;
                m_ImageTimeNumer.DrawImage(g, nXDrawPos, nYDrawPos, nNumberWidth - 1, nNumberHeight, lCellNumber * nNumberWidth + 1, 0);

                //设置变量
                nNumer /= 10;
                nXDrawPos -= nNumberWidth;

                if (bTime && lNumberCount == 1)
                {
                    m_ImageTimeNumer.DrawImage(g, nXDrawPos, nYDrawPos, nNumberWidth, nNumberHeight, 0, 0);
                }

            };

            return;
        }

        void DrawScoreNumber(Graphics g, int nXPos, int nYPos, int nNumber)
        {
            //加载资源
            int nNumberHeight = m_ImageScoreNumber.GetHeight();
            int nNumberWidth = m_ImageScoreNumber.GetWidth() / 11;

            //计算数目
            int lNumberCount = GetNumberLen(nNumber);

            //位置定义
            int nYDrawPos = nYPos;
            int nXDrawPos = nXPos + lNumberCount * nNumberWidth / 2 - nNumberWidth;

            //绘画桌号
            for (int i = 0; i < lNumberCount; i++)
            {
                //绘画号码
                int lCellNumber = nNumber % 10;
                m_ImageScoreNumber.DrawImage(g, nXDrawPos, nYDrawPos, nNumberWidth - 1, nNumberHeight, lCellNumber * nNumberWidth + 1, 0);

                //设置变量
                nNumber /= 10;
                nXDrawPos -= nNumberWidth;
            }
        }

        void DrawRankFlash(Graphics g, int nRank, int nRankArea)
        {
            CPoint ptBenchmark = new CPoint();

            CSize sizeDisplayColumn = new CSize();

            ptBenchmark.SetPoint(m_ImageUserInfoL.GetWidth() + m_ImageChipPanel.GetWidth(), m_tagBenchmark.nHeight - m_ImageUserInfoL.GetHeight());
            sizeDisplayColumn.SetSize(m_tagBenchmark.nWidth - m_ImageUserInfoL.GetWidth() - m_ImageChipPanel.GetWidth(), 84);


            if (nRank == 1)
            {
                int nWidth = m_ImageRank1.GetWidth();
                int nHeight = m_ImageRank1.GetHeight();

                m_ImageRank1.DrawImage(g, ptBenchmark.x + sizeDisplayColumn.cx / 6 * nRankArea + (sizeDisplayColumn.cx / 6 - nWidth) / 2, m_tagBenchmark.nHeight - m_ImageBettingPanel.GetHeight() - nHeight / 3, nWidth, nHeight, 0, 0);
            }
            else if (nRank == 2)
            {
                int nWidth = m_ImageRank2.GetWidth();
                int nHeight = m_ImageRank2.GetHeight();

                m_ImageRank2.DrawImage(g, ptBenchmark.x + sizeDisplayColumn.cx / 6 * nRankArea + (sizeDisplayColumn.cx / 6 - nWidth) / 2, m_tagBenchmark.nHeight - m_ImageBettingPanel.GetHeight() - nHeight / 3, nWidth, nHeight, 0, 0);
            }

        }

        private int GetNumberLen(int nNumber)
        {
            int nLen = 0;

            do
            {
                nLen++;
                nNumber /= 10;
            }
            while (nNumber > 0);

            return nLen;
        }

        //定时器
        protected override void OnTimer(TimerInfo timerInfo)
        {
            int nIDEvent = timerInfo._Id;

            if (nIDEvent == IDI_HORSES_START)		//赛马开始
            {
                int callCount = (int)((DateTime.Now - _HosreStartTime).TotalMilliseconds / 30) - _HorseCallCount;

                if (m_cbGameStatus == HorseGameStatus.GS_HORSES)
                {
                    if (_renderDelays.Count == 0)
                    {
                        _renderDelays.Add(0);
                        prevDelayTime = DateTime.Now;
                        starTime = DateTime.Now;
                    }
                    else
                    {
                        _renderDelays.Add((int)(DateTime.Now - prevDelayTime).TotalMilliseconds);
                        prevDelayTime = DateTime.Now;

                        if (_renderDelays.Count > 20)
                        {
                            endTime = DateTime.Now;
                            _renderDelays.Clear();
                        }
                    }
                }

                for (int abc = 0; abc < callCount; abc++)
                {
                    _HorseCallCount++;

                    //地图移动 -------------------------------------
                    //到达尾地图则停止
                    if (m_tagBenchmark.ptBase.x >= m_szTotalSize.cx - m_tagBenchmark.nWidth)
                    {
                        //地图停止
                        m_tagBenchmark.ptBase.x = m_szTotalSize.cx - m_tagBenchmark.nWidth;

                        //马匹移动
                        for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
                        {
                            m_ptHorsesPos[nHorses].x += HorseDefine.BASIC_SPEED;
                        }

                        //检测是否到终点
                        int nOverCount = 0;
                        for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
                        {
                            if (m_ptHorsesPos[nHorses].x >= m_szTotalSize.cx - HorseDefine.TAIL_LENGTH - m_ImageHorses[nHorses].GetWidth() / 5)
                            {
                                nOverCount++;
                            }
                        }

                        if (nOverCount >= 2)
                        {
                            HorsesEnd();

                            StopDirectSound(m_SoundFolder + "HOS.wav");
                            //PlayDirectSound(m_ResourceFolder + "GAME_OVER.wav", false);
                            break;
                        }
                    }
                    else if (m_bRacing)
                    {
                        //移动
                        int nHorsesFirstPos = 0;			//马匹第一位置
                        int nHorsesFinallyPos = INT_MAX;	//马匹最后位置
                        for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
                        {
                            m_ptHorsesPos[nHorses].x += HorseDefine.BASIC_SPEED;
                            int temp = m_ptHorsesPos[nHorses].x - m_tagBenchmark.nWidth / 2 + m_ImageHorses[nHorses].GetWidth() / 5 / 2;
                            if (temp > nHorsesFirstPos || nHorsesFirstPos == 0)
                                nHorsesFirstPos = temp;

                            if (temp < nHorsesFinallyPos || nHorsesFirstPos == 0)
                                nHorsesFinallyPos = temp;
                        }

                        //地图跟着马移动
                        if (m_tagBenchmark.ptBase.x <= nHorsesFinallyPos + (nHorsesFirstPos - nHorsesFinallyPos) / 2)
                            m_tagBenchmark.ptBase.x = nHorsesFinallyPos + (nHorsesFirstPos - nHorsesFinallyPos) / 2;
                    }

                    //马匹移动-----------------------------------
                    if (m_bRacing)
                    {
                        //马匹移动
                        for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
                        {
                            if (m_nHorsesSpeedIndex[nHorses] > 0 && m_nHorsesSpeedIndex[nHorses] != HorseDefine.STEP_SPEED - 1)
                            {
                                m_ptHorsesPos[nHorses].x -= (m_nHorsesSpeed[nHorses, m_nHorsesSpeedIndex[nHorses] - 1]);
                                m_ptHorsesPos[nHorses].x += (m_nHorsesSpeed[nHorses, m_nHorsesSpeedIndex[nHorses]]);
                            }
                            else if (m_nHorsesSpeedIndex[nHorses] == 0)
                            {
                                m_ptHorsesPos[nHorses].x += (m_nHorsesSpeed[nHorses, m_nHorsesSpeedIndex[nHorses]]);
                            }

                            m_nHorsesSpeedIndex[nHorses]++;
                            if (m_nHorsesSpeedIndex[nHorses] == HorseDefine.STEP_SPEED)
                                m_nHorsesSpeedIndex[nHorses] = HorseDefine.STEP_SPEED - 1;
                        }

                        //检测是否到终点
                        int nOverCount = 0;
                        for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
                        {
                            if (m_ptHorsesPos[nHorses].x >= m_szTotalSize.cx - HorseDefine.TAIL_LENGTH - m_ImageHorses[nHorses].GetWidth() / 5)
                            {
                                nOverCount++;
                            }
                        }

                        if (nOverCount >= 2)
                        {
                            HorsesEnd();

                            StopDirectSound(m_SoundFolder + "HOS.wav");
                            //PlayDirectSound(m_ResourceFolder + "GAME_OVER.wav", false);
                            break;
                        }
                    }

                    //马匹换帧-------------------------------------
                    if (m_bRacing)
                    {
                        m_nInFrameDelay++;
                        if (m_nInFrameDelay % 2 == 1)
                        {
                            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
                            {
                                m_cbHorsesIndex[nHorses] = (m_cbHorsesIndex[nHorses] + 1) % 5;
                            }
                        }
                    }
                }

                //Invalidate();
            }
            else if (IDI_HORSES_END == nIDEvent)// 赛马结束
            {
                ChatEngine.StringInfo messageInfo = new ChatEngine.StringInfo();

                //if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                //{
                    //UserInfo pUserData = GetClientUserItem(m_wMeChairID);
                    UserInfo pUserData = m_UserInfo;

                    if (pUserData != null)
                    {
                        messageInfo.UserId = pUserData.Id;
                        messageInfo.strRoomID = pUserData.GameId;

                        m_ClientSocket.Send(NotifyType.Request_GameResult, messageInfo);
                    }
                //}

                KillTimer(IDI_HORSES_END);

                m_bGameOver = true;
                m_btGameOver.ShowWindow(true);

                //Invalidate();

                // added by usc at 2014/03/18
                PlayDirectSound(m_SoundFolder + "GAME_OVER.wav", false);
            }
            else if (IDI_PLAY_SOUNDS == nIDEvent)
            {
                if (m_bRacing)
                {
                    PlayDirectSound(m_SoundFolder + "HOS.wav", false);
                }
                else
                {
                    //m_HorsesSound.Stop();
                    KillTimer(IDI_PLAY_SOUNDS);
                }
            }
            else if (IDI_RENDER == nIDEvent)
            {
                //Invalidate();
                UpdateGameView();
            }

            //CGameFrameView::OnTimer(nIDEvent);

        }

        DateTime prevDelayTime;
        DateTime starTime;
        DateTime endTime;
        List<int> _renderDelays = new List<int>();

        ////鼠标消息
        //void OnLButtonDown(UINT nFlags, CPoint point)
        //{
        //    //CGameFrameView::OnLButtonDown(nFlags, point);
        //}

        //个人下注显示
        void OnPlayerBetShow(object sender, EventArgs e)
        {
            m_bMeOrAll = true;
            m_btPlayerBetShow.EnableWindow(false);
            m_btAllBetShow.EnableWindow(true);
        }

        //全场下注显示
        void OnAllBetShow(object sender, EventArgs e)
        {
            m_bMeOrAll = false;
            m_btPlayerBetShow.EnableWindow(true);
            m_btAllBetShow.EnableWindow(false);
        }

        //历史记录
        void OnRecordShow(object sender, EventArgs e)
        {
            if (m_DlgRecord.IsWindowVisible())
            {
                m_DlgRecord.Visible = false;
            }
            else
            {
                m_DlgRecord.ShowWindow(true);
                m_DlgRecord.SetForegroundWindow();

                CRect rcButton = new CRect();
                //m_btRecord.GetWindowRect(ref rcButton);
                //ScreenToClient(&rcButton);

                // modified by usc at 2013/07/21
                m_DlgRecord.SetWindowPos(this, rcButton.left - 50, rcButton.top + 40, 0, 0, CDialog.SWP_NOACTIVATE | CDialog.SWP_NOZORDER | CDialog.SWP_NOCOPYBITS | CDialog.SWP_NOSIZE);
            }
        }

        //个人下注
        void OnPlayerBet(object sender, EventArgs e)
        {
            if (m_DlgPlayBet.IsWindowVisible())
            {
                m_DlgPlayBet.Visible = false;
            }
            else
            {
                m_DlgPlayBet.m_Parent = this;

                m_DlgPlayBet.ShowWindow(true);
                m_DlgPlayBet.SetForegroundWindow();

                CRect rcButton = new CRect();
                CRect rcBet = new CRect();
                m_DlgPlayBet.GetWindowRect(ref rcBet);
                m_btPlayerBet.GetWindowRect(ref rcButton);
                //ScreenToClient(&rcButton);
                m_DlgPlayBet.SetWindowPos(this, rcButton.left - 100, rcButton.top - rcBet.Height() - 40, 0, 0, CDialog.SWP_NOACTIVATE | CDialog.SWP_NOZORDER | CDialog.SWP_NOCOPYBITS | CDialog.SWP_NOSIZE); ;

                if (m_cbGameStatus == HorseGameStatus.GS_BET)
                    m_DlgPlayBet.SetCanBet(true);

            }
        }


        //统计窗口
        void OnStatistics(object sender, EventArgs e)
        {
            if (m_DlgStatistics.IsWindowVisible())
            {
                m_DlgStatistics.ShowWindow(false);
            }
            else
            {
                m_DlgStatistics.ShowWindow(true);
                m_DlgStatistics.SetForegroundWindow();

                CRect rcButton = new CRect();
                CRect rcStatistics = new CRect();
                m_DlgStatistics.GetWindowRect(ref rcStatistics);
                m_btStatistics.GetWindowRect(ref rcButton);
                //ScreenToClient(&rcButton);
                m_DlgStatistics.SetWindowPos(this, rcButton.left, rcButton.top - rcStatistics.Height() - 40, 0, 0, CDialog.SWP_NOACTIVATE | CDialog.SWP_NOZORDER | CDialog.SWP_NOCOPYBITS | CDialog.SWP_NOSIZE); ;
            }
        }

        //下注成绩
        void OnBetRecordShow(object sender, EventArgs e)
        {
            if (m_DlgBetRecord.IsWindowVisible())
            {
                m_DlgBetRecord.ShowWindow(false);
            }
            else
            {
                m_DlgBetRecord.ShowWindow(true);
                m_DlgBetRecord.SetForegroundWindow();

                CRect rcButton = new CRect();
                CRect rcBetRecord = new CRect();
                m_DlgBetRecord.GetWindowRect(ref rcBetRecord);
                //m_btBetRecord.GetWindowRect(ref rcButton);
                //ScreenToClient(&rcButton);
                m_DlgBetRecord.SetWindowPos(this, rcButton.left, rcButton.top - rcBetRecord.Height() - 20, 0, 0, CDialog.SWP_NOACTIVATE | CDialog.SWP_NOZORDER | CDialog.SWP_NOCOPYBITS | CDialog.SWP_NOSIZE); ;
            }
        }

        //说明
        void OnExplain(object sender, EventArgs e)
        {
            if (m_wKindID == INVALID_WORD)
                return;

            //TCHAR szhttp[256];
            //_sntprintf(szhttp, sizeof(szhttp), TEXT("http://%s/GameRule.asp?KindID=%d"),szStationPage,m_wKindID);

            //ShellExecute(null, TEXT("open"), szhttp, null, null, SW_SHOWDEFAULT);
        }

        //成绩关闭
        void OnGameOverClose(object sender, EventArgs e)
        {
            m_bGameOver = false;
            m_btGameOver.ShowWindow(false);
        }

        //管理员控制
        //void OpenControlWnd(object sender, EventArgs e)
        //{
        //    //有权限
        //    //if((GetClientUserItem(m_wMeChairID)->dwUserRight&UR_GAME_CONTROL)!=0)
        //    {
        //        if (null==m_DlgControl.Handle) 
        //            m_DlgControl = new DialogControl();

        //        if(!m_DlgControl.IsWindowVisible()) 
        //        {
        //            m_DlgControl.ShowWindow(true);
        //            m_DlgControl.SetForegroundWindow();
        //        }
        //        else 
        //        {
        //            m_DlgControl.ShowWindow(false);
        //        }
        //    }
        //}

        Random _random = new Random();

        public int rand()
        {
            return _random.Next();
        }

        public int GetTickCount()
        {
            return 0;
            //System.Environment.TickCount;
        }

        //跑马开始
        void HorsesStart(int[,] nHorsesSpeed)
        {
            //归零
            ZeroMemory(m_cbHorsesIndex);
            ZeroMemory(m_nHorsesSpeed);
            ZeroMemory(m_nHorsesSpeedIndex);

            //帧数随机
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
                m_cbHorsesIndex[nHorses] = rand() % 5;

            //得到马匹速度(模糊速度转为精确速度)
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                //速步索引
                int nSpeedIndex = HorseDefine.STEP_SPEED - 1;

                for (int nTime = HorseDefine.COMPLETION_TIME - 1; nTime >= 0; nTime--)
                {
                    //速度
                    int nVelocity = 1;
                    //高速维持
                    int nMaintenance = 0;
                    //频率
                    int nFrequency = (rand() + GetTickCount() * 2) % HorseDefine.FREQUENCY + 1;
                    //加速度
                    int nAcceleration = HorseDefine.ACCELERATION;

                    if (nTime % 2 == 0)
                    {
                        nAcceleration = ((rand() + GetTickCount()) % HorseDefine.ACCELERATION + 1);
                        nAcceleration = -nAcceleration;
                    }
                    else
                    {
                        nAcceleration = ((rand() + GetTickCount()) % HorseDefine.ACCELERATION + 2);
                    }

                    //设置速度
                    nVelocity = nHorsesSpeed[nHorses, nTime];

                    //50为加速器.. 要从0 - 50 加速到现在的速度
                    while (nSpeedIndex >= 50)
                    {
                        m_nHorsesSpeed[nHorses, nSpeedIndex] = nVelocity;

                        //达到低速界或是高速界
                        if ((nTime % 2 == 0 && nTime > 0 && nVelocity <= nHorsesSpeed[nHorses, nTime - 1])		//低速界
                            || (nTime % 2 == 1 && nTime > 0 && nVelocity >= nHorsesSpeed[nHorses, nTime - 1]))//高速界
                        {
                            break;
                        }

                        //设置下一步速度
                        if (nMaintenance < nFrequency)
                        {
                            nMaintenance++;
                        }
                        else
                        {
                            nFrequency = (rand() + GetTickCount() * 2) % (HorseDefine.FREQUENCY / 2) + 1;
                            nMaintenance = 0;
                            nVelocity += nAcceleration;
                        }
                        nSpeedIndex--;
                    }

                    //100以前的加速器
                    for (int i = 0; i < 50; ++i)
                    {
                        m_nHorsesSpeed[nHorses, i] = m_nHorsesSpeed[nHorses, 50] * i / 50;
                    }

                }
            }

            m_bRacing = true;
            m_nInFrameDelay = 0;
            PlayDirectSound(m_SoundFolder + "HOS.wav", false);
            SetTimer(IDI_HORSES_START, 30);		//赛马开始
            SetTimer(IDI_PLAY_SOUNDS, 4000);		//跑马声音

            _HosreStartTime = DateTime.Now;
            _HorseCallCount = 0;
        }

        DateTime _HosreStartTime;
        int _HorseCallCount = 0;

        //赛马结束
        void HorsesEnd()
        {
            m_bRacing = false;
            KillTimer(IDI_HORSES_START);
            KillTimer(IDI_PLAY_SOUNDS);
            //m_HorsesSound.Stop();

            if (m_bRanking[HorseDefine.RANKING_SECOND] < 0 || m_bRanking[HorseDefine.RANKING_SECOND] > (HorseDefine.HORSES_ALL - 1))
            {
                return;
            }

            //地图停止
            m_tagBenchmark.ptBase.x = m_szTotalSize.cx - m_tagBenchmark.nWidth;

            int nHorsesPos = m_szTotalSize.cx - HorseDefine.TAIL_LENGTH - m_ImageHorses[m_bRanking[HorseDefine.RANKING_SECOND]].GetWidth() / 5 - m_nHorsesSpeed[m_bRanking[HorseDefine.RANKING_SECOND], HorseDefine.STEP_SPEED - 1];

            //终点定位
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                m_ptHorsesPos[nHorses].x = nHorsesPos + m_nHorsesSpeed[nHorses, HorseDefine.STEP_SPEED - 1];
            }

            ////开启结束界面
            //SetTimer(IDI_HORSES_END,2000);
        }

        //新一局开始
        void NweHorses()
        {
            m_bRacing = false;
            m_bGameOver = false;
            KillTimer(IDI_HORSES_START);
            KillTimer(IDI_HORSES_END);
            KillTimer(IDI_PLAY_SOUNDS);
            m_btGameOver.ShowWindow(false);
            //m_HorsesSound.Stop();

            m_ptHorsesPos[HorseDefine.HORSES_ONE].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_ONE_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_TWO].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_TWO_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_THREE].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_THREE_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_FOUR].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_FOUR_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_FIVE].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_FIVE_Y_POS);
            m_ptHorsesPos[HorseDefine.HORSES_SIX].SetPoint(HorseDefine.HORSES_X_POS, HorseDefine.HORSES_SIX_Y_POS);

            m_tagBenchmark.ptBase.x = 0;
            m_tagBenchmark.ptBase.y = 0;

            //归零
            ZeroMemory(m_cbHorsesIndex);
            ZeroMemory(m_nHorsesSpeed);
            ZeroMemory(m_nHorsesSpeedIndex);

            //分数
            m_nBetPlayerCount = 0;
            ZeroMemory(m_lPlayerBet);
            ZeroMemory(m_lPlayerBetAll);
            m_lPlayerWinning = 0;

        }

        //设置马名字
        void SetHorsesName(int wMeChairID, string szName)
        {
            //memcpy( m_szHorsesName[wMeChairID], szName, sizeof(m_szHorsesName[wMeChairID]) );
            m_szHorsesName[wMeChairID] = szName;
        }

        //设置所有下注
        void SetAllBet(int cbArea, int lScore)
        {
            m_lPlayerBetAll[cbArea] = lScore;

        }

        //玩家加注
        void SetPlayerBet(int wMeChairID, int cbArea, int lScore)
        {
            if (wMeChairID == m_wMeChairID)
            {
                m_lPlayerBet[cbArea] += lScore;
            }
            m_lPlayerBetAll[cbArea] += lScore;
        }

        //设置名次
        void SetRanking(int[] bRanking)
        {
            memcpy(m_bRanking, bRanking);
        }

        //动画驱动
        void CartoonMovie()
        {

        }

        //配置设备
        protected override void InitGameView()
        {
            //变量定义
            Control hResInstance = this;

            //背景资源
            m_ImagePenHand.LoadImage(hResInstance, GetImage("PEN_HEAD.png"));
            m_ImagePenNum.LoadImage(hResInstance, GetImage("PEN_NUM.png"));
            m_ImagePenTail.LoadImage(hResInstance, GetImage("PEN_TAIL.png"));

            m_ImageBackdrop.LoadImage(hResInstance, GetImage("FULL_BACK.png"));				//背景
            m_ImageBackdropHand.LoadImage(hResInstance, GetImage("BACK_HAND.png"));			//背景尾
            m_ImageBackdropTail.LoadImage(hResInstance, GetImage("BACK_TAIL.png"));			//背景头
            m_ImageDistanceFrame.LoadImage(hResInstance, GetImage("DISTANCE_FRAME.png"));	//背景距离

            //信息栏
            m_ImageUserInfoL.LoadImage(hResInstance, GetImage("USER_INFO_L.png"));
            m_ImageChipPanel.LoadImage(hResInstance, GetImage("USER_INFO_LM.png"));
            //m_ImageUserInfoM.LoadImage( hResInstance, GetImage( "USER_INFO_M.png") );
            //m_ImageUserInfoR.LoadImage( hResInstance, GetImage( "USER_INFO_R.png") );
            //m_ImageUserInfoShu.LoadImage( hResInstance, GetImage( "USER_INFO_SHU.png") );
            m_ImageBettingPanel.LoadImage(hResInstance, GetImage("USER_INFO_M1.png"));

            //马匹
            m_ImageHorses[HorseDefine.HORSES_ONE].LoadImage(hResInstance, GetImage("HORSE1.png"));
            m_ImageHorses[HorseDefine.HORSES_TWO].LoadImage(hResInstance, GetImage("HORSE2.png"));
            m_ImageHorses[HorseDefine.HORSES_THREE].LoadImage(hResInstance, GetImage("HORSE3.png"));
            m_ImageHorses[HorseDefine.HORSES_FOUR].LoadImage(hResInstance, GetImage("HORSE4.png"));
            m_ImageHorses[HorseDefine.HORSES_FIVE].LoadImage(hResInstance, GetImage("HORSE5.png"));
            m_ImageHorses[HorseDefine.HORSES_SIX].LoadImage(hResInstance, GetImage("HORSE6.png"));

            m_ImageTimeNumer.LoadImage(hResInstance, GetImage("TIME_NUMBER.png"));			//数字
            m_ImageTimeBet.LoadImage(hResInstance, GetImage("TIPS_BET.png"));				//下注时间
            m_ImageTimeBetEnd.LoadImage(hResInstance, GetImage("TIPS_BET_END.png"));			//下注结束
            m_ImageTimeFree.LoadImage(hResInstance, GetImage("TIPS_FREE.png"));				//空闲时间
            m_ImageTimeHorseEnd.LoadImage(hResInstance, GetImage("TIPS_HORSE.png"));			//跑马结束
            m_ImageTimeHorse.LoadImage(hResInstance, GetImage("TIPS_HORSE_END.png"));		//跑马

            m_ImageGameOver.LoadImage(hResInstance, GetImage("GAME_SCORE.png"));			//游戏结束

            m_szBackdrop.SetSize(m_ImageBackdrop.GetWidth(), m_ImageBackdrop.GetHeight());
            m_szBackdropHand.SetSize(m_ImageBackdropHand.GetWidth(), m_ImageBackdropHand.GetHeight());
            m_szBackdropTai.SetSize(m_ImageBackdropTail.GetWidth(), m_ImageBackdropTail.GetHeight());
            m_szTotalSize.cx = m_szBackdropHand.cx + m_szBackdropTai.cx + m_szBackdrop.cx * HorseDefine.BACK_COUNT;
            m_szTotalSize.cy = m_szBackdrop.cy;

            // added by usc at 2014/03/11
            m_ImageCash.LoadImage(hResInstance, GetImage("GameCashIcon.png"));
            m_ImagePoint.LoadImage(hResInstance, GetImage("GamePointIcon.png"));

            // added by usc at 2014/03/21
            m_ImageScoreNumber.LoadImage(hResInstance, GetImage("SCORE_NUMBER.png"));
            m_ImageRank1.LoadImage(hResInstance, GetImage("Rank1.png"));
            m_ImageRank2.LoadImage(hResInstance, GetImage("Rank2.png"));

            DrawBettingPanel();
        }


        private void DrawBettingPanel()
        {
            m_bmpBettingPanel = new Bitmap(m_ImageBettingPanel.GetWidth(), m_ImageBettingPanel.GetHeight());

            Graphics gr = Graphics.FromImage(m_bmpBettingPanel);

            m_ImageBettingPanel.DrawImage(gr, 0, 0);

            gr.Dispose();
        }

        private void DrawChip(tagChipInfo chipInfo, int nChipIndex)
        {
            Graphics gr = Graphics.FromImage(m_bmpBettingPanel);

            m_ImageChipView.DrawImage(gr, chipInfo.nXPos, chipInfo.nYPos, 30, 30, nChipIndex * 30, 0);

            gr.Dispose();
        }

        //private int GetChipIndex(int nScore)
        //{
        //    int nIndex = 0;

        //    for (int i = nScore; i > 0; i = i / 10)
        //        nIndex++;

        //    return (nIndex - 1);
        //}

        public void StartRenderThread()
        {
            m_RenderTimer = SetTimer(IDI_RENDER, 130);
            //RenderWorker.RunWorkerAsync();
        }

        //销毁窗口
        void OnDestroy()
        {
            //__super::OnDestroy();

            //m_WndUserFace.SetFrameView(null);
            //m_WndUserFace.SetUserData(null);
        }

        void SetStreak(int nStreak)
        {
            m_nStreak = nStreak;
        }

        public override void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_TableDetail:
                    {
                        if (!(baseInfo is HorseInfo))
                            return;

                        m_ReceiveInfo = (HorseInfo)baseInfo;

                        m_wMeChairID = GameDefine.INVALID_CHAIR;

                        for (int i = 0; i < m_ReceiveInfo._Players.Count; i++)
                        {
                            UserInfo userInfo = m_ReceiveInfo._Players[i];

                            if (userInfo != null && userInfo.Id == m_UserInfo.Id)
                            {
                                m_wMeChairID = i;
                                break;
                            }
                        }

                        if (this.m_cbGameStatus == HorseGameStatus.GS_EMPTY)
                        {
                            // modified by usc at 2014/03/22
                            //m_cbGameStatus = (HorseGameStatus)m_ReceiveInfo._RoundIndex;
                            //m_cbGameStatus = HorseGameStatus.GS_FREE;

                            // added by usc at 2014/03/22
                            ZeroMemory(m_lPlayerBetAll);

                            m_lPlayerBetAll = m_ReceiveInfo.m_lPlayerBetAll;

                            // added at 03/26
                            for (int i = 0; i < HorseDefine.AREA_COUNT; i++)
                                PlaceUserChip_First(i, m_lPlayerBetAll[i]);

                            // added by usc at 2014/04/22
                            SetStreak(m_ReceiveInfo.m_nStreak);
                        }

                        if (m_cbGameStatus != (HorseGameStatus)m_ReceiveInfo._RoundIndex)
                        {
                            SetGameStatus((HorseGameStatus)m_ReceiveInfo._RoundIndex);
                        }
                        else
                        {
                            RefreshGameStatus();
                        }
                    }
                    break;

                case NotifyType.Reply_Betting:
                    {
                        if (!(baseInfo is BettingInfo))
                            return;

                        BettingInfo pPlayerBet = (BettingInfo)baseInfo;

                        if (pPlayerBet._UserIndex != m_wMeChairID || IsLookonMode())
                        {
                            PlaceUserChip(pPlayerBet._Area, pPlayerBet._Score);

                            PlayDirectSound(m_SoundFolder + "ADD_GOLD.wav", false);
                        }

                        m_lPlayerBetAll[pPlayerBet._Area] += pPlayerBet._Score;

                        //if (pPlayerBet._UserIndex == m_wMeChairID)
                        //{
                        //    PlaceUserChip(pPlayerBet._Area, pPlayerBet._Score);

                        //    PlayDirectSound(m_ResourceFolder + "ADD_GOLD.wav", false);

                        //    //m_lUserChipScore[pPlayerBet._Area] += pPlayerBet._Score;
                        //    m_lPlayerBet[pPlayerBet._Area] += pPlayerBet._Score;
                        //}

                        //m_lPlayerBetAll[pPlayerBet._Area] += pPlayerBet._Score;

                        //UpdateButton();
                    }
                    break;
            }
        }

        private void UpdateButton()
        {
            int lUserAllBet = 0;
            for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
                lUserAllBet += m_lPlayerBet[i];

            UserInfo pUserData = GetClientUserItem(m_wMeChairID);

            if (pUserData == null)
                return;

            double nCommissionMulti = (100 + m_GameInfo.Commission) / 100d;

            int nCurUserMoney = pUserData.GetGameMoney();

            if (nCurUserMoney - lUserAllBet * nCommissionMulti < 100 * nCommissionMulti || lUserAllBet * nCommissionMulti > (GameDefine.MAX_BETTING_MONEY - 100) * nCommissionMulti)
            {
                m_btBet1000.EnableWindow(false);

                if (m_nCurScore == 100 && this.Cursor != Cursors.Default)
                {
                    this.Cursor = Cursors.Default;
                    m_nCurScore = 0;
                }
            }
            else
                m_btBet1000.EnableWindow(true);

            if (nCurUserMoney - lUserAllBet * nCommissionMulti < 1000 * nCommissionMulti || lUserAllBet * nCommissionMulti > (GameDefine.MAX_BETTING_MONEY - 1000) * nCommissionMulti)
            {
                m_btBet1W.EnableWindow(false);

                if (m_nCurScore == 1000 && this.Cursor != Cursors.Default)
                {
                    this.Cursor = Cursors.Default;
                    m_nCurScore = 0;
                }
            }
            else
                m_btBet1W.EnableWindow(true);

            if (nCurUserMoney - lUserAllBet * nCommissionMulti < 10000 * nCommissionMulti || lUserAllBet * nCommissionMulti > (GameDefine.MAX_BETTING_MONEY - 10000) * nCommissionMulti)
            {
                m_btBet10W.EnableWindow(false);

                if (m_nCurScore == 10000 && this.Cursor != Cursors.Default)
                {
                    this.Cursor = Cursors.Default;
                    m_nCurScore = 0;
                }
            }
            else
                m_btBet10W.EnableWindow(true);

            if (nCurUserMoney - lUserAllBet * nCommissionMulti < 100000 * nCommissionMulti || lUserAllBet * nCommissionMulti > (GameDefine.MAX_BETTING_MONEY - 100000) * nCommissionMulti)
            {
                m_btBet100W.EnableWindow(false);

                if (m_nCurScore == 100000 && this.Cursor != Cursors.Default)
                {
                    this.Cursor = Cursors.Default;
                    m_nCurScore = 0;
                }
            }
            else
                m_btBet100W.EnableWindow(true);
        }

        int GetMeChairID()
        {
            return this.m_wMeChairID;
        }

        void SetPlayerWinning(int lPlayerWinning)
        {
            m_lPlayerWinning = lPlayerWinning;
        }

        private void RefreshGameStatus()
        {
            switch (m_cbGameStatus)
            {
                case HorseGameStatus.GS_FREE:
                case HorseGameStatus.GS_END:
                    {
                        //效验数据
                        //ASSERT(wDataSize==sizeof(CMD_S_SceneFree));
                        //if (wDataSize!=sizeof(CMD_S_SceneFree)) return false;

                        //消息处理
                        //CMD_S_SceneFree * pSceneFree = (CMD_S_SceneFree *)pBuffer;

                        //const tagServerAttribute * pServerAttribute = (m_pIClientKernel!=NULL)?m_pIClientKernel->GetServerAttribute():NULL;
                        //if ( pServerAttribute != NULL )
                        //    m_GameClientView.SetKindID(pServerAttribute->wKindID);


                        //设置状态
                        //SetGameStatus(GS_FREE);
                        //m_GameClientView.SetGameStatus(GS_FREE);
                        m_DlgPlayBet.SetCanBet(false);

                        //设置位置
                        int wMeChairID = GetMeChairID();
                        //SetMeChairID(SwitchViewChairID(wMeChairID));

                        //设置时间
                        //SetGameClock(GetMeChairID(), IDI_FREE, pSceneFree->nTimeLeave);
                        SetUserClock(GetMeChairID(), HorseInfo.IDI_FREE, m_ReceiveInfo.m_nFreeTime);

                        //设置场次
                        SetStreak(m_ReceiveInfo.m_nStreak);

                        //设置倍数
                        //memcpy(m_nMultiple, pSceneFree->nMultiple, sizeof(m_nMultiple));
                        // added by usc at 2014/01/07
                        m_nMultiple = m_ReceiveInfo.m_nMultiple;
                        //m_DlgPlayBet.SetMultiple(m_nMultiple);

                        //设置名字
                        for (int i = 0; i < HorseDefine.HORSES_ALL; ++i)
                            SetHorsesName(i, m_ReceiveInfo.m_szHorsesName[i]);

                        //历史记录
                        //m_DlgRecord.m_GameRecords.Clear();
                        //for ( int i = 0; i < HorseDefine.MAX_SCORE_HISTORY; ++i )
                        //{
                        //    if ( pSceneFree->GameRecords[i].nStreak != 0)
                        //    {
                        //        m_GameClientView.m_DlgRecord.m_GameRecords.Add(pSceneFree->GameRecords[i]);
                        //    }
                        //}

                        //全天赢的场次
                        m_DlgStatistics.SetWinCount(m_ReceiveInfo.m_nWinCount);

                        //限制变量
                        //m_lAreaLimitScore = pSceneFree->lAreaLimitScore;						//区域总限制
                        //m_lUserLimitScore = pSceneFree->lUserLimitScore;						//个人区域限制

                        //更新马匹
                        NweHorses();

                        //游戏分数
                        //ZeroMemory(m_lPlayerBet);
                        //ZeroMemory(m_lPlayerBetAll);


                        //开启
                        //if(CUserRight::IsGameCheatUser(m_pIClientKernel->GetUserAttribute()->dwUserRight)&&m_GameClientView.GetSafeHwnd())
                        //m_btOpenControl.ShowWindow(true);

                        //更新控制
                        //UpdateControls();
                        return;
                    }
                    break;

                case HorseGameStatus.GS_BET:
                    {
                        //效验数据
                        //ASSERT(wDataSize==sizeof(CMD_S_SceneBet));
                        //if (wDataSize!=sizeof(CMD_S_SceneBet)) return false;

                        //消息处理
                        //CMD_S_SceneBet * pSceneBet = (CMD_S_SceneBet *)pBuffer;

                        //设置ID
                        //const tagServerAttribute * pServerAttribute = (m_pIClientKernel!=NULL)?m_pIClientKernel->GetServerAttribute():NULL;
                        //if ( pServerAttribute != NULL )
                        //    m_GameClientView.SetKindID(pServerAttribute->wKindID);


                        //设置状态
                        //SetGameStatus(GS_BET);
                        //m_GameClientView.SetGameStatus(GS_BET);
                        m_DlgPlayBet.SetCanBet(true);

                        //设置位置
                        int wMeChairID = GetMeChairID();
                        //WORD wMeViewChairID = SwitchViewChairID(wMeChairID);
                        //m_GameClientView.SetMeChairID(wMeViewChairID);

                        //设置时间
                        //SetGameClock(GetMeChairID(), IDI_BET_START, pSceneBet->nTimeLeave);
                        SetTimer(HorseInfo.IDI_BET_START, m_ReceiveInfo.m_nBetTime);

                        //设置场次
                        SetStreak(m_ReceiveInfo.m_nStreak);

                        //设置倍数
                        //memcpy(m_nMultiple, pSceneBet->nMultiple, sizeof(m_nMultiple));
                        m_nMultiple = m_ReceiveInfo.m_nMultiple;
                        //m_DlgPlayBet.SetMultiple(m_nMultiple);

                        //设置名字
                        for (int i = 0; i < HorseDefine.HORSES_ALL; ++i)
                            SetHorsesName(i, m_ReceiveInfo.m_szHorsesName[i]);

                        //历史记录
                        //m_DlgRecord.m_GameRecords.Clear();
                        //for ( int i = 0; i < HorseDefine.MAX_SCORE_HISTORY; ++i )
                        //{
                        //    if ( pSceneBet->GameRecords[i].nStreak != 0)
                        //    {
                        //        m_GameClientView.m_DlgRecord.m_GameRecords.Add(pSceneBet->GameRecords[i]);
                        //    }
                        //}

                        //全天赢的场次
                        m_DlgStatistics.SetWinCount(m_ReceiveInfo.m_nWinCount);

                        //设置下注人数
                        //SetBetPlayerCount(pSceneBet->nBetPlayerCount);

                        //个人最大下注
                        //pSceneBet->lUserMaxScore;

                        //本人下注
                        //memcpy(m_lPlayerBet, pSceneBet->lPlayerBet, sizeof(m_lPlayerBet));
                        //for ( int i = 0; i < HorseDefine.AREA_ALL; ++i)
                        //    SetPlayerBet(wMeViewChairID, i, m_lPlayerBet[i]);

                        //所有下注
                        //memcpy(m_lPlayerBetAll, pSceneBet->lPlayerBetAll, sizeof(m_lPlayerBetAll));

                        // deleted by usc at 2014/03/22
                        //m_lPlayerBetAll = m_ReceiveInfo.m_lPlayerBetAll;

                        //for ( int i = 0; i < AREA_ALL; ++i)
                        //    m_GameClientView.SetAllBet(i, m_lPlayerBetAll[i]);

                        //限制变量
                        //m_lAreaLimitScore = pSceneBet->lAreaLimitScore;						//区域总限制
                        //m_lUserLimitScore = pSceneBet->lUserLimitScore;						//个人区域限制

                        //开启
                        //if(CUserRight::IsGameCheatUser(m_pIClientKernel->GetUserAttribute()->dwUserRight)&&m_GameClientView.GetSafeHwnd())
                        //m_btOpenControl.ShowWindow(true);

                        //更新控制
                        //UpdateControls();
                        // added by usc at 2014/03/17
                        UpdateButton();
                        return;
                    }
                    break;

                case HorseGameStatus.GS_BET_END:
                    {
                        //效验数据
                        //ASSERT(wDataSize==sizeof(CMD_S_SceneBetEnd));
                        //if (wDataSize!=sizeof(CMD_S_SceneBetEnd)) return false;

                        //消息处理
                        //CMD_S_SceneBetEnd * pSceneBetEnd = (CMD_S_SceneBetEnd *)pBuffer;

                        //const tagServerAttribute * pServerAttribute = (m_pIClientKernel!=NULL)?m_pIClientKernel->GetServerAttribute():NULL;
                        //if ( pServerAttribute != NULL )
                        //    m_GameClientView.SetKindID(pServerAttribute->wKindID);


                        //设置状态
                        //SetGameStatus(GS_BET_END);
                        //m_GameClientView.SetGameStatus(GS_BET_END);
                        m_DlgPlayBet.SetCanBet(false);

                        //设置位置
                        int wMeChairID = GetMeChairID();
                        //WORD wMeViewChairID = SwitchViewChairID(wMeChairID);
                        //m_GameClientView.SetMeChairID(wMeViewChairID);

                        //设置时间
                        //SetGameClock(GetMeChairID(), IDI_BET_START, pSceneBetEnd->nTimeLeave);
                        SetUserClock(GetMeChairID(), HorseInfo.IDI_BET_END, m_ReceiveInfo.m_nBetEndTime);

                        //设置场次
                        SetStreak(m_ReceiveInfo.m_nStreak);

                        //设置倍数
                        //memcpy(m_nMultiple, pSceneBetEnd->nMultiple, sizeof(m_nMultiple));
                        m_nMultiple = m_ReceiveInfo.m_nMultiple;
                        //m_DlgPlayBet.SetMultiple(m_nMultiple);

                        //设置名字
                        for (int i = 0; i < HorseDefine.HORSES_ALL; ++i)
                            SetHorsesName(i, m_ReceiveInfo.m_szHorsesName[i]);

                        //历史记录
                        //m_DlgRecord.m_GameRecords.Clear();
                        //for ( int i = 0; i < MAX_SCORE_HISTORY; ++i )
                        //{
                        //    if ( pSceneBetEnd->GameRecords[i].nStreak != 0)
                        //    {
                        //        m_GameClientView.m_DlgRecord.m_GameRecords.Add(pSceneBetEnd->GameRecords[i]);
                        //    }
                        //}

                        //全天赢的场次
                        m_DlgStatistics.SetWinCount(m_ReceiveInfo.m_nWinCount);

                        //设置下注人数
                        //m_GameClientView.SetBetPlayerCount(pSceneBetEnd->nBetPlayerCount);

                        //本人下注
                        //memcpy(m_lPlayerBet, pSceneBetEnd->lPlayerBet, sizeof(m_lPlayerBet));
                        //for ( int i = 0; i < AREA_ALL; ++i)
                        //    m_GameClientView.SetPlayerBet(wMeViewChairID, i, m_lPlayerBet[i]);

                        //所有下注
                        //memcpy(m_lPlayerBetAll, pSceneBetEnd->lPlayerBetAll, sizeof(m_lPlayerBetAll));

                        // deleted by usc at 2014/03/22
                        //m_lPlayerBetAll = m_ReceiveInfo.m_lPlayerBetAll;

                        //for ( int i = 0; i < AREA_ALL; ++i)
                        //    m_GameClientView.SetAllBet(i, m_lPlayerBetAll[i]);

                        //限制变量
                        //m_lAreaLimitScore = pSceneBetEnd->lAreaLimitScore;						//区域总限制
                        //m_lUserLimitScore = pSceneBetEnd->lUserLimitScore;						//个人区域限制

                        //开启
                        //if(CUserRight::IsGameCheatUser(m_pIClientKernel->GetUserAttribute()->dwUserRight)&&m_GameClientView.GetSafeHwnd())
                        ///m_btOpenControl.ShowWindow(true);

                        //更新控制
                        //UpdateControls();
                        return;
                    }
                    break;

                case HorseGameStatus.GS_HORSES:
                    {
                        //效验数据
                        //ASSERT(wDataSize==sizeof(CMD_S_SceneHorses));
                        //if (wDataSize!=sizeof(CMD_S_SceneHorses)) return false;

                        //消息处理
                        //CMD_S_SceneHorses * pSceneHorses = (CMD_S_SceneHorses *)pBuffer;

                        //const tagServerAttribute * pServerAttribute = (m_pIClientKernel!=NULL)?m_pIClientKernel->GetServerAttribute():NULL;
                        //if ( pServerAttribute != NULL )
                        //    m_GameClientView.SetKindID(pServerAttribute->wKindID);

                        //设置状态
                        //SetGameStatus(GS_HORSES);
                        //m_GameClientView.SetGameStatus(GS_HORSES);
                        m_DlgPlayBet.SetCanBet(false);

                        //设置位置
                        int wMeChairID = GetMeChairID();
                        //WORD wMeViewChairID = SwitchViewChairID(wMeChairID);
                        //m_GameClientView.SetMeChairID(wMeViewChairID);

                        //设置时间
                        //SetGameClock(GetMeChairID(), IDI_HORSES_START, pSceneHorses->nTimeLeave);
                        SetUserClock(GetMeChairID(), HorseInfo.IDI_HORSES_START, m_ReceiveInfo.m_nHorsesTime);

                        //设置场次
                        SetStreak(m_ReceiveInfo.m_nStreak);

                        //设置倍数
                        //memcpy(m_nMultiple, pSceneHorses->nMultiple, sizeof(m_nMultiple));
                        m_nMultiple = m_ReceiveInfo.m_nMultiple;

                        //m_DlgPlayBet.SetMultiple(m_nMultiple);

                        //设置名字
                        for (int i = 0; i < HorseDefine.HORSES_ALL; ++i)
                            SetHorsesName(i, m_ReceiveInfo.m_szHorsesName[i]);

                        //历史记录
                        //m_DlgRecord.m_GameRecords.Clear();
                        //for ( int i = 0; i < MAX_SCORE_HISTORY; ++i )
                        //{
                        //    if ( pSceneHorses->GameRecords[i].nStreak != 0)
                        //    {
                        //        m_GameClientView.m_DlgRecord.m_GameRecords.Add(pSceneHorses->GameRecords[i]);
                        //    }
                        //}

                        //全天赢的场次
                        m_DlgStatistics.SetWinCount(m_ReceiveInfo.m_nWinCount);

                        //设置下注人数
                        //m_GameClientView.SetBetPlayerCount(pSceneHorses->nBetPlayerCount);

                        //本人下注
                        //memcpy(m_lPlayerBet, pSceneHorses->lPlayerBet, sizeof(m_lPlayerBet));
                        //for ( int i = 0; i < AREA_ALL; ++i)
                        //    m_GameClientView.SetPlayerBet(wMeViewChairID, i, m_lPlayerBet[i]);

                        //所有下注
                        //memcpy(m_lPlayerBetAll, pSceneHorses->lPlayerBetAll, sizeof(m_lPlayerBetAll));

                        // deleted by usc at 2014/03/22
                        //m_lPlayerBetAll = m_ReceiveInfo.m_lPlayerBetAll;

                        //for ( int i = 0; i < AREA_ALL; ++i)
                        //    m_GameClientView.SetAllBet(i, m_lPlayerBetAll[i]);

                        //设置玩家输赢
                        if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                            SetPlayerWinning(m_ReceiveInfo.m_lPlayerWinning[m_wMeChairID]);

                        //限制变量
                        //m_lAreaLimitScore = pSceneHorses->lAreaLimitScore;						//区域总限制
                        //m_lUserLimitScore = pSceneHorses->lUserLimitScore;						//个人区域限制

                        //开启
                        //if(CUserRight::IsGameCheatUser(m_pIClientKernel->GetUserAttribute()->dwUserRight)&&m_GameClientView.GetSafeHwnd())
                        //m_btOpenControl.ShowWindow(true);

                        //更新控制
                        //UpdateControls();
                        return;
                    }
                    break;
            }

            //Invalidate();
        }

        private void SetGameStatus(HorseGameStatus gameStatus)
        {
            if (this.m_cbGameStatus != gameStatus)
            {
                m_cbGameStatus = gameStatus;

                switch (m_cbGameStatus)
                {
                    case HorseGameStatus.GS_FREE:
                        {
                            ResetGameView();
                        }
                        break;

                    case HorseGameStatus.GS_BET:
                        {
                            //效验数据
                            //if (m_DlgControl.GetSafeHwnd())
                            //{
                            //    m_DlgControl.ResetUserBet();
                            //}

                            //消息处理
                            //CMD_S_BetStart* pBetStart = (CMD_S_BetStart*)pBuffer;
                            
                            //设置时间
                            //SetGameClock(GetMeChairID(), IDI_BET_START, m_ReceiveInfo.m_nBetTime);// added by usc at 2014/03/23
                            int nElapsedTime = (int)(Math.Round((DateTime.Now - m_ReceiveInfo._dtReceiveTime).TotalMilliseconds / 1000d));

                            // added by usc at 2014/03/12
                            m_nHorseTime = m_ReceiveInfo.m_nBetTime - m_ReceiveInfo._RoundDelayTime - nElapsedTime;
                            m_BetStartTime = DateTime.Now;

                            SetUserClock(GetMeChairID(), HorseInfo.IDI_BET_START, m_nHorseTime);

                            //SetBetEndTime(m_ReceiveInfo.m_nBetEndTime);
                            m_nBetEndTime = m_ReceiveInfo.m_nBetEndTime;

                            //保存倍数
                            //memcpy(m_nMultiple, pHorsesEnd->nMultiple, sizeof(m_nMultiple));
                            m_nMultiple = m_ReceiveInfo.m_nMultiple;

                            //设置状态
                            //SetGameStatus(GS_BET);
                            //m_GameClientView.SetGameStatus(GS_BET);
                            m_DlgPlayBet.SetCanBet(true);

                            // added by usc at 2014/03/22
                            PlayDirectSound(m_SoundFolder + "GAME_START.WAV", false);

                            //设置位置
                            //SetMeChairID(SwitchViewChairID(GetMeChairID()));
                            UpdateButton();

                        }
                        break;

                    case HorseGameStatus.GS_BET_END:
                        {
                            //效验数据
                            //ASSERT(wDataSize == sizeof(CMD_S_BetEnd));
                            //if (wDataSize != sizeof(CMD_S_BetEnd)) return false;

                            //消息处理
                            //CMD_S_BetEnd* pBetEnd = (CMD_S_BetEnd*)pBuffer;

                            //设置时间
                            //SetGameClock(GetMeChairID(), IDI_BET_END, pBetEnd->nTimeLeave);
                            SetUserClock(GetMeChairID(), HorseInfo.IDI_BET_END, m_ReceiveInfo.m_nBetEndTime);

                            //设置状态
                            //SetGameStatus(GS_BET_END);
                            //m_GameClientView.SetGameStatus(GS_BET_END);
                            m_DlgPlayBet.SetCanBet(false);

                            // added at 2014/01/04
                            m_nCurScore = 0xFF;

                            this.Cursor = Cursors.Default;

                            m_btBet1000.EnableWindow(false);
                            m_btBet1W.EnableWindow(false);
                            m_btBet10W.EnableWindow(false);
                            m_btBet100W.EnableWindow(false);
                        }
                        break;

                    case HorseGameStatus.GS_HORSES:
                        {
                            //效验数据
                            //ASSERT(wDataSize == sizeof(CMD_S_HorsesStart));
                            //if (wDataSize != sizeof(CMD_S_HorsesStart)) return false;

                            //消息处理
                            //CMD_S_HorsesStart* pHorsesStart = (CMD_S_HorsesStart*)pBuffer;

                            //设置时间
                            //SetGameClock(GetMeChairID(), IDI_HORSES_START, pHorsesStart->nTimeLeave);
                            SetUserClock(GetMeChairID(), HorseInfo.IDI_HORSES_START, m_ReceiveInfo.m_nHorsesTime);

                            //设置状态
                            //SetGameStatus(GS_HORSES);
                            //m_GameClientView.SetGameStatus(GS_HORSES);
                            m_DlgPlayBet.SetCanBet(false);

                            //设置名次
                            SetRanking(m_ReceiveInfo.m_cbHorsesRanking);

                            //设置输赢
                            if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                                SetPlayerWinning(m_ReceiveInfo.m_lPlayerWinning[m_wMeChairID]);
                            else
                                SetPlayerWinning(0);

                            //开始动画
                            HorsesStart(m_ReceiveInfo.m_nHorsesSpeed);
                            return;
                        }
                        break;

                    case HorseGameStatus.GS_END:
                        {
                            StopDirectSound(m_SoundFolder + "HOS.wav");

                            SetUserClock(GetMeChairID(), HorseInfo.IDI_HORSES_END, m_ReceiveInfo.m_nHorsesEndTime);

                            //开启结束界面
                            SetTimer(IDI_HORSES_END, 1000);
                        }
                        break;

                    case HorseGameStatus.GS_EMPTY:
                        {


                        }
                        break;
                }
            }
        }

        private void HorseView_Resize(object sender, EventArgs e)
        {
            RectifyControl(this.Width, this.Height);
        }

        List<int> delteTimeList = new List<int>();
        DateTime prevTime = DateTime.Now;

        private void HorseView_Paint(object sender, PaintEventArgs e)
        {
            //DateTime startTime = DateTime.Now;

            //int nDelTime = (int)((startTime - prevTime).TotalMilliseconds);

            //if(nDelTime > 100)
            //    delteTimeList.Add(nDelTime);

            //prevTime = startTime;

            DrawGameView(e.Graphics, this.Width, this.Height);
        }

        protected override void OnGameTimer(TimerInfo timerInfo)
        {
            int nTimerID = timerInfo._Id;
            int wChairID = GetMeChairID();

            switch (nTimerID)
            {
                case HorseInfo.IDI_FREE:			//游戏空闲
                    {
                        if (timerInfo.Elapse == 0 && wChairID == GetMeChairID())
                        {
                            m_DlgPlayBet.SetCanBet(false);
                            return;
                        }
                    }
                    break;

                case HorseInfo.IDI_BET_START:		//下注开始
                    {
                        if (timerInfo.Elapse == 0 && wChairID == GetMeChairID())
                        {
                            m_DlgPlayBet.SetCanBet(false);
                            return;
                        }
                    }
                    break;

                case HorseInfo.IDI_BET_END:		//下注结束
                    {
                        PlayDirectSound(m_SoundFolder + "TIME_WARIMG.wav", false);

                        if (timerInfo.Elapse == 0 && wChairID == GetMeChairID())
                        {
                            m_DlgPlayBet.SetCanBet(false);
                            return;
                        }
                    }
                    break;

                case HorseInfo.IDI_HORSES_START:	//跑马开始
                    {
                        if (timerInfo.Elapse == 0 && wChairID == GetMeChairID())
                        {
                            m_DlgPlayBet.SetCanBet(false);
                            return;
                        }
                    }
                    break;
            }

            //Invalidate();
        }

        private void RenderWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            int nPaintingTime = 0;

            //6603渲染循环
            while (true)
            {
                //6603渲染等待
                if (nPaintingTime >= 30)
                {
                    System.Threading.Thread.Sleep(1);
                }
                else
                {
                    System.Threading.Thread.Sleep(30 - nPaintingTime);
                }

                ////6603发送消息
                DateTime prevTime = DateTime.Now;

                this.BeginInvoke(new MethodInvoker(delegate
                {
                    //this.Invalidate();
                    UpdateGameView();
                }));

                nPaintingTime = (int)((DateTime.Now - prevTime).TotalMilliseconds);

                ////TRACE(TEXT(" F [%d] \n"), nPaintingTime);
                string str = nPaintingTime.ToString();
            }
        }

        void UpdateGameView()
        {
            Image buffer = new Bitmap(this.Width, this.Height); //I usually use 32BppARGB as my color depth
            Graphics gr = Graphics.FromImage(buffer);

            //Do all your drawing with "gr"
            DrawGameView(gr, this.Width, this.Height);

            gr.Dispose();
            Graphics g = this.CreateGraphics();
            g.DrawImage(buffer, 0, 0);
            buffer.Dispose();
        }

        private void HorseView_Load(object sender, EventArgs e)
        {
            //启动渲染
            StartRenderThread();

        }

        public override void NotifyMessage(int message, object wParam, object lParam)
        {

        }

        public override void CloseView()
        {
            this.m_DlgBetRecord.ShowWindow(false);
            this.m_DlgBetRecord.Dispose();

            this.m_DlgPlayBet.ShowWindow(false);
            this.m_DlgPlayBet.Dispose();

            this.m_DlgRecord.ShowWindow(false);
            this.m_DlgRecord.Dispose();

            this.m_DlgStatistics.ShowWindow(false);
            this.m_DlgStatistics.Dispose();

            base.CloseView();
        }

        // added at 2014/01/04
        void OnBnClickedButton1000(object sender, EventArgs e)
        {
            m_nCurScore = 100;
        }

        void OnBnClickedButton1w(object sender, EventArgs e)
        {
            m_nCurScore = 1000;
        }

        void OnBnClickedButton10w(object sender, EventArgs e)
        {
            m_nCurScore = 10000;
        }

        void OnBnClickedButton100w(object sender, EventArgs e)
        {
            m_nCurScore = 100000;
        }

        private void HorseView_MouseMove(object sender, MouseEventArgs e)
        {

            CPoint MousePoint = new CPoint();
            MousePoint.SetPoint(e.Location.X, e.Location.Y);

            int nAreaNo = GetBettingArea(MousePoint);

            if (nAreaNo == 0xFF)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                switch (m_nCurScore)
                {
                    case 100:
                        this.Cursor = GetCursor("CURSOR_100.cur");
                        break;
                    case 1000:
                        this.Cursor = GetCursor("CURSOR_1000.cur");
                        break;
                    case 10000:
                        this.Cursor = GetCursor("CURSOR_1W.cur");
                        break;
                    case 100000:
                        this.Cursor = GetCursor("CURSOR_10W.cur");
                        break;
                }
            }
        }

        private void HorseView_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Cursor == Cursors.Default)
                return;

            CPoint MousePoint = new CPoint();
            MousePoint.SetPoint(e.Location.X, e.Location.Y);

            int nAreaNo = GetBettingArea(MousePoint);

            if (nAreaNo == 0xFF)
                return;

            if (nAreaNo < 0 || nAreaNo >= HorseDefine.AREA_ALL)
                return;

            if (m_cbGameStatus != HorseGameStatus.GS_BET)
            {
                PlayDirectSound(m_SoundFolder + "Alert.wav", false);
                return;
            }

            // modified by usc at 2014/02/28
            if (IsLookonMode())
                return;

            UserInfo pUserData = GetClientUserItem(m_wMeChairID);
            //m_WndUserFace.SetUserData(pUserData);
            if (pUserData == null)
                return;

            int nCurMoney = pUserData.GetGameMoney();

            int lUserAllBet = 0;
            for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
                lUserAllBet += m_lPlayerBet[i];

            // added by usc at 2014/04/10
            double CommissionMulti = (100 + m_GameInfo.Commission) / 100d;

            // 2만이상 베팅할수 없다.
            if (lUserAllBet + m_nCurScore > GameDefine.MAX_BETTING_MONEY)
            {
                PlayDirectSound(m_SoundFolder + "Alert.wav", false);
                return;
            }
            // 돈이 부족한 경우 베팅할수 없다.
            else if (nCurMoney - lUserAllBet * CommissionMulti < m_nCurScore * CommissionMulti)
            {
                PlayDirectSound(m_SoundFolder + "Alert.wav", false);
                return;
            }
            else
            {
                // 최대값과 최소값차가 5만을 넘을수 없다.
                int nMinScore = m_lPlayerBetAll[0];

                for (int i = 1; i < 4; i++)
                {
                    if (m_lPlayerBetAll[i] < nMinScore)
                        nMinScore = m_lPlayerBetAll[i];
                }

                int nNewScore = m_lPlayerBetAll[nAreaNo] + m_nCurScore;

                if (nNewScore - nMinScore > 50000 * CommissionMulti)
                {
                    PlayDirectSound(m_SoundFolder + "Alert.wav", false);
                    return;
                }
            }

            PlaceUserChip(nAreaNo, m_nCurScore);

            PlayDirectSound(m_SoundFolder + "ADD_GOLD.wav", false);

            //m_lUserChipScore[pPlayerBet._Area] += pPlayerBet._Score;
            m_lPlayerBet[nAreaNo] += m_nCurScore;
            //m_lPlayerBetAll[nAreaNo] += m_nCurScore;

            BettingInfo bettingInfo = new BettingInfo();

            bettingInfo._UserIndex = m_wMeChairID;
            bettingInfo._Area = nAreaNo;
            bettingInfo._Score = m_nCurScore;

            m_ClientSocket.Send(NotifyType.Request_Betting, bettingInfo);

            UpdateButton();
        }

        int GetBettingArea(CPoint MousePoint)
        {
            for (int i = 0; i < HorseDefine.AREA_ALL; i++)
            {
                if (m_RectArea[i].PtInRect(MousePoint))
                {
                    return i;
                }
            }
            return 0xFF;
        }

        private void PlaceUserChip_First(int nArea, int nCurScore)
        {
            int[] lScoreIndex = new int[] { 100000, 10000, 1000, 100 };

            for (int i = 0; i < CountArray(lScoreIndex); i++)
            {
                int lCellCount = nCurScore / lScoreIndex[i];

                if (lCellCount == 0) continue;

                for (int j = 0; j < lCellCount; j++)
                {
                    PlaceUserChip(nArea, lScoreIndex[i]);
                }

                nCurScore -= lCellCount * lScoreIndex[i];
            }
        }

        private void PlaceUserChip(int nArea, int nCurScore)
        {
            double fRectWidth = (double)(m_ImageBettingPanel.GetWidth()) / 6.0 + 1;

            int nXPos = 0;
            int nYPos = 0;

            bool bFound = false;
            do
            {
                nXPos = _random.Next(nArea * (int)fRectWidth, (nArea + 1) * (int)fRectWidth - 30);
                nYPos = _random.Next(30, m_ImageBettingPanel.GetHeight() - 15 - 30);

                bFound = false;

                for (int k = 0; k < m_aChipInfos[nArea].Count; k++)
                {
                    tagChipInfo pChipInfo = m_aChipInfos[nArea][k];

                    if (pChipInfo.nXPos == nXPos && pChipInfo.nYPos == nYPos)
                    {
                        bFound = true;
                        break;
                    }
                }

            } while (bFound);

            tagChipInfo chipInfo;
            chipInfo.nScore = nCurScore;
            chipInfo.nXPos = nXPos;
            chipInfo.nYPos = nYPos;

            m_aChipInfos[nArea].Add(chipInfo);

            // added by usc at 2014/04/01
            int nChipIndex = -1;

            switch (nCurScore)
            {
                case 100:
                    nChipIndex = 0;
                    break;
                case 1000:
                    nChipIndex = 1;
                    break;
                case 10000:
                    nChipIndex = 2;
                    break;
                case 100000:
                    nChipIndex = 3;
                    break;
            }

            if (nChipIndex < 0)
                return;

            DrawChip(chipInfo, nChipIndex);
        }

        bool IsLookonMode()
        {
            if (m_wMeChairID == GameDefine.INVALID_CHAIR)
                return true;

            return false;
        }

        //筹码信息
        struct tagChipInfo
        {
            public int nXPos;
            public int nYPos;
            public int nScore;
        };
    }

}
