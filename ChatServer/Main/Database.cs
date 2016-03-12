using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ChatEngine;
using System.Windows.Forms;

namespace ChatServer
{
    public class Database : BaseInfo
    {
        private static Database _instance = null;

        public static string _IP = null;

        public static Database GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Database();
            }

            return _instance;
        }

        private string GetConnectionString()
        {
            _IP = "210.116.91.44";


            return "Data Source=" + _IP + ";Initial Catalog=dbChat;Persist Security Info=True;User ID=sa;Password=NeedPlus1102;Connection Timeout=15";

            //return "Data Source=(local);Initial Catalog=dbChat;Integrated Security=SSPI;";
        }

        private SqlConnection ConnectDb()
        {
            string connectionString = GetConnectionString();

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                SetError(ErrorType.Fail_OpenDB, "资料库连接失败: " + e.Message);

                connection = null;
            }

            return connection;
        }

        private DataRowCollection GetData(string queryString)
        {
            SqlConnection connection = ConnectDb();
            if (connection == null)
                return null;

            DataRowCollection rows = null;

            try
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                if (dataSet.Tables.Count > 0)
                {
                    rows = dataSet.Tables[0].Rows;
                }
                else
                {
                    SetError(ErrorType.Fail_GetDB, "资料库读取失败: ");
                }
            }
            catch (Exception er)
            {
                SetError(ErrorType.Fail_GetDB, "资料库读取失败: " + er.Message);
            }
            finally
            {
                connection.Close();
            }

            return rows;
        }

        private bool UpdateData(string queryString)
        {
            SqlConnection connection = ConnectDb();
            if (connection == null)
                return false;

            bool ret = false;

            try
            {
                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandText = queryString;

                int affectRows = command.ExecuteNonQuery();

                if (affectRows > 0)
                    ret = true;
            }
            catch (Exception er)
            {
                SetError(ErrorType.Fail_WriteDB, "资料库填写失败: " + er.Message);
                ErrorView.AddErrorString();
            }
            finally
            {
                connection.Close();
            }

            return ret;
        }

        public bool AddUser(UserInfo userInfo)
        {
            UserInfo findInfo = FindUser(userInfo.Id);

            if (findInfo != null)
            {
                SetError(ErrorType.Duplicate_Id, "已存在帐号.");
                return false;
            }

            string strQuery = string.Format("insert into tblUser (Id, Password, Nickname, Icon, Sign, Year, Month, Day, Address, Cash, Point, LoginTime, Kind, Friend, Recommender, MaxBuyers, ChatPercent, GamePercent, Evaluation, Visitors, ChargeSum, DischargeSum, ChatSum, GameSum, SendSum, ReceiveSum, RegistTime, Auto, f_URL, phoneNumber ) " +
                                           "Values ('{0}','{1}', N'{2}', '{3}', N'{4}', {5}, {6}, {7}, N'{8}', {9}, {10}, '', {11}, '{12}', '{13}', {14}, {15}, {16}, 0, 0, 0, 0, 0, 0, 0, 0, '{17}', {18}, N'{19}', '{20}')",
                                           userInfo.Id, userInfo.Password, userInfo.Nickname, userInfo.Icon, userInfo.Sign, userInfo.Year, userInfo.Month, userInfo.Day, userInfo.Address, userInfo.Cash, userInfo.Point, userInfo.Kind, userInfo.Friend, userInfo.Recommender, userInfo.MaxBuyers, userInfo.ChatPercent, userInfo.GamePercent, ConvDateToString(DateTime.Now), userInfo.Auto, userInfo.strUrl, userInfo.strPhoneNumber);

            string strQuery1 = string.Format("insert into icuser (UserName, Password) Values ('{0}', '{1}')", userInfo.Id, userInfo.Password);
            UpdateData(strQuery1);

            return UpdateData(strQuery);
        }

        // 2013-12-17: GreenRose
        public bool AddPaymentInfo(PaymentInfo paymentInfo)
        {
            UserInfo userInfo = FindUser(paymentInfo.strID);
            userInfo.Cash = userInfo.Cash - paymentInfo.nPaymentMoney * 100 / 90;

            string strQuery = "";
            if (UpdateUser(userInfo))
            {

                strQuery = string.Format("insert into tblPaymentInfo (f_user_id, f_account_id, f_account_number, f_payment_money) " +
                                                "Values ('{0}', '{1}', '{2}', {3})",
                                                paymentInfo.strID, paymentInfo.strAccountID, paymentInfo.strAccountNumber, paymentInfo.nPaymentMoney);
            }

            return UpdateData(strQuery);
        }

        public bool UpdateUser(UserInfo userInfo)
        {
            string strQuery = "update tblUser set";

            strQuery += string.Format(" Password='{0}',", userInfo.Password);
            strQuery += string.Format(" Nickname=N'{0}',", userInfo.Nickname);
            strQuery += string.Format(" Icon='{0}',", userInfo.Icon);
            strQuery += string.Format(" Sign=N'{0}',", userInfo.Sign);
            strQuery += string.Format(" Year={0},", userInfo.Year);
            strQuery += string.Format(" Month={0},", userInfo.Month);
            strQuery += string.Format(" Day={0},", userInfo.Day);
            strQuery += string.Format(" Address=N'{0}',", userInfo.Address);
            strQuery += string.Format(" Cash={0},", userInfo.Cash);
            strQuery += string.Format(" Point={0},", userInfo.Point);
            strQuery += string.Format(" LoginTime='{0}',", userInfo.LoginTime);
            strQuery += string.Format(" Friend='{0}',", userInfo.Friend);
            strQuery += string.Format(" Recommender='{0}',", userInfo.Recommender);
            strQuery += string.Format(" MaxBuyers={0},", userInfo.MaxBuyers);
            strQuery += string.Format(" ChatPercent={0},", userInfo.ChatPercent);
            strQuery += string.Format(" GamePercent={0},", userInfo.GamePercent);
            strQuery += string.Format(" Evaluation={0},", userInfo.Evaluation);
            strQuery += string.Format(" Visitors={0},", userInfo.Visitors);
            strQuery += string.Format(" ChargeSum={0},", userInfo.ChargeSum);
            strQuery += string.Format(" DischargeSum={0},", userInfo.DischargeSum);
            strQuery += string.Format(" ChatSum={0},", userInfo.ChatSum);
            strQuery += string.Format(" GameSum={0},", userInfo.GameSum);
            strQuery += string.Format(" SendSum={0},", userInfo.SendSum);
            strQuery += string.Format(" ReceiveSum={0},", userInfo.ReceiveSum);
            strQuery += string.Format(" Auto={0},", userInfo.Auto);
            strQuery += string.Format(" AccountNumber='{0}',", userInfo.strAccountNumber);
            strQuery += string.Format(" AccountID=N'{0}',", userInfo.strAccountID);
            strQuery += string.Format(" AccountPassword='{0}',", userInfo.strAccountPass);
            strQuery += string.Format(" f_URL=N'{0}',", userInfo.strUrl);
            strQuery += string.Format(" phoneNumber='{0}', ", userInfo.strPhoneNumber);
            strQuery += string.Format(" f_VIP={0}", userInfo.nVIP);  

            strQuery += string.Format(" where Id='{0}'", userInfo.Id);

            //string strQuery1 = "update icuser set";
            //strQuery1 += string.Format(" Password='{0}'", userInfo.Password);
            //strQuery1 += string.Format(" where UserName='{0}'", userInfo.Id);

            //UpdateData(strQuery1);

            return UpdateData(strQuery);
        }

        // 2014-02-19: GreenRose
        public bool UpdateLoginCount(string strUserID, int nCount)
        {
            string strQuery = string.Empty;
            strQuery = string.Format("update tblUser set f_login_count={0} where Id='{1}'", nCount, strUserID);

            return UpdateData(strQuery);
        }

        public bool DelUser(string userId)
        {
            string strQuery = string.Format("delete from tblUser where Id='{0}'", userId);

            string strQuery1 = string.Format("delete from icuser where UserName='{0}'", userId);
            UpdateData(strQuery1);

            return UpdateData(strQuery);
        }

        public UserInfo FindUser(string Id)
        {
            string queryString = string.Format("select * from tblUser where Id='{0}' or Address='{0}'", Id);

            List<UserInfo> userList = GetUserList(queryString);

            if (userList == null)
                return null;

            if (userList.Count < 1)
            {
                string errorString = string.Format("帐号搜索失败: {0}", Id);
                SetError(ErrorType.Invalid_Id, errorString);

                return null;
            }

            return userList[0];
        }

        // 2013-12-30: GreenRose
        // Find User By URL
        public UserInfo FindUserByUrl(string strUrl)
        {
            string queryString = string.Format("select * from tblUser where f_URL='{0}'", strUrl);

            List<UserInfo> userList = GetUserList(queryString);

            if (userList == null)
                return null;

            if (userList.Count < 1)
            {
                string errorString = string.Format("帐号搜索失败: {0}", strUrl);
                SetError(ErrorType.Invalid_Id, errorString);

                return null;
            }

            return userList[0];
        }

        public UserInfo FindUserByPhoneNumber(string phoneNumber)
        {
            string queryString = string.Format("select * from tblUser where phoneNumber='{0}'", phoneNumber);

            List<UserInfo> userList = GetUserList(queryString);

            if (userList == null)
                return null;

            if (userList.Count < 1)
            {
                string errorString = string.Format("帐号搜索失败: {0}", phoneNumber);
                SetError(ErrorType.Invalid_Id, errorString);

                return null;
            }

            return userList[0];
        }

        public List<PaymentInfo> GetAllPaymentInfos()
        {
            List<PaymentInfo> listPaymentInfo = new List<PaymentInfo>();

            string strQuery = string.Format("select * from tblPaymentInfo");
            listPaymentInfo = GetPaymentList(strQuery);

            return listPaymentInfo;
        }

        public List<UserInfo> GetAllUsers()
        {
            string queryString = string.Format("select * from tblUser");

            return GetUserList(queryString);
        }

        public List<UserInfo> GetBestUsers()
        {
            string queryString = string.Format("select * from tblUser where Kind={0} and Visitors > 0 order by Evaluation/Visitors", (int)UserKind.ServiceWoman);

            return GetUserList(queryString);
        }

        public List<UserInfo> GetRecommenders()
        {
            string queryString = string.Format("select * from tbluser where Kind={0}", (int)UserKind.Recommender);

            return GetUserList(queryString);
        }

        public List<UserInfo> GetRecommenderUsers(string recommenderId)
        {
            string queryString = string.Format("select * from tblUser where Recommender='{0}'", recommenderId);

            return GetUserList(queryString);
        }

        public List<UserInfo> GetFriendUsers(string friendId)
        {
            string queryString = string.Format("select * from tblUser where Friend='{0}'", friendId);

            return GetUserList(queryString);
        }

        public List<UserInfo> GetBuyers(string recommender)
        {
            string queryString = string.Format("select * from tbluser where Recommender='{0}'", recommender);

            return GetUserList(queryString);
        }

        public List<UserInfo> GetServicemans()
        {
            string queryString = string.Format("select * from tbluser where Kind={0}", (int)UserKind.ServiceWoman);

            return GetUserList(queryString);
        }

        public List<UserInfo> GetManager(UserKind userKind)
        {
            string queryString = string.Format("select * from tbluser where Kind={0}", (int)userKind);

            List<UserInfo> userList = GetUserList(queryString);

            return userList;
        }

        public List<UserInfo> GetPartners(UserInfo userInfo)
        {
            List<UserInfo> userList = null;

            switch (userInfo.Kind)
            {
                case (int)UserKind.Buyer:
                case (int)UserKind.ServiceWoman:
                    userList = GetFriendUsers(userInfo.Id);
                    break;

                case (int)UserKind.Recommender:
                    userList = GetRecommenderUsers(userInfo.Id);
                    break;

                case (int)UserKind.ServiceOfficer:
                    userList = GetServicemans();
                    break;

                case (int)UserKind.RecommendOfficer:
                    userList = GetRecommenders();
                    break;

                case (int)UserKind.Manager:
                    userList = GetAllUsers();
                    break;
            }

            return userList;
        }

        public List<UserInfo> GetAutoList()
        {
            string queryString = "select * from tbluser where Auto=1";

            return GetUserList(queryString);
        }

        // 2013-12-17: GreenRose
        public List<PaymentInfo> GetPaymentList(string strQuery)
        {
            DataRowCollection rows = GetData(strQuery);
            if (rows == null)
                return null;

            List<PaymentInfo> listPaymentInfos = new List<PaymentInfo>();
            for (int i = 0; i < rows.Count; i++)
            {
                PaymentInfo paymentInfo = new PaymentInfo();
                paymentInfo.strID = rows[i]["f_user_id"].ToString();
                paymentInfo.strAccountID = rows[i]["f_account_id"].ToString();
                paymentInfo.strAccountNumber = rows[i]["f_account_number"].ToString();
                paymentInfo.nPaymentMoney = ConvToInt(rows[i]["f_payment_money"]);

                listPaymentInfos.Add(paymentInfo);
            }

            return listPaymentInfos;
        }

        public List<UserInfo> GetUserList(string queryString)
        {
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<UserInfo> userInfos = new List<UserInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                UserInfo userInfo = new UserInfo();

                userInfo.Id = rows[i]["Id"].ToString();
                userInfo.Password = rows[i]["Password"].ToString();

                userInfo.Nickname = rows[i]["Nickname"].ToString();
                userInfo.Icon = rows[i]["Icon"].ToString();
                userInfo.Sign = rows[i]["Sign"].ToString();
                userInfo.Year = ConvToInt(rows[i]["Year"]);
                userInfo.Month = ConvToInt(rows[i]["Month"]);
                userInfo.Day = ConvToInt(rows[i]["Day"]);
                userInfo.Address = rows[i]["Address"].ToString();

                userInfo.Point = ConvToInt(rows[i]["Point"]);
                userInfo.Cash = ConvToInt(rows[i]["Cash"].ToString());
                userInfo.RegistTime = Convert.ToDateTime(rows[i]["RegistTime"]);
                userInfo.LoginTime = rows[i]["LoginTime"].ToString();

                userInfo.Kind = ConvToInt(rows[i]["Kind"]);

                userInfo.Friend = rows[i]["Friend"].ToString();
                userInfo.Recommender = rows[i]["Recommender"].ToString();
                userInfo.MaxBuyers = ConvToInt(rows[i]["MaxBuyers"]);
                userInfo.ChatPercent = ConvToInt(rows[i]["ChatPercent"]);
                userInfo.GamePercent = ConvToInt(rows[i]["GamePercent"]);
                userInfo.Evaluation = ConvToInt(rows[i]["Evaluation"]);
                userInfo.Visitors = ConvToInt(rows[i]["Visitors"]);

                userInfo.ChargeSum = ConvToInt(rows[i]["ChargeSum"]);
                userInfo.DischargeSum = ConvToInt(rows[i]["DischargeSum"]);
                userInfo.ChatSum = ConvToInt(rows[i]["ChatSum"]);
                userInfo.GameSum = ConvToInt(rows[i]["GameSum"]);
                userInfo.SendSum = ConvToInt(rows[i]["SendSum"]);
                userInfo.ReceiveSum = ConvToInt(rows[i]["ReceiveSum"]);
                userInfo.Auto = ConvToInt(rows[i]["Auto"]);

                // 2013-12-17: GreenRose
                userInfo.strAccountID = rows[i]["AccountID"].ToString();
                userInfo.strAccountNumber = rows[i]["AccountNumber"].ToString();
                userInfo.strAccountPass = rows[i]["AccountPassword"].ToString();

                // 2013-12-25: GreenRose
                userInfo.strPhoneNumber = rows[i]["phoneNumber"].ToString();

                // 2013-12-30: GreenRose
                userInfo.strUrl = rows[i]["f_URL"].ToString();

                // 2014-02-19: GreenRose
                if (rows[i]["f_login_count"].ToString() != string.Empty)
                    userInfo.nLoginCount = Convert.ToInt32(rows[i]["f_login_count"]);

                // 2014-04-03: GreenRose
                if (rows[i]["f_VIP"].ToString() != string.Empty)
                    userInfo.nVIP = Convert.ToInt32(rows[i]["f_VIP"]);

                userInfos.Add(userInfo);
            }

            return userInfos;
        }

        public int GetAutoCount()
        {
            string queryString = "select Count(*) as AutoCount from tblUser where Auto=1";

            DataRowCollection rows = GetData(queryString);
            if (rows == null || rows.Count != 1)
                return 0;

            return ConvToInt(rows[0]["AutoCount"]);
        }

        public RoomInfo FindRoom(string Id)
        {
            string queryString = string.Format("select * from tblRoom where Id='{0}'", Id);

            List<RoomInfo> roomList = GetRoomList(queryString);

            if (roomList == null)
                return null;

            if (roomList.Count < 1)
            {
                string errorString = string.Format("频道搜索失败: {0}", Id);
                SetError(ErrorType.Invalid_RoomId, errorString);

                return null;
            }

            return roomList[0];
        }

        public List<RoomInfo> FindOwnRooms(string userId)
        {
            string queryString = string.Format("select * from tblRoom where Owner='{0}'", userId);

            return GetRoomList(queryString);
        }

        public List<RoomInfo> GetAllRooms()
        {
            string queryString = string.Format("select * from tblRoom");

            List<RoomInfo> rooms = GetRoomList(queryString);

            if (rooms == null)
                return null;

            for (int i = 0; i < rooms.Count; i++)
            {
                List<UserInfo> roomUsers = Users.GetInstance().GetRoomUsers(rooms[i].Id);
                rooms[i].UserCount = roomUsers.Count;
            }

            return rooms;
        }

        public List<RoomInfo> GetRoomList(string queryString)
        {
            List<RoomInfo> roomInfos = new List<RoomInfo>();

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return roomInfos;

            for (int i = 0; i < rows.Count; i++)
            {
                RoomInfo roomInfo = new RoomInfo();

                roomInfo.Id = rows[i]["Id"].ToString();
                roomInfo.Name = rows[i]["Name"].ToString();
                roomInfo.Kind = ConvToInt(rows[i]["Kind"]);
                roomInfo.Icon = rows[i]["Icon"].ToString();
                roomInfo.Owner = rows[i]["Owner"].ToString();
                roomInfo.Cash = ConvToInt(rows[i]["Cash"]);
                roomInfo.Point = ConvToInt(rows[i]["Point"]);
                roomInfo.MaxUsers = ConvToInt(rows[i]["MaxUsers"]);
                roomInfo.RoomPass = rows[i]["RoomPass"].ToString();

                roomInfos.Add(roomInfo);
            }

            return roomInfos;
        }

        public bool AddRoom(RoomInfo roomInfo)
        {
            RoomInfo findInfo = FindRoom(roomInfo.Id);

            if (findInfo != null)
            {
                SetError(ErrorType.Duplicate_RoomId, "已存在的帐号.");
                return false;
            }

            string strQuery = string.Format("insert into tblRoom (Id, Name, Kind, Owner, Icon, Cash, Point, MaxUsers, RoomPass ) " +
                                           "Values (N'{0}',N'{1}', '{2}', N'{3}', N'{4}', {5}, {6}, '{7}', N'{8}' )",
                                           roomInfo.Id,
                                           roomInfo.Name,
                                           roomInfo.Kind,
                                           roomInfo.Owner,
                                           roomInfo.Icon,
                                           roomInfo.Cash,
                                           roomInfo.Point,
                                           roomInfo.MaxUsers,
                                           roomInfo.RoomPass);

            return UpdateData(strQuery);
        }

        public bool UpdateRoom(RoomInfo roomInfo)
        {
            string strQuery = string.Format("update tblRoom set Name=N'{0}', Kind={1}, Owner=N'{2}', Cash={3}, Point={4}, MaxUsers={5}, RoomPass='{6}' where Id=N'{7}'",
                roomInfo.Name, roomInfo.Kind, roomInfo.Owner, roomInfo.Cash, roomInfo.Point, roomInfo.MaxUsers, roomInfo.RoomPass, roomInfo.Id);

            return UpdateData(strQuery);
        }

        public bool DelRoom(string roomId)
        {
            string strQuery = string.Format("delete from tblRoom where Id='{0}'", roomId);

            return UpdateData(strQuery);
        }

        // 2013-12-17: GreenRose
        public bool DeletePaymentInfos()
        {
            string strQuery = string.Format("delete from tblPaymentInfo");
            return UpdateData(strQuery);
        }

        public bool UpdateGameBank(string gameId, int amount)
        {
            string strQuery = string.Format("update tblGame set Bank={0} where GameId='{1}'", amount, gameId);

            return UpdateData(strQuery);
        }


        public GameInfo FindGame(string Id)
        {
            string queryString = string.Format("select * from tblGame where GameId='{0}'", Id);

            List<GameInfo> gameList = GetGameList(queryString);

            if (gameList == null)
                return null;

            if (gameList.Count < 1)
            {
                string errorString = string.Format("游戏搜索失败: {0}", Id);
                SetError(ErrorType.Invalid_GameId, errorString);

                return null;
            }

            return gameList[0];
        }

        public GameInfo FindGameSource(string gameSource)
        {
            string queryString = string.Format("select * from tblGame where Source='{0}'", gameSource);

            List<GameInfo> gameList = GetGameList(queryString);

            if (gameList == null)
                return null;

            if (gameList.Count < 1)
            {
                string errorString = string.Format("游戏搜索失败: {0}", gameSource);
                SetError(ErrorType.Invalid_GameId, errorString);

                return null;
            }

            return gameList[0];
        }

        public List<GameInfo> GetAllGames()
        {
            string queryString = string.Format("select * from tblGame where Width != 0 and Height != 0");

            return GetGameList(queryString);
        }

        // 2014-01-21: GreenRose
        public List<string> GetDWGameFiles(string strGameID)
        {
            string queryString = string.Format("select * from tblDWGameFileInfo where f_game_id='{0}'", strGameID);

            return GetDWGameFileList(queryString);
        }

        public List<GameInfo> GetGameList(string queryString)
        {
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<GameInfo> gameInfos = new List<GameInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                GameInfo gameInfo = new GameInfo();

                gameInfo.GameId = rows[i]["GameId"].ToString();
                gameInfo.GameName = rows[i]["GameName"].ToString();

                gameInfo.Width = ConvToInt(rows[i]["Width"]);
                gameInfo.Height = ConvToInt(rows[i]["Height"]);

                gameInfo.Icon = rows[i]["Icon"].ToString();
                gameInfo.Source = rows[i]["Source"].ToString();
                gameInfo.Bank = ConvToInt(rows[i]["Bank"]);
                gameInfo.Commission = ConvToInt(rows[i]["Commission"]);

                gameInfo.Downloadfolder = rows[i]["DownloadFolder"].ToString();
                gameInfo.RunFile = rows[i]["RunFile"].ToString();

                gameInfos.Add(gameInfo);
            }

            return gameInfos;
        }

        // 2014-01-21: GreenRose
        public List<string> GetDWGameFileList(string strQuery)
        {
            List<string> listString = new List<string>();

            DataRowCollection rows = GetData(strQuery);
            if (rows == null)
                return null;

            for (int i = 0; i < rows.Count; i++)
            {
                string strFileName = rows[i]["f_game_file"].ToString();
                listString.Add(strFileName);
            }

            return listString;
        }

        public IconInfo AddFace(IconInfo faceInfo)
        {
            string strQry = string.Format("select * from tblIcon where Name='{0}' and Icon=N'{1}' and Type='Face'", faceInfo.Name, faceInfo.Icon);
            DataRowCollection collection = GetData(strQry);
            if (collection != null)
            {
                if (collection.Count > 0)
                    return faceInfo;
            }

            string strQuery = string.Format("insert into tblIcon ( Name, Icon, Point, Type ) " +
                                           "Values ('{0}', N'{1}', 0, 'Face' )",
                                           faceInfo.Name, faceInfo.Icon);

            if (UpdateData(strQuery) == false)
                return null;

            strQuery = string.Format("select MAX(Id) as maxId from tblIcon where Name='{0}'", faceInfo.Name);

            DataRowCollection rows = GetData(strQuery);

            if (rows == null || rows.Count != 1)
                return null;

            faceInfo.Id = rows[0]["maxId"].ToString();

            return faceInfo;

        }

        public IconInfo AddVideo(IconInfo faceInfo)
        {
            string strQuery = string.Format("select MAX(Id) as maxId from tblIcon where Type='Videos' and Icon='{0}'", faceInfo.Icon);
            DataRowCollection rows = GetData(strQuery);
            if (rows[0]["maxId"].ToString() == "")
            {

                strQuery = string.Format("insert into tblIcon ( Name, Icon, Point, Type ) " +
                                               "Values ('{0}', '{1}', 0, 'Videos' )",
                                               faceInfo.Name, faceInfo.Icon);

                if (UpdateData(strQuery) == false)
                    return null;

            }

            if (rows == null || rows.Count != 1)
                return null;

            faceInfo.Id = rows[0]["maxId"].ToString();

            return faceInfo;

        }

        public bool DelFace(string iconId)
        {
            string strQuery = string.Format("delete from tblIcon where Id={0}", iconId);

            return UpdateData(strQuery);
        }

        public List<IconInfo> GetAllEmoticons()
        {
            string queryString = string.Format("select * from tblIcon where Type='Emoticon'");

            return GetIconList(queryString);
        }

        public List<IconInfo> GetAllPresents()
        {
            string queryString = string.Format("select * from tblIcon where Type='Present'");

            return GetIconList(queryString);
        }

        public List<IconInfo> GetAllFaces(string name)
        {
            string queryString = string.Format("select * from tblIcon where (Type='Face' or Type='Videos') and Name='{0}'", name);

            return GetIconList(queryString);
        }



        private List<IconInfo> GetIconList(string queryString)
        {
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<IconInfo> presentInfos = new List<IconInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                IconInfo presentInfo = new IconInfo();

                presentInfo.Id = rows[i]["Id"].ToString();
                presentInfo.Name = rows[i]["Name"].ToString();
                presentInfo.Icon = rows[i]["Icon"].ToString();
                presentInfo.Point = ConvToInt(rows[i]["Point"]);

                presentInfos.Add(presentInfo);
            }

            return presentInfos;
        }



        public string GetNewIDList(UserInfo userInfo)
        {
            List<UserInfo> partners = GetPartners(userInfo);

            int newCount = 1;

            if (userInfo.Kind == (int)UserKind.Recommender)
            {
                newCount = userInfo.MaxBuyers - partners.Count;
            }
            else if (userInfo.Kind == (int)UserKind.Buyer || userInfo.Kind == (int)UserKind.ServiceWoman)
            {
                int todayCount = 0;
                DateTime today = DateTime.Now;

                for (int i = 0; i < partners.Count; i++)
                {
                    UserInfo partnerInfo = partners[i];

                    if (today.Year == partnerInfo.RegistTime.Year &&
                        today.Month == partnerInfo.RegistTime.Month &&
                        today.Day == partnerInfo.RegistTime.Day)
                    {
                        todayCount++;
                    }
                }

                EnvInfo envInfo = GetEnviroment();
                newCount = envInfo.NewCount - todayCount;
            }


            if (newCount <= 0)
            {
                SetError(ErrorType.Notenough_NewID, "没有申请帐号的次数.");
                return null;
            }

            Random random = new Random();
            string newId = null;

            while (true)
            {
                int randomNum = random.Next(1111, 9999);

                newId = string.Format("{0}{1}", userInfo.Id.Substring(0, 2), randomNum);

                if (FindUser(newId) == null)
                    break;
            }

            return newId;
        }

        public int AddChatHistory(ChatHistoryInfo chatHistoryInfo)
        {
            string strQuery = string.Format("insert into tblChatHistory ( RoomId, BuyerId, ServicemanId, ServicePrice, StartTime, EndTime, BuyerTotal, ServicemanTotal, ServiceOfficerTotal, ManagerTotal ) " +
                                           "Values ( '{0}', '{1}', '{2}', {3}, '{4}', '{5}', 0, 0, 0, 0 )",
                                           chatHistoryInfo.RoomId,
                                           chatHistoryInfo.BuyerId,
                                           chatHistoryInfo.ServicemanId,
                                           chatHistoryInfo.ServicePrice,
                                           ConvDateToLongString(chatHistoryInfo.StartTime),
                                           ConvDateToLongString(chatHistoryInfo.EndTime)
                                           );

            if (UpdateData(strQuery) == false)
                return 0;

            strQuery = string.Format("select MAX(Id) as maxId from tblChatHistory where BuyerId='{0}' and ServicemanId='{1}'",
                chatHistoryInfo.BuyerId, chatHistoryInfo.ServicemanId);

            DataRowCollection rows = GetData(strQuery);

            if (rows == null || rows.Count != 1)
                return 0;

            int id = 0;

            try
            {
                id = ConvToInt(rows[0][0]);
            }
            catch
            {
                id = 0;
            }

            return id;
        }

        public bool UpdateChatHistory(ChatHistoryInfo chatHistoryInfo)
        {
            string strQuery = string.Format("update tblChatHistory set BuyerTotal={0}, ServicemanTotal={1}, ServiceOfficerTotal={2}, ManagerTotal={3}, EndTime='{4}', OfficerId='{5}', ManagerId='{6}' where Id={7}",
                                           chatHistoryInfo.BuyerTotal,
                                           chatHistoryInfo.ServicemanTotal,
                                           chatHistoryInfo.ServiceOfficerTotal,
                                           chatHistoryInfo.ManagerTotal,
                                           ConvDateToLongString(chatHistoryInfo.EndTime),
                                           chatHistoryInfo.OfficerId,
                                           chatHistoryInfo.ManagerId,
                                           chatHistoryInfo.Id
                                           );

            return UpdateData(strQuery);
        }

        public int SumChatHistory(UserInfo userInfo, DateTime curDate)
        {
            int cash = 0;
            int sumIndex = 0;

            string queryString = string.Format("select sum(BuyerTotal), sum(ServicemanTotal), sum(ServiceOfficerTotal), sum(ManagerTotal) from tblChatHistory where YEAR(StartTime)={0} and MONTH(StartTime) = {1} and DAY(StartTime) = {2}",
                    curDate.Year, curDate.Month, curDate.Day);

            if (userInfo != null)
            {
                switch (userInfo.Kind)
                {
                    case (int)UserKind.Buyer:
                    case (int)UserKind.Recommender:
                    case (int)UserKind.RecommendOfficer:
                        {
                            queryString += string.Format(" and BuyerId='{0}'", userInfo.Id);
                            sumIndex = 0;
                        }
                        break;

                    case (int)UserKind.ServiceWoman:
                        {
                            queryString += string.Format(" and ServicemanId='{0}'", userInfo.Id);
                            sumIndex = 1;
                        }
                        break;

                    case (int)UserKind.ServiceOfficer:
                        {
                            sumIndex = 2;
                        }
                        break;

                    case (int)UserKind.Manager:
                        {
                            sumIndex = 3;
                        }
                        break;
                }
            }

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return 0;

            try
            {
                cash = ConvToInt(rows[0][sumIndex]);
            }
            catch
            {
                cash = 0;
            }

            return cash;
        }

        public int SumChatHistory(string roomId, DateTime curDate)
        {
            int cash = 0;

            string queryString = string.Format("select sum(BuyerTotal) from tblChatHistory where YEAR(StartTime)={0} and MONTH(StartTime) = {1} and DAY(StartTime) = {2} ",
                    curDate.Year, curDate.Month, curDate.Day);

            if (string.IsNullOrEmpty(roomId) == false)
                queryString += string.Format("and RoomId = '{0}'", roomId);

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return 0;

            try
            {
                cash = ConvToInt(rows[0][0]);
            }
            catch
            {
                cash = 0;
            }

            return cash;
        }

        public ChatHistoryInfo FindChatHistory(int Id)
        {
            string queryString = string.Format("select * from tblChatHistory where Id = '{0}'", Id);

            List<ChatHistoryInfo> chatHistoryList = GetChatHistoryList(queryString);

            if (chatHistoryList == null || chatHistoryList.Count != 1)
                return null;

            return chatHistoryList[0];
        }

        public List<ChatHistoryInfo> GetAllChatHistories(string userId)
        {
            string queryString = string.Format("select * from tblChatHistory");

            if (userId != null)
            {
                UserInfo userInfo = FindUser(userId);

                if (userInfo != null)
                {
                    switch (userInfo.Kind)
                    {
                        case (int)UserKind.Buyer:
                        case (int)UserKind.Recommender:
                        case (int)UserKind.RecommendOfficer:
                            queryString += string.Format(" where BuyerId='{0}'", userId);
                            break;

                        case (int)UserKind.ServiceWoman:
                            queryString += string.Format(" where ServicemanId='{0}'", userId);
                            break;

                        case (int)UserKind.ServiceOfficer:
                            queryString += string.Format(" where OfficerId='{0}'", userId);
                            break;

                        case (int)UserKind.Manager:
                            queryString += string.Format(" where ManagerId='{0}'", userId);
                            break;
                    }
                }
            }

            return GetChatHistoryList(queryString);
        }

        public List<ChatHistoryInfo> GetDayChatHistories(int year, int month, int day)
        {
            string queryString = string.Format("select * from tblChatHistory where 1=1 ");

            if (year != 0)
                queryString += string.Format(" and YEAR(StartTime)='{0}'", year);

            if (month != 0)
                queryString += string.Format(" and MONTH(StartTime)='{0}'", month);

            if (day != 0)
                queryString += string.Format(" and DAY(StartTime)='{0}'", day);

            return GetChatHistoryList(queryString);
        }

        private List<ChatHistoryInfo> GetChatHistoryList(string queryString)
        {
            queryString += " order by StartTime desc";

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<ChatHistoryInfo> chatHistoryList = new List<ChatHistoryInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                ChatHistoryInfo chatHistoryInfo = new ChatHistoryInfo();

                chatHistoryInfo.Id = ConvToInt(rows[i]["Id"]);
                chatHistoryInfo.RoomId = ConvToString(rows[i]["RoomId"]);
                chatHistoryInfo.BuyerId = ConvToString(rows[i]["BuyerId"]);
                chatHistoryInfo.ServicemanId = ConvToString(rows[i]["ServicemanId"]);
                chatHistoryInfo.OfficerId = ConvToString(rows[i]["OfficerId"]);
                chatHistoryInfo.ManagerId = ConvToString(rows[i]["ManagerId"]);
                chatHistoryInfo.ServicePrice = ConvToInt(rows[i]["ServicePrice"]);
                chatHistoryInfo.StartTime = Convert.ToDateTime(rows[i]["StartTime"]);
                chatHistoryInfo.EndTime = Convert.ToDateTime(rows[i]["EndTime"]);
                chatHistoryInfo.BuyerTotal = ConvToInt(rows[i]["BuyerTotal"]);
                chatHistoryInfo.ServicemanTotal = ConvToInt(rows[i]["ServicemanTotal"]);
                chatHistoryInfo.ServiceOfficerTotal = ConvToInt(rows[i]["ServiceOfficerTotal"]);
                chatHistoryInfo.ManagerTotal = ConvToInt(rows[i]["ManagerTotal"]);

                chatHistoryList.Add(chatHistoryInfo);
            }

            return chatHistoryList;
        }

        public bool AddChargeHistory(ChargeHistoryInfo chargeHistoryInfo)
        {
            string strQuery = string.Format("insert into tblChargeHistory ( Kind, Cash, StartTime, EndTime, ApproveId, OwnId, Complete, BankAccount ) " +
                                           "Values ( {0}, {1}, '{2}', '{3}', '{4}', '{5}', {6}, N'{7}' )",
                                           chargeHistoryInfo.Kind,
                                           chargeHistoryInfo.Cash,
                                           ConvDateToString(chargeHistoryInfo.StartTime),
                                           ConvDateToString(chargeHistoryInfo.EndTime),
                                           chargeHistoryInfo.ApproveId,
                                           chargeHistoryInfo.OwnId,
                                           chargeHistoryInfo.Complete,
                                           chargeHistoryInfo.BankAccount
                                           );

            return UpdateData(strQuery);
        }

        public bool UpdateChargeHistory(ChargeHistoryInfo chargeHistoryInfo)
        {
            string strQuery = string.Format("update tblChargeHistory set EndTime='{0}', ApproveId='{1}', Complete={2} where Id={3}",
                                           ConvDateToString(chargeHistoryInfo.EndTime),
                                           chargeHistoryInfo.ApproveId,
                                           chargeHistoryInfo.Complete,
                                           chargeHistoryInfo.Id
                                           );

            return UpdateData(strQuery);
        }

        public bool DeleteChargeHistory(int chargeId)
        {
            string strQuery = string.Format("delete from tblChargeHistory where Id='{0}'", chargeId);

            return UpdateData(strQuery);
        }

        public int SumChargeHistory(string OwnId, DateTime curDate, int kind)
        {
            int cash = 0;

            string queryString = string.Format("select sum(Cash) as SumVal from tblChargeHistory where YEAR(StartTime)={0} and MONTH(StartTime) = {1} and DAY(StartTime) = {2}",
                    curDate.Year, curDate.Month, curDate.Day);

            if (OwnId != null)
            {
                queryString += string.Format(" and OwnId = '{0}' ", OwnId);
            }

            if (kind >= 0)
            {
                queryString += string.Format(" and Kind = {0}", kind);
            }

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return 0;

            try
            {
                cash = ConvToInt(rows[0][0]);
            }
            catch
            {
                cash = 0;
            }

            return cash;
        }

        public ChargeHistoryInfo FindChargeHistory(int Id)
        {
            string queryString = string.Format("select * from tblChargeHistory where Id = '{0}'", Id);

            List<ChargeHistoryInfo> chargeHistoryList = GetChargeHistoryList(queryString);

            if (chargeHistoryList == null || chargeHistoryList.Count != 1)
                return null;

            return chargeHistoryList[0];
        }

        public List<ChargeHistoryInfo> GetAllChargeList(string ownId)
        {
            string queryString = "select * from tblChargeHistory";

            if (ownId != null)
                queryString += string.Format(" where OwnId='{0}'", ownId);

            return GetChargeHistoryList(queryString);
        }

        public List<ChargeHistoryInfo> GetChargeDayList(int kind, int complete, int year, int month, int day)
        {
            string queryString = string.Format("select * from tblChargeHistory where 1=1 ");

            //if (kind == (int)ChargeKind.Charge)
            //    queryString += string.Format(" Cash > 0 ");
            //else
            //    queryString += string.Format(" Cash < 0 ");

            //queryString += string.Format(" Complete={0}", complete);

            if (year != 0)
                queryString += string.Format(" and YEAR(EndTime)='{0}'", year);

            if (month != 0)
                queryString += string.Format(" and MONTH(EndTime)='{0}'", month);

            if (day != 0)
                queryString += string.Format(" and DAY(EndTime)='{0}'", day);

            return GetChargeHistoryList(queryString);
        }

        public List<ChargeHistoryInfo> GetChargeRequestList()
        {
            string queryString = "select * from tblChargeHistory where Complete=0";

            return GetChargeHistoryList(queryString);
        }

        private List<ChargeHistoryInfo> GetChargeHistoryList(string queryString)
        {
            queryString += " order by EndTime desc";

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<ChargeHistoryInfo> chargeHistoryList = new List<ChargeHistoryInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                ChargeHistoryInfo chargeHistoryInfo = new ChargeHistoryInfo();

                chargeHistoryInfo.Id = ConvToInt(rows[i]["Id"]);
                chargeHistoryInfo.OwnId = rows[i]["OwnId"].ToString();
                chargeHistoryInfo.ApproveId = rows[i]["ApproveId"].ToString();
                chargeHistoryInfo.Kind = ConvToInt(rows[i]["Kind"]);
                chargeHistoryInfo.Cash = ConvToInt(rows[i]["Cash"]);
                chargeHistoryInfo.StartTime = Convert.ToDateTime(rows[i]["StartTime"]);
                chargeHistoryInfo.EndTime = Convert.ToDateTime(rows[i]["EndTime"]);
                chargeHistoryInfo.Complete = ConvToInt(rows[i]["Complete"]);
                chargeHistoryInfo.BankAccount = rows[i]["BankAccount"].ToString();

                chargeHistoryList.Add(chargeHistoryInfo);
            }

            return chargeHistoryList;
        }

        public bool AddEvalHistory(EvalHistoryInfo evalInfo)
        {
            EvalHistoryInfo existInfo = GetEvalHistory(evalInfo.OwnId, evalInfo.BuyerId);

            string queryString = "";

            if (existInfo == null)
            {
                queryString = string.Format("insert into tblEvalHistory ( OwnId, BuyerId, Value, EvalTime ) " +
                                               "Values ('{0}','{1}', {2}, '{3}' )",
                                               evalInfo.OwnId, evalInfo.BuyerId, evalInfo.Value, ConvDateToString(DateTime.Now));
            }
            else
            {
                queryString = string.Format("update tblEvalHistory set Value={0}, EvalTime='{1}'  where Id='{2}'",
                    evalInfo.Value, ConvDateToString(DateTime.Now), existInfo.Id);

            }

            return UpdateData(queryString);
        }

        public List<EvalHistoryInfo> GetAllEvalHistoryList(string servicemanId)
        {
            string queryString = string.Format("select * from tblEvalHistory where OwnId='{0}'", servicemanId);

            return GetEvalHistoryList(queryString);
        }

        public EvalHistoryInfo GetEvalHistory(string servicemanId, string buyerId)
        {
            string queryString = string.Format("select * from tblEvalHistory where OwnId='{0}' and BuyerId='{1}'", servicemanId, buyerId);

            List<EvalHistoryInfo> evalList = GetEvalHistoryList(queryString);

            if (evalList == null || evalList.Count != 1)
                return null;

            return evalList[0];
        }

        private List<EvalHistoryInfo> GetEvalHistoryList(string queryString)
        {
            queryString += " order by EvalTime desc";

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<EvalHistoryInfo> evalHistoryList = new List<EvalHistoryInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                EvalHistoryInfo historyInfo = new EvalHistoryInfo();

                historyInfo.Id = ConvToInt(rows[i]["Id"]);
                historyInfo.OwnId = rows[i]["OwnId"].ToString();
                historyInfo.BuyerId = rows[i]["BuyerId"].ToString();
                historyInfo.Value = ConvToInt(rows[i]["Value"].ToString());
                historyInfo.EvalTime = Convert.ToDateTime(rows[i]["EvalTime"].ToString());

                evalHistoryList.Add(historyInfo);
            }

            return evalHistoryList;
        }

        public bool AddBoard(BoardInfo boardInfo)
        {
            string strQuery = string.Format("insert into tblBoard ( Kind, Title, ContentString, Source, WriteTime, UserId, UserKind, SendId ) " +
                                           "Values ( {0}, N'{1}',N'{2}', '{3}', '{4}', '{5}', '{6}', '{7}' )",
                                           boardInfo.Kind, boardInfo.Title, boardInfo.Content, boardInfo.Source, DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), boardInfo.UserId, boardInfo.UserKind, boardInfo.SendId
                                           );
            return UpdateData(strQuery);
        }

        public bool UpdateBoard(BoardInfo boardInfo)
        {
            string strQuery = string.Format("update tblBoard set Readed = {0}, ReadTime='{1}' where Id={2} ",
                                           boardInfo.Readed, DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), boardInfo.Id
                                           );

            return UpdateData(strQuery);
        }

        public bool DelBoard(int boardId)
        {
            string strQuery = string.Format("delete from tblBoard where Id={0}", boardId);

            return UpdateData(strQuery);
        }

        public List<BoardInfo> GetLetters(string userId)
        {
            string queryString = string.Format("select * from tblBoard where Kind='{0}'", (int)BoardKind.Letter);

            if (userId != null)
                queryString += string.Format(" and UserId='{0}'", userId);

            queryString += " order by Id desc ";

            return GetBoardList(queryString);
        }

        public List<BoardInfo> GetNotices(string userId)
        {
            string queryString = string.Format("select * from tblBoard where Kind='{0}'", (int)BoardKind.Notice);

            if (userId != null)
                queryString += string.Format(" and UserId='{0}' or SendId = '{1}'", userId, userId);

            queryString += " order by Id desc";

            return GetBoardList(queryString);
        }

        private List<BoardInfo> GetBoardList(string queryString)
        {
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<BoardInfo> boardList = new List<BoardInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                BoardInfo boardInfo = new BoardInfo();

                boardInfo.Id = ConvToInt(rows[i]["Id"]);
                boardInfo.Kind = ConvToInt(rows[i]["Kind"]);
                boardInfo.UserId = ConvToString(rows[i]["UserId"]);
                boardInfo.UserKind = ConvToInt(rows[i]["UserKind"]);
                boardInfo.Title = rows[i]["Title"].ToString();
                boardInfo.Content = rows[i]["ContentString"].ToString();
                boardInfo.Source = rows[i]["Source"].ToString();
                boardInfo.WriteTime = Convert.ToDateTime(rows[i]["WriteTime"]);
                boardInfo.Readed = ConvToInt(rows[i]["Readed"]);
                boardInfo.SendId = rows[i]["SendId"].ToString();

                boardList.Add(boardInfo);
            }

            return boardList;
        }

        public BoardInfo GetAdminNotice()
        {
            string queryString = string.Format("select top 1 * from tblBoard where Kind='{0}' ORDER BY WriteTime DESC ", (int)BoardKind.AdminNotice);

            return GetAdminNotice(queryString);
        }

        private BoardInfo GetAdminNotice(string queryString)
        {
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            BoardInfo boardInfo = new BoardInfo();

            for (int i = 0; i < rows.Count; i++)
            {
                boardInfo.Id = ConvToInt(rows[i]["Id"]);
                boardInfo.Kind = ConvToInt(rows[i]["Kind"]);
                boardInfo.UserId = ConvToString(rows[i]["UserId"]);
                boardInfo.UserKind = ConvToInt(rows[i]["UserKind"]);
                boardInfo.Title = rows[i]["Title"].ToString();
                boardInfo.Content = rows[i]["ContentString"].ToString();
                boardInfo.Source = rows[i]["Source"].ToString();
                boardInfo.WriteTime = Convert.ToDateTime(rows[i]["WriteTime"]);
                boardInfo.Readed = ConvToInt(rows[i]["Readed"]);
                boardInfo.SendId = rows[i]["SendId"].ToString();
            }

            return boardInfo;
        }

        public int AddPointHistory(PointHistoryInfo pointInfo)
        {
            string queryString = string.Format("insert into tblPointHistory ( TargetId, Point, AgreeTime, ContentString, Kind ) " +
                                               "Values ('{0}',{1}, '{2}', N'{3}', {4} )",
                                               pointInfo.TargetId, pointInfo.Point, ConvDateToString(DateTime.Now), pointInfo.Content, pointInfo.Kind);

            if (UpdateData(queryString) == false)
                return 0;

            string strQuery = string.Format("select MAX(Id) as maxId from tblPointHistory where TargetId='{0}' and Point={1}",
                pointInfo.TargetId, pointInfo.Point);

            DataRowCollection rows = GetData(strQuery);

            if (rows == null || rows.Count != 1)
                return 0;

            int id = 0;

            try
            {
                id = ConvToInt(rows[0][0]);
            }
            catch
            {
                id = 0;
            }

            return id;
        }

        public bool UpdatePointHistory(PointHistoryInfo pointHistoryInfo)
        {
            string strQuery = string.Format("update tblPointHistory set Point={0}, AgreeTime='{1}' where Id={2}",
                                           pointHistoryInfo.Point,
                                           ConvDateToString(pointHistoryInfo.AgreeTime),
                                           pointHistoryInfo.Id
                                           );

            return UpdateData(strQuery);
        }

        public PointHistoryInfo FindPointHistory(int Id)
        {
            string queryString = string.Format("select * from tblPointHistory where Id = '{0}'", Id);

            List<PointHistoryInfo> pointHistoryList = GetPointHistoryList(queryString);

            if (pointHistoryList == null || pointHistoryList.Count != 1)
                return null;

            return pointHistoryList[0];
        }

        public List<PointHistoryInfo> GetAllPointHistoryList(string userId)
        {
            string queryString = string.Format("select * from tblPointHistory where TargetId='{0}'", userId);

            return GetPointHistoryList(queryString);
        }

        public List<PointHistoryInfo> GetDayPointHistoryList(int kind, int year, int month, int day)
        {
            string queryString = "select * from tblPointHistory where 1=1  ";

            if (kind >= 0)
                queryString += string.Format(" and Kind={0}", kind);

            if (year != 0)
                queryString += string.Format(" and YEAR(AgreeTime)='{0}'", year);

            if (month != 0)
                queryString += string.Format(" and MONTH(AgreeTime)='{0}'", month);

            if (day != 0)
                queryString += string.Format(" and DAY(AgreeTime)='{0}'", day);

            return GetPointHistoryList(queryString);
        }

        private List<PointHistoryInfo> GetPointHistoryList(string queryString)
        {
            queryString += " order by AgreeTime desc";

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<PointHistoryInfo> pointHistoryList = new List<PointHistoryInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                PointHistoryInfo historyInfo = new PointHistoryInfo();

                historyInfo.Id = ConvToInt(rows[i]["Id"]);
                historyInfo.TargetId = rows[i]["TargetId"].ToString();
                historyInfo.Point = ConvToInt(rows[i]["Point"].ToString());
                historyInfo.AgreeTime = Convert.ToDateTime(rows[i]["AgreeTime"].ToString());
                historyInfo.Content = rows[i]["ContentString"].ToString();
                historyInfo.Kind = ConvToInt(rows[i]["Kind"]);

                pointHistoryList.Add(historyInfo);
            }

            return pointHistoryList;
        }

        public EnvInfo GetEnviroment()
        {
            string strQuery = "select * from tblEnviroment";

            DataRowCollection rows = GetData(strQuery);
            if (rows == null || rows.Count != 1)
                return null;

            EnvInfo envInfo = new EnvInfo();

            envInfo.NewCount = ConvToInt(rows[0]["NewCount"]);
            envInfo.LoginBonusPoint = ConvToInt(rows[0]["LoginBonusPoint"]);
            envInfo.ChargeBonusPoint = ConvToInt(rows[0]["ChargeBonusPoint"]);
            envInfo.ImageUploadPath = rows[0]["ImageUploadPath"].ToString();
            envInfo.ChargeSiteUrl = rows[0]["ChargeSiteUrl"].ToString();
            envInfo.ChargeGivePercent = ConvToInt(rows[0]["ChargeGivePercent"]);
            envInfo.EveryDayPoint = ConvToInt(rows[0]["EveryDayPoint"]);
            envInfo.CashRate = ConvToInt(rows[0]["CashRate"]);

            return envInfo;
        }

        public bool SetEnviroment(EnvInfo envInfo)
        {
            string strQuery = "update tblEnviroment set ";

            strQuery += string.Format(" NewCount={0}, ", envInfo.NewCount);
            strQuery += string.Format(" LoginBonusPoint={0}, ", envInfo.LoginBonusPoint);
            strQuery += string.Format(" ChargeBonusPoint={0}, ", envInfo.ChargeBonusPoint);
            strQuery += string.Format(" ChargeSiteUrl='{0}', ", envInfo.ChargeSiteUrl);
            strQuery += string.Format(" ImageUploadPath='{0}', ", envInfo.ImageUploadPath);

            strQuery += string.Format(" ChargeGivePercent={0}, ", envInfo.ChargeGivePercent);
            strQuery += string.Format(" EveryDayPoint={0}, ", envInfo.EveryDayPoint);
            strQuery += string.Format(" CashRate={0}", envInfo.CashRate);

            return UpdateData(strQuery);
        }

        public bool AddPresentHistory(PresentHistoryInfo presentInfo)
        {
            string strQuery = string.Format("insert into tblPresentHistory (SendId, ReceiveId, Cash, Description, SendTime ) " +
                                           "Values ('{0}','{1}', {2}, '{3}', '{4}' )",
                                           presentInfo.SendId,
                                           presentInfo.ReceiveId,
                                           presentInfo.Cash,
                                           presentInfo.Descripiton,
                                           ConvDateToString(presentInfo.SendTime)
                                           );

            return UpdateData(strQuery);
        }

        public List<PresentHistoryInfo> GetAllPresentHistories(string userId)
        {
            string strQuery = string.Format("select * from tblPresentHistory where SendId= '{0}' or ReceiveId='{1}'", userId, userId);

            return GetPresentHistoryList(strQuery);
        }

        private List<PresentHistoryInfo> GetPresentHistoryList(string queryString)
        {
            queryString += " order by SendTime desc";

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<PresentHistoryInfo> pointHistoryList = new List<PresentHistoryInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                PresentHistoryInfo historyInfo = new PresentHistoryInfo();

                historyInfo.SendId = rows[i]["SendId"].ToString();
                historyInfo.ReceiveId = rows[i]["ReceiveId"].ToString();
                historyInfo.Cash = ConvToInt(rows[i]["Cash"].ToString());
                historyInfo.Descripiton = rows[i]["Description"].ToString();
                historyInfo.SendTime = Convert.ToDateTime(rows[i]["SendTime"].ToString());

                pointHistoryList.Add(historyInfo);
            }

            return pointHistoryList;
        }

        public int AddGameHistory(GameHistoryInfo gameHistoryInfo)
        {
            string strQuery = string.Format("insert into tblGameHistory ( GameId, GameSource, BuyerId, RecommenderId, StartTime, EndTime, BuyerTotal, RecommenderTotal, RecommendOfficerTotal, ManagerTotal, AdminTotal ) " +
                                           "Values (N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}', 0, 0, 0, 0, 0 )",
                                           gameHistoryInfo.GameId,
                                           gameHistoryInfo.GameSource,
                                           gameHistoryInfo.BuyerId,
                                           gameHistoryInfo.RecommenderId,
                                           ConvDateToLongString(gameHistoryInfo.StartTime),
                                           ConvDateToLongString(gameHistoryInfo.EndTime),
                                           gameHistoryInfo.AdminTotal
                                           );

            if (UpdateData(strQuery) == false)
                return 0;

            strQuery = string.Format("select MAX(Id) as maxId from tblGameHistory where BuyerId='{0}' and GameId='{1}'", gameHistoryInfo.BuyerId, gameHistoryInfo.GameId);

            DataRowCollection rows = GetData(strQuery);

            if (rows == null || rows.Count != 1)
                return 0;

            int id = 0;

            try
            {
                id = ConvToInt(rows[0][0]);
            }
            catch
            {
                id = 0;
            }

            return id;
        }

        public bool UpdateGameHistory(GameHistoryInfo gameHistoryInfo)
        {
            string strQuery = string.Format("update tblGameHistory set BuyerTotal={0}, RecommenderTotal={1}, RecommendOfficerTotal={2}, ManagerTotal={3}, EndTime='{4}', OfficerId='{5}', ManagerId=N'{6}', AdminTotal={7}  where Id={8}",
                                           gameHistoryInfo.BuyerTotal,
                                           gameHistoryInfo.RecommenderTotal,
                                           gameHistoryInfo.RecommendOfficerTotal,
                                           gameHistoryInfo.ManagerTotal,
                                           ConvDateToLongString(gameHistoryInfo.EndTime),
                                           gameHistoryInfo.OfficerId,
                                           gameHistoryInfo.ManagerId,
                                           gameHistoryInfo.AdminTotal,
                                           gameHistoryInfo.Id
                                           );

            return UpdateData(strQuery);
        }

        public bool DeleteGameHistory(int strHistoryId)
        {
            string strQuery = string.Format("delete from tblGameHistory where Id={0}", strHistoryId);

            return UpdateData(strQuery);
        }


        public GameHistoryInfo FindGameHistory(int id)
        {
            string queryString = string.Format("select * from tblGameHistory where Id = '{0}'", id);

            List<GameHistoryInfo> gameHistoryList = GetGameHistoryList(queryString);

            if (gameHistoryList == null || gameHistoryList.Count != 1)
                return null;

            return gameHistoryList[0];
        }

        public List<GameHistoryInfo> GetAllGameHistories(string userId, int year, int month, int day)
        {
            string strQuery = string.Format("select * from tblGameHistory where BuyerId='{0}' ", userId);

            if (year != 0)
                strQuery += string.Format(" and YEAR(EndTime)='{0}'", year);

            if (month != 0)
                strQuery += string.Format(" and MONTH(EndTime)='{0}'", month);

            if (day != 0)
                strQuery += string.Format(" and DAY(EndTime)='{0}'", day);

            return GetGameHistoryList(strQuery);
        }

        public List<GameHistoryInfo> GetDayGameHistories(int year, int month, int day)
        {
            string queryString = string.Format("select * from tblGameHistory where 1=1 ");

            if (year != 0)
                queryString += string.Format(" and YEAR(EndTime)='{0}'", year);

            if (month != 0)
                queryString += string.Format(" and MONTH(EndTime)='{0}'", month);

            if (day != 0)
                queryString += string.Format(" and DAY(EndTime)='{0}'", day);

            return GetGameHistoryList(queryString);
        }

        private List<GameHistoryInfo> GetGameHistoryList(string queryString)
        {
            queryString += " order by EndTime desc";

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<GameHistoryInfo> gameHistoryList = new List<GameHistoryInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                GameHistoryInfo historyInfo = new GameHistoryInfo();

                historyInfo.Id = ConvToInt(rows[i]["Id"]);
                historyInfo.GameId = rows[i]["GameId"].ToString();
                historyInfo.GameSource = rows[i]["GameSource"].ToString();
                historyInfo.BuyerId = ConvToString(rows[i]["BuyerId"]);
                historyInfo.RecommenderId = ConvToString(rows[i]["RecommenderId"]);
                historyInfo.OfficerId = ConvToString(rows[i]["OfficerId"]);
                historyInfo.ManagerId = ConvToString(rows[i]["ManagerId"]);
                historyInfo.StartTime = Convert.ToDateTime(rows[i]["StartTime"]);
                historyInfo.EndTime = Convert.ToDateTime(rows[i]["EndTime"]);
                historyInfo.BuyerTotal = ConvToInt(rows[i]["BuyerTotal"]);
                historyInfo.RecommenderTotal = ConvToInt(rows[i]["RecommenderTotal"]);
                historyInfo.RecommendOfficerTotal = ConvToInt(rows[i]["RecommendOfficerTotal"]);
                historyInfo.ManagerTotal = ConvToInt(rows[i]["ManagerTotal"]);
                historyInfo.AdminTotal = ConvToInt(rows[i]["AdminTotal"]);

                gameHistoryList.Add(historyInfo);
            }

            return gameHistoryList;
        }

        public int SumGameHistory(UserInfo userInfo, DateTime curDate)
        {
            if (userInfo == null)
                return SumGameHistory(userInfo, (int)UserKind.Buyer, curDate);

            int cash = SumGameHistory(userInfo, userInfo.Kind, curDate);

            if (userInfo.Kind == (int)UserKind.Recommender ||
                userInfo.Kind == (int)UserKind.RecommendOfficer)
            {
                cash += SumGameHistory(userInfo, (int)UserKind.Buyer, curDate);
            }

            return cash;
        }

        private int SumGameHistory(UserInfo userInfo, int kind, DateTime curDate)
        {
            int cash = 0;
            int sumIndex = 0;

            string queryString = string.Format("select sum(BuyerTotal), sum(RecommenderTotal), sum(RecommendOfficerTotal), sum(ManagerTotal) from tblGameHistory where YEAR(StartTime)={0} and MONTH(StartTime) = {1} and DAY(StartTime) = {2} ",
                    curDate.Year, curDate.Month, curDate.Day);

            if (userInfo != null)
            {
                switch (kind)
                {
                    case (int)UserKind.Buyer:
                    case (int)UserKind.ServiceWoman:
                    case (int)UserKind.ServiceOfficer:
                        {
                            queryString += string.Format(" and BuyerId='{0}'", userInfo.Id);
                            sumIndex = 0;
                        }
                        break;

                    case (int)UserKind.Recommender:
                        {
                            queryString += string.Format(" and RecommenderId='{0}'", userInfo.Id);
                            sumIndex = 1;
                        }
                        break;

                    case (int)UserKind.RecommendOfficer:
                        {
                            sumIndex = 2;
                        }
                        break;

                    case (int)UserKind.Manager:
                        {
                            sumIndex = 3;
                        }
                        break;
                }
            }

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return 0;

            try
            {
                cash = ConvToInt(rows[0][sumIndex]);
            }
            catch
            {
                cash = 0;
            }

            return cash;
        }

        public int SumGameHistory(string gameId, DateTime curDate)
        {
            int cash = 0;

            string queryString = string.Format("select sum(BuyerTotal) from tblGameHistory where YEAR(StartTime)={0} and MONTH(StartTime) = {1} and DAY(StartTime) = {2} ",
                    curDate.Year, curDate.Month, curDate.Day);

            if (string.IsNullOrEmpty(gameId) == false)
                queryString += string.Format("and GameId = {0}", gameId);

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return 0;

            try
            {
                cash = ConvToInt(rows[0][0]);
            }
            catch
            {
                cash = 0;
            }

            return cash;
        }

        public bool AddGateway(GatewayInfo gateway)
        {
            string strQuery = string.Format("insert into tblGateway (Bank, UserId, SalfStr, Gateway ) " +
                                           "Values ( N'{0}', '{1}', '{2}', '{3}' )",
                                           gateway.Bank,
                                           gateway.UserId,
                                           gateway.SalfStr,
                                           gateway.Gateway);

            return UpdateData(strQuery);
        }

        public bool UpdateGateway(GatewayInfo gateway)
        {
            string strQuery = string.Format("update tblGateway set Bank=N'{0}', UserId='{1}', SalfStr='{2}', Gateway='{3}'",
                gateway.Bank, gateway.UserId, gateway.SalfStr, gateway.Gateway);

            return UpdateData(strQuery);
        }

        public bool DelGateway(int gatewayId)
        {
            string strQuery = string.Format("delete from tblGateway where Id={0}", gatewayId);

            return UpdateData(strQuery);
        }

        public GatewayInfo FindGateway(int gatewayId)
        {
            string strQuery = string.Format("select * from tblGateway where Id = {0}", gatewayId);

            List<GatewayInfo> gatewayList = GetGatewayList(strQuery);

            if (gatewayList == null || gatewayList.Count != 1)
                return null;

            return gatewayList[0];
        }

        public List<GatewayInfo> GetGatewayList(string queryString)
        {
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<GatewayInfo> gameHistoryList = new List<GatewayInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                GatewayInfo gatewayInfo = new GatewayInfo();

                gatewayInfo.Id = ConvToInt(rows[i]["Id"]);
                gatewayInfo.Bank = ConvToString(rows[i]["Bank"]);
                gatewayInfo.UserId = ConvToString(rows[i]["UserId"]);
                gatewayInfo.SalfStr = ConvToString(rows[i]["SalfStr"]);
                gatewayInfo.Gateway = ConvToString(rows[i]["Gateway"]);

                gameHistoryList.Add(gatewayInfo);
            }

            return gameHistoryList;
        }

        public List<ClassInfo> GetAllClasses()
        {
            //string queryString = string.Format("select * from tblClassType inner join tblClassInfo on tblClassType.classInfo_id = tblClassInfo.class_id");

            string queryString = string.Format("select (select COUNT(f_id) from tblFileInfo where f_file_area=tblClassType.class_type_id) as countNumber, * from tblClassType inner join tblClassInfo on tblClassType.classInfo_id = tblClassInfo.class_id");

            return GetClassList(queryString);
        }

        public List<ClassInfo> GetClassList(string queryString)
        {
            List<ClassInfo> classInfos = new List<ClassInfo>();
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return classInfos;

            for (int i = 0; i < rows.Count; i++)
            {
                ClassInfo classInfo = new ClassInfo();

                classInfo.Class_Type_Id = ConvToInt(rows[i]["class_type_id"]);
                classInfo.Class_Type_Name = rows[i]["class_type_name"].ToString();
                classInfo.Class_Name = rows[i]["class_name"].ToString();
                classInfo.Class_Img_Uri = rows[i]["class_img_uri"].ToString();
                classInfo.ToIndex = 0;
                classInfo.FromIndex = 0;
                classInfo.ClassInfo_Id = ConvToInt(rows[i]["class_id"]);
                //classInfo.ClassCount = ClassCount(classInfo.Class_Type_Id);
                classInfo.ClassCount = ConvToInt(rows[i]["countNumber"]);
                classInfos.Add(classInfo);
            }

            return classInfos;
        }

        private int ClassCount(int class_id)
        {
            string queryString = string.Format("select COUNT(*) AS counts from tblFileInfo where f_file_area = {0}", class_id);
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return 0;
            int count = ConvToInt(rows[0]["counts"]);

            return count;
        }

        public List<ClassTypeInfo> GetAllClassType(ClassInfo classInfo)
        {
            string queryString = string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY f_update_date desc) AS rownum,(SELECT COUNT(*) as Counts FROM tblFileInfo WHERE f_file_area = {0}) AS imageCount, * FROM tblFileInfo  WHERE f_file_area = {1}) AS EMP ", classInfo.Class_Type_Id, classInfo.Class_Type_Id);
            queryString += string.Format(" WHERE rownum BETWEEN {0} AND {1}", classInfo.ToIndex, classInfo.FromIndex);

            return GetClassTypeList(queryString, 0);
        }

        public List<ClassTypeInfo> GetAllPictureClassType(ClassInfo classInfo)
        {
            //string queryString = string.Format("select * from (SELECT ROW_NUMBER() OVER (ORDER BY f_image_folder desc) AS Row,(select count(*) as rowsCounts from (select f_image_folder from tblFileInfo where f_file_area={0} group by f_image_folder) as scripts) as rowsCounts, * from tblFileInfo", classInfo.Class_Type_Id);
            //queryString += string.Format(" where tblFileInfo.f_file_area = {0} and f_id IN (select Min(f_id) from tblFileInfo", classInfo.Class_Type_Id);
            //queryString += string.Format(" group by f_image_folder having f_image_folder IN (select f_image_folder from tblFileInfo group by f_image_folder))) as EMP,");
            //queryString += string.Format(" (select COUNT(*) as counts, f_image_folder from tblFileInfo where f_file_area = {0} group by f_image_folder) AS ERP", classInfo.Class_Type_Id);
            //queryString += string.Format(" where EMP.f_image_folder = ERP.f_image_folder AND Row BETWEEN {0} AND {1}", classInfo.ToIndex, classInfo.FromIndex);

            string queryString = string.Format("SELECT *, ( select COUNT(*) from ( select f_image_folder from tblFileInfo where f_file_area = {0} group by f_image_folder ) EEP ) as totalCount", classInfo.Class_Type_Id);
            queryString += string.Format(" FROM ( ");
            queryString += string.Format("SELECT Row_Number() OVER (ORDER BY f_image_folder DESC) AS rownum, ");
            queryString += string.Format("Min(f_id) as firstId,");
            queryString += string.Format("COUNT(f_id) as imageCount,");
            queryString += string.Format("f_image_folder");
            queryString += string.Format(" FROM tblFileInfo");
            queryString += string.Format(" where f_file_area = {0}", classInfo.Class_Type_Id);
            queryString += string.Format(" group by f_image_folder) EMP INNER JOIN tblFileInfo ERP ON EMP.firstId = ERP.f_id WHERE rownum BETWEEN {0} AND {1}	", classInfo.ToIndex, classInfo.FromIndex);

            return GetClassTypeList(queryString, 1);
        }

        public List<ClassTypeInfo> GetClassTypeList(string queryString, int nFlag)
        {
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<ClassTypeInfo> classTypeInfos = new List<ClassTypeInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                ClassTypeInfo classTypeInfo = new ClassTypeInfo();

                if (nFlag == 0)
                    classTypeInfo.Class_File_Id = ConvToInt(rows[i]["f_id"]);
                else if (nFlag == 1)
                    classTypeInfo.Class_File_Id = ConvToInt(rows[i]["totalCount"]);

                classTypeInfo.Class_File_Name = rows[i]["f_file_name"].ToString();
                classTypeInfo.Class_File_Type = ConvToInt(rows[i]["f_file_type"]);
                classTypeInfo.Class_File_Date = Convert.ToDateTime(rows[i]["f_update_date"]);
                classTypeInfo.Class_File_Area = ConvToInt(rows[i]["f_file_area"]);
                classTypeInfo.Class_Video_Title = rows[i]["f_video_title"].ToString();
                classTypeInfo.Class_Img_Folder = rows[i]["f_image_folder"].ToString();
                classTypeInfo.Class_File_Count = ConvToInt(rows[i]["imageCount"]);

                classTypeInfo.Class_Row_Number = ConvToInt(rows[i]["rownum"]);

                //if (nFlag == 0)
                //    classTypeInfo.Class_Row_Number = ConvToInt(rows[i]["Row"]);
                //else if (nFlag == 1)
                //    classTypeInfo.Class_Row_Number = ConvToInt(rows[i]["rowsCounts"]);

                classTypeInfos.Add(classTypeInfo);
            }

            return classTypeInfos;
        }

        public List<ClassTypeInfo> GetAllClassPictureType(ClassTypeInfo classTypeInfo)
        {
            string queryString = string.Format("select  * from (SELECT ROW_NUMBER() OVER (ORDER BY f_image_folder desc) AS Row,");
            queryString += string.Format(" (SELECT COUNT(*) as Counts FROM tblFileInfo WHERE f_image_folder = N'{0}') AS counts,", classTypeInfo.Class_Img_Folder);
            queryString += string.Format(" * from tblFileInfo where f_image_folder = N'{0}') as EMP", classTypeInfo.Class_Img_Folder);

            return GetClassPictureTypeList(queryString);
        }

        public List<ClassTypeInfo> GetClassPictureTypeList(string queryString)
        {
            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<ClassTypeInfo> classTypeInfos = new List<ClassTypeInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                ClassTypeInfo classTypeInfo = new ClassTypeInfo();

                classTypeInfo.Class_File_Id = ConvToInt(rows[i]["f_id"]);
                classTypeInfo.Class_File_Name = rows[i]["f_file_name"].ToString();
                classTypeInfo.Class_File_Type = ConvToInt(rows[i]["f_file_type"]);
                classTypeInfo.Class_File_Date = Convert.ToDateTime(rows[i]["f_update_date"]);
                classTypeInfo.Class_File_Area = ConvToInt(rows[i]["f_file_area"]);
                classTypeInfo.Class_Video_Title = rows[i]["f_video_title"].ToString();
                classTypeInfo.Class_Img_Folder = rows[i]["f_image_folder"].ToString();
                classTypeInfo.Class_File_Count = ConvToInt(rows[i]["counts"]);
                classTypeInfo.Class_Row_Number = ConvToInt(rows[i]["Row"]);


                classTypeInfos.Add(classTypeInfo);
            }

            return classTypeInfos;
        }

        public List<FinanceInfo> GetFinanceInfo(int year, int month)
        {
            string queryString = "select dayNo, sum(Cash) as ChargeSum from tblDayFinance ";
            queryString += "left outer join tblChargeHistory ON tblDayFinance.dayNo=DAY(tblChargeHistory.StartTime) ";
            queryString += string.Format("where tblChargeHistory.Kind = 0 and YEAR(tblChargeHistory.StartTime) = {0} and MONTH(tblChargeHistory.StartTime) = {1}", year, month);

            DataRowCollection rows = GetData(queryString);
            if (rows == null)
                return null;

            List<FinanceInfo> finaceInfos = new List<FinanceInfo>();

            for (int i = 0; i < rows.Count; i++)
            {
                FinanceInfo financeInfo = new FinanceInfo();

                financeInfo.dayNo = rows[i]["dayNo"].ToString();
                financeInfo.chargeSum = ConvToInt(rows[i]["ChargeSum"]);

                finaceInfos.Add(financeInfo);
            }

            return finaceInfos;
        }

        public bool UpdateNicknameInfo(NicknameInfo nicknameInfo)
        {
            string strQuery = string.Format("Update tblNicknameInfo SET AutoId='{0}', StartTime='{1}', EndTime='{2}' WHERE Id='{3}'", 
                                                        nicknameInfo.AutoId, 
                                                        ConvDateToString(nicknameInfo.StartTime),
                                                        ConvDateToString(nicknameInfo.EndTime), 
                                                        nicknameInfo.Id);

            return UpdateData(strQuery);
        }

        public NicknameInfo GetNicknameInfoByID(string strAutoId)
        {
            string strQuery = string.Format("SELECT * FROM tblNicknameInfo WHERE AutoId = '{0}'", strAutoId);

            return GetNicknameinfo(strQuery);
        }

        public NicknameInfo GetFreeNicknameInfo()
        {
            string strQuery = string.Format("SELECT * FROM tblNicknameInfo WHERE AutoId = '' AND (EndTime IS NULL OR DATEDIFF(dd, EndTime, '{0}') > 0)", ConvDateToString(DateTime.Now));

            return GetNicknameinfo(strQuery);
        }

        public NicknameInfo GetNicknameinfo(string strQuery)
        {
            NicknameInfo nameInfo = new NicknameInfo();

            DataRowCollection rows = GetData(strQuery);

            if (rows == null)
                return null;

            if (rows.Count > 0)
            {
                Random rnd = new Random();

                int nRow = rnd.Next() % rows.Count;

                nameInfo.Id = ConvToInt(rows[nRow]["Id"]);
                nameInfo.Nickname = rows[nRow]["Nickname"].ToString();
                nameInfo.AutoId = rows[nRow]["AutoID"].ToString();
                nameInfo.StartTime = ConvToDateTime(rows[nRow]["StartTime"]);
                nameInfo.EndTime = ConvToDateTime(rows[nRow]["EndTime"]);
            }

            return nameInfo;
        }
    }
}
