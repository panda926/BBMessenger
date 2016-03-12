using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ChatEngine
{
    public class StringInfo : BaseInfo
    {
        public string UserId = "";   // 20
        public string String = "";

        public int FontSize = 0;
        public string FontName = "";
        public string FontStyle = "";
        public string FontWeight = "";
        public string FontColor = "";

        // 2013-12-25: GreenRose
        public string strRoomID = "";

        public StringInfo()
        {
            _InfoType = InfoType.String;
        }

        override public int GetSize()
        {
            int size = base.GetSize();
            
            size += EncodeCount(UserId);
            size += EncodeCount(String);
            size += EncodeCount( FontSize );
            size += EncodeCount( FontName );
            size += EncodeCount( FontStyle );
            size += EncodeCount( FontWeight );
            size += EncodeCount( FontColor );
            size += EncodeCount(strRoomID);

            return size;
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString( bw, UserId );
                EncodeString(bw, String );
                EncodeInteger( bw, FontSize );
                EncodeString( bw, FontName );
                EncodeString( bw, FontStyle );
                EncodeString( bw, FontWeight );
                EncodeString( bw, FontColor );
                EncodeString(bw, strRoomID);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            UserId = DecodeString( br );
            String = DecodeString( br );
            FontSize = DecodeInteger( br );
            FontName = DecodeString(br);;
            FontStyle = DecodeString(br);
            FontWeight = DecodeString(br);
            FontColor = DecodeString(br);
            strRoomID = DecodeString(br);

        }
    }
}
