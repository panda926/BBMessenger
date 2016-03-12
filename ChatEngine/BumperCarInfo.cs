using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class BumperCarDefine 
    {
        public const int KIND_ID		=				108;									//游戏 I D
        public const int GAME_PLAYER	=				100;									//游戏人数
        public const string GAME_NAME	=				"碰碰车";						//游戏名字

        //版本信息
        //public const int VERSION_SERVER	=		    PROCESS_VERSION(6,0,3)				//程序版本
        //public const int VERSION_CLIENT	=			PROCESS_VERSION(6,0,3)				//程序版本

        //状态定义
        //#define	GS_PLACE_JETTON		=		GAME_STATUS_PLAY						//下注状态
        //#define	GS_GAME_END			=		GAME_STATUS_PLAY+1						//结束状态
        //#define	GS_MOVECARD_END		=		GAME_STATUS_PLAY+2						//结束状态

        //区域索引
        public const int ID_TIAN_MEN	=				1;									//顺门
        public const int ID_DI_MEN		=			2;									//左边角
        public const int ID_XUAN_MEN	=				3;									//桥
        public const int ID_HUANG_MEN	=			4;									//对门

        //玩家索引
        public const int BANKER_INDEX	=			0;									//庄家索引
        public const int SHUN_MEN_INDEX		=		1;									//顺门索引
        public const int DUI_MEN_INDEX	=			2;									//对门索引
        public const int DAO_MEN_INDEX		=		3;									//倒门索引
        public const int HUAN_MEN_INDEX	=			4;									//倒门索引

        public const int AREA_COUNT		=			8;									//区域数目

        //赔率定义
        public const int RATE_TWO_PAIR	=			12;									//对子赔率

        //常量定义
        public const int SEND_COUNT		=			300;									//发送次数

        //索引定义
        public const int INDEX_PLAYER	=			0;									//闲家索引
        public const int INDEX_BANKER	=			1;									//庄家索引

        //下注时间
        public const int IDI_FREE		=			1;									//空闲时间
        public const int TIME_FREE		=			1;									//空闲时间

        //下注时间
        public const int IDI_PLACE_JETTON	=		2;									//下注时间
        public const int TIME_PLACE_JETTON	=		30;									//下注时间

        //结束时间
        public const int IDI_GAME_END		=		3;									//结束时间
        public const int TIME_GAME_END = 23;									        //结束时间

        public const int AREA_LIMIT_SCORE = 1000000;
        public const int USER_LIMIT_SCORE = 1000000;

        // added
        public const int FIRST_EVENT_SOCRE = 2001;
    }

    public class BumperCarInfo : TableInfo
    {
	    //总下注数
	    public int[]				m_lAllJettonScore = new int[BumperCarDefine.AREA_COUNT+1];	//全体总注
    	
	    //个人下注
	    public int[,]				m_lUserJettonScore = new int[BumperCarDefine.AREA_COUNT+1,BumperCarDefine.GAME_PLAYER];//个人总注

	    //控制变量
	    public int						m_lAreaLimitScore = BumperCarDefine.AREA_LIMIT_SCORE;						//区域限制
	    public int						m_lUserLimitScore = BumperCarDefine.USER_LIMIT_SCORE;						//区域限制
	    public int						m_lApplyBankerCondition;				//申请条件

	    //玩家成绩
	    //public int[]					m_lUserWinScore = new int[BumperCarDefine.GAME_PLAYER];			//玩家成绩
	    public int[]					m_lUserReturnScore = new int[BumperCarDefine.GAME_PLAYER];		//返回下注
	    public int[]					m_lUserRevenue = new int[BumperCarDefine.GAME_PLAYER];			//玩家税收
	    public int						m_cbLeftCardCount;						//扑克数目
	    public bool						m_bContiueCard;							//继续发牌

	    //扑克信息
	    public int[]						m_cbCardCount = new int[1];						//扑克数目
        public int[,]						m_cbTableCardArray = new int[1,1];				//桌面扑克

	    //状态变量
	    public int							m_dwJettonTime;							//开始时间

	    //庄家信息
	    //public CWHArray<WORD>					m_ApplyUserArray;						//申请玩家
	    public int							m_wCurrentBanker = GameDefine.INVALID_CHAIR;						//当前庄家
	    public int							m_wBankerTime;							//做庄次数
	    public int						m_lBankerScore;							//
	    public int						m_lBankerWinScore;						//累计成绩
	    public int						m_lBankerCurGameScore;					//当前成绩
	    public bool							m_bEnableSysBanker =true;						//系统做庄


	    //记录变量
        //public tagServerGameRecord				m_GameRecordArrary[MAX_SCORE_HISTORY];	//游戏记录
        //public int								m_nRecordFirst;							//开始记录
        //public int								m_nRecordLast;							//最后记录
        //public CFile							m_FileRecord;							//记录结果
        //public DWORD							m_dwRecordCount;						//记录数目
        //public int								m_CheckImage;

	    //控制变量
	    public bool							m_bRefreshCfg;							//每盘刷新
	    public int						m_StorageScore;							//房间启动每桌子的库存数值，读取失败按 0 设置
	    public int						m_StorageDeduct;						//每局游戏结束后扣除的库存比例，读取失败按 1.00 设置
	    public string							m_szConfigFileName;			//配置文件
	    public string							m_szGameRoomName;			//房间名称

	    //机器人控制
	    public int								m_nMaxChipRobot;						//最大数目 (下注机器人)
	    public int								m_nChipRobotCount;						//人数统计 (下注机器人)
	    public int						m_lRobotAreaLimit;						//机器人区域限制
	    public int						m_lRobotBetCount;						//机器人下注个数
	    public int[]						m_lRobotAreaScore = new int[BumperCarDefine.AREA_COUNT+1];		//机器人区域下注

	    //庄家设置
	    //加庄局数设置：当庄家坐满设定的局数之后(m_lBankerMAX)，
	    //所带金币值还超过下面申请庄家列表里面所有玩家金币时，
	    //可以再加坐庄m_lBankerAdd局，加庄局数可设置。
	    public int						m_lBankerMAX;							//最大庄家数
	    public int						m_lBankerAdd;							//庄家增加数

	    //金币超过m_lBankerScoreMAX之后，就算是下面玩家的金币值大于他的金币值，他也可以再加庄m_lBankerScoreAdd局。
	    public int						m_lBankerScoreMAX;						//庄家钱
	    public int						m_lBankerScoreAdd;						//庄家钱大时,坐庄增加数

	    //最大庄家数
	    public int						m_lPlayerBankerMAX;						//玩家最大庄家数

	    //换庄
	    public bool							m_bExchangeBanker;						//交换庄家

	    //时间设置
	    public int							m_cbFreeTime = BumperCarDefine.TIME_FREE;							//空闲时间
	    public int							m_cbBetTime = BumperCarDefine.TIME_PLACE_JETTON;							//下注时间
	    public int							m_cbEndTime = BumperCarDefine.TIME_GAME_END;							//结束时间
    	

	    //组件变量
	    //public CGameLogic						m_GameLogic;							//游戏逻辑
	    //public ITableFrame						* m_pITableFrame;						//框架接口
	    //public const tagGameServiceOption		* m_pGameServiceOption;					//配置参数

	    //属性变量
	    //public static const WORD				m_wPlayerCount;							//游戏人数


        //开始模式

        public BumperCarInfo()
        {
            _InfoType = InfoType.BumperCar;

            m_EndingTime = m_cbEndTime;

            // added by usc at 2014/03/21
            m_StorageScore = BumperCarDefine.FIRST_EVENT_SOCRE;
            m_StorageDeduct = 1;

            return;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += (BumperCarDefine.AREA_COUNT + 1) * 4;
            size += 4;

            // added by usc at 2014/03/18
            size += _Players.Count * (BumperCarDefine.AREA_COUNT + 1) * 4;

            //size += m_cbHorsesRanking.Length * 4;
            //size += _Players.Count * 4;
            //size += _Players.Count * 4;

            //size += HorseDefine.HORSES_ALL * HorseDefine.COMPLETION_TIME * 4;

            //size += m_nWinCount.Length * 4;
            //size += m_nMultiple.Length * 4;

            //size += 4; // m_nStreak;
            //size += m_lPlayerBetAll.Length * 4;

            //size += 24;

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                for (int i = 0; i <= BumperCarDefine.AREA_COUNT; i++)
                    EncodeInteger(bw, m_lAllJettonScore[i]);

                EncodeInteger(bw, m_cbTableCardArray[0, 0]);

                // added by usc at 2014/03/18
                for (int i = 0; i < _Players.Count; i++)
                    for (int j = 0; j <= BumperCarDefine.AREA_COUNT; j++)
                        EncodeInteger(bw, m_lUserJettonScore[j, i]);

                //for (int i = 0; i < m_cbHorsesRanking.Length; i++)
                //    EncodeInteger(bw, m_cbHorsesRanking[i]);

                //for (int i = 0; i < _Players.Count; i++)
                //    EncodeInteger(bw, m_lPlayerWinning[i]);

                //for (int i = 0; i < _Players.Count; i++)
                //    EncodeInteger(bw, m_lPlayerReturnBet[i]);

                //for (int i = 0; i < HorseDefine.HORSES_ALL; i++)
                //{
                //    for (int k = 0; k < HorseDefine.COMPLETION_TIME; k++)
                //        EncodeInteger(bw, m_nHorsesSpeed[i, k]);
                //}

                //for (int i = 0; i < m_nWinCount.Length; i++)
                //    EncodeInteger(bw, m_nWinCount[i]);

                //for (int i = 0; i < m_nMultiple.Length; i++)
                //    EncodeInteger(bw, m_nMultiple[i]);

                //EncodeInteger(bw, m_nStreak);

                //for (int i = 0; i < m_lPlayerBetAll.Length; i++)
                //    EncodeInteger(bw, m_lPlayerBetAll[i]);


                //tagHistoryRecord historyRecord = new tagHistoryRecord();

                //if (m_GameRecords.Count > 0)
                //    historyRecord = m_GameRecords[m_GameRecords.Count - 1];

                //EncodeInteger( bw, historyRecord.nStreak );						//场次
                //EncodeInteger( bw, historyRecord.nRanking );						//排名
                //EncodeInteger( bw, historyRecord.nRiskCompensate );				//赔率
                //EncodeInteger( bw, historyRecord.nHours );							//小时
                //EncodeInteger( bw, historyRecord.nMinutes );						//分钟
                //EncodeInteger( bw, historyRecord.nSeconds );						//秒钟

            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            for (int i = 0; i <= BumperCarDefine.AREA_COUNT; i++)
                m_lAllJettonScore[i] = DecodeInteger(br);

            m_cbTableCardArray[0, 0] = DecodeInteger(br);

            // added by usc at 2014/03/18
            for (int i = 0; i < _Players.Count; i++)
                for (int j = 0; j <= BumperCarDefine.AREA_COUNT; j++)
                    m_lUserJettonScore[j, i] = DecodeInteger(br);

            //for (int i = 0; i < m_cbHorsesRanking.Length; i++)
            //    m_cbHorsesRanking[i] = DecodeInteger(br);

            //for (int i = 0; i < _Players.Count; i++)
            //    m_lPlayerWinning[i] = DecodeInteger(br);

            //for (int i = 0; i < _Players.Count; i++)
            //    m_lPlayerReturnBet[i] = DecodeInteger(br);

            //for (int i = 0; i < HorseDefine.HORSES_ALL; i++)
            //{
            //    for (int k = 0; k < HorseDefine.COMPLETION_TIME; k++)
            //        m_nHorsesSpeed[i, k] = DecodeInteger(br);
            //}

            //for (int i = 0; i < m_nWinCount.Length; i++)
            //    m_nWinCount[i] = DecodeInteger(br);

            //for (int i = 0; i < m_nMultiple.Length; i++)
            //    m_nMultiple[i] = DecodeInteger(br);

            //m_nStreak = DecodeInteger(br);

            //for (int i = 0; i < m_lPlayerBetAll.Length; i++)
            //    m_lPlayerBetAll[i] = DecodeInteger(br);


            //tagHistoryRecord historyRecord = new tagHistoryRecord();

            //historyRecord.nStreak = DecodeInteger(br);						//场次
            //historyRecord.nRanking = DecodeInteger(br);						//排名
            //historyRecord.nRiskCompensate = DecodeInteger(br);				//赔率
            //historyRecord.nHours = DecodeInteger(br);							//小时
            //historyRecord.nMinutes = DecodeInteger(br);						//分钟
            //historyRecord.nSeconds = DecodeInteger(br);						//秒钟

            //m_GameRecords.Add(historyRecord);
        }
    }

    public class BumperLogic
    {
        //获取牌型
        public static bool GetCardType(int cbCardData, int cbCardCount, int[] bcOutCadDataWin)
        {
            //ASSERT(1==cbCardCount);
            if (!(1 == cbCardCount))
                return false;

            Array.Clear(bcOutCadDataWin, 0, bcOutCadDataWin.Length);
            int bcData = cbCardData;

            if (1 == bcData || bcData == 2 || bcData == 1 + 8 || bcData == 2 + 8 || bcData == 1 + 2 * 8 || bcData == 2 + 2 * 8 || bcData == 1 + 3 * 8 || bcData == 2 + 3 * 8)
            {
                if (bcData % 2 == 1)
                    bcOutCadDataWin[0] = 40 - 1;
                else
                    bcOutCadDataWin[4] = 5 - 1;

            }
            else if (3 == bcData || bcData == 4 || bcData == 3 + 8 || bcData == 4 + 8 || bcData == 3 + 2 * 8 || bcData == 4 + 2 * 8 || bcData == 3 + 3 * 8 || bcData == 4 + 3 * 8)
            {
                if (bcData % 2 == 1)
                    bcOutCadDataWin[1] = 30 - 1;
                else
                    bcOutCadDataWin[5] = 5 - 1;



            }
            else if (5 == bcData || bcData == 6 || bcData == 5 + 8 || bcData == 6 + 8 || bcData == 5 + 2 * 8 || bcData == 6 + 2 * 8 || bcData == 5 + 3 * 8 || bcData == 6 + 3 * 8)
            {
                if (bcData % 2 == 1)
                    bcOutCadDataWin[2] = 20 - 1;
                else
                    bcOutCadDataWin[6] = 5 - 1;

            }
            else if (7 == bcData || bcData == 8 || bcData == 7 + 8 || bcData == 8 + 8 || bcData == 7 + 2 * 8 || bcData == 8 + 2 * 8 || bcData == 7 + 3 * 8 || bcData == 8 + 3 * 8)
            {
                if (bcData % 2 == 1)
                    bcOutCadDataWin[3] = 10 - 1;
                else
                    bcOutCadDataWin[7] = 5 - 1;

            }

            return true;
        }

        //混乱数组
        static Random _random = new Random();

        public static void ChaosArray(int[] nArrayOne, int nCountOne, int[] nArrayTwo, int nCountTwo)
        {
            //ASSERT( nCountOne == nCountTwo );
            if (nCountTwo != nCountOne)
                return;

            for (int i = 1; i < nCountOne; ++i)
            {
                int nTempIndex = _random.Next() % nCountOne;

                int nTempValueOne = nArrayOne[i];
                nArrayOne[i] = nArrayOne[nTempIndex];
                nArrayOne[nTempIndex] = nTempValueOne;

                int nTempValueTwo = nArrayTwo[i];
                nArrayTwo[i] = nArrayTwo[nTempIndex];
                nArrayTwo[nTempIndex] = nTempValueTwo;
            }
        }
    }
}
