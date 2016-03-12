using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.Net.Sockets;
using ChatEngine;

namespace ChatServer
{
    class DiceTable : GameTable
    {
        public DiceTable(string tableId)
        {
            _TableInfo = new DiceInfo();
            _TableInfo._TableId = tableId;

            _Rounds.Add(new ReadyRound());
            _Rounds.Add(new DiceBettingRound());
            _Rounds.Add(new DiceEndRound());

            for (int i = 0; i < _Rounds.Count; i++)
                _Rounds[i]._GameTable = this;

            // added by usc at 2014/01/20
            _TableInfo.m_lMinScore = 100;
        }

        public override bool PlayerOutTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            int seaterIndex = GetPlayerIndex(userInfo);

            if (seaterIndex >= 0)
            {
                if (_Rounds[_TableInfo._RoundIndex] is DiceBettingRound)
                {
                    DiceInfo diceInfo = (DiceInfo)_TableInfo;

                    int lAllScore = 0;

                    for (int i = 0; i < 4; i++)
                        lAllScore += diceInfo.m_lUserScore[seaterIndex, i];

                    // added by usc at 2014/02/23
                    for (int i = 0; i < 4; i++)
                    {
                        // deleted by usc at 2014/04/03
                        //diceInfo.m_lPlayerBetAll[i] -= diceInfo.m_lUserScore[seaterIndex, i];
                        diceInfo.m_lUserScore[seaterIndex, i] = 0;
                    }

                    if (lAllScore > 0)
                    {
                        diceInfo.m_lUserWinScore[seaterIndex] = -lAllScore;

                        Cash.GetInstance().ProcessGameCash(seaterIndex, _GameInfo, _TableInfo);
                    }

                    for (int i = seaterIndex; i < _TableInfo._Players.Count; i++)
                    {
                        for (int k = 0; k < 4; k++)
                            diceInfo.m_lUserScore[i,k] = diceInfo.m_lUserScore[i + 1, k];

                        // added by usc at 2014/04/09
                        diceInfo.m_lUserBetScore[i] = diceInfo.m_lUserBetScore[i + 1];
                    }
                }
            }

            return base.PlayerOutTable(baseInfo, userInfo);
        }
    }

    public class DiceBettingRound : StartRound
    {
        Random _random = new Random();

        public override void Start()
        {
            _TimerId = TimerID.Betting;
            _GameTable.AddGameTimer(_TimerId, DiceDefine.BET_TIME);

            base.Start();
        }

        public override void InitTableData(TableInfo tableInfo)
        {
            DiceInfo diceInfo = (DiceInfo)tableInfo;

            //变量定义
            Array.Clear(diceInfo.m_lUserScore, 0, diceInfo.m_lUserScore.Length);
            Array.Clear(diceInfo.m_lUserWinScore, 0, diceInfo.m_lUserWinScore.Length);

            // added newly
            Array.Clear(diceInfo.m_lPlayerBetAll, 0 , diceInfo.m_lPlayerBetAll.Length);

            Array.Clear(diceInfo.m_enCards, 0, diceInfo.m_enCards.Length);
            Array.Clear(diceInfo.m_bWinner, 0, diceInfo.m_bWinner.Length);

            // added by usc at 2014/04/09
            Array.Clear(diceInfo.m_lUserBetScore, 0, diceInfo.m_lUserBetScore.Length);
        }

        public override void AutoAction(UserInfo userInfo)
        {
            // modified by usc at 2014/02/27
            int bettingCount = _random.Next(3, 10);

            for (int i = 0; i < bettingCount; i++)
            {
                int delay = _random.Next(3, DiceDefine.BET_TIME - 5);

                _GameTable.AddAutoTimer( userInfo, delay );
            }
        }

        public override void NotifyGameTimer(GameTimer gameTimer)
        {
            if (gameTimer.timerId != TimerID.Custom || gameTimer.autoInfo == null)
                return;

            int[] bettingScores = new int[] { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 
                                              100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 
                                                1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 
                                                10000, 10000, 10000, 10000, 10000,
                                                /*100000 , 100000, 
                                                500000*/ };

            // added by usc at 2014/02/26
            DiceInfo diceInfo = (DiceInfo)_GameTable.GetTableInfo();

            CurBettingInfo curBettingInfo = GetCurBettingInfo(diceInfo);

            int nBettingArea = curBettingInfo.nMinArea;
            int nMinScore = curBettingInfo.nMinScore;
            int nMaxScore = curBettingInfo.nMaxScore;

            int nScore = 0;

            if (nMaxScore <= 1000)
            {
                if (nMaxScore == 0)
                    nScore = (int)(Math.Pow(10, (double)(_random.Next() % 2) + 2));
                else
                    nScore = bettingScores[_random.Next() % bettingScores.Length];

                nBettingArea = _random.Next() % 4;
            }
            else
            {
                int nReaptCnt = 0;

                while (true)
                {
                    nReaptCnt++;

                    nScore = bettingScores[_random.Next() % bettingScores.Length];

                    if (nMinScore + nScore <= nMaxScore + nMaxScore * _random.Next(25, 75) / 100)
                        break;

                    if (nReaptCnt > 20)
                        break;
                }
            }

            // added by usc at 2014/03/19
            if (nMinScore + nScore > 50000)
                return;

            BettingInfo bettingInfo = new BettingInfo();

            bettingInfo._Area = nBettingArea;
            bettingInfo._Score = nScore;
            bettingInfo._UserIndex = _GameTable.GetPlayerIndex(gameTimer.autoInfo);
            
            Action(NotifyType.Request_Betting, bettingInfo, gameTimer.autoInfo);
        }

        // added by usc at 2014/02/27
        private CurBettingInfo GetCurBettingInfo(DiceInfo diceInfo)
        {
            CurBettingInfo curBettingInfo = new CurBettingInfo();

            int[] aAreaScore = { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                aAreaScore[i] = diceInfo.m_lPlayerBetAll[i];
            }

            int nMinArea = 0;
            int nMinScore = aAreaScore[0];
            int nMaxScore = aAreaScore[0];


            for (int i = 1; i < 4; i++)
            {

                if (aAreaScore[i] > nMaxScore)
                    nMaxScore = aAreaScore[i];

                if (aAreaScore[i] < nMinScore)
                {
                    nMinArea = i;
                    nMinScore = aAreaScore[i];                    
                }
            }

            curBettingInfo.nMinArea = nMinArea;
            curBettingInfo.nMinScore = nMinScore;
            curBettingInfo.nMaxScore = nMaxScore;

            return curBettingInfo;

        }

        public override bool Action(NotifyType notifyType, BaseInfo baseInfo, UserInfo userInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Request_Betting:
                    {
                        DiceInfo diceInfo = (DiceInfo)_GameTable.GetTableInfo();

                        int playerIndex = _GameTable.GetPlayerIndex(userInfo);

                        if (playerIndex < 0)
                            return false;
                        
                        BettingInfo bettingInfo = (BettingInfo)baseInfo;

                        int nCurUserMoney = userInfo.GetGameMoney();
                        
                        // added by usc at 2014/02/03
                        int curBettingMoney = 0;
                        for (int i = 0; i < 4; i++)
                            curBettingMoney += diceInfo.m_lUserScore[playerIndex, i];

                        if (nCurUserMoney < curBettingMoney + bettingInfo._Score)
                        {
                            BaseInfo.SetError(ErrorType.Notenough_Cash, "베팅할 금액이 부족합니다");
                            return false;
                        }

                        if (curBettingMoney + bettingInfo._Score > GameDefine.MAX_BETTING_MONEY)
                            return false;

                        diceInfo.m_lUserScore[playerIndex, bettingInfo._Area] += bettingInfo._Score;
                        diceInfo.m_lPlayerBetAll[bettingInfo._Area] += bettingInfo._Score;

                        // added by usc at 2014/04/09
                        diceInfo.m_lUserBetScore[playerIndex] += bettingInfo._Score;

                        _GameTable.BroadCastGame(NotifyType.Reply_Betting, baseInfo);
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

    public class DiceEndRound : EndRound
    {
        
        public override void CheckWinner()
        {
            Random random = new Random();

            int miniSum = 1000000;
            int[] miniCards = new int[3];

            for (int i = 0; i < 11; i++)
            {
                DiceInfo tableInfo = (DiceInfo)_GameTable.GetTableInfo();

                            // 查看是否需要控制
                int[] cards = tableInfo.m_enCards;


                if (i == 10)
                {
                    cards[0] = miniCards[0];
                    cards[1] = miniCards[1];
                    cards[2] = miniCards[2];
                }
                else
                {
                    cards[0] = (random.Next() % 12 + 1);

                    if (random.Next() % 12 == 0)
                    {
                        cards[1] = cards[0];
                        cards[2] = cards[0];
                    }
                    else
                    {
                        cards[1] = (random.Next() % 12 + 1);
                        cards[2] = (random.Next() % 12 + 1);
                    }
                }

                int sum = cards[0] + cards[1] + cards[2];

                Array.Clear(tableInfo.m_bWinner, 0, tableInfo.m_bWinner.Length);

                if (cards[0] == cards[1] && cards[1] == cards[2])
                {
                    tableInfo.m_bWinner[random.Next() % 4] = 1;
                }
                else
                {
                    if (sum > 10)
                    {
                        tableInfo.m_bWinner[0] = 1;
                    }
                    else
                    {
                        tableInfo.m_bWinner[1] = 1;
                    }

                    if (sum % 2 == 1)
                    {
                        tableInfo.m_bWinner[2] = 1;
                    }
                    else
                    {
                        tableInfo.m_bWinner[3] = 1;
                    }
                }

                CheckScore();

                int totalSum = 0;
                int lSystemScore = 0;
                int nBuyerCount = 0;

                for (int k = 0; k < tableInfo._Players.Count; k++)
                {
                    if (tableInfo._Players[k].Auto > 0 || tableInfo._Players[k].Kind != (int)UserKind.Buyer)
                        continue;

                    totalSum += tableInfo.m_lUserWinScore[k];

                    nBuyerCount++;
                }

                if (nBuyerCount == 0)
                    break;

                lSystemScore = -totalSum;

                if (tableInfo.m_StorageScore + lSystemScore < 0)
                {
                    if (totalSum < miniSum)
                    {
                        miniSum = totalSum;

                        for (int m = 0; m < 3; m++)
                            miniCards[m] = cards[m];
                    }
                }
                else
                {
                    if (tableInfo.m_StorageScore == DiceDefine.FIRST_EVENT_SCORE)
                        tableInfo.m_StorageScore -= DiceDefine.FIRST_EVENT_SCORE;

                    tableInfo.m_StorageScore += lSystemScore;
                    tableInfo.m_StorageScore -= (tableInfo.m_StorageScore * tableInfo.m_StorageDeduct / 1000);
                    break;
                }
            }
        }

        public override void CheckScore()
        {
            DiceInfo tableInfo = (DiceInfo)_GameTable.GetTableInfo();

            Array.Clear(tableInfo.m_lUserWinScore, 0, tableInfo.m_lUserWinScore.Length);

	                //计算金币
            int[] lUserLostScore = new int[GameDefine.GAME_PLAYER];
            //int lBankerWinScore = 0;

            //for (int i = 0; i < tableInfo._Players.Count; i++)
            //{
            //    for (int k = 0; k < 4; k++)
            //    {
            //        lBankerWinScore += tableInfo.m_lUserScore[i, k];
            //    }
            //}

                    //计算金币
            int[] cards = tableInfo.m_enCards;

            for (int i = 0; i < tableInfo._Players.Count; ++i)
            {
                            //庄家判断
                //if (m_wCurrentBanker==i) continue;

                            //获取用户
                for (int j = 0; j < 4; ++j)
                {
                    lUserLostScore[i] += tableInfo.m_lUserScore[i, j];

                                    // 该区域是否赢了
                    if (tableInfo.m_bWinner[j] > 0)
                    {
                        int winScore = 0;

                        if (tableInfo.m_lUserScore[i, j] > 0)
                        {
                            int multi = 2;

                            if (cards[0] == cards[1] && cards[1] == cards[2])
                                multi = cards[0];

                            winScore = tableInfo.m_lUserScore[i, j] * multi;
                        }

                        tableInfo.m_lUserWinScore[i] += winScore;
                        //lBankerWinScore -= winScore;
                    }
                }

                            //计算税收
                //if (tableInfo.m_lUserWinScore[i] > 0)
                //{
                //    tableInfo.m_lUserRevenue[i] = (tableInfo.m_lUserWinScore[i] * gamePercent) / 1000;
                //    tableInfo.m_lUserWinScore[i] -= tableInfo.m_lUserRevenue[i];
                //}

                            //总的分数
                tableInfo.m_lUserWinScore[i] -= lUserLostScore[i];
            }
        }
    }

}
