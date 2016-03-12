using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace UpdateServer
{

    public delegate void NotifyHandler( NotifyType notifyType, Socket socket, BaseInfo receiveInfo );
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



        public static BaseInfo CreateInstance(BinaryReader br)
        {
            BaseInfo baseInfo = null;

            InfoType infoType = (InfoType)DecodeInteger(br);

            switch (infoType)
            {
                case InfoType.Header:
                    baseInfo = new HeaderInfo();
                    break;

                case InfoType.UpdateCheck:
                    baseInfo = new UpdateCheckInfo();
                    break;

                case InfoType.UpdateFile:
                    baseInfo = new UpdateFileInfo();
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

        public static void EncodeString(BinaryWriter bw, string str)
        {
            int byteCount = EncodeCount(str) - 4;

            EncodeInteger(bw, byteCount);

            byte[] btPWD = new byte[byteCount];
            Encoding.GetEncoding("GB18030").GetBytes(str, 0, str.Length, btPWD, 0);

            bw.Write(btPWD);
        }

        public static string DecodeString(BinaryReader br )
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

        public static string ConvDateToString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm");
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

        public void NotifyBase(NotifyType notifyType, Socket socket, BaseInfo receiveInfo )
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
