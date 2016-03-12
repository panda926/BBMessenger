using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public enum PlayState
    {
        Ready = 0,
        Start = 1,
        Playing = 2,
        Stop = 3
    }

    public class VoiceInfo : BaseInfo
    {
        public string UserId = "";
        public int PlayState = 0;

        public List<byte[]> Frames = new List<byte[]>();

        public VoiceInfo()
        {
            _InfoType = InfoType.Voice;
        }


        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(UserId);
            size += EncodeCount(PlayState);
            
            size += 4;

            for( int i = 0; i < Frames.Count; i++ )
            {
                size += 4;
                size += Frames[i].Length;
            }

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, UserId );
                EncodeInteger(bw, PlayState);

                EncodeInteger( bw, Frames.Count);

                for (int i = 0; i < Frames.Count; i++)
                {
                    EncodeInteger(bw, Frames[i].Length);
                    bw.Write(Frames[i]);
                }
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            UserId = DecodeString(br );
            PlayState = DecodeInteger(br);

            int frameCount = DecodeInteger(br);

            for (int i = 0; i < frameCount; i++)
            {
                int length = DecodeInteger(br);

                byte[] frame = new byte[length];
                frame = br.ReadBytes(length);

                Frames.Add(frame);
            }
        }
    }
}
