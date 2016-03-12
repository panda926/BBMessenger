using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class ClassInfo : BaseInfo
    {
        public int Class_Type_Id = 0;    // 20
        public string Class_Name = "";
        public string Class_Type_Name = "";
        public string Class_Img_Uri = "";
        public int ToIndex = 0;    // 20
        public int FromIndex = 0;    // 20
        public int ClassInfo_Id = 0;    // 20
        public int ClassCount = 0;    // 20

        public ClassInfo()
        {
            _InfoType = InfoType.ClassInfo;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount(Class_Type_Id) + EncodeCount(Class_Name) + EncodeCount(Class_Type_Name) +
                EncodeCount(Class_Img_Uri) + EncodeCount(ToIndex) + EncodeCount(FromIndex) + EncodeCount(ClassInfo_Id) + EncodeCount(ClassCount);
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, Class_Type_Id);
                EncodeString(bw, Class_Name);
                EncodeString(bw, Class_Type_Name);
                EncodeString(bw, Class_Img_Uri);
                EncodeInteger(bw, ToIndex);
                EncodeInteger(bw, FromIndex);
                EncodeInteger(bw, ClassInfo_Id);
                EncodeInteger(bw, ClassCount);
              
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            Class_Type_Id = DecodeInteger(br);
            Class_Name = DecodeString(br);
            Class_Type_Name = DecodeString(br);
            Class_Img_Uri = DecodeString(br);
            ToIndex = DecodeInteger(br);
            FromIndex = DecodeInteger(br);
            ClassInfo_Id = DecodeInteger(br);
            ClassCount = DecodeInteger(br);

        }
        
    }
}
