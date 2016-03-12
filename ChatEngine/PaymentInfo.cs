using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatEngine
{
    public class PaymentInfo : BaseInfo
    {
        public string strID = "";
        public string strAccountID = "";
        public string strAccountNumber = "";
        public int nPaymentMoney = 0;

        public PaymentInfo()
        {
            _InfoType = InfoType.Payment;
        }

        public override int GetSize()
        {
            int nSize = base.GetSize();

            nSize += EncodeCount(strID);
            nSize += EncodeCount(strAccountID);
            nSize += EncodeCount(strAccountNumber);
            nSize += EncodeCount(nPaymentMoney);

            return nSize;
        }

        public override void GetBytes(System.IO.BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, strID);
                EncodeString(bw, strAccountID);
                EncodeString(bw, strAccountNumber);
                EncodeInteger(bw, nPaymentMoney);
            }
            catch (System.Exception)
            {
            }
        }

        public override void FromBytes(System.IO.BinaryReader br)
        {
            base.FromBytes(br);

            strID = DecodeString(br);
            strAccountID = DecodeString(br);
            strAccountNumber = DecodeString(br);
            nPaymentMoney = DecodeInteger(br);
        }
    }
}
