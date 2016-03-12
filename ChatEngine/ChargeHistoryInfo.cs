using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;
using System.IO;


namespace ChatEngine
{
    public enum ChargeKind
    {
        Charge = 0,
        Commission = 1
    }

    public enum CompleteKind
    {
        Request = 0,
        Agree = 1,
        Cancel = 2
    }

    public class ChargeHistoryInfo : BaseInfo
    {
        public int Id = 0;
        public int Kind = 0;
        public int Cash = 0;
        public DateTime StartTime;
        public DateTime EndTime;
        public string OwnId = "";
        public string ApproveId = "";
        public int Complete = 0;
        public string BankAccount = "";

        private string[] _KindList = { "충전", "환전" };
        private string[] _CompleteList = { "신청", "승인", "취소" };

        public ChargeHistoryInfo()
        {
            _InfoType = InfoType.ChargeHistory;
        }

        public string KindString
        {
            get
            {
                return _KindList[Kind];
            }
        }

        public string CompleteString
        {
            get
            {
                return _CompleteList[Complete];
            }
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(Kind);
            size += EncodeCount(Cash);
            size += EncodeCount(ConvDateToString(StartTime));
            size += EncodeCount(ConvDateToString(EndTime));
            size += EncodeCount(OwnId);
            size += EncodeCount(ApproveId);
            size += EncodeCount(Complete);
            size += EncodeCount(BankAccount);

            return size;
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, Kind);
                EncodeInteger(bw, Cash);
                EncodeString(bw, ConvDateToString( StartTime ));
                EncodeString(bw, ConvDateToString( EndTime ));
                EncodeString(bw, OwnId);
                EncodeString(bw, ApproveId);
                EncodeInteger(bw, Complete);
                EncodeString(bw, BankAccount);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            Kind = DecodeInteger(br);
            Cash = DecodeInteger(br);
            StartTime = Convert.ToDateTime(DecodeString(br).ToString());
            EndTime = Convert.ToDateTime(DecodeString(br).ToString());
            OwnId = DecodeString(br);
            ApproveId = DecodeString(br);
            Complete = DecodeInteger(br);
            BankAccount = DecodeString(br);
        }
    }
}
