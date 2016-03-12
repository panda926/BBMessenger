using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class MusiceInfo : BaseInfo
    {
        public byte[] MusiceData = null;
        public string MusiceName = "";    // 20

        public MusiceInfo()
        {
            _InfoType = InfoType.Musice;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount(MusiceName) + 4 + MusiceData.Length;
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, MusiceName);
                EncodeInteger(bw, MusiceData.Length);
                bw.Write(MusiceData);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            MusiceName = DecodeString(br);
            int length = DecodeInteger(br);
            MusiceData = new byte[length];
            MusiceData = br.ReadBytes(length);
        }
    }
}
