using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace UpdateServer
{
    public class Server : BaseInfo
    {
        private static Server _instance = null;

        private Socket _ServerSocket;
        private AsyncCallback _AsyncCallBack;

        private Byte[] _CompleteBuffer = null;
        private int _CompleteCount = 0;
        private int _ReceiveCount = 0;

        public static Server GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Server();
            }

            return _instance;
        }

        public Server()
        {
        }

        public bool Send( Socket socket, NotifyType notifyType, BaseInfo sendInfo )
        {
            try
            {
                if (socket.Connected == false )        // auto
                    return true;

                int sendSize = 4 + sendInfo.GetSize();

                HeaderInfo headerInfo = new HeaderInfo();
                headerInfo._BodySize = sendSize;

                byte[] btBuffer = new byte[headerInfo.GetSize() + sendSize];

                MemoryStream ms = new MemoryStream(btBuffer, true);
                BinaryWriter bw = new BinaryWriter(ms);

                headerInfo.GetBytes(bw);
                EncodeInteger(bw, (int)notifyType);
                sendInfo.GetBytes(bw);

                bw.Close();
                ms.Close();

                socket.Send(btBuffer);
            }
            catch (Exception e)
            {
                _NotifyHandler(NotifyType.Notify_Socket, socket, null );
                return false;
            }

            return true;
        }

        public void Connect(string ipAddr, string port )
        {
            _ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, Convert.ToInt32(port));

            _ServerSocket.Bind(ipLocal);//bind to the local IP Address...
            _ServerSocket.Listen(5);//start listening...

            // create the call back for any client connections...
            _ServerSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }

        public void Disconnect()
        {
            _ServerSocket.Close();
            _ServerSocket = null;
        }

        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                if (_ServerSocket != null)
                {
                    Socket socket = _ServerSocket.EndAccept(asyn);
                    WaitForData(socket);

                    _ServerSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                }
            }
            catch (Exception e)
            {
                _NotifyHandler(NotifyType.Notify_Socket, null, null);
            }
        }

        public void WaitForData(Socket soc)
        {
            try
            {
                if (_AsyncCallBack == null)
                    _AsyncCallBack = new AsyncCallback(OnDataReceived);

                KeyValuePair aKeyValuePair = new KeyValuePair();

                aKeyValuePair._Socket = soc;
                aKeyValuePair._DataBuffer = new byte[soc.ReceiveBufferSize];

                soc.BeginReceive(aKeyValuePair._DataBuffer, 0, aKeyValuePair._DataBuffer.Length, SocketFlags.None, _AsyncCallBack, aKeyValuePair);
            }
            catch (SocketException e)
            {
                _NotifyHandler(NotifyType.Notify_Socket, soc, null);
            }
        }

        public void OnDataReceived(IAsyncResult asyn)//For Server Mode
        {
            KeyValuePair aKeyValuePair = (KeyValuePair)asyn.AsyncState;
            
            Socket socket = aKeyValuePair._Socket;
            Byte[] buffer = aKeyValuePair._DataBuffer;

            //_NotifyHandler(NotifyType.Notify_notify, socket, null);

            try
            {
                int iRx = socket.EndReceive(asyn);

                MemoryStream ms = new MemoryStream(buffer, false);
                BinaryReader br = new BinaryReader(ms);

                while (iRx > 0)
                {
                    if ( _CompleteBuffer == null)
                    {
                        HeaderInfo headerInfo = (HeaderInfo)BaseInfo.CreateInstance(br);

                        if (headerInfo == null)
                            break;

                        if (headerInfo._Header != HeaderInfo.Marker)
                            break;

                        _CompleteCount = headerInfo._BodySize;
                        _ReceiveCount = iRx - headerInfo.GetSize();

                        if (_CompleteCount > _ReceiveCount)
                        {
                            _CompleteBuffer = new byte[_CompleteCount + 1];

                            for (int i = 0; i < _ReceiveCount; i++)
                                _CompleteBuffer[i] = br.ReadByte();

                            break;
                        }

                        iRx -= headerInfo.GetSize();
                        iRx -= _CompleteCount;
                    }
                    else
                    {
                        int readCount = _CompleteCount - _ReceiveCount;

                        if (readCount > iRx)
                            readCount = iRx;

                        for (int i = 0; i < readCount; i++)
                            _CompleteBuffer[_ReceiveCount + i] = br.ReadByte();

                        _ReceiveCount += readCount;

                        if (_CompleteCount > _ReceiveCount)
                            break;

                        iRx -= readCount;
                    }

                    if (_CompleteBuffer != null)
                    {
                        MemoryStream memoryStream = new MemoryStream(_CompleteBuffer, false);
                        BinaryReader binaryReader = new BinaryReader(memoryStream);

                        HandleEvent(binaryReader, socket);

                        binaryReader.Close();
                        memoryStream.Close();
                    }
                    else
                    {
                        HandleEvent(br, socket);
                    }
                }
                br.Close();
                ms.Close();

                WaitForData(socket);
            }
            catch (SocketException e)
            {
                _NotifyHandler(NotifyType.Notify_Socket, socket, null);
            }
        }

        private void HandleEvent(BinaryReader br, Socket socket )
        {
            try
            {
                NotifyType notifyType = (NotifyType)DecodeInteger(br);

                BaseInfo baseInfo = BaseInfo.CreateInstance(br);

                _NotifyHandler(notifyType, socket, baseInfo);

                _CompleteBuffer = null;
            }
            catch (Exception)
            {
                //BaseInfo.SetError(ErrorType.Exception_Occur, ex.Message + "\n" + ex.StackTrace);
                //ErrorView.AddErrorString();
            }
        }
    }
}
