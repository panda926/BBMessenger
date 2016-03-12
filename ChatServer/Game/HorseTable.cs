using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;

namespace ChatServer
{
    class HorseTable : GameTable
    {
        public HorseTable(string tableId)
        {
            _TableInfo = new HorseInfo();
            _TableInfo._TableId = tableId;

            _Rounds.Add(new HorseReadyRound());
            _Rounds.Add(new HorseBetStartRound());
            _Rounds.Add(new HorseBetEndRound());
            _Rounds.Add(new HorseRunRound());
            _Rounds.Add(new HorseEndRound());

            for (int i = 0; i < _Rounds.Count; i++)
                _Rounds[i]._GameTable = this;

            _TableInfo.m_lMinScore = 100;
        }

        public override bool PlayerOutTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            int seaterIndex = GetPlayerIndex(userInfo);

            if (seaterIndex >= 0)
            {
                //if (_Rounds[_TableInfo._RoundIndex] is SicboBettingRound)
                //{
                HorseInfo horseInfo = (HorseInfo)_TableInfo;

                int lAllScore = 0;

                for (int i = 0; i < HorseDefine.AREA_ALL; i++)
                    lAllScore += horseInfo.m_lPlayerBet[seaterIndex, i];

                // added by usc at 2014/03/04
                for (int i = 0; i < HorseDefine.AREA_ALL; i++)
                {
                    // deleted by usc at 2014/04/03
                    //horseInfo.m_lPlayerBetAll[i] -= horseInfo.m_lPlayerBet[seaterIndex, i];
                    horseInfo.m_lPlayerBet[seaterIndex, i] = 0;
                }

                if (lAllScore > 0)
                {
                    horseInfo.m_lUserWinScore[seaterIndex] = -lAllScore;

                    Cash.GetInstance().ProcessGameCash(seaterIndex, _GameInfo, _TableInfo);
                }

                for (int i = seaterIndex; i < _TableInfo._Players.Count; i++)
                {
                    for (int k = 0; k < HorseDefine.AREA_ALL; k++)
                    {
                        horseInfo.m_lPlayerBet[i, k] = horseInfo.m_lPlayerBet[i + 1, k];
                        horseInfo.m_lPlayerBetWin[i, k] = horseInfo.m_lPlayerBetWin[i + 1, k];                        
                    }

                    horseInfo.m_lPlayerWinning[i] = horseInfo.m_lPlayerWinning[i + 1];
                    horseInfo.m_lPlayerReturnBet[i] = horseInfo.m_lPlayerReturnBet[i + 1];
                    horseInfo.m_lPlayerRevenue[i] = horseInfo.m_lPlayerRevenue[i + 1];

                    // added by usc at 2014/04/09
                    horseInfo.m_lUserBetScore[i] = horseInfo.m_lUserBetScore[i + 1];
                }
                //}
            }

            return base.PlayerOutTable(baseInfo, userInfo);
        }
    }

    public class HorseReadyRound : ReadyRound
    {
        public override int GetNeedPlayers()
        {
            return 1;
        }
    }

    public class HorseBetStartRound : StartRound
    {
        Random _random = new Random();

        public override void Start()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            ZeroMemory(horseInfo.m_lPlayerBet);
            ZeroMemory(horseInfo.m_lUserBetScore);

            _TimerId = HorseInfo.IDI_BET_START;
            _GameTable.AddGameTimer(_TimerId, horseInfo.m_nBetTime);

            // added at 2014/01/07
            MultipleControl();

            base.Start();
        }

        public override void InitTableData(TableInfo tableInfo)
        {
            //定义变量
            //CMD_S_BetStart stBetStart;
            //ZeroMemory(&stBetStart, sizeof(stBetStart));
            //stBetStart.nTimeLeave = m_nBetTime;
            //stBetStart.nTimeBetEnd = m_nBetEndTime;

            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            // added by usc at 2014/02/25
            ZeroMemory(horseInfo.m_lPlayerBet);
            ZeroMemory(horseInfo.m_lPlayerBetAll);
            ZeroMemory(horseInfo.m_lUserWinScore);

            // added by usc at 2014/04/09
            ZeroMemory(horseInfo.m_lUserBetScore);

            //下注机器人数量
            //int nChipRobotCount = 0;
            //for (int i = 0; i < GameDefine.GAME_PLAYER; i++)
            //{
            //    IServerUserItem* pIServerUserItem = m_pITableFrame->GetTableUserItem(i);
            //    if (pIServerUserItem != NULL && pIServerUserItem->IsAndroidUser())
            //        nChipRobotCount++;
            //}
            //stBetStart.nChipRobotCount = min(nChipRobotCount, m_nMaxChipRobot);

            ////机器人控制
            //m_nChipRobotCount = 0;

            //发送消息 
            //----------------------------
            //旁观玩家
            //m_pITableFrame->SendLookonData(INVALID_CHAIR, SUB_S_BET_START, &stBetStart, sizeof(stBetStart));

            //游戏玩家
            //for (WORD wChairID = 0; wChairID < GAME_PLAYER; ++wChairID)
            //{
            //    IServerUserItem* pIServerUserItem = m_pITableFrame->GetTableUserItem(wChairID);
            //    if (pIServerUserItem == NULL) continue;

            //    //设置积分
            //    stBetStart.lUserMaxScore = min(pIServerUserItem->GetUserScore(), m_lUserLimitScore);

            //    m_pITableFrame->SendTableData(wChairID, SUB_S_BET_START, &stBetStart, sizeof(stBetStart));
            //}
            //return true;

            //AutoProcess();
        }

        public override void AutoAction(UserInfo userInfo)
        {
            // modified by usc at 2014/02/27
            int bettingCount = _random.Next(3, 10);

            for (int i = 0; i < bettingCount; i++)
            {
                int delay = _random.Next(2, HorseDefine.BET_TIME - 1);

                _GameTable.AddAutoTimer(userInfo, delay);
            }
        }

        public override void NotifyGameTimer(GameTimer gameTimer)
        {
            if (gameTimer.timerId != TimerID.Custom || gameTimer.autoInfo == null)
                return;

            int[] bettingScores = new int[] { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100,
                                              100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100,
                                                1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000,
                                                10000, 10000, 10000, 10000, 10000/*,
                                                100000*/ };

            // added by usc at 2014/02/26
            HorseInfo horseInfo = (HorseInfo)_GameTable.GetTableInfo();

            CurBettingInfo curBettingInfo = GetCurBettingInfo(horseInfo);

            int nBettingArea = curBettingInfo.nMinArea;
            int nMinScore = curBettingInfo.nMinScore;
            int nMaxArea = curBettingInfo.nMaxArea;
            int nMaxScore = curBettingInfo.nMaxScore;

            int nScore = 0;

            if (nMaxScore <= 1000 * horseInfo.m_nMultiple[nMaxArea])
            {
                if (nMaxScore == 0)
                    nScore = (int)(Math.Pow(10, (double)(_random.Next() % 2) + 2));
                else
                    nScore = bettingScores[_random.Next() % bettingScores.Length];

                nBettingArea = _random.Next() % HorseDefine.AREA_COUNT;
            }
            else
            {
                int nReaptCnt = 0;

                while (true)
                {
                    nReaptCnt++;

                    nScore = bettingScores[_random.Next() % bettingScores.Length];

                    if (nMinScore + nScore * horseInfo.m_nMultiple[nBettingArea] <= nMaxScore + nMaxScore * _random.Next(25, 75) / 100)
                        break;

                    if (nReaptCnt > 20)
                        break;
                }
            }

            // added by usc at 2014/03/19
            if (nMinScore + nScore * horseInfo.m_nMultiple[nBettingArea] > 50000 * horseInfo.m_nMultiple[nBettingArea])
                return;

            BettingInfo bettingInfo = new BettingInfo();

            bettingInfo._Area = nBettingArea;
            bettingInfo._Score = nScore;
            bettingInfo._UserIndex = _GameTable.GetPlayerIndex(gameTimer.autoInfo);

            Action(NotifyType.Request_Betting, bettingInfo, gameTimer.autoInfo);
        }

        // added by usc at 2014/02/27
        private CurBettingInfo GetCurBettingInfo(HorseInfo horseInfo)
        {
            CurBettingInfo curBettingInfo = new CurBettingInfo();

            int[] aAreaScore = new int[HorseDefine.AREA_COUNT];

            for (int i = 0; i < HorseDefine.AREA_COUNT; i++)
            {
                aAreaScore[i] = horseInfo.m_lPlayerBetAll[i] * horseInfo.m_nMultiple[i];
            }


            int nMinArea = 0;
            int nMinScore = aAreaScore[0];
            int nMaxArea = 0;
            int nMaxScore = aAreaScore[0];

            for (int i = 1; i < HorseDefine.AREA_COUNT; i++)
            {
                if (aAreaScore[i] > nMaxScore)
                {
                    nMaxArea = i;
                    nMaxScore = aAreaScore[i];
                }

                if (aAreaScore[i] < nMinScore)
                {
                    nMinArea = i;
                    nMinScore = aAreaScore[i];
                }
            }

            curBettingInfo.nMinArea = nMinArea;
            curBettingInfo.nMinScore = nMinScore;

            curBettingInfo.nMaxArea = nMaxArea;
            curBettingInfo.nMaxScore = nMaxScore;

            return curBettingInfo;

        }


        public override bool Action(NotifyType notifyType, BaseInfo baseInfo, UserInfo userInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Request_Betting:
                    {
                        //效验数据
                        if (!(baseInfo is BettingInfo))
                            return false;

                        //用户效验
                        if (_GameTable.GetPlayerIndex(userInfo) < 0)
                            return false;

                        //消息处理
                        BettingInfo pPlayerBet = (BettingInfo)baseInfo;

                        //状态验证
                        //if ( m_pITableFrame->GetGameStatus() != GS_BET)
                        //{
                        //    //发送下注失败消息
                        //    CMD_S_PlayerBetFail stPlayerBetFail;
                        //    stPlayerBetFail.cbFailType = FAIL_TYPE_TIME_OVER;
                        //    stPlayerBetFail.wChairID = pIServerUserItem->GetChairID();
                        //    memcpy(stPlayerBetFail.lBetScore, pPlayerBet->lBetScore, sizeof(stPlayerBetFail.lBetScore));
                        //    m_pITableFrame->SendTableData(pIServerUserItem->GetChairID(),SUB_S_PLAYER_BET_FAIL,&stPlayerBetFail,sizeof(stPlayerBetFail));
                        //    return true;
                        //}

                        //判断下注是否合法
                        //for ( int i = 0 ; i < AREA_ALL ; ++i)
                        //{
                        //if ( pPlayerBet._Score > GetPlayersMaxBet(pIServerUserItem->GetChairID(), i) )
                        //{
                        //    //发送下注失败消息
                        //    CMD_S_PlayerBetFail stPlayerBetFail;
                        //    stPlayerBetFail.cbFailType = FAIL_TYPE_OVERTOP;
                        //    stPlayerBetFail.wChairID = pIServerUserItem->GetChairID();
                        //    memcpy(stPlayerBetFail.lBetScore, pPlayerBet->lBetScore, sizeof(stPlayerBetFail.lBetScore));
                        //    m_pITableFrame->SendTableData(pIServerUserItem->GetChairID(),SUB_S_PLAYER_BET_FAIL,&stPlayerBetFail,sizeof(stPlayerBetFail));
                        //    return true;
                        //}
                        //}

                        ////机器人验证
                        //if(pIServerUserItem->IsAndroidUser())
                        //{
                        //    for ( int i = 0 ; i < AREA_ALL; ++i )
                        //    {
                        //        if ( m_lRobotAreaScore[i] + pPlayerBet->lBetScore[i] > m_lRobotAreaLimit )
                        //            return true;
                        //    }

                        //    //数目限制
                        //    bool bHaveChip = false;
                        //    for (int i = 0; i < AREA_ALL; i++)
                        //    {
                        //        if (m_lPlayerBet[pIServerUserItem->GetChairID()][i] != 0)
                        //            bHaveChip = true;
                        //    }

                        //    if (!bHaveChip)
                        //    {
                        //        if ( m_nChipRobotCount + 1 > m_nMaxChipRobot )
                        //            return true;
                        //        else
                        //            m_nChipRobotCount++;
                        //    }
                        //}

                        //添加人数                        
                        int playerIndex = _GameTable.GetPlayerIndex(userInfo);

                        if (playerIndex < 0)
                            return false;

                        HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

                        int lTempPlayerAllBet = 0;
                        for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
                            lTempPlayerAllBet += horseInfo.m_lPlayerBet[playerIndex, i];

                        // added by usc at 2014/02/03
                        int nCurUserMoney = userInfo.GetGameMoney();

                        if (nCurUserMoney < lTempPlayerAllBet + pPlayerBet._Score)
                        {
                            BaseInfo.SetError(ErrorType.Notenough_Cash, "베팅할 금액이 부족합니다");
                            return false;
                        }

                        if (lTempPlayerAllBet + pPlayerBet._Score > GameDefine.MAX_BETTING_MONEY)
                            return false;


                        //玩家第一次下注,则添加人数
                        //if (lTempPlayerAllBet > 0)
                        //    horseInfo.m_nBetPlayerCount++;

                        //添加注
                        //for ( int i = 0 ; i < HorseDefine.AREA_ALL ; ++i)
                        //{
                        horseInfo.m_lPlayerBet[playerIndex, pPlayerBet._Area] += pPlayerBet._Score;
                        horseInfo.m_lPlayerBetAll[pPlayerBet._Area] += pPlayerBet._Score;

                        //}

                        // added by usc at 2014/04/09
                        horseInfo.m_lUserBetScore[playerIndex] += pPlayerBet._Score;

                        //发送消息
                        //CMD_S_PlayerBet stPlayerBet;
                        //memcpy(stPlayerBet.lBetScore, pPlayerBet->lBetScore, sizeof(stPlayerBet.lBetScore));
                        //stPlayerBet.wChairID = pIServerUserItem->GetChairID();
                        //stPlayerBet.nBetPlayerCount = m_nBetPlayerCount;
                        //stPlayerBet.bIsAndroid=pIServerUserItem->IsAndroidUser();
                        //m_pITableFrame->SendTableData(INVALID_CHAIR,SUB_S_PLAYER_BET,&stPlayerBet,sizeof(stPlayerBet));
                        //m_pITableFrame->SendLookonData(INVALID_CHAIR,SUB_S_PLAYER_BET,&stPlayerBet,sizeof(stPlayerBet)); 
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
            if (this._IsLeaveTime == false)
                return false;

            return true;
        }

        //是否需要控制
        bool NeedControl()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            if (horseInfo.m_cbCLTimes > 0 && horseInfo.m_bControl)
            {
                return true;
            }
            return false;
        }

        //倍数控制
        void MultipleControl()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            //倍数控制
            //----------------------------
            //INT nControl = GetPrivateProfileInt(m_szGameRoomName, TEXT("MultipleControl"), 0, m_szConfigFileName);
            //m_bMultipleControl == nControl == 1 ? TRUE : FALSE;

            //需要控制
            //if(m_bMultipleControl)
            //{
            //    m_nMultiple[AREA_1_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_6"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_1_5] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_5"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_1_4] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_4"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_1_3] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_3"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_1_2] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_2"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_2_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_2_6"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_2_5] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_2_5"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_2_4] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_2_4"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_2_3] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_2_3"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_3_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_3_6"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_3_5] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_3_5"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_3_4] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_3_4"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_4_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_4_6"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_4_5] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_4_5"), 1, m_szConfigFileName);
            //    m_nMultiple[AREA_5_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_5_6"), 1, m_szConfigFileName);

            //    for ( int i = 0;  i < AREA_ALL; ++i)
            //    {
            //        if ( m_nMultiple[i] < 1 )
            //        {
            //            m_nMultiple[i] = 1;
            //        }
            //    }
            //}
            //else
            //{


            // modified by usc at 2014/03/05
            //随机换倍数            
            //RandomMultiples();
            horseInfo.m_nMultiple = new int[] { 4, 4, 4, 4, 4, 4 };

            //}

            // deleted by usc at 2014/03/05
            //管理员控制
            //if (NeedControl())
            //{
            //    for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
            //    {
            //        if (horseInfo.m_nCLMultiple[i] > 0)
            //        {
            //            horseInfo.m_nMultiple[i] = horseInfo.m_nCLMultiple[i];
            //        }
            //    }
            //}
        }

        static int nStFluc = 1;

        void RandomMultiples()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            int wTick = 0;// GetTickCount();
            // modified by usc at 2014/02/20
            int[] nMultiples = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 15, 16, 17, 18, 18 };//倍数

            int[] nChance = new int[] { 6, 6, 6, 6, 6, 6, 6, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1 };	//几率
            int[] nBigAreaMultiples = new int[] { 0, 1, 2, 3, 4, 5 };							//大倍几率

            //混乱
            ChaosArray(nBigAreaMultiples, nBigAreaMultiples.Length);
            ChaosArray(nMultiples, nMultiples.Length, nChance, nChance.Length);

            //大倍率
            int nBigArea = nBigAreaMultiples[(rand() + wTick) % nBigAreaMultiples.Length];
            horseInfo.m_nMultiple[nBigArea] = 20;

            //随机倍数
            for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
            {
                if (i == nBigArea)
                    continue;

                //几率和值
                int nChanceAndValue = 0;
                for (int n = 0; n < nChance.Length; ++n)
                    nChanceAndValue += nChance[n];

                int nMuIndex = 0;
                int nRandNum = 0;					//随机辅助
                nRandNum = (rand() + wTick + nStFluc * 3 + i) % nChanceAndValue;
                for (int j = 0; j < nChance.Length; j++)
                {
                    nRandNum -= nChance[j];
                    if (nRandNum < 0)
                    {
                        nMuIndex = j;
                        break;
                    }
                }
                nStFluc = nStFluc % 3 + 1;

                horseInfo.m_nMultiple[i] = nMultiples[nMuIndex];
                nChance[nMuIndex] = 0;
            }
        }

        void ChaosArray(int[] nArray, int nCount)
        {
            int wTick = GetTickCount();
            for (int i = 0; i < nCount; ++i)
            {
                int nTempIndex = (rand() + wTick) % nCount;
                int nTempValue = nArray[i];
                nArray[i] = nArray[nTempIndex];
                nArray[nTempIndex] = nTempValue;
            }
        }

        //混乱数组
        void ChaosArray(int[] nArrayOne, int nCountOne, int[] nArrayTwo, int nCountTwo)
        {
            //ASSERT( nCountOne == nCountTwo );
            if (nCountTwo != nCountOne)
                return;

            int wTick = GetTickCount();
            for (int i = 1; i < nCountOne; ++i)
            {
                int nTempIndex = (rand() + wTick) % nCountOne;

                int nTempValueOne = nArrayOne[i];
                nArrayOne[i] = nArrayOne[nTempIndex];
                nArrayOne[nTempIndex] = nTempValueOne;

                int nTempValueTwo = nArrayTwo[i];
                nArrayTwo[i] = nArrayTwo[nTempIndex];
                nArrayTwo[nTempIndex] = nTempValueTwo;
            }
        }

        int rand()
        {
            return _random.Next();
        }

        int GetTickCount()
        {
            return 0;
            //return System.Environment.TickCount;
        }
    }

    public class HorseBetEndRound : GameRound
    {
        public override bool CanStart()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            if (horseInfo._Players.Count < 1)
                return false;

            return true;
        }

        public override void Start()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            _TimerId = HorseInfo.IDI_BET_END;
            _GameTable.AddGameTimer(_TimerId, horseInfo.m_nBetEndTime);

            base.Start();
        }

        public override bool CanEnd()
        {
            if (_IsLeaveTime == false)
                return false;

            return true;
        }
    }

    public class HorseRunRound : GameRound
    {
        public override bool CanStart()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            if (horseInfo._Players.Count < 1)
                return false;

            return true;
        }

        public override void Start()
        {
            //跑马
            HorsesProcess();

            //CMD_S_HorsesStart stHorsesStart;
            //ZeroMemory(&stHorsesStart, sizeof(stHorsesStart));
            //stHorsesStart.nTimeLeave = m_nHorsesTime;
            //memcpy(stHorsesStart.nHorsesSpeed, m_nHorsesSpeed, sizeof(stHorsesStart.nHorsesSpeed));
            //memcpy(stHorsesStart.cbHorsesRanking, horseInfo.m_cbHorsesRanking, sizeof(stHorsesStart.cbHorsesRanking));

            //发送消息
            //----------------------------
            //旁观玩家
            //m_pITableFrame->SendLookonData(INVALID_CHAIR, SUB_S_HORSES_START, &stHorsesStart, sizeof(stHorsesStart));

            //游戏玩家
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            //for (int wChairID = 0; wChairID < horseInfo._Players.Count; ++wChairID)
            //{
            //    //IServerUserItem* pIServerUserItem = m_pITableFrame->GetTableUserItem(wChairID);
            //    //if (pIServerUserItem == NULL) continue;

            //    //设置积分
            //    stHorsesStart.lPlayerWinning = m_lPlayerWinning[wChairID];
            //    stHorsesStart.lPlayerReturnBet = m_lPlayerReturnBet[wChairID];

            //    m_pITableFrame->SendTableData(wChairID, SUB_S_HORSES_START, &stHorsesStart, sizeof(stHorsesStart));
            //}


            //设置时间           
            horseInfo.m_dwGameTime = 0;// (DWORD)time(NULL);

            _TimerId = HorseInfo.IDI_HORSES_START;
            _GameTable.AddGameTimer(_TimerId, horseInfo.m_nHorsesTime);

            // deleted by usc at 2014/03/04
            //_GameTable.AddGameTimer(HorseInfo.IDI_HORSES_END, horseInfo.m_nHorsesEndTime);

            base.Start();
        }

        public override bool CanEnd()
        {
            if (_IsLeaveTime == false)
                return false;

            return true;
        }

        //跑马过程
        void HorsesProcess()
        {
            const int INT_MAX = 2147483647;    /* maximum (signed) int value */

            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            int nRepeatCount = 0;

            while (true)
            {
                nRepeatCount++;

                HorsesStart();

                //先看结果是否符合常规条件或控制
                //再看积分最后是否正确
                if (HorsesEnd() && CalculateScore())
                    break;

                if (nRepeatCount > 20)
                    break;
            }

            //记录
            horseInfo.m_nStreak++;
            DateTime time = DateTime.Now;					//获取当前时间. 
            int nDay = time.Day;								//天
            if (horseInfo.m_nStreak == INT_MAX || horseInfo.m_nDay != nDay)
            {
                horseInfo.m_nStreak = 1;
                horseInfo.m_nDay = nDay;
                ZeroMemory(horseInfo.m_nWinCount);
            }

            tagHistoryRecord HistoryRecord = new tagHistoryRecord();

            HistoryRecord.nStreak = horseInfo.m_nStreak;
            HistoryRecord.nRanking = horseInfo.m_cbHorsesRanking[0];
            HistoryRecord.nRiskCompensate = horseInfo.m_nMultiple[horseInfo.m_cbHorsesRanking[0]];
            HistoryRecord.nHours = time.Hour;
            HistoryRecord.nMinutes = time.Minute;
            HistoryRecord.nSeconds = time.Second;

            horseInfo.m_nWinCount[horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST]]++;

            horseInfo.m_GameRecords.Add(HistoryRecord);
            if (horseInfo.m_GameRecords.Count > HorseDefine.MAX_SCORE_HISTORY)
                horseInfo.m_GameRecords.RemoveAt(0);
        }

        public int GetTickCount()
        {
            return 0;
            //return System.Environment.TickCount;
        }

        Random _random = new Random();

        public int rand()
        {
            return _random.Next();
        }

        //跑马开始
        void HorsesStart()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            //马匹信息
            ZeroMemory(horseInfo.m_nHorsesSpeed);

            //------------------------------------------------------
            //此是得到每秒的速度.. 模糊速度.. 但是数据小.
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                for (int i = 0; i < HorseDefine.COMPLETION_TIME; ++i)
                {
                    //高速 
                    if (i % 2 == 0)
                    {
                        horseInfo.m_nHorsesSpeed[nHorses, i] = (rand() + GetTickCount() + nHorses) % HorseDefine.HIGH_SPEED + 10;
                    }
                    //低速
                    else
                    {
                        horseInfo.m_nHorsesSpeed[nHorses, i] = (rand() + GetTickCount() + nHorses) % HorseDefine.LOW_SPEED;
                        horseInfo.m_nHorsesSpeed[nHorses, i] = -horseInfo.m_nHorsesSpeed[nHorses, i];
                    }
                }

                horseInfo.m_nHorsesSpeed[nHorses, HorseDefine.COMPLETION_TIME - 1] = horseInfo.m_nHorsesSpeed[nHorses, HorseDefine.COMPLETION_TIME - 1] / 20 * 20;
            }

            //------------------------------------------------------
            //此是得到每30毫秒的速度.. 精确速度.. 但是由于数据太大. 不利传输.. 放弃.
            //for ( int nHorses = 0 ; nHorses < HORSES_ALL; ++nHorses )
            //{
            //	//速步索引
            //	int nSpeedIndex = 0;	
            //	//速度
            //	int nVelocity = 1;	
            //	//加速度
            //	int nAcceleration = ACCELERATION;
            //	//高速维持
            //	int nMaintenance = 0;
            //	//频率
            //	int nFrequency = (rand() + GetTickCount()*2 ) % FREQUENCY + 1;
            //	//最高速度
            //	int nHighestSpeed = (rand() + GetTickCount() + nHorses) % HIGH_SPEED;
            //	//最低速度
            //	int nMinimumSpeed = (rand() + GetTickCount() + nHorses) % LOW_SPEED;
            //	nMinimumSpeed = -nMinimumSpeed;

            //	if ( nHighestSpeed == nMinimumSpeed )
            //		nHighestSpeed = nMinimumSpeed + rand()%HIGH_SPEED + 1;

            //	//周期循环
            //	bool bCycle = false;
            //	bool bHighSpeedAppeared = false;
            //	while( nSpeedIndex < STEP_SPEED )
            //	{
            //		m_nHorsesSpeed[nHorses][nSpeedIndex] = nVelocity;

            //		//设置加速度
            //		if ( nVelocity >= nHighestSpeed )
            //		{
            //			bHighSpeedAppeared = true;
            //			nAcceleration = ((rand() + GetTickCount())%ACCELERATION + 1);
            //			nAcceleration = -nAcceleration;
            //		}
            //		else if ( nVelocity <= nMinimumSpeed )
            //		{
            //			nAcceleration = ((rand() + GetTickCount())%ACCELERATION + 2);
            //		}

            //		//设置下一步速度
            //		if( nMaintenance < nFrequency )
            //		{
            //			nMaintenance++;
            //		}
            //		else
            //		{
            //			nFrequency = (rand() + GetTickCount()*2 ) % (FREQUENCY/2) + 1;
            //			nMaintenance = 0;
            //			nVelocity += nAcceleration;
            //		}

            //		//一周期完成
            //		if ( nVelocity == nMinimumSpeed && bHighSpeedAppeared)
            //			bCycle = true;

            //		if ( bCycle )
            //		{
            //			bCycle = false;
            //			bHighSpeedAppeared = false;
            //			nAcceleration = 1;
            //			nMaintenance = 0;
            //			nFrequency = (rand() + GetTickCount()*2 ) % FREQUENCY + 1;
            //			nHighestSpeed = (rand() + GetTickCount() + nHorses) % HIGH_SPEED;
            //			nMinimumSpeed = (rand() + GetTickCount() + nHorses) % LOW_SPEED;
            //			nMinimumSpeed = -nMinimumSpeed;

            //			if ( nHighestSpeed == nMinimumSpeed )
            //				nHighestSpeed = nMinimumSpeed + rand()%HIGH_SPEED + 1;
            //		}

            //		nSpeedIndex++;
            //	}
            //}
        }


        //跑马结束
        bool HorsesEnd()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            //马速度和
            int[] nSpeedTotal = new int[HorseDefine.HORSES_ALL];

            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                nSpeedTotal[nHorses] += horseInfo.m_nHorsesSpeed[nHorses, HorseDefine.COMPLETION_TIME - 1];
            }

            //错误判断
            for (int nHorsesX = 0; nHorsesX < HorseDefine.HORSES_ALL; ++nHorsesX)
            {
                for (int nHorsesY = 0; nHorsesY < HorseDefine.HORSES_ALL; ++nHorsesY)
                {
                    //如果有相等的就错误
                    if (nSpeedTotal[nHorsesX] == nSpeedTotal[nHorsesY] && nHorsesX != nHorsesY)
                    {
                        return false;
                    }
                }
            }

            //找出名次
            int nFirst = HorseDefine.HORSES_ALL;
            int nSecond = HorseDefine.HORSES_ALL;
            int nThird = HorseDefine.HORSES_ALL;
            int nFourth = HorseDefine.HORSES_ALL;
            int nFifth = HorseDefine.HORSES_ALL;
            int nSixth = HorseDefine.HORSES_ALL;

            //第一名
            int nTemp = 0;
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                if (nTemp == 0 || nTemp < nSpeedTotal[nHorses])
                {
                    nTemp = nSpeedTotal[nHorses];
                    nFirst = nHorses;
                }
            }

            //第二名
            nTemp = 0;
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                if (nHorses != nFirst && (nTemp == 0 || nTemp < nSpeedTotal[nHorses]))
                {
                    nTemp = nSpeedTotal[nHorses];
                    nSecond = nHorses;
                }
            }

            //第三名
            nTemp = 0;
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                if (nHorses != nFirst && nHorses != nSecond && (nTemp == 0 || nTemp < nSpeedTotal[nHorses]))
                {
                    nTemp = nSpeedTotal[nHorses];
                    nThird = nHorses;
                }
            }

            //第四名
            nTemp = 0;
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                if (nHorses != nFirst && nHorses != nSecond && nHorses != nThird && (nTemp == 0 || nTemp < nSpeedTotal[nHorses]))
                {
                    nTemp = nSpeedTotal[nHorses];
                    nFourth = nHorses;
                }
            }

            //第五名
            nTemp = 0;
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                if (nHorses != nFirst && nHorses != nSecond && nHorses != nThird && nHorses != nFourth && (nTemp == 0 || nTemp < nSpeedTotal[nHorses]))
                {
                    nTemp = nSpeedTotal[nHorses];
                    nFifth = nHorses;
                }
            }

            //第六名
            nTemp = 0;
            for (int nHorses = 0; nHorses < HorseDefine.HORSES_ALL; ++nHorses)
            {
                if (nHorses != nFirst && nHorses != nSecond && nHorses != nThird && nHorses != nFourth && nHorses != nFifth && (nTemp == 0 || nTemp < nSpeedTotal[nHorses]))
                {
                    nTemp = nSpeedTotal[nHorses];
                    nSixth = nHorses;
                }
            }

            //错误判断
            if (nFirst == HorseDefine.HORSES_ALL || nSecond == HorseDefine.HORSES_ALL || nThird == HorseDefine.HORSES_ALL
                || nFourth == HorseDefine.HORSES_ALL || nFifth == HorseDefine.HORSES_ALL || nSixth == HorseDefine.HORSES_ALL)
            {
                //ASSERT(FALSE);
                return false;
            }

            //马匹信息
            for (int i = 0; i < horseInfo.m_cbHorsesRanking.Length; ++i)
                horseInfo.m_cbHorsesRanking[i] = HorseDefine.HORSES_ALL;

            horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] = nFirst;
            horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] = nSecond;
            horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_THIRD] = nThird;
            horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FOURTH] = nFourth;
            horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIFTH] = nFifth;
            horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SIXTH] = nSixth;

            //最后结果是否可行
            return FinalResults();
        }

        //最后结果
        bool FinalResults()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            //游戏结果
            //if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_ONE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_SIX)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_ONE&& horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_SIX)  )
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_1_6;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_ONE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FIVE)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_ONE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FIVE)  )
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_1_5;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_ONE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FOUR)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_ONE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FOUR)  )
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_1_4;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_ONE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_THREE)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_ONE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_THREE)  )
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_1_3;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_ONE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_TWO)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_ONE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_TWO)  )
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_1_2;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_TWO && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_SIX)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_TWO && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_SIX)  )
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_2_6;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_TWO && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FIVE)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_TWO && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FIVE)  )
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_2_5;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_TWO && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FOUR)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_TWO && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FOUR)  )
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_2_4;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_TWO && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_THREE)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_TWO && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_THREE)  )	
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_2_3;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_THREE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_SIX)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_THREE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_SIX)  )	
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_3_6;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_THREE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FIVE)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_THREE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FIVE)  )	
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_3_5;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_THREE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FOUR)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_THREE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FOUR)  )	
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_3_4;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FOUR && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_SIX)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FOUR && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_SIX)  )	
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_4_6;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FOUR && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FIVE)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FOUR && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FIVE)  )	
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_4_5;
            //}
            //else if ( (horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_FIVE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_SIX)
            //    ||(horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_SECOND] == HorseDefine.HORSES_FIVE && horseInfo.m_cbHorsesRanking[HorseDefine.RANKING_FIRST] == HorseDefine.HORSES_SIX)  )	
            //{
            //    horseInfo.m_cbGameResults = HorseDefine.AREA_5_6;
            //}
            //else
            //{
            //    //ASSERT(FALSE);
            //    return false;
            //}

            if (NeedControl())
            {
                return MeetControl();
            }

            return true;
        }

        //是否需要控制
        bool NeedControl()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            if (horseInfo.m_cbCLTimes > 0 && horseInfo.m_bControl)
            {
                return true;
            }
            return false;
        }

        //是否满足控制
        bool MeetControl()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            //if ( horseInfo.m_cbCLArea != 255 && horseInfo.m_cbCLArea >= HorseDefine.AREA_1_6 && horseInfo.m_cbCLArea < HorseDefine.AREA_ALL && horseInfo.m_cbGameResults != horseInfo.m_cbCLArea )
            //{
            //    return false;
            //}

            return true;
        }


        //结果计算
        bool CalculateScore()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            //玩家输赢
            int[] lPlayerLost = new int[GameDefine.GAME_PLAYER];

            //ZeroMemory(lPlayerLost);
            ZeroMemory(horseInfo.m_lPlayerWinning);
            ZeroMemory(horseInfo.m_lPlayerReturnBet);
            ZeroMemory(horseInfo.m_lPlayerRevenue);
            ZeroMemory(horseInfo.m_lPlayerBetWin);

            ZeroMemory(horseInfo.m_lUserWinScore);

            //系统输赢
            int lSystemScore = 0;
            int nBuyerCount = 0;

            //计算积分
            for (int wChairID = 0; wChairID < horseInfo._Players.Count; wChairID++)
            {
                //获取用户
                //IServerUserItem * pIServerUserItem=m_pITableFrame->GetTableUserItem(wChairID);
                //if (pIServerUserItem==NULL) continue;

                bool bIsAndroidUser = false;// pIServerUserItem->IsAndroidUser();

                // added by usc at 2014/03/17
                bool bBuyer = true;

                if (horseInfo._Players[wChairID].Auto > 0 || horseInfo._Players[wChairID].Kind != (int)UserKind.Buyer)
                    bBuyer = false;
                else
                    nBuyerCount++;
                
                ZeroMemory(lPlayerLost);

                //每个人定注数
                for (int wAreaIndex = 0; wAreaIndex < HorseDefine.AREA_ALL; ++wAreaIndex)
                {
                    if (horseInfo.m_cbHorsesRanking[0] == wAreaIndex)
                    {
                        horseInfo.m_lPlayerWinning[wChairID] += horseInfo.m_lPlayerBet[wChairID, wAreaIndex] * (horseInfo.m_nMultiple[wAreaIndex] - 1);

                        // added by usc at 2014/02/25
                        horseInfo.m_lUserWinScore[wChairID] += horseInfo.m_lPlayerBet[wChairID, wAreaIndex] * (horseInfo.m_nMultiple[wAreaIndex] - 1);

                        horseInfo.m_lPlayerReturnBet[wChairID] += horseInfo.m_lPlayerBet[wChairID, wAreaIndex];
                        horseInfo.m_lPlayerBetWin[wChairID, wAreaIndex] = horseInfo.m_lPlayerBet[wChairID, wAreaIndex] * (horseInfo.m_nMultiple[wAreaIndex] - 1);

                        //系统得分
                        if (!bIsAndroidUser && bBuyer)
                            lSystemScore -= (horseInfo.m_lPlayerBet[wChairID, wAreaIndex] * (horseInfo.m_nMultiple[wAreaIndex] - 1));
                    }
                    else if (horseInfo.m_cbHorsesRanking[1] == wAreaIndex)
                    {
                        horseInfo.m_lPlayerWinning[wChairID] += horseInfo.m_lPlayerBet[wChairID, wAreaIndex] * (horseInfo.m_nMultiple[wAreaIndex] / 2 - 1);

                        // added by usc at 2014/02/25
                        horseInfo.m_lUserWinScore[wChairID] += horseInfo.m_lPlayerBet[wChairID, wAreaIndex] * (horseInfo.m_nMultiple[wAreaIndex] / 2 - 1);

                        horseInfo.m_lPlayerReturnBet[wChairID] += horseInfo.m_lPlayerBet[wChairID, wAreaIndex];
                        horseInfo.m_lPlayerBetWin[wChairID, wAreaIndex] = horseInfo.m_lPlayerBet[wChairID, wAreaIndex] * (horseInfo.m_nMultiple[wAreaIndex] / 2 - 1);

                        //系统得分
                        if (!bIsAndroidUser && bBuyer)
                            lSystemScore -= (horseInfo.m_lPlayerBet[wChairID, wAreaIndex] * (horseInfo.m_nMultiple[wAreaIndex] / 2 - 1));
                    }
                    else
                    {
                        lPlayerLost[wChairID] -= horseInfo.m_lPlayerBet[wChairID, wAreaIndex];
                        horseInfo.m_lPlayerBetWin[wChairID, wAreaIndex] = -horseInfo.m_lPlayerBet[wChairID, wAreaIndex];

                        //系统得分
                        if (!bIsAndroidUser && bBuyer)
                            lSystemScore += horseInfo.m_lPlayerBet[wChairID, wAreaIndex];
                    }
                }

                //总的分数
                horseInfo.m_lPlayerWinning[wChairID] += lPlayerLost[wChairID];

                // added by usc at 2014/02/25
                horseInfo.m_lUserWinScore[wChairID] += lPlayerLost[wChairID];

                ////计算税收
                //if (0 < horseInfo.m_lPlayerWinning[wChairID])
                //{
                //    DOUBLE fRevenuePer = DOUBLE( (DOUBLE)m_pGameServiceOption->wRevenueRatio / (DOUBLE)1000.000000 );
                //    m_lPlayerRevenue[wChairID]  = LONGLONG(m_lPlayerWinning[wChairID] * fRevenuePer);
                //    m_lPlayerWinning[wChairID] -= m_lPlayerRevenue[wChairID];
                //}
            }

            if (NeedControl())
            {
                return CompleteControl();
            }

            if (nBuyerCount == 0)
                return true;

            //系统分值计算
            if ((horseInfo.m_StorageScore + lSystemScore) < 0)
            {
                return false;
            }
            else
            {
                if (horseInfo.m_StorageScore == HorseDefine.FIRST_EVENT_SCORE)
                    horseInfo.m_StorageScore -= HorseDefine.FIRST_EVENT_SCORE;

                horseInfo.m_StorageScore += lSystemScore;
                horseInfo.m_StorageScore -= (horseInfo.m_StorageScore * horseInfo.m_StorageDeduct / 1000);
                return true;
            }
        }

        //完成控制
        bool CompleteControl()
        {
            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            horseInfo.m_cbCLTimes--;

            if (horseInfo.m_cbCLTimes == 0)
            {
                horseInfo.m_cbCLTimes = 0;
                horseInfo.m_bControl = false;
                horseInfo.m_cbCLArea = 255;

                for (int i = 0; i < HorseDefine.AREA_ALL; ++i)
                    horseInfo.m_nCLMultiple[i] = -1;
            }
            return true;
        }

    }

    public class HorseEndRound : EndRound
    {
        Random _random = new Random();

        public override void Start()
        {

            HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

            // deleted by usc at 2014/02/25
            /*
            //写分
            for (int wChairID = 0; wChairID < horseInfo._Players.Count; ++wChairID)
            {
                //IServerUserItem* pIServerUserItem = m_pITableFrame->GetTableUserItem(wChairID);
                //if (pIServerUserItem == NULL)
                //    continue;

                if (horseInfo.m_lPlayerWinning[wChairID] != 0)
                {
                    //tagScoreInfo ScoreInfo;
                    //ZeroMemory(&ScoreInfo, sizeof(tagScoreInfo));
                    //ScoreInfo.cbType = (m_lPlayerWinning[wChairID] > 0L) ? SCORE_TYPE_WIN : SCORE_TYPE_LOSE;
                    horseInfo.m_lUserWinScore[wChairID] = horseInfo.m_lPlayerWinning[wChairID];

                    //写入积分
                    //m_pITableFrame->WriteUserScore(wChairID, ScoreInfo);
                }
                else
                {
                    horseInfo.m_lUserWinScore[wChairID] = 0;
                }
            }
            */


            //如有控制开启控制
            if (horseInfo.m_cbCLTimes > 0)
                horseInfo.m_bControl = true;

            // modified by usc at 2014/01/07
            /*
            //倍数获得
            MultipleControl();
             * */

            //设置时间
            horseInfo.m_dwGameTime = 0;// (DWORD)time(NULL);
            //_GameTable.AddGameTimer(_TimerId, horseInfo.m_nFreeTime);

            //发送消息
            //CMD_S_HorsesEnd stHorsesEnd;
            //ZeroMemory(&stHorsesEnd, sizeof(stHorsesEnd));
            //stHorsesEnd.nTimeLeave = m_nFreeTime;
            //stHorsesEnd.RecordRecord = m_GameRecords[m_GameRecords.GetCount() - 1];
            //memcpy(stHorsesEnd.nWinCount, m_nWinCount, sizeof(stHorsesEnd.nWinCount));
            //memcpy(stHorsesEnd.nMultiple, m_nMultiple, sizeof(stHorsesEnd.nMultiple));
            //m_pITableFrame->SendLookonData(INVALID_CHAIR, SUB_S_HORSES_END, &stHorsesEnd, sizeof(stHorsesEnd));

            //游戏玩家
            //for (WORD wChairID = 0; wChairID < GAME_PLAYER; ++wChairID)
            //{
            //    IServerUserItem* pIServerUserItem = m_pITableFrame->GetTableUserItem(wChairID);
            //    if (pIServerUserItem == NULL) continue;

            //    //设置积分
            //    memcpy(stHorsesEnd.lPlayerBet, m_lPlayerBet[wChairID], sizeof(stHorsesEnd.lPlayerBet));
            //    memcpy(stHorsesEnd.lPlayerWinning, m_lPlayerBetWin[wChairID], sizeof(stHorsesEnd.lPlayerWinning));

            //    m_pITableFrame->SendTableData(wChairID, SUB_S_HORSES_END, &stHorsesEnd, sizeof(stHorsesEnd));
            //}

            //结束游戏
            //m_pITableFrame->ConcludeGame(GS_FREE);

            // modified by usc at 2014/03/04
            //horseInfo.m_EndingTime = horseInfo.m_nFreeTime;            
            horseInfo.m_EndingTime = horseInfo.m_nHorsesEndTime;

            base.Start();
        }

        public override void CheckWinner()
        {
        }

        ////是否需要控制
        //bool NeedControl()
        //{
        //    HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

        //    if (horseInfo.m_cbCLTimes > 0 && horseInfo.m_bControl)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        ////倍数控制
        //void MultipleControl()
        //{
        //    HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

        //    //倍数控制
        //    //----------------------------
        //    //INT nControl = GetPrivateProfileInt(m_szGameRoomName, TEXT("MultipleControl"), 0, m_szConfigFileName);
        //    //m_bMultipleControl == nControl == 1 ? TRUE : FALSE;

        //    //需要控制
        //    //if(m_bMultipleControl)
        //    //{
        //    //    m_nMultiple[AREA_1_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_6"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_1_5] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_5"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_1_4] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_4"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_1_3] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_3"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_1_2] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_1_2"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_2_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_2_6"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_2_5] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_2_5"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_2_4] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_2_4"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_2_3] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_2_3"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_3_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_3_6"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_3_5] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_3_5"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_3_4] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_3_4"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_4_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_4_6"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_4_5] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_4_5"), 1, m_szConfigFileName);
        //    //    m_nMultiple[AREA_5_6] = GetPrivateProfileInt(m_szGameRoomName, TEXT("AREA_5_6"), 1, m_szConfigFileName);

        //    //    for ( int i = 0;  i < AREA_ALL; ++i)
        //    //    {
        //    //        if ( m_nMultiple[i] < 1 )
        //    //        {
        //    //            m_nMultiple[i] = 1;
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //        //随机换倍数
        //        RandomMultiples();
        //    //}

        //    //管理员控制
        //    if ( NeedControl() )
        //    {
        //        for ( int i = 0; i < HorseDefine.AREA_ALL; ++i)
        //        {
        //            if ( horseInfo.m_nCLMultiple[i] > 0 )
        //            {
        //                horseInfo.m_nMultiple[i] = horseInfo.m_nCLMultiple[i];
        //            }
        //        }
        //    }
        //}

        //int GetTickCount()
        //{
        //    return 0;
        //    //return System.Environment.TickCount;
        //}

        //int rand()
        //{
        //    return _random.Next();
        //}

        //static int nStFluc = 1;				

        //随机获得倍数
        //void RandomMultiples()
        //{
        //    HorseInfo horseInfo = (HorseInfo)_GameTable._TableInfo;

        //    int wTick = 0;// GetTickCount();
        //    int[] nMultiples	=	new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 15, 16, 17, 18, 18 };//倍数
        //    int[] nChance =	 new int[]{ 6, 6, 6, 6, 6, 6, 6, 4,  4,  3,  3,  3,  2,  2,  2,  1,  1 };	//几率
        //    int[] nBigAreaMultiples = new int[]{ 0, 1, 2, 3, 4, 5 };							//大倍几率

        //    //混乱
        //    ChaosArray(nBigAreaMultiples, nBigAreaMultiples.Length);
        //    ChaosArray(nMultiples, nMultiples.Length, nChance, nChance.Length);

        //    //大倍率
        //    int nBigArea = nBigAreaMultiples[(rand() + wTick)%nBigAreaMultiples.Length];
        //    horseInfo.m_nMultiple[nBigArea] = 20;

        //    //随机倍数
        //    for ( int i = 0; i < HorseDefine.AREA_ALL; ++i )
        //    {
        //        if ( i == nBigArea )
        //            continue;

        //        //几率和值
        //        int nChanceAndValue = 0;
        //        for ( int n = 0; n < nChance.Length; ++n )
        //            nChanceAndValue += nChance[n];

        //        int nMuIndex = 0;
        //        int nRandNum = 0;					//随机辅助
        //        nRandNum = (rand() + wTick + nStFluc*3 + i) % nChanceAndValue;
        //        for (int j = 0; j < nChance.Length; j++)
        //        {
        //            nRandNum -= nChance[j];
        //            if (nRandNum < 0)
        //            {
        //                nMuIndex = j;
        //                break;
        //            }
        //        }
        //        nStFluc = nStFluc%3 + 1;

        //        horseInfo.m_nMultiple[i] = nMultiples[nMuIndex];
        //        nChance[nMuIndex] = 0;
        //    }
        //}

        //混乱数组
        //void ChaosArray( int[] nArray, int nCount )
        //{
        //    int wTick = GetTickCount();
        //    for (int i = 0; i < nCount; ++i)
        //    {
        //        int nTempIndex = (rand()+wTick)%nCount;
        //        int nTempValue = nArray[i];
        //        nArray[i] = nArray[nTempIndex];
        //        nArray[nTempIndex] = nTempValue;
        //    }
        //}

        ////混乱数组
        //void ChaosArray( int[] nArrayOne, int nCountOne, int[] nArrayTwo, int nCountTwo )
        //{
        //    //ASSERT( nCountOne == nCountTwo );
        //    if( nCountTwo != nCountOne )
        //        return;

        //    int wTick = GetTickCount();
        //    for (int i = 1; i < nCountOne; ++i)
        //    {
        //        int nTempIndex = (rand()+wTick)%nCountOne;

        //        int nTempValueOne = nArrayOne[i];
        //        nArrayOne[i] = nArrayOne[nTempIndex];
        //        nArrayOne[nTempIndex] = nTempValueOne;

        //        int nTempValueTwo = nArrayTwo[i];
        //        nArrayTwo[i] = nArrayTwo[nTempIndex];
        //        nArrayTwo[nTempIndex] = nTempValueTwo;
        //    }
        //}

        public override void CheckScore()
        {
        }
    }

}
