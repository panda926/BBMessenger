using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public enum BoardKind
    {
        Letter = 0,
        Notice = 1,
        AdminNotice = 2
    }

    public class BoardInfo : BaseInfo
    {
        public int Id = 0;
        public int Kind = (int)BoardKind.Notice;

        public string Title = "";
        public string Content = "";
        public string Source = "";
        public DateTime WriteTime;
        public string UserId = "";
        public int UserKind = 0;
        public int Readed = 0;
        public string SendId = "";

        public BoardInfo()
        {
            _InfoType = InfoType.Board;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(Id);
            size += EncodeCount(Title);
            size += EncodeCount(Content);
            size += EncodeCount(Source);
            size += EncodeCount(ConvDateToLongString(WriteTime));
            size += EncodeCount(UserId);
            size += EncodeCount(UserKind);
            size += EncodeCount(Readed);
            size += EncodeCount(SendId);

            return size;
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, Id);
                EncodeString( bw, Title );
                EncodeString(bw, Content );
                EncodeString(bw, Source);
                EncodeString(bw, ConvDateToString(WriteTime));
                EncodeString(bw, UserId);
                EncodeInteger(bw, UserKind);
                EncodeInteger(bw, Readed);
                EncodeString(bw, SendId);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            Id = DecodeInteger(br);
            Title = DecodeString( br );
            Content = DecodeString( br );
            Source = DecodeString( br );
            WriteTime = Convert.ToDateTime(DecodeString(br).ToString());
            UserId = DecodeString(br);
            UserKind = DecodeInteger(br);
            Readed = DecodeInteger(br);
            SendId = DecodeString(br);
        }
    }
}
