using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class GiveInfo : BaseInfo
    {
        private string _UserId = "";     
        private string _PresentId = "";  

        public GiveInfo()
        {
            _InfoType = InfoType.Give;
        }

        public string UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value;
            }
        }

        public string PresentId
        {
            get
            {
                return _PresentId;
            }
            set
            {
                _PresentId = value;
            }
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount( _UserId ) + EncodeCount( _PresentId );
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString( bw, _UserId );
                EncodeString( bw, _PresentId );
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            _UserId = DecodeString( br );
            _PresentId = DecodeString( br );
        }
    }
}
