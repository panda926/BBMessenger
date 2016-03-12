using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatEngine
{
    public class UdpServerClient : BaseInfo
    {
        private static UdpServerClient _instance = null;

        private string _ServerAddress = null;
        private int _ServerPort = 0;
        private int _ClientPort = 0;

        private List<UdpClient> _Sockets = null;
        private int _CurSocketIndex = 0;

        private NotifyUdpHandler _NotifyUdpHandler = null;

        public static UdpServerClient GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UdpServerClient();
            }

            return _instance;
        }

        public UdpServerClient()
        {
        }

        public void Connect(int basePort, int socketCount )
        {
            if (_Sockets != null)
                return;

            _ClientPort = basePort + 10;
            _ServerPort = _ClientPort + 200;

            _Sockets = new List<UdpClient>();

            for (int i = 0; i < socketCount; i++)
            {
                UdpClient udpClient = new UdpClient(_ServerPort + i);
                udpClient.BeginReceive(udpReceiveCallback, udpClient);

                _Sockets.Add( udpClient );
            }
        }

        public string Connect( string serverAddress, int basePort, int socketCount)
        {
            if (_Sockets != null)
                return "";

            string str = "";

            try
            {
                _ServerAddress = serverAddress;

                _ClientPort = basePort + 10;
                _ServerPort = _ClientPort + 200;

                _Sockets = new List<UdpClient>();

                for (int i = 0; i < socketCount; i++)
                {
                    UdpClient udpClient = new UdpClient(_ClientPort + i);
                    udpClient.BeginReceive(udpReceiveCallback, udpClient);

                    _Sockets.Add(udpClient);
                }
            }
            catch( Exception e )
            {
                str = e.Message;
            }

            return str;
        }

        public void Ping( BaseInfo baseInfo )
        {
            UserInfo userInfo = (UserInfo)baseInfo;

            for (int i = 0; i < _Sockets.Count; i++)
            {
                userInfo.UdpPort = i;

                Send(NotifyType.Request_VideoPort, userInfo);
            }
        }

        public bool Send(NotifyType notifyType, BaseInfo sendInfo)
        {
            if (_ServerAddress == null)
                return false;

            IPAddress ipAddress = IPAddress.Parse(_ServerAddress);
            IPEndPoint remotePoint = new IPEndPoint(ipAddress, _ServerPort + _CurSocketIndex);

            return Send(remotePoint, notifyType, sendInfo);
        }

        public bool Send(Socket socket, int[] ports, NotifyType notifyType, BaseInfo sendInfo)
        {
            IPEndPoint socketPoint = (IPEndPoint)socket.RemoteEndPoint;
            IPEndPoint remotePoint = new IPEndPoint(socketPoint.Address, ports[_CurSocketIndex] );

            return Send(remotePoint, notifyType, sendInfo);
        }

        private bool Send(IPEndPoint remotePoint, NotifyType notifyType, BaseInfo sendInfo)
        {
            try
            {
                byte[] btBuffer = new byte[4 + sendInfo.GetSize()];

                MemoryStream ms = new MemoryStream(btBuffer, true);
                BinaryWriter bw = new BinaryWriter(ms);

                EncodeInteger(bw, (int)notifyType);
                sendInfo.GetBytes(bw);

                bw.Close();
                ms.Close();

                _Sockets[ _CurSocketIndex ].Send(btBuffer, btBuffer.Length, remotePoint);
            }
            catch (Exception e)
            {
                SetError(ErrorType.Fail_SendSocket, "소켓송신 실패: " + e.Message );
                return false;
            }

            _CurSocketIndex = (_CurSocketIndex + 1) % _Sockets.Count;

            return true;
        }


        void udpReceiveCallback(IAsyncResult ar)
        {
            try
            {
                UdpClient udpServer = ar.AsyncState as UdpClient;
                IPEndPoint remoteEndPoint = null;

                byte[] receiveBytes = udpServer.EndReceive(ar, ref remoteEndPoint);

                MemoryStream ms = new MemoryStream(receiveBytes, false);
                BinaryReader br = new BinaryReader(ms);

                NotifyType notifyType = (NotifyType)DecodeInteger(br);

                BaseInfo baseInfo = BaseInfo.CreateInstance(br);

                br.Close();
                ms.Close();

                if (_NotifyUdpHandler != null)
                    _NotifyUdpHandler(notifyType, remoteEndPoint, baseInfo);

                udpServer.BeginReceive(udpReceiveCallback, udpServer);

            }
            catch(Exception e)
            {
                SetError(ErrorType.Fail_WaitSocket, "소켓대기 실패: " + e.Message);
                //_notifyUdpHandler(NotifyType.Notify_Socket, soc, null);
            }
        }

        public void Disconnect()
        {
            if (_Sockets == null)
                return;

            for (int i = 0; i < _Sockets.Count; i++)
            {
                _Sockets[i].Close();
            }

            _Sockets = null;
        }

        public void AttachUdpHandler(NotifyUdpHandler notifyHandler)
        {
            _NotifyUdpHandler += notifyHandler;
        }

        public void DetachUdpHander(NotifyUdpHandler notifyUdpHandler)
        {
            _NotifyUdpHandler -= notifyUdpHandler;
        }
    }
}
