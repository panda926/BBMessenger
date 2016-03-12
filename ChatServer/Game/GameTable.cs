using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.Net.Sockets;

namespace ChatServer
{
    public class GameRound
    {
        public GameTable _GameTable;

        public int _TimerId;
        public bool _IsLeaveTime = false;

        public virtual bool CanStart()
        {
            return false;
        }

        public virtual void Start()
        {
            _IsLeaveTime = false;

            TableInfo tableInfo = _GameTable.GetTableInfo();

            tableInfo._RoundStartTime = DateTime.Now;

            foreach (UserInfo userInfo in tableInfo._Players)
            {
                if (userInfo.Auto > 0)
                {
                    AutoAction(userInfo);
                }
            }
        }

        public virtual void AutoAction(UserInfo userInfo)
        {
        }

        public virtual void NotifyGameTimer(GameTimer gameTimer)
        {
            if (gameTimer.timerId == _TimerId)
            {
                _IsLeaveTime = true;
            }
        }

        public virtual bool Action(NotifyType notifyType, BaseInfo baseInfo, UserInfo userInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Request_EnterGame:
                    {
                        if (_GameTable.ViewerEnterTable(userInfo) == false)
                        {
                            Main.ReplyError(userInfo.Socket);
                            return false;
                        }

                        Server.GetInstance().Send(userInfo.Socket, NotifyType.Reply_EnterGame, _GameTable._GameInfo);

                        _GameTable.BroadCastGame(NotifyType.Reply_TableDetail, _GameTable._TableInfo);
                    }
                    break;

                case NotifyType.Request_PlayerEnter:
                    {
                        if (_GameTable.PlayerEnterTable(baseInfo, userInfo) == false)
                        {
                            Main.ReplyError(userInfo.Socket);
                            return false;
                        }

                        _GameTable.BroadCastGame(NotifyType.Reply_TableDetail, _GameTable._TableInfo);
                    }
                    break;

                //case NotifyType.Request_PlayerOut:
                //    {
                //        if (_GameTable.GetPlayerIndex(userInfo) < 0)
                //            return true;

                //        foreach (UserInfo outInfo in _GameTable._TableInfo._Outers)
                //        {
                //            if (outInfo == userInfo)
                //                return true;
                //        }

                //            _GameTable._TableInfo._Outers.Add(userInfo);
                //        //}
                //    }
                //    break;

                case NotifyType.Request_OutGame:
                    {
                        _GameTable.OutTable(userInfo);
                    }
                    break;

                case NotifyType.Notify_GameTimer:
                    {
                        GameTimer gameTimer = (GameTimer)baseInfo;

                        if (gameTimer.timerId == _TimerId)
                        {
                            _IsLeaveTime = true;
                        }

                        NotifyGameTimer(gameTimer);
                    }
                    break;

                case NotifyType.Notify_Ping:
                    break;

                default:
                    return false;
            }

            return true;
        }

        //public void BroadCast(string roomId, NotifyType notifyType, BaseInfo sendInfo, string ownerId)
        //{
        //    List<UserInfo> users = Users.GetInstance().GetRoomUsers(roomId);

        //    for (int i = 0; i < users.Count; i++)
        //    {
        //        if (users[i].Id != ownerId)
        //        {
        //            Server.GetInstance().Send(users[i].Socket, notifyType, sendInfo);
        //        }
        //    }
        //}

        public virtual bool CanEnd()
        {
            return false;
        }

        public virtual void End()
        {
        }

        public void ZeroMemory(Array array)
        {
            Array.Clear(array, 0, array.Length);
        }

        private static Random random = new Random();

        public int rand()
        {
            return random.Next();
        }
    }

    public class ReadyRound : GameRound
    {
        public override bool CanStart()
        {
            return true;
        }

        public override void Start()
        {
            TableInfo tableInfo = _GameTable._TableInfo;

            int i = 0;
            while (i < tableInfo._Players.Count)
            {
                UserInfo userInfo = tableInfo._Players[i];

                if (userInfo.nCashOrPointGame == 0)
                {
                    if (userInfo.Cash <= tableInfo.m_lMinScore)
                    {
                        _GameTable.PlayerOutTable(userInfo, userInfo);

                        if (_GameTable.GetPlayerIndex(userInfo) < 0)
                            continue;
                    }
                }
                else
                {
                    if (userInfo.Point <= tableInfo.m_lMinScore)
                    {
                        _GameTable.PlayerOutTable(userInfo, userInfo);

                        if (_GameTable.GetPlayerIndex(userInfo) < 0)
                            continue;
                    }
                }

                i++;
            }

            base.Start();
        }

        public override bool CanEnd()
        {
            TableInfo tableInfo = _GameTable.GetTableInfo();

            if (tableInfo._Players.Count < GetNeedPlayers())
                return false;
            if (_TimerId != 0 && _IsLeaveTime == false)
                return false;

            return true;
        }

        public virtual int GetNeedPlayers()
        {
            return 1;
        }
    }

    public class StartRound : GameRound
    {
        public override bool CanStart()
        {
            return true;
        }

        public override void Start()
        {
            TableInfo tableInfo = (TableInfo)_GameTable.GetTableInfo();

            InitTableData(tableInfo);

            base.Start();
        }

        public virtual void InitTableData(TableInfo tableInfo)
        {
        }
    }

    public class EndRound : GameRound
    {
        public override bool CanStart()
        {
            TableInfo tableInfo = (TableInfo)_GameTable.GetTableInfo();

            if (tableInfo._Players.Count <= 0)
                return false;

            return true;
        }

        public override void Start()
        {
            TableInfo tableInfo = (TableInfo)_GameTable.GetTableInfo();

            CheckWinner();

            CheckScore();

            for (int i = 0; i < tableInfo._Players.Count; i++)
            {
                Cash.GetInstance().ProcessGameCash(i, _GameTable._GameInfo, tableInfo);
            }

            _TimerId = TimerID.End;
            _GameTable.AddGameTimer(_TimerId, tableInfo.m_EndingTime);

            base.Start();
        }

        public override bool CanEnd()
        {
            if (_IsLeaveTime == false)
                return false;

            return true;
        }

        public virtual void CheckWinner()
        {
        }

        public virtual void CheckScore()
        {
        }

        public override void End()
        {
            TableInfo tableInfo = _GameTable._TableInfo;

            if (tableInfo._Players.Count > 0)
            {
                Random rnd = new Random();

                int nIndex = rnd.Next(0, tableInfo._Players.Count - 1);

                int i = 0;
                while (i < tableInfo._Players.Count)
                {
                    UserInfo userInfo = tableInfo._Players[i];

                    if (i == nIndex && userInfo.Auto > 0)
                    {
                        if (rnd.Next() % 8 == 0)
                        {
                            _GameTable.OutTable(userInfo);
                            break;
                        }
                    }

                    i++;
                }
            }

            base.End();
        }
    }

    public static class TimerID
    {
        public static int Join = 1;
        public static int Betting = 2;
        public static int End = 3;
        public static int Custom = 1000;
    }

    public class GameTimer : BaseInfo
    {
        public int timerId = 0;
        public DateTime startTime;
        public int delaySecond = 0;

        public UserInfo autoInfo;
    }

    // added by usc at 2014/02/27
    public class CurBettingInfo
    {
        public int nMinArea = 0;
        public int nMinScore = 0;

        public int nMaxArea = 0;
        public int nMaxScore = 0;
    }

    public class GameTable
    {
        public GameInfo _GameInfo;
        public TableInfo _TableInfo;

        public List<GameRound> _Rounds = new List<GameRound>();
        public List<GameTimer> _Timers = new List<GameTimer>();

        public List<UserInfo> _AutoEners = new List<UserInfo>();

        //public Object _Lock = new Object();

        public GameTable()
        {
        }

        public GameTable(string tableId)
        {
        }

        public TableInfo GetTableInfo()
        {
            return _TableInfo;
        }

        public bool Run(NotifyType notifyType, BaseInfo baseInfo, UserInfo userInfo)
        {
            //lock (this._Lock)
            {
                GameRound curRound = _Rounds[_TableInfo._RoundIndex];

                bool ret = curRound.Action(notifyType, baseInfo, userInfo);

                if (ret == false)
                    return false;

                while (true)
                {
                    if (curRound != null)
                    {
                        if (curRound.CanEnd() == false)
                            break;

                        curRound.End();
                    }

                    _TableInfo._RoundIndex = (_TableInfo._RoundIndex + 1) % _Rounds.Count;

                    curRound = _Rounds[_TableInfo._RoundIndex];

                    if (curRound.CanStart() == true)
                    {
                        curRound.Start();
                        BroadCastGame(NotifyType.Reply_TableDetail, _TableInfo);

                        View._isUpdateGameList = true;
                    }
                    else
                        curRound = null;
                }

                return ret;
            }
        }

        public void AddGameTimer(int timerId, int dealySecond)
        {
            GameTimer gameTimer = new GameTimer();

            gameTimer.timerId = timerId;
            gameTimer.delaySecond = dealySecond;
            gameTimer.startTime = DateTime.Now;

            _Timers.Add(gameTimer);
        }

        public void RemoveGameTimer(int timerId)
        {
            int i = 0;
            while (i < _Timers.Count)
            {
                GameTimer gameTimer = _Timers[i];

                if (gameTimer.timerId == timerId)
                {
                    _Timers.Remove(gameTimer);
                    break;
                }
                i++;
            }
        }

        public void AddAutoTimer(UserInfo userInfo, int dealySecond)
        {
            GameTimer gameTimer = new GameTimer();

            gameTimer.timerId = TimerID.Custom;
            gameTimer.delaySecond = dealySecond;
            gameTimer.startTime = DateTime.Now;
            gameTimer.autoInfo = userInfo;

            _Timers.Add(gameTimer);
        }

        //public GameTimer FindGameTimer(int timerId)
        //{
        //    GameTimer gameTimer = null;

        //    for (int i = 0; i < _Timers.Count; i++)
        //    {
        //        if (_Timers[i].timerId == timerId)
        //        {
        //            gameTimer = _Timers[i];
        //            break;
        //        }
        //    }

        //    return gameTimer;
        //}

        public void ProcessTimer()
        {
            int i = 0;

            if (_Timers.Count == 0)
            {
                Run(NotifyType.Notify_Ping, null, null);
                return;
            }

            while (i < _Timers.Count)
            {
                GameTimer gameTimer = _Timers[i];

                if ((DateTime.Now - gameTimer.startTime).TotalSeconds > gameTimer.delaySecond)
                {
                    _Timers.Remove(gameTimer);
                    Run(NotifyType.Notify_GameTimer, gameTimer, null);
                    continue;
                }
                i++;
            }

        }

        public int GetPlayerIndex(UserInfo userInfo)
        {
            for (int i = 0; i < _TableInfo._Players.Count; i++)
            {
                if (_TableInfo._Players[i].Id == userInfo.Id)
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetViewerIndex(UserInfo userInfo)
        {
            for (int i = 0; i < _TableInfo._Viewers.Count; i++)
            {
                if (_TableInfo._Viewers[i] == userInfo)
                {
                    return i;
                }
            }

            return -1;
        }

        public virtual bool PlayerEnterTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            _AutoEners.Remove(userInfo);

            if (GetPlayerIndex(userInfo) >= 0)
                return true;

            if (userInfo.nCashOrPointGame == 0)
            {
                if (userInfo.Cash <= _TableInfo.m_lMinScore)
                {
                    BaseInfo.SetError(ErrorType.Notenough_Cash, "캐쉬가 부족하여 게임을 할수 없습니다.");
                    return false;
                }
            }
            else
            {
                if (userInfo.Point <= _TableInfo.m_lMinScore)
                {
                    BaseInfo.SetError(ErrorType.Notenough_Cash, "포인트가 부족하여 게임을 할수 없습니다.");
                    return false;
                }
            }

            if (_TableInfo._Players.Count >= GameDefine.GAME_PLAYER)
            {
                BaseInfo.SetError(ErrorType.Full_Gamer, "인원이 꽉 찼습니다.");
                return false;
            }

            _TableInfo._Players.Add(userInfo);
            _TableInfo._Viewers.Remove(userInfo);

            View._isUpdateGameList = true;

            if (_GameInfo.nCashOrPointGame == 0 && userInfo.Auto == 0 && userInfo.Kind == (int)UserKind.Buyer)
            {
                GameHistoryInfo gameHistoryInfo = new GameHistoryInfo();

                gameHistoryInfo.GameId = _GameInfo.GameId;
                gameHistoryInfo.GameSource = _GameInfo.GameName;
                gameHistoryInfo.BuyerId = userInfo.Id;
                gameHistoryInfo.RecommenderId = userInfo.Recommender;
                gameHistoryInfo.StartTime = DateTime.Now;
                gameHistoryInfo.EndTime = DateTime.Now;

                userInfo.GameHistoryId = Database.GetInstance().AddGameHistory(gameHistoryInfo);
            }

            return true;
        }

        public virtual bool PlayerOutTable(BaseInfo baseInfo, UserInfo userInfo)
        {
            _AutoEners.Remove(userInfo);

            if (GetPlayerIndex(userInfo) < 0)
                return true;

            _TableInfo._Players.Remove(userInfo);

            // modified by usc at 2014/04/02
            //_TableInfo._Viewers.Add(userInfo);

            if (userInfo.Auto == 0)
                _TableInfo._Viewers.Add(userInfo);
            else
                userInfo.GameId = string.Empty;

            GameHistoryInfo gameHistoryInfo = Database.GetInstance().FindGameHistory(userInfo.GameHistoryId);

            if (gameHistoryInfo != null && gameHistoryInfo.BuyerTotal == 0 && gameHistoryInfo.RecommenderTotal == 0 && gameHistoryInfo.ManagerTotal == 0 && gameHistoryInfo.AdminTotal == 0)
            {
                Database.GetInstance().DeleteGameHistory(gameHistoryInfo.Id);
            }

            // added by usc at 2014/03/04
            if (userInfo.Auto == 0)
                Server.GetInstance().Send(userInfo.Socket, NotifyType.Reply_UserInfo, userInfo);

            return true;
        }

        public bool ViewerEnterTable(UserInfo userInfo)
        {
            _AutoEners.Remove(userInfo);

            if (GetViewerIndex(userInfo) >= 0)
            {
                userInfo.GameId = _TableInfo._TableId;
                return true;
            }

            if (GetPlayerIndex(userInfo) >= 0)
            {
                userInfo.GameId = _TableInfo._TableId;
                return true;
            }

            if (_TableInfo._Viewers.Count >= GameDefine.GAME_PLAYER)
            {
                BaseInfo.SetError(ErrorType.Full_Gamer, "인원이 꽉 찼습니다.");
                return false;
            }

            _TableInfo._Viewers.Add(userInfo);
            userInfo.GameId = _TableInfo._TableId;

            return true;
        }

        public void BroadCastGame(NotifyType notifyType, BaseInfo baseInfo)
        {
            if (notifyType == NotifyType.Reply_TableDetail)
            {
                TableInfo tableInfo = (TableInfo)baseInfo;

                if (tableInfo._RoundStartTime.Year != 1)
                    tableInfo._RoundDelayTime = (int)(DateTime.Now - tableInfo._RoundStartTime).TotalSeconds;
            }

            for (int i = 0; i < _TableInfo._Players.Count; i++)
            {
                if (_GameInfo.GameId != _TableInfo._Players[i].GameId)
                    continue;
                if (_TableInfo._Players[i].Auto > 0)
                    continue;

                Server.GetInstance().Send(_TableInfo._Players[i].Socket, notifyType, baseInfo);
            }

            for (int i = 0; i < _TableInfo._Viewers.Count; i++)
            {
                if (_GameInfo.GameId != _TableInfo._Viewers[i].GameId)
                    continue;
                if (_TableInfo._Viewers[i].Auto > 0)
                    continue;

                Server.GetInstance().Send(_TableInfo._Viewers[i].Socket, notifyType, baseInfo);
            }
        }

        public void SendGameData(int playerIndex, NotifyType notifyType, BaseInfo baseInfo)
        {
            if (playerIndex >= _TableInfo._Players.Count())
                return;

            UserInfo userInfo = _TableInfo._Players[playerIndex];

            Server.GetInstance().Send(userInfo.Socket, notifyType, baseInfo);
        }

        public void OutTable(UserInfo userInfo)
        {
            if (GetPlayerIndex(userInfo) >= 0)
            {
                PlayerOutTable(userInfo, userInfo);
            }

            _TableInfo._Viewers.Remove(userInfo);

            userInfo.GameId = string.Empty;

            BroadCastGame(NotifyType.Reply_TableDetail, _TableInfo);
        }
    }
}
