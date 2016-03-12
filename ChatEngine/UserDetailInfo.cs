using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class UserDetailInfo : BaseInfo
    {
        public List<IconInfo> Faces = new List<IconInfo>();
        public List<RoomInfo> Rooms = new List<RoomInfo>();

        public List<UserInfo> Partners = new List<UserInfo>();
        public List<EvalHistoryInfo> EvalHistories = new List<EvalHistoryInfo>();
        
        public List<ChatHistoryInfo> ChatHistories = new List<ChatHistoryInfo>();
        public List<ChargeHistoryInfo> ChargeHistories = new List<ChargeHistoryInfo>();
        public List<GameHistoryInfo> GameHistories = new List<GameHistoryInfo>();
        public List<PresentHistoryInfo> PresentHistories = new List<PresentHistoryInfo>();

        public string _strDownUrl = string.Empty;
        

        public UserDetailInfo()
        {
            _InfoType = InfoType.UserDetail;
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4;

            for (int i = 0; i < Faces.Count; i++)
                size += Faces[i].GetSize();

            for( int i = 0; i < Rooms.Count; i++ )
                size += Rooms[i].GetSize();

            for (int i = 0; i < Partners.Count; i++)
                size += Partners[i].GetSize();

            for (int i = 0; i < EvalHistories.Count; i++)
                size += EvalHistories[i].GetSize();

            for (int i = 0; i < ChatHistories.Count; i++)
                size += ChatHistories[i].GetSize();

            for (int i = 0; i < ChargeHistories.Count; i++)
                size += ChargeHistories[i].GetSize();

            for (int i = 0; i < GameHistories.Count; i++)
                size += GameHistories[i].GetSize();

            for (int i = 0; i < PresentHistories.Count; i++)
                size += PresentHistories[i].GetSize();

            size += EncodeCount(_strDownUrl);
            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, Faces.Count);
                EncodeInteger(bw, Rooms.Count);
                EncodeInteger(bw, Partners.Count);
                EncodeInteger(bw, EvalHistories.Count);
                EncodeInteger(bw, ChatHistories.Count);
                EncodeInteger(bw, ChargeHistories.Count);
                EncodeInteger(bw, GameHistories.Count);
                EncodeInteger(bw, PresentHistories.Count);
                
                

                for (int i = 0; i < Faces.Count; i++)
                    Faces[i].GetBytes(bw);

                for( int i = 0; i < Rooms.Count; i++ )
                    Rooms[i].GetBytes(bw);

                for (int i = 0; i < Partners.Count; i++)
                    Partners[i].Body.GetBytes(bw);

                for (int i = 0; i < EvalHistories.Count; i++)
                    EvalHistories[i].GetBytes(bw);

                for (int i = 0; i < ChatHistories.Count; i++)
                    ChatHistories[i].GetBytes(bw);

                for (int i = 0; i < ChargeHistories.Count; i++)
                    ChargeHistories[i].GetBytes(bw);

                for (int i = 0; i < GameHistories.Count; i++)
                    GameHistories[i].GetBytes(bw);

                for (int i = 0; i < PresentHistories.Count; i++)
                    PresentHistories[i].GetBytes(bw);


                EncodeString(bw, _strDownUrl);

            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            int faceCount = DecodeInteger(br);
            int roomCount = DecodeInteger(br);
            int partnerCount = DecodeInteger(br);
            int evalHistoryCount = DecodeInteger(br);
            int chatHistoryCount = DecodeInteger(br);
            int chargeHistoryCount = DecodeInteger(br);
            int gameHistoryCount = DecodeInteger(br);
            int presentHistoryCount = DecodeInteger(br);



            for (int i = 0; i < faceCount; i++)
            {
                IconInfo faceInfo = (IconInfo)BaseInfo.CreateInstance(br);
                Faces.Add(faceInfo);
            }

            for (int i = 0; i < roomCount; i++)
            {
                RoomInfo roomInfo = (RoomInfo)BaseInfo.CreateInstance(br);
                Rooms.Add(roomInfo);
            }

            for (int i = 0; i < partnerCount; i++)
            {
                UserInfo partnerInfo = (UserInfo)BaseInfo.CreateInstance(br);
                Partners.Add(partnerInfo);
            }

            for (int i = 0; i < evalHistoryCount; i++)
            {
                EvalHistoryInfo evalHistoryInfo = (EvalHistoryInfo)BaseInfo.CreateInstance(br);
                EvalHistories.Add(evalHistoryInfo);
            }

            for (int i = 0; i < chatHistoryCount; i++)
            {
                ChatHistoryInfo chatHistoryInfo = (ChatHistoryInfo)BaseInfo.CreateInstance(br);
                ChatHistories.Add(chatHistoryInfo);
            }

            for (int i = 0; i < chargeHistoryCount; i++)
            {
                ChargeHistoryInfo chargeHistoryInfo = (ChargeHistoryInfo)BaseInfo.CreateInstance(br);
                ChargeHistories.Add(chargeHistoryInfo);
            }

            for (int i = 0; i < gameHistoryCount; i++)
            {
                GameHistoryInfo gameHistoryInfo = (GameHistoryInfo)BaseInfo.CreateInstance(br);
                GameHistories.Add(gameHistoryInfo);
            }

            for (int i = 0; i < presentHistoryCount; i++)
            {
                PresentHistoryInfo presentHistoryInfo = (PresentHistoryInfo)BaseInfo.CreateInstance(br);
                PresentHistories.Add(presentHistoryInfo);
            }



            _strDownUrl = DecodeString(br);
        }
    }
}
