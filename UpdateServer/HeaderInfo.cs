using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UpdateServer
{
    class HeaderInfo : BaseInfo
    {
        public const string Marker = "QAZWSXEDCRFVTGBHNUJMIK<OL>";

        public string _Header = Marker;
        public int _BodySize = 0;

        public HeaderInfo()
        {
            _InfoType = InfoType.Header;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount( _Header ) + EncodeCount( _BodySize );
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, _Header );
                EncodeInteger(bw, _BodySize );
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            _Header = DecodeString(br );
            _BodySize = DecodeInteger(br);
        }
    }
}
