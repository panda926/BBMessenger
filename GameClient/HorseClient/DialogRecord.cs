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
    public partial class DialogRecord : CDialog
    {
        //游戏记录
        public List<tagHistoryRecord> m_GameRecords = new List<tagHistoryRecord>();			//游戏记录

        //游戏变量
        int m_lPlayerScore;						//玩家积分
        int m_lAllBet;							//本局投注
        int m_lBetMumber;						//投注人数

        //资源变量
        CSize m_szTotalSize;						//总大小
        CFont m_InfoFont;							//字体
        CPngImage m_ImageBackdropHand;				//背景头
        CPngImage m_ImageBackdropTail;				//背景尾
        CPngImage m_ImageBackdrop;					//背景中
        CSkinButton m_btOk = new CSkinButton();								//OK

        public DialogRecord()
        {
            InitializeComponent();

            // modified by usc at 2013/07/21
            m_InfoFont.CreateFont(10,0,0,0,400,0,0,0,134,3,2,CFont.ANTIALIASED_QUALITY,2,TEXT("宋体"));
	        m_lPlayerScore = 0;
	        m_lAllBet = 0;
	        m_lBetMumber = 0;

            InitDialog();
        }

        const int IDC_BUTTON_LOGOK           =     1099;

        public override void DoDataExchange()
        {
            Control pDX = this;
	        DDX_Control(pDX, IDC_BUTTON_LOGOK, m_btOk);
        }


        public override void BEGIN_MESSAGE_MAP()
        {
            ON_BN_CLICKED(IDC_BUTTON_LOGOK, OnBnClickedButtonLogok);
        }


        // CDialogRecord 消息处理程序

        public override bool OnInitDialog()
        {
	        Control hInstance = this;

            //m_ImageBackdrop.LoadFromResource( hInstance, Properties.Resources.HISTORYSCORE_M );
            //m_ImageBackdropHand.LoadFromResource( hInstance, Properties.Resources.HISTORYSCORE_T );
            //m_ImageBackdropTail.LoadFromResource( hInstance, Properties.Resources.HISTORYSCORE_B );
            //m_btOk.SetButtonImage( Properties.Resources.BT_CLOSE_HISTORYSCORE,  hInstance , false, false);

	        //ModifyStyle(0, WS_CLIPCHILDREN, 0);

	        m_szTotalSize.SetSize(m_ImageBackdrop.GetWidth(), m_ImageBackdrop.GetHeight() * HorseDefine.MAX_SCORE_HISTORY + m_ImageBackdropHand.GetHeight() + m_ImageBackdropTail.GetHeight());
	        SetWindowPos(null, 0, 0, m_szTotalSize.cx, m_szTotalSize.cy, SWP_NOACTIVATE|SWP_NOZORDER|SWP_NOCOPYBITS|SWP_NOMOVE);

	        m_btOk.MoveWindow(m_szTotalSize.cx/2 - 11, m_szTotalSize.cy - 25, 23, 20);


	        return true; 
        }

        //BOOL OnEraseBkgnd(CDC* pDC)
        //{
        //    return true;
        //}

        private void DialogRecord_Paint(object sender, PaintEventArgs e)
        {
	        CDC dc = new CDC();
            dc.SetGraphics(e.Graphics); 

	        //获取位置
	        CRect rcClient = new CRect();
	        GetClientRect( ref rcClient);

	        //创建缓冲
	        CDC DCBuffer = new CDC();
	        CBitmap ImageBuffer = new CBitmap();
	        ImageBuffer.CreateCompatibleBitmap(dc,rcClient.Width(),rcClient.Height());
	        DCBuffer.CreateCompatibleDC(ImageBuffer);

	        //设置 DC
	        //DCBuffer.SetBkMode(TRANSPARENT);
	        //CBitmap* oldBitmap = DCBuffer.SelectObject(&ImageBuffer);
	        DCBuffer.SelectObject(m_InfoFont);
	        DCBuffer.SetTextAlign(CDC.TA_TOP|CDC.TA_LEFT);
	        DCBuffer.SetTextColor(Color.FromArgb(250,250,255));

	        //背景
	        m_ImageBackdropHand.BitBlt(DCBuffer.GetSafeHdc(), 0, 0);

	        int nBcakY = m_ImageBackdropHand.GetHeight();
	        for ( int i = 0; i < HorseDefine.MAX_SCORE_HISTORY; ++i)
	        {
		        m_ImageBackdrop.BitBlt(DCBuffer.GetSafeHdc(), 0, nBcakY);
		        nBcakY += m_ImageBackdrop.GetHeight();
	        }
	        m_ImageBackdropTail.BitBlt(DCBuffer.GetSafeHdc(), 0, nBcakY);

	        //写记录
	        int nRecordsY = m_ImageBackdropHand.GetHeight();
	        string szInfo = string.Empty;
	        CRect rect = new CRect();
	        for ( int i = 0; i < m_GameRecords.Count; ++i)
	        {
		        rect.SetRect( 0, nRecordsY, 28, nRecordsY + 15);
		        szInfo = m_GameRecords[i].nStreak.ToString();
                DCBuffer.DrawText(szInfo, rect, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_CENTER);

		        rect.SetRect( 28, nRecordsY, 57, nRecordsY + 15);
		        szInfo = IdentifyAreas(m_GameRecords[i].nRanking);
		        DCBuffer.DrawText(szInfo, rect , CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER );

		        rect.SetRect( 57, nRecordsY, 86, nRecordsY + 15);
		        szInfo = m_GameRecords[i].nRiskCompensate.ToString();
		        DCBuffer.DrawText(szInfo, rect , CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER );
        		
		        rect.SetRect( 86, nRecordsY, 151, nRecordsY + 15);
		        szInfo = string.Format( "{0}{1}:{2}{3}:{4}{5}", 
			        m_GameRecords[i].nHours >= 10 ? TEXT("") : TEXT("0"), m_GameRecords[i].nHours, 
			        m_GameRecords[i].nMinutes >= 10 ? TEXT("") : TEXT("0"), m_GameRecords[i].nMinutes, 
			        m_GameRecords[i].nSeconds >= 10 ? TEXT("") : TEXT("0"), m_GameRecords[i].nSeconds);
		        DCBuffer.DrawText(szInfo, rect , CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER );
		        nRecordsY += 15;
	        }
        	
	        nRecordsY = m_szTotalSize.cy - 85;
	        rect.SetRect( 5, nRecordsY, 146, nRecordsY + 15);
	        szInfo = string.Format( "本局总投注：{0}", m_lAllBet);
	        DCBuffer.DrawText(szInfo, rect , CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_LEFT );

	        nRecordsY += 15;
	        rect.SetRect( 5, nRecordsY, 146, nRecordsY + 15);
	        szInfo = string.Format("本局投注人数：{0}", m_lBetMumber);
	        DCBuffer.DrawText(szInfo, rect , CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_LEFT );

	        nRecordsY += 20;
	        rect.SetRect( 5, nRecordsY, 146, nRecordsY + 15);
	        szInfo = string.Format( "您的余额为：{0}", m_lPlayerScore);
	        DCBuffer.DrawText(szInfo, rect , CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_LEFT );

	        //绘画界面
	        dc.BitBlt(0,0,rcClient.Width(),rcClient.Height(),ImageBuffer,0,0,0);

	        //清理资源
            //DCBuffer.SetTextColor(oldColor);
            //DCBuffer.SetTextAlign(nTextAlign);
            //DCBuffer.SelectObject(oldBitmap);
            //DCBuffer.SelectObject(oldFont);
            //DCBuffer.DeleteDC();
            //ImageBuffer.DeleteObject();
        }

        void OnBnClickedButtonLogok()
        {
	        this.Visible = false;
        }

        //设置积分
        public void SetScore( int lScore, int lAllBet, int lBetMumber )
        {
	        if( m_lPlayerScore != lScore || m_lAllBet != lAllBet || m_lBetMumber != lBetMumber )
	        {
		        m_lPlayerScore = lScore;
		        m_lAllBet = lAllBet;
		        m_lBetMumber = lBetMumber;

		        if( IsWindowVisible())
		        {
			        Invalidate();
		        }
	        }
        }

        //区域辨认
        string IdentifyAreas( int cbArea )
        {
	        string str = TEXT("");

            str = string.Format("{0}", cbArea);

            //if ( cbArea == HorseDefine.AREA_1_6 )
            //{
            //    str = TEXT("1-6");
            //}
            //else if( cbArea == HorseDefine.AREA_1_5 )
            //{
            //    str = TEXT("1-5");
            //}
            //else if( cbArea == HorseDefine.AREA_1_4 )
            //{
            //    str = TEXT("1-4");
            //}
            //else if( cbArea == HorseDefine.AREA_1_3 )
            //{
            //    str = TEXT("1-3");
            //}
            //else if( cbArea == HorseDefine.AREA_1_2 )
            //{
            //    str = TEXT("1-2");
            //}
            //else if( cbArea == HorseDefine.AREA_2_6 )
            //{
            //    str = TEXT("2-6");
            //}
            //else if( cbArea == HorseDefine.AREA_2_5 )
            //{
            //    str = TEXT("2-5");
            //}
            //else if( cbArea == HorseDefine.AREA_2_4 )
            //{
            //    str = TEXT("2-4");
            //}
            //else if( cbArea == HorseDefine.AREA_2_3 )
            //{
            //    str = TEXT("2-3");
            //}
            //else if( cbArea == HorseDefine.AREA_3_6 )
            //{
            //    str = TEXT("3-6");
            //}
            //else if( cbArea == HorseDefine.AREA_3_5 )
            //{
            //    str = TEXT("3-5");
            //}
            //else if( cbArea == HorseDefine.AREA_3_4 )
            //{
            //    str = TEXT("3-4");
            //}
            //else if( cbArea == HorseDefine.AREA_4_6 )
            //{
            //    str = TEXT("4-6");
            //}
            //else if( cbArea == HorseDefine.AREA_4_5 )
            //{
            //    str = TEXT("4-5");
            //}
            //else if( cbArea == HorseDefine.AREA_5_6 )
            //{
            //    str = TEXT("5-6");
            //}
            //else 
            //{
            //    ASSERT(false);
            //}
	        return str;
        }

    }
}
