using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GameControls;

namespace DzCardClient
{
    public partial class GoldControl : UserControl
    {
        //宏定义
        const int CELL_WIDTH =							23;							//单元宽度
        const int LESS_WIDTH =							197;							//最小宽度
        const int CONTROL_HEIGHT =						75;							//控件高度
        const int LEFT_WIDTH =							65;							//左边宽度
        const int RIGHT_WIDTH =							65;							//右边宽度	
        const int IMAGE_HEIGHT =						39;							//图片高度
        const int IMAGE_WIDTH =                         23;							//图片宽度

        //按钮标识
        const int IDC_MAXSCORE	=					101;							//最大下注
        const int IDC_MINSCORE	=					102;							//最小下注
        const int IDC_OK =								103;							//确定按纽
        const int IDC_CANCEL = 104;							//取消按纽	

	    //变量定义
	    int								m_lMaxGold;						//最大金币
	    int								m_lMinGold;						//最小金币
	    int[]							m_lGoldCell = new int[7];					//单元金币
	    Point							m_BasicPoint = new Point();					//基础位置

	    //位置变量
	    int								m_nWidth;						//控件宽度
	    int								m_nCellCount;					//单元数目

	    //按纽控件
	    PictureButton					m_btMaxScore= new PictureButton();					//最大按钮
	    PictureButton					m_btMinScore= new PictureButton();					//最小按钮
	    PictureButton					m_btOK= new PictureButton();							//确定按纽
	    PictureButton					m_btCancel= new PictureButton();						//取消按纽


	    //资源变量
	    Bitmap							m_ImageLeft;					//背景资源
	    Bitmap							m_ImageMid;						//背景资源
	    Bitmap							m_ImageRight;					//背景资源
	    Bitmap							m_ImageNumber;					//数字资源

        public GoldControl()
        {
            InitializeComponent();

	        //界面变量
	        m_nWidth=0;
	        m_nCellCount=0;

	        //设置变量
	        m_lMaxGold = 0;
	        m_lMinGold = 0;
	        //ZeroMemory(m_lGoldCell,sizeof(m_lGoldCell));

	        //加载资源
	        //HINSTANCE hInstance=AfxGetInstanceHandle();
	        m_ImageLeft = Properties.Resources.LEFT;
	        m_ImageMid = Properties.Resources.MID;
	        m_ImageRight = Properties.Resources.RIGHT;
	        m_ImageNumber = Properties.Resources.NUMBER;

	        //创建控件
	        Rectangle rcCreate = new Rectangle();
	        m_btMaxScore.Create(true, true, rcCreate,this,IDC_MAXSCORE);
	        m_btMinScore.Create(true, true, rcCreate,this,IDC_MINSCORE);
	        m_btOK.Create(true, true, rcCreate,this,IDC_OK);
	        m_btCancel.Create(true, true, rcCreate,this,IDC_CANCEL);

	        //加载位图
	        m_btMaxScore.SetButtonImage(Properties.Resources.BT_MAX);
            m_btMinScore.SetButtonImage(Properties.Resources.BT_MIN);
            m_btOK.SetButtonImage(Properties.Resources.BT_OK);
            m_btCancel.SetButtonImage(Properties.Resources.BT_CANCEL);

            m_btMaxScore.Click += OnMaxScore;
            m_btMinScore.Click += OnMinScore;
            m_btOK.Click += OnOKScore;
            m_btCancel.Click += OnCancelScore;
        }

        //获取金币
        public int GetGold()
        {
	        int lGold=0;

	        for (int i=0;i<m_lGoldCell.Length;i++) 
		        lGold+=m_lGoldCell[i]*(int)Math.Pow(10L,i);

	        return lGold;
        }

        //设置金币
        public void SetGold(int lGold)
        {
	        //调整参数
	        if (lGold>m_lMaxGold) 
                lGold=m_lMaxGold;

	        Array.Clear(m_lGoldCell,0, m_lGoldCell.Length);

	        //设置变量
	        int nIndex=0;
	        while (lGold>0L)
	        {
		        m_lGoldCell[nIndex++]=lGold%10;
		        lGold/=10;
	        }
	        //调整界面
	        RectifyControl();

            Invalidate();

	        return;
        }

        //设置用户最小下注
        public void SetMinGold(int lMinGold)
        {
	        //校验改变
	        if (lMinGold>m_lMaxGold) 
                return;
	        if((lMinGold == m_lMinGold)||(lMinGold <0L)) 
                return;

	        Array.Clear(m_lGoldCell, 0, m_lGoldCell.Length);

	        //设置变量
	        m_lMinGold = lMinGold;

	        //设置变量
	        int nIndex=0;
	        while (lMinGold>0L)
	        {
		        m_lGoldCell[nIndex++]=lMinGold%10;
		        lMinGold/=10;
	        }

	        //调整界面
	        RectifyControl();

	        return;
        }

        //设置用户最高下注数
        public void SetMaxGold(int lMaxGold)
        {
	        //效验改变
	        if (m_lMaxGold==lMaxGold) return;

	        //设置变量
	        m_lMaxGold=lMaxGold;
	        if (m_lMaxGold>9999999) 
                m_lMaxGold=9999999;

	        Array.Clear(m_lGoldCell, 0, m_lGoldCell.Length);

	        //计算单元
	        m_nCellCount=0;
	        while (lMaxGold>0L)
	        {
		        lMaxGold/=10;
		        m_nCellCount++;
	        }
	        m_nCellCount= Math.Min(m_lGoldCell.Length,Math.Max(m_nCellCount,1));

	        //设置界面
	        m_nWidth=(m_nCellCount+(m_nCellCount-1)/3)*CELL_WIDTH+LEFT_WIDTH+RIGHT_WIDTH;

            this.Left =m_BasicPoint.X-m_nWidth;
            this.Top = m_BasicPoint.Y-CONTROL_HEIGHT;
            this.Width = m_nWidth;
            this.Height = CONTROL_HEIGHT;
	        return;
        }

        //设置位置
        public void SetBasicPoint(int nXPos, int nYPos)
        {
            //设置变量
            m_BasicPoint.X = nXPos;
            m_BasicPoint.Y = nYPos;

            //调整界面
            RectifyControl();

            return;
        }



        //调整控件
        void RectifyControl()
        {
            this.Left = m_BasicPoint.X - m_nWidth;
            this.Top = m_BasicPoint.Y - CONTROL_HEIGHT;
            this.Width = m_nWidth;
            this.Height = CONTROL_HEIGHT;

            Rectangle rcButton = new Rectangle();
            IntPtr hDwp = GameGraphics.BeginDeferWindowPos(32);
            const uint uFlags = GameGraphics.SWP_NOACTIVATE | GameGraphics.SWP_NOZORDER | GameGraphics.SWP_NOCOPYBITS | GameGraphics.SWP_NOSIZE;

            //最大按钮
            //m_btMaxScore.GetWindowRect(&rcButton);
            rcButton.Width = 47;
            rcButton.Height = 28;
            GameGraphics.DeferWindowPos(hDwp, m_btMaxScore.Handle, 0, 6, 7, 0, 0, uFlags);

            //最小按钮
            GameGraphics.DeferWindowPos(hDwp, m_btMinScore.Handle, 0, 6, CONTROL_HEIGHT - rcButton.Height - 9, 0, 0, uFlags);

            //确定按纽
            GameGraphics.DeferWindowPos(hDwp, m_btOK.Handle, 0, m_nWidth - rcButton.Width - 6, 7, 0, 0, uFlags);

            //加注按钮
            GameGraphics.DeferWindowPos(hDwp, m_btCancel.Handle, 0, m_nWidth - rcButton.Width - 6, CONTROL_HEIGHT - rcButton.Height - 9, 0, 0, uFlags);


            //结束移动
            GameGraphics.EndDeferWindowPos(hDwp);
	        return;
        }

        //绘画金币
        void DrawGoldCell(Graphics g, int nXPos, int nYPox, int lGold)
        {
	        //CImageHandle ImageNumberHandle(&m_ImageNumber);
	        if (lGold<10) 
	        {
		        GameGraphics.DrawImage( g, m_ImageNumber, nXPos,nYPox,IMAGE_WIDTH,IMAGE_HEIGHT,lGold*IMAGE_WIDTH,0);
	        }
	        else
	        {
		        GameGraphics.DrawImage( g, m_ImageNumber,nXPos,nYPox,IMAGE_WIDTH,IMAGE_HEIGHT,10*IMAGE_WIDTH,0);
	        }

	        return;
        }

        //重画函数
        private void GoldControl_Paint(object sender, PaintEventArgs e)
        {
	        if(GetGold()>=m_lMinGold)
                m_btOK.Enabled = true;
	        else 
                m_btOK.Enabled = false;

            Graphics g = e.Graphics;

	        //获取位置
	        Rectangle ClientRect = this.ClientRectangle;
	        //GetClientRect(&ClientRect);

	        //建立缓冲图
	        Bitmap BufferBmp = new Bitmap( ClientRect.Width, ClientRect.Height, g);
            Graphics BackFaceDC = Graphics.FromImage(BufferBmp);

	        //加载资源
	        //CImageHandle ImageMidHandle(&m_ImageMid);
	        //CImageHandle ImageLeftHandle(&m_ImageLeft);
	        //CImageHandle ImageRightHandle(&m_ImageRight);

	        //绘画背景
            GameGraphics.DrawImage(BackFaceDC, m_ImageLeft, 0, 0, m_ImageLeft.Width, m_ImageLeft.Height, 0, 0);

            Rectangle dstRect = new Rectangle(m_ImageLeft.Width, 0, ClientRect.Width - m_ImageRight.Width, m_ImageMid.Height);
            Rectangle srcRect = new Rectangle(0, 0, m_ImageMid.Width, m_ImageMid.Height);

            g.DrawImage(m_ImageMid, dstRect, srcRect, GraphicsUnit.Pixel);
            
            //GameGraphics.DrawImage(BackFaceDC, m_ImageMid, m_ImageLeft.Width, 0, , , 0, 0);
            GameGraphics.DrawImage(BackFaceDC, m_ImageRight, ClientRect.Width - m_ImageRight.Width, 0, m_ImageRight.Width, m_ImageRight.Height, 0, 0);

	        //绘画金币
            int nXExcursion = ClientRect.Width - RIGHT_WIDTH;
            for (int i = 0; i < m_nCellCount; i++)
            {
                //绘画逗号
                if ((i != 0) && (i % 3) == 0)
                {
                    nXExcursion -= IMAGE_WIDTH;
                    DrawGoldCell(BackFaceDC, nXExcursion, 12, 10);
                }

                //绘画数字
                nXExcursion -= IMAGE_WIDTH;
                DrawGoldCell(BackFaceDC, nXExcursion, 12, m_lGoldCell[i]);
            }

	        //绘画界面
            GameGraphics.DrawImage( g, BufferBmp, 0,0,ClientRect.Width,ClientRect.Height,0,0);
        }

        private void GoldControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                OnLButtonUp(e.Location);

            if (e.Button == MouseButtons.Right)
                OnRButtonUp(e.Location);
        }

        void OnLButtonUp(Point point)
        {
            //变量定义
            int nViewCellCount = (m_nCellCount - 1) / 3 + m_nCellCount;

            //位置过虑
            int nHeadWidth = LEFT_WIDTH;
            if ((point.Y <= 5) || (point.Y >= 65)) return;
            if ((point.X <= nHeadWidth) || (point.X >= (CELL_WIDTH * nViewCellCount + nHeadWidth))) return;

            //按钮测试
            int iCellPos = (nViewCellCount - (point.X - nHeadWidth) / CELL_WIDTH) - 1;
            if ((iCellPos == 3) || (iCellPos == 7)) return;
            if (iCellPos > 3) iCellPos = iCellPos - (iCellPos - 1) / 3;

            //计算限制
            //ASSERT((iCellPos >= 0) && (iCellPos < CountArray(m_lGoldCell)));
            if (m_lGoldCell[iCellPos] != 9L)
            {
                int lAddGold = (int)Math.Pow(10L, iCellPos);
                if ((GetGold() + lAddGold) > m_lMaxGold) return;
            }
            //if (m_lGoldCell[iCellPos]==9L)
            //{
            //	LONG lAddGold=(LONG)pow(10L,iCellPos)*9;
            //	if((GetGold()-lAddGold <m_lMinGold)) return;
            //}

            //设置变量
            m_lGoldCell[iCellPos] = (m_lGoldCell[iCellPos] + 1) % 10;
            int iCellPosTemp = (nViewCellCount - (point.X - nHeadWidth) / CELL_WIDTH) - 1;

            if (iCellPosTemp > 3)
                Invalidate(new Rectangle(m_nWidth - (iCellPosTemp + 1) * IMAGE_WIDTH - RIGHT_WIDTH, 5, m_nWidth - iCellPosTemp * IMAGE_WIDTH - RIGHT_WIDTH, 12 + IMAGE_HEIGHT));
            else
                Invalidate(new Rectangle(m_nWidth - (iCellPos + 1) * IMAGE_WIDTH - RIGHT_WIDTH, 5, m_nWidth - iCellPos * IMAGE_WIDTH - RIGHT_WIDTH, 12 + IMAGE_HEIGHT));

            return;
        }

        //右键按下消息
        void OnRButtonUp(Point point)
        {
	        //变量定义
	        int nViewCellCount=(m_nCellCount-1)/3+m_nCellCount;

	        //位置过虑
	        int nHeadWidth=LEFT_WIDTH;
	        if ((point.Y<=5)||(point.Y>=65)) 
                return;
	        if ((point.X<=nHeadWidth)||(point.X>=(CELL_WIDTH*nViewCellCount+nHeadWidth))) 
                return;

	        //按钮测试
	        int iCellPos=(nViewCellCount-(point.X-nHeadWidth)/CELL_WIDTH)-1;
	        if ((iCellPos==3)||(iCellPos==7)) 
                return;
	        if (iCellPos>3) iCellPos=iCellPos-(iCellPos-1)/3;

	        //计算限制
	        //ASSERT((iCellPos>=0)&&(iCellPos<CountArray(m_lGoldCell)));
	        if (m_lGoldCell[iCellPos]==0L)
	        {
		        int lAddGold=9*(int)Math.Pow(10,iCellPos);
		        if ((GetGold()+lAddGold)>m_lMaxGold) 
                    return;
	        }
	        //if (m_lGoldCell[iCellPos]!=0L)
	        //{
	        //	LONG lAddGold=(LONG)pow(10L,iCellPos);
	        //	if ((GetGold()-lAddGold)<m_lMinGold) return;

	        //}

	        //设置变量
	        m_lGoldCell[iCellPos]=(m_lGoldCell[iCellPos]+9)%10;

	        int iCellPosTemp =(nViewCellCount-(point.X-nHeadWidth)/CELL_WIDTH)-1;

	        if(iCellPosTemp>3)
		        Invalidate( new Rectangle(m_nWidth-(iCellPosTemp+1)*IMAGE_WIDTH -RIGHT_WIDTH,5,m_nWidth-iCellPosTemp*IMAGE_WIDTH-RIGHT_WIDTH,12+IMAGE_HEIGHT));
	        else
		        Invalidate( new Rectangle(m_nWidth-(iCellPos+1)*IMAGE_WIDTH -RIGHT_WIDTH,5,m_nWidth-iCellPos*IMAGE_WIDTH-RIGHT_WIDTH,12+IMAGE_HEIGHT));

	        return;
        }

        //设置光标
        private void GoldControl_MouseMove(object sender, MouseEventArgs e)
        {
	        //获取鼠标
	        Point point = e.Location;

	        //设置光标
	        int nHeadWidth=LEFT_WIDTH;
	        int nViewCellCount=(m_nCellCount-1)/3+m_nCellCount;
	        if ((point.Y>5)&&(point.Y<65)&&(point.X>nHeadWidth)&&(point.X<(CELL_WIDTH*nViewCellCount+nHeadWidth)))
	        {
		        int iCellPos=(m_nCellCount-(point.X-nHeadWidth)/CELL_WIDTH);
		        if ((iCellPos!=3)&&(iCellPos!=7))
		        {
			        this.Cursor = GameGraphics.LoadCursorFromResource(Properties.Resources.CARD_CUR);
		        }
	        }
        }

        //最小下注
        void OnMinScore(object sender, EventArgs e)
        {
	        SetGold(m_lMinGold);
	        Invalidate(false);

            if( this.Parent != null && this.Parent is GameView )
	            ((GameView)this.Parent).NotifyMessage(NotifyMessageKind.IDM_MIN_SCORE,m_lMinGold,m_lMinGold);
	        return ;
        }

        //最大下注
        void OnMaxScore(object sender, EventArgs e)
        {
	        SetGold(m_lMaxGold);
	        Invalidate(false);

            if( this.Parent != null && this.Parent is GameView )
	            ((GameView)this.Parent).NotifyMessage(NotifyMessageKind.IDM_MAX_SCORE,m_lMaxGold,m_lMaxGold);
	        return ;
        }

        //确定消息
        void OnOKScore(object sender, EventArgs e)
        {
	        int lGold = GetGold();

            if( this.Parent != null && this.Parent is GameView )
	            ((GameView)this.Parent).NotifyMessage(NotifyMessageKind.IDM_OK_SCORE,lGold,lGold);

	        return;
        }

        //取消消息
        void OnCancelScore(object sender, EventArgs e)
        {
            if( this.Parent != null && this.Parent is GameView )
	            ((GameView)this.Parent).NotifyMessage(NotifyMessageKind.IDM_CANCEL_SCORE,0,0);

	        return;
        }


    }
}
