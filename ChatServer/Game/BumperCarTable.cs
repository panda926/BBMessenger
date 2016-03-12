using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;

namespace ChatServer
{
    class BumperCarTable : GameTable
    {
        public BumperCarTable(string tableId)
        {
            _TableInfo = new BumperCarInfo();
            _TableInfo._TableId = tableId;

            _Rounds.Add(new BumperCarReadyRound());
            _Rounds.Add(new BumperCarBetRound());
            _Rounds.Add(new BumperCarEndRound());

            for (int i = 0; i < _Rounds.Count; i++)
                _Rounds[i]._GameTable = this;

            _TableInfo.m_lMinScore = 100;
        }

        public override bool PlayerOutTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_TableInfo;
            int wChairID = GetPlayerIndex(userInfo);

			//闲家判断
			if (bumperCarInfo.m_wCurrentBanker!= wChairID)
			{
				//变量定义
				int lAllScore=0;

				//统计成绩
				for (int nAreaIndex=1; nAreaIndex<=BumperCarDefine.AREA_COUNT; ++nAreaIndex) 
                    lAllScore += bumperCarInfo.m_lUserJettonScore[nAreaIndex,wChairID];

                // added by usc at 2014/03/04
                for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
                {
                    // deleted by usc at 2014/04/03
                    //bumperCarInfo.m_lAllJettonScore[i + 1] -= bumperCarInfo.m_lUserJettonScore[i + 1, wChairID];
                    bumperCarInfo.m_lUserJettonScore[i + 1, wChairID] = 0;
                }

                if (lAllScore > 0)
                {
                    bumperCarInfo.m_lUserWinScore[wChairID] = -lAllScore;

                    Cash.GetInstance().ProcessGameCash(wChairID, _GameInfo, _TableInfo);
                }

                for (int i = wChairID; i < _TableInfo._Players.Count; i++)
                {
                    for (int k = 0; k < BumperCarDefine.AREA_COUNT; k++)
                        bumperCarInfo.m_lUserJettonScore[k,i] = bumperCarInfo.m_lUserJettonScore[k, i + 1];

                    bumperCarInfo.m_lUserRevenue[i] = bumperCarInfo.m_lUserRevenue[i + 1];
                    bumperCarInfo.m_lUserWinScore[i] = bumperCarInfo.m_lUserWinScore[i + 1];

                    // added by usc at 2014/04/09
                    bumperCarInfo.m_lUserBetScore[i] = bumperCarInfo.m_lUserBetScore[i + 1];
                }  
			}

            return base.PlayerOutTable(baseInfo, userInfo);
        }
    }

    public class BumperCarReadyRound : ReadyRound
    {
        public override int GetNeedPlayers()
        {
            return 1;
        }

        public override void Start()
        {
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;

            //设置时间
            _TimerId = BumperCarDefine.IDI_FREE;
            _GameTable.AddGameTimer(_TimerId, bumperCarInfo.m_cbFreeTime);

            base.Start();
        }
    }

    public class BumperCarBetRound : StartRound
    {
        Random _random = new Random();

        public override void Start()
        {
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;

            //设置时间
            //bumperCarInfo.m_dwJettonTime = (DWORD)time(null);

            _TimerId = BumperCarDefine.IDI_PLACE_JETTON;
            _GameTable.AddGameTimer(_TimerId, bumperCarInfo.m_cbBetTime);

            base.Start();
        }

        public override void InitTableData(TableInfo tableInfo)
        {
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;

            //总下注数
            ZeroMemory(bumperCarInfo.m_lAllJettonScore);

            //个人下注
            ZeroMemory(bumperCarInfo.m_lUserJettonScore);

            //玩家成绩	
            ZeroMemory(bumperCarInfo.m_lUserWinScore);
            ZeroMemory(bumperCarInfo.m_lUserReturnScore);
            ZeroMemory(bumperCarInfo.m_lUserRevenue);

            //机器人控制
            bumperCarInfo.m_nChipRobotCount = 0;
            ZeroMemory(bumperCarInfo.m_lRobotAreaScore);

            // added by usc at 2014/04/09
            ZeroMemory(bumperCarInfo.m_lUserBetScore);
        }

        public override void AutoAction(UserInfo userInfo)
        {
            // modified by usc at 2014/02/27
            int bettingCount = _random.Next(3, 10);

            for (int i = 0; i < bettingCount; i++)
            {
                int delay = _random.Next(2, BumperCarDefine.TIME_PLACE_JETTON - 5);

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
                                                100000*/};

            // added by usc at 2014/02/27
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable.GetTableInfo();

            int[] aAreaMultiple = { 40, 30, 20, 10, 5, 5, 5, 5 };

            CurBettingInfo curBettingInfo = GetCurBettingInfo(bumperCarInfo);

            int nBettingArea = curBettingInfo.nMinArea;
            int nMinScore = curBettingInfo.nMinScore;
            int nMaxArea = curBettingInfo.nMaxArea;
            int nMaxScore = curBettingInfo.nMaxScore;

            int nScore = 0;

            if (nMaxScore <= 1000 * aAreaMultiple[nMaxArea - 1])
            {
                if (nMaxScore == 0)
                    nScore = (int)(Math.Pow(10, (double)(_random.Next() % 2) + 2));
                else
                    nScore = bettingScores[_random.Next() % bettingScores.Length];

                nBettingArea = _random.Next() % BumperCarDefine.AREA_COUNT + 1;
            }
            else
            {
                int nReaptCnt = 0;

                while (true)
                {
                    nReaptCnt++;

                    nScore = bettingScores[_random.Next() % bettingScores.Length];

                    if (nMinScore + nScore * aAreaMultiple[nBettingArea - 1] <= nMaxScore + nMaxScore * _random.Next(25, 75) / 100)
                        break;

                    if (nReaptCnt > 20)
                        break;
                }
            }

            BettingInfo bettingInfo = new BettingInfo();

            bettingInfo._Area = nBettingArea;
            bettingInfo._Score = nScore;
            bettingInfo._UserIndex = _GameTable.GetPlayerIndex(gameTimer.autoInfo);

            Action(NotifyType.Request_Betting, bettingInfo, gameTimer.autoInfo);

        }

        // added by usc at 2014/02/27
        private CurBettingInfo GetCurBettingInfo(BumperCarInfo bumperCarInfo)
        {
            CurBettingInfo curBettingInfo = new CurBettingInfo();

            int[] aAreaScore = new int[BumperCarDefine.AREA_COUNT];
            int[] aAreaMultiple = { 40, 30, 20, 10, 5, 5, 5, 5 };

            for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
            {
                aAreaScore[i] = bumperCarInfo.m_lAllJettonScore[i+1] * aAreaMultiple[i];
            }


            int nMinArea = 1;
            int nMinScore = aAreaScore[0];
            int nMaxArea = 1;
            int nMaxScore = aAreaScore[0];

            for (int i = 1; i < BumperCarDefine.AREA_COUNT; i++)
            {
                if (aAreaScore[i] > nMaxScore)
                {
                    nMaxArea = i+1;
                    nMaxScore = aAreaScore[i];
                }

                if (aAreaScore[i] < nMinScore)
                {
                    nMinArea = i+1;
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
	                    if ( !(baseInfo is BettingInfo)) 
                            return false;

	                    //用户效验
	                    if ( _GameTable.GetPlayerIndex( userInfo ) < 0 ) 
                            return false;

                        BettingInfo bettingInfo = (BettingInfo)baseInfo;

                        if (OnUserPlaceJetton(userInfo, bettingInfo._Area, bettingInfo._Score) == false)
                            return false;

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
            if( this._IsLeaveTime == false )
                return false;

            return true;
        }

        //加注事件
        bool OnUserPlaceJetton(UserInfo userInfo, int cbJettonArea, int lJettonScore)
        {
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;
            int wChairID = _GameTable.GetPlayerIndex(userInfo);

	        //效验参数
	        if ((cbJettonArea>BumperCarDefine.AREA_COUNT)||(lJettonScore<=0L) || cbJettonArea<1)
		        return false;

	        //庄家判断
	        if (bumperCarInfo.m_wCurrentBanker==wChairID) 
                return true;
	        if (bumperCarInfo.m_bEnableSysBanker==false && bumperCarInfo.m_wCurrentBanker==GameDefine.INVALID_CHAIR) 
                return true;

	        //变量定义
	        int lJettonCount=0;
	        for (int nAreaIndex=1; nAreaIndex<=BumperCarDefine.AREA_COUNT; ++nAreaIndex) 
                lJettonCount += bumperCarInfo.m_lUserJettonScore[nAreaIndex,wChairID];

	        //玩家积分
            int nGameMoney = userInfo.GetGameMoney();

	        //合法校验
        if (nGameMoney < lJettonCount + lJettonScore)
        {
            BaseInfo.SetError(ErrorType.Notenough_Cash, "베팅할 금액이 부족합니다");
            return false;
        }

	        //if (bumperCarInfo.m_lUserLimitScore < lJettonCount + lJettonScore) return true;

	        //成功标识
	        bool bPlaceJettonSuccess=true;

	        //合法验证
            //if (GetUserMaxJetton(userInfo,cbJettonArea) >= lJettonScore)
            //{
		        //机器人验证
                //if(userInfo.Auto > 0 )
                //{
                //    //区域限制
                //    // deleted by usc at 2014/02/24
                //    //if (bumperCarInfo.m_lRobotAreaScore[cbJettonArea] + lJettonScore > bumperCarInfo.m_lRobotAreaLimit)
                //    //    return true;

                //    //数目限制
                //    //bool bHaveChip = false;
                //    //for (int i = 0; i < BumperCarDefine.AREA_COUNT; i++)
                //    //{
                //    //    if (bumperCarInfo.m_lUserJettonScore[i + 1, wChairID] > 0)
                //    //    {
                //    //        bHaveChip = true;
                //    //        break;
                //    //    }
                //    //}

                //    //if (!bHaveChip)
                //    //{
                //    //    if (bumperCarInfo.m_nChipRobotCount+1 > bumperCarInfo.m_nMaxChipRobot)
                //    //    {
                //    //        bPlaceJettonSuccess = false;
                //    //    }
                //    //    else
                //    //        bumperCarInfo.m_nChipRobotCount++;
                //    //}

                //    //统计分数
                //    if (bPlaceJettonSuccess)
                //        bumperCarInfo.m_lRobotAreaScore[cbJettonArea] += lJettonScore;
                //}

		        if (bPlaceJettonSuccess)
		        {
			        //保存下注
			        bumperCarInfo.m_lAllJettonScore[cbJettonArea] += lJettonScore;
			        bumperCarInfo.m_lUserJettonScore[cbJettonArea,wChairID] += lJettonScore;
                 
                 // added by usc at 2014/04/09
                 bumperCarInfo.m_lUserBetScore[wChairID] += lJettonScore;

                 return true;
		        }	
            //}

	        return false;
        }

        void   GetAllWinArea(int[] bcWinArea,int bcAreaCount,int InArea)
        {
	        if (InArea==0xFF)
		        return ;

	        ZeroMemory(bcWinArea);

	        int lMaxSocre = 0;
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;

	        for (int i = 0;i<32;i++)
	        {
		        int[] bcOutCadDataWin = new int[BumperCarDefine.AREA_COUNT];

		        BumperLogic.GetCardType(i+1,1,bcOutCadDataWin);

		        for (int j= 0;j<BumperCarDefine.AREA_COUNT;j++)
		        {

			        if(bcOutCadDataWin[j]>1&&j==InArea-1)
			        {
				        int Score = 0; 
				        for (int nAreaIndex=1; nAreaIndex<=BumperCarDefine.AREA_COUNT; ++nAreaIndex) 
				        {
					        if(bcOutCadDataWin[nAreaIndex-1]>1)
					        {
						        Score += bumperCarInfo.m_lAllJettonScore[nAreaIndex]*(bcOutCadDataWin[nAreaIndex-1]);
					        }
				        }
				        if(Score>=lMaxSocre)
				        {
					        lMaxSocre = Score;
					        bcWinArea = bcOutCadDataWin;

				        }
				        break;
			        }
		        }
	        }
        }

        //最大下注
        int GetUserMaxJetton(UserInfo userInfo,int Area)
        {
            int wChairID = _GameTable.GetPlayerIndex(userInfo);
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;

	        //已下注额
	        int lNowJetton = 0;
	        //ASSERT(BumperCarDefine.AREA_COUNT<=CountArray(bumperCarInfo.m_lUserJettonScore));
	        for (int nAreaIndex=1; nAreaIndex<=BumperCarDefine.AREA_COUNT; ++nAreaIndex) 
                lNowJetton += bumperCarInfo.m_lUserJettonScore[nAreaIndex,wChairID];

	        //庄家金币
	        int lBankerScore=0x7fffffff;
	        if (bumperCarInfo.m_wCurrentBanker!=GameDefine.INVALID_CHAIR)
	        {
		        userInfo = _GameTable._TableInfo._Players[bumperCarInfo.m_wCurrentBanker];
		        
                if (null!=userInfo) 
                    lBankerScore=userInfo.GetGameMoney();
	        }

	        int[] bcWinArea = new int[BumperCarDefine.AREA_COUNT];
	        int LosScore = 0;
	        int WinScore = 0;

	        GetAllWinArea(bcWinArea,BumperCarDefine.AREA_COUNT,Area);

	        for (int nAreaIndex=1; nAreaIndex<=BumperCarDefine.AREA_COUNT; ++nAreaIndex) 
	        {
		        if(bcWinArea[nAreaIndex-1]>1)
		        {
			        LosScore+=bumperCarInfo.m_lAllJettonScore[nAreaIndex]*(bcWinArea[nAreaIndex-1]);
		        }else
		        {
			        if(bcWinArea[nAreaIndex-1]==0)
			        {
				        WinScore+=bumperCarInfo.m_lAllJettonScore[nAreaIndex];

			        }
		        }
	        }
	        lBankerScore = lBankerScore + WinScore - LosScore;

	        if ( lBankerScore < 0 )
	        {
		        if (bumperCarInfo.m_wCurrentBanker!=GameDefine.INVALID_CHAIR)
		        {
			        userInfo = _GameTable._TableInfo._Players[ bumperCarInfo.m_wCurrentBanker ];

			        if (null!=userInfo) 
				        lBankerScore=userInfo.GetGameMoney();
		        }
		        else
		        {
			        lBankerScore = 0x7fffffff;
		        }
	        }

	        //个人限制
	        int lMeMaxScore = Math.Min((userInfo.GetGameMoney()-lNowJetton), bumperCarInfo.m_lUserLimitScore);

	        //区域限制
	        lMeMaxScore=Math.Min(lMeMaxScore,bumperCarInfo.m_lAreaLimitScore);

	        int[] diMultiple = new int[BumperCarDefine.AREA_COUNT];

	        for (int i = 0;i<32;i++)
	        {
	           int[]  bcOutCadDataWin = new int[BumperCarDefine.AREA_COUNT];
	           BumperLogic.GetCardType(i+1,1,bcOutCadDataWin);
		           
                for (int j = 0;j<BumperCarDefine.AREA_COUNT;j++)
                {
	               if(bcOutCadDataWin[j]>1)
	               {
		               diMultiple[j] = bcOutCadDataWin[j];

	               }
                }
	        }
	        //庄家限制
	        lMeMaxScore=Math.Min(lMeMaxScore,lBankerScore/(diMultiple[Area-1]));

	        //非零限制
	        //ASSERT(lMeMaxScore >= 0);
	        lMeMaxScore = Math.Max(lMeMaxScore, 0);

	        return (int)(lMeMaxScore);
        }
    }

    public class BumperCarEndRound : EndRound
    {
        Random _random = new Random();

        public override void CheckWinner()
        {
            int nRepeatCount = 0;

            while (true)
            {
                nRepeatCount++;

                //派发扑克
                DispatchTableCard();
                //试探性判断
                if (ProbeJudge() || nRepeatCount > 20)
                    break;                
            }
        }

        public override void CheckScore()
        {
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;

            //计算分数
            int lBankerWinScore = CalculateScore();

            //递增次数
            bumperCarInfo.m_wBankerTime++;

            //m_pITableFrame->WriteTableScore(ScoreInfo, CountArray(ScoreInfo));
        }

        //发送扑克
        static int nStFluc = 1;				

        bool DispatchTableCard()
        {
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;

            int[] cbControlArea = new int[] { 1, 9, 17, 25, 3, 11, 19, 27, 5, 13, 21, 29, 7, 15, 23, 31, 2, 10, 18, 26, 4, 12, 20, 28, 6, 14, 22, 30, 8, 16, 24, 32 };
            int[] cbnChance = new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };

            BumperLogic.ChaosArray(cbControlArea, cbControlArea.Length, cbnChance, cbnChance.Length);

            //随机倍数
            int wTick = 0;// System.Environment.TickCount;

            //几率和值
            int nChanceAndValue = 0;
            for (int n = 0; n < cbnChance.Length; ++n)
                nChanceAndValue += cbnChance[n];

            int nMuIndex = 0;
            int nRandNum = 0;					//随机辅助

            nRandNum = (_random.Next() + wTick + nStFluc * 3) % nChanceAndValue;
            for (int j = 0; j < cbnChance.Length; j++)
            {
                nRandNum -= cbnChance[j];
                if (nRandNum < 0)
                {
                    nMuIndex = j;
                    break;
                }
            }
            nStFluc = nStFluc % 3 + 1;

            bumperCarInfo.m_cbTableCardArray[0, 0] = cbControlArea[nMuIndex];
            bumperCarInfo.m_cbCardCount[0] = 1;

            //发牌标志
            bumperCarInfo.m_bContiueCard = false;

            return true;
        }

        //计算得分
        static int cbRevenue = 0;//m_pGameServiceOption->lServiceScore;
        static bool bWinTianMen, bWinDiMen, bWinXuanMen, bWinHuang;

        int CalculateScore()
        {
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;

	        //变量定义

	        //推断玩家
	        int TianMultiple,diMultiple,TianXuanltiple,HuangMultiple;
	        TianMultiple  = 1;
	        diMultiple = 1 ;
	        TianXuanltiple = 1;
	        HuangMultiple = 1;

            int[]  bcResulteOut = new int[BumperCarDefine.AREA_COUNT];
	        
	        BumperLogic.GetCardType(bumperCarInfo.m_cbTableCardArray[0,0],1,bcResulteOut);

	        //游戏记录
	        //tagServerGameRecord &GameRecord = m_GameRecordArrary[bumperCarInfo.m_nRecordLast];

	        int[]  cbMultiple = new int[BumperCarDefine.AREA_COUNT];
            
            for( int i = 0; i < cbMultiple.Length; i++ )
                cbMultiple[i] = 1;

            //for (int wAreaIndex = 1; wAreaIndex <= BumperCarDefine.AREA_COUNT; ++wAreaIndex)
            //{

            //    if(bcResulteOut[wAreaIndex-1]>0)
            //    {
            //        GameRecord.bWinMen[wAreaIndex-1] = 4;
            //    }
            //    else
            //    {
            //        GameRecord.bWinMen[wAreaIndex-1] = 0;
            //    }
            //}

	        //移动下标
            //bumperCarInfo.m_nRecordLast = (bumperCarInfo.m_nRecordLast+1) % MAX_SCORE_HISTORY;
            //if ( bumperCarInfo.m_nRecordLast == bumperCarInfo.m_nRecordFirst ) bumperCarInfo.m_nRecordFirst = (bumperCarInfo.m_nRecordFirst+1) % MAX_SCORE_HISTORY;

	        //庄家总量
	        int lBankerWinScore = 0;

	        //玩家成绩
	        ZeroMemory(bumperCarInfo.m_lUserWinScore);
	        ZeroMemory(bumperCarInfo.m_lUserReturnScore);
	        ZeroMemory(bumperCarInfo.m_lUserRevenue);

	        int[] lUserLostScore = new int[BumperCarDefine.GAME_PLAYER];

	        //玩家下注
	        int[,] pUserScore = bumperCarInfo.m_lUserJettonScore;

	        //计算积分
	        for (int wChairID=0; wChairID<_GameTable._TableInfo._Players.Count; wChairID++)
	        {
		        //庄家判断
		        if (bumperCarInfo.m_wCurrentBanker==wChairID) 
                    continue;

		        //获取用户
		        UserInfo userInfo = _GameTable._TableInfo._Players[wChairID];

		        for (int wAreaIndex = 1; wAreaIndex <= BumperCarDefine.AREA_COUNT; ++wAreaIndex)
		        {

			        if (bcResulteOut[wAreaIndex-1]>0) 
			        {
				        bumperCarInfo.m_lUserWinScore[wChairID] += ( pUserScore[wAreaIndex,wChairID] *(bcResulteOut[wAreaIndex-1])) ;
				        bumperCarInfo.m_lUserReturnScore[wChairID] += pUserScore[wAreaIndex,wChairID] ;
				        lBankerWinScore -= ( pUserScore[wAreaIndex,wChairID] * (bcResulteOut[wAreaIndex-1]) ) ;
			        }
			        else
			        {
				        if (bcResulteOut[wAreaIndex-1]==0)
				        {
					        lUserLostScore[wChairID] -= pUserScore[wAreaIndex,wChairID];
					        lBankerWinScore += pUserScore[wAreaIndex,wChairID];
				        }
				        else
				        {
					        //如果为1则不少分
					        bumperCarInfo.m_lUserWinScore[wChairID] += 0;
					        bumperCarInfo.m_lUserReturnScore[wChairID] += pUserScore[wAreaIndex,wChairID] ;
				        }
			        }
		        }

		        //计算税收
		        if (0 < bumperCarInfo.m_lUserWinScore[wChairID])
		        {
			        float fRevenuePer=(float)cbRevenue/1000;
			        bumperCarInfo.m_lUserRevenue[wChairID]  = (int)(bumperCarInfo.m_lUserWinScore[wChairID]*fRevenuePer);
			        bumperCarInfo.m_lUserWinScore[wChairID] -= bumperCarInfo.m_lUserRevenue[wChairID];
		        }

		        //总的分数
		        bumperCarInfo.m_lUserWinScore[wChairID] += lUserLostScore[wChairID];
	        }

	        //庄家成绩
	        if (bumperCarInfo.m_wCurrentBanker!=GameDefine.INVALID_CHAIR)
	        {
		        bumperCarInfo.m_lUserWinScore[bumperCarInfo.m_wCurrentBanker] = lBankerWinScore;

		        //计算税收
		        if (0 < bumperCarInfo.m_lUserWinScore[bumperCarInfo.m_wCurrentBanker])
		        {
			        float fRevenuePer=(float)cbRevenue/1000;
			        bumperCarInfo.m_lUserRevenue[bumperCarInfo.m_wCurrentBanker]  = (int)(bumperCarInfo.m_lUserWinScore[bumperCarInfo.m_wCurrentBanker]*fRevenuePer);
			        bumperCarInfo.m_lUserWinScore[bumperCarInfo.m_wCurrentBanker] -= bumperCarInfo.m_lUserRevenue[bumperCarInfo.m_wCurrentBanker];
			        lBankerWinScore = bumperCarInfo.m_lUserWinScore[bumperCarInfo.m_wCurrentBanker];
		        }	
	        }

	        //累计积分
	        bumperCarInfo.m_lBankerWinScore += lBankerWinScore;

	        //当前积分
	        bumperCarInfo.m_lBankerCurGameScore=lBankerWinScore;

	        return lBankerWinScore;
        }

        //试探性判断
        bool ProbeJudge()
        {
            BumperCarInfo bumperCarInfo = (BumperCarInfo)_GameTable._TableInfo;

	        int[]  bcResulteOut = new int[BumperCarDefine.AREA_COUNT];
	        
	        BumperLogic.GetCardType(bumperCarInfo.m_cbTableCardArray[0,0],1,bcResulteOut);

	        //系统输赢
	        int lSystemScore = 0;
            int nBuyerCount = 0;

	        //玩家下注
            int[,] pUserScore = bumperCarInfo.m_lUserJettonScore;

	        //庄家是不是机器人
	        bool bIsBankerAndroidUser = false;
            //if ( bumperCarInfo.m_wCurrentBanker != GameDefine.INVALID_CHAIR )
            //{
            //    UserInfo userInfo = _GameTable._TableInfo._Players[bumperCarInfo.m_wCurrentBanker];
            //    if (userInfo != null) 
            //    {
            //        bIsBankerAndroidUser = pIBankerUserItem->IsAndroidUser();
            //    }
            //}

	        //计算积分
            for (int wChairID = 0; wChairID < bumperCarInfo._Players.Count; wChairID++)
	        {
		        //庄家判断
		        if (bumperCarInfo.m_wCurrentBanker == wChairID) 
                    continue;

		        //获取用户
                UserInfo userInfo = bumperCarInfo._Players[wChairID];

                if (userInfo == null)
                    return false;

                // added by usc at 2014/03/17
                bool bBuyer = true;

                if (userInfo.Auto > 0 || userInfo.Kind != (int)UserKind.Buyer)
                    bBuyer = false;
                else
                    nBuyerCount++;

		        for (int wAreaIndex = 1; wAreaIndex <= BumperCarDefine.AREA_COUNT; ++wAreaIndex)
		        {
			        if (bcResulteOut[wAreaIndex - 1] > 0) 
			        {
				        if ( !bBuyer )
					        lSystemScore += (pUserScore[wAreaIndex,wChairID] *(bcResulteOut[wAreaIndex-1]));

				        if (bumperCarInfo.m_wCurrentBanker == GameDefine.INVALID_CHAIR || bIsBankerAndroidUser)
					        lSystemScore -= (pUserScore[wAreaIndex,wChairID] *(bcResulteOut[wAreaIndex-1]));
			        }
                    else if (bcResulteOut[wAreaIndex - 1] == 0)
			        {
					    if ( !bBuyer )
						    lSystemScore -= pUserScore[wAreaIndex,wChairID];

					    if (bumperCarInfo.m_wCurrentBanker == GameDefine.INVALID_CHAIR || bIsBankerAndroidUser)
						    lSystemScore += pUserScore[wAreaIndex,wChairID];
				        
			        }
		        }
	        }

            // added by usc at 2014/03/21
            if (nBuyerCount == 0)
                return true;

	        //系统分值计算
	        if (bumperCarInfo.m_StorageScore + lSystemScore < 0)
	        {
		        return false;
	        }
	        else
	        {
                if (bumperCarInfo.m_StorageScore == HorseDefine.FIRST_EVENT_SCORE)
                    bumperCarInfo.m_StorageScore -= HorseDefine.FIRST_EVENT_SCORE;

		        bumperCarInfo.m_StorageScore += lSystemScore;
                bumperCarInfo.m_StorageScore -= (bumperCarInfo.m_StorageScore * bumperCarInfo.m_StorageDeduct / 1000);
		        return true;
	        }
        }

    }

}
