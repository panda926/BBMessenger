using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ChatEngine
{
    public class RoomListInfo : BaseInfo
    {
        private List<RoomInfo> _Rooms = new List<RoomInfo>();

        public RoomListInfo()
        {
            _InfoType = InfoType.RoomList;
        }

        public List<RoomInfo> Rooms
        {
            get
            {
                return _Rooms;
            }
            set
            {
                _Rooms = value;
            }
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4;

            for (int i = 0; i < _Rooms.Count; i++)
            {
                size += _Rooms[i].GetSize();
            }

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);
                EncodeInteger(bw, _Rooms.Count);

                for (int i = 0; i < _Rooms.Count; i++)
                    _Rooms[i].GetBytes(bw);
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
                RoomInfo roomInfo = new RoomInfo();
                DecodeInteger(br);

                roomInfo.FromBytes(br);

                _Rooms.Add(roomInfo);
            }
        }
    }
}
