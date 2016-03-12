using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.Sockets;

using GameControls;
using ChatEngine;

namespace SicboClient
{
    public partial class SicboView : GameView
    {
        //时间标识
        private const int IDI_FLASH_WINNER = 100;									//闪动标识
        private const int IDI_SHOW_CHANGE_BANKER = 101;									//轮换庄家
        private const int IDI_SHOW_CHANGE_SICBO = 102;                                 // 跟换摇骰子方式
        private const int IDI_DISPATCH_CARD = 103;									//发牌标识
        private const int IDI_CHANGE_AZIMUTH = 104;                                 // 更改方位
        private const int IDI_BOMB_EFFECT = 200;									//爆炸标识
        private const int IDI_PLACE_JETTON = 300;									//下注时间


        //按钮标识
        private const int IDC_JETTON_BUTTON_100 = 200;									//按钮标识
        private const int IDC_JETTON_BUTTON_1000 = 201;									//按钮标识
        private const int IDC_JETTON_BUTTON_10000 = 202;									//按钮标识
        private const int IDC_JETTON_BUTTON_100000 = 203;									//按钮标识
        private const int IDC_JETTON_BUTTON_500000 = 204;									//按钮标识
        private const int IDC_JETTON_BUTTON_1000000 = 205;									//按钮标识
        private const int IDC_JETTON_BUTTON_5000000 = 206;									//按钮标识
        private const int IDC_APPY_BANKER = 207;									//按钮标识
        private const int IDC_CANCEL_BANKER = 208;									//按钮标识
        private const int IDC_SCORE_MOVE_L = 209;									//按钮标识
        private const int IDC_SCORE_MOVE_R = 210;									//按钮标识
        private const int IDC_BANKER_MOVE_U = 211;                                 // 移动庄家
        private const int IDC_BANKER_MOVE_D = 212;                                 // 移动庄家

        private const int IDC_BT_SICBO_DESKTOP = 220;									//按钮标识
        private const int IDC_BT_SICBO_INMIDAIR = 221;									//按钮标识
        private const int IDC_BT_SICBO_INTHEAIR = 223;									//按钮标识

        private const int IDC_BT_OPEN_BANK = 230;									//按钮标识
        private const int IDC_TC_PROMPT = 231;									//按钮标识

        //爆炸数目
        private const int BOMB_EFFECT_COUNT = 8;									// 爆炸数目
        private const int SICBO_EFFECT_COUNT = 55;//35								// 摇骰子的数目
        private const int SICBO_EFFECT_FRAME_COUNT = 35;									// 摇骰子的数目
        private const int SICBO_FRAME_COUNT_NORMAL = 10;                                  // 单个骰子使用的帧数

        private const int SHOW_APPLY_BANKER = 6;                                      // 庄家数目
        private const int SICBO_ANIM_FRAME_COUNT = 18;
        private const int JETTON_COUNT = 9;         		    					//筹码数目

        string[] g_szPrompt = new string[]
        {
	        "除围骰外，三个骰子的点\n数和为11点或11点以上",
            "除围骰外，三个骰子的点\n数和为10点或10点以下",
            "除围骰外，三个骰子的点\n数和为奇数",
            "除围骰外，三个骰子的点\n数和为偶数",
            "除围骰外，三个骰子的点\n数和为4",
            "除围骰外，三个骰子的点\n数和为5",
            "除围骰外，三个骰子的点\n数和为6",
            "除围骰外，三个骰子的点\n数和为7",
            "除围骰外，三个骰子的点\n数和为8",
            "除围骰外，三个骰子的点\n数和为9",
            "除围骰外，三个骰子的点\n数和为10",
            "除围骰外，三个骰子的点\n数和为11",
            "除围骰外，三个骰子的点\n数和为12",
            "除围骰外，三个骰子的点\n数和为13",
            "除围骰外，三个骰子的点\n数和为14",
            "除围骰外，三个骰子的点\n数和为15",
            "除围骰外，三个骰子的点\n数和为16",
            "除围骰外，三个骰子的点\n数和为17",
            "三个骰子中出现1点的骰\n子，如果有2个则是双骰，\n3个则是3筛",
            "三个骰子中出现2点的骰\n子，如果有2个则是双骰，\n3个则是3筛",
            "三个骰子中出现3点的骰\n子，如果有2个则是双骰，\n3个则是3筛",
            "三个骰子中出现4点的骰\n子，如果有2个则是双骰，\n3个则是3筛",
            "三个骰子中出现5点的骰\n子，如果有2个则是双骰，\n3个则是3筛",
            "三个骰子中出现6点的骰\n子，如果有2个则是双骰，\n3个则是3筛",
            "除围骰外，三个骰子中同\n时存在1点和2点",
            "除围骰外，三个骰子中同\n时存在1点和3点",
            "除围骰外，三个骰子中同\n时存在1点和4点",
            "除围骰外，三个骰子中同\n时存在1点和5点",
            "除围骰外，三个骰子中同\n时存在1点和6点",
            "除围骰外，三个骰子中同\n时存在2点和3点",
            "除围骰外，三个骰子中同\n时存在2点和4点",
            "除围骰外，三个骰子中同\n时存在2点和5点",
            "除围骰外，三个骰子中同\n时存在2点和6点",
            "除围骰外，三个骰子中同\n时存在3点和4点",
            "除围骰外，三个骰子中同\n时存在3点和5点",
            "除围骰外，三个骰子中同\n时存在3点和6点",
            "除围骰外，三个骰子中同\n时存在4点和5点",
            "除围骰外，三个骰子中同\n时存在4点和6点",
            "除围骰外，三个骰子中同\n时存在5点和6点",
            "除围骰外，三个骰子中同\n时出现两个1点",
            "除围骰外，三个骰子中同\n时出现两个2点",
            "除围骰外，三个骰子中同\n时出现两个3点",
            "除围骰外，三个骰子中同\n时出现两个4点",
            "除围骰外，三个骰子中同\n时出现两个5点",
            "除围骰外，三个骰子中同\n时出现两个6点",
            "三个骰子的点数都是1",
            "三个骰子的点数都是2",
            "三个骰子的点数都是3",
            "三个骰子的点数都是4",
            "三个骰子的点数都是5",
            "三个骰子的点数都是6",
            "三个骰子的点数相同"
        };



        public GameStatus m_Status = GameStatus.GS_EMPTY;

        //限制信息
        int m_lMeMaxScore;						// 最大下注
        int m_lAllMaxScore;                     // 当局可以下注的总注
        int m_lAreaLimitScore;					//区域限制

        //下注信息
        int[] m_lMeScore = new int[SicboDefine.COUNT_AZIMUTH];	        //每个方位总注

        //全体下注
        int[] m_lAllScore = new int[SicboDefine.COUNT_AZIMUTH];			//每个方位总注

        //位置信息
        int m_nWinFlagsExcursionX;				//偏移位置
        int m_nWinFlagsExcursionY;				//偏移位置
        Point m_ptApplyListPos = new Point();
        int m_nScoreHead;						//成绩位置
        Rectangle[] m_rcUser = new Rectangle[SicboDefine.COUNT_AZIMUTH];			//玩家下注区域

        //扑克信息
        int[] m_enCards = new int[SicboDefine.MAX_COUNT_SICBO];         // 骰子点数
        int[] m_bWinner = new int[SicboDefine.COUNT_AZIMUTH];           // 输赢结果

        //动画变量
        Point[] m_ptSicboAnim = new Point[SicboDefine.SICBOTYPE_COUNT];         // 骰子动画坐标(每种摇骰子的方式坐标不同)
        tagSicboAnim[] m_SicboAnimInfo = new tagSicboAnim[SicboDefine.MAX_COUNT_SICBO];       // 正常、与空中摇骰子的动画信息
        byte[] m_bySicboDraw = new byte[SicboDefine.MAX_COUNT_SICBO];         // 画骰子的顺序
        bool m_bSicboShow;                           // 动画完成后显示结果
        bool m_bSicboEffect;		                    // 摇骰子效果
        byte m_bySicboFrameIndex;                    // 当前帧
        bool[] m_bBombEffect = new bool[SicboDefine.COUNT_AZIMUTH];			// 爆炸效果
        byte[] m_cbBombFrameIndex = new byte[SicboDefine.COUNT_AZIMUTH];		// 帧数索引

        //历史信息
        int m_lMeStatisticScore;				  // 游戏成绩
        //tagClientGameRecord[]			m_GameRecordArrary = new tagClientGameRecord[SicboDefine.MAX_SCORE_HISTORY];// 游戏记录
        int m_nRecordFirst;						  // 开始记录
        int m_nRecordLast;						  // 最后记录
        List<UserInfo> m_ApplyUserArray = new List<UserInfo>();                     // 申请庄家队列
        int m_iApplyFirst;                        // 重第几个开始显示

        //状态变量
        int m_wMeChairID;						//我的位置
        byte m_cbWinnerSide;						//胜利玩家
        byte m_cbAreaFlash;						//胜利玩家
        int m_lCurrentJetton;					//当前筹码
        string m_strDispatchCardTips;				//发牌提示
        bool m_bShowChangeBanker;				//轮换庄家
        bool m_bShowChangeSicbo;				    // 改变摇骰子方式

        //庄家信息
        int m_wBankerUser;						//当前庄家
        int m_wBankerTime;						//做庄次数
        int m_lBankerScore;						//庄家积分
        int m_lTmpBankerWinScore;				//庄家成绩	
        bool m_bEnableSysBanker;					//系统做庄
        E_CARD_TYPE m_enSelArea;                        // 选择区域
        int m_lSelAreaMax;                      // 选择区域可下分

        //当局成绩
        int m_lMeCurGameScore;					//我的成绩
        int m_lMeCurGameReturnScore;			//我的成绩
        int m_lBankerCurGameScore;				//庄家成绩
        int m_lGameRevenue;						//游戏税收
        int m_lBankerWinScore;					//庄家成绩

        //bettintTotalmoney
        int m_bettingTotalMoney;
        int m_bettingChipMoney;

        //数据变量
        Point[] m_PointJetton = new Point[SicboDefine.COUNT_AZIMUTH];					//筹码位置
        Point[] m_PointJettonNumber = new Point[SicboDefine.COUNT_AZIMUTH];				//数字位置
        List<tagJettonInfo>[] m_JettonInfoArray = new List<tagJettonInfo>[SicboDefine.COUNT_AZIMUTH];				//筹码数组

        //控件变量
        int m_enSicboType;                      // 摇骰子类型						//筹码按钮						//筹码按钮					//筹码按钮					//筹码按钮					//筹码按钮					//筹码按钮					//筹码按钮

        PictureButton m_btJetton100 = new PictureButton();
        PictureButton m_btJetton1000 = new PictureButton();
        PictureButton m_btJetton10000 = new PictureButton();
        PictureButton m_btJetton100000 = new PictureButton();
        PictureButton m_btJetton500000 = new PictureButton();
        PictureButton m_btJetton1000000 = new PictureButton();
        PictureButton m_btJetton5000000 = new PictureButton();

        PictureButton m_btApplyBanker = new PictureButton();					//申请庄家
        PictureButton m_btCancelBanker = new PictureButton();					//取消庄家
        PictureButton m_btBankerMoveUp = new PictureButton();					//移动庄家
        PictureButton m_btBankerMoveDown = new PictureButton();					//移动庄家

        Button m_btScoreMoveL;						//移动成绩
        Button m_btScoreMoveR;						//移动成绩

        Button m_btSicboDesktop;                    // 桌面摇骰子
        Button m_btSicboInMidair;                   // 半空摇骰子
        Button m_btSicboInTheAir;                   // 空中摇骰子

        Button m_btOpenBank;                       // 银行

        //控件变量
        //CGameClientDlg					*m_pGameClientDlg;					//父类指针
        //CGameLogic						m_GameLogic;						//游戏逻辑

        //界面变量
        Bitmap m_ImageViewFill;					//背景位图
        Bitmap m_ImageViewBack;					//背景位图
        Bitmap m_ImageWinFlags;					//标志位图
        Bitmap m_ImageCardDot;					    //骰子点数
        Bitmap m_ImageJettonView;					//筹码视图
        Bitmap m_ImageScoreNumber;					//数字视图
        Bitmap m_ImageMeScoreNumber;				//数字视图
        Bitmap m_ImageTimeFlag;					//时间标识
        Bitmap m_ImageBombEffect;					//动画图片
        Bitmap m_ImgSicboEffectDesktop;			//动画图片
        Bitmap m_ImgSicboEffectInMidair;			//动画图片
        Bitmap m_ImgSicboEffectInTheAir;			//动画图片
        Bitmap m_ImgSicboEffectNormal;			    //动画图片
        Bitmap m_ImgSicboEffectResult;			    //动画图片
        Bitmap m_ImgSicboEffectBack;			    //动画图片
        Bitmap m_ImgSicboGiftInTheAir;			    //动画图片
        bool m_bShowSicboEffectBack;             // 是否显示动画背景
        Bitmap[] m_PngSicboArea = new Bitmap[SicboDefine.COUNT_AZIMUTH];		// 每个区域鼠标移动时的背景图


        Bitmap m_ImgSicboEPlan;			        //动画图片
        Bitmap m_ImgSicboPlan;			            //动画图片
        Bitmap m_ImgSicboNum;			            //动画图片
        Bitmap[] m_ImgSicboAnim = new Bitmap[SICBO_ANIM_FRAME_COUNT]; //动画图片

        //边框
        Bitmap m_ImageFrameWin;		//边框图片
        Bitmap m_ImageMeBanker;					//切换庄家
        Bitmap m_ImageChangeBanker;				//切换庄家
        Bitmap m_ImageNoBanker;					//切换庄家

        Bitmap m_ImageSicboDesktop;                 // 桌面摇骰子
        Bitmap m_ImageSicboInMidair;                // 半空摇骰子
        Bitmap m_ImageSicboInTheAir;                // 空中摇骰子

        //结束资源
        Bitmap m_PngGameEnd;						//成绩图片
        Bitmap m_ImageCardType;					//牌型图片
        Bitmap m_ImageSicboPoint;					//牌点数图片
        //CMutex                          m_ApplyBankerLock;
        //CTransparentCtrl                m_TCPrompt;

        UnmanagedMemoryStream[] m_DTSDCheer = new UnmanagedMemoryStream[3];

        Brush textBrush = new SolidBrush(Color.FromArgb(255, 234, 0));
        int m_BettingTimeLeave;
        System.Media.SoundPlayer m_BackPlayer = null;
        SicboInfo m_ReceiveInfo = null;

        public SicboView()
        {
            InitializeComponent();

            for (int i = 0; i < m_JettonInfoArray.Length; i++)
                m_JettonInfoArray[i] = new List<tagJettonInfo>();

            //加载位图
            //HINSTANCE hInstance=AfxGetInstanceHandle();
            m_ImageViewFill = Properties.Resources.VIEW_FILL;
            m_ImageViewBack = Properties.Resources.VIEW_BACK;
            m_ImageWinFlags = Properties.Resources.WIN_FLAGS;
            m_ImageScoreNumber = Properties.Resources.SCORE_NUMBER;
            m_ImageMeScoreNumber = Properties.Resources.ME_SCORE_NUMBER;
            m_ImageCardDot = Properties.Resources.SICBO_NUMBER_DOT;
            m_ImageCardType = Properties.Resources.CARD_TYPE;

            m_ImageMeBanker = Properties.Resources.ME_BANKER;
            m_ImageChangeBanker = Properties.Resources.CHANGE_BANKER;
            m_ImageNoBanker = Properties.Resources.NO_BANKER;

            m_ImageTimeFlag = Properties.Resources.TIME_FLAG;
            m_ImageSicboPoint = Properties.Resources.SICBO_POINT;

            m_ImageBombEffect = Properties.Resources.FIRE_EFFECT;
            m_ImgSicboEffectNormal = Properties.Resources.SICBO_EFFECT_NORMAL;
            m_ImgSicboEffectDesktop = Properties.Resources.SICBO_EFFECT_DESKTOP;
            m_ImgSicboEffectInMidair = Properties.Resources.SICBO_EFFECT_INMIDAIR;
            m_ImgSicboEffectInTheAir = Properties.Resources.SICBO_IN_THE_AIR;
            m_ImgSicboEffectResult = Properties.Resources.SICBO_EFFECT_RESULT;
            m_ImgSicboEffectBack = Properties.Resources.SICBO_EFFECT_BACK;
            m_ImgSicboGiftInTheAir = Properties.Resources.GIFT_INTHEAIR;

            m_ImgSicboEPlan = Properties.Resources.SICBO_EPLAN;			//动画图片
            m_ImgSicboPlan = Properties.Resources.SICBO_PLAN;			    //动画图片
            m_ImgSicboNum = Properties.Resources.SICBO_NUM;			    //动画图片
            m_PngGameEnd = Properties.Resources.GAME_END;
            m_ImageJettonView = Properties.Resources.JETTOM_VIEW1;
            m_ImageFrameWin = Properties.Resources.FRAME_WIN;

            for (int i = 0; i < SicboDefine.COUNT_AZIMUTH; i++)
                m_PngSicboArea[i] = (Bitmap)Properties.Resources.ResourceManager.GetObject(string.Format("one_{0:D2}", i));

            for (int i = 0; i < SICBO_ANIM_FRAME_COUNT; i++)
                m_ImgSicboAnim[i] = (Bitmap)Properties.Resources.ResourceManager.GetObject(string.Format("Ani_{0}", i + 1));

            m_ImageSicboDesktop = Properties.Resources.SICBO_DESKTOP;                // 桌面摇骰子
            m_ImageSicboInMidair = Properties.Resources.SICBO_EFFECT_INMIDAIR;             // 半空摇骰子
            m_ImageSicboInTheAir = Properties.Resources.SICBO_IN_THE_AIR;            // 空中摇骰子
            m_bSicboShow = false;
            m_lSelAreaMax = 0;

            m_bettingTotalMoney = 0;


            ////创建控件
            Rectangle rcCreate = new Rectangle();

            //下注按钮
            int controlCount = this.Controls.Count;

            m_btJetton100.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_100);
            m_btJetton100.Click += m_btJetton100_Click;

            m_btJetton1000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_1000);
            m_btJetton1000.Click += m_btJetton1000_Click;

            m_btJetton10000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_10000);
            m_btJetton10000.Click += m_btJetton10000_Click;

            m_btJetton100000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_100000);
            m_btJetton100000.Click += m_btJetton100000_Click;

            m_btJetton500000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_500000);
            m_btJetton500000.Click += m_btJetton500000_Click;

            m_btJetton1000000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_1000000);
            m_btJetton1000000.Click += m_btJetton1000000_Click;

            m_btJetton5000000.Create(true, false, rcCreate, this, IDC_JETTON_BUTTON_5000000);
            m_btJetton5000000.Click += m_btJetton5000000_Click;

            controlCount = this.Controls.Count;

            ////申请按钮
            //m_btApplyBanker.Create(true, true, rcCreate, this, IDC_APPY_BANKER);
            //m_btApplyBanker.Click += btApplyBanker_Click;

            //m_btCancelBanker.Create(false, true, rcCreate, this, IDC_CANCEL_BANKER);
            //m_btScoreMoveL.Create(true, false, rcCreate, this, IDC_SCORE_MOVE_L);
            //m_btScoreMoveR.Create(true, false, rcCreate, this, IDC_SCORE_MOVE_R);

            m_btBankerMoveUp.Create(true, false, rcCreate, this, IDC_BANKER_MOVE_U);
            m_btBankerMoveDown.Create(true, false, rcCreate, this, IDC_BANKER_MOVE_D);

            //m_btSicboDesktop.Create(true, false, rcCreate, this, IDC_BT_SICBO_DESKTOP);                     // 桌面摇骰子
            //m_btSicboInMidair.Create(true, false, rcCreate, this, IDC_BT_SICBO_INMIDAIR);                   // 半空摇骰子
            //m_btSicboInTheAir.Create(true, false, rcCreate, this, IDC_BT_SICBO_INTHEAIR);                   // 空中摇骰子

            //m_btSicboDesktop.ShowWindow(SW_HIDE);
            //m_btSicboInMidair.ShowWindow(SW_HIDE);
            //m_btSicboInTheAir.ShowWindow(SW_HIDE);

            //m_btOpenBank.Create(NULL, WS_CHILD | WS_VISIBLE, rcCreate, this, IDC_BT_OPEN_BANK);                   // 空中摇骰子

            ////设置按钮
            //HINSTANCE hResInstance=AfxGetInstanceHandle();
            m_btJetton100.SetButtonImage(Properties.Resources.BT_JETTON_100);
            m_btJetton1000.SetButtonImage(Properties.Resources.BT_JETTON_1000);
            m_btJetton10000.SetButtonImage(Properties.Resources.BT_JETTON_10000);
            m_btJetton100000.SetButtonImage(Properties.Resources.BT_JETTON_100000);
            m_btJetton500000.SetButtonImage(Properties.Resources.BT_JETTON_500000);
            m_btJetton1000000.SetButtonImage(Properties.Resources.BT_JETTON_1000000);
            m_btJetton5000000.SetButtonImage(Properties.Resources.BT_JETTON_5000000);

            //m_btApplyBanker.SetButtonImage(Properties.Resources.BT_APPLY_BANKER);
            //m_btCancelBanker.SetButtonImage(Properties.Resources.BT_CANCEL_BANKER);

            //m_btScoreMoveL.SetButtonImage(IDB_BT_SCORE_MOVE_L, hResInstance, false);
            //m_btScoreMoveR.SetButtonImage(IDB_BT_SCORE_MOVE_R, hResInstance, false);

            m_btBankerMoveUp.SetButtonImage(Properties.Resources.BT_UP);
            m_btBankerMoveDown.SetButtonImage(Properties.Resources.BT_DOWN);

            //m_btSicboDesktop.SetButtonImage(IDB_BT_SICBO_DESKTOP, hResInstance, false);
            //m_btSicboInMidair.SetButtonImage(IDB_BT_SICBO_INMIDAIR, hResInstance, false);
            //m_btSicboInTheAir.SetButtonImage(IDB_BT_SICBO_INTHEAIR, hResInstance, false);
            //m_btOpenBank.SetButtonImage(IDB_OPEN_BANK, hResInstance, false);
            //m_TCPrompt.Create(NULL, WS_CHILD | WS_VISIBLE | SS_CENTERIMAGE, rcCreate, this, IDC_TC_PROMPT);  // 提示
            //m_TCPrompt.ShowWindow(SW_HIDE);	
            //return 0;

            //m_TCPrompt.m_PngBack = Properties.Resources.USER_PROMPT;

            m_DTSDCheer[0] = Properties.Resources.CHEER1;
            m_DTSDCheer[1] = Properties.Resources.CHEER2;
            m_DTSDCheer[2] = Properties.Resources.CHEER3;
        }


        private void SicboView_Load(object sender, EventArgs e)
        {
            ResetGameView();

            SetGameStatus(GameStatus.GS_EMPTY);
        }

        private void ResetGameView()
        {
            //个人下注
            Array.Clear(m_lMeScore, 0, m_lMeScore.Length);

            //全体下注
            Array.Clear(m_lAllScore, 0, m_lAllScore.Length);

            //庄家信息
            //m_wBankerUser = INVALID_CHAIR;
            m_wBankerTime = 0;
            m_lBankerScore = 0;
            m_lBankerWinScore = 0;
            m_lTmpBankerWinScore = 0;

            //当局成绩
            m_lMeCurGameScore = 0;
            m_lMeCurGameReturnScore = 0;
            m_lBankerCurGameScore = 0;
            m_lGameRevenue = 0;

            //动画变量
            Array.Clear(m_bBombEffect, 0, m_bBombEffect.Length);
            Array.Clear(m_cbBombFrameIndex, 0, m_cbBombFrameIndex.Length);
            m_bSicboEffect = false;                             // 摇骰子效果
            m_bySicboFrameIndex = 0;
            Array.Clear(m_SicboAnimInfo, 0, m_SicboAnimInfo.Length);
            Array.Clear(m_bySicboDraw, 0, m_bySicboDraw.Length);
            Array.Clear(m_enCards, 0, m_enCards.Length); ;
            Array.Clear(m_bWinner, 0, m_bWinner.Length);   // 输赢结果

            //状态信息
            m_lCurrentJetton = 0;
            m_cbWinnerSide = 0xFF;
            m_cbAreaFlash = 0xFF;
            m_wMeChairID = GameDefine.INVALID_CHAIR;
            m_bShowChangeBanker = false;
            m_bShowChangeSicbo = false;
            m_bShowSicboEffectBack = false;
            m_bSicboShow = false;

            m_lMeCurGameScore = 0;
            m_lMeCurGameReturnScore = 0;
            m_lBankerCurGameScore = 0;

            m_lAreaLimitScore = 0;

            //位置信息
            m_nScoreHead = 0;
            m_nRecordFirst = 0;
            m_nRecordLast = 0;

            //历史成绩
            //m_lMeStatisticScore = 0;

            //清空列表
            //m_ApplyUserArray.Clear();
            m_iApplyFirst = 0;
            m_lSelAreaMax = 0;

            //清除桌面
            CleanUserJetton();

            m_enSelArea = E_CARD_TYPE.enCardType_Illegal;

            //设置按钮
            //m_btApplyBanker.Visible = true;
            //m_btApplyBanker.Enabled = false;
            //m_btCancelBanker.Visible = false;

            return;
        }


        void RectifyGameView(int nWidth, int nHeight)
        {
            //区域位置
            int nCenterX = nWidth / 2, nCenterY = nHeight / 2;

            //位置信息
            m_nWinFlagsExcursionX = nCenterX - 234;
            m_nWinFlagsExcursionY = nCenterY + 263;

            // 52 个区域
            int iX = nCenterX + 223;
            int iY = nCenterY - 233;
            m_rcUser[(int)E_CARD_TYPE.enCardType_Big] = new Rectangle(iX, iY, 141, 156);
            iX = nCenterX - 365;
            m_rcUser[(int)E_CARD_TYPE.enCardType_Small] = new Rectangle(iX, iY, 141, 156);
            iY = nCenterY + 110;
            m_rcUser[(int)E_CARD_TYPE.enCardType_Single] = new Rectangle(iX, iY, 140, 102);
            iX = nCenterX + 224;
            m_rcUser[(int)E_CARD_TYPE.enCardType_Double] = new Rectangle(iX, iY, 140, 102);
            int iW = 53;
            int iH = 99;
            iX = nCenterX - 365;
            iY = nCenterY - 78;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberFour] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 313;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberFive] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 261;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberSix] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 209;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberSeven] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 157;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberEight] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 105;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberNine] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 53;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberTen] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 1;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberEleven] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 51;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberTwelve] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 103;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberThirteen] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 155;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberFourteen] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 207;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberFifteen] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 259;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberSixteen] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 311;
            m_rcUser[(int)E_CARD_TYPE.enCardType_NumberSeventeen] = new Rectangle(iX, iY, iW, iH);
            //////////////////////////////////////////////////////////////////////////
            iX = nCenterX - 226;
            iY = nCenterY + 110;
            iW = 76;
            iH = 54;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboOne] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 151;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboTwo] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 76;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThree] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 1;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboFour] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 74;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboFive] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 149;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboSix] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 361;
            iY = nCenterY + 20;
            iW = 49;
            iH = 91;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboOneAndTwo] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 313;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboOneAndThree] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 265;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboOneAndFour] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 217;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboOneAndFive] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 169;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboOneAndSix] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 121;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboTwoAndThree] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 73;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboTwoAndFour] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 25;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboTwoAndFive] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 23;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboTwoAndSix] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 71;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeAndFour] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 119;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeAndFive] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 167;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeAndSix] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 215;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboFourAndFive] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 263;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboFourAndSix] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 311;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboFiveAndSix] = new Rectangle(iX, iY, iW, iH);
            iW = 78;
            iH = 46;
            iX = nCenterX - 225;
            iY = nCenterY - 213;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboDoubleOne] = new Rectangle(iX, iY, iW, iH);
            iY = nCenterY - 168;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboDoubleTwo] = new Rectangle(iX, iY, iW, iH);
            iY = nCenterY - 123;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboDoubleThree] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 146;
            iY = nCenterY - 213;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboDoubleFour] = new Rectangle(iX, iY, iW, iH);
            iY = nCenterY - 168;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboDoubleFive] = new Rectangle(iX, iY, iW, iH);
            iY = nCenterY - 123;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboDoubleSix] = new Rectangle(iX, iY, iW, iH);

            iW = 50;
            iH = 46;
            iX = nCenterX - 148;
            iY = nCenterY - 213;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeOne] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 99;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeTwo] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 50;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeThree] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX - 1;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeFour] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 48;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeFive] = new Rectangle(iX, iY, iW, iH);
            iX = nCenterX + 97;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeSix] = new Rectangle(iX, iY, iW, iH);

            iW = 295;
            iH = 91;
            iX = nCenterX - 148;
            iY = nCenterY - 168;
            m_rcUser[(int)E_CARD_TYPE.enCardType_SicboThreeSame] = new Rectangle(iX, iY, iW, iH);

            // 背景中心点
            m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Normal] = new Point(nWidth / 2, nHeight / 2 - 30);
            m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Desktop] = new Point(nWidth / 2 - 245, nHeight / 2 + 95);
            m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_InMidair] = new Point(nWidth / 2 - 245, nHeight / 2 + 95);
            m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_InTheAir] = new Point(nWidth / 2, nHeight / 2 - 40);
            m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo] = new Point(nWidth / 2 + 15, nHeight / 2 - 100);

            m_ptApplyListPos = new Point(nWidth / 2 - 110, nHeight / 2 - 333);

            int ExcursionY = 10;
            for (byte i = 0; i < SicboDefine.COUNT_AZIMUTH; ++i)
            {
                //筹码数字
                m_PointJettonNumber[i] = new Point((m_rcUser[i].Right  + m_rcUser[i].Left ) / 2, (m_rcUser[i].Bottom  + m_rcUser[i].Top ) / 2 - ExcursionY);

                //筹码位置
                m_PointJetton[i] = new Point((m_rcUser[i].Left + (m_rcUser[i].Right  - m_rcUser[i].Left ) / 2), (m_rcUser[i].Top  + (m_rcUser[i].Bottom  - m_rcUser[i].Top ) / 2));
            }

            //移动控件
            IntPtr hDwp = GameGraphics.BeginDeferWindowPos(32);
            const uint uFlags = GameGraphics.SWP_NOACTIVATE | GameGraphics.SWP_NOZORDER | GameGraphics.SWP_NOCOPYBITS;

            //筹码按钮
            m_btJetton100.Width = 60;
            m_btJetton1000.Width = 60;
            m_btJetton10000.Width = 60;
            m_btJetton100000.Width = 60;
            m_btJetton500000.Width = 60;
            m_btJetton1000000.Width = 60;
            m_btJetton5000000.Width = 60;

            m_btJetton100.Height = 60;
            m_btJetton1000.Height = 60;
            m_btJetton10000.Height = 60;
            m_btJetton100000.Height = 60;
            m_btJetton500000.Height = 60;
            m_btJetton1000000.Height = 60;
            m_btJetton5000000.Height = 60;

            //m_btApplyBanker.Width = 82;
            //m_btCancelBanker.Width = 82;

            //m_btApplyBanker.Height = 20;
            //m_btCancelBanker.Height = 20;

            int buttonWidth = 60;// m_btJetton100.Width;
            int buttonHeight = 60;// m_btJetton100.Height;

            int nYPos = nHeight / 2 + 194;
            int nXPos = nWidth / 2 - 218;
            int nSpace = 3;

            GameGraphics.DeferWindowPos(hDwp, m_btJetton100.Handle, 0, nXPos, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton1000.Handle, 0, nXPos + nSpace + buttonWidth, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton10000.Handle, 0, nXPos + nSpace * 2 + buttonWidth * 2, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton100000.Handle, 0, nXPos + nSpace * 3 + buttonWidth * 3, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton500000.Handle, 0, nXPos + nSpace * 4 + buttonWidth * 4, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton1000000.Handle, 0, nXPos + nSpace * 5 + buttonWidth * 5, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btJetton5000000.Handle, 0, nXPos + nSpace * 6 + buttonWidth * 6, nYPos, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);

            //上庄按钮
            //DeferWindowPos(hDwp, m_btApplyBanker.Handle, 0, nWidth / 2 + 271, nHeight / 2 - 266, 0, 0, uFlags | SWP_NOSIZE);
            //DeferWindowPos(hDwp, m_btCancelBanker.Handle, 0, nWidth / 2 + 271, nHeight / 2 - 266, 0, 0, uFlags | SWP_NOSIZE);

            //DeferWindowPos(hDwp, m_btScoreMoveL.Handle, 0, nWidth / 2 - 265, nHeight / 2 + 261, 0, 0, uFlags | SWP_NOSIZE);
            //DeferWindowPos(hDwp, m_btScoreMoveR.Handle, 0, nWidth / 2 + 240, nHeight / 2 + 261, 0, 0, uFlags | SWP_NOSIZE);

            GameGraphics.DeferWindowPos(hDwp, m_btBankerMoveUp.Handle, 0, nWidth / 2 + 119, nHeight / 2 - 333, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);
            GameGraphics.DeferWindowPos(hDwp, m_btBankerMoveDown.Handle, 0, nWidth / 2 + 119, nHeight / 2 - 280, 0, 0, uFlags | GameGraphics.SWP_NOSIZE);

            //DeferWindowPos(hDwp, m_btOpenBank.Handle, 0, nWidth / 2 - 358, nHeight / 2 + 262, 0, 0, uFlags | SWP_NOSIZE);

            //Rectangle rcSicboDesktop = new Rectangle();
            //GetClientRect( m_btSicboDesktop.Handle, ref rcSicboDesktop );

            //DeferWindowPos(hDwp, m_btSicboDesktop.Handle, 0, nWidth / 2 + 50, nHeight / 2 + 115, 0, 0, uFlags | SWP_NOSIZE);
            //DeferWindowPos(hDwp, m_btSicboInMidair.Handle, 0, nWidth / 2 + 50 + rcSicboDesktop.Width + 10, nHeight / 2 + 115, 0, 0, uFlags | SWP_NOSIZE);
            //DeferWindowPos(hDwp, m_btSicboInTheAir.Handle, 0, nWidth / 2 + 50 + (rcSicboDesktop.Width + 10) * 2, nHeight / 2 + 115, 0, 0, uFlags | SWP_NOSIZE);

            //结束移动
            GameGraphics.EndDeferWindowPos(hDwp);
            return;
        }

        GameStatus GetGameStatus()
        {
            return this.m_Status;
        }




        //绘画界面
        void DrawGameView( Graphics g, int nWidth, int nHeight)
        {
	        //绘画背景
            for( int i = 0; i < nWidth; i+= m_ImageViewFill.Width )
            {
                for( int k = 0; k < nHeight; k+= m_ImageViewFill.Height )
                {
                    g.DrawImage( m_ImageViewFill, new Point( i, k ));
                }
            }

	        g.DrawImage(m_ImageViewBack, new Point( (nWidth - m_ImageViewBack.Width)/2, (nHeight-m_ImageViewBack.Height)/2));

	        //获取状态	
	        GameStatus cbGameStatus=GetGameStatus();

	        if (E_CARD_TYPE.enCardType_Illegal != m_enSelArea)
	        {
		        g.DrawImage( m_PngSicboArea[(int)m_enSelArea], new Point( m_rcUser[(int)m_enSelArea].X +2, m_rcUser[(int)m_enSelArea].Y +2));
	        }


	        //时间提示
	        Image ImageHandleTimeFlag = m_ImageTimeFlag;
	        int nTimeFlagWidth = m_ImageTimeFlag.Width/3;
	        int nFlagIndex=0;

	        if (cbGameStatus==GameStatus.GS_JOIN) 
                nFlagIndex=0;
	        else if (cbGameStatus==GameStatus.GS_BETTING) 
                nFlagIndex=1;
	        else if (cbGameStatus==GameStatus.GS_END) 
                nFlagIndex=2;

            Rectangle dstRect = new Rectangle( nWidth/2+285, nHeight/2+300, nTimeFlagWidth, m_ImageTimeFlag.Height);
            Rectangle srcRect = new Rectangle( nFlagIndex * nTimeFlagWidth, 0, nTimeFlagWidth, m_ImageTimeFlag.Height);

	        g.DrawImage( m_ImageTimeFlag, dstRect, srcRect, GraphicsUnit.Pixel );

	        //最大下注
	        //pDC->SetTextColor(RGB(255,234,0));

	        //胜利边框
	        FlashJettonAreaFrame(nWidth,nHeight,g);

	        //筹码资源
	        Size SizeJettonItem = new Size(m_ImageJettonView.Width/JETTON_COUNT,m_ImageJettonView.Height);

	        // 绘画筹码
	        for (int i=0;i<SicboDefine.COUNT_AZIMUTH;i++)
	        {
		        //变量定义
		        int lScoreCount=0;
                int[] lScoreJetton = new int[JETTON_COUNT] { 5, 500, 10, 50, 50000, 100, 500, 1000, 5000 };

		        //绘画筹码
		        for (int j=0;j<m_JettonInfoArray[i].Count;j++)
		        {
			        //获取信息
			        tagJettonInfo pJettonInfo = m_JettonInfoArray[i][j];

			        //累计数字
			        //ASSERT(pJettonInfo->cbJettonIndex<JETTON_COUNT);
			        lScoreCount+=lScoreJetton[pJettonInfo.cbJettonIndex];

			        //绘画界面
			        DrawAlphaImage( g, m_ImageJettonView, pJettonInfo.nXPos+m_PointJetton[i].X-SizeJettonItem.Width/2,
				        pJettonInfo.nYPos+m_PointJetton[i].Y-SizeJettonItem.Height/2,
				        SizeJettonItem.Width,SizeJettonItem.Height,
				        pJettonInfo.cbJettonIndex*SizeJettonItem.Width,0, Color.Red);
		        }

		        //绘画数字
		        //if (lScoreCount>0)	DrawNumberString(pDC,lScoreCount,m_PointJettonNumber[i].x,m_PointJettonNumber[i].y);
	        }

	        //庄家信息
	        //pDC->SetTextColor(RGB(255,234,0));

	        //获取玩家
	        //tagUserData pUserData = m_wBankerUser==INVALID_CHAIR ? null : GetUserInfo(m_wBankerUser);

	        //位置信息
	        Rectangle StrRect = new Rectangle();
	        StrRect.X = nWidth/2-300;
	        StrRect.Y  = nHeight/2 - 332;
	        StrRect.Width  = 190;
	        StrRect.Height  = 13;
        	

	        //庄家名字
	        //pDC->DrawText(pUserData==NULL?(m_bEnableSysBanker?TEXT("系统坐庄"):TEXT("无人坐庄")):pUserData->szName, StrRect, DT_END_ELLIPSIS | DT_LEFT | DT_TOP| DT_SINGLELINE );

	        //庄家总分
	        //CString strBankerTotalScore;
	        StrRect.X = nWidth/2-300;
	        StrRect.Y  = nHeight/2 - 310;
	        StrRect.Width  = StrRect.X + 190;
	        StrRect.Height  = StrRect.Y  + 13;
	        //DrawNumberStringWithSpace(g,pUserData==NULL?0:pUserData->lScore, StrRect);

	        //庄家局数
	        //CString strBankerTime;
	        //strBankerTime.Format( "%d", m_wBankerTime );
	        StrRect.X = nWidth/2-300;
	        StrRect.Y  = nHeight/2 - 287;
	        StrRect.Width  = StrRect.X + 190;
	        StrRect.Height  = StrRect.Y  + 13;
	        //DrawNumberStringWithSpace(pDC,m_wBankerTime,StrRect);

	        //庄家成绩
	        StrRect.X = nWidth/2-300;
	        StrRect.Y  = nHeight/2 - 264;
	        StrRect.Width  = StrRect.X + 190;
	        StrRect.Height  = StrRect.Y  + 13;
	        //DrawNumberStringWithSpace(pDC,m_lBankerWinScore,StrRect);

	        // 绘制庄家列表
	        for (int i=0; i<SHOW_APPLY_BANKER; ++i)
	        {
		        int iIndex = m_iApplyFirst +i;
		        if ((iIndex<0) || (iIndex>=m_ApplyUserArray.Count()))
		        {
			        break;
		        }
		        // 位置
		        Rectangle rc = new Rectangle();
		        rc.X = m_ptApplyListPos.X;
		        rc.Y  = m_ptApplyListPos.Y + i*14;
		        rc.Width  = 100;
		        rc.Height  = 14;
                g.DrawString(m_ApplyUserArray[iIndex].Id, SystemFonts.DefaultFont, textBrush, new Point(rc.X, rc.Y));
		        rc.X = rc.Right  + 5;
		        rc.Width  = 120;

                if (m_ApplyUserArray[iIndex].nCashOrPointGame == 0)
		            DrawNumberStringWithSpace(g,m_ApplyUserArray[iIndex].Cash,rc.X, rc.Y);
                else
                    DrawNumberStringWithSpace(g, m_ApplyUserArray[iIndex].Point, rc.X, rc.Y);
	        }

	        //绘画用户
	        if (m_wMeChairID!=GameDefine.INVALID_CHAIR)
	        {
		        if ( m_UserInfo != null )
		        {
			        //游戏信息
			        string szResultScore ="";
			        string szGameScore="";

			        int lMeJetton = 0;
			        for (byte i=0; i<SicboDefine.COUNT_AZIMUTH; ++i)
			        {
				        lMeJetton += m_lMeScore[i];
			        }

			        Rectangle rcAccount = new Rectangle(nWidth/2+206, nHeight/2 - 334,190 ,13);
			        Rectangle rcGameScore = new Rectangle(nWidth/2+206, nHeight/2 - 309,190, 13);
			        Rectangle rcResultScore = new Rectangle(nWidth/2+206,nHeight/2 - 287,190,13);
        			
                    if(m_UserInfo.nCashOrPointGame == 0)
			            DrawNumberStringWithSpace(g,m_UserInfo.Cash-lMeJetton,rcGameScore.X, rcGameScore.Y);
                    else
                        DrawNumberStringWithSpace(g, m_UserInfo.Point - lMeJetton, rcGameScore.X, rcGameScore.Y);

                    DrawNumberStringWithSpace(g, m_lMeStatisticScore, rcResultScore.X, rcResultScore.Y);

			        g.DrawString( m_UserInfo.Id, SystemFonts.DefaultFont, textBrush,rcAccount.X, rcAccount.Y);
		        }
	        }

	        //切换庄家
            //if ( m_bShowChangeBanker )
            //{
            //    int	nXPos = nWidth / 2 - 130;
            //    int	nYPos = nHeight / 2 - 160;

            //    //由我做庄
            //    if ( m_wMeChairID == m_wBankerUser )
            //    {
            //        CImageHandle ImageHandleBanker(&m_ImageMeBanker);
            //        m_ImageMeBanker.BitBlt(pDC->GetSafeHdc(), nXPos, nYPos);
            //    }
            //    else if ( m_wBankerUser != INVALID_CHAIR )
            //    {
            //        CImageHandle ImageHandleBanker(&m_ImageChangeBanker);
            //        m_ImageChangeBanker.BitBlt(pDC->GetSafeHdc(), nXPos, nYPos);
            //    }
            //    else
            //    {
            //        CImageHandle ImageHandleBanker(&m_ImageNoBanker);
            //        m_ImageNoBanker.BitBlt(pDC->GetSafeHdc(), nXPos, nYPos);
            //    }
            //}

            //if ( m_bShowChangeSicbo )
            //{
            //    int	nXPos = nWidth / 2 - 130;
            //    int	nYPos = nHeight / 2 - 160;

            //    //由我做庄
            //    if ( enSicboType_Desktop == m_enSicboType )
            //    {
            //        CImageHandle ImageHandleDesktop(&m_ImageSicboDesktop);
            //        m_ImageSicboDesktop.BitBlt(pDC->GetSafeHdc(), nXPos, nYPos);
            //    }
            //    else if ( enSicboType_InMidair == m_enSicboType )
            //    {
            //        CImageHandle ImageHandleInMidair(&m_ImageSicboInMidair);
            //        m_ImageSicboInMidair.BitBlt(pDC->GetSafeHdc(), nXPos, nYPos);
            //    }
            //    else if ( enSicboType_InTheAir == m_enSicboType )
            //    {
            //        CImageHandle ImageHandleInTheAir(&m_ImageSicboInTheAir);
            //        m_ImageSicboInTheAir.BitBlt(pDC->GetSafeHdc(), nXPos, nYPos);
            //    }
            //}


	        //我的下注
	        //DrawMeJettonNumber(pDC);

	        //绘画时间
            //if (m_wMeChairID!=INVALID_CHAIR)
            //{
            //    WORD wUserTimer=GetUserTimer(m_wMeChairID);
            //    if (wUserTimer!=0) DrawUserTimer(pDC,nWidth/2+323,nHeight/2+278,wUserTimer);
            //}

	        //胜利标志
	        DrawWinFlags(g);

	        //显示结果
	        ShowGameResult(g, nWidth, nHeight);
        	
	        //爆炸效果
	        DrawBombEffect(g);

	        // 摇骰子动画
	        DrawSicboAnimSicbo(g);
	        //m_TCPrompt.Invalidate();
	        //m_TCPrompt.ShowWindow(SW_SHOW);
	        return;
        }

        //绘画爆炸
        void DrawBombEffect(Graphics g)
        {
	        //绘画爆炸
	        int nImageHeight=m_ImageBombEffect.Height;
	        int nImageWidth=m_ImageBombEffect.Width/BOMB_EFFECT_COUNT;

	        for (int i=0; i<SicboDefine.COUNT_AZIMUTH; ++i)
	        {
		        if (m_bBombEffect[i])
		        {
			        DrawImage( g, m_ImageBombEffect,m_PointJettonNumber[i].X-nImageWidth/2,m_PointJettonNumber[i].Y-10,nImageWidth,nImageHeight,
				        nImageWidth*(m_cbBombFrameIndex[i]%BOMB_EFFECT_COUNT),0);
		        }
	        }
        }

        //胜利边框
        void FlashJettonAreaFrame(int nWidth, int nHeight, Graphics g )
        {
	        if (m_cbAreaFlash==0xFF) 
                return;

            int iXPos = 0;
            int iYPos = 0;
            int iXSrc = 0;

	        // 结束判断,并且发完牌了
	        if ((GetGameStatus()==GameStatus.GS_END) && m_bSicboShow)
	        {		
		        // 查看谁是赢家		
		        int iWidth = m_ImageFrameWin.Width/8;
		        int iHeight = m_ImageFrameWin.Height/2;
		        int iSrcHeight = 0;
		        if (0 != m_cbAreaFlash)
		        {
			        iSrcHeight = iHeight;
		        }

		        if ((iWidth<3) || (iHeight<3))
		        {
			        return ;
		        }
		        for (int i=0; i<SicboDefine.COUNT_AZIMUTH; ++i)
		        {
			        if (m_bWinner[i] > 0 )
			        {
				        Rectangle rcTemp = m_rcUser[i];
                        rcTemp.Inflate(5, 5);
                        //rcTemp.X = rcTemp.X - 5;
                        //rcTemp.Y  = rcTemp.Y  - 5;
                        //rcTemp.Width  = rcTemp.Width  + 5;
                        //rcTemp.Height  = rcTemp.Height  + 5;
        				
				        // 绘制赢的边框
				        // 顶部,底部
				        for (int j=0; j<(rcTemp.Width-2*iWidth); j+=iWidth)
				        {
					        iXPos = rcTemp.X +iWidth + j;
					        iXSrc = iWidth * 4;
					        int iWidthTemp = (rcTemp.Width-2*iWidth)-j;

					        if (iWidthTemp>iWidth)
					        {
						        iWidthTemp = iWidth;
					        }
					        DrawImage( g, m_ImageFrameWin, iXPos, rcTemp.Y ,iWidthTemp, iHeight, iXSrc,iSrcHeight);
					        iYPos = rcTemp.Bottom  - iHeight;
					        iXSrc = iWidth * 6;
					        DrawImage( g, m_ImageFrameWin,iXPos,iYPos,iWidthTemp, iHeight, iXSrc,iSrcHeight);
				        }

				        // 左右
				        for (int j=0; j<(rcTemp.Height-2*iHeight); j+=iHeight)
				        {
					        iXPos = rcTemp.X ;
					        iYPos = rcTemp.Y  + iHeight +j;
					        iXSrc = iWidth * 5;

					        int iHeightTemp = (rcTemp.Height-2*iHeight)-j;
					        if (iHeightTemp>iHeight)
					        {
						        iHeightTemp = iHeight;
					        }

					        DrawImage( g, m_ImageFrameWin, iXPos,iYPos,iWidth, iHeightTemp, iXSrc,iSrcHeight );
        					
					        iXPos = rcTemp.Right -iWidth;
					        iXSrc = iWidth * 7;
					        DrawImage( g, m_ImageFrameWin,iXPos,iYPos,iWidth, iHeightTemp, iXSrc,iSrcHeight );
				        }

				        // 画四个角
				        iXPos =  rcTemp.X ;
				        iYPos =  rcTemp.Y ;
				        iXSrc = 0;
				        DrawImage( g, m_ImageFrameWin,iXPos,iYPos,iWidth, iHeight, iXSrc,iSrcHeight );

				        iXPos =rcTemp.Right -iWidth;
				        iYPos = rcTemp.Y ;
				        iXSrc = iWidth;
				        DrawImage( g, m_ImageFrameWin,iXPos,iYPos,iWidth, iHeight, iXSrc,iSrcHeight );

				        iXPos =rcTemp.X ;
				        iYPos = rcTemp.Bottom  - iHeight;
				        iXSrc = iWidth*2;
				        DrawImage( g, m_ImageFrameWin,iXPos,iYPos,iWidth, iHeight, iXSrc,iSrcHeight );

				        iXPos =rcTemp.Right -iWidth;
				        iYPos = rcTemp.Bottom  - iHeight;
				        iXSrc = iWidth*3;
				        DrawImage( g, m_ImageFrameWin,iXPos,iYPos,iWidth, iHeight, iXSrc,iSrcHeight );
			        }
		        }
	        }
        }

        //绘画标识
        void DrawWinFlags(Graphics g)
        {
            //Image ImageHandleWinFlags = m_ImageWinFlags;
            //Image ImageHandleSicboNumber = m_ImageCardDot;
	        
            //int nIndex = m_nScoreHead;
            ////COLORREF clrOld ;
            ////clrOld = pDC->SetTextColor(RGB(255,234,0));
            //int iHistoryCount = abs((m_nRecordLast - m_nRecordFirst + MAX_SCORE_HISTORY) % MAX_SCORE_HISTORY);
            //iHistoryCount = iHistoryCount>=SHOW_SCORE_HISTORY?0:(SHOW_SCORE_HISTORY-iHistoryCount);
            //CString strPoint;
            //int nDrawCount = 0;
            //while ( nIndex != m_nRecordLast && ( m_nRecordLast!=m_nRecordFirst ) && nDrawCount < SHOW_SCORE_HISTORY )
            //{
            //    tagClientGameRecord &ClientGameRecord = m_GameRecordArrary[nIndex];

            //    int nXPos = m_nWinFlagsExcursionX + (((nIndex - m_nScoreHead + MAX_SCORE_HISTORY) % MAX_SCORE_HISTORY) + iHistoryCount) * 30;
            //    int nYPos = 0;
            //    int nFlagsIndex = 0;
            //    int iCardDot = 0;
            //    // 显示3个骰子
            //    for (BYTE i=0; i<MAX_COUNT_SICBO; ++i)
            //    {
            //        nYPos = m_nWinFlagsExcursionY + ((m_ImageWinFlags.GetHeight()/2 - 3)*i);
            //        nFlagsIndex = ClientGameRecord.iCards[i]-1;
            //        //OUTPUT_DEBUG_STRING("点数 %d  index %d",ClientGameRecord.iCards[i], i);
            //        iCardDot += ClientGameRecord.iCards[i];

            //        m_ImageWinFlags.AlphaDrawImage(pDC, nXPos, nYPos, m_ImageWinFlags.GetWidth()/6 , 
            //            m_ImageWinFlags.GetHeight()/2,m_ImageWinFlags.GetWidth()/6 * nFlagsIndex,
            //            ClientGameRecord.bFlags?0:(m_ImageWinFlags.GetHeight()/2), RGB(255, 0, 255));
            //    }
            //    nYPos += 23;
            //    nYPos += 2;
            //    m_ImageCardDot.AlphaDrawImage(pDC, nXPos, nYPos,m_ImageCardDot.GetWidth()/2, m_ImageCardDot.GetHeight(),
            //        ClientGameRecord.bFlags?0:m_ImageCardDot.GetWidth()/2, 0, RGB(255, 0, 255));
            //    nIndex = (nIndex+1) % MAX_SCORE_HISTORY;
            //    nDrawCount++;
            //}

            //pDC->SetTextColor(clrOld);
        }

        //清理筹码
        void CleanUserJetton()
        {
            //清理数组
            for (byte i = 0; i < m_JettonInfoArray.Length; i++)
            {
                m_JettonInfoArray[i].Clear();
            }

            //个人下注
            Array.Clear(m_lMeScore, 0, m_lMeScore.Length);

            //全体下注
            Array.Clear(m_lAllScore, 0, m_lAllScore.Length);

            m_strDispatchCardTips = "";

            //更新界面
            Invalidate();

            return;
        }

        //个人下注
        void SetMePlaceJetton(byte cbViewIndex, int lJettonCount)
        {
            //效验参数
            if (cbViewIndex >= SicboDefine.COUNT_AZIMUTH)
                return;

            m_lMeScore[cbViewIndex] = lJettonCount;

            //更新界面
            Invalidate();
        }

        //设置扑克
        void SetCardInfo(int[] enCards, bool[] bWinner)
        {
            m_cbWinnerSide = 0xFF;

            if (null != enCards)
            {
                m_enCards = (int[])enCards.Clone();
                m_bWinner = (int[])m_bWinner.Clone();
                m_bSicboShow = false;

                //开始发牌
                DispatchCard();
            }
            else
            {
                Array.Clear(m_enCards, 0, m_enCards.Length);
                Array.Clear(m_bWinner, 0, m_bWinner.Length);
                Invalidate();
            }
        }

        //设置筹码
        void PlaceUserJetton(byte cbViewIndex, int lScoreCount)
        {
            //效验参数
            if (cbViewIndex >= SicboDefine.COUNT_AZIMUTH)
                return;

            //设置炸弹
            if (lScoreCount == 5000000)
            {
                SetBombEffect(true, cbViewIndex);
            }

            //变量定义
            bool bPlaceJetton = false;
            int[] lScoreIndex = new int[JETTON_COUNT] { 5, 500, 10, 50, 50000, 100, 500, 1000, 5000 };

            //边框宽度
            int nFrameWidth = 0, nFrameHeight = 0;
            int nBorderWidth = 6;
            m_lAllScore[cbViewIndex] += lScoreCount;
            nFrameWidth = m_rcUser[cbViewIndex].Width  - m_rcUser[cbViewIndex].X ;
            nFrameHeight = m_rcUser[cbViewIndex].Height  - m_rcUser[cbViewIndex].Y ;
            nFrameWidth -= nBorderWidth;
            nFrameHeight -= nBorderWidth;

            //增加筹码
            for (byte i = 0; i < lScoreIndex.Length; i++)
            {
                //计算数目
                byte cbScoreIndex = (byte)(JETTON_COUNT - i - 1);
                int lCellCount = lScoreCount / lScoreIndex[cbScoreIndex];

                //插入过虑
                if (lCellCount == 0) continue;

                //加入筹码
                Random random = new Random();

                for (int j = 0; j < lCellCount; j++)
                {
                    //构造变量
                    tagJettonInfo JettonInfo;
                    int nJettonSize = 48;
                    JettonInfo.cbJettonIndex = cbScoreIndex;
                    int iRand = nFrameWidth - nJettonSize;
                    if (iRand < 1)
                    {
                        iRand = 1;
                    }
                    JettonInfo.nXPos = random.Next() % iRand - iRand / 2;

                    iRand = nFrameHeight - nJettonSize - 15;
                    if (iRand < 1)
                    {
                        iRand = 1;
                    }
                    JettonInfo.nYPos = random.Next() % iRand - iRand / 2;

                    //插入数组
                    bPlaceJetton = true;
                    m_JettonInfoArray[cbViewIndex].Add(JettonInfo);
                }

                //减少数目
                lScoreCount -= lCellCount * lScoreIndex[cbScoreIndex];
            }

            //更新界面
            if (bPlaceJetton == true)
                Invalidate();

            return;
        }

        //设置爆炸
        bool SetBombEffect(bool bBombEffect, int iAreaIndex)
        {
            if (bBombEffect == true)
            {
                //设置变量
                m_bBombEffect[iAreaIndex] = true;
                m_cbBombFrameIndex[iAreaIndex] = 0;

                //启动时间
                SetTimer(IDI_BOMB_EFFECT + iAreaIndex, 100);
            }
            else
            {
                //停止动画
                if (m_bBombEffect[iAreaIndex] == true)
                {
                    //删除时间
                    KillTimer(IDI_BOMB_EFFECT + iAreaIndex);

                    //设置变量
                    m_bBombEffect[iAreaIndex] = false;
                    m_cbBombFrameIndex[iAreaIndex] = 0;

                    //更新界面
                    Invalidate();
                }
            }

            return true;
        }

        //当局成绩
        void SetCurGameScore(int lMeCurGameScore, int lMeCurGameReturnScore, int lBankerCurGameScore, int lGameRevenue)
        {
            m_lMeCurGameScore = lMeCurGameScore;
            m_lMeCurGameReturnScore = lMeCurGameReturnScore;
            m_lBankerCurGameScore = lBankerCurGameScore;
            m_lGameRevenue = lGameRevenue;
        }

        //设置胜方
        void SetWinnerSide(byte cbWinnerSide)
        {
            m_cbWinnerSide = cbWinnerSide;
            //设置时间
            if (cbWinnerSide != 0xFF)
            {
                SetTimer(IDI_FLASH_WINNER, 500);
            }
            else
            {
                KillTimer(IDI_FLASH_WINNER);
                m_cbAreaFlash = 0xFF;
            }

            //更新界面
            Invalidate();

            return;
        }

        //获取区域
        byte GetJettonArea(Point MousePoint)
        {
            for (byte i = 0; i < SicboDefine.COUNT_AZIMUTH; ++i)
            {
                if (m_rcUser[i].Contains(MousePoint))
                {
                    return i;
                }
            }
            return 0xFF;
        }

        //绘画数字
        void DrawNumberString(Graphics g, int lNumber, int nXPos, int nYPos, bool bMeScore)
        {
            //加载资源
            Image HandleScoreNumber = m_ImageScoreNumber;
            Image HandleMeScoreNumber = m_ImageMeScoreNumber;
            Size SizeScoreNumber = new Size(m_ImageScoreNumber.Width / 10, m_ImageScoreNumber.Height);

            if (bMeScore)
                SizeScoreNumber = new Size(m_ImageMeScoreNumber.Width / 10, m_ImageMeScoreNumber.Height);

            //计算数目
            int lNumberCount = 0;
            int lNumberTemp = lNumber;
            do
            {
                lNumberCount++;
                lNumberTemp /= 10;
            } while (lNumberTemp > 0);

            //位置定义
            int nYDrawPos = nYPos - SizeScoreNumber.Height / 2;
            int nXDrawPos = nXPos + lNumberCount * SizeScoreNumber.Width / 2 - SizeScoreNumber.Width;

            //绘画桌号
            for (int i = 0; i < lNumberCount; i++)
            {
                //绘画号码
                int lCellNumber = (int)(lNumber % 10);
                if (bMeScore)
                {
                    Rectangle dstRect = new Rectangle(nXDrawPos, nYDrawPos, SizeScoreNumber.Width, SizeScoreNumber.Height);
                    Rectangle srcRect = new Rectangle(lCellNumber * SizeScoreNumber.Width, 0, SizeScoreNumber.Width, SizeScoreNumber.Height);

                    g.DrawImage(m_ImageMeScoreNumber, dstRect, srcRect, GraphicsUnit.Pixel);
                }
                else
                {
                    Rectangle dstRect = new Rectangle(nXDrawPos, nYDrawPos, SizeScoreNumber.Width, SizeScoreNumber.Height);
                    Rectangle srcRect = new Rectangle(lCellNumber * SizeScoreNumber.Width, 0, SizeScoreNumber.Width, SizeScoreNumber.Height);

                    g.DrawImage(m_ImageScoreNumber, dstRect, srcRect, GraphicsUnit.Pixel);
                }

                //设置变量
                lNumber /= 10;
                nXDrawPos -= SizeScoreNumber.Width;
            };

            return;
        }

        //绘画数字
        void DrawNumberStringWithSpace(Graphics g, int lNumber, int nXPos, int nYPos)
        {
            string strNumber = string.Format("{0:D4}", lNumber > 0 ? lNumber : -lNumber);
            //int p=strNumber.GetLength()-4;
            //while(p>0)
            //{
            //	strNumber.Insert(p," ");
            //	p-=4;
            //}
            if (lNumber < 0)
            {
                strNumber = "-" + strNumber;
            }

            //输出数字
            g.DrawString(strNumber, SystemFonts.DefaultFont, textBrush, new Point(nXPos, nYPos));
        }

        //绘画数字
        void DrawNumberStringWithSpace(Graphics g, int lNumber, Rectangle rcRect, int nFormat)
        {
            string strNumber = string.Format("{0:D4}", lNumber > 0 ? lNumber : -lNumber);
            //int p=strNumber.GetLength()-4;
            //while(p>0)
            //{
            //	strNumber.Insert(p," ");
            //	p-=4;
            //}
            if (lNumber < 0)
            {
                strNumber = "-" + strNumber;
            }

            //输出数字
            if (nFormat == -1)
            {
                g.DrawString(strNumber, SystemFonts.DefaultFont, textBrush, rcRect.X , rcRect.Y );
            }
            else
            {
                g.DrawString(strNumber, SystemFonts.DefaultFont, textBrush, rcRect.X , rcRect.Y );
            }
        }

        private void SicboView_Paint(object sender, PaintEventArgs e)
        {
            DrawGameView(e.Graphics, this.Width, this.Height);
        }


        private void m_btJetton100_Click(object sender, EventArgs e)
        {
            m_bettingChipMoney =  5;
            this.m_lCurrentJetton = 5;

        }

        private void m_btJetton1000_Click(object sender, EventArgs e)
        {
            m_bettingChipMoney = 10;
            this.m_lCurrentJetton = 10;

        }

        private void m_btJetton10000_Click(object sender, EventArgs e)
        {
            m_bettingChipMoney = 50;
            this.m_lCurrentJetton = 50;

        }

        private void m_btJetton100000_Click(object sender, EventArgs e)
        {
            m_bettingChipMoney = 100;
            this.m_lCurrentJetton = 100;

        }

        private void m_btJetton500000_Click(object sender, EventArgs e)
        {
            m_bettingChipMoney = 500;
            this.m_lCurrentJetton = 500;

        }

        private void m_btJetton1000000_Click(object sender, EventArgs e)
        {
            m_bettingChipMoney = 1000;
            this.m_lCurrentJetton = 1000;

        }

        private void m_btJetton5000000_Click(object sender, EventArgs e)
        {
            m_bettingChipMoney = 5000;
            this.m_lCurrentJetton = 5000;

        }

        //开始发牌
        void DispatchCard()
        {
            //设置变量
            m_bSicboEffect = true;
            m_bShowSicboEffectBack = true;
            m_bySicboFrameIndex = 0;

            // 给定七个区域，每次在七个区域中随机
            Rectangle[] rect = new Rectangle[7];

            for (int i = 0; i < rect.Length; i++)
                rect[i] = new Rectangle();

            rect[0].X = -5;
            rect[0].Y = -15;
            rect[0].Width = 10;
            rect[0].Height = 10;

            rect[1].X = -65;
            rect[1].Y = -15;
            rect[1].Width = 10;
            rect[1].Height = 10;

            rect[2].X = +45;
            rect[2].Y = -15;
            rect[2].Width = 10;
            rect[2].Height = 10;

            rect[3].X = -45;
            rect[3].Y = -52;
            rect[3].Width = 10;
            rect[3].Height = 10;

            rect[4].X = +22;
            rect[4].Y = -55;
            rect[4].Width = 10;
            rect[4].Height = 10;

            rect[5].X = -45;
            rect[5].Y = +14;
            rect[5].Width = 10;
            rect[5].Height = 10;

            rect[6].X = +22;
            rect[6].Y = +14;
            rect[6].Width = 10;
            rect[6].Height = 10;

            Random random = new Random();

            for (byte i = 0; i < SicboDefine.MAX_COUNT_SICBO; ++i)
            {
                m_bySicboDraw[i] = i;
                m_SicboAnimInfo[i].byAnimX = (byte)(random.Next() % 6 + 3);
                m_SicboAnimInfo[i].byAnimY = (byte)(random.Next() % 6);
                m_SicboAnimInfo[i].byResultX = (byte)(m_enCards[i] - 1);
                m_SicboAnimInfo[i].byResultY = (byte)(random.Next() % 4);

                byte byTemp = (byte)(random.Next() % (7 - i));
                m_SicboAnimInfo[i].ptEnd.X = rect[byTemp].X + (random.Next() % (rect[byTemp].Width + 1));
                m_SicboAnimInfo[i].ptEnd.Y = rect[byTemp].Y + (random.Next() % (rect[byTemp].Height + 1));
                if ((7 - i - 1) != byTemp)
                {
                    Rectangle rectTemp = rect[byTemp];
                    rect[byTemp] = rect[7 - i - 1];
                    rect[7 - i - 1] = rectTemp;
                }
            }

            // 画骰子的顺序
            for (byte i = 1; i < SicboDefine.MAX_COUNT_SICBO; ++i)
            {
                for (byte j = 0; j < i; ++j)
                {
                    if (m_SicboAnimInfo[m_bySicboDraw[j]].ptEnd.Y > m_SicboAnimInfo[m_bySicboDraw[i]].ptEnd.Y)
                    {
                        byte byTemp = m_bySicboDraw[j];
                        m_bySicboDraw[j] = m_bySicboDraw[i];
                        m_bySicboDraw[i] = byTemp;
                    }
                    else if ((m_SicboAnimInfo[m_bySicboDraw[j]].ptEnd.Y == m_SicboAnimInfo[m_bySicboDraw[i]].ptEnd.Y)
                        && (m_SicboAnimInfo[m_bySicboDraw[j]].ptEnd.X > m_SicboAnimInfo[m_bySicboDraw[i]].ptEnd.X))
                    {
                        byte byTemp = m_bySicboDraw[j];
                        m_bySicboDraw[j] = m_bySicboDraw[i];
                        m_bySicboDraw[i] = byTemp;
                    }
                }
            }

            SetTimer(IDI_DISPATCH_CARD, 80);
        }

        void DrawSicboAnimSicbo(Graphics g)
        {
            // 骰子的点数
            if (m_bSicboEffect)
            {
                if (m_bySicboFrameIndex > 35)
                {
                    //int nImgHeight=m_ImgSicboEPlan.GetHeight();
                    //int nImgWidth=m_ImgSicboEPlan.Width;
                    //m_ImgSicboEPlan.DrawImage(pDC, m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].x-nImgWidth/2-8,m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].y-nImgHeight/2+42,
                    //	nImgWidth, nImgHeight, 0,0);

                    int nImageHeight = m_ImgSicboEffectResult.Height / 4;
                    int nImageWidth = m_ImgSicboEffectResult.Width / 6;


                    // y坐标最小的先画，x坐标小的也先画				
                    for (byte j = 0; j < SicboDefine.MAX_COUNT_SICBO; ++j)
                    {
                        Rectangle dstRect = new Rectangle(m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].X + m_SicboAnimInfo[m_bySicboDraw[j]].ptEnd.X - nImageWidth / 2,
                            m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].Y + m_SicboAnimInfo[m_bySicboDraw[j]].ptEnd.Y - nImageHeight / 2 + 100, nImageWidth, nImageHeight);

                        Rectangle srcRect = new Rectangle(m_SicboAnimInfo[m_bySicboDraw[j]].byResultX * nImageWidth,
                            m_SicboAnimInfo[m_bySicboDraw[j]].byResultY * nImageHeight, nImageWidth, nImageHeight);

                        g.DrawImage(m_ImgSicboEffectResult, dstRect, srcRect, GraphicsUnit.Pixel);
                    }
                }

                // 前面16帧重复播放
                if (m_bySicboFrameIndex >= 5 && m_bySicboFrameIndex < (16 + 5))
                {
                    int iIndex = m_bySicboFrameIndex - 5;
                    int nImageHeight = m_ImgSicboAnim[iIndex].Height;
                    int nImageWidth = m_ImgSicboAnim[iIndex].Width;

                    Rectangle dstRect = new Rectangle(m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].X - nImageWidth / 2, m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].Y - nImageHeight / 2, nImageWidth, nImageHeight);
                    Rectangle srcRect = new Rectangle(0, 0, nImageWidth, nImageHeight);

                    g.DrawImage(m_ImgSicboAnim[iIndex], dstRect, srcRect, GraphicsUnit.Pixel);
                }
                else if (m_bySicboFrameIndex >= (16 + 5) && m_bySicboFrameIndex < 10 + (16 + 5))
                {
                    int iIndex = m_bySicboFrameIndex - (16 + 5) + 7;
                    int nImageHeight = m_ImgSicboAnim[iIndex].Height;
                    int nImageWidth = m_ImgSicboAnim[iIndex].Width;

                    Rectangle dstRect = new Rectangle(m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].X - nImageWidth / 2, m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].Y - nImageHeight / 2, nImageWidth, nImageHeight);
                    Rectangle srcRect = new Rectangle(0, 0, nImageWidth, nImageHeight);

                    g.DrawImage(m_ImgSicboAnim[iIndex], dstRect, srcRect, GraphicsUnit.Pixel);
                }
                else if (m_bySicboFrameIndex >= (10 + (16 + 5)) && m_bySicboFrameIndex < (15 + (16 + 5)))
                {
                    int iIndex = 16;
                    int nImageHeight = m_ImgSicboAnim[iIndex].Height;
                    int nImageWidth = m_ImgSicboAnim[iIndex].Width;

                    Rectangle dstRect = new Rectangle(m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].X - nImageWidth / 2, m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].Y - nImageHeight / 2, nImageWidth, nImageHeight);
                    Rectangle srcRect = new Rectangle(0, 0, nImageWidth, nImageHeight);

                    g.DrawImage(m_ImgSicboAnim[iIndex], dstRect, srcRect, GraphicsUnit.Pixel);
                }
                else if (m_bySicboFrameIndex > (10 + (16 + 5)) && m_bySicboFrameIndex < 38)
                {
                    int iIndex = 17;
                    int nImageHeight = m_ImgSicboAnim[iIndex].Height;
                    int nImageWidth = m_ImgSicboAnim[iIndex].Width;

                    Rectangle dstRect = new Rectangle(m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].X - nImageWidth / 2, m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].Y - nImageHeight / 2, nImageWidth, nImageHeight);
                    Rectangle srcRect = new Rectangle(0, 0, nImageWidth, nImageHeight);

                    g.DrawImage(m_ImgSicboAnim[iIndex], dstRect, srcRect, GraphicsUnit.Pixel);
                }
            }
        }

        private bool IsEnableSound()
        {
            return true;
        }

        public override void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Reply_TableDetail:
                    {
                        if (!(baseInfo is SicboInfo))
                            return;

                        SicboInfo tableInfo = (SicboInfo)baseInfo;

                        m_wMeChairID = GameDefine.INVALID_CHAIR;

                        for (int i = 0; i < tableInfo._Players.Count; i++)
                        {
                            UserInfo userInfo = tableInfo._Players[i];

                            if (userInfo.Id == m_UserInfo.Id)
                            {
                                m_wMeChairID = i;
                                break;
                            }
                        }

                        m_bWinner = tableInfo.m_bWinner;
                        m_enCards = tableInfo.m_enCards;
                        m_BettingTimeLeave = tableInfo.m_BettingTimeLeave;

                        if (tableInfo._RoundIndex == 0)
                            m_bettingTotalMoney = 0;

                        if( tableInfo._RoundIndex != (int)GameStatus.GS_END )
                            m_ApplyUserArray = tableInfo._Players;

                        if (m_wMeChairID != GameDefine.INVALID_CHAIR)
                        {
                            if (tableInfo._RoundIndex != (int)GameStatus.GS_END)
                                m_UserInfo = m_ApplyUserArray[m_wMeChairID];
                        }

                        m_ReceiveInfo = tableInfo;

                        SetGameStatus((GameStatus)tableInfo._RoundIndex);
                    }
                    break;

                case NotifyType.Reply_Betting:
                    {
                        BettingInfo bettingInfo = (BettingInfo)baseInfo;

                        //设置炸弹
                        if (bettingInfo._Score == 5000)
                        {
                            SetBombEffect(true, bettingInfo._Area);
                        }

                        if (m_wMeChairID != GameDefine.INVALID_CHAIR && m_wMeChairID == bettingInfo._UserIndex)
                        {
                            m_lMeScore[bettingInfo._Area] += bettingInfo._Score;
                        }
                        else
                        {
                            // 播放声音
                            if (IsEnableSound())
                            {
                                if (bettingInfo._UserIndex != m_wMeChairID)
                                {
                                    if (bettingInfo._Score == 5000)
                                    {
                                        PlayGameSound(Properties.Resources.ADD_GOLD_EX);
                                    }
                                    else
                                    {
                                        PlayGameSound(Properties.Resources.ADD_GOLD);
                                    }
                                    Random random = new Random();
                                    //PlayGameSound(m_DTSDCheer[random.Next() % 3]);
                                }
                            }
                        }

	                    //变量定义
	                    bool bPlaceJetton=false;
	                    int[] lScoreIndex = new int[JETTON_COUNT]{5,500,10,50,50000,100,500,1000,5000};

	                    //边框宽度
	                    int nFrameWidth=0, nFrameHeight=0;
	                    int nBorderWidth=6;
                        m_lAllScore[bettingInfo._Area] += bettingInfo._Score;
                        nFrameWidth = m_rcUser[bettingInfo._Area].Right - m_rcUser[bettingInfo._Area].Left;
                        nFrameHeight = m_rcUser[bettingInfo._Area].Bottom - m_rcUser[bettingInfo._Area].Top;
	                    nFrameWidth -= nBorderWidth;
	                    nFrameHeight -=  nBorderWidth;

	                    //增加筹码
	                    for (byte i=0;i<lScoreIndex.Length;i++)
	                    {
		                    //计算数目
		                    byte cbScoreIndex=(byte)(JETTON_COUNT-i-1);
                            int lCellCount = bettingInfo._Score / lScoreIndex[cbScoreIndex];

		                    //插入过虑
		                    if (lCellCount==0) 
                                continue;

		                    //加入筹码
                            Random random = new Random();

		                    for (int j=0;j<lCellCount;j++)
		                    {
			                    //构造变量
			                    tagJettonInfo JettonInfo;
			                    int nJettonSize=48;
			                    JettonInfo.cbJettonIndex=cbScoreIndex;
			                    int iRand = nFrameWidth-nJettonSize;
			                    if (iRand<1)
			                    {
				                    iRand= 1;
			                    }
			                    JettonInfo.nXPos=random.Next()%iRand - iRand/2;

			                    iRand = nFrameHeight-nJettonSize-15;
			                    if (iRand<1)
			                    {
				                    iRand= 1;
			                    }
			                    JettonInfo.nYPos=random.Next()%iRand - iRand/2;

			                    //插入数组
			                    bPlaceJetton=true;
                                m_JettonInfoArray[bettingInfo._Area].Add(JettonInfo);
		                    }

		                    //减少数目
                            bettingInfo._Score -= lCellCount * lScoreIndex[cbScoreIndex];
	                    }

	                    //更新界面
	                    if (bPlaceJetton==true) 
                            Invalidate();
                    }
                    break;
            }
        }

        //显示结果
        void ShowGameResult(Graphics g, int nWidth, int nHeight)
        {
            //显示判断
            if (GetGameStatus()!=GameStatus.GS_END)
                return;

            Image ImageHandleSicbo = m_ImageSicboPoint;
            Image ImageHandleCardsType = m_ImageCardType;

            if (m_bSicboShow)
            {
                //// 画骰子点数
                int nImageHeight = m_ImgSicboEffectResult.Height / 4;
                int nImageWidth = m_ImgSicboEffectResult.Width / 6;

                // y坐标最小的先画，x坐标小的也先画				
                for (byte j = 0; j < SicboDefine.MAX_COUNT_SICBO; ++j)
                {
                    Rectangle dstRect = new Rectangle(m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].X + m_SicboAnimInfo[m_bySicboDraw[j]].ptEnd.X - nImageWidth / 2,
                        m_ptSicboAnim[(int)E_SICBO_TYPE.enSicboType_Sicbo].Y + m_SicboAnimInfo[m_bySicboDraw[j]].ptEnd.Y - nImageHeight / 2 + 100, nImageWidth, nImageHeight);

                    Rectangle srcRect = new Rectangle(m_SicboAnimInfo[m_bySicboDraw[j]].byResultX * nImageWidth, m_SicboAnimInfo[m_bySicboDraw[j]].byResultY * nImageHeight, nImageWidth, nImageHeight);

                    g.DrawImage(m_ImgSicboEffectResult, dstRect, srcRect, GraphicsUnit.Pixel);
                }
            }

            //结束判断,并且发完牌了
            if ((GetGameStatus() == GameStatus.GS_END) && m_bSicboShow)
            {
                int nXPos = nWidth / 2 - 129;
                int nYPos = nHeight / 2 - 220;
                g.DrawImage( m_PngGameEnd, nXPos, nYPos);

                //pDC->SetTextColor(RGB(255, 234, 0));
                Rectangle rcMeWinScore = new Rectangle();
                rcMeWinScore.X = nXPos + 25 + 40;
                rcMeWinScore.Y = nYPos + 20 + 32;
                rcMeWinScore.Width = rcMeWinScore.X + 111;
                rcMeWinScore.Height = rcMeWinScore.Y + 34;

                Rectangle rcMeReturnScore = new Rectangle();
                rcMeReturnScore.X = nXPos + 25 + 150;
                rcMeReturnScore.Y = nYPos + 20 + 32;
                rcMeReturnScore.Width = rcMeReturnScore.X + 111;
                rcMeReturnScore.Height = rcMeReturnScore.Y + 34;

                string strMeGameScore, strMeReturnScore;
                DrawNumberStringWithSpace(g, m_lMeCurGameScore, rcMeWinScore, 0);
                DrawNumberStringWithSpace(g, m_lMeCurGameReturnScore, rcMeReturnScore, 0);

                Rectangle rcBankerWinScore = new Rectangle();
                rcBankerWinScore.X = nXPos + 25 + 40;
                rcBankerWinScore.Y = nYPos + 20 + 69;
                rcBankerWinScore.Width = rcBankerWinScore.X + 111;
                rcBankerWinScore.Height = rcBankerWinScore.Y + 34;
                DrawNumberStringWithSpace(g, m_lBankerCurGameScore, rcBankerWinScore, 0);
            }
        }

        void FinishDispatchCard()
        {
            m_ApplyUserArray = m_ReceiveInfo._Players;

            if (m_wMeChairID != GameDefine.INVALID_CHAIR)
            {
                m_UserInfo = m_ApplyUserArray[m_wMeChairID];
                SetCurGameScore(m_ReceiveInfo.m_lUserWinScore[m_wMeChairID], m_ReceiveInfo.m_lUserReturnScore[m_wMeChairID], 0, m_ReceiveInfo.m_lUserRevenue[m_wMeChairID]);
            }

            Array.Clear(m_lMeScore, 0, m_lMeScore.Length);

            //删除定时器
            //    KillTimer(IDI_CHANGE_AZIMUTH);
            m_bSicboEffect = false;
            m_bySicboFrameIndex = 0;
            m_bShowSicboEffectBack = false;
            m_bSicboShow = true;

            //    //保存记录
            //    tagClientGameRecord gameRecord;
            //    Array.Clear(&gameRecord, sizeof(gameRecord));
            //    int iCardDot = 0;
            //    for (BYTE i=0; i<SicboDefine.MAX_COUNT_SICBO; ++i)
            //    {
            //        gameRecord.iCards[i] = m_enCards[i];
            //        iCardDot += gameRecord.iCards[i];
            //    }
            //    if (iCardDot>(int)E_CARD_TYPE.enCardType_NumberTen)
            //    {
            //        gameRecord.bFlags = true;
            //    }
            //    else
            //    {
            //        gameRecord.bFlags = false;
            //    }

            //    SetGameHistory(gameRecord);

            //    // 累计金币
                m_lMeStatisticScore+=m_lMeCurGameScore;
                m_lBankerWinScore=m_lTmpBankerWinScore;

            //    //设置赢家
                SetWinnerSide(0);

            //    //播放声音
                if (m_lMeCurGameScore>0) 
                    PlayGameSound(Properties.Resources.END_WIN);
                else if (m_lMeCurGameScore<0) 
                    PlayGameSound(Properties.Resources.END_LOST);
                else 
                    PlayGameSound(Properties.Resources.END_DRAW);
        }


        protected override void OnTimer( TimerInfo timerInfo )
        {
            //闪动胜方
            if (timerInfo._Id == IDI_FLASH_WINNER)
            {
                //设置变量
                if (0xFF != m_cbWinnerSide)
                {
                    if (0x01 != m_cbAreaFlash)
                    {
                        m_cbAreaFlash = 0x01;
                    }
                    else
                    {
                        m_cbAreaFlash = 0x00;
                    }
                }
                else
                {
                    m_cbAreaFlash = 0xFF;
                    KillTimer(IDI_FLASH_WINNER);
                }

                //更新界面
                Invalidate();

                return;
            }

            //轮换庄家
            if (timerInfo._Id == IDI_SHOW_CHANGE_BANKER)
            {
                //ShowChangeBanker(false);

                return;
            }
            // 更换摇骰子方式
            if (timerInfo._Id == IDI_SHOW_CHANGE_SICBO)
            {
                KillTimer(timerInfo._Id);
                m_bShowChangeSicbo = false;
                Invalidate();
                return;
            }
            else if (IDI_DISPATCH_CARD == timerInfo._Id)
            {
                //停止判断
                if (m_bSicboEffect == false)
                {
                    KillTimer(timerInfo._Id);
                    return;
                }
                int iCountFrame = (((int)E_SICBO_TYPE.enSicboType_Normal == m_enSicboType)
                    || ((int)E_SICBO_TYPE.enSicboType_InTheAir == m_enSicboType)) ? SICBO_EFFECT_COUNT : SICBO_EFFECT_FRAME_COUNT;
                if ((int)E_SICBO_TYPE.enSicboType_Sicbo == m_enSicboType)
                {
                    iCountFrame = 40;
                }
                if ((m_bySicboFrameIndex + 1) >= iCountFrame)
                {
                    //删除时间
                    KillTimer(timerInfo._Id);
                    m_bShowSicboEffectBack = false;
                    FinishDispatchCard();
                }
                else
                {
                    ++m_bySicboFrameIndex;
                }

                //更新界面
                Invalidate();

                return;
            }

            //爆炸动画
            if ((timerInfo._Id > IDI_BOMB_EFFECT + (int)E_CARD_TYPE.enCardType_Illegal)
                && (timerInfo._Id < IDI_BOMB_EFFECT + SicboDefine.COUNT_AZIMUTH))
            {
                int wIndex = timerInfo._Id - IDI_BOMB_EFFECT;
                //停止判断
                if (m_bBombEffect[wIndex] == false)
                {
                    KillTimer(timerInfo._Id);
                    return;
                }

                //设置变量
                if ((m_cbBombFrameIndex[wIndex] + 1) >= BOMB_EFFECT_COUNT)
                {
                    //删除时间
                    KillTimer(timerInfo._Id);

                    //设置变量
                    m_bBombEffect[wIndex] = false;
                    m_cbBombFrameIndex[wIndex] = 0;
                }
                else
                {
                    ++m_cbBombFrameIndex[wIndex];
                }

                //更新界面
                Invalidate();

                return;
            }
        }

        protected void OnGameTimer( TimerInfo timerInfo )
        {
            if (IsEnableSound())
            {
                if (timerInfo._Id == IDI_PLACE_JETTON && timerInfo.Elapse <= 5)
                {
                    PlayGameSound(Properties.Resources.TIME_WARIMG);
                }
            }
        }


        private void SicboView_Resize(object sender, EventArgs e)
        {
            RectifyGameView(this.Width, this.Height);
        }

        private void SetGameStatus(GameStatus gameStatus)
        {
            //if (m_wMeChairID != INVALID_CHAIR)
            //{
            //    //m_btApplyBanker.Visible = false;
            //    m_btCancelBanker.Visible = true;
            //}
            //else
            //{
            //    //m_btApplyBanker.Visible = true;
            //    m_btCancelBanker.Visible = false;
            //}

            if (m_Status != gameStatus)
            {
                m_Status = gameStatus;

                bool enableBetting = false;

                switch (m_Status)
                {
                    case GameStatus.GS_JOIN:
                        {
                            ResetGameView();
                        }
                        break;

                    case GameStatus.GS_BETTING:
                        {
                            enableBetting = true;
                            SetGameTimer(IDI_PLACE_JETTON, m_BettingTimeLeave);

                            PlayGameSound(Properties.Resources.GAME_START);

                            if (this.m_BackPlayer == null)
                            {
                                //m_BackPlayer = new System.Media.SoundPlayer(Properties.Resources.BACK_GROUND);
                                //m_BackPlayer.PlayLooping();
                            }
                        }
                        break;

                    case GameStatus.GS_END:
                        {
                            m_bSicboShow = false;

                            //开始发牌
                            DispatchCard();
                        }
                        break;
                }

                m_btJetton100.Enabled = enableBetting;
                m_btJetton1000.Enabled = enableBetting;
                m_btJetton10000.Enabled = enableBetting;
                m_btJetton100000.Enabled = enableBetting;
                m_btJetton1000000.Enabled = enableBetting;
                m_btJetton500000.Enabled = enableBetting;
                m_btJetton5000000.Enabled = enableBetting;
            }

            Invalidate();
        }

        //private void btApplyBanker_Click(object sender, EventArgs e)
        //{
        //   // m_ClientSocket.Send(NotifyType.Request_PlayerEnter, m_UserInfo);
        //}

        private void SicboView_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_lCurrentJetton != 0L)
            {
                m_bettingTotalMoney = m_bettingTotalMoney + m_bettingChipMoney;

                if (m_UserInfo.nCashOrPointGame == 0)
                {
                    if (m_UserInfo.Cash >= m_bettingTotalMoney && m_bettingTotalMoney < 9999)
                    {
                        byte cbJettonArea = GetJettonArea(e.Location);

                        //大小判断
                        //int lMaxPlayerScore = GetMaxPlayerScore(cbJettonArea);
                        //if (lMaxPlayerScore < m_lCurrentJetton)
                        //{
                        //    return;
                        //}

                        if (cbJettonArea != 0xFF)
                        {
                            BettingInfo bettingInfo = new BettingInfo();

                            bettingInfo._Area = cbJettonArea;
                            bettingInfo._Score = m_lCurrentJetton;
                            bettingInfo._UserIndex = this.m_wMeChairID;

                            m_ClientSocket.Send(ChatEngine.NotifyType.Request_Betting, bettingInfo);


                            //播放声音
                            if (IsEnableSound())
                            {
                                if (m_lCurrentJetton == 5000)
                                {
                                    PlayGameSound(Properties.Resources.ADD_GOLD_EX);
                                }
                                else
                                {
                                    PlayGameSound(Properties.Resources.ADD_GOLD);
                                }
                                Random random = new Random();

                                //PlayGameSound(m_DTSDCheer[random.Next() % 3]);
                            }
                        }
                    }
                    else
                    {
                        m_bettingTotalMoney = m_bettingTotalMoney - m_bettingChipMoney;
                    }
                }
                else
                {
                    if (m_UserInfo.Point >= m_bettingTotalMoney && m_bettingTotalMoney < 9999)
                    {
                        byte cbJettonArea = GetJettonArea(e.Location);

                        //大小判断
                        //int lMaxPlayerScore = GetMaxPlayerScore(cbJettonArea);
                        //if (lMaxPlayerScore < m_lCurrentJetton)
                        //{
                        //    return;
                        //}

                        if (cbJettonArea != 0xFF)
                        {
                            BettingInfo bettingInfo = new BettingInfo();

                            bettingInfo._Area = cbJettonArea;
                            bettingInfo._Score = m_lCurrentJetton;
                            bettingInfo._UserIndex = this.m_wMeChairID;

                            m_ClientSocket.Send(ChatEngine.NotifyType.Request_Betting, bettingInfo);


                            //播放声音
                            if (IsEnableSound())
                            {
                                if (m_lCurrentJetton == 5000)
                                {
                                    PlayGameSound(Properties.Resources.ADD_GOLD_EX);
                                }
                                else
                                {
                                    PlayGameSound(Properties.Resources.ADD_GOLD);
                                }
                                Random random = new Random();

                                //PlayGameSound(m_DTSDCheer[random.Next() % 3]);
                            }
                        }
                    }
                    else
                    {
                        m_bettingTotalMoney = m_bettingTotalMoney - m_bettingChipMoney;
                    }
                }
            }

        }

        private void SicboView_MouseMove(object sender, MouseEventArgs e)
        {
	        E_CARD_TYPE enArea = E_CARD_TYPE.enCardType_Illegal;
	        
            for (int i=0;i<SicboDefine.COUNT_AZIMUTH; ++i)
	        {
		        //获取区域
		        Point MousePoint = e.Location;

		        if (m_rcUser[i].Contains(MousePoint))
		        {
			        enArea = (E_CARD_TYPE)(i);
			        break;
		        }
	        }

            if (enArea != m_enSelArea)
            {
                //m_TCPrompt.ShowWindow(SW_HIDE);
                //m_TCPrompt.MoveWindow(0, 0, 0, 0);
                m_enSelArea = enArea;
                Invalidate();
            }

            //if ((E_CARD_TYPE.enCardType_Illegal != m_enSelArea) && (GameStatus.GS_BETTING== GetGameStatus()))
            //{
            //    m_lSelAreaMax = GetMaxPlayerScore(m_enSelArea);
            //    if (m_lSelAreaMax < 0)
            //    {
            //        m_lSelAreaMax = 0L;
            //    }		
            //    UpdateGameView(NULL);
            //}
            //if (E_CARD_TYPE.enCardType_Illegal != m_enSelArea)
            //{
            //    int iSrcX = m_TCPrompt.m_PngBack.GetWidth()/2;
            //    INT iXPos = m_rcUser[m_enSelArea].left + m_rcUser[m_enSelArea].Width()/2 -10;
            //    INT iYPos = m_rcUser[m_enSelArea].top - m_TCPrompt.m_PngBack.GetHeight() + 20;
            //    switch (m_enSelArea)
            //    {
            //    case enCardType_Big:              case enCardType_Double:           case enCardType_NumberThirteen:
            //    case enCardType_NumberFourteen:   case enCardType_NumberFifteen:    case enCardType_NumberSixteen:
            //    case enCardType_NumberSeventeen:  case enCardType_SicboSix:         case enCardType_SicboFive:
            //    case enCardType_SicboThreeAndFour:case enCardType_SicboThreeAndFive:case enCardType_SicboThreeAndSix:
            //    case enCardType_SicboFourAndFive: case enCardType_SicboFourAndSix:  case enCardType_SicboFiveAndSix:
            //    case enCardType_SicboDoubleFour:  case enCardType_SicboDoubleFive:  case enCardType_SicboDoubleSix:
            //    case enCardType_SicboThreeFive:   case enCardType_SicboThreeSix:
            //        {
            //            iXPos = m_rcUser[m_enSelArea].left - iSrcX +m_rcUser[m_enSelArea].Width()/2 + 10;
            //            iSrcX = 0; break;				
            //        }
            //    default: break;
            //    }

            //    TCHAR szText[_MAX_PATH]={0};
            //    if (GS_PLACE_JETTON == m_pGameClientDlg->GetGameStatus())
            //    {		
            //        _sntprintf(szText, sizeof(szText), _T("可下分  ：%I64d\r\n已下分  ：%I64d\r\n我的下分：%I64d"),
            //            m_lSelAreaMax, m_lAllScore[m_enSelArea], m_lMeScore[m_enSelArea]);
            //        m_TCPrompt.SetWindowText(szText);
            //    }
            //    else
            //    {
            //        m_TCPrompt.SetWindowText(g_szPrompt[m_enSelArea]);
            //    }
            //    CRect rc;
            //    rc = CRect(iXPos,iYPos, iXPos + m_TCPrompt.m_PngBack.GetWidth()/2,iYPos + m_TCPrompt.m_PngBack.GetHeight());
            //    m_TCPrompt.m_iXPos = iSrcX;
            //    m_TCPrompt.MoveWindow(iXPos,iYPos, m_TCPrompt.m_PngBack.GetWidth()/2, m_TCPrompt.m_PngBack.GetHeight(), TRUE);	
            //    m_TCPrompt.ShowWindow(SW_SHOW);
            //}

	        if (m_lCurrentJetton!=0L)
	        {
		        //区域判断
		        if (E_CARD_TYPE.enCardType_Illegal == m_enSelArea)
		        {
                    this.Cursor = Cursors.Default;
			        return;
		        }

		        //大小判断
		        //OUTPUT_DEBUG_STRING("client %d  区域 最大可以下 %I64d", m_enSelArea, m_lSelAreaMax);
                //if ( m_lSelAreaMax < m_lCurrentJetton )
                //{
                //    SetCursor(LoadCursor(NULL,IDC_NO));
                //    return TRUE;
                //}

		        //设置光标
		        switch (m_lCurrentJetton)
		        {
		        case 5:
			        {
                        this.Cursor = GameGraphics.LoadCursorFromResource(Properties.Resources.SCORE_100);
				        return;
			        }
		        case 10:
			        {
                        this.Cursor = GameGraphics.LoadCursorFromResource(Properties.Resources.SCORE_1000);
                        return;
			        }
		        case 50:
			        {
                        this.Cursor = GameGraphics.LoadCursorFromResource(Properties.Resources.SCORE_10000);
                        return;
			        }
		        case 100:
			        {
                        this.Cursor = GameGraphics.LoadCursorFromResource(Properties.Resources.SCORE_100000);
                        return;
			        }
		        case 500:
			        {
                        this.Cursor = GameGraphics.LoadCursorFromResource(Properties.Resources.SCORE_500000);
                        return;
			        }
		        case 1000:
			        {
                        this.Cursor = GameGraphics.LoadCursorFromResource(Properties.Resources.SCORE_1000000);
                        return;
			        }
		        case 5000:
			        {
                        this.Cursor = GameGraphics.LoadCursorFromResource(Properties.Resources.SCORE_5000000);
                        return;
			        }
		        default: 
                    break;
		        }
	        }
	        else
	        {
                this.Cursor = GameGraphics.LoadCursorFromResource(Properties.Resources.NORMAL);
                return;
	        }
        }
    }

    public enum E_SICBO_TYPE
    {
        enSicboType_Normal = 0x00,             // 普通的摇骰子
        enSicboType_Desktop,                 // 桌面摇骰子
        enSicboType_InMidair,                // 半空摇骰子
        enSicboType_InTheAir,                // 空中摇骰子
        enSicboType_Sicbo                    // one游戏的摇骰子方式
    };

    //记录信息
    class tagClientGameRecord
    {
	    bool					bFlags;						// 是开大还是开小
	    int[]                   iCards = new int[SicboDefine.MAX_COUNT_SICBO];
    };

    //筹码信息
    struct tagJettonInfo
    {
	    public int						nXPos;								//筹码位置
	    public int						nYPos;								//筹码位置
	    public int		    			cbJettonIndex;						//筹码索引
    };

    // 正常时，骰子的动画轨迹
    struct tagSicboAnim
    {
	    public byte byResultX;  // 骰子的点数(点数确定了静态时取X列)
	    public byte byResultY;  // 静止状态时取Y行
	    public byte byAnimX;   // 运动过程中最后一个骰子取x列
	    public byte byAnimY;   // 运动过程中最后一个骰子取y行
	    public Point ptEnd;   // 最后停止的坐标
    };

    struct tagApplyUser
    {
	    //玩家信息
	    public string				strUserName;						//玩家帐号
	    public int					lUserScore;							//玩家金币
    }; 


}
