using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GameControls;

namespace DzCardClient
{
    //扑克结构
    class tagCardItem
    {
        public bool bEffect;							//特效标志
        public bool bMy;								//自己标志
        public bool bShoot;								//弹起标志
        public int cbCardData;							//扑克数据
    };

    class SmallCardControl
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
        const int CARD_MASK_VALUE = 0x0F;									//数值掩码

        

        //扑克数据
        int m_wCardCount; //扑克数目
        tagCardItem[] m_CardItemArray = new tagCardItem[MAX_CARD_COUNT]; //扑克数据

        //间隔变量
        int m_nXDistance; //横向间隔
        int m_nYDistance; //竖向间隔
        int m_nShootDistance; //弹起高度

        //位置变量
        Point m_BenchmarkPos = new Point(); //基准位置
        enXCollocateMode m_XCollocateMode = new enXCollocateMode(); //显示模式
        enYCollocateMode m_YCollocateMode = new enYCollocateMode(); //显示模式

        //资源变量
        Size m_CardSize = new Size(); //扑克大小
        static Bitmap m_ImageCard; //图片资源

        //构造函数
        public SmallCardControl()
        {
	        //状态变量
	        m_bHorizontal=true;
	        m_bPositively=false;
	        m_bDisplayItem=false;

	        //扑克数据
	        m_wCardCount=0;

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
	        m_ImageCard = Properties.Resources.SMALL_CARD;

	        //获取大小
	        //CImageHandle HandleImage(&m_ImageCard);
	        m_CardSize = new Size(m_ImageCard.Width, m_ImageCard.Height);

	        return;
        }

        //设置扑克
        public bool SetCardData(int wCardCount)
        {
	        //ASSERT(wCardCount>=m_wCardCount);
	        //if(m_wCardCount==2 && (wCardCount>=m_wCardCount))return false;

	        //效验参数
	        //ASSERT(wCardCount<=CountArray(m_CardItemArray));
	        if (wCardCount>m_CardItemArray.Length) 
                return false;

	        //设置变量
	        m_wCardCount=wCardCount;

            for (int i = 0; i < m_CardItemArray.Length; i++)
                m_CardItemArray[i] = new tagCardItem();
            

	        return true;
        }

        //设置扑克
        public bool SetCardData(int[] cbCardData, int wCardCount)
        {
	        //效验参数
	        //ASSERT(wCardCount<=CountArray(m_CardItemArray));
	        if (wCardCount>m_CardItemArray.Length) 
                return false;

	        //设置变量
	        m_wCardCount=wCardCount;

	        //设置扑克
	        for (int i=0;i<wCardCount;i++)
	        {
		        m_CardItemArray[i].bShoot=false;
		        m_CardItemArray[i].cbCardData=cbCardData[i];
	        }

	        return true;
        }

        //设置扑克
        bool SetShootCard(byte[] cbCardData, int wCardCount)
        {
	        //变量定义
	        bool bChangeStatus=false;

	        //收起扑克
	        for (int i=0;i<m_wCardCount;i++) 
	        {
		        if (m_CardItemArray[i].bShoot==true)
		        {
			        bChangeStatus=true;
			        m_CardItemArray[i].bShoot=false;
		        }
	        }

	        //弹起扑克
	        for (int i=0;i<wCardCount;i++)
	        {
		        for (int j=0;j<m_wCardCount;j++)
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

        //设置扑克
        bool SetCardItem(tagCardItem[] CardItemArray, int wCardCount)
        {
	        //效验参数
	        //ASSERT(wCardCount<=CountArray(m_CardItemArray));
	        if (wCardCount>m_CardItemArray.Length) 
                return false;

	        //设置扑克
	        m_wCardCount=wCardCount;

            for(int i = 0; i < m_CardItemArray.Length; i++ )
	            m_CardItemArray[i] = CardItemArray[i];

	        return true;
        }
        
        //获取扑克
        tagCardItem GetCardFromIndex(int wIndex)
        {
            if (wIndex < m_wCardCount)
                return m_CardItemArray[wIndex];
            
            return null;
        }
        
        //获取扑克
        tagCardItem GetCardFromPoint(Point MousePoint)
        {
            int wIndex = SwitchCardPoint(MousePoint);

            if (wIndex != INVALID_ITEM)
                return m_CardItemArray[wIndex];
            
            return null;
        }

        //获取扑克
        private int GetCardData(int[] cbCardData, int wBufferCount)
        {
	        //效验参数
	        //void Assert(wBufferCount>=m_wCardCount);
	        if (wBufferCount<m_wCardCount) return 0;

	        //拷贝扑克
	        for (int i=0;i<m_wCardCount;i++) cbCardData[i]=m_CardItemArray[i].cbCardData;

	        return m_wCardCount;
        }
        //获取扑克
        private int GetShootCard(int[] cbCardData, int wBufferCount)
        {
	        //变量定义
	        int wShootCount=0;

	        //拷贝扑克
	        for (int i = 0; i < m_wCardCount; i++) 
	        {
		        //效验参数
		        //ASSERT(wBufferCount>wShootCount);
		        if (wBufferCount<=wShootCount) break;

		        //拷贝扑克
		        if (m_CardItemArray[i].bShoot==true) cbCardData[wShootCount++]=m_CardItemArray[i].cbCardData;
	        }

	        return wShootCount;
        }
        //获取扑克
        private int GetCardData(tagCardItem[] CardItemArray, int wBufferCount)
        {
	        //效验参数
	        //ASSERT(wBufferCount>=m_wCardCount);
	        if (wBufferCount<m_wCardCount) return 0;

	        //拷贝扑克
            
            for (int i = 0; i < m_wCardCount; i++)
            {
                CardItemArray = m_CardItemArray;
            }
            //    CopyMemory(CardItemArray, m_CardItemArray, sizeof(tagCardItem) * m_wCardCount);

	        return m_wCardCount;
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
	        CenterPoint.X = OriginPoint.X + ControlSize.Width/2;
	        CenterPoint.Y = OriginPoint.Y + ControlSize.Height/2;

	        return;
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
	            case enXCollocateMode.enXCenter: { OriginPoint.X=m_BenchmarkPos.X - ControlSize.Width/2; break; }
	            case enXCollocateMode.enXRight:	{ OriginPoint.X=m_BenchmarkPos.X - ControlSize.Width; break; }
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

        //获取大小
        Size GetControlSize()
        {
            Size ControlSize = new Size();

	        //获取大小
	        if (m_bHorizontal==true)
	        {
		        ControlSize.Height=m_CardSize.Height + m_nShootDistance;
		        ControlSize.Width=(m_wCardCount>0)?(m_CardSize.Width+(m_wCardCount-1)*m_nXDistance):0;
	        }
	        else
	        {
		        ControlSize.Width=m_CardSize.Width;
		        ControlSize.Height=(m_wCardCount>0)?(m_CardSize.Height+(m_wCardCount-1)*m_nYDistance):0;
	        }

	        return ControlSize;
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
        public void DrawCardControl(Graphics g)
        {
	        //加载位图
	        //CImageHandle HandleCard(&m_ImageCard);

	        //获取位置
	        Point OriginPoint = GetOriginPoint();

	        //变量定义
	        int nXDrawPos=0;
            int nYDrawPos=0;
	        int nXImagePos=0;
            int nYImagePos=0;

	        //绘画扑克
	        for (int i=0;i<m_wCardCount;i++)
	        {
		        //获取扑克
		        bool bShoot=m_CardItemArray[i].bShoot;
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
			        nXImagePos=0;
			        nYImagePos=0;
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
		        if ((nYPos<=0)||(nYPos>=ControlSize.Width)) return INVALID_ITEM;

		        //计算索引
		        int wCardIndex=nXPos/m_nXDistance;
		        if (wCardIndex>=m_wCardCount) wCardIndex=(m_wCardCount-1);

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

        // 在指定位置绘制一张牌, bCardData为0时绘制背面
        public void DrawOneCard(Graphics g, byte bCardData,int nX , int nY  )
        {
            //变量定义

            Color clrTrans = Color.FromArgb( 255, 0, 255 );
            //CImageHandle HandleCard(&m_ImageCard);
            
            GameGraphics.DrawAlphaImage( g, m_ImageCard, nX, nY, SMALL_CARD_WIDTH, SMALL_CARD_HEIGHT, 0, 0, clrTrans );
            return;

        }

        //扑克数码
        public int GetCardCount() { return m_wCardCount; }

	    //获取大小
	    public Size GetCardSize() { return m_CardSize; }
	    //查询方向
	    public bool GetDirection() { return m_bHorizontal; }
	    //查询响应
	    public bool GetPositively() { return m_bPositively; }
	    //查询显示
	    public bool GetDisplayItem() { return m_bDisplayItem; }

	    //状态控制
	    //设置方向
	    public void SetDirection(bool bHorizontal) { m_bHorizontal=bHorizontal; }
	    //设置响应
	    public void SetPositively(bool bPositively) { m_bPositively=bPositively; }
	    //设置显示
	    public void SetDisplayItem(bool bDisplayItem) { m_bDisplayItem=bDisplayItem; }

    }
}
