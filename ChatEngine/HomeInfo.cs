using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class HomeInfo : BaseInfo
    {
        public List<UserInfo> Users = new List<UserInfo>();
        public List<GameInfo> Games = new List<GameInfo>();

        public List<BoardInfo> Letters = new List<BoardInfo>();
        public List<BoardInfo> Notices = new List<BoardInfo>();

        public HomeInfo()
        {
            _InfoType = InfoType.Home;
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4 + 4 + 4 + 4;

            for (int i = 0; i < Users.Count; i++)
            {
                size += Users[i].GetSize();
            }

            for (int i = 0; i < Games.Count; i++)
            {
                size += Games[i].GetSize();
            }

            for (int i = 0; i < Letters.Count; i++)
            {
                size += Letters[i].GetSize();
            }

            for (int i = 0; i < Notices.Count; i++)
            {
                size += Notices[i].GetSize();
            }

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, Users.Count);
                EncodeInteger(bw, Games.Count);
                EncodeInteger(bw, Letters.Count);
                EncodeInteger(bw, Notices.Count);

                for (int i = 0; i < Users.Count; i++)
                    Users[i].Body.GetBytes(bw);

                for (int i = 0; i < Games.Count; i++)
                    Games[i].GetBytes(bw);

                for (int i = 0; i < Letters.Count; i++)
                    Letters[i].GetBytes(bw);

                for (int i = 0; i < Notices.Count; i++)
                    Notices[i].GetBytes(bw);
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
            int gameCount = DecodeInteger(br);
            int boardCount = DecodeInteger(br);
            int noticeCount = DecodeInteger(br);

            for (int i = 0; i < userCount; i++)
            {
                UserInfo userInfo = new UserInfo();
                DecodeInteger(br);

                userInfo.FromBytes(br);
                Users.Add(userInfo);
            }

            for (int i = 0; i < gameCount; i++)
            {
                GameInfo gameInfo = new GameInfo();
                DecodeInteger(br);

                gameInfo.FromBytes(br);
                Games.Add(gameInfo);
            }

            for (int i = 0; i < boardCount; i++)
            {
                BoardInfo boardInfo = new BoardInfo();
                DecodeInteger(br);

                boardInfo.FromBytes(br);
                Letters.Add(boardInfo);
            }

            for (int i = 0; i < noticeCount; i++)
            {
                BoardInfo noticeInfo = new BoardInfo();
                DecodeInteger(br);

                noticeInfo.FromBytes(br);
                Notices.Add(noticeInfo);
            }
        }
    }
}
