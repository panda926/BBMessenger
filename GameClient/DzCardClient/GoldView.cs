using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GameControls;

namespace DzCardClient
{
    public class GoldView
    {
        const int GOLD_IMAGE_WIDTH =		20;										//筹码宽
        const int GOLD_IMAGE_HEIGHT = 19;										//筹码高

	    //变量定义
	    int								m_lGold;						//筹码数目
	    int								m_lMaxLayer;					//最大层数
	    int[]							m_lGoldCount = new int[7];	 			//筹码数目

	    //辅助变量
	    static bool						m_bInit;						//初始标志
	    static Bitmap					m_ImageGold;					//筹码图片
	    Bitmap						    m_ImageNumber;					//数字图片

        //构造函数
        public GoldView()
        {
	        m_lGold=0;
	        m_lMaxLayer=6;
	        Array.Clear(m_lGoldCount,0,m_lGoldCount.Length);
	        
            if (m_bInit==false)
	        {
		        m_bInit=true;
		        m_ImageGold = Properties.Resources.SCORE;
	        }

	        m_ImageNumber = Properties.Resources.NUMBER;

	        return;
        }

        //设置筹码
        public void SetGold(int lGold)
        {
	        if (m_lGold!=lGold)
	        {
		        m_lGold=lGold;
		        RectifyGoldLayer();
	        }
	        return;
        }

	    //获取筹码
	    public int GetGold() 
        { 
            return m_lGold; 
        }


        //设置层数
        void SetMaxGoldLayer(int lMaxLayer)
        {
	        if (m_lMaxLayer!=lMaxLayer)
	        {
		        m_lMaxLayer=lMaxLayer; 
		        RectifyGoldLayer();
	        }
	        return;
        }


        //绘画筹码
        public void DrawGoldView(Graphics g, int nXPos, int nYPos, bool bCount,bool bCenter,byte bDUser)
        {
	        //加载位图
	        //CImageHandle ImageHandle(&m_ImageGold);

	        if(!bCenter)
	        {
		        //绘画筹码
		        int nYPosDraw=nYPos-GOLD_IMAGE_HEIGHT/2;
		        int iDrawCount =0;
		        for (int i=0;i<m_lGoldCount.Length;i++)
		        {
			        for (int j=0;j<m_lGoldCount[i];j++)
			        {
				        iDrawCount++;
				        GameGraphics.DrawAlphaImage( g, m_ImageGold, nXPos-GOLD_IMAGE_WIDTH/2,nYPosDraw,GOLD_IMAGE_WIDTH,GOLD_IMAGE_HEIGHT,
					        (m_lGoldCount.Length-i-1)*GOLD_IMAGE_WIDTH,0,Color.FromArgb(255,0,255));
				        nYPosDraw-=3;
				        if(iDrawCount>=m_lMaxLayer)break;
			        }
			        if(iDrawCount>=m_lMaxLayer)break;
		        }
	        }
	        else //中心筹码
	        {
		        //绘画筹码
		        int nYPosDraw=nYPos-GOLD_IMAGE_HEIGHT/2;
		        int nXPosDraw=nXPos-GOLD_IMAGE_HEIGHT/2;
		        int iCount = 0,iDrawCount =0;
		        int xTemp=0,yTemp=0;
		        for (int i=0;i<m_lGoldCount.Length;i++)
		        {
			        for (int j=0;j<m_lGoldCount[i];j++)
			        {
				        iDrawCount++;
				        GameGraphics.DrawAlphaImage( g, m_ImageGold, nXPosDraw+xTemp,nYPosDraw+GOLD_IMAGE_HEIGHT/2+2,GOLD_IMAGE_WIDTH,
					        GOLD_IMAGE_HEIGHT,(m_lGoldCount.Length-i-1)*GOLD_IMAGE_WIDTH,0,Color.FromArgb(255,0,255));
				        nYPosDraw-=3;
				        if(iDrawCount>=m_lMaxLayer)
				        {
					        iDrawCount=0;
					        nYPosDraw=nYPos-GOLD_IMAGE_HEIGHT/2;
					        iCount++;
					        if(iCount==1)
					        {
						        xTemp=GOLD_IMAGE_WIDTH;
					        }
					        else if(iCount==2)
					        {
						        xTemp=-GOLD_IMAGE_WIDTH;
					        }
					        else if(iCount==3)
					        {
						        xTemp=GOLD_IMAGE_WIDTH*2;
					        }
					        else if(iCount==4)
					        {
						        xTemp=-GOLD_IMAGE_WIDTH*2;
					        }
				        }	
			        }
		        }
	        }

	        //绘画数字
	        if((m_lGold>=1L)&&(bCount==true))
	        {
		        Font ViewFont;
			    Brush brush;
		        if(bCenter)
		        {
			        //ViewFont = new Font((-17,0,0,0,400,0,0,0,134,3,2,1,1,TEXT("Arial"));
                    ViewFont = new Font("Arial", 17 );
			        brush = new SolidBrush(Color.FromArgb(255,255,4));
		        }
		        else 
		        {
			        //ViewFont = new Font(-15,0,0,0,400,0,0,0,134,3,2,1,1,TEXT("Arial"));
                    ViewFont = new Font( "Arial", 15);
			        brush = new SolidBrush( Color.FromArgb(255,255,4));
		        }
		        //CFont *pOldFont=pDC->SelectObject(&ViewFont);

		        //int iBkMode = pDC->SetBkMode( TRANSPARENT );

		        string szBuffer = GameGraphics.GetGoldString(m_lGold);
		        int iY = nYPos+GOLD_IMAGE_HEIGHT/((bCenter)?1:2)+1;
		        int iX = (bDUser!=4)?0:62;
		        Rectangle DrawRect = new Rectangle(nXPos-50+iX,iY,96,20);


                if (bDUser != 4)
                {
                    GameGraphics.DrawString(g, szBuffer, ViewFont, brush, DrawRect);
                }
                else
                {
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    //stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.FormatFlags = StringFormatFlags.NoWrap;

                    g.DrawString(szBuffer, ViewFont, brush, DrawRect, stringFormat);
                }
		        //pDC->SetBkMode(iBkMode);

		        //pDC->SelectObject(pOldFont);
		        //ViewFont.DeleteObject();	
	        }

	        return;
        }


        //调整筹码层
        void RectifyGoldLayer()
        {
	        //变量定义
	        int[] lBasicGold= new int[]{1000,500,100,50,10,5,1};
            int lGold=(int)m_lGold;

	        Array.Clear(m_lGoldCount,0,m_lGoldCount.Length);

	        //调整筹码层
	        for (int i=0;i<lBasicGold.Length;i++)
	        {
		        if (lGold>=lBasicGold[i])
		        {
			        m_lGoldCount[i]=lGold/lBasicGold[i];
			        lGold-=m_lGoldCount[i]*lBasicGold[i];
			        if (m_lGoldCount[i]>m_lMaxLayer)
			        {
				        m_lGoldCount[i]=m_lMaxLayer;
			        }
		        }
	        }

	        return;
        }

    }
}
