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
    public partial class DialogBetRecord : CDialog
    {
        //列表
        CListCtrl m_listBetRecord = new CListCtrl();

        CFont m_InfoFont;
        CPngImage m_ImageBackdrop;		//背景
        CSkinButton m_btClosee = new CSkinButton();				//关闭

        ////标题栏重绘
        //void CColorHeaderCtrl::OnPaint()
        //{
        //    CPaintDC dc(this);

        //    CRect rect;
        //    GetClientRect(rect);
        //    dc.FillSolidRect(rect,RGB(70,76,79));		//重绘标题栏颜色

        //    int nItems = GetItemCount();
        //    CRect rectItem;

        //    CPen m_pen(PS_SOLID,1,RGB(200,200,200));		//分隔线颜色
        //    CFont m_font;
        //    m_font.CreatePointFont(90,TEXT("宋体"));				//字体
        //    CPen * pOldPen = dc.SelectObject(&m_pen);
        //    CFont * pOldFont=dc.SelectObject(&m_font);

        //    dc.SetTextColor(RGB(200, 243, 39));				//字体颜色

        //    for(int i = 0; i <nItems; i++)					//对标题的每个列进行重绘
        //    {  
        //        GetItemRect(i, &rectItem);
        //        rectItem.top+=2;
        //        rectItem.bottom+=2; 
        //        dc.MoveTo(rectItem.right,rect.top);         //重绘分隔栏
        //        dc.LineTo(rectItem.right,rectItem.bottom);

        //        TCHAR buf[256];
        //        HD_ITEM hditem;

        //        hditem.mask = HDI_TEXT | HDI_FORMAT | HDI_ORDER;
        //        hditem.pszText = buf;
        //        hditem.cchTextMax = 255;
        //        GetItem( i, &hditem );                      //获取当然列的文字

        //        UINT uFormat = DT_SINGLELINE | DT_NOPREFIX | DT_TOP |DT_CENTER | DT_END_ELLIPSIS ;
        //        dc.DrawText(buf, &rectItem, uFormat);       //重绘标题栏的文字
        //    }

        //    dc.SelectObject(pOldPen);
        //    dc.SelectObject(pOldFont);
        //    m_pen.DeleteObject();
        //    m_font.DeleteObject();
        //}

        //BOOL CColorHeaderCtrl::OnEraseBkgnd(CDC* pDC)
        //{
        //    return true;
        //}

        //列表
        //BEGIN_MESSAGE_MAP(CMyListCtrl, CListCtrl)
        //    ON_WM_LBUTTONDOWN()
        //END_MESSAGE_MAP()

        //void CMyListCtrl::OnLButtonDown(UINT nFlags, CPoint point)
        //{
        //    //屏蔽鼠标操作
        //    return;
        //}


        // CDialogBetRecord 对话框
        public DialogBetRecord()
        {
            InitializeComponent();

	        m_InfoFont.CreateFont(12,0,0,0,400,0,0,0,134,3,2,CFont.ANTIALIASED_QUALITY,2,"宋体");

            InitDialog();
        }

        private void DialogBetRecord_Load(object sender, EventArgs e)
        {

        }

        const int IDC_LIST_BET_LOG           =     1109;
        const int IDC_BUTTON_BR_OK           =     1103;

        public override void DoDataExchange()
        {
            Control pDX = this;
	        DDX_Control(pDX, IDC_LIST_BET_LOG, m_listBetRecord);
	        DDX_Control(pDX, IDC_BUTTON_BR_OK, m_btClosee);
        }


        public override void BEGIN_MESSAGE_MAP()
        {
            ON_BN_CLICKED(IDC_BUTTON_BR_OK, OnBnClickedButtonBrOk);
        }


        // CDialogBetRecord 消息处理程序


        public override bool OnInitDialog()
        {
	        Control hInstance = this;
            //m_ImageBackdrop.LoadImage( Properties.Resources.BET_RECORD);
            //m_btClosee.SetButtonImage( Properties.Resources.BT_CLOSE_BL,  hInstance , false , false);
        	
	        SetWindowPos( null, 0, 0, m_ImageBackdrop.GetWidth(), m_ImageBackdrop.GetHeight(), SWP_NOACTIVATE|SWP_NOZORDER|SWP_NOCOPYBITS|SWP_NOMOVE);

	        m_listBetRecord.MoveWindow(4, 40, m_ImageBackdrop.GetWidth()-8, m_ImageBackdrop.GetHeight() - 44);
	        m_btClosee.MoveWindow(m_ImageBackdrop.GetWidth() - 24, 2, 22, 22);

	        //m_ColorHeader.SubclassWindow(m_listBetRecord.GetHeaderCtrl()->GetSafeHwnd());

	        m_listBetRecord.SetBkColor(Color.FromArgb(40,46,59));
	        m_listBetRecord.SetTextBkColor(Color.FromArgb(40,46,59));
	        m_listBetRecord.SetTextColor(Color.FromArgb(255,255,255));
	        m_listBetRecord.InsertColumn(0,"场次组合",CListCtrl.LVCFMT_CENTER, 60);
            m_listBetRecord.InsertColumn(1, "下注", CListCtrl.LVCFMT_CENTER, 40);
            m_listBetRecord.InsertColumn(2, "得分", CListCtrl.LVCFMT_CENTER, 40);
            m_listBetRecord.InsertColumn(3, "时间", CListCtrl.LVCFMT_CENTER, m_ImageBackdrop.GetWidth() - 8 - 140); 

	        return true;  
        }

        //BOOL OnEraseBkgnd(CDC* pDC)
        //{
        //    return true;
        //}

        private void DialogBetRecord_Paint(object sender, PaintEventArgs e)
        {
            CDC dc = new CDC();
            dc.SetGraphics(e.Graphics); 

	        //获取位置
	        CRect rcClient = new CRect();
	        GetClientRect(ref rcClient);

	        //创建缓冲
	        CDC DCBuffer = new CDC();
	        CBitmap ImageBuffer = new CBitmap();
	        ImageBuffer.CreateCompatibleBitmap(dc,rcClient.Width(),rcClient.Height());
            DCBuffer.CreateCompatibleDC(ImageBuffer);

	        //设置 DC
	        //DCBuffer.SetBkMode(TRANSPARENT);
	        //DCBuffer.SelectObject(&ImageBuffer);
	        DCBuffer.SelectObject(m_InfoFont);
	        DCBuffer.SetTextAlign(CDC.TA_TOP|CDC.TA_LEFT);
	        DCBuffer.SetTextColor(Color.FromArgb(250,250,0));

	        m_ImageBackdrop.BitBlt(DCBuffer.GetSafeHdc(), 0, 0);

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

        //区域辨认
        string IdentifyAreas( int cbArea )
        {
	        string str = string.Empty;

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

        //添加信息
        public void AddInfo( BetRecordInfo pInfo )
        {
        	
	        string szInfo = TEXT("");
	        szInfo = string.Format( "{0}：{1}", pInfo.nStreak, IdentifyAreas(pInfo.nRanking));
	        m_listBetRecord.InsertItem(m_listBetRecord.GetItemCount(),szInfo);

	        szInfo = pInfo.lBet.ToString();
	        m_listBetRecord.SetItemText(m_listBetRecord.GetItemCount()-1, 1, szInfo);

	        szInfo = pInfo.lWin.ToString();
	        m_listBetRecord.SetItemText(m_listBetRecord.GetItemCount()-1, 2, szInfo);

	        szInfo = string.Format( "{0}:{1}", pInfo.nHours, pInfo.nMinutes);
	        m_listBetRecord.SetItemText(m_listBetRecord.GetItemCount()-1, 3, szInfo);

	        if(m_listBetRecord.GetSafeHwnd() && IsWindowVisible())
	        {
		        m_listBetRecord.Invalidate();
	        }
        }


        void OnBnClickedButtonBrOk()
        {
	        this.Visible = false;
        }
    }

    //信息结构
    public class BetRecordInfo
    {
        public int nStreak;			//场次
        public int nRanking;			//排名
        public int lBet;				//下注
        public int lWin;				//输赢
        public int nHours;				//小时
        public int nMinutes;			//分钟
    };

}
