using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ChatEngine
{
    public class RoomDetailInfo : BaseInfo
    {
        public List<UserInfo> Users = new List<UserInfo>();
        public List<IconInfo> Emoticons = new List<IconInfo>();
        public List<IconInfo> Presents = new List<IconInfo>();

        public string strRoomID = string.Empty;

        public RoomDetailInfo()
        {
            _InfoType = InfoType.RoomDetail;
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4 + 4 + 4;

            for (int i = 0; i < Users.Count; i++)
            {
                size += Users[i].GetSize();
            }

            for (int i = 0; i < Emoticons.Count; i++)
            {
                size += Emoticons[i].GetSize();
            }

            for (int i = 0; i < Presents.Count; i++)
            {
                size += Presents[i].GetSize();
            }

            size += EncodeCount(strRoomID);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);
                
                EncodeInteger(bw, Users.Count);
                EncodeInteger(bw, Emoticons.Count);
                EncodeInteger(bw, Presents.Count);

                for (int i = 0; i < Users.Count; i++)
                    Users[i].Body.GetBytes(bw);

                for (int i = 0; i < Emoticons.Count; i++)
                    Emoticons[i].GetBytes(bw);

                for (int i = 0; i < Presents.Count; i++)
                    Presents[i].GetBytes(bw);

                EncodeString(bw, strRoomID);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            int userCount = DecodeInteger(br);
            int emoticonCount = DecodeInteger(br);
            int presentCount = DecodeInteger(br);

            for (int i = 0; i < userCount; i++)
            {
                UserInfo userInfo = (UserInfo)BaseInfo.CreateInstance(br);
                Users.Add(userInfo);
            }

            for (int i = 0; i < emoticonCount; i++)
            {
                IconInfo presentInfo = (IconInfo)BaseInfo.CreateInstance(br);
                Emoticons.Add(presentInfo);
            }

            for (int i = 0; i < presentCount; i++)
            {
                IconInfo presentInfo = (IconInfo)BaseInfo.CreateInstance(br);
                Presents.Add(presentInfo);
            }

            strRoomID = DecodeString(br);
        }
    }
}
