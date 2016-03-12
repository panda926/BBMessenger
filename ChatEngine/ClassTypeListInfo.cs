using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class ClassTypeListInfo : BaseInfo
    {
        public List<ClassTypeInfo> _ClassType = new List<ClassTypeInfo>();

        public ClassTypeListInfo()
        {
            _InfoType = InfoType.ClassTypeList;
        }

        public List<ClassTypeInfo> ClassType
        {
            get
            {
                return _ClassType;
            }
            set
            {
                _ClassType = value;
            }
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4;

            for (int i = 0; i < _ClassType.Count; i++)
                size += _ClassType[i].GetSize();

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, _ClassType.Count);

                for (int i = 0; i < _ClassType.Count; i++)
                    _ClassType[i].GetBytes(bw);
            }
            catch (Exception ex)
            {
                // Error Handling 
            }
           
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            int count = DecodeInteger(br);

            for (int i = 0; i < count; i++)
            {
                ClassTypeInfo classTypeInfo = new ClassTypeInfo();
                DecodeInteger(br);

                classTypeInfo.FromBytes(br);

                _ClassType.Add(classTypeInfo);
            }
        }
    }
}
