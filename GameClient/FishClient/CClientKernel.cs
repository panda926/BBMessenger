using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatEngine;

namespace FishClient
{
    public class CClientKernel
    {
        public FishView fishView_ = null;

        public void SendSocketData(int type, object data)
        {
            fishView_.SendSocketData(type, data);
        }

        public UserInfo GetUserInfo(int chairID)
        {
            return fishView_.GetUserInfo(chairID);
        }

        public UserInfo GetMeUserInfo()
        {
            return fishView_.GetMeUserInfo();
        }

        public int GetMeChairID()
        {
            return fishView_.GetMeChairID();
        }

        public void WindowClosed()
        {
            fishView_.WindowClosed();
        }
    }
}
