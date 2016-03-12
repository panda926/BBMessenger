using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class AVMsg : BaseInfo
    {
        public string _strRoomID = string.Empty;

        public string _strLocalIP = string.Empty;
        public int _nLocalPort = 0;
        public string _strRemoteIP = string.Empty;
        public int _nRemotePort = 0;

        public int bBitCount = 0;
        public int biClrImportant = 0;
        public int biClrUsed = 0;
        public int biCompression = 0;
        public int biHeight = 0;
        public int biPlanes = 0;
        public int biSize = 0;
        public int biSizeImage = 0;
        public int biWidth = 0;
        public int biXPelsPerMeter = 0;
        public int biYPelsPerMeter = 0;

        public AVMsg()
        {
            _InfoType = InfoType.AVMsg;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(_strRoomID);
            size += EncodeCount(_strLocalIP);
            size += EncodeCount(_nLocalPort);
            size += EncodeCount(_strRemoteIP);
            size += EncodeCount(_nRemotePort);

            size += EncodeCount(bBitCount);
            size += EncodeCount(biClrImportant);
            size += EncodeCount(biClrUsed);
            size += EncodeCount(biCompression);
            size += EncodeCount(biHeight);
            size += EncodeCount(biPlanes);
            size += EncodeCount(biSize);
            size += EncodeCount(biSizeImage);
            size += EncodeCount(biWidth);
            size += EncodeCount(biXPelsPerMeter);
            size += EncodeCount(biYPelsPerMeter);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, _strRoomID);
                EncodeString(bw, _strLocalIP);
                EncodeInteger(bw, _nLocalPort);
                EncodeString(bw, _strRemoteIP);
                EncodeInteger(bw, _nRemotePort);

                EncodeInteger(bw, bBitCount);
                EncodeInteger(bw, biClrImportant);
                EncodeInteger(bw, biClrUsed);
                EncodeInteger(bw, biCompression);
                EncodeInteger(bw, biHeight);
                EncodeInteger(bw, biPlanes);
                EncodeInteger(bw, biSize);
                EncodeInteger(bw, biSizeImage);
                EncodeInteger(bw, biXPelsPerMeter);
                EncodeInteger(bw, biYPelsPerMeter);
            }
            catch (Exception)
            { }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            _strRoomID = DecodeString(br);
            _strLocalIP = DecodeString(br);
            _nLocalPort = DecodeInteger(br);
            _strRemoteIP = DecodeString(br);
            _nRemotePort = DecodeInteger(br);

            bBitCount = DecodeInteger(br);
            biClrImportant = DecodeInteger(br);
            biClrUsed = DecodeInteger(br);
            biCompression = DecodeInteger(br);
            biHeight = DecodeInteger(br);
            biPlanes = DecodeInteger(br);
            biSize = DecodeInteger(br);
            biSizeImage = DecodeInteger(br);
            biXPelsPerMeter = DecodeInteger(br);
            biYPelsPerMeter = DecodeInteger(br);
        }
    }
}
