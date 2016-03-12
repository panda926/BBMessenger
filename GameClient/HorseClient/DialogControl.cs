using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ChatEngine;
using GameControls;

namespace HorseClient
{
    public partial class DialogControl : CDialog
    {
        int[]  m_lAllUserBet = new int[HorseDefine.AREA_ALL];				//用户下注
        CEdit[]  m_editInput = new CEdit[HorseDefine.AREA_ALL];		//输入框
        CRadioButton[] m_radioArea = new CRadioButton[HorseDefine.AREA_ALL];		//控制区域
        CComboBox m_comboTime = new CComboBox();

        public DialogControl()
        {
            InitializeComponent();

            for ( int i = 0; i < m_lAllUserBet.Length; i++)
		        m_lAllUserBet[i] = 0;
        }

        const int IDC_EDIT_C_1_6 = 1131;
        const int IDC_EDIT_C_1_5 = 1136;
        const int IDC_EDIT_C_1_4 = 1140;
        const int IDC_EDIT_C_1_3 = 1143;
        const int IDC_EDIT_C_1_2 = 1145;
        const int IDC_EDIT_C_2_6 = 1132;
        const int IDC_EDIT_C_2_5 = 1137;
        const int IDC_EDIT_C_2_4 = 1141;
        const int IDC_EDIT_C_2_3 = 1144;
        const int IDC_EDIT_C_3_6 = 1133;
        const int IDC_EDIT_C_3_5 = 1138;
        const int IDC_EDIT_C_3_4 = 1142;
        const int IDC_EDIT_C_4_6 = 1134;
        const int IDC_EDIT_C_4_5 = 1139;
        const int IDC_EDIT_C_5_6 = 1135;

        const int IDC_RADIO_1_6 = 1115;
        const int IDC_RADIO_1_5 = 1116;
        const int IDC_RADIO_1_4 = 1117;
        const int IDC_RADIO_1_3 = 1118;
        const int IDC_RADIO_1_2 = 1119;
        const int IDC_RADIO_2_6 = 1120;
        const int IDC_RADIO_2_5 = 1121;
        const int IDC_RADIO_2_4 = 1122;
        const int IDC_RADIO_2_3 = 1123;
        const int IDC_RADIO_3_6 = 1124;
        const int IDC_RADIO_3_5 = 1125;
        const int IDC_RADIO_3_4 = 1126;
        const int IDC_RADIO_4_6 = 1127;
        const int IDC_RADIO_4_5 = 1128;
        const int IDC_RADIO_5_6 = 1129;

        const int IDC_BUTTON_RESET = 1035;
        const int IDC_BUTTON_SYN = 1036;
        const int IDC_BUTTON_OK = 1037;
        const int IDC_BUTTON_CANCEL = 1038;
        const int IDC_RADIO_NWE = 1147;
        const int IDC_RADIO_NEXT = 1148;

        const int IDC_COMBO_TIMES = 1011;
        const int IDC_STATIC_TIMES = 1039;
        const int IDC_STATIC_AREA = 1040;
        const int IDC_STATIC_NOTIC = 1041;
        const int IDC_STATIC_TEXT = 1042;
        const int IDC_STATIC_ALL_BET = 1149;

        const int IDC_ST_AREA1 =                   1150;
        const int IDC_ST_AREA2   =                 1151;
        const int IDC_ST_AREA3    =                1152;
        const int IDC_ST_AREA4     =               1153;
        const int IDC_ST_AREA5      =              1154;
        const int IDC_ST_AREA6       =             1155;
        const int IDC_ST_AREA7        =            1156;
        const int IDC_ST_AREA8         =           1157;
        const int IDC_ST_AREA9          =          1158;
        const int IDC_ST_AREA10          =         1159;
        const int IDC_ST_AREA11           =        1160;
        const int IDC_ST_AREA12            =       1161;
        const int IDC_ST_AREA13             =      1162;
        const int IDC_ST_AREA14              =     1163;
        const int IDC_ST_AREA15               =    1164;

        const int IDC_EDIT_INFO = 1146;

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

            //m_radioArea[HorseDefine.AREA_1_6] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_1_5] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_1_4] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_1_3] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_1_2] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_2_6] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_2_5] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_2_4] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_2_3] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_3_6] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_3_5] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_3_4] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_4_6] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_4_5] = new CRadioButton();
            //m_radioArea[HorseDefine.AREA_5_6] = new CRadioButton();


            //DDX_Control(pDX, IDC_EDIT_C_1_6, m_editInput[HorseDefine.AREA_1_6]);
            //DDX_Control(pDX, IDC_EDIT_C_1_5, m_editInput[HorseDefine.AREA_1_5]);
            //DDX_Control(pDX, IDC_EDIT_C_1_4, m_editInput[HorseDefine.AREA_1_4]);
            //DDX_Control(pDX, IDC_EDIT_C_1_3, m_editInput[HorseDefine.AREA_1_3]);
            //DDX_Control(pDX, IDC_EDIT_C_1_2, m_editInput[HorseDefine.AREA_1_2]);
            //DDX_Control(pDX, IDC_EDIT_C_2_6, m_editInput[HorseDefine.AREA_2_6]);
            //DDX_Control(pDX, IDC_EDIT_C_2_5, m_editInput[HorseDefine.AREA_2_5]);
            //DDX_Control(pDX, IDC_EDIT_C_2_4, m_editInput[HorseDefine.AREA_2_4]);
            //DDX_Control(pDX, IDC_EDIT_C_2_3, m_editInput[HorseDefine.AREA_2_3]);
            //DDX_Control(pDX, IDC_EDIT_C_3_6, m_editInput[HorseDefine.AREA_3_6]);
            //DDX_Control(pDX, IDC_EDIT_C_3_5, m_editInput[HorseDefine.AREA_3_5]);
            //DDX_Control(pDX, IDC_EDIT_C_3_4, m_editInput[HorseDefine.AREA_3_4]);
            //DDX_Control(pDX, IDC_EDIT_C_4_6, m_editInput[HorseDefine.AREA_4_6]);
            //DDX_Control(pDX, IDC_EDIT_C_4_5, m_editInput[HorseDefine.AREA_4_5]);
            //DDX_Control(pDX, IDC_EDIT_C_5_6, m_editInput[HorseDefine.AREA_5_6]);

            //DDX_Control(pDX, IDC_RADIO_1_6, m_radioArea[HorseDefine.AREA_1_6]);
            //DDX_Control(pDX, IDC_RADIO_1_5, m_radioArea[HorseDefine.AREA_1_5]);
            //DDX_Control(pDX, IDC_RADIO_1_4, m_radioArea[HorseDefine.AREA_1_4]);
            //DDX_Control(pDX, IDC_RADIO_1_3, m_radioArea[HorseDefine.AREA_1_3]);
            //DDX_Control(pDX, IDC_RADIO_1_2, m_radioArea[HorseDefine.AREA_1_2]);
            //DDX_Control(pDX, IDC_RADIO_2_6, m_radioArea[HorseDefine.AREA_2_6]);
            //DDX_Control(pDX, IDC_RADIO_2_5, m_radioArea[HorseDefine.AREA_2_5]);
            //DDX_Control(pDX, IDC_RADIO_2_4, m_radioArea[HorseDefine.AREA_2_4]);
            //DDX_Control(pDX, IDC_RADIO_2_3, m_radioArea[HorseDefine.AREA_2_3]);
            //DDX_Control(pDX, IDC_RADIO_3_6, m_radioArea[HorseDefine.AREA_3_6]);
            //DDX_Control(pDX, IDC_RADIO_3_5, m_radioArea[HorseDefine.AREA_3_5]);
            //DDX_Control(pDX, IDC_RADIO_3_4, m_radioArea[HorseDefine.AREA_3_4]);
            //DDX_Control(pDX, IDC_RADIO_4_6, m_radioArea[HorseDefine.AREA_4_6]);
            //DDX_Control(pDX, IDC_RADIO_4_5, m_radioArea[HorseDefine.AREA_4_5]);
            //DDX_Control(pDX, IDC_RADIO_5_6, m_radioArea[HorseDefine.AREA_5_6]);

            DDX_Control(pDX, IDC_COMBO_TIMES, m_comboTime);
        }


        public override void BEGIN_MESSAGE_MAP()
        {
	        ON_BN_CLICKED(IDC_BUTTON_RESET, OnBnClickedButtonReset);
	        ON_BN_CLICKED(IDC_BUTTON_SYN, OnBnClickedButtonSyn);
	        ON_BN_CLICKED(IDC_BUTTON_OK, OnBnClickedButtonOk);
	        ON_BN_CLICKED(IDC_BUTTON_CANCEL, OnBnClickedButtonCancel);
	        ON_BN_CLICKED(IDC_RADIO_NWE, OnBnClickedRadioNwe);
	        ON_BN_CLICKED(IDC_RADIO_NEXT, OnBnClickedRadioNext);
        }


        // CDialogControl 消息处理程序
        //初始化
        private void DialogControl_Load(object sender, EventArgs e)
        {
            CComboBox comboBox = ((CComboBox)GetDlgItem(IDC_COMBO_TIMES));

            comboBox.AddString("1");
            comboBox.AddString("2");
            comboBox.AddString("3");
            comboBox.AddString("4");
            comboBox.AddString("5");

	        SetDlgItemText(IDC_STATIC_TIMES,"控制局数：");
	        SetDlgItemText(IDC_STATIC_AREA, "区域控制：");
	        SetDlgItemText(IDC_STATIC_NOTIC, "控制说明：");
	        SetDlgItemText(IDC_STATIC_TEXT,"区域输赢控制比游戏库存控制策略优先！");
        	
	        SetDlgItemText(IDC_STATIC_ALL_BET,"下注总览");
	        SetDlgItemText(IDC_ST_AREA1,"1-6");
	        SetDlgItemText(IDC_ST_AREA2,"1-5");
	        SetDlgItemText(IDC_ST_AREA3,"1-4");
	        SetDlgItemText(IDC_ST_AREA4,"1-3");
	        SetDlgItemText(IDC_ST_AREA5,"1-2");

	        SetDlgItemText(IDC_ST_AREA6,"2-6");
	        SetDlgItemText(IDC_ST_AREA7,"2-5");
	        SetDlgItemText(IDC_ST_AREA8,"2-4");
	        SetDlgItemText(IDC_ST_AREA9,"2-3");

	        SetDlgItemText(IDC_ST_AREA10,"3-6");
	        SetDlgItemText(IDC_ST_AREA11,"3-5");
	        SetDlgItemText(IDC_ST_AREA12,"3-4");

	        SetDlgItemText(IDC_ST_AREA13,"4-6");
	        SetDlgItemText(IDC_ST_AREA14,"4-5");

	        SetDlgItemText(IDC_ST_AREA15,"5-6");

	        SetDlgItemText(IDC_RADIO_1_6,"1-6");
	        SetDlgItemText(IDC_RADIO_1_5,"1-5");
	        SetDlgItemText(IDC_RADIO_1_4,"1-4");
	        SetDlgItemText(IDC_RADIO_1_3,"1-3");
	        SetDlgItemText(IDC_RADIO_1_2,"1-2");

	        SetDlgItemText(IDC_RADIO_2_6,"2-6");
	        SetDlgItemText(IDC_RADIO_2_5,"2-5");
	        SetDlgItemText(IDC_RADIO_2_4,"2-4");
	        SetDlgItemText(IDC_RADIO_2_3,"2-3");

	        SetDlgItemText(IDC_RADIO_3_6,"3-6");
	        SetDlgItemText(IDC_RADIO_3_5,"3-5");
	        SetDlgItemText(IDC_RADIO_3_4,"3-4");

	        SetDlgItemText(IDC_RADIO_4_6,"4-6");
	        SetDlgItemText(IDC_RADIO_4_5,"4-5");

	        SetDlgItemText(IDC_RADIO_5_6,"5-6");

	        SetDlgItemText(IDC_RADIO_NWE,"立即执行(无法设置倍数)");
	        SetDlgItemText(IDC_RADIO_NEXT,"下局执行");

	        SetDlgItemText(IDC_BUTTON_RESET,"取消控制");
	        SetDlgItemText(IDC_BUTTON_SYN,"本局信息");
	        SetDlgItemText(IDC_BUTTON_OK,"执行");
	        SetDlgItemText(IDC_BUTTON_CANCEL,"取消");

            ((CRadioButton)GetDlgItem(IDC_RADIO_NWE)).SetCheck(false);
            ((CRadioButton)GetDlgItem(IDC_RADIO_NEXT)).SetCheck(true);
        }


        //设置颜色
        public override void OnCtlColor()
        {
            for( int i = 0; i < m_editInput.Length; i++ )
            {
                if(m_editInput[i].GetDlgCtrlID() == IDC_EDIT_INFO)
                {
                    m_editInput[i].SetTextColor(Color.FromArgb(255,10,10));
                    break;
                }
            }
        }

        //取消控制
        void OnBnClickedButtonReset()
        {
	        CMD_C_ControlApplication ControlApplication = new CMD_C_ControlApplication();
            ControlApplication.ZeroMemory();

	        ControlApplication.cbControlAppType = HorseDefine.C_CA_CANCELS;
            ((GameView)this.Parent).NotifyMessage( HorseDefine.IDM_ADMIN_COMMDN, ControlApplication, 0 );
        }

        //本局控制
        void OnBnClickedButtonSyn()
        {
	        CMD_C_ControlApplication ControlApplication = new CMD_C_ControlApplication();
            ControlApplication.ZeroMemory();

	        ControlApplication.cbControlAppType = HorseDefine.C_CA_CANCELS;
            ((GameView)this.Parent).NotifyMessage( HorseDefine.IDM_ADMIN_COMMDN, ControlApplication, 0 );
        }

        //开启控制
        void OnBnClickedButtonOk()
        {
	        //定义变量
	        CMD_C_ControlApplication ControlApplication = new CMD_C_ControlApplication();

	        //初始变量
	        ControlApplication.cbControlTimes = 0;
	        ControlApplication.cbControlAppType = HorseDefine.C_CA_CANCELS;
	        ControlApplication.cbControlArea = 255;
	        for ( int i = 0; i < HorseDefine.AREA_ALL; ++i)
		        ControlApplication.nControlMultiple[i] = -1;

	        //获得执行时间
	        if ( ((CRadioButton)GetDlgItem(IDC_RADIO_NWE)).GetCheck() )
	        {
		        ControlApplication.bAuthoritiesExecuted = true;
	        }
	        else if( ((CRadioButton)GetDlgItem(IDC_RADIO_NEXT)).GetCheck() )
	        {
		        ControlApplication.bAuthoritiesExecuted = false;
	        }
        	
	        //获得控制区域
	        bool bControlArea = false;
	        for (int i = 0; i < HorseDefine.AREA_ALL; ++i )
	        {
		        if ( m_radioArea[i].GetCheck() )
		        {
			        ControlApplication.cbControlArea = (Byte)i;
			        bControlArea = true;
			        break;
		        }
	        }

	        //获得控制倍率
	        bool bControlMultiple = false;
	        for ( int i = 0 ; i < HorseDefine.AREA_ALL; ++i)
	        {
                string strCount = m_editInput[i].GetWindowText();
                int nTemp = ConverToInt(strCount);

		        if ( nTemp > 0 )
		        {
			        bControlMultiple = true;
			        ControlApplication.nControlMultiple[i] = nTemp;
		        }
	        }

	        //获得时间
	        int nSelectTimes = ((CComboBox)GetDlgItem(IDC_COMBO_TIMES)).GetCurSel();

	        if (  ( bControlMultiple || bControlArea ) && nSelectTimes >= 0 && nSelectTimes != -1 )
	        {
		        ControlApplication.cbControlAppType = HorseDefine.C_CA_CANCELS;
		        ControlApplication.cbControlTimes = (Byte)(nSelectTimes + 1);
                ((GameView)this.Parent).NotifyMessage( HorseDefine.IDM_ADMIN_COMMDN, ControlApplication, 0 );
	        }
	        else
	        {
		        if ( nSelectTimes == -1 )
		        {
			        SetDlgItemText(IDC_EDIT_INFO,"请选择受控次数！");
		        }
		        else if ( !bControlMultiple && !bControlArea )
		        {
			        SetDlgItemText(IDC_EDIT_INFO,"请选择控制区域或控制倍率！");
		        }
	        }
        }

        //取消关闭
        void OnBnClickedButtonCancel()
        {
	        ShowWindow(false);
        }

        //当局生效按钮
        void OnBnClickedRadioNwe()
        {
	        if(((CRadioButton)GetDlgItem(IDC_RADIO_NWE)).GetCheck())
	        {
		        for ( int i = 0; i < HorseDefine.AREA_ALL; ++i )
		        {
			        m_editInput[i].SetWindowText(string.Empty);
			        m_editInput[i].EnableWindow(false);
		        }
	        }
        }

        //下局生效按钮
        void OnBnClickedRadioNext()
        {
	        if(((CRadioButton)GetDlgItem(IDC_RADIO_NEXT)).GetCheck())
	        {
		        for ( int i = 0; i < HorseDefine.AREA_ALL; ++i )
		        {
			        m_editInput[i].EnableWindow(true);
		        }
	        }
        }

        //更新控制
        //void UpdateControl( CMD_S_ControlReturns pControlReturns )
        //{
        //    switch(pControlReturns.cbReturnsType)
        //    {
        //    case HorseDefine.S_CR_FAILURE:
        //        {
        //            for (int i = 0; i < HorseDefine.AREA_ALL; ++i )
        //            {
        //                m_radioArea[i].SetCheck(false);
        //                m_editInput[i].SetWindowText(string.Empty);
        //            }
        //            SetDlgItemText(IDC_EDIT_INFO,"操作失败！");
        //            break;
        //        }
        //    case HorseDefine.S_CR_UPDATE_SUCCES:
        //        {
        //            string strText = string.Empty;
        //            string strTextTemp = string.Empty;

        //            PrintingInfo(strTextTemp,512,pControlReturns->cbControlArea,pControlReturns->nControlMultiple,pControlReturns->cbControlTimes);
        //            strText = string.Format("更新数据成功！\r\n {0}", strTextTemp);

        //            SetDlgItemText(IDC_EDIT_INFO,strText);
        //            break;
        //        }
        //    case HorseDefine.S_CR_SET_SUCCESS:
        //        {
        //            string strText = string.Empty;
        //            string strTextTemp = string.Empty;

        //            PrintingInfo(strTextTemp,512,pControlReturns->cbControlArea,pControlReturns->nControlMultiple,pControlReturns->cbControlTimes);
        //            if( pControlReturns->bAuthoritiesExecuted )
        //            {
        //                strText = string.Format("设置数据成功！开始执行！\r\n {0}", strTextTemp);
        //            }
        //            else
        //            {
        //                strText = string.Format("设置数据成功！下一局开始执行！\r\n {0}", strTextTemp);
        //            }

        //            SetDlgItemText(IDC_EDIT_INFO,strText);
        //            break;
        //        }
        //    case HorseDefine.S_CR_CANCEL_SUCCESS:
        //        {
        //            for (int i = 0; i < HorseDefine.AREA_ALL; ++i )
        //            {
        //                m_radioArea[i].SetCheck(false);
        //                m_editInput[i].SetWindowText(string.Empty);
        //            }
        //            SetDlgItemText(IDC_EDIT_INFO,"取消设置成功！");
        //            break;
        //        }
        //    }
        //}

        //信息
        //void PrintingInfo( string strText, int cbCount, int cbArea, int[] nMultiple, byte cbTimes )
        //{	        
        //    string strDesc = string.Empty;

        //    //获取控制区域以及倍数
        //    strDesc = string.Format("{0}", "控制区域：");

        //    if ( cbArea >= HorseDefine.AREA_1_6 && cbArea < HorseDefine.AREA_ALL )
        //    {
        //        strDesc = string.Format("{0}, ", cbArea);

        //        strText += strDesc;

        //        if ( nMultiple[cbArea] > 0 )
        //        {
        //            strDesc =  string.Format("胜利倍数：{0}，", nMultiple[cbArea]);
        //            strText += strDesc;
        //        }
        //        else
        //        {
        //            strDesc =  "胜利倍数：服务器默认值，";
        //            strText += strDesc;
        //        }

        //        strDesc =  string.Format("执行次数：{0}。\r\n ", nMultiple[cbArea]);
        //        strText += strDesc;
        //    }
        //    else 
        //    {
        //        strDesc =  "暂无。\r\n ";
        //        strText += strDesc;
        //    }

        //    //倍数设置
        //    int nMultipleCount = 0;
        //    for ( int i = 0; i < HorseDefine.AREA_ALL; ++i)
        //    {
        //        if ( nMultiple[i] > 0)
        //        {
        //            if ( nMultipleCount == 0 )
        //            {
        //                strDesc =  "控制倍数：";
        //                strText += strDesc;
        //            }

        //            strDesc =  string.Format("【{0}：{1}】，",  ObtainArea(i), nMultiple[i]);
        //            strText += strDesc;

        //            nMultipleCount++;
        //        }
        //    }
        //    if ( nMultipleCount == 0 )
        //    {
        //        strDesc = "控制倍数：暂无（按服务默认值处理）。\r\n";
        //        strText += strDesc;
        //    }
        //    else if ( nMultipleCount > 0 && nMultipleCount < HorseDefine.AREA_ALL )
        //    {
        //        strDesc = "其余未控制区域倍数按服务默认值处理。\r\n";
        //        strText += strDesc;
        //    }
        //}

        //string ObtainArea( int cbArea )
        //{
        //    string str = string.Empty;

        //    if ( cbArea < HorseDefine.AREA_1_6 && cbArea >= HorseDefine.AREA_ALL )
        //    {
        //        ASSERT(false);
        //        return str;
        //    }

        //    if ( cbArea == HorseDefine.AREA_1_6 )
        //        str = "1-6";
        //    else if ( cbArea == HorseDefine.AREA_1_5 )
        //        str = "1-5";
        //    else if ( cbArea == HorseDefine.AREA_1_4 )
        //        str = "1-4";
        //    else if ( cbArea == HorseDefine.AREA_1_3 )
        //        str = "1-3";
        //    else if ( cbArea == HorseDefine.AREA_1_2 )
        //        str = "1-2";
        //    else if ( cbArea == HorseDefine.AREA_2_6 )
        //        str = "2-6";
        //    else if ( cbArea == HorseDefine.AREA_2_5 )
        //        str = "2-5";
        //    else if ( cbArea == HorseDefine.AREA_2_4 )
        //        str = "2-4";
        //    else if ( cbArea == HorseDefine.AREA_2_3 )
        //        str = "2-3";
        //    else if ( cbArea == HorseDefine.AREA_2_6 )
        //        str = "3-6";
        //    else if ( cbArea == HorseDefine.AREA_3_5 )
        //        str = "3-5";
        //    else if ( cbArea == HorseDefine.AREA_3_4 )
        //        str = "3-4";
        //    else if ( cbArea == HorseDefine.AREA_4_6 )
        //        str = "4-6";
        //    else if ( cbArea == HorseDefine.AREA_4_5 )
        //        str = "4-5";
        //    else if ( cbArea == HorseDefine.AREA_5_6 )
        //        str = "5-6";

        //    return str;
        //}

        public void ResetUserBet()
        {
	        string strPrint = string.Empty;

	        for ( int i = 0; i < m_lAllUserBet.Length; i++)
		        m_lAllUserBet[i] = 0;

	        SetDlgItemText(IDC_ST_AREA1,"1-6");
	        SetDlgItemText(IDC_ST_AREA2,"1-5");
	        SetDlgItemText(IDC_ST_AREA3,"1-4");
	        SetDlgItemText(IDC_ST_AREA4,"1-3");
	        SetDlgItemText(IDC_ST_AREA5,"1-2");

	        SetDlgItemText(IDC_ST_AREA6,"2-6");
	        SetDlgItemText(IDC_ST_AREA7,"2-5");
	        SetDlgItemText(IDC_ST_AREA8,"2-4");
	        SetDlgItemText(IDC_ST_AREA9,"2-3");

	        SetDlgItemText(IDC_ST_AREA10,"3-6");
	        SetDlgItemText(IDC_ST_AREA11,"3-5");
	        SetDlgItemText(IDC_ST_AREA12,"3-4");

	        SetDlgItemText(IDC_ST_AREA13,"4-6");
	        SetDlgItemText(IDC_ST_AREA14,"4-5");

	        SetDlgItemText(IDC_ST_AREA15,"5-6");
        }

        public void SetUserBetScore(int area, int score)
        {
            string[] strPrint = new string[HorseDefine.AREA_ALL];

            SetDlgItemText(IDC_ST_AREA1, "1-6");
            SetDlgItemText(IDC_ST_AREA2, "1-5");
            SetDlgItemText(IDC_ST_AREA3, "1-4");
            SetDlgItemText(IDC_ST_AREA4, "1-3");
            SetDlgItemText(IDC_ST_AREA5, "1-2");

            SetDlgItemText(IDC_ST_AREA6, "2-6");
            SetDlgItemText(IDC_ST_AREA7, "2-5");
            SetDlgItemText(IDC_ST_AREA8, "2-4");
            SetDlgItemText(IDC_ST_AREA9, "2-3");

            SetDlgItemText(IDC_ST_AREA10, "3-6");
            SetDlgItemText(IDC_ST_AREA11, "3-5");
            SetDlgItemText(IDC_ST_AREA12, "3-4");

            SetDlgItemText(IDC_ST_AREA13, "4-6");
            SetDlgItemText(IDC_ST_AREA14, "4-5");

            SetDlgItemText(IDC_ST_AREA15, "5-6");

            for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
            {
                if( i == area )
                    m_lAllUserBet[i] += score;

                switch (i)//IDC_ST_AREA1
                {
                    case 0: strPrint[i] = string.Format("1-6:{0}", m_lAllUserBet[i]); break;
                    case 1: strPrint[i] = string.Format("1-5:{0}", m_lAllUserBet[i]); break;
                    case 2: strPrint[i] = string.Format("1-4:{0}", m_lAllUserBet[i]); break;
                    case 3: strPrint[i] = string.Format("1-3:{0}", m_lAllUserBet[i]); break;
                    case 4: strPrint[i] = string.Format("1-2:{0}", m_lAllUserBet[i]); break;

                    case 5: strPrint[i] = string.Format("2-6:{0}", m_lAllUserBet[i]); break;
                    case 6: strPrint[i] = string.Format("2-5:{0}", m_lAllUserBet[i]); break;
                    case 7: strPrint[i] = string.Format("2-4:{0}", m_lAllUserBet[i]); break;
                    case 8: strPrint[i] = string.Format("2-3:{0}", m_lAllUserBet[i]); break;

                    case 9: strPrint[i] = string.Format("3-6:{0}", m_lAllUserBet[i]); break;
                    case 10: strPrint[i] = string.Format("3-5:{0}", m_lAllUserBet[i]); break;
                    case 11: strPrint[i] = string.Format("3-4:{0}", m_lAllUserBet[i]); break;
                    case 12: strPrint[i] = string.Format("4-6:{0}", m_lAllUserBet[i]); break;
                    case 13: strPrint[i] = string.Format("4-5:{0}", m_lAllUserBet[i]); break;
                    case 14: strPrint[i] = string.Format("5-6:{0}", m_lAllUserBet[i]); break;

                    default: break;
                }

                //if(GetSafeHwnd())
                    SetDlgItemText(IDC_ST_AREA1 + i, strPrint[i]);
            }
        }
    }

    public class CMD_C_ControlApplication
    {
        public Byte cbControlAppType;				            //申请类型
        public Byte cbControlTimes;				            //控制次数
        public Byte cbControlArea;					            //控制区域
        public bool bAuthoritiesExecuted;			            //当局执行
        public int[] nControlMultiple = new int[HorseDefine.AREA_ALL];	//控制倍率    	

        public void ZeroMemory()
        {
            cbControlAppType = 0;
            cbControlTimes = 0;
            cbControlArea = 0;
            bAuthoritiesExecuted = false;

            for (int i = 0; i < nControlMultiple.Length; i++)
            {
                nControlMultiple[i] = 0;
            }
        }
    };

}
