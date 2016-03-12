using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatEngine
{

    public delegate void NotifyHandler(NotifyType notifyType, Socket socket, BaseInfo receiveInfo);
    public delegate void NotifyUdpHandler(NotifyType notifyType, IPEndPoint ipEndPoint, BaseInfo receiveInfo);


    // socket
    public class KeyValuePair
    {
        public Socket _Socket;
        public byte[] _DataBuffer = new byte[1];
    }


    // base
    public class BaseInfo
    {
        protected InfoType _InfoType;
        public NotifyHandler _NotifyHandler;

        protected static ErrorInfo _LastError = new ErrorInfo();
        public DateTime _dtReceiveTime;


        public static BaseInfo CreateInstance(BinaryReader br)
        {
            BaseInfo baseInfo = null;

            InfoType infoType = (InfoType)DecodeInteger(br);

            switch (infoType)
            {
                case InfoType.Result:
                    baseInfo = new ResultInfo();
                    break;

                case InfoType.Header:
                    baseInfo = new HeaderInfo();
                    break;

                case InfoType.Home:
                    baseInfo = new HomeInfo();
                    break;

                case InfoType.User:
                    baseInfo = new UserInfo();
                    break;

                case InfoType.UserDetail:
                    baseInfo = new UserDetailInfo();
                    break;

                case InfoType.Room:
                    baseInfo = new RoomInfo();
                    break;

                case InfoType.RoomList:
                    baseInfo = new RoomListInfo();
                    break;

                case InfoType.RoomDetail:
                    baseInfo = new RoomDetailInfo();
                    break;

                case InfoType.String:
                    baseInfo = new StringInfo();
                    break;

                case InfoType.Voice:
                    baseInfo = new VoiceInfo();
                    break;

                case InfoType.Video:
                    baseInfo = new VideoInfo();
                    break;

                case InfoType.Present:
                    baseInfo = new IconInfo();
                    break;

                case InfoType.Give:
                    baseInfo = new GiveInfo();
                    break;

                case InfoType.Game:
                    baseInfo = new GameInfo();
                    break;

                case InfoType.GameList:
                    baseInfo = new GameListInfo();
                    break;

                case InfoType.GameDetail:
                    baseInfo = new GameDetailInfo();
                    break;

                case InfoType.ChatHistory:
                    baseInfo = new ChatHistoryInfo();
                    break;

                case InfoType.PointHistory:
                    baseInfo = new PointHistoryInfo();
                    break;

                case InfoType.ChargeHistory:
                    baseInfo = new ChargeHistoryInfo();
                    break;

                case InfoType.EvalHistory:
                    baseInfo = new EvalHistoryInfo();
                    break;

                case InfoType.PresentHistory:
                    baseInfo = new PresentHistoryInfo();
                    break;

                case InfoType.GameHistory:
                    baseInfo = new GameHistoryInfo();
                    break;

                case InfoType.UserList:
                    baseInfo = new UserListInfo();
                    break;

                case InfoType.AskChat:
                    baseInfo = new AskChatInfo();
                    break;

                case InfoType.Board:
                    baseInfo = new BoardInfo();
                    break;

                case InfoType.Betting:
                    baseInfo = new BettingInfo();
                    break;

                case InfoType.Table:
                    baseInfo = new TableInfo();
                    break;

                case InfoType.Sicbo:
                    baseInfo = new SicboInfo();
                    break;

                case InfoType.Dice:
                    baseInfo = new DiceInfo();
                    break;

                case InfoType.DzCard:
                    baseInfo = new DzCardInfo();
                    break;

                case InfoType.Horse:
                    baseInfo = new HorseInfo();
                    break;

                case InfoType.BumperCar:
                    baseInfo = new BumperCarInfo();
                    break;

                case InfoType.Fish:
                    baseInfo = new FishInfo();
                    break;

                case InfoType.FishSend:
                    baseInfo = new FishSendInfo();
                    break;

                case InfoType.AddScore:
                    baseInfo = new AddScoreInfo();
                    break;

                case InfoType.SendCard:
                    baseInfo = new SendCardInfo();
                    break;

                case InfoType.Musice:
                    baseInfo = new MusiceInfo();
                    break;

                case InfoType.MusiceState:
                    baseInfo = new MusiceStateInfo();
                    break;

                case InfoType.ClassInfo:
                    baseInfo = new ClassInfo();
                    break;

                case InfoType.ClassList:
                    baseInfo = new ClassListInfo();
                    break;

                case InfoType.ClassTypeInfo:
                    baseInfo = new ClassTypeInfo();
                    break;

                case InfoType.ClassTypeList:
                    baseInfo = new ClassTypeListInfo();
                    break;

                case InfoType.ClassPictureDetail:
                    baseInfo = new ClassPictureDetailInfo();
                    break;

                case InfoType.RoomValue:
                    baseInfo = new RoomPrice();
                    break;

                case InfoType.Payment:
                    baseInfo = new PaymentInfo();
                    break;

                case InfoType.DWGameFile:
                    baseInfo = new DWGameFileInfo();
                    break;

                case InfoType.UserState:
                    baseInfo = new UserState();
                    break;

                case InfoType.Server:
                    baseInfo = new ServerInfo();
                    break;

                case InfoType.AVMsg:
                    baseInfo = new AVMsg();
                    break;
            }

            if (baseInfo != null)
            {
                baseInfo.FromBytes(br);
            }

            return baseInfo;
        }

        public static int EncodeCount(string s)
        {
            int byteCount = Encoding.GetEncoding("GB18030").GetByteCount(s) + 4;

            return byteCount;
        }

        public static int EncodeCount(int value)
        {
            return 4;
        }

        public static int EncodeCount(long value)
        {
            return 8;
        }

        public static void EncodeString(BinaryWriter bw, string str)
        {
            int byteCount = EncodeCount(str) - 4;

            EncodeInteger(bw, byteCount);

            byte[] btPWD = new byte[byteCount];
            Encoding.GetEncoding("GB18030").GetBytes(str, 0, str.Length, btPWD, 0);

            bw.Write(btPWD);
        }

        public static string DecodeString(BinaryReader br)
        {
            int byteCount = DecodeInteger(br);

            string source = Encoding.GetEncoding("GB18030").GetString(br.ReadBytes(byteCount));
            string dest = source;

            int index = dest.IndexOf('\0');
            if (index > -1)
            {
                dest = source.Substring(0, index + 1);
            }
            return dest.TrimEnd('\0').Trim();
        }

        public static void EncodeInteger(BinaryWriter bw, int num)
        {
            bw.Write(IPAddress.HostToNetworkOrder(num));
        }

        public static int DecodeInteger(BinaryReader br)
        {
            return IPAddress.NetworkToHostOrder(br.ReadInt32());
        }

        public static void EncodeLong(BinaryWriter bw, long num)
        {
            bw.Write(IPAddress.HostToNetworkOrder(num));
        }

        public static long DecodeLong(BinaryReader br)
        {
            return IPAddress.NetworkToHostOrder(br.ReadInt64());
        }

        public static string ConvDateToString(DateTime dateTime)
        {
            if (dateTime.Year < 1900)
                return string.Empty;

            return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public static string ConvDateToLongString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public static int ConvToInt(object str)
        {
            int val = 0;

            try
            {
                val = Convert.ToInt32(str);
            }
            catch { }

            return val;
        }

        public static string ConvToString(object str)
        {
            string val = string.Empty;

            try
            {
                val = Convert.ToString(str);
            }
            catch { }

            return val;
        }

        public static DateTime ConvToDateTime(object str)
        {
            DateTime dateTime = new DateTime();

            try
            {
                dateTime = Convert.ToDateTime(str);
            }
            catch { }

            return dateTime;
        }

        public static void SetError(ErrorType errorType, string errorString)
        {
            try
            {
                _LastError.ErrorType = errorType;
                _LastError.ErrorString = errorString;

                string strAppPath = ".\\";
                string strErrorFilePath = strAppPath + string.Format("Error.txt");

                FileStream fileStream = new FileStream(strErrorFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] wb = null;

                wb = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}{1}< {2}>{3}", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), Environment.NewLine, errorString, Environment.NewLine));

                fileStream.Seek(0, SeekOrigin.End);
                fileStream.Write(wb, 0, wb.Length);
                fileStream.Close();
            }
            catch (Exception)
            {
            }
        }

        public static ErrorInfo GetError()
        {
            return _LastError;
        }

        public BaseInfo()
        {
            _InfoType = InfoType.None;

            //_ErrorList = new List<ErrorInfo>();

            _NotifyHandler = this.NotifyBase;
        }

        public void AttachHandler(NotifyHandler notifyHandler)
        {
            _NotifyHandler += notifyHandler;
        }

        public void DetachHandler(NotifyHandler notifyHandler)
        {
            _NotifyHandler -= notifyHandler;
        }

        public void NotifyBase(NotifyType notifyType, Socket socket, BaseInfo receiveInfo)
        {
        }

        virtual public int GetSize()
        {
            return 4;
        }

        virtual public void GetBytes(BinaryWriter bw)
        {
            EncodeInteger(bw, (int)_InfoType);
        }

        virtual public void FromBytes(BinaryReader br)
        {
        }

    }


}
