using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public static class GameSource
    {
        public const string Sicbo = "Sicbo";
        public const string Dice = "Dice";
        public const string DzCard = "DzCard";
        public const string Horse = "Horse";
        public const string BumperCar = "BumperCar";
        public const string Fish = "Fish";
    }

    public class GameInfo : BaseInfo
    {
        public string GameId = "";         // 20
        public string GameName = "";       // 20

        public int Width = 0;              // 4
        public int Height = 0;             // 4

        public string Icon = "";           // 50
        public string Source = "";         // 50

        public int UserCount = 0;          // 4
        public int Bank = 0;

        public int Commission = 0;

        // 2013-12-18: GreenRose
        public int nCashOrPointGame = -1;    // 캐쉬게임인가 포인트게임인가를 결정한다. 0이면 캐쉬 1이면 포인트게임.

        public string Downloadfolder = "";
        public string RunFile = "";

        public GameInfo()
        {
            _InfoType = InfoType.Game;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount( GameId );
            size += EncodeCount(GameName);
            size += EncodeCount(Width);
            size += EncodeCount( Height );
            size += EncodeCount(Icon);
            size += EncodeCount(Source);
            size += EncodeCount( UserCount);

            // added by usc at 2014/04/10
            size += EncodeCount(Bank);
            size += EncodeCount(Commission);

            size += EncodeCount(nCashOrPointGame);
            size += EncodeCount(Downloadfolder);
            size += EncodeCount(RunFile);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, GameId );
                EncodeString(bw, GameName );
                EncodeInteger(bw, Width);
                EncodeInteger(bw, Height);
                EncodeString(bw, Icon );
                EncodeString(bw, Source );
                EncodeInteger(bw, UserCount);
                
                // added newly
                EncodeInteger(bw, Bank);
                EncodeInteger(bw, Commission);

                EncodeInteger(bw, nCashOrPointGame);
                EncodeString(bw, Downloadfolder);
                EncodeString(bw, RunFile);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            GameId = DecodeString(br );
            GameName = DecodeString(br );
            Width = DecodeInteger(br);
            Height = DecodeInteger(br);
            Icon = DecodeString(br );
            Source = DecodeString(br );
            UserCount = DecodeInteger(br);

            // added newly
            Bank = DecodeInteger(br);
            Commission = DecodeInteger(br);

            nCashOrPointGame = DecodeInteger(br);
            Downloadfolder = DecodeString(br);
            RunFile = DecodeString(br);
        }
    }
}
