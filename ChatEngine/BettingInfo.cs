using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class BettingInfo : BaseInfo
    {
        public int _UserIndex = 0;
        public int _Area = 0;     
        public int _Score = 0;

        public bool _isGiveUp = false;

        public BettingInfo()
        {
            _InfoType = InfoType.Betting;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount( _Area ) + EncodeCount( _Score ) + EncodeCount( _UserIndex );
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger( bw, _Area );
                EncodeInteger( bw, _Score );
                EncodeInteger(bw, _UserIndex);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            _Area = DecodeInteger( br );
            _Score = DecodeInteger( br );
            _UserIndex = DecodeInteger(br);
        }
    }
}
