using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public enum MusiceKind
    {
        Play = 0,
        Pause = 1,
        Stop = 2
    }

    public class MusiceStateInfo : BaseInfo
    {
        
        public string MusiceName = "";       // 20
        public int M_Kind = 0;
        static public string[] M_KindList = { "Paly", "Pause", "Stop" };

        public MusiceStateInfo()
        {
            _InfoType = InfoType.MusiceState;
        }

        public string KindString
        {
            get
            {
                return M_KindList[M_Kind];
            }
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount(MusiceName) + EncodeCount(M_Kind);
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, MusiceName);
                EncodeInteger(bw, M_Kind);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            MusiceName = DecodeString(br);
            M_Kind = DecodeInteger(br);
        }
    }
}
