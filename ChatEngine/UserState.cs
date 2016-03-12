using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class UserState : BaseInfo
    {
        public string _strUserID = string.Empty;
        public int _nUserState = 0;     // 0: Online, 1:Offline, 2: Busy, 3: GoAway

        public UserState()
        {
            _InfoType = InfoType.UserState;
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount(_strUserID);
            size += EncodeCount(_nUserState);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, _strUserID);
                EncodeInteger(bw, _nUserState);
            }
            catch (Exception)
            { }
        }

        override public void FromBytes(BinaryReader br)
        {
            base.FromBytes(br);

            _strUserID = DecodeString(br);
            _nUserState = DecodeInteger(br);
        }
    }
}
