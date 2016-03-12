using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public enum GameStatus
    {
        GS_EMPTY = 0,
        GS_JOIN,
        GS_BETTING,
        GS_END,

        GS_NONE = 1000
    }

    public class GameDefine
    {
        public const int GAME_PLAYER = 100;
        public const int INVALID_CHAIR = -1;

        public const int MAX_CHAIR = 100;
        public const int MAX_BETTING_MONEY = 20000;
    }

    public class TableInfo : BaseInfo
    {
        public string _TableId = "";     

        public int _RoundIndex = 0;

        public DateTime _RoundStartTime;
        public int _RoundDelayTime;

        public List<UserInfo> _Players = new List<UserInfo>();
        public List<UserInfo> _Viewers = new List<UserInfo>();

        public int m_lMinScore;
        public int[] m_lUserBetScore = new int[GameDefine.GAME_PLAYER];
        public int[] m_lUserWinScore = new int[GameDefine.GAME_PLAYER];

        public int m_ReadyTime;
        public int m_EndingTime;

        public int m_nCashOrPointGame = 0;


        public TableInfo()
        {
            _InfoType = InfoType.Table;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(_TableId);
            size += EncodeCount((int)_RoundIndex);
            size += EncodeCount(_RoundDelayTime);
            
            size += 4;  // player count

            for (int i = 0; i < _Players.Count; i++)
                size += _Players[i].GetSize();

            // added by usc at 2014/04/03
            size += 4;  // viewer count

            for (int i = 0; i < _Viewers.Count; i++)
                size += _Viewers[i].GetSize();

            size += EncodeCount(m_lMinScore);
            size += _Players.Count * 4;     // winer

            size += EncodeCount(m_nCashOrPointGame);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, _TableId);
                EncodeInteger(bw, _RoundIndex);
                EncodeInteger(bw, _RoundDelayTime);

                EncodeInteger(bw, _Players.Count);

                for (int i = 0; i < _Players.Count; i++)
                    _Players[i].GetBytes(bw);

                // added by usc at 2014/04/03
                EncodeInteger(bw, _Viewers.Count);

                for (int i = 0; i < _Viewers.Count; i++)
                    _Viewers[i].GetBytes(bw);

                EncodeInteger(bw, m_lMinScore);

                for (int i = 0; i < _Players.Count; i++)
                    EncodeInteger(bw, m_lUserWinScore[i]);

                EncodeInteger(bw, m_nCashOrPointGame);

            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            _TableId = DecodeString(br);
            _RoundIndex = DecodeInteger(br);
            _RoundDelayTime = DecodeInteger(br);

            int playerCount = DecodeInteger(br);

            for (int i = 0; i < playerCount; i++)
            {
                UserInfo userInfo = (UserInfo)BaseInfo.CreateInstance(br);
                _Players.Add(userInfo);
            }

            // added by usc at 2014/04/03
            int viewerCount = DecodeInteger(br);

            for (int i = 0; i < viewerCount; i++)
            {
                UserInfo userInfo = (UserInfo)BaseInfo.CreateInstance(br);
                _Viewers.Add(userInfo);
            }

            m_lMinScore = DecodeInteger(br);

            for (int i = 0; i < playerCount; i++)
                m_lUserWinScore[i] = DecodeInteger(br);

            m_nCashOrPointGame = DecodeInteger(br);

        }
    }
}
