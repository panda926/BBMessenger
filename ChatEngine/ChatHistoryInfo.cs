using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class ChatHistoryInfo : BaseInfo
    {
        public int Id = 0;

        public string BuyerId = "";
        public string ServicemanId = "";
        public string OfficerId = string.Empty;
        public string ManagerId = string.Empty;

        public int ServicePrice = 0;
        public DateTime StartTime;
        public DateTime EndTime;

        public int BuyerTotal = 0;
        public int ServicemanTotal = 0;
        public int ServiceOfficerTotal = 0;
        public int ManagerTotal = 0;

        // server
        public string RoomId = string.Empty;

        public ChatHistoryInfo()
        {
            _InfoType = InfoType.ChatHistory;
        }


        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(BuyerId);
            size += EncodeCount(ServicemanId);

            size += EncodeCount(ServicePrice);
            size += EncodeCount(ConvDateToLongString( StartTime ));
            size += EncodeCount(ConvDateToLongString( EndTime ));

            size += EncodeCount(BuyerTotal);
            size += EncodeCount(ServicemanTotal);
            size += EncodeCount(ServiceOfficerTotal);
            size += EncodeCount(ManagerTotal);

            return size;
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString( bw, BuyerId );
                EncodeString( bw, ServicemanId );

                EncodeInteger( bw, ServicePrice );
                EncodeString( bw, ConvDateToString( StartTime ));
                EncodeString( bw, ConvDateToString( EndTime ));

                EncodeInteger( bw, BuyerTotal );
                EncodeInteger( bw, ServicemanTotal );
                EncodeInteger(bw, ServiceOfficerTotal);
                EncodeInteger(bw, ManagerTotal);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            BuyerId = DecodeString( br );
            ServicemanId = DecodeString(br );

            ServicePrice = DecodeInteger(br);
            StartTime = Convert.ToDateTime( DecodeString(br).ToString());
            EndTime = Convert.ToDateTime(DecodeString(br).ToString());

            BuyerTotal = DecodeInteger(br);
            ServicemanTotal = DecodeInteger(br);
            ServiceOfficerTotal = DecodeInteger(br);
            ManagerTotal = DecodeInteger(br);
        }
    }
}
