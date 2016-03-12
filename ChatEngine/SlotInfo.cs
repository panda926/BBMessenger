using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class SlotInfo : BaseInfo
    {
        private int _LineCount = 1;             // 4
        private int _Bet = 0;                   // 4
        private int _FreespinCount = 0;         // 4

        private int _TotalWin = 0;              // 4
        private int[] _Wins = new int[9];       // 36


        public SlotInfo()
        {
            _InfoType = InfoType.Slot;
        }

        public int LineCount
        {
            get
            {
                return _LineCount;
            }
            set
            {
                _LineCount = value;
            }
        }

        public int Bet
        {
            get
            {
                return _Bet;
            }
            set
            {
                _Bet = value;
            }
        }

        public int FreespinCount
        {
            get
            {
                return _FreespinCount;
            }
            set
            {
                _FreespinCount = value;
            }
        }

        public int TotalWin
        {
            get
            {
                return _TotalWin;
            }
            set
            {
                _TotalWin = value;
            }
        }

        public int[] Wins
        {
            get
            {
                return _Wins;
            }
        }

        override public int GetSize()
        {
            return base.GetSize() + 52;
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, _LineCount);
                EncodeInteger(bw, _Bet);
                EncodeInteger(bw, _FreespinCount);
                EncodeInteger(bw, _TotalWin);

                for( int i = 0; i < 9; i++ )
                    EncodeInteger(bw, _Wins[i]);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            _LineCount = DecodeInteger(br);
            _Bet = DecodeInteger(br);
            _FreespinCount = DecodeInteger(br);
            _TotalWin = DecodeInteger(br);

            for (int i = 0; i < 9; i++)
                _Wins[i] = DecodeInteger(br);
        }

    }
}
