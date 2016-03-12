using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;

namespace ChatServer
{
    class DzCardTable : GameTable
    {
        public static DzCardLogic m_GameLogic = new DzCardLogic();

        public DzCardTable(string tableId)
        {
            _TableInfo = new DzCardInfo();
            _TableInfo._TableId = tableId;

            _Rounds.Add(new DzCardReadyRound());
            _Rounds.Add(new DzCardStartRound());
            _Rounds.Add(new DzCardEndRound());

            for (int i = 0; i < _Rounds.Count; i++)
                _Rounds[i]._GameTable = this;

            _TableInfo.m_lMinScore = 100;
        }

        public int GetSeatterIndex(UserInfo userInfo)
        {
            if (userInfo == null)
                return GameDefine.INVALID_CHAIR;

            DzCardInfo dzCardInfo = (DzCardInfo)_TableInfo;

            for (int i = 0; i < dzCardInfo.m_Seatter.Length; i++)
            {
                if (dzCardInfo.m_Seatter[i] == userInfo)
                    return i;
            }

            return GameDefine.INVALID_CHAIR;
        }

        public override bool PlayerEnterTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            if (!(baseInfo is UserInfo))
                return false;

            UserInfo seatInfo = (UserInfo)baseInfo;

            DzCardInfo dzCardInfo = (DzCardInfo)_TableInfo;

            if (userInfo.Auto > 0)
            {
                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                {
                    if (dzCardInfo.m_Seatter[i] == null)
                    {
                        userInfo.userSeat = i;
                        break;
                    }
                }
            }

            if (seatInfo.userSeat < 0 || seatInfo.userSeat >= DzCardDefine.GAME_PLAYER)
                return false;
            if (dzCardInfo.m_Seatter[seatInfo.userSeat] != null)
                return false;

            if (base.PlayerEnterTable(baseInfo, userInfo) == false)
                return false;

            dzCardInfo.m_Seatter[seatInfo.userSeat] = userInfo;

            return true;
        }

        public override bool PlayerOutTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            int seaterIndex = GetSeatterIndex(userInfo);

            if( seaterIndex >= 0 )
            {
                if (_Rounds[_TableInfo._RoundIndex] is DzCardStartRound)
                {
                    AddScoreInfo giveInfo = new AddScoreInfo();

                    _Rounds[_TableInfo._RoundIndex].Action(NotifyType.Request_GiveUp, giveInfo, userInfo);
                }

                DzCardInfo dzCardInfo = (DzCardInfo)_TableInfo;

                dzCardInfo.m_cbPlayStatus[seaterIndex] = false;
                dzCardInfo.m_Seatter[seaterIndex] = null;
            }

 	        return base.PlayerOutTable(baseInfo, userInfo);
        }

    }

    public class DzCardReadyRound : ReadyRound
    {
        public override int GetNeedPlayers()
        {
            return 2;
        }
    }

    public class DzCardStartRound : StartRound
    {
        Random _random = new Random();
        public bool _isEnd = false;

        public override void Start()
        {
            _isEnd = false;

            base.Start();
        }

        public override void InitTableData(TableInfo tableInfo)
        {
            DzCardInfo dzCardInfo = (DzCardInfo)tableInfo;

            dzCardInfo.cbTotalEnd = 0;
            Array.Clear(dzCardInfo.m_GameScore, 0, dzCardInfo.m_GameScore.Length);

            //玩家变量
            dzCardInfo.m_wCurrentUser = GameDefine.INVALID_CHAIR;

            //玩家状态
            Array.Clear(dzCardInfo.m_cbPlayStatus, 0, dzCardInfo.m_cbPlayStatus.Length);

            //扑克变量
            dzCardInfo.m_cbSendCardCount = 0;
            Array.Clear(dzCardInfo.m_cbCenterCardData, 0, dzCardInfo.m_cbCenterCardData.Length);
            
            for( int i = 0; i < dzCardInfo.m_cbHandCardData.Length; i++ )
                Array.Clear( dzCardInfo.m_cbHandCardData[i], 0, dzCardInfo.m_cbHandCardData[i].Length);

            //加注变量
            dzCardInfo.m_lCellScore = 0;
            dzCardInfo.m_lTurnLessScore = 0;
            dzCardInfo.m_lTurnMaxScore = 0;
            dzCardInfo.m_lAddLessScore = 0;
            dzCardInfo.m_wOperaCount = 0;
            dzCardInfo.m_cbBalanceCount = 0;
            dzCardInfo.m_lBalanceScore = 0;
            dzCardInfo.m_cbSendCardCount = 0;
            Array.Clear(dzCardInfo.m_lTableScore, 0, dzCardInfo.m_lTableScore.Length);
            Array.Clear(dzCardInfo.m_lTotalScore, 0, dzCardInfo.m_lTotalScore.Length);
            Array.Clear(dzCardInfo.m_cbShowHand, 0, dzCardInfo.m_cbShowHand.Length);

            //税收变量
            //ZeroMemory(m_bUserTax,sizeof(m_bUserTax));	
            //ZeroMemory(m_bLastTax,sizeof(m_bLastTax));	

            //游戏变量
            //int wUserCount = 0;
            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
            {
                //获取用户
                UserInfo userInfo = dzCardInfo.m_Seatter[i];

                //无效用户
                if (userInfo == null)
                    continue;

                //获取金币
                //const tagUserScore* pUserScore = pIServerUserItem->GetUserScore();
                //ASSERT(pUserScore->lScore >= m_pGameServiceOption->lCellScore);

                if(userInfo.nCashOrPointGame == 0)
                    dzCardInfo.m_lUserMaxScore[i] = userInfo.Cash;
                else
                    dzCardInfo.m_lUserMaxScore[i] = userInfo.Point;

                //设置状态
                dzCardInfo.m_cbPlayStatus[i] = true;
                //wUserCount++;
            }
            dzCardInfo.m_lCellScore = dzCardInfo.m_lMinScore/2;

            //混乱扑克
            //srand((unsigned int)time(NULL));
            int[] cbRandCard = new int[DzCardDefine.FULL_COUNT];
            Array.Clear(cbRandCard, 0, cbRandCard.Length);
            
            DzCardTable.m_GameLogic.RandCardList(cbRandCard, dzCardInfo._Players.Count * DzCardDefine.MAX_COUNT + DzCardDefine.MAX_CENTERCOUNT);


	        //用户扑克
	        int wCardCount=0;
	        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
                if (dzCardInfo.m_cbPlayStatus[i] == true)
                {
                    for (int k = 0; k < DzCardDefine.MAX_COUNT; k++)
                        dzCardInfo.m_cbHandCardData[i][k] = cbRandCard[wCardCount++];
                }
	        }

	        //中心扑克
            for( int k = 0; k < dzCardInfo.m_cbCenterCardData.Length; k++ )
                dzCardInfo.m_cbCenterCardData[k] = cbRandCard[wCardCount++];

	        //扑克数目
	        dzCardInfo.m_cbSendCardCount = 0;
	        dzCardInfo.m_cbBalanceCount = 0;

	        //首家判断
	        if(dzCardInfo.m_wDUser == GameDefine.INVALID_CHAIR)
                dzCardInfo.m_wDUser = 0;
	        else 
                dzCardInfo.m_wDUser =(dzCardInfo.m_wDUser+1)%DzCardDefine.GAME_PLAYER;

	        //盲注玩家
	        int[] wPlayer= new int[]{GameDefine.INVALID_CHAIR,GameDefine.INVALID_CHAIR,GameDefine.INVALID_CHAIR};
            int wPlayerCount = 0;
	        int wNextUser = dzCardInfo.m_wDUser;

	        do
	        {
		        if (dzCardInfo.m_cbPlayStatus[wNextUser]==true) 
		        {
			        wPlayer[wPlayerCount++] = wNextUser;	
		        }
		        wNextUser =(wNextUser+1)%DzCardDefine.GAME_PLAYER;
	        }
            while(wPlayerCount < 3);

	        dzCardInfo.m_wDUser = wPlayer[0];
	        dzCardInfo.m_wCurrentUser = wPlayer[2];

	        //当前下注
	        dzCardInfo.m_lTableScore[dzCardInfo.m_wDUser] = dzCardInfo.m_lCellScore;
	        dzCardInfo.m_lTableScore[wPlayer[1]] = 2*dzCardInfo.m_lCellScore;
	        dzCardInfo.m_lTotalScore[dzCardInfo.m_wDUser] = dzCardInfo.m_lCellScore;
	        dzCardInfo.m_lTotalScore[wPlayer[1]] = 2*dzCardInfo.m_lCellScore;

	        //设置变量
            dzCardInfo.m_lBalanceScore = 2 * dzCardInfo.m_lCellScore;
            dzCardInfo.m_lTurnMaxScore = dzCardInfo.m_lUserMaxScore[dzCardInfo.m_wCurrentUser] - dzCardInfo.m_lTotalScore[dzCardInfo.m_wCurrentUser];
            dzCardInfo.m_lTurnLessScore = dzCardInfo.m_lBalanceScore - dzCardInfo.m_lTotalScore[dzCardInfo.m_wCurrentUser];
            dzCardInfo.m_lAddLessScore = 2 * dzCardInfo.m_lCellScore + dzCardInfo.m_lTurnLessScore;

	        //构造变量
            //CMD_S_GameStart GameStart;
            //ZeroMemory(&GameStart,sizeof(GameStart));
            //GameStart.wDUser = m_wDUser;
            //GameStart.wMaxChipInUser = wPlayer[1];
            //GameStart.wCurrentUser = m_wCurrentUser;
            //GameStart.lCellScore = m_pGameServiceOption->lCellScore;
            //GameStart.lAddLessScore = m_lAddLessScore;
            //GameStart.lTurnLessScore = m_lTurnLessScore;
            //GameStart.lTurnMaxScore = m_lTurnMaxScore;

	        //作弊/漏洞数据
	        //CopyMemory(GameStart.cbAllData,m_cbHandCardData,sizeof(m_cbHandCardData));

	        //发送数据
            //for (int i=0;i<m_wPlayerCount;i++)
            //{
            //    if (m_cbPlayStatus[i]==TRUE)
            //    {
            //        //发送数据
            //        CopyMemory(GameStart.cbCardData[i],m_cbHandCardData[i],MAX_COUNT);
            //        m_pITableFrame->SendTableData(i,SUB_S_GAME_START,&GameStart,sizeof(GameStart));
            //        ZeroMemory(GameStart.cbCardData[i],MAX_COUNT);
            //    }	
            //    m_pITableFrame->SendLookonData(i,SUB_S_GAME_START,&GameStart,sizeof(GameStart));
            //}

            AutoProcess();
        }

        public override void NotifyGameTimer(GameTimer gameTimer)
        {
            if (gameTimer.timerId != TimerID.Custom || gameTimer.autoInfo == null)
                return;

            DzCardInfo dzCardInfo = (DzCardInfo)_GameTable._TableInfo;

            DzCardTable dzCardTable = (DzCardTable)_GameTable;
            int seaterIndex = dzCardTable.GetSeatterIndex(gameTimer.autoInfo);

            if (seaterIndex < 0 || seaterIndex >= DzCardDefine.GAME_PLAYER)
                return;

            int[] bestCardData = new int[DzCardDefine.MAX_CENTERCOUNT];
            int cbEndCardKind = DzCardTable.m_GameLogic.FiveFromSeven(dzCardInfo.m_cbHandCardData[seaterIndex], DzCardDefine.MAX_COUNT, dzCardInfo.m_cbCenterCardData, DzCardDefine.MAX_CENTERCOUNT, bestCardData, DzCardDefine.MAX_CENTERCOUNT);

            int actionType = _random.Next() % 4;

            if (cbEndCardKind >= 3)
            {
                actionType = _random.Next() % 4;
            }
            else
            {
                actionType = _random.Next() % 4 + 1;
            }

            switch (actionType)
            {
                case 0:     // add
                    {
                        AddScoreInfo AddScore = new AddScoreInfo();
                        //AddScore.lAddLessScore = dzCardInfo.m_lTurnMaxScore;
                        AddScore.lAddLessScore = dzCardInfo.m_lAddLessScore;

                        Action(NotifyType.Request_AddScore, AddScore, gameTimer.autoInfo);
                    }
                    break;

                case 1:     // follow
                case 2:
                case 3:
                    {
                        //发送消息
                        AddScoreInfo AddScore = new AddScoreInfo();
                        AddScore.lAddLessScore = dzCardInfo.m_lTurnLessScore;

                        Action(NotifyType.Request_AddScore, AddScore, gameTimer.autoInfo);
                    }
                    break;

                case 4:
                    {
                        AddScoreInfo AddScore = new AddScoreInfo();

                        Action(NotifyType.Request_GiveUp, AddScore, gameTimer.autoInfo);
                    }
                    break;
            }

            
        }

        public void AutoProcess()
        {
            if (CanEnd() == true)
                return;

            DzCardInfo dzCardInfo = (DzCardInfo)_GameTable._TableInfo;

            if (dzCardInfo.m_wCurrentUser == GameDefine.INVALID_CHAIR)
                return;

            if (dzCardInfo.m_cbPlayStatus[dzCardInfo.m_wCurrentUser] == false)
                return;

            UserInfo nextInfo = dzCardInfo.m_Seatter[dzCardInfo.m_wCurrentUser];

            if (nextInfo == null)
                return;

            if (nextInfo.Auto == 0)
                return;

            int delay = _random.Next() % 10 + 2;
            _GameTable.AddAutoTimer(nextInfo, delay);
        }

        public override bool Action(NotifyType notifyType, BaseInfo baseInfo, UserInfo userInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Request_AddScore:
                    {
                        AddScoreInfo addScoreInfo = (AddScoreInfo)baseInfo;

                        int chairID = ((DzCardTable)_GameTable).GetSeatterIndex(userInfo);

                        OnUserAddScore(chairID, addScoreInfo.lAddLessScore, false);

                        AutoProcess();
                    }
                    break;

                case NotifyType.Request_GiveUp:
                    {
                        AddScoreInfo addScoreInfo = (AddScoreInfo)baseInfo;

                        bool ret = OnUserGiveUp(userInfo, addScoreInfo);

                        AutoProcess();

                        return ret;
                    }
                    break;

                case NotifyType.Request_OpenCard:
                    {
                        AddScoreInfo addScoreInfo = new AddScoreInfo();
                        addScoreInfo.wAddScoreUser = ((DzCardTable)_GameTable).GetSeatterIndex(userInfo);

                        _GameTable.BroadCastGame(NotifyType.Reply_OpenCard, addScoreInfo);
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
            if( _isEnd == false )
                return false;

            return true;
        }


        bool OnUserGiveUp(UserInfo userInfo, AddScoreInfo giveInfo )
        {
            int wChairID = ((DzCardTable)_GameTable).GetSeatterIndex(userInfo);

            if (wChairID == GameDefine.INVALID_CHAIR)
                return true;

            DzCardInfo dzCardInfo = (DzCardInfo)_GameTable._TableInfo;

	        //重置状态
            dzCardInfo.m_cbPlayStatus[wChairID] = false;
            dzCardInfo.m_cbShowHand[wChairID] = false;

	        //发送消息
            AddScoreInfo GiveUp = giveInfo;
            GiveUp.wAddScoreUser = wChairID;
            GiveUp.lAddLessScore = -dzCardInfo.m_lTotalScore[wChairID];

            _GameTable.BroadCastGame(NotifyType.Reply_GiveUp, GiveUp);

	        //写入金币
	        //m_pITableFrame->WriteUserScore(wChairID,-m_lTotalScore[wChairID],0,enScoreKind_Lost);
            int playerIndex = _GameTable.GetPlayerIndex(userInfo);

            if (playerIndex >= 0)
            {
                _GameTable._TableInfo.m_lUserWinScore[playerIndex] = -dzCardInfo.m_lTotalScore[wChairID];
                Cash.GetInstance().ProcessGameCash(playerIndex, _GameTable._GameInfo, _GameTable._TableInfo);

                _GameTable._TableInfo.m_lUserWinScore[playerIndex] = 0;
            }

	        //清空下注
	        dzCardInfo.m_lTableScore[wChairID] = 0;

	        //人数统计
	        int wPlayerCount=0;
	        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        if (dzCardInfo.m_cbPlayStatus[i]==true) 
                    wPlayerCount++;
	        }

	        //判断结束
            if (wPlayerCount >= 2)
            {
                if (dzCardInfo.m_wCurrentUser == wChairID)
                    OnUserAddScore(wChairID, 0, true);
            }
            else
            {
                //OnEventGameEnd(INVALID_CHAIR, NULL, GER_NO_PLAYER);
                _isEnd = true;
            }

	        return true;
        }

        //加注事件 
        bool OnUserAddScore(int wChairID, int lScore, bool bGiveUp)
        {
            DzCardInfo dzCardInfo = (DzCardInfo)_GameTable._TableInfo;
	        //校验用户
	        //ASSERT(m_wCurrentUser==wChairID);
	        if (dzCardInfo.m_wCurrentUser!=wChairID) 
                return false; 

	        //校验金币
	        //ASSERT((lScore +m_lTotalScore[wChairID])<= m_lUserMaxScore[wChairID]);	
	        if ((lScore+dzCardInfo.m_lTotalScore[wChairID])>dzCardInfo.m_lUserMaxScore[wChairID]) 
                return false;
	        //ASSERT(lScore>=0L);
	        if ((lScore<0)) 
                return false;

	        //累计金币
	        dzCardInfo.m_lTableScore[wChairID] += lScore;
            dzCardInfo.m_lTotalScore[wChairID] += lScore;

	        //平衡下注
            if (dzCardInfo.m_lTableScore[wChairID] > dzCardInfo.m_lBalanceScore)
	        {
                dzCardInfo.m_lBalanceScore = dzCardInfo.m_lTableScore[wChairID];
	        }

	        //梭哈判断
            if (dzCardInfo.m_lTotalScore[wChairID] == dzCardInfo.m_lUserMaxScore[wChairID])
	        {
                dzCardInfo.m_cbShowHand[wChairID] = true;
	        }

	        //用户切换
	        int wNextPlayer=GameDefine.INVALID_CHAIR;
	        for (int i=1;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        //设置变量
                dzCardInfo.m_wOperaCount++;
                wNextPlayer = (dzCardInfo.m_wCurrentUser + i) % DzCardDefine.GAME_PLAYER;

		        //继续判断
                if ((dzCardInfo.m_cbPlayStatus[wNextPlayer] == true) && (dzCardInfo.m_cbShowHand[wNextPlayer] == false)) 
                    break;
	        }
	        //ASSERT(wNextPlayer < m_wPlayerCount);

	        //完成判断
	        bool bFinishTurn=false;
            if (dzCardInfo.m_wOperaCount >= DzCardDefine.GAME_PLAYER)
	        {
                int i = 0;

		        for (i=0;i<DzCardDefine.GAME_PLAYER;i++)
		        {
			        //过滤未平衡 和未梭哈用户
                    if ((dzCardInfo.m_cbPlayStatus[i] == true) && (dzCardInfo.m_lTableScore[i] < dzCardInfo.m_lBalanceScore) && (dzCardInfo.m_cbShowHand[i] == false)) 
				        break;
		        }
		        if (i==DzCardDefine.GAME_PLAYER) 
			        bFinishTurn=true;
	        }

	        //A家show190,B放弃,C还选择?
	        if(!bFinishTurn)
	        {
                int wPlayCount = 0;
                int wShowCount = 0;

		        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
		        {
                    if (dzCardInfo.m_cbPlayStatus[i] == true)
			        {
                        if (dzCardInfo.m_cbShowHand[i] == true)
				        {
					        wShowCount++;
				        }
				        wPlayCount++;
			        }
		        }
                if (wPlayCount - 1 == wShowCount && dzCardInfo.m_lTableScore[wNextPlayer] >= dzCardInfo.m_lBalanceScore) 
                    bFinishTurn = true;
	        }

	        //继续加注
	        if (!bFinishTurn)
	        {
		        //当前用户
                dzCardInfo.m_wCurrentUser = wNextPlayer;

		        //最小值为平衡下注 -桌面下注  和 剩余金币中取小 可能梭哈
                dzCardInfo.m_lTurnLessScore = Math.Min(dzCardInfo.m_lBalanceScore - dzCardInfo.m_lTableScore[dzCardInfo.m_wCurrentUser], dzCardInfo.m_lUserMaxScore[dzCardInfo.m_wCurrentUser] - dzCardInfo.m_lTotalScore[dzCardInfo.m_wCurrentUser]);
                dzCardInfo.m_lTurnMaxScore = dzCardInfo.m_lUserMaxScore[dzCardInfo.m_wCurrentUser] - dzCardInfo.m_lTotalScore[dzCardInfo.m_wCurrentUser];
                if (dzCardInfo.m_lTotalScore[dzCardInfo.m_wCurrentUser] == dzCardInfo.m_lCellScore)
		        {
                    int bTemp = (dzCardInfo.m_lBalanceScore == dzCardInfo.m_lCellScore * 2) ? (dzCardInfo.m_lCellScore * 2) : ((dzCardInfo.m_lBalanceScore - dzCardInfo.m_lCellScore * 2) * 2);
                    dzCardInfo.m_lAddLessScore = dzCardInfo.m_lCellScore + bTemp;
		        }
		        else
                    dzCardInfo.m_lAddLessScore = (int)((dzCardInfo.m_lBalanceScore == 0) ? (2 * dzCardInfo.m_lCellScore) : (Math.Max((dzCardInfo.m_lBalanceScore - dzCardInfo.m_lTableScore[dzCardInfo.m_wCurrentUser]) * 2, 2L * dzCardInfo.m_lCellScore)));

		        //构造数据
                AddScoreInfo AddScore = new AddScoreInfo();

                AddScore.lAddScoreCount = lScore;
                AddScore.wAddScoreUser = wChairID;
                AddScore.wCurrentUser = dzCardInfo.m_wCurrentUser;
                AddScore.lTurnLessScore = dzCardInfo.m_lTurnLessScore;
                AddScore.lTurnMaxScore = dzCardInfo.m_lTurnMaxScore;
                AddScore.lAddLessScore = dzCardInfo.m_lAddLessScore;

		        //发送数据
                _GameTable.BroadCastGame(NotifyType.Reply_AddScore, AddScore);
		        //m_pITableFrame->SendTableData(INVALID_CHAIR,SUB_S_ADD_SCORE,&AddScore,sizeof(AddScore));
		        //m_pITableFrame->SendLookonData(INVALID_CHAIR,SUB_S_ADD_SCORE,&AddScore,sizeof(AddScore));

		        return true;
	        }

	        //平衡次数
            dzCardInfo.m_cbBalanceCount++;
            dzCardInfo.m_wOperaCount = 0;

	        //第1次下注平衡后就开始发给三张公牌
	        //第2次下注平衡后就开始发第四张公牌
	        //第3次下注平衡后就开始发第五张公牌
	        //第4次下注平衡后就结束游戏 

	        //D家下注
            int wDUser = dzCardInfo.m_wDUser;
	        for(int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
                wDUser = (dzCardInfo.m_wDUser + i) % DzCardDefine.GAME_PLAYER;

                if (dzCardInfo.m_cbPlayStatus[wDUser] == true && dzCardInfo.m_cbShowHand[wDUser] == false) 
                    break;
	        }

	        //重值变量
            dzCardInfo.m_lBalanceScore = 0;
            dzCardInfo.m_lTurnLessScore = 0;
            dzCardInfo.m_lTurnMaxScore = dzCardInfo.m_lUserMaxScore[wDUser] - dzCardInfo.m_lTotalScore[wDUser];
            dzCardInfo.m_lAddLessScore = 2 * dzCardInfo.m_lCellScore;

	        //构造数据
            AddScoreInfo AddScoreInfo = new AddScoreInfo();

            AddScoreInfo.wAddScoreUser = wChairID;
            AddScoreInfo.wCurrentUser = GameDefine.INVALID_CHAIR;
            AddScoreInfo.lAddScoreCount = lScore;
            AddScoreInfo.lTurnLessScore = dzCardInfo.m_lTurnLessScore;
            AddScoreInfo.lTurnMaxScore = dzCardInfo.m_lTurnMaxScore;
            AddScoreInfo.lAddLessScore = dzCardInfo.m_lAddLessScore;

	        //清理数据
            Array.Clear(dzCardInfo.m_lTableScore, 0, dzCardInfo.m_lTableScore.Length);
            dzCardInfo.m_lBalanceScore = 0;

	        //发送数据
            _GameTable.BroadCastGame(NotifyType.Reply_AddScore, AddScoreInfo);
	        //m_pITableFrame->SendTableData(INVALID_CHAIR,SUB_S_ADD_SCORE,&AddScore,sizeof(AddScore));
	        //m_pITableFrame->SendLookonData(INVALID_CHAIR,SUB_S_ADD_SCORE,&AddScore,sizeof(AddScore));

	        //结束判断
            if (dzCardInfo.m_cbBalanceCount == 4) 
	        {
		        //OnEventGameEnd(INVALID_CHAIR,NULL,GER_NORMAL);
                _isEnd = true;
                dzCardInfo.cbTotalEnd = 1;
		        return true;
	        }

	        //梭哈用户统计
	        int wShowHandCount=0;
            int wPlayerCount=0;

	        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        if (dzCardInfo.m_cbShowHand[i]==true)	
                    wShowHandCount++;
                if (dzCardInfo.m_cbPlayStatus[i] == true) 
                    wPlayerCount++;
	        }

	        //只剩一玩家没梭或者全梭
            if ((wShowHandCount >= wPlayerCount - 1) && dzCardInfo.m_cbBalanceCount < 4)
	        {
		        //构造数据
                SendCardInfo SendCard = new SendCardInfo();

                SendCard.cbPublic = dzCardInfo.m_cbBalanceCount;
                SendCard.wCurrentUser = GameDefine.INVALID_CHAIR;
                SendCard.cbSendCardCount = DzCardDefine.MAX_CENTERCOUNT;
                SendCard.cbCenterCardData = dzCardInfo.m_cbCenterCardData;

                ////发送数据
                _GameTable.BroadCastGame(NotifyType.Reply_SendCard, SendCard);
                //m_pITableFrame->SendTableData(INVALID_CHAIR,SUB_S_SEND_CARD,&SendCard,sizeof(SendCard));
                //m_pITableFrame->SendLookonData(INVALID_CHAIR,SUB_S_SEND_CARD,&SendCard,sizeof(SendCard));

                ////结束游戏
                //OnEventGameEnd(INVALID_CHAIR,NULL,GER_NORMAL);

                dzCardInfo.m_cbSendCardCount = DzCardDefine.MAX_CENTERCOUNT;

                _isEnd = true;
                dzCardInfo.cbTotalEnd = 1;

		        return true;
	        }

	        //盲注玩家
	        for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
	        {
		        //临时变量
		        int cbNextUser =(dzCardInfo.m_wDUser+i)%DzCardDefine.GAME_PLAYER;

		        //获取用户
		        UserInfo pIServerUserItem= dzCardInfo.m_Seatter[cbNextUser];

		        //无效用户 梭哈用户过滤
                if (pIServerUserItem == null || dzCardInfo.m_cbPlayStatus[cbNextUser] == false || dzCardInfo.m_cbShowHand[cbNextUser] == true) 
			        continue;

                dzCardInfo.m_wCurrentUser = cbNextUser;
		        break;
	        }

	        //构造数据
            SendCardInfo sendCardInfo = new SendCardInfo();

            sendCardInfo.cbPublic = 0;
            sendCardInfo.wCurrentUser = dzCardInfo.m_wCurrentUser;
            sendCardInfo.cbSendCardCount = 3 + (dzCardInfo.m_cbBalanceCount - 1);
            sendCardInfo.cbCenterCardData = dzCardInfo.m_cbCenterCardData;

            ////发送数据
            _GameTable.BroadCastGame(NotifyType.Reply_SendCard, sendCardInfo);
            //m_pITableFrame->SendTableData(INVALID_CHAIR,SUB_S_SEND_CARD,&SendCard,sizeof(SendCard));
            //m_pITableFrame->SendLookonData(INVALID_CHAIR,SUB_S_SEND_CARD,&SendCard,sizeof(SendCard));

            dzCardInfo.m_cbSendCardCount = 3 + (dzCardInfo.m_cbBalanceCount - 1);
	        return true;
        }
    }

    public class DzCardEndRound : EndRound
    {
        public override void CheckWinner()
        {
            DzCardInfo dzCardInfo = (DzCardInfo)_GameTable._TableInfo;

            if (dzCardInfo.cbTotalEnd == 0)
            {
                //定义变量
                //CMD_S_GameEnd GameEnd;
                //ZeroMemory(&GameEnd, sizeof(GameEnd));
                //GameEnd.cbTotalEnd = 0;

                //效验结果
                //int wUserCount = 0;
                //for (int i = 0; i < GAME_PLAYER; i++)
                //{
                //    if (m_cbPlayStatus[i] != FALSE) wUserCount++;
                //}
                //if (wUserCount != 1)
                //{
                //    ASSERT(FALSE);
                //    TraceMessage("没有玩家//效验结果出错");
                //}

                //统计分数
                int lScore = 0, lRevenue = 0;
                //enScoreKind nScoreKind;
                int wWinner = GameDefine.INVALID_CHAIR;
                
                for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                {
                    if (dzCardInfo.m_cbPlayStatus[i] == false)
                    {
                        if (dzCardInfo.m_lTotalScore[i] > 0)
                        {
                            dzCardInfo.m_GameScore[i] -= dzCardInfo.m_lTotalScore[i];
                        }
                        continue;
                    }

                    wWinner = i;

                    ////处理税收
                    //for (WORD t=0;t<m_wPlayerCount;t++)
                    //{
                    //	//赢家不用收税
                    //	if(wWinner==t)continue;
                    //	if(m_lTotalScore[t]>0L)
                    //	{
                    //		GameEnd.lGameScore[t]=-m_lTotalScore[t];
                    //		m_lTotalScore[t]-=m_bUserTax[t];
                    //		GameEnd.lGameTax[t] = m_bUserTax[t];
                    //	}
                    //}

                    //总下注数目
                    int lAllScore = 0;
                    for (int j = 0; j < DzCardDefine.GAME_PLAYER; j++)
                    {
                        if (wWinner == j) 
                            continue;
                        lAllScore += dzCardInfo.m_lTotalScore[j];
                    }
                    //ASSERT(lAllScore >= 0);
                    dzCardInfo.m_GameScore[i] = lAllScore;

                    //统计税收
                    //if (dzCardInfo.m_GameScore[i] > 0)
                    //{
                    //    //扣税变量
                    //    WORD wRevenue = m_pGameServiceOption->wRevenue;
                    //    GameEnd.lGameTax[i] = GameEnd.lGameScore[i] * wRevenue / 100L;
                    //    GameEnd.lGameScore[i] -= GameEnd.lGameTax[i];
                    //}

                    //构造扑克
                    //CopyMemory(GameEnd.cbCardData, m_cbHandCardData, sizeof(m_cbHandCardData));

                    lScore = dzCardInfo.m_GameScore[i];
                    lRevenue = 0;// GameEnd.lGameTax[i];
                    //nScoreKind = (GameEnd.lGameScore[i] > 0L) ? enScoreKind_Win : enScoreKind_Lost;
                }

                //发送消息
                //m_pITableFrame->SendTableData(INVALID_CHAIR, SUB_S_GAME_END, &GameEnd, sizeof(GameEnd));
                //m_pITableFrame->SendLookonData(INVALID_CHAIR, SUB_S_GAME_END, &GameEnd, sizeof(GameEnd));

                //写入金币
                if (wWinner < DzCardDefine.GAME_PLAYER)
                {
                    int index = _GameTable.GetPlayerIndex(dzCardInfo.m_Seatter[wWinner]);

                    dzCardInfo.m_lUserWinScore[index] = dzCardInfo.m_GameScore[wWinner];

                    //m_pITableFrame->WriteUserScore(wWinner, lScore, lRevenue, nScoreKind);
                }
                //else TraceMessage("//写入金币ffff");

                ////结束游戏
                //m_pITableFrame->ConcludeGame();
                //Array.Clear(dzCardInfo.m_lTotalScore, 0, dzCardInfo.m_lTotalScore.Length);

                return;
            }

			//扑克数据
            List<int[]> cbEndCardData = new List<int[]>();
            
            for( int i = 0; i < DzCardDefine.GAME_PLAYER; i++ )
                cbEndCardData.Add( new int[DzCardDefine.MAX_CENTERCOUNT] );

			//Array.Clear(cbEndCardData, 0, cbEndCardData.Length);

			try{
				//获取扑克
				for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
				{
					//用户过滤
					if (dzCardInfo.m_cbPlayStatus[i]==false) 
                        continue;

					//最大牌型
					int cbEndCardKind = DzCardTable.m_GameLogic.FiveFromSeven(dzCardInfo.m_cbHandCardData[i],DzCardDefine.MAX_COUNT,dzCardInfo.m_cbCenterCardData,DzCardDefine.MAX_CENTERCOUNT,cbEndCardData[i],DzCardDefine.MAX_CENTERCOUNT);
					//ASSERT(cbEndCardKind!=FALSE);			
					//CopyMemory(GameEnd.cbLastCenterCardData[i],cbEndCardData[i],sizeof(BYTE)*CountArray(cbEndCardData));
                    Array.Copy(cbEndCardData[i], dzCardInfo.m_cbOverCardData[i], cbEndCardData[i].Length);
				}
			}catch( Exception e )
			{
				//TraceMessage("用户过滤v最大牌型");
				//ASSERT(FALSE);
			}

			//总下注备份
			int[] lTotalScore = new int[DzCardDefine.GAME_PLAYER];
			//ZeroMemory(lTotalScore,sizeof(lTotalScore));
			Array.Copy( dzCardInfo.m_lTotalScore, lTotalScore, dzCardInfo.m_lTotalScore.Length );

			//胜利列表
			UserWinList[] WinnerList = new UserWinList[DzCardDefine.GAME_PLAYER];

            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
                WinnerList[i] = new UserWinList();

			//ZeroMemory(WinnerList,sizeof(WinnerList));

			//临时数据
			//int[,] bTempData = new int[DzCardDefine.GAME_PLAYER, DzCardDefine.MAX_CENTERCOUNT];
			//CopyMemory(bTempData,cbEndCardData,GAME_PLAYER*MAX_CENTERCOUNT);
            //List<int[]> bTempData = new List<int[]>();
            //Array.Copy( cbEndCardData, bTempData, cbEndCardData.Count );
            int[][] bTempData = new int[cbEndCardData.Count][];

            for (int i = 0; i < cbEndCardData.Count; i++)
            {
                bTempData[i] = new int[cbEndCardData[i].Length];

                for (int k = 0; k < cbEndCardData[i].Length; k++)
                    bTempData[i][k] = cbEndCardData[i][k];

            }

			int wWinCount=0;
			try{
				//用户得分顺序
				for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
				{
					//查找最大用户
                    if (!DzCardTable.m_GameLogic.SelectMaxUser(bTempData, WinnerList[i], lTotalScore))
					{
						wWinCount=i;
						break;
					}

					//删除胜利数据
					for (int j=0;j<WinnerList[i].bSameCount;j++)
					{
						int wRemoveId=WinnerList[i].wWinerList[j];
						//ASSERT(bTempData[wRemoveId][0]!=0);
						Array.Clear(bTempData[wRemoveId],0, bTempData[wRemoveId].Length);
					}
				}
			}catch( Exception e )
			{
				//TraceMessage("用户得分顺序");
				//ASSERT(FALSE);
			}

			//强退用户
			for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
			{
				if(dzCardInfo.m_cbPlayStatus[i]==false && lTotalScore[i]>0l)
				{					
					WinnerList[wWinCount].wWinerList[WinnerList[wWinCount].bSameCount++] = i;
				}
			}

			//得分变量
			int[] lUserScore = new int[DzCardDefine.GAME_PLAYER];
			Array.Clear(lUserScore, 0, lUserScore.Length);
			//CopyMemory(lTotalScore,m_lTotalScore,sizeof(m_lTotalScore));

			try
			{
				//得分情况
				for (int i=0;i<DzCardDefine.GAME_PLAYER-1;i++)
				{
					//胜利人数
					int iWinCount = (int)WinnerList[i].bSameCount;
					if(0 == iWinCount)break;

					//胜利用户得分情况
					for(int j=0;j<iWinCount;j++)
					{
						if(0 == lTotalScore[WinnerList[i].wWinerList[j]])continue;

						if(j>0 && lTotalScore[WinnerList[i].wWinerList[j]] - 
							lTotalScore[WinnerList[i].wWinerList[j-1]] == 0)continue;

						//失败用户失分情况
						for(int k=i+1;k<DzCardDefine.GAME_PLAYER;k++)
						{
							//失败人数
							if(0 == WinnerList[k].bSameCount)break;

							for(int l=0;l<WinnerList[k].bSameCount;l++)
							{
								//用户已赔空
								if(0 == lTotalScore[WinnerList[k].wWinerList[l]])continue;

								int wLostId=WinnerList[k].wWinerList[l];
								int wWinId=WinnerList[i].wWinerList[j];
								int lMinScore = 0;

								//上家得分数目
								int lLastScore = ((j>0)?lTotalScore[WinnerList[i].wWinerList[j-1]]:0);
								//if(j>0)ASSERT(lLastScore>0L);							
								lMinScore = Math.Min(lTotalScore[wWinId]-lLastScore,lTotalScore[wLostId]);

								for(int m=j;m<iWinCount;m++)
								{
									//得分数目
									lUserScore[WinnerList[i].wWinerList[m]]+=lMinScore/(iWinCount-j);
								}

								//赔偿数目
								lUserScore[wLostId]-=lMinScore;
								lTotalScore[wLostId]-=lMinScore;
							}
						}
					}
				}
			}catch
			{
				//TraceMessage("得分数目/赔偿数目");
				//ASSERT(FALSE);
			}

            ////扣税变量
            //int wRevenue=m_pGameServiceOption->wRevenue;

            //统计用户分数(税收)
            for (int i = 0; i < DzCardDefine.GAME_PLAYER; i++)
            {
                dzCardInfo.m_GameScore[i] = lUserScore[i];
                //ASSERT(lUserScore[i] + m_lTotalScore[i] >= 0L);
                //if (GameEnd.lGameScore[i] > 0L)
                //{
                //    GameEnd.lGameTax[i] = GameEnd.lGameScore[i] * wRevenue / 100L;
                //    GameEnd.lGameScore[i] -= GameEnd.lGameTax[i];
                //}
            }

			////统计用户分数
			//for(WORD i=0;i<m_wPlayerCount;i++)
			//{
			//	GameEnd.lGameScore[i]=lUserScore[i];
			//	GameEnd.lGameScore[i]-=GameEnd.lGameTax[i];
			//}

			//CopyMemory(GameEnd.cbCardData,m_cbHandCardData,sizeof(m_cbHandCardData));

			//发送信息
			//m_pITableFrame->SendTableData(INVALID_CHAIR,SUB_S_GAME_END,&GameEnd,sizeof(GameEnd));
			//m_pITableFrame->SendLookonData(INVALID_CHAIR,SUB_S_GAME_END,&GameEnd,sizeof(GameEnd));

			//金币变量
			for (int i=0;i<DzCardDefine.GAME_PLAYER;i++)
			{
				if (dzCardInfo.m_cbPlayStatus[i]==true)
				{
					//enScoreKind nScoreKind;
					//if(GameEnd.lGameScore[i]==0L)nScoreKind=enScoreKind_Draw;
					//else nScoreKind=(GameEnd.lGameScore[i]>0L)?enScoreKind_Win:enScoreKind_Lost;

					//写入金币
					//m_pITableFrame->WriteUserScore(i,GameEnd.lGameScore[i],GameEnd.lGameTax[i],nScoreKind);

                    int index = _GameTable.GetPlayerIndex( dzCardInfo.m_Seatter[i] );
                    dzCardInfo.m_lUserWinScore[index] = lUserScore[i];
				}
			}

			//结束游戏
			//m_pITableFrame->ConcludeGame();
            //Array.Clear(dzCardInfo.m_lTotalScore, 0, dzCardInfo.m_lTotalScore.Length);
        }

        public override void CheckScore()
        {
        }
    }

}
