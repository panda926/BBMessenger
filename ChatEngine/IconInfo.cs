using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class IconInfo : BaseInfo
    {
        public string Id = "";         // 20
        public string Name = "";       // 20
        
        public string Icon = "";       // 50
        public int Point = 0;          // 4


        public IconInfo()
        {
            _InfoType = InfoType.Present;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount( Id ) + EncodeCount( Name ) + EncodeCount( Icon ) + EncodeCount( Point );
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, Id );
                EncodeString(bw, Name );
                EncodeString(bw, Icon );
                EncodeInteger(bw, Point);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            Id = DecodeString(br );
            Name = DecodeString(br );
            Icon = DecodeString(br );
            Point = DecodeInteger(br);
        }
    }
}
