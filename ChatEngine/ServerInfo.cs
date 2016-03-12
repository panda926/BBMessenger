using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class ServerInfo : BaseInfo
    {
        public string ServerAddress = string.Empty;
        public int ServerPort;
        public string DownloadAddress = string.Empty;

        public ServerInfo()
        {
            _InfoType = InfoType.Server;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(ServerAddress);
            size += EncodeCount(ServerPort);
            size += EncodeCount(DownloadAddress);

            return size;
        }

        override public void GetBytes( BinaryWriter bw )
        {
            base.GetBytes(bw);

            EncodeString( bw, ServerAddress );
            EncodeInteger(bw, ServerPort);
            EncodeString(bw, DownloadAddress);
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            ServerAddress = DecodeString( br );
            ServerPort = DecodeInteger(br);
            DownloadAddress = DecodeString(br);
        }
    }
}
