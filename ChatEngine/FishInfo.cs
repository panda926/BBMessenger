using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class FishDefine 
    {
        public const int GAME_PLAYER = 3;									    //游戏人数
        public const int MAX_SHOW_TIME_COUNT = 4;
        public const int MAX_SHOW_GOLDEN_COUNT = 30;
        public const int MAX_SHOW_STEP_TIME = 288;
        public const int MAX_CHANGE_STEP_TIME = 1;

        public const int MAX_GATE_COUNT = 20;

        public const int MAX_FISH_OBJECT     =    2048;
        public const int MAX_FISH_IN_NET     =    4;
        public const int MAX_FISH_IN_NET_BOMB =   100;
        public const int MAX_FISH_SEND        =   50;
        public const int MAX_FISH_IN_POOL = 48;

        public const float M_PI = 3.14159265358979323846f;
        public const float M_PI_2 = 1.57079632679489661923f;
        public const float M_PI_4 = 0.785398163397448309616f;
        public const float M_1_PI = 0.318309886183790671538f;
        public const float M_2_PI = 0.636619772367581343076f;

        public const int INVALID_WORD = 0xFFFF;

        public const int CHANGE_SCENE_TIME_COUNT   = 300;                //切换场景
        public const int SEND_SCENE_TIME_COUNT    =  10;
        public const int FISH_DESTROY_TIME = 300;


        public const int SUB_C_BUY_BULLET        =      200;
        public const int SUB_C_FIRE              =      201;
        public const int SUB_C_CAST_NET          =      202;
        public const int SUB_C_CHANGE_CANNON     =      203;
        public const int SUB_C_ACCOUNT           =      204;
        public const int SUB_C_LASER_BEAN        =      205;
        public const int SUB_C_BOMB              =      206;
        public const int SUB_C_BONUS             =      207;
        public const int SUB_C_COOK              =      208;
        public const int SUB_C_TASK_PREPARED     =      209;
        public const int SUB_C_SEND_MESSAGE      =      210;
        public const int SUB_C_CANNON_RUN        =      211;
        public const int SUB_C_MATCH_START       =      212;
        public const int SUB_C_GATE_CTRL_SEND	 =	    213;
        public const int SUB_C_MATCH_OVER = 214;
        public const int SUB_C_END_GAME = 216;

        public const int EXP_CHANGE_TO_LEVEL = 3000;

        public const int SUB_S_CHANGE_SCENE = 100;
        public const int SUB_S_BUY_BULLET_SUCCESS = 101;
        public const int SUB_S_BUY_BULLET_FAILED = 102;
        public const int SUB_S_SEND_LINE_PATH_FISH = 103;
        public const int SUB_S_SEND_POINT_PATH_FISH = 104;
        public const int SUB_S_FIRE_SUCCESS = 105;
        public const int SUB_S_FIRE_FAILED = 106;
        public const int SUB_S_CAST_NET_SUCCESS = 107;
        public const int SUB_S_CAST_NET_FAILED = 108;
        public const int SUB_S_CHANGE_CANNON = 109;
        public const int SUB_S_ACCOUNT = 110;
        public const int SUB_S_SEND_GROUP_POINT_PATH_FISH = 111;
        public const int SUB_S_SEND_SPECIAL_POINT_PATH = 112;
        public const int SUB_S_TASK = 114;
        public const int SUB_S_LASER_BEAN_SUCCESS = 115;
        public const int SUB_S_BOMB_SUCCESS = 116;
        public const int SUB_S_LASER_BEAN_FAILED = 117;
        public const int SUB_S_BOMB_SUCCESS_FAILED = 118;
        public const int SUB_S_BONUS = 119;
        public const int SUB_S_COOK = 120;
        public const int SUB_S_BOMB_FISH = 121;
        public const int SUB_S_SEND_LINE_PATH_SMALL_BOTTOM_FISH = 122;
        public const int SUB_S_SEND_MESSAGE = 123;
        public const int SUB_S_CANNON_RUN = 124;
        public const int SUB_S_MATCH_START = 125;
        public const int SUB_S_MATCH_INDEX = 126;
        public const int SUB_S_MATCH_TIME_SEND = 127;
        public const int SUB_S_SMALL_FISH_GROUP = 128;
        public const int SUB_S_GATE_CTRL_SEND = 129;
        public const int SUB_S_MATCH_OVER = 130;

        public const int BASE_MUL_RATE = 5;										// 基准炮弹赔率


        public const int IDI_SCENE_START = 1;
        public const int IDI_SCENE_END = 2;
        public const int IDI_NORMAL_ADD_FISH_START = 3;
        public const int IDI_NORMAL_ADD_FISH_END = 4;
        public const int IDI_SCENE_ADD_FISH_START = 5;
        public const int IDI_SCENE_ADD_FISH_END = 6;
        public const int IDI_CHECK_FISH_DESTORY_START = 7;
        public const int IDI_CHECK_FISH_DESTORY_END = 8;
        public const int IDI_ADD_SCENE_FISH_SEND_COUNT = 9;
        public const int IDI_ADD_SCENE_FISH_SEND_SMALL_COUNT = 10;
        public const int IDI_ADD_BIG_FISH_11_ENABLE = 11;
        public const int IDI_ADD_BIG_FISH_12_ENABLE = 12;
        public const int IDI_ADD_BIG_FISH_13_ENABLE = 13;
        public const int IDI_ADD_BIG_FISH_14_ENABLE = 14;
        public const int IDI_ADD_SMALL_FISH_GROUP0 = 15;
        public const int IDI_ADD_SMALL_FISH_GROUP1 = 16;
        public const int IDI_ADD_SMALL_FISH_GROUP2 = 17;
        public const int IDI_GET_POOL_SCORE = 18;
        public const int TIME_SCENE_START = 1000;
        public const int TIME_SCENE_END = 1000;
        public const int TIME_NORMAL_ADD_FISH_START = 2000;
        public const int TIME_SCENE_ADD_FISH_START = 1000;
        public const int TIME_SCENE_ADD_FISH_END = 1000;
        public const int TIME_CHECK_FISH_DESTORY_START = 1000;
        public const int TIME_CHECK_FISH_DESTORY_END = 1000;
        public const int TIME_IDI_ADD_BIG_FISH_ENABLE = 60000;

        public const int MAX_CANNON_LEVEL = 11;

        public const int SCORE_TYPE_WIN			=	0x01;								//6603胜局积分
        public const int SCORE_TYPE_LOSE = 0x02;								//6603输局积分
        public const int SCORE_TYPE_DRAW = 0x03;								//6603和局积分

        public const int MAX_FISH_PATH        =   14;
        public const int MAX_FISH_STYLE = 15;


        public const int MAX_CANNON_STYLE = 7;
        public const int MAX_SCENE = 4;

        public const int MAX_SMALL_POINT_PATH   = 70;//208;
        public const int MAX_BIG_POINT_PATH = 30;
        public const int MAX_HUGE_POINT_PATH  =  20;//62;

        public const int MAX_LEFT_LINE_SPATH  =   3;
        public const int MAX_RIGHT_LINE_SPATH = 3;

        public enum enCannonType
        {
            CannonType_0 = 0,
            CannonType_1,
            CannonType_2,
            CannonType_3,
            CannonType_4,
            CannonType_5,
            CannonType_6,
            CannonTypeCount
        };

        public enum enSceneType
        {
            SceneType_0 = 0,
            SceneType_1,
            SceneType_2,
            SceneType_3,
            SceneTypeCount
        };

        public enum enFishType
        {
            FishType_0 = 0,
            FishType_1,
            FishType_2,
            FishType_3,
            FishType_4,
            FishType_5,
            FishType_6,
            FishType_7,
            FishType_8,
            FishType_9,
            FishType_10,
            FishType_11,
            FishType_12,
            FishType_13,
            FishType_14,
            FishType_15,
            FishTypeCount
        };

        public static enFishType WordToFishType(int wFishType)
        {
            switch (wFishType)
            {
                case 0:
                    return enFishType.FishType_0;
                case 1:
                    return enFishType.FishType_1;
                case 2:
                    return enFishType.FishType_2;
                case 3:
                    return enFishType.FishType_3;
                case 4:
                    return enFishType.FishType_4;
                case 5:
                    return enFishType.FishType_5;
                case 6:
                    return enFishType.FishType_6;
                case 7:
                    return enFishType.FishType_7;
                case 8:
                    return enFishType.FishType_8;
                case 9:
                    return enFishType.FishType_9;
                case 10:
                    return enFishType.FishType_10;
                case 11:
                    return enFishType.FishType_11;
                case 12:
                    return enFishType.FishType_12;
                case 13:
                    return enFishType.FishType_13;
                case 14:
                    return enFishType.FishType_14;
                case 15:
                    return enFishType.FishType_15;
                default:
                    return enFishType.FishTypeCount;
            }
        }

        public static enCannonType WordToCannonType(int wCannonType)
        {
            switch (wCannonType)
            {
                case 0:
                    return enCannonType.CannonType_0;
                case 1:
                    return enCannonType.CannonType_1;
                case 2:
                    return enCannonType.CannonType_2;
                case 3:
                    return enCannonType.CannonType_3;
                case 4:
                    return enCannonType.CannonType_4;
                case 5:
                    return enCannonType.CannonType_5;
                case 6:
                    return enCannonType.CannonType_6;

                default:
                    return enCannonType.CannonTypeCount;
            }
        }

        public static enSceneType ByteToSceneType(int cbScene)
        {
            switch (cbScene)
            {
                case 0: return enSceneType.SceneType_0;
                case 1: return enSceneType.SceneType_1;
                case 2: return enSceneType.SceneType_2;
                case 3: return enSceneType.SceneType_3;
                default: return enSceneType.SceneTypeCount;
            }
        }

        public static long time()
        {
            return DateTime.Now.Ticks / (10000 * 1000);
        }
    }

    public class FishInfo : TableInfo
    {
        public int m_cbScene;
        public long m_dwSceneStartTime; //场景时间计数
        public int m_cbSendSmallFishType;
        public int m_cbSendFishType; //场景鱼群类型，0 --> 平行线2排，1 --> 广口斜线2排
        public int m_dwSendFishCount; //场景鱼群计数
        public int m_dwSendFishSmallCount;
        public bool m_bIsSceneing;

        public int m_wAndroidLogicUserID;
        public int m_wAndroidLogicChairID; //机器人碰撞逻辑检测用户椅子ID
        public int m_wFishCount; //鱼记数
        public int m_wSceneSendFishCount;
        public int[] m_wCookFishCount = new int[FishDefine.GAME_PLAYER];
        public int[] m_nFireCount = new int[FishDefine.GAME_PLAYER];
        public Role_Net_Object[] m_RoleObjects = new Role_Net_Object[FishDefine.GAME_PLAYER];
        public Fish_Net_Object[] m_FishObjects = new Fish_Net_Object[FishDefine.MAX_FISH_OBJECT];
        public CTaskDate[] m_TaskObjects = new CTaskDate[FishDefine.GAME_PLAYER];
        public bool[] m_bTaskSended = new bool[FishDefine.GAME_PLAYER];

        public int[] m_cbShootFish14Count = new int[FishDefine.GAME_PLAYER];
        public int[] m_nCurGateCount = new int[FishDefine.GAME_PLAYER];

        public bool m_bIsBigDataBase;

        public byte m_cbSendFish11; //限制发送黑鲨鱼数量
        public byte m_cbSendFish12; //限制发送黄金鲨鱼数量
        public byte m_cbSendFish13; //限制发送炸弹鱼数量
        public byte m_cbSendFish14; //限制发送章鱼数量

        public bool m_bIsSmallFishGroupSendOk; //小鱼发送结束
        public DateTime m_ctCurTime = new DateTime(); //当前系统时间
        public SmallFishInf[,] m_wSmallFishGroupBottom = new SmallFishInf[6, 30]; //鱼群ID
        public SmallFishInf[,] m_wSmallFishGroupTop = new SmallFishInf[6, 30]; //鱼群ID

        //---------------------------配置文件变量-----------------------------------------
        public string m_szConfigFileName; //配置文件名
        public string m_szUserDataFileName; //用户数据文件名

        public int m_nAwardRate; //抽奖几率
        public int m_nAwardFishBase; //N倍以上鱼有几率触发抽奖

        public int[] m_nCannonLevel = new int[FishDefine.MAX_CANNON_LEVEL]; //炮台等级对应的炮台倍率

        public int m_nCellScore; //房间倍数
        public int m_nSendMessage; //房间是否可以发送消息

        public int m_nScoreMaxBuy; //一次最多购买渔币数
        public int m_nChangeRate; //就是ChangeRate%的几率将玩家输进来的分转换成返还值

        //public tagUserInf[] m_tagUserInf = new tagUserInf[FishDefine.GAME_PLAYER + 4]; //用户信息

        public byte m_cbSceneSound;
        public List<CMD_S_Send_Line_Path_Fish> m_LeftSceneFishPaths = new List<CMD_S_Send_Line_Path_Fish>();
        public List<CMD_S_Send_Line_Path_Fish> m_RightSceneFishPaths = new List<CMD_S_Send_Line_Path_Fish>();

        //组件变量
        //protected CGameLogic m_GameLogic = new CGameLogic(); //游戏逻辑
        //protected ITableFrame m_pITableFrame; //框架接口
        //protected tagGameServiceOption m_pGameServiceOption; //游戏配置
        //public int lCellScore = 1;
        //protected tagGameServiceAttrib m_pGameServiceAttrib; //游戏属性

        //属性变量
        protected readonly ushort m_wPlayerCount; //游戏人数

        public int m_dwRoomType;
        public int m_dwMatchScore;
        public int m_cbMatchNot;

        public static int[]	m_XXX = new int[100];
        public static int	m_nProbability;								//总体几率
        public static int[]	m_ProbabilityFish = new int[FishDefine.MAX_FISH_STYLE+20];		//鱼的优先级
        public static int[]	m_YYY = new int[100];

        public static int[] m_nFishType     = new int[]{  8,  8,  8,  8,    9,  9,  9,   10, 10, 10,   11, 11, 11,   12, 12, 12,   14,  0,0};
        public static int[] m_nFishWidth    = new int[]{256,256,256,256,  256,256,256,  278,278,278,  512,512,512,  512,512,512,  512,  0,0};
        public static int[] m_nFishHeight   = new int[]{180,180,180,180,  256,256,256,  160,160,160,  256,256,256,  256,256,256,  411,  0,0};
        public static int[] m_nFishBaseOff1 = new int[]{160,160,160,160,  128,128,128,  160,160,160,  128,128,128,  128,128,128,  128,  0,0};
        public static int[] m_nFishBaseOff2 = new int[]{160,160,160,160,  128,128,128,  160,160,160,  128,128,128,  128,128,128,  128,  0,0};

        public static long[] m_dwFishSendTime = new long[]{8000,3000,3000,3000,  4000,4000,4000,  4000,4000,4000,  4000,6000,6000,  6000,6000,6000,  5000,  0,0};

        public static int[,] m_dwSmallFishBase = new int[,]
        {
            {0   ,30  ,45  ,0   ,40  ,45  },
            {50  ,80  ,95  ,50  ,90  ,95  },
            {100 ,130 ,145 ,100 ,140 ,145 },
            {150 ,180 ,195 ,150 ,190 ,195 },
            {200 ,230 ,245 ,200 ,240 ,245 },
            {250 ,280 ,295 ,250 ,290 ,295 },
            {300 ,330 ,345 ,300 ,340 ,345 },
            {350 ,380 ,395 ,350 ,390 ,395 },
            {400 ,430 ,445 ,400 ,440 ,445 },
            {450 ,480 ,495 ,450 ,490 ,495 },
            {500 ,530 ,545 ,500 ,540 ,545 },
            {550 ,580 ,595 ,550 ,590 ,595 },
            {600 ,630 ,645 ,600 ,640 ,645 },
            {650 ,680 ,695 ,650 ,690 ,695 },
            {700 ,730 ,745 ,700 ,740 ,745 },
            {750 ,780 ,795 ,750 ,790 ,795 },
            {800 ,830 ,845 ,800 ,840 ,845 },
            {850 ,880 ,895 ,850 ,890 ,895 },
            {900 ,930 ,945 ,900 ,940 ,945 },
            {950 ,980 ,995 ,950 ,990 ,995 },
            {1000,1030,1045,1000,1040,1045},
            {1050,1080,1095,1050,1090,1095},
            {1100,1130,1145,1100,1140,1145},
            {1150,1180,1195,1150,1190,1195},
            {1200,1230,1245,1200,1240,1245},
            {1250,1280,1295,1250,1290,1295},
            {1300,1330,1345,1300,1340,1345},
            {1350,1380,1395,1350,1390,1395},
            {1400,1430,1445,1400,1440,1445},
            {1450,1480,1495,1450,1490,1495},
            {1500,1530,1545,1500,1540,1545},
            {1550,1580,1595,1550,1590,1595},
            {1600,1630,1645,1600,1640,1645},
            {1650,1680,1695,1650,1690,1695},
            {1700,1730,1745,1700,1740,1745},
        };

        //开始模式
        public FishInfo()
        {
            _InfoType = InfoType.Fish;


            //m_pITableFrame = null;
            //m_pGameServiceOption = null;
            //	m_pITableFrameControl=null;
            //	m_pITableFrameManager=null;

            //logic
            m_cbScene = 1;
            m_dwSceneStartTime = 0;
            m_wSceneSendFishCount = 0;

            m_wAndroidLogicUserID = 0;
            m_wAndroidLogicChairID = GameDefine.INVALID_CHAIR;

            m_cbSendSmallFishType = 2;
            m_cbSendFishType = 0;
            m_dwSendFishCount = 0;
            m_dwSendFishSmallCount = 0;
            m_bIsSceneing = false;

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                m_RoleObjects[i] = new Role_Net_Object();
                m_RoleObjects[i].wID = GameDefine.INVALID_CHAIR;
                m_RoleObjects[i].dwFishGold = 0;
                m_RoleObjects[i].wCannonType = 0;
                m_RoleObjects[i].wFireCount = 0;
                m_RoleObjects[i].dwMaxMulRate = 0;
                m_RoleObjects[i].dwMulRate = 0;
                m_RoleObjects[i].dwExpValue = 0;
            }

            for (int i = 0; i < FishDefine.MAX_FISH_OBJECT; i++)
            {
                m_FishObjects[i] = new Fish_Net_Object();
                m_FishObjects[i].wID = FishDefine.INVALID_WORD;
                m_FishObjects[i].wRoundID = 0;
                m_FishObjects[i].wType = 0;
                m_FishObjects[i].dwTime = 0;
            }

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                m_nFireCount[i] = 0;
                m_bTaskSended[i] = false;
                m_wCookFishCount[i] = 0;
                m_cbShootFish14Count[i] = 0;
                m_nCurGateCount[i] = 0;
            }

            for (int i = 0; i < m_TaskObjects.Length; i++)
                m_TaskObjects[i] = new CTaskDate();

            for (int i = 0; i < 6; i++)
            {
                for (int k = 0; k < 30; k++)
                {
                    m_wSmallFishGroupBottom[i,k] = new SmallFishInf(); //鱼群ID
                    m_wSmallFishGroupTop[i,k] = new SmallFishInf(); //鱼群ID
                }
            }

            LoadLinePaths();
            m_bIsBigDataBase = false;

            m_cbSendFish11 = 4;
            m_cbSendFish12 = 0;
            m_cbSendFish13 = 0;
            m_cbSendFish14 = 0;

            m_nCellScore = 1;

            m_nProbability = 1000;
            //ZeroMemory(m_ProbabilityFish, sizeof(m_ProbabilityFish));

            m_cbSceneSound = 0;

            //return;
        }

        bool LoadLinePaths()
        {
            FishInfo fishInfo = this;// (FishInfo)_GameTable._TableInfo;

            string ostr;
            //CMD_S_Send_Line_Path_Fish SendLinePath = new CMD_S_Send_Line_Path_Fish();

            for (int i = 0; i < FishDefine.MAX_LEFT_LINE_SPATH; i++)
            {
                ostr = string.Format("FishServer\\left\\{0}.spth", i);

                CMD_S_Send_Line_Path_Fish SendLinePath = new CMD_S_Send_Line_Path_Fish();
                SendLinePath.FromFile(ostr);

                //std::ifstream ifs(ostr.str(), std::ios::binary);
                //if (!ifs.is_open())
                //{
                //    return true;
                //}

                //ifs.read((char*)&SendLinePath, sizeof(CMD_S_Send_Line_Path_Fish));
                fishInfo.m_LeftSceneFishPaths.Add(SendLinePath);
            }

            for (int i = 0; i < FishDefine.MAX_RIGHT_LINE_SPATH; i++)
            {
                ostr = string.Format("FishServer\\right\\{0}.spth", i);

                //std::ifstream ifs(ostr.str(), std::ios::binary);
                //if (!ifs.is_open())
                //{
                //    MessageBox(0,"LoadLinePaths","",0);
                //    return true;
                //}

                //ifs.read((char*)&SendLinePath, sizeof(CMD_S_Send_Line_Path_Fish));

                CMD_S_Send_Line_Path_Fish SendLinePath = new CMD_S_Send_Line_Path_Fish();
                SendLinePath.FromFile(ostr);

                fishInfo.m_RightSceneFishPaths.Add(SendLinePath);
            }

            return true;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(this.m_cbScene);
            size += EncodeCount(this.m_nCellScore);
            size += EncodeCount(this.m_nScoreMaxBuy);

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                size += m_RoleObjects[i].GetSize();
            }
            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, this.m_cbScene);
                EncodeInteger(bw, this.m_nCellScore);
                EncodeInteger(bw, this.m_nScoreMaxBuy);

                for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
                    m_RoleObjects[i].GetBytes(bw);

            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            this.m_cbScene = DecodeInteger(br);
            this.m_nCellScore = DecodeInteger(br);
            this.m_nScoreMaxBuy = DecodeInteger(br);

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
                m_RoleObjects[i].FromBytes(br);
        }
    }

    public class FishSendInfo : BaseInfo
    {
        public int _SendType;
        public BaseInfo _SendInfo;

        public FishSendInfo()
        {
            _InfoType = InfoType.FishSend;
        }

        public FishSendInfo(int sendType, BaseInfo sendInfo)
        {
            _InfoType = InfoType.FishSend;

            _SendType = sendType;
            _SendInfo = sendInfo;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(_SendType);
            size += _SendInfo.GetSize();

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, _SendType);

                _SendInfo.GetBytes(bw);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            _SendType = DecodeInteger(br);

            switch (_SendType)
            {
                case FishDefine.SUB_C_BUY_BULLET:
                    _SendInfo = new CMD_C_Buy_Bullet();
                    break;
                case FishDefine.SUB_S_BUY_BULLET_SUCCESS:
                    _SendInfo = new CMD_S_Buy_Bullet_Success();
                    break;
                case FishDefine.SUB_C_CHANGE_CANNON:
                    _SendInfo = new CMD_C_Change_Cannon();
                    break;
                case FishDefine.SUB_S_CHANGE_CANNON:
                    _SendInfo = new CMD_S_Change_Cannon();
                    break;
                case FishDefine.SUB_C_FIRE:
                    _SendInfo = new CMD_C_Fire();
                    break;
                case FishDefine.SUB_S_FIRE_SUCCESS:
                    _SendInfo = new CMD_S_Fire_Success();
                    break;
                case FishDefine.SUB_C_CAST_NET:
                    _SendInfo = new CMD_C_Cast_Net();
                    break;
                case FishDefine.SUB_S_CAST_NET_SUCCESS:
                    _SendInfo = new CMD_S_Cast_Net_Success();
                    break;
                case FishDefine.SUB_S_SEND_POINT_PATH_FISH:
                    _SendInfo = new CMD_S_Send_Point_Path_Fish();
                    break;
                case FishDefine.SUB_S_SEND_GROUP_POINT_PATH_FISH:
                    _SendInfo = new CMD_S_Send_Group_Point_Path_Fish();
                    break;
                case FishDefine.SUB_S_CHANGE_SCENE:
                    _SendInfo = new CMD_S_Change_Scene();
                    break;
                case FishDefine.SUB_S_SMALL_FISH_GROUP:
                    _SendInfo = new CMD_S_Small_Fish_Group();
                    break;
                case FishDefine.SUB_S_SEND_LINE_PATH_FISH:
                    _SendInfo = new CMD_S_Send_Line_Path_Fish();
                    break;
                case FishDefine.SUB_S_SEND_LINE_PATH_SMALL_BOTTOM_FISH:
                    _SendInfo = new CMD_S_Send_Line_Path_Fish();
                    break;
                case FishDefine.SUB_S_BOMB_FISH:
                    _SendInfo = new CMD_S_Bomb_Fish();
                    break;
                case FishDefine.SUB_C_BOMB:
                    _SendInfo = new CMD_C_Bomb();
                    break;
                case FishDefine.SUB_S_BOMB_SUCCESS:
                    _SendInfo = new CMD_S_Bomb_Success();
                    break;
                case FishDefine.SUB_C_ACCOUNT:
                    _SendInfo = new CMD_C_Account();
                    break;
                case FishDefine.SUB_S_ACCOUNT:
                    _SendInfo = new CMD_S_Account();
                    break;
            }

            if (_SendInfo == null)
                _SendInfo = null;

            _SendInfo.FromBytes(br);
        }

    }

    public class CTaskDate
    {
        public enum Type
        {
            TYPE_NULL = -1,
            TYPE_BONUS = 0,
            TYPE_COOK,
            TYPE_BEAN,
            TYPE_BOMB,
            TYPE_COUNT
        }

        public enum State
        {
            STATE_NULL = -1,
            STATE_PREPARE1 = 0,
            STATE_PREPARE2,
            STATE_RUNNING,
            STATE_COMPLETE,
            STATE_COUNT
        }

        public Type m_enType;
        public State m_enState;

        public int m_nBonus;
        public int m_nDuration;

        public int[] m_nStartWheel = new int[3];
        public int[] m_nEndWheel = new int[3];

        public int m_nFishType;
        public int m_nFishCount;

        public CTaskDate()
        {
            this.m_enType = Type.TYPE_NULL;
            this.m_enState = State.STATE_NULL;
            this.m_nDuration = 0;
            this.m_nBonus = 0;
            this.m_nFishType = 0;
            this.m_nFishCount = 0;
        }
    }

    public class Role_Net_Object : BaseInfo
    {
        public  int  wID;
        public int dwFishGold;
	    public int dwMatchGold;
        public int wCannonType;  
        public int wFireCount;
	    public int dwMulRate;
	    public int dwMaxMulRate;
	    public int dwExpValue;
	    public int dwIndex;
        public string userId = string.Empty;

        override public int GetSize()
        {
            int size = 0;

            size += EncodeCount( wID ); 
            size += EncodeCount( dwFishGold );
	        size += EncodeCount( dwMatchGold );
            size += EncodeCount( wCannonType );  
            size += EncodeCount( wFireCount );
	        size += EncodeCount( dwMulRate );
	        size += EncodeCount( dwMaxMulRate );
	        size += EncodeCount( dwExpValue );
	        size += EncodeCount( dwIndex );
            size += EncodeCount( userId );

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                EncodeInteger( bw, wID );
                EncodeInteger( bw, dwFishGold );
	            EncodeInteger( bw, dwMatchGold );
                EncodeInteger( bw, wCannonType );  
                EncodeInteger( bw, wFireCount );
	            EncodeInteger( bw, dwMulRate );
	            EncodeInteger( bw, dwMaxMulRate );
	            EncodeInteger( bw, dwExpValue );
	            EncodeInteger( bw, dwIndex );
                EncodeString( bw, userId );
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            wID = DecodeInteger(br);
            dwFishGold = DecodeInteger(br);
	        dwMatchGold = DecodeInteger(br);
            wCannonType = DecodeInteger(br);  
            wFireCount = DecodeInteger(br);
	        dwMulRate = DecodeInteger(br);
	        dwMaxMulRate = DecodeInteger(br);
	        dwExpValue = DecodeInteger(br);
	        dwIndex = DecodeInteger(br);
            userId = DecodeString(br);
        }
    };

    public class Fish_Net_Object : BaseInfo
    {
        public int wID;
        public int wRoundID;
        public int wType;
        public long dwTime;

        public override int GetSize()
        {
            int size = EncodeCount(wID);
            size += EncodeCount(wRoundID);
            size += EncodeCount(wType);
            size += EncodeCount(dwTime);

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, wID);
            EncodeInteger(bw, wRoundID);
            EncodeInteger(bw, wType);
            EncodeLong(bw, dwTime);
        }

        public override void FromBytes(BinaryReader br)
        {
            wID = DecodeInteger(br);
            wRoundID = DecodeInteger(br);
            wType = DecodeInteger(br);
            dwTime = DecodeLong(br);
        }
    };


    public class GateCtrlInf
    {
        public int nFishScore;
        public int nGetScore;
    };

    public class Fish_With_Line_Path : BaseInfo 
    {
        public Fish_Net_Object FishNetObject = new Fish_Net_Object();
        public Line_Path LinePath = new Line_Path();

        public override int GetSize()
        {
            int size = FishNetObject.GetSize();
            size += LinePath.GetSize();

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            FishNetObject.GetBytes(bw);
            LinePath.GetBytes(bw);
        }

        public override void FromBytes(BinaryReader br)
        {
            FishNetObject.FromBytes(br);
            LinePath.FromBytes(br);
        }
    };

    public class Line_Path : BaseInfo
    {
        public SPoint spStart;
        public SPoint spEnd;
        public int dwTime;

        public override int GetSize()
        {
            int size = EncodeCount(dwTime);
            size += 8;
            size += 8;
            
            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, dwTime);
            EncodeInteger(bw, spStart.x);
            EncodeInteger(bw, spStart.y);
            EncodeInteger(bw, spEnd.x);
            EncodeInteger(bw, spEnd.y);
        }

        public override void FromBytes(BinaryReader br)
        {
            dwTime = DecodeInteger(br);
            spStart.x = DecodeInteger(br);
            spStart.y = DecodeInteger(br);
            spEnd.x = DecodeInteger(br);
            spEnd.y = DecodeInteger(br);
        }
    };

    public struct SPoint 
    {
        public int x;
        public int y;
    };

    public class Fish_With_Point_Path : BaseInfo
    {
        public Fish_Net_Object FishNetObject = new Fish_Net_Object();
        public int wPath;
        public int dwTime;

        public override int GetSize()
        {
            int size = FishNetObject.GetSize();
            size += EncodeCount(wPath);
            size += EncodeCount(dwTime);

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            FishNetObject.GetBytes(bw);

            EncodeInteger(bw, wPath);
            EncodeInteger(bw, dwTime);
        }

        public override void FromBytes(BinaryReader br)
        {
            FishNetObject.FromBytes(br);

            wPath = DecodeInteger(br);
            dwTime = DecodeInteger(br);
        }
    };

    public class Fish_With_Special_Point_Path
    {
        public int wPath;
        public float fDelay;
        public int dwTime;
        public Fish_Net_Object FishNetObject;
    }

    public class SmallFishInf : BaseInfo
    {
        public int wFishID;
        public int wRoundID;

        override public int GetSize()
        {
            return 8;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                EncodeInteger(bw, wFishID);
                EncodeInteger(bw, wRoundID);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            wFishID = DecodeInteger(br);
            wRoundID = DecodeInteger(br);
        }
    }

    public class CMD_S_StatusFree
    {
        public int cbScene;
        public int cbSceneSound;
        public int cbMatchNot;
        public string cbVer;
        public string cbURL;
        public string cbIPList;
        public int nScoreMaxBuy;
        public int lCellScore;
        public int dwRoomType;
        public int dwMatchScore;
        public Role_Net_Object[] RoleObjects = new Role_Net_Object[FishDefine.GAME_PLAYER];
    };

    public class CMD_C_Bomb : BaseInfo
    {
        public int cbCount;
        public Fish_Net_Object[] FishNetObjects = new Fish_Net_Object[FishDefine.MAX_FISH_IN_NET_BOMB];

        public CMD_C_Bomb()
        {
            for (int i = 0; i < FishNetObjects.Length; i++)
            {
                FishNetObjects[i] = new Fish_Net_Object();
            }
        }

        public override int GetSize()
        {
            int size = EncodeCount(cbCount);

            for (int i = 0; i < cbCount; i++)
            {
                size += FishNetObjects[i].GetSize();
            }

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, cbCount);

            for (int i = 0; i < cbCount; i++)
            {
                FishNetObjects[i].GetBytes(bw);
            }
        }

        public override void FromBytes(BinaryReader br)
        {
            cbCount = DecodeInteger(br);

            for (int i = 0; i < cbCount; i++)
            {
                FishNetObjects[i].FromBytes(br);
            }
        }

    };

    public class CMD_C_Laser_Bean : BaseInfo
    {
        public int cbCount;
        public double fRote;
        public Fish_Net_Object[] FishNetObjects = new Fish_Net_Object[FishDefine.MAX_FISH_IN_NET];
    };

    public class CMD_C_Bonus : BaseInfo
    {
        public int nBonus;
    }

    public class CMD_C_Cook : BaseInfo
    {
        public int nBonus;
    }

    public class CMD_S_Fire_Success : BaseInfo
    {
        public int cbIsBack;
        public int wChairID;
        public int wAndroidLogicChairID;				//机器人碰撞逻辑检测用户椅子ID
        public int nFireCount;
        public double fRote;
        public int dwMulRate;
        public int xStart;
        public int yStart;

        public int cbIsMatchEnd;
        public int dwMatchScore;
        public int dwIndex;

        public override int GetSize()
        {
            int size = EncodeCount(cbIsBack);
            size += EncodeCount(wChairID);
            size += EncodeCount(wAndroidLogicChairID);
            size += EncodeCount(nFireCount);
            size += 8;// double
            size += EncodeCount(dwMulRate);
            size += EncodeCount(xStart);
            size += EncodeCount(yStart);
            size += EncodeCount( cbIsMatchEnd );
            size += EncodeCount( dwMatchScore );
            size += EncodeCount(dwIndex );

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, cbIsBack);
            EncodeInteger(bw, wChairID);
            EncodeInteger(bw, wAndroidLogicChairID);
            EncodeInteger(bw, nFireCount);
            EncodeLong(bw, (long)(fRote * 100000));
            EncodeInteger(bw, dwMulRate);
            EncodeInteger(bw, xStart);
            EncodeInteger(bw, yStart);
            EncodeInteger(bw, cbIsMatchEnd);
            EncodeInteger(bw, dwMatchScore);
            EncodeInteger(bw, dwIndex);
        }

        public override void FromBytes(BinaryReader br)
        {
            cbIsBack = DecodeInteger(br);
            wChairID = DecodeInteger(br);
            wAndroidLogicChairID = DecodeInteger(br);
            nFireCount = DecodeInteger(br);
            fRote = (double)DecodeLong(br) / 100000;
            dwMulRate = DecodeInteger(br);
            xStart = DecodeInteger(br);
            yStart = DecodeInteger(br);
            cbIsMatchEnd = DecodeInteger(br);
            dwMatchScore = DecodeInteger(br);
            dwIndex = DecodeInteger(br);
        }
    }

    public class CMD_S_Task
    {
        public int wChairID;
        public int cbLevel;
        public int nTask;
        public int nBonus;
        public int nDuration;
        public int[] nStartWheel = new int[3];
        public int[] nEndWheel = new int[3];
        public int cbFishType;
        public int cbFishCount;
    }

    public class CMD_S_Cast_Net_Success : BaseInfo
    {
        public int cbLevelUp;
        public int cbCount;
        public int wChairID;
        public int dwExpValue;
        public int dwLevel;
        public Fish_Net_Object[] FishNetObjects = new Fish_Net_Object[FishDefine.MAX_FISH_IN_NET];

        public CMD_S_Cast_Net_Success()
        {
            for (int i = 0; i < FishNetObjects.Length; i++)
                FishNetObjects[i] = new Fish_Net_Object();
        }

        public override int GetSize()
        {
            int size = EncodeCount(cbLevelUp);
            size += EncodeCount(cbCount);
            size += EncodeCount(wChairID);
            size += EncodeCount(dwExpValue);
            size += EncodeCount(dwLevel);

            for (int i = 0; i < cbCount; i++)
                size += FishNetObjects[i].GetSize();

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, cbLevelUp);
            EncodeInteger(bw, cbCount);
            EncodeInteger(bw, wChairID);
            EncodeInteger(bw, dwExpValue);
            EncodeInteger(bw, dwLevel);

            for( int i = 0; i < cbCount; i++ )
                FishNetObjects[i].GetBytes(bw);
        }

        public override void FromBytes(BinaryReader br)
        {
            cbLevelUp = DecodeInteger(br);
            cbCount = DecodeInteger(br);
            wChairID = DecodeInteger(br);
            dwExpValue = DecodeInteger(br);
            dwLevel = DecodeInteger(br);

            for (int i = 0; i < cbCount; i++)
                FishNetObjects[i].FromBytes(br);
        }
    }

    public class CMD_S_Bomb_Success : BaseInfo
    {
        public int cbLevelUp;
        public int cbCount;
        public int wChairID;
        public Fish_Net_Object[] FishNetObjects = new Fish_Net_Object[FishDefine.MAX_FISH_IN_NET_BOMB];

        public CMD_S_Bomb_Success()
        {
            for (int i = 0; i < FishNetObjects.Length; i++)
                FishNetObjects[i] = new Fish_Net_Object();
        }

        public override int GetSize()
        {
            int size = EncodeCount(cbLevelUp);
            size += EncodeCount(cbCount);
            size += EncodeCount(wChairID);

            for (int i = 0; i < cbCount; i++)
                size += FishNetObjects[i].GetSize();

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, cbLevelUp);
            EncodeInteger(bw, cbCount);
            EncodeInteger(bw, wChairID);

            for (int i = 0; i < cbCount; i++)
                FishNetObjects[i].GetBytes(bw);
        }

        public override void FromBytes(BinaryReader br)
        {
            cbLevelUp = DecodeInteger(br);
            cbCount = DecodeInteger(br);
            wChairID = DecodeInteger(br);

            for (int i = 0; i < cbCount; i++)
                FishNetObjects[i].FromBytes(br);
        }
    }

    public class CMD_S_Bonus : BaseInfo
    {
        public int wChairID;
        public int nBonus;
    }

    public class CMD_C_Cast_Net : BaseInfo
    {
        public int wChairID;
        public int cbCount;
        public Fish_Net_Object[] FishNetObjects = new Fish_Net_Object[FishDefine.MAX_FISH_IN_NET];

        public CMD_C_Cast_Net()
        {
            for (int i = 0; i < FishNetObjects.Length; i++)
                FishNetObjects[i] = new Fish_Net_Object();
        }

        public override int GetSize()
        {
            int size = EncodeCount(wChairID);
            size += EncodeCount(cbCount);

            for (int i = 0; i < cbCount; i++)
                size += FishNetObjects[i].GetSize();

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, wChairID);
            EncodeInteger(bw, cbCount);

            for( int i = 0; i < cbCount; i++ )
                FishNetObjects[i].GetBytes(bw);
        }

        public override void FromBytes(BinaryReader br)
        {
            wChairID = DecodeInteger(br);
            cbCount = DecodeInteger(br);

            for (int i = 0; i < cbCount; i++)
                FishNetObjects[i].FromBytes(br);
        }
    };

    public class CMD_S_Send_Group_Point_Path_Fish : BaseInfo
    {
        public int wPath;
        public int dwTime;
        public int cbPahtType;
        public int cbCount;
        public Fish_Net_Object[] FishNetObject = new Fish_Net_Object[FishDefine.MAX_FISH_SEND];

        public CMD_S_Send_Group_Point_Path_Fish()
        {
            for (int i = 0; i < FishNetObject.Length; i++)
                FishNetObject[i] = new Fish_Net_Object();
        }

        public override int GetSize()
        {
            int size = EncodeCount(wPath);
            size += EncodeCount(dwTime);
            size += EncodeCount(cbPahtType);
            size += EncodeCount(cbCount);

            for (int i = 0; i < cbCount; i++)
                size += FishNetObject[i].GetSize();

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, wPath);
            EncodeInteger(bw, dwTime);
            EncodeInteger(bw, cbPahtType);
            EncodeInteger(bw, cbCount);

            for (int i = 0; i < cbCount; i++)
                FishNetObject[i].GetBytes(bw);
        }

        public override void FromBytes(BinaryReader br)
        {
            wPath = DecodeInteger(br);
            dwTime = DecodeInteger(br);
            cbPahtType = DecodeInteger(br);
            cbCount = DecodeInteger(br);

            for (int i = 0; i < cbCount; i++)
                FishNetObject[i].FromBytes(br);
        }
    };

    public class CMD_S_Send_Line_Path_Fish : BaseInfo
    {
        public int cbCount;
        public int cbForward;
        public Fish_With_Line_Path[] FishPaths = new Fish_With_Line_Path[FishDefine.MAX_FISH_SEND];

        public CMD_S_Send_Line_Path_Fish()
        {
            for (int i = 0; i < FishPaths.Length; i++)
                FishPaths[i] = new Fish_With_Line_Path();
        }

        public void FromFile(string fileName)
        {
        }

        public override int GetSize()
        {
            int size = EncodeCount(cbCount);
            size += EncodeCount(cbForward);

            for (int i = 0; i < cbCount; i++)
                size += FishPaths[i].GetSize();

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, cbCount);
            EncodeInteger(bw, cbForward);

            for (int i = 0; i < cbCount; i++)
                FishPaths[i].GetBytes(bw);
        }

        public override void FromBytes(BinaryReader br)
        {
            cbCount = DecodeInteger(br);
            cbForward = DecodeInteger(br);

            for (int i = 0; i < cbCount; i++)
                FishPaths[i].FromBytes(br);
        }
    };


    public class CMD_S_Send_Point_Path_Fish : BaseInfo
    {
        public int cbCount;
        public Fish_With_Point_Path[] FishPaths = new Fish_With_Point_Path[FishDefine.MAX_FISH_SEND];

        public CMD_S_Send_Point_Path_Fish()
        {
            cbCount = 0;

            for (int i = 0; i < FishPaths.Length; i++)
                FishPaths[i] = new Fish_With_Point_Path();
        }

        public override int GetSize()
        {
            int size = EncodeCount(cbCount);

            for (int i = 0; i < cbCount; i++)
                size += FishPaths[i].GetSize();

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw,cbCount);

            for (int i = 0; i < cbCount; i++)
                FishPaths[i].GetBytes(bw);
        }

        public override void FromBytes(BinaryReader br)
        {
            cbCount = DecodeInteger(br);

            for (int i = 0; i < cbCount; i++)
                FishPaths[i].FromBytes(br);
        }
    };

    public class CMD_S_Small_Fish_Group : BaseInfo
    {
        public int nTime;
        //public int wChair;
        public int wFishType;
        public SmallFishInf[,] wFishGroupBottom = new SmallFishInf[3, 30];
        public SmallFishInf[,] wFishGroupTop = new SmallFishInf[3, 30];

        public CMD_S_Small_Fish_Group()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 30; k++)
                {
                    wFishGroupBottom[i, k] = new SmallFishInf();
                    wFishGroupTop[i, k] = new SmallFishInf();
                }
            }
        }

        override public int GetSize()
        {
            int size = 0;

            size += EncodeCount( nTime );
            size += EncodeCount(wFishType);

            size += 90 * wFishGroupBottom[0, 0].GetSize();
            size += 90 * wFishGroupTop[0,0].GetSize();

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                EncodeInteger( bw, nTime );
                EncodeInteger( bw, wFishType );

                for (int i = 0; i < 3; i++)
                {
                    for (int k = 0; k < 30; k++)
                    {
                        wFishGroupBottom[i,k].GetBytes(bw);
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int k = 0; k < 30; k++)
                    {
                        wFishGroupTop[i, k].GetBytes(bw);
                    }
                }

            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            nTime = DecodeInteger(br);
            wFishType = DecodeInteger(br);

            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 30; k++)
                {
                    wFishGroupBottom[i, k] = new SmallFishInf();
                    wFishGroupBottom[i, k].FromBytes(br);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 30; k++)
                {
                    wFishGroupTop[i, k] = new SmallFishInf();
                    wFishGroupTop[i, k].FromBytes(br);
                }
            }
        }
    }

    public class CMD_C_Change_Cannon : BaseInfo
    {
        public int wStyle;
        public int dwMulRate;

        public override int GetSize()
        {
            int size = EncodeCount(wStyle);
            size += EncodeCount(dwMulRate);

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, wStyle);
            EncodeInteger(bw, dwMulRate);
        }

        public override void FromBytes(BinaryReader br)
        {
            wStyle = DecodeInteger(br);
            dwMulRate = DecodeInteger(br);
        }
    }

    public class CMD_S_Send_Special_Point_Path : BaseInfo
    {
        public int cbCount;
        public Fish_With_Special_Point_Path[] SpecialPointPath = new Fish_With_Special_Point_Path[FishDefine.MAX_FISH_SEND];
    }


    public class CMD_C_Task_Prepared : BaseInfo
    {
        public int nTask;
    }

    public class CMD_C_Account : BaseInfo
    {
        public int wChairID;

        public override int GetSize()
        {
            return EncodeCount(wChairID);
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, wChairID);
        }

        public override void FromBytes(BinaryReader br)
        {
            wChairID = DecodeInteger(br);
        }
    }

    public class CMD_C_Gate_Ctrl_Send : BaseInfo
    {
        public int cbFirst;
        public int wChair;
        public int nGateCount;
    }

    public class CMD_C_Buy_Bullet : BaseInfo
    {
        public int dwCount;

        public override int GetSize()
        {
            return EncodeCount(dwCount);
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, dwCount);
        }

        public override void FromBytes(BinaryReader br)
        {
            dwCount = DecodeInteger(br);
        }
    }

    public class CMD_C_Match_Start : BaseInfo
    {
        public int wChair;
        public int dwScore;
        public int dwMatchScore;
    }

    public class CMD_C_Send_Message : BaseInfo
    {
	    public int wChair;
	    public int nLen;
	    public string cbData;
    };

    public class CMD_S_Buy_Bullet_Success : BaseInfo
    {
        public int wChairID;
        public int dwCount;

        public override int GetSize()
        {
            int size = EncodeCount(wChairID);
            size += EncodeCount(dwCount);

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, wChairID);
            EncodeInteger(bw, dwCount);
        }

        public override void FromBytes(BinaryReader br)
        {
            wChairID = DecodeInteger(br);
            dwCount = DecodeInteger(br);
        }
    };

    public class CMD_S_Buy_Bullet_Failed : BaseInfo
    {
        public int wChairID;
        string szReason;
    };

    public class CMD_S_Laser_Bean_Success : BaseInfo
    {
        public int cbCount;
        public int wChairID;
        public double fRote;
        public Fish_Net_Object[] FishNetObjects = new Fish_Net_Object[FishDefine.MAX_FISH_IN_NET];
    }

    public class CMD_S_Cook : BaseInfo
    {
        public int wChairID;
        public int nBonus;
        public int cbSucess;
    }

    public class CMD_S_Gate_Ctrl_Send : BaseInfo
    {
        public int cbFirst;
        public int wChair;
        public int nGateCount;
        public int nCurGateCount;
        public int nFishScoreBase;

        public GateCtrlInf[] tagGateCtrlInf = new GateCtrlInf[FishDefine.MAX_GATE_COUNT];
    }

    public class CMD_S_Account : BaseInfo
    {
        public int wChairID;

        public override int GetSize()
        {
            return EncodeCount(wChairID);
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, wChairID);
        }

        public override void FromBytes(BinaryReader br)
        {
            wChairID = DecodeInteger(br);
        }

    }

    public class CMD_S_Bomb_Fish : BaseInfo
    {
        public int wChairID;
        public int dwMulRate;
        public long lBigStock;
        public long lSmallStock;

        public override int GetSize()
        {
            int size = EncodeCount(wChairID);
            size += EncodeCount(dwMulRate);
            size += 8;
            size += 8;

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, wChairID);
            EncodeInteger(bw, dwMulRate);
            EncodeLong(bw, lBigStock);
            EncodeLong(bw, lSmallStock);
        }

        public override void FromBytes(BinaryReader br)
        {
            wChairID = DecodeInteger(br);
            dwMulRate = DecodeInteger(br);
            lBigStock = DecodeLong(br);
            lSmallStock = DecodeLong(br);
        }
    }

    public class CMD_S_Cast_Net_Failed
    {
        public ushort wChairID;
        public string szReason = new string(new char[32]);
    }

    public class CMD_S_Change_Cannon : BaseInfo
    {
        public int wChairID;
        public int wStyle;
        public int dwMulRate;
        public int dwMaxMulRate;
        public int dwExpValue;
        public int dwLevel;

        public override int GetSize()
        {
            int size = EncodeCount(wChairID);
            size += EncodeCount(wStyle);
            size += EncodeCount(dwMulRate);
            size += EncodeCount(dwMaxMulRate);
            size += EncodeCount(dwExpValue);
            size += EncodeCount(dwLevel);

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, wChairID);
            EncodeInteger(bw, wStyle);
            EncodeInteger(bw, dwMulRate);
            EncodeInteger(bw, dwMaxMulRate);
            EncodeInteger(bw, dwExpValue);
            EncodeInteger(bw, dwLevel);
        }

        public override void FromBytes(BinaryReader br)
        {
            wChairID = DecodeInteger(br);
            wStyle = DecodeInteger(br);
            dwMulRate = DecodeInteger(br);
            dwMaxMulRate = DecodeInteger(br);
            dwExpValue = DecodeInteger(br);
            dwLevel = DecodeInteger(br);
        }
    }

    public class CMD_S_Change_Scene : BaseInfo
    {
        public int cbScene;
        public int cbSceneSound;

        public override int GetSize()
        {
            int size = EncodeCount(cbScene);
            size += EncodeCount(cbSceneSound);

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, cbScene);
            EncodeInteger(bw, cbSceneSound);
        }

        public override void FromBytes(BinaryReader br)
        {
            cbScene = DecodeInteger(br);
            cbSceneSound = DecodeInteger(br);
        }
    }

    public class CMD_S_Connon_Run : BaseInfo
    {
        public int wChair;
        public double fRote;
    }

    public class CMD_S_Match_Index
    {
        public ushort wChair;
        public int dwScore;
        public int dwMatchScore;
        public int dwIndex;
    }

    public class CMD_S_Match_Over : BaseInfo
    {
        public int wChair;
        public long lMatchScore;
        public long lMatchScoreCheck;
    }

    public class CMD_S_Match_Start : BaseInfo
    {
        public int wChair;
        public int dwScore;
        public int dwMatchScore;
    }

    public class CMD_S_Match_Time_Send
    {
        public int nHour;
        public int nMinute;
        public int nSecond;
        public int[] dwMatchScore = new int[FishDefine.GAME_PLAYER];
    }

    public class CMD_S_Send_Message : BaseInfo
    {
        public int wChair;
        public int nLen;
        public string cbData;
    }

    public class CMD_C_Connon_Run : BaseInfo
    {
        public int wChair;
        public double fRote;
    }

    public class CMD_C_Match_Over : BaseInfo
    {
        public int wChair;
        public int lMatchScore;
        public int lMatchScoreCheck;
    }

    public class CMD_C_Fire : BaseInfo
    {
        public int cbIsBack;
        public double fRote;
        public int dwMulRate;
        public int xStart;
        public int yStart;

        public override int GetSize()
        {
            int size = EncodeCount(cbIsBack);
            size += 8; // double
            size += EncodeCount(dwMulRate);
            size += EncodeCount(xStart);
            size += EncodeCount(yStart);

            return size;
        }

        public override void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, cbIsBack);
            EncodeLong(bw, (long)(fRote * 100000));
            EncodeInteger(bw, dwMulRate);
            EncodeInteger(bw, xStart);
            EncodeInteger(bw, yStart);
        }

        public override void FromBytes(BinaryReader br)
        {
            cbIsBack = DecodeInteger(br);
            fRote = (double)DecodeLong(br) / 100000;
            dwMulRate = DecodeInteger(br);
            xStart = DecodeInteger(br);
            yStart = DecodeInteger(br);
        }
    }

    //6603积分信息
    public class tagScoreInfo
    {
        public int cbType;								//6603积分类型
        public int lScore;								//6603用户分数
        public int lGrade;								//6603用户成绩
        public int lRevenue;							//6603游戏税收
    };

    public class CMD_S_Fire_Failed : BaseInfo
    {
        public int wChairID;
        public string szReason;
    };

    public class FishGameLogic
    {
        public static int FishGoldByStyle(int nFishStyle, int wID)
        {
            switch (nFishStyle)
            {
                case 0:
                    return 1;
                case 1:
                    return 2;
                case 2:
                    return 3;
                case 3:
                    return 4;
                case 4:
                    return 5;
                case 5:
                    return 7;
                case 6:
                    return 9;
                case 7:
                    return 10;
                case 8:
                    return 12;
                case 9:
                    return 15;
                case 10:
                    return 20;
                case 11:
                    return 30 + wID % 21;
                case 12:
                    return 40 + wID % 81;
                case 13:
                    return 0;
                case 14:
                    return 300;
                case 15:
                    return 300;
                default:
                    return 0;
            }
        }
    }


}
