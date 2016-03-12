using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatServer
{
    public class Users : BaseInfo
    {
        private static Users _instance = null;

        private List<UserInfo> _Users = new List<UserInfo>();

        public static UserInfo ManagerInfo = null;
        public static string WebResourceUrl = string.Empty;

        public static bool IsOldServer;

        public static Users GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Users();
            }

            return _instance;
        }

        public void NotifyPing()
        {
            ResultInfo resultInfo = new ResultInfo();

            int index = 0;

            while (index < _Users.Count)
            {
                UserInfo userInfo = _Users[index];

                // 사용자가 삭제되였을경우.
                if (userInfo.Auto == 0)
                {
                    UserInfo findUserInfo = Database.GetInstance().FindUser(userInfo.Id);
                    if (findUserInfo == null)
                    {
                        SetError(ErrorType.Unknown_User, "要修改百分比的帐号不准确.");
                        Main.ReplyError(userInfo.Socket);

                        Logout(userInfo.Socket);

                        continue;
                    }
                }

                if ((DateTime.Now - userInfo.PingDate).TotalSeconds > 120)
                {
                    if (userInfo.Auto != 1)
                    {
                        NotifyOccured(NotifyType.Notify_Socket, userInfo.Socket, resultInfo);
                        continue;
                    }
                }
                else
                {
                    if (userInfo.Auto != 1)
                        Server.GetInstance().Send(userInfo.Socket, NotifyType.Notify_Ping, resultInfo);
                }

                index++;
            }

            if (IsOldServer == true)
            {
                if (_Users.Count == 0)
                    Environment.Exit(0);
            }

        }

        public void NotifyOccured(NotifyType notifyType, Socket socket, BaseInfo baseInfo)
        {
            Database database = Database.GetInstance();
            Server server = Server.GetInstance();

            ResultInfo resultInfo = new ResultInfo();

            switch (notifyType)
            {
                case NotifyType.Notify_Socket:
                    {
                        if (socket != null)
                        {
                            UserInfo userInfo = FindUser(socket);

                            if (userInfo != null)
                            {
                                lock (Server.GetInstance()._objLockMain)
                                {
                                    Logout(socket);
                                }

                                string logStr = string.Format("与{0} 连接中断", userInfo.Id);
                                LogView.AddLogString(logStr);
                            }
                        }
                    }
                    break;

                case NotifyType.Notify_Ping:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo != null)
                        {
                            userInfo.PingDate = DateTime.Now;
                        }
                    }
                    break;

                case NotifyType.Notify_OldServer:
                    {
                        IsOldServer = true;
                    }
                    break;

                case NotifyType.Request_UserDetail:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "不明会员 准备获取个人信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        UserDetailInfo userDetailInfo = GetUserDetailInfo(userInfo.Id);

                        if (userDetailInfo == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        server.Send(socket, NotifyType.Reply_UserDetail, userDetailInfo);
                    }
                    break;

                case NotifyType.Request_PartnerDetail:
                    {
                        UserInfo partnerInfo = (UserInfo)baseInfo;

                        UserDetailInfo userDetailInfo = GetUserDetailInfo(partnerInfo.Id);

                        if (userDetailInfo == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        server.Send(socket, NotifyType.Reply_PartnerDetail, userDetailInfo);
                    }
                    break;

                case NotifyType.Request_ClassInfo:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        List<ClassInfo> classInfo = new List<ClassInfo>();
                        classInfo = Database.GetInstance().GetAllClasses();

                        ClassListInfo classListInfo = new ClassListInfo();
                        classListInfo.Classes = classInfo;

                        server.Send(socket, NotifyType.Reply_ClassInfo, classListInfo);

                    }
                    break;

                case NotifyType.Request_ClassTypeInfo:
                    {
                        ClassInfo classInfo = (ClassInfo)baseInfo;

                        if (classInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        List<ClassTypeInfo> classTypeInfo = null;
                        if (classInfo.ClassInfo_Id == 2 || classInfo.ClassInfo_Id == 1)
                            classTypeInfo = Database.GetInstance().GetAllClassType(classInfo);
                        else if (classInfo.ClassInfo_Id == 0)
                            classTypeInfo = Database.GetInstance().GetAllPictureClassType(classInfo);

                        ClassTypeListInfo classTypeListInfo = new ClassTypeListInfo();
                        classTypeListInfo.ClassType = classTypeInfo;

                        server.Send(socket, NotifyType.Reply_ClassTypeInfo, classTypeListInfo);

                    }
                    break;

                case NotifyType.Request_ClassPictureDetail:
                    {
                        ClassTypeInfo classTypeInfo = (ClassTypeInfo)baseInfo;

                        if (classTypeInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        ClassPictureDetailInfo classPictureDetailInfo = new ClassPictureDetailInfo();
                        classPictureDetailInfo.ClassType = Database.GetInstance().GetAllClassPictureType(classTypeInfo);

                        server.Send(socket, NotifyType.Reply_ClassPictureDetail, classPictureDetailInfo);

                    }
                    break;

                case NotifyType.Request_NewID:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "不明使用者正在准备申请帐号.");
                            Main.ReplyError(socket);
                            return;
                        }

                        string newId = database.GetNewIDList(userInfo);

                        if (newId == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        UserInfo registInfo = new UserInfo();

                        registInfo.Id = newId;

                        //if (userInfo.Kind == (int)UserKind.Recommender)
                        //    registInfo.Recommender = userInfo.Id;
                        //else
                        registInfo.Friend = userInfo.Id;

                        registInfo.Icon = "Images\\Face\\1.gif";
                        registInfo.RegistTime = DateTime.Now;

                        bool ret = database.AddUser(registInfo);

                        if (ret == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        string str = string.Format("{0} 申请了新的帐号", userInfo.Id);
                        LogView.AddLogString(str);

                        server.Send(socket, NotifyType.Reply_NewID, registInfo);
                    }
                    break;

                case NotifyType.Request_IconUpload:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "不明会员正在准备上传照片.");
                            Main.ReplyError(socket);
                            return;
                        }

                        VideoInfo videoInfo = (VideoInfo)baseInfo;
                        string fileName = videoInfo.ImgName;

                        //EnvInfo envInfo = database.GetEnviroment();

                        //while (true)
                        //{
                        //    string path = string.Format("{0}\\Images\\Face\\{1}", envInfo.ImageUploadPath, fileName);

                        //    if (File.Exists(path))
                        //    {
                        //        string[] part = fileName.Split('.');
                        //        fileName = part[0] + "1." + part[1];

                        //        continue;
                        //    }

                        //    File.WriteAllBytes(path, videoInfo.Data);
                        //    break;
                        //}

                        IconInfo iconInfo = new IconInfo();
                        iconInfo.Name = userInfo.Id;
                        iconInfo.Icon = string.Format("Images\\Face\\{0}", fileName);

                        IconInfo newInfo = database.AddFace(iconInfo);

                        if (newInfo == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        server.Send(socket, NotifyType.Reply_IconUpload, newInfo);
                    }
                    break;
                case NotifyType.Request_VideoUpload:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "不明会员正在准备上传视频.");
                            Main.ReplyError(socket);
                            return;
                        }

                        VideoInfo videoInfo = (VideoInfo)baseInfo;
                        string fileName = videoInfo.UserId;
                        string imgName = videoInfo.ImgName;

                        //EnvInfo envInfo = database.GetEnviroment();

                        //while (true)
                        //{
                        //    string path = string.Format("{0}\\Videos\\{1}", envInfo.ImageUploadPath, fileName);
                        //    string imgpath = string.Format("{0}\\Videos\\{1}", envInfo.ImageUploadPath, imgName);

                        //    if (File.Exists(path))
                        //    {
                        //        string[] part = fileName.Split('.');
                        //        fileName = part[0] + "1." + part[1];

                        //        string[] imgpart = imgName.Split('.');
                        //        imgName = imgpart[0] + "1." + imgpart[1];

                        //        continue;
                        //    }

                        //    File.WriteAllBytes(path, videoInfo.Data);
                        //    File.WriteAllBytes(imgpath, videoInfo.ImgData);
                        //    break;


                        //}

                        IconInfo iconInfo = new IconInfo();
                        iconInfo.Name = userInfo.Id;
                        iconInfo.Icon = string.Format("Images\\Face\\{0}", imgName);

                        IconInfo newInfo = database.AddVideo(iconInfo);

                        if (newInfo == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }
                        server.Send(socket, NotifyType.Request_VideoUpload, newInfo);
                    }
                    break;
                case NotifyType.Request_IconRemove:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            BaseInfo.SetError(ErrorType.Unknown_User, "不明会员准备删除过照片信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        IconInfo iconInfo = (IconInfo)baseInfo;

                        if (database.DelFace(iconInfo.Id) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        server.Send(socket, NotifyType.Reply_IconRemove, iconInfo);
                    }
                    break;

                case NotifyType.Request_UpdateUser:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员设定过使用设置.");
                            Main.ReplyError(socket);
                            return;
                        }

                        UserInfo updateInfo = (UserInfo)baseInfo;
                        userInfo.Body = updateInfo;

                        if (database.UpdateUser(userInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        server.Send(socket, NotifyType.Reply_UpdateUser, userInfo);
                        View._isUpdateUserList = true;
                    }
                    break;

                case NotifyType.Request_UpdatePercent:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员设定过使用设置.");
                            Main.ReplyError(socket);
                            return;
                        }

                        if (userInfo.Kind == (int)UserKind.Buyer ||
                            userInfo.Kind == (int)UserKind.ServiceWoman ||
                            userInfo.Kind == (int)UserKind.Recommender)
                        {
                            SetError(ErrorType.Notallow_Previlege, "没有权限的会员要修改百分比设置.");
                            Main.ReplyError(socket);
                            return;
                        }

                        UserInfo percentInfo = (UserInfo)baseInfo;

                        UserInfo targetInfo = database.FindUser(percentInfo.Id);

                        if (targetInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "要修改百分比的帐号不准确.");
                            Main.ReplyError(socket);
                            return;
                        }

                        targetInfo.ChatPercent = percentInfo.ChatPercent;
                        targetInfo.GamePercent = percentInfo.GamePercent;

                        if (database.UpdateUser(targetInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        UserInfo targetUser = Users.GetInstance().FindUser(targetInfo.Id);

                        if (targetUser != null)
                        {
                            targetUser.ChatPercent = targetInfo.ChatPercent;
                            targetUser.GamePercent = targetInfo.GamePercent;
                        }

                        server.Send(socket, NotifyType.Reply_UpdatePercent, targetInfo);
                        View._isUpdateUserList = true;
                    }
                    break;

                case NotifyType.Request_Reconnect:              // 2014-01-10: GreenRose
                    {
                        UserInfo loginInfo = (UserInfo)baseInfo;

                        UserInfo findInfo = Database.GetInstance().FindUser(loginInfo.Id);

                        if (findInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, string.Format("没登记的帐号要试着登陆软件: {0}", loginInfo.Id));
                            Main.ReplyError(socket);
                            return;
                        }

                        if (findInfo.Password != loginInfo.Password)
                        {
                            SetError(ErrorType.Invalid_Password, string.Format("登陆失败: {0}的密码错误: {1}", loginInfo.Id, loginInfo.Password));
                            Main.ReplyError(socket);
                            return;
                        }

                        if (ReconnectUser(findInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        findInfo.Socket = socket;
                        findInfo.PingDate = DateTime.Now;

                        string str = string.Format("{0} 已登陆.", findInfo.Id);
                        LogView.AddLogString(str);

                        ReplyUserList(null);

                        View._isUpdateUserList = true;
                    }
                    break;



                case NotifyType.Request_Login:
                    {
                        UserInfo loginInfo = (UserInfo)baseInfo;

                        UserInfo findInfo = Database.GetInstance().FindUser(loginInfo.Id);

                        if (findInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, string.Format("没登记的帐号要试着登陆软件: {0}", loginInfo.Id));
                            Main.ReplyError(socket);
                            return;
                        }

                        if (findInfo.Password != loginInfo.Password)
                        {
                            SetError(ErrorType.Invalid_Password, string.Format("登陆失败: {0}的密码错误: {1}", loginInfo.Id, loginInfo.Password));
                            Main.ReplyError(socket);
                            return;
                        }

                        if (LoginUser(findInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        findInfo.Socket = socket;
                        findInfo.PingDate = DateTime.Now;

                        //int udpPort = View._TcpPort + 2;

                        //while (findInfo.UdpPort == 0)
                        //{
                        //    bool bFound = false;

                        //    for (int i = 0; i < _Users.Count; i++)
                        //    {
                        //        if (_Users[i].UdpPort == udpPort)
                        //        {
                        //            bFound = true;
                        //            break;
                        //        }
                        //    }

                        //    if (bFound)
                        //        udpPort++;
                        //    else
                        //        findInfo.UdpPort = udpPort;
                        //}

                        string str = string.Format("{0} 已登陆.", findInfo.Id);
                        LogView.AddLogString(str);

                        server.Send(socket, NotifyType.Reply_Login, findInfo);

                        ReplyUserList(null);

                        BoardInfo boardInfo = Database.GetInstance().GetAdminNotice();
                        if (boardInfo != null)
                            server.Send(socket, NotifyType.Reply_AdminNotice, boardInfo);

                        View._isUpdateUserList = true;
                    }
                    break;

                case NotifyType.Request_Phone_Login:
                    {
                        UserInfo loginInfo = (UserInfo)baseInfo;

                        UserInfo findInfo = Database.GetInstance().FindUserByPhoneNumber(loginInfo.strPhoneNumber);

                        if (findInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, string.Format("没登记的帐号要试着登陆软件: {0}", loginInfo.strPhoneNumber));
                            Main.ReplyError(socket);
                            return;
                        }

                        //if (findInfo.Password != loginInfo.Password)
                        //{
                        //    SetError(ErrorType.Invalid_Password, string.Format("登陆失败: {0}的密码错误: {1}", loginInfo.Id, loginInfo.Password));
                        //    Main.ReplyError(socket);
                        //    return;
                        //}

                        if (LoginUser(findInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        findInfo.Socket = socket;
                        findInfo.PingDate = DateTime.Now;

                        //int udpPort = View._TcpPort + 2;

                        //while (findInfo.UdpPort == 0)
                        //{
                        //    bool bFound = false;

                        //    for (int i = 0; i < _Users.Count; i++)
                        //    {
                        //        if (_Users[i].UdpPort == udpPort)
                        //        {
                        //            bFound = true;
                        //            break;
                        //        }
                        //    }

                        //    if (bFound)
                        //        udpPort++;
                        //    else
                        //        findInfo.UdpPort = udpPort;
                        //}

                        string str = string.Format("{0} 已登陆.", findInfo.strPhoneNumber);
                        LogView.AddLogString(str);

                        server.Send(socket, NotifyType.Reply_Login, findInfo);

                        ReplyUserList(null);

                        View._isUpdateUserList = true;
                    }
                    break;

                case NotifyType.Request_Logout:
                    {
                        if (Logout(socket) == false)
                            Main.ReplyError(socket);
                    }
                    break;

                case NotifyType.Request_UserList:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请会员信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        ReplyUserList(userInfo);
                    }
                    break;

                // 2014-02-03: GreenRose
                case NotifyType.Request_UserState:
                    {
                        //UserInfo userInfo = FindUser(socket);
                        UserInfo userInfo = (UserInfo)baseInfo;

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        ReplyUserState(userInfo);
                    }
                    break;

                case NotifyType.Request_Home:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        HomeInfo homeInfo = new HomeInfo();

                        //homeInfo.Users = Database.GetInstance().GetBestUsers();
                        //homeInfo.Games = Database.GetInstance().GetAllGames();
                        homeInfo.Letters = Database.GetInstance().GetLetters(userInfo.Id);
                        homeInfo.Notices = Database.GetInstance().GetNotices(userInfo.Id);

                        server.Send(socket, NotifyType.Reply_Home, homeInfo);
                    }
                    break;

                case NotifyType.Request_Message:
                    {
                        UserInfo senderInfo = FindUser(socket);

                        if (senderInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请短信信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        StringInfo messageInfo = (StringInfo)baseInfo;

                        UserInfo userInfo = FindUser(messageInfo.UserId);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "短信对象不明.");
                            Main.ReplyError(socket);
                            return;
                        }

                        messageInfo.UserId = senderInfo.Id;
                        server.Send(userInfo.Socket, NotifyType.Reply_Message, messageInfo);
                    }
                    break;

                case NotifyType.Request_ChargeSiteUrl:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请银行信息网站.");
                            Main.ReplyError(socket);
                            return;
                        }

                        EnvInfo envInfo = Database.GetInstance().GetEnviroment();

                        StringInfo stringInfo = new StringInfo();
                        stringInfo.String = envInfo.ChargeSiteUrl;

                        Server.GetInstance().Send(socket, NotifyType.Reply_ChargeSiteUrl, stringInfo);
                    }
                    break;

                case NotifyType.Request_SendLetter:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请会员信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        BoardInfo boardInfo = (BoardInfo)baseInfo;

                        boardInfo.Kind = (int)BoardKind.Letter;
                        boardInfo.UserId = userInfo.Id;

                        if (Database.GetInstance().AddBoard(boardInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        List<BoardInfo> letters = Database.GetInstance().GetLetters(userInfo.Id);

                        if (letters == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        HomeInfo homeInfo = new HomeInfo();
                        homeInfo.Letters = letters;

                        for (int i = 0; i < homeInfo.Letters.Count; i++)
                        {
                            if (homeInfo.Letters[i].Readed != 1)
                            {
                                if (homeInfo.Letters[i].SendId == "admin")
                                    break;
                                else
                                {
                                    UserInfo liveUser = Users.GetInstance().FindUser(homeInfo.Letters[i].SendId);
                                    homeInfo.Notices = Database.GetInstance().GetNotices(liveUser.Id);
                                    Server.GetInstance().Send(liveUser.Socket, NotifyType.Reply_NoticeList, homeInfo);
                                }
                            }
                        }

                        Server.GetInstance().Send(socket, NotifyType.Reply_LetterList, homeInfo);


                    }
                    break;

                case NotifyType.Request_DelLetter:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请会员信息");
                            Main.ReplyError(socket);
                            return;
                        }

                        BoardInfo boardInfo = (BoardInfo)baseInfo;

                        if (Database.GetInstance().DelBoard(boardInfo.Id) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        List<BoardInfo> letters = Database.GetInstance().GetLetters(userInfo.Id);

                        if (letters == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        HomeInfo homeInfo = new HomeInfo();
                        homeInfo.Letters = letters;

                        Server.GetInstance().Send(socket, NotifyType.Reply_LetterList, homeInfo);
                    }
                    break;

                case NotifyType.Request_ReadNotice:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请会员信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        BoardInfo boardInfo = (BoardInfo)baseInfo;
                        boardInfo.UserId = userInfo.Id;
                        boardInfo.Readed = 1;

                        if (Database.GetInstance().UpdateBoard(boardInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        HomeInfo homeInfo = new HomeInfo();
                        homeInfo.Notices = Database.GetInstance().GetNotices(userInfo.Id);

                        if (homeInfo.Notices == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        Server.GetInstance().Send(socket, NotifyType.Reply_NoticeList, homeInfo);
                    }
                    break;

                case NotifyType.Request_DelNotice:
                    {
                        UserInfo userInfo = FindUser(socket);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "不明会员邀请会员信息.");
                            Main.ReplyError(socket);
                            return;
                        }

                        BoardInfo boardInfo = (BoardInfo)baseInfo;

                        if (Database.GetInstance().DelBoard(boardInfo.Id) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        HomeInfo homeInfo = new HomeInfo();
                        homeInfo.Notices = Database.GetInstance().GetNotices(userInfo.Id);

                        if (homeInfo.Notices == null)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        Server.GetInstance().Send(socket, NotifyType.Reply_NoticeList, homeInfo);
                    }
                    break;

                case NotifyType.Request_Evaluation:
                    {
                        EvalHistoryInfo evalInfo = (EvalHistoryInfo)baseInfo;

                        UserInfo userInfo = database.FindUser(evalInfo.OwnId);

                        if (userInfo == null)
                        {
                            SetError(ErrorType.Unknown_User, "评价对象不明.");
                            Main.ReplyError(socket);
                            return;
                        }

                        EvalHistoryInfo existInfo = database.GetEvalHistory(evalInfo.OwnId, evalInfo.BuyerId);

                        if (existInfo == null)
                        {
                            userInfo.Visitors++;
                            userInfo.Evaluation += evalInfo.Value;
                        }
                        else
                        {
                            userInfo.Evaluation -= existInfo.Value;
                            userInfo.Evaluation += evalInfo.Value;
                        }

                        if (database.UpdateUser(userInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        UserInfo loginInfo = Users.GetInstance().FindUser(evalInfo.OwnId);

                        if (loginInfo != null)
                        {
                            loginInfo.Visitors = userInfo.Visitors;
                            loginInfo.Evaluation = userInfo.Evaluation;

                            if (string.IsNullOrEmpty(loginInfo.RoomId) == false)
                                Chat.GetInstance().ReplyRoomDetailInfo(loginInfo.RoomId);

                            ReplyUserList(null);
                        }

                        if (database.AddEvalHistory(evalInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        //server.Send(socket, NotifyType.Reply_Evaluation, evalInfo);
                    }
                    break;

                // 2013-12-17: GreenRose
                case NotifyType.Request_PaymentInfo:
                    {
                        PaymentInfo paymentInfo = (PaymentInfo)baseInfo;
                        if (database.AddPaymentInfo(paymentInfo) == false)
                        {
                            Main.ReplyError(socket);
                            return;
                        }

                        UserInfo userInfo = database.FindUser(paymentInfo.strID);
                        server.Send(socket, NotifyType.Reply_UserInfo, userInfo);
                        server.Send(socket, NotifyType.Reply_PaymentInfo, paymentInfo);
                    }
                    break;

            }
        }

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

        Random _rnd = new Random();

        public void NotifyAutoCheck()
        {
            // added by usc at 2014/02/25
            List<GameInfo> gameList = Database.GetInstance().GetAllGames();

            if (gameList == null || gameList.Count == 0)
                return;

            for (int i = 0; i < _Users.Count; i++)
            {
                UserInfo userInfo = _Users[i];

                if (userInfo.Auto > 0 && userInfo.GameId == string.Empty)
                {
                    // added by usc at 2014/04/13
                    ChangeNickname(userInfo);

                    int nGameNo = _rnd.Next() % gameList.Count;

                    GameInfo gameInfo = gameList[nGameNo];

                    // modified by usc at 2014/02/26
                    gameInfo.nCashOrPointGame = _rnd.Next() % 2;

                    if (gameInfo.nCashOrPointGame == 0)
                    {
                        Auto.GetInstance().AddAuto(gameInfo, userInfo, true);
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

                        Auto.GetInstance().AddAuto(pointGameInfo, userInfo, true);
                    }

                    break;
                }
            }
        }

        public List<UserInfo> GetUsers()
        {
            return _Users;
        }

        //public List<UserInfo> GetRoomUsers(string roomId)
        //{
        //    List<UserInfo> roomUsers = new List<UserInfo>();

        //    for (int i = 0; i < _Users.Count; i++)
        //    {
        //        if (_Users[i].RoomId == roomId)
        //        {
        //            roomUsers.Add(_Users[i]);
        //        }
        //    }

        //    return roomUsers;
        //}

        // 2013-12-26: GreenRose
        //public List<UserInfo> GetRoomUsers(string roomID)
        //{
        //    List<UserInfo> roomUsers = new List<UserInfo>();

        //    List<MeetingInfo> listMeetingInfo = Chat.GetInstance().GetListRoomInfo();
        //    MeetingInfo meetingInfo = listMeetingInfo.Find(item => item._strRoomID == roomID);
        //    if (meetingInfo == null)
        //        return roomUsers;

        //    foreach (UserInfo userInfo in meetingInfo._listUserInfo)
        //    {
        //        roomUsers.Add(userInfo);
        //    }

        //    //roomUsers = meetingInfo._listUserInfo;

        //    if (roomUsers == null)
        //        roomUsers = new List<UserInfo>();

        //    return roomUsers;
        //}

        // 2013-12-29: GreenRose
        // GetAllRooms
        public List<RoomInfo> GetRoomListJoinedUser(string userID)
        {
            List<RoomInfo> listRoomInfo = new List<RoomInfo>();

            List<RoomInfo> listAllRoomInfo = Chat.GetInstance().GetListRoomInfo();
            listRoomInfo = listAllRoomInfo.FindAll(item => item._strUserID == userID);

            if (listRoomInfo == null)
                listRoomInfo = new List<RoomInfo>();

            return listRoomInfo;
        }

        public List<UserInfo> GetRoomUsers(string roomID)
        {
            List<UserInfo> roomUsers = new List<UserInfo>();

            List<RoomInfo> listRoomInfo = Chat.GetInstance().GetListRoomInfo();
            RoomInfo roomInfo = listRoomInfo.Find(item => item.Id == roomID);
            if (roomInfo == null)
                return roomUsers;

            //roomUsers = roomInfo.listUserInfo;

            //if (roomUsers == null)
            //    roomUsers = new List<UserInfo>();

            // 2013-12-27: GreenRose
            //foreach (string strID in roomInfo.listUserID)
            {
                for (int i = 0; i < _Users.Count; i++)
                {
                    if (_Users[i].Id == roomInfo._strUserID)
                    {
                        roomUsers.Add(_Users[i]);
                        break;
                    }
                }

                for (int i = 0; i < _Users.Count; i++)
                {
                    if (_Users[i].Id == roomInfo._strTargetID)
                    {
                        roomUsers.Add(_Users[i]);
                        break;
                    }
                }
            }

            return roomUsers;
        }


        public List<UserInfo> GetGameUsers(string gameId)
        {
            List<UserInfo> gameUsers = new List<UserInfo>();

            for (int i = 0; i < _Users.Count; i++)
            {
                if (_Users[i].GameId == gameId)
                {
                    gameUsers.Add(_Users[i]);
                }
            }

            return gameUsers;
        }

        public UserInfo FindUser(Socket socket)
        {
            UserInfo userInfo = null;

            for (int i = 0; i < _Users.Count; i++)
            {
                if (_Users[i].Socket == socket)
                {
                    userInfo = _Users[i];
                    break;
                }
            }

            return userInfo;
        }

        public UserInfo FindUser(IPEndPoint ipEndPoint)
        {
            UserInfo userInfo = null;

            for (int i = 0; i < _Users.Count; i++)
            {
                if (_Users[i].Socket != null)
                {
                    IPEndPoint endPoint = (IPEndPoint)_Users[i].Socket.RemoteEndPoint;

                    if (endPoint.Address.ToString() == ipEndPoint.Address.ToString() && _Users[i].UdpPort == ipEndPoint.Port)
                    {
                        userInfo = _Users[i];
                        break;
                    }
                }
            }

            return userInfo;
        }

        public UserInfo FindUser(string userId)
        {
            UserInfo userInfo = null;

            for (int i = 0; i < _Users.Count; i++)
            {
                if (_Users[i].Id == userId)
                {
                    userInfo = _Users[i];
                    break;
                }
            }

            return userInfo;
        }

        // 2014-03-18: GreenRose
        public bool ReconnectUser(UserInfo loginInfo)
        {
            try
            {
                UserInfo userInfo = FindUser(loginInfo.Id);

                if (userInfo != null)
                {
                    Logout(userInfo.Socket);
                }

                _Users.Add(loginInfo);

                loginInfo.LoginTime = BaseInfo.ConvDateToString(DateTime.Now);
                Database.GetInstance().UpdateUser(loginInfo);
            }
            catch (Exception ex)
            {
                BaseInfo.SetError(ErrorType.Unknown_User, "GreenRose - ReconnectUser() Function Exception." + ex.ToString());
            }

            return true;
        }

        public bool LoginUser(UserInfo loginInfo)
        {
            //UserInfo userInfo = FindUser(loginInfo.Socket);

            //if (userInfo != null)
            //{
            //    string errorStr = string.Format("{0} 重复登陆过", userInfo.Id);
            //    SetError(ErrorType.Duplicate_login, errorStr);
            //    return false;
            //}

            UserInfo userInfo = FindUser(loginInfo.Id);

            if (userInfo != null)
            {
                //string errorStr = string.Format("{0} 重复登陆过", userInfo.Id);
                //SetError(ErrorType.Duplicate_login, errorStr);
                //return false;            
                string errorStr = string.Format("{0} 重复登陆过", userInfo.Id);
                SetError(ErrorType.Duplicate_logout, errorStr);
                Main.ReplyError(userInfo.Socket);
                Logout(userInfo.Socket);
            }

            if (loginInfo.LoginTime.Trim() == "")
            {
                EnvInfo envInfo = Database.GetInstance().GetEnviroment();
                loginInfo.Point += envInfo.LoginBonusPoint;
            }
            else // 2014-01-07: GreenRose
            {
                DateTime dateTimeLastLogin = Convert.ToDateTime(loginInfo.LoginTime).Date;
                DateTime dateTimeCur = DateTime.Now.Date;

                if (dateTimeCur > dateTimeLastLogin)
                {
                    Database.GetInstance().UpdateLoginCount(loginInfo.Id, 0);
                }

                if (loginInfo.Point <= 105 && loginInfo.nLoginCount < 3)
                {
                    EnvInfo envInfo = Database.GetInstance().GetEnviroment();
                    loginInfo.Point += envInfo.EveryDayPoint;

                    loginInfo.nLoginCount = loginInfo.nLoginCount + 1;
                    Database.GetInstance().UpdateLoginCount(loginInfo.Id, loginInfo.nLoginCount);
                }
            }

            _Users.Add(loginInfo);

            loginInfo.LoginTime = BaseInfo.ConvDateToString(DateTime.Now);
            Database.GetInstance().UpdateUser(loginInfo);

            return true;
        }

        public bool Logout(Socket socket)
        {
            UserInfo userInfo = FindUser(socket);

            if (userInfo == null)
                return false;

            //Chat.GetInstance().OutMeeting(socket, null);
            // 2013-12-29: GreenRose
            List<RoomInfo> listRoomInfo = GetRoomListJoinedUser(userInfo.Id);
            foreach (RoomInfo roomInfo in listRoomInfo)
            {
                Chat.GetInstance().OutMeeting(socket, roomInfo);
            }

            //Chat.GetInstance().OutRoom(socket);
            Game.GetInstance().OutGame(userInfo);

            _Users.Remove(userInfo);

            View._isUpdateUserList = true;

            string str = string.Format("{0} 退出.", userInfo.Id);
            LogView.AddLogString(str);

            Server.GetInstance().Send(socket, NotifyType.Reply_Logout, userInfo);

            ReplyUserList(null);

            // 2014-04-05: GreenRose
            Server.GetInstance().RemoveSocketBuffer(socket);

            return true;
        }

        public bool ReplyUserList(UserInfo userInfo)
        {
            List<UserInfo> replyUsers = new List<UserInfo>();

            if (userInfo != null)
                replyUsers.Add(userInfo);
            else
                replyUsers = _Users;

            UserListInfo buyerListInfo = new UserListInfo();
            UserListInfo servicemanListInfo = new UserListInfo();

            for (int i = 0; i < _Users.Count; i++)
            {
                if (_Users[i].Kind == (int)UserKind.ServiceWoman)
                    servicemanListInfo._Users.Add(_Users[i]);
                else
                {
                    // modified by usc at 2014/03/09
                    if (_Users[i].Auto == 0)
                        buyerListInfo._Users.Add(_Users[i]);
                }
            }

            for (int i = 0; i < replyUsers.Count; i++)
            {
                UserInfo replyUser = replyUsers[i];

                if (replyUser.Kind == (int)UserKind.ServiceWoman)
                    Server.GetInstance().Send(replyUser.Socket, NotifyType.Reply_UserList, buyerListInfo);
                else
                    Server.GetInstance().Send(replyUser.Socket, NotifyType.Reply_UserList, servicemanListInfo);
            }

            return true;
        }

        // 2014-02-03: GreenRose
        public bool ReplyUserState(UserInfo userInfo)
        {
            List<UserInfo> replyUsers = new List<UserInfo>();

            for (int i = 0; i < _Users.Count; i++)
            {
                if (_Users[i].Id == userInfo.Id)
                {
                    _Users[i].nUserState = userInfo.nUserState;
                    continue;
                }

                Server.GetInstance().Send(_Users[i].Socket, NotifyType.Reply_UserState, userInfo);
            }

            return true;
        }

        public UserDetailInfo GetUserDetailInfo(string userId)
        {
            UserInfo userInfo = Database.GetInstance().FindUser(userId);

            if (userInfo == null)
            {
                BaseInfo.SetError(ErrorType.Unknown_User, "不明会员要获取个人信息.");
                return null;
            }

            UserDetailInfo userDetailInfo = new UserDetailInfo();

            userDetailInfo.Partners = Database.GetInstance().GetPartners(userInfo);
            if (userDetailInfo.Partners == null)
                userDetailInfo.Partners = new List<UserInfo>();
            //userDetailInfo.Rooms = Database.GetInstance().FindOwnRooms(userInfo.Id);
            //if (userDetailInfo.Rooms == null)
            //    userDetailInfo.Rooms = new List<RoomInfo>();
            userDetailInfo.Faces = Database.GetInstance().GetAllFaces(userInfo.Id);
            if (userDetailInfo.Faces == null)
                userDetailInfo.Faces = new List<IconInfo>();
            //userDetailInfo.ChatHistories = Database.GetInstance().GetAllChatHistories(userInfo.Id);
            //if (userDetailInfo.ChatHistories == null)
            //    userDetailInfo.ChatHistories = new List<ChatHistoryInfo>();
            userDetailInfo.ChargeHistories = Database.GetInstance().GetAllChargeList(userInfo.Id);
            if (userDetailInfo.ChargeHistories == null)
                userDetailInfo.ChargeHistories = new List<ChargeHistoryInfo>();
            //userDetailInfo.EvalHistories = Database.GetInstance().GetAllEvalHistoryList(userInfo.Id);
            //if (userDetailInfo.EvalHistories == null)
            //    userDetailInfo.EvalHistories = new List<EvalHistoryInfo>();
            userDetailInfo.PresentHistories = Database.GetInstance().GetAllPresentHistories(userInfo.Id);
            if (userDetailInfo.PresentHistories == null)
                userDetailInfo.PresentHistories = new List<PresentHistoryInfo>();

            DateTime curTime = DateTime.Now;
            userDetailInfo.GameHistories = Database.GetInstance().GetAllGameHistories(userInfo.Id, curTime.Year, curTime.Month, curTime.Day);
            if (userDetailInfo.GameHistories == null)
                userDetailInfo.GameHistories = new List<GameHistoryInfo>();

            string configPath = System.IO.Path.GetFullPath("Config.ini");
            IniFileEdit configInfo = new IniFileEdit(configPath);
            string strDownUrl = configInfo.GetIniValue("DownloadURL", "DOWNURL");

            userDetailInfo._strDownUrl = strDownUrl;

            return userDetailInfo;
        }
    }
}
