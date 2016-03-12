using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class DiceDefine
    {
        public const int BET_TIME = 30;

        public const int FIRST_EVENT_SCORE = 2001;
    }

    public class DiceInfo : TableInfo
    {
	    //庄家信息
	    //public UserInfo				m_wCurrentBanker;					            //当前庄家
	    //public List<UserInfo>       m_arApplyBanker = new List<UserInfo>();         // 申请庄家队列

        //个人下注
        public int[,] m_lUserScore = new int[GameDefine.GAME_PLAYER, 4];			//个玩家每个区域已下的总注

        //added by usc at 2014/04/03
        public int[] m_lPlayerBetAll = new int[HorseDefine.AREA_ALL];				//所有下注

        //扑克信息
        public int[] m_enCards = new int[3];       // 骰子点数
        public int[] m_bWinner = new int[4];       // 输赢结果

        // added by usc at 2014/03/21
        public int m_StorageScore;
        public int m_StorageDeduct;

        public DiceInfo()
        {
            _InfoType = InfoType.Dice;
            
            m_EndingTime = 23;

            // added by usc at 2014/03/21
            m_StorageScore = DiceDefine.FIRST_EVENT_SCORE;
            m_StorageDeduct = 1;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += this._Players.Count * 16;
            //size += m_lUserScore.Length * 4;

            // added newly
            size += m_lPlayerBetAll.Length * 4;

            size += m_enCards.Length * 4;
            size += m_bWinner.Length * 4;

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                for (int i = 0; i < this._Players.Count; i++)
                {
                    for( int k = 0; k < 4; k++ )
                        EncodeInteger(bw, m_lUserScore[i,k]);
                }

                // added newly
                for (int i = 0; i < m_lPlayerBetAll.Length; i++)
                    EncodeInteger(bw, m_lPlayerBetAll[i]);

                for (int i = 0; i < m_enCards.Length; i++)
                    EncodeInteger(bw, m_enCards[i]);

                for (int i = 0; i < m_bWinner.Length; i++)
                    EncodeInteger(bw, m_bWinner[i]);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            for (int i = 0; i < this._Players.Count; i++)
            {
                for( int k = 0; k < 4; k++ )
                    m_lUserScore[i,k] = DecodeInteger(br);
            }

            for (int i = 0; i < m_lPlayerBetAll.Length; i++)
                m_lPlayerBetAll[i] = DecodeInteger(br);

            for (int i = 0; i < 3; i++)
                m_enCards[i] = DecodeInteger(br);

            for (int i = 0; i < 4; i++)
                m_bWinner[i] = DecodeInteger(br);
        }
    }
}
