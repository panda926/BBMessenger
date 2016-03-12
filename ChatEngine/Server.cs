using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatEngine
{
    // 2013-12-27: GReenRose
    //public class SocketInfo
    //{
    //    public Socket socket = null;
    //    public NotifyType notifyType;
    //    public BaseInfo baseInfo = null;
    //}

    public class SocketBuffer
    {
        public Socket _socket = null;

        public Byte[] _CompleteBuffer = null;
        public int _CompleteCount = 0;
        public int _ReceiveCount = 0;

        public List<Byte[]> _SendBuffers = new List<Byte[]>();
    }

    public class Server : BaseInfo
    {
        private static Server _instance = null;

        private Socket _ServerSocket;
        private AsyncCallback _AsyncCallBack;

        public List<string> _errorLogs = new List<string>();
        public List<SocketBuffer> _bufferList = new List<SocketBuffer>();

        //// 2013-12-27: GreenRose
        //System.Timers.Timer _timer = new System.Timers.Timer(1);
        //List<SocketInfo> _listSocketInfo = new List<SocketInfo>();
        //bool _bSendFinished = true;

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
            // 2013-12-27: GreenRose
            //_timer.Elapsed += new System.Timers.ElapsedEventHandler(SocketTimer);
            //_timer.Enabled = true;
        }

        // 2013-12-27: GreenRose
        //public void SocketTimer(object obj, System.Timers.ElapsedEventArgs e)
        //{
        //    if (_listSocketInfo.Count > 0 && _bSendFinished)
        //    {
        //        SendSocketInfo(_listSocketInfo[0].socket, _listSocketInfo[0].notifyType, _listSocketInfo[0].baseInfo);
        //    }
        //}

        // 2013-12-27: GreenRose
        //private bool SendSocketInfo(Socket socket, NotifyType notifyType, BaseInfo sendInfo)
        //{
        //    _bSendFinished = false;

        //    try
        //    {
        //        if (socket.Connected == false)        // auto
        //            return true;

        //        int sendSize = 4 + sendInfo.GetSize();

        //        HeaderInfo headerInfo = new HeaderInfo();
        //        headerInfo._BodySize = sendSize;

        //        byte[] btBuffer = new byte[headerInfo.GetSize() + sendSize];

        //        MemoryStream ms = new MemoryStream(btBuffer, true);
        //        BinaryWriter bw = new BinaryWriter(ms);

        //        headerInfo.GetBytes(bw);
        //        EncodeInteger(bw, (int)notifyType);
        //        sendInfo.GetBytes(bw);

        //        bw.Close();
        //        ms.Close();

        //        socket.Send(btBuffer);

        //        _listSocketInfo.RemoveAt(0);
        //        _bSendFinished = true;
        //    }
        //    catch (Exception e)
        //    {
        //        SetError(ErrorType.Fail_SendSocket, "소켓송신 실패: " + e.Message);
        //        return false;
        //    }

        //    return true;
        //}

        public bool Send(Socket socket, NotifyType notifyType, BaseInfo sendInfo)
        {
            //SocketInfo socketInfo = new SocketInfo();
            //socketInfo.socket = socket;
            //socketInfo.notifyType = notifyType;
            //socketInfo.baseInfo = sendInfo;

            //_listSocketInfo.Add(socketInfo);

            try
            {
                if (socket.Connected == false)        // auto
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

                //int nSendBuffersize = 0;

                //while (nSendBuffersize < btBuffer.Length)
                //{
                //    nSendBuffersize += socket.Send(btBuffer, nSendBuffersize, btBuffer.Length, SocketFlags.None);
                //}                
                //if (notifyType == NotifyType.Reply_RoomDetail)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //}

                SocketBuffer socketBuffer = FindBuffer(socket);

                socketBuffer._SendBuffers.Add( btBuffer);

                if (socketBuffer._SendBuffers.Count == 1)
                    SendAsync(socket);

                //System.DateTime dateTime1 = DateTime.Now;
                //byte[] tempBuffer = new byte[100000];
                //for (int i = 0; i < 10; i++)
                //{
                //    int nResult = socket.Send(tempBuffer);
                //    if (nResult != 10000)
                //    {
                //        SetError(ErrorType.Fail_SendSocket, "10000 fail");
                //    }
                //}

                //int nSum = 0;
                //DateTime dateTime2 = DateTime.Now;

                //double nSecond = (dateTime2 - dateTime1).TotalMilliseconds;
            }
            catch (Exception e)
            {
                SetError(ErrorType.Fail_SendSocket, "소켓송신 실패: " + e.Message);
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(true);
                socket.Close();
                _NotifyHandler(NotifyType.Notify_Socket, socket, null);
                return false;
            }

            return true;
        }

        public bool SendAsync( Socket socket )
        {
            SocketBuffer socketBuffer = FindBuffer(socket);

            if( socketBuffer._SendBuffers.Count == 0)
                return true;

            Byte[] btBuffer = socketBuffer._SendBuffers[0];

            socket.BeginSend(btBuffer, 0, btBuffer.Length, 0, new AsyncCallback(SendCallback), socket);
            return true;
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);

                SocketBuffer socketBuffer = Server.GetInstance().FindBuffer(handler);

                socketBuffer._SendBuffers.RemoveAt(0);

                Server.GetInstance().SendAsync(handler);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Connect(string ipAddr, string port)
        {
            _ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, Convert.ToInt32(port));
            _ServerSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            _ServerSocket.Bind(ipLocal);//bind to the local IP Address...
            _ServerSocket.Listen(100);//start listening...

            // create the call back for any client connections...
            _ServerSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }

        public void Disconnect()
        {
            _ServerSocket.Close();
            _ServerSocket = null;
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

        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                if (_ServerSocket != null)
                {
                    Socket socket = _ServerSocket.EndAccept(asyn);
                    socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                    WaitForData(socket);

                    _ServerSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                }
            }
            catch (Exception e)
            {
                SetError(ErrorType.Fail_CoonectSocket, "소켓접속 실패: " + e.Message);
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
                SetError(ErrorType.Fail_WaitSocket, "소켓대기 실패: " + e.Message);
                _NotifyHandler(NotifyType.Notify_Socket, soc, null);
            }
        }

        private SocketBuffer FindBuffer(Socket socket)
        {
            foreach (SocketBuffer socketBuffer in _bufferList)
            {
                if (socketBuffer._socket == socket)
                    return socketBuffer;
            }

            SocketBuffer newBuffer = new SocketBuffer();

            newBuffer._socket = socket;
            _bufferList.Add(newBuffer);

            return newBuffer;
        }

        public Object _objLockMain = new Object();
        public void OnDataReceived(IAsyncResult asyn)//For Server Mode
        {
            lock (_objLockMain)
            {
                KeyValuePair aKeyValuePair = (KeyValuePair)asyn.AsyncState;

                Socket socket = aKeyValuePair._Socket;
                Byte[] buffer = aKeyValuePair._DataBuffer;


                try
                {
                    int iRx = socket.EndReceive(asyn);
                    if (iRx == 0 && socket.Available == 0)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Disconnect(true);
                        socket.Close();
                        _NotifyHandler(NotifyType.Notify_Socket, socket, null);
                    }

                    MemoryStream ms = new MemoryStream(buffer, false);
                    BinaryReader br = new BinaryReader(ms);

                    // Temp
                    //SetError(ErrorType.Fail_ReceiveSocket, iRx.ToString());

                    SocketBuffer socketBuffer = FindBuffer(socket);

                    while (iRx > 0)
                    {
                        if (socketBuffer._CompleteBuffer == null)
                        {
                            HeaderInfo headerInfo = (HeaderInfo)BaseInfo.CreateInstance(br);

                            if (headerInfo == null)
                                break;

                            if (headerInfo._Header != HeaderInfo.Marker)
                                break;

                            socketBuffer._CompleteCount = headerInfo._BodySize;
                            socketBuffer._ReceiveCount = iRx - headerInfo.GetSize();

                            if (socketBuffer._CompleteCount > socketBuffer._ReceiveCount)
                            {
                                socketBuffer._CompleteBuffer = new byte[socketBuffer._CompleteCount + 1];

                                for (int i = 0; i < socketBuffer._ReceiveCount; i++)
                                    socketBuffer._CompleteBuffer[i] = br.ReadByte();

                                break;
                            }

                            iRx -= headerInfo.GetSize();
                            iRx -= socketBuffer._CompleteCount;
                        }
                        else
                        {
                            int readCount = socketBuffer._CompleteCount - socketBuffer._ReceiveCount;

                            if (readCount > iRx)
                                readCount = iRx;

                            for (int i = 0; i < readCount; i++)
                                socketBuffer._CompleteBuffer[socketBuffer._ReceiveCount + i] = br.ReadByte();

                            socketBuffer._ReceiveCount += readCount;

                            if (socketBuffer._CompleteCount > socketBuffer._ReceiveCount)
                                break;

                            iRx -= readCount;
                        }

                        if (socketBuffer._CompleteBuffer != null)
                        {
                            MemoryStream memoryStream = new MemoryStream(socketBuffer._CompleteBuffer, false);
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

                    System.Threading.Thread.Sleep(0);
                }
                catch (Exception e)
                {
                    SetError(ErrorType.Fail_ReceiveSocket, "소켓수신 실패: 소켓을 열수 없습니다." + e.ToString());
                    //socket.Shutdown(SocketShutdown.Both);
                    //socket.Disconnect(true);
                    //socket.Close();
                    //_NotifyHandler(NotifyType.Notify_Socket, socket, null);
                }
            }
        }

        private void HandleEvent(BinaryReader br, Socket socket)
        {
            try
            {
                NotifyType notifyType = (NotifyType)DecodeInteger(br);

                // Temp
                //SetError(ErrorType.Fail_ReceiveSocket, notifyType.ToString());

                BaseInfo baseInfo = BaseInfo.CreateInstance(br);

                _NotifyHandler(notifyType, socket, baseInfo);

                SocketBuffer socketBuffer = FindBuffer(socket);
                socketBuffer._CompleteBuffer = null;
            }
            catch (Exception ex)
            {
                _errorLogs.Add(ex.ToString());
                //BaseInfo.SetError(ErrorType.Exception_Occur, ex.Message + "\n" + ex.StackTrace);
                //ErrorView.AddErrorString();
            }
        }

        public void RemoveSocketBuffer(Socket socket)
        {
            try
            {
                for (int i = 0; i < _bufferList.Count; i++)
                {
                    if (_bufferList[i]._socket == socket)
                    {                        
                        _bufferList.RemoveAt(i);
                        break;
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }
    }
}
