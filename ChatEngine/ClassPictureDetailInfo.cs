using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ChatEngine
{
    public class ClassPictureDetailInfo : BaseInfo
    {
        public List<ClassTypeInfo> ClassType = new List<ClassTypeInfo>();

        public ClassPictureDetailInfo()
        {
            _InfoType = InfoType.ClassPictureDetail;
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4;

            for (int i = 0; i < ClassType.Count; i++)
            {
                size += ClassType[i].GetSize();
            }

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, ClassType.Count);

                for (int i = 0; i < ClassType.Count; i++)
                    ClassType[i].GetBytes(bw);
               
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            int pictureCount = DecodeInteger(br);

            for (int i = 0; i < pictureCount; i++)
            {
                ClassTypeInfo classTypeInfo = (ClassTypeInfo)BaseInfo.CreateInstance(br);
                ClassType.Add(classTypeInfo);
            }

            
        }
    }
}
