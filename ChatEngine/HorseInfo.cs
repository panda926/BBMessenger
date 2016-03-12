using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

// temp1

namespace ChatEngine
{
    public class HorseDefine
    {
        //public const int KIND_ID	=					114;								//游戏 I D
        //public const int GAME_PLAYER	=				100;								//游戏人数
        //public const int GAME_NAME		=			"百人跑马";				//游戏名字

        //public const int VERSION_SERVER		=		PROCESS_VERSION(6,0,3)				//程序版本
        //public const int VERSION_CLIENT		=		PROCESS_VERSION(6,0,3)				//程序版本
        
        //状态定义
        //public const int GS_FREE			=			GAME_STATUS_FREE;				//等待开始
        //public const int	GS_BET			=			GAME_STATUS_PLAY;				//下注状态
        //public const int	GS_BET_END		=			GAME_STATUS_PLAY+1;				//下注结束状态
        //public const int	GS_HORSES		=			GAME_STATUS_PLAY+2;				//结束状态

        //游戏消息
        public const int IDM_PLAYER_BET		=		(1000);				//加住信息
        public const int IDM_ADMIN_COMMDN =         (999);					//控制信息

        public const int SERVER_LEN			=		32;								//房间长度

        //历史记录
        public const int MAX_SCORE_HISTORY	=		20;								//历史个数

        //马匹索引
        public const int	HORSES_ONE		=			0;								//1号马
        public const int	HORSES_TWO		=			1;								//2号马
        public const int	HORSES_THREE	=			2;								//3号马
        public const int	HORSES_FOUR		=			3;								//4号马
        public const int	HORSES_FIVE		=			4;								//5号马
        public const int	HORSES_SIX		=			5;								//6号马
        public const int HORSES_ALL			=		6;								//合计索引

        //马匹位置
        public const int	HORSES_X_POS		=		180;									//起始X位置
        public const int	HORSES_ONE_Y_POS	=		315; 								//1号马
        public const int	HORSES_TWO_Y_POS	=		285; 								//2号马
        public const int	HORSES_THREE_Y_POS	=		255 ;								//3号马
        public const int	HORSES_FOUR_Y_POS	=		220;									//4号马
        public const int	HORSES_FIVE_Y_POS	=		190;									//5号马
        public const int	HORSES_SIX_Y_POS	=		160;									//6号马

        //下注区域索引
        //public const int AREA_1_6				=	0;								//1_6 索引
        //public const int AREA_1_5				=	1;								//1_5 索引
        //public const int AREA_1_4				=	2;								//1_4 索引
        //public const int AREA_1_3				=	3;								//1_3 索引
        //public const int AREA_1_2				=	4;								//1_2 索引
        //public const int AREA_2_6				=	5;								//2_6 索引
        //public const int AREA_2_5				=	6;								//2_5 索引
        //public const int AREA_2_4				=	7;								//2_4 索引
        //public const int AREA_2_3				=	8;								//2_3 索引
        //public const int AREA_3_6				=	9;								//3_6 索引
        //public const int AREA_3_5				=	10;								//3_5 索引
        //public const int AREA_3_4				=	11;								//3_4 索引
        //public const int AREA_4_6				=	12;								//4_6 索引
        //public const int AREA_4_5				=	13;								//4_5 索引
        //public const int AREA_5_6				=	14;								//5_6 索引
        //public const int AREA_ALL				=	15;								//合计索引
        public const int AREA_1 = 0;								//1_6 索引
        public const int AREA_2 = 1;								//2_6 索引
        public const int AREA_3 = 2;								//3_6 索引
        public const int AREA_4 = 3;								//4_6 索引
        public const int AREA_5 = 4;								//5_6 索引
        public const int AREA_6 = 5;								//5_6 索引
        public const int AREA_ALL = 6;								//合计索引


        //跑马名次
        public const int RANKING_FIRST			=	0;								//第一名
        public const int RANKING_SECOND			=	1;								//第二名
        public const int RANKING_THIRD			=	2;								//第三名
        public const int RANKING_FOURTH			=	3;								//第四名
        public const int RANKING_FIFTH			=	4;								//第五名
        public const int RANKING_SIXTH			=	5;								//第六名
        public const int RANKING_NULL			=	6;								//无名次
            
        //下注失败类型
        public const int FAIL_TYPE_OVERTOP		=	1;								//超出限制
        public const int FAIL_TYPE_TIME_OVER	=		0;								//超时

        //跑道长度
        public const int HAND_LENGTH			=		170;							//头
        public const int TAIL_LENGTH			=		185	;						//尾

        //预计完成时间
        public const int COMPLETION_TIME        =       25;								//25秒

        //速度跨步
        public const int STEP_SPEED				=	950;//(COMPLETION_TIME*10)			//

        //中间背景重复次数
        public const int BACK_COUNT				=	25;

        //基础速度
        public const int BASIC_SPEED			=		20;

        //高速度
        public const int HIGH_SPEED				=	300;

        //低速度
        public const int LOW_SPEED				=	300;

        //加速频率
        public const int FREQUENCY				=	5;

        //加速度
        public const int ACCELERATION			=	3;

        //马匹名字
        public const int HORSES_NAME_LENGTH		=	32;

        // added by usc at 2013/07/17
        public const int C_CA_CANCELS = 3;		//取消

        //控制
        //服务器控制返回
        public const int  S_CR_FAILURE = 0;		//失败
        public const int  S_CR_UPDATE_SUCCES = 1;		//更新成功
        public const int  S_CR_SET_SUCCESS = 2;		//设置成功
        public const int  S_CR_CANCEL_SUCCESS = 3;		//取消成功

        //added at 2014/01/04
        public const int AREA_COUNT = 6;									//区域数目

        // added by usc at 2014/02/27
        public const int BET_TIME = 25;

        // added by usc at 2014/03/21
        public const int FIRST_EVENT_SCORE = 2001;
    }

    public class HorseInfo : TableInfo
    {
        //空闲时间
        public const int IDI_FREE		=			1;									//空闲时间
        const int TIME_FREE		=			1;									//空闲时间

        //下注时间
        public const int IDI_BET_START			=	2;									//开始下注
        const int TIME_BET_START = HorseDefine.BET_TIME;									//下注时间

        //下注结束时间
        public const int IDI_BET_END			=		3;									//开始下注
        const int TIME_BET_END			=	5;									//下注结束时间

        //跑马时间
        public const int IDI_HORSES_START	=		4;									//跑马开始
        // modified by usc at 2014/03/04
        const int TIME_HORSES_START	=		30;									//跑马时间

        //跑马时间
        public const int IDI_HORSES_END		=		5	;								//跑马结束(强制客户端结束)
        // modified by usc at 2014/03/04
        const int TIME_HORSES_END		=		10;				//跑马时间

        //结束时间
        const int IDI_STORAGE_INTERVAL	=	6;									//结束时间

	    //游戏变量
	    int[]			        		m_lRobotScoreRange = new int[2];				//最大范围
	    int		        				m_lRobotBankGetScore;				//提款数额
	    int					        	m_lRobotBankGetScoreBanker;			//提款数额 (庄家)
	    int								m_nRobotBankStorageMul;				//存款倍数

	    public int								m_nStreak;								//场次
	    public int								m_nDay = DateTime.Now.Day;									//天数
	    public int							    m_dwGameTime;							//游戏时间
	    public int						        m_nFreeTime = TIME_FREE;							//空闲时间
	    public int								m_nBetTime = TIME_BET_START;								//下注时间
	    public int								m_nBetEndTime = TIME_BET_END;							//下注结束时间
	    public int								m_nHorsesTime = TIME_HORSES_START;							//跑马时间
	    public int								m_nHorsesEndTime = TIME_HORSES_END;						//跑马结束时间

	    //控制变量
	    public int[]							m_nCLMultiple = new int[HorseDefine.AREA_ALL];				//区域倍数
	    public int							m_cbCLTimes;							//控制次数
	    public int							m_cbCLArea = 255;								//控制区域
	    public bool							m_bControl;								//是否控制

	    //游戏结果
	    //public int							m_cbGameResults;						//跑马名次(2马)

	    //库存
	    public int						m_StorageScore;							//房间启动每桌子的库存数值，读取失败按 0 设置
	    int								m_nStorageNowNode;						//当前库存点
	    int								m_nStorageIntervalTime;					//库存更换间隔时间
	    int								m_nStorageCount;						//库存数目
	    int[]						m_StorageArray = new int[30];			//房间启动每桌子的库存数值，读取失败按 0 设置
	    public int								m_StorageDeduct;						//每局游戏结束后扣除的库存比例，读取失败按 1.00 设置

	    //分数
	    public int								m_nBetPlayerCount;						//下注人数
	    public int[,]						m_lPlayerBet = new int[GameDefine.GAME_PLAYER, HorseDefine.AREA_ALL];	//玩家下注
	    public int[,]						m_lPlayerBetWin = new int[GameDefine.GAME_PLAYER, HorseDefine.AREA_ALL];	//玩家区域输赢
	    public int[]						m_lPlayerBetAll = new int[HorseDefine.AREA_ALL];				//所有下注
	    public int[]						m_lPlayerWinning = new int[GameDefine.GAME_PLAYER];			//玩家输赢
	    public int[]						m_lPlayerReturnBet = new int[GameDefine.GAME_PLAYER];		//玩家返回下注
	    public int[]						m_lPlayerRevenue = new int[GameDefine.GAME_PLAYER];			//玩家税收

	    //区域倍数
	    bool							m_bMultipleControl;						//倍数控制
	    public int[]							m_nMultiple = new int[HorseDefine.AREA_ALL];					//区域倍数

	    //马匹变量
	    public int[,]					m_nHorsesSpeed = new int[HorseDefine.HORSES_ALL, HorseDefine.COMPLETION_TIME];	//每匹马的每秒速度
	    public int[]					m_cbHorsesRanking = new int[HorseDefine.RANKING_NULL];					//马匹名次
	    public string[]						m_szHorsesName = new string[HorseDefine.HORSES_ALL];	//马匹名字

	    //限制变量
	    int						m_lAreaLimitScore;						//区域总限制
	    int						m_lUserLimitScore;						//个人区域限制

	    //房间信息
	    string							m_szConfigFileName = string.Empty;			//配置文件
	    string							m_szGameRoomName = string.Empty;			//房间名称

	    //游戏记录
	    public List<tagHistoryRecord> m_GameRecords = new List<tagHistoryRecord>();							//游戏记录
	    public int[]							m_nWinCount = new int[HorseDefine.HORSES_ALL];				//全天赢的场次

	    //机器人控制
	    int								m_nMaxChipRobot;						//最大数目 (下注机器人)
	    int								m_nChipRobotCount;						//人数统计 (下注机器人)
	    int						m_lRobotAreaLimit;						//机器人区域限制
	    int[]						m_lRobotAreaScore = new int[HorseDefine.AREA_ALL];			//机器人区域下注

	    //组件变量
        //CGameLogic						m_GameLogic;							//游戏逻辑
        //ITableFrame						* m_pITableFrame;						//框架接口
        //tagGameServiceOption			* m_pGameServiceOption;					//配置参数
        //tagGameServiceAttrib *			m_pGameServiceAttrib;					//游戏属性

        public HorseInfo()
        {
            _InfoType = InfoType.Horse;

	        //游戏变量
	        m_nStreak = 0;
	        m_dwGameTime = 0;
	        m_nFreeTime = TIME_FREE;
	        m_nBetTime = TIME_BET_START;
	        m_nBetEndTime = TIME_BET_END;
	        m_nHorsesTime = TIME_HORSES_START;
	        m_nHorsesEndTime = TIME_HORSES_END;
	        
	        m_nDay = DateTime.Now.Day;									//天

	        //控制变量
	        m_cbCLTimes = 0;
	        m_bControl = false;
	        m_cbCLArea = 255;
	        for (int i = 0 ; i < HorseDefine.AREA_ALL; ++i)
		        m_nCLMultiple[i] = -1;


	        //限制变量
	        m_lAreaLimitScore = 0;
	        m_lUserLimitScore = 0;

	        //库存
	        m_StorageScore = HorseDefine.FIRST_EVENT_SCORE;
	        m_StorageDeduct = 1;

	        //游戏结果
	        //m_cbGameResults = HorseDefine.AREA_ALL;

	        //分数
	        m_nBetPlayerCount = 0;
            //ZeroMemory(m_lPlayerBet,sizeof(m_lPlayerBet));
            //ZeroMemory(m_lPlayerBetWin,sizeof(m_lPlayerBetWin));
            //ZeroMemory(m_lPlayerBetAll,sizeof(m_lPlayerBetAll));
            //ZeroMemory(m_lPlayerWinning,sizeof(m_lPlayerWinning));
            //ZeroMemory(m_lPlayerReturnBet,sizeof(m_lPlayerReturnBet));
            //ZeroMemory(m_lPlayerRevenue,sizeof(m_lPlayerRevenue));

	        //区域倍数
	        m_bMultipleControl = false;
	        for ( int i = 0; i < m_nMultiple.Length; ++i)
		        m_nMultiple[i] = 1;

	        //马匹信息
	        //ZeroMemory(m_nHorsesSpeed,sizeof(m_nHorsesSpeed));
	        for ( int i = 0; i < m_cbHorsesRanking.Length; ++i)
		        m_cbHorsesRanking[i] = HorseDefine.HORSES_ALL;

	        //马匹名字
	        m_szHorsesName[HorseDefine.HORSES_ONE] = "一号马";
	        m_szHorsesName[HorseDefine.HORSES_TWO] = "二号马";
	        m_szHorsesName[HorseDefine.HORSES_THREE] = "三号马";
	        m_szHorsesName[HorseDefine.HORSES_FOUR] = "四号马";
	        m_szHorsesName[HorseDefine.HORSES_FIVE] = "五号马";
	        m_szHorsesName[HorseDefine.HORSES_SIX] = "六号马";

	        //房间信息
            //ZeroMemory(m_szConfigFileName,sizeof(m_szConfigFileName));
            //ZeroMemory(m_szGameRoomName,sizeof(m_szGameRoomName));

	        //游戏记录
	        //m_GameRecords.Clear();
	        //ZeroMemory(m_nWinCount,sizeof(m_nWinCount));

	        //机器人控制
	        m_nChipRobotCount = 0;
	        //ZeroMemory(m_lRobotAreaScore, sizeof(m_lRobotAreaScore));
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += m_cbHorsesRanking.Length * 4;
            size += _Players.Count * 4;
            //size += _Players.Count * 4;

            size += HorseDefine.HORSES_ALL * HorseDefine.COMPLETION_TIME * 4;

            size += m_nWinCount.Length * 4;
            size += m_nMultiple.Length * 4;

            size += 4; // m_nStreak;
            size += m_lPlayerBetAll.Length * 4;

            // added by usc at 2014/03/18
            size += _Players.Count * HorseDefine.AREA_COUNT * 4;

            size += 24;

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                for (int i = 0; i < m_cbHorsesRanking.Length; i++)
                    EncodeInteger(bw, m_cbHorsesRanking[i]);

                for (int i = 0; i < _Players.Count; i++)
                    EncodeInteger(bw, m_lPlayerWinning[i]);

                //for (int i = 0; i < _Players.Count; i++)
                //    EncodeInteger(bw, m_lPlayerReturnBet[i]);

                for (int i = 0; i < HorseDefine.HORSES_ALL; i++)
                {
                    for (int k = 0; k < HorseDefine.COMPLETION_TIME; k++)
                        EncodeInteger(bw, m_nHorsesSpeed[i, k]);
                }

                for (int i = 0; i < m_nWinCount.Length; i++)
                    EncodeInteger(bw, m_nWinCount[i]);

                for (int i = 0; i < m_nMultiple.Length; i++)
                    EncodeInteger(bw, m_nMultiple[i]);

                EncodeInteger(bw, m_nStreak);

                for (int i = 0; i < m_lPlayerBetAll.Length; i++)
                    EncodeInteger(bw, m_lPlayerBetAll[i]);

                // added by usc at 2014/03/18
                for (int i = 0; i < _Players.Count; i++)
                    for (int j = 0; j < HorseDefine.AREA_COUNT; j++)
                        EncodeInteger(bw, m_lPlayerBet[i, j]);

                tagHistoryRecord historyRecord = new tagHistoryRecord();

                if (m_GameRecords.Count > 0)
                    historyRecord = m_GameRecords[m_GameRecords.Count - 1];

                            EncodeInteger( bw, historyRecord.nStreak );						//场次
                            EncodeInteger( bw, historyRecord.nRanking );						//排名
                            EncodeInteger( bw, historyRecord.nRiskCompensate );				//赔率
                            EncodeInteger( bw, historyRecord.nHours );							//小时
                            EncodeInteger( bw, historyRecord.nMinutes );						//分钟
                            EncodeInteger( bw, historyRecord.nSeconds );						//秒钟

            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            for (int i = 0; i < m_cbHorsesRanking.Length; i++)
                m_cbHorsesRanking[i] = DecodeInteger(br);

            for (int i = 0; i < _Players.Count; i++)
                m_lPlayerWinning[i] = DecodeInteger(br);

            //for (int i = 0; i < _Players.Count; i++)
            //    m_lPlayerReturnBet[i] = DecodeInteger(br);

            for (int i = 0; i < HorseDefine.HORSES_ALL; i++)
            {
                for (int k = 0; k < HorseDefine.COMPLETION_TIME; k++)
                    m_nHorsesSpeed[i, k] = DecodeInteger(br);
            }

            for (int i = 0; i < m_nWinCount.Length; i++)
                m_nWinCount[i] = DecodeInteger(br);

            for (int i = 0; i < m_nMultiple.Length; i++)
                m_nMultiple[i] = DecodeInteger(br);

            m_nStreak = DecodeInteger(br);

            for (int i = 0; i < m_lPlayerBetAll.Length; i++)
                m_lPlayerBetAll[i] = DecodeInteger(br);

            // added by usc at 2014/03/18
            for (int i = 0; i < _Players.Count; i++)
                for (int j = 0; j < HorseDefine.AREA_COUNT; j++)
                    m_lPlayerBet[i, j] = DecodeInteger(br);

            tagHistoryRecord historyRecord = new tagHistoryRecord();

                    historyRecord.nStreak = DecodeInteger(br);						//场次
                    historyRecord.nRanking = DecodeInteger(br);						//排名
                    historyRecord.nRiskCompensate = DecodeInteger(br);				//赔率
                    historyRecord.nHours = DecodeInteger(br);							//小时
                    historyRecord.nMinutes = DecodeInteger(br);						//分钟
                    historyRecord.nSeconds = DecodeInteger(br);						//秒钟

            m_GameRecords.Add(historyRecord);
        }
    }

    //记录信息
    public class tagHistoryRecord
    {
        public int nStreak;						//场次
        public int nRanking;						//排名
        public int nRiskCompensate;				//赔率
        public int nHours;							//小时
        public int nMinutes;						//分钟
        public int nSeconds;						//秒钟
    };


}
