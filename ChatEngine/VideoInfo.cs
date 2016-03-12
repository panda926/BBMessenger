using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class VideoInfo : BaseInfo
    {
        public string UserId = "";    // 20
        public int IsEnd = 0;
        public byte[] Data = null;
        public byte[] ImgData = null;
        public string ImgName = "";    // 20

        public VideoInfo()
        {
            _InfoType = InfoType.Video;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount(UserId) + EncodeCount(IsEnd) + 4 + Data.Length + 4 + ImgData.Length + EncodeCount(ImgName);
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, UserId );
                EncodeInteger(bw, IsEnd);

                EncodeInteger( bw, Data.Length);
                bw.Write(Data);
                EncodeInteger(bw, ImgData.Length);
                bw.Write(ImgData);

                EncodeString(bw, ImgName);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            UserId = DecodeString(br);
            IsEnd = DecodeInteger(br);

            int length = DecodeInteger(br);

            Data = new byte[length];
            Data = br.ReadBytes(length);

            int imglength = DecodeInteger(br);

            ImgData = new byte[imglength];
            ImgData = br.ReadBytes(imglength);

            ImgName = DecodeString(br);

        }
    }
}
