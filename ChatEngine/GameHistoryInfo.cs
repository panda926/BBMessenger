using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class GameHistoryInfo : BaseInfo
    {
        public int Id = 0;

        public string GameId = "";
        public string BuyerId = "";
        public string OfficerId = string.Empty;
        public string ManagerId = string.Empty;

        public string RecommenderId = "";
        public DateTime StartTime;
        public DateTime EndTime;
        public int BuyerTotal = 0;
        public int RecommenderTotal = 0;
        public int RecommendOfficerTotal = 0;
        public int ManagerTotal = 0;
        
        // added newly
        public int AdminTotal = 0;

        public string GameSource = "";

        public GameHistoryInfo()
        {
            _InfoType = InfoType.GameHistory;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(GameId);
            size += EncodeCount(GameSource);
            size += EncodeCount(BuyerId);
            size += EncodeCount(RecommenderId);
            size += EncodeCount(ConvDateToString(StartTime));
            size += EncodeCount(ConvDateToString(EndTime));
            size += EncodeCount(BuyerTotal);
            size += EncodeCount(RecommenderTotal);
            size += EncodeCount(RecommendOfficerTotal);
            size += EncodeCount(ManagerTotal);
            size += EncodeCount(AdminTotal);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, GameId );
                EncodeString(bw, GameSource);
                EncodeString(bw, BuyerId);
                EncodeString(bw, RecommenderId);
                EncodeString(bw, ConvDateToString(StartTime));
                EncodeString(bw, ConvDateToString(EndTime));
                EncodeInteger(bw, BuyerTotal);
                EncodeInteger(bw, RecommenderTotal);
                EncodeInteger(bw, RecommendOfficerTotal);
                EncodeInteger(bw, ManagerTotal);
                EncodeInteger(bw, AdminTotal);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            GameId = DecodeString(br);
            GameSource = DecodeString(br);
            BuyerId = DecodeString(br);
            RecommenderId = DecodeString(br);
            StartTime = Convert.ToDateTime(DecodeString(br).ToString());
            EndTime = Convert.ToDateTime(DecodeString(br).ToString());
            BuyerTotal = DecodeInteger(br);
            RecommenderTotal = DecodeInteger(br);
            RecommendOfficerTotal = DecodeInteger(br);
            ManagerTotal = DecodeInteger(br);
            AdminTotal = DecodeInteger(br);
        }
    }
}
