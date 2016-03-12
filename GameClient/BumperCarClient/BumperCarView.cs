using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameControls;
using ChatEngine;
using System.IO;
using System.Net.Sockets;
using Microsoft.DirectX.AudioVideoPlayback;
using System.Diagnostics;


namespace BumperCarClient
{
    public partial class BumperCarView : GameView
    {
        //时间标识
        private const int IDI_FLASH_WINNER = 100;								//闪动标识
        private const int IDI_SHOW_CHANGE_BANKER = 101;									//轮换庄家
        private const int IDI_DISPATCH_CARD = 102;									//发牌标识
        private const int IDI_SHOWDISPATCH_CARD_TIP = 103;									//发牌提示
        private const int IDI_OPENCARD = 104;									//发牌提示
        private const int IDI_MOVECARD_END = 105;								//移動牌結束
        private const int IDI_POSTCARD = 106;									//发牌提示
        private const int IDI_OUT_CARD_TIME = 107;
        private const int IDI_JETTON_ANIMATION = 108;
        private const int IDI_HANDLELEAVE_ANIMATION = 109;
        private const int IDI_OPENBOX_ANIMATION = 110;
        private const int IDI_LEAVHANDLESOUND = 111;
        private const int IDI_PLEASEJETTONSOUND = 112;
        private const int IDI_SPEEKSOUND = 113;
        private const int IDI_FLASH_CARD = 115;								//闪动标识
        private const int IDI_FLASH_RAND_SIDE = 116;									//闪动标识
        private const int IDI_SHOW_CARD_RESULT = 118;

        //机器人下注,以下所有定时器索引保留(1000以后),专机器人提供
        private const int IDI_ANDROID_BET = 1000;

        //按钮标识
        private const int IDC_JETTON_BUTTON_100 = 200;									//按钮标识
        private const int IDC_JETTON_BUTTON_1000 = 201;									//按钮标识
        private const int IDC_JETTON_BUTTON_10000 = 202;									//按钮标识
        private const int IDC_JETTON_BUTTON_100000 = 203;									//按钮标识
        private const int IDC_JETTON_BUTTON_1000000 = 204;									//按钮标识
        private const int IDC_JETTON_BUTTON_5000000 = 205;									//按钮标识
        private const int IDC_APPY_BANKER = 206;									//按钮标识
        private const int IDC_CANCEL_BANKER = 207;									//按钮标识
        private const int IDC_SCORE_MOVE_L = 209;									//按钮标识
        private const int IDC_SCORE_MOVE_R = 210;									//按钮标识
        private const int IDC_VIEW_CHART = 211;									//按钮标识
        private const int IDC_JETTON_BUTTON_50000 = 219;									//按钮标识
        private const int IDC_JETTON_BUTTON_500000 = 212;									//按钮标识
        private const int IDC_AUTO_OPEN_CARD = 213;									//按钮标识
        private const int IDC_OPEN_CARD = 214;									//按钮标识
        private const int IDC_BANK = 215;									//按钮标识
        private const int IDC_CONTINUE_CARD = 216;									//按钮标识
        private const int IDC_BANK_STORAGE = 217;									//按钮标识
        private const int IDC_BANK_DRAW = 218;									//按钮标识
        private const int IDC_UP = 223;									//按钮标识
        private const int IDC_DOWN = 224;								//按钮标识
        private const int IDC_RADIO = 225;									//按钮标识
        private const int IDC_ADMIN = 226;									//按钮标识

        //历史记录
        const int MAX_SCORE_HISTORY = 65;									//历史个数
        const int MAX_FALG_COUNT = 9;									//标识个数
        const int COUNT_DRAWINFO = 18;

        //筹码定义
        const int JETTON_COUNT = 6;									//筹码数目
        const int JETTON_RADII = 68;									//筹码半径


        BumperCarInfo m_ReceiveInfo;

        //限制信息
        int m_lMeMaxScore;						//最大下注
        int m_lAreaLimitScore = 1000000;					//区域限制
        int m_lRobotMaxJetton;					//机器人最大筹码

        //下注信息
        int[] m_lUserJettonScore = new int[BumperCarDefine.AREA_COUNT + 1];	//个人总注
        int[] m_lAllJettonScore = new int[BumperCarDefine.AREA_COUNT + 1];	//全体总注
        int[] m_JettonQue = new int[COUNT_DRAWINFO];
        int[] m_JettonQueArea = new int[COUNT_DRAWINFO];
        int m_JettonQueIndex;

        //位置信息
        int m_nWinFlagsExcursionX;				//偏移位置
        int m_nWinFlagsExcursionY;				//偏移位置
        int m_nScoreHead;						//成绩位置
        CRect m_rcTianMen;						//闲家区域
        CRect m_rcDimen;							//闲天王区域
        CRect m_rcQiao;							//对子区域
        CRect m_rcHuangMen;							//平区域
        CRect m_rcXuanMen;							//同点平区域
        CRect m_rcHuang;							//同点平区域
        CRect m_rcJiaoR;							//庄家区域
        int m_OpenCardIndex;					//開牌順序
        int m_PostCardIndex;					//發牌順序
        int m_PostStartIndex;					//发牌起始位置
        CPoint[] m_CardTypePoint = new CPoint[BumperCarDefine.AREA_COUNT + 1];


        //扑克信息
        int m_cbTableCardArray;			//桌面扑克
        int[,] m_cbTableSortCardArray = new int[1, 1];			//桌面扑克
        bool m_blMoveFinish;
        int m_bcfirstShowCard;
        int m_bcShowCount;
        bool m_blAutoOpenCard;					//手自动
        int[] m_lUserCardType = new int[BumperCarDefine.AREA_COUNT + 1];	//个人总注
        CRect[] m_JettonAreaRect = new CRect[BumperCarDefine.AREA_COUNT];
        CRect[] m_cTmpRect = new CRect[64];
        sT_ShowJetton m_T_ShowJetton = new sT_ShowJetton();

        //历史信息
        int m_lMeStatisticScore;				//游戏成绩
        //tagClientGameRecord				m_GameRecordArrary[MAX_SCORE_HISTORY];//游戏记录
        //int								m_nRecordFirst;						//开始记录
        //int								m_nRecordLast;						//最后记录

        //状态变量
        //int							m_wMeChairID;						//我的位置
        int m_cbAreaFlash = 0xFF;						//胜利玩家
        int m_lCurrentJetton;					//当前筹码
        bool m_bShowChangeBanker;				//轮换庄家
        bool m_bNeedSetGameRecord;				//完成设置
        bool m_bWinTianMen;						//胜利标识
        bool m_bWinHuangMen;						//胜利标识
        bool m_bWinDiMen;
        bool m_bWinXuanMen;						//胜利标识
        bool m_bFlashResult;						//显示结果
        bool m_bShowGameResult;					//显示结果
        enDispatchCardTip m_enDispatchCardTip;				//发牌提示
        CPngImage m_ViewBackPng;


        //庄家信息
        int m_wBankerUser = GameDefine.INVALID_CHAIR;						//当前庄家
        int m_wBankerTime;						//做庄次数
        int m_lBankerScore;						//庄家积分
        int m_lBankerWinScore;					//庄家成绩	
        int m_lTmpBankerWinScore;				//庄家成绩	
        //bool					m_bEnableSysBanker = true;					//系统做庄

        //当局成绩
        int m_lMeCurGameScore;					//我的成绩
        int m_lMeCurGameReturnScore;			//我的成绩
        int m_lBankerCurGameScore;				//庄家成绩
        int m_lGameRevenue;						//游戏税收

        //数据变量
        CPoint[] m_PointJetton = new CPoint[BumperCarDefine.AREA_COUNT];			//筹码位置
        CPoint[] m_PointJettonNumber = new CPoint[BumperCarDefine.AREA_COUNT];	//数字位置
        List<tagJettonInfo>[] m_JettonInfoArray = new List<tagJettonInfo>[BumperCarDefine.AREA_COUNT];		//筹码数组
        bool[] m_bWinFlag = new bool[BumperCarDefine.AREA_COUNT];						//胜利标识
        int m_TopHeight;
        int m_LifeWidth;
        int m_CurArea;

        //机器人下注
        List<tagAndroidBet> m_ArrayAndroid = new List<tagAndroidBet>();

        //控件变量
        PictureButton m_btJetton100 = new PictureButton();						//筹码按钮
        PictureButton m_btJetton1000 = new PictureButton();						//筹码按钮
        PictureButton m_btJetton10000 = new PictureButton();					//筹码按钮
        PictureButton m_btJetton50000 = new PictureButton();					//筹码按钮
        PictureButton m_btJetton100000 = new PictureButton();					//筹码按钮
        PictureButton m_btJetton500000 = new PictureButton();					//筹码按钮
        PictureButton m_btJetton1000000 = new PictureButton();					//筹码按钮
        PictureButton m_btJetton5000000 = new PictureButton();					//筹码按钮

        //CSkinButton						m_btApplyBanker = new CSkinButton();					//申请庄家
        //CSkinButton						m_btCancelBanker = new CSkinButton();					//取消庄家
        CSkinButton m_btScoreMoveL = new CSkinButton();						//移动成绩
        CSkinButton m_btScoreMoveR = new CSkinButton();						//移动成绩


        CSkinButton m_btAutoOpenCard = new CSkinButton();					//自动开牌
        CSkinButton m_btOpenCard = new CSkinButton();						//手动开牌

        //CSkinButton m_btBankStorage = new CSkinButton();					//存款按钮
        //CSkinButton m_btBankDraw = new CSkinButton();						//取款按钮
        //CSkinButton						m_btUp = new CSkinButton();								//存款按钮
        //CSkinButton						m_btDown = new CSkinButton();							//取款按钮
        //CButton							m_btOpenAdmin = new CButton();						//系统控制

        int m_ShowImageIndex;
        int m_CheckImagIndex;

        //控件变量
        //CApplyUser						//m_ApplyUser;						//申请列表

        //CGameClientDlg					*m_pGameClientDlg;					//父类指针
        //BumperLogic						m_GameLogic;						//游戏逻辑
        //#ifdef  __BANKER___
        //    CDlgBank						m_DlgBank;							//银行控件
        //#endif
        CRect m_MeInfoRect;						//
        int m_Out_Bao_y;
        bool m_bShowBao;
        CRect[] m_CarRect = new CRect[32];
        CPngImage m_idb_selPng;
        int m_CarIndex;

        //控制
        //HINSTANCE						m_hInst;
        //IClientControlDlg*				m_pClientControlDlg;	

        //银行
        bool m_blCanStore;                       //是否能保存

        //界面变量
        CPngImage m_ImageViewFill;					//背景位图
        //CPngImage						m_ImageViewcentre;					//背景位图
        CPngImage m_ImageWinFlags;					//标志位图
        CPngImage m_ImageJettonView;					//筹码视图

        CPngImage m_ImageTimeFlagPng;					//时间标识
        CPngImage m_ImageDispatchCardTip;				//发牌提示
        CPngImage[] m_ImageCardType = new CPngImage[BumperCarDefine.AREA_COUNT + 1];		//数字视图
        //CPngImage						m_ImageBao;							//发牌提示
        CRect m_BaoPosion;

        //边框资源
        CPngImage m_ImageFrameTianMen;				//边框图片
        CPngImage m_ImageFrameDiMen;					//边框图片
        CPngImage m_ImageFrameHuangMen;				//边框图片
        CPngImage m_ImageFrameXuanMen;				//边框图片
        CPngImage m_ImageFrameQiao;					//边框图片
        CPngImage m_ImageFrameJiaoR;					//边框图片
        CPngImage m_ImageMeBanker;					//切换庄家
        CPngImage m_ImageChangeBanker;				//切换庄家
        CPngImage m_ImageNoBanker;					//切换庄家
        bool m_blMoveShowInfo;

        CPngImage[] m_PngPushBox = new CPngImage[7];
        CPngImage[] m_PngShowJetton = new CPngImage[4];
        bool m_bShowJettonAn;
        int m_bShowJettonIndex;
        CPngImage[] m_PngShowLeaveHandle = new CPngImage[4];
        bool m_bShowLeaveHandleAn;
        int m_bShowLeaveHandleIndex;
        bool m_bOPenBoxAn;
        int m_bOPenBoxIndex;
        bool m_blShowLastResult;

        CPngImage[] m_PngResult = new CPngImage[4];
        CPngImage m_TimerCount_png;
        CPngImage m_ImageMeScoreNumberPng;				//数字视图
        CPngImage m_ImageScoreNumberPng;

        bool m_bShowResult;
        bool m_blShowResultIndex;
        int m_lLastJetton;


        //结束资源
        CPngImage m_ImageGameEnd;						//成绩图片
        CPngImage m_pngGameEnd;
        bool m_DrawBack;
        bool m_bEnablePlaceJetton;


        int ifirstTimer;

        bool m_bFlashrandShow;

        bool m_blRungingCar;

        int iTimerStep;

        int iOpenSide;

        int iRunIndex;

        int iTotoalRun;

        CRect[] m_RectArea = new CRect[8];

        //int m_wCurrentBanker = GameDefine.INVALID_CHAIR;

        public GameStatus m_Status = GameStatus.GS_EMPTY;

        bool m_blUsing = false;

        string m_SoundFolder;

        // added by usc at 2014/03/11
        CPngImage m_ImageUserFace;
        CPngImage m_ImageCash;
        CPngImage m_ImagePoint;

        //CPngImage m_ImageNiu;

        // added by usc at 2014/03/12
        int m_nBettingTime = 0;
        DateTime m_BetStartTime;

        Image m_BackScreen; // memory bitmap for background and chips

        public string TEXT(string str)
        {
            return str;
        }

        private Bitmap GetImage(string fileName)
        {
            Bitmap resultBmp = null;

            try
            {
                string fullPath = string.Format("{0}Games\\BumperCar\\image\\{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

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
                string fullPath = string.Format("{0}Games\\BumperCar\\cursor\\{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

                using (MemoryStream ms = FileEncoder.DecryptToMemory(fullPath))
                {
                    resultCursor = GameGraphics.LoadCursorFromResource(ms.ToArray());
                }
            }
            catch { }

            return resultCursor;
        }


        public BumperCarView()
        {
            InitializeComponent();

            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPointingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);

            m_SoundFolder = AppDomain.CurrentDomain.BaseDirectory + "Games\\BumperCar\\sound\\";

            for (int i = 0; i < m_JettonInfoArray.Length; i++)
            {
                m_JettonInfoArray[i] = new List<tagJettonInfo>();
            }

            //for (int i = 0;i<AREA_COUNT;i++) m_sT_ShowInfo.blShow[i]=false;


            //string szBuffer = "";

            //加载推宝资源

            // pakcj
            //for (int i = 0;i<7;i++)
            //{
            //    szBuffer = string.Format("GetImage( "PUSHBOX{0}",i+1);
            //    m_PngPushBox[i].LoadImage(hInstance,szBuffer);
            //}

            //m_PngShowJetton[0].LoadImage(GetImage( "JETTON1.png"));
            //m_PngShowJetton[1].LoadImage(GetImage( "JETTON2.png"));
            //m_PngShowJetton[2].LoadImage(GetImage( "JETTON3.png"));
            //m_PngShowJetton[3].LoadImage(GetImage( "JETTON4.png"));


            //for (int i = 0;i<4;i++)
            //{
            //    _sntprintf(szBuffer,CountArray(szBuffer),TEXT("GetImage( "HANDLELEAVE%d"),i+1);
            //    m_PngShowLeaveHandle[i].LoadImage(hInstance,szBuffer);
            //}


            //for (int i = 0;i<4;i++)
            //{
            //    _sntprintf(szBuffer,CountArray(szBuffer),TEXT("GetImage( "RESULT%d"),i+1);
            //    m_PngResult[i].LoadImage(hInstance,szBuffer);
            //}

            m_bEnablePlaceJetton = false;

            //m_pClientControlDlg = null;
            //m_hInst = null;

            m_DTSDCheer[0] = "CHEER1.wav";
            m_DTSDCheer[1] = "CHEER2.wav";
            m_DTSDCheer[2] = "CHEER3.wav";

            SwitchToCheck();
            SetJettonHide(0);

            //控制
            //m_hInst = null;
            //m_pClientControlDlg = null;
            //m_hInst = LoadLibrary(TEXT("BumperCarBattleClientControl.dll"));
            //if ( m_hInst )
            //{
            //    typedef void * (*CREATE)(CWnd* pParentWnd); 
            //    CREATE ClientControl = (CREATE)GetProcAddress(m_hInst,"CreateClientControl"); 
            //    if ( ClientControl )
            //    {
            //        m_pClientControlDlg = static_cast<IClientControlDlg*>(ClientControl(this));
            //        m_pClientControlDlg->ShowWindow( false );
            //    }
            //}            
        }

        protected override void InitGameView()
        {
            //创建控件
            Rectangle rcCreate = new Rectangle();
            //pakcj //m_ApplyUser.Create( IDD_DLG_GAME_RECORD	, this );

            //下注按钮
            m_btJetton100.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_100);
            m_btJetton100.Click += Button_Click;

            m_btJetton1000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_1000);
            m_btJetton1000.Click += Button_Click;

            m_btJetton10000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_10000);
            m_btJetton10000.Click += Button_Click;

            m_btJetton100000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_100000);
            m_btJetton100000.Click += Button_Click;

            m_btJetton500000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_500000);
            m_btJetton500000.Click += Button_Click;

            m_btJetton1000000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_1000000);
            m_btJetton1000000.Click += Button_Click;

            m_btJetton5000000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_5000000);
            m_btJetton5000000.Click += Button_Click;

            //申请按钮
            //m_btApplyBanker.Create(null,WS_CHILD|WS_VISIBLE|WS_DISABLED,rcCreate,this,IDC_APPY_BANKER);
            //m_btCancelBanker.Create(null,WS_CHILD|WS_DISABLED,rcCreate,this,IDC_CANCEL_BANKER);
            CRect rect = new CRect();

            m_btScoreMoveL.Create("", true, false, rect, this, IDC_SCORE_MOVE_L);
            m_btScoreMoveR.Create("", true, false, rect, this, IDC_SCORE_MOVE_R);

            m_btAutoOpenCard.Create("", true, true, rect, this, IDC_AUTO_OPEN_CARD);
            m_btOpenCard.Create("", true, false, rect, this, IDC_OPEN_CARD);

            //m_btBankStorage.Create("", true, false, rect, this, IDC_BANK_STORAGE);
            //m_btBankDraw.Create("", true, true, rect, this, IDC_BANK_DRAW);

            //m_btUp.Create("", true, true, rect, this, IDC_UP);
            //m_btDown.Create("", true, true, rect, this, IDC_DOWN);

            //设置按钮
            m_btJetton100.SetButtonImage(GetImage("BT_JETTON_100.BMP"));
            m_btJetton1000.SetButtonImage(GetImage("BT_JETTON_1000.bmp"));
            m_btJetton50000.SetButtonImage(GetImage("BT_JETTON_50000.bmp"));
            m_btJetton10000.SetButtonImage(GetImage("BT_JETTON_10000.bmp"));
            m_btJetton100000.SetButtonImage(GetImage("BT_JETTON_100000.bmp"));
            m_btJetton500000.SetButtonImage(GetImage("BT_JETTON_500000.bmp"));
            m_btJetton1000000.SetButtonImage(GetImage("BT_JETTON_1000000.bmp"));
            m_btJetton5000000.SetButtonImage(GetImage("BT_JETTON_5000000.bmp"));

            m_btJetton100.SetTransparentColor(Color.FromArgb(255, 0, 255));
            m_btJetton1000.SetTransparentColor(Color.FromArgb(255, 0, 255));
            m_btJetton50000.SetTransparentColor(Color.FromArgb(255, 0, 255));
            m_btJetton10000.SetTransparentColor(Color.FromArgb(255, 0, 255));
            m_btJetton100000.SetTransparentColor(Color.FromArgb(255, 0, 255));
            m_btJetton500000.SetTransparentColor(Color.FromArgb(255, 0, 255));
            m_btJetton1000000.SetTransparentColor(Color.FromArgb(255, 0, 255));
            m_btJetton5000000.SetTransparentColor(Color.FromArgb(255, 0, 255));

            //m_btApplyBanker.SetButtonImage(GetImage( "BT_APPLY_BANKER,hResInstance,false,false);
            //m_btCancelBanker.SetButtonImage(GetImage( "BT_CANCEL_APPLY,hResInstance,false,false);

            Control hResInstance = this;
            //m_btUp.SetButtonImage(GetImage("BT_S.bmp"), hResInstance, false, false);
            //m_btDown.SetButtonImage(GetImage("BT_X.bmp"), hResInstance, false, false);

            m_btScoreMoveL.SetButtonImage(GetImage("BT_SCORE_MOVE_L.bmp"), hResInstance, false, false);
            m_btScoreMoveR.SetButtonImage(GetImage("BT_SCORE_MOVE_R.bmp"), hResInstance, false, false);

            m_btAutoOpenCard.SetButtonImage(GetImage("BT_AUTO_OPEN_CARD.bmp"), hResInstance, false, false);
            m_btOpenCard.SetButtonImage(GetImage("BT_OPEN_CARD.bmp"), hResInstance, false, false);

            //m_btBankStorage.SetButtonImage(GetImage("BT_STORAGE.bmp"), hResInstance, false, false);
            //m_btBankDraw.SetButtonImage(GetImage("BT_DRAW.bmp"), hResInstance, false, false);

            //m_btOpenAdmin.Create(null,WS_CHILD|WS_VISIBLE,CRect(4,4,11,11),this,IDC_ADMIN);
            //m_btOpenAdmin.ShowWindow(false);

            Control hInstance = this;

            m_ImageViewFill.LoadFromResource(hInstance, GetImage("VIEW_FILL.bmp"));
            //m_ImageViewcentre.LoadFromResource(hInstance,GetImage( "CENTREBACK);
            //m_ImageBao.LoadFromResource(hInstance,GetImage( "CENTREBACK);


            m_ImageWinFlags.LoadImage(GetImage("WIN_FLAGS.bmp"));

            m_ImageJettonView.LoadFromResource(hInstance, GetImage("JETTOM_VIEW.bmp"));
            m_ViewBackPng.LoadImage(GetImage("VIEW_BACK.png"));
            m_idb_selPng.LoadImage(GetImage("idb_sel_.png"));

            //for (int i = 0;i<5;i++) 
            //    m_ImageCardType[i].LoadFromResource(hInstance,GetImage( "CARDTYPE.bmp"));

            //m_ImageGameEnd.LoadFromResource(hInstance, GetImage( "Game_end.png"));
            m_ImageMeBanker.LoadFromResource(hInstance, GetImage("ME_BANKER.bmp"));
            m_ImageChangeBanker.LoadFromResource(hInstance, GetImage("CHANGE_BANKER.bmp"));
            m_ImageNoBanker.LoadFromResource(hInstance, GetImage("NO_BANKER.bmp"));
            m_ImageTimeFlagPng.LoadImage(GetImage("TIME_FLAG.png"));
            m_TimerCount_png.LoadImage(GetImage("TimerCount.png"));
            m_ImageMeScoreNumberPng.LoadImage(GetImage("ME_SCORE_NUMBER.bmp"));
            m_ImageScoreNumberPng.LoadImage(GetImage("SCORE_NUMBER.bmp"));
            m_pngGameEnd.LoadImage(GetImage("Game_end.png"));

            // added by usc at 2014/03/12
            //m_ImageCash.LoadFromResource(hInstance, GetImage("GameCashIcon.png"));
            //m_ImagePoint.LoadFromResource(hInstance, GetImage("GamePointIcon.png"));
            
            Color transColor = Color.FromArgb(255, 0, 255);

            m_ImageTimeFlagPng.SetTransparentColor(transColor);
            m_ImageJettonView.SetTransparentColor(transColor);
            m_ImageScoreNumberPng.SetTransparentColor(transColor);
            m_ImageMeScoreNumberPng.SetTransparentColor(transColor);

            // added by pakcj at 2014/04/01
            DrawBackground();
        }

        void DrawBackground()
        {
            m_BackScreen = new Bitmap(m_ViewBackPng.GetWidth(), m_ViewBackPng.GetHeight());

            Graphics gr = Graphics.FromImage(m_BackScreen);

            m_ViewBackPng.DrawImage(gr, 0, 0);

            gr.Dispose();
        }

        // added at 2014/04/01
        void DrawUserJetton(tagJettonInfo pJettonInfo, int nArea)
        {
            Graphics gr = Graphics.FromImage(m_BackScreen);

            CSize SizeJettonItem = new CSize();
            SizeJettonItem.SetSize(m_ImageJettonView.GetWidth() / 6, m_ImageJettonView.GetHeight());

            nJettonViewIndex = pJettonInfo.cbJettonIndex;

            m_ImageJettonView.DrawImage(    gr,
                                            (m_ViewBackPng.GetWidth() - this.Width) / 2 + m_PointJetton[nArea].x + pJettonInfo.nXPos,
                                            m_ViewBackPng.GetHeight() / 2 - this.Height / 2 - 120 + m_PointJetton[nArea].y + pJettonInfo.nYPos, 
                                            SizeJettonItem.cx, 
                                            SizeJettonItem.cy,
                                            nJettonViewIndex * SizeJettonItem.cx, 
                                            0);
            gr.Dispose();
        }

        //private void PlayChipSound()
        //{
        //    Audio _chipAudio = new Audio("F:\\USongchol\\Halbin\\GameClient\\BumperCarClient\\Resources\\ADD_GOLD.wav");

        //    _chipAudio.Play();
        //}

        private void BumperCarVew_Load(object sender, EventArgs e)
        {

        }


        //重置界面
        void ResetGameView()
        {
            //下注信息
            ZeroMemory(m_lUserJettonScore);

            //全体下注
            ZeroMemory(m_lAllJettonScore);

            //庄家信息
            m_wBankerUser = GameDefine.INVALID_CHAIR;
            m_wBankerTime = 0;
            m_lBankerScore = 0;
            m_lBankerWinScore = 0;
            m_lTmpBankerWinScore = 0;

            //当局成绩
            m_lMeCurGameScore = 0;
            m_lMeCurGameReturnScore = 0;
            m_lBankerCurGameScore = 0;
            m_lGameRevenue = 0;

            //状态信息
            m_lCurrentJetton = 0;
            m_cbAreaFlash = 0xFF;
            m_wMeChairID = GameDefine.INVALID_CHAIR;
            m_bShowChangeBanker = false;
            m_bNeedSetGameRecord = false;
            m_bWinTianMen = false;
            m_bWinHuangMen = false;
            m_bWinXuanMen = false;
            m_bFlashResult = false;
            m_bShowGameResult = false;
            m_enDispatchCardTip = enDispatchCardTip.enDispatchCardTip_NULL;

            m_lMeCurGameScore = 0;
            m_lMeCurGameReturnScore = 0;
            m_lBankerCurGameScore = 0;

            //m_lAreaLimitScore=0;	

            //位置信息
            m_nScoreHead = 0;
            //m_nRecordFirst= 0;	
            //m_nRecordLast= 0;	

            //历史成绩
            m_lMeStatisticScore = 0;

            //清空列表
            ////m_ApplyUser.ClearAll();

            //清除桌面
            //CleanUserJetton();

            //设置按钮
            //m_btApplyBanker.ShowWindow(true);
            //m_btApplyBanker.EnableWindow(false);
            //m_btCancelBanker.ShowWindow(false);
            //m_btCancelBanker.SetButtonImage(GetImage( "BT_CANCEL_APPLY,AfxGetInstanceHandle(),false,false);

            // added at 2014/04/1
            KillTimer(IDI_FLASH_CARD);
            KillTimer(IDI_SHOW_CARD_RESULT);
            m_blRungingCar = false;
            m_blMoveFinish = true;

            return;
        }

        //调整控件
        void RectifyControl(int nWidth, int nHeight)
        {
            //位置信息
            CSize Size = new CSize();
            Size.cy = Size.cy / 2;

            //CImageHandle ImageHandle(&m_ImageViewBack[0]);
            int iWidth = 756;// m_ImageViewBack[0].GetWidth();
            int iHeight = 705;// m_ImageViewBack[0].GetHeight();

            int LifeWidth = nWidth / 2 - iWidth / 2;
            int TopHeight = nHeight / 2 - iHeight / 2;



            for (int i = 0, j = 0; i < 32; i++)
            {
                if (j == 0)
                {
                    m_CarRect[i].top = nHeight / 2 - 276 - 8;
                    m_CarRect[i].left = nWidth / 2 - 362 - 17 + i * 81 - 5;

                }
                else
                {
                    if (j == 1)
                    {
                        m_CarRect[i].top = nHeight / 2 - 276 - 8 + (i - 8) * 57;
                        m_CarRect[i].left = nWidth / 2 - 362 - 17 + 8 * 81 - 8;

                    }
                    else
                    {
                        if (j == 2)
                        {
                            m_CarRect[i].top = nHeight / 2 - 276 - 8 + 8 * 57;
                            m_CarRect[i].left = nWidth / 2 - 362 - 17 + 8 * 81 - 5 - (i - (2 * 8)) * 81;

                        }
                        else
                        {
                            if (j == 3)
                            {
                                m_CarRect[i].top = nHeight / 2 - 276 - 8 + 8 * 57 - (i - 24) * 57;
                                m_CarRect[i].left = nWidth / 2 - 362 - 17/*nWidth/2-362-17+8*81-8-(24-(2*8))*81*/;

                            }
                        }
                    }
                }

                j = i / 8;
            }


            for (int i = 0; i < 8; i++)
            {
                if (i < 4)
                {
                    m_RectArea[i].top = nHeight / 2 - 276 - 8 + 133;
                    m_RectArea[i].left = nWidth / 2 - 362 - 17 + i * 128 - 5 + 106 - 96 - 8 + 128;
                    m_RectArea[i].bottom = m_RectArea[i].top + 162;
                    m_RectArea[i].right = m_RectArea[i].left + 128;

                }
                else
                {
                    m_RectArea[i].top = nHeight / 2 - 276 - 8 + 133 + 162;
                    m_RectArea[i].left = nWidth / 2 - 362 - 17 + (i - 4) * 128 - 5 + 106 - 96 - 8 + 128;
                    m_RectArea[i].bottom = m_RectArea[i].top + 162;
                    m_RectArea[i].right = m_RectArea[i].left + 128;

                }
            }

            int ExcursionY = 10;
            for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
            {
                m_PointJettonNumber[i].SetPoint((m_RectArea[i].right + m_RectArea[i].left) / 2, (m_RectArea[i].bottom + m_RectArea[i].top) / 2 - ExcursionY);
                m_PointJetton[i].SetPoint(m_RectArea[i].left, m_RectArea[i].top);
            }

            m_CardTypePoint[0].SetPoint(nWidth / 2 + 125, nHeight / 2 - 355 + 135 + 62);

            //移动控件
            IntPtr hDwp = GameGraphics.BeginDeferWindowPos(33);
            const uint uFlags = GameGraphics.SWP_NOACTIVATE | GameGraphics.SWP_NOZORDER | GameGraphics.SWP_NOCOPYBITS;

            //m_ApplyUser.m_viewHandle = m_hWnd;

            //列表控件
            //GameGraphics.DeferWindowPos(hDwp, m_ApplyUser,0,nWidth/2 + 148-10+50,nHeight/2-291-7-35,178,28,uFlags);

            //if(m_btUp._control != null)
            //    GameGraphics.DeferWindowPos(hDwp,m_btUp._control.Handle,0,LifeWidth+245+20+310+85,TopHeight+10+10+40,205/5,16,uFlags);

            //if(m_btDown._control != null)
            //    GameGraphics.DeferWindowPos(hDwp,m_btDown._control.Handle,0,LifeWidth+245+20+310+85+40,TopHeight+10+10+40,205/5,16,uFlags);

            m_MeInfoRect.top = TopHeight + 10 + 12;
            m_MeInfoRect.left = LifeWidth + 245 + 20 + 389;

            // deleted
            //m_btBankStorage.EnableWindow(true);

            m_BaoPosion.top = TopHeight;
            m_BaoPosion.left = nWidth / 2;


            //筹码按钮
            m_btJetton100.Width = 59;
            m_btJetton1000.Width = 59;
            m_btJetton10000.Width = 59;
            m_btJetton100000.Width = 59;
            m_btJetton1000000.Width = 59;
            m_btJetton50000.Width = 59;
            m_btJetton500000.Width = 59;
            m_btJetton5000000.Width = 59;

            m_btJetton100.Height = 51;
            m_btJetton1000.Height = 51;
            m_btJetton10000.Height = 51;
            m_btJetton100000.Height = 51;
            m_btJetton1000000.Height = 51;
            m_btJetton50000.Height = 51;
            m_btJetton500000.Height = 51;
            m_btJetton5000000.Height = 51;

            CRect rcJetton = new CRect();
            rcJetton.right = m_btJetton100.Width + 25;
            rcJetton.bottom = m_btJetton100.Height;

            int nYPos = TopHeight + 40 + 434 + 5 + 16 - 2 + 100 + 20;
            int nXPos = LifeWidth + 182 + 4 + 1 + 10 + 15;
            int nSpace = 15 + 25;

            m_TopHeight = TopHeight;
            m_LifeWidth = LifeWidth;

            m_nWinFlagsExcursionX = LifeWidth + 152 + 48 + 10;
            m_nWinFlagsExcursionY = TopHeight + 545 + 7 + 93;

            nSpace = 0;

            GameGraphics.DeferWindowPos(hDwp, m_btJetton100.Handle, 0, nXPos, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton1000.Handle, 0, nXPos + nSpace + rcJetton.Width(), nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton10000.Handle, 0, nXPos + nSpace * 2 + rcJetton.Width() * 2, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton100000.Handle, 0, nXPos + nSpace * 3 + rcJetton.Width() * 3, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton1000000.Handle, 0, nXPos + nSpace * 4 + rcJetton.Width() * 4, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton5000000.Handle, 0, nXPos + nSpace * 5 + rcJetton.Width() * 5, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            //GameGraphics.DeferWindowPos(hDwp,m_btApplyBanker,null,m_LifeWidth+609-3-41,m_TopHeight+35-3+22,0,0,uFlags|GameGraphics.SWP_NOSIZE);
            //GameGraphics.DeferWindowPos(hDwp,m_btCancelBanker,null,m_LifeWidth+609-3-41,m_TopHeight+35-3+22,0,0,uFlags|GameGraphics.SWP_NOSIZE);

            if (m_btScoreMoveL._control != null)
                GameGraphics.DeferWindowPos(hDwp, m_btScoreMoveL._control.Handle, 0, LifeWidth + 123 + 70, TopHeight + 562 + 21 + 86, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            if (m_btScoreMoveR._control != null)
                GameGraphics.DeferWindowPos(hDwp, m_btScoreMoveR._control.Handle, 0, LifeWidth + 606 - 63 + 150 - 3, TopHeight + 562 + 21 + 86, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);

            //开牌按钮
            if (m_btAutoOpenCard._control != null)
                GameGraphics.DeferWindowPos(hDwp, m_btAutoOpenCard._control.Handle, 0, LifeWidth + 624 + 20, TopHeight + 198, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            if (m_btOpenCard._control != null)
                GameGraphics.DeferWindowPos(hDwp, m_btOpenCard._control.Handle, 0, LifeWidth + 624 + 20, TopHeight + 198 + 30, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            //其他按钮
            //其他按钮
            //if (m_btBankStorage._control != null)
            //    GameGraphics.DeferWindowPos(hDwp, m_btBankStorage._control.Handle, 0, LifeWidth + 10 - 1 + 563, TopHeight + 592 - 12 + 31, 104, 400, uFlags | GameGraphics.SWP_NOSIZE);
            //if (m_btBankDraw._control != null)
            //    GameGraphics.DeferWindowPos(hDwp, m_btBankDraw._control.Handle, 0, LifeWidth + 10 - 1 + 563, TopHeight + 592 + 50 - 17 + 31, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);

            //// added by usc at 2014/03/23
            //m_btBankStorage.EnableWindow(false);
            //m_btBankDraw.EnableWindow(false);

            //结束移动

            GameGraphics.EndDeferWindowPos(hDwp);

            return;
        }

        void SwitchToCheck()
        {
            SwithToNormalView();
            return;

            //m_btAutoOpenCard.ShowWindow(false);
            //m_btOpenCard.ShowWindow(false);
            ////m_ApplyUser.ShowWindow(false);
            ////m_btApplyBanker.ShowWindow(false);
            ////m_btCancelBanker.ShowWindow(false);
            //m_btScoreMoveL.ShowWindow(false);
            //m_btScoreMoveR.ShowWindow(false);
            //m_btJetton100.ShowWindow(false);
            //m_btJetton1000.ShowWindow(false);
            //m_btJetton10000.ShowWindow(false);
            //m_btJetton50000.ShowWindow(false);
            //m_btJetton100000.ShowWindow(false);
            //m_btJetton500000.ShowWindow(false);
            //m_btJetton1000000.ShowWindow(false);
            //m_btJetton5000000.ShowWindow(false);
            //m_DrawBack = false;
            //SwithToNormalView();
        }

        void SwithToNormalView()
        {
            m_btAutoOpenCard.ShowWindow(false);
            m_btOpenCard.ShowWindow(false);
            //m_ApplyUser.ShowWindow(true);
            //m_btApplyBanker.ShowWindow(true);
            //m_btCancelBanker.ShowWindow(true);

            m_btScoreMoveL.ShowWindow(true);
            m_btScoreMoveR.ShowWindow(true);
            m_btScoreMoveL.EnableWindow(false);
            m_btScoreMoveR.EnableWindow(false);

            m_btJetton100.ShowWindow(true);
            m_btJetton1000.ShowWindow(true);
            m_btJetton10000.ShowWindow(true);
            m_btJetton50000.ShowWindow(false);
            m_btJetton100000.ShowWindow(true);
            m_btJetton500000.ShowWindow(false);
            m_btJetton1000000.ShowWindow(true);
            m_btJetton1000000.ShowWindow(true);
            m_btJetton5000000.ShowWindow(true);
            m_DrawBack = true;
        }

        //绘画界面
        static CFont InfoFont;
        static int[] lScoreJetton = new int[] { 100, 1000, 10000, 100000, 1000000, 5000000 };
        static int nJettonViewIndex = 0;

        void DrawGameView(Graphics pDC, int nWidth, int nHeight)
        {

            //获取玩家
            //UserInfo bankerInfo = null; //IClientUserItem  *pUserData = m_wBankerUser==GameDefine.INVALID_CHAIR ? null : GetClientUserItem(m_wBankerUser);

            //绘画背景
            //绘画背景
            //for (int i = 0; i < nWidth; i += m_ImageViewFill.GetWidth())
            //{
            //    for (int k = 0; k < nHeight; k += m_ImageViewFill.GetHeight())
            //    {
            //        m_ImageViewFill.DrawImage(pDC, i, k);
            //    }
            //}

            //m_ViewBackPng.DrawImage(pDC, (nWidth - m_ViewBackPng.GetWidth()) / 2, nHeight / 2 - m_ViewBackPng.GetHeight() / 2 + 120);
            pDC.DrawImage(m_BackScreen, (nWidth - m_ViewBackPng.GetWidth()) / 2, nHeight / 2 - m_ViewBackPng.GetHeight() / 2 + 120);

            if (m_blRungingCar)
            {
                m_idb_selPng.DrawImage(pDC, m_CarRect[m_CarIndex].left, m_CarRect[m_CarIndex].top,
                    m_idb_selPng.GetWidth() / 8, m_idb_selPng.GetHeight(), (m_CarIndex % 8) * (m_idb_selPng.GetWidth() / 8), 0, m_idb_selPng.GetWidth() / 8, m_idb_selPng.GetHeight());

            }
            if (m_bFlashrandShow)
            {
                m_idb_selPng.DrawImage(pDC, m_CarRect[m_CarIndex].left, m_CarRect[m_CarIndex].top,
                    m_idb_selPng.GetWidth() / 8, m_idb_selPng.GetHeight(), (m_CarIndex % 8) * (m_idb_selPng.GetWidth() / 8), 0, m_idb_selPng.GetWidth() / 8, m_idb_selPng.GetHeight());

            }
            //获取状态
            GameStatus cbGameStatus = GetGameStatus();

            //状态提示
            InfoFont.CreateFont(-16, 0, 0, 0, 400, 0, 0, 0, 134, 3, 2, CFont.ANTIALIASED_QUALITY, 2, TEXT("宋体"));
            //CFont * pOldFont=pDC->SelectObject(&InfoFont);
            //pDC->SetTextColor(RGB(255,255,0));

            //pDC->SelectObject(pOldFont);
            //InfoFont.DeleteObject();

            //时间提示
            if (m_DrawBack)
            {

                int nTimeFlagWidth = m_ImageTimeFlagPng.GetWidth() / 3;

                int nFlagIndex = 0;
                if (cbGameStatus == GameStatus.GS_JOIN)
                    nFlagIndex = 0;
                else if (cbGameStatus == GameStatus.GS_BETTING)
                    nFlagIndex = 1;
                else if (cbGameStatus == GameStatus.GS_END)
                    nFlagIndex = 2;

                m_ImageTimeFlagPng.DrawImage(pDC, nWidth / 2 - 348 + 215 + 30, m_TopHeight + 566 + 15 + 7 + 20 + 5 - 441 + 7 - 16, nTimeFlagWidth, m_ImageTimeFlagPng.GetHeight(),
                    nFlagIndex * nTimeFlagWidth, 0);

                // modified by usc at 2014/03/12
                int wUserTimer = m_nBettingTime - (int)(DateTime.Now - m_BetStartTime).TotalSeconds;

                //int wUserTimer = GetUserClock(m_wMeChairID);
                if (wUserTimer > 0)
                {
                    DrawNumberString(pDC, wUserTimer, m_LifeWidth + 424, m_TopHeight + 177, true, false);
                }

            }
            CRect rcDispatchCardTips = new CRect();
            rcDispatchCardTips.SetRect(m_LifeWidth + 612, m_TopHeight + 297, m_LifeWidth + 612 + 200, m_TopHeight + 297 + 115);

            //胜利边框
            FlashJettonAreaFrame(nWidth, nHeight, pDC);

            //筹码资源
            //CImageHandle HandleJettonView(&m_ImageJettonView);
            CSize SizeJettonItem = new CSize();
            SizeJettonItem.SetSize(m_ImageJettonView.GetWidth() / 6, m_ImageJettonView.GetHeight());

            //绘画筹码
            for (int i = 0; i < BumperCarDefine.AREA_COUNT && m_DrawBack; i++)
            {
                //变量定义
                //int lScoreCount = 0;

                ////绘画筹码
                //for (int j = 0; j < m_JettonInfoArray[i].Count; j++)
                //{
                //    //获取信息
                //    tagJettonInfo pJettonInfo = m_JettonInfoArray[i][j];

                //    //累计数字
                //    //ASSERT(pJettonInfo->cbJettonIndex<JETTON_COUNT);
                //    //lScoreCount += lScoreJetton[pJettonInfo.cbJettonIndex];

                //    nJettonViewIndex = pJettonInfo.cbJettonIndex;

                //    //绘画界面
                //    //m_ImageJettonView.DrawImage(pDC,pJettonInfo.nXPos+m_PointJetton[i].x,
                //    //    pJettonInfo.nYPos+m_PointJetton[i].y,SizeJettonItem.cx,SizeJettonItem.cy,
                //    //    nJettonViewIndex*SizeJettonItem.cx,0,Color.FromArgb(255,0,255));
                //    m_ImageJettonView.DrawImage(pDC, pJettonInfo.nXPos + m_PointJetton[i].x,
                //        pJettonInfo.nYPos + m_PointJetton[i].y, SizeJettonItem.cx, SizeJettonItem.cy,
                //        nJettonViewIndex * SizeJettonItem.cx, 0);
                //}
                //绘画数字
                if (m_lUserJettonScore[i + 1] > 0)
                    DrawNumberString(pDC, m_lUserJettonScore[i + 1], m_PointJettonNumber[i].x - 5, m_PointJettonNumber[i].y + 75, false, true);
            }

            //绘画数字
            for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex)
            {
                if (m_lAllJettonScore[nAreaIndex] > 0)
                    DrawNumberString(pDC, m_lAllJettonScore[nAreaIndex], m_PointJettonNumber[nAreaIndex - 1].x - 5, m_PointJettonNumber[nAreaIndex - 1].y, false, false);
            }

            //绘画庄家
            if (m_DrawBack)
            {
                //pakcj DrawBankerInfo(pDC,nWidth,nHeight);
                //绘画用户
                DrawMeInfo(pDC, nWidth, nHeight);
            }


            //切换庄家
            if (m_bShowChangeBanker)
            {
                int nXPos = nWidth / 2 - 130;
                int nYPos = nHeight / 2 - 160 + 120;

                //由我做庄
                if (m_wMeChairID == m_wBankerUser)
                {
                    //CImageHandle ImageHandleBanker(&m_ImageMeBanker);
                    m_ImageMeBanker.BitBlt(pDC, nXPos, nYPos);
                }
                else if (m_wBankerUser != GameDefine.INVALID_CHAIR)
                {
                    //CImageHandle ImageHandleBanker(&m_ImageChangeBanker);
                    m_ImageChangeBanker.BitBlt(pDC, nXPos, nYPos);
                }
                else
                {
                    //CImageHandle ImageHandleBanker(&m_ImageNoBanker);
                    m_ImageNoBanker.BitBlt(pDC, nXPos, nYPos);
                }
            }

            //发牌提示
            m_enDispatchCardTip = enDispatchCardTip.enDispatchCardTip_NULL;//暂时屏蔽

            if (enDispatchCardTip.enDispatchCardTip_NULL != m_enDispatchCardTip)
            {
                //if (m_ImageDispatchCardTip.IsNull()==false) 
                //    m_ImageDispatchCardTip.Destroy();

                if (enDispatchCardTip.enDispatchCardTip_Dispatch == m_enDispatchCardTip)
                    m_ImageDispatchCardTip.LoadFromResource(this, GetImage("DISPATCH_CARD.bmp"));
                else
                    m_ImageDispatchCardTip.LoadFromResource(this, GetImage("CONTINUE_CARD.bmp"));
                //CImageHandle ImageHandle(&m_ImageDispatchCardTip);
                m_ImageDispatchCardTip.BitBlt(pDC, (nWidth - m_ImageDispatchCardTip.GetWidth()) / 2, nHeight / 2);
            }

            //胜利标志
            DrawWinFlags(pDC);


            if (GetGameStatus() != GameStatus.GS_JOIN)
            {
                m_Out_Bao_y = 6;
            }

            // modified by usc at 2014/04/02
            //if (m_blMoveFinish && cbGameStatus == GameStatus.GS_END)
            if (m_blMoveFinish && cbGameStatus == GameStatus.GS_END && IsLookonMode() == false)
            {
                ShowGameResult(pDC, nWidth, nHeight);
            }

            if (m_DrawBack)
                DrawMoveInfo(pDC, m_rcTianMen);



            //绘画界面
            //if(m_bShowBao && 0)
            //{
            //    if(m_bEnableSysBanker==0&&pUserData==null)
            //        ;
            //    else
            //    {
            //        if(0<=m_Out_Bao_y&&m_Out_Bao_y<7)
            //        {
            //            if(m_Out_Bao_y>=6)
            //            {
            //                m_PngPushBox[m_Out_Bao_y].DrawImage(pDC,m_BaoPosion.left-m_PngPushBox[m_Out_Bao_y].GetWidth()/2 ,m_BaoPosion.top-120+6*45-50);

            //            }else
            //            {
            //                m_PngPushBox[m_Out_Bao_y].DrawImage(pDC,m_BaoPosion.left-m_PngPushBox[m_Out_Bao_y].GetWidth()/2 ,m_BaoPosion.top-120+m_Out_Bao_y*20-30);
            //            }
            //        }

            //    }
            //}

            //if(m_bShowJettonAn&&cbGameStatus!=GameStatus.GS_JOIN && 0)
            //{
            //    if(0<=m_bShowJettonIndex&&m_bShowJettonIndex<4)
            //    m_PngShowJetton[m_bShowJettonIndex].DrawImage(pDC,m_BaoPosion.left-m_PngShowJetton[m_bShowJettonIndex].GetWidth()/2 ,m_BaoPosion.top-120+6*20-30);
            //}
            //if(m_bShowLeaveHandleAn&&cbGameStatus==GameStatus.GS_END && 0)
            //{
            //    if(0<=m_bShowLeaveHandleIndex&&m_bShowLeaveHandleIndex<4)
            //        m_PngShowLeaveHandle[m_bShowLeaveHandleIndex].DrawImage(pDC,m_BaoPosion.left-m_PngShowLeaveHandle[m_bShowLeaveHandleIndex].GetWidth()/2 ,m_BaoPosion.top-120+6*20-30);

            //}
            return;
        }

        //设置信息
        void SetMeMaxScore(int lMeMaxScore)
        {
            if (m_lMeMaxScore != lMeMaxScore)
            {
                //设置变量
                m_lMeMaxScore = lMeMaxScore;
            }

            return;
        }

        //最大下注
        void SetAreaLimitScore(int lAreaLimitScore)
        {
            if (m_lAreaLimitScore != lAreaLimitScore)
            {
                //设置变量
                m_lAreaLimitScore = lAreaLimitScore;
            }
        }

        //设置筹码
        void SetCurrentJetton(int lCurrentJetton)
        {
            //设置变量
            //ASSERT(lCurrentJetton>=0);
            m_lCurrentJetton = lCurrentJetton;

            if (lCurrentJetton == 0)
            {
                SetJettonHide(0);
            }
            return;
        }


        ////历史记录
        //void SetGameHistory(int *bcResulte)
        //{
        //    //设置数据
        //    int bcResulteTmp[BumperCarDefine.AREA_COUNT];
        //    memcpy(bcResulteTmp,bcResulte,BumperCarDefine.AREA_COUNT);
        //    tagClientGameRecord &GameRecord = m_GameRecordArrary[m_nRecordLast];


        //    for (int i = 1;i<=BumperCarDefine.AREA_COUNT;i++)
        //    {

        //        if(bcResulteTmp[i-1]>0)
        //        {
        //            GameRecord.enOperateMen[i]=enOperateResult_Win;

        //        }else
        //        {
        //            GameRecord.enOperateMen[i]=enOperateResult_Lost;

        //        }
        //        /*if (0==m_lUserJettonScore[i]) GameRecord.enOperateMen[i]=enOperateResult_null;
        //        else if (m_lUserJettonScore[i] > 0 && (bcResulte[i-1]==4)) GameRecord.enOperateMen[i]=enOperateResult_Win;
        //        else if (m_lUserJettonScore[i] > 0 && (0==bcResulte[i-1])) GameRecord.enOperateMen[i]=enOperateResult_Lost;*/

        //    }
        //    //移动下标
        //    m_nRecordLast = (m_nRecordLast+1) % MAX_SCORE_HISTORY;
        //    if ( m_nRecordLast == m_nRecordFirst )
        //    {
        //        m_nRecordFirst = (m_nRecordFirst+1) % MAX_SCORE_HISTORY;
        //        if ( m_nScoreHead < m_nRecordFirst ) m_nScoreHead = m_nRecordFirst;
        //    }

        //    int nHistoryCount = (m_nRecordLast - m_nRecordFirst + MAX_SCORE_HISTORY) % MAX_SCORE_HISTORY;
        //    if ( MAX_FALG_COUNT < nHistoryCount ) m_btScoreMoveR.EnableWindow(true);

        //    //移到最新记录
        //    if ( MAX_FALG_COUNT < nHistoryCount )
        //    {
        //        m_nScoreHead = (m_nRecordLast - MAX_FALG_COUNT + MAX_SCORE_HISTORY) % MAX_SCORE_HISTORY;
        //        m_btScoreMoveL.EnableWindow(true);
        //        m_btScoreMoveR.EnableWindow(false);
        //    }

        //    return;
        //}

        //清理筹码
        void CleanUserJetton()
        {
            //清理数组
            for (int i = 0; i < CountArray(m_JettonInfoArray); i++)
            {
                m_JettonInfoArray[i].Clear();
            }

            DrawBackground();

            //下注信息
            ZeroMemory(m_lUserJettonScore);

            //全体下注
            ZeroMemory(m_lAllJettonScore);


            ZeroMemory(m_JettonQue);


            ZeroMemory(m_JettonQueArea);


            m_JettonQueIndex = 0;



            //更新界面
            UpdateGameView(null);

            return;
        }

        //个人下注
        void SetMePlaceJetton(int cbViewIndex, int lJettonCount)
        {
            //效验参数
            //ASSERT(cbViewIndex<=BumperCarDefine.AREA_COUNT);
            if (cbViewIndex > BumperCarDefine.AREA_COUNT) return;

            m_lUserJettonScore[cbViewIndex] = lJettonCount;

            bool blHave = false;
            for (int i = 0; i < 18; i++)
            {
                if (m_JettonQueArea[i] == cbViewIndex && m_JettonQueIndex > i)
                {
                    m_JettonQue[i] = m_lUserJettonScore[cbViewIndex];

                    blHave = true;
                    break;

                }
            }
            if (blHave == false)
            {
                if (m_JettonQueIndex > COUNT_DRAWINFO - 1)
                {
                    for (int i = 0; i < COUNT_DRAWINFO - 1; i++)
                    {
                        m_JettonQueArea[i] = m_JettonQueArea[i + 1];
                        m_JettonQue[i] = m_JettonQue[i + 1];

                    }
                    m_JettonQue[COUNT_DRAWINFO - 1] = m_lUserJettonScore[cbViewIndex];
                    m_JettonQueArea[COUNT_DRAWINFO - 1] = cbViewIndex;
                }
                else
                {
                    m_JettonQueArea[m_JettonQueIndex] = cbViewIndex;
                    m_JettonQue[m_JettonQueIndex++] = m_lUserJettonScore[cbViewIndex];

                }

                //m_JettonQueIndex = m_JettonQueIndex%16;

            }


            //更新界面
            //UpdateGameView(null);
        }

        //设置扑克
        void SetCardInfo(int cbTableCardArray)
        {
            m_cbTableCardArray = cbTableCardArray;
            //if (cbTableCardArray!=null)
            //{
            //    CopyMemory(m_cbTableCardArray,cbTableCardArray,sizeof(m_cbTableCardArray));

            //    //开始发牌
            //    DispatchCard();
            //}
            //else
            //{
            //    ZeroMemory(m_cbTableCardArray,sizeof(m_cbTableCardArray));
            //}
        }

        //设置筹码
        Random _Random = new Random();

        void PlaceUserJetton_First(int cbViewIndex, int lScoreCount)
        {

            //效验参数
            //ASSERT(cbViewIndex<=BumperCarDefine.AREA_COUNT);
            if (cbViewIndex > BumperCarDefine.AREA_COUNT) return;

            //变量定义
            //bool bPlaceJetton = false;
            int[] lScoreIndex = new int[] { 100, 1000, 10000, 100000, 1000000, 5000000 };

            int nXPos = 0, nYPos = 0;

            //边框宽度
            int nFrameWidth = 0, nFrameHeight = 0;
            int nBorderWidth = 6;

            // added by usc at 2014/03/12
            //m_lAllJettonScore[cbViewIndex] += lScoreCount;
            nFrameWidth = m_RectArea[cbViewIndex - 1].right - m_RectArea[cbViewIndex - 1].left;
            nFrameHeight = m_RectArea[cbViewIndex - 1].bottom - m_RectArea[cbViewIndex - 1].top;


            //增加判断
            //bool bAddJetton = lScoreCount > 0 ? true : false;

            //if (lScoreCount < 0)
            //{
            //    lScoreCount = -lScoreCount;
            //}

            //增加筹码
            for (int i = 0; i < CountArray(lScoreIndex); i++)
            {
                //计算数目
                int cbScoreIndex = JETTON_COUNT - i - 1;
                int lCellCount = lScoreCount / lScoreIndex[cbScoreIndex];

                //插入过虑
                if (lCellCount == 0) continue;

                //加入筹码
                for (int j = 0; j < lCellCount; j++)
                {
                    //if (true == bAddJetton)
                    //{
                        //构造变量
                        tagJettonInfo JettonInfo;
                        int nJettonSize = 55;
                        JettonInfo.cbJettonIndex = cbScoreIndex;
                        int iWSize = nFrameWidth - nJettonSize - 5;
                        int iHSize = nFrameHeight - nJettonSize - 40;

                        bool bFound = false;
                        do
                        {
                            nXPos = _Random.Next() % (iWSize);
                            nYPos = _Random.Next() % (iHSize);

                            bFound = false;

                            for (int k = 0; k < m_JettonInfoArray[cbViewIndex - 1].Count; k++)
                            {
                                tagJettonInfo pJettonInfo = m_JettonInfoArray[cbViewIndex - 1][k];

                                if (pJettonInfo.nXPos == nXPos && pJettonInfo.nYPos == nYPos)
                                {
                                    bFound = true;
                                    break;
                                }
                            }

                        } while (bFound);

                        JettonInfo.nXPos = nXPos;
                        JettonInfo.nYPos = nYPos;

                        //插入数组
                        //bPlaceJetton = true;
                        m_JettonInfoArray[cbViewIndex - 1].Add(JettonInfo);

                        // added by usc at 2014/04/01
                        DrawUserJetton(JettonInfo, cbViewIndex - 1);

                    //}
                    //else
                    //{
                    //    for (int nIndex = 0; nIndex < m_JettonInfoArray[cbViewIndex - 1].Count(); ++nIndex)
                    //    {
                    //        //移除判断
                    //        tagJettonInfo JettonInfo = m_JettonInfoArray[cbViewIndex - 1][nIndex];
                    //        if (JettonInfo.cbJettonIndex == cbScoreIndex)
                    //        {
                    //            m_JettonInfoArray[cbViewIndex - 1].RemoveAt(nIndex);
                    //            break;
                    //        }
                    //    }
                    //}
                }

                //减少数目
                lScoreCount -= lCellCount * lScoreIndex[cbScoreIndex];
            }

            //更新界面
            //if (bPlaceJetton == true)
            //    UpdateGameView(null);

            return;
        }

        void PlaceUserJetton(int cbViewIndex, int lScoreCount)
        {
            //效验参数
            //ASSERT(cbViewIndex<=BumperCarDefine.AREA_COUNT);
            if (cbViewIndex > BumperCarDefine.AREA_COUNT) return;

            //变量定义
            //bool bPlaceJetton = false;
            int[] lScoreIndex = new int[] { 100, 1000, 10000, 100000, 1000000, 5000000 };

            int cbScoreIndex = -1;

            for (int i = 0; i < CountArray(lScoreIndex); i++)
            {
                if (lScoreIndex[i] == lScoreCount)
                    cbScoreIndex = i;
            }

            if (cbScoreIndex < 0)
                return;

            int nXPos = 0, nYPos = 0;

            int nFrameWidth = 0, nFrameHeight = 0;

            // added by usc at 2014/03/12
            //m_lAllJettonScore[cbViewIndex] += lScoreCount;
            nFrameWidth = m_RectArea[cbViewIndex - 1].right - m_RectArea[cbViewIndex - 1].left;
            nFrameHeight = m_RectArea[cbViewIndex - 1].bottom - m_RectArea[cbViewIndex - 1].top;

            tagJettonInfo JettonInfo;
            int nJettonSize = 55;
            JettonInfo.cbJettonIndex = cbScoreIndex;
            int iWSize = nFrameWidth - nJettonSize - 5;
            int iHSize = nFrameHeight - nJettonSize - 40;

            bool bFound = false;
            do
            {
                nXPos = _Random.Next() % (iWSize);
                nYPos = _Random.Next() % (iHSize);

                bFound = false;

                for (int k = 0; k < m_JettonInfoArray[cbViewIndex - 1].Count; k++)
                {
                    tagJettonInfo pJettonInfo = m_JettonInfoArray[cbViewIndex - 1][k];

                    if (pJettonInfo.nXPos == nXPos && pJettonInfo.nYPos == nYPos)
                    {
                        bFound = true;
                        break;
                    }
                }

            } while (bFound);

            JettonInfo.nXPos = nXPos;
            JettonInfo.nYPos = nYPos;

            //插入数组
            //bPlaceJetton = true;
            m_JettonInfoArray[cbViewIndex - 1].Add(JettonInfo);

            // added by usc at 2014/04/01
            DrawUserJetton(JettonInfo, cbViewIndex - 1);
        }

        ////机器人下注
        //void AndroidBet(int cbViewIndex, int lScoreCount)
        //{
        //    //效验参数
        //    ASSERT(cbViewIndex<=BumperCarDefine.AREA_COUNT);
        //    if (cbViewIndex>BumperCarDefine.AREA_COUNT) 
        //        return;

        //    if ( lScoreCount <= 0 )
        //        return;

        //    tagAndroidBet Androi;
        //    Androi.cbJettonArea = cbViewIndex;
        //    Androi.lJettonScore = lScoreCount;
        //    m_ArrayAndroid.Add(Androi);
        //    SetTimer(IDI_ANDROID_BET,100,null);
        //    int nHaveCount = 0;
        //    for ( int i = 0 ; i < m_ArrayAndroid.GetCount(); ++i)
        //    {
        //        if(m_ArrayAndroid[i].lJettonScore > 0)
        //            nHaveCount++;
        //    }
        //    int nElapse = 0;
        //    if ( nHaveCount <= 1 )
        //        nElapse = 260;
        //    else if ( nHaveCount <= 2 )
        //        nElapse = 160;
        //    else
        //        nElapse = 100;

        //    SetTimer(IDI_ANDROID_BET+m_ArrayAndroid.GetCount(),nElapse,null);
        //}

        //当局成绩
        void SetCurGameScore(int lMeCurGameScore, int lMeCurGameReturnScore, int lBankerCurGameScore, int lGameRevenue)
        {
            m_lMeCurGameScore = lMeCurGameScore;
            m_lMeCurGameReturnScore = lMeCurGameReturnScore;
            m_lBankerCurGameScore = lBankerCurGameScore;
            m_lGameRevenue = lGameRevenue;
        }

        //设置胜方
        void SetWinnerSide(bool[] blWin, bool bSet)
        {
            //设置时间
            for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
            {

                m_bWinFlag[i] = blWin[i];
            }
            if (true == bSet)
            {
                //设置定时器
                SetTimer(IDI_FLASH_WINNER, 500);

                //全胜判断
                bool blWinAll = true;

                for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
                {

                    if (m_bWinFlag[i] == true)
                    {
                        blWinAll = false;
                    }
                }
                if (blWinAll)
                {
                    //重设资源
                    //HINSTANCE hInstance=AfxGetInstanceHandle();
                }
            }
            else
            {
                //清楚定时器
                KillTimer(IDI_FLASH_WINNER);

                //全胜判断
                bool blWinAll = true;

                for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
                {

                    if (m_bWinFlag[i] == true)
                    {
                        blWinAll = false;
                    }
                }
                if (blWinAll)
                {
                }
            }

            //设置变量
            m_bFlashResult = bSet;
            m_bShowGameResult = bSet;
            m_cbAreaFlash = 0xFF;

            //更新界面
            UpdateGameView(null);

            return;
        }

        //获取区域
        int GetJettonArea(CPoint MousePoint)
        {
            for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
            {
                if (m_RectArea[i].PtInRect(MousePoint))
                {
                    return i + 1;
                }
            }
            return 0xFF;
        }

        //绘画数字
        void DrawNumberString(Graphics pDC, int lNumber, int nXPos, int nYPos, bool blTimer, bool bMeScore)
        {
            //加载资源
            //CImageHandle HandleScoreNumber(&m_ImageScoreNumber);
            //CImageHandle HandleMeScoreNumber(&m_ImageMeScoreNumber);
            CSize SizeScoreNumber = new CSize();
            SizeScoreNumber.SetSize(m_ImageScoreNumberPng.GetWidth() / 10, m_ImageScoreNumberPng.GetHeight());

            if (bMeScore) SizeScoreNumber.SetSize(m_ImageMeScoreNumberPng.GetWidth() / 10, m_ImageMeScoreNumberPng.GetHeight());

            if (blTimer)
            {
                SizeScoreNumber.SetSize(m_TimerCount_png.GetWidth() / 10, m_TimerCount_png.GetHeight());
            }

            //计算数目
            int lNumberCount = 0;
            int lNumberTemp = lNumber;
            do
            {
                lNumberCount++;
                lNumberTemp /= 10;
            } while (lNumberTemp > 0);

            int tmpNum = lNumberCount + lNumberCount / 4;

            //位置定义
            int nYDrawPos = nYPos - SizeScoreNumber.cy / 2;
            int nXDrawPos = (int)(nXPos + tmpNum * SizeScoreNumber.cx / 2 - SizeScoreNumber.cx);

            if (!blTimer)
                nXDrawPos = (int)(nXPos + (SizeScoreNumber.cx - 2) * lNumberCount / 2 + (SizeScoreNumber.cx - 5) * (lNumberCount > 3 ? 0 : (lNumberCount - 1) / 3) / 2);

            //绘画桌号
            for (int i = 0; i < lNumberCount; i++)
            {
                //绘画号码
                if (i != 0 && i % 3 == 0)
                {
                    if (blTimer)
                    {
                    }
                    else
                    {
                        nXDrawPos += 5;
                        if (bMeScore)
                        {
                            m_ImageMeScoreNumberPng.DrawImage(pDC, nXDrawPos, nYDrawPos, SizeScoreNumber.cx, SizeScoreNumber.cy,
                                10 * SizeScoreNumber.cx, 0);
                        }
                        else
                        {
                            m_ImageScoreNumberPng.DrawImage(pDC, nXDrawPos, nYDrawPos, SizeScoreNumber.cx, SizeScoreNumber.cy,
                                10 * SizeScoreNumber.cx, 0);
                        }
                        nXDrawPos -= (SizeScoreNumber.cx - 2);
                    }
                }
                int lCellNumber = (int)(lNumber % 10);
                if (blTimer)
                {
                    m_TimerCount_png.DrawImage(pDC, nXDrawPos, nYDrawPos, SizeScoreNumber.cx, SizeScoreNumber.cy,
                        lCellNumber * SizeScoreNumber.cx, 0);

                }
                else
                {
                    if (bMeScore)
                    {
                        m_ImageMeScoreNumberPng.DrawImage(pDC, nXDrawPos, nYDrawPos, SizeScoreNumber.cx, SizeScoreNumber.cy,
                            lCellNumber * SizeScoreNumber.cx, 0);
                    }
                    else
                    {
                        m_ImageScoreNumberPng.DrawImage(pDC, nXDrawPos, nYDrawPos, SizeScoreNumber.cx, SizeScoreNumber.cy,
                            lCellNumber * SizeScoreNumber.cx, 0);
                    }

                }
                //设置变量
                lNumber /= 10;

                if (blTimer)
                    nXDrawPos -= SizeScoreNumber.cx;
                else
                    nXDrawPos -= (SizeScoreNumber.cx - 2);
            };

            return;
        }

        //绘画数字
        void DrawNumberStringWithSpace(Graphics pDC, int lNumber, int nXPos, int nYPos)
        {
            int lTmpNumber = lNumber;
            String strNumber;
            String strTmpNumber1;
            String strTmpNumber2;
            bool blfirst = true;
            bool bLongNum = false;
            int nNumberCount = 0;

            strTmpNumber1 = "";
            strTmpNumber2 = "";
            strNumber = "";

            if (lNumber == 0)
                strNumber = TEXT("0");

            if (lNumber < 0)
                lNumber = -lNumber;

            if (lNumber >= 100)
                bLongNum = true;

            while (lNumber > 0)
            {
                strTmpNumber1 = string.Format("{0}", lNumber % 10);
                nNumberCount++;
                strTmpNumber2 = strTmpNumber1 + strTmpNumber2;

                if (nNumberCount == 3)
                {
                    if (blfirst)
                    {
                        strTmpNumber2 += (TEXT("") + strNumber);
                        blfirst = false;
                    }
                    else
                    {
                        strTmpNumber2 += (TEXT(",") + strNumber);
                    }

                    strNumber = strTmpNumber2;
                    nNumberCount = 0;
                    strTmpNumber2 = TEXT("");
                }
                lNumber /= 10;
            }

            if (strTmpNumber2.Length == 0)
            {
                if (bLongNum)
                    strTmpNumber2 += (TEXT(",") + strNumber);
                else
                    strTmpNumber2 += strNumber;

                strNumber = strTmpNumber2;
            }

            if (lTmpNumber < 0)
                strNumber = TEXT("-") + strNumber;
            //输出数字
            //pDC->TextOut(nXPos,nYPos,strNumber);
            pDC.DrawString(strNumber, InfoFont.GetFont(), Brushes.White, new Point(nXPos, nYPos));

        }

        //绘画数字
        static String strNumber = "", strTmpNumber1, strTmpNumber2;

        String NumberStringWithSpace(int lNumber)
        {
            strNumber = "";

            if (lNumber == 0)
                strNumber = TEXT("0");

            int nNumberCount = 0;
            int lTmpNumber = lNumber;
            if (lNumber < 0) lNumber = -lNumber;

            while (lNumber > 0)
            {
                strNumber = string.Format("{0}{1}", lNumber % 10, strNumber);

                nNumberCount++;

                if (nNumberCount == 3 && lNumber > 10)
                {
                    strNumber = "," + strNumber;
                    nNumberCount = 0;
                }
                lNumber /= 10;
            }

            if (lTmpNumber < 0)
                strNumber = "-" + strNumber;

            return strNumber;
        }

        //绘画数字
        void DrawNumberStringWithSpace(Graphics pDC, int lNumber, CRect rcRect, int nFormat)
        {
            CDC cDC = new CDC();
            cDC.SetGraphics(pDC);
            cDC.SelectObject(InfoFont);
            cDC.SetTextColor(Color.White);

            strNumber = NumberStringWithSpace(lNumber);

            //输出数字
            if (nFormat == -1)
                cDC.DrawText(strNumber, rcRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP | CDC.DT_SINGLELINE);
            else
                cDC.DrawText(strNumber, rcRect, nFormat);
        }

        void SetMoveCardTimer()
        {
            KillTimer(IDI_POSTCARD);
            SetTimer(IDI_POSTCARD, 300);
            SetTimer(IDI_DISPATCH_CARD, 5000);

        }
        void KillCardTime()
        {
            KillTimer(IDI_FLASH_WINNER);
            KillTimer(IDI_POSTCARD);
            KillTimer(IDI_OPENCARD);
            KillTimer(IDI_DISPATCH_CARD);
            KillTimer(IDI_SHOWDISPATCH_CARD_TIP);

        }
        void StartRunCar(int iTimer, int nRoundDelayTime)
        {
            m_bFlashrandShow = false;
            KillTimer(IDI_FLASH_RAND_SIDE);
            KillTimer(IDI_FLASH_CARD);
            KillTimer(IDI_SHOW_CARD_RESULT);
            iTimerStep = 400;
            SetTimer(IDI_FLASH_CARD, iTimer);
            ifirstTimer = 0;
            iOpenSide = m_cbTableCardArray;

            // modified by usc at 20014/03/17
            // iTotoalRun=iOpenSide+32*3-2;
            iTotoalRun = iOpenSide - 2 + 32 * 3;

            iRunIndex = 0;
            m_CarIndex = 1;
            

            if (nRoundDelayTime <= 1)
            {
                SetTimer(IDI_SHOW_CARD_RESULT, 15 * 1000);
            }
            else if (nRoundDelayTime > 1 && nRoundDelayTime < 15)
            {
                PreRunCar(nRoundDelayTime);

                SetTimer(IDI_SHOW_CARD_RESULT, (15 - nRoundDelayTime) * 1000);
            }
            else
            {
                SetTimer(IDI_SHOW_CARD_RESULT, 20);
            }

            m_blRungingCar = true;
        }

        // added by usc at 2014/04/06
        private void PreRunCar(int nRoundDelayTime)
        {
            int nTotalMSec = 400;
            do
            {
                m_CarIndex = (m_CarIndex + 1) % 32;
                iRunIndex++;

                if (iRunIndex < 10)
                {
                    iTimerStep -= 43;

                }
                if (iRunIndex >= iTotoalRun - 15)
                {
                    iTimerStep += 47;
                }

                nTotalMSec += iTimerStep;

            } while (nTotalMSec < nRoundDelayTime * 1000);
        }

        void RuningCar(int iTimer)
        {
            if (iRunIndex < 10)
            {
                iTimerStep -= 43;

            }
            if (iRunIndex >= iTotoalRun - 15)
            {
                iTimerStep += 47;
            }

            if (iTotoalRun == iRunIndex)
            {
                // added by usc at 2014/03/22
                ShowCardResult();

                return;

            }
            if (iTimerStep < 0)
            {
                return;
            }
            KillTimer(IDI_FLASH_CARD);
            SetTimer(IDI_FLASH_CARD, iTimer);

        }

        void FlashWinOpenSide()
        {

        }

        void StartRandShowSide()
        {
            return;

            KillTimer(IDI_FLASH_RAND_SIDE);
            iTimerStep = 100;
            m_bFlashrandShow = true;
            SetTimer(IDI_FLASH_RAND_SIDE, iTimerStep);
        }

        //定时器消息
        Random random = new Random();

        protected override void OnTimer(TimerInfo timerInfo)
        {
            //return;

            int nIDEvent = timerInfo._Id;

            if (GetGameStatus() == GameStatus.GS_END)
            {
                if (IDI_FLASH_CARD == nIDEvent)
                {
                    m_CarIndex = (m_CarIndex + 1) % 32;
                    iRunIndex++;

                    RuningCar(iTimerStep);

                    OnPlaySound(3, 3);

                    //更新界面
                    UpdateGameView(null);
                }
                if (IDI_SHOW_CARD_RESULT == nIDEvent)
                {

                    // modified by usc at 2014/03/16
                    for (int i = iRunIndex; i < iTotoalRun; i++)
                    {
                        m_CarIndex = (m_CarIndex + 1) % 32;
                        iRunIndex++;

                        if (iRunIndex == iTotoalRun)
                        {
                            ShowCardResult();

                            OnPlaySound(3, 3);

                            //更新界面
                            UpdateGameView(null);
                        }
                    }

                    // added by usc at 2014/03/22
                    //ShowCardResult();

                    return;
                }

                //闪动胜方
                if (nIDEvent == IDI_FLASH_WINNER)
                {
                    //设置变量
                    m_bFlashResult = !m_bFlashResult;

                    //更新界面
                    UpdateGameView(null);
                    return;
                }
            }

            if (IDI_FLASH_RAND_SIDE == nIDEvent)
            {
                m_bFlashrandShow = !m_bFlashrandShow;
                m_CarIndex = _Random.Next() % 32;
                //更新界面
                UpdateGameView(null);
            }
            
            //轮换庄家
            else if (nIDEvent == IDI_SHOW_CHANGE_BANKER)
            {
                ShowChangeBanker(false);
                return;
            }
            else if (nIDEvent == IDI_DISPATCH_CARD)
            {
                return;
            }
            else if (IDI_SHOWDISPATCH_CARD_TIP == nIDEvent)
            {
                SetDispatchCardTip(enDispatchCardTip.enDispatchCardTip_NULL);
            }
            else if (nIDEvent == IDI_ANDROID_BET)
            {
                //更新界面
                UpdateGameView(null);
                return;
            }
            else if (nIDEvent >= (int)(IDI_ANDROID_BET + 1) && nIDEvent < (int)(IDI_ANDROID_BET + m_ArrayAndroid.Count() + 1))
            {
                int Index = nIDEvent - IDI_ANDROID_BET - 1;
                if (Index < 0 || Index >= m_ArrayAndroid.Count)
                {
                    //ASSERT(false);
                    KillTimer(nIDEvent);
                    return;
                }

                if (m_ArrayAndroid[Index].lJettonScore > 0)
                {
                    int[] lScoreIndex = new int[] { 5000000, 1000000, 100000, 10000, 1000, 100 };
                    int cbViewIndex = m_ArrayAndroid[Index].cbJettonArea;

                    //增加筹码
                    for (int i = 0; i < CountArray(lScoreIndex); i++)
                    {
                        if (lScoreIndex[i] > m_lRobotMaxJetton)
                            continue;

                        if (m_ArrayAndroid[Index].lJettonScore >= lScoreIndex[i])
                        {
                            m_ArrayAndroid[Index].lJettonScore -= lScoreIndex[i];
                            m_lAllJettonScore[cbViewIndex] += lScoreIndex[i];

                            tagJettonInfo JettonInfo;
                            int iWSize = m_RectArea[cbViewIndex - 1].right - m_RectArea[cbViewIndex - 1].left - 60;
                            int iHSize = m_RectArea[cbViewIndex - 1].bottom - m_RectArea[cbViewIndex - 1].top - 95;
                            JettonInfo.nXPos = _Random.Next() % (iWSize);
                            JettonInfo.nYPos = _Random.Next() % (iHSize);
                            JettonInfo.cbJettonIndex = JETTON_COUNT - i - 1;

                            //插入数组
                            m_JettonInfoArray[cbViewIndex - 1].Add(JettonInfo);
                            
                            // added at 2014/04/01
                            DrawUserJetton(JettonInfo, cbViewIndex - 1);

                            //播放声音
                            OnPlaySound(7, 7);
                            break;
                        }
                    }
                }
                return;
            }

            //__super::OnTimer(nIDEvent);
        }

        // added by usc at 2014/03/22
        private void ShowCardResult()
        {
            ChatEngine.StringInfo messageInfo = new ChatEngine.StringInfo();

            // added by usc at 2014/02/26
            //if (m_wMeChairID != GameDefine.INVALID_CHAIR)
            //{
                UserInfo pUserData = m_UserInfo;

                if (pUserData != null)
                {
                    messageInfo.UserId = pUserData.Id;
                    messageInfo.strRoomID = pUserData.GameId;

                    m_ClientSocket.Send(NotifyType.Request_GameResult, messageInfo);
                }
            //}

            KillTimer(IDI_FLASH_CARD);
            KillTimer(IDI_SHOW_CARD_RESULT);
            m_blRungingCar = false;
            m_blMoveFinish = true;

            //设置定时器
            DispatchCard();
            FinishDispatchCard(true);
        }

        //接受其他控件传来的消息
        int OnViLBtUp(int wParam, CPoint pPoint)
        {
            //CPoint *pPoint = (CPoint*)lParam;

            //ScreenToClient(pPoint);
            //OnLButtonUp(1,*pPoint);
            return 1;
        }

        void SetEnablePlaceJetton(bool bEnablePlaceJetton)
        {
            m_bEnablePlaceJetton = bEnablePlaceJetton;
        }

        int GetCurrentJetton()
        {
            return m_lCurrentJetton;
        }

        void UpdataJettonButton()
        {
            if (m_CurArea == 0xFF || m_bEnablePlaceJetton == false)
            {
                return;
            }
            //计算积分
            int lCurrentJetton = GetCurrentJetton();
            int lLeaveScore = m_lMeMaxScore;
            for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex) lLeaveScore -= m_lUserJettonScore[nAreaIndex];
            //最大下注
            int lUserMaxJetton = 0;

            lUserMaxJetton = GetUserMaxJetton(m_CurArea);

            lLeaveScore = Math.Min((lLeaveScore), lUserMaxJetton); //用户可下分 和最大分比较 由于是五倍 


            lCurrentJetton = m_lLastJetton;

            //设置光标
            if (lCurrentJetton > lLeaveScore)
            {
                if (lLeaveScore >= 5000000) SetCurrentJetton(5000000);
                else if (lLeaveScore >= 1000000) SetCurrentJetton(1000000);
                else if (lLeaveScore >= 100000) SetCurrentJetton(100000);
                else if (lLeaveScore >= 10000) SetCurrentJetton(10000);
                else if (lLeaveScore >= 1000) SetCurrentJetton(1000);
                else if (lLeaveScore >= 100) SetCurrentJetton(100);
                else SetCurrentJetton(0);
            }
            else
            {
                SetCurrentJetton(m_lLastJetton);
            }
        }

        //鼠标消息
        void OnRButtonDown(int nFlags, CPoint Point)
        {
            //设置变量
            m_lCurrentJetton = 0;

            if (GetGameStatus() != GameStatus.GS_END && m_cbAreaFlash != 0xFF)
            {
                m_cbAreaFlash = 0xFF;
                SetJettonHide(0);
                UpdateGameView(null);
            }
            m_lLastJetton = 0;

            //__super::OnLButtonDown(nFlags,Point);
        }

        void SetJettonHide(int ID)
        {
            PictureButton[] btJetton = new PictureButton[] { null, m_btJetton100, m_btJetton1000, m_btJetton10000, m_btJetton100000, m_btJetton1000000, m_btJetton5000000 };

            if (1 <= ID && ID <= 6)
            {
                for (int i = 1; i <= 6; i++)
                {
                    if (i != ID)
                        btJetton[i].ShowWindow(true);
                }
                btJetton[ID].ShowWindow(false);

                //OutputDebugString("设置ID\r\n");

            }
            else
            {
                for (int i = 1; i <= 6; i++)
                {
                    btJetton[i].ShowWindow(true);
                }
            }
        }

        //光标消息
        bool OnSetCursor(Point mousePosition)
        {
            if (m_lCurrentJetton != 0)
            {
                //获取区域
                CPoint MousePoint = new CPoint();
                MousePoint.SetPoint(mousePosition.X, mousePosition.Y);
                //GetCursorPos(&MousePoint);
                //ScreenToClient(&MousePoint);

                int cbJettonArea = GetJettonArea(MousePoint);

                //设置变量
                if (m_cbAreaFlash != cbJettonArea)
                {
                    m_cbAreaFlash = cbJettonArea;
                    UpdateGameView(null);
                }

                bool bButtonArea = false;
                PictureButton[] pRgnButton = new PictureButton[] { m_btJetton100, m_btJetton1000, m_btJetton10000, m_btJetton100000, m_btJetton1000000, m_btJetton5000000 };
                for (int i = 0; i < CountArray(pRgnButton) && cbJettonArea == 0xFF; ++i)
                {
                    //CRgn rgn;
                    //CRect rect;
                    //rgn.CreateRectRgn(0,0,0,0);
                    //pRgnButton[i]->GetWindowRgn(rgn);
                    //pRgnButton[i]->GetWindowRect(rect);
                    //ScreenToClient(&rect);

                    //rgn.OffsetRgn(CPoint(rect.left,rect.top));

                    CRect rgn = new CRect(pRgnButton[i].Left, pRgnButton[i].Top, pRgnButton[i].Right, pRgnButton[i].Bottom);

                    if (rgn.PtInRect(MousePoint))
                    {
                        //rgn.DeleteObject();
                        bButtonArea = true;
                        break;
                    }
                    //rgn.DeleteObject();
                }

                //区域判断
                if (cbJettonArea == 0xFF && !bButtonArea)
                {
                    this.Cursor = Cursors.Arrow;// (LoadCursor(null, IDC_ARROW));
                    SetJettonHide(0);
                    return true;
                }

                //最大下注
                int lMaxJettonScore = GetUserMaxJetton(cbJettonArea);

                //合法判断
                int iTimer = 1;

                if (cbJettonArea != 0xFF)
                {
                    if ((m_lAllJettonScore[cbJettonArea] + m_lCurrentJetton) > m_lAreaLimitScore)
                    {
                        SetJettonHide(0);
                        this.Cursor = Cursors.Default;// SetCursor(LoadCursor(null, IDC_NO));
                        return true;

                    }
                    if (lMaxJettonScore < m_lCurrentJetton * iTimer && !bButtonArea)
                    {
                        SetJettonHide(0);
                        this.Cursor = Cursors.Default;//DefSetCursor(LoadCursor(null,IDC_NO));
                        return true;
                    }

                }

                //设置光标
                switch (m_lCurrentJetton)
                {
                    case 100:
                        {
                            this.Cursor = GetCursor("SCORE_100.cur");
                            return true;
                        }
                    case 1000:
                        {
                            this.Cursor = GetCursor("SCORE_1000.cur");
                            return true;
                        }
                    case 10000:
                        {
                            this.Cursor = GetCursor("SCORE_10000.cur");
                            return true;
                        }
                    case 50000:
                        {
                            this.Cursor = GetCursor("SCORE_50000.cur");
                            return true;
                        }
                    case 100000:
                        {
                            this.Cursor = GetCursor("SCORE_100000.cur");
                            return true;
                        }
                    case 500000:
                        {
                            this.Cursor = GetCursor("SCORE_500000.cur");
                            return true;
                        }
                    case 1000000:
                        {
                            this.Cursor = GetCursor("SCORE_1000000.cur");
                            return true;
                        }
                    case 5000000:
                        {
                            this.Cursor = GetCursor("SCORE_5000000.cur");
                            return true;
                        }
                        //UpdateGameView(null);
                }
            }
            else
            {
                ClearAreaFlash();
            }

            return false;
            //__super::OnSetCursor(pWnd, nHitTest, uMessage);
        }

        void ClearAreaFlash()
        {
            m_cbAreaFlash = 0xFF;

            // added by usc at 2014/03/19
            this.Cursor = Cursors.Default;
        }

        //轮换庄家
        void ShowChangeBanker(bool bChangeBanker)
        {
            //轮换庄家
            if (bChangeBanker)
            {
                SetTimer(IDI_SHOW_CHANGE_BANKER, 3000);
                m_bShowChangeBanker = true;
            }
            else
            {
                KillTimer(IDI_SHOW_CHANGE_BANKER);
                m_bShowChangeBanker = false;
            }

            //更新界面
            UpdateGameView(null);
        }

        ////上庄按钮
        //void OnApplyBanker()
        //{
        //    CGameFrameEngine::GetInstance()->SendMessage(IDM_APPLY_BANKER,1,0);

        //}

        ////下庄按钮
        //void OnCancelBanker()
        //{
        //    CGameFrameEngine::GetInstance()->SendMessage(IDM_APPLY_BANKER,0,0);
        //}

        //艺术字体
        void DrawTextString(Graphics pDC, string pszString, Color crText, Color crFrame, int nXPos, int nYPos)
        {
            //变量定义
            int nStringLength = pszString.Length;
            int[] nXExcursion = new int[] { 1, 1, 1, 0, -1, -1, -1, 0 };
            int[] nYExcursion = new int[] { -1, 0, 1, 1, 1, 0, -1, -1 };

            //绘画边框
            //pDC->SetTextColor(crFrame);
            Brush foreBrush = new SolidBrush(crFrame);

            for (int i = 0; i < CountArray(nXExcursion); i++)
            {
                //pDC->TextOut(nXPos+nXExcursion[i],nYPos+nYExcursion[i],pszString,nStringLength);
                pDC.DrawString(pszString, InfoFont.GetFont(), foreBrush, new Point(nXPos + nXExcursion[i], nYPos + nYExcursion[i]));

            }

            //绘画字体
            //pDC->SetTextColor(crText);
            Brush brush = new SolidBrush(crText);

            //pDC->TextOut(nXPos,nYPos,pszString,nStringLength);
            pDC.DrawString(pszString, InfoFont.GetFont(), brush, new Point(nXPos, nYPos));

            return;
        }

        //庄家信息
        void SetBankerInfo(int dwBankerUserID, int lBankerScore)
        {
            //    //庄家椅子号
            //    int wBankerUser=GameDefine.INVALID_CHAIR;

            //    //查找椅子号
            //    if (0!=dwBankerUserID)
            //    {
            //        for (int wChairID=0; wChairID<MAX_CHAIR; ++wChairID)
            //        {
            //            IClientUserItem  *pUserData=GetClientUserItem(wChairID);
            //            if (null!=pUserData && dwBankerUserID==pUserData->GetUserID())
            //            {
            //                wBankerUser=wChairID;
            //                break;
            //            }
            //        }
            //    }

            //    //切换判断
            //    if (m_wBankerUser!=wBankerUser)
            //    {
            //        m_wBankerUser=wBankerUser;
            //        m_wBankerTime=0;
            //        m_lBankerWinScore=0;	
            //        m_lTmpBankerWinScore=0;
            //    }
            //    m_lBankerScore=lBankerScore;
        }

        void DrawMoveInfo(Graphics pDC, CRect rcRect)
        {
            return;
        }

        //绘画标识
        static Color clrOld;
        static int m_nYPos = 0, m_nXPos = 0;
        static int nFlagsIndex = 0;

        void DrawWinFlags(Graphics pDC)
        {
            //非空判断
            //if (m_nRecordLast==m_nRecordFirst) 
            //    return;
            //int nIndex = m_nScoreHead;

            //clrOld = pDC->SetTextColor(RGB(52,116,23));
            //int nDrawCount=0;

            //while ( nIndex != m_nRecordLast && ( m_nRecordLast!=m_nRecordFirst ) && nDrawCount < MAX_FALG_COUNT )
            //{
            //    //胜利标识
            //    tagClientGameRecord &ClientGameRecord = m_GameRecordArrary[nIndex];



            //    for (int i=1; i<=1; ++i)
            //    {
            //        //位置变量
            //        nYPos=m_nWinFlagsExcursionY+i*27-i*1;
            //        nXPos=m_nWinFlagsExcursionX + ((nIndex - m_nScoreHead + MAX_SCORE_HISTORY) % MAX_SCORE_HISTORY) * (37);

            //        //胜利标识

            //        for (int i = 1;i<=BumperCarDefine.AREA_COUNT;i++)
            //        {

            //            if(ClientGameRecord.enOperateMen[i]==enOperateResult_Win)
            //            {
            //                nFlagsIndex = (i-1)*2;
            //                if(i==5)
            //                {
            //                    nFlagsIndex = 1;

            //                }
            //                if(i==6)
            //                {
            //                    nFlagsIndex = 3;

            //                }
            //                if(i==7)
            //                {
            //                    nFlagsIndex = 5;

            //                }
            //                if(i==8)
            //                {
            //                    nFlagsIndex = 7;

            //                }

            //            }
            //        }
            //        {
            //            //绘画标识
            //            m_ImageWinFlags.DrawImage( pDC, nXPos, nYPos, m_ImageWinFlags.GetWidth()/8, 
            //                m_ImageWinFlags.GetHeight(),m_ImageWinFlags.GetWidth()/8 * nFlagsIndex, 0);

            //        }

            //    }

            //    //移动下标
            //    nIndex = (nIndex+1) % MAX_SCORE_HISTORY;
            //    nDrawCount++;
            //}
            //pDC->SetTextColor(clrOld);
        }

        //手工搓牌
        void OnOpenCard()
        {
            //CGameFrameEngine::GetInstance()->SendMessage(IDM_OPEN_CARD,0,0);
            m_blAutoOpenCard = false;
        }
        //自动搓牌
        void OnAutoOpenCard()
        {
            //CGameFrameEngine::GetInstance()->SendMessage(IDM_AUTO_OPEN_CARD,0,0);
            m_blAutoOpenCard = true;
        }

        //移动按钮
        void OnScoreMoveL()
        {
            //if ( m_nRecordFirst == m_nScoreHead ) return;

            //m_nScoreHead = (m_nScoreHead - 1 + MAX_SCORE_HISTORY) % MAX_SCORE_HISTORY;
            //if ( m_nScoreHead == m_nRecordFirst ) m_btScoreMoveL.EnableWindow(false);

            //m_btScoreMoveR.EnableWindow(true);

            ////更新界面
            //UpdateGameView(null);
        }

        //移动按钮
        void OnScoreMoveR()
        {
            //int nHistoryCount = ( m_nRecordLast - m_nScoreHead + MAX_SCORE_HISTORY ) % MAX_SCORE_HISTORY;
            //if ( nHistoryCount == MAX_FALG_COUNT ) return;

            //m_nScoreHead = ( m_nScoreHead + 1 ) % MAX_SCORE_HISTORY;
            //if ( nHistoryCount-1 == MAX_FALG_COUNT ) m_btScoreMoveR.EnableWindow(false);

            //m_btScoreMoveL.EnableWindow(true);

            ////更新界面
            //UpdateGameView(null);
        }

        //显示结果
        void ShowGameResult(Graphics pDC, int nWidth, int nHeight)
        {
            if (false == m_bShowGameResult)
                return;

            int nXPos = nWidth / 2 - 200 + 20 + 30;
            int nYPos = nHeight / 2 - 208;
            m_pngGameEnd.DrawImage(pDC, nXPos + 2 - 20 - 30, nYPos + 60,
                                    m_pngGameEnd.GetWidth(), m_pngGameEnd.GetHeight(), 0, 0);

            //pDC->SetTextColor(RGB(255,255,255));

            CRect rcMeWinScore = new CRect();
            CRect rcMeReturnScore = new CRect();

            rcMeWinScore.left = nXPos + 2 + 40 + 30;
            rcMeWinScore.top = nYPos + 70 + 32 + 10 + 10;
            rcMeWinScore.right = rcMeWinScore.left + 111;
            rcMeWinScore.bottom = rcMeWinScore.top + 34;

            rcMeReturnScore.left = nXPos + 2 + 150 + 50;
            rcMeReturnScore.top = nYPos + 70 + 32 + 10 + 10;
            rcMeReturnScore.right = rcMeReturnScore.left + 111;
            rcMeReturnScore.bottom = rcMeReturnScore.top + 34;

            String strMeGameScore, strMeReturnScore;
            DrawNumberStringWithSpace(pDC, m_lMeCurGameScore, rcMeWinScore, CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_CENTER | CDC.DT_SINGLELINE);
            DrawNumberStringWithSpace(pDC, m_lMeCurGameReturnScore, rcMeReturnScore, CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_CENTER | CDC.DT_SINGLELINE);

            CRect rcBankerWinScore = new CRect();
            rcBankerWinScore.left = nXPos + 2 + 40 + 30;
            rcBankerWinScore.top = nYPos + 70 + 32 + 10 + 35 + 10;
            rcBankerWinScore.right = rcBankerWinScore.left + 111;
            rcBankerWinScore.bottom = rcBankerWinScore.top + 34;

            string strBankerCurGameScore = "";
            strBankerCurGameScore = string.Format("{0}", m_lBankerCurGameScore);
            DrawNumberStringWithSpace(pDC, m_lBankerCurGameScore, rcBankerWinScore, CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_CENTER | CDC.DT_SINGLELINE);
        }

        //透明绘画
        void GetAllWinArea(int[] bcWinArea, int bcAreaCount, int InArea)
        {
            if (InArea == 0xFF)
            {
                return;
            }
            ZeroMemory(bcWinArea);


            int lMaxSocre = 0;

            for (int i = 0; i < 32; i++)
            {
                int[] bcOutCadDataWin = new int[BumperCarDefine.AREA_COUNT];
                int bcData = i + 1;

                BumperLogic.GetCardType(bcData, 1, bcOutCadDataWin);

                for (int j = 0; j < BumperCarDefine.AREA_COUNT; j++)
                {

                    if (bcOutCadDataWin[j] > 0 && j == InArea - 1)
                    {
                        int Score = 0;
                        for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex)
                        {
                            if (bcOutCadDataWin[nAreaIndex - 1] > 1)
                            {
                                Score += m_lAllJettonScore[nAreaIndex] * (bcOutCadDataWin[nAreaIndex - 1]);
                            }
                        }
                        if (Score >= lMaxSocre)
                        {
                            lMaxSocre = Score;
                            bcWinArea = bcOutCadDataWin;

                        }
                        break;
                    }
                }
            }
        }
        //最大下注
        int GetUserMaxJetton(int cbJettonArea)
        {
            if (cbJettonArea == 0xFF)
                return 0;

            //已下注额
            int lNowJetton = 0;
            //ASSERT(BumperCarDefine.AREA_COUNT<=CountArray(m_lUserJettonScore));

            for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex)
                lNowJetton += m_lUserJettonScore[nAreaIndex];

            //庄家金币
            int lBankerScore = 0x7fffffff;
            if (m_wBankerUser != GameDefine.INVALID_CHAIR)
                lBankerScore = m_lBankerScore;

            int[] bcWinArea = new int[BumperCarDefine.AREA_COUNT];
            int LosScore = 0;
            int WinScore = 0;

            GetAllWinArea(bcWinArea, BumperCarDefine.AREA_COUNT, cbJettonArea);

            for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex)
            {
                if (bcWinArea[nAreaIndex - 1] > 1)
                {
                    LosScore += m_lAllJettonScore[nAreaIndex] * (bcWinArea[nAreaIndex - 1]);
                }
                else
                {
                    if (bcWinArea[nAreaIndex - 1] == 0)
                    {
                        WinScore += m_lAllJettonScore[nAreaIndex];
                    }
                }
            }

            int lTemp = lBankerScore;
            lBankerScore = lBankerScore + WinScore - LosScore;

            if (lBankerScore < 0)
            {
                if (m_wBankerUser != GameDefine.INVALID_CHAIR)
                {
                    lBankerScore = m_lBankerScore;
                }
                else
                {
                    lBankerScore = 0x7fffffff;
                }
            }

            //区域限制
            int lMeMaxScore;

            if ((m_lMeMaxScore - lNowJetton) > m_lAreaLimitScore)
            {
                lMeMaxScore = m_lAreaLimitScore;
            }
            else
            {
                lMeMaxScore = m_lMeMaxScore - lNowJetton;
                lMeMaxScore = lMeMaxScore;
            }

            //庄家限制
            if (bcWinArea[cbJettonArea - 1] > 0)
                lMeMaxScore = Math.Min(lMeMaxScore, (lBankerScore) / (bcWinArea[cbJettonArea - 1]));

            //非零限制
            lMeMaxScore = Math.Max(lMeMaxScore, 0);

            return lMeMaxScore;
        }

        //成绩设置
        void SetGameScore(int lMeCurGameScore, int lMeCurGameReturnScore, int lBankerCurGameScore)
        {
            m_lMeCurGameScore = lMeCurGameScore;
            m_lMeCurGameReturnScore = lMeCurGameReturnScore;
            m_lBankerCurGameScore = lBankerCurGameScore;
        }

        //开始发牌
        void DispatchCard()
        {

            //设置标识
            m_bNeedSetGameRecord = true;
        }

        //结束发牌
        void FinishDispatchCard(bool bRecord /*= true*/ )
        {
            //完成判断
            if (m_bNeedSetGameRecord == false) return;

            //设置标识
            m_bNeedSetGameRecord = false;

            //删除定时器
            KillTimer(IDI_DISPATCH_CARD);

            int[] bcResulteOut = new int[BumperCarDefine.AREA_COUNT];
            //memset(bcResulteOut,0,BumperCarDefine.AREA_COUNT);

            BumperLogic.GetCardType(m_cbTableCardArray, 1, bcResulteOut);

            //保存记录
            if (bRecord)
            {
                //SetGameHistory(bcResulteOut);
            }

            //累计积分
            m_lMeStatisticScore += m_lMeCurGameScore;
            m_lBankerWinScore = m_lTmpBankerWinScore;


            bool[] blWin = new bool[BumperCarDefine.AREA_COUNT];
            for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
            {

                if (bcResulteOut[i] > 0)
                {
                    blWin[i] = true;
                }
                else
                {
                    blWin[i] = false;
                }
            }

            //设置赢家
            SetWinnerSide(blWin, true);

            //播放声音
            if (m_lMeCurGameScore > 0)
            {
                OnPlaySound(5, 5);

            }
            else if (m_lMeCurGameScore < 0)
            {
                OnPlaySound(4, 4);

            }
            else
            {
                OnPlaySound(4, 4);
            }
        }

        //胜利边框
        void FlashJettonAreaFrame(int nWidth, int nHeight, Graphics pDC)
        {
            //合法判断
            if (m_bFlashResult && !m_blRungingCar)
            {
                m_idb_selPng.DrawImage(pDC, m_CarRect[m_CarIndex].left, m_CarRect[m_CarIndex].top,
                    m_idb_selPng.GetWidth() / 8, m_idb_selPng.GetHeight(), (m_CarIndex % 8) * (m_idb_selPng.GetWidth() / 8), 0, m_idb_selPng.GetWidth() / 8, m_idb_selPng.GetHeight());
            }

            //下注判断
            if (m_bFlashResult)
            {
                for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
                {
                    if (m_bWinFlag[i])
                    {
                        //pakcj pDC->Draw3dRect(m_RectArea[i].left,m_RectArea[i].top,m_RectArea[i].Width()  ,m_RectArea[i].Height(),RGB(255,255,0),RGB(255,255,0));
                        pDC.DrawRectangle(new Pen(Color.FromArgb(255, 255, 0)), m_RectArea[i].left, m_RectArea[i].top, m_RectArea[i].Width(), m_RectArea[i].Height());
                    }
                }
            }
            else
            {
                if (GetGameStatus() == GameStatus.GS_BETTING && m_cbAreaFlash != 0xFF)
                {
                    //pakcj pDC->Draw3dRect(m_RectArea[m_cbAreaFlash-1].left,m_RectArea[m_cbAreaFlash-1].top,m_RectArea[m_cbAreaFlash-1].Width()  ,m_RectArea[m_cbAreaFlash-1].Height(),RGB(255,255,0),RGB(255,255,0));
                    pDC.DrawRectangle(new Pen(Color.FromArgb(255, 255, 0)), m_RectArea[m_cbAreaFlash - 1].left, m_RectArea[m_cbAreaFlash - 1].top, m_RectArea[m_cbAreaFlash - 1].Width(), m_RectArea[m_cbAreaFlash - 1].Height());
                }                
            }
        }

        //推断赢家
        void DeduceWinner(bool[] bWinMen)
        {
            int bcData = m_cbTableCardArray;

            if (1 == bcData || bcData == 2 || bcData == 1 + 8 || bcData == 2 + 8 || bcData == 1 + 2 * 8 || bcData == 2 + 2 * 8 || bcData == 1 + 3 * 8 || bcData == 2 + 3 * 8)
            {
                if (bcData % 2 == 1)
                    bWinMen[0] = true;
                else
                    bWinMen[1] = true;

            }
            else if (3 == bcData || bcData == 4 || bcData == 3 + 8 || bcData == 4 + 8 || bcData == 3 + 2 * 8 || bcData == 4 + 2 * 8 || bcData == 3 + 3 * 8 || bcData == 4 + 3 * 8)
            {
                if (bcData % 2 == 1)
                    bWinMen[2] = true;
                else
                    bWinMen[3] = true;



            }
            else if (5 == bcData || bcData == 6 || bcData == 5 + 8 || bcData == 6 + 8 || bcData == 5 + 2 * 8 || bcData == 6 + 2 * 8 || bcData == 5 + 3 * 8 || bcData == 6 + 3 * 8)
            {
                if (bcData % 2 == 1)
                    bWinMen[4] = true;
                else
                    bWinMen[5] = true;

            }
            else if (7 == bcData || bcData == 8 || bcData == 7 + 8 || bcData == 8 + 8 || bcData == 7 + 2 * 8 || bcData == 8 + 2 * 8 || bcData == 7 + 3 * 8 || bcData == 8 + 3 * 8)
            {
                if (bcData % 2 == 1)
                    bWinMen[6] = true;
                else
                    bWinMen[7] = true;

            }
        }

        //控件命令
        private void Button_Click(object sender, EventArgs e)
        {
            if (!(sender is PictureButton))
                return;

            PictureButton button = (PictureButton)sender;

            //获取ID
            int wControlID = button._ButtonId;

            //控件判断
            switch (wControlID)
            {
                case IDC_JETTON_BUTTON_100:
                    {
                        //设置变量
                        m_lCurrentJetton = 100;
                        break;
                    }
                case IDC_JETTON_BUTTON_1000:
                    {
                        //设置变量
                        m_lCurrentJetton = 1000;
                        break;
                    }
                case IDC_JETTON_BUTTON_10000:
                    {
                        //设置变量
                        m_lCurrentJetton = 10000;
                        break;
                    }
                case IDC_JETTON_BUTTON_50000:
                    {
                        //设置变量
                        m_lCurrentJetton = 50000;
                        break;
                    }
                case IDC_JETTON_BUTTON_100000:
                    {
                        //设置变量
                        m_lCurrentJetton = 100000;
                        break;
                    }
                case IDC_JETTON_BUTTON_500000:
                    {
                        //设置变量
                        m_lCurrentJetton = 500000;
                        break;
                    }
                case IDC_JETTON_BUTTON_1000000:
                    {
                        //设置变量
                        m_lCurrentJetton = 1000000;
                        break;
                    }
                case IDC_JETTON_BUTTON_5000000:
                    {
                        //设置变量
                        m_lCurrentJetton = 5000000;
                        break;
                    }
                case IDC_AUTO_OPEN_CARD:
                    {
                        break;
                    }
                case IDC_OPEN_CARD:
                    {
                        break;
                    }
                case IDC_BANK:
                    {
                        break;
                    }
                case IDC_CONTINUE_CARD:
                    {
                        SwithToNormalView();
                        OnContinueCard(0, 0);
                        //CGameFrameEngine::GetInstance()->SendMessage(IDM_CONTINUE_CARD,0,0);
                        break;
                    }
                case IDC_RADIO:
                    {
                        m_CheckImagIndex = 0;

                    }

                    break;
                case IDC_RADIO + 1:
                    {
                        m_CheckImagIndex = 1;
                    }
                    break;
                case IDC_RADIO + 2:
                    {
                        m_CheckImagIndex = 2;

                    }
                    break;
                case IDC_RADIO + 3:
                    {
                        m_CheckImagIndex = 3;

                    }
                    break;
                case IDC_BANK_DRAW:
                    {
                        OnBankDraw();
                        break;
                    }
                case IDC_BANK_STORAGE:
                    {
                        OnBankStorage();
                        break;
                    }

            }

            m_lLastJetton = m_lCurrentJetton;

            //return CGameFrameView::OnCommand(wParam, lParam);
        }

        //我的位置
        void SetMeChairID(int dwMeUserID)
        {
            m_wMeChairID = dwMeUserID;

            //查找椅子号
            //for (int wChairID=0; wChairID<MAX_CHAIR; ++wChairID)
            //{
            //    IClientUserItem  *pUserData=GetClientUserItem(wChairID);
            //    if (null!=pUserData && dwMeUserID==pUserData->GetUserID())
            //    {
            //        m_wMeChairID=wChairID;
            //        break;
            //    }
            //}
        }

        //设置提示
        void SetDispatchCardTip(enDispatchCardTip DispatchCardTip)
        {
            //设置变量
            m_enDispatchCardTip = DispatchCardTip;

            //设置定时器
            if (enDispatchCardTip.enDispatchCardTip_NULL != DispatchCardTip)
                SetTimer(IDI_SHOWDISPATCH_CARD_TIP, 2 * 1000);
            else KillTimer(IDI_SHOWDISPATCH_CARD_TIP);

            //更新界面
            UpdateGameView(null);
        }

        //绘画庄家
        static CRect StrRect;

        //void DrawBankerInfo(Graphics pDC,int nWidth,int nHeight)
        //{
        //    //庄家信息																											
        //    pDC->SetTextColor(RGB(255,255,0));

        //    //获取玩家
        //    IClientUserItem  *pUserData = m_wBankerUser==GameDefine.INVALID_CHAIR ? null : GetClientUserItem(m_wBankerUser);

        //    //位置信息
        //    m_LifeWidth;	
        //    m_TopHeight;

        //    StrRect.left = m_LifeWidth+79-66;
        //    StrRect.top = m_TopHeight+84-66;
        //    StrRect.right = StrRect.left + 171;
        //    StrRect.bottom = StrRect.top + 15;

        //    //庄家名字
        //    string Text = "";
        //    if(m_bEnableSysBanker)
        //    {
        //        _sntprintf(Text,256,_TEXT("当前庄家：%s"),pUserData==null?(m_bEnableSysBanker?TEXT("系统坐庄"):TEXT("系统坐庄")):pUserData->GetNickName());
        //        pDC->DrawText(Text, StrRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP|CDC.DT_SINGLELINE );
        //    }
        //    else
        //    {
        //        _sntprintf(Text,256,_TEXT("当前庄家：%s"),pUserData==null?(m_bEnableSysBanker==false?TEXT("无人坐庄"):TEXT("无人坐庄")):pUserData->GetNickName());
        //        pDC->DrawText(Text, StrRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP|CDC.DT_SINGLELINE );
        //    }

        //    StrRect.top = StrRect.bottom;
        //    StrRect.bottom = StrRect.top + 15;
        //    _sntprintf(Text,256,_TEXT("连庄盘数：%d"),m_wBankerTime);
        //    pDC->DrawText(Text, StrRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP|CDC.DT_SINGLELINE );

        //    StrRect.top = StrRect.bottom;
        //    StrRect.bottom = StrRect.top + 15;
        //    _sntprintf(Text,256,_TEXT("庄家财富：%s"),NumberStringWithSpace(pUserData==null?0:pUserData->GetUserScore()));
        //    pDC->DrawText(Text, StrRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP|CDC.DT_SINGLELINE);

        //    StrRect.top = StrRect.bottom;
        //    StrRect.bottom = StrRect.top + 15;
        //    _sntprintf(Text,256,_TEXT("庄家输赢：%s"),NumberStringWithSpace(m_lBankerWinScore));
        //    pDC->DrawText(Text, StrRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP|CDC.DT_SINGLELINE );
        //}


        void SetFirstShowCard(int bcCard)
        {


        }
        //绘画玩家
        static CRect StrRect_DrawMe;

        void DrawMeInfo(Graphics g, int nWidth, int nHeight)
        {
            CDC pDC = new CDC();
            pDC.SetGraphics(g);

            //合法判断
            // deleted by usc at 2014/04/02
            //if (GameDefine.INVALID_CHAIR == m_wMeChairID) return;

            //庄家信息																											
            //pDC->SetTextColor(RGB(255,255,0));
            pDC.SetTextColor(Color.FromArgb(255, 255, 0));

            //获取玩家
            UserInfo pUserData = m_UserInfo;

            StrRect.left = m_LifeWidth + 79 - 66 + 17;
            StrRect.top = m_TopHeight + 84 - 66 + 610;
            StrRect.right = StrRect.left + 164;
            StrRect.bottom = StrRect.top + 15;

            //庄家名字
            string Text = string.Format("昵称：{0}", pUserData.Nickname);
            pDC.DrawText(Text, StrRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP | CDC.DT_SINGLELINE);

            StrRect.top = StrRect.bottom;
            StrRect.bottom = StrRect.top + 15;
            Text = string.Format("金币：{0}", NumberStringWithSpace(pUserData.GetGameMoney()));
            pDC.DrawText(Text, StrRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP | CDC.DT_SINGLELINE);

            StrRect.top = StrRect.bottom;
            StrRect.bottom = StrRect.top + 15;

            int lMeJetton = 0;
            for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex) 
                lMeJetton += m_lUserJettonScore[nAreaIndex];

            // added by usc at 2014/04/10
            int nCommission = lMeJetton * m_GameInfo.Commission / 100;

            Text = string.Format("可用：{0}", NumberStringWithSpace(pUserData == null ? 0 : pUserData.GetGameMoney() - lMeJetton - nCommission));
            pDC.DrawText(Text, StrRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP | CDC.DT_SINGLELINE);

            StrRect.top = StrRect.bottom;
            StrRect.bottom = StrRect.top + 15;
            Text = string.Format("成绩：{0}", NumberStringWithSpace(m_lMeStatisticScore));
            pDC.DrawText(Text, StrRect, CDC.DT_END_ELLIPSIS | CDC.DT_LEFT | CDC.DT_TOP | CDC.DT_SINGLELINE);
        }

        //////////////////////////////////////////////////////////////////////////
        //银行存款
        void OnBankStorage()
        {
            //#ifdef __SPECIAL___
            //    //获取接口
            //    CGameClientDlg *pGameClientDlg=CONTAINING_RECORD(this,CGameClientDlg,m_GameClientView);
            //    IClientKernel *pIClientKernel=(IClientKernel *)pGameClientDlg->m_pIClientKernel;

            //    if (null!=pIClientKernel)
            //    {
            //        CRect btRect;
            //        m_btBankStorage.GetWindowRect(&btRect);
            //        ShowInsureSave(pIClientKernel,CPoint(btRect.right,btRect.top));

            //    }
            //#endif
        }

        //银行取款
        void OnBankDraw()
        {
            //#ifdef __SPECIAL___
            //    //获取接口
            //    CGameClientDlg *pGameClientDlg=CONTAINING_RECORD(this,CGameClientDlg,m_GameClientView);
            //    IClientKernel *pIClientKernel=(IClientKernel *)pGameClientDlg->m_pIClientKernel;

            //    if (null!=pIClientKernel)
            //    {
            //        CRect btRect;
            //        m_btBankDraw.GetWindowRect(&btRect);
            //        ShowInsureGet(pIClientKernel,CPoint(btRect.right,btRect.top));

            //    }
            //#endif
        }


        //void OnUp()
        //{
        //    m_ApplyUser.m_AppyUserList.SendMessage(WM_VSCROLL, MAKEint(SB_LINEUP,0),null);
        //    m_ApplyUser.m_AppyUserList.Invalidate(true);
        //    double nPos = m_ApplyUser.m_AppyUserList.GetScrollPos(SB_VERT);
        //    double nMax = m_ApplyUser.m_AppyUserList.GetScrollLimit(SB_VERT);

        //}

        //void OnDown()
        //{
        //    tagSCROLLINFO scrollInfo = {};
        //    m_ApplyUser.m_AppyUserList.GetScrollInfo(SB_VERT, &scrollInfo);
        //    int nPos = scrollInfo.nPos - scrollInfo.nMin;
        //    if(nPos>//m_ApplyUser.m_AppyUserList.GetItemCount()-3)
        //    {
        //        return ;
        //    }
        //    m_ApplyUser.m_AppyUserList.SendMessage(WM_VSCROLL, MAKEint(SB_LINEDOWN,0),null);
        //    m_ApplyUser.m_AppyUserList.Invalidate(true);
        //    double nMax = m_ApplyUser.m_AppyUserList.GetScrollLimit(SB_VERT);
        //}

        void StartMove()
        {
            SetTimer(IDI_MOVECARD_END, 250);
            m_Out_Bao_y = 0;
            m_bShowJettonIndex = 0;
            m_bShowLeaveHandleIndex = 0;
            m_bShowBao = true;
            m_bShowJettonAn = false;
            m_bShowLeaveHandleAn = false;
            m_bOPenBoxAn = false;
            m_bOPenBoxIndex = 0;
            m_blShowLastResult = false;
            m_bShowResult = false;

        }
        void StartJetton_AniMationN()
        {
            SetTimer(IDI_JETTON_ANIMATION, 400);
            m_bShowJettonAn = true;
            m_bShowJettonIndex = 0;


        }

        void StartOPenBox()
        {
            m_bOPenBoxAn = true;
            m_bOPenBoxIndex = 0;
            m_bShowBao = false;
            m_Out_Bao_y = 6;
            SetJettonHide(0);
            SetTimer(IDI_OPENBOX_ANIMATION, 400);

        }

        void StartHandle_Leave()
        {
            return;

            m_bShowLeaveHandleAn = true;
            m_bShowLeaveHandleIndex = 0;
            SetTimer(IDI_HANDLELEAVE_ANIMATION, 400);

            OnPlaySound(0, 1);


        }
        void StarShowResult()
        {
            m_bShowResult = true;

        }
        //绘画时间
        void MyDrawUserTimer(Graphics pDC, int nXPos, int nYPos, int wTime, int wTimerArea)
        {
            //加载资源
            //CPngImage ImageTimeBack;
            //CPngImage ImageTimeNumber;
            //ImageTimeBack.LoadImage( GetModuleHandle(GAME_FRAME_DLL_NAME),TEXT("TIME_BACK"));
            //ImageTimeNumber.LoadImage(GetModuleHandle(GAME_FRAME_DLL_NAME),TEXT("TIME_NUMBER"));

            ////获取属性
            //int nNumberHeight=ImageTimeNumber.GetHeight();
            //int nNumberWidth=ImageTimeNumber.GetWidth()/10;

            ////计算数目
            //int lNumberCount=0;
            //int wNumberTemp=wTime;
            //do
            //{
            //    lNumberCount++;
            //    wNumberTemp/=10;
            //} while (wNumberTemp>0);

            ////位置定义
            //int nYDrawPos=nYPos-nNumberHeight/2+1;
            //int nXDrawPos=(int)(nXPos+(lNumberCount*nNumberWidth)/2-nNumberWidth);

            ////绘画背景
            //CSize SizeTimeBack;
            //SizeTimeBack.SetSize(ImageTimeBack.GetWidth(),ImageTimeBack.GetHeight());

            //ImageTimeBack.DrawImage(pDC,nXPos-SizeTimeBack.cx/2,nYPos-SizeTimeBack.cy/2);

            ////绘画号码
            //for (int i=0;i<lNumberCount;i++)
            //{
            //    //绘画号码
            //    int wCellNumber=wTime%10;
            //    ImageTimeNumber.DrawImage(pDC,nXDrawPos,nYDrawPos,nNumberWidth,nNumberHeight,wCellNumber*nNumberWidth,0);

            //    //设置变量
            //    wTime/=10;
            //    nXDrawPos-=nNumberWidth;
            //};

            //return;
        }

        //管理员控制
        //void OpenAdminWnd()
        //{
        //    //有权限
        //    CGameClientDlg *pGameClientDlg=CONTAINING_RECORD(this,CGameClientDlg,m_GameClientView);
        //    IClientKernel *pIClientKernel=(IClientKernel *)pGameClientDlg->m_pIClientKernel;
        //    //if(m_pClientControlDlg != null && ((pIClientKernel->GetUserAttribute()->dwUserRight)&UR_GAME_CONTROL)!=0)
        //    if(m_pClientControlDlg!=null && (CUserRight::IsGameCheatUser(pIClientKernel->GetUserAttribute()->dwUserRight)))
        //    {
        //        if(!m_pClientControlDlg->IsWindowVisible()) 
        //            m_pClientControlDlg->ShowWindow(true);
        //        else 
        //            m_pClientControlDlg->ShowWindow(false);
        //    }
        //}

        //执行剩余所有的缓冲动画
        void PerformAllBetAnimation()
        {
            KillTimer(IDI_ANDROID_BET);
            for (int i = 0; i < m_ArrayAndroid.Count(); ++i)
            {
                KillTimer(IDI_ANDROID_BET + i + 1);
                PlaceUserJetton(m_ArrayAndroid[i].cbJettonArea, m_ArrayAndroid[i].lJettonScore);
            }
            m_ArrayAndroid.Clear();
        }

        Image _DrawGameViewBuffer = null;

        void UpdateGameView(Object pRect)
        {
            if( _DrawGameViewBuffer == null )
                _DrawGameViewBuffer = new Bitmap(this.Width, this.Height); //I usually use 32BppARGB as my color depth

            Graphics gr = Graphics.FromImage(_DrawGameViewBuffer);

            //Do all your drawing with "gr"
            DrawGameView(gr, this.Width, this.Height);

            gr.Dispose();
            Graphics g = this.CreateGraphics();
            g.DrawImage(_DrawGameViewBuffer, 0, 0);
            //buffer.Dispose();


            //Invalidate();
            //if(pRect == null)
            //{
            //    CRect rect;
            //    GetClientRect(&rect);
            //    InvalidGameView(rect.left,rect.top,rect.Width(),rect.Height());
            //    return;
            //}
            //InvalidGameView(pRect->left,pRect->top,pRect->Width(),pRect->Height());
        }

        int OnPlaySound(int wParam, int lParam)
        {
            //if (IsEnableSound()) 
            {
                if (lParam == 0)
                {
                    //pakcj PlayGameSound(GetImage( "pleAfxGetInstanceHandle(),TEXT("PLEASEJETTONWAV"));
                }
                else if (lParam == 1)
                {
                    //pakcj PlayGameSound(AfxGetInstanceHandle(),TEXT("LEAVEHANDLEWAV"));
                }
                else if (lParam == 3)
                {
                    PlayDirectSound(m_SoundFolder + "idc_snd_.WAV", false);
                }
                else if (lParam == 4)
                {
                    PlayDirectSound(m_SoundFolder + "END_LOST.wav", false);
                }
                else if (lParam == 5)
                {
                    PlayDirectSound(m_SoundFolder + "END_WIN.wav", false);
                }
                else if (lParam == 6)
                {
                    PlayDirectSound(m_SoundFolder + "END_DRAW.wav", false);
                }
                else if (lParam == 7)
                {
                    PlayDirectSound(m_SoundFolder + "ADD_GOLD.wav", false);
                }
            }
            return 1;
        }

        ////更新控制
        //void UpdateButtonContron()
        //{
        //    /*if(__TEST__)
        //    {
        //        return ;
        //    }*/
        //    //置能判断
        //    bool bEnablePlaceJetton=true;

        //    if(m_wCurrentBanker==GameDefine.INVALID_CHAIR)
        //    {
        //        bEnablePlaceJetton = true;

        //    }
        //    if (GetGameStatus()!=GameStatus.GS_BETTING)
        //    {
        //        bEnablePlaceJetton=false;

        //    }
        //    if (m_wCurrentBanker==GetMeChairID()) 
        //    {
        //        bEnablePlaceJetton=false;
        //    }
        //    if (IsLookonMode())
        //    {
        //        bEnablePlaceJetton=false;
        //    }
        //    if (m_bEnableSysBanker==false&&m_wCurrentBanker==GameDefine.INVALID_CHAIR) 
        //    {
        //        bEnablePlaceJetton=false;
        //    }

        //    if(GetGameStatus()==GameStatus.GS_END)
        //    {
        //        m_btOpenCard.EnableWindow(false);
        //        m_btAutoOpenCard.EnableWindow(false);

        //    }else
        //    {
        //        m_btOpenCard.EnableWindow(true);
        //        m_btAutoOpenCard.EnableWindow(true);

        //    }

        //    SetEnablePlaceJetton(bEnablePlaceJetton);

        //    //下注按钮
        //    if (bEnablePlaceJetton==true)
        //    {

        //        //计算积分
        //        int lCurrentJetton= GetCurrentJetton();
        //        int lLeaveScore=m_lMeMaxScore;

        //        for (int nAreaIndex=1; nAreaIndex<=BumperCarDefine.AREA_COUNT; ++nAreaIndex) 
        //            lLeaveScore -= m_lUserJettonScore[nAreaIndex];

        //        //最大下注
        //        int lUserMaxJetton = 0;

        //        for (int nAreaIndex=1; nAreaIndex<=BumperCarDefine.AREA_COUNT; ++nAreaIndex)
        //        {
        //            if(lUserMaxJetton==0&&nAreaIndex == 1)
        //            {
        //                lUserMaxJetton = GetUserMaxJetton(nAreaIndex);

        //            }else
        //            {
        //                if(GetUserMaxJetton(nAreaIndex)>lUserMaxJetton)
        //                {
        //                    lUserMaxJetton = GetUserMaxJetton(nAreaIndex);
        //                }
        //            }
        //        }
        //        lLeaveScore = Math.Min((lLeaveScore),lUserMaxJetton); //用户可下分 和最大分比较 由于是五倍 

        //        //设置光标
        //        if (lCurrentJetton>lLeaveScore)
        //        {
        //            if (lLeaveScore>=5000000L) SetCurrentJetton(5000000);
        //            else if (lLeaveScore>=1000000L) SetCurrentJetton(1000000);
        //            else if (lLeaveScore>=100000L) SetCurrentJetton(100000);
        //            else if (lLeaveScore>=10000L) SetCurrentJetton(10000);
        //            else if (lLeaveScore>=1000L) SetCurrentJetton(1000);
        //            else if (lLeaveScore>=100L) SetCurrentJetton(100);
        //            else SetCurrentJetton(0);
        //        }

        //        //控制按钮
        //        int iTimer = 1;

        //        if(m_blUsing==false)
        //        {
        //            lLeaveScore = 0;
        //            lUserMaxJetton = 0;

        //        }
        //        m_btJetton100.EnableWindow((lLeaveScore>=100*iTimer && lUserMaxJetton>=100*iTimer)?true:false);
        //        m_btJetton1000.EnableWindow((lLeaveScore>=1000*iTimer && lUserMaxJetton>=1000*iTimer)?true:false);
        //        m_btJetton10000.EnableWindow((lLeaveScore>=10000*iTimer && lUserMaxJetton>=10000*iTimer)?true:false);
        //        m_btJetton100000.EnableWindow((lLeaveScore>=100000*iTimer && lUserMaxJetton>=100000*iTimer)?true:false);
        //        m_btJetton1000000.EnableWindow((lLeaveScore>=1000000*iTimer && lUserMaxJetton>=1000000*iTimer)?true:false);		
        //        m_btJetton5000000.EnableWindow((lLeaveScore>=5000000*iTimer && lUserMaxJetton>=5000000*iTimer)?true:false);
        //    }
        //    else
        //    {
        //        //设置光标
        //        SetCurrentJetton(0);

        //        //禁止按钮
        //        m_btJetton100.EnableWindow(false);		
        //        m_btJetton1000.EnableWindow(false);
        //        m_btJetton50000.EnableWindow(false);
        //        m_btJetton10000.EnableWindow(false);

        //        m_btJetton100000.EnableWindow(false);
        //        m_btJetton500000.EnableWindow(false);
        //        m_btJetton1000000.EnableWindow(false);
        //        m_btJetton5000000.EnableWindow(false);
        //    }




        //    //庄家按钮
        //    if (!IsLookonMode())
        //    {
        //        //获取信息
        //         //IClientUserItem *pMeUserData=GetTableUserItem(GetMeChairID());
        //        UserInfo MeUserData = m_UserInfo;

        //        //申请按钮
        //        bool bEnableApply=true;
        //        if (m_wCurrentBanker==GetMeChairID()) 
        //            bEnableApply=false;

        //        //if (m_bMeApplyBanker) 
        //        //    bEnableApply=false;

        //        //if (pMeUserData->GetUserScore()<m_lApplyBankerCondition) 
        //        //    bEnableApply=false;

        //        //m_btApplyBanker.EnableWindow(bEnableApply?true:false);

        //        //取消按钮
        //        bool bEnableCancel=true;
        //        //if (m_wCurrentBanker==GetMeChairID() && GetGameStatus()!=GAME_STATUS_FREE) bEnableCancel=false;
        //        //if (m_bMeApplyBanker==false) bEnableCancel=false;
        //        //m_btCancelBanker.EnableWindow(bEnableCancel?true:false);
        //        //m_btCancelBanker.SetButtonImage(m_wCurrentBanker==GetMeChairID()?IDB_BT_CANCEL_BANKER:IDB_BT_CANCEL_APPLY,AfxGetInstanceHandle(),false,false);

        //        ////显示判断
        //        //if (m_bMeApplyBanker)
        //        //{
        //        //    m_btApplyBanker.ShowWindow(SW_HIDE);
        //        //    m_btCancelBanker.ShowWindow(SW_SHOW);

        //        //}
        //        //else
        //        //{
        //        //    m_btCancelBanker.ShowWindow(SW_HIDE);
        //        //    m_btApplyBanker.ShowWindow(SW_SHOW);
        //        //}

        //    }
        //    else
        //    {
        //        //m_btCancelBanker.EnableWindow(false);
        //        //m_btApplyBanker.EnableWindow(false);
        //        //m_btApplyBanker.ShowWindow(SW_SHOW);
        //        //m_btCancelBanker.ShowWindow(SW_HIDE);

        //    }
        //    //获取信息
        //     //IClientUserItem *pMeUserData=GetTableUserItem(GetMeChairID());
        //    //pakcj ReSetBankCtrol(GetGameStatus());
        //    return;
        //}

        int m_wMeChairID = GameDefine.INVALID_CHAIR;						//我的位置

        //加注消息
        string[] m_DTSDCheer = new string[3];

        bool OnPlaceJetton(int cbJettonArea, int lJettonScore)
        {
            //合法判断
            //ASSERT(cbJettonArea>=1 && cbJettonArea<=BumperCarDefine.AREA_COUNT);
            if (!(cbJettonArea >= 1 && cbJettonArea <= BumperCarDefine.AREA_COUNT))
                return false;

            //庄家判断
            //if ( GetMeChairID() == m_wCurrentBanker ) 
            //    return true;

            //状态判断
            if (GetGameStatus() != GameStatus.GS_BETTING)
            {
                //UpdateButtonContron();
                return true;
            }

            // deleted by usc at 2014/03/05
            //int[] aAreaMultiple = { 0, 39, 29, 19, 9, 4, 4, 4, 4 };

            //int nFirstLimit = 0;

            //if (aAreaMultiple[cbJettonArea] < 5)
            //    nFirstLimit = 4000;
            //else if (aAreaMultiple[cbJettonArea] < 20)
            //    nFirstLimit = 8000;
            //else
            //    nFirstLimit = 12000;

            //if ((m_lAllJettonScore[cbJettonArea] + lJettonScore) * aAreaMultiple[cbJettonArea] > nFirstLimit / 2)
            //{
            //    int nMaxScore = m_lAllJettonScore[1] * aAreaMultiple[1];
            //    int nOtherScoreSum = 0;

            //    for (int i = 1; i <= BumperCarDefine.AREA_COUNT; i++)
            //    {
            //        if (m_lAllJettonScore[i] * aAreaMultiple[i] > nMaxScore)
            //            nMaxScore = m_lAllJettonScore[i] * aAreaMultiple[i];

            //        if (i == cbJettonArea)
            //            continue;

            //        nOtherScoreSum += m_lAllJettonScore[i] * aAreaMultiple[i];
            //    }

            //    if (m_lAllJettonScore[cbJettonArea] * aAreaMultiple[cbJettonArea] == nMaxScore)     // 현재 베팅값이 가장 높은 판에는 베팅할수 없다.
            //    {
            //        PlayDirectSound(m_SoundFolder + "Alert.wav", false);
            //        return false;
            //    }
            //    else if (nOtherScoreSum > 0)
            //    {
            //        int nNewScore = (m_lAllJettonScore[cbJettonArea] + lJettonScore) * aAreaMultiple[cbJettonArea];

            //        if ((nNewScore - nMaxScore >= nFirstLimit * aAreaMultiple[cbJettonArea]) ||     // 현재의 최대베팅금액보다 500 정도 큰 금액을 베팅할수 없다.
            //            (nNewScore > nMaxScore * 1.5) ||                                            // 현재의 최대베팅점수의 1.5배이상을 베팅할수 없다.
            //            (nNewScore > nOtherScoreSum))                                               // 현재판의 베팅점수합이 다른 모든 판의 베팅점수의 합보다 클수 없다.
            //        {
            //            PlayDirectSound(m_SoundFolder + "Alert.wav", false);
            //            return false;
            //        }
            //    }                
            //}

            //变量定义
            BettingInfo bettingInfo = new BettingInfo();
            //CMD_C_PlaceJetton PlaceJetton;
            //ZeroMemory(&PlaceJetton,sizeof(PlaceJetton));

            //构造变量
            bettingInfo._Area = cbJettonArea;
            bettingInfo._Score = lJettonScore;
            bettingInfo._UserIndex = m_wMeChairID;
            //PlaceJetton.cbJettonArea=cbJettonArea;
            //PlaceJetton.lJettonScore=lJettonScore;

            //发送消息
            //SendSocketData(SUB_C_PLACE_JETTON,&PlaceJetton,sizeof(PlaceJetton));
            m_ClientSocket.Send(NotifyType.Request_Betting, bettingInfo);

            //预先显示
            PlaceUserJetton(cbJettonArea, lJettonScore);

            //设置变量
            m_lUserJettonScore[cbJettonArea] += lJettonScore;
            SetMePlaceJetton(cbJettonArea, m_lUserJettonScore[cbJettonArea]);

            //更新按钮
            UpdateButtonContron();

            //播放声音
            //if (IsEnableSound()) 
            {
                if (lJettonScore >= 500000)
                    PlayDirectSound(m_SoundFolder + "ADD_GOLD_EX.wav", false);
                else
                    PlayDirectSound(m_SoundFolder + "ADD_GOLD.wav", false);

                if (_Random.Next() % 100 > 80)
                {
                    PlayDirectSound(m_SoundFolder + m_DTSDCheer[_Random.Next() % 3], false);
                }
            }

            return false;
        }

        bool IsLookonMode()
        {
            if (m_wMeChairID == GameDefine.INVALID_CHAIR)
                return true;

            return false;
        }

        //继续发牌
        int OnContinueCard(int wParam, int lParam)
        {
            //合法判断
            //if (GetMeChairID()!=m_wCurrentBanker) return 0;
            //if (GetGameStatus()!=GameStatus.GS_JOIN) return 0;
            //if (m_cbLeftCardCount < 8) return 0;
            //if (IsLookonMode()) return 0;

            ////发送消息
            ////SendSocketData(SUB_C_CONTINUE_CARD);
            //CMD_C_CheckImage CheckImage;
            //CheckImage.Index =   m_CheckImagIndex;
            //SendSocketData(SUB_C_CHECK_IMAGE,&CheckImage,sizeof(CheckImage));
            //设置按钮

            return 0;
        }

        private void BumperCarVew_MouseMove(object sender, MouseEventArgs e)
        {
            OnSetCursor(e.Location);
        }

        GameStatus GetGameStatus()
        {
            return this.m_Status;
        }

        private void BumperCarVew_Resize(object sender, EventArgs e)
        {
            //return;
            this.RectifyControl(this.Width, this.Height);
        }



        private void BumperCarVew_Paint(object sender, PaintEventArgs e)
        {
            DrawGameView(e.Graphics, this.Width, this.Height);
        }

        public override void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_TableDetail:
                    {
                        if (!(baseInfo is BumperCarInfo))
                            return;

                        BumperCarInfo tableInfo = (BumperCarInfo)baseInfo;

                        m_wMeChairID = GameDefine.INVALID_CHAIR;

                        for (int i = 0; i < tableInfo._Players.Count; i++)
                        {
                            UserInfo userInfo = tableInfo._Players[i];

                            if (userInfo != null && userInfo.Id == m_UserInfo.Id)
                            {
                                m_wMeChairID = i;
                                break;
                            }
                        }

                        // added by usc at 2014/03/22
                        if (m_ReceiveInfo == null)
                        {
                            Array.Clear(m_lAllJettonScore, 0, m_lUserJettonScore.Length);

                            m_lAllJettonScore = tableInfo.m_lAllJettonScore;

                            for (int i = 1; i <= BumperCarDefine.AREA_COUNT; i++)
                            {
                                PlaceUserJetton_First(i, m_lAllJettonScore[i]);

                            }
                        }

                        m_ReceiveInfo = tableInfo;

                        GameStatus gameStatus = GameStatus.GS_NONE;

                        switch (m_ReceiveInfo._RoundIndex)
                        {
                            case 0:
                                gameStatus = GameStatus.GS_JOIN;
                                break;
                            case 1:
                                gameStatus = GameStatus.GS_BETTING;
                                break;
                            case 2:
                                gameStatus = GameStatus.GS_END;
                                break;
                        }

                        // added by usc at 2014/03/19
                        //if (m_wMeChairID == GameDefine.INVALID_CHAIR)
                        //    return;

                        if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                        {
                            if (gameStatus != GameStatus.GS_END)
                                m_UserInfo = tableInfo._Players[m_wMeChairID];
                        }

                        SetGameStatus(gameStatus);
                    }
                    break;

                case NotifyType.Reply_Betting:
                    {
                        // added by usc at 2014/03/21
                        if (!(baseInfo is BettingInfo))
                            return;

                        BettingInfo bettingInfo = (BettingInfo)baseInfo;

                        m_lAllJettonScore[bettingInfo._Area] += bettingInfo._Score;

                        if (m_wMeChairID != bettingInfo._UserIndex || IsLookonMode())
                        {
                            // added by usc at 2014/03/26
                            PlaceUserJetton(bettingInfo._Area, bettingInfo._Score);

                            if (bettingInfo._Score == 5000000)
                                PlayDirectSound(m_SoundFolder + "ADD_GOLD_EX.wav", false);
                            else
                                PlayDirectSound(m_SoundFolder + "ADD_GOLD.wav", false);

                            if (bettingInfo._Score >= 1000)
                            {
                                if (_Random.Next() % 100 > 80)
                                {
                                    PlayDirectSound(m_SoundFolder + m_DTSDCheer[_Random.Next() % 3], false);
                                }
                            }
                        }
                        
                        UpdateGameView(null);

                        // deleted by usc at 2014/03/14
                        /*
                        BettingInfo bettingInfo = (BettingInfo)baseInfo;

	                    //消息处理
                        //if (pPlaceJetton->cbAndroid==FALSE && (m_GameClientView.m_pClientControlDlg->GetSafeHwnd())&&bGameMes)
                        //{
                        //    m_GameClientView.m_pClientControlDlg->SetAllUserBetScore(pPlaceJetton->cbJettonArea - 1,pPlaceJetton->lJettonScore);
                        //}

	                    if (GetMeChairID()!=bettingInfo._UserIndex || IsLookonMode())
	                    {
		                    //加注界面
                            //if(pPlaceJetton->cbAndroid == TRUE)
                            //{
                            //    //保存
                            //    static WORD wStFluc = 1;	//随机辅助
                            //    tagAndroidBet androidBet = {};
                            //    androidBet.cbJettonArea = pPlaceJetton->cbJettonArea;
                            //    androidBet.lJettonScore = pPlaceJetton->lJettonScore;
                            //    androidBet.wChairID = pPlaceJetton->wChairID;
                            //    androidBet.nLeftTime = ((rand()+androidBet.wChairID+wStFluc*3)%10+1)*100;
                            //    wStFluc = wStFluc%3 + 1;

                            //    m_ListAndroid.AddTail(androidBet);
                            //}
		                    //else
		                    //{
                            if (IsLookonMode())
                            {
                                PlaceUserJetton(bettingInfo._Area, bettingInfo._Score);

                                if (bettingInfo._Score == 5000000)
                                    PlayDirectSound(m_SoundFolder + "ADD_GOLD_EX.wav", false);
                                else
                                    PlayDirectSound(m_SoundFolder + "ADD_GOLD.wav", false);

                                if (_Random.Next() % 100 > 80)
                                {
                                    PlayDirectSound(m_SoundFolder + m_DTSDCheer[_Random.Next() % 3], false);
                                }
                            }

			                    //播放声音
		                    //	if (IsEnableSound()) 

                            // modified by usc at 2014/02/19
                            //{
                            //    if (bettingInfo._UserIndex!=GetMeChairID() || IsLookonMode())
                            //    {
                            //        if (bettingInfo._Score==5000000) 
                            //            PlayDirectSound(m_ResourceFolder + "ADD_GOLD_EX.wav", false);
                            //        else 
                            //            PlayDirectSound(m_ResourceFolder + "ADD_GOLD.wav", false);

                            //        if(_Random.Next()%100 > 80 )
                            //        {
                            //            PlayDirectSound( m_ResourceFolder + m_DTSDCheer[_Random.Next()%3], false );
                            //        }	
                            //    }
                            //}
		                    //}
	                    }
	                    UpdateButtonContron();
                        */
                    }
                    break;
            }
        }

        private void SetGameStatus(GameStatus gameStatus)
        {
            if (m_Status != gameStatus)
            {
                GameStatus oldStatus = m_Status;
                m_Status = gameStatus;

                switch (m_Status)
                {
                    case GameStatus.GS_JOIN:
                        {
                            ResetGameView();

                            m_blUsing = true;

                            //设置时间
                            SetGameTimer(IDI_FREE, m_ReceiveInfo.m_cbFreeTime);

                            //m_GameClientView.StartMove();

                            //清理时间
                            //KillTimer(IDI_ANDROID_BET);

                            ////清理桌面
                            bool[] blWin = new bool[BumperCarDefine.AREA_COUNT];

                            for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
                            {
                                blWin[i] = false;
                            }

                            FinishDispatchCard(true);

                            SetWinnerSide(blWin, false);


                            for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex)
                                SetMePlaceJetton(nAreaIndex, 0);

                            CleanUserJetton();

                            //更新成绩
                            //for (int wUserIndex = 0; wUserIndex < GameDefine.MAX_CHAIR; ++wUserIndex)
                            //{
                            //    IClientUserItem  *pUserData = GetTableUserItem(wUserIndex);

                            //    if ( pUserData == NULL ) continue;
                            //    tagApplyUser ApplyUser ;

                            //    //更新信息
                            //    ApplyUser.lUserScore = pUserData->GetUserScore();
                            //    ApplyUser.strUserName = pUserData->GetNickName();
                            //    m_ApplyUser.UpdateUser(ApplyUser);                        	

                            //}

                            SwitchToCheck();

                            //更新控件
                            UpdateButtonContron();
                        }
                        break;

                    case GameStatus.GS_BETTING:
                        {
                            //消息处理
                            SwithToNormalView();
                            KillCardTime();

                            m_blUsing = true;

                            //庄家信息
                            //SetBankerInfo( pGameStart->wBankerUser, pGameStart->lBankerScore);

                            //玩家信息
                            m_lMeMaxScore = m_UserInfo.GetGameMoney();// pGameStart->lUserMaxScore;
                            SetMeMaxScore(m_lMeMaxScore);

                            //设置时间
                            //SetGameClock(GetMeChairID(), IDI_PLACE_JETTON, pGameStart->cbTimeLeave);
                            //SetGameTimer(IDI_PLACE_JETTON, m_ReceiveInfo.m_cbBetTime);

                            // modified by usc at 2014/03/23
                            int nElapsedTime = (int)(Math.Round((DateTime.Now - m_ReceiveInfo._dtReceiveTime).TotalMilliseconds / 1000d));

                            // added by usc at 2014/03/12
                            m_nBettingTime = m_ReceiveInfo.m_cbBetTime - m_ReceiveInfo._RoundDelayTime - nElapsedTime;
                            m_BetStartTime = DateTime.Now;

                            SetUserClock(m_wMeChairID, IDI_PLACE_JETTON, m_nBettingTime);

                            StartRandShowSide();

                            //设置状态
                            //SetTimer(IDI_ANDROID_BET, 100, NULL);

                            m_bShowBao = true;

                            //更新控制
                            UpdateButtonContron();

                            //设置提示
                            SetDispatchCardTip(m_ReceiveInfo.m_bContiueCard ? enDispatchCardTip.enDispatchCardTip_Continue : enDispatchCardTip.enDispatchCardTip_Dispatch);

                            //播放声音
                            //if (IsEnableSound()) 
                            {
                                PlayDirectSound(m_SoundFolder + "GAME_START.wav", false);
                            }


                            //if (m_pClientControlDlg->GetSafeHwnd())
                            //{
                            //    m_GameClientView.m_pClientControlDlg->ResetUserBet();
                            //    //m_GameClientView.m_pClientControlDlg->ResetUserNickName();
                            //}
                        }
                        break;

                    case GameStatus.GS_END:
                        {
                            this.Cursor = Cursors.Default;

                            //消息处理
                            m_blMoveFinish = false;

                            m_GameEndTime = m_ReceiveInfo.m_cbEndTime;

                            m_blUsing = true;

                            //设置时间
                            SetGameTimer(IDI_DISPATCHCARD, m_GameEndTime);

                            //扑克信息
                            SetCardInfo(m_ReceiveInfo.m_cbTableCardArray[0, 0]);
                            ClearAreaFlash();

                            // added by usc at 2014/04/06
                            int nRoundDelayTime = m_ReceiveInfo._RoundDelayTime + (int)(Math.Round((DateTime.Now - m_ReceiveInfo._dtReceiveTime).TotalMilliseconds / 1000d));

                            //設置撲克移動
                            StartRunCar(20, nRoundDelayTime);

                            //设置状态
                            //m_cbLeftCardCount = m_ReceiveInfo.m_cbLeftCardCount;

                            //庄家信息
                            //SetBankerScore(pGameEnd->nBankerTime, pGameEnd->lBankerTotallScore);

                            //成绩信息
                            //SetCurGameScore(pGameEnd->lUserScore, pGameEnd->lUserReturnScore, pGameEnd->lBankerScore, pGameEnd->lRevenue);

                            if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                                SetCurGameScore(m_ReceiveInfo.m_lUserWinScore[m_wMeChairID], 0, 0, 0);

                            PerformAllBetAnimation();

                            //m_TempData.a = pGameEnd->lUserScore;
                            //m_TempData.b = pGameEnd->lUserReturnScore;
                            //m_TempData.c = pGameEnd->lBankerScore;
                            //m_TempData.d = pGameEnd->lRevenue;

                            //更新控件
                            UpdateButtonContron();

                            //停止声音
                            //for (int i = 0; i < CountArray(m_DTSDCheer); ++i) 
                            //    m_DTSDCheer[i].Stop();
                        }
                        break;
                }

                if (oldStatus == GameStatus.GS_EMPTY)
                {
                    PlayDirectSound(m_SoundFolder + "BACK_GROUND.wav", true);

                    if (m_Status != GameStatus.GS_JOIN)
                    {
                        //下注信息
                        for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex)
                        {
                            // deleted by usc at 2014/03/19
                            /*
                            PlaceUserJetton(nAreaIndex, m_ReceiveInfo.m_lAllJettonScore[nAreaIndex]);
                            */

                            //SetMePlaceJetton(nAreaIndex, m_ReceiveInfo.m_lUserJettonScore[nAreaIndex, GetMeChairID()]);
                        }

                        //玩家积分
                        m_lMeMaxScore = m_UserInfo.GetGameMoney();//pStatusPlay->lUserMaxScore;			
                        SetMeMaxScore(m_lMeMaxScore);

                        //控制信息
                        //m_lApplyBankerCondition=pStatusPlay->lApplyBankerCondition;
                        //m_lAreaLimitScore=pStatusPlay->lAreaLimitScore;
                        //m_GameClientView.SetAreaLimitScore(m_lAreaLimitScore);

                        if (m_Status == GameStatus.GS_END)
                        {
                            //扑克信息
                            SetCardInfo(m_ReceiveInfo.m_cbTableCardArray[0, 0]);
                            FinishDispatchCard(false);

                            //设置成绩
                            //SetCurGameScore(pStatusPlay->lEndUserScore, pStatusPlay->lEndUserReturnScore, pStatusPlay->lEndBankerScore, pStatusPlay->lEndRevenue);
                        }
                        else
                        {
                            SetCardInfo(0);
                            m_blUsing = true;
                        }

                        //播放声音
                        PlayDirectSound(m_SoundFolder + "BACK_GROUND.wav", true);


                        //庄家信息
                        //SetBankerInfo(pStatusPlay->wBankerUser,pStatusPlay->lBankerScore);
                        //m_GameClientView.SetBankerScore(pStatusPlay->cbBankerTime,pStatusPlay->lBankerWinScore);
                        //m_bEnableSysBanker=pStatusPlay->bEnableSysBanker;
                        //m_GameClientView.EnableSysBanker(m_bEnableSysBanker);

                        //开启
                        //if(CUserRight::IsGameCheatUser(m_pIClientKernel->GetUserAttribute()->dwUserRight) && m_GameClientView.m_pClientControlDlg)
                        //    m_GameClientView.m_btOpenAdmin.ShowWindow(SW_SHOW);

                        //设置状态
                        //SetGameStatus(pStatusPlay->cbGameStatus);

                        //设置时间
                        //int nTimerID = m_Status == GameStatus.GS_END ? IDI_OPEN_CARD : IDI_PLACE_JETTON;
                        //SetGameTimer(GetMeChairID(), nTimerID, m_ReceiveInfo.c.cbTimeLeave);

                        StartRandShowSide();

                        //更新按钮
                        UpdateButtonContron();
                    }
                }
            }


            UpdateGameView(null);
        }

        ////设置筹码
        //void SetCurrentJetton(int lCurrentJetton)
        //{
        //    //设置变量
        //    //ASSERT(lCurrentJetton>=0L);
        //    m_lCurrentJetton=lCurrentJetton;
        //    if(lCurrentJetton==0)
        //    {
        //        SetJettonHide(0);
        //    }
        //    return;
        //}

        //更新控制
        void UpdateButtonContron()
        {
            /*if(__TEST__)
            {
                return ;
            }*/
            //置能判断
            bool bEnablePlaceJetton = true;

            //if(m_wCurrentBanker==GameDefine.INVALID_CHAIR)
            //{
            //    bEnablePlaceJetton = true;

            //}
            if (GetGameStatus() == GameStatus.GS_BETTING)
            {
                int nElapse = m_nBettingTime - (int)(DateTime.Now - m_BetStartTime).TotalSeconds;

                if (nElapse > 0 && nElapse < 5)
                    bEnablePlaceJetton = false;
            }
            else
            {
                bEnablePlaceJetton = false;

            }
            //if (m_wCurrentBanker == m_wMeChairID) 
            //   {
            //       bEnablePlaceJetton=false;
            //   }
            if (IsLookonMode())
            {
                bEnablePlaceJetton = false;
            }
            //if (m_bEnableSysBanker==false&&m_wCurrentBanker==GameDefine.INVALID_CHAIR) 
            //{
            //    bEnablePlaceJetton=false;
            //}

            if (GetGameStatus() == GameStatus.GS_END)
            {
                m_btOpenCard.EnableWindow(false);
                m_btAutoOpenCard.EnableWindow(false);

            }
            else
            {
                m_btOpenCard.EnableWindow(true);
                m_btAutoOpenCard.EnableWindow(true);

            }

            SetEnablePlaceJetton(bEnablePlaceJetton);

            //下注按钮
            if (bEnablePlaceJetton == true)
            {

                //计算积分
                int lCurrentJetton = GetCurrentJetton();
                int lLeaveScore = m_lMeMaxScore;

                for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex)
                    lLeaveScore -= m_lUserJettonScore[nAreaIndex];

                //最大下注
                int lUserMaxJetton = 0;

                for (int nAreaIndex = 1; nAreaIndex <= BumperCarDefine.AREA_COUNT; ++nAreaIndex)
                {
                    if (lUserMaxJetton == 0 && nAreaIndex == 1)
                    {
                        lUserMaxJetton = GetUserMaxJetton(nAreaIndex);

                    }
                    else
                    {
                        if (GetUserMaxJetton(nAreaIndex) > lUserMaxJetton)
                        {
                            lUserMaxJetton = GetUserMaxJetton(nAreaIndex);
                        }
                    }
                }

                lLeaveScore = Math.Min((lLeaveScore), lUserMaxJetton); //用户可下分 和最大分比较 由于是五倍 

                //设置光标
                if (lCurrentJetton > lLeaveScore)
                {
                    if (lLeaveScore >= 5000000L) SetCurrentJetton(5000000);
                    else if (lLeaveScore >= 1000000L) SetCurrentJetton(1000000);
                    else if (lLeaveScore >= 100000L) SetCurrentJetton(100000);
                    else if (lLeaveScore >= 10000L) SetCurrentJetton(10000);
                    else if (lLeaveScore >= 1000L) SetCurrentJetton(1000);
                    else if (lLeaveScore >= 100L) SetCurrentJetton(100);
                    else SetCurrentJetton(0);
                }

                //控制按钮
                //int iTimer = 1;
                double iTimer = (100 + m_GameInfo.Commission) / 100d;

                if (m_blUsing == false)
                {
                    lLeaveScore = 0;
                    lUserMaxJetton = 0;

                }
                m_btJetton100.EnableWindow((lLeaveScore >= 100 * iTimer && lUserMaxJetton >= 100 * iTimer) ? true : false);
                m_btJetton1000.EnableWindow((lLeaveScore >= 1000 * iTimer && lUserMaxJetton >= 1000 * iTimer) ? true : false);
                m_btJetton10000.EnableWindow((lLeaveScore >= 10000 * iTimer && lUserMaxJetton >= 10000 * iTimer) ? true : false);
                m_btJetton100000.EnableWindow((lLeaveScore >= 100000 * iTimer && lUserMaxJetton >= 100000 * iTimer) ? true : false);
                m_btJetton1000000.EnableWindow((lLeaveScore >= 1000000 * iTimer && lUserMaxJetton >= 1000000 * iTimer) ? true : false);
                m_btJetton5000000.EnableWindow((lLeaveScore >= 5000000 * iTimer && lUserMaxJetton >= 5000000 * iTimer) ? true : false);
            }
            else
            {
                //设置光标
                SetCurrentJetton(0);

                //禁止按钮
                m_btJetton100.EnableWindow(false);
                m_btJetton1000.EnableWindow(false);                
                m_btJetton10000.EnableWindow(false);
                m_btJetton50000.EnableWindow(false);
                m_btJetton100000.EnableWindow(false);
                m_btJetton500000.EnableWindow(false);
                m_btJetton1000000.EnableWindow(false);
                m_btJetton5000000.EnableWindow(false);
            }




            //庄家按钮
            //if (!IsLookonMode())
            //{
            //    //申请按钮
            //    bool bEnableApply=true;
            //    if (m_wCurrentBanker==GetMeChairID()) bEnableApply=false;
            //    if (m_bMeApplyBanker) bEnableApply=false;
            //    if (pMeUserData->GetUserScore()<m_lApplyBankerCondition) bEnableApply=false;
            //    m_GameClientView.m_btApplyBanker.EnableWindow(bEnableApply?TRUE:FALSE);

            //    //取消按钮
            //    bool bEnableCancel=true;
            //    if (m_wCurrentBanker==GetMeChairID() && GetGameStatus()!=GAME_STATUS_FREE) bEnableCancel=false;
            //    if (m_bMeApplyBanker==false) bEnableCancel=false;
            //    m_GameClientView.m_btCancelBanker.EnableWindow(bEnableCancel?TRUE:FALSE);
            //    m_GameClientView.m_btCancelBanker.SetButtonImage(m_wCurrentBanker==GetMeChairID()?IDB_BT_CANCEL_BANKER:IDB_BT_CANCEL_APPLY,AfxGetInstanceHandle(),false,false);

            //    //显示判断
            //    if (m_bMeApplyBanker)
            //    {
            //        m_GameClientView.m_btApplyBanker.ShowWindow(SW_HIDE);
            //        m_GameClientView.m_btCancelBanker.ShowWindow(SW_SHOW);

            //    }
            //    else
            //    {
            //        m_GameClientView.m_btCancelBanker.ShowWindow(SW_HIDE);
            //        m_GameClientView.m_btApplyBanker.ShowWindow(SW_SHOW);
            //    }

            //}
            //else
            //{
            //    m_GameClientView.m_btCancelBanker.EnableWindow(FALSE);
            //    m_GameClientView.m_btApplyBanker.EnableWindow(FALSE);
            //    m_GameClientView.m_btApplyBanker.ShowWindow(SW_SHOW);
            //    m_GameClientView.m_btCancelBanker.ShowWindow(SW_HIDE);

            //}
            //获取信息
            //IClientUserItem *pMeUserData=GetTableUserItem(GetMeChairID());
            //ReSetBankCtrol(GetGameStatus());
            return;
        }

        const int IDI_FREE = 99;									//空闲时间
        const int IDI_PLACE_JETTON = 100;									//下注时间
        const int IDI_DISPATCHCARD = 301;									//发牌时间
        const int IDI_OPEN_CARD = 302;								    //发牌时间
        int m_GameEndTime = 0;

        //时间消息
        protected override void OnGameTimer(TimerInfo timerInfo)
        {
            int nTimerID = timerInfo._Id;

            // modified by usc at 2014/03/15
            //int nElapse = timerInfo.Elapse;
            int nElapse = m_nBettingTime - (int)(DateTime.Now - m_BetStartTime).TotalSeconds;

            if ((nTimerID == IDI_PLACE_JETTON) && (nElapse <= 5))
            {
                //设置光标
                SetCurrentJetton(0);

                //禁止按钮
                m_btJetton100.EnableWindow(false);
                m_btJetton1000.EnableWindow(false);
                m_btJetton10000.EnableWindow(false);
                m_btJetton100000.EnableWindow(false);
                m_btJetton500000.EnableWindow(false);
                m_btJetton1000000.EnableWindow(false);
                m_btJetton5000000.EnableWindow(false);

                //庄家按钮
                //m_btApplyBanker.EnableWindow( false );
                //m_btCancelBanker.EnableWindow( false );
            }

            if (nTimerID == IDI_DISPATCHCARD)
            {
                StartHandle_Leave();
                KillTimer(IDI_DISPATCH_CARD);

                if (m_GameEndTime <= 0 || m_GameEndTime > 100)
                    m_GameEndTime = 2;

                SetGameTimer(IDI_OPEN_CARD, m_GameEndTime - 1);
                return;
            }

            //	if (IsEnableSound()) 
            {
                if (nTimerID == IDI_PLACE_JETTON)
                {
                    // added by usc at 2014/01/08
                    UpdateGameView(null);

                    if (nElapse > 0 && nElapse < 5)
                    {
                        PlayDirectSound(m_SoundFolder + "TIME_WARIMG.wav", false);
                    }
                }
            }

            //	if (IsEnableSound()) 
            //{
            //    if (nTimerID==IDI_PLACE_JETTON)
            //    {                    
            //        /*if(nElapse%3==0)
            //            m_DTSDCheer[3].Play();	*/
            //    }
            //}

        }

        //鼠标消息
        private void BumperCarVew_MouseDown(object sender, MouseEventArgs e)
        {
            CPoint point = new CPoint();
            point.SetPoint(e.Location.X, e.Location.Y);

            if (m_lCurrentJetton != 0)
            {
                int iTimer = 1;
                //下注区域
                int cbJettonArea = GetJettonArea(point);

                // modified by usc at 2014/01/09
                if (cbJettonArea != 0xFF)
                {
                    //最大下注
                    int lMaxJettonScore = GetUserMaxJetton(cbJettonArea);

                    if ((m_lAllJettonScore[cbJettonArea] + m_lCurrentJetton) > m_lAreaLimitScore)
                    {
                        return;
                    }

                    //合法判断
                    if (lMaxJettonScore < m_lCurrentJetton)
                    {
                        SetJettonHide(0);
                        return;
                    }

                    //发送消息

                    OnPlaceJetton(cbJettonArea, m_lCurrentJetton);
                }
            }

            UpdateGameView(null);

            ////__super::OnLButtonDown(nFlags,Point);
        }

    }

    //筹码信息
    struct tagJettonInfo
    {
        public int nXPos;								//筹码位置
        public int nYPos;								//筹码位置
        public int cbJettonIndex;						//筹码索引
    };

    //发牌提示
    public enum enDispatchCardTip
    {
        enDispatchCardTip_NULL,
        enDispatchCardTip_Continue,											//继续发牌
        enDispatchCardTip_Dispatch											//重新洗牌
    };

    public class sT_ShowJetton
    {
        public bool[] blShow = new bool[6];

        public sT_ShowJetton()
        {
            for (int i = 0; i < 6; i++)
            {
                blShow[i] = true;
            }
        }
    }

    public class tagAndroidBet
    {
        public int cbJettonArea;						//筹码区域
        public int lJettonScore;						//加注数目
        public int wChairID;							//玩家位置
        public int nLeftTime;							//剩余时间 (100ms为单位)
    };


}
