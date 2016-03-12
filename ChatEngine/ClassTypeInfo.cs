using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class ClassTypeInfo : BaseInfo
    {
        public int Class_File_Id = 0;    // 20
        public string Class_File_Name = "";
        public DateTime Class_File_Date;
        public int Class_File_Type = 0;
        public int Class_File_Area = 0;
        public string Class_Video_Title = "";
        public string Class_Img_Folder = "";
        public int Class_File_Count = 0;
        public int Class_Row_Number = 0;

        public ClassTypeInfo()
        {
            _InfoType = InfoType.ClassTypeInfo;
        }

        override public int GetSize()
        {
            return base.GetSize() + EncodeCount(Class_File_Id) + EncodeCount(Class_File_Name) + EncodeCount(ConvDateToString(Class_File_Date)) + 
                EncodeCount(Class_File_Type) + EncodeCount(Class_File_Area) +
                EncodeCount(Class_Video_Title) + EncodeCount(Class_Img_Folder) + EncodeCount(Class_File_Count) + EncodeCount(Class_Row_Number);
        }

        override public void GetBytes( BinaryWriter bw )
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, Class_File_Id);
                EncodeString(bw, Class_File_Name);
                EncodeString(bw, ConvDateToString(Class_File_Date));
                EncodeInteger(bw, Class_File_Type);
                EncodeInteger(bw, Class_File_Area);
                EncodeString(bw, Class_Video_Title);
                EncodeString(bw, Class_Img_Folder);
                EncodeInteger(bw, Class_File_Count);
                EncodeInteger(bw, Class_Row_Number);
              
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br )
        {
            base.FromBytes(br);

            Class_File_Id = DecodeInteger(br);
            Class_File_Name = DecodeString(br);
            Class_File_Date = Convert.ToDateTime(DecodeString(br).ToString());
            Class_File_Type = DecodeInteger(br);
            Class_File_Area = DecodeInteger(br);
            Class_Video_Title = DecodeString(br);
            Class_Img_Folder = DecodeString(br);
            Class_File_Count = DecodeInteger(br);
            Class_Row_Number = DecodeInteger(br);

        }
        
    }
}
