using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class PresentHistoryInfo : BaseInfo
    {
        public string SendId = "";
        public string ReceiveId = "";
        public int Cash = 0;
        public string Descripiton = "";
        public DateTime SendTime;

        // 2013-12-29: GreenRose
        public string strRoomID = "";

        public PresentHistoryInfo()
        {
            _InfoType = InfoType.PresentHistory;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(SendId);
            size += EncodeCount(ReceiveId);
            size += EncodeCount(Cash);
            size += EncodeCount(Descripiton);
            size += EncodeCount(ConvDateToString(SendTime));

            size += EncodeCount(strRoomID);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, SendId );
                EncodeString(bw, ReceiveId );
                EncodeInteger(bw, Cash);
                EncodeString(bw, Descripiton);
                EncodeString(bw, ConvDateToString(SendTime));

                EncodeString(bw, strRoomID);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            SendId = DecodeString(br);
            ReceiveId = DecodeString(br);
            Cash = DecodeInteger(br);
            Descripiton = DecodeString(br);
            SendTime = Convert.ToDateTime(DecodeString(br).ToString());

            strRoomID = DecodeString(br);
        }
    }
}
