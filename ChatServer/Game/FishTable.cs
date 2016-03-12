using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.Drawing;

namespace ChatServer
{
    class FishTable : GameTable
    {
        public FishTable(string tableId)
        {
            _TableInfo = new FishInfo();
            _TableInfo._TableId = tableId;

            _Rounds.Add(new FishReadyRound());
            _Rounds.Add(new FishBetRound());

            for (int i = 0; i < _Rounds.Count; i++)
                _Rounds[i]._GameTable = this;

            ReadConfig();

            _TableInfo.m_lMinScore = 100;
        }

        public int GetSeatterIndex(UserInfo userInfo)
        {
            if (userInfo == null)
                return GameDefine.INVALID_CHAIR;

            FishInfo fishInfo = (FishInfo)_TableInfo;

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                if (fishInfo.m_RoleObjects[i] == null)
                    continue;

                if (fishInfo.m_RoleObjects[i].userId == userInfo.Id)
                    return i;
            }

            return GameDefine.INVALID_CHAIR;
        }

        public int ReadExpValue(int wChair)						//读炮台等级
        {
            FishInfo fishInfo = (FishInfo)_TableInfo;

            int nValue = fishInfo.m_RoleObjects[wChair].dwExpValue;
            //UserInfo pIServerUser = fishInfo._Players[wChair];

            //if (pIServerUser == null) return 0;

            ////	tagServerUserData *pServerUserData = pIServerUser.GetUserData();

            ////tagUserInfo * pUserData=pIServerUser.GetUserInfo();

            ////if(pUserData==null) return 0;

            //string strData = string.Format("{0}", pIServerUser.GameId);
            //int nValue = GetPrivateProfileInt(strData, "ExpValue", 0, fishInfo.m_szUserDataFileName);

            return nValue;
        }

        public bool WriteExpValue(int wChair, int nValue)			//写炮台等级
        {
            FishInfo fishInfo = (FishInfo)_TableInfo;

            fishInfo.m_RoleObjects[wChair].dwExpValue = nValue;

            //UserInfo pIServerUser = fishInfo._Players[wChair];

            //if (pIServerUser == null) return false;

            ////	tagServerUserData *pServerUserData = pIServerUser.GetUserData();

            ////	if(pServerUserData==null) return false;

            //string strData = string.Format("{0}", wChair);
            //string strValue = string.Format("{0}", nValue);
            //WritePrivateProfileString(strData, "ExpValue", strValue, fishInfo.m_szUserDataFileName);

            return true;
        }

        public static void WritePrivateProfileString(string appName, string keyName, string defaultString, string fileName)
        {
        }

        public static int GetPrivateProfileInt(string appName, string keyName, int defaultValue, string fileName)
        {
            return defaultValue;
        }

        public static int GetPrivateProfileString(string appName, string keyName, string defaultString, string returndString, int size, string fileName)
        {
            return 0;
        }

        void ReadConfig()
        {
            FishInfo fishInfo = (FishInfo)_TableInfo;

            string szBuffer;

            fishInfo.m_nAwardRate = FishTable.GetPrivateProfileInt("AwardCtrl", "AwardRate", 10, fishInfo.m_szConfigFileName);
            fishInfo.m_nAwardFishBase = FishTable.GetPrivateProfileInt("AwardCtrl", "AwardFishBase", 10, fishInfo.m_szConfigFileName);

            for (int i = 0; i < FishDefine.MAX_CANNON_LEVEL; i++)
            {
                string strTemp = string.Format("Level{0}", i);
                fishInfo.m_nCannonLevel[i] = FishTable.GetPrivateProfileInt("CannonCtrl", strTemp, (i == 0) ? 50 : 100 * i, fishInfo.m_szConfigFileName);
            }

            for (int i = 0; i < FishDefine.MAX_FISH_STYLE; i++)
            {
                string strTemp = string.Format("Fish{0}", i);

                FishInfo.m_ProbabilityFish[i] = FishTable.GetPrivateProfileInt("Probability", strTemp, 1000, fishInfo.m_szConfigFileName);
            }

            fishInfo.m_nCellScore = FishTable.GetPrivateProfileInt("ScoreCtrl", "CellScore", 1, fishInfo.m_szConfigFileName); if (fishInfo.m_nCellScore <= 0) fishInfo.m_nCellScore = 1;
            FishInfo.m_nProbability = FishTable.GetPrivateProfileInt("ScoreCtrl", "Probability", 1000, fishInfo.m_szConfigFileName);
            fishInfo.m_nScoreMaxBuy = FishTable.GetPrivateProfileInt("ScoreCtrl", "ScoreMaxBuy", 5000, fishInfo.m_szConfigFileName);
            fishInfo.m_nChangeRate = FishTable.GetPrivateProfileInt("ScoreCtrl", "ChangeRate", 100, fishInfo.m_szConfigFileName);

            int nVIPflag = FishTable.GetPrivateProfileInt("ScoreCtrl", "VIPflag", 0, fishInfo.m_szConfigFileName);
            for (int wChair = 0; wChair < FishDefine.GAME_PLAYER; wChair++)
            {
                fishInfo.m_RoleObjects[wChair].dwExpValue = ReadExpValue(wChair);

                if (nVIPflag == 1)
                {
                    fishInfo.m_RoleObjects[wChair].dwMaxMulRate = fishInfo.m_nCannonLevel[10];
                }
                else
                {
                    fishInfo.m_RoleObjects[wChair].dwMaxMulRate = fishInfo.m_nCannonLevel[((fishInfo.m_RoleObjects[wChair].dwExpValue / FishDefine.EXP_CHANGE_TO_LEVEL) >= FishDefine.MAX_CANNON_LEVEL) ? FishDefine.MAX_CANNON_LEVEL - 1 : (fishInfo.m_RoleObjects[wChair].dwExpValue / FishDefine.EXP_CHANGE_TO_LEVEL)];
                }
            }
            // 	string d;
            // 	d.Format("%d %d %s",fishInfo.m_nAwardRate,fishInfo.m_nAwardFishBase,fishInfo.m_szConfigFileName);
            // 	AfxMessageBox(d);
        }

        public override bool PlayerEnterTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            if (!(baseInfo is UserInfo))
                return false;

            UserInfo seatInfo = (UserInfo)baseInfo;

            FishInfo fishInfo = (FishInfo)_TableInfo;

            for (int i = 0; i < FishDefine.GAME_PLAYER; i++)
            {
                if (fishInfo.m_RoleObjects[i].wID == GameDefine.INVALID_CHAIR)
                {
                    userInfo.userSeat = i;
                    break;
                }
            }

            if (base.PlayerEnterTable(baseInfo, userInfo) == false)
                return false;

            int wChairID = userInfo.userSeat;

            fishInfo.m_RoleObjects[wChairID].dwExpValue = ReadExpValue(wChairID);
            fishInfo.m_RoleObjects[wChairID].wID = wChairID;
            fishInfo.m_RoleObjects[wChairID].dwFishGold = 0;
            fishInfo.m_RoleObjects[wChairID].wCannonType = 0;
            fishInfo.m_RoleObjects[wChairID].wFireCount = fishInfo.m_nFireCount[userInfo.userSeat];
            fishInfo.m_RoleObjects[wChairID].dwMulRate = 5;
            fishInfo.m_RoleObjects[wChairID].userId = userInfo.Id;

            int nVIPflag = GetPrivateProfileInt("ScoreCtrl", "VIPflag", 0, fishInfo.m_szConfigFileName);
            if (nVIPflag == 1)
            {
                fishInfo.m_RoleObjects[wChairID].dwMaxMulRate = fishInfo.m_nCannonLevel[10];
            }
            else
            {
                fishInfo.m_RoleObjects[wChairID].dwMaxMulRate = fishInfo.m_nCannonLevel[((fishInfo.m_RoleObjects[wChairID].dwExpValue / FishDefine.EXP_CHANGE_TO_LEVEL) >= FishDefine.MAX_CANNON_LEVEL) ? FishDefine.MAX_CANNON_LEVEL - 1 : (fishInfo.m_RoleObjects[wChairID].dwExpValue / FishDefine.EXP_CHANGE_TO_LEVEL)];
            }


            fishInfo.m_wCookFishCount[wChairID] = 0;
            //fishInfo.m_bTaskSended[wChairID] = false;
            fishInfo.m_cbShootFish14Count[wChairID] = 0;

            if (fishInfo.m_dwSceneStartTime == 0)
            {
                fishInfo.m_cbScene = 1;
                fishInfo.m_dwSceneStartTime = FishDefine.time();

                /*
                    * static readonly DateTime _unixEpoch =
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                public static DateTime DateFromTimestamp(long timestamp)
                {
                    return _unixEpoch.AddSeconds(timestamp);
                }
                    * */

                AddGameTimer(FishDefine.IDI_SCENE_START, FishDefine.TIME_SCENE_START * 3 / 1000);
                AddGameTimer(FishDefine.IDI_CHECK_FISH_DESTORY_START, FishDefine.TIME_CHECK_FISH_DESTORY_START * 30 / 1000);
            }

            return true;
        }

        public override bool PlayerOutTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            FishInfo fishInfo = (FishInfo)_TableInfo;
            FishBetRound betRound = (FishBetRound)_Rounds[_TableInfo._RoundIndex];

            int wChairID = GetSeatterIndex(userInfo);

            if (wChairID != GameDefine.INVALID_CHAIR)
            {
                //
			    // 剩余子弹
			    int nScore = fishInfo.m_RoleObjects[wChairID].dwFishGold*fishInfo.m_nCellScore;//fishInfo.m_pGameServiceOption.lCellScore;

			    if(nScore!=0)
			    {
                    int userIndex = GetPlayerIndex(userInfo);
                    fishInfo.m_lUserWinScore[userIndex] = nScore;

                    Cash.GetInstance().ProcessGameCash(userIndex, this._GameInfo, this._TableInfo);
                    fishInfo.m_lUserWinScore[userIndex] = 0;
			    }

			    fishInfo.m_RoleObjects[wChairID].wID = GameDefine.INVALID_CHAIR;
			    fishInfo.m_RoleObjects[wChairID].dwFishGold = 0;
			    fishInfo.m_RoleObjects[wChairID].wCannonType = 0;
			    fishInfo.m_RoleObjects[wChairID].wFireCount = 0;
			    fishInfo.m_RoleObjects[wChairID].dwMaxMulRate = 0;
			    fishInfo.m_RoleObjects[wChairID].dwMulRate = 0;
			    fishInfo.m_RoleObjects[wChairID].userId = string.Empty;
			    fishInfo.m_wCookFishCount[wChairID] = 0;

			    //fishInfo.m_bTaskSended[wChairID] = false;

			    fishInfo.m_TaskObjects[wChairID].m_enType =  CTaskDate.Type.TYPE_NULL;

			    //if (fishInfo.m_wAndroidLogicChairID == pIServerUserItem.GetChairID() && !pIServerUserItem.IsAuto())
                if (fishInfo.m_wAndroidLogicUserID == wChairID)
			    {
				    fishInfo.m_wAndroidLogicUserID = 0;
				    fishInfo.m_wAndroidLogicChairID = GameDefine.INVALID_CHAIR;
			    }

		        WriteExpValue(wChairID,fishInfo.m_RoleObjects[wChairID].dwExpValue);

                //fishInfo.m_RoleObjects[wChairID] = null;
            }

            return base.PlayerOutTable(baseInfo, userInfo);
        }
    }

    public class FishReadyRound : ReadyRound
    {
        public override int GetNeedPlayers()
        {
            return 1;
        }

        public override void Start()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

            //设置时间
            //_TimerId = FishDefine.IDI_FREE;
            //_GameTable.AddGameTimer(_TimerId, FishInfo.m_cbFreeTime);

            base.Start();
        }
    }

    public class FishBetRound : StartRound
    {
        Random _random = new Random();

        public override void Start()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

            for( int i = 0; i < FishDefine.GAME_PLAYER; i++ )
            {
                if( fishInfo.m_RoleObjects[i].wID == GameDefine.INVALID_CHAIR )
                    continue;

                int wChiarID = i;

		        fishInfo.m_nCurGateCount[wChiarID] = 0;  

                //TCHAR cbBuf[255] = {};

                //GetPrivateProfileString(TEXT("ServerControl"),TEXT("CurVer"),TEXT(""),cbBuf ,32,m_szConfigFileName);
                //swscanf(cbBuf,_T("%s"),StatusFree.cbVer);
                //GetPrivateProfileString(TEXT("ServerControl"),TEXT("ResultURL"),TEXT(""),cbBuf ,128,m_szConfigFileName);
                //swscanf(cbBuf,_T("%s"),StatusFree.cbURL);
                //GetPrivateProfileString(TEXT("ServerControl"),TEXT("IpList"),TEXT(""),cbBuf ,128,m_szConfigFileName);
                //swscanf(cbBuf,_T("%s"),StatusFree.cbIPList);

                //StatusFree.dwMatchScore = GetPrivateProfileInt(TEXT("ServerControl"),TEXT("MatchScore"),0,m_szConfigFileName);
                //StatusFree.dwRoomType = GetPrivateProfileInt(TEXT("ServerControl"),TEXT("RoomType"),0,m_szConfigFileName);

                //if(StatusFree.dwRoomType==0)
                //{
                //    StatusFree.cbMatchNot = 0;
                //}
                //else if((StatusFree.dwRoomType==1)||(StatusFree.dwRoomType==2))
                //{
                //    StatusFree.RoleObjects[wChiarID].dwMatchGold = 0;
                //    StatusFree.RoleObjects[wChiarID].dwFishGold = 0;

                //    StatusFree.cbMatchNot = 0;

                //    //判断比赛时间
                //    INT		nYear=0,nMonth=0,nDay=0,nHour=0,nMinute=0;
                //    CTime	ctBeginDate,ctEndDate;

                //    //获取系统时间
                //    CTime ctCurDate = CTime::GetCurrentTime();

                //    //获取比赛开始时间
                //    GetPrivateProfileString(TEXT("ServerControl"),TEXT("TimeBegin"),TEXT(""),cbBuf ,128,m_szConfigFileName);
                //    swscanf(cbBuf,TEXT("%d-%d-%d %d:%d"),&nYear,&nMonth,&nDay,&nHour,&nMinute);
                //    if((nYear>0)&&(nMonth>0)&&(nDay>0)) 
                //    {
                //        ctBeginDate = CTime(nYear,nMonth,nDay,nHour,nMinute,1);

                //        //获取比赛结束时间
                //        GetPrivateProfileString(TEXT("ServerControl"),TEXT("TimeEnd"),TEXT(""),cbBuf ,128,m_szConfigFileName);
                //        swscanf(cbBuf,TEXT("%d-%d-%d %d:%d"),&nYear,&nMonth,&nDay,&nHour,&nMinute);
                //        if((nYear>0)&&(nMonth>0)&&(nDay>0)) 
                //        {
                //            ctEndDate = CTime(nYear,nMonth,nDay,nHour,nMinute,59);
                //            if((ctCurDate>=ctBeginDate)&&(ctCurDate<=ctEndDate))
                //            {
                //                StatusFree.cbMatchNot = 0; //符合比赛时间
                //            }
                //            else
                //            {
                //                StatusFree.cbMatchNot = 1;	//非比赛时间
                //            }
                //        }
                //    }
                ////	m_pITableFrame->SetGameTimer(IDI_MATCH_TIME_SEND,1000,1,0L);
                //}

			    //是否平行鱼群
			    if((fishInfo.m_bIsSceneing)&&(fishInfo.m_cbSendFishType==0))
			    {
                    switch(wChiarID)
				    {
					    case 0:_GameTable.AddGameTimer(FishDefine.IDI_ADD_SMALL_FISH_GROUP0,500/1000);break;
					    case 1:_GameTable.AddGameTimer(FishDefine.IDI_ADD_SMALL_FISH_GROUP1,500/1000);break;
					    case 2:_GameTable.AddGameTimer(FishDefine.IDI_ADD_SMALL_FISH_GROUP2,500/1000);break;
				    }
			    }
            }
            base.Start();
        }

        public override void InitTableData(TableInfo tableInfo)
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
        }

        public override void AutoAction(UserInfo userInfo)
        {
            int bettingCount = _random.Next(0, 8);

            for (int i = 0; i < bettingCount; i++)
            {
                int delay = _random.Next(2, 10);

                _GameTable.AddAutoTimer(userInfo, delay);
            }
        }

        public override void NotifyGameTimer(GameTimer gameTimer)
        {
            int dwTimerID = gameTimer.timerId;
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        switch (dwTimerID)
            {
	        case FishDefine.IDI_GET_POOL_SCORE:
		        {
			        //_GameTable.AddGameTimer(FishDefine.IDI_GET_POOL_SCORE,30000,1,0L);

			        string d = string.Format("总体概率={0} 鱼的概率={1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", FishInfo.m_nProbability,
                        FishInfo.m_ProbabilityFish[0], FishInfo.m_ProbabilityFish[1], FishInfo.m_ProbabilityFish[2], FishInfo.m_ProbabilityFish[3], FishInfo.m_ProbabilityFish[4],
                        FishInfo.m_ProbabilityFish[5], FishInfo.m_ProbabilityFish[6], FishInfo.m_ProbabilityFish[7], FishInfo.m_ProbabilityFish[8], FishInfo.m_ProbabilityFish[9],
                        FishInfo.m_ProbabilityFish[10], FishInfo.m_ProbabilityFish[11], FishInfo.m_ProbabilityFish[12], FishInfo.m_ProbabilityFish[13], FishInfo.m_ProbabilityFish[14]);

			        FishTable.WritePrivateProfileString("ScorePool","CurTotalScore",d,fishInfo.m_szUserDataFileName);

			        break;
		        }

	        case FishDefine.IDI_ADD_SMALL_FISH_GROUP0:
		        {
			        //pakcj //pakcj fishInfo.m_pITableFrame.KillGameTimer(FishDefine.IDI_ADD_SMALL_FISH_GROUP0);
			        CMD_S_Small_Fish_Group SmallFishGroup = new CMD_S_Small_Fish_Group();
			        //SmallFishGroup.wChair = 0;
			        SmallFishGroup.nTime = 0;
			        SmallFishGroup.wFishType = fishInfo.m_cbSendSmallFishType;

			        DateTime ctCurDate = DateTime.Now;
			        if(ctCurDate > fishInfo.m_ctCurTime)
			        {
				        TimeSpan timeSpan = ctCurDate - fishInfo.m_ctCurTime;
				        SmallFishGroup.nTime = (int)(100 - timeSpan.TotalSeconds);
			        }

			        for(int i=0;i<1;i++)
			        {
				        for(int j=0;j<30;j++)
				        {
					        SmallFishGroup.wFishGroupBottom[i,j].wFishID  = fishInfo.m_wSmallFishGroupBottom[i,j].wFishID;
					        SmallFishGroup.wFishGroupBottom[i,j].wRoundID = fishInfo.m_wSmallFishGroupBottom[i,j].wRoundID;
					        SmallFishGroup.wFishGroupTop[i,j].wFishID     = fishInfo.m_wSmallFishGroupTop[i,j].wFishID;
					        SmallFishGroup.wFishGroupTop[i,j].wRoundID    = fishInfo.m_wSmallFishGroupTop[i,j].wRoundID;
				        }
			        }

			        _GameTable.SendGameData(0, NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SMALL_FISH_GROUP, SmallFishGroup ));
			        break;
		        }
	        case FishDefine.IDI_ADD_SMALL_FISH_GROUP1:
		        {
			        //pakcj //pakcj fishInfo.m_pITableFrame.KillGameTimer(FishDefine.IDI_ADD_SMALL_FISH_GROUP1);
			        CMD_S_Small_Fish_Group SmallFishGroup = new CMD_S_Small_Fish_Group();
			        SmallFishGroup.nTime = 0;
			        SmallFishGroup.wFishType = fishInfo.m_cbSendSmallFishType;

			        DateTime ctCurDate = DateTime.Now;
			        if(ctCurDate > fishInfo.m_ctCurTime)
			        {
				        TimeSpan timeSpan = ctCurDate - fishInfo.m_ctCurTime;
				        SmallFishGroup.nTime = (int)(100 - timeSpan.TotalSeconds);
			        }

			        for(int i=0;i<1;i++)
			        {
				        for(int j=0;j<30;j++)
				        {
					        SmallFishGroup.wFishGroupBottom[i,j].wFishID  = fishInfo.m_wSmallFishGroupBottom[i,j].wFishID;
					        SmallFishGroup.wFishGroupBottom[i,j].wRoundID = fishInfo.m_wSmallFishGroupBottom[i,j].wRoundID;
					        SmallFishGroup.wFishGroupTop[i,j].wFishID     = fishInfo.m_wSmallFishGroupTop[i,j].wFishID;
					        SmallFishGroup.wFishGroupTop[i,j].wRoundID    = fishInfo.m_wSmallFishGroupTop[i,j].wRoundID;
				        }
			        }

			        _GameTable.SendGameData(1, NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SMALL_FISH_GROUP,SmallFishGroup));

			        break;
		        }
	        case FishDefine.IDI_ADD_SMALL_FISH_GROUP2:
		        {
			        //pakcj fishInfo.m_pITableFrame.KillGameTimer(FishDefine.IDI_ADD_SMALL_FISH_GROUP2);
			        CMD_S_Small_Fish_Group SmallFishGroup = new CMD_S_Small_Fish_Group();
			        SmallFishGroup.nTime = 0;
			        SmallFishGroup.wFishType = fishInfo.m_cbSendSmallFishType;

			        DateTime ctCurDate = DateTime.Now;
			        if(ctCurDate > fishInfo.m_ctCurTime)
			        {
				        TimeSpan timeSpan = ctCurDate - fishInfo.m_ctCurTime;
				        SmallFishGroup.nTime = (int)(100 - timeSpan.TotalSeconds);
			        }

			        for(int i=0;i<1;i++)
			        {
				        for(int j=0;j<30;j++)
				        {
					        SmallFishGroup.wFishGroupBottom[i,j].wFishID  = fishInfo.m_wSmallFishGroupBottom[i,j].wFishID;
					        SmallFishGroup.wFishGroupBottom[i,j].wRoundID = fishInfo.m_wSmallFishGroupBottom[i,j].wRoundID;
					        SmallFishGroup.wFishGroupTop[i,j].wFishID     = fishInfo.m_wSmallFishGroupTop[i,j].wFishID;
					        SmallFishGroup.wFishGroupTop[i,j].wRoundID    = fishInfo.m_wSmallFishGroupTop[i,j].wRoundID;
				        }
			        }

			        _GameTable.SendGameData(2, NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SMALL_FISH_GROUP,SmallFishGroup));
			        break;
		        }

	        case FishDefine.IDI_ADD_SCENE_FISH_SEND_COUNT:
		        {
			        SendSceneFishCountObject();
			        fishInfo.m_dwSendFishCount++;
			        if(fishInfo.m_dwSendFishCount<17)
			        {
				        _GameTable.AddGameTimer(FishDefine.IDI_ADD_SCENE_FISH_SEND_COUNT,(int)(FishInfo.m_dwFishSendTime[fishInfo.m_dwSendFishCount]/1000));
			        }
			        else
			        {
				        fishInfo.m_dwSendFishSmallCount = 0;
				        _GameTable.RemoveGameTimer(FishDefine.IDI_ADD_SCENE_FISH_SEND_SMALL_COUNT);
				        fishInfo.m_dwSendFishCount = 0;
				        //pakcj fishInfo.m_pITableFrame.KillGameTimer(FishDefine.IDI_ADD_SCENE_FISH_SEND_COUNT);
				        fishInfo.m_wSceneSendFishCount++;
				        _GameTable.AddGameTimer(FishDefine.IDI_SCENE_ADD_FISH_END,5000 / 1000);
			        }
			        break;
		        }
	        case FishDefine.IDI_ADD_SCENE_FISH_SEND_SMALL_COUNT:
		        {
			        fishInfo.m_ctCurTime = DateTime.Now;
			        if(fishInfo.m_cbSendFishType==0)
			        {
				        SendSceneTopSmallFishObject();
				        SendSceneBottomSmallFishObject();
				        fishInfo.m_dwSendFishSmallCount++;
				        if(fishInfo.m_dwSendFishSmallCount<1)
				        {
					        _GameTable.AddGameTimer(FishDefine.IDI_ADD_SCENE_FISH_SEND_SMALL_COUNT,2000 / 1000);
				        }
				        else
				        {
					        fishInfo.m_bIsSmallFishGroupSendOk = false;

					        fishInfo.m_dwSendFishSmallCount = 0;
					        //pakcj fishInfo.m_pITableFrame.KillGameTimer(FishDefine.IDI_ADD_SCENE_FISH_SEND_SMALL_COUNT);
				        }
			        }
			        else
			        {
				        SendSceneBottomSmallFishObject1();
				        SendSceneTopSmallFishObject1();
				        _GameTable.AddGameTimer(FishDefine.IDI_ADD_SCENE_FISH_SEND_SMALL_COUNT,6000 / 1000 );
			        }
			        break;
		        }
	        case FishDefine.IDI_ADD_BIG_FISH_11_ENABLE:
		        {
			        _GameTable.AddGameTimer(FishDefine.IDI_ADD_BIG_FISH_11_ENABLE,45*1000 / 1000);
			
			        fishInfo.m_cbSendFish11 = 1;
			        break;
		        }
	        case FishDefine.IDI_ADD_BIG_FISH_12_ENABLE:
		        {
			        _GameTable.AddGameTimer(FishDefine.IDI_ADD_BIG_FISH_12_ENABLE,90*1000/1000);

			        fishInfo.m_cbSendFish12 = 1;
			        break;
		        }
	        case FishDefine.IDI_ADD_BIG_FISH_13_ENABLE:
		        {
			        _GameTable.AddGameTimer(FishDefine.IDI_ADD_BIG_FISH_13_ENABLE,120*1000/1000);

			        if((rand()%2)==0) fishInfo.m_cbSendFish13 = 1;
			        else			  fishInfo.m_cbSendFish14 = 1;

			        break;
		        }
	        case FishDefine.IDI_SCENE_START:
                {
                    fishInfo.m_dwSceneStartTime=time();

                    if (!SendNormalFishObject(true))
			        {
				        LogView.AddLogString("OnTimerMessage FishDefine.IDI_SCENE_START SendNormalFishObject()");
                        return;
			        }

                    _GameTable.AddGameTimer(FishDefine.IDI_NORMAL_ADD_FISH_START,FishDefine.TIME_NORMAL_ADD_FISH_START/1000);

                    break;
                }
            case FishDefine.IDI_SCENE_END:
                {
                    fishInfo.m_cbScene ++;
                    fishInfo.m_cbScene %= FishDefine.MAX_SCENE;

                    CMD_S_Change_Scene ChangeScene = new CMD_S_Change_Scene();

                    ChangeScene.cbScene = fishInfo.m_cbScene;
			        ChangeScene.cbSceneSound = rand()%10;
                    _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_CHANGE_SCENE, ChangeScene ));

                    _GameTable.AddGameTimer(FishDefine.IDI_SCENE_ADD_FISH_START,(FishDefine.TIME_SCENE_ADD_FISH_START+6000)/1000);
                    break;
                }
            case FishDefine.IDI_NORMAL_ADD_FISH_START:
                {
                    long dwPassTime=time()-fishInfo.m_dwSceneStartTime;

                    if ((dwPassTime>240 && dwPassTime<270)
                        ||(dwPassTime>480 && dwPassTime<510))
                    {
                        if (!SendSpecialFishObject())
				        {
					        LogView.AddLogString("OnTimerMessage FishDefine.IDI_NORMAL_ADD_FISH_START SendNormalFishObject()");
                            return;
				        }
                    }
                    else
                    {
                        if (!SendNormalFishObject(false))
				        {
					        LogView.AddLogString("OnTimerMessage FishDefine.IDI_NORMAL_ADD_FISH_START1 SendNormalFishObject()");
                            return;
				        }
                    }

                    break;
                }
            case FishDefine.IDI_NORMAL_ADD_FISH_END:
                {
                    long dwPassTime=time()-fishInfo.m_dwSceneStartTime;
                    if (dwPassTime>FishDefine.CHANGE_SCENE_TIME_COUNT)
                    {
                        fishInfo.m_dwSceneStartTime = time();
                        _GameTable.AddGameTimer(FishDefine.IDI_SCENE_END,FishDefine.TIME_SCENE_END/1000);
                    }
                    else
                    {
                        _GameTable.AddGameTimer(FishDefine.IDI_NORMAL_ADD_FISH_START,FishDefine.TIME_NORMAL_ADD_FISH_START/1000);
                    }
                    break;
                }

            case FishDefine.IDI_SCENE_ADD_FISH_START:
                {
                    SendSceneFishObject();
                    break;
                }
            case FishDefine.IDI_SCENE_ADD_FISH_END:
                {
                    _GameTable.AddGameTimer(FishDefine.IDI_SCENE_ADD_FISH_START,FishDefine.TIME_SCENE_ADD_FISH_START/1000);

                    break;
                }

            case FishDefine.IDI_CHECK_FISH_DESTORY_START:
                {
                    CheckFishDestroy();
                    _GameTable.AddGameTimer(FishDefine.IDI_CHECK_FISH_DESTORY_END,FishDefine.TIME_CHECK_FISH_DESTORY_START/1000);

                    break;
                }
            case FishDefine.IDI_CHECK_FISH_DESTORY_END:
                {
                    _GameTable.AddGameTimer(FishDefine.IDI_CHECK_FISH_DESTORY_START,FishDefine.TIME_CHECK_FISH_DESTORY_END/1000);

                    break;
                }
            }        
        }

        private string TEXT(string s)
        {
            return s;
        }

        public override bool Action(NotifyType notifyType, BaseInfo baseInfo, UserInfo userInfo)
        {
            if (!(baseInfo is FishSendInfo))
            {
                return base.Action(notifyType, baseInfo, userInfo);
            }

            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

            FishSendInfo sendInfo = (FishSendInfo)baseInfo;
            int wSubCmdID = sendInfo._SendType;

            switch (wSubCmdID)
            {
                case FishDefine.SUB_C_BUY_BULLET:
                    {
                        CMD_C_Buy_Bullet pBuyBullet = (CMD_C_Buy_Bullet)sendInfo._SendInfo;
                        return OnBuyBullet(userInfo, pBuyBullet);
                    }
                case FishDefine.SUB_C_FIRE:
                    {
                        CMD_C_Fire pFire = (CMD_C_Fire)sendInfo._SendInfo;
                        return OnFire(userInfo, pFire);
                    }
                case FishDefine.SUB_C_CAST_NET:
                    {
                        CMD_C_Cast_Net pCastNet = (CMD_C_Cast_Net)sendInfo._SendInfo;
                        return OnCastNet(userInfo, pCastNet);
                    }

                case FishDefine.SUB_C_CHANGE_CANNON:
                    {
                        CMD_C_Change_Cannon pChangeCanon = (CMD_C_Change_Cannon)sendInfo._SendInfo;
                        return OnChangeCannon(userInfo, pChangeCanon);
                    }
                case FishDefine.SUB_C_ACCOUNT:
                    {
                        CMD_C_Account pAccount = (CMD_C_Account)sendInfo._SendInfo;
                        return OnAccount(userInfo, pAccount);
                    }
                case FishDefine.SUB_C_LASER_BEAN:
                    {
                        CMD_C_Laser_Bean pLaserBean = (CMD_C_Laser_Bean)sendInfo._SendInfo;
                        return OnLaserBean(userInfo, pLaserBean);
                    }
                case FishDefine.SUB_C_BOMB:
                    {
                        CMD_C_Bomb pBomb = (CMD_C_Bomb)sendInfo._SendInfo;
                        return OnBomb(userInfo, pBomb);
                    }
                case FishDefine.SUB_C_BONUS:
                    {
                        CMD_C_Bonus pBonus = (CMD_C_Bonus)sendInfo._SendInfo;
                        return OnBonus(userInfo, pBonus);
                    }
                case FishDefine.SUB_C_COOK:
                    {
                        CMD_C_Cook pCook = (CMD_C_Cook)sendInfo._SendInfo;
                        return OnCook(userInfo, pCook);
                    }
                case FishDefine.SUB_C_TASK_PREPARED:
                    {
                        CMD_C_Task_Prepared TaskPrepared = (CMD_C_Task_Prepared)sendInfo._SendInfo;
                        return OnTaskPrepared(userInfo, TaskPrepared);
                    }
                case FishDefine.SUB_C_SEND_MESSAGE:
                    {
                        CMD_C_Send_Message pSendMessage = (CMD_C_Send_Message)sendInfo._SendInfo;

                        fishInfo.m_nSendMessage = FishTable.GetPrivateProfileInt(TEXT("ScoreCtrl"), TEXT("SendMessage"), 1, fishInfo.m_szConfigFileName);

                        CMD_S_Send_Message SendMessage = new CMD_S_Send_Message();

                        SendMessage.wChair = pSendMessage.wChair;
                        SendMessage.nLen = (fishInfo.m_nSendMessage == 0) ? 0 : pSendMessage.nLen;
                        SendMessage.cbData = pSendMessage.cbData;

                        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_MESSAGE, SendMessage));
                        return true;
                    }
                case FishDefine.SUB_C_CANNON_RUN:
                    {
                        CMD_C_Connon_Run pConnonRun = (CMD_C_Connon_Run)sendInfo._SendInfo;

                        CMD_S_Connon_Run ConnonRun = new CMD_S_Connon_Run();

                        ConnonRun.wChair = pConnonRun.wChair;
                        ConnonRun.fRote = pConnonRun.fRote;

                        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_CANNON_RUN, ConnonRun));
                        return true;
                    }
                case FishDefine.SUB_C_MATCH_START:
                    {
                        long dwRoomType = FishTable.GetPrivateProfileInt(TEXT("ServerControl"), TEXT("RoomType"), 0, fishInfo.m_szConfigFileName);

                        if ((dwRoomType == 1) || (dwRoomType == 2))
                        {
                            CMD_C_Match_Start pMatchStart = (CMD_C_Match_Start)sendInfo._SendInfo;

                            CMD_S_Match_Start MatchStart = new CMD_S_Match_Start();

                            MatchStart.wChair = pMatchStart.wChair;
                            MatchStart.dwMatchScore = 0;
                            MatchStart.dwScore = FishTable.GetPrivateProfileInt(TEXT("ServerControl"), TEXT("GameScore"), 0, fishInfo.m_szConfigFileName);
                            fishInfo.m_RoleObjects[pMatchStart.wChair].dwFishGold = MatchStart.dwScore;
                            fishInfo.m_RoleObjects[pMatchStart.wChair].dwMatchGold = 0;

                            //收费赛
                            if (dwRoomType == 2)
                            {
                                int nMatchScore = FishTable.GetPrivateProfileInt(TEXT("ServerControl"), TEXT("MatchScore"), 0, fishInfo.m_szConfigFileName);
                                int wChairID = ((FishTable)_GameTable).GetSeatterIndex(userInfo);
                            }

                            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_MATCH_START, MatchStart));
                        }

                        return true;
                    }
                case FishDefine.SUB_C_GATE_CTRL_SEND:
                    {
                        long dwRoomType = FishTable.GetPrivateProfileInt(TEXT("ServerControl"), TEXT("RoomType"), 0, fishInfo.m_szConfigFileName);

                        if (dwRoomType == 3)
                        {
                            CMD_C_Gate_Ctrl_Send pGateCtrlSend = (CMD_C_Gate_Ctrl_Send)sendInfo._SendInfo;

                            CMD_S_Gate_Ctrl_Send GateCtrlSend = new CMD_S_Gate_Ctrl_Send();

                            //读取配置信息
                            GateCtrlSend.wChair = pGateCtrlSend.wChair;
                            GateCtrlSend.nGateCount = FishTable.GetPrivateProfileInt(TEXT("GateCtrl"), TEXT("GateCount"), 0, fishInfo.m_szConfigFileName);
                            GateCtrlSend.nFishScoreBase = FishTable.GetPrivateProfileInt(TEXT("GateCtrl"), TEXT("FishScoreBase"), 0, fishInfo.m_szConfigFileName);

                            if (GateCtrlSend.nGateCount >= FishDefine.MAX_GATE_COUNT) GateCtrlSend.nGateCount = FishDefine.MAX_GATE_COUNT;

                            for (int i = 0; i < GateCtrlSend.nGateCount; i++)
                            {
                                string strData = string.Format("Gate{0}", i);

                                GateCtrlSend.tagGateCtrlInf[i].nFishScore = FishTable.GetPrivateProfileInt(strData, TEXT("FishScore"), 0, fishInfo.m_szConfigFileName);
                                GateCtrlSend.tagGateCtrlInf[i].nGetScore = FishTable.GetPrivateProfileInt(strData, TEXT("GetScore"), 0, fishInfo.m_szConfigFileName);
                            }

                            if (pGateCtrlSend.cbFirst == 0)						//首次闯关
                            {
                                fishInfo.m_nCurGateCount[pGateCtrlSend.wChair] = 0;
                                fishInfo.m_RoleObjects[pGateCtrlSend.wChair].dwFishGold = GateCtrlSend.nFishScoreBase;
                            }
                            else if ((pGateCtrlSend.cbFirst == 1) || (pGateCtrlSend.cbFirst == 7))					//结束闯关
                            {
                                int nScore = GateCtrlSend.tagGateCtrlInf[fishInfo.m_nCurGateCount[pGateCtrlSend.wChair]].nGetScore;

                                tagScoreInfo ScoreInfo = new tagScoreInfo();
                                //ZeroMemory(&ScoreInfo, sizeof(ScoreInfo));
                                ScoreInfo.lScore = nScore;
                                ScoreInfo.lRevenue = 0;
                                if (nScore > 0)
                                    ScoreInfo.cbType = FishDefine.SCORE_TYPE_WIN;
                                else if (nScore < 0)
                                    ScoreInfo.cbType = FishDefine.SCORE_TYPE_LOSE;
                                else
                                    ScoreInfo.cbType = FishDefine.SCORE_TYPE_DRAW;

                                //fishInfo.m_pITableFrame.WriteUserScore(userInfo, nScore , 0,fishInfo.m_pGameServiceOption.wCharge,enScoreKind_Draw,false);
                                //fishInfo.m_pITableFrame.WriteUserScore(userInfo, nScore , 0,enScoreKind_Draw);
                                //pakcj fishInfo.m_pITableFrame.WriteUserScore(userInfo.GetChairID(), ScoreInfo);

                                fishInfo.m_nCurGateCount[pGateCtrlSend.wChair] = 0;
                                fishInfo.m_RoleObjects[pGateCtrlSend.wChair].dwFishGold = GateCtrlSend.nFishScoreBase;
                            }
                            else												//继续闯关
                            {
                                fishInfo.m_nCurGateCount[pGateCtrlSend.wChair] = (fishInfo.m_nCurGateCount[pGateCtrlSend.wChair] + 1) % GateCtrlSend.nGateCount;
                            }

                            GateCtrlSend.nCurGateCount = fishInfo.m_nCurGateCount[pGateCtrlSend.wChair];
                            GateCtrlSend.cbFirst = pGateCtrlSend.cbFirst;

                            if (pGateCtrlSend.cbFirst != 7) _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_GATE_CTRL_SEND, GateCtrlSend));
                        }

                        return true;
                    }
                case FishDefine.SUB_C_MATCH_OVER:
                    {
                        CMD_C_Match_Over pMatch_Over = (CMD_C_Match_Over)sendInfo._SendInfo;

                        CMD_S_Match_Over Match_Over = new CMD_S_Match_Over();

                        Match_Over.wChair = pMatch_Over.wChair;
                        Match_Over.lMatchScore = fishInfo.m_RoleObjects[pMatch_Over.wChair].dwFishGold;
                        Match_Over.lMatchScoreCheck = fishInfo.m_RoleObjects[pMatch_Over.wChair].dwFishGold + 888888;

                        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_MATCH_OVER, Match_Over));
                        return true;
                    }
                case FishDefine.SUB_C_END_GAME:
                    {
                        //用户效验
                        //pakcj fishInfo.m_pITableFrame.PerformStandUpAction(userInfo);
                        return true;
                    }

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

        bool OnBuyBullet(UserInfo pIServerUserItem, CMD_C_Buy_Bullet pBuyBullet)
        {
            FishTable fishTable = (FishTable)_GameTable;
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

            int wChairID = fishTable.GetSeatterIndex(pIServerUserItem);
	        int nBuyBulletPrice = pBuyBullet.dwCount*fishInfo.m_nCellScore;
	        //const tagUserInfo *pUserScore = pIServerUserItem.GetUserInfo();

	        if (pIServerUserItem.GetGameMoney() > nBuyBulletPrice)
	        {
		        fishInfo.m_RoleObjects[wChairID].dwFishGold += pBuyBullet.dwCount;

		        if(-nBuyBulletPrice!=0)
		        {
			        int enKind = (-nBuyBulletPrice>0)?FishDefine.SCORE_TYPE_WIN:FishDefine.SCORE_TYPE_LOSE;

			        //积分变量
                    int userIndex = _GameTable.GetPlayerIndex(pIServerUserItem);
                    fishInfo.m_lUserWinScore[userIndex] = -nBuyBulletPrice;

                    Cash.GetInstance().ProcessGameCash(userIndex, _GameTable._GameInfo, fishInfo);
                    fishInfo.m_lUserWinScore[userIndex] = 0;
		        }

		        CMD_S_Buy_Bullet_Success BuyBulletSuccess = new CMD_S_Buy_Bullet_Success();

		        BuyBulletSuccess.wChairID = wChairID;
		        BuyBulletSuccess.dwCount = pBuyBullet.dwCount;
		        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_BUY_BULLET_SUCCESS, BuyBulletSuccess));
	        }
	        else
	        {
		        CMD_S_Buy_Bullet_Failed BuyBulletFailed = new CMD_S_Buy_Bullet_Failed();
		        BuyBulletFailed.wChairID = wChairID;
		        /*BuyBulletFailed.szReason = "金币不足！"*/

		        /// 机器人没有钱起立
                if (pIServerUserItem.IsAuto())
		        {
                    return true; //pakcj fishInfo.m_pITableFrame.PerformStandUpAction(userInfo);
		        }

		        _GameTable.SendGameData(wChairID, NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_BUY_BULLET_FAILED, BuyBulletFailed));
	        }

	        return true;
        }

        //发射子弹
        bool OnFire(UserInfo pIServerUserItem, CMD_C_Fire pFire)
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        int wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);
	        int dwFishGold = fishInfo.m_RoleObjects[wChairID].dwFishGold;

            long dwRoomType = FishTable.GetPrivateProfileInt(TEXT("ServerControl"), TEXT("RoomType"), 0, fishInfo.m_szConfigFileName);
            fishInfo.m_nChangeRate = FishTable.GetPrivateProfileInt(TEXT("ScoreCtrl"), TEXT("ChangeRate"), 100, fishInfo.m_szConfigFileName);
	        int dwMulRate = (pFire.dwMulRate > 1000000) ? pFire.dwMulRate - 1000000 : pFire.dwMulRate;
	        int dwShootScore = fishInfo.m_RoleObjects[wChairID].dwMulRate==FishDefine.INVALID_WORD ? 1 : dwMulRate;
	        
            if (dwFishGold > 0) // 送子弹不足炮类型要求 送子弹
	        {
			    {
			        if(dwFishGold >= dwShootScore)  fishInfo.m_RoleObjects[wChairID].dwFishGold = dwFishGold - dwShootScore;
			        else                            fishInfo.m_RoleObjects[wChairID].dwFishGold = 0;
		        }
	
		        CMD_S_Fire_Success FireSuccess = new CMD_S_Fire_Success();

		        FireSuccess.cbIsBack = pFire.cbIsBack;
		        FireSuccess.wChairID = wChairID;
		        FireSuccess.wAndroidLogicChairID = fishInfo.m_wAndroidLogicChairID;
		        FireSuccess.fRote = pFire.fRote;
		        FireSuccess.dwMulRate = pFire.dwMulRate;
		        FireSuccess.xStart = pFire.xStart;
		        FireSuccess.yStart = pFire.yStart;

		        int nLevel = 0;
		        bool bTask = false;

		        if(pFire.cbIsBack==0)
		        {
			        if(dwRoomType==0)
			        {
				        //是否大鱼库存
				        if(fishInfo.m_bIsBigDataBase) fishInfo.m_bIsBigDataBase = false;
				        else                 fishInfo.m_bIsBigDataBase = true;

				        //计数库存
				        int lScore = dwShootScore*fishInfo.m_nChangeRate/100;
				        //写库存
				        //int lTempScore = ReadFireValue(wChairID,2) + lScore;
				        //WriteFireValue(wChairID,2,lTempScore);
			        }

			        fishInfo.m_nFireCount[wChairID] += dwShootScore;//fishInfo.m_GameLogic.GetExp(fishInfo.m_RoleObjects[wChairID].wCannonType);
			        fishInfo.m_RoleObjects[wChairID].wFireCount = fishInfo.m_nFireCount[wChairID];
		        }
	
		        FireSuccess.nFireCount = fishInfo.m_nFireCount[wChairID];
		        FireSuccess.cbIsMatchEnd = 0;
		        FireSuccess.dwMatchScore = 0;
		        FireSuccess.dwIndex = 0;

		        if((dwRoomType==1)||(dwRoomType==2))
		        {
			        if(fishInfo.m_RoleObjects[wChairID].dwFishGold<=0)
			        {
				        FireSuccess.cbIsMatchEnd = 1;
				        FireSuccess.dwMatchScore = fishInfo.m_RoleObjects[wChairID].dwMatchGold;
			        }
		        }

                _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_FIRE_SUCCESS, FireSuccess));
	        }
	        else
	        {
                CMD_S_Fire_Failed FireFailed = new CMD_S_Fire_Failed();

		        FireFailed.wChairID = wChairID;
		        /*FireFailed.szReason = "金币不足！";*/

		        _GameTable.SendGameData(wChairID, NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_FIRE_FAILED, FireFailed));
	        }
            return true;
        }


        bool OnCastNet(UserInfo pIServerUserItem, CMD_C_Cast_Net pCastNet)
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

        //	if (!(userInfo.GetChairID() == pCastNet.wChairID || fishInfo.m_wAndroidLogicUserID == pIServerUserItem.GetUserID())) return true;

	        //捕获概率
            long dwRoomType = FishTable.GetPrivateProfileInt(TEXT("ServerControl"), TEXT("RoomType"), 0, fishInfo.m_szConfigFileName);
	        int nProbability = FishInfo.m_nProbability;
	        int wFishID;
	        int wRoundID;
	        int nSucessCount = 0;
	        //int wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);
	        int wChairID = pCastNet.wChairID;
	        int nFishInNetCount = pCastNet.cbCount;

        //	if (fishInfo.m_wAndroidLogicChairID == GameDefine.INVALID_CHAIR && pIServerUserItem.GetUserStatus() != US_OFFLINE && !pIServerUserItem.IsAuto())
        //	{
        //		fishInfo.m_wAndroidLogicUserID = pIServerUserItem.GetUserID();
        //		fishInfo.m_wAndroidLogicChairID = _GameTable.GetPlayerIndex(pIServerUserItem);
        //	}

	        CMD_S_Cast_Net_Success CastNetSuccess = new CMD_S_Cast_Net_Success();
	        //ZeroMemory(&CastNetSuccess,sizeof(CMD_S_Cast_Net_Success));

	        CastNetSuccess.wChairID = wChairID;
	        CastNetSuccess.cbLevelUp = 0;

	        //判断是否有库存,炸弹鱼的存在
	        bool bHaveStock = true,bHaveBombFish = false;
	        int	dwMulRateBombFish = 0;
	        int lNetBigFishScore = 0,lNetSmallFishScore = 0;
	        
            for (int i=0; i<nFishInNetCount; i++)
	        {
		        if (nSucessCount >= FishDefine.MAX_FISH_IN_NET) break;

		        if(pCastNet.FishNetObjects[i].dwTime > 1000) return true;

		        wFishID = pCastNet.FishNetObjects[i].wID;
		        wRoundID = pCastNet.FishNetObjects[i].wRoundID;

		        if (fishInfo.m_FishObjects[wFishID].wID!=wFishID || fishInfo.m_FishObjects[wFishID].wRoundID!=wRoundID)
		        {
			        continue;
		        }
		        else
		        {
			        if(fishInfo.m_FishObjects[wFishID].wType==13) //炸弹鱼
			        {
				        int nDropValue = FishInfo.m_ProbabilityFish[fishInfo.m_FishObjects[wFishID].wType];
				        int nProbabilityAll = nDropValue*nProbability;
				        if ((rand()%1000) < nProbabilityAll/1000)
				        {
					        bHaveBombFish = true;
					        dwMulRateBombFish = (int)pCastNet.FishNetObjects[i].dwTime;
				        }
			        }
			        else
			        {
				        int nStyle = FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wRoundID);

				        if(nStyle>=10)	lNetBigFishScore   += (int)(nStyle*pCastNet.FishNetObjects[i].dwTime);
				        else			lNetSmallFishScore += (int)(nStyle*pCastNet.FishNetObjects[i].dwTime);
			        }
		        }
	        }

	        int lStock = ReadFireValue(wChairID,2);

	        //有炸弹鱼
	        if(bHaveBombFish)
	        {
		        fishInfo.m_cbShootFish14Count[wChairID] = 1;

		        CMD_S_Bomb_Fish BombFish = new CMD_S_Bomb_Fish();
		
		        BombFish.wChairID = wChairID;
		        BombFish.lBigStock = lStock;
		        BombFish.lSmallStock = 0;
		        BombFish.dwMulRate = dwMulRateBombFish;
		
		        _GameTable.SendGameData(wChairID, NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_BOMB_FISH, BombFish));
		        return true;
	        }


	        if(lStock<(lNetBigFishScore+lNetSmallFishScore))
	        {
		        bHaveStock = false;
	        }
	
	        int lBigFishScore   = 0;
	        int lSmallFishScore = 0;

	        for (int i=0; i<nFishInNetCount; i++)
            {
		        if (nSucessCount >= FishDefine.MAX_FISH_IN_NET) break;

                wFishID = pCastNet.FishNetObjects[i].wID;
                wRoundID = pCastNet.FishNetObjects[i].wRoundID;

		        if(wFishID >= FishDefine.MAX_FISH_OBJECT) continue;

                if ((fishInfo.m_FishObjects[wFishID].wID!=wFishID) || (fishInfo.m_FishObjects[wFishID].wRoundID!=wRoundID))
                {
                    continue;
                }
                else
                {
			        if(fishInfo.m_FishObjects[wFishID].wType==13) continue;

			        if(bHaveStock==false) continue;

			        if(fishInfo.m_FishObjects[wFishID].wType >= FishDefine.MAX_FISH_STYLE) continue;

                    int nDropTempValue = FishInfo.m_ProbabilityFish[fishInfo.m_FishObjects[wFishID].wType];//fishInfo.m_GameLogic.GetProbability(fishInfo.m_RoleObjects[wChairID].wCannonType, fishInfo.m_FishObjects[wFishID].wType);

			        int nProbabilityAll = nDropTempValue*nProbability;
			        int nDropValue = nProbabilityAll/1000;
			
			        //如果是涨潮鱼群   场景鱼群类型，0 -. 平行线2排  增加小鱼难度3~5倍
			        //if((fishInfo.m_bIsSceneing)&&(fishInfo.m_cbSendFishType==0))
			        //{
			        //	if(fishInfo.m_FishObjects[wFishID].wType<8)
			        //	{
			        //		nDropValue = nDropValue*(100+(rand()%100))/1000;
			        //	}
			        //}

                    if ((rand()%1000) < nDropValue)
                    {
				        if (fishInfo.m_TaskObjects[wChairID].m_enType == CTaskDate.Type.TYPE_COOK && fishInfo.m_TaskObjects[wChairID].m_nFishType==fishInfo.m_FishObjects[wFishID].wType
                            && fishInfo.m_TaskObjects[wChairID].m_enState==CTaskDate.State.STATE_RUNNING)
                        {
                            fishInfo.m_wCookFishCount[wChairID]++;
                        }

				        //经验值达到，炮台升级
				        int nFishValue = FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wFishID);
				        if(( nFishValue >= 10)&&(dwRoomType==0))
				        {
					        int nCannonLevelOld = ((fishInfo.m_RoleObjects[wChairID].dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL) >= FishDefine.MAX_CANNON_LEVEL) ? FishDefine.MAX_CANNON_LEVEL - 1 : (fishInfo.m_RoleObjects[wChairID].dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL);
                            fishInfo.m_RoleObjects[wChairID].dwExpValue = ((FishTable)_GameTable).ReadExpValue(wChairID) + nFishValue;
					        if(fishInfo.m_RoleObjects[wChairID].dwExpValue > 2000000000) fishInfo.m_RoleObjects[wChairID].dwExpValue = 2000000000;
                            ((FishTable)_GameTable).WriteExpValue(wChairID, fishInfo.m_RoleObjects[wChairID].dwExpValue);
					        int nVIPflag = FishTable.GetPrivateProfileInt(TEXT("ScoreCtrl"),TEXT("VIPflag"),    0,fishInfo.m_szConfigFileName);
					        if(nVIPflag==1)
					        {
						        fishInfo.m_RoleObjects[wChairID].dwMaxMulRate = fishInfo.m_nCannonLevel[10];
					        }
					        else
					        {
						        int nCannonLevel = ((fishInfo.m_RoleObjects[wChairID].dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL) >= FishDefine.MAX_CANNON_LEVEL) ? FishDefine.MAX_CANNON_LEVEL - 1 : (fishInfo.m_RoleObjects[wChairID].dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL);
						        fishInfo.m_RoleObjects[wChairID].dwMaxMulRate = fishInfo.m_nCannonLevel[nCannonLevel];
						        if(nCannonLevelOld!=nCannonLevel) CastNetSuccess.cbLevelUp = 1;
					        }
				        }

				        CastNetSuccess.FishNetObjects[nSucessCount].wID = wFishID;
                        CastNetSuccess.FishNetObjects[nSucessCount].wRoundID = wRoundID;
				        CastNetSuccess.FishNetObjects[nSucessCount].wType = fishInfo.m_FishObjects[wFishID].wType;
				        CastNetSuccess.FishNetObjects[nSucessCount].dwTime = pCastNet.FishNetObjects[i].dwTime;

                        nSucessCount++;

				        int lFishScore = (int)(FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wRoundID)*pCastNet.FishNetObjects[i].dwTime);

				        int nStyle = FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wRoundID);

				        if(nStyle>=10)	lBigFishScore   += lFishScore;
				        else			lSmallFishScore += lFishScore;

				        ClearSmallFishGroup(fishInfo.m_FishObjects[wFishID].wID);
				        fishInfo.m_FishObjects[wFishID].wID = FishDefine.INVALID_WORD;
                        fishInfo.m_RoleObjects[wChairID].dwFishGold += lFishScore;
				        //if(dwRoomType==0) fishInfo.m_RoleObjects[wChairID].dwFishGold += lFishScore;
				        //else			  fishInfo.m_RoleObjects[wChairID].dwMatchGold += lFishScore;
                    }
                }
            }

	        if((lBigFishScore+lSmallFishScore)>0)   
	        {
		        if(dwRoomType==0) WriteFireValue(wChairID,2,lStock-(lBigFishScore+lSmallFishScore));
	        }

	        CastNetSuccess.cbCount = nSucessCount;
	        CastNetSuccess.dwExpValue = fishInfo.m_RoleObjects[wChairID].dwExpValue;
            CastNetSuccess.dwLevel = ((fishInfo.m_RoleObjects[wChairID].dwExpValue / FishDefine.EXP_CHANGE_TO_LEVEL) >= FishDefine.MAX_CANNON_LEVEL) ? FishDefine.MAX_CANNON_LEVEL - 1 : (fishInfo.m_RoleObjects[wChairID].dwExpValue / FishDefine.EXP_CHANGE_TO_LEVEL);

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_CAST_NET_SUCCESS, CastNetSuccess));
	        return true;
        }

        bool OnBomb(UserInfo pIServerUserItem, CMD_C_Bomb pBomb)
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

            int wFishID;
            int wRoundID;
            int nSucessCount = 0;
            int wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);
            int nFishInNetCount = pBomb.cbCount;

            if ((fishInfo.m_TaskObjects[wChairID].m_enType != CTaskDate.Type.TYPE_BOMB) && (fishInfo.m_cbShootFish14Count[wChairID]==0))
            {
                LogView.AddLogString("TableFrameSink::OnBomb");
                return true;
            }
            long dwRoomType = FishTable.GetPrivateProfileInt(TEXT("ServerControl"), TEXT("RoomType"), 0, fishInfo.m_szConfigFileName);
	        fishInfo.m_cbShootFish14Count[wChairID] = 0;

            CMD_S_Bomb_Success BombSuccess = new CMD_S_Bomb_Success();
            BombSuccess.wChairID = wChairID;
	        BombSuccess.cbLevelUp = 0;

	        int lBigFishScore = 0;
	        int lSmallFishScore = 0;

	        for (int i=0; i<nFishInNetCount; i++)
            {
		        if (nSucessCount >= FishDefine.MAX_FISH_IN_NET_BOMB)
        	        break;

                wFishID = pBomb.FishNetObjects[i].wID;
                wRoundID = pBomb.FishNetObjects[i].wRoundID;

                if (fishInfo.m_FishObjects[wFishID].wID!=wFishID || fishInfo.m_FishObjects[wFishID].wRoundID!=wRoundID )
                {
                    continue;
                }
                else
                {
			        if (nSucessCount >= FishDefine.MAX_FISH_IN_NET_BOMB)
				        break;

			        //if(fishInfo.m_FishObjects[wFishID].wType==14) continue;

                    BombSuccess.FishNetObjects[nSucessCount].wID = wFishID;
                    BombSuccess.FishNetObjects[nSucessCount].wRoundID = wRoundID;
			        BombSuccess.FishNetObjects[nSucessCount].wType = fishInfo.m_FishObjects[wFishID].wType;
			        BombSuccess.FishNetObjects[nSucessCount].dwTime = fishInfo.m_RoleObjects[wChairID].dwMulRate;

                    nSucessCount++;

			        //经验值达到，炮台升级
			        int nFishValue = FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wFishID);
			        if((nFishValue >= 10)&&(dwRoomType == 0))
			        {
				        int nCannonLevelOld = ((fishInfo.m_RoleObjects[wChairID].dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL) >= FishDefine.MAX_CANNON_LEVEL) ? FishDefine.MAX_CANNON_LEVEL - 1 : (fishInfo.m_RoleObjects[wChairID].dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL);
                        fishInfo.m_RoleObjects[wChairID].dwExpValue = ((FishTable)_GameTable).ReadExpValue(wChairID) + nFishValue;
				        if(fishInfo.m_RoleObjects[wChairID].dwExpValue > 2000000000) fishInfo.m_RoleObjects[wChairID].dwExpValue = 2000000000;
                        ((FishTable)_GameTable).WriteExpValue(wChairID, fishInfo.m_RoleObjects[wChairID].dwExpValue);
				        int nVIPflag = FishTable.GetPrivateProfileInt(TEXT("ScoreCtrl"),TEXT("VIPflag"),    0,fishInfo.m_szConfigFileName);
				        if(nVIPflag==1)
				        {
					        fishInfo.m_RoleObjects[wChairID].dwMaxMulRate = fishInfo.m_nCannonLevel[10];
				        }
				        else
				        {
					        int nCannonLevel = ((fishInfo.m_RoleObjects[wChairID].dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL) >= FishDefine.MAX_CANNON_LEVEL) ? FishDefine.MAX_CANNON_LEVEL - 1 : (fishInfo.m_RoleObjects[wChairID].dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL);
					        fishInfo.m_RoleObjects[wChairID].dwMaxMulRate = fishInfo.m_nCannonLevel[nCannonLevel];
					        if(nCannonLevelOld!=nCannonLevel) BombSuccess.cbLevelUp = 1;
				        }
			        }

			        ClearSmallFishGroup(fishInfo.m_FishObjects[wFishID].wID);
			        fishInfo.m_FishObjects[wFishID].wID = FishDefine.INVALID_WORD;
			        fishInfo.m_RoleObjects[wChairID].dwFishGold += FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wFishID)*fishInfo.m_RoleObjects[wChairID].dwMulRate;
			        //if(dwRoomType==0)	fishInfo.m_RoleObjects[wChairID].dwFishGold += FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wFishID)*fishInfo.m_RoleObjects[wChairID].dwMulRate;
			        //else				fishInfo.m_RoleObjects[wChairID].dwMatchGold += FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wFishID)*fishInfo.m_RoleObjects[wChairID].dwMulRate;		
	
			        if(FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wFishID)>=10) 
				        lBigFishScore += FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wFishID)*fishInfo.m_RoleObjects[wChairID].dwMulRate;
			        else								
				        lSmallFishScore += FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wFishID)*fishInfo.m_RoleObjects[wChairID].dwMulRate;
                }
            }

	        //因为炸弹鱼已经判断了库存，所以可以扣库存
	        if(dwRoomType==0)
	        {
        // 		int lStock = 0;//ReadFireValue(wChairID,2);
        // 
        // 		lStock -= (lBigFishScore+lSmallFishScore);
        // 
        // 		WriteFireValue(wChairID,2,lStock);
	        }

            BombSuccess.cbCount = nSucessCount;

            fishInfo.m_TaskObjects[wChairID].m_enType = CTaskDate.Type.TYPE_NULL;
	        fishInfo.m_cbShootFish14Count[wChairID] = 0;

	        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_BOMB_SUCCESS, BombSuccess));
	        return true;
        }

        bool OnBonus(UserInfo pIServerUserItem, CMD_C_Bonus pBonus)
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            int wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);

            if (fishInfo.m_TaskObjects[wChairID].m_enType != CTaskDate.Type.TYPE_BONUS)
            {
                LogView.AddLogString("OnBonus");
                return true;
            }

            CMD_S_Bonus Bonus = new CMD_S_Bonus();
            Bonus.wChairID = wChairID;
            Bonus.nBonus = fishInfo.m_TaskObjects[wChairID].m_nBonus;

            fishInfo.m_RoleObjects[wChairID].dwFishGold +=  fishInfo.m_TaskObjects[wChairID].m_nBonus;

            fishInfo.m_TaskObjects[wChairID].m_enType = CTaskDate.Type.TYPE_NULL;

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_BONUS, Bonus));
	        return true;
        }

        bool OnCook(UserInfo pIServerUserItem, CMD_C_Cook pCook)
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            int wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);

            if (fishInfo.m_TaskObjects[wChairID].m_enType != CTaskDate.Type.TYPE_COOK)
            {
                LogView.AddLogString("OnCook  if (fishInfo.m_TaskObjects[wChairID].m_enType != CTaskDate.Type.TYPE_COOK)") ;
                return true;
            }

            CMD_S_Cook Cook = new CMD_S_Cook();
            Cook.wChairID = wChairID;
            Cook.nBonus = fishInfo.m_TaskObjects[wChairID].m_nBonus;
   
            if (fishInfo.m_wCookFishCount[wChairID] >= fishInfo.m_TaskObjects[wChairID].m_nFishCount)
            {
                 Cook.cbSucess = 1;
                 fishInfo.m_RoleObjects[wChairID].dwFishGold +=  fishInfo.m_TaskObjects[wChairID].m_nBonus;
            }
            else
            {
                Cook.cbSucess = 0;
            }

            fishInfo.m_wCookFishCount[wChairID] = 0;
            fishInfo.m_TaskObjects[wChairID].m_enType = CTaskDate.Type.TYPE_NULL;

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_COOK, Cook));
	        return true;
        }

        bool OnTaskPrepared(UserInfo pIServerUserItem, CMD_C_Task_Prepared pTaskPrepared)
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
             int wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);

             if (fishInfo.m_TaskObjects[wChairID].m_enType !=CTaskDate.Type.TYPE_COOK)
	         {
		          LogView.AddLogString("OnTaskPrepared") ;
                 return true;
	         }

             fishInfo.m_TaskObjects[wChairID].m_enState = CTaskDate.State.STATE_RUNNING;

             return true;
        }

        bool OnLaserBean(UserInfo pIServerUserItem, CMD_C_Laser_Bean pLaserBean)
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            int wFishID;
            int wRoundID;
            int nSucessCount = 0;
            int wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);
            int nFishInNetCount = pLaserBean.cbCount;

            if (fishInfo.m_TaskObjects[wChairID].m_enType != CTaskDate.Type.TYPE_BEAN)
            {
                LogView.AddLogString("OnLaserBean  ") ;
                return true;
            }

            CMD_S_Laser_Bean_Success LaserBeanSuccess = new CMD_S_Laser_Bean_Success();
            LaserBeanSuccess.wChairID = wChairID;
	        LaserBeanSuccess.fRote = pLaserBean.fRote;

            for (int i=0; i<nFishInNetCount; i++)
            {
                if (nSucessCount >= FishDefine.MAX_FISH_IN_NET)
        	        break;

                wFishID = pLaserBean.FishNetObjects[i].wID;
                wRoundID = pLaserBean.FishNetObjects[i].wRoundID;

                if (fishInfo.m_FishObjects[wFishID].wID!=wFishID || fishInfo.m_FishObjects[wFishID].wRoundID!=wRoundID )
                {
                    continue;
                }
                else
                {
                    LaserBeanSuccess.FishNetObjects[nSucessCount].wID = wFishID;
                    LaserBeanSuccess.FishNetObjects[nSucessCount].wRoundID = wRoundID;

                    nSucessCount++;

                    fishInfo.m_FishObjects[wFishID].wID = FishDefine.INVALID_WORD;
                    fishInfo.m_RoleObjects[wChairID].dwFishGold += FishGameLogic.FishGoldByStyle(fishInfo.m_FishObjects[wFishID].wType,wFishID)*fishInfo.m_RoleObjects[wChairID].dwMulRate;
                }
            }

            LaserBeanSuccess.cbCount = nSucessCount;

            fishInfo.m_TaskObjects[wChairID].m_enType = CTaskDate.Type.TYPE_NULL;

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_LASER_BEAN_SUCCESS, LaserBeanSuccess));
	        return true;
        }

        bool OnChangeCannon(UserInfo pIServerUserItem, CMD_C_Change_Cannon pChangeCannon)
        {  
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            //ASSERT(pChangeCannon.wStyle>=0 && pChangeCannon.wStyle<FishDefine.MAX_CANNON_STYLE);
            if (pChangeCannon.wStyle<0 || pChangeCannon.wStyle>=FishDefine.MAX_CANNON_STYLE)
	        {
		        LogView.AddLogString("OnChangeCannon") ;
		        return true;
	        }

	        int wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);

	        int dwMulRate = pChangeCannon.dwMulRate == FishDefine.INVALID_WORD                       ? FishDefine.INVALID_WORD                        : 
	                          pChangeCannon.dwMulRate > fishInfo.m_RoleObjects[wChairID].dwMaxMulRate ? 5                                    :
	                          pChangeCannon.dwMulRate < 5                                    ? fishInfo.m_RoleObjects[wChairID].dwMaxMulRate : pChangeCannon.dwMulRate;

            fishInfo.m_RoleObjects[wChairID].wCannonType = pChangeCannon.wStyle;
	        fishInfo.m_RoleObjects[wChairID].dwMulRate = dwMulRate;

            CMD_S_Change_Cannon ChangeCannon = new CMD_S_Change_Cannon();

            ChangeCannon.wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);
            ChangeCannon.wStyle = pChangeCannon.wStyle;
	        ChangeCannon.dwMulRate = dwMulRate;
	        ChangeCannon.dwMaxMulRate = fishInfo.m_RoleObjects[wChairID].dwMaxMulRate;
	        ChangeCannon.dwExpValue = fishInfo.m_RoleObjects[wChairID].dwExpValue;

            int nVIPflag = FishTable.GetPrivateProfileInt(TEXT("ScoreCtrl"), TEXT("VIPflag"), 0, fishInfo.m_szConfigFileName);
	        if(nVIPflag==1)
	        {
		        ChangeCannon.dwLevel = 10;
	        }
	        else
	        {
		        ChangeCannon.dwLevel = ((ChangeCannon.dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL) >= FishDefine.MAX_CANNON_LEVEL) ? FishDefine.MAX_CANNON_LEVEL - 1 : (ChangeCannon.dwExpValue/FishDefine.EXP_CHANGE_TO_LEVEL);
	        }

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_CHANGE_CANNON, ChangeCannon));
	        return true;
        }

        bool OnAccount(UserInfo pIServerUserItem, CMD_C_Account pAccount)
        { 
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
	        int wChairID = _GameTable.GetPlayerIndex(pIServerUserItem);
            //fishInfo.m_pITableFrame.WriteUserScore(userInfo,fishInfo.m_RoleObjects[wChairID].dwFishGold*fishInfo.m_pGameServiceOption.lCellScore,0,fishInfo.m_pGameServiceOption.wCharge,enScoreKind_Draw,false);
		
		    int nScore = fishInfo.m_RoleObjects[wChairID].dwFishGold*fishInfo.m_nCellScore;
		    if(nScore!=0)
		    {
                int userIndex = _GameTable.GetPlayerIndex(pIServerUserItem);
                fishInfo.m_lUserWinScore[userIndex] = nScore;

                Cash.GetInstance().ProcessGameCash(userIndex, _GameTable._GameInfo, fishInfo);
                fishInfo.m_lUserWinScore[userIndex] = 0;
		    }
	
            //	fishInfo.m_pITableFrame.WriteUserScore(userInfo,fishInfo.m_RoleObjects[wChairID].dwFishGold*fishInfo.m_pGameServiceOption.lCellScore,0,enScoreKind_Draw);
    
	        fishInfo.m_RoleObjects[wChairID].dwFishGold = 0;

            CMD_S_Account Account = new CMD_S_Account();
            Account.wChairID = wChairID;

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_ACCOUNT, Account));
	        return true;
        }


        bool SendNormalFishObject(bool bInit) 
        {
            SendPointPathNormalFishObjects();

            SendGroupPointPathNormalFishObjects();
            //SendGroupPointPathNormalFishObjects();
            //SendGroupPointPathNormalFishObjects();

	        if (!bInit)
               _GameTable.AddGameTimer(FishDefine.IDI_NORMAL_ADD_FISH_END,5000 / 1000);

	        return true;
        }

        bool SendGroupPointPathNormalFishObjects()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            CMD_S_Send_Group_Point_Path_Fish SendGroupPath = new CMD_S_Send_Group_Point_Path_Fish();

            SendGroupPath.wPath = rand()%FishDefine.MAX_SMALL_POINT_PATH;
            SendGroupPath.cbPahtType = rand()%2+1; // 跟随
            SendGroupPath.dwTime = 16;
            SendGroupPath.cbCount = rand()%6+6;

            int nFreeFishID;
            int nFishType = rand()%4;
            long wFishTime = time();

            for (int i=0; i<SendGroupPath.cbCount; i++)
            {
                nFreeFishID = GetFreeFishID();
                if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
                {
                    LogView.AddLogString("SendGroupPointPathNormalFishObjects nFreeFishID no id");
                    return true;
                }

                SendGroupPath.FishNetObject[i].wID = nFreeFishID;
                SendGroupPath.FishNetObject[i].wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
                SendGroupPath.FishNetObject[i].wType = nFishType;
                SendGroupPath.FishNetObject[i].dwTime = wFishTime;

                fishInfo.m_FishObjects[nFreeFishID] = SendGroupPath.FishNetObject[i];
            }

	        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_GROUP_POINT_PATH_FISH, SendGroupPath));
	        return true;
        }

        bool SendPointPathNormalFishObjects()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            CMD_S_Send_Point_Path_Fish SendPointPath = new CMD_S_Send_Point_Path_Fish();

            SendPointPath.cbCount = 0;

            int nFreeFishID;
            int wFishType ;
            int wFishPath;
            long wFishTime = time();

            int[] nHugeFishPath = new int[FishDefine.MAX_HUGE_POINT_PATH];
            RandNumer(nHugeFishPath, FishDefine.MAX_HUGE_POINT_PATH);
            int[] nBigFishPath = new int[FishDefine.MAX_BIG_POINT_PATH];
            RandNumer(nBigFishPath, FishDefine.MAX_BIG_POINT_PATH);
            int[] nSmallFishPath = new int[FishDefine.MAX_SMALL_POINT_PATH];
            RandNumer(nSmallFishPath, FishDefine.MAX_SMALL_POINT_PATH);

            for (int i=0; i<15; i++)
            {
                nFreeFishID = GetFreeFishID();

		        //限制大鱼
		        switch(i)
		        {
			        case 11:if(fishInfo.m_cbSendFish11>0) {wFishType = i; fishInfo.m_cbSendFish11--;} else wFishType = rand()%11; break;
			        case 12:if(fishInfo.m_cbSendFish12>0) {wFishType = i; fishInfo.m_cbSendFish12--;} else wFishType = rand()%11; break;
			        case 13:if(fishInfo.m_cbSendFish13>0) {wFishType = i; fishInfo.m_cbSendFish13--;} else wFishType = rand()%11; break;
			        case 14:if(fishInfo.m_cbSendFish14>0) {wFishType = i; fishInfo.m_cbSendFish14--;} else wFishType = rand()%11; break;
			        default:wFishType = i;break;
		        }

                if (wFishType >= 7)
                {
                    wFishPath = nHugeFishPath[i];
                }
                else if (wFishType > 3 && wFishType < 7)
                {

                    wFishPath = nBigFishPath[i];
                }
                else
                {
                    wFishPath = nSmallFishPath[i];
                }


                if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
                {
                    LogView.AddLogString("SendPointPathNormalFishObjects nFreeFishID no id");
                    return true;
                }

                SendPointPath.FishPaths[SendPointPath.cbCount].FishNetObject.wID = nFreeFishID;
                SendPointPath.FishPaths[SendPointPath.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
                SendPointPath.FishPaths[SendPointPath.cbCount].FishNetObject.wType = wFishType;
                SendPointPath.FishPaths[SendPointPath.cbCount].FishNetObject.dwTime = wFishTime;
                SendPointPath.FishPaths[SendPointPath.cbCount].wPath = wFishPath;
		        SendPointPath.FishPaths[SendPointPath.cbCount].dwTime = (wFishType==13) ? 88 : 16;

                fishInfo.m_FishObjects[nFreeFishID] = SendPointPath.FishPaths[SendPointPath.cbCount].FishNetObject;
                SendPointPath.cbCount++;
            }

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_POINT_PATH_FISH, SendPointPath ));
	        return true;
        }

        bool SendSpecialPointPathNormalFishObject()
        {
            return true;
        }

        bool SendSpecialFishObject()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

            if (fishInfo.m_cbScene == 0)
            {
		        SendNormalFishObject(true);
                //SendSpecialFishObject0();
                _GameTable.AddGameTimer(FishDefine.IDI_NORMAL_ADD_FISH_END,3000 / 1000);
            }
            else if (fishInfo.m_cbScene == 1)
            {
		        SendNormalFishObject(true);
                //SendSpecialFishObject1();
                _GameTable.AddGameTimer(FishDefine.IDI_NORMAL_ADD_FISH_END,2000 / 1000);
            }
            else
            {
		        SendNormalFishObject(true);
                //SendSpecialFishObject2();
                _GameTable.AddGameTimer(FishDefine.IDI_NORMAL_ADD_FISH_END,2000 / 1000);
            }

            return true;
        }

        bool SendSpecialFishObject0()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            CMD_S_Send_Special_Point_Path SpecialPointPath = new CMD_S_Send_Special_Point_Path();

            SpecialPointPath.cbCount = 0;

            int nFreeFishID;
            int wFishPath = 0;
            int wFishType = rand()%2+8;
            long wFishTime = time();

            for (int i=0; i<4; i++)
            {
                wFishPath = i;

                for (int j=0; j<5; j++)
                {
                    nFreeFishID = GetFreeFishID();
			        if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
                    {
                        LogView.AddLogString("SendSpecialFishObject0 nFreeFishID no id");
                        return true;
                    }

                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.wID = nFreeFishID;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.wType = wFishType;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.dwTime = wFishTime;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].wPath = wFishPath;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].dwTime = 16;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].fDelay = 0.8f*j+0.1f;

                    fishInfo.m_FishObjects[nFreeFishID] = SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject;
                    SpecialPointPath.cbCount++;
                }
            }

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_SPECIAL_POINT_PATH, SpecialPointPath));
	        return true;
        }

        bool SendSpecialFishObject1()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            CMD_S_Send_Special_Point_Path SpecialPointPath = new CMD_S_Send_Special_Point_Path();

            SpecialPointPath.cbCount = 0;

            int nFreeFishID;
            int wFishPath = 0;
            int wFishType = 8;
            long wFishTime = time();

            for (int i=0; i<13; i++)
            {
                wFishPath = 4+i;

                for (int j=0; j<3; j++)
                {
                    nFreeFishID = GetFreeFishID();
                    if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
                    {
                        LogView.AddLogString("SendSpecialFishObject1 nFreeFishID no id");
                        return true;
                    }
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.wID = nFreeFishID;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.wType = wFishType;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.dwTime = wFishTime;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].wPath = wFishPath;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].dwTime = 16;
                    SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].fDelay = 0.8f*j+0.1f;

                    fishInfo.m_FishObjects[nFreeFishID] = SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject;
                    SpecialPointPath.cbCount++;
                }
            }

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_SPECIAL_POINT_PATH, SpecialPointPath));
	        return true;
        }

        bool SendSpecialFishObject2()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            CMD_S_Send_Special_Point_Path SpecialPointPath = new CMD_S_Send_Special_Point_Path();

            SpecialPointPath.cbCount = 0;

            int nFreeFishID;
            int wFishPath = 16;
            int wFishType = 0;
            long wFishTime = time();


            for (int m=0; m<4; m++)
            {
                SpecialPointPath.cbCount = 0;

                for (int i=0; i<16; i++)
                    for (int j=0; j<3; j++)
                    {
                        wFishPath = 16+j+m*3;
                        nFreeFishID = GetFreeFishID();
                        if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
                        {
                            LogView.AddLogString(" SendSpecialFishObject2 nFreeFishID no id");
                            return true;
                        }

                        SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.wID = nFreeFishID;
                        SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
                        SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.wType = wFishType;
                        SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject.dwTime = wFishTime;
                        SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].wPath = wFishPath;
                        SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].dwTime = 16;
                        SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].fDelay = 0.2f*i+0.1f;

                        fishInfo.m_FishObjects[nFreeFishID] = SpecialPointPath.SpecialPointPath[SpecialPointPath.cbCount].FishNetObject;
                        SpecialPointPath.cbCount++;
                    }


                    _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_SPECIAL_POINT_PATH, SpecialPointPath));
            }

            return true;
        }

        bool SendSceneFishObject()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        if (fishInfo.m_wSceneSendFishCount<1)
	        {
		        //启动，批量发送大鱼
		        fishInfo.m_cbSendFishType = ((rand()%100)<50) ? 0 : 1;
		        fishInfo.m_dwSendFishCount = 0;
		        _GameTable.AddGameTimer(FishDefine.IDI_ADD_SCENE_FISH_SEND_COUNT,(int)FishInfo.m_dwFishSendTime[fishInfo.m_dwSendFishCount]/1000);

		        //启动，批量发送
		        fishInfo.m_dwSendFishSmallCount = 0;
		        fishInfo.m_cbSendSmallFishType =  rand()%4 + 2;
		        _GameTable.AddGameTimer(FishDefine.IDI_ADD_SCENE_FISH_SEND_SMALL_COUNT,1);
		        fishInfo.m_bIsSceneing = true;

	        }
	        else if(fishInfo.m_wSceneSendFishCount==2)
	        {
		        SendSceneLeftFishObject();
		        fishInfo.m_wSceneSendFishCount++;
		        _GameTable.AddGameTimer(FishDefine.IDI_SCENE_ADD_FISH_END,16000/1000);
	        }
	        else if (fishInfo.m_wSceneSendFishCount>2 && fishInfo.m_wSceneSendFishCount < 6)
	        {
		        SendSceneRightFishObject();
		        fishInfo.m_wSceneSendFishCount++;
		        _GameTable.AddGameTimer(FishDefine.IDI_SCENE_ADD_FISH_END,4000/1000);
	        }
	        else
	        {
		        fishInfo.m_bIsSceneing = true;
		        ResetBigFishSend();
		        ClearAllSmallFishGroup();
		        fishInfo.m_wSceneSendFishCount = 0;
		        _GameTable.AddGameTimer(FishDefine.IDI_SCENE_START,(fishInfo.m_cbSendFishType==0) ? 26000/1000 : 22000/1000);
	        }


	        return true;
        }

        bool SendSceneFishCountObject()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        int nFreeFishID;
	        long wFishTime = time();
	        CMD_S_Send_Line_Path_Fish SendLinePathFish = new CMD_S_Send_Line_Path_Fish();
	        SendLinePathFish.cbCount = 0;

	        Point ptBegin1,ptBegin2,ptEnd1,ptEnd2;

	        if((fishInfo.m_cbScene%2)==0)
	        {
		        if(fishInfo.m_cbSendFishType==0)
		        {
			        ptBegin1 = new Point(-800,260);
			        ptBegin2 = new Point(-800,460);

			        ptEnd1   = new Point(1880,260);
			        ptEnd2   = new Point(1880,460);
		        }
		        else
		        {
			        ptBegin1 = new Point(-800,320);
			        ptBegin2 = new Point(-800,320);

			        ptEnd1   = new Point(1880,  0);
			        ptEnd2   = new Point(1880,738);

		        }
	        }
	        else
	        {
		        if(fishInfo.m_cbSendFishType==0)
		        {
			        ptBegin1 = new Point(1880,260);
			        ptBegin2 = new Point(1880,460);

			        ptEnd1   = new Point(-800,260);
			        ptEnd2   = new Point(-800,460);
		        }
		        else
		        {
			        ptBegin1 = new Point(1880,320);
			        ptBegin2 = new Point(1880,320);

			        ptEnd1   = new Point(-800 , 0);
			        ptEnd2   = new Point(-800,738);

		        }
	        }

	        for(int i=0;i<2;i++)
	        {
		        Point ptMoveBegin = (i==0) ? ptBegin1 : ptBegin2;
		        Point ptMoveEnd   = (i==0) ? ptEnd1   : ptEnd2;

		        nFreeFishID = GetFreeFishID();if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT) return true;

		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wID      = nFreeFishID;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wType    = FishInfo.m_nFishType[fishInfo.m_dwSendFishCount];
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.dwTime   = wFishTime;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x     = ptMoveBegin.X;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.y     = ptMoveBegin.Y;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.x       = ptMoveEnd.X;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.y       = ptMoveEnd.Y;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.dwTime        = 30;

		        fishInfo.m_FishObjects[nFreeFishID] = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject;
		        SendLinePathFish.cbCount++;
	        }

	        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_LINE_PATH_FISH, SendLinePathFish));
	        return true;
        }

        bool SendSceneLeftFishObject()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
	        int nFreeFishID;
	        long wFishTime = time();
	        CMD_S_Send_Line_Path_Fish SendLinePathFish = new CMD_S_Send_Line_Path_Fish();
	        SendLinePathFish.cbCount = 0;

	        //----0----1----2----3----4----5----6----7---|8----9----10----11----12----
	        //    1倍  2倍  3倍  4倍  5倍  7倍  9倍 10倍  12倍 15倍  20倍  50倍 120倍
	        int[] nFishType     = new int[]{  8,  8,  8,  8,    9,  9,  9,   10,10,  10,   11, 11, 11,   12, 12, 12,   14,  0,0};
	        int[] nFishWidth    = new int[]{256,256,256,256,  256,256,256,  278,278,278,  512,512,512,  512,512,512,  512,  0,0};
	        int[] nFishHeight   = new int[]{180,180,180,180,  256,256,256,  160,160,160,  256,256,256,  256,256,256,  411,  0,0};
	        int[] nFishBaseOff1 = new int[]{160,160,160,160,  128,128,128,  160,160,160,  128,128,128,  128,128,128,  128,  0,0};
	        int[] nFishBaseOff2 = new int[]{160,160,160,160,  128,128,128,  160,160,160,  128,128,128,  128,128,128,  128,  0,0};


	        int nLineLen = 0;
	        for(int i=0;i<17;i++) nLineLen += (nFishWidth[i] + 30);

	        //Point ptBegin1 = Point(1380, 80);
	        //Point ptBegin2 = Point(1380,580);

	        //Point ptEnd1   = Point(-800 - nLineLen, 80);
	        //Point ptEnd2   = Point(-800 - nLineLen,580);

	        Point ptBegin1 = new Point(1380,300);
	        Point ptBegin2 = new Point(1380,300);

	        Point ptEnd1   = new Point(-800 - nLineLen,-1000);
	        Point ptEnd2   = new Point(-800 - nLineLen, 2000);

	        int nLen = 0;

	        for (int i=0; i<17; i++)
	        {
		        for(int j=0;j<2;j++)
		        {
			        Point ptMoveBegin = (j==0) ? ptBegin1 : ptBegin2;
			        Point ptMoveEnd   = (j==0) ? ptEnd1   : ptEnd2;

			        int nBaseOff = 0;//(j==0) ? nFishBaseOff1[i] : -nFishBaseOff2[i];

			        nFreeFishID = GetFreeFishID();if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT) return true;

			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wID      = nFreeFishID;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wType    = nFishType[i];
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.dwTime   = wFishTime;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x     = ptMoveBegin.X + nLen;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.y     = ptMoveBegin.Y + nBaseOff;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.x       = ptMoveEnd.X + nLen;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.y       = ptMoveEnd.Y + nBaseOff;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.dwTime        = 88;

			        fishInfo.m_FishObjects[nFreeFishID] = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject;
			        SendLinePathFish.cbCount++;
		        }

		        nLen += (nFishWidth[i] + 30);
	        }

            _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_LINE_PATH_FISH, SendLinePathFish));
	        return true;
        }

        bool SendSceneRightFishObject()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        int nFreeFishID;
	        long wFishTime = time();
	        CMD_S_Send_Line_Path_Fish SendLinePathFish = new CMD_S_Send_Line_Path_Fish();
	        SendLinePathFish.cbCount = 0;

	        //----0----1----2----3----4----5----6----7----8----9----10----11----12----
	        //    1倍  2倍  3倍  4倍  5倍  7倍  9倍 10倍 12倍 15倍  20倍  50倍 120倍
	        int[] nFishCnt  = { 4,4,  3,3,  3,3,  3,3,   3,3,   1,1,   0,0};
	        int[] nFishType = { 8,8,  9,9, 10,10, 11,11, 12,12, 14,14, 0,0};

	        int[] nFishWidth  = {256,256,  256,256,  278,278,  512,512,  512,512,  512,512,  512,512};
	        int[] nFishHeight = {180,180,  256,256,  160,160,  256,256,  256,256,  411,411,  512,512};

	        int xPos = 0;
	        int yPos = 0;

	        int nLoop = 12;

	        int nLen = 0;
	        for(int i=0;i<nLoop;i++) nLen +=  nFishWidth[i];nLen += 22000;

	        for (int i=0; i<nLoop; i++)
	        {
		        yPos  = (738-nFishCnt[i]*nFishHeight[i]/2)/2;
		        xPos += nFishWidth[i]/2;

		        for(int j=0;j<nFishCnt[i];j++)
		        {
			        yPos += nFishHeight[i]/4;

			        nFreeFishID = GetFreeFishID();
			        if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
			        {
				        LogView.AddLogString(" SendSceneRightFishObject nFreeFishID no id");
				        return true;
			        }

			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wID      = nFreeFishID;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wType    = nFishType[i];
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.dwTime   = wFishTime;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x     = -xPos;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.y     = yPos;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.x       = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x + nLen;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.y       = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.y;
			        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.dwTime        = 188;


			        fishInfo.m_FishObjects[nFreeFishID] = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject;
			        SendLinePathFish.cbCount++;
		        }
	        }

	        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_LINE_PATH_FISH, SendLinePathFish));
	        return true;
        }

        bool SendSceneBottomSmallFishObject()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        int nFreeFishID;
	        long wFishTime = time();
	        CMD_S_Send_Line_Path_Fish SendLinePathFish = new CMD_S_Send_Line_Path_Fish();
	        SendLinePathFish.cbCount = 0;
	        SendLinePathFish.cbForward = 1;

	        for(int i=0;i<30;i++)
	        {
		        nFreeFishID = GetFreeFishID();
		        if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
		        {
			        LogView.AddLogString(" SendSceneRightFishObject nFreeFishID no id");
			        return true;
		        }

		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wID      = nFreeFishID;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wType    = fishInfo.m_cbSendSmallFishType;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.dwTime   = wFishTime;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x     = FishInfo.m_dwSmallFishBase[i,fishInfo.m_dwSendFishSmallCount];
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.y     = 1024;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.x       = FishInfo.m_dwSmallFishBase[i,fishInfo.m_dwSendFishSmallCount];
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.y       = 580;// + fishInfo.m_dwSendFishSmallCount*40;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.dwTime        = 6;

		        fishInfo.m_wSmallFishGroupBottom[fishInfo.m_dwSendFishSmallCount,i].wFishID  = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wID;
		        fishInfo.m_wSmallFishGroupBottom[fishInfo.m_dwSendFishSmallCount,i].wRoundID = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wRoundID;
		        fishInfo.m_FishObjects[nFreeFishID] = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject;
		        SendLinePathFish.cbCount++;
	        }


	        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_LINE_PATH_SMALL_BOTTOM_FISH, SendLinePathFish));
	        return true;
        }

        bool SendSceneTopSmallFishObject()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        int nFreeFishID;
	        long wFishTime = time();
	        CMD_S_Send_Line_Path_Fish SendLinePathFish = new CMD_S_Send_Line_Path_Fish();
	        SendLinePathFish.cbCount = 0;
	        SendLinePathFish.cbForward = 0;


	        for(int i=0;i<30;i++)
	        {
		        nFreeFishID = GetFreeFishID();
		        if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
		        {
			        LogView.AddLogString(" SendSceneRightFishObject nFreeFishID no id");
			        return true;
		        }

		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wID      = nFreeFishID;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wType    = fishInfo.m_cbSendSmallFishType;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.dwTime   = wFishTime;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x     = FishInfo.m_dwSmallFishBase[i,fishInfo.m_dwSendFishSmallCount];
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.y     = -600;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.x       = FishInfo.m_dwSmallFishBase[i,fishInfo.m_dwSendFishSmallCount];
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.y       = 150;// - fishInfo.m_dwSendFishSmallCount*40;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.dwTime        = 6;

		        fishInfo.m_wSmallFishGroupTop[fishInfo.m_dwSendFishSmallCount,i].wFishID  = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wID;
		        fishInfo.m_wSmallFishGroupTop[fishInfo.m_dwSendFishSmallCount,i].wRoundID = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wRoundID;
		        fishInfo.m_FishObjects[nFreeFishID] = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject;
		        SendLinePathFish.cbCount++;
	        }

	        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_LINE_PATH_SMALL_BOTTOM_FISH, SendLinePathFish));
	        return true;
        }

        bool SendSceneBottomSmallFishObject1()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        int nFreeFishID;
	        long wFishTime = time();
	        CMD_S_Send_Line_Path_Fish SendLinePathFish = new CMD_S_Send_Line_Path_Fish();
	        SendLinePathFish.cbCount = 0;
	        SendLinePathFish.cbForward = 3;

	        int nNum = 2 + rand()%10;
	        for(int i=0;i<nNum;i++)
	        {
		        nFreeFishID = GetFreeFishID();
		        if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
		        {
			        LogView.AddLogString(" SendSceneRightFishObject nFreeFishID no id");
			        return true;
		        }

		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wID      = nFreeFishID;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wType    = (fishInfo.m_cbSendFishType==0) ? fishInfo.m_cbSendSmallFishType : rand()%4 + 2;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.dwTime   = wFishTime;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x     = FishInfo.m_dwSmallFishBase[rand()%28,rand()%6];
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.y     = 1024 + rand()%800;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.x       = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.y       = 580;// + fishInfo.m_dwSendFishSmallCount*40;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.dwTime        = 8 + rand()%28;

		        fishInfo.m_FishObjects[nFreeFishID] = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject;
		        SendLinePathFish.cbCount++;
	        }


	        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_LINE_PATH_SMALL_BOTTOM_FISH, SendLinePathFish));
	        return true;
        }

        bool SendSceneTopSmallFishObject1()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        int nFreeFishID;
	        long wFishTime = time();
	        CMD_S_Send_Line_Path_Fish SendLinePathFish = new CMD_S_Send_Line_Path_Fish();
	        SendLinePathFish.cbCount = 0;
	        SendLinePathFish.cbForward = 2;

	        int nNum = 2 + rand()%10;
	        for(int i=0;i<nNum;i++)
	        {
		        nFreeFishID = GetFreeFishID();
		        if (nFreeFishID>=FishDefine.MAX_FISH_OBJECT)
		        {
			        LogView.AddLogString(" SendSceneRightFishObject nFreeFishID no id");
			        return true;
		        }

		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wID      = nFreeFishID;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wRoundID = fishInfo.m_FishObjects[nFreeFishID].wRoundID+1;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.wType    = (fishInfo.m_cbSendFishType==0) ? fishInfo.m_cbSendSmallFishType : rand()%4 + 2;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject.dwTime   = wFishTime;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x     = FishInfo.m_dwSmallFishBase[rand()%28,rand()%6];
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.y     = -600 - rand()%800;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.x       = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spStart.x;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.spEnd.y       = 150;// - fishInfo.m_dwSendFishSmallCount*40;
		        SendLinePathFish.FishPaths[SendLinePathFish.cbCount].LinePath.dwTime        = 12 + rand()%28;

		        fishInfo.m_FishObjects[nFreeFishID] = SendLinePathFish.FishPaths[SendLinePathFish.cbCount].FishNetObject;
		        SendLinePathFish.cbCount++;
	        }

	        _GameTable.BroadCastGame( NotifyType.Reply_FishSend, new FishSendInfo( FishDefine.SUB_S_SEND_LINE_PATH_SMALL_BOTTOM_FISH, SendLinePathFish));
	        return true;
        }

        int GetFreeFishID()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

            for (int i=0; i<FishDefine.MAX_FISH_OBJECT; i++)
            {
                if (fishInfo.m_FishObjects[i].wID == FishDefine.INVALID_WORD)
                {
                    return i;
                }
            }

            return FishDefine.MAX_FISH_OBJECT;
        }

        int GetFreeFishCount()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
            int count = 0;

            for (int i = 0; i < FishDefine.MAX_FISH_OBJECT; i++)
            {
                if (fishInfo.m_FishObjects[i].wID == FishDefine.INVALID_WORD)
                {
                    count++;
                }
            }

            return count;
        }


        void CheckFishDestroy()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

            long dwTime = time();

            for (int i=0; i<FishDefine.MAX_FISH_OBJECT; i++)
            {
                if (fishInfo.m_FishObjects[i].wID == i)
                {
                    if (dwTime-fishInfo.m_FishObjects[i].dwTime >= FishDefine.FISH_DESTROY_TIME)
                    {
                        fishInfo.m_FishObjects[i].wID = FishDefine.INVALID_WORD;
                    }
                }
            }
        }

        public static long time()
        {
            return FishDefine.time();
        }


        void RandNumer(int[] nPaths, int nCount)
        {
            for(int i=0; i<nCount; ++i) nPaths[i]=i;
            for(int i=nCount-1; i>=1; --i) 
            {
                int randIndex = rand()%i;

                int temp = nPaths[i];
                nPaths[i] = nPaths[randIndex];
                nPaths[randIndex] = temp;
            }
        }

        void RandNumer1(int[] nPaths, int nCount)
        {
	        for(int i=0;i<nCount;i++)
	        {
		        nPaths[i]=rand()%4;
		        for(int j=0;j<i;j++)
			        while(nPaths[j]==nPaths[i])
			        {
				        nPaths[i]=rand()%4;
				        j=0;
			        }
	        }
        }


        string GetCurExeFilePath()
        {
            return System.Windows.Forms.Application.ExecutablePath;
        }



        int  ReadFireValue(int wChair,int cbType)					//读库存
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
	        UserInfo pIServerUser = fishInfo._Players[wChair];
	        if(pIServerUser==null) return 0;

	        string OutBuf = string.Empty;
	        //memset(OutBuf,0,255);

	        int lRet = 0;
	        if(cbType==1)		FishTable.GetPrivateProfileString(TEXT("ScoreCtrl"),TEXT("ScoreStockBig"),TEXT("0"),OutBuf ,255,fishInfo.m_szConfigFileName);
	        else if(cbType==0)  FishTable.GetPrivateProfileString(TEXT("ScoreCtrl"),TEXT("ScoreStockSmall"),TEXT("0"),OutBuf ,255,fishInfo.m_szConfigFileName);
	        else if(cbType==2)  FishTable.GetPrivateProfileString(TEXT("ScoreCtrl"),TEXT("ScoreStockAll"),TEXT("0"),OutBuf ,255,fishInfo.m_szConfigFileName);
	        //pakcj _sntscanf(OutBuf,lstrlen(OutBuf),TEXT("%I64d"),&lRet);

	        //return  fishInfo.m_pScorePoolInfo.lTotalScore;
	        return 100000000;
        }

        bool WriteFireValue(int wChair,int cbType,int lValue)		//写库存
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;
	        UserInfo pIServerUser= fishInfo._Players[wChair];
	        if(pIServerUser==null) return false;

	        if(lValue<0) return false;

	        //string strValue;strValue.Format("%I64d",lValue);

	        //if(cbType==1)		WritePrivateProfileString(TEXT("ScoreCtrl"),TEXT("ScoreStockBig"),strValue,fishInfo.m_szConfigFileName);
	        //else if(cbType==0)  WritePrivateProfileString(TEXT("ScoreCtrl"),TEXT("ScoreStockSmall"),strValue,fishInfo.m_szConfigFileName);
	        //else if(cbType==2)  WritePrivateProfileString(TEXT("ScoreCtrl"),TEXT("ScoreStockAll"),strValue,fishInfo.m_szConfigFileName);
	        //fishInfo.m_pScorePoolInfo.lTotalScore = lValue;

	        return true;
        }

        void ResetBigFishSend()
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        fishInfo.m_cbSendFish11 = 0;
	        fishInfo.m_cbSendFish12 = 0;
	        fishInfo.m_cbSendFish13 = 0;
	        fishInfo.m_cbSendFish14 = 0;

	        int nRand = 1 + rand()%120;
	        _GameTable.AddGameTimer(FishDefine.IDI_ADD_BIG_FISH_11_ENABLE,nRand*1000/1000);
	        nRand = 1 + rand()%120;
	        _GameTable.AddGameTimer(FishDefine.IDI_ADD_BIG_FISH_12_ENABLE,nRand*1000/1000);
	        nRand = 1 + rand()%120;
	        _GameTable.AddGameTimer(FishDefine.IDI_ADD_BIG_FISH_13_ENABLE,nRand*1000/1000);
        }

        void ClearAllSmallFishGroup()	//清空鱼群ID
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        for(int i=0;i<3;i++)
	        {
		        for(int j=0;j<30;j++)
		        {
			        fishInfo.m_wSmallFishGroupBottom[i,j].wFishID   = FishDefine.INVALID_WORD;
			        fishInfo.m_wSmallFishGroupBottom[i,j].wRoundID  = FishDefine.INVALID_WORD;
			        fishInfo.m_wSmallFishGroupTop[i,j].wFishID		= FishDefine.INVALID_WORD;
			        fishInfo.m_wSmallFishGroupTop[i,j].wRoundID		= FishDefine.INVALID_WORD;
		        }
	        }
        }

        void ClearSmallFishGroup(int wFishID)	//清空指定ID鱼
        {
            FishInfo fishInfo = (FishInfo)_GameTable._TableInfo;

	        for(int i=0;i<3;i++)
	        {
		        for(int j=0;j<30;j++)
		        {
			        if(fishInfo.m_wSmallFishGroupBottom[i,j].wFishID==wFishID)
			        {
				        fishInfo.m_wSmallFishGroupBottom[i,j].wFishID  = FishDefine.INVALID_WORD;
				        fishInfo.m_wSmallFishGroupBottom[i,j].wRoundID = FishDefine.INVALID_WORD;
			        }
			
			        if(fishInfo.m_wSmallFishGroupTop[i,j].wFishID==wFishID)
			        {
				        fishInfo.m_wSmallFishGroupTop[i,j].wFishID  = FishDefine.INVALID_WORD;
				        fishInfo.m_wSmallFishGroupTop[i,j].wRoundID = FishDefine.INVALID_WORD;
			        }
		        }
	        }
        }
    }
}
