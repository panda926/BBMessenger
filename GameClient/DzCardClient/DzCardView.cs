using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChatEngine;
using GameControls;
using System.Net.Sockets;

namespace DzCardClient
{
    public partial class DzCardView : GameView
    {
        //按钮标识
        const int IDC_START	=					100;								//开始按钮
        const int IDC_EXIT =						101;								//离开按钮
        const int IDC_ADD =							102;								//加注按钮
        const int IDC_GIVEUP =						103;								//放弃按钮
        const int IDC_SHOWHAND =					104;								//梭哈按钮
        const int IDC_FOLLOW =						105;								//跟注按钮
        const int IDC_PASS =						106;								//让牌按钮
        const int IDC_AUTO_START =					107;								//自动按钮
        const int IDC_SIT_DOWN =					108;								//坐下按钮
        //IDC_SIT_DOWN 	108+GAME_PLAYER之内不能用
        const int IDC_OPEN_CARD	=				117;								//开牌按钮

        //时间定时器
        const int IDI_GAME_OVER	=				30;								//动画定时器
        const int IDI_NO_SCORE =				31;								//结束定时器

        //定时器标识
        //#define IDI_SEND_CARD					100								//发牌定时器
        const int IDI_GAME_END =					101;								//结束定时器
        const int IDI_USER_ACTION =					102;								//动画定时器
        //const int IDI_GAME_START = 200;
        const int IDI_USER_ADD_SCORE =			    201;									//加注定时器

        //定时器时间
        const int TIME_USER_ACTION =				60;								//动画定时器

        //动画参数
        const int TO_USERCARD =						1;								//用户扑克
        const int TO_GIVEUP_CARD =					2;								//回收扑克
        const int TO_CENTER_CARD =					3;								//中心扑克


        //状态变量
        bool m_bReset;                              //复位标志
        int  m_wMeChairID;                        //椅子位置
        int  m_wDUser;                            //D标示
        int  m_wMyIsLookOn;                       //旁观用户
        int[] m_wUserGender = new int[DzCardDefine.GAME_PLAYER]; //用户姓别
        //int[] m_wUserPost = new int[DzCardDefine.GAME_PLAYER]; //用户位置

	    //数据变量
	    bool							m_bShowUser;						//显示标志
	    int[]							m_lTableScore = new int[DzCardDefine.GAME_PLAYER];			//桌面下注
	    int[]							m_lTotalScore = new int[DzCardDefine.GAME_PLAYER];			//总下注
        int 							m_lCenterScore;						//中心筹码
	    int	    						m_lTurnLessScore;					//最小下注
	    int		    					m_lTurnMaxScore;					//最大下注


        //界面变量
        Bitmap m_ImageViewCenter;					//背景位图
        Bitmap m_ImageHuman;						//人物图像
        Bitmap m_ImageSmallCard;					//小牌位图
        Bitmap m_ImageCard;						//扑克位图
        Bitmap m_ImageD;							//D位图
        Bitmap m_ImageCardMask;					//扑克掩图
        Bitmap m_ImageTitleInfo;					//下注信息
        Bitmap m_ImageArrowhead;					//时间箭头
        Bitmap m_ImageUserFrame;					//用户框架

	    //移动类型
	    enum ANTE_ANIME_ENUM
	    {
		    AA_BASEFROM_TO_BASEDEST=0,										// 底注筹码下注
		    AA_BASEDEST_TO_CENTER,											// 加注筹码移至中间
		    AA_CENTER_TO_BASEFROM,											// 中加筹码移至底注
	    };

	    GoldView[]						m_MoveGoldView = new GoldView[DzCardDefine.GAME_PLAYER];		//筹码控件
	    bool							m_bJettonAction;					//动画标志
	    bool							m_bCardAction;						//动画标志
	    tagJettonStatus[]				m_JettonStatus = new tagJettonStatus[DzCardDefine.GAME_PLAYER];		//筹码信息
	    List<tagCardStatus>[]           m_CardStatus = new List<tagCardStatus>[DzCardDefine.GAME_PLAYER+1];		//扑克信息
	    byte							m_bCenterCount;						//中心次数

	    //位置变量
        Point[] m_ptHuman = new Point[DzCardDefine.GAME_PLAYER];          //人物位置
        Point[] m_ptWomen = new Point[DzCardDefine.GAME_PLAYER];          //人物位置
        Point[] m_ptSmallCard = new Point[DzCardDefine.GAME_PLAYER];      //小牌位置
        Point[] m_ptCard = new Point[DzCardDefine.GAME_PLAYER];           //扑克位置
        Point[] m_ptJetton = new Point[DzCardDefine.GAME_PLAYER];         //筹码位置
        Point m_ptCenterCard = new Point();                             //中心扑克位置
        Point m_ptCenterJetton = new Point();                           //中心筹码位置
        Point[] m_ptD = new Point[DzCardDefine.GAME_PLAYER];              //D位置
        Point[] m_SendEndingPos = new Point[DzCardDefine.GAME_PLAYER];    //发牌结束位置
        Point[] m_ptSitDown = new Point[DzCardDefine.GAME_PLAYER];        //坐下按钮位置

	    //按钮控件
	    //PictureButton						m_btStart = new PictureButton();							//开始按钮
	    //PictureButton						m_btExit = new PictureButton();							//离开按钮
	    PictureButton						m_btPassCard = new PictureButton();						//放弃按纽
	    PictureButton						m_btShowHand = new PictureButton();						//梭哈按钮
	    PictureButton						m_btGiveUp = new PictureButton();							//放弃按钮	
	    PictureButton						m_btAdd = new PictureButton();							//加注按钮	
	    PictureButton						m_btFollow = new PictureButton();							//跟注按钮	
	    //PictureButton						m_btAutoStart = new PictureButton();						//自动开始
	    PictureButton						m_btOpenCard = new PictureButton();						//开牌按钮
	    PictureButton[]						m_btSitDown = new PictureButton[DzCardDefine.GAME_PLAYER];			//坐下按钮

	    //动画变量
	    bool							m_bGameEnd;							//游戏结束
	    Point							m_SendCardPos = new Point();						//发牌位置

	    //控件变量
	    Prompt							m_Prompt = new Prompt();							//提示结束
	    ScoreView						m_ScoreView = new ScoreView();		//成绩窗口
	    GoldControl					    m_GoldControl = new GoldControl();						//加注视图
	    GoldView[]						m_GoldView = new GoldView[DzCardDefine.GAME_PLAYER];			//筹码控件
	    GoldView						m_CenterGoldView = new GoldView();					//中心筹码
        CardControl[]					m_CardControl = new CardControl[DzCardDefine.GAME_PLAYER];			//扑克视图
	    CardControl					    m_CenterCardControl = new CardControl();				//中心视图
        SmallCardControl[]              m_SmallCardControl = new SmallCardControl[DzCardDefine.GAME_PLAYER];	//小扑克视图


        bool m_bOpenCard = false;
	    bool m_bExitTag	= false;
	    //bool m_bAutoStart = false;


        public DzCardGameStatus m_Status = DzCardGameStatus.GS_EMPTY;
        public DzCardInfo m_ReceiveInfo = null;
        public DzCardLogic m_GameLogic = new DzCardLogic();

        public DzCardView()
        {
            InitializeComponent();
        }

        protected override void InitGameView()
        {
            //加载位图
            m_ImageViewCenter = Properties.Resources.VIEW_BACK;
            m_ImageHuman = Properties.Resources.HUMAN;
            m_ImageSmallCard = Properties.Resources.SMALL_CARD;
            m_ImageCard = Properties.Resources.CARD;
            m_ImageD = Properties.Resources.D;
            m_ImageCardMask = Properties.Resources.CARD_MASK;
            m_ImageTitleInfo = Properties.Resources.TITLE;
            m_ImageArrowhead = Properties.Resources.TIME_ARROWHEAD;
            m_ImageUserFrame = Properties.Resources.FRAME;

            //位置变量
            //Array.Clear(m_wUserPost, 0, m_wUserPost.Length);
            Array.Clear(m_ptSmallCard, 0, m_ptSmallCard.Length);
            Array.Clear(m_ptCard, 0, m_ptCard.Length);
            Array.Clear(m_ptJetton, 0, m_ptJetton.Length);
            m_ptCenterCard = new Point();
            m_ptCenterJetton = new Point();

            //动画变量
            for (int i = 0; i < DzCardDefine.GAME_PLAYER + 1; i++)
                m_CardStatus[i] = new List<tagCardStatus>();


            for (int i = 0; i < m_JettonStatus.Length; i++)
                m_JettonStatus[i] = new tagJettonStatus();

            m_bGameEnd = false;
            m_bJettonAction = false;
            m_bCardAction = false;
            m_bCenterCount = 0;

            //数据变量
            Array.Clear(m_lTableScore, 0, m_lTableScore.Length);
            Array.Clear(m_lTotalScore, 0, m_lTotalScore.Length);
            m_lCenterScore = 0;
            m_lTurnLessScore = 0;
            m_lTurnMaxScore = 0;
            m_wMeChairID = GameDefine.INVALID_CHAIR;
            m_bShowUser = true;

            //状态变量
            m_bReset = true;
            m_wDUser = GameDefine.INVALID_CHAIR;
            m_wMyIsLookOn = GameDefine.INVALID_CHAIR;
            Array.Clear(m_wUserGender, 0, m_wUserGender.Length);


	        //创建控件
	        Rectangle rcCreate = new Rectangle(0,0,0,0);
            //m_ScoreView.Create(IDD_GAME_SCORE,this);
            //m_Prompt.Create(IDD_DIALOG2,this);
            //m_GoldControl.Create(NULL,NULL,WS_CHILD|WS_CLIPSIBLINGS,rcCreate,this,8);
            this.Controls.Add(m_GoldControl);

	        //创建按钮
            //m_btStart.Create(false, true, rcCreate,this,IDC_START);
            //m_btExit.Create(false, true, rcCreate,this,IDC_EXIT);
            m_btAdd.Create(false, true, rcCreate,this,IDC_ADD);
            m_btGiveUp.Create(false, true, rcCreate,this,IDC_GIVEUP);
            m_btShowHand.Create(false, true, rcCreate,this,IDC_SHOWHAND);
            m_btFollow.Create(false, true, rcCreate,this,IDC_FOLLOW);
            m_btPassCard.Create(false, true, rcCreate,this,IDC_PASS);
            //m_btAutoStart.Create(false, true, rcCreate,this,IDC_AUTO_START);
            m_btOpenCard.Create(false, true, rcCreate,this,IDC_OPEN_CARD);

            for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
            {
                m_btSitDown[i] = new PictureButton();
                m_btSitDown[i].Create(false, true, rcCreate,this,IDC_SIT_DOWN+i);
            }

            //加载位图
            //m_btStart.SetButtonImage(Properties.Resources.BT_START);
            //m_btExit.SetButtonImage(Properties.Resources.BT_EXIT);
            m_btAdd.SetButtonImage(Properties.Resources.BT_ADD);
            m_btGiveUp.SetButtonImage(Properties.Resources.BT_GIVEUP);
            m_btShowHand.SetButtonImage(Properties.Resources.BT_SHOWHAND);
            m_btFollow.SetButtonImage(Properties.Resources.BT_FOLLOW);
            m_btPassCard.SetButtonImage(Properties.Resources.BT_PASS_CARD);
            //m_btAutoStart.SetButtonImage(Properties.Resources.AUTO_START_FALSE);
            m_btOpenCard.SetButtonImage(Properties.Resources.BT_OPEN_CARD);

            //m_btStart.Click += OnStart;
            //m_btExit.Click += OnExit;
            m_btGiveUp.Click += OnGiveUp;
            m_btAdd.Click += OnAddScore;
            m_btPassCard.Click += OnPassCard;
            m_btFollow.Click += OnFollow;
            m_btShowHand.Click += OnShowHand;
            //m_btAutoStart.Click += OnAutoStart;


            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
            {
                m_btSitDown[i].SetButtonImage(Properties.Resources.BT_SITDOWN);
                m_btSitDown[i].Click += OnSitDown;
            }

            
            
            m_CenterCardControl.SetDisplayItem(true);

            //用户扑克
            for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
            {
                m_MoveGoldView[i] = new GoldView();
                m_GoldView[i] = new GoldView();

                m_CardControl[i] = new CardControl();
                m_CardControl[i].SetDisplayItem(true);

                m_SmallCardControl[i] = new SmallCardControl();
                m_SmallCardControl[i].SetCardDistance(5,0,0);
            }
            m_CenterCardControl.SetCardDistance(57+10,0,0);

        }

        private void DzCardView_Load(object sender, EventArgs e)
        {
            ResetGameView();
        }

        private void ResetGameView()
        {
            if (!m_bReset)
                return;

            m_bOpenCard = false;
            m_bExitTag = false;
            //m_bAutoStart = false;


            //状态变量
            m_wDUser = GameDefine.INVALID_CHAIR;
            m_wMyIsLookOn = GameDefine.INVALID_CHAIR;
            m_lTurnLessScore = 0;
            m_lTurnMaxScore = 0;

            //隐藏控件
            m_ScoreView.Visible = false;
            m_Prompt.Visible = false;
            m_GoldControl.Visible = false;
            //m_btStart.Visible = false;
            //m_btExit.Visible = false;
            m_btGiveUp.Visible = false;
            m_btShowHand.Visible = false;
            m_btFollow.Visible = false;

            //动画变量
            //Array.Clear(m_wUserPost, 0, m_wUserPost.Length);

            for (int i = 0; i < DzCardDefine.GAME_PLAYER + 1; i++)
                m_CardStatus[i].Clear();

            for (int i = 0; i < m_JettonStatus.Length; i++)
                m_JettonStatus[i] = new tagJettonStatus();

            m_bGameEnd = false;
            m_bJettonAction = false;
            m_bCardAction = false;
            m_bCenterCount = 0;

            //删除定时器
            //KillTimer(IDI_SEND_CARD);
            KillTimer(IDI_USER_ACTION);
            KillTimer(IDI_GAME_END);

            //数据变量
            Array.Clear(m_lTableScore, 0, m_lTableScore.Length);
            Array.Clear(m_lTotalScore, 0, m_lTotalScore.Length);

            //用户扑克
            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
            {
                //隐藏控件
                //m_btSitDown[i].Visible = false;
                m_CardControl[i].SetCardData(null, 0);
                m_SmallCardControl[i].SetCardData(null, 0);
                m_CardControl[i].SetDisplayItem(true);
                m_SmallCardControl[i].SetDisplayItem(false);
                m_CardControl[i].SetPositively(false);
                m_SmallCardControl[i].SetPositively(false);
            }

            //m_bShowUser = false;
            m_wMeChairID = GameDefine.INVALID_CHAIR;
            m_lCenterScore = 0;
            m_lTurnLessScore = 0;
            m_lTurnMaxScore = 0;
            m_CenterCardControl.SetCardData(null, 0);
            m_CenterCardControl.SetDisplayItem(true);
            m_CenterCardControl.SetPositively(false);
            return;
        }

        private void DzCardView_Resize(object sender, EventArgs e)
        {
            RectifyGameView(this.Width, this.Height);
        }

        //调整控件
        void RectifyGameView(int nWidth, int nHeight)
        {
	        //人物位置
	        m_ptHuman[0].X = nWidth/2 + 60;
	        m_ptHuman[0].Y = nHeight/2-259;
	        m_ptHuman[1].X = nWidth/2 + 225;
	        m_ptHuman[1].Y = nHeight/2-182;	
	        m_ptHuman[2].X = nWidth/2 +201;
	        m_ptHuman[2].Y = nHeight/2+29;	
	        m_ptHuman[3].X = nWidth/2 +46;
	        m_ptHuman[3].Y = nHeight/2+111;	
	        m_ptHuman[4].X = nWidth/2 -173;
	        m_ptHuman[4].Y = nHeight/2 +110;	
	        m_ptHuman[5].X = nWidth/2 -344;
	        m_ptHuman[5].Y = nHeight/2 + 28;	
	        m_ptHuman[6].X = nWidth/2 - 358;
	        m_ptHuman[6].Y = nHeight/2 - 182;	
	        m_ptHuman[7].X = nWidth/2 - 201;
	        m_ptHuman[7].Y = nHeight/2-265;

	        //人物位置
	        m_ptWomen[0].X = nWidth/2 + 63;
	        m_ptWomen[0].Y = nHeight/2-263;
	        m_ptWomen[1].X = nWidth/2 + 228;
	        m_ptWomen[1].Y = nHeight/2-186;	
	        m_ptWomen[2].X = nWidth/2 +226;
	        m_ptWomen[2].Y = nHeight/2+33;	
	        m_ptWomen[3].X = nWidth/2 +42;
	        m_ptWomen[3].Y = nHeight/2+106;	
	        m_ptWomen[4].X = nWidth/2 -170;
	        m_ptWomen[4].Y = nHeight/2 +106;	
	        m_ptWomen[5].X = nWidth/2 -358;
	        m_ptWomen[5].Y = nHeight/2 + 33;	
	        m_ptWomen[6].X = nWidth/2 - 355;
	        m_ptWomen[6].Y = nHeight/2 - 186;	
	        m_ptWomen[7].X = nWidth/2 - 193;
	        m_ptWomen[7].Y = nHeight/2-263;

	        //名字位置	
	        m_ptName[0].X = nWidth/2+74;
	        m_ptName[0].Y = nHeight/2-279-10;
	        m_ptName[1].X = nWidth/2+256;
	        m_ptName[1].Y = nHeight/2-220-10;	
	        m_ptName[2].X = nWidth/2+256;
	        m_ptName[2].Y = nHeight/2+163-10;	
	        m_ptName[3].X = nWidth/2+76;
	        m_ptName[3].Y = nHeight/2+231-10;	
	        m_ptName[4].X = nWidth/2-176;
	        m_ptName[4].Y = nHeight/2+231-10;
	        m_ptName[5].X = nWidth/2-363;
	        m_ptName[5].Y = nHeight/2+162-10;	
	        m_ptName[6].X = nWidth/2-363;
	        m_ptName[6].Y = nHeight/2-221-10;	
	        m_ptName[7].X = nWidth/2-179;
	        m_ptName[7].Y = nHeight/2-279-10;

	        //举手位置 
	        m_ptReady[0].X = nWidth/2 + 130;
	        m_ptReady[0].Y = nHeight/2-120;
	        m_ptReady[1].X = nWidth/2 + 220;
	        m_ptReady[1].Y = nHeight/2-70;	
	        m_ptReady[2].X = nWidth/2 +200;
	        m_ptReady[2].Y = nHeight/2+30;	
	        m_ptReady[3].X = nWidth/2 +100;
	        m_ptReady[3].Y = nHeight/2+90;	
	        m_ptReady[4].X = nWidth/2 -110;
	        m_ptReady[4].Y = nHeight/2 +90;	
	        m_ptReady[5].X = nWidth/2 -200;
	        m_ptReady[5].Y = nHeight/2 + 30;	
	        m_ptReady[6].X = nWidth/2 - 220;
	        m_ptReady[6].Y = nHeight/2 - 70;	
	        m_ptReady[7].X = nWidth/2 - 130;
	        m_ptReady[7].Y = nHeight/2-120;
        	
	        //m_ptReady[0].X = nWidth/2 + 130;
	        //m_ptReady[0].Y = nHeight/2-220;
	        //m_ptReady[1].X = nWidth/2 + 300;
	        //m_ptReady[1].Y = nHeight/2-160;	
	        //m_ptReady[2].X = nWidth/2 +290;
	        //m_ptReady[2].Y = nHeight/2+70;	
	        //m_ptReady[3].X = nWidth/2 +100;
	        //m_ptReady[3].Y = nHeight/2+150;	
	        //m_ptReady[4].X = nWidth/2 -110;
	        //m_ptReady[4].Y = nHeight/2 +150;	
	        //m_ptReady[5].X = nWidth/2 -285;
	        //m_ptReady[5].Y = nHeight/2 + 70;	
	        //m_ptReady[6].X = nWidth/2 - 300;
	        //m_ptReady[6].Y = nHeight/2 - 160;	
	        //m_ptReady[7].X = nWidth/2 - 130;
	        //m_ptReady[7].Y = nHeight/2-220;

	        //时间位置
	        //m_ptTimer[0].X = nWidth/2 + 160;
	        //m_ptTimer[0].Y = nHeight/2-250;
	        //m_ptTimer[1].X = nWidth/2 + 200;
	        //m_ptTimer[1].Y = nHeight/2-180;	
	        //m_ptTimer[2].X = nWidth/2 +260;
	        //m_ptTimer[2].Y = nHeight/2+180;	
	        //m_ptTimer[3].X = nWidth/2 +180;
	        //m_ptTimer[3].Y = nHeight/2+170;	
	        //m_ptTimer[4].X = nWidth/2 -200;
	        //m_ptTimer[4].Y = nHeight/2 +170;	
	        //m_ptTimer[5].X = nWidth/2 -280;
	        //m_ptTimer[5].Y = nHeight/2 + 180;	
	        //m_ptTimer[6].X = nWidth/2 - 260;
	        //m_ptTimer[6].Y = nHeight/2 - 180;	
	        //m_ptTimer[7].X = nWidth/2 - 210;
	        //m_ptTimer[7].Y = nHeight/2-250;

	        //时间位置
	        m_ptTimer[0].X = nWidth/2 + 60;
	        m_ptTimer[0].Y = nHeight/2-170;
	        m_ptTimer[1].X = nWidth/2 + 210;
	        m_ptTimer[1].Y = nHeight/2-145;	
	        m_ptTimer[2].X = nWidth/2 +325;
	        m_ptTimer[2].Y = nHeight/2+35;	
	        m_ptTimer[3].X = nWidth/2 +180;
	        m_ptTimer[3].Y = nHeight/2+190;	
	        m_ptTimer[4].X = nWidth/2 -40;
	        m_ptTimer[4].Y = nHeight/2 +190;	
	        m_ptTimer[5].X = nWidth/2 -225;
	        m_ptTimer[5].Y = nHeight/2 + 170;	
	        m_ptTimer[6].X = nWidth/2 - 335;
	        m_ptTimer[6].Y = nHeight/2 - 30;	
	        m_ptTimer[7].X = nWidth/2 - 210;
	        m_ptTimer[7].Y = nHeight/2-160;

	        for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        m_ptTimer[i].Y -=10; 
	        }

	        //小牌位置
	        m_ptSmallCard[0].X = nWidth/2 + 90;
	        m_ptSmallCard[0].Y = nHeight/2 - 175+60;
	        m_ptSmallCard[1].X = nWidth/2 + 210;
	        m_ptSmallCard[1].Y = nHeight/2-120+60;	
	        m_ptSmallCard[2].X = nWidth/2 +200;
	        m_ptSmallCard[2].Y = nHeight/2-15+60;	
	        m_ptSmallCard[3].X = nWidth/2 +65;
	        m_ptSmallCard[3].Y = nHeight/2+35+60;	
	        m_ptSmallCard[4].X = nWidth/2 -100;
	        m_ptSmallCard[4].Y = nHeight/2 +35+60;	
	        m_ptSmallCard[5].X = nWidth/2 -230;
	        m_ptSmallCard[5].Y = nHeight/2 - 15+60;	
	        m_ptSmallCard[6].X = nWidth/2 - 235;
	        m_ptSmallCard[6].Y = nHeight/2 - 120+60;	
	        m_ptSmallCard[7].X = nWidth/2 - 110;
	        m_ptSmallCard[7].Y = nHeight/2-175+60;
	        
            for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
                m_ptSmallCard[i].Y -=10; 
        	
	        //小扑克视图
	        m_SmallCardControl[0].SetBenchmarkPos(m_ptSmallCard[0],enXCollocateMode.enXLeft,enYCollocateMode.enYCenter);
	        m_SmallCardControl[1].SetBenchmarkPos(m_ptSmallCard[1],enXCollocateMode.enXLeft,enYCollocateMode.enYCenter);
	        m_SmallCardControl[2].SetBenchmarkPos(m_ptSmallCard[2],enXCollocateMode.enXLeft,enYCollocateMode.enYCenter);
	        m_SmallCardControl[3].SetBenchmarkPos(m_ptSmallCard[3],enXCollocateMode.enXLeft,enYCollocateMode.enYCenter);
	        m_SmallCardControl[4].SetBenchmarkPos(m_ptSmallCard[4],enXCollocateMode.enXLeft,enYCollocateMode.enYCenter);
	        m_SmallCardControl[5].SetBenchmarkPos(m_ptSmallCard[5],enXCollocateMode.enXLeft,enYCollocateMode.enYCenter);
	        m_SmallCardControl[6].SetBenchmarkPos(m_ptSmallCard[6],enXCollocateMode.enXLeft,enYCollocateMode.enYCenter);
	        m_SmallCardControl[7].SetBenchmarkPos(m_ptSmallCard[7],enXCollocateMode.enXLeft,enYCollocateMode.enYCenter);

	        //扑克位置
	        m_ptCard[0].X = nWidth/2 + 90;
	        m_ptCard[0].Y = nHeight/2-240;
	        m_ptCard[1].X = nWidth/2 + 270;
	        m_ptCard[1].Y = nHeight/2-160;	
	        m_ptCard[2].X = nWidth/2 +260;
	        m_ptCard[2].Y = nHeight/2+60;	
	        m_ptCard[3].X = nWidth/2 +70;
	        m_ptCard[3].Y = nHeight/2+130;	
	        m_ptCard[4].X = nWidth/2 -140;
	        m_ptCard[4].Y = nHeight/2 +130;	
	        m_ptCard[5].X = nWidth/2 -340;
	        m_ptCard[5].Y = nHeight/2 + 60;	
	        m_ptCard[6].X = nWidth/2 - 347;
	        m_ptCard[6].Y = nHeight/2 -160;	
	        m_ptCard[7].X = nWidth/2 - 170;
	        m_ptCard[7].Y = nHeight/2-240;
	        
            for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
                m_ptCard[i].Y -=14; 

	        //扑克视图
	        m_CardControl[0].SetBenchmarkPos(m_ptCard[0],enXCollocateMode.enXLeft,enYCollocateMode.enYTop);
	        m_CardControl[1].SetBenchmarkPos(m_ptCard[1],enXCollocateMode.enXLeft,enYCollocateMode.enYTop);
	        m_CardControl[2].SetBenchmarkPos(m_ptCard[2],enXCollocateMode.enXLeft,enYCollocateMode.enYTop);
	        m_CardControl[3].SetBenchmarkPos(m_ptCard[3],enXCollocateMode.enXLeft,enYCollocateMode.enYTop);
	        m_CardControl[4].SetBenchmarkPos(m_ptCard[4],enXCollocateMode.enXLeft,enYCollocateMode.enYTop);
	        m_CardControl[5].SetBenchmarkPos(m_ptCard[5],enXCollocateMode.enXLeft,enYCollocateMode.enYTop);
	        m_CardControl[6].SetBenchmarkPos(m_ptCard[6],enXCollocateMode.enXLeft,enYCollocateMode.enYTop);
	        m_CardControl[7].SetBenchmarkPos(m_ptCard[7],enXCollocateMode.enXLeft,enYCollocateMode.enYTop);

	        //筹码位置
	        m_ptJetton[0].X = nWidth/2 + 60;
	        m_ptJetton[0].Y = nHeight/2-80;
	        m_ptJetton[1].X = nWidth/2 +145;
	        m_ptJetton[1].Y = nHeight/2-66;
	        m_ptJetton[2].X = nWidth/2 +175;//170
	        m_ptJetton[2].Y = nHeight/2+68;	
	        m_ptJetton[3].X = nWidth/2 +44;
	        m_ptJetton[3].Y = nHeight/2+107;
	        m_ptJetton[4].X = nWidth/2 -44;
	        m_ptJetton[4].Y = nHeight/2 +107;
	        m_ptJetton[5].X = nWidth/2 -170;
	        m_ptJetton[5].Y = nHeight/2 + 68;	
	        m_ptJetton[6].X = nWidth/2 - 140-10;
	        m_ptJetton[6].Y = nHeight/2 - 66;	
	        m_ptJetton[7].X = nWidth/2 - 60;
	        m_ptJetton[7].Y = nHeight/2-80;
	        
            for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
                m_ptJetton[i].Y -=13; 

	        //发牌位置
	        m_SendCardPos = new Point(nWidth/2-5,nHeight/2-145-10);

	        //中心扑克位置
	        m_ptCenterCard = new Point(nWidth/2-165,nHeight/2-4-14);

	        //中心扑克
	        m_CenterCardControl.SetBenchmarkPos(m_ptCenterCard,enXCollocateMode.enXLeft,enYCollocateMode.enYCenter);

	        //中心筹码位置
	        m_ptCenterJetton = new Point(nWidth/2,nHeight/2+45-10);

	        //D位置
	        m_ptD[0].X = nWidth/2 -12;
	        m_ptD[0].Y = nHeight/2 -100;
	        m_ptD[1].X = nWidth/2 + 155;
	        m_ptD[1].Y = nHeight/2-84;	
	        m_ptD[2].X = nWidth/2 +185;
	        m_ptD[2].Y = nHeight/2-20;	
	        m_ptD[3].X = nWidth/2+140;
	        m_ptD[3].Y = nHeight/2 +50;	
	        m_ptD[4].X = nWidth/2 -12;
	        m_ptD[4].Y = nHeight/2 +65;	
	        m_ptD[5].X = nWidth/2 -160;
	        m_ptD[5].Y = nHeight/2 + 50;	
	        m_ptD[6].X = nWidth/2 - 210;
	        m_ptD[6].Y = nHeight/2 - 20;	
	        m_ptD[7].X = nWidth/2 -184;
	        m_ptD[7].Y = nHeight/2 -84;
	        
            for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
                m_ptD[i].Y -=10; 

	        //发牌结束位置
	        m_SendEndingPos[0] = new Point(nWidth/2+85,nHeight/2-100);
	        m_SendEndingPos[1] = new Point(nWidth/2+235,nHeight/2-120);
	        m_SendEndingPos[2] = new Point(nWidth/2+210,nHeight/2+140);
	        m_SendEndingPos[3] = new Point(nWidth/2+80,nHeight/2+90);
	        m_SendEndingPos[4] = new Point(nWidth/2-70,nHeight/2+90);
	        m_SendEndingPos[5] = new Point(nWidth/2-160,nHeight/2+140);
	        m_SendEndingPos[6] = new Point(nWidth/2-260,nHeight/2-120);
	        m_SendEndingPos[7] = new Point(nWidth/2-92,nHeight/2-240);
	        
            for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
                m_SendEndingPos[i].Y -=10; 

	        //坐下按钮位置
	        m_ptSitDown[0] = new Point(nWidth/2+95,nHeight/2-210);
	        m_ptSitDown[1] = new Point(nWidth/2+270,nHeight/2-120);
	        m_ptSitDown[2] = new Point(nWidth/2+270,nHeight/2+120);
	        m_ptSitDown[3] = new Point(nWidth/2+75,nHeight/2+200);
	        m_ptSitDown[4] = new Point(nWidth/2-140,nHeight/2+200);
	        m_ptSitDown[5] = new Point(nWidth/2-340,nHeight/2+120);
	        m_ptSitDown[6] = new Point(nWidth/2-340,nHeight/2-120);
	        m_ptSitDown[7] = new Point(nWidth/2-165,nHeight/2-210);
	        
            for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
                m_ptSitDown[i].Y -=10; 

	        //按钮控件
	        Rectangle rcButton = new Rectangle();
	        IntPtr  hDwp = GameGraphics.BeginDeferWindowPos(32);
            const uint uFlags = GameGraphics.SWP_NOACTIVATE | GameGraphics.SWP_NOZORDER | GameGraphics.SWP_NOCOPYBITS | GameGraphics.SWP_NOSIZE;

	        //开始按钮
	        //m_btStart.GetWindowRect(&rcButton);
            rcButton.Width = 73;
            rcButton.Height = 43;

            //GameGraphics.DeferWindowPos(hDwp, m_btStart.Handle, 0, nWidth - rcButton.Width - m_nXBorder * 2, nHeight / 2 + 240 - rcButton.Height / 2, 0, 0, uFlags);

	        //离开按钮
            //GameGraphics.DeferWindowPos(hDwp, m_btExit.Handle, 0, nWidth - rcButton.Width - m_nXBorder * 2, nHeight / 2 + 298 - rcButton.Height / 2, 0, 0, uFlags);

	        //自动开始按钮
            //GameGraphics.DeferWindowPos(hDwp, m_btAutoStart.Handle, 0, m_nXBorder * 2, nHeight / 2 + 300, 0, 0, uFlags);

	        //放弃按钮
            GameGraphics.DeferWindowPos(hDwp, m_btGiveUp.Handle, 0, nWidth / 2 + 88, nHeight / 2 + 283, 0, 0, uFlags);

	        //梭哈按钮
            GameGraphics.DeferWindowPos(hDwp, m_btShowHand.Handle, 0, nWidth / 2 + 4, nHeight / 2 + 283, 0, 0, uFlags);

	        //加注按钮
            GameGraphics.DeferWindowPos(hDwp, m_btAdd.Handle, 0, nWidth / 2 - 79, nHeight / 2 + 283, 0, 0, uFlags);

	        //开牌按钮
            GameGraphics.DeferWindowPos(hDwp, m_btOpenCard.Handle, 0, nWidth / 2 - 35, nHeight / 2 + 283, 0, 0, uFlags);

	        //跟注按钮
            GameGraphics.DeferWindowPos(hDwp, m_btFollow.Handle, 0, nWidth / 2 - 163, nHeight / 2 + 283, 0, 0, uFlags);

	        //让牌按钮
            GameGraphics.DeferWindowPos(hDwp, m_btPassCard.Handle, 0, nWidth / 2 - 163, nHeight / 2 + 283, 0, 0, uFlags);

	        //坐下按钮
	        for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
                GameGraphics.DeferWindowPos(hDwp, m_btSitDown[i].Handle, 0, m_ptSitDown[i].X, m_ptSitDown[i].Y, 0, 0, uFlags);

	        //结束移动
            GameGraphics.EndDeferWindowPos(hDwp);

	        //金币视图
	        //CRect rcControl;
	        ////m_ScoreView.GetWindowRect(&rcControl);
	        ////m_ScoreView.SetWindowPos(NULL,10,nHeight/2+235,/*nWidth/2-rcControl.Width()/2,nHeight/2+56,*/0,0,SWP_NOZORDER|SWP_NOSIZE);

	        //提示弹起视图
	        m_Prompt.Left=nWidth/2+395;
            m_Prompt.Top=nHeight/2-200;

	        //加注窗口
	        m_GoldControl.SetBasicPoint(nWidth/2+100,nHeight/2+100);

	        return;
        }

        private void DzCardView_Paint(object sender, PaintEventArgs e)
        {
            if (m_ReceiveInfo == null)
                return;

            DrawGameView(e.Graphics, this.Width, this.Height);
        }

        //获取时间
        protected int GetUserTimer(int wChairID)
        {
            if (wChairID != m_ReceiveInfo.m_wCurrentUser)
                return 0;

            int wTimerElapse = 0;

            foreach (TimerInfo timerInfo in _TimerList)
            {
                if (timerInfo._Id == IDI_USER_ADD_SCORE)
                {
                    wTimerElapse = timerInfo.Elapse;
                    break;
                }
            }

            return wTimerElapse;
        }



        //绘画界面
        void DrawGameView(Graphics g, int nWidth, int nHeight)
        {
	        //绘画背景
            g.DrawImage(m_ImageViewCenter, new Point( (nWidth - m_ImageViewCenter.Width)/2, (nHeight-m_ImageViewCenter.Height)/2));
            //g.DrawImage(m_ImageViewCenter, 0, 0);

	        //人物句柄
	        //CImageHandle ImageHumanHandle(&m_ImageHuman);

	        //获取大小
	        int nItemWidth=m_ImageHuman.Width/8;
	        int nItemHeight=m_ImageHuman.Height/2;

	        //扑克句柄
	        //CImageHandle ImageCardHandle(&m_ImageCard);

	        //扑克大小
	        int nCardWidth=m_ImageCard.Width/13;
	        int nCardHeight=m_ImageCard.Height/5;

	        //D句柄
	        //CImageHandle ImageDHandle(&m_ImageD);

	        //D大小
	        int nDWidth = m_ImageD.Width;
	        int nDHeight = m_ImageD.Height;

	        //扑克掩图
	        //CImageHandle ImageCardMaskHandle(&m_ImageCardMask);

	        //掩图大小
	        int nCardMaskWidth = m_ImageCardMask.Width;
	        int nCardMaskHeight = m_ImageCardMask.Height;

	        //绘画用户
	        if(!m_bReset)
	        {
		        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
		        {
			        //绘制人物
			        if(m_wUserGender[i] == 2)//男
			        {
				        DrawAlphaImage(g, m_ImageHuman, m_ptHuman[i].X, m_ptHuman[i].Y, nItemWidth, nItemHeight, nItemWidth*i, 0, Color.FromArgb(255,0,255));
			        }
			        else if(m_wUserGender[i] == 1)//女
			        {
				        DrawAlphaImage(g, m_ImageHuman, m_ptWomen[i].X, m_ptWomen[i].Y, nItemWidth,nItemHeight,nItemWidth*i,nItemHeight,Color.FromArgb(255,0,255));
			        }
		        }
	        }

	        //绘画用户
	        if(m_bShowUser==false)
                return ;
	        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        //变量定义
		        int wUserTimer=GetUserTimer(i);
		        UserInfo pUserData=GetUserInfo(i);

        //#ifdef _DEBUG
		        //tagUserData Obj;
		        //Obj.cbGender = 0;
		        //_sntprintf( Obj.szName,sizeof(Obj.szName),TEXT("用户的名字"));
		        //Obj.lScore = 10000;
		        //Obj.cbUserStatus = US_READY;
		        //m_wUserPost[i] = i;
		        //pUserData = &Obj;
		        //wUserTimer = 10;
        //#endif

		        //绘画用户
		        if (pUserData!=null)
		        {
			        //int wUserId = m_wUserPost[i];
                    int wUserId = i;
			        //pDC->SetTextColor((wUserTimer>0)?RGB(0,250,0):RGB(0,220,0));

			        //绘制人物
			        if(pUserData.Kind != (int)UserKind.ServiceWoman)
				        DrawAlphaImage( g, m_ImageHuman, m_ptHuman[wUserId].X, m_ptHuman[wUserId].Y, nItemWidth,nItemHeight,nItemWidth*wUserId,0, Color.FromArgb(255,0,255));
			        else
				        DrawAlphaImage( g, m_ImageHuman, m_ptWomen[wUserId].X, m_ptWomen[wUserId].Y, nItemWidth,nItemHeight,nItemWidth*wUserId,nItemHeight, Color.FromArgb(255,0,255));

			        //绘制举手
                    //if (m_Status == GameStatus.GS_JOIN && m_ReceiveInfo.m_cbPlayStatus[i] == true )
                    //    DrawUserReady(g,m_ptReady[wUserId].X,m_ptReady[wUserId].Y);

			        //绘制时间
			        if (wUserTimer>0)
			        {
				        DrawUserTimer(g,m_ptTimer[wUserId].X,m_ptTimer[wUserId].Y,wUserTimer,99);
				        
                        if(wUserTimer%2==0)
				        {
					        //CImageHandle ImageArrowheadHandle(&m_ImageArrowhead);
					        int wPost = (wUserId+2)%8;
					        int iAddY=0,iAddX=0;
					        if(wUserId==0 || wUserId==1)
					        {
						        iAddY=25;
						        //iAddX=10;
					        }
					        else if(wUserId==2)
					        {
						        iAddX=-40;
					        }
					        else if(wUserId==3||wUserId==4)
					        {
						        wPost = 6;
						        iAddY=-45;
						        iAddX=-10;
					        }
					        else if(wUserId==5)
					        {
						        iAddY=-45;
						        //iAddX=10;
					        }
					        else if(wUserId==6)
					        {
						        iAddY=5;
						        iAddX=35;
					        }
					        else if(wUserId==7)
					        {
						        iAddY=25;
						        iAddX=10;
					        }
					        
                            //DrawAlphaImage( g, m_ImageArrowhead, m_ptTimer[wUserId].X+iAddX,m_ptTimer[wUserId].Y+iAddY,
                            //    m_ImageArrowhead.Width/8,m_ImageArrowhead.Height,
                            //    m_ImageArrowhead.Width / 8 * (wUserTimer%8), 0, Color.FromArgb(255, 0, 255));
				        }
			        }

			        //绘制D
			        if(m_wDUser != GameDefine.INVALID_CHAIR)
			        {
				        DrawAlphaImage( g, m_ImageD, m_ptD[m_wDUser].X, m_ptD[m_wDUser].Y, nDWidth,nDHeight,0,0, Color.FromArgb(255,0,255));
			        }

			        //绘制小扑克
			        //if (m_Status != GameStatus.GS_JOIN)
		            m_SmallCardControl[wUserId].DrawCardControl(g);

			        m_CardControl[wUserId].DrawCardControl(g,m_bGameEnd);

			        if(m_lTableScore[wUserId]>0)
			        {
				        m_GoldView[wUserId].DrawGoldView(g,m_ptJetton[wUserId].X,m_ptJetton[wUserId].Y,true, false, 0);
			        }
		        }

		        //用户扑克
		        //m_CardControl[i].DrawCardControl(g,m_bGameEnd);

		        //中心扑克
		        m_CenterCardControl.DrawCardControl(g,m_bGameEnd);
	        }

	        //绘制中心筹码
	        if(m_lCenterScore >0)
	        {
		        m_CenterGoldView.DrawGoldView(g,m_ptCenterJetton.X,m_ptCenterJetton.Y,true,true,(byte)m_wDUser);
	        }

	        //绘画扑克
	        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        ////绘制小扑克
		        //if(i<GAME_PLAYER)
		        //{
		        //	m_SmallCardControl[i].DrawCardControl(pDC);
		        //}

		        //筹码移动
		        if(i < m_MoveGoldView.Length && m_MoveGoldView[i].GetGold()>0 && i<DzCardDefine.GAME_PLAYER)
		        {
			        m_MoveGoldView[i].DrawGoldView(g,m_JettonStatus[i].ptCourse.X,m_JettonStatus[i].ptCourse.Y,true, false, 0);
		        }

		        //小扑克
		        if(m_CardStatus[i].Count==0)
                    continue;

		        tagCardStatus pCardStatus = m_CardStatus[i][0];
		        if(pCardStatus.wMoveCount>0)
		        {
			        //小扑克
			        m_SmallCardControl[i].DrawOneCard(g,0,pCardStatus.ptCourse.X,pCardStatus.ptCourse.Y);			
		        }
	        }

	        //绘画用户
	        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        UserInfo pUserData=GetUserInfo(i);
		        
                //绘画用户
		        if (pUserData!=null)
		        {
			        //int wUserId = m_wUserPost[i];
                    int wUserId = i;

			        //绘画框架
			        //CImageHandle ImageFrameHandle(&m_ImageUserFrame);
			        DrawAlphaImage( g, m_ImageUserFrame, m_ptName[wUserId].X,m_ptName[wUserId].Y,
				        m_ImageUserFrame.Width/8,m_ImageUserFrame.Height,
				        m_ImageUserFrame.Width/8*wUserId,0,Color.FromArgb(255,0,255));

			        Font ViewFont = new Font("Arial", 15);
			        //ViewFont.CreateFont(-15,0,0,0,400,0,0,0,134,3,2,1,1,TEXT("Arial"));
			        //CFont *pOldFont=pDC->SelectObject(&ViewFont);

			        Size rcSize = new Size(103,30);
			        Rectangle rcName = new Rectangle(m_ptName[wUserId],rcSize);
			        //pDC->SetBkMode(TRANSPARENT);
			        //pDC->DrawText(pUserData->szName,lstrlen(pUserData->szName),&rcName,DT_CENTER|DT_VCENTER|DT_SINGLELINE|DT_END_ELLIPSIS);
                    Brush textBrush = new SolidBrush(Color.FromArgb(255,255,255));
                    GameGraphics.DrawString(g, pUserData.Nickname, ViewFont, textBrush, rcName);


			        //用户金币
			        Rectangle rcName1 = new Rectangle(m_ptName[wUserId].X,m_ptName[wUserId].Y+20,rcSize.Width, rcSize.Height);
                    textBrush = new SolidBrush(Color.FromArgb(255,255,4));
                    string szBuffer = string.Empty;

                    int lLeaveScore = 0;

                    if (pUserData.nCashOrPointGame == 0)
                    {
                        lLeaveScore = pUserData.Cash - ((m_lTotalScore[wUserId] > 0) ? m_lTotalScore[wUserId] : 0);

                        if (m_Status == DzCardGameStatus.GS_END)
                            lLeaveScore = pUserData.Cash;

                        if (lLeaveScore < 0)
                            lLeaveScore = pUserData.Cash;
                    }
                    else
                    {
                        lLeaveScore = pUserData.Point - ((m_lTotalScore[wUserId] > 0) ? m_lTotalScore[wUserId] : 0);

                        if (m_Status == DzCardGameStatus.GS_END)
                            lLeaveScore = pUserData.Point;

                        if (lLeaveScore < 0)
                            lLeaveScore = pUserData.Point;
                    }

                    //加千数点
                    int lTempCount = lLeaveScore;
                    szBuffer = GameGraphics.GetGoldString(lTempCount);

                    GameGraphics.DrawString(g, szBuffer, ViewFont, textBrush, rcName1 );
		        }
	        }

	        //坐下按钮
	        //查找空位
	        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
                if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                {
                    m_btSitDown[i].Visible = false;
                }
                else
                {
                    UserInfo pUserData = GetUserInfo(i);

                    //int wUserId = m_wUserPost[i];
                    int wUserId = i;

                    //坐下按钮
                    if (pUserData == null)
                    {
                        m_btSitDown[wUserId].Visible = true;
                    }
                    else
                    {
                        m_btSitDown[wUserId].Visible = false;
                    }
                }
	        }

            if (m_Status == DzCardGameStatus.GS_END)
            {
                HideScoreControl();
            }
            else
            {
                m_btOpenCard.Visible = false;
            }

	        return;
        }

        UserInfo GetUserInfo(int chairID)
        {
            if (chairID == GameDefine.INVALID_CHAIR)
                return null;
            if (chairID >= DzCardDefine.GAME_PLAYER)
                return null;
            if (m_ReceiveInfo == null)
                return null;

            return m_ReceiveInfo.m_Seatter[chairID];
        }

        //时间消息
        protected override void OnTimer(TimerInfo timerInfo)
        {
	        //消息处理
	        switch (timerInfo._Id)
	        {
	        case IDI_GAME_END:		//动画变量
		        {
			        m_bGameEnd = !m_bGameEnd;
			        Invalidate();//UpdateGameView(NULL);

			        return;
		        }
	        case IDI_USER_ACTION:	//筹码动作
		        {
			        bool bKillTime = true;

			        //筹码移动
			        if(m_bJettonAction && MoveJetton()) 
                        bKillTime = false;

			        //筹码移动
			        if(m_bCardAction && MoveCard()) 
                        bKillTime = false;

			        //删除定时器
			        if(bKillTime)
			        {
				        m_bJettonAction = false;
				        m_bCardAction = false;
				        KillTimer(IDI_USER_ACTION);
			        }

			        return;
		        }
	        case IDI_GAME_OVER:		//游戏结束
		        {
			        if(!m_bJettonAction && !m_bCardAction)
			        {
				        KillTimer(IDI_GAME_OVER);				
				        this.NotifyMessage( NotifyMessageKind.IDM_GAME_OVER,0,0);
			        }

			        return;
		        }
	        case IDI_NO_SCORE:		//不够钱提示框
		        {
			        byte bCount = (byte)m_CenterCardControl.GetCardCount();
			        if(bCount==DzCardDefine.MAX_CENTERCOUNT)
			        {
				        KillTimer(IDI_NO_SCORE);
				        m_Prompt.Visible = true;
			        }

			        return;
		        }
	        }

        }

        void SetShowUserView(bool bShow)
        {
	        m_bShowUser=bShow;

	        //更新界面
	        //Invalidate();//UpdateGameView(NULL);

	        return ;
        }

        //设置位置
        void SetMyLookOn(int wIsLookOnId)
        {
	        m_wMyIsLookOn=wIsLookOnId;

	        //更新界面
	        //Invalidate();//UpdateGameView(NULL);

	        return;
        }

        void SetDFlag(int wDUser)
        {
	        //设置数据
	        m_wDUser = wDUser;

	        //更新界面
	        //Invalidate();//UpdateGameView(NULL);
	        return;
        }

        //设置下注
        void SetUserTableScore(int wChairID, int lTableScore)
        {
	        //设置数据
	        if (wChairID!=GameDefine.INVALID_CHAIR) 
	        {
		        m_lTableScore[wChairID]=lTableScore;
		        m_GoldView[wChairID].SetGold(lTableScore);
	        }
	        else
	        {
		        Array.Clear(m_lTableScore,0, m_lTableScore.Length);
		        for (int i =0;i<DzCardDefine.GAME_PLAYER;i++)
		        {
			        m_GoldView[i].SetGold(0);
		        }
	        }

	        //更新界面
	        //Invalidate();//UpdateGameView(NULL);

	        return;
        }

        //设置总筹码
        void SetTotalScore(int wChairID, int lTotalScore)
        {
	        //ASSERT(wChairID>=0 && wChairID <GAME_PLAYER);
	        m_lTotalScore[wChairID] = lTotalScore;

	        //更新界面
	        //Invalidate();//UpdateGameView(NULL);
	        return;

        }

        //筹码信息
        void SetTitleInfo(int lTurnLessScore, int lTurnMaxScore)
        {
	        //设置变量
	        m_lTurnLessScore = lTurnLessScore;
	        m_lTurnMaxScore = lTurnMaxScore;

	        //更新界面
	        //Invalidate();//UpdateGameView(NULL);

	        return ;
        }

        //中心筹码
        void SetCenterScore(int lCenterScore)
        {
	        //设置数据
	        m_lCenterScore = lCenterScore;
	        m_CenterGoldView.SetGold(lCenterScore);

	        //更新变量
	        //Invalidate();//UpdateGameView(NULL);

	        return;

        }

        //设置结束
        void SetGameEndStart()
        {
	        SetTimer(IDI_GAME_END,400);
	        return;
        }

        void SetGameEndEnd()
        {
	        KillTimer(IDI_GAME_END);
	        return;
        }

        //绘制动画, 0: 底注动画, 1: 加注动画, 2: 加注合并到底注, 3: 赢家收筹码
        void DrawMoveAnte( int wViewChairID, int iMoveType,int lTableScore)
        {
	        //动画步数
	        int nAnimeStep = 60;		

	        m_JettonStatus[wViewChairID].wMoveIndex = 0;
	        m_JettonStatus[wViewChairID].iMoveType = iMoveType;
	        m_MoveGoldView[wViewChairID].SetGold(lTableScore);

	        switch( iMoveType )
	        {
	        case (int)ANTE_ANIME_ENUM.AA_BASEFROM_TO_BASEDEST:	// 底注筹码下注
		        m_JettonStatus[wViewChairID].ptFrom = m_ptCard[wViewChairID];
		        m_JettonStatus[wViewChairID].ptDest = m_ptJetton[wViewChairID];
		        m_JettonStatus[wViewChairID].ptCourse= m_ptCard[wViewChairID];
		        m_JettonStatus[wViewChairID].lGold =lTableScore;
		        break;

	        case (int)ANTE_ANIME_ENUM.AA_CENTER_TO_BASEFROM:		// 中间筹码移至下注
		        m_JettonStatus[wViewChairID].ptFrom = m_ptCenterJetton;
		        m_JettonStatus[wViewChairID].ptDest = m_ptJetton[wViewChairID];
		        m_JettonStatus[wViewChairID].ptCourse =m_ptCenterJetton;
		        m_JettonStatus[wViewChairID].lGold =lTableScore;
		        break;

	        case (int)ANTE_ANIME_ENUM.AA_BASEDEST_TO_CENTER:		// 加注合并到中间
		        m_JettonStatus[wViewChairID].ptFrom = m_ptJetton[wViewChairID];
		        m_JettonStatus[wViewChairID].ptDest = m_ptCenterJetton;
		        m_JettonStatus[wViewChairID].ptCourse= m_ptJetton[wViewChairID];
		        m_JettonStatus[wViewChairID].lGold =lTableScore;
		        break;

	        //default:
		        //ASSERT(FALSE);
	        }

	        //位移计算
            Random random = new Random();

	        int nXCount=Math.Abs(m_JettonStatus[wViewChairID].ptDest.X-m_JettonStatus[wViewChairID].ptFrom.X)/nAnimeStep+random.Next()%8;
	        int nYCount=Math.Abs(m_JettonStatus[wViewChairID].ptDest.Y-m_JettonStatus[wViewChairID].ptFrom.Y)/nAnimeStep+random.Next()%8;
	        m_JettonStatus[wViewChairID].wMoveCount = Math.Max(1,Math.Max(nXCount,nYCount));;

	        //设置时间
	        if(!m_bJettonAction)
	        {
		        m_bJettonAction = true;
		        SetTimer(IDI_USER_ACTION,TIME_USER_ACTION);
	        }

	        return;
        }

        //移动筹码
        bool MoveJetton()
        {
	        bool bAllClean = true;

	        //设置变量
	        for(byte i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        //移动步数
		        if(m_JettonStatus[i].wMoveIndex<m_JettonStatus[i].wMoveCount)
		        {
			        bAllClean = false;
			        m_JettonStatus[i].wMoveIndex++;
			        int wMoveIndex = m_JettonStatus[i].wMoveIndex;
			        int wMoveCount = m_JettonStatus[i].wMoveCount;
			        m_JettonStatus[i].ptCourse.X =m_JettonStatus[i].ptFrom.X + (m_JettonStatus[i].ptDest.X-m_JettonStatus[i].ptFrom.X)*wMoveIndex/wMoveCount;
			        m_JettonStatus[i].ptCourse.Y =m_JettonStatus[i].ptFrom.Y + (m_JettonStatus[i].ptDest.Y-m_JettonStatus[i].ptFrom.Y)*wMoveIndex/wMoveCount;
		        }
		        else if(m_JettonStatus[i].wMoveCount>0)
		        {
			        //筹码处理
			        switch( m_JettonStatus[i].iMoveType )
			        {
			        case (int)ANTE_ANIME_ENUM.AA_BASEFROM_TO_BASEDEST:	// 底注筹码下注
				        //m_lTableScore[i] += m_JettonStatus[i].lGold ;
				        m_GoldView[i].SetGold(m_lTableScore[i]);
				        break;

			        case (int)ANTE_ANIME_ENUM.AA_BASEDEST_TO_CENTER:		// 加注合并到中间
				        //m_lCenterScore += m_JettonStatus[i].lGold;
				        m_CenterGoldView.SetGold(m_lCenterScore);
				        break;

			        case (int)ANTE_ANIME_ENUM.AA_CENTER_TO_BASEFROM:		// 中间筹码移至下注
				        m_CenterGoldView.SetGold(0);
				        break;

			        //default:
				        //ASSERT(FALSE);
			        }

			        //清理信息
			        m_MoveGoldView[i].SetGold(0);
			        m_JettonStatus[i] = new tagJettonStatus();
		        }
	        }

	        if(bAllClean)
	        {
		        m_bJettonAction = false;
	        }

	        //更新界面
	        Invalidate();//UpdateGameView(NULL);

	        return !bAllClean;
        }

        //移动扑克
        void DrawMoveCard( int wViewChairID,int iMoveType,int bCard)
        {
	        //动画步数
	        int nAnimeStep = 45;

	        tagCardStatus CardStatus = new tagCardStatus();
	        //ZeroMemory(&CardStatus,sizeof(CardStatus));

	        CardStatus.wMoveIndex = 0;
	        CardStatus.iMoveType = iMoveType;
	        CardStatus.bCard = bCard;

	        switch( iMoveType )
	        {
	        case TO_USERCARD:				//用户扑克
		        CardStatus.ptFrom = m_SendCardPos;
                CardStatus.ptDest = m_SmallCardControl[wViewChairID].GetOriginPoint();
		        CardStatus.ptCourse= m_SendCardPos;
		        break;

	        case TO_GIVEUP_CARD:			//回收扑克
                CardStatus.ptFrom = m_SmallCardControl[wViewChairID].GetOriginPoint();
		        CardStatus.ptDest = m_SendCardPos;
		        CardStatus.ptCourse =CardStatus.ptFrom;
		        break;

	        case TO_CENTER_CARD:			//中心扑克
		        {
			        m_bCenterCount++;

			        //CImageHandle obj(&m_ImageCard);
			        Size size = new Size(m_ImageCard.Width/13,m_ImageCard.Height/5);
			        int iTemp = size.Width*(m_bCenterCount-1)+25;

			        CardStatus.ptDest.X = iTemp+m_ptCenterCard.X;
			        CardStatus.ptDest.Y = m_ptCenterCard.Y;
			        CardStatus.ptFrom = m_SendCardPos;
			        CardStatus.ptCourse = m_SendCardPos;

			        //声音效果
                    PlayGameSound( Properties.Resources.SEND_CARD );
		        }
		        break;

	        //default:
		        //ASSERT(FALSE);
	        }

	        //位移计算
            Random random = new Random();

	        int nXCount=Math.Abs(CardStatus.ptDest.X-CardStatus.ptFrom.X)/nAnimeStep+random.Next()%4;
	        int nYCount=Math.Abs(CardStatus.ptDest.Y-CardStatus.ptFrom.Y)/nAnimeStep+random.Next()%4;
	        CardStatus.wMoveCount = Math.Max(1,Math.Max(nXCount,nYCount));

	        //加载数据
	        m_CardStatus[wViewChairID].Add(CardStatus);

	        //设置时间
	        if(!m_bJettonAction)
	        {
		        m_bJettonAction=true;
		        SetTimer(IDI_USER_ACTION,TIME_USER_ACTION);
	        }
	        if(!m_bCardAction)m_bCardAction=true;
        }

        //移动扑克
        bool MoveCard()
        {
	        bool bAllClean = true;

	        //设置变量
	        for(int i=0;i<DzCardDefine.GAME_PLAYER+1;i++)
	        {
		        if(m_CardStatus[i].Count==0)
                    continue;

		        tagCardStatus pCardStatus = m_CardStatus[i][0];

		        //移动步数
		        if(pCardStatus.wMoveIndex<pCardStatus.wMoveCount)
		        {
			        bAllClean = false;
			        pCardStatus.wMoveIndex++;
			        int wMoveIndex = pCardStatus.wMoveIndex;
			        int wMoveCount = pCardStatus.wMoveCount;
			        pCardStatus.ptCourse.X =pCardStatus.ptFrom.X + (pCardStatus.ptDest.X-pCardStatus.ptFrom.X)*wMoveIndex/wMoveCount;
			        pCardStatus.ptCourse.Y =pCardStatus.ptFrom.Y + (pCardStatus.ptDest.Y-pCardStatus.ptFrom.Y)*wMoveIndex/wMoveCount;
		        }
		        else if(pCardStatus.wMoveCount>0)
		        {
			        //扑克处理
			        switch( pCardStatus.iMoveType )
			        {
			        case TO_USERCARD:			//用户扑克
				        {
					        //扑克数目
					        int bCount = (int)m_SmallCardControl[i].GetCardCount();
					        //ASSERT(bCount<2);

					        //数据变量
					        int[] bTempCard = new int[DzCardDefine.MAX_COUNT];
					        //ZeroMemory(bTempCard,sizeof(bTempCard));

					        //设置控件
					        if(pCardStatus.bCard>0) 
					        {
						        m_CardControl[i].GetCardData(bTempCard,bCount);
						        bTempCard[bCount] = pCardStatus.bCard;
						        bCount++;
						        m_CardControl[i].SetCardData(bTempCard,bCount);
					        }
					        bCount = (int)m_SmallCardControl[i].GetCardCount()+1;
					        m_SmallCardControl[i].SetCardData(bCount);

					        //清理信息
					        m_CardStatus[i].RemoveAt(0);

					        //控件处理
                            if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                            {
                                if (m_SmallCardControl[m_wMeChairID].GetCardCount() == 2 && m_btGiveUp.Visible == false)
                                {
                                    NotifyMessage(NotifyMessageKind.IDM_SEND_FINISH, 0, 0);
                                }
                            }

					        //声音效果
					        if(m_CardStatus[i].Count>0)
					        {
                                PlayGameSound(Properties.Resources.SEND_CARD);
					        }
				        }

				        break;

			        case TO_GIVEUP_CARD:		//回收扑克

				        //清理信息
				        m_CardStatus[i].RemoveAt(0);

				        break;

			        case TO_CENTER_CARD:		//中心扑克
				        {
					        //扑克数目
					        int bCount = (int)m_CenterCardControl.GetCardCount();
					        //ASSERT(bCount<=MAX_CENTERCOUNT);

					        //
					        int[] bTempCard = new int[DzCardDefine.MAX_CENTERCOUNT];
					        //ZeroMemory(bTempCard,sizeof(bTempCard));

					        //
					        //ASSERT(pCardStatus->bCard>0);

					        if(bCount>0)
                                m_CenterCardControl.GetCardData(bTempCard,bCount);

					        bTempCard[bCount] = pCardStatus.bCard;
					        bCount++;
					        m_CenterCardControl.SetCardData(bTempCard,bCount);

					        //控件处理
					        if(bCount>=3)
					        {
						        NotifyMessage( NotifyMessageKind.IDM_SEND_FINISH,0,0);
					        }

					        //清理信息
					        m_CardStatus[i].RemoveAt(0);

					        //声音效果
					        if(m_CardStatus[i].Count>0)
					        {
                                PlayGameSound(Properties.Resources.SEND_CARD);
					        }
				        }

				        break;
			        }


			        if(m_CardStatus[i].Count>0)
                        bAllClean=false;
			        //ZeroMemory(pCardStatus,sizeof(tagCardStatus));
		        }
	        }

	        if(bAllClean)
	        {
		        m_bCardAction = false;
	        }

	        //更新界面
	        Invalidate();//UpdateGameView(NULL);

	        return !bAllClean;
        }

        //物理位置
        //void SetUserPost(int wPhysicsPost,int wViewPost)
        //{
        //    //设置变量
        //    m_wUserPost[wPhysicsPost] = wViewPost;

        //    return ;
        //}

        int GetMeChairID()
        {
            return this.m_wMeChairID;
        }

        public override void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_TableDetail:
                    {
                        if (!(baseInfo is DzCardInfo))
                            return;

                        KillTimer(IDI_USER_ADD_SCORE);

                        m_ReceiveInfo = (DzCardInfo)baseInfo;

                        m_wMeChairID = GameDefine.INVALID_CHAIR;

                        for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                        {
                            UserInfo userInfo = m_ReceiveInfo.m_Seatter[i];

                            if (userInfo != null && userInfo.Id == m_UserInfo.Id)
                            {
                                m_wMeChairID = i;
                                break;
                            }
                        }

                        //物理位置
                        //for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                        //{
                        //    SetUserPost(i, SwitchViewChairID(i));
                        //}

                        m_lTableScore = m_ReceiveInfo.m_lTableScore;
                        m_lTotalScore = m_ReceiveInfo.m_lTotalScore;

                        SetShowUserView(true);

                        if (this.m_wMeChairID == (int)GameDefine.INVALID_CHAIR)
                        {
                            SetMyLookOn(0);
                        }

                        if (m_Status == DzCardGameStatus.GS_EMPTY)
                            m_Status = (DzCardGameStatus)m_ReceiveInfo._RoundIndex;

                        if (m_Status != (DzCardGameStatus)m_ReceiveInfo._RoundIndex)
                        {
                            SetGameStatus((DzCardGameStatus)m_ReceiveInfo._RoundIndex);
                        }
                        else
                        {
                            switch (m_Status)
                            {
                                case DzCardGameStatus.GS_READY:
                                case DzCardGameStatus.GS_END:
                                    {
                                        //下注信息
                                        //SetTitleInfo(pStatusFree->lCellMinScore, pStatusFree->lCellMaxScore);

                                        //设置控件
                                        //if (IsLookonMode() == false)
                                        //{
                                        //    m_btStart.ShowWindow(SW_SHOW);
                                        //    m_btStart.SetFocus();
                                        //    m_btExit.ShowWindow(SW_SHOW);
                                        //    m_btAutoStart.ShowWindow(SW_SHOW);
                                        //}

                                        //设置时间
                                        //SetGameTimer(GetMeChairID(), IDI_START_GAME, TIME_START_GAME);

                                        //旁观设置
                                        //OnLookonChanged(bLookonOther, NULL, 0);

                                        //return true;
                                    }
                                    break;

                                case DzCardGameStatus.GS_START:
                                    {
                                        //设置变量
                                        //m_wDUser = pStatusPlay->wDUser;
                                        //m_wCurrentUser = pStatusPlay->wCurrentUser;//当前玩家
                                        //CopyMemory(m_lTableScore, pStatusPlay->lTableScore, sizeof(m_lTableScore));//下注数目
                                        //CopyMemory(m_lTotalScore, pStatusPlay->lTotalScore, sizeof(m_lTotalScore));//下注数目
                                        //CopyMemory(m_cbPlayStatus, pStatusPlay->cbPlayStatus, sizeof(m_cbPlayStatus));//用户游戏状态

                                        //加注信息
                                        //m_lAddLessScore = pStatusPlay->lAddLessScore;
                                        //m_lCellScore = pStatusPlay->lCellScore;
                                        //m_lTurnMaxScore = pStatusPlay->lTurnMaxScore;
                                        //m_lTurnLessScore = pStatusPlay->lTurnLessScore;

                                        //总下注数目
                                        int lAllScore = 0;
                                        for (int j = 0; j < DzCardDefine.GAME_PLAYER; j++)
                                        {
                                            lAllScore += m_lTotalScore[j];
                                            lAllScore -= m_lTableScore[j];
                                        }
                                        //ASSERT(lAllScore >= 0);
                                        m_lCenterScore = lAllScore;

                                        //CopyMemory(m_cbHandCardData[GetMeChairID()], pStatusPlay->cbHandCardData, MAX_COUNT);
                                        //CopyMemory(m_cbCenterCardData, pStatusPlay->cbCenterCardData, sizeof(m_cbCenterCardData));

                                        //设置扑克
                                        if (!IsLookonMode())
                                        {
                                            if (m_ReceiveInfo.m_cbPlayStatus[GetMeChairID()] == true)
                                                m_CardControl[GetMeChairID()].SetCardData(m_ReceiveInfo.m_cbHandCardData[GetMeChairID()], DzCardDefine.MAX_COUNT);
                                            
                                            //m_GameClientView.m_btAutoStart.ShowWindow(SW_SHOW);
                                        }

                                        //中心扑克
                                        //if (m_ReceiveInfo.m_cbBalanceCount > 0)
                                        //{
                                        //    int cbTempCount = m_ReceiveInfo.m_cbBalanceCount + 2;
                                        //    m_CenterCardControl.SetCardData(m_ReceiveInfo.m_cbCenterCardData, Math.Min(cbTempCount, DzCardDefine.MAX_CENTERCOUNT));
                                        //}
                                        m_CenterCardControl.SetCardData(m_ReceiveInfo.m_cbCenterCardData, m_ReceiveInfo.m_cbSendCardCount);

                                        //设置界面
                                        for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                                        {
                                            if (m_ReceiveInfo.m_cbPlayStatus[i] == false) 
                                                continue;
                                            
                                            m_SmallCardControl[i].SetCardData(DzCardDefine.MAX_COUNT);
                                            SetUserTableScore(i, m_lTableScore[i]);
                                            SetTotalScore(i, m_lTotalScore[i]);
                                        }

                                        SetDFlag(m_wDUser);
                                        SetTitleInfo(m_ReceiveInfo.m_lTurnLessScore, m_ReceiveInfo.m_lCellScore);
                                        SetCenterScore(m_lCenterScore);

                                        //更新界面
                                        Invalidate();

                                        //当前玩家
                                        if ((IsLookonMode() == false) && (m_ReceiveInfo.m_wCurrentUser == GetMeChairID()))
                                            UpdateScoreControl();

                                        //设置时间
                                        SetGameTimer( IDI_USER_ADD_SCORE, DzCardDefine.TIME_USER_ADD_SCORE);

                                        //旁观设置
                                        //OnLookonChanged(bLookonOther, NULL, 0);

                                        //坐下按钮
                                        //if (IsLookonMode())
                                        //{
                                        //    m_GameClientView.SetMyLookOn(GetMeChairID());

                                        //    //更新界面
                                        //    m_GameClientView.UpdateGameView(NULL);
                                        //}
                                    }
                                    break;
                            }
                        }

                        Invalidate();
                    }
                    break;

                case NotifyType.Reply_AddScore:
                    {
	                    //效验数据
	                    if( !( baseInfo is AddScoreInfo ))
                            return;

                        KillTimer(IDI_USER_ADD_SCORE);
	                    
                        AddScoreInfo addScoreInfo = (AddScoreInfo)baseInfo;

	                    //变量定义
	                    int wAddScoreUser=addScoreInfo.wAddScoreUser;

	                    //加注处理
	                    if ((m_wMeChairID == GameDefine.INVALID_CHAIR)||(addScoreInfo.wAddScoreUser!=m_wMeChairID))
	                    {
		                    //加注界面
		                    if(addScoreInfo.lAddScoreCount>0)
		                    {
			                    DrawMoveAnte( addScoreInfo.wAddScoreUser, (int)ANTE_ANIME_ENUM.AA_BASEFROM_TO_BASEDEST, addScoreInfo.lAddScoreCount);
			                    //m_GameClientView.SetUserTableScore(pAddScore->wAddScoreUser,pAddScore->lAddScoreCount+m_lTableScore[pAddScore->wAddScoreUser]);
			                    m_lTotalScore[addScoreInfo.wAddScoreUser] += addScoreInfo.lAddScoreCount;
			                    SetTotalScore(addScoreInfo.wAddScoreUser,m_lTotalScore[addScoreInfo.wAddScoreUser]);
			                    m_lTableScore[addScoreInfo.wAddScoreUser] +=addScoreInfo.lAddScoreCount;
		                    }

		                    //播放声音
		                    if (m_ReceiveInfo.m_cbPlayStatus[wAddScoreUser]==true)
		                    {
			                    //播放声音
			                    if (addScoreInfo.lAddScoreCount==0) 
				                    PlayGameSound(Properties.Resources.NO_ADD);
			                    else if (addScoreInfo.lAddScoreCount==m_lTurnMaxScore)
				                    PlayGameSound(Properties.Resources.SHOW_HAND);
			                    else if (addScoreInfo.lAddScoreCount==m_lTurnLessScore)
				                    PlayGameSound(Properties.Resources.FOLLOW);
			                    else 
				                    PlayGameSound(Properties.Resources.ADD_SCORE);
		                    }
	                    }

	                    //设置变量
	                    m_ReceiveInfo.m_wCurrentUser= addScoreInfo.wCurrentUser;
	                    m_lTurnLessScore= addScoreInfo.lTurnLessScore;
	                    m_lTurnMaxScore = addScoreInfo.lTurnMaxScore;
	                    m_ReceiveInfo.m_lAddLessScore = addScoreInfo.lAddLessScore;

	                    //控制界面
	                    if ( this.m_wMeChairID != GameDefine.INVALID_CHAIR && m_ReceiveInfo.m_wCurrentUser==this.m_wMeChairID)
	                    {
		                    //ActiveGameFrame();
		                    UpdateScoreControl();
	                    }

	                    //设置时间
	                    if (m_ReceiveInfo.m_wCurrentUser==GameDefine.INVALID_CHAIR) 
	                    {
		                    //一轮平衡
		                    //中心金币累计
		                    for (int i =0;i<DzCardDefine.GAME_PLAYER;i++)
		                    {			
			                    m_lCenterScore += m_lTableScore[i];
		                    }

		                    //筹码移动
		                    for (int i =0;i<DzCardDefine.GAME_PLAYER;i++)
		                    {
			                    if(m_ReceiveInfo.m_cbPlayStatus[i] == false) 
                                    continue;

			                    if(m_lTableScore[i]!=0)
			                    {
				                    //m_GameClientView.SetUserTableScore(i,m_lTableScore[i]);
                                    DrawMoveAnte(i, (int)ANTE_ANIME_ENUM.AA_BASEDEST_TO_CENTER, m_lTableScore[i]);
				                    SetUserTableScore(i,0);
			                    }
		                    }

		                    //m_GameClientView.SetCenterScore(m_lCenterScore);

		                    Array.Clear(m_lTableScore, 0, m_lTableScore.Length);
                            Invalidate();
	                    }
                        else if (m_ReceiveInfo.m_wCurrentUser < DzCardDefine.GAME_PLAYER)
                        {
                            if (m_Status == DzCardGameStatus.GS_START)
                                SetGameTimer(IDI_USER_ADD_SCORE, DzCardDefine.TIME_USER_ADD_SCORE);
                        }
                    }
                    break;

                case NotifyType.Reply_GiveUp:
                    {
	                    //效验数据
                        if(!( baseInfo is AddScoreInfo ))
                            return;

                        KillTimer(IDI_USER_ADD_SCORE);

	                    AddScoreInfo giveInfo = (AddScoreInfo)baseInfo;

	                    //设置变量
	                    m_ReceiveInfo.m_cbPlayStatus[giveInfo.wAddScoreUser]=false;

	                    //界面设置
	                    m_SmallCardControl[giveInfo.wAddScoreUser].SetCardData(null,0);
	                    m_CardControl[giveInfo.wAddScoreUser].SetCardData(null,0);
	                    
                        Invalidate();

	                    DrawMoveCard(giveInfo.wAddScoreUser,TO_GIVEUP_CARD,0);

	                    //状态设置
                        //if ((IsLookonMode()==false)&&(pGiveUp->wGiveUpUser==GetMeChairID()))
                        //    SetGameStatus(GS_FREE);

	                    //变量定义
	                    int wGiveUpUser=giveInfo.wAddScoreUser;

	                    //环境设置
	                    //if (wGiveUpUser==this.m_wMeChairID)
		                //    KillTimer(IDI_USER_ADD_SCORE);

	                    if (m_wMeChairID != GameDefine.INVALID_CHAIR || wGiveUpUser!=m_wMeChairID)
		                    PlayGameSound(Properties.Resources.GIVE_UP);

	                    //显示金币
	                    if (wGiveUpUser==this.m_wMeChairID)
	                    {
		                    if(m_lTableScore[wGiveUpUser]!=0)
		                    {
			                    DrawMoveAnte(wGiveUpUser,(int)ANTE_ANIME_ENUM.AA_BASEDEST_TO_CENTER,m_lTableScore[wGiveUpUser]);
			                    SetUserTableScore(wGiveUpUser,0);
			                    m_lTableScore[wGiveUpUser] = 0;
		                    }

		                    if(this.m_wMeChairID != GameDefine.INVALID_CHAIR)
		                    {
			                    //调整位置
			                    Rectangle rcControl = new Rectangle();
			                    rcControl.Width = m_ScoreView.Width;
                                rcControl.Height = m_ScoreView.Height;
			                    
                                Rectangle rcView = this.RectangleToScreen(this.ClientRectangle);
			                    
                                m_ScoreView.Left = rcView.Left+5;
                                m_ScoreView.Top = rcView.Bottom-15-rcControl.Height*3/2;//rcControl.Width(),rcControl.Height();

			                    m_ScoreView.SetGameScore(wGiveUpUser, giveInfo.lAddLessScore);
			                    m_ScoreView.Visible = true;
			                    m_ScoreView.SetShowTimes();
		                    }
	                    }
	                    else
	                    {
		                    if(m_lTableScore[wGiveUpUser]!=0)
		                    {
			                    DrawMoveAnte(wGiveUpUser,(int)ANTE_ANIME_ENUM.AA_BASEDEST_TO_CENTER,m_lTableScore[wGiveUpUser]);
			                    SetUserTableScore(wGiveUpUser,0);
			                    m_lTableScore[wGiveUpUser] = 0;
		                    }
	                    }

                    }
                    break;

                case NotifyType.Reply_SendCard:
                    {
	                    //校验数据
	                    if( !(baseInfo is SendCardInfo)) 
                            return;

                        KillTimer(IDI_USER_ADD_SCORE);

	                    SendCardInfo sendCardInfo = (SendCardInfo)baseInfo;

	                    //当前玩家
	                    m_ReceiveInfo.m_wCurrentUser = sendCardInfo.wCurrentUser;
	                    m_ReceiveInfo.m_cbCenterCardData = sendCardInfo.cbCenterCardData;

	                    //发送共牌
                        //if((sendCardInfo.cbSendCardCount >= 3)&&(sendCardInfo.cbSendCardCount <= 5)&&(sendCardInfo.cbPublic==0))
                        //{
                        //    //发送共牌 
                        //    if((sendCardInfo.cbSendCardCount == 3))
                        //    {
                        //        for (int j = 0;j<sendCardInfo.cbSendCardCount;j++)
                        //        {
                        //            DrawMoveCard(DzCardDefine.GAME_PLAYER,TO_CENTER_CARD,sendCardInfo.cbCenterCardData[j]);
                        //        }
                        //    }
                        //    else if((sendCardInfo.cbSendCardCount >3))
                        //    {
                        //        int bTemp = sendCardInfo.cbSendCardCount-1;
                        //        DrawMoveCard(DzCardDefine.GAME_PLAYER,TO_CENTER_CARD,sendCardInfo.cbCenterCardData[bTemp]);
                        //    }
                        //}

                        //if((sendCardInfo.cbSendCardCount == 5)&&(sendCardInfo.cbPublic >= 1))
                        //{
                        //    int bFirstCard = sendCardInfo.cbPublic ;
		                    
                        //    if(bFirstCard==1)
                        //        bFirstCard = 0;
                        //    else if(bFirstCard==2)
                        //        bFirstCard = 3;
                        //    else if(bFirstCard==3)
                        //        bFirstCard = 4;

                        //    for (int j = bFirstCard;j<sendCardInfo.cbSendCardCount;j++)
                        //    {
                        //        DrawMoveCard(DzCardDefine.GAME_PLAYER,TO_CENTER_CARD,sendCardInfo.cbCenterCardData[j]);
                        //    }
                        //}

                        for (int j = m_ReceiveInfo.m_cbSendCardCount;j<sendCardInfo.cbSendCardCount;j++)
                        {
                            DrawMoveCard(DzCardDefine.GAME_PLAYER,TO_CENTER_CARD,sendCardInfo.cbCenterCardData[j]);
                        }

                        m_ReceiveInfo.m_cbSendCardCount = sendCardInfo.cbSendCardCount;

	                    //更新界面
                        Invalidate();
	                    //m_GameClientView.UpdateGameView(NULL);
                    }
                    break;

                case NotifyType.Reply_OpenCard:
                    {
	                    //效验数据
	                    if( !(baseInfo is AddScoreInfo)) 
                            return;

                        KillTimer(IDI_USER_ADD_SCORE);

                        AddScoreInfo addScoreInfo = (AddScoreInfo)baseInfo;

	                    int wMeChairID = m_wMeChairID;
	                    UserInfo pUserData= GetUserInfo(wMeChairID);

	                    if(pUserData!=null && m_ReceiveInfo.m_cbPlayStatus[wMeChairID] != true )
	                    {
		                    m_CardControl[addScoreInfo.wAddScoreUser].SetCardData(m_ReceiveInfo.m_cbHandCardData[addScoreInfo.wAddScoreUser],2);
		                    m_CardControl[addScoreInfo.wAddScoreUser].SetDisplayItem(true);
		                    
                            Invalidate();
	                    }
                    }
                    break;
            }
        }

        //更新控制
        void UpdateScoreControl()
        {
	        //显示按钮
	        if(m_lTurnLessScore>0)
	        {
		        m_btPassCard.Visible = false;
		        
                if(m_lTurnLessScore==m_lTurnMaxScore)
                    m_btFollow.Visible = false;
                else
                    m_btFollow.Visible = true;;
	        }
	        else
	        {
		        m_btPassCard.Visible = true;;
		        m_btFollow.Visible = false;;
	        }

	        //要整理
	        if(m_ReceiveInfo.m_lAddLessScore> m_lTurnMaxScore)
	        {
		        m_btAdd.Visible = false;
	        }
	        else
	        {
		        m_btAdd.Visible = true;
	        }

	        m_btGiveUp.Visible = true;
	        m_btShowHand.Visible = true;

	        return;
        }

        //切换椅子
        int SwitchViewChairID(int wChairID)
        {
            return wChairID;

	        //转换椅子
	        int wViewChairID=(wChairID+DzCardDefine.GAME_PLAYER-this.m_wMeChairID);
	        
            switch (DzCardDefine.GAME_PLAYER)
	        {
	        case 2: { wViewChairID+=1; break; }
	        case 3: { wViewChairID+=1; break; }
	        case 4: { wViewChairID+=2; break; }
	        case 5: { wViewChairID+=2; break; }
	        case 6: { wViewChairID+=3; break; }
	        case 7: { wViewChairID+=3; break; }
	        case 8: { wViewChairID+=4; break; }
	        }
	        return wViewChairID%DzCardDefine.GAME_PLAYER;
        }

        private void SetGameStatus(DzCardGameStatus gameStatus)
        {
            if (m_Status != gameStatus)
            {
                m_Status = gameStatus;

                switch (m_Status)
                {
                    case DzCardGameStatus.GS_READY:
                        {
                            ResetGameView();
                        }
                        break;

                    case DzCardGameStatus.GS_START:
                        {
                            SetCenterCount();

                            if (m_wMeChairID == GameDefine.INVALID_CHAIR)
                            {
                                SetGameEndEnd();
                                m_ScoreView.Visible = false;
                                //m_btStart.Visible = false;
                                //m_btExit.Visible = false;
                                m_ScoreView.SetStartTimes(false);

                                //for (int i = 0; i < m_ReceiveInfo.m_cbHandCardData.Length; i++)
                                //    Array.Clear(m_ReceiveInfo.m_cbHandCardData[i], 0, m_ReceiveInfo.m_cbHandCardData[i].Length);

                                //Array.Clear(m_ReceiveInfo.m_cbCenterCardData, 0, m_ReceiveInfo.m_cbCenterCardData.Length);
                                //Array.Clear(m_ReceiveInfo.m_cbPlayStatus, 0, m_ReceiveInfo.m_cbPlayStatus.Length);
                                //Array.Clear(m_ReceiveInfo.m_lTableScore, 0, m_ReceiveInfo.m_lTableScore.Length);

                                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                                {
                                    //SetUserTableScore(i, m_lTableScore[i]);
                                    //SetTotalScore(i, 0);
                                    m_SmallCardControl[i].SetCardData(null, 0);
                                    m_CardControl[i].SetCardData(null, 0);
                                }

                                m_CenterCardControl.SetCardData(null, 0);
                                SetDFlag(GameDefine.INVALID_CHAIR);
                                SetCenterScore(0);
                                m_lCenterScore = 0;
                                //m_ReceiveInfo.m_lCellScore = 0;
                            }

	                        //消息处理
	                        //CMD_S_GameStart * pGameStart=(CMD_S_GameStart *)pBuffer;

	                        //设置变量
                            //m_wDUser = pGameStart->wDUser;
                            //m_wCurrentUser = pGameStart->wCurrentUser;
                            //m_lAddLessScore = pGameStart->lAddLessScore;
                            //m_lTurnLessScore = pGameStart->lTurnLessScore;
                            //m_lTurnMaxScore = pGameStart->lTurnMaxScore;
                            //m_lCellScore = pGameStart->lCellScore;


	                        //用户状态
                            //for (WORD i=0;i<GAME_PLAYER;i++)
                            //{
                            //    //获取用户
                            //    const tagUserData * pUserData=GetUserData(i);
                            //    if (pUserData!=NULL) 
                            //    {
                            //        //游戏信息
                            //        //m_GameClientView.SetUserIdInfo(i,pUserData);
                            //        m_cbPlayStatus[i]=TRUE;
                            //    }
                            //    else 
                            //    {
                            //        //m_GameClientView.SetUserIdInfo(i,NULL);
                            //        m_cbPlayStatus[i]=FALSE;
                            //    }
                            //}

	                        //环境设置
	                        PlayGameSound(Properties.Resources.GAME_START);

	                        //加注信息
                            //m_lTableScore[pGameStart->wDUser] += m_lCellScore;
                            //m_lTableScore[pGameStart->wMaxChipInUser] = 2*m_lCellScore;
                            //m_lTotalScore[pGameStart->wDUser] =  m_lCellScore;
                            //m_lTotalScore[pGameStart->wMaxChipInUser] = 2*m_lCellScore;

	                        //设置界面
	                        SetDFlag(m_ReceiveInfo.m_wDUser);
                            SetTitleInfo(m_ReceiveInfo.m_lTurnLessScore, m_ReceiveInfo.m_lTurnMaxScore);

	                        DrawMoveAnte( m_ReceiveInfo.m_wDUser, (int)ANTE_ANIME_ENUM.AA_BASEFROM_TO_BASEDEST, m_lTotalScore[m_ReceiveInfo.m_wDUser]);

                            int nextUser = GameDefine.INVALID_CHAIR;

                            for (int i = 1; i < DzCardDefine.GAME_PLAYER; i++)
                            {
                                nextUser = (m_ReceiveInfo.m_wDUser + i) % DzCardDefine.GAME_PLAYER;

                                if (m_lTotalScore[nextUser] == m_ReceiveInfo.m_lCellScore * 2)
                                    break;
                            }

                            DrawMoveAnte(nextUser, (int)ANTE_ANIME_ENUM.AA_BASEFROM_TO_BASEDEST, m_lTotalScore[nextUser]);	
	                        
                            //SetTotalScore(m_ReceiveInfo.m_wDUser, m_lTotalScore[m_ReceiveInfo.m_wDUser]);
	                        //pcj-later SetTotalScore(pGameStart->wMaxChipInUser,m_lTotalScore[pGameStart->wMaxChipInUser]);

	                        //发送暗牌
                            //CopyMemory(m_cbHandCardData,pGameStart->cbCardData,sizeof(m_cbHandCardData));

	                        for (int j = 0;j<2;j++)
	                        {
		                        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
		                        {
			                        if (m_ReceiveInfo.m_cbPlayStatus[i]==true)
			                        {
				                        if(this.m_wMeChairID==i)
				                        {
					                        DrawMoveCard(i,TO_USERCARD, m_ReceiveInfo.m_cbHandCardData[i][j]);
				                        }
				                        else 
				                        {
					                        DrawMoveCard(i,TO_USERCARD,0);
				                        }
			                        }
		                        }
	                        }

                        }
                        break;

                    case DzCardGameStatus.GS_END:
                        {
	                        //效验数据
	                        HideScoreControl();

	                        //开牌标志
	                        m_bOpenCard = false;

	                        //桌面金币移至中间
	                        for (int i = 0;i< DzCardDefine.GAME_PLAYER;i++)
	                        {
		                        if((m_lTableScore[i]>0)&&(m_ReceiveInfo.m_cbPlayStatus[i]==true))
		                        {
			                        DrawMoveAnte(i,(int)ANTE_ANIME_ENUM.AA_BASEDEST_TO_CENTER,m_lTableScore[i]);			
			                        SetUserTableScore(i,0);
		                        }
	                        }

	                        //金币信息
	                        //CopyMemory(m_dEndScore,pGameEnd->lGameScore,sizeof(m_dEndScore));
	                        //CopyMemory(m_cbHandCardData,pGameEnd->cbCardData,sizeof(m_cbHandCardData));
	                        //CopyMemory(m_cbOverCardData,pGameEnd->cbLastCenterCardData,sizeof(m_cbOverCardData));

	                        //保存信息
	                        if(m_ReceiveInfo.cbTotalEnd == 1)
	                        {
		                        m_bExitTag = true;
	                        }
	                        else 
	                        {
                                if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                                {
                                    if (m_ReceiveInfo.m_GameScore[m_wMeChairID] > 0)
                                    {
                                        //开牌标志
                                        m_bOpenCard = true;
                                    }
                                }
		                        m_bExitTag = false;
	                        }

	                        //盐时定时器
                            if (IsMoveing())
                            {
                                SetTimer(IDI_GAME_OVER,50);
                            }
                            else
                            {
                                OnGameOver();
                            }

	                        return;
                        }
                        break;
                }
            }
        }

        void SetCenterCount() 
        { 
            m_bCenterCount = 0; 
        }

        bool IsMoveing() 
        { 
            return (m_bJettonAction || m_bCardAction); 
        }

        //游戏结束
        int OnGameOver()
        {
	        //设置状态
	        //SetGameStatus(GS_FREE);

	        if(!m_bExitTag)
	        {
                if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                {
                    if (m_ReceiveInfo.m_cbPlayStatus[m_wMeChairID] == true)
                        m_CardControl[m_wMeChairID].SetCardData(m_ReceiveInfo.m_cbHandCardData[m_wMeChairID], 2);
                }
	        }
	        else
	        {
		        //胜利列表
		        UserWinList WinnerList = new UserWinList();

		        //临时数据
		        int[][] bTempData = new int[DzCardDefine.GAME_PLAYER][];

                for( int i = 0; i < DzCardDefine.GAME_PLAYER; i++ )
                    bTempData[i] = new int[DzCardDefine.MAX_CENTERCOUNT];

                for( int i = 0; i < DzCardDefine.GAME_PLAYER; i++ )
                    for( int k = 0; k < DzCardDefine.MAX_CENTERCOUNT; k++ )
                        bTempData[i][k] = m_ReceiveInfo.m_cbOverCardData[i][k];
		        //CopyMemory(bTempData,m_cbOverCardData,GAME_PLAYER*MAX_CENTERCOUNT);

		        //查找胜利者
		        m_GameLogic.SelectMaxUser(bTempData,WinnerList,null);
		        //ASSERT(WinnerList.bSameCount>0);

		        //设置扑克
		        for (int i = 0;i<DzCardDefine.GAME_PLAYER;i++)
		        {
			        if(m_ReceiveInfo.m_cbPlayStatus[i] == true) 
                        m_CardControl[i].SetCardData(m_ReceiveInfo.m_cbHandCardData[i],2);
			        else 
                        m_CardControl[i].SetCardData(null,0);
		        }

		        //特效变量
		        bool wIsMyWin =false ;
		        int wWinnerID = GameDefine.INVALID_CHAIR;		
		        int[] cbEffectHandCard = new int[DzCardDefine.MAX_COUNT];
		        int[] cbEffectCenterCardData = new int[DzCardDefine.MAX_CENTERCOUNT];
		        //ZeroMemory(cbEffectHandCard,sizeof(cbEffectHandCard));
		        //ZeroMemory(cbEffectCenterCardData,sizeof(cbEffectCenterCardData));
		        int bTempCount1,bTempCount2;

		        //查找胜利扑克
		        for (int i=0;i<WinnerList.bSameCount;i++)
		        {
			        wWinnerID=WinnerList.wWinerList[i];
			        if(!wIsMyWin && m_wMeChairID==WinnerList.wWinerList[i])
			        {
				        wIsMyWin = true;
			        }

			        //查找扑克数据
			        bTempCount1=m_GameLogic.GetSameCard(m_ReceiveInfo.m_cbHandCardData[wWinnerID],bTempData[wWinnerID], DzCardDefine.MAX_COUNT, DzCardDefine.MAX_CENTERCOUNT,cbEffectHandCard);
			        bTempCount2=m_GameLogic.GetSameCard(m_ReceiveInfo.m_cbCenterCardData,bTempData[wWinnerID],DzCardDefine.MAX_CENTERCOUNT,DzCardDefine.MAX_CENTERCOUNT,cbEffectCenterCardData);
			        //ASSERT(bTempCount1+bTempCount2<=MAX_CENTERCOUNT);

			        //设置扑克特效数据
			        m_CardControl[wWinnerID].SetCardEffect(cbEffectHandCard,bTempCount1);
			        m_CenterCardControl.SetCardEffect(cbEffectCenterCardData,bTempCount2);
		        }

		        //自己扑克
                if (!wIsMyWin && m_wMeChairID != GameDefine.INVALID_CHAIR)
		        {
			        wWinnerID = m_wMeChairID;

			        //自己扑克数据
			        Array.Clear(cbEffectHandCard, 0, cbEffectHandCard.Length);
			        Array.Clear(cbEffectCenterCardData, 0, cbEffectCenterCardData.Length);

			        //查找扑克数据
			        bTempCount1=m_GameLogic.GetSameCard(m_ReceiveInfo.m_cbHandCardData[wWinnerID],bTempData[wWinnerID], DzCardDefine.MAX_COUNT, DzCardDefine.MAX_CENTERCOUNT,cbEffectHandCard);
			        bTempCount2=m_GameLogic.GetSameCard(m_ReceiveInfo.m_cbCenterCardData,bTempData[wWinnerID], DzCardDefine.MAX_CENTERCOUNT, DzCardDefine.MAX_CENTERCOUNT,cbEffectCenterCardData);
			        //ASSERT(bTempCount1+bTempCount2<=MAX_CENTERCOUNT);

			        //设置标志扑克数据
			        m_CardControl[wWinnerID].SetMyCard(cbEffectHandCard,bTempCount1);
			        m_CenterCardControl.SetMyCard(cbEffectCenterCardData,bTempCount2);
		        }

		        //游戏结束
		        SetGameEndStart();
	        }

	        //赢金币
	        for (int i =0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        if(m_ReceiveInfo.m_cbPlayStatus[i] == false) 
                    continue;
		        if(m_ReceiveInfo.m_GameScore[i]>0)
		        {
			        m_lCenterScore = m_lCenterScore - m_ReceiveInfo.m_GameScore[i]-m_lTotalScore[i];
			        Invalidate();

			        DrawMoveAnte(i, (int)ANTE_ANIME_ENUM.AA_CENTER_TO_BASEFROM, m_ReceiveInfo.m_GameScore[i]+m_lTotalScore[i]);
			        SetCenterScore(m_lCenterScore);
		        }
		        else if(m_ReceiveInfo.m_GameScore[i] == 0)
		        {
			        DrawMoveAnte(i,(int)ANTE_ANIME_ENUM.AA_CENTER_TO_BASEFROM,m_lTotalScore[i]);
			        m_lCenterScore = m_lCenterScore-m_lTotalScore[i];
			        SetCenterScore(m_lCenterScore);
			        Invalidate();
		        }

		        SetTotalScore(i,0);
		        Invalidate();
	        }

	        //播放声音
	        if (this.m_wMeChairID != GameDefine.INVALID_CHAIR)
	        {
		        if (m_ReceiveInfo.m_GameScore[m_wMeChairID]>=0) 
			        PlayGameSound(Properties.Resources.GAME_WIN1);
		        else 
			        PlayGameSound(Properties.Resources.GAME_LOST);
	        }
	        else 
                PlayGameSound(Properties.Resources.GAME_END);

	        if(m_wMeChairID != GameDefine.INVALID_CHAIR && m_ReceiveInfo.m_cbPlayStatus[m_wMeChairID]==true )
	        {
		        //调整位置
		        Rectangle rcControl = new Rectangle();
		        rcControl.Width = m_ScoreView.Width;
                rcControl.Height = m_ScoreView.Height;

                Rectangle rcView = this.RectangleToScreen(this.ClientRectangle);
		        
		        m_ScoreView.Left = rcView.Left+5;
                m_ScoreView.Top = rcView.Bottom-15-rcControl.Height*3/2;

		        m_ScoreView.SetGameScore(m_wMeChairID, m_ReceiveInfo.m_GameScore[m_wMeChairID]);
		        m_ScoreView.Visible = true;
		        m_ScoreView.SetShowTimes();
	        }

	        if(m_wMeChairID != GameDefine.INVALID_CHAIR)
	        {
		        //开牌按钮
                //if(m_bOpenCard)
                //{
                //    m_btOpenCard.Visible = true;
                //}

		        if (m_ScoreView.Visible == true)
		        {
			        m_ScoreView.SetStartTimes(true);
		        }
                //else if(m_bAutoStart==false)
                //{
                //    SetGameTimer(IDI_GAME_START,DzCardDefine.TIME_START_GAME);
                //}
		        else //自动开始
		        {
			        m_ScoreView.SetStartTimes(true);
			        m_ScoreView.SetShowTimes();
			        OnStart(null,null);
		        }
	        }

	        //状态设置
	        KillTimer(IDI_USER_ADD_SCORE);

	        //开始按钮
            //if (m_wMeChairID != GameDefine.INVALID_CHAIR && m_bAutoStart==false)
            //{
            //    m_btStart.Visible = true;
            //    m_btExit.Visible = true;
            //}

	        //成绩显示在即时聊天对话框
	        string szBuffer = "\n本局结束,成绩统计";
	        //InsertGeneralString(szBuffer,RGB(0,128,255),true);

	        if(m_bReset)
	        {
		        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
		        {
			        if(m_lTotalScore[i]==0)
                        continue;

                    UserInfo pUserData = GetUserInfo(i);

			        //成绩输出
			        if (pUserData!=null)
			        {
                        //_snprintf(szBuffer,CountArray(szBuffer),TEXT("%s：%+ld"),/*%s玩家\n得分:%ld*/
                        //    pUserData->szName,m_dEndScore[i]);
                        //InsertGeneralString(szBuffer,RGB(0,128,255),true);
			        }
			        else
			        {
                        //_snprintf(szBuffer,CountArray(szBuffer),TEXT("用户已离开：%+ld"),-m_lTotalScore[i]);/*\n得分:%ld*/
                        //InsertGeneralString(szBuffer,RGB(0,128,255),true);
			        }
		        }
	        }
	        else	//不足金额
	        {
                //for (WORD i=0;i<GAME_PLAYER;i++)
                //{
                //    if(m_lTotalScore[i]==0)continue;
                //    //成绩输出
                //    if (m_bUserName[i]!=NULL)
                //    {
                //        _snprintf(szBuffer,CountArray(szBuffer),TEXT("%s：%+ld"),
                //            &m_bUserName[i],m_dEndScore[i]);
                //        InsertGeneralString(szBuffer,RGB(0,128,255),true);
                //    }
                //    else
                //    {
                //        _snprintf(szBuffer,CountArray(szBuffer),TEXT("用户已离开：%+ld"),-m_lTotalScore[i]);
                //        InsertGeneralString(szBuffer,RGB(0,128,255),true);
                //    }
                //}
	        }

	        //重值变量
	        Array.Clear(m_lTotalScore, 0, m_lTotalScore.Length);
	        Array.Clear(m_lTableScore, 0, m_lTableScore.Length);
	        m_lCenterScore = 0;

	        return 0;
        }        

        //隐藏控制
        void HideScoreControl()
        {
	        m_GoldControl.Visible = false;
	        m_btAdd.Visible = false;
	        m_btFollow.Visible = false;
	        m_btGiveUp.Visible = false;
	        m_btShowHand.Visible = false;
	        m_btPassCard.Visible = false;

	        return;
        }

        private void OnSitDown(object sender, EventArgs e)
        {
            //隐藏控件
            int userSeat = -1;

            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
            {
                m_btSitDown[i].Visible = false;;

                if (m_btSitDown[i] == sender)
                    userSeat = i;
            }

            //取消状态
            SetMyLookOn(GameDefine.INVALID_CHAIR);

            Invalidate();

            //坐下游戏
            if (userSeat == -1)
                return;

            this.m_UserInfo.userSeat = userSeat;
            m_ClientSocket.Send(NotifyType.Request_PlayerEnter, m_UserInfo);            
        }


        private void OnStart_Click(object sender, EventArgs e)
        {
            ////删除定时器
            //KillTimer(IDI_GAME_START);
            //SetGameEndEnd();
            //m_ScoreView.Visible = false;

            ////设置界面	
            //m_btOpenCard.Visible = false;
            //m_btStart.Visible = false;
            //m_btExit.Visible = false;

            //m_ScoreView.SetStartTimes(false);

            ////Array.Clear(m_cbHandCardData, sizeof(m_cbHandCardData));
            ////Array.Clear(m_cbCenterCardData, sizeof(m_cbCenterCardData));
            ////Array.Clear(m_cbPlayStatus, sizeof(m_cbPlayStatus));
            //Array.Clear(m_lTableScore, 0, m_lTableScore.Length);

            ////加注变量
            ////m_lCenterScore = 0L;
            ////m_lCellScore = 0L;

            ////设置界面
            //for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
            //{
            //    SetUserTableScore(i, m_lTableScore[i]);
            //    SetTotalScore(i, 0);
            //    m_SmallCardControl[i].SetCardData(null, 0);
            //    m_CardControl[i].SetCardData(null, 0);
            //}
            //m_CenterCardControl.SetCardData(null, 0);
            //SetDFlag(GameDefine.INVALID_CHAIR);
            //SetCenterScore(0);

            //发送消息
            //SendUserReady(NULL, 0);

            //return 0;
        }

        protected override void OnGameTimer(TimerInfo timerInfo)
        {
            switch (timerInfo._Id)
            {
                //case IDI_GAME_START:
                //    {
                //        //if (timerInfo.Elapse == 0)
                //        //{
                //        //    if ((this.m_wMeChairIDIsLookonMode() == false) && (wChairID == GetMeChairID()))
                //        //        PostMessage(WM_CLOSE, 0, 0);
                //        //    return true;
                //        //}
                //        if ((timerInfo.Elapse <= 3) && (this.m_wMeChairID != GameDefine.INVALID_CHAIR))
                //            PlayGameSound(Properties.Resources.GAME_WARN);
                //    }
                //    break;

                case IDI_USER_ADD_SCORE:
                    {
                        Invalidate();

                        if (timerInfo.Elapse == 0)
                        {
                            if ((this.m_wMeChairID == m_ReceiveInfo.m_wCurrentUser))
                            {
                                //删除定时器
                                //KillGameTimer(IDI_USER_ADD_SCORE);
                                OnGiveUp(null, null);
                            }
                            return;
                        }

                        if (timerInfo.Elapse <= 3 && this.m_wMeChairID == m_ReceiveInfo.m_wCurrentUser)
                            PlayGameSound(Properties.Resources.GAME_WARN);
                    }
                    break;
            }
        }


        //自动开始
        private void OnAutoStart(object sender, EventArgs e)
        {
            //m_bAutoStart = !m_bAutoStart;

            //int oldWidth = m_btAutoStart.Width;
            //int oldHeight = m_btAutoStart.Height;

            //if(m_bAutoStart==true)
            //{
            //    if(m_btStart.Visible == true)
            //    {
            //        OnStart(null,null);
            //    }
            //    m_btAutoStart.SetButtonImage(Properties.Resources.AUTO_START_TRUE);
            //}
            //else
            //{
            //    UserInfo pUserData = GetUserInfo(this.m_wMeChairID);

            //    if(m_ReceiveInfo.m_cbPlayStatus[this.m_wMeChairID] != true && m_Status != DzCardGameStatus.GS_START &&
            //        m_btStart.Visible == false)
            //    {
            //        m_btStart.Visible = true;
            //        m_btExit.Visible = true;
            //    }
            //    m_btAutoStart.SetButtonImage(Properties.Resources.AUTO_START_FALSE);
            //}

            //m_btAutoStart.Width = oldWidth;
            //m_btAutoStart.Height = oldHeight;

	        return;
        }

        //梭哈消息
        private void OnShowHand(object sender, EventArgs e)
        {
	        //构造数据
	        ShowHand ShowHandObj = new ShowHand();

	        //配置数据
	        if (ShowHandObj.ShowDialog() == DialogResult.OK)
	        {
                int i = 0;

		        for( i=0;i<DzCardDefine.GAME_PLAYER;i++)
		        {
                    UserInfo pUserData = GetUserInfo(i);
			        if(pUserData!=null && m_ReceiveInfo.m_cbPlayStatus[i] == true )
                        break;
		        }

		        if(i<DzCardDefine.GAME_PLAYER)
		        {
			        //删除定时器
			        KillTimer(IDI_USER_ADD_SCORE);

			        //获取筹码
			        int wMeChairID=this.m_wMeChairID;
                    m_lTableScore[wMeChairID] += m_lTurnMaxScore;
                    m_lTotalScore[wMeChairID] += m_lTurnMaxScore;

                    SetTotalScore(wMeChairID, m_lTotalScore[wMeChairID]);
                    SetUserTableScore(wMeChairID, m_lTableScore[wMeChairID]);

                    Invalidate();
			        HideScoreControl();

			        //发送消息
			        AddScoreInfo AddScore = new AddScoreInfo();
			        AddScore.lAddLessScore=m_lTurnMaxScore;

                    m_ClientSocket.Send(NotifyType.Request_AddScore, AddScore);

			        //声音效果
			        PlayGameSound(Properties.Resources.SHOW_HAND);
		        }
	        }

	        return;
        }

        //跟注按钮
        private void OnFollow(object sender, EventArgs e)
        {
	        //删除定时器
	        KillTimer(IDI_USER_ADD_SCORE);

	        //获取筹码
	        int wMeChairID=this.m_wMeChairID;
	        m_lTableScore[wMeChairID] +=m_lTurnLessScore;
	        m_lTotalScore[wMeChairID] += m_lTurnLessScore;
	        SetTotalScore(wMeChairID,m_lTotalScore[wMeChairID]);

	        if(m_lTableScore[wMeChairID]!=0)
                DrawMoveAnte(wMeChairID, (int)ANTE_ANIME_ENUM.AA_BASEFROM_TO_BASEDEST, m_lTurnLessScore);

	        PlayGameSound(Properties.Resources.ADD_SCORE);
	        HideScoreControl();

	        //发送消息
	        AddScoreInfo AddScore = new AddScoreInfo();
	        AddScore.lAddLessScore=m_lTurnLessScore;

            m_ClientSocket.Send(NotifyType.Request_AddScore, AddScore);
            //SendData(SUB_C_ADD_SCORE,&AddScore,sizeof(AddScore));

	        return;
        }

        //让牌消息
        private void OnPassCard( object sender, EventArgs e)
        {
	        //删除定时器
	        KillTimer(IDI_USER_ADD_SCORE);
	        HideScoreControl();

	        //发送消息
            AddScoreInfo AddScore = new AddScoreInfo();
	        AddScore.lAddLessScore=0;

            m_ClientSocket.Send(NotifyType.Request_AddScore, AddScore);

	        //声音效果
	        PlayGameSound( Properties.Resources.NO_ADD);

	        return;
        }

        //加注按钮 
        private void OnAddScore(object sender, EventArgs e)
        {
	        if (m_GoldControl.Visible == false)
	        {
		        m_GoldControl.SetMaxGold(m_lTurnMaxScore);
		        m_GoldControl.SetMinGold( Math.Min(m_ReceiveInfo.m_lAddLessScore,m_lTurnMaxScore));
		        m_GoldControl.SetGold( Math.Min( m_ReceiveInfo.m_lAddLessScore,m_lTurnMaxScore));
		        m_GoldControl.Visible = true;
	        }

	        return; 
        }

        //放弃按钮
        private void OnGiveUp( object sender, EventArgs e)
        {
	        //界面设置
	        KillTimer(IDI_USER_ADD_SCORE);
	        PlayGameSound(Properties.Resources.GIVE_UP);

	        //发送消息
            AddScoreInfo giveInfo = new AddScoreInfo();

            m_ClientSocket.Send(NotifyType.Request_GiveUp, giveInfo);
	        //SendData(SUB_C_GIVE_UP);

	        HideScoreControl();

	        return;
        }

        //离开按钮
        private void OnExit(object sender, EventArgs e)
        {
	        //界面设置
	        //KillGameTimer(IDI_USER_ADD_SCORE);
	        //PlayGameSound(AfxGetInstanceHandle(),TEXT("GIVE_UP"));
        //#ifdef _DEBUG
	        //m_GameClientView.UpdateFrameSize();
	        //return 0;
        //#endif
	        //PostMessage(WM_CLOSE,0,0);

	        return;
        }

        //开始按钮
        private void OnStart(object sender, EventArgs e)
        {
            ////删除定时器
            ////KillTimer(IDI_GAME_START);
            //SetGameEndEnd();
            //m_ScoreView.Visible = false;

            ////设置界面	
            //m_btOpenCard.Visible = false;
            ////m_btStart.Visible = false;
            ////m_btExit.Visible = false;
            //m_ScoreView.SetStartTimes(false);

            //if (m_ReceiveInfo != null)
            //{
            //    Array.Clear(m_ReceiveInfo.m_cbHandCardData, 0, m_ReceiveInfo.m_cbHandCardData.Length);
            //    Array.Clear(m_ReceiveInfo.m_cbCenterCardData, 0, m_ReceiveInfo.m_cbCenterCardData.Length);
            //    Array.Clear(m_ReceiveInfo.m_cbPlayStatus, 0, m_ReceiveInfo.m_cbPlayStatus.Length);
            //}

            //Array.Clear(m_lTableScore, 0, m_lTableScore.Length);

            ////加注变量
            //m_lCenterScore = 0;
            ////m_ReceiveInfo.m_lCellScore = 0;

            ////设置界面
            //for (int i = 0;i< DzCardDefine.GAME_PLAYER;i++)
            //{
            //    SetUserTableScore(i,m_lTableScore[i]);
            //    SetTotalScore(i,0);
            //    m_SmallCardControl[i].SetCardData(null,0);
            //    m_CardControl[i].SetCardData(null,0);
            //}

            //m_CenterCardControl.SetCardData(null,0);
            //SetDFlag(GameDefine.INVALID_CHAIR);
            //SetCenterScore(0);

            ////发送消息
            ////SendUserReady(NULL,0);
            ////m_ClientSocket.Send(NotifyType.Request_Ready, m_UserInfo);

            //return;
        }

        //发牌结束
        int OnSendFinish()
        {
	        //控制界面
	        if (IsLookonMode()==false && m_ReceiveInfo.m_wCurrentUser==this.m_wMeChairID)
	        {
		        //ActiveGameFrame();
		        UpdateScoreControl();
	        }
	        if(m_ReceiveInfo.m_wCurrentUser<DzCardDefine.GAME_PLAYER)
	        {
                if( m_Status == DzCardGameStatus.GS_START )
		            SetGameTimer(IDI_USER_ADD_SCORE,DzCardDefine.TIME_USER_ADD_SCORE);
	        }

	        return 0;
        }

        bool IsLookonMode()
        {
            if (m_wMeChairID == GameDefine.INVALID_CHAIR)
                return true;

            return false;
        }

        //开牌信息
        int OnOpenCard()
        {
	        //隐藏控件
	        m_btOpenCard.Visible = false;

	        //发送消息
            m_ClientSocket.Send(NotifyType.Request_OpenCard, m_UserInfo);
	        //SendData(SUB_C_OPEN_CARD,NULL,0);

	        return 0;
        }

        public override void NotifyMessage(int message, object wParam, object lParam)
        {
            switch (message)
            {
                case NotifyMessageKind.IDM_MIN_SCORE:
                    {
                        //最少筹码
                        m_GoldControl.SetGold( Math.Min(m_ReceiveInfo.m_lAddLessScore, m_lTurnMaxScore));
                    }
                    break;

                case NotifyMessageKind.IDM_MAX_SCORE:
                    {
	                    //最大筹码
	                    m_GoldControl.SetGold(m_lTurnMaxScore);
                    }
                    break;

                case NotifyMessageKind.IDM_OK_SCORE:
                    {
	                    //删除定时器
	                    KillTimer(IDI_USER_ADD_SCORE);
	                    HideScoreControl();

	                    //获取筹码
	                    int wMeChairID=this.m_wMeChairID;
	                    int lCurrentScore=m_GoldControl.GetGold();
	                    m_lTableScore[wMeChairID] += lCurrentScore;
	                    m_lTotalScore[wMeChairID] += lCurrentScore;
	                    SetTotalScore(wMeChairID,m_lTotalScore[wMeChairID]);

	                    if(lCurrentScore>0)
		                    DrawMoveAnte(wMeChairID, (int)ANTE_ANIME_ENUM.AA_BASEFROM_TO_BASEDEST,lCurrentScore);

	                    PlayGameSound( Properties.Resources.ADD_SCORE);

	                    //发送消息
	                    AddScoreInfo AddScore = new AddScoreInfo();
	                    AddScore.lAddLessScore=lCurrentScore;
	                    m_ClientSocket.Send(NotifyType.Request_AddScore, AddScore );

                    }
                    break;

                case NotifyMessageKind.IDM_CANCEL_SCORE:
                    {
	                    m_GoldControl.Visible = false;
                    }
                    break;

                //case NotifyMessageKind.IDM_START_TIMES:
                //    {
                //        //隐藏控件
                //        m_btOpenCard.Visible = false;

                //        if(m_bAutoStart!=true)
                //        {
                //            SetGameTimer(IDI_GAME_START,DzCardDefine.TIME_START_GAME);
                //        }
                //        else //自动开始
                //        {
                //            OnStart(null,null);
                //        }
                //    }
                //    break;

                case NotifyMessageKind.IDM_GAME_OVER:
                    {
                        OnGameOver();
                    }
                    break;

                case NotifyMessageKind.IDM_SEND_FINISH:
                    {
                        OnSendFinish();
                    }
                    break;

                case NotifyMessageKind.IDM_OPEN_CARD:
                    {
                        OnOpenCard();
                    }
                    break;
            }
        }

    }


    //扑克状态
    class tagCardStatus
    {
        //属性信息
        public int wMoveCount;							//移动次数
        public int wMoveIndex;							//移动索引

        //筹码信息
        public Point ptFrom = new Point();								//出发位置
        public Point ptDest = new Point();								//目的位置
        public Point ptCourse = new Point();							//过程位置
        public int bCard;								//扑克数据

        //移动形式
        public int iMoveType;							//移动形式
    };

    //筹码状态
    class tagJettonStatus
    {
        //属性信息
        public int wMoveCount;							//移动次数
        public int wMoveIndex;							//移动索引

        //筹码信息
        public Point ptFrom = new Point();								//出发位置
        public Point ptDest = new Point();								//目的位置
        public Point ptCourse = new Point();							//过程位置
        public int lGold;								//筹码数目

        //移动形式
        public int iMoveType;							//移动形式
    };

    //X 排列方式
    public enum enXCollocateMode
    {
        enXLeft,						//左对齐
        enXCenter,						//中对齐
        enXRight,						//右对齐
    };

    //Y 排列方式
    public enum enYCollocateMode
    {
        enYTop,							//上对齐
        enYCenter,						//中对齐
        enYBottom,						//下对齐
    };

    public class NotifyMessageKind
    {
        public const int IDM_START_TIMES = 107;						//时间消息

        public const int IDM_GAME_OVER = 111;							//结束消息
        public const int IDM_SEND_FINISH = 112;							//发牌结束
        public const int IDM_OPEN_CARD = 113;							//开牌消息

        public const int IDM_MAX_SCORE = 120;						//最大下注
        public const int IDM_MIN_SCORE = 121;						//最小下注	
        public const int IDM_OK_SCORE = 122;						//确定消息
        public const int IDM_CANCEL_SCORE = 123;					//取消消息	
    }

    public enum DzCardGameStatus
    {
        GS_READY = 0,
        GS_START,
        GS_END,

        GS_EMPTY = 100
    }

}
