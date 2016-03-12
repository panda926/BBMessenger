using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.Net.Sockets;

namespace ChatServer
{
    public partial class Game : BaseInfo
    {
        private static Game _instance = null;

        private List<GameTable> _Tables = new List<GameTable>();

        public static Game GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Game();
            }

            return _instance;
        }

        public void NotifyTimer()
        {
            for (int i = 0; i < _Tables.Count; )
            {
                GameTable gameTable = _Tables[i];

                gameTable.ProcessTimer();

                if (gameTable._TableInfo._Players.Count <= 0 && gameTable._TableInfo._Viewers.Count <= 0)
                {
                    _Tables.Remove(gameTable);
                    View._isUpdateGameList = true;
                    continue;
                }
                i++;
            }
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            Database database = Database.GetInstance();
            Server server = Server.GetInstance();
            Users users = Users.GetInstance();

            UserInfo userInfo = users.FindUser(socket);

            switch (notifyType)
            {
                case NotifyType.Request_GameList:
                    {
                        List<GameInfo> games = database.GetAllGames();

                        if (games == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        for (int i = 0; i < games.Count; i++)
                        {
                            games[i].UserCount = users.GetGameUsers(games[i].GameId).Count;
                        }

                        GameListInfo gameListInfo = new GameListInfo();
                        gameListInfo.Games = games;

                        server.Send(socket, NotifyType.Reply_GameList, gameListInfo);
                    }
                    break;

                //case NotifyType.Request_Download_GameFile: // 2014-01-21: GreenRose
                //    {
                //        GameInfo gameInfo = (GameInfo)baseInfo;

                //        List<string> listDWGameFiles = database.GetDWGameFiles(gameInfo.GameId);

                //        DWGameFileInfo dwGameFileInfo = new DWGameFileInfo();
                //        dwGameFileInfo.gameInfo = gameInfo;
                //        dwGameFileInfo.listGameFile = listDWGameFiles;

                //        server.Send(socket, NotifyType.Reply_Download_GameFile, dwGameFileInfo);
                //    }
                //    break;

                case NotifyType.Request_EnterGame:
                    {
                        GameInfo enterInfo = (GameInfo)baseInfo;

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "알수 없는 사용자가 게임에 들어가려고 하였습니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        if (enterInfo.nCashOrPointGame == 0)
                        {
                            userInfo.nCashOrPointGame = 0;
                        }
                        else
                        {
                            userInfo.nCashOrPointGame = 1;
                        }

                        if (userInfo.GameId.Trim() != "")
                        {
                            if (OutGame(userInfo) == false)
                            {
                                Main.ReplyError(socket);
                                return;
                            }
                        }

                        GameInfo gameInfo = new GameInfo();
                        if (enterInfo.nCashOrPointGame == 0)
                        {
                            gameInfo = database.FindGame(enterInfo.GameId);

                            if (gameInfo == null)
                            {
                                SetError(ErrorType.Invalid_GameId, "게임방아이디가 틀립니다.");
                                Main.ReplyError(socket);
                                return;
                            }

                            gameInfo.nCashOrPointGame = enterInfo.nCashOrPointGame;
                        }
                        else
                        {
                            int nGameID = Convert.ToInt32(enterInfo.GameId) - 1;

                            gameInfo = database.FindGame(nGameID.ToString());

                            if (gameInfo == null)
                            {
                                SetError(ErrorType.Invalid_GameId, "게임방아이디가 틀립니다.");
                                Main.ReplyError(socket);
                                return;
                            }

                            gameInfo.nCashOrPointGame = enterInfo.nCashOrPointGame;
                            gameInfo.GameId = enterInfo.GameId;
                        }

                        if (string.IsNullOrEmpty(userInfo.RoomId) == false)
                            gameInfo = enterInfo;

                        GameTable gameTable = FindTable(gameInfo.GameId);

                        if (gameTable == null)
                        {
                            gameTable = MakeTable(gameInfo);

                            if (gameTable == null)
                            {
                                Main.ReplyError(socket);
                                return;
                            }
                        }

                        gameTable.Run(notifyType, baseInfo, userInfo);
                    }
                    break;

                case NotifyType.Request_PlayerEnter:
                    {
                        //GameInfo enterInfo = (GameInfo)baseInfo;

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "알수 없는 사용자가 게임에 들어가려고 하였습니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        if (string.IsNullOrEmpty(userInfo.GameId) == true)
                        {
                            SetError(ErrorType.Unknown_User, "게임방아이다가 없습니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        GameTable gameTable = FindTable(userInfo.GameId);

                        if (gameTable == null)
                        {
                            SetError(ErrorType.Invalid_GameId, "게임방아이디가 틀립니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        gameTable.Run(notifyType, baseInfo, userInfo);

                        // added by usc at 2014/03/25
                        TableInfo tableInfo = gameTable.GetTableInfo();

                        StringInfo messageInfo = new StringInfo();

                        messageInfo.UserId = "AMDIN_MSG";
                        messageInfo.String = string.Format("{0} 进入房间", userInfo.Nickname);
                        messageInfo.FontSize = 12;
                        messageInfo.FontName = "Segoe UI";
                        messageInfo.FontStyle = "b";
                        messageInfo.FontWeight = "d";
                        messageInfo.FontColor = "#9adf8c";
                        messageInfo.strRoomID = userInfo.GameId;

                        BroadCastGame(tableInfo, NotifyType.Reply_GameChat, messageInfo, userInfo.Id);
                    }
                    break;

                //case NotifyType.Request_PlayerOut:
                //    {
                //        if (userInfo == null)
                //        {
                //            SetError(ErrorType.Unknown_User, "알수 없는 사용자가 게임에 들어가려고 하였습니다.");
                //            Main.ReplyError(socket);
                //            return;
                //        }

                //        if (string.IsNullOrEmpty(userInfo.GameId) == true)
                //        {
                //            SetError(ErrorType.Unknown_User, "게임방아이다가 없습니다.");
                //            Main.ReplyError(socket);
                //            return;
                //        }

                //        GameTable gameTable = FindTable(userInfo.GameId);

                //        if (gameTable == null)
                //        {
                //            SetError(ErrorType.Invalid_GameId, "게임방아이디가 틀립니다.");
                //            Main.ReplyError(socket);
                //            return;
                //        }

                //        gameTable.Run(notifyType, baseInfo, userInfo);
                //    }
                //    break;

                case NotifyType.Request_OutGame:
                    {
                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "알수 없는 사용자가 게임을 탈퇴하려고 하였습니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        if (OutGame(userInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                    }
                    break;
                // added by usc at 2014/01/08
                case NotifyType.Request_GameChat:
                    {
                        StringInfo stringInfo = (StringInfo)baseInfo;

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "알수 없는 사용자가 채팅하려고 합니다.");
                        }

                        stringInfo.UserId = userInfo.Id;

                        GameTable gameTable = FindTable(userInfo.GameId);

                        if (gameTable == null)
                        {
                            SetError(ErrorType.Invalid_GameId, "게임방아이디가 틀립니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        TableInfo tableInfo = gameTable.GetTableInfo();

                        BroadCastGame(tableInfo, NotifyType.Reply_GameChat, stringInfo, null);
                    }
                    break;
                // added by usc at 2014/02/15
                case NotifyType.Request_GameResult:
                    {
                        StringInfo stringInfo = (StringInfo)baseInfo;

                        if (userInfo == null)
                            return;

                        GameTable gameTable = FindTable(userInfo.GameId);

                        if (gameTable == null)
                        {
                            SetError(ErrorType.Invalid_GameId, "게임방아이디가 틀립니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        TableInfo tableInfo = gameTable.GetTableInfo();

                        BroadCastGame(tableInfo, NotifyType.Reply_GameResult, stringInfo, userInfo.Id);

                        // added by usc at 2014/03/04
                        if (userInfo.Auto == 0)
                            Server.GetInstance().Send(userInfo.Socket, NotifyType.Reply_UserInfo, userInfo);
                    }
                    break;
                default:
                    {
                        if (userInfo == null)
                            return;

                        GameTable gameTable = FindTable(userInfo.GameId);

                        if (gameTable == null)
                            return;

                        gameTable.Run(notifyType, baseInfo, userInfo);
                    }
                    break;
            }
        }

        public List<GameTable> GetGameTables()
        {
            return _Tables;
        }

        public bool OutGame(UserInfo userInfo)
        {
            GameTable gameTable = FindTable(userInfo.GameId);

            if (gameTable == null)
            {
                //SetError(ErrorType.Invalid_GameId, "나가려는 게임방이 틀립니다.");
                //return false;
                userInfo.GameId = string.Empty;
                return true;
            }

            // added by usc at 2014/03/25
            TableInfo tableInfo = gameTable.GetTableInfo();

            StringInfo messageInfo = new StringInfo();

            messageInfo.UserId = "AMDIN_MSG";
            messageInfo.String = string.Format("{0} 离开房间", userInfo.Nickname);
            messageInfo.FontSize = 12;
            messageInfo.FontName = "Segoe UI";
            messageInfo.FontStyle = "b";
            messageInfo.FontWeight = "d";
            messageInfo.FontColor = "#9adf8c";
            messageInfo.strRoomID = userInfo.GameId;

            BroadCastGame(tableInfo, NotifyType.Reply_GameChat, messageInfo, userInfo.Id);

            gameTable.Run(NotifyType.Request_OutGame, userInfo, userInfo);

            return true;
        }

        public GameTable MakeTable(GameInfo gameInfo)
        {
            GameTable table = null;

            switch (gameInfo.Source)
            {
                case GameSource.Sicbo:
                    table = new SicboTable(gameInfo.GameId);
                    break;

                case GameSource.Dice:
                    table = new DiceTable(gameInfo.GameId);
                    break;

                case GameSource.DzCard:
                    table = new DzCardTable(gameInfo.GameId);
                    break;

                case GameSource.Horse:
                    table = new HorseTable(gameInfo.GameId);
                    break;

                case GameSource.BumperCar:
                    table = new BumperCarTable(gameInfo.GameId);
                    break;

                case GameSource.Fish:
                    table = new FishTable(gameInfo.GameId);
                    break;
            }

            if (table == null)
            {
                SetError(ErrorType.Invalid_GameSource, "게임원천이 없습니다.");
                return null;
            }

            table._GameInfo = gameInfo;

            _Tables.Add(table);

            return table;
        }

        public GameTable FindTable(string tableId)
        {
            GameTable tableInfo = null;

            for (int i = 0; i < _Tables.Count; i++)
            {
                if (_Tables[i]._TableInfo._TableId == tableId)
                {
                    tableInfo = _Tables[i];
                    break;
                }
            }

            return tableInfo;
        }

        // added by usc at 2014/01/08
        public void BroadCastGame(TableInfo tableInfo, NotifyType notifyType, BaseInfo sendInfo, string ownerId)
        {
            for (int i = 0; i < tableInfo._Players.Count; i++)
            {
                if (tableInfo._TableId != tableInfo._Players[i].GameId)
                    continue;
                if (tableInfo._Players[i].Auto > 0)
                    continue;

                if (notifyType == NotifyType.Reply_GameResult)
                {
                    if (tableInfo._Players[i].Id == ownerId)
                    {
                        Server.GetInstance().Send(tableInfo._Players[i].Socket, notifyType, sendInfo);
                        return;
                    }
                }
                else if (notifyType == NotifyType.Reply_GameChat)
                {
                    if (tableInfo._Players[i].Id != ownerId)
                    {
                        Server.GetInstance().Send(tableInfo._Players[i].Socket, notifyType, sendInfo);
                    }
                }
            }

            for (int i = 0; i < tableInfo._Viewers.Count; i++)
            {
                if (tableInfo._TableId != tableInfo._Viewers[i].GameId)
                    continue;
                if (tableInfo._Viewers[i].Auto > 0)
                    continue;

                if (notifyType == NotifyType.Reply_GameResult)
                {
                    if (tableInfo._Viewers[i].Id == ownerId)
                    {
                        Server.GetInstance().Send(tableInfo._Viewers[i].Socket, notifyType, sendInfo);
                        return;
                    }
                }
                else if (notifyType == NotifyType.Reply_GameChat)
                {
                    if (tableInfo._Viewers[i].Id != ownerId)
                    {
                        Server.GetInstance().Send(tableInfo._Viewers[i].Socket, notifyType, sendInfo);
                    }
                }
            }
        }
    }
}
