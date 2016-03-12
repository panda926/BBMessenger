using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatEngine
{
    public delegate void InvokeMethod();

    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 8192;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }

    public class Client : BaseInfo
    {
        private Socket _ClientSocket;

        private Byte[] _CompleteBuffer = null;
        private int _CompleteCount = 0;
        private int _ReceiveCount = 0;

        private Thread _ClientListener;
        private Thread _ClientReconnect;
        private Thread _ClientConneted;
        private bool _IsEndClientListener;

        private List<NotifyType> _NotifyTypes = new List<NotifyType>();
        private List<BaseInfo> _ReceiveInfos = new List<BaseInfo>();

        public UserInfo _UserInfo = new UserInfo();
        private InvokeMethod _InvokeMethod;

        public AsyncCallback m_pfnCallBack;
        IAsyncResult m_result;

        private DateTime _dtPing;           // 서버에서 마지막으로 Nitify_Ping정보를 보내온 시간정보.
        private bool _bFirstPing = false;   // 처음 Notify_Ping정보인가 아닌가를 판정.

        public Client(InvokeMethod invokeMethod)
        {
            _InvokeMethod = invokeMethod;
        }

        public void AttachHandler(NotifyHandler notifyHandler)
        {
            _NotifyHandler += notifyHandler;
        }

        public bool Send(NotifyType notifyType, BaseInfo sendInfo)
        {
            try
            {
                // 2014-01-09: GreenRose
                // 재접속을 시도할때 필요한 유저정보를 보관한다.
                if (notifyType == NotifyType.Request_Login)
                {
                    _UserInfo = (UserInfo)sendInfo;
                }

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

                

                _ClientSocket.Send(btBuffer);
            }
            catch (Exception e)
            {
                SetError(ErrorType.Fail_SendSocket, "소켓송신 실패: " + e.ToString());
                return false;
            }

            return true;
        }

        //// 2014-03-18: GreenRose
        //// 소켓의 접속상태를 확인한다.
        //static public bool IsConnectedSocket(Socket client_socket)
        //{
        //    return !(client_socket.Poll(1000, SelectMode.SelectRead) && client_socket.Available == 0);
        //}

        // 2014-01-09: GreenRose
        public string strIPAddr = string.Empty;
        public string strPort = string.Empty;
        public ProtocolType typeProtocol;

        public bool Connect(string ipAddr, string port, ProtocolType protocolType)
        {
            // 2014-01-09: GreenRose
            strIPAddr = ipAddr;
            strPort = port;
            typeProtocol = protocolType;

            try
            {
                _ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType);
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ipAddr), Convert.ToInt32(port));

                _ClientSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);                
                _ClientSocket.Connect(ipe);

                //SetKeepAlive(_ClientSocket, 30000, 1000);

                WaitForData();                
            }
            catch (SocketException ex)
            {
                SetError(ErrorType.Fail_ReceiveSocket, "소켓연결 실패: 서버와의 연결을 할수 없습니다." + ex.ToString());
                return false;
            }

            return true;
        }

        public void WaitForData()
        {
            try
            {
                if (m_pfnCallBack == null)
                {
                    m_pfnCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.thisSocket = _ClientSocket;
                theSocPkt.dataBuffer = new byte[_ClientSocket.ReceiveBufferSize];
                // Start listening to the data asynchronously
                m_result = _ClientSocket.BeginReceive(theSocPkt.dataBuffer,
                                                        0, theSocPkt.dataBuffer.Length,
                                                        SocketFlags.None,
                                                        m_pfnCallBack,
                                                        theSocPkt);
            }
            catch (SocketException)
            {
            }
        }

        // socket
        public class SocketPacket
        {
            public Socket thisSocket;
            public byte[] dataBuffer = new byte[1];
        }

        private void OnClientConnected()
        {
            while(true)
            {
                Thread.Sleep(1000);
                if ((DateTime.Now - _dtPing).TotalSeconds > 90)
                {
                    try
                    {
                        _ClientSocket.Shutdown(SocketShutdown.Both);
                        _ClientSocket.Disconnect(true);
                        _ClientSocket.Close();
                        _ClientSocket.Dispose();
                        _ClientSocket = null;
                    }
                    catch (Exception ex)
                    {
                        SetError(ErrorType.Fail_CoonectSocket, ex.ToString());
                    }

                    Thread.Sleep(1000);
                    _ClientReconnect = new Thread(Reconnect);
                    _ClientReconnect.Start();

                    _ClientConneted.Abort();
                    _ClientConneted = null;

                    break;
                }
            }
        }


        public void Disconnect()
        {
            _IsEndClientListener = true;

            _ClientSocket.Close();
            _ClientSocket = null;

            if (_ClientConneted != null)
                _ClientConneted.Abort();

            if (_ClientReconnect != null)
                _ClientReconnect.Abort();
        }

        //int nRecevieSum = 0;
        private void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                Byte[] buffer = theSockId.dataBuffer;
                int iRx = theSockId.thisSocket.EndReceive(asyn);

                //nRecevieSum += iRx;

                //SetError(ErrorType.Fail_SendSocket, nRecevieSum.ToString());

                // Temp
                //SetError(ErrorType.Fail_ReceiveSocket, iRx.ToString());

                MemoryStream ms = new MemoryStream(buffer, false);
                BinaryReader br = new BinaryReader(ms);

                while (iRx > 0)
                {
                    if (_CompleteBuffer == null)
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

                        HandleEvent(binaryReader);

                        binaryReader.Close();
                        memoryStream.Close();
                    }
                    else
                    {
                        HandleEvent(br);
                    }
                }
                br.Close();
                ms.Close();

                WaitForData();

                Thread.Sleep(0);
            }
            catch (ObjectDisposedException)
            {                
                //SetError(ErrorType.Fail_ReceiveSocket, "OnDataReceived: Socket has been closed");
                //_NotifyHandler(NotifyType.Notify_Socket, null, null);

                //_ClientSocket.Shutdown(SocketShutdown.Both);
                //_ClientSocket.Disconnect(true);
                //_ClientSocket.Close();
                //_ClientSocket.Dispose();

                //_ClientReconnect = new Thread(Reconnect);
                //_ClientReconnect.Start();
            }
            catch (SocketException ex)
            {
                SetError(ErrorType.Fail_ReceiveSocket, "소켓수신 실패: 소켓을 열수 없습니다." + ex.ToString());                

                //try
                //{
                //    _ClientSocket.Shutdown(SocketShutdown.Both);
                //    _ClientSocket.Disconnect(true);
                //    _ClientSocket.Close();
                //    _ClientSocket.Dispose();
                //    _ClientSocket = null;
                //}
                //catch (Exception ex1)
                //{
                //    SetError(ErrorType.Fail_CoonectSocket, ex1.ToString());
                //}

                //Thread.Sleep(1000);

                //if (_ClientReconnect != null)
                //{
                //    _ClientReconnect.Abort();
                //    _ClientReconnect = null;
                //}

                //_ClientReconnect = new Thread(Reconnect);
                //_ClientReconnect.Start();
            }
            catch { }
        }

        private const int BytesPerLong = 4; // 32 / 8
        private const int BitsPerByte = 8;

        public static bool SetKeepAlive(Socket soc, ulong time, ulong interval)
        {
            try
            {
                // Array to hold input values.

                var input = new[]
            	{
            		(time == 0 || interval == 0) ? 0UL : 1UL, // on or off

					time,
					interval
				};

                // Pack input into byte struct.

                byte[] inValue = new byte[3 * BytesPerLong];
                for (int i = 0; i < input.Length; i++)
                {
                    inValue[i * BytesPerLong + 3] = (byte)(input[i] >> ((BytesPerLong - 1) * BitsPerByte) & 0xff);
                    inValue[i * BytesPerLong + 2] = (byte)(input[i] >> ((BytesPerLong - 2) * BitsPerByte) & 0xff);
                    inValue[i * BytesPerLong + 1] = (byte)(input[i] >> ((BytesPerLong - 3) * BitsPerByte) & 0xff);
                    inValue[i * BytesPerLong + 0] = (byte)(input[i] >> ((BytesPerLong - 4) * BitsPerByte) & 0xff);
                }

                // Create bytestruct for result (bytes pending on server socket).

                byte[] outValue = BitConverter.GetBytes(0);

                // Write SIO_VALS to Socket IOControl.

                soc.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.KeepAlive, true);
                soc.IOControl(IOControlCode.KeepAliveValues, inValue, outValue);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Failed to set keep-alive: {0} {1}", e.ErrorCode, e);
                return false;
            }

            return true;
        }

        // 2014-01-09: GreenRose
        // 서버와의 연결이 끊어졌을경우 재접속을 시도한다.
        private void Reconnect()
        {
            bool bFlag = false;
            while (bFlag == false)
            {
                bFlag = Connect(strIPAddr, strPort, typeProtocol);
            }

            Send(NotifyType.Request_Reconnect, _UserInfo);
            _ClientReconnect.Abort();
        }

        private void HandleEvent(BinaryReader br)
        {
            // 마지막으로 접수된 패킷의 시간.

            _dtPing = DateTime.Now;

            NotifyType notifyType = (NotifyType)DecodeInteger(br);

            // Temp
            //SetError(ErrorType.Fail_ReceiveSocket, notifyType.ToString());

            BaseInfo baseInfo = BaseInfo.CreateInstance(br);
            baseInfo._dtReceiveTime = System.DateTime.Now;

            // 2013-12-27: GreenRose
            switch (notifyType)
            {
                case NotifyType.Reply_RoomDetail:
                    {
                        Console.WriteLine("Reply_roomDetail");
                        RoomDetailInfo roomDetailInfo = (RoomDetailInfo)baseInfo;
                    }
                    break;
                case NotifyType.Reply_EnterMeeting:
                    {
                        Console.WriteLine("Reply_EnterMeeting");
                    }
                    break;
                case NotifyType.Reply_ClassInfo:
                    {
                        Console.WriteLine("Reply_ClassInfo");
                    }
                    break;
            }

            ResultInfo resultInfo = new ResultInfo();

            if (notifyType == NotifyType.Notify_Ping)
            {
                _dtPing = DateTime.Now;
                if (_ClientConneted == null || !_ClientConneted.IsAlive)
                {
                    _ClientConneted = new Thread(OnClientConnected);
                    _ClientConneted.Start();
                }

                Send(NotifyType.Notify_Ping, resultInfo);
            }
            else if (notifyType == NotifyType.Reply_VoiceChat)
            {
                _NotifyHandler(notifyType, null, baseInfo);
            }
            else
            {                
                _NotifyTypes.Add(notifyType);
                _ReceiveInfos.Add(baseInfo);
            }

            _CompleteBuffer = null;

            if (_InvokeMethod != null)
                _InvokeMethod();
        }

        public void NotifyReceiveData()
        {
            try
            {
                ResultInfo resultInfo = new ResultInfo();
                int count = _NotifyTypes.Count;

                for (int i = 0; i < count; i++)
                {
                    NotifyType notifyType = _NotifyTypes[i];
                    BaseInfo baseInfo = _ReceiveInfos[i];

                    // 2014-04-01: GreenRose
                    _NotifyTypes.RemoveAt(0);
                    _ReceiveInfos.RemoveAt(0);

                    count = _NotifyTypes.Count;
                    i = -1;

                    //_NotifyHandler(_NotifyTypes[i], null, _ReceiveInfos[i]);
                    _NotifyHandler(notifyType, null, baseInfo);
                }

                //_NotifyTypes.RemoveRange(0, count);
                //_ReceiveInfos.RemoveRange(0, count);
            }
            catch (Exception e)
            {
                string str = e.ToString();
                SetErrorInfo(str);
            }
        }


        public void SetErrorInfo(string strErrorMessage)
        {
            try
            {
                string strAppPath = ".\\";
                string strErrorFilePath = strAppPath + "Error.txt";

                FileStream fileStream = new FileStream(strErrorFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] wb = null;

                wb = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}{1}< {2}>{3}", DateTime.Now.ToString(), Environment.NewLine, strErrorMessage, Environment.NewLine));

                fileStream.Seek(0, SeekOrigin.End);
                fileStream.Write(wb, 0, wb.Length); //파일의 끝에 문자열을 붙인다.
                fileStream.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
