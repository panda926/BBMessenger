using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatEngine
{
    //分析结构
    public class tagAnalyseResult
    {
        public int 							cbFourCount;						//四张数目
        public int cbThreeCount;						//三张数目
        public int cbLONGCount;						//两张数目
        public int cbSignedCount;						//单张数目
        public int[] cbFourLogicVolue = new int[1];				//四张列表
        public int[] cbThreeLogicVolue = new int[1];				//三张列表
        public int[] cbLONGLogicVolue = new int[2];				//两张列表
        public int[] cbSignedLogicVolue = new int[5];				//单张列表
        public int[] cbFourCardData = new int[DzCardDefine.MAX_CENTERCOUNT];			//四张列表
        public int[] cbThreeCardData = new int[DzCardDefine.MAX_CENTERCOUNT];			//三张列表
        public int[] cbLONGCardData = new int[DzCardDefine.MAX_CENTERCOUNT];		//两张列表
        public int[] cbSignedCardData = new int[DzCardDefine.MAX_CENTERCOUNT];		//单张数目
    };

    //胜利信息结构
    public class UserWinList
    {
	    public int bSameCount;
	    public int[] wWinerList = new int[DzCardDefine.GAME_PLAYER];
    };


    public class DzCardLogic
    {
        const int CT_SINGLE	=				1;									//单牌类型
        const int CT_ONE_LONG=				2;									//对子类型
        const int CT_TWO_LONG=				3;									//两对类型
        const int CT_THREE_TIAO=				4;									//三条类型
        const int CT_SHUN_ZI	=				5;									//顺子类型
        const int CT_TONG_HUA	=				6;									//同花类型
        const int CT_HU_LU		=			7;									//葫芦类型
        const int CT_TIE_ZHI	=				8;									//铁支类型
        const int CT_TONG_HUA_SHUN	=		9;									//同花顺型
        const int CT_KING_TONG_HUA_SHUN	=	10;									//皇家同花顺


        //数值掩码
        const int LOGIC_MASK_COLOR	=		0xF0;								//花色掩码
        const int LOGIC_MASK_VALUE = 0x0F;								//数值掩码

        //////////////////////////////////////////////////////////////////////////
        //扑克数据
        int[] m_cbCardData = new int[]
        {
	        //0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,	//方块 A - K
	        //0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1A,0x1B,0x1C,0x1D,	//梅花 A - K
	        //0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,	//红桃 A - K
	        //0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3A,0x3B,0x01,0x3C,0x3D,	//黑桃 A - K

	        0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,	//方块 A - K
	        0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1A,0x1B,0x1C,0x1D,	//梅花 A - K
	        0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,	//红桃 A - K
	        0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3A,0x3B,0x3C,0x3D,	//黑桃 A - K
        };

        Random random = new Random();

        //构造函数
        public DzCardLogic()
        {
        }

        //获取数值
        int GetCardValue(int cbCardData) { return cbCardData & LOGIC_MASK_VALUE; }
        
        //获取花色
        int GetCardColor(int cbCardData) { return cbCardData & LOGIC_MASK_COLOR; }


        //混乱扑克
        public void RandCardList(int[] cbCardBuffer, int cbBufferCount)
        {
	        //测试代码
	        //CopyMemory(cbCardBuffer,m_cbCardData,cbBufferCount);
	        //混乱准备
	        int[] cbCardData = new int[m_cbCardData.Length];
            Array.Copy(m_cbCardData, cbCardData, m_cbCardData.Length);
	        //CopyMemory(cbCardData,m_cbCardData,sizeof(m_cbCardData));

	        //混乱扑克
	        int cbRandCount=0,cbPosition=0;

	        do
	        {
                cbPosition = random.Next() % (cbCardData.Length - cbRandCount);
		        cbCardBuffer[cbRandCount++]=cbCardData[cbPosition];
                cbCardData[cbPosition] = cbCardData[cbCardData.Length - cbRandCount];
	        } 
            while (cbRandCount<cbBufferCount);

	        return;
        }

        //获取类型
        int GetCardType(int[] cbCardData, int cbCardCount)
        {
	        //数据校验
	        //ASSERT(cbCardCount == DzCardDefine.MAX_CENTERCOUNT);
	        if(cbCardCount !=DzCardDefine.MAX_CENTERCOUNT) 
                return 0;

	        //变量定义
	        bool cbSameColor=true,bLineCard=true;
	        int cbFirstColor=GetCardColor(cbCardData[0]);
	        int cbFirstValue=GetCardLogicValue(cbCardData[0]);

	        //牌形分析
	        for (int i=1;i<cbCardCount;i++)
	        {
		        //数据分析
		        if (GetCardColor(cbCardData[i])!=cbFirstColor) cbSameColor=false;
		        if (cbFirstValue!=(GetCardLogicValue(cbCardData[i])+i)) bLineCard=false;

		        //结束判断
		        if ((cbSameColor==false)&&(bLineCard==false)) break;
	        }

	        //最小同花顺
	        if((bLineCard == false)&&(cbFirstValue == 14))
	        {
		        int i=1;
		        for (i=1;i<cbCardCount;i++)
		        {
			        int cbLogicValue=GetCardLogicValue(cbCardData[i]);
			        if ((cbFirstValue!=(cbLogicValue+i+8)))break;
		        }
		        if( i == cbCardCount)
			        bLineCard =true;

	        }

	        //皇家同花顺
	        if ((cbSameColor==true)&&(bLineCard==true)&&(GetCardLogicValue(cbCardData[1]) ==13 )) 
		        return CT_KING_TONG_HUA_SHUN;

	        //顺子类型
	        if ((cbSameColor==false)&&(bLineCard==true)) 
		        return CT_SHUN_ZI;

	        //同花类型
	        if ((cbSameColor==true)&&(bLineCard==false)) 
		        return CT_TONG_HUA;

	        //同花顺类型
	        if ((cbSameColor==true)&&(bLineCard==true))
		        return CT_TONG_HUA_SHUN;

	        //扑克分析
	        tagAnalyseResult AnalyseResult = new tagAnalyseResult();
	        AnalysebCardData(cbCardData,cbCardCount,AnalyseResult);

	        //类型判断
	        if (AnalyseResult.cbFourCount==1) 
		        return CT_TIE_ZHI;
	        if (AnalyseResult.cbLONGCount==2) 
		        return CT_TWO_LONG;
	        if ((AnalyseResult.cbLONGCount==1)&&(AnalyseResult.cbThreeCount==1))
		        return CT_HU_LU;
	        if ((AnalyseResult.cbThreeCount==1)&&(AnalyseResult.cbLONGCount==0))
		        return CT_THREE_TIAO;
	        if ((AnalyseResult.cbLONGCount==1)&&(AnalyseResult.cbSignedCount==3)) 
		        return CT_ONE_LONG;

	        return CT_SINGLE;
        }

        //查找扑克
        public int GetSameCard( int[] bCardData, int[] bMaxCard,int bCardCount,int bMaxCardCount,int[] bResultData)
        {
	        if(bCardData[0]==0 || bMaxCard[0]==0)return 0;
	        int bTempCount = 0;
	        for (int i=0;i<bCardCount;i++)
	        {
		        for (int j=0;j<bMaxCardCount;j++)
		        {
			        if(bCardData[i]==bMaxCard[j])bResultData[bTempCount++] = bMaxCard[j];
		        }
	        }
	        return bTempCount;
        }

        //最大牌型
        public int FiveFromSeven(int[] cbHandCardData,int cbHandCardCount,int[] cbCenterCardData,int cbCenterCardCount,int[] cbLastCardData,int cbLastCardCount)
        {
	        //临时变量
	        int[] cbTempCardData = new int[DzCardDefine.MAX_COUNT+DzCardDefine.MAX_CENTERCOUNT];
            int[] cbTempLastCardData = new int[5];

	        //ZeroMemory(cbTempCardData,sizeof(cbTempCardData));
	        //ZeroMemory(cbTempLastCardData,sizeof(cbTempLastCardData));

	        //拷贝数据
	        Array.Copy( cbHandCardData, cbTempCardData, cbHandCardData.Length );
	        Array.Copy( cbCenterCardData, 0, cbTempCardData, 2, DzCardDefine.MAX_CENTERCOUNT);

	        //排列扑克
	        SortCardList(cbTempCardData,cbTempCardData.Length);

	        Array.Copy( cbTempCardData, cbLastCardData, DzCardDefine.MAX_CENTERCOUNT );
	        int cbCardKind = GetCardType(cbLastCardData,DzCardDefine.MAX_CENTERCOUNT);
	        int cbTempCardKind = 0;

	        //组合算法
	        for (int i=0;i<3;i++)
	        {
		        for (int j= i+1;j<4;j++)
		        {
			        for (int k = j+1;k<5;k++)
			        {
				        for (int l =k+1;l<6;l++)
				        {
					        for (int m=l+1;m<7;m++)
					        {
						        //获取数据
						        cbTempLastCardData[0]=cbTempCardData[i];
						        cbTempLastCardData[1]=cbTempCardData[j];
						        cbTempLastCardData[2]=cbTempCardData[k];
						        cbTempLastCardData[3]=cbTempCardData[l];
						        cbTempLastCardData[4]=cbTempCardData[m];

						        //获取牌型
						        cbTempCardKind = GetCardType(cbTempLastCardData,cbTempLastCardData.Length);

						        //牌型大小
						        if(CompareCard(cbTempLastCardData,cbLastCardData,cbTempLastCardData.Length)==2)
						        {
							        cbCardKind = cbTempCardKind;
							        Array.Copy(cbTempLastCardData, cbLastCardData,cbTempLastCardData.Length);
						        }
					        }
				        }
			        }
		        }
	        }

	        return cbCardKind;
        }

        //查找最大
        public bool SelectMaxUser(int[][] bCardData, UserWinList EndResult, int[] lAddScore)
        {
	        //清理数据
	        //EndResult = new UserWinList();

	        //First数据
	        int wWinnerID = GameDefine.INVALID_CHAIR;
	        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        if(bCardData[i][0]!=0)
		        {
			        wWinnerID=i;
			        break;
		        }
	        }

	        //过滤全零
	        if(wWinnerID==GameDefine.INVALID_CHAIR)
                return false;

	        //查找最大用户
	        int wTemp = wWinnerID;
	        for(int i=1;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        int j=(i+wTemp)%DzCardDefine.GAME_PLAYER;
		        if(bCardData[j][0]==0)continue;
		        if(CompareCard(bCardData[j],bCardData[wWinnerID],DzCardDefine.MAX_CENTERCOUNT)>1)
		        {
			        wWinnerID=j;
		        }
	        }

	        //查找相同数据
	        EndResult.wWinerList[EndResult.bSameCount++] = wWinnerID;
	        for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        if(i==wWinnerID || bCardData[i][0]==0)continue;
		        if(CompareCard(bCardData[i],bCardData[wWinnerID],DzCardDefine.MAX_CENTERCOUNT)==0)
		        {
			        EndResult.wWinerList[EndResult.bSameCount++] = i;
		        }
	        }

	        //从小到大
	        if(EndResult.bSameCount>1 && lAddScore!=null)
	        {
		        int iSameCount=(int)EndResult.bSameCount;
		        while((iSameCount--)>0)
		        {
			        int lTempSocre = lAddScore[EndResult.wWinerList[iSameCount]];
			        for(int i=iSameCount-1;i>=0;i--)
			        {
				        //ASSERT(lAddScore[EndResult.wWinerList[i]]>0);
				        if(lTempSocre < lAddScore[EndResult.wWinerList[i]])
				        {
					        lTempSocre = lAddScore[EndResult.wWinerList[i]];
					        wTemp = EndResult.wWinerList[iSameCount];
					        EndResult.wWinerList[iSameCount] = EndResult.wWinerList[i];
					        EndResult.wWinerList[i] = wTemp;
				        }
			        }
		        }
	        }

	        return true;
        }

        //排列扑克
        void SortCardList(int[] cbCardData, int cbCardCount)
        {
	        //转换数值
	        int[] cbLogicValue = new int[DzCardDefine.FULL_COUNT];
	        for (int i=0;i<cbCardCount;i++) 
		        cbLogicValue[i]=GetCardLogicValue(cbCardData[i]);	

	        //排序操作
	        bool bSorted=true;
	        int cbTempData,bLast=cbCardCount-1;
	        do
	        {
		        bSorted=true;
		        for (int i=0;i<bLast;i++)
		        {
			        if ((cbLogicValue[i]<cbLogicValue[i+1])||
				        ((cbLogicValue[i]==cbLogicValue[i+1])&&(cbCardData[i]<cbCardData[i+1])))
			        {
				        //交换位置
				        cbTempData=cbCardData[i];
				        cbCardData[i]=cbCardData[i+1];
				        cbCardData[i+1]=cbTempData;
				        cbTempData=cbLogicValue[i];
				        cbLogicValue[i]=cbLogicValue[i+1];
				        cbLogicValue[i+1]=cbTempData;
				        bSorted=false;
			        }	
		        }
		        bLast--;
	        } while(bSorted==false);

	        return;
        }

        //逻辑数值
        int GetCardLogicValue(int cbCardData)
        {
	        //扑克属性
	        int bCardColor=GetCardColor(cbCardData);
	        int bCardValue=GetCardValue(cbCardData);

	        //转换数值
	        return (bCardValue==1)?(bCardValue+13):bCardValue;
        }

        //对比扑克
        int CompareCard(int[] cbFirstData, int[] cbNextData, int cbCardCount)
        {
	        //获取类型
	        int cbNextType=GetCardType(cbNextData,cbCardCount);
	        int cbFirstType=GetCardType(cbFirstData,cbCardCount);

	        //类型判断
	        //大
	        if(cbFirstType>cbNextType)
		        return 2;

	        //小
	        if(cbFirstType<cbNextType)
		        return 1;

            int i = 0;

	        //简单类型
	        switch(cbFirstType)
	        {
	        case CT_SINGLE:			//单牌
		        {
			        //对比数值
			        for (i=0;i<cbCardCount;i++)
			        {
				        int cbNextValue=GetCardLogicValue(cbNextData[i]);
				        int cbFirstValue=GetCardLogicValue(cbFirstData[i]);

				        //大
				        if(cbFirstValue > cbNextValue)
					        return 2;
				        //小
				        else if(cbFirstValue <cbNextValue)
					        return 1;
				        //等
				        else
					        continue;
			        }
			        //平
			        if( i == cbCardCount)
				        return 0;
			        //ASSERT(FALSE);
		        }
                break;

	        case CT_ONE_LONG:		//对子
	        case CT_TWO_LONG:		//两对
	        case CT_THREE_TIAO:		//三条
	        case CT_TIE_ZHI:		//铁支
	        case CT_HU_LU:			//葫芦
		        {
			        //分析扑克
			        tagAnalyseResult AnalyseResultNext = new tagAnalyseResult();
			        tagAnalyseResult AnalyseResultFirst = new tagAnalyseResult();
			        AnalysebCardData(cbNextData,cbCardCount,AnalyseResultNext);
			        AnalysebCardData(cbFirstData,cbCardCount,AnalyseResultFirst);

			        //四条数值
			        if (AnalyseResultFirst.cbFourCount>0)
			        {
				        int cbNextValue=AnalyseResultNext.cbFourLogicVolue[0];
				        int cbFirstValue=AnalyseResultFirst.cbFourLogicVolue[0];

				        //比较四条
				        if(cbFirstValue != cbNextValue)return (cbFirstValue > cbNextValue)?2:1;

				        //比较单牌
				        //ASSERT(AnalyseResultFirst.cbSignedCount==1 && AnalyseResultNext.cbSignedCount==1);
				        cbFirstValue = AnalyseResultFirst.cbSignedLogicVolue[0];
				        cbNextValue = AnalyseResultNext.cbSignedLogicVolue[0];
				        if(cbFirstValue != cbNextValue)return (cbFirstValue > cbNextValue)?2:1;
				        else return 0;
			        }

			        //三条数值
			        if (AnalyseResultFirst.cbThreeCount>0)
			        {
				        int cbNextValue=AnalyseResultNext.cbThreeLogicVolue[0];
				        int cbFirstValue=AnalyseResultFirst.cbThreeLogicVolue[0];

				        //比较三条
				        if(cbFirstValue != cbNextValue)return (cbFirstValue > cbNextValue)?2:1;

				        //葫芦牌型
				        if(CT_HU_LU == cbFirstType)
				        {
					        //比较对牌
					        //ASSERT(AnalyseResultFirst.cbLONGCount==1 && AnalyseResultNext.cbLONGCount==1);
					        cbFirstValue = AnalyseResultFirst.cbLONGLogicVolue[0];
					        cbNextValue = AnalyseResultNext.cbLONGLogicVolue[0];
					        if(cbFirstValue != cbNextValue)return (cbFirstValue > cbNextValue)?2:1;
					        else return 0;
				        }
				        else //三条带单
				        {
					        //比较单牌
					        //ASSERT(AnalyseResultFirst.cbSignedCount==2 && AnalyseResultNext.cbSignedCount==2);

					        //散牌数值
					        for (i=0;i<AnalyseResultFirst.cbSignedCount;i++)
					        {
						        cbNextValue=AnalyseResultNext.cbSignedLogicVolue[i];
						        cbFirstValue=AnalyseResultFirst.cbSignedLogicVolue[i];

						        //大
						        if(cbFirstValue > cbNextValue)
							        return 2;
						        //小
						        else if(cbFirstValue <cbNextValue)
							        return 1;
						        //等
						        else continue;
					        }
					        if( i == AnalyseResultFirst.cbSignedCount)
						        return 0;
					        //ASSERT(FALSE);
				        }
			        }

			        //对子数值
			        i=0;
			        for ( i=0;i<AnalyseResultFirst.cbLONGCount;i++)
			        {
				        int cbNextValue=AnalyseResultNext.cbLONGLogicVolue[i];
				        int cbFirstValue=AnalyseResultFirst.cbLONGLogicVolue[i];
				        //大
				        if(cbFirstValue > cbNextValue)
					        return 2;
				        //小
				        else if(cbFirstValue <cbNextValue)
					        return 1;
				        //平
				        else
					        continue;
			        }

			        //比较单牌
			        //ASSERT( i == AnalyseResultFirst.cbLONGCount);
			        {
				        //ASSERT(AnalyseResultFirst.cbSignedCount==AnalyseResultNext.cbSignedCount && AnalyseResultNext.cbSignedCount>0);
				        //散牌数值
				        for (i=0;i<AnalyseResultFirst.cbSignedCount;i++)
				        {
					        int cbNextValue=AnalyseResultNext.cbSignedLogicVolue[i];
					        int cbFirstValue=AnalyseResultFirst.cbSignedLogicVolue[i];
					        //大
					        if(cbFirstValue > cbNextValue)
						        return 2;
					        //小
					        else if(cbFirstValue <cbNextValue)
						        return 1;
					        //等
					        else continue;
				        }
				        //平
				        if( i == AnalyseResultFirst.cbSignedCount)
					        return 0;
			        }
			        break;
		        }
	        case CT_SHUN_ZI:		//顺子
	        case CT_TONG_HUA_SHUN:	//同花顺
		        {
			        //数值判断
			        int cbNextValue=GetCardLogicValue(cbNextData[0]);
			        int cbFirstValue=GetCardLogicValue(cbFirstData[0]);

			        bool bFirstmin= (cbFirstValue ==(GetCardLogicValue(cbFirstData[1])+9));
			        bool bNextmin= (cbNextValue ==(GetCardLogicValue(cbNextData[1])+9));

			        //大小顺子
			        if ((bFirstmin==true)&&(bNextmin == false))
			        {
				        return 1;
			        }

			        //大小顺子
			        else if ((bFirstmin==false)&&(bNextmin == true))
			        {
				        return 2;
			        }

			        //等同顺子
			        else
			        {
				        //平
				        if(cbFirstValue == cbNextValue)return 0;		
				        return (cbFirstValue > cbNextValue)?2:1;			
			        }
		        }
                break;

	        case CT_TONG_HUA:		//同花
		        {	
			        //散牌数值
			        for (i=0;i<cbCardCount;i++)
			        {
				        int cbNextValue=GetCardLogicValue(cbNextData[i]);
				        int cbFirstValue=GetCardLogicValue(cbFirstData[i]);
				        if(cbFirstValue == cbNextValue)continue;
				        return (cbFirstValue > cbNextValue)?2:1;
			        }
			        //平
			        if( i == cbCardCount) return 0;
		        }
                break;
	        }

	        return  0;
        }

        //分析扑克
        void AnalysebCardData( int[] cbCardData, int cbCardCount, tagAnalyseResult AnalyseResult)
        {
	        //设置结果
	        //AnalyseResult = new tagAnalyseResult();

	        //扑克分析
	        for (int i=0;i<cbCardCount;i++)
	        {
		        //变量定义
		        int cbSameCount=1;
		        int[] cbSameCardData= new int[]{cbCardData[i],0,0,0};
		        int cbLogicValue=GetCardLogicValue(cbCardData[i]);

		        //获取同牌
		        for (int j=i+1;j<cbCardCount;j++)
		        {
			        //逻辑对比
			        if (GetCardLogicValue(cbCardData[j])!=cbLogicValue) break;

			        //设置扑克
			        cbSameCardData[cbSameCount++]=cbCardData[j];
		        }

		        //保存结果
		        switch (cbSameCount)
		        {
		        case 1:		//单张
			        {
				        AnalyseResult.cbSignedLogicVolue[AnalyseResult.cbSignedCount]=cbLogicValue;
				        Array.Copy( cbSameCardData, 0, AnalyseResult.cbSignedCardData, (AnalyseResult.cbSignedCount++)*cbSameCount,cbSameCount);
				        break;
			        }
		        case 2:		//两张
			        {
				        AnalyseResult.cbLONGLogicVolue[AnalyseResult.cbLONGCount]=cbLogicValue;
				        Array.Copy(cbSameCardData, 0, AnalyseResult.cbLONGCardData, (AnalyseResult.cbLONGCount++)*cbSameCount,cbSameCount);
				        break;
			        }
		        case 3:		//三张
			        {
				        AnalyseResult.cbThreeLogicVolue[AnalyseResult.cbThreeCount]=cbLogicValue;
				        Array.Copy(cbSameCardData, 0, AnalyseResult.cbThreeCardData, (AnalyseResult.cbThreeCount++)*cbSameCount,cbSameCount);
				        break;
			        }
		        case 4:		//四张
			        {
				        AnalyseResult.cbFourLogicVolue[AnalyseResult.cbFourCount]=cbLogicValue;
				        Array.Copy(cbSameCardData, 0, AnalyseResult.cbFourCardData, (AnalyseResult.cbFourCount++)*cbSameCount,cbSameCount);
				        break;
			        }
		        }

		        //设置递增
		        i+=cbSameCount-1;
	        }

	        return;
        }
//////////////////////////////////////////////////////////////////////////

    }


}
