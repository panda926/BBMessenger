using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class DzCardDefine
    {
        public const int GAME_PLAYER = 8;
        public const int FULL_COUNT = 52;									//全牌数目
        public const int MAX_CENTERCOUNT = 5;									//最大数目
        public const int MAX_COUNT = 2;									//最大数目

        public const int TIME_START_GAME = 15;
        public const int TIME_USER_ADD_SCORE =				20;								//放弃定时器
    }

    public class DzCardInfo : TableInfo
    {
	    //玩家变量
	    public int							m_wDUser;								//D玩家
	    public int							m_wCurrentUser;							//当前玩家
        public UserInfo[]                   m_Seatter = new UserInfo[DzCardDefine.GAME_PLAYER];

	//玩家状态
	    public bool[]						m_cbPlayStatus = new bool[DzCardDefine.GAME_PLAYER];			//游戏状态


	//加注信息
	    public int							m_lCellScore;							//单元下注
	    public int							m_lTurnLessScore;						//最小下注
	    public int							m_lAddLessScore;						//加最小注
	    public int							m_lTurnMaxScore;						//最大下注
	    public int							m_lBalanceScore;						//平衡下注
	    public int							m_wOperaCount;							//操作次数
	    public int							m_cbBalanceCount;						//平衡次数
	    public int[]						m_lTableScore = new int[DzCardDefine.GAME_PLAYER];				//桌面下注
	    public int[]						m_lTotalScore = new int[DzCardDefine.GAME_PLAYER];				//累计下注
	    public int[]						m_lUserMaxScore = new int[DzCardDefine.GAME_PLAYER];			//最大下注
	    public bool[]						m_cbShowHand = new bool[DzCardDefine.GAME_PLAYER];				//梭哈用户

	    //税收变量
	    //int							m_bUserTax[GAME_PLAYER];				//用户税收,若不用处理界面,可不用保存
	    //int							m_bLastTax[GAME_PLAYER];				//最后税收

	    //扑克信息
	    public int					m_cbSendCardCount;						//发牌数目
	    public int[]				m_cbCenterCardData = new int[DzCardDefine.MAX_CENTERCOUNT];	//中心扑克
        public int[][]              m_cbHandCardData = new int[DzCardDefine.GAME_PLAYER][];//[DzCardDefine.MAX_COUNT];//List<int[]>();//int[DzCardDefine.GAME_PLAYER,DzCardDefine.MAX_COUNT];//手上扑克

        public int cbTotalEnd = 0;
        public int[] m_GameScore = new int[DzCardDefine.GAME_PLAYER];

        public int[][] m_cbOverCardData = new int[DzCardDefine.GAME_PLAYER][];

        public DzCardInfo()
        {
            _InfoType = InfoType.DzCard;

            for (int i = 0; i < m_cbHandCardData.Length; i++)
                m_cbHandCardData[i] = new int[DzCardDefine.MAX_COUNT];

            for (int i = 0; i < m_cbOverCardData.Length; i++)
                m_cbOverCardData[i] = new int[DzCardDefine.MAX_CENTERCOUNT];

            m_ReadyTime = 10;
            m_EndingTime = 15;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(m_wDUser);
            size += EncodeCount(m_wCurrentUser);

            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
            {
                string userName = string.Empty;

                if (m_Seatter[i] != null)
                    userName = m_Seatter[i].Id;

                size += EncodeCount(userName);
            }

            for (int i = 0; i < m_cbPlayStatus.Length; i++)
                size += EncodeCount(Convert.ToInt32(m_cbPlayStatus[i]));

            size += EncodeCount(m_lCellScore);
            size += EncodeCount(m_lTurnLessScore);
            size += EncodeCount(m_lAddLessScore);
            size += EncodeCount(m_lTurnMaxScore);
            size += EncodeCount(m_lBalanceScore);
            size += EncodeCount(m_wOperaCount);
            size += EncodeCount(m_cbBalanceCount);

            for (int i = 0; i < m_lTableScore.Length; i++)
                size += EncodeCount(m_lTableScore[i]);

            for (int i = 0; i < m_lTotalScore.Length; i++)
                size += EncodeCount(m_lTotalScore[i]);

            for (int i = 0; i < m_lUserMaxScore.Length; i++)
                size += EncodeCount(m_lUserMaxScore[i]);

            for( int i = 0; i < m_cbShowHand.Length; i++ )
                size += EncodeCount(Convert.ToInt32( m_cbShowHand[i] ));

            size += EncodeCount(m_cbSendCardCount);

            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                for (int k = 0; k < DzCardDefine.MAX_COUNT; k++)
                    size += EncodeCount(m_cbHandCardData[i][k]);

            for (int i = 0; i < m_cbCenterCardData.Length; i++)
                size += EncodeCount(m_cbCenterCardData[i]);

            size += EncodeCount(cbTotalEnd);

            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                size += EncodeCount(m_GameScore[i]);

            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                for (int k = 0; k < DzCardDefine.MAX_CENTERCOUNT; k++)
                    size += EncodeCount(m_cbOverCardData[i][k]);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, m_wDUser);
                EncodeInteger(bw, m_wCurrentUser);

                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                {
                    string userName = string.Empty;

                    if (m_Seatter[i] != null)
                        userName = m_Seatter[i].Id;

                    EncodeString( bw, userName);
                }

                for (int i = 0; i < m_cbPlayStatus.Length; i++)
                    EncodeInteger(bw, Convert.ToInt32(m_cbPlayStatus[i]));

                EncodeInteger(bw, m_lCellScore);
                EncodeInteger(bw, m_lTurnLessScore);
                EncodeInteger(bw, m_lAddLessScore);
                EncodeInteger(bw, m_lTurnMaxScore);
                EncodeInteger(bw, m_lBalanceScore);
                EncodeInteger(bw, m_wOperaCount);
                EncodeInteger(bw, m_cbBalanceCount);

                for (int i = 0; i < m_lTableScore.Length; i++)
                    EncodeInteger(bw, m_lTableScore[i]);

                for (int i = 0; i < m_lTotalScore.Length; i++)
                    EncodeInteger(bw, m_lTotalScore[i]);

                for (int i = 0; i < m_lUserMaxScore.Length; i++)
                    EncodeInteger(bw, m_lUserMaxScore[i]);

                for (int i = 0; i < m_cbShowHand.Length; i++)
                    EncodeInteger(bw, Convert.ToInt32(m_cbShowHand[i]));

                EncodeInteger(bw, m_cbSendCardCount);

                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                    for (int k = 0; k < DzCardDefine.MAX_COUNT; k++)
                        EncodeInteger(bw, m_cbHandCardData[i][k]);

                for (int i = 0; i < m_cbCenterCardData.Length; i++)
                    EncodeInteger(bw, m_cbCenterCardData[i]);

                EncodeInteger(bw, cbTotalEnd);

                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                    EncodeInteger(bw, m_GameScore[i]);

                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                    for (int k = 0; k < DzCardDefine.MAX_CENTERCOUNT; k++)
                        EncodeInteger(bw, m_cbOverCardData[i][k]);

            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            try
            {
                base.FromBytes(br);

                m_wDUser = DecodeInteger(br);
                m_wCurrentUser = DecodeInteger(br);

                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                {
                    string userName = DecodeString(br);

                    foreach (UserInfo userInfo in _Players)
                    {
                        if (userInfo != null && userName == userInfo.Id)
                        {
                            m_Seatter[i] = userInfo;
                            break;
                        }
                    }
                }

                for (int i = 0; i < m_cbPlayStatus.Length; i++)
                    m_cbPlayStatus[i] = Convert.ToBoolean(DecodeInteger(br));

                m_lCellScore = DecodeInteger(br);
                m_lTurnLessScore = DecodeInteger(br);
                m_lAddLessScore = DecodeInteger(br);
                m_lTurnMaxScore = DecodeInteger(br);
                m_lBalanceScore = DecodeInteger(br);
                m_wOperaCount = DecodeInteger(br);
                m_cbBalanceCount = DecodeInteger(br);


                for (int i = 0; i < m_lTableScore.Length; i++)
                    m_lTableScore[i] = DecodeInteger(br);

                for (int i = 0; i < m_lTotalScore.Length; i++)
                    m_lTotalScore[i] = DecodeInteger(br);

                for (int i = 0; i < m_lUserMaxScore.Length; i++)
                    m_lUserMaxScore[i] = DecodeInteger(br);

                for (int i = 0; i < m_cbShowHand.Length; i++)
                    m_cbShowHand[i] = Convert.ToBoolean(DecodeInteger(br));

                m_cbSendCardCount = DecodeInteger(br);

                for( int i = 0; i < DzCardDefine.GAME_PLAYER; i++ )
                    for( int k = 0; k < DzCardDefine.MAX_COUNT; k++ )
                        m_cbHandCardData[i][k] = DecodeInteger(br);

                for (int i = 0; i < m_cbCenterCardData.Length; i++)
                    m_cbCenterCardData[i] = DecodeInteger(br);

                cbTotalEnd = DecodeInteger(br);

                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                    m_GameScore[i] = DecodeInteger(br);

                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                    for (int k = 0; k < DzCardDefine.MAX_CENTERCOUNT; k++)
                        m_cbOverCardData[i][k] = DecodeInteger(br);

            }
            catch { }
        }
    }
}
