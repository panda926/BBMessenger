using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class ClassListInfo : BaseInfo
    {
        public List<ClassInfo> _Classes = new List<ClassInfo>();

        public ClassListInfo()
        {
            _InfoType = InfoType.ClassList;
        }

        public List<ClassInfo> Classes
        {
            get
            {
                return _Classes;
            }
            set
            {
                _Classes = value;
            }
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4;

            for (int i = 0; i < _Classes.Count; i++)
                size += _Classes[i].GetSize();

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, _Classes.Count);

                for (int i = 0; i < _Classes.Count; i++)
                    _Classes[i].GetBytes(bw);
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
                ClassInfo classInfo = new ClassInfo();
                DecodeInteger(br);

                classInfo.FromBytes(br);

                _Classes.Add(classInfo);
            }
        }
    }
}
