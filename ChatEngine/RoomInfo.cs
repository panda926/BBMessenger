using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ChatEngine
{
    public class RoomInfo : BaseInfo
    {
        public string Id = "";         // 20
        public string Name = "";       // 20
        public int Kind;               // 4
        public string Icon = "";       // 50
        
        public string Owner = "";      // 20
        public int UserCount = 0;

        public int Cash = 0;
        public int Point = 0;
        public int MaxUsers = 0;
        //public int Total = 0;
        public string RoomPass = "";

        public int IsGame = 0;
        public GameInfo _GameInfo = null;

        //public List<UserInfo> listUserInfo = new List<UserInfo>();
        //public List<string> listUserID = new List<string>();

        public string _strUserID = "";
        public string _strTargetID = "";

        public RoomInfo()
        {
            _InfoType = InfoType.Room;
        }

        public string KindString
        {
            get
            {
                string str = "";

                switch (Kind)
                {
                    case 1:
                        str = "연예";
                        break;
                }

                return str;
            }
        }

        override public int GetSize()
        {
            int size = base.GetSize();

            size += EncodeCount( Id );
            size += EncodeCount( Name );
            size += EncodeCount( Kind );
            size += EncodeCount( Icon );
            size += EncodeCount( Owner );
            size += EncodeCount( UserCount );
            size += EncodeCount(Cash);
            size += EncodeCount(Point);
            size += EncodeCount(MaxUsers);
            size += EncodeCount(IsGame);
            size += EncodeCount(RoomPass);

            if (IsGame > 0)
                size += _GameInfo.GetSize();

            size += EncodeCount(_strUserID);
            size += EncodeCount(_strTargetID);

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);

                EncodeString(bw, Id );
                EncodeString(bw, Name );
                EncodeInteger(bw, Kind);
                EncodeString(bw, Icon );
                EncodeString(bw, Owner );
                EncodeInteger(bw, UserCount);
                EncodeInteger(bw, Cash);
                EncodeInteger(bw, Point);
                EncodeInteger(bw, MaxUsers);
                EncodeInteger(bw, IsGame);
                EncodeString(bw, RoomPass);

                if (IsGame > 0)
                    _GameInfo.GetBytes(bw);

                EncodeString(bw, _strUserID);
                EncodeString(bw, _strTargetID);
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
            Kind = DecodeInteger(br);
            Icon = DecodeString(br );
            Owner = DecodeString(br );
            UserCount = DecodeInteger(br);
            Cash = DecodeInteger(br);
            Point = DecodeInteger(br);
            MaxUsers = DecodeInteger(br);
            IsGame = DecodeInteger(br);
            RoomPass = DecodeString(br);

            if (IsGame > 0)
                _GameInfo = (GameInfo)BaseInfo.CreateInstance(br);

            _strUserID = DecodeString(br);
            _strTargetID = DecodeString(br);
        }

        public RoomInfo Body
        {
            get
            {
                return this;
            }
            set
            {
                Name = value.Name;
                UserCount = value.UserCount;
                Cash = value.Cash;
                Point = value.Point;
                MaxUsers = value.MaxUsers;
                RoomPass = value.RoomPass;
            }
        }
    }
}
