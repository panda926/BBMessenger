using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class AskChatInfo : BaseInfo
    {
        public string TargetId = "";
        public string AskContent = "";
        public int Price = 0;
        
        public int Agree = 0;
        public string AskingID = "";
        public RoomInfo MeetingInfo = new RoomInfo();

        public AskChatInfo()
        {
            _InfoType = InfoType.AskChat;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount( TargetId ) + EncodeCount( AskContent ) + EncodeCount( Price ) + EncodeCount( Agree ) + MeetingInfo.GetSize();
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString( bw, TargetId );
                EncodeString(bw, AskContent );
                EncodeInteger(bw, Price);
                EncodeInteger(bw, Agree);
                EncodeString(bw, AskingID);
                
                MeetingInfo.GetBytes(bw);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            TargetId = DecodeString(br);
            AskContent = DecodeString(br);
            Price = DecodeInteger(br);
            
            Agree = DecodeInteger(br);
            AskingID = DecodeString(br);
            DecodeInteger(br);
            MeetingInfo.FromBytes(br);
        }
    }
}
