using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class UserListInfo : BaseInfo
    {
        public List<UserInfo> _Users = new List<UserInfo>();

        public UserListInfo()
        {
            _InfoType = InfoType.UserList;
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4;

            for (int i = 0; i < _Users.Count; i++)
                size += _Users[i].GetSize();

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeInteger(bw, _Users.Count);

                for (int i = 0; i < _Users.Count; i++)
                    _Users[i].Body.GetBytes(bw);
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
                UserInfo userInfo = (UserInfo)BaseInfo.CreateInstance(br);
                _Users.Add(userInfo);
            }
        }
    }
}
