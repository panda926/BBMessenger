using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public enum PointKind
    {
        Bonus = 0,
        Chat = 1,
        Game = 2
    }

    public class PointHistoryInfo : BaseInfo
    {
        public int Id = 0;
        public string TargetId = "";
        public int Point = 0;
        //public string AgreeId = "";
        public DateTime AgreeTime;
        public string Content;
        public int Kind;

        public PointHistoryInfo()
        {
            _InfoType = InfoType.PointHistory;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount(TargetId) + EncodeCount(Point) + EncodeCount(ConvDateToString(AgreeTime)) + EncodeCount(Content);
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, TargetId);
                EncodeInteger(bw, Point);
                EncodeString(bw, ConvDateToString(AgreeTime));
                EncodeString(bw, Content);
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
            Point = DecodeInteger(br);
            AgreeTime = Convert.ToDateTime(DecodeString(br).ToString());
            Content = DecodeString(br);
        }
    }
}
