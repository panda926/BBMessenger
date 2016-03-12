using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    // 牌型
    public enum E_CARD_TYPE
    {
        enCardType_Illegal = (int)-1,        // 非法
        enCardType_Big,                    // 大
        enCardType_Small,                  // 小
        enCardType_Single,                 // 单
        enCardType_Double,                 // 双		

        enCardType_NumberFour = (int)4,      // 3个骰子点数和为4
        enCardType_NumberFive,             // 3个骰子点数和为5
        enCardType_NumberSix,              // 3个骰子点数和为6
        enCardType_NumberSeven,            // 3个骰子点数和为7
        enCardType_NumberEight,            // 3个骰子点数和为8
        enCardType_NumberNine,             // 3个骰子点数和为9
        enCardType_NumberTen,              // 3个骰子点数和为10
        enCardType_NumberEleven,           // 3个骰子点数和为11
        enCardType_NumberTwelve,           // 3个骰子点数和为12
        enCardType_NumberThirteen,         // 3个骰子点数和为13
        enCardType_NumberFourteen,         // 3个骰子点数和为14
        enCardType_NumberFifteen,          // 3个骰子点数和为15
        enCardType_NumberSixteen,          // 3个骰子点数和为16
        enCardType_NumberSeventeen,        // 3个骰子点数和为17

        enCardType_SicboOne,               // 骰点1
        enCardType_SicboTwo,               // 骰点2
        enCardType_SicboThree,             // 骰点3
        enCardType_SicboFour,              // 骰点4
        enCardType_SicboFive,              // 骰点5
        enCardType_SicboSix,               // 骰点6

        enCardType_SicboOneAndTwo,         // 骰点1,2
        enCardType_SicboOneAndThree,       // 骰点1,3
        enCardType_SicboOneAndFour,        // 骰点1,4
        enCardType_SicboOneAndFive,        // 骰点1,5
        enCardType_SicboOneAndSix,         // 骰点1,6
        enCardType_SicboTwoAndThree,       // 骰点2,3
        enCardType_SicboTwoAndFour,        // 骰点2,4
        enCardType_SicboTwoAndFive,        // 骰点2,5
        enCardType_SicboTwoAndSix,         // 骰点2,6
        enCardType_SicboThreeAndFour,      // 骰点3,4
        enCardType_SicboThreeAndFive,      // 骰点3,5
        enCardType_SicboThreeAndSix,       // 骰点3,6
        enCardType_SicboFourAndFive,       // 骰点4,5
        enCardType_SicboFourAndSix,        // 骰点4,6
        enCardType_SicboFiveAndSix,        // 骰点5,6

        enCardType_SicboDoubleOne,         // 骰点2个1
        enCardType_SicboDoubleTwo,         // 骰点2个2
        enCardType_SicboDoubleThree,       // 骰点2个3
        enCardType_SicboDoubleFour,        // 骰点2个4
        enCardType_SicboDoubleFive,        // 骰点2个5
        enCardType_SicboDoubleSix,         // 骰点2个6

        enCardType_SicboThreeOne,          // 骰点3个1
        enCardType_SicboThreeTwo,          // 骰点3个2
        enCardType_SicboThreeThree,        // 骰点3个3
        enCardType_SicboThreeFour,         // 骰点3个4
        enCardType_SicboThreeFive,         // 骰点3个5
        enCardType_SicboThreeSix,          // 骰点3个6

        enCardType_SicboThreeSame          // 骰点3个相同点数
    };

    public class SicboDefine
    {
        public const int COUNT_AZIMUTH = 52;
        
        public const int MAX_COUNT_SICBO = 0x03;           // 一组3个骰子
        public const int MAX_SICBO_NUMBER = 0x06;            // 最大6点

        public const int TIME_PLACE_JETTON = 20;		   //下注时间
        public const int SICBOTYPE_COUNT = 5;   // 摇骰子类型总数
    }

    public class SicboInfo : TableInfo
    {
	    //庄家信息
	    //public UserInfo				m_wCurrentBanker;					            //当前庄家
	    //public List<UserInfo>       m_arApplyBanker = new List<UserInfo>();         // 申请庄家队列

        //个人下注
        public int[,] m_lUserScore = new int[SicboDefine.COUNT_AZIMUTH, GameDefine.GAME_PLAYER];			//个玩家每个区域已下的总注

        //玩家成绩
        public int[] m_lUserReturnScore = new int[GameDefine.GAME_PLAYER];		//返回下注
        public int[] m_lUserRevenue = new int[GameDefine.GAME_PLAYER];

        //扑克信息
        public int[] m_enCards = new int[SicboDefine.MAX_COUNT_SICBO];       // 骰子点数
        public int[] m_bWinner = new int[SicboDefine.COUNT_AZIMUTH];        // 输赢结果

        //状态变量
        public int m_iSameCount;                           // 三个相同开出的间隔
        public int m_BettingTimeLeave = 20;

        public SicboInfo()
        {
            _InfoType = InfoType.Sicbo;

            m_EndingTime = 20;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += SicboDefine.COUNT_AZIMUTH * GameDefine.GAME_PLAYER * 4;
            size += m_lUserReturnScore.Length * 4;
            size += m_lUserRevenue.Length * 4;

            size += m_enCards.Length * 4;
            size += m_bWinner.Length * 4;
            
            size += EncodeCount(m_BettingTimeLeave);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                for (int i = 0; i < SicboDefine.COUNT_AZIMUTH; i++)
                {
                    for (int k = 0; k < GameDefine.GAME_PLAYER; k++)
                        EncodeInteger( bw, this.m_lUserScore[i, k]);
                }

                for (int i = 0; i < m_lUserReturnScore.Length; i++)
                    EncodeInteger(bw, m_lUserReturnScore[i]);

                for (int i = 0; i < m_lUserRevenue.Length; i++)
                    EncodeInteger(bw, m_lUserRevenue[i]);

                for (int i = 0; i < m_enCards.Length; i++)
                    EncodeInteger(bw, m_enCards[i]);

                for (int i = 0; i < m_bWinner.Length; i++)
                    EncodeInteger(bw, m_bWinner[i]);

                EncodeInteger(bw, m_BettingTimeLeave);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            for (int i = 0; i < SicboDefine.COUNT_AZIMUTH; i++)
            {
                for (int k = 0; k < GameDefine.GAME_PLAYER; k++)
                    m_lUserScore[i, k] = DecodeInteger(br);
            }

            for (int i = 0; i < GameDefine.GAME_PLAYER; i++)
                m_lUserReturnScore[i] = DecodeInteger(br);

            for (int i = 0; i < GameDefine.GAME_PLAYER; i++)
                m_lUserRevenue[i] = DecodeInteger(br);

            for (int i = 0; i < SicboDefine.MAX_COUNT_SICBO; i++)
                m_enCards[i] = DecodeInteger(br);

            for (int i = 0; i < m_bWinner.Length; i++)
                m_bWinner[i] = DecodeInteger(br);

            m_BettingTimeLeave = DecodeInteger(br);

        }
    }
}
