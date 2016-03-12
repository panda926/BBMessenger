using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.Windows.Forms;

namespace ChatServer
{
    public class Cash : BaseInfo
    {
        private static Cash _instance = null;

        public static Cash GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Cash();
            }

            return _instance;
        }

        public void NotifyCash()
        {
            List<RoomInfo> meetList = Chat.GetInstance().GetMeetings();

            if (meetList != null)
            {
                for (int i = 0; i < meetList.Count; i++)
                {
                    RoomInfo meetInfo = meetList[i];

                    if (meetInfo.Cash == 0)
                        continue;

                    List<UserInfo> users = Users.GetInstance().GetRoomUsers(meetInfo.Id);

                    for (int k = 0; k < users.Count; k++)
                    {
                        UserInfo userInfo = users[k];

                        if (userInfo.WaitSecond == 0)
                            continue;

                        if (DateTime.Now <= userInfo.CashTime)
                            continue;

                        ProcessChatCash(userInfo, meetList[i], true);
                    }
                }
            }

            //List<RoomInfo> roomList = Chat.GetInstance().GetRooms();

            //if (roomList != null)
            //{
            //    for (int i = 0; i < roomList.Count; i++)
            //        ProcessChatCash(roomList[i], false );
            //}
        }

        public bool GiveCash(string userId, int cash)
        {
            if (cash == 0)
                return true;

            UserInfo userInfo = Database.GetInstance().FindUser(userId);

            if (userInfo == null)
            {
                SetError(ErrorType.Unknown_User, "收得元宝的会员信息不准确.");
                return false;
            }

            userInfo.Cash += cash;

            if (userInfo.Cash < 0 && userInfo.Kind == (int)UserKind.Buyer)
            {
                SetError(ErrorType.Notenough_Point, "余额不足.");
                return false;
            }

            if (Database.GetInstance().UpdateUser(userInfo) == false)
                return false;

            UserInfo loginInfo = Users.GetInstance().FindUser(userId);

            if (loginInfo != null)
            {
                loginInfo.Cash = userInfo.Cash;
                View._isUpdateUserList = true;

                if (loginInfo.Auto == 0)
                    Server.GetInstance().Send(loginInfo.Socket, NotifyType.Reply_UserInfo, loginInfo);
            }

            return true;
        }

        public bool GiveCashOrPoint(string userId, int cash, GameInfo gameInfo)
        {
            if (cash == 0)
                return true;

            UserInfo userInfo = Database.GetInstance().FindUser(userId);

            if (userInfo == null)
            {
                SetError(ErrorType.Unknown_User, "收得元宝的会员信息不准确.");
                return false;
            }

            if (gameInfo.nCashOrPointGame == 0)
            {
                userInfo.Cash += cash;

                if (userInfo.Cash < 0)
                {
                    SetError(ErrorType.Notenough_Cash, "余额不足.");
                    return false;
                }
            }
            else
            {
                userInfo.Point += cash;

                if (userInfo.Point < 0)
                {
                    SetError(ErrorType.Notenough_Point, "积分不足.");
                    return false;
                }
            }

            if (Database.GetInstance().UpdateUser(userInfo) == false)
                return false;

            UserInfo loginInfo = Users.GetInstance().FindUser(userId);

            if (loginInfo != null)
            {
                if (gameInfo.nCashOrPointGame == 0)
                    loginInfo.Cash = userInfo.Cash;
                else
                    loginInfo.Point = userInfo.Point;

                View._isUpdateUserList = true;

                // deleted by usc at 2014/03/04
                //if (loginInfo.Auto == 0)
                //    Server.GetInstance().Send(loginInfo.Socket, NotifyType.Reply_UserInfo, loginInfo);
            }

            return true;
        }

        public bool GiveChatSum(string userId, int chatSum)
        {
            if (GiveCash(userId, chatSum) == false)
                return false;

            if (chatSum == 0)
                return true;

            UserInfo userInfo = Database.GetInstance().FindUser(userId);

            if (userInfo == null)
                return false;

            userInfo.ChatSum += chatSum;

            if (Database.GetInstance().UpdateUser(userInfo) == false)
                return false;

            UserInfo loginInfo = Users.GetInstance().FindUser(userId);

            if (loginInfo != null)
            {
                loginInfo.ChatSum = userInfo.ChatSum;
                View._isUpdateUserList = true;
            }

            return true;
        }

        public bool GiveGameSum(string userId, int gameSum, GameInfo gameInfo)
        {
            if (GiveCashOrPoint(userId, gameSum, gameInfo) == false)
                return false;

            if (gameSum == 0)
                return true;

            UserInfo userInfo = Database.GetInstance().FindUser(userId);

            if (userInfo == null)
                return false;

            userInfo.GameSum += gameSum;

            if (Database.GetInstance().UpdateUser(userInfo) == false)
                return false;

            UserInfo loginInfo = Users.GetInstance().FindUser(userId);

            if (loginInfo != null)
            {
                loginInfo.GameSum = userInfo.GameSum;
                View._isUpdateUserList = true;
            }

            return true;
        }

        public bool GiveChargeSum(string userId, int chargeSum)
        {
            if (chargeSum == 0)
                return true;

            UserInfo userInfo = Database.GetInstance().FindUser(userId);

            if (userInfo == null)
                return false;

            userInfo.ChargeSum += chargeSum;

            if (Database.GetInstance().UpdateUser(userInfo) == false)
                return false;

            UserInfo loginInfo = Users.GetInstance().FindUser(userId);

            if (loginInfo != null)
            {
                loginInfo.ChargeSum = userInfo.ChargeSum;
                View._isUpdateUserList = true;
            }

            return true;
        }

        public bool GivePresentSum(string userId, int presentSum)
        {
            if (presentSum == 0)
                return true;

            UserInfo userInfo = Database.GetInstance().FindUser(userId);

            if (userInfo == null)
                return false;

            if (presentSum < 0)
                userInfo.SendSum += presentSum;
            else
                userInfo.ReceiveSum += presentSum;

            if (Database.GetInstance().UpdateUser(userInfo) == false)
                return false;

            UserInfo loginInfo = Users.GetInstance().FindUser(userId);

            if (loginInfo != null)
            {
                loginInfo.SendSum = userInfo.SendSum;
                loginInfo.ReceiveSum = userInfo.ReceiveSum;
                View._isUpdateUserList = true;
            }

            return true;
        }

        public int ProcessCashPercent(UserInfo userInfo, int percent, int cash)
        {
            if (userInfo == null)
                return 0;

            int percentCash = cash * percent / 100;

            //if (GiveCash(userInfo.Id, percentCash) == false)
            //    return 0;

            return percentCash;
        }

        public bool ProcessChatCash(UserInfo userInfo, RoomInfo roomInfo, bool bMeeting)
        {
            //List<UserInfo> users = Users.GetInstance().GetRoomUsers(roomInfo.Id);

            //if (users == null)
            //{
            //    ErrorView.AddErrorString();
            //    return false;
            //}

            //bool bUpdate = false;

            //for (int i = 0; i < users.Count; i++)
            //{
            //    UserInfo userInfo = users[i];

            if (userInfo.Kind == (int)UserKind.Manager ||
                userInfo.Kind == (int)UserKind.ServiceWoman ||
                userInfo.Kind == (int)UserKind.ServiceOfficer)
                return true;

            //bUpdate = true;

            if (roomInfo.Cash > 0)
            {
                if (GiveChatSum(userInfo.Id, -roomInfo.Cash) == false)
                {
                    Chat.GetInstance().OutMeeting(userInfo.Socket, roomInfo);
                    return false;
                }

                if (userInfo.Auto > 0)
                    return true;

                //GiveChatSum(userInfo.Id, -roomInfo.Cash);

                if (bMeeting == true)
                    userInfo.CashTime = userInfo.CashTime.AddMinutes(1);
                else
                    userInfo.CashTime = userInfo.CashTime.AddYears(1);

                UserInfo servicer = Database.GetInstance().FindUser(roomInfo.Owner);
                UserInfo serviceOfficer = Database.GetInstance().FindUser(servicer.Recommender); // Database.GetInstance().GetManager(UserKind.ServiceOfficer);
                UserInfo manager = Database.GetInstance().GetManager(UserKind.Manager)[0];

                int servicePercent = servicer.ChatPercent;
                int serviceOfficerPercent = serviceOfficer.ChatPercent;

                if (servicePercent > 100)
                    servicePercent = 100;

                if (serviceOfficerPercent > 100)
                    serviceOfficerPercent = 100;

                if (servicePercent > serviceOfficerPercent)
                    servicePercent = serviceOfficerPercent;

                serviceOfficerPercent -= servicePercent;

                int serviceCash = ProcessCashPercent(servicer, servicePercent, roomInfo.Cash);
                int serviceOfficerCash = ProcessCashPercent(serviceOfficer, serviceOfficerPercent, roomInfo.Cash);
                int managerCash = roomInfo.Cash - serviceOfficerCash - serviceCash;

                if (servicer != null)
                    GiveChatSum(servicer.Id, serviceCash);

                if (serviceOfficer != null)
                    GiveChatSum(serviceOfficer.Id, serviceOfficerCash);

                GiveChatSum(manager.Id, managerCash);

                ChatHistoryInfo chatHistoryInfo = Database.GetInstance().FindChatHistory(userInfo.ChatHistoryId);

                if (chatHistoryInfo != null)
                {
                    chatHistoryInfo.OfficerId = serviceOfficer.Id;
                    chatHistoryInfo.ManagerId = manager.Id;

                    chatHistoryInfo.BuyerTotal -= roomInfo.Cash;
                    chatHistoryInfo.ServicemanTotal += serviceCash;
                    chatHistoryInfo.ServiceOfficerTotal += serviceOfficerCash;
                    chatHistoryInfo.ManagerTotal += managerCash;
                    chatHistoryInfo.EndTime = DateTime.Now;

                    Database.GetInstance().UpdateChatHistory(chatHistoryInfo);
                }

                View._isUpdateRoomList = true;
            }

            if (roomInfo.Point > 0)
            {
                if (userInfo.Point < roomInfo.Point)
                {
                    SetError(ErrorType.Notenough_Point, string.Format("因{0}的账号余额不足 退出聊天频道.", userInfo.Id));
                    Main.ReplyError(userInfo.Socket);

                    Chat.GetInstance().OutMeeting(userInfo.Socket, roomInfo);
                    return false;
                }

                UserInfo updateInfo = Database.GetInstance().FindUser(userInfo.Id);

                if (updateInfo == null)
                {
                    Chat.GetInstance().OutMeeting(userInfo.Socket, roomInfo);
                    return false;
                }

                updateInfo.Point -= roomInfo.Point;

                if (Database.GetInstance().UpdateUser(updateInfo) == false)
                {
                    Chat.GetInstance().OutMeeting(userInfo.Socket, roomInfo);
                    return false;
                }

                userInfo.Point = updateInfo.Point;

                PointHistoryInfo pointHistoryInfo = Database.GetInstance().FindPointHistory(userInfo.PointHistoryId);

                if (pointHistoryInfo != null)
                {
                    pointHistoryInfo.Point -= roomInfo.Point;
                    pointHistoryInfo.AgreeTime = DateTime.Now;

                    Database.GetInstance().UpdatePointHistory(pointHistoryInfo);
                }

                userInfo.CashTime = userInfo.CashTime.AddMinutes(1);

                View._isUpdateUserList = true;
                View._isUpdateRoomList = true;
            }
            //}

            //if( bUpdate == true )
            Chat.GetInstance().ReplyRoomDetailInfo(roomInfo.Id);

            return true;
        }

        public bool ProcessCharge( string userID, int money )
        {
            EnvInfo envInfo = Database.GetInstance().GetEnviroment();
            UserInfo manager = Database.GetInstance().GetManager(UserKind.Manager)[0];

            int cash = money * envInfo.CashRate;
            int serviceCash = cash * envInfo.ChargeGivePercent / 100;

            // 수수료를 관리자에게서 제한다.
            if (GiveCash(manager.Id, -serviceCash) == false)
                return false;

            if (GiveCash(userID, cash) == false)
                return false;

            // 고객 충전기록
            ChargeHistoryInfo chargeHistoryInfo = new ChargeHistoryInfo();

            chargeHistoryInfo.Kind = (int)ChargeKind.Charge;
            chargeHistoryInfo.Cash = cash;
            chargeHistoryInfo.StartTime = DateTime.Now;
            chargeHistoryInfo.EndTime = DateTime.Now;
            chargeHistoryInfo.OwnId = userID;
            chargeHistoryInfo.ApproveId = Users.ManagerInfo.Id;
            chargeHistoryInfo.BankAccount = string.Format( "收费 {0}", money - money * envInfo.ChargeGivePercent / 100 );

            if (Database.GetInstance().AddChargeHistory(chargeHistoryInfo) == false)
                return false;

            // 관리자 수수료기록
            chargeHistoryInfo = new ChargeHistoryInfo();

            chargeHistoryInfo.Kind = (int)ChargeKind.Commission;
            chargeHistoryInfo.Cash = -serviceCash;
            chargeHistoryInfo.StartTime = DateTime.Now;
            chargeHistoryInfo.EndTime = DateTime.Now;
            chargeHistoryInfo.OwnId = manager.Id;
            chargeHistoryInfo.ApproveId = Users.ManagerInfo.Id;
            chargeHistoryInfo.BankAccount = string.Format( "佣金 {0}", money * envInfo.ChargeGivePercent / 100 );

            if (Database.GetInstance().AddChargeHistory(chargeHistoryInfo) == false)
                return false;


            // 2014-02-05: GreenRose
            // 사용자가에 충전상태를 알린다.
            UserInfo userInfo = Database.GetInstance().FindUser(userID);
            Server.GetInstance().Send(userInfo.Socket, NotifyType.Reply_UserInfo, userInfo);

            // 충전상태를 메세지로 보낸다.
            BoardInfo chargeBoardInfo = new BoardInfo();
            chargeBoardInfo.UserId = Users.ManagerInfo.Id;
            chargeBoardInfo.SendId = userInfo.Id;
            chargeBoardInfo.Title = "通知";
            chargeBoardInfo.Content = string.Format( "{0}缓冲填充. 您当前的缓存是{1}", cash, userInfo.Cash );
            chargeBoardInfo.Kind = (int)BoardKind.Letter;

            if (Database.GetInstance().AddBoard(chargeBoardInfo) == false)
            {                
                return false;
            }

            List<BoardInfo> letters = Database.GetInstance().GetLetters(Users.ManagerInfo.Id);

            if (letters == null)
            {
                return false;
            }

            HomeInfo homeInfo = new HomeInfo();
            homeInfo.Letters = letters;

            for (int i = 0; i < homeInfo.Letters.Count; i++)
            {
                if (homeInfo.Letters[i].Readed != 1)
                {
                    if (homeInfo.Letters[i].SendId == Users.ManagerInfo.Id)
                        break;
                    else
                    {
                        UserInfo liveUser = Users.GetInstance().FindUser(homeInfo.Letters[i].SendId);

                        if (liveUser != null)
                        {
                            homeInfo.Notices = Database.GetInstance().GetNotices(liveUser.Id);
                            Server.GetInstance().Send(liveUser.Socket, NotifyType.Reply_NoticeList, homeInfo);
                        }
                    }
                }
            }

            return true;
        }

        public bool ProcessPoint(string userId, int point)
        {
            UserInfo userInfo = Database.GetInstance().FindUser(userId);

            if (userInfo == null)
            {
                SetError(ErrorType.Unknown_User, "申请者信息不准确.");
                return false;
            }

            userInfo.Point += point;

            if (userInfo.Point < 0)
            {
                SetError(ErrorType.Notenough_Point, "积分不足.");
                return false;
            }

            if (Database.GetInstance().UpdateUser(userInfo) == false)
            {
                ErrorInfo errorInfo = BaseInfo.GetError();
                MessageBox.Show(errorInfo.ErrorString);
                return false;
            }

            UserInfo loginUserInfo = Users.GetInstance().FindUser(userId);

            if (loginUserInfo != null)
            {
                loginUserInfo.Point = userInfo.Point;
                View._isUpdateUserList = true;
            }

            PointHistoryInfo pointHistoryInfo = new PointHistoryInfo();

            pointHistoryInfo.TargetId = userId;
            pointHistoryInfo.Point = point;
            pointHistoryInfo.AgreeTime = DateTime.Now;
            pointHistoryInfo.Content = "manager";

            if (Database.GetInstance().AddPointHistory(pointHistoryInfo) == 0)
                return false;

            return true;
        }

        public bool ProcessPresent(PresentHistoryInfo presentInfo)
        {
            if (GiveCash(presentInfo.SendId, -presentInfo.Cash) == false)
                return false;

            GivePresentSum(presentInfo.SendId, -presentInfo.Cash);

            if (GiveCash(presentInfo.ReceiveId, presentInfo.Cash) == false)
                return false;

            GivePresentSum(presentInfo.ReceiveId, presentInfo.Cash);

            presentInfo.SendTime = DateTime.Now;

            if (Database.GetInstance().AddPresentHistory(presentInfo) == false)
                return false;

            return true;
        }

        public bool ProcessGameCash(int userIndex, GameInfo gameInfo, TableInfo tableInfo)
        {
            UserInfo admin = null;
            UserInfo manager = null;

            List<UserInfo> listAdmin= Database.GetInstance().GetManager(UserKind.Administrator);
            List<UserInfo> listManager = Database.GetInstance().GetManager(UserKind.Manager);

            if (listAdmin.Count > 0 && listManager.Count > 0)
            {
                admin = listAdmin[0];
                manager = listManager[0];
            }
            else
                return false;

            UserInfo userInfo = tableInfo._Players[userIndex];

            int userCash = 0;
            int recommenderCash = 0;
            int managerCash = 0;
            int commissionCash = 0;
                        
            commissionCash = tableInfo.m_lUserBetScore[userIndex] * gameInfo.Commission / 100;              // 관리자:
            userCash = tableInfo.m_lUserWinScore[userIndex] - commissionCash;                               // 사용자:

            if (userCash == 0 && commissionCash == 0)
                return true;

            if (gameInfo.nCashOrPointGame == 0 && userInfo.Auto == 0 && userInfo.Kind == (int)UserKind.Buyer)
            {
                UserInfo recommender = null;

                if (string.IsNullOrEmpty(userInfo.Recommender) == false)
                    recommender = Database.GetInstance().FindUser(userInfo.Recommender);

                int recommenderPercent = 0;

                if (recommender != null)
                {
                    recommenderPercent = recommender.GamePercent;

                    if (recommenderPercent > 100)
                        recommenderPercent = 100;
                }

                //commissionCash = tableInfo.m_lUserBetScore[userIndex] * gameInfo.Commission / 100;              // 관리자:
                
                // added by usc at 2014/04/09
                //GiveGameSum(admin.Id, commissionCash, gameInfo);

                //userCash = tableInfo.m_lUserWinScore[userIndex] - commissionCash;                               // 사용자:

                // 광고대리가 있으면
                if (recommender != null)
                {
                    managerCash = -tableInfo.m_lUserWinScore[userIndex] * (100 - recommenderPercent) / 100;     // 경영자:  
                    recommenderCash = -tableInfo.m_lUserWinScore[userIndex] * recommenderPercent / 100;         // 광고대리:                         

                    GiveGameSum(admin.Id, commissionCash, gameInfo);
                    GiveGameSum(manager.Id, managerCash, gameInfo);
                    GiveGameSum(recommender.Id, recommenderCash, gameInfo);
                }
                else
                {
                    managerCash = -tableInfo.m_lUserWinScore[userIndex];                                        // 경영자:

                    GiveGameSum(admin.Id, commissionCash, gameInfo);
                    GiveGameSum(manager.Id, managerCash, gameInfo);
                }

                GameHistoryInfo gameHistoryInfo = Database.GetInstance().FindGameHistory(userInfo.GameHistoryId);

                if (gameHistoryInfo != null)
                {
                    gameHistoryInfo.Id = userInfo.GameHistoryId;
                    gameHistoryInfo.BuyerTotal += userCash; // modified by usc at 2014/03/20
                    gameHistoryInfo.RecommenderTotal += recommenderCash;
                    gameHistoryInfo.RecommendOfficerTotal += 0;
                    gameHistoryInfo.ManagerId = manager.Id;
                    gameHistoryInfo.ManagerTotal += managerCash;

                    // added newly
                    gameHistoryInfo.AdminTotal += commissionCash;

                    gameHistoryInfo.EndTime = DateTime.Now;

                    Database.GetInstance().UpdateGameHistory(gameHistoryInfo);
                }
                
            }

            if (GiveGameSum(userInfo.Id, userCash, gameInfo) == false)
            {
                // modified by usc at 2014/06/15
                //if (userInfo.Auto > 0)
                //    Users.GetInstance().Logout(userInfo.Socket);
                //else
                //    Game.GetInstance().OutGame(userInfo);

                Game.GetInstance().OutGame(userInfo);
            }

            return true;

            //UserInfo manager = Database.GetInstance().GetManager(UserKind.Manager)[0];
            //UserInfo userInfo = tableInfo._Players[userIndex];

            //int recommenderCash = 0;
            //int recommendOfficerCash = 0;
            //int managerCash = 0;

            //if (userInfo.Auto == 0)
            //    managerCash = -tableInfo.m_lUserWinScore[userIndex];


            //if (userInfo.Kind == (int)UserKind.Buyer && tableInfo.m_lUserWinScore[userIndex] > 0 && gameInfo.Commission > 0 && userInfo.Auto == 0)
            //{
            //    UserInfo recommender = null;
            //    UserInfo recommendOfficer = null;

            //    if (string.IsNullOrEmpty(userInfo.Recommender) == false)
            //    {
            //        recommender = Database.GetInstance().FindUser(userInfo.Recommender);
            //        recommendOfficer = Database.GetInstance().FindUser(recommender.Recommender);
            //    }
            //    else
            //    {
            //        UserInfo friendInfo = Database.GetInstance().FindUser(userInfo.Friend);

            //        if (friendInfo != null && friendInfo.Kind != (int)UserKind.Buyer)
            //        {
            //            recommender = friendInfo;
            //        }
            //    }

            //    int commissionCash = tableInfo.m_lUserWinScore[userIndex] * gameInfo.Commission / 100;
            //    tableInfo.m_lUserWinScore[userIndex] -= commissionCash;

            //    int recommenderPercent = 0;

            //    if (recommender != null)
            //    {
            //        recommenderPercent = recommender.GamePercent;

            //        if (recommenderPercent > 100)
            //            recommenderPercent = 100;
            //    }

            //    int recommendOfficerPercent = 0;

            //    if (recommendOfficer != null)
            //    {
            //        recommendOfficerPercent = recommendOfficer.GamePercent;

            //        if (recommendOfficerPercent > 100)
            //            recommendOfficerPercent = 100;
            //    }

            //    if (recommenderPercent > recommendOfficerPercent)
            //        recommenderPercent = recommendOfficerPercent;

            //    recommendOfficerPercent -= recommenderPercent;

            //    recommenderCash = ProcessCashPercent(recommender, recommenderPercent, commissionCash);
            //    recommendOfficerCash = ProcessCashPercent(recommendOfficer, recommendOfficerPercent, commissionCash);
            //    managerCash += commissionCash - recommendOfficerCash - recommenderCash;


            //    if (recommender != null)
            //        GiveGameSum(recommender.Id, recommenderCash, gameInfo);

            //    if (recommendOfficer != null)
            //        GiveGameSum(recommendOfficer.Id, recommendOfficerCash, gameInfo);

            //}

            //GiveGameSum(manager.Id, managerCash, gameInfo);

            ////GiveCash( userInfo.Id, tableInfo.m_lUserWinScore[i] );
            //if (GiveGameSum(userInfo.Id, tableInfo.m_lUserWinScore[userIndex], gameInfo) == false)
            //{
            //    if (userInfo.Auto > 0)
            //        Users.GetInstance().Logout(userInfo.Socket);
            //    else
            //        Game.GetInstance().OutGame(userInfo);
            //}

            //GameHistoryInfo gameHistoryInfo = Database.GetInstance().FindGameHistory(userInfo.GameHistoryId);

            //if (gameHistoryInfo != null)
            //{
            //    gameHistoryInfo.Id = userInfo.GameHistoryId;
            //    gameHistoryInfo.BuyerTotal += tableInfo.m_lUserWinScore[userIndex];
            //    gameHistoryInfo.RecommenderTotal += recommenderCash;
            //    gameHistoryInfo.RecommendOfficerTotal += recommendOfficerCash;
            //    gameHistoryInfo.ManagerTotal += managerCash;
            //    gameHistoryInfo.EndTime = DateTime.Now;

            //    Database.GetInstance().UpdateGameHistory(gameHistoryInfo);
            //}
        }

    }
}
