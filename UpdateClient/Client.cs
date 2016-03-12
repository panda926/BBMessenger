using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace UpdateClient
{
    public class Client : BaseInfo
    {
        private Socket _ClientSocket;

        private Byte[] _CompleteBuffer = null;
        private int _CompleteCount = 0;
        private int _ReceiveCount = 0;
        
        private Thread _ClientListener;
        private bool _IsEndClientListener;

        private List<NotifyType> _NotifyTypes = new List<NotifyType>();
        private List<BaseInfo> _ReceiveInfos = new List<BaseInfo>();

        public Client()
        {
        }

        public bool Send(NotifyType notifyType, BaseInfo sendInfo )
        {
            try
            {
                int sendSize = 4 + sendInfo.GetSize() ;

                HeaderInfo headerInfo = new HeaderInfo();
                headerInfo._BodySize = sendSize;

                byte[] btBuffer = new byte[ headerInfo.GetSize() + sendSize];

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
                _NotifyHandler(NotifyType.Notify_Socket, null, null);
                return false;
            }

            return true;
        }

        public bool Connect(string ipAddr, string port, ProtocolType protocolType )
        {
            try
            {
                _ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType);
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ipAddr), Convert.ToInt32(port));
                _ClientSocket.Connect(ipe);

                _IsEndClientListener = false;

                _ClientListener = new Thread(OnDataReceived);
                _ClientListener.Start();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void Disconnect()
        {
            _IsEndClientListener = true;
            
            _ClientSocket.Close();
            _ClientSocket = null;
            
            _ClientListener.Abort();
        }

        private void OnDataReceived()
        {
            try
            {
                while (_IsEndClientListener == false)
                {
                    byte[] receiveData = new byte[_ClientSocket.ReceiveBufferSize];
                    int iRx = _ClientSocket.Receive(receiveData);
                    
                    MemoryStream ms = new MemoryStream(receiveData, false);
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
                }
            }
            catch (SocketException e)
            {
                _NotifyHandler(NotifyType.Notify_Socket, null, null);
            }
        }

        private void HandleEvent(BinaryReader br)
        {
            NotifyType notifyType = (NotifyType)DecodeInteger(br);

            BaseInfo baseInfo = BaseInfo.CreateInstance(br);

            _NotifyHandler(notifyType, null, baseInfo);

            //if (p.Dispatcher.CheckAccess()) AddText(p, "Added directly.");
            //else p.Dispatcher.BeginInvoke(
            //    new AddTextDelegate(AddText), p, "Added by Dispatcher.");

            _CompleteBuffer = null;
        }


    }
}
