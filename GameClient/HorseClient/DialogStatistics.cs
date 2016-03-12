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
    public partial class DialogStatistics : CDialog
    {
	    //游戏变量
	    int[]				m_nWinCount = new int[HorseDefine.HORSES_ALL];			//全天赢的场次

	    //资源变量
	    CFont				m_InfoFont;							//字体
	    CPngImage			m_ImageLine;						//线
        CPngImage           m_ImageBackdrop;					//背景
	    CSkinButton			m_btClose = new CSkinButton();							//关闭
	    CSkinButton			m_btDetermine = new CSkinButton();						//确定

        public DialogStatistics()
        {
            InitializeComponent();

            m_InfoFont.CreateFont(12,0,0,0,400,0,0,0,134,3,2,CFont.ANTIALIASED_QUALITY,2,TEXT("宋体"));
	        //ZeroMemory(m_nWinCount, sizeof(m_nWinCount));

            InitDialog();
        }

        const int IDC_BUTTON_TJ_CLOSE         =    1100;
        const int IDC_BUTTON_TJ_CLOSE_2       =    1102;

        public override void DoDataExchange()
        {
            Control pDX = this;
	        DDX_Control(pDX, IDC_BUTTON_TJ_CLOSE, m_btClose);
	        DDX_Control(pDX, IDC_BUTTON_TJ_CLOSE_2, m_btDetermine);
        }

        public override void BEGIN_MESSAGE_MAP()
        {
	        ON_BN_CLICKED(IDC_BUTTON_TJ_CLOSE, OnBnClickedButtonTjClose);
	        ON_BN_CLICKED(IDC_BUTTON_TJ_CLOSE_2, OnBnClickedButtonTjClose2);
        }


        // CDialogStatistics 消息处理程序

        public override bool OnInitDialog()
        {
	        Control hInstance = this;
            //m_ImageBackdrop.LoadFromResource( hInstance,Properties.Resources.TONGJI_FRAME );
            //m_ImageLine.LoadFromResource( hInstance,Properties.Resources.LINE );
            //m_btClose.SetButtonImage( Properties.Resources.BT_CLOSE,  hInstance , false,false);
            //m_btDetermine.SetButtonImage( Properties.Resources.BT_MESSAGE,  hInstance , false,false);

	        //ModifyStyle(0, WS_CLIPCHILDREN, 0);

	        //CImageHandle HandleBack(&m_ImageBackdrop);
	        SetWindowPos(null, 0, 0, m_ImageBackdrop.GetWidth(), m_ImageBackdrop.GetHeight(), SWP_NOACTIVATE|SWP_NOZORDER|SWP_NOCOPYBITS|SWP_NOMOVE);

	        m_btClose.MoveWindow(m_ImageBackdrop.GetWidth() - 26, 1, 25, 25);
	        m_btDetermine.MoveWindow(m_ImageBackdrop.GetWidth()/2 - 30, 330, 60, 22);
	        return true; 
        }

        //BOOL OnEraseBkgnd(CDC* pDC)
        //{
        //    return TRUE;
        //}

        private void DialogStatistics_Paint(object sender, PaintEventArgs e)
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
	        m_ImageBackdrop.BitBlt(DCBuffer.GetSafeHdc(), 0, 0);

	        //线
	        CPoint point = new CPoint();
	        point.SetPoint(51, 284);
	        for ( int i = 0; i < HorseDefine.HORSES_ALL; ++i )
	        {
		        for ( int j = 0 ; j < m_nWinCount[i]; ++j)
		        {
			        m_ImageLine.BitBlt(DCBuffer.GetSafeHdc(), point.x, point.y);
			        point.y -= 1;
		        }

		        string szInfo = string.Empty;
		        CRect rect = new CRect();
		        rect.SetRect( point.x - 2, point.y - 15, point.x + 17, point.y);
		        szInfo = m_nWinCount[i].ToString();
		        DCBuffer.DrawText(szInfo, rect , CDC.DT_SINGLELINE|CDC.DT_END_ELLIPSIS|CDC.DT_VCENTER|CDC.DT_CENTER );

		        point.x += 21;
		        point.y = 284;
	        }

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

        public void SetWinCount( int[] nWinCount )
        {
	        //memcpy(m_nWinCount, nWinCount, sizeof(m_nWinCount));
            Array.Copy(nWinCount, m_nWinCount, nWinCount.Length);

	        if ( IsWindowVisible())
	        {
		        Invalidate(false);
	        }
        }
        void OnBnClickedButtonTjClose()
        {
	        this.Visible = false;
        }

        void OnBnClickedButtonTjClose2()
        {
	        this.Visible = false;
        }

    }
}
