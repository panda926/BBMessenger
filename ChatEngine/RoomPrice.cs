using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class RoomPrice : BaseInfo
    {
        public string RoomId = "";         // 20
        public string UserId = "";         // 20
        public string ReceiveId = "";
        public int RoomValue = 0;       // 20


        public RoomPrice()
        {
            _InfoType = InfoType.RoomValue;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(RoomId);
            size += EncodeCount(UserId);
            size += EncodeCount(ReceiveId);
            size += EncodeCount(RoomValue);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, RoomId);
                EncodeString(bw, UserId);
                EncodeString(bw, ReceiveId);
                EncodeInteger(bw, RoomValue);

            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            RoomId = DecodeString(br);
            UserId = DecodeString(br);
            ReceiveId = DecodeString(br);
            RoomValue = DecodeInteger(br);
            

        }
        
    }
}
