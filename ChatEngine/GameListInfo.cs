using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ChatEngine
{
    public class GameListInfo : BaseInfo
    {
        private List<GameInfo> _Games = new List<GameInfo>();

        public GameListInfo()
        {
            _InfoType = InfoType.GameList;
        }

        public List<GameInfo> Games
        {
            get
            {
                return _Games;
            }
            set
            {
                _Games = value;
            }
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4;

            for (int i = 0; i < _Games.Count; i++)
            {
                size += _Games[i].GetSize();
            }

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);
                EncodeInteger(bw, _Games.Count);

                for (int i = 0; i < _Games.Count; i++)
                    _Games[i].GetBytes(bw);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            int count = DecodeInteger(br);

            for (int i = 0; i < count; i++)
            {
                GameInfo gameInfo = new GameInfo();
                DecodeInteger(br);

                gameInfo.FromBytes(br);

                _Games.Add(gameInfo);
            }
        }
    }
}
