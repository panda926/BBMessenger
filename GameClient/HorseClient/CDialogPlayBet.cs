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

namespace HorseClient
{
    public partial class CDialogPlayBet : CDialog
    {
        // CDialogPlayBet 对话框
        public const int MAX_BET =		9999999;


	    //游戏变量
	    int[]					m_nMultiple = new int[HorseDefine.AREA_ALL];	//区域倍数
	    int[]				    m_lLastRound = new int[HorseDefine.AREA_ALL];	//上一轮积分
	    int				        m_lPlayerScore;			//玩家积分
	    int				        m_lTheNote;				//当前注数

	    //图片
	    CPngImage				    m_ImageBackdrop;		//背景

	    //控件
	    CEdit[]					m_editInput = new CEdit[HorseDefine.AREA_ALL];		//输入框
	    CSkinButton[]			    m_btAdd = new CSkinButton[HorseDefine.AREA_ALL];			//增加按钮
	    CSkinButton[]			    m_btReduce = new CSkinButton[HorseDefine.AREA_ALL];		//减少按钮
	    CSkinButton				m_btBet1000 = new CSkinButton();				//下注1000
	    CSkinButton				m_btBet1W = new CSkinButton();					//下注1W
	    CSkinButton				m_btBet10W = new CSkinButton();					//下注10W
	    CSkinButton				m_btBet100W = new CSkinButton();				//下注100W
	    CRect					m_rcBet1000;				//1000位置
	    CRect					m_rcBet1W;					//1W位置
	    CRect					m_rcBet10W;					//10W位置
	    CRect					m_rcBet100W;				//100W位置
	    CSkinButton				m_btDetermine = new CSkinButton();				//确定
	    CSkinButton				m_btReset = new CSkinButton();					//重置
	    CSkinButton				m_btRepeat = new CSkinButton();					//重复
	    CSkinButton				m_btClosee = new CSkinButton();					//关闭

	    //资源
	    CFont					m_InfoFont;
	    CFont					m_MultipleFont;
	    CBrush					m_InfoBrush;

        public HorseView m_Parent;

        public CDialogPlayBet()
        {
            InitializeComponent();

	        //区域倍数
	        for ( int i = 0; i < m_nMultiple.Length; ++i)
		        m_nMultiple[i] = 1;

	        m_InfoFont.CreateFont(12,0,0,0,400,0,0,0,134,3,2,CFont.ANTIALIASED_QUALITY,2,"宋体");
	        m_MultipleFont.CreateFont(13,0,0,0,800,0,0,0,134,3,2,CFont.ANTIALIASED_QUALITY,2,"宋体");
	        m_InfoBrush.CreateSolidBrush(CBrush.RGB(79,87,100));
	        GameGraphics.ZeroMemory(m_lLastRound);
	        m_lPlayerScore = 0;
	        m_lTheNote = 5;

            InitDialog();
        }

        const int IDC_EDIT_1_6      =              1044;
        const int IDC_EDIT_1_5       =             1045;
        const int IDC_EDIT_1_4        =            1046;
        const int IDC_EDIT_1_3         =           1047;
        const int IDC_EDIT_1_2         =           1048;
        const int IDC_EDIT_2_6          =          1049;
        const int IDC_EDIT_2_5         =           1050;
        const int IDC_EDIT_2_4          =          1051;
        const int IDC_EDIT_2_3           =         1052;
        const int IDC_EDIT_3_6            =        1053;
        const int IDC_EDIT_3_5             =       1054;
        const int IDC_EDIT_3_4              =      1055;
        const int IDC_EDIT_4_6       =             1056;
        const int IDC_EDIT_4_5        =            1057;
        const int IDC_EDIT_5_6         =           1058;
        const int IDC_BUTTON_ADD_1_6 =             1074;
        const int IDC_BUTTON_ADD_1_5  =            1079;
        const int IDC_BUTTON_ADD_1_4   =           1080;
        const int IDC_BUTTON_ADD_1_3    =          1081;
        const int IDC_BUTTON_ADD_1_2     =         1082;
        const int IDC_BUTTON_ADD_2_6      =        1083;
        const int IDC_BUTTON_ADD_2_5    =          1084;
        const int IDC_BUTTON_ADD_2_4     =         1085;
        const int IDC_BUTTON_ADD_2_3      =        1086;
        const int IDC_BUTTON_ADD_3_6       =       1087;
        const int IDC_BUTTON_ADD_3_5    =          1088;
        const int IDC_BUTTON_ADD_3_4     =         1089;
        const int IDC_BUTTON_ADD_4_6      =        1090;
        const int IDC_BUTTON_ADD_4_5       =       1091;
        const int IDC_BUTTON_ADD_5_6        =      1092;

        const int IDC_BUTTON_REDUCE_1_6  =         1059;
        const int IDC_BUTTON_REDUCE_1_5   =        1060;
        const int IDC_BUTTON_REDUCE_1_4    =       1061;
        const int IDC_BUTTON_REDUCE_1_3    =       1062;
        const int IDC_BUTTON_REDUCE_1_2     =      1063;
        const int IDC_BUTTON_REDUCE_2_6  =         1064;
        const int IDC_BUTTON_REDUCE_2_5   =        1065;
        const int IDC_BUTTON_REDUCE_2_4    =       1066;
        const int IDC_BUTTON_REDUCE_2_3     =      1067;
        const int IDC_BUTTON_REDUCE_3_6      =     1068;
        const int IDC_BUTTON_REDUCE_3_5    =       1069;
        const int IDC_BUTTON_REDUCE_3_4    =       1070;
        const int IDC_BUTTON_REDUCE_4_6     =      1071;
        const int IDC_BUTTON_REDUCE_4_5      =     1072;
        const int IDC_BUTTON_REDUCE_5_6       =    1073;

        const int IDC_BUTTON_1000  =               1075;
        const int IDC_BUTTON_1W     =              1076;
        const int IDC_BUTTON_10W     =             1077;
        const int IDC_BUTTON_100W     =            1078;
        const int IDC_BUTTON_DETERMINE =           1093;
        const int IDC_BUTTON_RESET      =          1035;
        const int IDC_BUTTON_REPEAT      =         1095;
        const int IDC_BUTTON_CLOSE_BET    =        1097;


        public override void DoDataExchange()
        {
            Control pDX = this;

            //m_editInput[HorseDefine.AREA_1_6] = new CEdit();
            //m_editInput[HorseDefine.AREA_1_5] = new CEdit();
            //m_editInput[HorseDefine.AREA_1_4] = new CEdit();
            //m_editInput[HorseDefine.AREA_1_3] = new CEdit();
            //m_editInput[HorseDefine.AREA_1_2] = new CEdit();
            //m_editInput[HorseDefine.AREA_2_6] = new CEdit();
            //m_editInput[HorseDefine.AREA_2_5] = new CEdit();
            //m_editInput[HorseDefine.AREA_2_4] = new CEdit();
            //m_editInput[HorseDefine.AREA_2_3] = new CEdit();
            //m_editInput[HorseDefine.AREA_3_6] = new CEdit();
            //m_editInput[HorseDefine.AREA_3_5] = new CEdit();
            //m_editInput[HorseDefine.AREA_3_4] = new CEdit();
            //m_editInput[HorseDefine.AREA_4_6] = new CEdit();
            //m_editInput[HorseDefine.AREA_4_5] = new CEdit();
            //m_editInput[HorseDefine.AREA_5_6] = new CEdit();

            //m_btAdd[HorseDefine.AREA_1_6] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_1_5] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_1_4] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_1_3] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_1_2] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_2_6] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_2_5] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_2_4] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_2_3] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_3_6] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_3_5] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_3_4] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_4_6] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_4_5] = new CSkinButton();
            //m_btAdd[HorseDefine.AREA_5_6] = new CSkinButton();

            //m_btReduce[HorseDefine.AREA_1_6] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_1_5] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_1_4] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_1_3] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_1_2] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_2_6] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_2_5] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_2_4] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_2_3] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_3_6] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_3_5] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_3_4] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_4_6] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_4_5] = new CSkinButton();
            //m_btReduce[HorseDefine.AREA_5_6] = new CSkinButton();


            //DDX_Control(pDX, IDC_EDIT_1_6, m_editInput[HorseDefine.AREA_1_6]);
            //DDX_Control(pDX, IDC_EDIT_1_5, m_editInput[HorseDefine.AREA_1_5]);
            //DDX_Control(pDX, IDC_EDIT_1_4, m_editInput[HorseDefine.AREA_1_4]);
            //DDX_Control(pDX, IDC_EDIT_1_3, m_editInput[HorseDefine.AREA_1_3]);
            //DDX_Control(pDX, IDC_EDIT_1_2, m_editInput[HorseDefine.AREA_1_2]);
            //DDX_Control(pDX, IDC_EDIT_2_6, m_editInput[HorseDefine.AREA_2_6]);
            //DDX_Control(pDX, IDC_EDIT_2_5, m_editInput[HorseDefine.AREA_2_5]);
            //DDX_Control(pDX, IDC_EDIT_2_4, m_editInput[HorseDefine.AREA_2_4]);
            //DDX_Control(pDX, IDC_EDIT_2_3, m_editInput[HorseDefine.AREA_2_3]);
            //DDX_Control(pDX, IDC_EDIT_3_6, m_editInput[HorseDefine.AREA_3_6]);
            //DDX_Control(pDX, IDC_EDIT_3_5, m_editInput[HorseDefine.AREA_3_5]);
            //DDX_Control(pDX, IDC_EDIT_3_4, m_editInput[HorseDefine.AREA_3_4]);
            //DDX_Control(pDX, IDC_EDIT_4_6, m_editInput[HorseDefine.AREA_4_6]);
            //DDX_Control(pDX, IDC_EDIT_4_5, m_editInput[HorseDefine.AREA_4_5]);
            //DDX_Control(pDX, IDC_EDIT_5_6, m_editInput[HorseDefine.AREA_5_6]);

            //DDX_Control(pDX, IDC_BUTTON_ADD_1_6, m_btAdd[HorseDefine.AREA_1_6]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_1_5, m_btAdd[HorseDefine.AREA_1_5]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_1_4, m_btAdd[HorseDefine.AREA_1_4]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_1_3, m_btAdd[HorseDefine.AREA_1_3]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_1_2, m_btAdd[HorseDefine.AREA_1_2]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_2_6, m_btAdd[HorseDefine.AREA_2_6]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_2_5, m_btAdd[HorseDefine.AREA_2_5]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_2_4, m_btAdd[HorseDefine.AREA_2_4]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_2_3, m_btAdd[HorseDefine.AREA_2_3]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_3_6, m_btAdd[HorseDefine.AREA_3_6]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_3_5, m_btAdd[HorseDefine.AREA_3_5]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_3_4, m_btAdd[HorseDefine.AREA_3_4]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_4_6, m_btAdd[HorseDefine.AREA_4_6]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_4_5, m_btAdd[HorseDefine.AREA_4_5]);
            //DDX_Control(pDX, IDC_BUTTON_ADD_5_6, m_btAdd[HorseDefine.AREA_5_6]);

            //DDX_Control(pDX, IDC_BUTTON_REDUCE_1_6, m_btReduce[HorseDefine.AREA_1_6]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_1_5, m_btReduce[HorseDefine.AREA_1_5]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_1_4, m_btReduce[HorseDefine.AREA_1_4]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_1_3, m_btReduce[HorseDefine.AREA_1_3]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_1_2, m_btReduce[HorseDefine.AREA_1_2]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_2_6, m_btReduce[HorseDefine.AREA_2_6]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_2_5, m_btReduce[HorseDefine.AREA_2_5]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_2_4, m_btReduce[HorseDefine.AREA_2_4]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_2_3, m_btReduce[HorseDefine.AREA_2_3]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_3_6, m_btReduce[HorseDefine.AREA_3_6]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_3_5, m_btReduce[HorseDefine.AREA_3_5]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_3_4, m_btReduce[HorseDefine.AREA_3_4]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_4_6, m_btReduce[HorseDefine.AREA_4_6]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_4_5, m_btReduce[HorseDefine.AREA_4_5]);
            //DDX_Control(pDX, IDC_BUTTON_REDUCE_5_6, m_btReduce[HorseDefine.AREA_5_6]);

            //DDX_Control(pDX, IDC_BUTTON_1000, m_btBet1000);
            //DDX_Control(pDX, IDC_BUTTON_1W, m_btBet1W);
            //DDX_Control(pDX, IDC_BUTTON_10W, m_btBet10W);
            //DDX_Control(pDX, IDC_BUTTON_100W, m_btBet100W);
            //DDX_Control(pDX, IDC_BUTTON_DETERMINE, m_btDetermine);
            //DDX_Control(pDX, IDC_BUTTON_RESET, m_btReset);
            //DDX_Control(pDX, IDC_BUTTON_REPEAT, m_btRepeat);
            //DDX_Control(pDX, IDC_BUTTON_CLOSE_BET, m_btClosee);

        }


        public override void BEGIN_MESSAGE_MAP()
        {
            ON_BN_CLICKED(IDC_BUTTON_1000, OnBnClickedButton1000);
            ON_BN_CLICKED(IDC_BUTTON_1W, OnBnClickedButton1w);
            ON_BN_CLICKED(IDC_BUTTON_10W, OnBnClickedButton10w);
            ON_BN_CLICKED(IDC_BUTTON_100W, OnBnClickedButton100w);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_1_6, OnBnClickedButtonReduce16);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_1_6, OnBnClickedButtonAdd16);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_1_5, OnBnClickedButtonReduce15);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_1_5, OnBnClickedButtonAdd15);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_1_4, OnBnClickedButtonReduce14);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_1_4, OnBnClickedButtonAdd14);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_1_3, OnBnClickedButtonReduce13);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_1_3, OnBnClickedButtonAdd13);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_1_2, OnBnClickedButtonReduce12);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_1_2, OnBnClickedButtonAdd12);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_2_6, OnBnClickedButtonReduce26);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_2_6, OnBnClickedButtonAdd26);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_2_5, OnBnClickedButtonReduce25);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_2_5, OnBnClickedButtonAdd25);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_2_4, OnBnClickedButtonReduce24);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_2_4, OnBnClickedButtonAdd24);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_2_3, OnBnClickedButtonReduce23);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_2_3, OnBnClickedButtonAdd23);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_3_6, OnBnClickedButtonReduce36);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_3_6, OnBnClickedButtonAdd36);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_3_5, OnBnClickedButtonReduce35);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_3_5, OnBnClickedButtonAdd35);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_3_4, OnBnClickedButtonReduce34);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_3_4, OnBnClickedButtonAdd34);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_4_6, OnBnClickedButtonReduce46);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_4_6, OnBnClickedButtonAdd46);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_4_5, OnBnClickedButtonReduce45);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_4_5, OnBnClickedButtonAdd45);
            //ON_BN_CLICKED(IDC_BUTTON_REDUCE_5_6, OnBnClickedButtonReduce56);
            //ON_BN_CLICKED(IDC_BUTTON_ADD_5_6, OnBnClickedButtonAdd56);
            //ON_BN_CLICKED(IDC_BUTTON_DETERMINE, OnBnClickedButtonDetermine);
            //ON_BN_CLICKED(IDC_BUTTON_RESET, OnBnClickedButtonReset);
            //ON_BN_CLICKED(IDC_BUTTON_REPEAT, OnBnClickedButtonRepeat);
            //ON_BN_CLICKED(IDC_BUTTON_CLOSE_BET, OnBnClickedButtonCloseBet);
            //ON_EN_CHANGE(IDC_EDIT_1_6, OnEnChangeEdit16);
            //ON_EN_CHANGE(IDC_EDIT_1_5, OnEnChangeEdit15);
            //ON_EN_CHANGE(IDC_EDIT_1_4, OnEnChangeEdit14);
            //ON_EN_CHANGE(IDC_EDIT_1_3, OnEnChangeEdit13);
            //ON_EN_CHANGE(IDC_EDIT_1_2, OnEnChangeEdit12);
            //ON_EN_CHANGE(IDC_EDIT_2_6, OnEnChangeEdit26);
            //ON_EN_CHANGE(IDC_EDIT_2_5, OnEnChangeEdit25);
            //ON_EN_CHANGE(IDC_EDIT_2_4, OnEnChangeEdit24);
            //ON_EN_CHANGE(IDC_EDIT_2_3, OnEnChangeEdit23);
            //ON_EN_CHANGE(IDC_EDIT_3_6, OnEnChangeEdit36);
            //ON_EN_CHANGE(IDC_EDIT_3_5, OnEnChangeEdit35);
            //ON_EN_CHANGE(IDC_EDIT_3_4, OnEnChangeEdit34);
            //ON_EN_CHANGE(IDC_EDIT_4_6, OnEnChangeEdit46);
            //ON_EN_CHANGE(IDC_EDIT_4_5, OnEnChangeEdit45);
            //ON_EN_CHANGE(IDC_EDIT_5_6, OnEnChangeEdit56);
        }


        // CDialogBet 消息处理程序
        private void CDialogPlayBet_Load(object sender, EventArgs e)
        {
            /*
            Control hInstance = this;
        
	        //HINSTANCE hInstance = AfxGetInstanceHandle();
	        m_ImageBackdrop.LoadImage( Properties.Resources.XIAZHU_FRAME );

	        for ( int i = 0; i < HorseDefine.AREA_ALL; ++i)
	        {
		        m_btAdd[i].SetButtonImage( Properties.Resources.BT_JIA,  hInstance , false,false);
		        m_btReduce[i].SetButtonImage( Properties.Resources.BT_JIAN,  hInstance , false,false);
	        }

	        m_btBet1000.SetButtonImage( Properties.Resources.BT_1000,  hInstance , false,false);
	        m_btBet1W.SetButtonImage( Properties.Resources.BT_1W,  hInstance , false,false);
	        m_btBet10W.SetButtonImage( Properties.Resources.BT_10W,  hInstance , false,false);
	        m_btBet100W.SetButtonImage( Properties.Resources.BT_100W,  hInstance , false,false);

	        m_btDetermine.SetButtonImage( Properties.Resources.BT_ENTER,  hInstance , false,false);
	        m_btReset.SetButtonImage( Properties.Resources.BT_REPLAY,  hInstance , false,false);
	        m_btRepeat.SetButtonImage( Properties.Resources.BT_REPLAYLASTROUND,  hInstance , false,false);
	        m_btDetermine.EnableWindow(false);
	        m_btRepeat.EnableWindow(false);

	        m_btClosee.SetButtonImage( Properties.Resources.BT_CLOSE,  hInstance , false,false);

	        //ModifyStyle(0, WS_CLIPCHILDREN, 0);
	        //SetWindowPos(null, 0, 0,m_ImageBackdrop.GetWidth(), m_ImageBackdrop.GetHeight(), SWP_NOACTIVATE|SWP_NOZORDER|SWP_NOCOPYBITS|SWP_NOMOVE);
            this.Width = m_ImageBackdrop.GetWidth();
            this.Height = m_ImageBackdrop.GetHeight();

            //m_btReduce[HorseDefine.AREA_1_6].MoveWindow( 98,		87 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_1_6].MoveWindow( 98 + 23,	92 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_1_6].MoveWindow( 98 + 23 + 42,	87 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_1_5].MoveWindow( 186,		87 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_1_5].MoveWindow( 186 + 23,	92 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_1_5].MoveWindow( 186 + 23 + 42,87 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_1_4].MoveWindow( 275,		87 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_1_4].MoveWindow( 275 + 23,	92 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_1_4].MoveWindow( 275 + 23 + 42,87 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_1_3].MoveWindow( 363,		87 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_1_3].MoveWindow( 363 + 23,	92 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_1_3].MoveWindow( 363 + 23 + 42,87 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_1_2].MoveWindow( 451,		87 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_1_2].MoveWindow( 451 + 23,	92 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_1_2].MoveWindow( 451 + 23 + 42,87 + 1 , 23, 23 );


            //m_btReduce[HorseDefine.AREA_2_6].MoveWindow( 98,		87 + 47 + 1, 23, 23 );
            //m_editInput[HorseDefine.AREA_2_6].MoveWindow( 98 + 23,	92 + 47 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_2_6].MoveWindow( 98 + 23 + 42,	87 + 47 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_2_5].MoveWindow( 186,		87 + 47 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_2_5].MoveWindow( 186 + 23,	92 + 47 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_2_5].MoveWindow( 186 + 23 + 42,87 + 47 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_2_4].MoveWindow( 275,		87 + 47 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_2_4].MoveWindow( 275 + 23,	92 + 47 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_2_4].MoveWindow( 275 + 23 + 42,87 + 47 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_2_3].MoveWindow( 363,		87 + 47 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_2_3].MoveWindow( 363 + 23,	92 + 47 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_2_3].MoveWindow( 363 + 23 + 42,87 + 47 + 1 , 23, 23 );


            //m_btReduce[HorseDefine.AREA_3_6].MoveWindow( 98,		87 + 47 * 2 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_3_6].MoveWindow( 98 + 23,	92 + 47 * 2 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_3_6].MoveWindow( 98 + 23 + 42,	87 + 47 * 2 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_3_5].MoveWindow( 186,		87 + 47 * 2 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_3_5].MoveWindow( 186 + 23,	92 + 47 * 2 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_3_5].MoveWindow( 186 + 23 + 42,87 + 47 * 2 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_3_4].MoveWindow( 275,		87 + 47 * 2 + 1 , 23, 23 );
            //m_editInput[HorseDefine.AREA_3_4].MoveWindow( 275 + 23,	92 + 47 * 2 + 1 , 42, 13 );
            //m_btAdd[HorseDefine.AREA_3_4].MoveWindow( 275 + 23 + 42,87 + 47 * 2 + 1 , 23, 23 );

            //m_btReduce[HorseDefine.AREA_4_6].MoveWindow( 98,		87 + 47 * 3 + 1, 23, 23 );
            //m_editInput[HorseDefine.AREA_4_6].MoveWindow( 98 + 23,	92 + 47 * 3 + 1, 42, 13 );
            //m_btAdd[HorseDefine.AREA_4_6].MoveWindow( 98 + 23 + 42,	87 + 47 * 3 + 1, 23, 23 );

            //m_btReduce[HorseDefine.AREA_4_5].MoveWindow( 186,		87 + 47 * 3 + 1, 23, 23 );
            //m_editInput[HorseDefine.AREA_4_5].MoveWindow( 186 + 23,	92 + 47 * 3 + 1, 42, 13 );
            //m_btAdd[HorseDefine.AREA_4_5].MoveWindow( 186 + 23 + 42,87 + 47 * 3 + 1, 23, 23 );

            //m_btReduce[HorseDefine.AREA_5_6].MoveWindow( 98,		87 + 47 * 4 + 2, 23, 23 );
            //m_editInput[HorseDefine.AREA_5_6].MoveWindow( 98 + 23,	92 + 47 * 4 + 2, 42, 13 );
            //m_btAdd[HorseDefine.AREA_5_6].MoveWindow( 98 + 23 + 42,	87 + 47 * 4 + 2, 23, 23 );

	        m_rcBet1000.SetRect(310			, 220, 310 + 58		, 220 + 59);
	        m_rcBet1W.SetRect(310 + 58		, 220, 310 + 58 * 2	, 220 + 59);
	        m_rcBet10W.SetRect(310 + 58 * 2	, 220, 310 + 58 * 3 , 220 + 59);
	        m_rcBet100W.SetRect(310 + 58 * 3, 220, 310 + 58 * 4 , 220 + 59);
	        m_btBet1000.MoveWindow(m_rcBet1000);
	        m_btBet1W.MoveWindow(m_rcBet1W);
	        m_btBet10W.MoveWindow(m_rcBet10W);
	        m_btBet100W.MoveWindow(m_rcBet100W);

	        m_btDetermine.MoveWindow(320	,288, 60, 22);
	        m_btReset.MoveWindow(320 + 65	,288, 60, 22);
	        m_btRepeat.MoveWindow(320 + 130	,288, 79, 22);

	        m_btClosee.MoveWindow( m_ImageBackdrop.GetWidth() - 26, 1, 25,25);
             * */
        }

        private void CDialogPlayBet_Paint(object sender, PaintEventArgs e)
        {
	        CDC dc = new CDC(); // device context for painting
            dc.SetGraphics(e.Graphics);
        	
            /*
	        //获取位置
	        Rectangle rcClient = this.ClientRectangle;
	        //GetClientRect(&rcClient);

	        //创建缓冲
	        CDC DCBuffer = new CDC();
	        CBitmap ImageBuffer = new CBitmap();
	        ImageBuffer.CreateCompatibleBitmap(dc,rcClient.Width,rcClient.Height);
	        DCBuffer.CreateCompatibleDC(ImageBuffer);

	        //设置 DC
	        //DCBuffer.SetBkMode(TRANSPARENT);
	        //CBitmap* oldBitmap = DCBuffer.SelectObject(&ImageBuffer);
	        DCBuffer.SelectObject(m_MultipleFont);
	        DCBuffer.SetTextAlign(CDC.TA_TOP|CDC.TA_LEFT);
	        DCBuffer.SetTextColor(Color.FromArgb(20,255,50));

	        m_ImageBackdrop.DrawImage(DCBuffer, 0, 0);

	        string szInfo = string.Empty;

	        for (int i = 0; i < HorseDefine.AREA_ALL; ++i )
	        {
		        CRect rect = new CRect();
		        m_btReduce[i].GetWindowRect(ref rect);
		        //ScreenToClient(&rect);
		        rect.OffsetRect(0, - 23);
		        rect.right += 66;

		        szInfo = string.Format( "{0}", m_nMultiple[i]);
		        DCBuffer.DrawText(szInfo, rect , CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER );
	        }

	        DCBuffer.SelectObject(m_InfoFont);
	        DCBuffer.SetTextColor(Color.FromArgb(250,250,0));
	        szInfo = string.Format( "您的余额为：{0}", m_lPlayerScore);
	        DCBuffer.DrawText(szInfo, new CRect( 188, 286, 188 + 130, 286 + 25) , CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER );

	        //绘画界面
	        dc.BitBlt(0,0,rcClient.Width,rcClient.Height,ImageBuffer,0,0,0);

	        //清理资源
            //DCBuffer.SetTextColor(oldColor);
            //DCBuffer.SetTextAlign(nTextAlign);
            //DCBuffer.SelectObject(oldBitmap);
            //DCBuffer.SelectObject(oldFont);
            //DCBuffer.DeleteDC();
            //ImageBuffer.DeleteObject();
             * */
        }

        public override void OnCtlColor()
        {
            /*
            for (int i = 0; i < m_editInput.Length; i++)
            {
                m_editInput[i].SetBorderStyle(BorderStyle.None);
                m_editInput[i].SetTextAlign(HorizontalAlignment.Center);

                m_editInput[i].SetBkColor(Color.FromArgb(79, 87, 100));
                m_editInput[i].SetTextColor(Color.White);
            }

            //if(nDlgCtrlID == IDC_EDIT_1_6 || nDlgCtrlID == IDC_EDIT_1_5 || nDlgCtrlID == IDC_EDIT_1_4 || nDlgCtrlID == IDC_EDIT_1_3 || nDlgCtrlID == IDC_EDIT_1_2  
            //    || nDlgCtrlID == IDC_EDIT_2_6 || nDlgCtrlID == IDC_EDIT_2_5 || nDlgCtrlID == IDC_EDIT_2_4 || nDlgCtrlID == IDC_EDIT_2_3
            //    || nDlgCtrlID == IDC_EDIT_3_6 || nDlgCtrlID == IDC_EDIT_3_5 || nDlgCtrlID == IDC_EDIT_3_4
            //    || nDlgCtrlID == IDC_EDIT_4_6 || nDlgCtrlID == IDC_EDIT_4_5
            //    || nDlgCtrlID == IDC_EDIT_5_6)
            //{
            //    pDC->SetBkMode(TRANSPARENT); 
            //    pDC->SetTextColor(RGB(255,255,255));
            //    return m_InfoBrush;
            //}
            //return hbr;
             * */
        }

        //bool CDialogPlayBet::OnEraseBkgnd(CDC* pDC)
        //{
        //    return true;
        //}

        void OnBnClickedButton1000()
        {
	        m_lTheNote = 5;
	        m_btBet1000.MoveWindow( m_rcBet1000.left, m_rcBet1000.top - 5, m_rcBet1000.Width(), m_rcBet1000.Height());
	        m_btBet1W.MoveWindow(m_rcBet1W);
	        m_btBet10W.MoveWindow(m_rcBet10W);
	        m_btBet100W.MoveWindow(m_rcBet100W);
        }

        void OnBnClickedButton1w()
        {
	        m_lTheNote = 10;
	        m_btBet1000.MoveWindow( m_rcBet1000 );
	        m_btBet1W.MoveWindow( m_rcBet1W.left, m_rcBet1W.top - 5, m_rcBet1W.Width(), m_rcBet1W.Height());
	        m_btBet10W.MoveWindow( m_rcBet10W );
	        m_btBet100W.MoveWindow( m_rcBet100W );
        }

        void OnBnClickedButton10w()
        {
	        m_lTheNote = 50;
	        m_btBet1000.MoveWindow( m_rcBet1000 );
	        m_btBet1W.MoveWindow( m_rcBet1W );
	        m_btBet10W.MoveWindow( m_rcBet10W.left, m_rcBet10W.top - 5, m_rcBet10W.Width(), m_rcBet10W.Height() );
	        m_btBet100W.MoveWindow( m_rcBet100W );

        }

        void OnBnClickedButton100w()
        {
	        m_lTheNote = 100;
	        m_btBet1000.MoveWindow( m_rcBet1000 );
	        m_btBet1W.MoveWindow( m_rcBet1W );
	        m_btBet10W.MoveWindow(m_rcBet10W);
	        m_btBet100W.MoveWindow(m_rcBet100W.left, m_rcBet100W.top - 5, m_rcBet100W.Width(), m_rcBet100W.Height());
        }


        //区域加
        void EditAdd(int cbArea)
        {
	        if( cbArea >= HorseDefine.AREA_ALL)
	        {
		        ASSERT(false);
		        return;
	        }

	        string szCount = m_editInput[cbArea].GetWindowText();

	        int lTemp = 0;
	        if (szCount.Length != 0)
                lTemp = ConverToInt(szCount);

	        lTemp += m_lTheNote;
	        string szBet = string.Empty;
	        szBet = string.Format( "{0}", lTemp);
	        m_editInput[cbArea].SetWindowText(szBet);
        }

        //区域减
        void EditReduce(int cbArea)
        {
	        if( cbArea >= HorseDefine.AREA_ALL)
	        {
		        ASSERT(false);
		        return;
	        }

	        string szCount = m_editInput[cbArea].GetWindowText();

	        int lTemp = 0;
	        if (szCount.Length != 0)
                lTemp = ConverToInt(szCount);

	        lTemp -= m_lTheNote;
	        if( lTemp < 0 )
		        lTemp = 0;
	        string szBet = string.Empty;
	        if( lTemp > 0 )
	        {
		        szBet = string.Format( "{0}", lTemp);
	        }
	        m_editInput[cbArea].SetWindowText(szBet);
        }

        //区域限制
        void EditLimit(int cbArea)
        {
	        if( cbArea >= HorseDefine.AREA_ALL)
	        {
		        ASSERT(false);
		        return;
	        }

	        string szCount= m_editInput[cbArea].GetWindowText();
        	
	        int lTemp = 0;
	        if (szCount.Length != 0)
                lTemp = ConverToInt(szCount);
        	
	        if ( lTemp > MAX_BET || lTemp < 0l )
	        {
		        if( lTemp > MAX_BET )
			        lTemp = MAX_BET;
		        else if ( lTemp < 0l )
			        lTemp = 0;

		        string szBet = string.Empty;
		        szBet = lTemp.ToString();
		        m_editInput[cbArea].SetWindowText(szBet);
	        }
        }


        //void OnBnClickedButtonReduce16()
        //{
        //    EditReduce(HorseDefine.AREA_1_6);
        //}

        //void OnBnClickedButtonAdd16()
        //{
        //    EditAdd(HorseDefine.AREA_1_6);
        //}

        //void OnBnClickedButtonReduce15()
        //{
        //    EditReduce(HorseDefine.AREA_1_5);
        //}

        //void OnBnClickedButtonAdd15()
        //{
        //    EditAdd(HorseDefine.AREA_1_5);
        //}

        //void OnBnClickedButtonReduce14()
        //{
        //    EditReduce(HorseDefine.AREA_1_4);
        //}

        //void OnBnClickedButtonAdd14()
        //{
        //    EditAdd(HorseDefine.AREA_1_4);
        //}

        //void OnBnClickedButtonReduce13()
        //{
        //    EditReduce(HorseDefine.AREA_1_3);
        //}

        //void OnBnClickedButtonAdd13()
        //{
        //    EditAdd(HorseDefine.AREA_1_3);
        //}

        //void OnBnClickedButtonReduce12()
        //{
        //    EditReduce(HorseDefine.AREA_1_2);
        //}

        //void OnBnClickedButtonAdd12()
        //{
        //    EditAdd(HorseDefine.AREA_1_2);
        //}

        //void OnBnClickedButtonReduce26()
        //{
        //    EditReduce(HorseDefine.AREA_2_6);
        //}

        //void OnBnClickedButtonAdd26()
        //{
        //    EditAdd(HorseDefine.AREA_2_6);
        //}

        //void OnBnClickedButtonReduce25()
        //{
        //    EditReduce(HorseDefine.AREA_2_5);
        //}

        //void OnBnClickedButtonAdd25()
        //{
        //    EditAdd(HorseDefine.AREA_2_5);
        //}

        //void OnBnClickedButtonReduce24()
        //{
        //    EditReduce(HorseDefine.AREA_2_4);
        //}

        //void OnBnClickedButtonAdd24()
        //{
        //    EditAdd(HorseDefine.AREA_2_4);
        //}

        //void OnBnClickedButtonReduce23()
        //{
        //    EditReduce(HorseDefine.AREA_2_3);
        //}

        //void OnBnClickedButtonAdd23()
        //{
        //    EditAdd(HorseDefine.AREA_2_3);
        //}

        //void OnBnClickedButtonReduce36()
        //{
        //    EditReduce(HorseDefine.AREA_3_6);
        //}

        //void OnBnClickedButtonAdd36()
        //{
        //    EditAdd(HorseDefine.AREA_3_6);
        //}

        //void OnBnClickedButtonReduce35()
        //{
        //    EditReduce(HorseDefine.AREA_3_5);
        //}

        //void OnBnClickedButtonAdd35()
        //{
        //    EditAdd(HorseDefine.AREA_3_5);
        //}

        //void OnBnClickedButtonReduce34()
        //{
        //    EditReduce(HorseDefine.AREA_3_4);
        //}

        //void OnBnClickedButtonAdd34()
        //{
        //    EditAdd(HorseDefine.AREA_3_4);
        //}

        //void OnBnClickedButtonReduce46()
        //{
        //    EditReduce(HorseDefine.AREA_4_6);
        //}

        //void OnBnClickedButtonAdd46()
        //{
        //    EditAdd(HorseDefine.AREA_4_6);
        //}

        //void OnBnClickedButtonReduce45()
        //{
        //    EditReduce(HorseDefine.AREA_4_5);
        //}

        //void OnBnClickedButtonAdd45()
        //{
        //    EditAdd(HorseDefine.AREA_4_5);
        //}

        //void OnBnClickedButtonReduce56()
        //{
        //    EditReduce(HorseDefine.AREA_5_6);
        //}

        //void OnBnClickedButtonAdd56()
        //{
        //    EditAdd(HorseDefine.AREA_5_6);
        //}

        //void OnEnChangeEdit16()
        //{
        //    EditLimit(HorseDefine.AREA_1_6);
        //}

        //void OnEnChangeEdit15()
        //{
        //    EditLimit(HorseDefine.AREA_1_5);
        //}

        //void OnEnChangeEdit14()
        //{
        //    EditLimit(HorseDefine.AREA_1_4);
        //}

        //void OnEnChangeEdit13()
        //{
        //    EditLimit(HorseDefine.AREA_1_3);
        //}

        //void OnEnChangeEdit12()
        //{
        //    EditLimit(HorseDefine.AREA_1_2);
        //}

        //void OnEnChangeEdit26()
        //{
        //    EditLimit(HorseDefine.AREA_2_6);
        //}


        //void OnEnChangeEdit25()
        //{
        //    EditLimit(HorseDefine.AREA_2_5);
        //}

        //void OnEnChangeEdit24()
        //{
        //    EditLimit(HorseDefine.AREA_2_4);
        //}

        //void OnEnChangeEdit23()
        //{
        //    EditLimit(HorseDefine.AREA_2_3);
        //}

        //void OnEnChangeEdit36()
        //{
        //    EditLimit(HorseDefine.AREA_3_6);
        //}

        //void OnEnChangeEdit35()
        //{
        //    EditLimit(HorseDefine.AREA_3_5);
        //}

        //void OnEnChangeEdit34()
        //{
        //    EditLimit(HorseDefine.AREA_3_4);
        //}

        //void OnEnChangeEdit46()
        //{
        //    EditLimit(HorseDefine.AREA_4_6);
        //}

        //void OnEnChangeEdit45()
        //{
        //    EditLimit(HorseDefine.AREA_4_5);
        //}

        //void OnEnChangeEdit56()
        //{
        //    EditLimit(HorseDefine.AREA_5_6);
        //}


        //确认
        void OnBnClickedButtonDetermine()
        {
	        int lAllScore = 0;
	        for ( int i = 0 ; i < HorseDefine.AREA_ALL; ++i)
	        {
		        string szCount = m_editInput[i].GetWindowText();
                m_lLastRound[i] = ConverToInt(szCount);
		        lAllScore += m_lLastRound[i];
	        }
	        if ( lAllScore > m_lPlayerScore )
	        {
                DialogMessage message = new DialogMessage();
                
                message.SetMessage( "您的余额不足，请充值！");
                message.ShowDialog();
		        return;
	        }

	        if ( lAllScore > 0 )
	        {
		        for ( int i = 0 ; i < HorseDefine.AREA_ALL; ++i)
		        {
			        m_editInput[i].SetWindowText("");
		        }

		        //CMD_C_PlayerBet stPlayerBet;
		        //Array.Copy( m_lLastRound, stPlayerBet.lBetScore, m_lLastRound.Length);
		        //CGameFrameEngine::GetInstance()->SendMessage(HorseDefine.IDM_PLAYER_BET, 0, (LPARAM)(&stPlayerBet));
                ((GameView)m_Parent).NotifyMessage( HorseDefine.IDM_PLAYER_BET, 0, m_lLastRound );
	        }
        }

        //重置
        void OnBnClickedButtonReset()
        {
	        for ( int i = 0 ; i < HorseDefine.AREA_ALL; ++i)
	        {
		        m_editInput[i].SetWindowText("");
	        }
        }

        //重复
        void OnBnClickedButtonRepeat()
        {
	        for ( int i = 0 ; i < HorseDefine.AREA_ALL; ++i)
	        {
		        string szBet = string.Empty;
		        if( m_lLastRound[i] > 0 )
		        {
			        szBet = m_lLastRound[i].ToString();
		        }
		        m_editInput[i].SetWindowText(szBet);
	        }
        }

        //关闭
        void OnBnClickedButtonCloseBet()
        {
	        this.Visible = false;
        }

        //设置积分
        public void SetScore( int lScore )
        {
	        if( m_lPlayerScore != lScore )
	        {
		        m_lPlayerScore = lScore;

		        if(this.Visible == true)
		        {
			        Invalidate(false);
		        }
	        }
        }

        //设置是否能下注
        public void SetCanBet( bool bCanBet)
        {
	        //if( m_btDetermine.GetSafeHwnd() && m_btRepeat.GetSafeHwnd() )
	        //{
		        m_btDetermine.EnableWindow(bCanBet);
		        m_btRepeat.EnableWindow(bCanBet);
	        //}
        }

        //设置倍数
        public void SetMultiple( int[] nMultiple )
        {
	        //memcpy(m_nMultiple, nMultiple, sizeof(m_nMultiple));
            Array.Copy(nMultiple, m_nMultiple, nMultiple.Length);
	        
            if(this.Visible == true)
	        {
		        Invalidate(false);

	        }
        }


    }
}
