using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GameControls;

namespace DzCardClient
{
   

    class CardControl
    {

        const int MAX_CARD_COUNT = 5;									//扑克数目
        const int DEF_X_DISTANCE = 17;									//默认间距
        const int DEF_Y_DISTANCE = 17;  								//默认间距
        const int DEF_SHOOT_DISTANCE = 20;								//默认间距


        //状态变量
        bool m_bHorizontal; //显示方向
        bool m_bPositively; //响应标志
        bool m_bDisplayItem; //显示标志

        const int INVALID_ITEM = 0xffff;

        const int SMALL_CARD_WIDTH = 25;
        const int SMALL_CARD_HEIGHT = 33;

        //属性定义
        const int SPACE_CARD_DATA = 255;
        //数值掩码
        const int 	CARD_MASK_COLOR = 0xF0;									//花色掩码
        const int CARD_MASK_VALUE = 0x0F;	

        //扑克数据
        int m_cbCardCount; //扑克数目
        tagCardItem[] m_CardItemArray = new tagCardItem[MAX_CARD_COUNT]; //扑克数据

        //间隔变量
        int m_nXDistance; //横向间隔
        int m_nYDistance; //竖向间隔
        int m_nShootDistance; //弹起高度

        //位置变量
        Point m_BenchmarkPos = new Point(); //基准位置
        enXCollocateMode m_XCollocateMode = new enXCollocateMode(); //显示模式
        enYCollocateMode m_YCollocateMode = new enYCollocateMode(); //显示模式

        Size m_CardSize = new Size(); //扑克大小
        static Bitmap m_ImageCard; //图片资源
        static Bitmap m_ImageCardMask; //图片资源
        static Bitmap m_ImageWin; //图片资源


        public CardControl()
        {
            //状态变量
	        m_bHorizontal=true;
	        m_bPositively=false;
	        m_bDisplayItem=false;

	        //扑克数据
	        m_cbCardCount=0;

            for( int i = 0; i < m_CardItemArray.Length; i++ )
                m_CardItemArray[i] = new tagCardItem();

	        //间隔变量
	        m_nXDistance=DEF_X_DISTANCE;
	        m_nYDistance=DEF_Y_DISTANCE;
	        m_nShootDistance=DEF_SHOOT_DISTANCE;

	        //位置变量
	        m_YCollocateMode=enYCollocateMode.enYTop;
	        m_XCollocateMode=enXCollocateMode.enXLeft;
	        m_BenchmarkPos = new Point(0,0);

	        //加载资源
            m_ImageCard = Properties.Resources.CARD;
            m_ImageCardMask = Properties.Resources.CARD_MASK;
            m_ImageWin = Properties.Resources.MAX;

	        //获取大小
            m_CardSize = new Size(m_ImageCard.Width/13, m_ImageCard.Height/5);

	        return;
        }
    
        //设置扑克
        public bool SetCardData(int cbCardCount)
        {
            //效验参数
            //ASSERT(cbCardCount<=CountArray(m_CardItemArray));
            if (cbCardCount>m_CardItemArray.Length) return false;

            //设置变量
            m_cbCardCount=cbCardCount;
            //ZeroMemory(m_CardItemArray,sizeof(m_CardItemArray));
            for(int i=0;i<m_CardItemArray.Length;i++)
            {
                m_CardItemArray = new tagCardItem[i];
            }

            return true;
        }

        //设置扑克
        public bool SetCardData(int[] cbCardData, int cbCardCount)
        {
            //效验参数
            //ASSERT(cbCardCount<=CountArray(m_CardItemArray));
            if (cbCardCount>m_CardItemArray.Length) return false;

            //设置变量
            m_cbCardCount=cbCardCount;

            //设置扑克
            for (int i=0;i<cbCardCount;i++)
            {
	            m_CardItemArray[i].bShoot=false;
	            m_CardItemArray[i].bEffect = false;
	            m_CardItemArray[i].bMy = false;
	            m_CardItemArray[i].cbCardData=cbCardData[i];
            }

            return true;
        }

        //设置扑克
        bool SetShootCard(byte[] cbCardData, byte cbCardCount)
        {
            //变量定义
            bool bChangeStatus=false;

            //收起扑克
            for (int i=0;i<m_cbCardCount;i++) 
            {
	            if (m_CardItemArray[i].bShoot==true)
	            {
		            bChangeStatus=true;
		            m_CardItemArray[i].bShoot=false;
	            }
            }

            //弹起扑克
            for (int i=0;i<cbCardCount;i++)
            {
	            for (int j=0;j<m_cbCardCount;j++)
	            {
		            if ((m_CardItemArray[j].bShoot==false)&&(m_CardItemArray[j].cbCardData==cbCardData[i])) 
		            {
			            bChangeStatus=true;
			            m_CardItemArray[j].bShoot=true;
			            break;
		            }
	            }
            }

            return bChangeStatus;
        }

        //设置特效
        public bool SetCardEffect(int[] cbCardData, int cbCardCount)
        {
            //变量定义
            byte bTempCount=0;

            //弹起扑克
            for (int i=0;i<cbCardCount;i++)
            {
	            for (int j=0;j<m_cbCardCount;j++)
	            {
		            if ((m_CardItemArray[j].bEffect==false)&&(m_CardItemArray[j].cbCardData==cbCardData[i])) 
		            {
			            bTempCount++;
			            m_CardItemArray[j].bEffect=true;
			            break;
		            }
	            }
            }

            return (bTempCount==cbCardCount);
        }

        //设置标志
        public bool SetMyCard(int[] cbCardData, int cbCardCount)
        {
            //变量定义
            byte bTempCount=0;

            //标志扑克
            for (int i=0;i<cbCardCount;i++)
            {
	            for (int j=0;j<m_cbCardCount;j++)
	            {
		            if ((m_CardItemArray[j].bMy==false)&&(m_CardItemArray[j].cbCardData==cbCardData[i])) 
		            {
			            bTempCount++;
			            m_CardItemArray[j].bMy=true;
			            break;
		            }
	            }
            }

            return (bTempCount==cbCardCount);
        }

        //设置扑克
        bool SetCardItem(tagCardItem[] CardItemArray, byte cbCardCount)
        {
            //效验参数
            //ASSERT(cbCardCount<=CountArray(m_CardItemArray));
            if (cbCardCount>m_CardItemArray.Length) return false;

            //设置扑克
            m_cbCardCount=cbCardCount;
            //CopyMemory(m_CardItemArray,CardItemArray,cbCardCount*sizeof(tagCardItem));
            for(int i=0;i<cbCardCount;i++)
            {
                m_CardItemArray = CardItemArray;
            }

            return true;
        }

        //获取扑克
        tagCardItem GetCardFromIndex(int cbIndex)
        {
            if(cbIndex<m_cbCardCount)
                return m_CardItemArray[cbIndex];
            
            return null;
        }

        //获取扑克
        tagCardItem GetCardFromPoint(Point MousePoint)
        {
            int wIndex=SwitchCardPoint(MousePoint);
            
            if(wIndex!=INVALID_ITEM)
                return m_CardItemArray[wIndex];
            
            return null;
        }

        //获取扑克
        public int GetCardData(int[] cbCardData, int cbBufferCount)
        {
            if(cbBufferCount==0) return 0;
            //效验参数
            //ASSERT(cbBufferCount>=m_cbCardCount);
            if (cbBufferCount<m_cbCardCount) return 0;

            //拷贝扑克
            for (int i=0;i<m_cbCardCount;i++) cbCardData[i]=m_CardItemArray[i].cbCardData;

            return m_cbCardCount;
        }

        //获取扑克
        int GetShootCard(int[] cbCardData, byte cbBufferCount)
        {
            //变量定义
            int wShootCount=0;

            //拷贝扑克
            for (int i=0;i<m_cbCardCount;i++) 
            {
	            //效验参数
	            //ASSERT(cbBufferCount>wShootCount);
	            if (cbBufferCount<=wShootCount) break;

	            //拷贝扑克
	            if (m_CardItemArray[i].bShoot==true) cbCardData[wShootCount++]=m_CardItemArray[i].cbCardData;
            }

            return wShootCount;
        }

        //获取扑克
        int GetCardData(tagCardItem[] CardItemArray, int cbBufferCount)
        {
            //效验参数
            //ASSERT(cbBufferCount>=m_cbCardCount);
            if (cbBufferCount<m_cbCardCount) return 0;

            //拷贝扑克
            //CopyMemory(CardItemArray,m_CardItemArray,sizeof(tagCardItem)*m_cbCardCount);
            for (int i = 0; i < m_cbCardCount; i++)
            {
                CardItemArray = m_CardItemArray;
            }

                return m_cbCardCount;
        }

        //设置距离
        public void SetCardDistance(int nXDistance, int nYDistance, int nShootDistance)
        {
            //设置变量
            m_nXDistance=nXDistance;
            m_nYDistance=nYDistance;
            m_nShootDistance=nShootDistance;

            return;
        }

        //获取中心
        void GetCenterPoint(Point CenterPoint)
        {
            //获取原点
            Point OriginPoint = GetOriginPoint();

            //获取位置
            Size ControlSize = GetControlSize();

            //设置中心
            CenterPoint.X=OriginPoint.X+ControlSize.Width/2;
            CenterPoint.Y = OriginPoint.Y+ControlSize.Height/2;

            return;
        }

        //基准位置
        public void SetBenchmarkPos(int nXPos, int nYPos, enXCollocateMode XCollocateMode, enYCollocateMode YCollocateMode)
        {
            //设置变量
            m_BenchmarkPos.X=nXPos;
            m_BenchmarkPos.Y=nYPos;
            m_XCollocateMode=XCollocateMode;
            m_YCollocateMode=YCollocateMode;

            return;
        }

        //基准位置
        public void SetBenchmarkPos(Point BenchmarkPos, enXCollocateMode XCollocateMode, enYCollocateMode YCollocateMode)
        {
            //设置变量
            m_BenchmarkPos=BenchmarkPos;
            m_XCollocateMode=XCollocateMode;
            m_YCollocateMode=YCollocateMode;

            return;
        }

        //绘画扑克
        public void DrawCardControl(Graphics g,bool ISbEffect)
        {
            //加载位图
            //CImageHandle HandleCard(&m_ImageCard);

            //扑克掩图
            //CImageHandle ImageCardMaskHandle(&m_ImageCardMask);
            //CImageHandle ImageWinHandle(&m_ImageWin);

            //掩图大小
            int nCardMaskWidth = m_ImageCardMask.Width;
            int nCardMaskHeight = m_ImageCardMask.Height;

            //获取位置
            Point OriginPoint = GetOriginPoint();

            //变量定义
            int nXDrawPos=0,nYDrawPos=0;
            int nXImagePos=0,nYImagePos=0;

            //绘画扑克
            for (int i=0;i<m_cbCardCount;i++)
            {
	            //获取扑克
	            bool bShoot=m_CardItemArray[i].bShoot;
	            bool bEffect = m_CardItemArray[i].bEffect;
	            int cbCardData=m_CardItemArray[i].cbCardData;

	            //间隙过滤
	            if (cbCardData==SPACE_CARD_DATA) continue;

	            //图片位置
	            if ((m_bDisplayItem==true)&&(cbCardData!=0))
	            {
		            if ((cbCardData==0x4E)||(cbCardData==0x4F))
		            {
			            nXImagePos=((cbCardData&CARD_MASK_VALUE)%14)*m_CardSize.Width;
			            nYImagePos=((cbCardData&CARD_MASK_COLOR)>>4)*m_CardSize.Height;
		            }
		            else
		            {
			            nXImagePos=((cbCardData&CARD_MASK_VALUE)-1)*m_CardSize.Width;
			            nYImagePos=((cbCardData&CARD_MASK_COLOR)>>4)*m_CardSize.Height;
		            }
	            }
	            else
	            {
		            nXImagePos=m_CardSize.Width*2;
		            nYImagePos=m_CardSize.Height*4;
	            }

	            //屏幕位置
	            if (m_bHorizontal==true)
	            {
		            nXDrawPos=m_nXDistance*i;
		            nYDrawPos=(bShoot==false)?m_nShootDistance:0;
	            }
	            else
	            {
		            nXDrawPos=0;
		            nYDrawPos=m_nYDistance*i;
	            }

	            //绘画扑克
	            GameGraphics.DrawAlphaImage( g, m_ImageCard, OriginPoint.X+nXDrawPos,OriginPoint.Y+nYDrawPos,m_CardSize.Width,m_CardSize.Height,nXImagePos,nYImagePos,Color.FromArgb(255,0,255));
	            if(bEffect&&ISbEffect)
		            GameGraphics.DrawAlphaImage( g, m_ImageCardMask,OriginPoint.X+nXDrawPos-2,OriginPoint.Y+nYDrawPos-2,nCardMaskWidth,nCardMaskHeight,0,0,Color.FromArgb(255,0,255));

	            //扑克标志
	            if(m_CardItemArray[i].bMy)
                    GameGraphics.DrawAlphaImage(g, m_ImageWin, OriginPoint.X + nXDrawPos, OriginPoint.Y + m_CardSize.Height + ((m_YCollocateMode == enYCollocateMode.enYTop) ? (m_CardSize.Height / 2 - m_ImageWin.Width) : 0) - m_ImageWin.Height,
			            m_ImageWin.Width,m_ImageWin.Height,0,0,Color.FromArgb(255,0,255));
            }

            return;
        }

        //光标消息
        // pakcj-later
        //bool OnEventSetCursor(Point Point)
        //{
        //    //光标处理
        //    if (m_bPositively==true)
        //    {
        //        //获取索引
        //        int wHoverItem=SwitchCardPoint(Point);

        //        //更新判断
        //        if (wHoverItem!=INVALID_ITEM)
        //        {
        //            SetCursor(LoadCursor(AfxGetInstanceHandle(),MAKEINTRESOURCE(IDC_CARD_CUR)));
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //获取大小
        public Size GetControlSize()
        {
            Size ControlSize = new Size();

            //获取大小
            if (m_bHorizontal==true)
            {
	            ControlSize.Height=m_CardSize.Height+m_nShootDistance;
	            ControlSize.Width=(m_cbCardCount>0)?(m_CardSize.Width+(m_cbCardCount-1)*m_nXDistance):0;
            }
            else
            {
	            ControlSize.Width=m_CardSize.Width;
	            ControlSize.Height=(m_cbCardCount>0)?(m_CardSize.Height+(m_cbCardCount-1)*m_nYDistance):0;
            }

            return ControlSize;
        }

        //获取原点
        public Point GetOriginPoint()
        {
            Point OriginPoint = new Point();

            //获取位置
            Size ControlSize = GetControlSize();

            //横向位置
            switch (m_XCollocateMode)
            {
            case enXCollocateMode.enXLeft:	{ OriginPoint.X=m_BenchmarkPos.X; break; }
            case enXCollocateMode.enXCenter: { OriginPoint.X=m_BenchmarkPos.X-ControlSize.Width/2; break; }
            case enXCollocateMode.enXRight:	{ OriginPoint.X=m_BenchmarkPos.X-ControlSize.Width; break; }
            }

            //竖向位置
            switch (m_YCollocateMode)
            {
            case enYCollocateMode.enYTop:	{ OriginPoint.Y=m_BenchmarkPos.Y; break; }
            case enYCollocateMode.enYCenter: { OriginPoint.Y=m_BenchmarkPos.Y-ControlSize.Height/2; break; }
            case enYCollocateMode.enYBottom: { OriginPoint.Y=m_BenchmarkPos.Y-ControlSize.Height; break; }
            }

            return OriginPoint;
        }

        //索引切换
        int SwitchCardPoint(Point MousePoint)
        {
            //获取位置
            Size ControlSize = GetControlSize();
            Point OriginPoint = GetOriginPoint();           
            

            //基准位置
            int nXPos=MousePoint.X-OriginPoint.X;
            int nYPos=MousePoint.Y-OriginPoint.Y;

            //横向模式
            if (m_bHorizontal==true)
            {
	            //越界判断
	            if ((nXPos<=0)||(nXPos>=ControlSize.Width)) return INVALID_ITEM;
	            if ((nYPos<=0)||(nYPos>=ControlSize.Height)) return INVALID_ITEM;

	            //计算索引
	            int wCardIndex=nXPos/m_nXDistance;
	            if (wCardIndex>=m_cbCardCount) wCardIndex=(m_cbCardCount-1);

	            //扑克搜索
	            for (int i=0;i<=wCardIndex;i++)
	            {
		            //变量定义
		            int wCurrentIndex=wCardIndex-i;

		            //横向测试
		            if (nXPos>=(int)(wCurrentIndex*m_nXDistance+m_CardSize.Width)) break;

		            //竖向测试
		            bool bShoot=m_CardItemArray[wCurrentIndex].bShoot;
		            if ((bShoot==true)&&(nYPos<=m_CardSize.Height)) return wCurrentIndex;
		            if ((bShoot==false)&&(nYPos>=(int)m_nShootDistance)) return wCurrentIndex;
	            }
            }

            return INVALID_ITEM;
        }

        //扑克数目
        public int GetCardCount() { return m_cbCardCount; }

        //设置方向
        public void SetDirection(bool bHorizontal) { m_bHorizontal = bHorizontal; }
        //设置响应
        public void SetPositively(bool bPositively) { m_bPositively = bPositively; }
        //设置显示
        public void SetDisplayItem(bool bDisplayItem) { m_bDisplayItem = bDisplayItem; }

        //获取大小
        public Size GetCardSize() { return m_CardSize; }
        //查询方向
        public bool GetDirection() { return m_bHorizontal; }
        //查询响应
        public bool GetPositively() { return m_bPositively; }
        //查询显示
        public bool GetDisplayItem() { return m_bDisplayItem; }


    }
}
