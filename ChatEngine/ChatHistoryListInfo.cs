using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ChatEngine
{
    public class ChatHistoryListInfo : BaseInfo
    {
        public List<ChatHistoryInfo> _ChatHistories = new List<ChatHistoryInfo>();

        public ChatHistoryListInfo()
        {
            _InfoType = InfoType.RoomList;
        }

        override public int GetSize()
        {
            int size = base.GetSize() + 4;

            for (int i = 0; i < _ChatHistories.Count; i++)
            {
                size += _ChatHistories[i].GetSize();
            }

            return size;
        }

        override public void GetBytes(BinaryWriter bw)
        {
            try
            {
                base.GetBytes(bw);
                EncodeInteger(bw, _ChatHistories.Count);

                for (int i = 0; i < _ChatHistories.Count; i++)
                    _ChatHistories[i].GetBytes(bw);
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
                ChatHistoryInfo roomInfo = new ChatHistoryInfo();
                DecodeInteger(br);

                roomInfo.FromBytes(br);

                _ChatHistories.Add(roomInfo);
            }
        }
    }
}
