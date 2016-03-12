using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.Net.Sockets;
using ChatEngine;

namespace ChatServer
{
    class SicboTable : GameTable
    {
        public SicboTable( string tableId )
        {
            _TableInfo = new SicboInfo();
            _TableInfo._TableId = tableId;

            _Rounds.Add(new ReadyRound());
            _Rounds.Add(new SicboJoinRound());
            _Rounds.Add(new SicboBettingRound());
            _Rounds.Add(new SicboEndRound());

            for (int i = 0; i < _Rounds.Count; i++)
                _Rounds[i]._GameTable = this;
        }

        public override bool PlayerOutTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            int seaterIndex = GetPlayerIndex(userInfo);

            if (seaterIndex >= 0)
            {
                if (_Rounds[_TableInfo._RoundIndex] is SicboBettingRound )
                {
                    SicboInfo sicboInfo = (SicboInfo)_TableInfo;

                    int lAllScore = 0;

                    for (int i = 0; i < SicboDefine.COUNT_AZIMUTH; i++)
                        lAllScore += sicboInfo.m_lUserScore[i, seaterIndex];

                    if (lAllScore > 0)
                    {
                        sicboInfo.m_lUserWinScore[seaterIndex] = -lAllScore;
                        Cash.GetInstance().ProcessGameCash(seaterIndex, _GameInfo, _TableInfo);

                        sicboInfo.m_lUserWinScore[seaterIndex] = 0;
                    }

                    for (int i = seaterIndex; i < _TableInfo._Players.Count; i++)
                    {
                        for (int k = 0; k < SicboDefine.COUNT_AZIMUTH; k++)
                            sicboInfo.m_lUserScore[k, i] = sicboInfo.m_lUserScore[k, i + 1];
                    }
                }
            }

            return base.PlayerOutTable(baseInfo, userInfo);
        }
    }

    public class SicboJoinRound : GameRound
    {
        public override bool CanStart()
        {
            return true;
        }

        public override void Start()
        {
            _TimerId = TimerID.Join;
            _GameTable.AddGameTimer(TimerID.Join, 10);

            base.Start();
        }

        public override bool CanEnd()
        {
            if (_IsLeaveTime == false)
                return false;

            return true;
        }
    }

    public class SicboBettingRound : StartRound
    {
        Random _random = new Random();

        public override void InitTableData(TableInfo tableInfo)
        {
            SicboInfo sicboInfo = (SicboInfo)tableInfo;

	        //变量定义
            //tableInfo.m_wCurrentBanker = null;	             // 当前庄家

            Array.Clear(sicboInfo.m_bWinner, 0, sicboInfo.m_bWinner.Length);        // 输赢结果
            Array.Clear(sicboInfo.m_lUserScore, 0, sicboInfo.m_lUserScore.Length);
            Array.Clear(sicboInfo.m_lUserWinScore, 0, sicboInfo.m_lUserWinScore.Length);
            Array.Clear(sicboInfo.m_lUserReturnScore, 0, sicboInfo.m_lUserReturnScore.Length);
            Array.Clear(sicboInfo.m_lUserRevenue, 0, sicboInfo.m_lUserRevenue.Length);

            _TimerId = TimerID.Betting;
            _GameTable.AddGameTimer(TimerID.Betting, sicboInfo.m_BettingTimeLeave);
        }

        public override void AutoAction(UserInfo userInfo)
        {
            int bettingCount = _random.Next() % 8;

            for (int i = 0; i < bettingCount; i++)
            {
                int delay = _random.Next() % 10 + 1;

                _GameTable.AddAutoTimer(userInfo, delay);
            }
        }

        public override void NotifyGameTimer(GameTimer gameTimer)
        {
            if (gameTimer.timerId != TimerID.Custom || gameTimer.autoInfo == null)
                return;

            BettingInfo bettingInfo = new BettingInfo();

            bettingInfo._Area = _random.Next() % SicboDefine.COUNT_AZIMUTH;

            int[] bettingScores = new int[] { 5, 10, 50, 100, 500, 1000, 5000 };
            bettingInfo._Score = bettingScores[_random.Next() % bettingScores.Length ];

            bettingInfo._UserIndex = _GameTable.GetPlayerIndex(gameTimer.autoInfo);

            Action(NotifyType.Request_Betting, bettingInfo, gameTimer.autoInfo);
        }

        public override bool Action(NotifyType notifyType, BaseInfo baseInfo, UserInfo userInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Request_Betting:
                    {
                        SicboInfo sicboInfo = (SicboInfo)_GameTable.GetTableInfo();

                        int playerIndex = _GameTable.GetPlayerIndex(userInfo);
                        
                        if (playerIndex < 0)
                            return false;

                        BettingInfo bettingInfo = (BettingInfo)baseInfo;

                        if (userInfo.nCashOrPointGame == 0)
                        {
                            if (userInfo.Cash < bettingInfo._Score)
                            {
                                BaseInfo.SetError(ErrorType.Notenough_Cash, "베팅할 금액이 부족합니다");
                                return false;
                            }
                        }
                        else
                        {
                            if (userInfo.Point < bettingInfo._Score)
                            {
                                BaseInfo.SetError(ErrorType.Notenough_Cash, "베팅할 금액이 부족합니다");
                                return false;
                            }
                        }

                        //Cash.GetInstance().GiveCash(userInfo.Id, -bettingInfo._Score);
                        //Cash.GetInstance().GiveGameSum(userInfo, -bettingInfo._Score);
                        //Cash.GetInstance().GiveGameSum(Database.GetInstance().GetManager(UserKind.Manager), bettingInfo._Score);

                        sicboInfo.m_lUserScore[bettingInfo._Area, playerIndex] += bettingInfo._Score;

                        _GameTable.BroadCastGame( NotifyType.Reply_Betting, baseInfo);
                    }
                    break;

                default:
                    return base.Action(notifyType, baseInfo, userInfo);
                    break;
            }

            return true;
        }

        public override bool CanEnd()
        {
            if (_IsLeaveTime == false)
                return false;

            return true;
        }
    }

    public class SicboEndRound : EndRound
    {
        public SicboLogic _GameLogic = new SicboLogic();

        public override void CheckWinner()
        {
            SicboInfo tableInfo = (SicboInfo)_GameTable.GetTableInfo();

	        // 查看是否需要控制
            E_CARD_TYPE[] enCardType = new E_CARD_TYPE[SicboDefine.COUNT_AZIMUTH];
	        
            int iCardTypeCount = 0;
	        int i=0;

            for (i = 0; i < SicboDefine.COUNT_AZIMUTH; ++i)
	        {
                enCardType[i] = E_CARD_TYPE.enCardType_Illegal;
	        }

            int[] cards = tableInfo.m_enCards;

	        do 
	        {
                Random random = new Random();

                cards[0] = (random.Next() % 6 + 1);
                cards[1] = (random.Next() % 6 + 1);
                cards[2] = (random.Next() % 6 + 1);		

		        int iCountNum = cards[0] + cards[1] + cards[2];

		        if (((cards[0] == cards[1]) && (cards[1]==cards[2]))  // 三个相同
			        || ((int)E_CARD_TYPE.enCardType_NumberFour==iCountNum) || ((int)E_CARD_TYPE.enCardType_NumberSeventeen==iCountNum))
		        {
			        ++tableInfo.m_iSameCount;
                    if (tableInfo.m_iSameCount < 30)
			        {
				        System.Threading.Thread.Sleep(10);
				        continue;
			        }
                    tableInfo.m_iSameCount = 0;
		        }
	        } while (false);

	        _GameLogic.GetCardType( cards, enCardType, ref iCardTypeCount);

            for (i = 0; i < iCardTypeCount; ++i)
            {
                if (E_CARD_TYPE.enCardType_Illegal != enCardType[i])
                {
                    tableInfo.m_bWinner[(int)enCardType[i]] = 1;
                }
            } 
        }

        public override void CheckScore()
        {
            SicboInfo tableInfo = (SicboInfo)_GameTable.GetTableInfo();
            int gamePercent = _GameTable._GameInfo.Commission;

            int[] lUserLostScore = new int[GameDefine.GAME_PLAYER];
            int lBankerWinScore = 0;

            for (int j = 0; j < SicboDefine.COUNT_AZIMUTH; ++j)
            {
                for (int k = 0; k < GameDefine.GAME_PLAYER; k++)
                    lBankerWinScore += tableInfo.m_lUserScore[j, k];
            }

	        //计算金币
            for (int i = 0; i < tableInfo._Players.Count; ++i)
	        {
		        //庄家判断
		        //if (m_wCurrentBanker==i) continue;

		        //获取用户
		        for (int j=0; j<SicboDefine.COUNT_AZIMUTH; ++j )
		        {
			        lUserLostScore[i] -= tableInfo.m_lUserScore[j,i];

			        // 该区域是否赢了
			        if (tableInfo.m_bWinner[j] > 0 )
			        {
				        int i64WinScore = 0;
				        int i64TmpScore = 0;

				        if ((j<(int)E_CARD_TYPE.enCardType_SicboOne) ||(j>(int)E_CARD_TYPE.enCardType_SicboSix))
				        {
					        if(tableInfo.m_lUserScore[j,i]>0)
						        i64WinScore = (tableInfo.m_lUserScore[j,i] * SicboLogic.m_i64Loss_Percent[j]);
					        else
						        i64TmpScore = -(tableInfo.m_lUserScore[j,i]) * SicboLogic.m_i64Loss_Percent[j];
				        }
				        else
				        {
					        // 需要换算个数
					        int enSicboNum = (j-(int)E_CARD_TYPE.enCardType_SicboOne+1);
					        int iCountNum = _GameLogic.GetSicboCountByNumber(tableInfo.m_enCards, enSicboNum);
					        int[] iMuti= new int[]{0,2,3,4};
					        
                            if(tableInfo.m_lUserScore[j,i]>0)
						        i64WinScore = (tableInfo.m_lUserScore[j,i] * iMuti[iCountNum]);
					        else
                                i64TmpScore = -(tableInfo.m_lUserScore[j,i]) * iMuti[iCountNum];
				        }

				        tableInfo.m_lUserWinScore[i] += i64WinScore;
                        tableInfo.m_lUserReturnScore[i] += (tableInfo.m_lUserScore[j, i] > 0) ? tableInfo.m_lUserScore[j, i] : 0;
                        lBankerWinScore -= i64WinScore - i64TmpScore;
			        }
                    //else
                    //{
                    //    __int64 i64LoseScore = m_lUserScore[j][i];
                    //    lUserLostScore[i] -= i64LoseScore;
                    //    lBankerWinScore  += i64LoseScore;
                    //}
		        }

		        //计算税收
                //if (0 < tableInfo.m_lUserWinScore[i])
                //{
                //    tableInfo.m_lUserRevenue[i]  = (tableInfo.m_lUserWinScore[i]*gamePercent)/1000;
                //    tableInfo.m_lUserWinScore[i] -= tableInfo.m_lUserRevenue[i];
                //}

                //总的分数
                tableInfo.m_lUserWinScore[i] += lUserLostScore[i];
	        }
        }
    }


    public class SicboLogic
    {
        int[] m_enSicboData = new int[SicboDefine.MAX_SICBO_NUMBER];				//扑克定义

        // 赔率
        public static int[] m_i64Loss_Percent = new int[]{
            2 /*大 ×2*/,  2/*小×2*/,  2/*单×2*/,  2/*双×2*/,

            64/*3个骰子点数和为4  ×64*/, 32/*3个骰子点数和为5  ×32*/, 
            20/*3个骰子点数和为6  ×20*/, 14/*3个骰子点数和为7  ×14*/, 
            10/*3个骰子点数和为8  ×10*/,   8/*3个骰子点数和为9  ×8*/, 
            8/*3个骰子点数和为10  ×8*/,    8/*3个骰子点数和为11  ×8*/,
            8/*3个骰子点数和为12  ×8*/,   10/*3个骰子点数和为13  ×10*/,
            14/*3个骰子点数和为14  ×14*/, 20/*3个骰子点数和为15  ×20*/,
            32/*3个骰子点数和为16  ×32*/, 64/*3个骰子点数和为17  ×64*/,

            2 /*骰点1 */, 2/*骰点2*/, 2/*骰点3*/, 2/*骰点4*/, 2/*骰点5*/, 2/*骰点6*/,

            6/*骰点1,2*/, 6/*骰点1,3*/, 6/*骰点1,4*/,6/*骰点1,5*/,6/*骰点1,6*/,
            6/*骰点2,3*/, 6/*骰点2,4*/, 6/*骰点2,5*/,6/*骰点2,6*/,6/*骰点3,4*/,
            6/*骰点3,5*/, 6/*骰点3,6*/, 6/*骰点4,5*/,6/*骰点4,6*/,6/*骰点5,6*/,

            12/*骰点2个1*/,	12/*骰点2个2*/, 12/*骰点2个3*/,
            12/*骰点2个4*/, 12/*骰点2个5*/, 12/*骰点2个6*/,

            210/*骰点3个1*/,210/*骰点3个2*/,210/*骰点3个3*/,
            210/*骰点3个4*/,210/*骰点3个5*/,210/*骰点3个6*/,
            32          // 骰点3个相同点数
        };


        void RandCardList(byte[] cbCardBuffer, byte cbBufferCount)
        {
            // 骰子有5组，六个,不需要使用牌值定义
            Random random = new Random();

            for (byte i = 0; i < cbBufferCount; ++i)
            {
                cbCardBuffer[i] = (byte)(random.Next() % 6 + 1);
            }
            return;
        }

        // 排序
        void SortCards(byte[] byCardsBuffer, byte byCardCount)
        {
            // 从大到小排列
            for (byte i = 1; i < byCardCount; ++i)
            {
                for (byte j = 0; j < i; ++j)
                {
                    if (byCardsBuffer[i] > byCardsBuffer[j])
                    {
                        byte byCardTemp = byCardsBuffer[i];
                        byCardsBuffer[i] = byCardsBuffer[j];
                        byCardsBuffer[j] = byCardTemp;
                    }
                }
            }
        }

        public int GetSicboCountByNumber(int[] enSicboBuffer, int enSicboNumber)
        {
            // 获取点数个数
            int i = 0;
            int iCount = 0;
            for (i = 0; i < SicboDefine.MAX_COUNT_SICBO; ++i)
            {
                if (enSicboNumber == enSicboBuffer[i])
                {
                    ++iCount;
                }
            }
            return iCount;
        }

        // 单双
        E_CARD_TYPE IsSicboSingleDouble(int[] enSicboNumber)
        {
            // 计算骰子点数和
            byte bySum = 0;
            for (byte i = 0; i < SicboDefine.MAX_COUNT_SICBO; ++i)
            {
                bySum += (byte)enSicboNumber[i];
            }
            return ((0 == bySum % 2) ? E_CARD_TYPE.enCardType_Double : E_CARD_TYPE.enCardType_Single);
        }

        // 3个骰子点数和
        E_CARD_TYPE IsSicboNumber(int[] enSicboNumber)
        {
            // 计算骰子点数和
            byte bySum = 0;
            for (byte i = 0; i < SicboDefine.MAX_COUNT_SICBO; ++i)
            {
                bySum += (byte)enSicboNumber[i];
            }
            if ((bySum < (byte)E_CARD_TYPE.enCardType_NumberFour) || (bySum > (byte)E_CARD_TYPE.enCardType_NumberSeventeen))
            {
                return E_CARD_TYPE.enCardType_Illegal;
            }
            return ((E_CARD_TYPE)bySum);
        }


        // 获取牌型
        public bool GetCardType(int[] enCardsBuffer, E_CARD_TYPE[] enCardType, ref int iCardTypeCount)
        {
            iCardTypeCount = 0;
            // 先要找出围骰
            // 获取点数个数
            int i = 0;
            for (i = 0; i < SicboDefine.MAX_SICBO_NUMBER; ++i)
            {
                int iCount = GetSicboCountByNumber(enCardsBuffer, m_enSicboData[i]);
                if (SicboDefine.MAX_COUNT_SICBO == iCount)
                {
                    enCardType[iCardTypeCount] = E_CARD_TYPE.enCardType_SicboThreeSame;
                    ++iCardTypeCount;

                    enCardType[iCardTypeCount] = (E_CARD_TYPE)(E_CARD_TYPE.enCardType_SicboThreeOne + i);
                    ++iCardTypeCount;

                    enCardType[iCardTypeCount] = (E_CARD_TYPE)(E_CARD_TYPE.enCardType_SicboOne + i);
                    ++iCardTypeCount;
                    return true;  // 不会有其他的牌型了

                }
                else if (iCount > 0) // 不存在围骰
                {
                    break;
                }
            }

            // 获取点数
            E_CARD_TYPE enCardTypeTemp = IsSicboNumber(enCardsBuffer);
            if (E_CARD_TYPE.enCardType_Illegal != enCardTypeTemp)
            {
                enCardType[iCardTypeCount] = enCardTypeTemp;
                ++iCardTypeCount;

                // 获取大小
                if (enCardTypeTemp > E_CARD_TYPE.enCardType_NumberTen)
                {
                    enCardType[iCardTypeCount] = E_CARD_TYPE.enCardType_Big;
                }
                else
                {
                    enCardType[iCardTypeCount] = E_CARD_TYPE.enCardType_Small;
                }
                ++iCardTypeCount;
            }

            // 获取单双
            enCardTypeTemp = IsSicboSingleDouble(enCardsBuffer);
            if (E_CARD_TYPE.enCardType_Illegal != enCardTypeTemp)
            {
                enCardType[iCardTypeCount] = enCardTypeTemp;
                ++iCardTypeCount;
            }

            // 获取点数个数
            for (i = 0; i < SicboDefine.MAX_SICBO_NUMBER; ++i)
            {
                int iCount = GetSicboCountByNumber(enCardsBuffer, m_enSicboData[i]);
                if (iCount > 0)
                {
                    enCardType[iCardTypeCount] = (E_CARD_TYPE)(E_CARD_TYPE.enCardType_SicboOne + i);
                    ++iCardTypeCount;

                    // 有一个才有两个
                    if (iCount > 1)
                    {
                        enCardType[iCardTypeCount] = (E_CARD_TYPE)(E_CARD_TYPE.enCardType_SicboDoubleOne + i);
                        ++iCardTypeCount;
                    }

                    // 散牌
                    for (int j = (i + 1); j < SicboDefine.MAX_SICBO_NUMBER; ++j)
                    {
                        int iCount2 = GetSicboCountByNumber(enCardsBuffer, m_enSicboData[j]);
                        if (iCount2 > 0)
                        {
                            enCardType[iCardTypeCount] = (E_CARD_TYPE)(E_CARD_TYPE.enCardType_SicboOneAndTwo + (i * SicboDefine.MAX_SICBO_NUMBER + j) - (((i + 1) * (i + 2)) / 2));
                            ++iCardTypeCount;
                        }
                    }
                }
            }
            return true;
        }
    }

}
