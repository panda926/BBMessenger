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

namespace DzCardClient
{
    public partial class ScoreView : Form
    {
        const int TIMES = 5;							//时间参数

	    //变量定义
	    int							m_lGameScore;						//游戏得分
	    int							m_bTimes;							//倒计时间
	    bool						m_bStart;							//开始标志

	    //资源变量
	    Bitmap					    m_ImageBack;						//背景图案


        public ScoreView()
        {
            InitializeComponent();

            //设置数据
            m_lGameScore = 0;
            m_bStart = false;
            m_bTimes = TIMES;

            //加载资源
            m_ImageBack = Properties.Resources.GAME_WIN;


            //设置数据
            m_lGameScore = 0;

            //设置界面
            //SetClassLong(m_hWnd,GCL_HBRBACKGROUND,NULL);

            //移动窗口
            //CImageHandle ImageHandle(&m_ImageBack);
            this.ClientSize = new Size(m_ImageBack.Width / 3, m_ImageBack.Height);
            //SetWindowPos(NULL,0,0,m_ImageBack.GetWidth()/3,m_ImageBack.GetHeight(),SWP_NOMOVE|SWP_NOZORDER);
        }

        private void ScoreView_Load(object sender, EventArgs e)
        {
        }

        private void ScoreView_Paint(object sender, PaintEventArgs e)
        {
	        if(this.Visible !=true)
                return;

	        //CPaintDC dc(this); 
	        //SetupRegion(&dc,m_ImageBack,RGB(255,0,255));

	        //设置 DC
	        //dc.SetBkMode(TRANSPARENT);
	        //dc.SetTextColor(RGB(250,250,250));
	        //dc.SelectObject(CSkinResourceManager::GetDefaultFont());

            Graphics g = e.Graphics;
            Brush brush = new SolidBrush( Color.FromArgb( 250, 250, 250 ));

            Font viewFont = new Font("Arial", 12);

	        //绘画背景
	        //CImageHandle ImageHandle(&m_ImageBack);
	        if (m_lGameScore > 0)
	        {
	        //	m_ImageBack.BitBlt(dc.GetSafeHdc(),0,0,m_ImageBack.GetWidth()/3,m_ImageBack.GetHeight(),0,0,SRCCOPY);
		        GameGraphics.DrawAlphaImage( g, m_ImageBack, 0,0,m_ImageBack.Width/3,m_ImageBack.Height,0,0,Color.FromArgb(255,0,255));
	        }
	        else if(m_lGameScore == 0L)
	        {
		        GameGraphics.DrawAlphaImage( g, m_ImageBack, 0,0,m_ImageBack.Width/3,m_ImageBack.Height,m_ImageBack.Width/3,0,Color.FromArgb(255,0,255));
	        }
	        else
	        {
		        GameGraphics.DrawAlphaImage( g, m_ImageBack, 0,0,m_ImageBack.Width/3,m_ImageBack.Height,2*m_ImageBack.Width/3,0,Color.FromArgb(255,0,255));
	        }

	        //显示分数
	        string szBuffer = string.Empty;
	        Rectangle rcScore = new Rectangle(60,60,65,19);

	        //用户金币
	        szBuffer = string.Format( "{0}",m_lGameScore);
            GameGraphics.DrawString(g, szBuffer, viewFont, brush, rcScore);

	        //dc.SetTextColor(RGB(0,0,0));

	        //显示秒数
	        string szBuffe = string.Empty;
	        int ileft=52;
	        int itop=8;
	        Rectangle rcScor = new Rectangle(ileft,itop,ileft+15,itop+15);

	        //创建资源
	        Font InfoFont;
	        //InfoFont.CreateFont(-14,0,0,0,10,0,0,0,4,0,2,1,2,TEXT("宋体"));
            InfoFont = new Font("宋体", 14);

	        //设置 DC
	        //dc.SetTextAlign(TA_CENTER|TA_TOP);
	        //CFont * pOldFont=dc.SelectObject(&InfoFont);

	        //绘画信息
	        szBuffe = string.Format( "{0}", m_bTimes);
	        //dc.Ellipse(&rcScor);
	        brush = new SolidBrush( Color.FromArgb(255,255,255));
	        g.DrawString( szBuffe, InfoFont, brush, ileft+132,itop-2);

	        //清理资源
	        //dc.SelectObject(pOldFont);
	        //InfoFont.DeleteObject();

	        return;
        }

        private void ScoreView_MouseDown(object sender, MouseEventArgs e)
        {

        }

        //重置金币
        void ResetScore()
        {
	        //设置数据
	        m_lGameScore =0;

	        //绘画视图
	        Invalidate();
        	
	        return;
        }


        //设置税收
        void SetGameTax(int lGameTax)
        {	
	        return;
        }

        //设置金币
        public void SetGameScore(int wChairID,  int lScore)
        {
	        //设置变量
	        if (wChairID<DzCardDefine.GAME_PLAYER)
	        {
		        m_lGameScore=lScore;
		        Invalidate();
	        }

	        return;
        }

        //开始时间
        public void SetStartTimes(bool bStart)
        {
	        //设置变量
	        //ASSERT(bStart);
	        m_bStart = bStart;

	        if(!bStart)
	        {
		        m_bTimes = TIMES;
                timer1.Enabled = false;
		        //KillTimer(DLG_IDI_CLOSE);
	        }

	        return;
        }

        //显示时间
        public void SetShowTimes()
        {
            timer1.Interval = 1000;
            timer1.Enabled = true;
	        //SetTimer(DLG_IDI_CLOSE,1000,NULL);

	        return;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //关闭倒计时
            //if (DLG_IDI_CLOSE == nIDEvent)
            //{
                m_bTimes--;
                if (m_bTimes <= 0)
                {
                    m_bTimes = TIMES;
                    timer1.Enabled = false;;
                    this.Visible = false;

                    if (m_bStart)
                    {
                        m_bStart = false;

                        if( Parent != null && Parent is GameView )
                            ((GameView)Parent).NotifyMessage( NotifyMessageKind.IDM_START_TIMES, 0, 0);
                    }
                }
                else
                {
                    Invalidate();
                    //UpdateWindow();
                }
            //}

            return;
            //CDialog::OnTimer(nIDEvent);
        
        }
    }
}
