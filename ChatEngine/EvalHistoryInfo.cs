using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class EvalHistoryInfo : BaseInfo
    {
        public int Id = 0;
        public string OwnId = "";
        public string BuyerId = "";
        public int Value = 0;
        public DateTime EvalTime;

        public EvalHistoryInfo()
        {
            _InfoType = InfoType.EvalHistory;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount(OwnId) + EncodeCount(BuyerId) + EncodeCount(Value) + EncodeCount( ConvDateToString( EvalTime ));
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, OwnId);
                EncodeString(bw, BuyerId);
                EncodeInteger(bw, Value);
                EncodeString(bw, ConvDateToString( EvalTime ));
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            OwnId = DecodeString(br);
            BuyerId = DecodeString(br);
            Value = DecodeInteger(br);
            EvalTime = Convert.ToDateTime(DecodeString(br).ToString());
        }
    }
}
