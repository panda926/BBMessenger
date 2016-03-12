using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using ChatEngine;

namespace ChatServer
{
    public class Chat : BaseInfo
    {
        private static Chat _instance = null;

        private List<RoomInfo> _Rooms = new List<RoomInfo>();
        private List<RoomInfo> _Meetings = new List<RoomInfo>();

        public static Chat GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Chat();
            }

            return _instance;
        }

        public void NotifyUdpOccured(NotifyType notifyType, IPEndPoint ipEndPoint, BaseInfo baseInfo)
        {
            switch (notifyType)
            {
                case NotifyType.Request_VideoPort:
                    {
                        UserInfo receiveInfo = (UserInfo)baseInfo;

                        UserInfo userInfo = Users.GetInstance().FindUser(receiveInfo.Id);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "알수 없는 사용자입니다.");
                            //Main.ReplyError(socket);
                            return;
                        }

                        userInfo.OpenPortes[ receiveInfo.UdpPort ] = ipEndPoint.Port;
                        //Server.GetInstance().Send(userInfo.Socket, NotifyType.Reply_VideoPort, userInfo);
                    }
                    break;

                case NotifyType.Request_VideoChat:
                    {
                        VideoInfo receiveInfo = (VideoInfo)baseInfo;

                        UserInfo userInfo = Users.GetInstance().FindUser(receiveInfo.UserId);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "알수 없는 사용자입니다.");
                            //Main.ReplyError(socket);
                            return;
                        }

                        BroadUdpCast(userInfo.RoomId, NotifyType.Reply_VideoChat, baseInfo, userInfo.Id);
                    }
                    break;

                case NotifyType.Request_VoiceChat:
                    {
                        VoiceInfo receiveInfo = (VoiceInfo)baseInfo;
                        UserInfo userInfo = Users.GetInstance().FindUser(receiveInfo.UserId);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "알수 없는 사용자입니다.");
                            //Main.ReplyError(socket);
                            return;
                        }

                        BroadUdpCast(userInfo.RoomId, NotifyType.Reply_VoiceChat, baseInfo, userInfo.Id);
                    }
                    break;
            }
        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            Database database = Database.GetInstance();
            Server server = Server.GetInstance();
            Users users = Users.GetInstance();

            ResultInfo resultInfo = new ResultInfo();

            switch (notifyType)
            {
                case NotifyType.Request_EnterMeeting:
                    {
                        UserInfo userInfo = Users.GetInstance().FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "알수 없는 사용자입니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        AskChatInfo askChatInfo = (AskChatInfo)baseInfo;

                        UserInfo targetInfo = Users.GetInstance().FindUser(askChatInfo.TargetId);

                        if (targetInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "채팅대상이 존재하지 않습니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        askChatInfo.TargetId = userInfo.Id;
                        server.Send(targetInfo.Socket, NotifyType.Request_EnterMeeting, askChatInfo);
                    }
                    break;

                case NotifyType.Reply_EnterMeeting:
                    {
                        AskChatInfo askChatInfo = (AskChatInfo)baseInfo;

                        UserInfo targetInfo = Users.GetInstance().FindUser(askChatInfo.TargetId);

                        if (targetInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "채팅대상이 존재하지 않습니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        if (askChatInfo.Agree == 0)
                        {
                            server.Send(targetInfo.Socket, NotifyType.Reply_EnterMeeting, askChatInfo);
                            return;
                        }

                        RoomInfo meetingInfo = EnterMeeting(socket, askChatInfo);

                        if (meetingInfo == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }
                    }
                    break;

                case NotifyType.Request_OutMeeting:
                    {
                        UserInfo userInfo = OutMeeting(socket);

                        if (userInfo == null)
                            Main.ReplyError(socket);
                    }
                    break;

                case NotifyType.Request_RoomList:
                    {
                        UserInfo userInfo = Users.GetInstance().FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "알수 없는 사용자가 방목록을 요구하였습니다.");
                            Main.ReplyError(socket);
                            return;
                        }


                        List<RoomInfo> rooms = null; 
                        
                        if( userInfo.Kind == (int)UserKind.ServiceWoman )
                            rooms = database.GetAllRooms();
                        else
                            rooms = GetRooms();

                        if (rooms == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        RoomListInfo roomListInfo = new RoomListInfo();
                        roomListInfo.Rooms = rooms;

                        server.Send(socket, NotifyType.Reply_RoomList, roomListInfo);
                    }
                    break;

                case NotifyType.Request_MakeRoom:
                    {
                        RoomInfo roomInfo = (RoomInfo)baseInfo;

                        UserInfo userInfo = users.FindUser(socket);

                        if (userInfo == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        roomInfo.Owner = userInfo.Id;

                        if (database.AddRoom(roomInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        string str = string.Format("{0} 님이 방 {1} 을 만들었습니다.", userInfo.Id, roomInfo.Id);
                        LogView.AddLogString(str);

                        server.Send(socket, NotifyType.Reply_MakeRoom, roomInfo);
                        View._isUpdateRoomList = true;

                        ReplyRoomList();
                    }
                    break;

                case NotifyType.Request_UpdateRoom:
                    {
                        RoomInfo updateInfo = (RoomInfo)baseInfo;

                        RoomInfo roomInfo = Database.GetInstance().FindRoom(updateInfo.Id);

                        if (roomInfo == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        roomInfo.Body = updateInfo;

                        if (Database.GetInstance().UpdateRoom(roomInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }
                        server.Send(socket, NotifyType.Reply_UpdateRoom, roomInfo);
                    }
                    break;

                case NotifyType.Request_DelRoom:
                    {
                        RoomInfo delInfo = (RoomInfo)baseInfo;

                        if (FindRoom(delInfo.Id) != null)
                        {
                            SetError(ErrorType.Live_Room, "유저들이 들어있는 방입니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        if( Database.GetInstance().FindRoom(delInfo.Id ) == null )
                        {
                            SetError(ErrorType.Invalid_RoomId, "삭제하려는 방이 없습니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        if (Database.GetInstance().DelRoom(delInfo.Id) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }
                        server.Send(socket, NotifyType.Reply_DelRoom, delInfo);
                    }
                    break;

                case NotifyType.Request_EnterRoom:
                    {
                        OutRoom(socket);

                        RoomInfo enterInfo = (RoomInfo)baseInfo;

                        UserInfo userInfo = EnterRoom(socket, enterInfo.Id );

                        if (userInfo == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }
                    }
                    break;

                case NotifyType.Request_OutRoom:
                    {
                        if( OutRoom(socket) == null )
                        {
                            Main.ReplyError(socket);
                            break;
                        }
                    }
                    break;

                case NotifyType.Request_RoomDetail:
                    {
                        RoomInfo roomInfo = (RoomInfo)baseInfo;

                        ReplyRoomDetailInfo(roomInfo.Id);
                    }
                    break;

                case NotifyType.Request_StringChat:
                    {
                        StringInfo stringInfo = (StringInfo)baseInfo;

                        UserInfo userInfo = users.FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "알수 없는 사용자가 채팅하려고 합니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        stringInfo.UserId = userInfo.Id;

                        BroadCast(userInfo.RoomId, NotifyType.Reply_StringChat, stringInfo, null );
                    }
                    break;

                case NotifyType.Request_VoiceChat:
                    {
                        VoiceInfo voiceInfo = (VoiceInfo)baseInfo;

                        UserInfo userInfo = users.FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "알수 없는 사용자가 채팅하려고 합니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        voiceInfo.UserId = userInfo.Id;

                        BroadCast(userInfo.RoomId, NotifyType.Reply_VoiceChat, voiceInfo, userInfo.Id );
                    }
                    break;

                case NotifyType.Request_VideoChat:
                    {
                        VideoInfo videoInfo = (VideoInfo)baseInfo;

                        UserInfo userInfo = users.FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "알수 없는 사용자가 채팅하려고 합니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        videoInfo.UserId = userInfo.Id;

                        BroadCast(userInfo.RoomId, NotifyType.Reply_VideoChat, videoInfo, userInfo.Id );
                    }
                    break;

                case NotifyType.Request_Give:
                    {
                        UserInfo userInfo = users.FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "알수 없는 사용자가 선물하려고 합니다.");
                            Main.ReplyError(socket);
                            return;

                        }

                        PresentHistoryInfo presentInfo = (PresentHistoryInfo)baseInfo;
                        presentInfo.SendId = userInfo.Id;

                        UserInfo targetInfo = database.FindUser(presentInfo.ReceiveId);

                        if (targetInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "받으려는 사용자정보가 정확치 않습니다.");
                            Main.ReplyError(socket);
                            return;
                        }

                        if (Cash.GetInstance().ProcessPresent(presentInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        BroadCast(userInfo.RoomId, NotifyType.Reply_Give, baseInfo, null);

                        ReplyRoomDetailInfo(userInfo.RoomId);

                        users.ReplyUserList(null);

                        View._isUpdateUserList = true;
                    }
                    break;

                case NotifyType.Request_MusiceInfo:
                    {
                        UserInfo userInfo = users.FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "不明会员准备删除过照片信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        MusiceInfo musiceInfo = (MusiceInfo)baseInfo;

                        BroadCast(userInfo.RoomId, NotifyType.Reply_MusiceInfo, musiceInfo, null);
                        //server.Send(socket, NotifyType.Reply_MusiceInfo, musiceInfo);
                    }
                    break;

                case NotifyType.Request_MusiceStateInfo:
                    {
                        UserInfo userInfo = users.FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "不明会员准备删除过照片信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        MusiceStateInfo musiceStateInfo = (MusiceStateInfo)baseInfo;

                        BroadCast(userInfo.RoomId, NotifyType.Reply_MusiceStateInfo, musiceStateInfo, null);
                        //server.Send(socket, NotifyType.Reply_MusiceStateInfo, musiceStateInfo);
                    }
                    break;
            }

        }

        public List<RoomInfo> GetMeetings()
        {
            return _Meetings;
        }

        public List<RoomInfo> GetRooms()
        {
            List <RoomInfo> roomList = _Rooms;

            for (int i = 0; i < roomList.Count; i++)
            {
                List<UserInfo> roomUsers = Users.GetInstance().GetRoomUsers(roomList[i].Id);
                roomList[i].UserCount = roomUsers.Count;
            }

            return roomList;
        }

        public RoomInfo FindMeeting(string meetId)
        {
            RoomInfo meetInfo = null;

            for (int i = 0; i < _Meetings.Count; i++)
            {
                if (_Meetings[i].Id == meetId)
                {
                    meetInfo = _Meetings[i];
                    break;
                }
            }

            return meetInfo;
        }

        public RoomInfo FindRoom(string roomId)
        {
            RoomInfo roomInfo = null;

            for (int i = 0; i < _Rooms.Count; i++)
            {
                if (_Rooms[i].Id == roomId)
                {
                    roomInfo = _Rooms[i];
                    break;
                }
            }

            return roomInfo;
        }

        public RoomInfo EnterMeeting(Socket socket, AskChatInfo askChatInfo)
        {
            UserInfo userInfo = Users.GetInstance().FindUser(socket);

            if (userInfo == null)
            {
                SetError(ErrorType.Unknown_User, "알수 없는 사용자가 1:1채팅을 하려고 합니다.");
                Main.ReplyError(socket);
                return null;
            }

            UserInfo targetInfo = Users.GetInstance().FindUser(askChatInfo.TargetId);

            if (targetInfo == null)
            {
                SetError(ErrorType.Unknown_User, "채팅대상이 존재하지 않습니다.");
                return null;
            }

            if (targetInfo.RoomId.Trim().Length > 0)
            {
                SetError(ErrorType.Already_Chat, "채팅대상자는 이미 1:1 채팅중입니다.");
                return null;
            }

            RoomInfo roomInfo = new RoomInfo();

            roomInfo.Id = (_Meetings.Count + 1).ToString();
            roomInfo.Name = string.Format("{0} - {1}", userInfo.Nickname, targetInfo.Nickname);
            roomInfo.Owner = targetInfo.Id;
            roomInfo.UserCount = 2;
            roomInfo.MaxUsers = 2;
            roomInfo.Cash = askChatInfo.Price;

            if (StartRoom(targetInfo, roomInfo, askChatInfo) == null)
                return null;

            if (StartRoom(userInfo, roomInfo, askChatInfo ) == null)
                return null;

            return roomInfo;
        }

        public UserInfo EnterRoom(Socket socket, string roomId)
        {
            UserInfo userInfo = Users.GetInstance().FindUser(socket);

            if (userInfo == null)
            {
                SetError(ErrorType.Unknown_User, "알수 없는 사용자가 채팅을 시도하였습니다.");
                return null;
            }

            RoomInfo roomInfo = FindRoom(roomId);
            
            if( roomInfo == null )
                roomInfo = Database.GetInstance().FindRoom(roomId);

            if (roomInfo == null)
            {
                SetError(ErrorType.Invalid_RoomId, "방아이디가 틀립니다.");
                return null;
            }

            return StartRoom(userInfo, roomInfo, null);
        }

        public UserInfo StartRoom( UserInfo userInfo, RoomInfo roomInfo, AskChatInfo askChatInfo )
        {
            List<UserInfo> roomUsers = Users.GetInstance().GetRoomUsers(roomInfo.Id);

            if (userInfo.Kind == (int)UserKind.ServiceWoman)
            {
                if ( roomUsers.Count > 0 )
                {
                    SetError(ErrorType.Already_Serviceman, "이미 봉사자가 들어있는 방입니다.");
                    return null;
                }

                roomInfo.Owner = userInfo.Id;
            }
            else
            {
                if ( roomUsers.Count <= 0 )
                {
                    SetError(ErrorType.Notallow_NoServiceman, "봉사자가 없는 방에 먼저 들어갈수 없습니다.");
                    return null;
                }

                if (roomInfo.Cash > 0 && userInfo.Cash < roomInfo.Cash)
                {
                    SetError(ErrorType.Notenough_Cash, string.Format("{0}님의 캐쉬가 작아 채팅을 할수 없습니다.", userInfo.Id));
                    return null;
                }

                if (roomInfo.Point > 0 && userInfo.Point < roomInfo.Point)
                {
                    SetError(ErrorType.Notenough_Point, string.Format("{0}님의 포인트가 작아 채팅을 할수 없습니다.", userInfo.Point));
                    return null;
                }
            }

            userInfo.RoomId = roomInfo.Id;
            userInfo.EnterTime = DateTime.Now;
            userInfo.CashTime = userInfo.EnterTime;

            if (askChatInfo == null)
            {
                if( FindRoom( roomInfo.Id ) == null )
                {
                    _Rooms.Add(roomInfo);

                    if (roomInfo.Point <= 0)
                    {
                        GameInfo gameInfo = Database.GetInstance().FindGameSource(GameSource.Dice);
                        gameInfo.GameId = roomInfo.Id;

                        GameTable gameTable = Game.GetInstance().MakeTable(gameInfo);

                        if (gameTable != null)
                        {
                            roomInfo.IsGame = 1;
                            roomInfo._GameInfo = gameInfo;
                        }
                    }
                }

                ReplyRoomList();

                Server.GetInstance().Send(userInfo.Socket, NotifyType.Reply_EnterRoom, roomInfo);
            }
            else
            {
                if (FindMeeting(roomInfo.Id) == null)
                {
                    _Meetings.Add(roomInfo);
                }
                askChatInfo.MeetingInfo = roomInfo;

                Server.GetInstance().Send(userInfo.Socket, NotifyType.Reply_EnterMeeting, askChatInfo);
            }

            if (roomInfo._GameInfo != null)
            {
                //userInfo.GameId = roomInfo._GameInfo.GameId;
                Game.GetInstance().NotifyOccured(NotifyType.Request_EnterGame, userInfo.Socket, roomInfo._GameInfo);
                Game.GetInstance().NotifyOccured(NotifyType.Request_PlayerEnter, userInfo.Socket, roomInfo._GameInfo);
            }

            Users.GetInstance().ReplyUserList(null);
            ReplyRoomDetailInfo(roomInfo.Id);

            View._isUpdateUserList = true;
            View._isUpdateRoomList = true;

            string str = string.Format("{0} 님이 {1}방에 들어갔습니다.", userInfo.Id, roomInfo.Id);
            LogView.AddLogString(str);

            if ( userInfo.Kind != (int)UserKind.ServiceWoman)
            {
                if (roomInfo.Cash > 0)
                {
                    ChatHistoryInfo chatHistoryInfo = new ChatHistoryInfo();

                    chatHistoryInfo.RoomId = roomInfo.Id;
                    chatHistoryInfo.BuyerId = userInfo.Id;
                    chatHistoryInfo.ServicemanId = roomInfo.Owner;
                    chatHistoryInfo.ServicePrice = roomInfo.Cash;
                    chatHistoryInfo.StartTime = userInfo.EnterTime;
                    chatHistoryInfo.EndTime = userInfo.EnterTime;

                    userInfo.ChatHistoryId = Database.GetInstance().AddChatHistory(chatHistoryInfo);
                }

                if (roomInfo.Point > 0)
                {
                    PointHistoryInfo pointHistoryInfo = new PointHistoryInfo();

                    pointHistoryInfo.TargetId = userInfo.Id;
                    pointHistoryInfo.AgreeTime = DateTime.Now;
                    pointHistoryInfo.Content = string.Format("{0}과 무료채팅", roomInfo.Owner);

                    userInfo.PointHistoryId = Database.GetInstance().AddPointHistory(pointHistoryInfo);
                }
            }

            return userInfo;
        }

        public UserInfo OutMeeting(Socket socket)
        {
            UserInfo userInfo = Users.GetInstance().FindUser(socket);

            if (userInfo == null)
            {
                SetError(ErrorType.Unknown_User, "알수 없는 사용자가 채팅을 끝내려고 합니다.");
                return null;
            }

            string meetId = userInfo.RoomId.Trim();

            if (meetId.Length == 0)
                return userInfo;
            if (Convert.ToInt32(meetId) > 1000)
                return userInfo;

            RoomInfo meetInfo = FindMeeting(meetId);

            if (meetInfo == null)
            {
                SetError(ErrorType.Invalid_RoomId, "나가려는 1:1방아이디가 틀립니다.");
                return null;
            }

            List<UserInfo> meetUsers = Users.GetInstance().GetRoomUsers(meetId);

            if( meetUsers == null )
            {
                return null;
            }

            for (int i = 0; i < meetUsers.Count; i++)
            {
                if (EndRoom(meetUsers[i], meetInfo, true) == null)
                    return null;
            }

            return userInfo;
        }

        public UserInfo OutRoom(Socket socket)
        {
            UserInfo userInfo = Users.GetInstance().FindUser(socket);

            if (userInfo == null)
                return null;

            string roomId = userInfo.RoomId.Trim();

            if (roomId.Length == 0)
                return userInfo;
            if (Convert.ToInt32(roomId) < 1000)
                return userInfo;

            RoomInfo roomInfo = FindRoom(roomId);

            if (roomInfo == null)
            {
                SetError(ErrorType.Invalid_RoomId, "나가려는 방아이디가 틀립니다.");
                return null;
            }

            UserInfo outInfo = EndRoom(userInfo, roomInfo, false);

            List<UserInfo> roomUsers = Users.GetInstance().GetRoomUsers(roomInfo.Id);

            bool isServiceman = false;

            foreach (UserInfo roomUser in roomUsers)
            {
                if (roomUser.Kind == (int)UserKind.ServiceWoman)
                {
                    isServiceman = true;
                    break;
                }
            }

            if (isServiceman == false)
            {
                foreach (UserInfo roomUser in roomUsers)
                    OutRoom(roomUser.Socket);
            }

            return outInfo;
        }

        public UserInfo EndRoom( UserInfo userInfo, RoomInfo roomInfo, bool isMeeting )
        {
            userInfo.RoomId = "";

            ChatHistoryInfo chatHistoryInfo = Database.GetInstance().FindChatHistory(userInfo.ChatHistoryId);

            if (chatHistoryInfo != null)
            {
                chatHistoryInfo.EndTime = DateTime.Now;

                Database.GetInstance().UpdateChatHistory(chatHistoryInfo);
            }

            PointHistoryInfo pointHistoryInfo = Database.GetInstance().FindPointHistory(userInfo.PointHistoryId);

            if (pointHistoryInfo != null)
            {
                pointHistoryInfo.AgreeTime = DateTime.Now;

                Database.GetInstance().UpdatePointHistory(pointHistoryInfo);
            }

            if (userInfo.GameId != null)
                Game.GetInstance().OutGame(userInfo);

            if (isMeeting == true)
                Server.GetInstance().Send(userInfo.Socket, NotifyType.Reply_OutMeeting, userInfo);
            else
                Server.GetInstance().Send(userInfo.Socket, NotifyType.Reply_OutRoom, userInfo);

            ReplyRoomDetailInfo(roomInfo.Id);

            List<UserInfo> roomUsers = Users.GetInstance().GetRoomUsers(roomInfo.Id);

            if (roomUsers.Count == 0)
            {
                if (isMeeting == true)
                {
                    _Meetings.Remove(roomInfo);
                }
                else
                {
                    _Rooms.Remove(roomInfo);
                }
            }

            ReplyRoomList();

            Users.GetInstance().ReplyUserList(null);

            View._isUpdateUserList = true;
            View._isUpdateRoomList = true;

            string str = string.Format("{0} 님이 {1} 방에서 나갔습니다.", userInfo.Id, roomInfo.Id);
            LogView.AddLogString(str);

            return userInfo;
        }

        public bool ReplyRoomDetailInfo(string roomId)
        {
            RoomDetailInfo roomDetailInfo = new RoomDetailInfo();

            roomDetailInfo.Users = Users.GetInstance().GetRoomUsers(roomId);

            if (roomDetailInfo.Users != null)
            {
                for (int i = 0; i < roomDetailInfo.Users.Count; i++)
                    roomDetailInfo.Users[i] = roomDetailInfo.Users[i];
            }

            roomDetailInfo.Emoticons = Database.GetInstance().GetAllEmoticons();
            roomDetailInfo.Presents = Database.GetInstance().GetAllPresents();

            BroadCast(roomId, NotifyType.Reply_RoomDetail, roomDetailInfo, null );

            return true;
        }

        public void BroadCast(string roomId, NotifyType notifyType, BaseInfo sendInfo, string ownerId )
        {
            List<UserInfo> users = Users.GetInstance().GetRoomUsers(roomId);

            for (int i = 0; i < users.Count; i++)
            {
                if( users[i].Id != ownerId )
                    Server.GetInstance().Send(users[i].Socket, notifyType, sendInfo);
            }
        }

        public void BroadUdpCast(string roomId, NotifyType notifyType, BaseInfo sendInfo, string ownerId)
        {
            List<UserInfo> users = Users.GetInstance().GetRoomUsers(roomId);

            for (int i = 0; i < users.Count; i++)
            {
                int port = users[i].UdpPort;

                if (users[i].Id != ownerId)
                    UdpServerClient.GetInstance().Send(users[i].Socket, users[i].OpenPortes, notifyType, sendInfo);
            }
        }

        public void ReplyRoomList()
        {
            RoomListInfo roomListInfo = new RoomListInfo();
            
            List<RoomInfo> AllRooms = Database.GetInstance().GetAllRooms();
            List<RoomInfo> LivingRooms = GetRooms();

            List<UserInfo> userList = Users.GetInstance().GetUsers();

            for (int i = 0; i < userList.Count; i++)
            {
                UserInfo userInfo = userList[i];

                if (userInfo.Kind == (int)UserKind.ServiceWoman)
                    roomListInfo.Rooms = AllRooms;
                else
                    roomListInfo.Rooms = LivingRooms;

                Server.GetInstance().Send(userList[i].Socket, NotifyType.Reply_RoomList, roomListInfo);
            }

            Users.GetInstance().ReplyUserList(null);
        }

    }
}
