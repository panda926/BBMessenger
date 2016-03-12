using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ChatEngine
{
    public class GameDetailInfo : BaseInfo
    {
        private string _Properties = "";

        public GameDetailInfo()
        {
            _InfoType = InfoType.GameDetail;
        }

        public string Properties
        {
            get
            {
                return _Properties;
            }
            set
            {
                _Properties = value;
            }
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount( _Properties );
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, _Properties );
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            _Properties = DecodeString( br );
        }
    }
}
