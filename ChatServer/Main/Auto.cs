using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;

namespace ChatServer
{
    public class Auto
    {
        private static Auto _instance = null;

        public List<UserInfo> _Autos = new List<UserInfo>();

        public static Auto GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Auto();
            }

            return _instance;
        }

        public int GetAutoCount()
        {
            return Database.GetInstance().GetAutoCount();
        }

        public int GetLiveCount()
        {
            return _Autos.Count;
        }

        public bool AddAuto(RoomInfo roomInfo, UserInfo userInfo)
        {
            userInfo.Socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            Server.GetInstance()._NotifyHandler(NotifyType.Request_Login, userInfo.Socket, userInfo);

            userInfo = Users.GetInstance().FindUser(userInfo.Id);

            if (userInfo == null)
                return false;

            Random random = new Random();

            if (userInfo.Cash < 1000)
            {
                int cash = random.Next() % 1000 + 500;

                Cash.GetInstance().GiveCash(userInfo.Id, cash);
                Cash.GetInstance().GiveChargeSum(userInfo.Id, cash);

            }

            Server.GetInstance()._NotifyHandler(NotifyType.Request_EnterRoom, userInfo.Socket, roomInfo);

            userInfo.WaitSecond = random.Next() % 30;
            _Autos.Add(userInfo);

            return true;
        }

        public bool AddAuto(GameInfo gameInfo, UserInfo userInfo, bool bAddMoney)
        {
            userInfo.Socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            Server.GetInstance()._NotifyHandler(NotifyType.Request_Login, userInfo.Socket, userInfo);

            userInfo = Users.GetInstance().FindUser(userInfo.Id);

            if (userInfo == null)
                return false;

            Random random = new Random();

            // added by usc at 2014/03/19
            if (bAddMoney)
            {
                int nGameMoney = 0;

                if (gameInfo.nCashOrPointGame == 0)
                    nGameMoney = userInfo.Cash;
                else
                    nGameMoney = userInfo.Point;

                if (nGameMoney < 10000)
                {
                    int nAddMoney = 15000 + (random.Next() % 100) * 100;

                    Cash.GetInstance().GiveCashOrPoint(userInfo.Id, nAddMoney, gameInfo);

                    if (gameInfo.nCashOrPointGame == 0)
                        Cash.GetInstance().GiveChargeSum(userInfo.Id, nAddMoney);
                }
            }

            Server.GetInstance()._NotifyHandler(NotifyType.Request_EnterGame, userInfo.Socket, gameInfo);
            Server.GetInstance()._NotifyHandler(NotifyType.Request_PlayerEnter, userInfo.Socket, userInfo);

            bool bFound = false;
            for (int i = 0; i < _Autos.Count; i++)
            {
                if (_Autos[i].Id == userInfo.Id)
                {
                    bFound = true;
                    break;
                }
            }

            if (!bFound)
            {
                //NicknameInfo nameInfo = Database.GetInstance().GetFreeNicknameInfo();

                //if (nameInfo.IsValid() == false)
                //    return false;

                //userInfo.Nickname = nameInfo.Nickname;
                //Database.GetInstance().UpdateUser(userInfo);

                //nameInfo.AutoId = userInfo.Id;
                //nameInfo.StartTime = DateTime.Now;
                //Database.GetInstance().UpdateNicknameInfo(nameInfo);
                userInfo.WaitSecond = random.Next() % 30;
                _Autos.Add(userInfo);
            }

            return true;
        }

        public UserInfo GetAuto(string userId)
        {
            foreach (UserInfo autoInfo in _Autos)
            {
                if (autoInfo.Id == userId)
                {
                    return autoInfo;
                }
            }

            return null;
        }

        public bool OutAuto(UserInfo userInfo)
        {
            Server.GetInstance()._NotifyHandler(NotifyType.Request_Logout, userInfo.Socket, userInfo);

            _Autos.Remove(userInfo);

            return true;
        }

        public void NotifyTimer()
        {
            //foreach (UserInfo userInfo in _Autos)
            //{
            //    userInfo.WaitSecond--;

            //    if (userInfo.WaitSecond > 0)
            //        continue;

            //    Random random = new Random();
            //    userInfo.WaitSecond = random.Next() % 30;

            //    GameTable gameTable = Game.GetInstance().FindTable(userInfo.GameId);

            //    if (gameTable == null)
            //        continue;

            //    if (gameTable._TableInfo._RoundIndex != 1)
            //        continue;

            //    BettingInfo bettingInfo = new BettingInfo();

            //    bettingInfo._Area = random.Next() % 4;

            //    int[] bettingScores = new int[]{ 5, 10, 50, 100, 400 };
            //    bettingInfo._Score = bettingScores[random.Next() % 5];

            //    Server.GetInstance()._NotifyHandler(NotifyType.Request_Betting, userInfo.Socket, bettingInfo);
            //}
        }

        /*
        public bool ChangeNickname(UserInfo userInfo)
        {
            NicknameInfo oldInfo = Database.GetInstance().GetNicknameInfoByID(userInfo.Id);

            if (oldInfo.IsValid() == false)
                return false;

            if (DateTime.Now.Day - oldInfo.StartTime.Day > 0)
            {
                NicknameInfo newInfo = Database.GetInstance().GetFreeNicknameInfo();

                if (newInfo.IsValid() == false)
                    return false;

                userInfo.Nickname = newInfo.Nickname;            
                Database.GetInstance().UpdateUser(userInfo);

                newInfo.AutoId = userInfo.Id;
                newInfo.StartTime = DateTime.Now;
                Database.GetInstance().UpdateNicknameInfo(newInfo);

                oldInfo.AutoId = string.Empty;
                oldInfo.EndTime = DateTime.Now;
                Database.GetInstance().UpdateNicknameInfo(oldInfo);
            }

            return true;
        }

        public void NotifyAutoCheck()
        {
            // added by usc at 2014/02/25
            List<GameInfo> gameList = Database.GetInstance().GetAllGames();

            if (gameList == null || gameList.Count == 0)
                return;

            Random rnd = new Random();

            for (int i = 0; i < _Autos.Count; i++)
            {
                UserInfo userInfo = _Autos[i];

                if (userInfo.Auto > 0 && userInfo.GameId == string.Empty)
                {
                    // added by usc at 2014/04/13
                    ChangeNickname(userInfo);

                    int nGameNo = rnd.Next() % gameList.Count;

                    GameInfo gameInfo = gameList[nGameNo];

                    // modified by usc at 2014/02/26
                    gameInfo.nCashOrPointGame = rnd.Next() % 2;

                    if (gameInfo.nCashOrPointGame == 0)
                    {
                        AddAuto(gameInfo, userInfo, true);
                    }
                    else
                    {
                        GameInfo pointGameInfo = new GameInfo();

                        pointGameInfo._NotifyHandler = gameInfo._NotifyHandler;
                        pointGameInfo.Bank = gameInfo.Bank;
                        pointGameInfo.Commission = gameInfo.Commission;
                        pointGameInfo.GameId = (Convert.ToInt32(gameInfo.GameId) + 1).ToString();
                        pointGameInfo.GameName = gameInfo.GameName;
                        pointGameInfo.Height = gameInfo.Height;
                        pointGameInfo.Icon = gameInfo.Icon;
                        pointGameInfo.nCashOrPointGame = 1;
                        pointGameInfo.Source = gameInfo.Source;
                        pointGameInfo.UserCount = gameInfo.UserCount;
                        pointGameInfo.Width = gameInfo.Width;

                        AddAuto(pointGameInfo, userInfo, true);
                    }

                    break;
                }
            }
        }
        */
    }
}
