using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace ChatEngine
{
    public enum UserKind
    {
        Buyer = 0,
        ServiceWoman = 1,
        Recommender = 2,
        ServiceOfficer = 3,
        RecommendOfficer = 4,
        Manager = 5,
        Banker = 6,
        Administrator = 7
    }

    public class MoneyKind
    {
        public const int Cash = 0;
        public const int Point = 1;
    }

    public class UserInfo : BaseInfo
    {
        // relative to db
        public string Id = "";         
        public string Password = "";

        public string Nickname = "";
        public string Icon = "";
        public string Sign = "";
        public int Year = 0;
        public int Month = 0;
        public int Day = 0;
        public string Address = "";

        public int Cash = 0;
        public int Point = 0;
        public DateTime RegistTime;
        public string LoginTime = "";

        public int Kind = 0;
        static public string[] KindList = { "消费者", "服务员", "广告员", "服务代表", "广告代表", "总经理", "银行员" };

        public string Friend = "";
        public string Recommender = "";
        public int MaxBuyers = 0;
        public int ChatPercent = 0;
        public int GamePercent = 0;
        public int Evaluation = 0;
        public int Visitors = 0;

        public int ChargeSum = 0;
        public int DischargeSum = 0;
        public int ChatSum = 0;
        public int GameSum = 0;
        public int SendSum = 0;
        public int ReceiveSum = 0;

        // relative to runtime
        public string RoomId = "";
        public string GameId = "";
        public int UdpPort = 0;

        public int userSeat;						//用户状态

        // relative to server
        public Socket Socket;
        public DateTime PingDate = new DateTime();
        public int State = 0;
        public int[] OpenPortes = new int[100];

        public DateTime EnterTime;
        public DateTime CashTime;
        public int ChatHistoryId = 0;
        public int PointHistoryId = 0;
        public int GameHistoryId = 0;
        public int Auto = 0;
        public int WaitSecond = 0;


        // 2013-12-17: GreenRose
        public string strAccountID = "";
        public string strAccountNumber = "";
        public string strAccountPass = "";

        // 2013-12-18: GreenRose
        public int nCashOrPointGame = 0;

        // 2013-12-25: GreenRose
        public string strPhoneNumber = "";

        // 2013-12-30: GreenRose
        public string strUrl = "";

        // 2014-02-04: GreenRose
        public int nUserState = 0;

        // 2014-02-11: GreenRose
        public string strOwnIP = "";

        // 2014-02-19: GreenRose
        public int nLoginCount = 0;

        // 2014-04-03: GreenRose
        // 1이면 VIP고객이고, 0이면 VIP고객이 아님.
        public int nVIP = 0;

        public UserInfo()
        {
            _InfoType = InfoType.User;
        }

        public string KindString
        {
            get
            {
                return KindList[Kind];
            }
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount( Id );         
            size += EncodeCount( Password );

            size += EncodeCount(Nickname);
            size += EncodeCount(Icon);
            size += EncodeCount(Sign);
            size += EncodeCount(Year);
            size += EncodeCount(Month);
            size += EncodeCount(Day);
            size += EncodeCount(Address);

            size += EncodeCount(Cash);
            size += EncodeCount(Point);
            //size += EncodeCount(ConvDateToString(RegistTime));
            size += EncodeCount(LoginTime);

            size += EncodeCount( Kind );

            size += EncodeCount(Friend);
            size += EncodeCount(Recommender);
            size += EncodeCount(MaxBuyers);
            size += EncodeCount(ChatPercent);
            size += EncodeCount(GamePercent);
            size += EncodeCount(Evaluation);
            size += EncodeCount(Visitors);

            size += EncodeCount( ChargeSum );
            size += EncodeCount( DischargeSum );
            size += EncodeCount( ChatSum );
            size += EncodeCount( GameSum );
            size += EncodeCount( SendSum );
            size += EncodeCount( ReceiveSum );

            size += EncodeCount( RoomId );
            size += EncodeCount( GameId );
            size += EncodeCount(UdpPort);
            
            size += EncodeCount(userSeat);

            size += EncodeCount(strAccountNumber);
            size += EncodeCount(strAccountID);
            size += EncodeCount(strAccountPass);

            size += EncodeCount(nCashOrPointGame);
            size += EncodeCount(strPhoneNumber);

            size += EncodeCount(strUrl);

            size += EncodeCount(nUserState);

            size += EncodeCount(strOwnIP);
            size += EncodeCount(nVIP);

            return size;
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, Id);
                EncodeString(bw, Password);

                EncodeString(bw, Nickname);
                EncodeString(bw, Icon);
                EncodeString(bw, Sign);
                EncodeInteger(bw, Year);
                EncodeInteger(bw, Month);
                EncodeInteger(bw, Day);
                EncodeString(bw, Address);

                EncodeInteger(bw, Cash);
                EncodeInteger(bw, Point);
                //EncodeString(bw, ConvDateToString(RegistTime));
                EncodeString(bw, LoginTime);

                EncodeInteger(bw, Kind);

                EncodeString(bw, Friend);
                EncodeString(bw, Recommender);
                EncodeInteger(bw, MaxBuyers);
                EncodeInteger(bw, ChatPercent);
                EncodeInteger(bw, GamePercent);
                EncodeInteger(bw, Evaluation);
                EncodeInteger(bw, Visitors);

                EncodeInteger(bw, ChargeSum);
                EncodeInteger(bw, DischargeSum);
                EncodeInteger(bw, ChatSum);
                EncodeInteger(bw, GameSum);
                EncodeInteger(bw, SendSum);
                EncodeInteger(bw, ReceiveSum);

                EncodeString(bw, RoomId);
                EncodeString(bw, GameId);
                EncodeInteger(bw, UdpPort);

                EncodeInteger(bw, userSeat);

                EncodeString(bw, strAccountNumber);
                EncodeString(bw, strAccountID);
                EncodeString(bw, strAccountPass);

                EncodeInteger(bw, nCashOrPointGame);

                EncodeString(bw, strPhoneNumber);

                EncodeString(bw, strUrl);

                EncodeInteger(bw, nUserState);

                EncodeString(bw, strOwnIP);

                EncodeInteger(bw, nVIP);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            Id = DecodeString(br);
            Password = DecodeString(br);

            Nickname = DecodeString(br);
            Icon = DecodeString(br);
            Sign = DecodeString(br);
            Year = DecodeInteger(br);
            Month = DecodeInteger(br);
            Day = DecodeInteger(br);
            Address = DecodeString(br);

            Cash = DecodeInteger(br);
            Point = DecodeInteger(br);
            //RegistTime = Convert.ToDateTime(DecodeString(br).ToString());
            LoginTime = DecodeString(br);

            Kind = DecodeInteger(br);

            Friend = DecodeString(br);
            Recommender = DecodeString(br);
            MaxBuyers = DecodeInteger(br);
            ChatPercent = DecodeInteger(br);
            GamePercent = DecodeInteger(br);
            Evaluation = DecodeInteger(br);
            Visitors = DecodeInteger(br);

            ChargeSum = DecodeInteger(br);
            DischargeSum = DecodeInteger(br);
            ChatSum = DecodeInteger(br);
            GameSum = DecodeInteger(br);
            SendSum = DecodeInteger(br);
            ReceiveSum = DecodeInteger(br);

            RoomId = DecodeString(br);
            GameId = DecodeString(br);
            UdpPort = DecodeInteger(br);

            userSeat = DecodeInteger(br);

            strAccountNumber = DecodeString(br);
            strAccountID = DecodeString(br);
            strAccountPass = DecodeString(br);

            nCashOrPointGame = DecodeInteger(br);

            strPhoneNumber = DecodeString(br);

            strUrl = DecodeString(br);

            nUserState = DecodeInteger(br);

            strOwnIP = DecodeString(br);
            nVIP = DecodeInteger(br);
        }

        public UserInfo Body
        {
            get
            {
                return this;

                byte[] btBuffer = new byte[GetSize()];

                MemoryStream ms = new MemoryStream(btBuffer, true);
                BinaryWriter bw = new BinaryWriter(ms);
                
                GetBytes(bw);
                bw.Close();

                ms = new MemoryStream(btBuffer, false);
                BinaryReader br = new BinaryReader(ms);

                UserInfo userInfo = new UserInfo();
                userInfo.FromBytes(br);

                br.Close();
                ms.Close();

                userInfo.Password = "";

                return userInfo;
            }
            set
            {
                Password = value.Password;

                Nickname = value.Nickname;
                Icon = value.Icon;
                Sign = value.Sign;
                Year = value.Year;
                Month = value.Month;
                Day = value.Day;
                Address = value.Address;

                MaxBuyers = value.MaxBuyers;
                ChatPercent = value.ChatPercent;
                GamePercent = value.GamePercent;

                strAccountNumber = value.strAccountNumber;
                strAccountID = value.strAccountID;
                strAccountPass = value.strAccountPass;

                strPhoneNumber = value.strPhoneNumber;

                strUrl = value.strUrl;

                nUserState = value.nUserState;

                strOwnIP = value.strOwnIP;
                nVIP = value.nVIP;
            }
        }

        public int GetGameMoney()
        {
            if (nCashOrPointGame == 0)
                return this.Cash;
            else
                return this.Point;
        }

        public void SetGameMoney(int money)
        {
            if (nCashOrPointGame == 0)
                this.Cash = money;
            else
                this.Point = money;
        }

        public bool IsAuto()
        {
            if (this.Auto > 0)
                return true;

            return false;
        }

    }
}
