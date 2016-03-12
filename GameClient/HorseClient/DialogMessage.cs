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
    public partial class DialogMessage : CDialog
    {
	    //变量
	    CFont				m_InfoFont;			//消息字体
	    string				m_szMessage = string.Empty;	//消息
	    CPngImage			m_ImageBackdrop;	//背景
	    CSkinButton			m_btDetermine = new CSkinButton();		//确定
	    CSkinButton			m_btClosee = new CSkinButton();			//关闭

        public DialogMessage()
        {
            InitializeComponent();

            m_InfoFont.CreateFont(12,0,0,0,400,0,0,0,134,3,2,CFont.ANTIALIASED_QUALITY,2,TEXT("宋体"));
	        //ZeroMemory(m_szMessage,sizeof(m_szMessage));

            InitDialog();
        }

        const int IDOK            =    1;
        const int IDC_BUTTON_CLOSE           =     1094;

        public override void DoDataExchange()
        {
            Control pDX = this;

	        DDX_Control(pDX, IDOK, m_btDetermine);
	        DDX_Control(pDX, IDC_BUTTON_CLOSE, m_btClosee);        	
        }


        public override void BEGIN_MESSAGE_MAP()
        {
            ON_BN_CLICKED(IDC_BUTTON_CLOSE, OnBnClickedButtonClose);
        }       


        // CDialogMessage 消息处理程序

        public override bool OnInitDialog()
        {
	        Control hInstance = this;
            //m_ImageBackdrop.LoadFromResource( hInstance,Properties.Resources.BACK_MESSAGE );
	        SetWindowPos(null, 0, 0,m_ImageBackdrop.GetWidth(), m_ImageBackdrop.GetHeight(), SWP_NOACTIVATE|SWP_NOZORDER|SWP_NOCOPYBITS|SWP_NOMOVE);

            //m_btDetermine.SetButtonImage( Properties.Resources.BT_MESSAGE,  hInstance , false , false);
            //m_btClosee.SetButtonImage( Properties.Resources.BT_CLOSE,  hInstance , false , false);
	        
            CRect rcButton = new CRect();
	        m_btDetermine.GetWindowRect(ref rcButton);
	        m_btDetermine.MoveWindow( m_ImageBackdrop.GetWidth()/2 - rcButton.Width()/2 , m_ImageBackdrop.GetHeight() - 50, rcButton.Width(), rcButton.Height());
	        m_btClosee.MoveWindow( m_ImageBackdrop.GetWidth() - 26, 1, 25,25);
	        return true; 
        }

        //BOOL OnEraseBkgnd(CDC* pDC)
        //{
        //    return TRUE;
        //}
        private void DialogMessage_Paint(object sender, PaintEventArgs e)
        {
	        CDC dc = new CDC();
            dc.SetGraphics(e.Graphics); // device context for painting

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

	        string szInfo = string.Empty;
            DCBuffer.DrawText(m_szMessage, rcClient, CDC.DT_SINGLELINE | CDC.DT_END_ELLIPSIS | CDC.DT_VCENTER | CDC.DT_CENTER);

	        //绘画界面
            dc.BitBlt(0, 0, rcClient.Width(), rcClient.Height(), ImageBuffer, 0, 0, 0);

	        //清理资源
            //DCBuffer.SetTextColor(oldColor);
            //DCBuffer.SetTextAlign(nTextAlign);
            //DCBuffer.SelectObject(oldBitmap);
            //DCBuffer.SelectObject(oldFont);
            //DCBuffer.DeleteDC();
            //ImageBuffer.DeleteObject();
        	
        }

        //设置消息
        public void SetMessage( string lpszString )
        {
	        m_szMessage = lpszString;
        }

        void OnBnClickedButtonClose()
        {
	        this.DialogResult = DialogResult.OK;
        }

    }
}
